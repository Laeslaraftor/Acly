using Acly.Commands;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Acly.Requests
{
	/// <summary>
	/// Базовый класс сервера, принимающего команды
	/// </summary>
#pragma warning disable CA1012
	public abstract class CommandServerBase
#pragma warning restore CA1012
	{
		/// <summary>
		/// Сервер, принимающий команды
		/// </summary>
		/// <param name="Address">Адрес</param>
		/// <param name="Port">Порт сервера</param>
		public CommandServerBase(string Address, int Port) : this(Requests.Server.Create(Address, Port))
		{
		}
		/// <summary>
		/// Сервер, принимающий команды
		/// </summary>
		/// <param name="Address">Адрес</param>
		/// <param name="Port">Порт сервера</param>
		public CommandServerBase(IPAddress Address, int Port) : this(Requests.Server.Create(Address, Port))
		{
        }
        /// <summary>
		/// Сервер, принимающий команды
		/// </summary>
		/// <param name="Server">Сервер</param>
		/// <exception cref="InvalidOperationException"></exception>
        private CommandServerBase(IServer Server)
		{
            this.Server = Server ?? throw new ArgumentNullException(nameof(Server));

            Server.Connected += OnClientConnected;
            Server.Received += OnDataReceived;
            Server.Disconnected += OnClientDisconnected;
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
		protected IServer Server { get; private set; }
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
		}

		#endregion

		#region Конвертация

		/// <summary>
		/// Конвертировать массив байтов в UTF8 строку
		/// </summary>
		/// <param name="Data">Данные</param>
		/// <returns>UTF8 строка</returns>
		protected static string BytesToString(byte[] Data)
		{
			return BytesToString(Data, Encoding.UTF8);
		}
		/// <summary>
		/// Конвертировать массив байтов в строку указанной кодировки
		/// </summary>
		/// <param name="Data">Данные</param>
		/// <param name="Encoding">Кодировка</param>
		/// <returns>Строка</returns>
		protected static string BytesToString(byte[] Data, Encoding Encoding)
		{
			if (Encoding == null)
			{
				throw new ArgumentNullException(nameof(Encoding), "Кодировка не указана");
			}

			return Encoding.GetString(Data);
		}

		#endregion

		#region Команды

		/// <summary>
		/// Проверить доступна ли указанная команда
		/// </summary>
		/// <param name="Input">Команда для проверки</param>
		/// <returns>Доступна ли команда</returns>
		protected bool IsAvailable(Command Input)
		{
			foreach (var Cmd in AvailableCommands)
			{
				if (Cmd.Equals(Input))
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Отправить клиенту список доступных команд
		/// </summary>
		/// <param name="Socket">Клиент, которому надо отпрать команды</param>
		protected void SendAvailableCommandsList(Socket Socket)
		{
			string Result = "\nДоступные команды:\n";
			string Lines = "";

			foreach (var Cmd in AvailableCommands)
			{
				if (Lines.Length > 0)
				{
					Lines += "\n";
				}

				Lines += Cmd.ToString();
			}

			Socket.SendText(Result + Lines);
		}

		#endregion

		#region События

		/// <summary>
		/// Вызывается при получении доступной команды
		/// </summary>
		/// <param name="Socket">Клиент, отправивший команду</param>
		/// <param name="Command">Полученная команда</param>
		protected abstract void OnAvailableCommandReceived(Socket Socket, Command Command);
		/// <summary>
		/// Вызывается при получении доступной команды
		/// </summary>
		/// <param name="Socket">Клиент, отправивший команду</param>
		/// <param name="Command">Полученная команда</param>
		protected virtual void OnCommandReceived(Socket Socket, Command Command)
		{
			if (Command == HelpCommand)
			{
				SendAvailableCommandsList(Socket);
				return;
			}
			if (IsAvailable(Command))
			{
				OnAvailableCommandReceived(Socket, Command);
				return;
			}

			Socket.SendText("Неизвестная команда. Для просмотра списка доступных команд используйте /help");
		}

		/// <summary>
		/// Вызывается при подключении клиента
		/// </summary>
		/// <param name="Socket">Клиент</param>
		protected virtual void OnClientConnected(Socket Socket)
		{
		}
		/// <summary>
		/// Вызывается при получении данных от клиента
		/// </summary>
		/// <param name="Socket">Клиент, отправивший данные</param>
		/// <param name="Data">Полученные данные</param>
		protected virtual void OnDataReceived(Socket Socket, byte[] Data)
		{
			string Value = BytesToString(Data);

			if (Command.TryParse(Value, out Command? Result))
			{
#pragma warning disable CS8629
				OnCommandReceived(Socket, Result.Value);
				return;
#pragma warning restore CS8629
			}

			SendAvailableCommandsList(Socket);
		}
		/// <summary>
		/// Вызывается при отключении клиента
		/// </summary>
		/// <param name="Socket">Отключившийся клиент</param>
		/// <param name="Reason">Причина отключения</param>
		protected virtual void OnClientDisconnected(Socket Socket, string Reason)
		{
		}

		#endregion
	}
}
