using System;
#if !DEBUG
using System.Diagnostics;
#endif

namespace Acly.Logger
{
    /// <summary>
    /// Реализация <see cref="ILogger"/> по умолчанию
    /// </summary>
    public class StandardLogger : ILogger
    {
        /// <summary>
        /// Отправить объект как сообщение
        /// </summary>
        /// <param name="obj">Объект для сообщения</param>
        public void Message(object obj)
        {
#if DEBUG
            Console.WriteLine(obj);
#else
            Debug.WriteLine(obj);
#endif
        }

        /// <summary>
        /// Отправить объект как предупреждение
        /// </summary>
        /// <param name="obj">Объект для предупреждения</param>
        public void Warning(object obj)
        {
#if DEBUG
            Console.WriteLine("Предупреждение: " + obj);
#else
            Debug.WriteLine("Предупреждение: " + obj);
#endif
        }

        /// <summary>
        /// Отправить объект как сообщение об ошибке
        /// </summary>
        /// <param name="obj">Объект для сообщения об ошибке</param>
        public void Error(object obj)
        {
#if DEBUG
            Console.WriteLine($"Ошибка: {obj}");
#else
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj), "Объект для вывода не указан");
            }

            Debug.Fail(obj.ToString());
#endif
        }

        #region Статика

        /// <summary>
        /// Глобальный экземпляр стандартного логгера
        /// </summary>
        public static readonly StandardLogger Instance = new();

        #endregion
    }
}
