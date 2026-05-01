using System;

namespace Acly
{
    /// <summary>
    /// Неизвестный метод
    /// </summary>
    public class UndefinedMethodException : Exception
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="message"><inheritdoc/></param>
        public UndefinedMethodException(string message) : base(message)
        {
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="message"><inheritdoc/></param>
        /// <param name="innerException"><inheritdoc/></param>
        public UndefinedMethodException(string message, Exception innerException) : base(message, innerException)
        {
        }
        /// <summary>
        /// Создать экземпляр исключения о неизвестном методе
        /// </summary>
        /// <param name="methodName">Название метода</param>
        /// <param name="searchType">Тип в котором происходил поиск</param>
#pragma warning disable CA1062
        public UndefinedMethodException(string methodName, Type searchType) : base(string.Format(_Message, methodName, searchType.FullName))
#pragma warning restore CA1062
        {
        }

        private const string _Message = "Неизвестный метод {0} в типе {1}";
    }
}
