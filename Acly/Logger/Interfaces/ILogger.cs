namespace Acly
{
	/// <summary>
	/// Интерфейс отправителя сообщений
	/// </summary>
	public interface ILogger
	{
		/// <summary>
		/// Отправить обычное сообщение
		/// </summary>
		/// <param name="Message">Сообщение</param>
		public void Message(string Message);
		/// <summary>
		/// Отправить объект как сообщение
		/// </summary>
		/// <param name="Object">Объект для сообщения</param>
		public void Message(object Object);

		/// <summary>
		/// Отправить предупреждение
		/// </summary>
		/// <param name="Message">Предупреждение</param>
		public void Warning(string Message);
		/// <summary>
		/// Отправить объект как предупреждение
		/// </summary>
		/// <param name="Object">Объект для предупреждения</param>
		public void Warning(object Object);

		/// <summary>
		/// Отправить сообщение об ошибке
		/// </summary>
		/// <param name="Message">Текст ошибки</param>
		public void Error(string Message);
		/// <summary>
		/// Отправить объект как сообщение об ошибке
		/// </summary>
		/// <param name="Object">Объект для сообщения об ошибке</param>
		public void Error(object Object);
	}
}
