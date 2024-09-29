using Acly.Commands;
using Acly.Requests;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace Acly.Test
{
    public class CommandServer : CommandServerBase
    {
        public CommandServer(string Address, int Port) : base(Address, Port)
        {
        }
        public CommandServer(IPAddress Address, int Port) : base(Address, Port)
        {
        }

        protected override IEnumerable<CommandTemplate> AvailableCommands => _Commands;

        private readonly List<CommandTemplate> _Commands = new()
            {
                new("message", "сообщение"),
                new("request", "адрес"),
                new("open", "путь к файлу")
            };
        private int _Connections = 0;

        #region Команды

        private void MessageCommand(Socket Socket, Command Command)
        {
            if (Command.Parameters.Count == 0)
            {
                Socket.SendText("Правильное использование: /message [сообщение]");
            }

            Socket.SendText(Command.Parameters[0]);
        }
        private async void RequestCommand(Socket Socket, Command Command)
        {
            if (Command.Parameters.Count == 0)
            {
                Socket.SendText("Правильное использование: /request [адрес]");
            }

            try
            {
                string Request = await Ajax.Get(Command.Parameters[0]);
                Socket.SendText(Request);
            }
            catch (Exception Error)
            {
                Socket.SendText(Error.Message);
            }
        }
        private void OpenCommand(Socket Socket, Command Command)
        {
            if (Command.Parameters.Count == 0)
            {
                Socket.SendText("Правильное использование: /open [путь к файлу]");
            }

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = Command.Parameters[0],
                    UseShellExecute = true
                });
            }
            catch (Exception Error)
            {
                Socket.SendText(Error.Message);
            }
        }

        #endregion

        protected override void OnAvailableCommandReceived(Socket Socket, Command Command)
        {
            switch (Command.Name)
            {
                case "message":
                    MessageCommand(Socket, Command);
                    break;
                case "request":
                    RequestCommand(Socket, Command);
                    break;
                case "open":
                    OpenCommand(Socket, Command);
                    break;
            }

            Console.WriteLine(Command.ToString());
        }
        protected override void OnClientConnected(Socket Socket)
        {
            base.OnClientConnected(Socket);
            _Connections++;
            Console.WriteLine("Новое подключение (всего {0})", _Connections);
        }
        protected override void OnClientDisconnected(Socket Socket, string Reason)
        {
            base.OnClientDisconnected(Socket, Reason);
            _Connections--;
            Console.WriteLine("Клиент отключился. Осталось: {0}", _Connections);
        }
    }
}
