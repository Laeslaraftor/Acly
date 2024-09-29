using Acly.Requests;
using Acly.Tasks;
using Acly.Tokens;
using System;
using System.Threading.Tasks;

namespace Acly.Performing.Tasks
{
	/// <summary>
	/// Контроллер асинхронных задач загрузки
	/// </summary>
	public class LoadingAsyncTasksController : IAsyncTaskController
	{
		/// <summary>
		/// Создать экземпляр котроллера асинхронных задач загрузки
		/// </summary>
		/// <param name="TasksForPerform">Задачи для выполнения</param>
		/// <exception cref="ArgumentNullException">Задачи не установлены</exception>
		public LoadingAsyncTasksController(LoadingTasksList TasksForPerform)
		{
			if (TasksForPerform == null)
			{
				throw new ArgumentNullException(nameof(TasksForPerform), "Задачи для выполнения не установлены");
			}

			TasksForPerform.MakeReadOnly();

			_Tasks = TasksForPerform;
		}

		/// <summary>
		/// Вызывается при обновлении прогресса выполнения
		/// </summary>
		public event Action? Completed;
		/// <summary>
		/// Вызывается при обновлении прогресса выполнения
		/// </summary>
		public event AsyncTaskProgress? ProgressUpdated;
		/// <summary>
		/// Вызывается если при выполнении задач произошла какая-то ошибка
		/// </summary>
		public event AsyncTaskFail? Failed;

		private readonly LoadingTasksList _Tasks;
		private Token? _ProcessingToken;

		#region Управление

		/// <summary>
		/// Начать выполнение
		/// </summary>
		public async void Start()
		{
			Token CurrentToken = new();
			_ProcessingToken = CurrentToken;

			for (int i = 0; i < _Tasks.Count; i++)
			{
				if (_ProcessingToken != CurrentToken)
				{
					break;
				}

				LoadingAsyncTask Task = _Tasks[i];
				IAsyncTaskError? Result = await TryPerformTask(Task, i);

				if (Result != null)
				{
					Failed?.Invoke(Result);
					return;
				}
			}

			Completed?.Invoke();
		}

		/// <summary>
		/// Прервать выполнение
		/// </summary>
		/// <param name="Response">Сообщение</param>
		public void BreakWithError(Response Response)
		{
			_ProcessingToken = null;
			Failed?.Invoke(new AclyAsyncTaskError(new LoadingException(Response)));
		}

		#endregion

		#region Выполнение задач

		private void UpdateProgress(int Current, float Percent)
		{
			float Completed = Progress.FromAmountsRange(_Tasks.Count, Current, Percent);
			ProgressUpdated?.Invoke(Completed);
		}
		private async Task<IAsyncTaskError?> TryPerformTask(LoadingAsyncTask TaskFunction, int TaskIndex)
		{
#pragma warning disable CS8600
			IAsyncTask AsyncTask = null;
#pragma warning restore CS8600

			try
			{
				AsyncTask = await TaskFunction.GetAsyncTask();
			}
			catch (Exception Error)
			{
				return new AclyAsyncTaskError(Error);
			}

			bool Completed = false;
			IAsyncTaskError? Result = null;

			void OnCompleted()
			{
				Completed = true;

				AsyncTask.Completed -= OnCompleted;
				AsyncTask.ProgressUpdated -= OnProgressUpdated;
				AsyncTask.Failed -= OnFailed;
			};
			void OnProgressUpdated(float Percent)
			{
				UpdateProgress(TaskIndex, Percent);
			};
			void OnFailed(IAsyncTaskError Info)
			{
				Result = Info;
				OnCompleted();
			};

			AsyncTask.Completed += OnCompleted;
			AsyncTask.ProgressUpdated += OnProgressUpdated;
			AsyncTask.Failed += OnFailed;

			while (!Completed)
			{
				await Task.Delay(100);
			}

			return Result;
		}

		#endregion
	}
}
