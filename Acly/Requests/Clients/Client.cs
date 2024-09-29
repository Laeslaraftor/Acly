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
	public sealed class Client : IClient
	{
		private Client(string Address, int Port) : this(Dns.GetHostEntry(Address).AddressList[0], Port)
        {
		}
        private Client(IPAddress Address, int Port)
        {
            _Address = Address;
            _EndPoint = new(_Address, Port);
            _Socket = new(_Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
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
		public int ReceiveInterval { get; set; } = 50;

		private readonly IPAddress _Address;
		private readonly IPEndPoint _EndPoint;
		private readonly Socket _Socket;
		private bool _Disconnected;

		#region Управление

		/// <summary>
		/// Отправить данные серверу
		/// </summary>
		/// <inheritdoc/>
		/// <exception cref="InvalidOperationException"></exception>
		public void Send(byte[] Data, int Offset, int Length)
		{
			if (_Disconnected)
			{
				throw new InvalidOperationException("Невозможно отправить сообщение после отключения от сервера");
			}

			_Socket.SendAsync(Data, Offset, Length);
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
        public void Disconnect()
		{
			if (_Disconnected)
			{
				throw new InvalidOperationException("Соединение уже разорвано");
			}

			Disconnect("Клиент прервал подключение");
		}

		private void Disconnect(string Message)
		{
			Dispose();
			Disconnected?.Invoke(Message);
		}

		#endregion

		#region Подключение

		private async void Connect()
		{
			try
			{
				await _Socket.ConnectAsync(_EndPoint);
			}
			catch (Exception Error)
			{
				Disconnected?.Invoke(Error.Message);
			}
			
			ReadLoop();
		}
		private async void ReadLoop()
		{
			while (_Socket != null)
			{
				if (!_Socket.IsConnected())
				{
					Disconnect("Отключён от сервера");
					break;
				}
				if (_Socket.Available == 0)
				{
					await Task.Delay(ReceiveInterval);
					continue;
				}

				byte[] Buffer = new byte[_Socket.Available];

				try
				{
					_Socket.Receive(Buffer);
				}
				catch (SocketException Exception)
				{
					Disconnect(Exception.Message);
					break;
				}

				if (!Received.TryInvoke(out Exception? Error, Buffer))
				{
#pragma warning disable CS8604
					Log.Error(Error);
#pragma warning restore CS8604
				}

				await Task.Delay(ReceiveInterval);
			}
		}

		private async Task WaitForConnection(TimeSpan Timeout)
		{
			int TimeoutMilliseconds = Convert.ToInt32(Timeout.TotalMilliseconds);
			int TimeLeft = 0;
			bool Success = true;

			try
			{
				while (!_Socket.IsConnected())
				{
					if (TimeLeft >= TimeoutMilliseconds)
					{
						Success = false;
						break;
					}

					TimeLeft += 50;
					await Task.Delay(50);
				}
			}
			catch
			{
				Success = false;
			}

			if (!Success)
			{
				throw new ServerConnectionException("Не удалось подключиться к серверу");
			}
		}

		#endregion

		#region Очистка

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public void Dispose()
		{
			_Disconnected = true;

			try
			{
				_Socket.Shutdown(SocketShutdown.Both);
				_Socket.Close();
			}
			catch (Exception Error)
			{
				Log.Error(Error);
			}
		}

		#endregion

		#region Статика

		/// <summary>
		/// Подключиться к серверу
		/// </summary>
		/// <param name="Address">Адрес сервера. Например, 192.168.0.1 или https://example.ru</param>
		/// <param name="Port">Порт для подключения</param>
		/// <param name="ConnectionTimeout">Время ожидания подключения</param>
		/// <returns>Подключение к серверу</returns>
		public static async Task<IClient> Connect(string Address, int Port, TimeSpan ConnectionTimeout)
		{
			Client Srv = new(Address, Port);
			Srv.Connect();

			await Srv.WaitForConnection(ConnectionTimeout);

			return Srv;
		}
        /// <summary>
        /// Подключиться к серверу
        /// </summary>
        /// <param name="Address">Адрес сервера. Например, 192.168.0.1 или https://example.ru</param>
        /// <param name="Port">Порт для подключения</param>
        /// <param name="ConnectionTimeout">Время ожидания подключения</param>
        /// <returns>Подключение к серверу</returns>
        public static async Task<IClient> Connect(IPAddress Address, int Port, TimeSpan ConnectionTimeout)
        {
            Client Srv = new(Address, Port);
            Srv.Connect();

            await Srv.WaitForConnection(ConnectionTimeout);

            return Srv;
        }
        /// <summary>
        /// Подключиться к серверу
        /// </summary>
        /// <param name="Address">Адрес сервера. Например, 192.168.0.1 или https://example.ru</param>
        /// <param name="Port">Порт для подключения</param>
        /// <returns>Подключение к серверу</returns>
        public static async Task<IClient> Connect(string Address, int Port)
		{
			return await Connect(Address, Port, TimeSpan.FromMinutes(1));
		}
        /// <summary>
        /// Подключиться к серверу
        /// </summary>
        /// <param name="Address">Адрес сервера. Например, 192.168.0.1 или https://example.ru</param>
        /// <param name="Port">Порт для подключения</param>
        /// <returns>Подключение к серверу</returns>
        public static async Task<IClient> Connect(IPAddress Address, int Port)
        {
            return await Connect(Address, Port, TimeSpan.FromMinutes(1));
        }

        #endregion
    }
}
