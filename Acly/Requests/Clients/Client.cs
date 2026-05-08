using Acly.Requests.Exceptions;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Acly.Requests
{
    /// <summary>
    /// Класс, для подключения к серверу
    /// </summary>
    public sealed class Client : Disposable, IClient
    {
        private Client(string address, int port)
            : this(Dns.GetHostEntry(address).AddressList[0], port)
        {
        }
        private Client(IPAddress address, int port)
        {
            _address = address;
            _endPoint = new(_address, port);
            _socket = new(_address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event IClient.ReceiveData? Received;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event IClient.GetDisconnectInfo? Disconnected;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public int ReceiveInterval
        {
            get => field;
            set
            {
                if (field != value)
                {
                    OnPropertyChanging(nameof(ReceiveInterval));
                    field = value;
                    OnPropertyChanged(nameof(ReceiveInterval));
                }
            }
        } = 50;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override bool UseDispatcherForLogging => false;

        private readonly IPAddress _address;
        private readonly IPEndPoint _endPoint;
        private readonly Socket _socket;
        private bool _disconnected;

        #region Управление

        /// <summary>
        /// Отправить данные серверу
        /// </summary>
        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException"></exception>
        public void Send(byte[] data, int offset, int length)
        {
            if (_disconnected)
            {
                throw new InvalidOperationException("Невозможно отправить сообщение после отключения от сервера");
            }

            _socket.SendAsync(data, offset, length);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="obj"><inheritdoc/></param>
        public void Send(object obj)
        {
            byte[] data = MessageData.Create(obj);
            Send(data, 0, data.Length);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Disconnect()
        {
            if (_disconnected)
            {
                throw new InvalidOperationException("Соединение уже разорвано");
            }

            Disconnect("Клиент прервал подключение");
        }

        private void Disconnect(string message)
        {
            Dispose();
            Dispatch(Disconnected, message);
        }

        #endregion

        #region Подключение

        private async void Connect()
        {
            try
            {
                await _socket.ConnectAsync(_endPoint);
            }
            catch (Exception error)
            {
                Dispatch(Disconnected, error.Message);
            }

            ReadLoop();
        }
        private async void ReadLoop()
        {
            while (_socket != null)
            {
                if (!_socket.IsConnected())
                {
                    Disconnect("Отключён от сервера");
                    break;
                }
                if (_socket.Available == 0)
                {
                    await Task.Delay(ReceiveInterval);
                    continue;
                }

                byte[] buffer = new byte[_socket.Available];

                try
                {
                    _socket.Receive(buffer);
                }
                catch (SocketException exception)
                {
                    Disconnect(exception.Message);
                    break;
                }

                Dispatch(() =>
                {
                    if (!Received.TryInvoke(out Exception? error, buffer))
                    {
                        LogError(error);
                    }
                });

                await Task.Delay(ReceiveInterval);
            }
        }

        private async Task WaitForConnection(TimeSpan timeout)
        {
            int timeoutMilliseconds = Convert.ToInt32(timeout.TotalMilliseconds);
            int timeLeft = 0;
            bool success = true;

            try
            {
                while (!_socket.IsConnected())
                {
                    if (timeLeft >= timeoutMilliseconds)
                    {
                        success = false;
                        break;
                    }

                    timeLeft += 50;
                    await Task.Delay(50);
                }
            }
            catch (Exception error)
            {
                success = false;
                LogError(error);
            }

            if (!success)
            {
                throw new ServerConnectionException("Не удалось подключиться к серверу");
            }
        }

        #endregion

        #region Очистка

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="isDisposing"><inheritdoc/></param>
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            _disconnected = true;

            try
            {
                _socket.Shutdown(SocketShutdown.Both);
                _socket.Close();
            }
            catch (Exception error)
            {
                LogError(error);
            }
        }

        #endregion

        #region Статика

        /// <summary>
        /// Подключиться к серверу
        /// </summary>
        /// <param name="address">Адрес сервера. Например, 192.168.0.1 или https://example.ru</param>
        /// <param name="port">Порт для подключения</param>
        /// <param name="connectionTimeout">Время ожидания подключения</param>
        /// <returns>Подключение к серверу</returns>
        public static async Task<IClient> Connect(string address, int port, TimeSpan connectionTimeout)
        {
            Client client = new(address, port);
            client.Connect();

            await client.WaitForConnection(connectionTimeout);

            return client;
        }
        /// <summary>
        /// Подключиться к серверу
        /// </summary>
        /// <param name="address">Адрес сервера. Например, 192.168.0.1 или https://example.ru</param>
        /// <param name="port">Порт для подключения</param>
        /// <param name="connectionTimeout">Время ожидания подключения</param>
        /// <returns>Подключение к серверу</returns>
        public static async Task<IClient> Connect(IPAddress address, int port, TimeSpan connectionTimeout)
        {
            Client client = new(address, port);
            client.Connect();

            await client.WaitForConnection(connectionTimeout);

            return client;
        }
        /// <summary>
        /// Подключиться к серверу
        /// </summary>
        /// <param name="address">Адрес сервера. Например, 192.168.0.1 или https://example.ru</param>
        /// <param name="port">Порт для подключения</param>
        /// <returns>Подключение к серверу</returns>
        public static async Task<IClient> Connect(string address, int port)
        {
            return await Connect(address, port, TimeSpan.FromMinutes(1));
        }
        /// <summary>
        /// Подключиться к серверу
        /// </summary>
        /// <param name="address">Адрес сервера. Например, 192.168.0.1 или https://example.ru</param>
        /// <param name="port">Порт для подключения</param>
        /// <returns>Подключение к серверу</returns>
        public static async Task<IClient> Connect(IPAddress address, int port)
        {
            return await Connect(address, port, TimeSpan.FromMinutes(1));
        }

        #endregion
    }
}
