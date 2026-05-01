using System;

namespace Acly.Requests
{
    /// <summary>
    /// Ответ как исключение
    /// </summary>
    [Serializable]
    public sealed class ResponseException : Exception
    {
        /// <summary>
        /// Создать исключение как ответ
        /// </summary>
        /// <param name="response">Ответ</param>
#pragma warning disable CA1062
        public ResponseException(Response response) : base(response.Text)
#pragma warning restore CA1062
        {
            Response = response ?? throw new ArgumentNullException(nameof(response), "Ответ не указан");
        }

        /// <summary>
        /// Ответ
        /// </summary>
        public Response Response { get; private set; }
    }
}
