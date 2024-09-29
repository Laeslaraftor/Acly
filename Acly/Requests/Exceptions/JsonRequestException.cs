using System;

namespace Acly.Requests
{
	/// <summary>
	/// Исключение, вызывающееся при возникновении какой-либо ошибки запроса JSON
	/// </summary>
	[Serializable]
	public sealed class JsonRequestException : Exception
	{
		/// <summary>
		/// Вызвать ошибку запроса JSON
		/// </summary>
		/// <param name="Url">Адрес запроса</param>
		/// <param name="Code">Код ошибки</param>
		/// <param name="Response">Ответ</param>
		public JsonRequestException(string Url, string Code, string Response) : base(string.Format(_Message, Url, Code, Code, Response))
		{
			this.Url = Url;
			this.Code = Code;
			this.Response = Response;
		}

		/// <summary>
		/// Адрес запроса
		/// </summary>
		public string Url { get; private set; }
		/// <summary>
		/// Код ошибки запроса
		/// </summary>
		public string Code { get; private set; }
		/// <summary>
		/// Ответ на запрос
		/// </summary>
		public string Response { get; private set; }

		private const string _Message = "Запрос по адресу '{0}' вернул код ошибки {1} {2} с результатом '{3}'";
	}
}
