using System;

namespace Acly.Player
{
    /// <summary>
    /// Исключение, вызывающееся при неправильной реализации <see cref="ISimplePlayer"/>
    /// </summary>
    public sealed class SimplePlayerImplementationException : ImplementationException
    {
        /// <summary>
        /// Вызвать исключение неправильной реализации <see cref="ISimplePlayer"/>
        /// </summary>
        /// <param name="implementation">Тип объекта с неправильной реализацией</param>
#pragma warning disable CA1062 // Проверить аргументы или открытые методы
        public SimplePlayerImplementationException(Type implementation) : base(string.Format(_message, implementation.Name, nameof(ISimplePlayer)))
#pragma warning restore CA1062 // Проверить аргументы или открытые методы
        {
        }

        private const string _message = "Объект {0} помечен как реализация SimplePlayer, но не реализует интерфейс {1}";
    }
}
