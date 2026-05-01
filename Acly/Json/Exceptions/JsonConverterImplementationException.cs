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
        /// <param name="type">Тип объекта с неправильной реализацией</param>
#pragma warning disable CA1062
        public JsonConverterImplementationException(Type type) : base(string.Format(_message, type.Name, nameof(IJsonConverter)))
#pragma warning restore CA1062
        {
        }

        private const string _message = "Тип {0} указан как реализация JSON конвертера, но не реализует интерфейс {1}";
    }
}
