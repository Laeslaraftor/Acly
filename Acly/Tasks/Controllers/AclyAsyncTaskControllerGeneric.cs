using System;
using System.Threading.Tasks;

namespace Acly.Tasks
{
	/// <summary>
	/// Стандартная реализация котроллера асинхронной задачи
	/// </summary>
	/// <typeparam name="TOutput">Тип выводимых данных</typeparam>
	public sealed class AclyAsyncTaskController<TOutput> : AclyAsyncTaskController, IAsyncTaskController<TOutput>
	{
		/// <summary>
		/// Создать экземпляр контроллера асинхронной задачи
		/// </summary>
		/// <param name="TaskToPerform">Асинхронная задача для выполнения</param>
		/// <exception cref="ArgumentNullException">Задача не указана</exception>
		public AclyAsyncTaskController(Func<Task<TOutput>> TaskToPerform)
		{
			if (TaskToPerform == null)
			{
				throw new ArgumentNullException(nameof(TaskToPerform), "Задача для выполнения не указана");
			}

			_Task = TaskToPerform;
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public new event AsyncTaskComplete<TOutput>? Completed;

		private readonly Func<Task<TOutput>> _Task;

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public override async void Start()
		{
#pragma warning disable CS8600
			TOutput Result = default;
#pragma warning restore CS8600

			try
			{
				Result = await _Task.Invoke();
			}
			catch (Exception Error)
			{
				IAsyncTaskError Fail = new AclyAsyncTaskError(Error);
				InvokeFailedEvent(Fail);
				return;
			}

			InvokeProgressUpdatedEvent(1);
			InvokeCompletedEvent();
			Completed?.Invoke(Result);
		}
	}
}
