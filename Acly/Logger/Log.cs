using Acly.Logger;
using System;
using System.Threading.Tasks;

namespace Acly
{
    /// <summary>
    /// Класс для отправки сообщений
    /// </summary>
    public static class Log
    {
        /// <summary>
        /// Если Logger выключен, то сообщения отправляться не будут
        /// </summary>
        public static bool Enabled { get; set; } = true;

        private static ILogger? _logger;

        #region Сообщения

        /// <summary>
        /// Отправить обычное сообщение
        /// </summary>
        /// <param name="message">Сообщение</param>
        public static async void Message(string message)
        {
            ILogger? logger = await GetImplementation();
            logger?.Message(message);
        }
        /// <summary>
        /// Отправить объект как сообщение
        /// </summary>
        /// <param name="obj">Объект для сообщения</param>
        public static async void Message(object obj)
        {
            ILogger? logger = await GetImplementation();
            logger?.Message(obj);
        }

        /// <summary>
        /// Отправить предупреждение
        /// </summary>
        /// <param name="message">Предупреждение</param>
        public static async void Warning(string message)
        {
            ILogger? logger = await GetImplementation();
            logger?.Warning(message);
        }
        /// <summary>
        /// Отправить объект как предупреждение
        /// </summary>
        /// <param name="obj">Объект для предупреждения</param>
        public static async void Warning(object obj)
        {
            ILogger? logger = await GetImplementation();
            logger?.Warning(obj);
        }

        /// <summary>
        /// Отправить сообщение об ошибке
        /// </summary>
        /// <param name="message">Текст ошибки</param>
        public static async void Error(string message)
        {
            ILogger? logger = await GetImplementation();
            logger?.Error(message);
        }
        /// <summary>
        /// Отправить объект как сообщение об ошибке
        /// </summary>
        /// <param name="obj">Объект для сообщения об ошибке</param>
        public static async void Error(object obj)
        {
            ILogger? logger = await GetImplementation();
            logger?.Error(obj);
        }

        #endregion

        #region Поиск реализации

        private static async Task<ILogger?> GetImplementation()
        {
            if (!Enabled)
            {
                return null;
            }
            if (_logger != null)
            {
                return _logger;
            }

            Type? implementation = await LoggerImplementations.GetImplementationType();

            if (implementation == null)
            {
                return StandardLogger.Instance;
            }
            if (!implementation.IsImplementsInterface<ILogger>())
            {
                throw new LoggerImplementationException(implementation);
            }

            _logger = (ILogger)Activator.CreateInstance(implementation);

            return _logger;
        }

        #endregion
    }
}
