using System;
using System.IO.Ports;

namespace Acly.Usb
{
    /// <summary>
    /// Стандартное Usb подключение, основанное на <see cref="SerialPort"/>
    /// </summary>
    public sealed class StandardUsbPort : IUsbPort
    {
        /// <summary>
        /// Создать Usb подключение по названию порта
        /// </summary>
        /// <param name="PortName"></param>
        public StandardUsbPort(string PortName) : this(CreateSerialPort(PortName))
        {
        }
        /// <summary>
        /// Создать Usb подключение через <see cref="SerialPort"/>
        /// </summary>
        /// <param name="Port"></param>
        public StandardUsbPort(SerialPort Port)
        {
            if (Port == null)
            {
                throw new ArgumentNullException(nameof(Port), "Порт не указан");
            }

            _Port = Port;

            Port.PinChanged += OnPortPinChanged;
            Port.DataReceived += OnPortDataReceived;

            if (!Port.IsOpen)
            {
                Port.Open();
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event UsbPortEvent? Disconnected;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event UsbPortEvent? DataReceived;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public TimeSpan ResponseTimeout
        {
            get => TimeSpan.FromMilliseconds(_Port.ReadTimeout);
            set
            {
                int Milliseconds = Convert.ToInt32(value.TotalMilliseconds);
                _Port.ReadTimeout = Milliseconds;
                _Port.WriteTimeout = Milliseconds;
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string PortName => _Port.PortName;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public int BytesToRead => _Port.BytesToRead;
        /// <summary>
        /// Размер буфера
        /// </summary>
        public int BufferSize
        {
            get => _Port.ReadBufferSize;
            set
            {
                _Port.ReadBufferSize = value;
                _Port.WriteBufferSize = value;
            }
        }

        private readonly SerialPort _Port;

        #region Управление

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="Data"><inheritdoc/></param>
        /// <param name="Offset"><inheritdoc/></param>
        /// <param name="Count"><inheritdoc/></param>
        public void Write(byte[] Data, int Offset, int Count) => _Port.Write(Data, Offset, Count);
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="Data"><inheritdoc/></param>
        /// <param name="Offset"><inheritdoc/></param>
        /// <param name="Count"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public int Read(byte[] Data, int Offset, int Count) => _Port.Read(Data, Offset, Count);
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        public string ReadLine() => _Port.ReadLine();

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Dispose()
        {
            _Port.Dispose();
        }

        #endregion

        #region События

        private void OnPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            DataReceived?.Invoke(this);
        }
        private void OnPortPinChanged(object sender, SerialPinChangedEventArgs e)
        {
            if (e.EventType == SerialPinChange.CDChanged)
            {
                Disconnected?.Invoke(this);
            }
        }

        #endregion

        #region Статика

        private static SerialPort CreateSerialPort(string PortName)
        {
            return new(PortName, 9600, Parity.None, 8, StopBits.One)
            {
                ReadBufferSize = 4096,
                WriteBufferSize = 4096
            };
        }

        #endregion
    }
}
