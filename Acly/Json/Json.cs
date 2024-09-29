using Acly.JsonData.Implementations;
using System;
using System.Threading.Tasks;

namespace Acly.JsonData
{
	/// <summary>
	/// Класс для работы с JSON данными
	/// </summary>
#pragma warning disable CA1724
	public static class Json
#pragma warning restore CA1724
	{
		private static readonly IJsonConverter _Default = new DefaultJsonConverter();
		private static IJsonConverter? _CustomConverter;

		#region Управление

		/// <summary>
		/// Конвертировать JSON в объект указанного типа
		/// </summary>
		/// <typeparam name="T">Тип объекта в который будет конвертирован JSON</typeparam>
		/// <param name="Json">JSON строка</param>
		/// <returns>Конвертированные объект</returns>
		public static async Task<T> Convert<T>(string Json)
		{
			IJsonConverter Converter = await GetConverter();

			return Converter.Convert<T>(Json);
		}
		/// <summary>
		/// Конвертировать объект в JSON строку
		/// </summary>
		/// <param name="Object">Объект для конвертации в строку</param>
		/// <returns>JSON строка</returns>
		public static async Task<string> Convert(object Object)
		{
			IJsonConverter Converter = await GetConverter();

			return Converter.Convert(Object);
		}

		#endregion

		#region Получение реализации

		private static async Task<IJsonConverter> GetConverter()
		{
			_CustomConverter ??= await GetCustomImplementation();

			if (_CustomConverter == null)
			{
				return _Default;
			}

			return _CustomConverter;
		}
		private static async Task<IJsonConverter?> GetCustomImplementation()
		{
			Type? Implementation = await JsonImplementations.GetConverterType();

			if (Implementation == null)
			{
				return null;
			}
			if (!Implementation.IsImplementsInterface<IJsonConverter>())
			{
				throw new JsonConverterImplementationException(Implementation);
			}

			return (IJsonConverter)Activator.CreateInstance(Implementation);
		}

		#endregion
	}
}
