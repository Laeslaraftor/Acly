using System;
using System.Threading.Tasks;

namespace Acly.Tasks
{
	/// <summary>
	/// Асинхронная задача
	/// </summary>
	/// <typeparam name="TOutput">Тип выводимых данных</typeparam>
	public sealed class AclyAsyncTask<TOutput> : AclyAsyncTask, IAsyncTask<TOutput>
	{
		/// <summary>
		/// Создать экземпляр асинхронной задачи
		/// </summary>
		/// <param name="Controller">Контроллер асинхронной задачи</param>
		/// <exception cref="ArgumentNullException">Контролер не установлен</exception>
#pragma warning disable CS8618
		public AclyAsyncTask(IAsyncTaskController<TOutput> Controller)
#pragma warning restore CS8618
		{
			if (Controller == null)
			{
				throw new ArgumentNullException(nameof(Controller), "Контроллер асинхронной задачи не указан");
			}

			_Controller = Controller;
			UpdateController(Controller);
		}
		/// <summary>
		/// Создать экземпляр асинхронной задачи c <see cref="AclyAsyncTaskController{TOutput}"/>
		/// </summary>
		/// <param name="TaskFunction">Асинхронная задача для выполнения</param>
#pragma warning disable CS8618
		public AclyAsyncTask(Func<Task<TOutput>> TaskFunction)
#pragma warning restore CS8618
		{
			if (TaskFunction == null)
			{
				throw new ArgumentNullException(nameof(TaskFunction), "Задача для выполнения не указана");
			}

			_Controller = new AclyAsyncTaskController<TOutput>(TaskFunction);
			UpdateController(_Controller);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public new event AsyncTaskComplete<TOutput>? Completed
		{
			add
			{
				LocalCompleted += value;

				if (IsCompleted && Error == null)
				{
					value?.Invoke(Result);
				}
			}
			remove => LocalCompleted -= value;
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public TOutput Result
		{
			get
			{
				if (!IsCompleted)
				{
					throw new InvalidOperationException("Невозможно получить результат до окончания выполнения асинхронной задачи");
				}

				return _Result;
			}
			private set => _Result = value;
		}

		private event AsyncTaskComplete<TOutput>? LocalCompleted;

		private IAsyncTaskController<TOutput> _Controller;
		private TOutput _Result;

		#region Установка

		private void UpdateController(IAsyncTaskController<TOutput> Controller)
		{
			Controller.Completed += OnTaskCompleted;
			Controller.ProgressUpdated += OnProgressUpdated;

			Controller.Start();
		}

        #endregion

        #region Очистка

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="All"><inheritdoc/></param>
#pragma warning disable CS8625
#pragma warning disable CS8601
        protected override void Dispose(bool All)
        {
            base.Dispose(All);

            _Result = default;
            _Controller = null;
            LocalCompleted = null;
        }
#pragma warning restore CS8601
#pragma warning restore CS8625

        #endregion

        #region События

        private void OnTaskCompleted(TOutput Value)
		{
			OnTaskCompleted();
			RemoveEvents();

			Result = Value;

#pragma warning disable CS8604
			if (!LocalCompleted.TryInvoke(Value, out Exception? Error))
			{
				Log.Error(Error);
			}
#pragma warning restore CS8604
		}

		private void RemoveEvents()
		{
			_Controller.Completed -= OnTaskCompleted;
			_Controller.ProgressUpdated -= OnProgressUpdated;
			_Controller.Failed -= OnTaskFailed;
		}

		#endregion
	}
}
