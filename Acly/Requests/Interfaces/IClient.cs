using System;

namespace Acly.Requests
{
	/// <summary>
	/// Интерфейс подключения к серверу
	/// </summary>
	public interface IClient : IDisposable, ISendable
	{
		/// <summary>
		/// Получение данных от сервера
		/// </summary>
		/// <param name="Data">Полученные данные</param>
		public delegate void ReceiveData(byte[] Data);
		/// <summary>
		/// Получение сообщение об отключении от сервера
		/// </summary>
		/// <param name="Reason">Причина отключения</param>
		public delegate void GetDisconnectInfo(string Reason);

		/// <summary>
		/// Вызывается при получении данных от сервера
		/// </summary>
		public event ReceiveData Received;
		/// <summary>
		/// Вызывается при отключении от сервера
		/// </summary>
		public event GetDisconnectInfo Disconnected;

		/// <summary>
		/// Интервал проверки сообщений от сервера
		/// </summary>
		public int ReceiveInterval { get; set; }

		/// <summary>
		/// Отключиться от сервера
		/// </summary>
		public void Disconnect();
	}
}
