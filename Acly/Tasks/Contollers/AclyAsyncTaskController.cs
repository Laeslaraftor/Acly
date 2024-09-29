using Acly.Requests;
using System;
using System.Threading.Tasks;

namespace Acly.Tasks
{
	/// <summary>
	/// Стандартная реализация котроллера асинхронной задачи
	/// </summary>
	public class AclyAsyncTaskController : IAsyncTaskController
	{
		/// <summary>
		/// Конструктор для наследования котроллера асинхронной задачи
		/// </summary>
		protected AclyAsyncTaskController()
		{
		}
		/// <summary>
		/// Создать экземпляр контроллера асинхронной задачи
		/// </summary>
		/// <param name="TaskToPerform">Асинхронная задача для выполнения</param>
		/// <exception cref="ArgumentNullException">Задача не указана</exception>
		public AclyAsyncTaskController(Func<Task> TaskToPerform)
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
		public event Action? Completed;
		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public event AsyncTaskProgress? ProgressUpdated;
		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public event AsyncTaskFail? Failed;

		private readonly Func<Task>? _Task;

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public virtual async void Start()
		{
			if (_Task == null)
			{
				return;
			}

			try
			{
				await _Task.Invoke();
			}
			catch (Exception Error)
			{
				IAsyncTaskError Fail = new AclyAsyncTaskError(Error);
				InvokeFailedEvent(Fail);
				return;
			}

			InvokeProgressUpdatedEvent(1);
			InvokeCompletedEvent();
		}

		#region События

		/// <summary>
		/// Вызвать событие успешного выполнения асинхронной задачи
		/// </summary>
		protected void InvokeCompletedEvent()
		{
#pragma warning disable CS8604
			if (!Completed.TryInvoke(out Exception? UserError))
			{
				Log.Error(UserError);
			}
#pragma warning restore CS8604
		}
		/// <summary>
		/// Вызвать событие обновления прогресса выполнения асинхронной задачи
		/// </summary>
		/// <param name="Percent">Прогресс</param>
		protected void InvokeProgressUpdatedEvent(float Percent)
		{
#pragma warning disable CS8604
			if (!ProgressUpdated.TryInvoke(Percent, out Exception? UserError))
			{
				Log.Error(UserError);
			}
#pragma warning restore CS8604
		}
		/// <summary>
		/// Вызвать событие ошибки выполнения асинхронной задачи
		/// </summary>
		/// <param name="Error">Ошибка</param>
		protected void InvokeFailedEvent(IAsyncTaskError Error)
		{
#pragma warning disable CS8604
			if (!Failed.TryInvoke(Error, out Exception? UserError))
			{
				Log.Error(UserError);
			}
#pragma warning restore CS8604
		}
		/// <summary>
		/// Вызвать событие ошибки выполнения асинхронной задачи
		/// </summary>
		/// <param name="Error">Ошибка</param>
		protected void InvokeFailedEvent(Exception Error)
		{
			InvokeFailedEvent(new AclyAsyncTaskError(Error));
		}
		/// <summary>
		/// Вызвать событие ошибки выполнения асинхронной задачи
		/// </summary>
		/// <param name="Error">Ошибка</param>
		protected void InvokeFailedEvent(ApiResponse Error)
		{
			InvokeFailedEvent(new AclyAsyncTaskError(Error));
		}

		#endregion
	}
}
