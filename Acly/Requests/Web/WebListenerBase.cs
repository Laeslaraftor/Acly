using System;
using System.Net;
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
        /// <param name="prefix">URI префикс для прослушивания. Например, http://localhost:8080/</param>
        protected WebListenerBase(string prefix)
        {
            Listener = new();
            Listener.Prefixes.Add(prefix);
        }

        /// <summary>
        /// Запущено ли прослушивание
        /// </summary>
        public override bool IsStarted => Listener.IsListening;

        /// <summary>
        /// <see cref="HttpListener"/>
        /// </summary>
        protected HttpListener Listener { get; }

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
        /// <param name="isDisposing"><inheritdoc/></param>
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

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
                while (!IsDisposed)
                {
                    if (!Listen())
                    {
                        Log.Warning("Http прослушивание остановлено из-за возникшей проблемы!");
                        break;
                    }
                }
            });

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
                catch (Exception error)
                {
                    OnHandledException(error);
                    return false;
                }

                if (context != null)
                {
                    OnHandledRequest(context);
                }
            }

            return true;
        }

        #endregion

        #region События

        /// <summary>
        /// Вызывается при получении веб запроса
        /// </summary>
        /// <param name="context">Запрос</param>
        protected abstract void OnHandledRequest(HttpListenerContext context);
        /// <summary>
        /// Вызывается при обработке исключения
        /// </summary>
        /// <param name="error">Обрабатываемое исключение</param>
        protected virtual void OnHandledException(Exception error)
        {
            Log.Error(error);
        }

        #endregion
    }
}
