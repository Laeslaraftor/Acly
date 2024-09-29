using System;
using System.Runtime.Serialization;

namespace Acly
{
	/// <summary>
	/// Исключение при неправильной реализации
	/// </summary>
	public class ImplementationException : Exception
	{
		/// <summary>
		/// Вызвать исключение неправильной реализации
		/// </summary>
		public ImplementationException()
		{
		}
		/// <summary>
		/// Вызвать исключение неправильной реализации
		/// </summary>
		/// <param name="message">Сообщение</param>
		public ImplementationException(string message) : base(message)
		{
		}
		/// <summary>
		/// Вызвать исключение неправильной реализации
		/// </summary>
		/// <param name="info">Информация</param>
		/// <param name="context">Контекст</param>
		public ImplementationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
		/// <summary>
		/// Вызвать исключение неправильной реализации
		/// </summary>
		/// <param name="message">Сообщение</param>
		/// <param name="innerException">Включённое искючение</param>
		public ImplementationException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
