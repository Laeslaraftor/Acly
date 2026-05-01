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
        /// <param name="data">Полученные данные</param>
        public delegate void ReceiveData(byte[] data);
        /// <summary>
        /// Получение сообщение об отключении от сервера
        /// </summary>
        /// <param name="reason">Причина отключения</param>
        public delegate void GetDisconnectInfo(string reason);

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
