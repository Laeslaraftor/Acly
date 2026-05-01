using System;
using System.Threading.Tasks;

namespace Acly.Tasks
{
    /// <summary>
    /// Асинхронная задача
    /// </summary>
    public class AclyAsyncTask : Disposable, IAsyncTask
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
        /// <param name="controller">Контроллер асинхронной задачи</param>
        /// <exception cref="ArgumentNullException">Контролер не установлен</exception>
        public AclyAsyncTask(IAsyncTaskController controller)
        {
            _controller = controller ??
                throw new ArgumentNullException(nameof(controller), "Контроллер асинхронной задачи не указан");
            UpdateController(controller);
        }
        /// <summary>
        /// Создать экземпляр асинхронной задачи c <see cref="AclyAsyncTaskController"/>
        /// </summary>
        /// <param name="taskFunction">Асинхронная задача для выполнения</param>
        public AclyAsyncTask(Func<Task> taskFunction)
        {
            if (taskFunction == null)
            {
                throw new ArgumentNullException(nameof(taskFunction), "Задача для выполнения не указана");
            }

            _controller = new AclyAsyncTaskController(taskFunction);
            UpdateController(_controller);
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

        private IAsyncTaskController? _controller;

        #region Установка

        private void UpdateController(IAsyncTaskController controller)
        {
            controller.Completed += OnTaskCompleted;
            controller.ProgressUpdated += OnProgressUpdated;
            controller.Failed += OnTaskFailed;

            controller.Start();
        }

        #endregion

        #region Очистка

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="isDisposing"><inheritdoc/></param>
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            Error = null;
            _controller = null;
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
        /// <param name="percent">Прогресс выполнения</param>
        protected void OnProgressUpdated(float percent)
        {
            ProgressUpdated?.Invoke(percent);
        }
        /// <summary>
        /// Во время выполнения задачи произошла ошибка
        /// </summary>
        /// <param name="error">Ошибка</param>
        protected void OnTaskFailed(IAsyncTaskError error)
        {
            RemoveEvents();

            Error = error;
            IsCompleted = true;

            LocalFailed?.Invoke(error);
        }

        private void RemoveEvents()
        {
            if (_controller == null)
            {
                return;
            }

            _controller.Completed -= OnTaskCompleted;
            _controller.ProgressUpdated -= OnProgressUpdated;
            _controller.Failed -= OnTaskFailed;
        }

        #endregion
    }
}
