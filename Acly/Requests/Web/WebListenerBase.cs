using System;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Acly.Requests
{
    /// <summary>
    /// Базовый класс веб прослушивания
    /// </summary>
    public abstract class WebListenerBase : ListenerBase
    {
        /// <summary>
        /// Создать экземпляр класс веб прослушивания
        /// </summary>
        /// <param name="Prefix">URI префикс для прослушивания. Например, http://localhost:8080/</param>
        protected WebListenerBase(string Prefix)
        {
            Listener = new();
            Listener.Prefixes.Add(Prefix);

            //StartListenTask();
        }

        /// <summary>
        /// Запущено ли прослушивание
        /// </summary>
        public override bool IsStarted => Listener.IsListening;

        /// <summary>
        /// <see cref="HttpListener"/>
        /// </summary>
        protected HttpListener Listener { get; }
        /// <summary>
        /// Очищен ли прослушиватель
        /// </summary>
        protected bool Disposed { get; private set; }

        #region Управление

        /// <summary>
        /// Начать прослушивание
        /// </summary>
        public override bool Start()
        {
            if (IsStarted)
            {
                return false;
            }

            Listener.Start();
            StartListenTask();
            InvokeStartedStateChanged();

            return true;
        }
        /// <summary>
        /// Остановить прослушивание
        /// </summary>
        public override bool Stop()
        {
            if (!IsStarted)
            {
                return false;
            }

            Listener.Stop();
            InvokeStartedStateChanged();

            return true;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
#pragma warning disable CA1816
            GC.SuppressFinalize(this);
#pragma warning restore CA1816
        }

        /// <summary>
        /// Очистить прослушиватель
        /// </summary>
        /// <param name="OnlyLocal"></param>
        protected virtual void Dispose(bool OnlyLocal)
        {
            Disposed = true;
            Listener.Stop();
            Listener.Abort();
            Listener.Close();

        }

        #endregion

        #region Чтение

        private async void StartListenTask()
        {
            await Task.Run(() =>
            {
                while (!Disposed)
                {
                    if (!Listen())
                    {
                        Debug.WriteLine("Http прослушивание остановлено из-за возникшей проблемы!");
                        break;
                    }
                }
            }).ConfigureAwait(true);

            if (IsStarted)
            {
                Stop();
            }
        }
        private bool Listen()
        {
            if (Listener.IsListening)
            {
                HttpListenerContext context;

                try
                {
                    context = Listener.GetContext();
                }
                catch (Exception Error)
                {
                    OnHandledException(Error);
                    PrintException(Error);
                    return false;
                }

                if (context != null)
                {
                    OnHandledRequest(context);
                }
            }

            return true;
        }

        private static void PrintException(Exception Error)
        {
            StringBuilder Builder = new();
            Builder.AppendLine($"Произошла ошибка {Error.GetType().Name}");
            Builder.AppendLine(Error.Message);
            Builder.AppendLine(Error.StackTrace);

            Debug.WriteLine(Builder.ToString());
        }

        #endregion

        #region События

        /// <summary>
        /// Вызывается при получении веб запроса
        /// </summary>
        /// <param name="Context">Запрос</param>
        protected abstract void OnHandledRequest(HttpListenerContext Context);
        /// <summary>
        /// Вызывается при обработке исключения
        /// </summary>
        /// <param name="Error">Обрабатываемое исключение</param>
        protected virtual void OnHandledException(Exception Error)
        {
        }

        #endregion
    }
}
