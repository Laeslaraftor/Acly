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
		/// <param name="Response">Ответ</param>
#pragma warning disable CA1062
		public ResponseException(Response Response) : base(Response.Text)
#pragma warning restore CA1062
		{
			if (Response == null)
			{
				throw new ArgumentNullException(nameof(Response), "Ответ не указан");
			}

			this.Response = Response;
		}

		/// <summary>
		/// Ответ
		/// </summary>
		public Response Response { get; private set; }
	}
}
