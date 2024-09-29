using System;
using System.Threading.Tasks;

namespace Acly.Tasks
{
	/// <summary>
	/// Асинхронная задача
	/// </summary>
	public class AclyAsyncTask : IAsyncTask
	{
		/// <summary>
		/// Пустой конструктор для наследования класса
		/// </summary>
		protected AclyAsyncTask()
		{
		}
		/// <summary>
		/// Создать экземпляр асинхронной задачи
		/// </summary>
		/// <param name="Controller">Контроллер асинхронной задачи</param>
		/// <exception cref="ArgumentNullException">Контролер не установлен</exception>
		public AclyAsyncTask(IAsyncTaskController Controller)
		{
			if (Controller == null)
			{
				throw new ArgumentNullException(nameof(Controller), "Контроллер асинхронной задачи не указан");
			}

			_Controller = Controller;
			UpdateController(Controller);
		}
		/// <summary>
		/// Создать экземпляр асинхронной задачи c <see cref="AclyAsyncTaskController"/>
		/// </summary>
		/// <param name="TaskFunction">Асинхронная задача для выполнения</param>
		public AclyAsyncTask(Func<Task> TaskFunction)
		{
			if (TaskFunction == null)
			{
				throw new ArgumentNullException(nameof(TaskFunction), "Задача для выполнения не указана");
			}

			_Controller = new AclyAsyncTaskController(TaskFunction);
			UpdateController(_Controller);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public event Action? Completed
		{
			add
			{
				LocalCompleted += value;

				if (IsCompleted && Error == null)
				{
					value?.Invoke();
				}
			}
			remove => LocalCompleted -= value;
		}
		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public event AsyncTaskProgress? ProgressUpdated;
		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public event AsyncTaskFail? Failed
		{
			add
			{
				LocalFailed += value;

				if (Error != null)
				{
					value?.Invoke(Error);
				}
			}
			remove => LocalFailed -= value;
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public IAsyncTaskError? Error { get; private set; }
		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public bool IsCompleted { get; private set; }

		private event Action? LocalCompleted;
		private event AsyncTaskFail? LocalFailed;

		private IAsyncTaskController? _Controller;

		#region Установка

		private void UpdateController(IAsyncTaskController Controller)
		{
			Controller.Completed += OnTaskCompleted;
			Controller.ProgressUpdated += OnProgressUpdated;
			Controller.Failed += OnTaskFailed;

			Controller.Start();
		}

        #endregion

        #region Очистка

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Dispose()
        {
			Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Очистить ресурсы
        /// </summary>
        /// <param name="All">true - очистка всех ресурсов, false - только ресурсы базового класса</param>
        protected virtual void Dispose(bool All)
		{
            Error = null;
            _Controller = null;
            LocalFailed = null;
            LocalCompleted = null;
        }

		#endregion

		#region События

		/// <summary>
		/// Задача успешно завершена
		/// </summary>
		protected void OnTaskCompleted()
		{
			RemoveEvents();

			IsCompleted = true;
			LocalCompleted?.Invoke();
		}
		/// <summary>
		/// Обновление прогресса выполнения задачи
		/// </summary>
		/// <param name="Percent">Прогресс выполнения</param>
		protected void OnProgressUpdated(float Percent)
		{
			ProgressUpdated?.Invoke(Percent);
		}
		/// <summary>
		/// Во время выполнения задачи произошла ошибка
		/// </summary>
		/// <param name="Error">Ошибка</param>
		protected void OnTaskFailed(IAsyncTaskError Error)
		{
			RemoveEvents();

			this.Error = Error;
			IsCompleted = true;

			LocalFailed?.Invoke(Error);
		}

		private void RemoveEvents()
		{
			if (_Controller == null)
			{
				return;
			}

			_Controller.Completed -= OnTaskCompleted;
			_Controller.ProgressUpdated -= OnProgressUpdated;
			_Controller.Failed -= OnTaskFailed;
		}

		#endregion
	}
}
