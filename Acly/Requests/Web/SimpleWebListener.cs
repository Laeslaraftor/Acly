using System;
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
        /// <param name="prefix"><inheritdoc/></param>
        public SimpleWebListener(string prefix) : base(prefix)
        {
        }

        /// <summary>
        /// Вызывается при получении запроса
        /// </summary>
        public event SimpleWebListenerRequestEvent? RequestHandled;
        /// <summary>
        /// Вызывается при обработке исключения
        /// </summary>
        public event SimpleWebListenerExceptionEvent? ExceptionHandled;

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
        /// <param name="context"><inheritdoc/></param>
        protected override void OnHandledRequest(HttpListenerContext context) => RequestHandled?.Invoke(this, context);
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="exception"><inheritdoc/></param>
        protected override void OnExceptionSent(Exception exception)
        {
            base.OnExceptionSent(exception);
            ExceptionHandled?.Invoke(this, exception);
        }

        #endregion
    }
}
