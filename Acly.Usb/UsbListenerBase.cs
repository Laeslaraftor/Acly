using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading.Tasks;
using Acly.Requests;
using Acly.Usb.Properties;

namespace Acly.Usb
{
    /// <summary>
    /// Читатель Usb портов
    /// </summary>
    public abstract class UsbListenerBase : ListenerBase
    {
        /// <summary>
        /// Создать экземпляр читателя Usb портов
        /// </summary>
        /// <param name="UsbManager">Менеджер Usb портов</param>
        protected UsbListenerBase(IUsbManager UsbManager)
        {
            this.UsbManager = UsbManager;
        }

        /// <summary>
        /// Идентификатор читателя
        /// </summary>
        public abstract Guid Id { get; }

        /// <summary>
        /// Время ожидания отклика устройства
        /// </summary>
        protected abstract TimeSpan DeviceResponseTimeout { get; }
        /// <summary>
        /// Интервал поиска подключённых устройств
        /// </summary>
        protected abstract TimeSpan DeviceSearchInterval { get; }
        /// <summary>
        /// Кодировка сообщений
        /// </summary>
        protected abstract Encoding Encoding { get; }

        /// <summary>
        /// Менеджер Usb портов
        /// </summary>
        protected IUsbManager UsbManager { get; }
        /// <summary>
        /// Список подключённых устройств
        /// </summary>
        protected ReferenceReadOnlyList<IUsbPort> ConnectedPorts
        {
            get
            {
                _ReferenceConnectedPorts ??= new(_ConnectedPorts);
                return _ReferenceConnectedPorts;
            }
        }

        private readonly ObservableList<IUsbPort> _ConnectedPorts = new();
        private readonly Dictionary<IUsbPort, MemoryStream> _DevicesReceiveBuffers = new();
        private ReferenceReadOnlyList<IUsbPort>? _ReferenceConnectedPorts;

