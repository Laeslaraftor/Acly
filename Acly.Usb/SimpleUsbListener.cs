using System;
using System.IO.Ports;
using System.Text;

namespace Acly.Usb
{
    /// <summary>
    /// Простой Usb слушатель
    /// </summary>
    public class SimpleUsbListener : UsbListenerBase
    {
        /// <summary>
        /// Создать экземпляр простого Usb слушателя
        /// </summary>
        /// <param name="Id">Идентификатор</param>
        /// <param name="UsbManager">Менеджер Usb портов</param>
        public SimpleUsbListener(IUsbManager UsbManager, Guid Id) : base(UsbManager)
        {
            this.Id = Id;
        }
        /// <summary>
        /// Создать экземпляр простого Usb слушателя с использованием стандартного Usb менеджера
        /// </summary>
        /// <param name="Id">Идентификатор</param>
        public SimpleUsbListener(Guid Id) : this(new StandardUsbManager(), Id)
        {
        }

        /// <summary>
        /// Вызывается при подключении устройства
        /// </summary>
        public event UsbListenerDeviceEvent? DeviceConnected;
        /// <summary>
        /// Вызывается при отключении устройства
        /// </summary>
        public event UsbListenerDeviceEvent? DeviceDisconnected;
        /// <summary>
        /// Вызывается при получении данных с устройства
        /// </summary>
        public event UsbListenerDeviceDataEvent? DataReceived;
        /// <summary>
        /// Вызывается при начале получения данных с устройства
        /// </summary>
        public event UsbListenerDeviceStartDataReceivingEvent? DataStartReceiving;
        /// <summary>
        /// Вызывается при прогрессе получения данных с устройства
        /// </summary>
        public event UsbListenerDeviceDataReceivingEvent? DataReceivingProgress;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override Guid Id { get; }
        /// <summary>
        /// Время ожидания отклика устройства
        /// </summary>
        public TimeSpan ResponseTimeout { get; set; } = TimeSpan.FromSeconds(1);
        /// <summary>
        /// Интервал поиска подключённых устройств
        /// </summary>
        public TimeSpan SearchInterval { get; set; } = TimeSpan.FromSeconds(0.2);
        /// <summary>
        /// Кодировка сообщений
        /// </summary>
        public Encoding MessagesEncoding { get; set; } = Encoding.UTF8;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override TimeSpan DeviceResponseTimeout => ResponseTimeout;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override TimeSpan DeviceSearchInterval => SearchInterval;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override Encoding Encoding => MessagesEncoding;

        #region Управление

        /// <summary>
        /// Отправить данные на подключённое устройство
        /// </summary>
        /// <param name="Device">Подключённое устройство</param>
        /// <param name="Data">Данные для отправки</param>
        /// <returns>Были ли отправлены данные</returns>
        public new UsbDataSendResult Send(IUsbPort Device, byte[] Data) => base.Send(Device, Data);
        /// <summary>
        /// Отправить сообщение на подключённое устройство
        /// </summary>
        /// <param name="Device">Подключённое устройство</param>
        /// <param name="Message">Сообщение для отправки</param>
        /// <returns>Было ли отправлено сообщение</returns>
        public UsbDataSendResult Send(IUsbPort Device, string Message)
        {
            return Send(Device, Encoding.GetBytes(Message));
        }

        /// <summary>
        /// Отправить данные на все подключённые устройства
        /// </summary>
        /// <param name="Data">Данные для отправки</param>
        public void Send(byte[] Data)
        {
            foreach (var Device in ConnectedPorts)
            {
                Send(Device, Data);
            }
        }
        /// <summary>
        /// Отправить сообщение на все подключенные устройства
        /// </summary>
        /// <param name="Message">Сообщение для отправки</param>
        public void Send(string Message)
        {
            foreach (var Device in ConnectedPorts)
            {
                Send(Device, Message);
            }
        }

        #endregion

        #region События

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="Device"><inheritdoc/></param>
        protected override void OnDeviceConnected(IUsbPort Device)
        {
            base.OnDeviceConnected(Device);
            DeviceConnected?.Invoke(this, Device);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="Device"><inheritdoc/></param>
        protected override void OnDeviceDisconnected(IUsbPort Device)
        {
            base.OnDeviceDisconnected(Device);
            DeviceDisconnected?.Invoke(this, Device);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="Device"><inheritdoc/></param>
        /// <param name="Data"><inheritdoc/></param>
        protected override void OnDeviceDataReceived(IUsbPort Device, byte[] Data)
        {
            base.OnDeviceDataReceived(Device, Data);
            DataReceived?.Invoke(this, Device, Data);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="Device"><inheritdoc/></param>
        /// <param name="Received"><inheritdoc/></param>
        /// <param name="Total"><inheritdoc/></param>
        protected override void OnDeviceDataReceivingProgress(IUsbPort Device, int Received, int Total)
        {
            base.OnDeviceDataReceivingProgress(Device, Received, Total);
            DataReceivingProgress?.Invoke(this, Device, Received, Total);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="Device"><inheritdoc/></param>
        /// <param name="Size"><inheritdoc/></param>
        protected override void OnDeviceStartDataReceiving(IUsbPort Device, int Size)
        {
            base.OnDeviceStartDataReceiving(Device, Size);
            DataStartReceiving?.Invoke(this, Device, Size);
        }

        #endregion
    }
}
