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
        /// <param name="message">Сообщение</param>
        public void Message(string message)
        {
            Debug.WriteLine(message);
        }
        /// <summary>
        /// Отправить объект как сообщение
        /// </summary>
        /// <param name="obj">Объект для сообщения</param>
        public void Message(object obj)
        {
            Debug.WriteLine(obj);
        }

        /// <summary>
        /// Отправить предупреждение
        /// </summary>
        /// <param name="message">Предупреждение</param>
        public void Warning(string message)
        {
            Debug.WriteLine("Предупреждение: " + message);
        }
        /// <summary>
        /// Отправить объект как предупреждение
        /// </summary>
        /// <param name="obj">Объект для предупреждения</param>
        public void Warning(object obj)
        {
            Debug.WriteLine("Предупреждение: " + obj);
        }

        /// <summary>
        /// Отправить сообщение об ошибке
        /// </summary>
        /// <param name="message">Текст ошибки</param>
        public void Error(string message)
        {
            Debug.Fail(message);
        }
        /// <summary>
        /// Отправить объект как сообщение об ошибке
        /// </summary>
        /// <param name="obj">Объект для сообщения об ошибке</param>
        public void Error(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj), "Объект для вывода не указан");
            }

            Debug.Fail(obj.ToString());
        }

        #region Статика

        /// <summary>
        /// Глобальный экземпляр стандартного логгера
        /// </summary>
        public static readonly StandardLogger Instance = new();

        #endregion
    }
}
