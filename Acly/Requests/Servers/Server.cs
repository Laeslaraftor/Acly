using System;
using System.Net.Sockets;
using System.Net;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Acly.Requests
{
	/// <summary>
	/// Класс, для создания сервера
	/// </summary>
	public sealed class Server : IServer
	{
		private Server(string Address, int Port) : this(Dns.GetHostEntry(Address).AddressList[0], Port)
		{
		}
        private Server(IPAddress Address, int Port)
        {
            Connected += OnClientConnected;
            Disconnected += OnClientDisconnected;

            _Address = Address;
            _EndPoint = new(_Address, Port);
            _Socket = new(_Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            _Socket.Bind(_EndPoint);
            _Socket.Listen(10);
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
		public int ReceiveInterval { get; set; } = 50;
		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public int MaximumConnections
		{
			get => _MaximumConnections;
			set
			{
				if (value < 0)
				{
					_MaximumConnections = -1;
					return;
				}

				_MaximumConnections = value;
			}
		}
		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public int TotalConnections => _Connections.Count;

		private readonly List<Socket> _Connections = new();
		private readonly IPAddress _Address;
		private readonly IPEndPoint _EndPoint;
		private readonly Socket _Socket;
		private bool _Disabled;
		private int _MaximumConnections = -1;

        #region Управление

        /// <summary>
        /// Отправить сообщение всем клиентам
        /// </summary>
        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException"></exception>
        public void Send(byte[] Data, int Offset, int Length)
		{
			if (_Disabled)
			{
				throw new InvalidOperationException("Сервер был отключен");
			}

			foreach (var Connection in _Connections)
			{
				Connection.SendAsync(Data, Offset, Length);
			}
		}
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="Object"><inheritdoc/></param>
        public void Send(object Object)
		{
			byte[] Data = MessageData.Create(Object);
			Send(Data, 0, Data.Length);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <exception cref="NotImplementedException"></exception>
		public void Shutdown()
		{
			if (_Disabled)
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

			Socket? Connection = null;

			try
			{
				while (true)
				{
					if (Connection == null)
					{
						Connection = _Socket.Accept();
						InvokeConnectedEvent(Connection);
					}
					if (MaximumConnections != -1 && _Connections.Count > MaximumConnections && _Connections[^1] == Connection)
					{
						ShutdownSocket(Connection);
						InvokeDisconnectEvent(Connection, "Превышен лимит подключений");
						break;
					}
					if (!Connection.IsConnected())
					{
						InvokeDisconnectEvent(Connection, "Клиент отключился");
						break;
					}
					if (Connection.Available == 0)
					{
						await Task.Delay(ReceiveInterval);
						continue;
					}

					byte[] Data = new byte[Connection.Available];
					Connection.Receive(Data);

					InvokeReceiveEvent(Connection, Data);

					await Task.Delay(ReceiveInterval);
				}
			}
			catch (SocketException Error)
			{
#pragma warning disable CS8604
				InvokeDisconnectEvent(Connection, Error.Message);
#pragma warning restore CS8604
			}
		}
		private static void ShutdownSocket(Socket Connection)
		{
			try
			{
				Connection.Shutdown(SocketShutdown.Both);
				Connection.Close();
			}
			catch
			{
			}
		}

		#endregion

		#region Очистка

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public void Dispose()
		{
			_Disabled = true;

			try
			{
				_Socket.Shutdown(SocketShutdown.Both);
				_Socket.Close();
			}
			catch (Exception Error)
			{
				Log.Error(Error);
			}

			Connected -= OnClientConnected;
			Disconnected -= OnClientDisconnected;
		}

		#endregion

		#region События

#pragma warning disable CS8604
		private void InvokeConnectedEvent(Socket Socket)
		{
			if (!Connected.TryInvoke(out Exception? Error, Socket))
			{
				Log.Error(Error);
			}
		}
		private void InvokeReceiveEvent(Socket Socket, byte[] Data)
		{
			if (!Received.TryInvoke(out Exception? Error, Socket, Data))
			{
				Log.Error(Error);
			}
		}
		private void InvokeDisconnectEvent(Socket Socket, string Reason)
		{
			if (!Disconnected.TryInvoke(out Exception? Error, Socket, Reason))
			{
				Log.Error(Error);
			}
		}
#pragma warning restore CS8604

		private void OnClientConnected(Socket Socket)
		{
			_Connections.Add(Socket);
			WaitForConnection();
		}
		private void OnClientDisconnected(Socket Socket, string Reason)
		{
			_Connections.Remove(Socket);
			Socket.Dispose();
		}

		#endregion

		#region Статика

		/// <summary>
		/// Создать сервер
		/// </summary>
		/// <param name="Address">Адрес сервера</param>
		/// <param name="Port">Порт сервера</param>
		/// <returns>Сервер</returns>
		public static IServer Create(string Address, int Port)
		{
			Server Srv = new(Address, Port);

			Srv.WaitForConnection();

			return Srv;
		}
        /// <summary>
        /// Создать сервер
        /// </summary>
        /// <param name="Address">Адрес сервера</param>
        /// <param name="Port">Порт сервера</param>
        /// <returns>Сервер</returns>
        public static IServer Create(IPAddress Address, int Port)
        {
            Server Srv = new(Address, Port);

            Srv.WaitForConnection();

            return Srv;
        }

        #endregion
    }
}
