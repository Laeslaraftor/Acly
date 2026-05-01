using Acly.Requests;
using System;

namespace Acly.Tasks
{
    /// <summary>
    /// Стандартная реализация <see cref="IAsyncTaskError"/>
    /// </summary>
    public class AclyAsyncTaskError : IAsyncTaskError
    {
        /// <summary>
        /// Создать экземпляр ошибки выполнения асинхронной задачи
        /// </summary>
        /// <param name="exception">Исключение, вызванное во время выполнения асинхронной задачи</param>
        /// <exception cref="ArgumentNullException">Исключение не указано</exception>
        public AclyAsyncTaskError(Exception exception)
        {
            Exception = exception ?? throw new ArgumentNullException(nameof(exception), "Исключение не указано");
            Response = new(exception);
        }
        /// <summary>
        /// Создать экземпляр ошибки выполнения асинхронной задачи
        /// </summary>
        /// <param name="response">Ответ, вызванный во время выполнения асинхронной задачи</param>
        /// <exception cref="ArgumentNullException">Ответ не указан</exception>
        public AclyAsyncTaskError(Response response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response), "Ответ не указан");
            }

            Exception = response.Exception;
            Response = response;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public Exception Exception { get; private set; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public Response Response { get; private set; }

        /// <summary>
        /// Получить строку с информацией об ошибке
        /// </summary>
        /// <returns>Строка с информацией об ошибке</returns>
        public override string ToString()
        {
            return Response.ToString();
        }
    }
}
