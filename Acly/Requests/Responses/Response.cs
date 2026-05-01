using Acly.Performing;
using Newtonsoft.Json;
using System;

namespace Acly.Requests
{
    /// <summary>
    /// Класс, реализующий ответ
    /// </summary>
    [Serializable]
    public class Response
    {
        /// <summary>
        /// Создать экземпляр ответа
        /// </summary>
        public Response()
        {
        }
        /// <summary>
        /// Создать экземпляр ответа
        /// </summary>
        /// <param name="code">Код ответа</param>
        /// <param name="text">Сообщение ответа</param>
        public Response(string code, string text)
        {
            Code = code;
            Text = text;
            Exception = new ResponseException(this);
        }
        /// <summary>
        /// Создать экземпляр ответа
        /// </summary>
        /// <param name="exception">Исключение как ответ</param>
        /// <exception cref="ArgumentNullException">Исключение не указано</exception>
        public Response(Exception exception) : this(exception, "{0}")
        {
        }
        /// <summary>
        /// Создать экземпляр ответа
        /// </summary>
        /// <param name="exception">Исключение как ответ</param>
		/// <param name="format">Формат сообщения об ошибке. {0} - сообщение ошибки, {1} - стек вызовов</param>
        /// <exception cref="ArgumentNullException">Исключение не указано</exception>
        public Response(Exception exception, string format)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception), "Исключение не указано");
            }
            if (format == null)
            {
                throw new ArgumentNullException(nameof(format), "Формат не указан");
            }

            if (exception is LoadingException loadingException)
            {
                Code = loadingException.Code;
            }
            else
            {
                Code = exception.GetType().Name;
            }

            Text = string.Format(format, exception.Message, exception.StackTrace);
            Exception = exception;
        }
        /// <summary>
        /// Создать экземпляр ответа
        /// </summary>
        /// <param name="response">Ответ для копирования</param>
        /// <exception cref="ArgumentNullException">Ответ не указан</exception>
        public Response(Response response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response), "Ответ для копирования не указан");
            }

            Code = response.Code;
            Text = response.Text;
            Exception = new ResponseException(this);
        }

        /// <summary>
        /// Код ответа
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; } = _DefaultCode;
        /// <summary>
        /// Сообщение ответа
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; } = _DefaultMessage;
        /// <summary>
        /// Ответ как исключение
        /// </summary>
        [JsonIgnore]
        [field: NonSerialized]
#pragma warning disable CA2201
        public Exception Exception { get; private set; } = new(_DefaultMessage);
#pragma warning restore CA2201

        /// <summary>
        /// Конвертировать ответ в одну строку
        /// </summary>
        /// <returns>Ответ как одна строка</returns>
        public override string ToString()
        {
            return Code + " | " + Text;
        }

        private const string _DefaultCode = "undefined";
        private const string _DefaultMessage = "Произошла неизвестная ошибка";
    }
}
