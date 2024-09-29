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
		/// <param name="Exception">Исключение, вызванное во время выполнения асинхронной задачи</param>
		/// <exception cref="ArgumentNullException">Исключение не указано</exception>
		public AclyAsyncTaskError(Exception Exception)
		{
			if (Exception == null)
			{
				throw new ArgumentNullException(nameof(Exception), "Исключение не указано");
			}

			this.Exception = Exception;
			Response = new(Exception);
		}
		/// <summary>
		/// Создать экземпляр ошибки выполнения асинхронной задачи
		/// </summary>
		/// <param name="Response">Ответ, вызванный во время выполнения асинхронной задачи</param>
		/// <exception cref="ArgumentNullException">Ответ не указан</exception>
		public AclyAsyncTaskError(Response Response)
		{
			if (Response == null)
			{
				throw new ArgumentNullException(nameof(Response), "Ответ не указан");
			}

			Exception = Response.Exception;
			this.Response = Response;
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
