using System;

namespace Acly.Player
{
    /// <summary>
    /// Исключение, вызывающееся при неправильном методе получения реализации <see cref="ISimplePlayer"/>
    /// </summary>
    public sealed class SimplePlayerIncorrectImplementationCreateMethod : Exception
    {
        /// <summary>
        /// Вызвать исключение о том что метод получения экземпляра реализации имеет какие-то параметры
        /// </summary>
        /// <param name="type">Тип объекта с реализацией</param>
        /// <param name="method">Метод получения экземпляра реализации</param>
        public SimplePlayerIncorrectImplementationCreateMethod(Type type, string method)
            : base(string.Format(_messageParameters, type, method))
        {
        }
        /// <summary>
        /// Вызвать исключение о том что метод получения экземпляра реализации ничего не возвращает
        /// </summary>
        /// <param name="type">Тип объекта с реализацией</param>
        /// <param name="method">Метод получения экземпляра реализации</param>
        /// <param name="mustReturn">Необходимый тип данных</param>
        public SimplePlayerIncorrectImplementationCreateMethod(Type type, string method, string mustReturn)
            : base(string.Format(_messageNoReturn, type, method, mustReturn))
        {
        }
        /// <summary>
        /// Вызвать исключение о том что метод получения экземпляра реализации возвращает не тот тип данных
        /// </summary>
        /// <param name="type">Тип объекта с реализацией</param>
        /// <param name="method">Метод получения экземпляра реализации</param>
        /// <param name="returning">Возвращаемый тип данных</param>
        /// <param name="mustReturn">Необходимый тип данных</param>
        public SimplePlayerIncorrectImplementationCreateMethod(Type type, string method, string returning, string mustReturn)
            : base(string.Format(_messageIncorrectReturn, type, method, returning, mustReturn))
        {
        }

        private const string _messageParameters = "Для типа {0} был отмечен метод {1}, как метод создания экземпляра, но он имеет параметры." +
            " Такой метод не может иметь каких-либо параметров";
        private const string _messageNoReturn = "Для типа {0} был отмечен метод {1}, как метод создания экземпляра, но он ничего не возвращает." +
            " Такой метод должен возвращать {2}";
        private const string _messageIncorrectReturn = "Для типа {0} был отмечен метод {1}, как метод создания экземпляра, но он возвращает {2}." +
            " Такой метод должен возвращать {3}";
    }
}
