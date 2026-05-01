using Newtonsoft.Json;
using System;

namespace Acly.Requests
{
    /// <summary>
    /// Класс, реализующий ответ Api сервера
    /// </summary>
    [Serializable]
    public class ApiResponse : Response
    {
        /// <summary>
        /// Создать экземпляр ответа сервера
        /// </summary>
        public ApiResponse()
        {
        }
        /// <summary>
        /// Создать экземпляр ответа сервера
        /// </summary>
        /// <param name="status">Статус ответа</param>
        /// <param name="code">Код ответа</param>
        /// <param name="text">Сообщение ответа</param>
        public ApiResponse(ApiResponseStatus status, string code, string text)
        {
            Status = status;
            Code = code;
            Text = text;
        }
        /// <summary>
        /// Создать экземпляр ответа сервера
        /// </summary>
        /// <param name="response">Ответ</param>
        /// <param name="status">Статус ответа</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ApiResponse(Response response, ApiResponseStatus status)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response), "Ответ не указан");
            }

            Status = status;
            Code = response.Code;
            Text = response.Text;
        }
        /// <summary>
        /// Создать экземпляр ответа сервера
        /// </summary>
        /// <param name="exception">Исключение как ответ сервера</param>
        /// <exception cref="ArgumentNullException">Исключение не указано</exception>
        public ApiResponse(Exception exception) : base(exception)
        {
        }

        /// <summary>
        /// Статус ответа
        /// </summary>
        [JsonProperty("status")]
        public ApiResponseStatus Status { get; set; } = ApiResponseStatus.Error;

        /// <summary>
        /// Является ли ответ успешным
        /// </summary>
        public bool IsSuccess => Status == ApiResponseStatus.Success;

        /// <summary>
        /// Конвертировать ответ в одну строку
        /// </summary>
        /// <returns>Ответ как одна строка</returns>
        public override string ToString()
        {
            return Status + " | " + Code + " | " + Text;
        }
    }
}