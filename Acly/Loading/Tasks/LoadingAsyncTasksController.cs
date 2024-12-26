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
		/// Вызывается при изменении описания
		/// </summary>
        public event Action? DescriptionChanged;
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

		/// <summary>
		/// Описание текущего этапа загрузки
		/// </summary>
		public string? Description
		{
			get => _Description;
			set
			{
				if (_Description == value)
				{
					return;
				}

				_Description = value;
				DescriptionChanged?.Invoke();
            }
        }

		private readonly LoadingTasksList _Tasks;
		private Token? _ProcessingToken;
		private string? _Description;

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
			IAsyncTask? AsyncTask = null;
			string? Description = TaskFunction.Description;

			TrySetDescription(Description, 0);

            try
			{
				AsyncTask = await TaskFunction.GetAsyncTask();
			}
			catch (Exception Error)
			{
				return new AclyAsyncTaskError(Error);
			}

			if (AsyncTask == null)
			{
				return null;
			}

			bool Completed = false;
			IAsyncTaskError? Result = null;
			bool Failed = false;

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
                TrySetDescription(Description, Percent);
            };
			void OnFailed(IAsyncTaskError Info)
			{
				Result = Info;
				Failed = true;
                OnCompleted();
			};

			AsyncTask.Completed += OnCompleted;
			AsyncTask.ProgressUpdated += OnProgressUpdated;
			AsyncTask.Failed += OnFailed;

			while (!Completed)
			{
				await Task.Delay(100);
			}

			if (!Failed)
			{
				TrySetDescription(Description, 1);
			}

			return Result;
		}

		private bool TrySetDescription(string? Value, float Progress)
		{
			if (Value == null)
			{
				return false;
			}

			try
			{
				Description = string.Format(Value, Math.Round(Progress * 100) + "%");
			}
			catch
			{
				return false;
			}

			return true;
		}

		#endregion
	}
}
