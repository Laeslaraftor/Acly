using Acly.Requests;
using System;

namespace Acly.Tasks
{
	/// <summary>
	/// Интерфейс реализации ошибки выполнения <see cref="IAsyncTask{TException}"/>
	/// </summary>
	public interface IAsyncTaskError
	{
		/// <summary>
		/// Исключение, вызванное во время выполнения асинхронной задачи
		/// </summary>
		public Exception Exception { get; }
		/// <summary>
		/// Ответ конвертированный из <see cref="Exception"/>
		/// </summary>
		public Response Response { get; }
	}
}
