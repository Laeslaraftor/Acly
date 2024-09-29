using Acly.Requests;
using System;

namespace Acly.Performing
{
	/// <summary>
	/// Исключение, вызывающееся если во время загрузки произошла какая-то ошибка
	/// </summary>
	public sealed class LoadingException : Exception
	{
		/// <summary>
		/// Создать экземпляр исключения во время загрузки
		/// </summary>
		/// <param name="Code">Код ошибки</param>
		/// <param name="Message">Сообщение</param>
		public LoadingException(string Code, string Message) : base(Message)
		{
			this.Code = Code;
		}
		/// <summary>
		/// Создать экземпляр исключения во время загрузки
		/// </summary>
		/// <param name="Response">Ответ</param>
		/// <exception cref="ArgumentNullException"></exception>
#pragma warning disable CA1062
		public LoadingException(Response Response) : base(Response.Text)
#pragma warning restore CA1062
		{
			if (Response == null)
			{
				throw new ArgumentNullException(nameof(Response), "Ответ не указан");
			}

			Code = Response.Code;
		}

		/// <summary>
		/// Код ошибки
		/// </summary>
		public string Code { get; private set; }
	}
}
