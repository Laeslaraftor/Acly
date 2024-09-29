using System;
using System.Net.Sockets;

namespace Acly.Requests
{
	/// <summary>
	/// Интерфейс управления сервером
	/// </summary>
	public interface IServer : IDisposable, ISendable
	{
		/// <summary>
		/// Получение нового подключения
		/// </summary>
		/// <param name="Socket">Подключение</param>
		public delegate void GetConnectedSocket(Socket Socket);
		/// <summary>
		/// Получение данных
		/// </summary>
		/// <param name="Socket">Отправитель</param>
		/// <param name="Data">Данные</param>
		public delegate void ReceiveData(Socket Socket, byte[] Data);
		/// <summary>
		/// Получение информации об отключении от сервера
		/// </summary>
		/// <param name="Socket">Отключающийся</param>
		/// <param name="Reason">Причина</param>
		public delegate void GetDisconnectedInfo(Socket Socket, string Reason);

		/// <summary>
		/// Вызывается при новом подключении к серверу
		/// </summary>
		public event GetConnectedSocket Connected;
		/// <summary>
		/// Вызывается при получении данных
		/// </summary>
		public event ReceiveData Received;
		/// <summary>
		/// Вызывается при отключении от сервера
		/// </summary>
		public event GetDisconnectedInfo Disconnected;

		/// <summary>
		/// Интервал проверки сообщений
		/// </summary>
		public int ReceiveInterval { get; set; }
		/// <summary>
		/// Максимальное количество подключений. Установите -1, чтобы убрать ограничение
		/// </summary>
		public int MaximumConnections { get; set; }
		/// <summary>
		/// Количество текущих подключений
		/// </summary>
		public int TotalConnections { get; }

		/// <summary>
		/// Выключить сервер
		/// </summary>
		public void Shutdown();
	}
}
