using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Acly.Requests
{
    /// <summary>
    /// Класс, для создания сервера
    /// </summary>
    public sealed class Server : Disposable, IServer
    {
        private Server(string address, int port)
            : this(Dns.GetHostEntry(address).AddressList[0], port)
        {
        }
        private Server(IPAddress address, int port)
        {
            Connected += OnClientConnected;
            Disconnected += OnClientDisconnected;

            _address = address;
            _endPoint = new(_address, port);
            _socket = new(_address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            _socket.Bind(_endPoint);
            _socket.Listen(10);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event IServer.GetConnectedSocket? Connected;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event IServer.ReceiveData? Received;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event IServer.GetDisconnectedInfo? Disconnected;

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
        public int MaximumConnections
        {
            get => field;
            set
            {
                if (field != value)
                {
                    OnPropertyChanging(nameof(MaximumConnections));
                    field = Math.Max(-1, value);
                    OnPropertyChanged(nameof(MaximumConnections));
                }
            }
        } = -1;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public int TotalConnections
        {
            get => field;
            private set
            {
                if (field != value)
                {
                    OnPropertyChanging(nameof(TotalConnections));
                    field = Math.Max(0, value);
                    OnPropertyChanged(nameof(TotalConnections));
                }
            }
        }

        private readonly List<Socket> _connections = [];
        private readonly IPAddress _address;
        private readonly IPEndPoint _endPoint;
        private readonly Socket _socket;
        private bool _disabled;

        #region Управление

        /// <summary>
        /// Отправить сообщение всем клиентам
        /// </summary>
        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException"></exception>
        public void Send(byte[] data, int offset, int length)
        {
            if (_disabled)
            {
                throw new InvalidOperationException("Сервер был отключен");
            }

            foreach (var connection in _connections)
            {
                connection.SendAsync(data, offset, length);
            }
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
        /// <exception cref="NotImplementedException"></exception>
        public void Shutdown()
        {
            if (_disabled)
            {
                throw new InvalidOperationException("Сервер уже отключен");
            }

            Dispose();
        }

        #endregion

        #region Подключение

        private async void WaitForConnection()
        {
            await Task.Delay(50);

            Socket? connection = null;

            try
            {
                while (true)
                {
                    if (connection == null)
                    {
                        connection = _socket.Accept();
                        InvokeConnectedEvent(connection);
                    }
                    if (MaximumConnections != -1 && _connections.Count > MaximumConnections && _connections[^1] == connection)
                    {
                        ShutdownSocket(connection);
                        InvokeDisconnectEvent(connection, "Превышен лимит подключений");
                        break;
                    }
                    if (!connection.IsConnected())
                    {
                        InvokeDisconnectEvent(connection, "Клиент отключился");
                        break;
                    }
                    if (connection.Available == 0)
                    {
                        await Task.Delay(ReceiveInterval);
                        continue;
                    }

                    byte[] data = new byte[connection.Available];
                    connection.Receive(data);

                    InvokeReceiveEvent(connection, data);

                    await Task.Delay(ReceiveInterval);
                }
            }
            catch (SocketException error)
            {
                if (connection != null)
                {
                    InvokeDisconnectEvent(connection, error.Message);
                }

                Log.Error(error);
            }
        }
        private static void ShutdownSocket(Socket connection)
        {
            try
            {
                connection.Shutdown(SocketShutdown.Both);
                connection.Close();
            }
            catch (Exception error)
            {
                Log.Error(error);
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

            _disabled = true;

            try
            {
                _socket.Shutdown(SocketShutdown.Both);
                _socket.Close();
            }
            catch (Exception error)
            {
                Log.Error(error);
            }

            Connected -= OnClientConnected;
            Disconnected -= OnClientDisconnected;
        }

        #endregion

        #region События

        private void InvokeConnectedEvent(Socket socket)
        {
            Dispatch(() =>
            {
                if (!Connected.TryInvoke(out Exception? error, socket))
                {
                    Log.Error(error);
                }
            });
        }
        private void InvokeReceiveEvent(Socket socket, byte[] data)
        {
            Dispatch(() =>
            {
                if (!Received.TryInvoke(out Exception? error, socket, data))
                {
                    Log.Error(error);
                }
            });
        }
        private void InvokeDisconnectEvent(Socket socket, string reason)
        {
            Dispatch(() =>
            {
                if (!Disconnected.TryInvoke(out Exception? error, socket, reason))
                {
                    Log.Error(error);
                }
            });
        }

        private void OnClientConnected(Socket socket)
        {
            _connections.Add(socket);
            WaitForConnection();
        }
        private void OnClientDisconnected(Socket socket, string reason)
        {
            _connections.Remove(socket);
            socket.Dispose();
        }

        #endregion

        #region Статика

        /// <summary>
        /// Создать сервер
        /// </summary>
        /// <param name="address">Адрес сервера</param>
        /// <param name="port">Порт сервера</param>
        /// <returns>Сервер</returns>
        public static IServer Create(string address, int port)
        {
            Server server = new(address, port);
            server.WaitForConnection();

            return server;
        }
        /// <summary>
        /// Создать сервер
        /// </summary>
        /// <param name="address">Адрес сервера</param>
        /// <param name="port">Порт сервера</param>
        /// <returns>Сервер</returns>
        public static IServer Create(IPAddress address, int port)
        {
            Server server = new(address, port);
            server.WaitForConnection();

            return server;
        }

        #endregion
    }
}
