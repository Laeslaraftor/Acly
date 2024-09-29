using System;
using System.Diagnostics;

namespace Acly.Logger
{
	/// <summary>
	/// Реализация <see cref="ILogger"/> по умолчанию
	/// </summary>
	public class StandardLogger : ILogger
	{
		/// <summary>
		/// Отправить обычное сообщение
		/// </summary>
		/// <param name="Message">Сообщение</param>
		public void Message(string Message)
		{
			Debug.WriteLine(Message);
		}
		/// <summary>
		/// Отправить объект как сообщение
		/// </summary>
		/// <param name="Object">Объект для сообщения</param>
		public void Message(object Object)
		{
			Debug.WriteLine(Object);
		}

		/// <summary>
		/// Отправить предупреждение
		/// </summary>
		/// <param name="Message">Предупреждение</param>
		public void Warning(string Message)
		{
			Debug.WriteLine("Предупреждение: " + Message);
		}
		/// <summary>
		/// Отправить объект как предупреждение
		/// </summary>
		/// <param name="Object">Объект для предупреждения</param>
		public void Warning(object Object)
		{
			Debug.WriteLine("Предупреждение: " + Object);
		}

		/// <summary>
		/// Отправить сообщение об ошибке
		/// </summary>
		/// <param name="Message">Текст ошибки</param>
		public void Error(string Message)
		{
			Debug.Fail(Message);
		}
		/// <summary>
		/// Отправить объект как сообщение об ошибке
		/// </summary>
		/// <param name="Object">Объект для сообщения об ошибке</param>
		public void Error(object Object)
		{
			if (Object == null)
			{
				throw new ArgumentNullException(nameof(Object), "Объект для вывода не указан");
			}

			Debug.Fail(Object.ToString());
		}
	}
}
