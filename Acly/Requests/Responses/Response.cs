using Acly.Performing;
using Newtonsoft.Json;
using System;

namespace Acly.Requests
{
	/// <summary>
	/// Класс, реализующий ответ
	/// </summary>
	[Serializable]
	public class Response
	{
		/// <summary>
		/// Создать экземпляр ответа
		/// </summary>
		public Response() { }
		/// <summary>
		/// Создать экземпляр ответа
		/// </summary>
		/// <param name="Code">Код ответа</param>
		/// <param name="Text">Сообщение ответа</param>
		public Response(string Code, string Text)
		{
			this.Code = Code;
			this.Text = Text;
			Exception = new ResponseException(this);
		}
		/// <summary>
		/// Создать экземпляр ответа
		/// </summary>
		/// <param name="Exception">Исключение как ответ</param>
		/// <exception cref="ArgumentNullException">Исключение не указано</exception>
		public Response(Exception Exception)
		{
			if (Exception == null)
			{
				throw new ArgumentNullException(nameof(Exception), "Исключение не указано");
			}

			Type ExceptionType = Exception.GetType();

			if (ExceptionType == typeof(LoadingException))
			{
				LoadingException Loading = (LoadingException)Exception;
				Code = Loading.Code;
			}
			else
			{
				Code = Exception.GetType().Name;
			}

			Text = Exception.Message;
			this.Exception = Exception;
		}
		/// <summary>
		/// Создать экземпляр ответа
		/// </summary>
		/// <param name="Response">Ответ для копирования</param>
		/// <exception cref="ArgumentNullException">Ответ не указан</exception>
		public Response(Response Response)
		{
			if (Response == null)
			{
				throw new ArgumentNullException(nameof(Response), "Ответ для копирования не указан");
			}

			Code = Response.Code;
			Text = Response.Text;
			Exception = new ResponseException(this);
		}

		/// <summary>
		/// Код ответа
		/// </summary>
		[JsonProperty("code")]
		public string Code { get; set; } = _DefaultCode;
		/// <summary>
		/// Сообщение ответа
		/// </summary>
		[JsonProperty("text")]
		public string Text { get; set; } = _DefaultMessage;
		/// <summary>
		/// Ответ как исключение
		/// </summary>
		[JsonIgnore]
		[field: NonSerialized]
#pragma warning disable CA2201
		public Exception Exception { get; private set; } = new(_DefaultMessage);
#pragma warning restore CA2201

		/// <summary>
		/// Конвертировать ответ в одну строку
		/// </summary>
		/// <returns>Ответ как одна строка</returns>
		public override string ToString()
		{
			return Code + " | " + Text;
		}

		private const string _DefaultCode = "undefined";
		private const string _DefaultMessage = "Произошла неизвестная ошибка";
	}
}
