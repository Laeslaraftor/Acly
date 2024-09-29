using System;
using System.Net;

namespace Acly.Requests
{
    /// <summary>
    /// Исключение, вызывающееся при возникновении какой-либо ошибки запроса
    /// </summary>
    [Serializable]
    public sealed class RequestException : Exception
	{
		/// <summary>
		/// Вызвать ошибку запроса
		/// </summary>
		/// <param name="Url">Адрес запроса</param>
		/// <param name="Code">Код ошибки</param>
		public RequestException(string Url, HttpStatusCode Code) : base(string.Format(_Message, Url, (int)Code, Code))
		{
			this.Url = Url;
			this.Code = Code;
		}

		/// <summary>
		/// Адрес запроса
		/// </summary>
		public string Url { get; private set; }
		/// <summary>
		/// HTTP код
		/// </summary>
		public HttpStatusCode Code { get; private set; }

		private const string _Message = "Запрос по адресу '{0}' вернул код ошибки {1} {2}";
	}
}
