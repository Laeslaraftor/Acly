using System;
using System.ComponentModel;
using System.Net.Sockets;

namespace Acly.Requests
{
    /// <summary>
    /// Интерфейс управления сервером
    /// </summary>
    public interface IServer : IDisposable, ISendable, INotifyPropertyChanging, INotifyPropertyChanged
    {
        /// <summary>
        /// Получение нового подключения
        /// </summary>
        /// <param name="socket">Подключение</param>
        public delegate void GetConnectedSocket(Socket socket);
        /// <summary>
        /// Получение данных
        /// </summary>
        /// <param name="socket">Отправитель</param>
        /// <param name="data">Данные</param>
        public delegate void ReceiveData(Socket socket, byte[] data);
        /// <summary>
        /// Получение информации об отключении от сервера
        /// </summary>
        /// <param name="socket">Отключающийся</param>
        /// <param name="Reason">Причина</param>
        public delegate void GetDisconnectedInfo(Socket socket, string Reason);

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
