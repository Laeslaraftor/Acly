using Newtonsoft.Json;
using System;

namespace Acly.JsonData.Implementations
{
	/// <summary>
	/// JsonConverter по умолчанию
	/// </summary>
	public class DefaultJsonConverter : IJsonConverter
	{
		/// <summary>
		/// Конвертировать JSON строку в указанные объект
		/// </summary>
		/// <typeparam name="T">Объект в который будет конвертирована JSON строка</typeparam>
		/// <param name="Json">JSON строка</param>
		/// <returns>Конвертированный объект</returns>
		public T Convert<T>(string Json)
		{
			T? Result = JsonConvert.DeserializeObject<T>(Json);
			Result ??= Activator.CreateInstance<T>();

			return Result;
		}
		/// <summary>
		/// Конвертировать объект в JSON строку
		/// </summary>
		/// <param name="Object">Объект для конвертации</param>
		/// <returns>JSON строка</returns>
		public string Convert(object Object)
		{
			return JsonConvert.SerializeObject(Object);
		}
	}
}
