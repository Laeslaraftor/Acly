using Acly.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Acly.Requests
{
    /// <summary>
    /// Базовый класс сервера, принимающего команды
    /// </summary>
#pragma warning disable CA1012
    public abstract class CommandServerBase : Disposable
#pragma warning restore CA1012
    {
        /// <summary>
        /// Сервер, принимающий команды
        /// </summary>
        /// <param name="address">Адрес</param>
        /// <param name="port">Порт сервера</param>
        public CommandServerBase(string address, int port)
            : this(Requests.Server.Create(address, port))
        {
        }
        /// <summary>
        /// Сервер, принимающий команды
        /// </summary>
        /// <param name="address">Адрес</param>
        /// <param name="port">Порт сервера</param>
        public CommandServerBase(IPAddress address, int port) : this(Requests.Server.Create(address, port))
        {
        }
        /// <summary>
		/// Сервер, принимающий команды
		/// </summary>
		/// <param name="server">Сервер</param>
		/// <exception cref="InvalidOperationException"></exception>
        private CommandServerBase(IServer server)
        {
            Server = server ?? throw new ArgumentNullException(nameof(server));

            server.Connected += OnClientConnected;
            server.Received += OnDataReceived;
            server.Disconnected += OnClientDisconnected;
            server.PropertyChanged += OnServerPropertyChanged;
            server.PropertyChanging += OnServerPropertyChanging;
        }

        /// <summary>
        /// Количество текущих подключений
        /// </summary>
        public int TotalConnections => Server.TotalConnections;
        /// <summary>
        /// Максимальное количество подключений. Установите -1, чтобы убрать ограничение
        /// </summary>
        public int MaximumConnections
        {
            get => Server.MaximumConnections;
            set => Server.MaximumConnections = value;
        }

        /// <summary>
        /// Сервер
        /// </summary>
        protected IServer Server { get; }
        /// <summary>
        /// Список доступных команд
        /// </summary>
        protected abstract IEnumerable<CommandTemplate> AvailableCommands { get; }
        /// <summary>
        /// Команда для отображения списка доступных команд
        /// </summary>
        protected static readonly Command HelpCommand = new("/help");

        #region Управление

        /// <summary>
        /// Выключить сервер
        /// </summary>
        public virtual void Shutdown()
        {
            Server.Shutdown();

            Server.Connected -= OnClientConnected;
            Server.Received -= OnDataReceived;
            Server.Disconnected -= OnClientDisconnected;
            Server.PropertyChanged -= OnServerPropertyChanged;
            Server.PropertyChanging -= OnServerPropertyChanging;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="isDisposing"><inheritdoc/></param>
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            Shutdown();
        }

        #endregion

        #region Конвертация

        /// <summary>
        /// Конвертировать массив байтов в UTF8 строку
        /// </summary>
        /// <param name="data">Данные</param>
        /// <returns>UTF8 строка</returns>
        protected static string BytesToString(byte[] data)
        {
            return BytesToString(data, Encoding.UTF8);
        }
        /// <summary>
        /// Конвертировать массив байтов в строку указанной кодировки
        /// </summary>
        /// <param name="data">Данные</param>
        /// <param name="encoding">Кодировка</param>
        /// <returns>Строка</returns>
        protected static string BytesToString(byte[] data, Encoding encoding)
        {
            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding), "Кодировка не указана");
            }

            return encoding.GetString(data);
        }

        #endregion

        #region Команды

        /// <summary>
        /// Проверить доступна ли указанная команда
        /// </summary>
        /// <param name="input">Команда для проверки</param>
        /// <returns>Доступна ли команда</returns>
        protected bool IsAvailable(Command input)
        {
            foreach (var command in AvailableCommands)
            {
                if (command.Equals(input))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Отправить клиенту список доступных команд
        /// </summary>
        /// <param name="socket">Клиент, которому надо отравить команды</param>
        protected void SendAvailableCommandsList(Socket socket)
        {
            if (socket == null)
            {
                throw new ArgumentNullException(nameof(socket));
            }

            string result = "\nДоступные команды:\n";
            string lines = string.Empty;

            foreach (var command in AvailableCommands)
            {
                if (lines.Length > 0)
                {
                    lines += "\n";
                }

                lines += command.ToString();
            }

            socket.SendText(result + lines);
        }

        #endregion

        #region События

        /// <summary>
        /// Вызывается при получении доступной команды
        /// </summary>
        /// <param name="socket">Клиент, отправивший команду</param>
        /// <param name="command">Полученная команда</param>
        protected abstract void OnAvailableCommandReceived(Socket socket, Command command);
        /// <summary>
        /// Вызывается при получении доступной команды
        /// </summary>
        /// <param name="socket">Клиент, отправивший команду</param>
        /// <param name="command">Полученная команда</param>
        protected virtual void OnCommandReceived(Socket socket, Command command)
        {
            if (socket == null)
            {
                throw new ArgumentNullException(nameof(socket));
            }
            if (command == HelpCommand)
            {
                SendAvailableCommandsList(socket);
                return;
            }
            if (IsAvailable(command))
            {
                OnAvailableCommandReceived(socket, command);
                return;
            }

            socket.SendText("Неизвестная команда. Для просмотра списка доступных команд используйте /help");
        }

        /// <summary>
        /// Вызывается при подключении клиента
        /// </summary>
        /// <param name="socket">Клиент</param>
        protected virtual void OnClientConnected(Socket socket)
        {
        }
        /// <summary>
        /// Вызывается при получении данных от клиента
        /// </summary>
        /// <param name="socket">Клиент, отправивший данные</param>
        /// <param name="data">Полученные данные</param>
        protected virtual void OnDataReceived(Socket socket, byte[] data)
        {
            string Value = BytesToString(data);

            if (Command.TryParse(Value, out var result))
            {
                OnCommandReceived(socket, result.Value);
                return;
            }

            SendAvailableCommandsList(socket);
        }
        /// <summary>
        /// Вызывается при отключении клиента
        /// </summary>
        /// <param name="socket">Отключившийся клиент</param>
        /// <param name="reason">Причина отключения</param>
        protected virtual void OnClientDisconnected(Socket socket, string reason)
        {
        }

        private void OnServerPropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            OnPropertyChanging(e);
        }
        private void OnServerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e);
        }

        #endregion
    }
}
