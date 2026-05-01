using System;

namespace Acly.Logger
{
    /// <summary>
    /// Исключение, вызываемое при неправильной реализации <see cref="ILogger"/>
    /// </summary>
    public sealed class LoggerImplementationException : ImplementationException
    {
        /// <summary>
        /// Вызвать исключение о неправильной реализации <see cref="ILogger"/>
        /// </summary>
        /// <param name="type">Тип с неправильной реализацией</param>
#pragma warning disable CA1062
        public LoggerImplementationException(Type type) : base(string.Format(_message, type.Name, "Logger", nameof(ILogger)))
#pragma warning restore CA1062
        {

        }

        private const string _message = "Тип {0} был помечен как реализация {1}, но не реализует {2}";
    }
}
