using System;

namespace Acly
{
    /// <summary>
    /// Аргументы события отправки исключения
    /// </summary>
    /// <param name="exception">Отправленное исключение</param>
#pragma warning disable CA1711 // Идентификаторы не должны иметь неправильных суффиксов
    public readonly struct ExceptionSendEventArgs(Exception exception)
#pragma warning restore CA1711 // Идентификаторы не должны иметь неправильных суффиксов
    {
        /// <summary>
        /// Отправленное исключение
        /// </summary>
        public Exception Exception { get; } = exception;
    }
}
