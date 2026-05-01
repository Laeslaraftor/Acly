using Acly.Requests;
using System;

namespace Acly.Performing
{
    /// <summary>
    /// Исключение, вызывающееся если во время загрузки произошла какая-то ошибка
    /// </summary>
    public sealed class LoadingException : Exception
    {
        /// <summary>
        /// Создать экземпляр исключения во время загрузки
        /// </summary>
        /// <param name="code">Код ошибки</param>
        /// <param name="message">Сообщение</param>
        public LoadingException(string code, string message) : base(message)
        {
            Code = code;
        }
        /// <summary>
        /// Создать экземпляр исключения во время загрузки
        /// </summary>
        /// <param name="response">Ответ</param>
        /// <exception cref="ArgumentNullException"></exception>
#pragma warning disable CA1062
        public LoadingException(Response response) : base(response.Text, response.Exception)
#pragma warning restore CA1062
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response), "Ответ не указан");
            }

            Code = response.Code;
        }

        /// <summary>
        /// Код ошибки
        /// </summary>
        public string Code { get; private set; }
    }
}
