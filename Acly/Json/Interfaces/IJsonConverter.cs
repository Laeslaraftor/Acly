namespace Acly.JsonData
{
	/// <summary>
	/// Интерфейс реализации JsonConverter
	/// </summary>
	public interface IJsonConverter
	{
		/// <summary>
		/// Конвертировать JSON в объект указанного типа
		/// </summary>
		/// <typeparam name="T">Тип объекта в который будет конвертирован JSON</typeparam>
		/// <param name="Json">JSON строка</param>
		/// <returns>Конвертированные объект</returns>
		public T Convert<T>(string Json);
		/// <summary>
		/// Конвертировать объект в JSON строку
		/// </summary>
		/// <param name="Object">Объект для конвертации в строку</param>
		/// <returns>JSON строка</returns>
		public string Convert(object Object);
	}
}
