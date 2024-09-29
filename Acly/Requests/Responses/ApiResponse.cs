using Newtonsoft.Json;
using System;

namespace Acly.Requests
{
	/// <summary>
	/// Класс, реализующий ответ Api сервера
	/// </summary>
	[Serializable]
	public class ApiResponse : Response
	{
		/// <summary>
		/// Создать экземпляр ответа сервера
		/// </summary>
		public ApiResponse() { }
		/// <summary>
		/// Создать экземпляр ответа сервера
		/// </summary>
		/// <param name="Status">Статус ответа</param>
		/// <param name="Code">Код ответа</param>
		/// <param name="Text">Сообщение ответа</param>
		public ApiResponse(ApiResponseStatus Status, string Code, string Text)
		{
			this.Status = Status;
			this.Code = Code;
			this.Text = Text;
		}
		/// <summary>
		/// Создать экземпляр ответа сервера
		/// </summary>
		/// <param name="Response">Ответ</param>
		/// <param name="Status">Статус ответа</param>
		/// <exception cref="ArgumentNullException"></exception>
		public ApiResponse(Response Response, ApiResponseStatus Status)
		{
			if (Response == null)
			{
				throw new ArgumentNullException(nameof(Response), "Ответ не указан");
			}

			this.Status = Status;
			Code = Response.Code;
			Text = Response.Text;
		}
		/// <summary>
		/// Создать экземпляр ответа сервера
		/// </summary>
		/// <param name="Exception">Исключение как ответ сервера</param>
		/// <exception cref="ArgumentNullException">Исключение не указано</exception>
		public ApiResponse(Exception Exception) : base(Exception)
		{
		}

		/// <summary>
		/// Статус ответа
		/// </summary>
		[JsonProperty("status")]
		public ApiResponseStatus Status { get; set; } = ApiResponseStatus.Error;

		/// <summary>
		/// Является ли ответ успешным
		/// </summary>
		public bool IsSuccess => Status == ApiResponseStatus.Success;

		/// <summary>
		/// Конвертировать ответ в одну строку
		/// </summary>
		/// <returns>Ответ как одна строка</returns>
		public override string ToString()
		{
			return Status + " | " + Code + " | " + Text;
		}
	}
}