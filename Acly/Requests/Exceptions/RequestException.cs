using System;
using System.Net;

namespace Acly.Requests
{
    /// <summary>
    /// Исключение, вызывающееся при возникновении какой-либо ошибки запроса
    /// </summary>
    /// <remarks>
    /// Вызвать ошибку запроса
    /// </remarks>
    /// <param name="url">Адрес запроса</param>
    /// <param name="code">Код ошибки</param>
    [Serializable]
    public sealed class RequestException(string url, HttpStatusCode code)
        : Exception(string.Format(_message, url, (int)code, code))
    {

        /// <summary>
        /// Адрес запроса
        /// </summary>
        public string Url { get; private set; } = url;
        /// <summary>
        /// HTTP код
        /// </summary>
        public HttpStatusCode Code { get; private set; } = code;

        private const string _message = "Запрос по адресу '{0}' вернул код ошибки {1} {2}";
    }
}
