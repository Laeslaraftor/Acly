using System.Net;

namespace Acly.Requests
{
    /// <summary>
    /// Простой прослушиватель веб запросов
    /// </summary>
    public class SimpleWebListener : WebListenerBase
    {
        /// <summary>
        /// Простой прослушиватель веб запросов
        /// </summary>
        /// <param name="Prefix"><inheritdoc/></param>
        public SimpleWebListener(string Prefix) : base(Prefix)
        {
        }

        /// <summary>
        /// Вызывается при получении запроса
        /// </summary>
        public event SimpleWebListenerRequestEvent? RequestHandled;

        /// <summary>
        /// Префиксы
        /// </summary>
        public HttpListenerPrefixCollection Prefixes => Listener.Prefixes;
        /// <summary>
        /// Времена ожидания
        /// </summary>
        public HttpListenerTimeoutManager TimeoutManager => Listener.TimeoutManager;

        #region События

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="Context"><inheritdoc/></param>
        protected override void OnHandledRequest(HttpListenerContext Context) => RequestHandled?.Invoke(this, Context);

        #endregion
    }
}
