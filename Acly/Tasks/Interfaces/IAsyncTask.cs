using System;

namespace Acly.Tasks
{
	/// <summary>
	/// Интерфейс реализации асинхронной задачи
	/// </summary>
	public interface IAsyncTask : IDisposable
	{
		/// <summary>
		/// Вызывается сразу после окончания выполнения задачи
		/// </summary>
		public event Action Completed;
		/// <summary>
		/// Вызывается при обновлении прогресса выполнения
		/// </summary>
		public event AsyncTaskProgress ProgressUpdated;
		/// <summary>
		/// Вызывается при возникновении какой-либо ошибки во время выполнения задачи
		/// </summary>
		public event AsyncTaskFail Failed;

		/// <summary>
		/// Информация о произошедшей ошибке. Если ошибок не было - NULL
		/// </summary>
		public IAsyncTaskError? Error { get; }
		/// <summary>
		/// Была ли завершена задача
		/// </summary>
		public bool IsCompleted { get; }
	}
}
