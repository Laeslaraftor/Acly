using System;

namespace Acly.JsonData
{
	/// <summary>
	/// Исключение, вызывающееся при неправильной реализации <see cref="IJsonConverter"/>
	/// </summary>
	public class JsonConverterImplementationException : ImplementationException
	{
		/// <summary>
		/// Вызвать исключение неправильной реализации <see cref="IJsonConverter"/>
		/// </summary>
		/// <param name="Type">Тип объекта с неправильной реализацией</param>
#pragma warning disable CA1062
		public JsonConverterImplementationException(Type Type) : base(string.Format(_Message, Type.Name, nameof(IJsonConverter)))
#pragma warning restore CA1062
		{
		}

		private const string _Message = "Тип {0} указан как реализация JSON конвертера, но не реализует интерфейс {1}";
	}
}
