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
        /// <param name="Message"><inheritdoc/></param>
        public UndefinedMethodException(string Message) : base(Message)
        {
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="Message"><inheritdoc/></param>
        /// <param name="InnerException"><inheritdoc/></param>
        public UndefinedMethodException(string Message, Exception InnerException) : base(Message, InnerException)
        {
        }
        /// <summary>
        /// Создать экземпляр исключения о неизвестном методе
        /// </summary>
        /// <param name="MethodName">Название метода</param>
        /// <param name="SearchType">Тип в котором происходил поиск</param>
#pragma warning disable CA1062
        public UndefinedMethodException(string MethodName, Type SearchType) : base(string.Format(_Message, MethodName, SearchType.FullName))
#pragma warning restore CA1062
        {
        }

        private const string _Message = "Неизвестный метод {0} в типе {1}";
    }
}
