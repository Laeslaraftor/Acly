using System;
using System.Net.Sockets;
using System.Text;

namespace Acly.Requests
{
	/// <summary>
	/// Методы расширения для запросов
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Отправить данные на сервер
		/// </summary>
		/// <param name="Server">Сервер, на который будут отправлены данные</param>
		/// <param name="Data">Данные</param>
		/// <exception cref="ArgumentNullException">Сервер или данные не указаны</exception>
		public static void Send(this ISendable Server, byte[] Data)
		{
			if (Server == null)
			{
				throw new ArgumentNullException(nameof(Server), "Сервер не указан");
			}
			if (Data == null)
			{
				throw new ArgumentNullException(nameof(Data), "Данные для отправки не указаны");
			}

			Server.Send(Data, 0, Data.Length);
		}
		/// <summary>
		/// Отправить сообщение с кодировкой <see cref="Encoding.UTF8"/>
		/// </summary>
		/// <param name="Server">Сервер, на который будет отправлено сообщение</param>
		/// <param name="Message">Сообщение</param>
		/// <exception cref="ArgumentNullException">Сервер или сообщение не указано</exception>
		public static void SendText(this ISendable Server, string Message)
		{
			if (Server == null)
			{
				throw new ArgumentNullException(nameof(Server), "Сервер не указан");
			}
			if (Message == null)
			{
				throw new ArgumentNullException(nameof(Message), "Сообщение не указано");
			}

			Server.SendText(Message, Encoding.UTF8);
		}
		/// <summary>
		/// Отправить сообщение с указанной кодировкой
		/// </summary>
		/// <param name="Server">Сервер, на который будет отправлено сообщение</param>
		/// <param name="Message">Сообщение</param>
		/// <param name="Encoding">Кодировка сообщения</param>
		/// <exception cref="ArgumentNullException">Сервер, сообщение, или кодировка не указана</exception>
		public static void SendText(this ISendable Server, string Message, Encoding Encoding)
		{
			if (Server == null)
			{
				throw new ArgumentNullException(nameof(Server), "Сервер не указан");
			}
			if (Message == null)
			{
				throw new ArgumentNullException(nameof(Message), "Сообщение не указано");
			}
			if (Encoding == null)
			{
				throw new ArgumentNullException(nameof(Encoding), "Кодировка не указана");
			}

			byte[] Data = Encoding.GetBytes(Message);
			Server.Send(Data, 0, Data.Length);
		}
		/// <summary>
		/// Отправить асинхронно данные сокету
		/// </summary>
		/// <param name="Socket">Сокет, которому надо отправить данные</param>
		/// <param name="Data">Данные</param>
		/// <param name="Offset">Смещение</param>
		/// <param name="Length">Размер данных</param>
		/// <exception cref="ArgumentNullException">Сокет не указан</exception>
		public static void SendAsync(this Socket Socket, byte[] Data, int Offset, int Length)
		{
			if (Socket == null)
			{
				throw new ArgumentNullException(nameof(Socket), "Сокет не указан");
			}

			using SocketAsyncEventArgs Args = new();
			Args.SetBuffer(Data, Offset, Length);

			Socket.SendAsync(Args);
		}
		/// <summary>
		/// Отправить сообщение с кодировкой <see cref="Encoding.UTF8"/>
		/// </summary>
		/// <param name="Server">Сервер, на который будет отправлено сообщение</param>
		/// <param name="Message">Сообщение</param>
		/// <exception cref="ArgumentNullException">Сервер или сообщение не указано</exception>
		public static int SendText(this Socket Server, string Message)
		{
			if (Server == null)
			{
				throw new ArgumentNullException(nameof(Server), "Сервер не указан");
			}
			if (Message == null)
			{
				throw new ArgumentNullException(nameof(Message), "Сообщение не указано");
			}

			return Server.SendText(Message, Encoding.UTF8);
		}
		/// <summary>
		/// Отправить сообщение с указанной кодировкой
		/// </summary>
		/// <param name="Server">Сервер, на который будет отправлено сообщение</param>
		/// <param name="Message">Сообщение</param>
		/// <param name="Encoding">Кодировка сообщения</param>
		/// <exception cref="ArgumentNullException">Сервер, сообщение, или кодировка не указана</exception>
		public static int SendText(this Socket Server, string Message, Encoding Encoding)
		{
			if (Server == null)
			{
				throw new ArgumentNullException(nameof(Server), "Сервер не указан");
			}
			if (Message == null)
			{
				throw new ArgumentNullException(nameof(Message), "Сообщение не указано");
			}
			if (Encoding == null)
			{
				throw new ArgumentNullException(nameof(Encoding), "Кодировка не указана");
			}

			byte[] Data = Encoding.GetBytes(Message);
			return Server.Send(Data);
		}
		/// <summary>
		/// Проверить подключен ли сокет к серверу
		/// </summary>
		/// <param name="Socket">Сокет для проверки</param>
		/// <returns>Подключен ли сокет</returns>
		public static bool IsConnected(this Socket Socket)
		{
			if (Socket == null)
			{
				throw new ArgumentNullException(nameof(Socket), "Сокет не указан");
			}

			bool Poll = Socket.Poll(1000, SelectMode.SelectRead);
			bool Data = Socket.Available == 0;

			return !(Poll && Data);
		}
	}
}
