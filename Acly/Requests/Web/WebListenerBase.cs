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
        /// <param name="Prefix">URI префикс для прослушивания. Например, http://localhost:8080/</param>
        protected WebListenerBase(string Prefix)
        {
            Listener = new();
            Listener.Prefixes.Add(Prefix);

            StartListenTask();
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
                    Listen();
                }
            }).ConfigureAwait(true);
        }
        private void Listen()
        {
            if (Listener.IsListening)
            {
                HttpListenerContext context = Listener.GetContext();

                if (context != null)
                {
                    OnHandledRequest(context);
                }
            }
        }

        #endregion

        #region События

        /// <summary>
        /// Вызывается при получении веб запроса
        /// </summary>
        /// <param name="Context">Запрос</param>
        protected abstract void OnHandledRequest(HttpListenerContext Context);

        #endregion
    }
}
