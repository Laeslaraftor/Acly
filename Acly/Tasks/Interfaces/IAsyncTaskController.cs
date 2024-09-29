using System;

namespace Acly.Tasks
{
	/// <summary>
	/// Интерфейс котроллера асинхронной задачи
	/// </summary>
	public interface IAsyncTaskController
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
		/// Начать выполнение задачи
		/// </summary>
		public void Start();
	}
}
