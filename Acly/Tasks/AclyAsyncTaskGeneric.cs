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
        /// <param name="controller">Контроллер асинхронной задачи</param>
        /// <exception cref="ArgumentNullException">Контролер не установлен</exception>
#pragma warning disable CS8618
        public AclyAsyncTask(IAsyncTaskController<TOutput> controller)
#pragma warning restore CS8618
        {
            _controller = controller
                ?? throw new ArgumentNullException(nameof(controller), "Контроллер асинхронной задачи не указан");
            UpdateController(controller);
        }
        /// <summary>
        /// Создать экземпляр асинхронной задачи c <see cref="AclyAsyncTaskController{TOutput}"/>
        /// </summary>
        /// <param name="taskFunction">Асинхронная задача для выполнения</param>
#pragma warning disable CS8618
        public AclyAsyncTask(Func<Task<TOutput>> taskFunction)
#pragma warning restore CS8618
        {
            if (taskFunction == null)
            {
                throw new ArgumentNullException(nameof(taskFunction), "Задача для выполнения не указана");
            }

            _controller = new AclyAsyncTaskController<TOutput>(taskFunction);
            UpdateController(_controller);
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

                return _result;
            }
            private set => _result = value;
        }

        private event AsyncTaskComplete<TOutput>? LocalCompleted;

        private IAsyncTaskController<TOutput> _controller;
        private TOutput _result;

        #region Установка

        private void UpdateController(IAsyncTaskController<TOutput> сontroller)
        {
            сontroller.Completed += OnTaskCompleted;
            сontroller.ProgressUpdated += OnProgressUpdated;

            сontroller.Start();
        }

        #endregion

        #region Очистка

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="isDisposing"><inheritdoc/></param>
#pragma warning disable CS8625
#pragma warning disable CS8601
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            _result = default;
            _controller = null;
            LocalCompleted = null;
        }
#pragma warning restore CS8601
#pragma warning restore CS8625

        #endregion

        #region События

        private void OnTaskCompleted(TOutput value)
        {
            OnTaskCompleted();
            RemoveEvents();

            Result = value;

#pragma warning disable CS8604
            if (!LocalCompleted.TryInvoke(value, out Exception? error))
            {
                Log.Error(error);
            }
#pragma warning restore CS8604
        }

        private void RemoveEvents()
        {
            _controller.Completed -= OnTaskCompleted;
            _controller.ProgressUpdated -= OnProgressUpdated;
            _controller.Failed -= OnTaskFailed;
        }

        #endregion
    }
}