        #region Управление

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        public override bool Start()
        {
            bool Result = base.Start();

            if (Result)
            {
                ConnectPorts();
            }

            return Result;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        public override bool Stop()
        {
            bool Result = base.Stop();

            if (Result)
            {
                DisconnectPorts();
            }

            return Result;
        }

        /// <summary>
        /// Отправить данные устройству
        /// </summary>
        /// <param name="Device">Устройство, которому будут отправлены данные</param>
        /// <param name="Data">Данные для отправки</param>
        /// <returns>
        /// Были ли отправлены данные
        /// </returns>
        protected virtual UsbDataSendResult Send(IUsbPort Device, byte[] Data)
        {
            if (Device == null)
            {
                throw new ArgumentNullException(nameof(Device), "Устройство не указано");
            }
            if (Data == null)
            {
                throw new ArgumentNullException(nameof(Device), "Данные для отправки не указаны");
            }

            Send(Device, Resources.StartDataSentRequest);
            string Response = Device.ReadLine();

            if (Response != Resources.DataSendAllow)
            {
                return UsbDataSendResult.DeviceDenied;
            }

            Send(Device, Resources.DataSendSize + "-" + Data.Length);
            Response = Device.ReadLine();

            if (Response != Resources.DataSendAllow)
            {
                return UsbDataSendResult.DataDenied;
            }

            Device.Write(Data, 0, Data.Length);
            Send(Device, Resources.EndDataSent);

            return UsbDataSendResult.Success;
        }
        private void Send(IUsbPort Device, string Value)
        {
            Device.Write(Value, Encoding);
        }

        #endregion

        #region Подключение и отключение

        private IUsbPort CreatePort(string PortName)
        {
            IUsbPort Port = UsbManager.GetPort(PortName);
            Port.ResponseTimeout = DeviceResponseTimeout;

            return Port;
        }

        private async void StartPortCheckingTask(string PortName)
        {
            await Task.Run(() =>
            {
                StartPortChecking(PortName);
            });
        }
        private async void StartPortChecking(string PortName)
        {
            IUsbPort Sp = CreatePort(PortName);

            while (IsStarted)
            {
                Sp.Write(Id.ToString(), Encoding);

                try
                {
                    string Response = Sp.ReadLine();

                    if (Response.Contains(Id.ToString(), StringComparison.InvariantCulture) && IsStarted)
                    {
                        ConnectPort(Sp);
                        return;
                    }
                }
                catch (Exception Error)
                {
                    if (Error.GetType() != typeof(TimeoutException))
                    {
                        break;
                    }
                }

                await Task.Delay(DeviceResponseTimeout);
            }

            Sp.Dispose();
        }

        private void ConnectPorts()
        {
            string[] Ports = UsbManager.GetPortNames();

            foreach (var Port in Ports)
            {
                StartPortCheckingTask(Port);
            }
        }
        private void DisconnectPorts()
        {
            List<IUsbPort> Ports = new(_ConnectedPorts);

            foreach (var Port in Ports)
            {
                DisconnectPort(Port);
            }
        }

        private void ConnectPort(IUsbPort Port)
        {
            Port.DataReceived += OnPortDataReceived;
            Port.Disconnected += DisconnectPort;
            _ConnectedPorts.Add(Port);

            OnDeviceConnected(Port);
        }
        private void DisconnectPort(IUsbPort Port)
        {
            Port.DataReceived -= OnPortDataReceived;
            Port.Disconnected -= DisconnectPort;
            _ConnectedPorts.Remove(Port);

            OnDeviceDisconnected(Port);

            Port.Dispose();

            if (IsStarted)
            {
                StartPortCheckingTask(Port.PortName);
            }
        }

        #endregion

        #region Чтение

        private string? ReadPortData(IUsbPort Port)
        {
            byte[] Data = new byte[Port.BytesToRead];
            Port.Read(Data, 0, Data.Length);
            string EncodedValue = Encoding.GetString(Data);

            if (EncodedValue == Resources.StartDataSentRequest)
            {
                if (_DevicesReceiveBuffers.ContainsKey(Port))
                {
                    return Resources.DataSendDenied;
                }
                else
                {
                    return Resources.DataSendAllow;
                }
            }
            else if (EncodedValue.Contains(Resources.DataSendSize, StringComparison.InvariantCulture))
            {
                string[] Parts = EncodedValue.Split("-");

                if (Parts.Length > 1 && int.TryParse(Parts[1], out int Result))
                {
                    _DevicesReceiveBuffers.Add(Port, new(Result));
                    OnDeviceStartDataReceiving(Port, Result);
                    return Resources.DataSendAllow;
                }
                else
                {
                    return Resources.DataSendDenied;
                }
            }
            else if (EncodedValue == Resources.EndDataSent)
            {
                if (_DevicesReceiveBuffers.TryGetValue(Port, out var Buffer))
                {
                    OnDeviceDataReceived(Port, Buffer.ToArray());
                    Buffer.Dispose();
                }

                _DevicesReceiveBuffers.Remove(Port);
            }
            else if (_DevicesReceiveBuffers.TryGetValue(Port, out var Buffer))
            {
                Buffer.Write(Data, (int)Buffer.Position, Data.Length);
                OnDeviceDataReceivingProgress(Port, (int)Buffer.Position, Buffer.Capacity);
            }

            return null;
        }

        #endregion

        #region События

        private async void OnPortDataReceived(IUsbPort Port)
        {
            string? Response = await Task.Run(() => ReadPortData(Port));

            if (Response != null)
            {
                Send(Port, Response);
            }
        }

        /// <summary>
        /// Вызывается при подключении устройства
        /// </summary>
        /// <param name="Device">Подключённое устройство</param>
        protected virtual void OnDeviceConnected(IUsbPort Device)
        {
        }
        /// <summary>
        /// Вызывается при отключении устройства
        /// </summary>
        /// <param name="Device">Отключённое устройство</param>
        protected virtual void OnDeviceDisconnected(IUsbPort Device)
        {
            _DevicesReceiveBuffers.Remove(Device);
        }
        /// <summary>
        /// Вызывается при начале получения данных от устройства
        /// </summary>
        /// <param name="Device">Устройство, которое присылает данные</param>
        /// <param name="Size">Размер данных в байтах</param>
        protected virtual void OnDeviceStartDataReceiving(IUsbPort Device, int Size)
        {
        }
        /// <summary>
        /// Вызывается при изменении прогресса получения данных от устройства
        /// </summary>
        /// <param name="Device">Устройство, которое присылает данные</param>
        /// <param name="Received">Размер уже полученных данных в байтах</param>
        /// <param name="Total">Общее количество данных в байтах</param>
        protected virtual void OnDeviceDataReceivingProgress(IUsbPort Device, int Received, int Total)
        {
        }
        /// <summary>
        /// Вызывается при получении данных от устройства
        /// </summary>
        /// <param name="Device">Устройство, приславшее данные</param>
        /// <param name="Data">Данные, полученные от устройства</param>
        protected virtual void OnDeviceDataReceived(IUsbPort Device, byte[] Data)
        {
        }

        #endregion
    }
}
