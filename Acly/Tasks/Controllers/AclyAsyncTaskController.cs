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
        /// <param name="taskToPerform">Асинхронная задача для выполнения</param>
        /// <exception cref="ArgumentNullException">Задача не указана</exception>
        public AclyAsyncTaskController(Func<Task> taskToPerform)
        {
            _task = taskToPerform
                ?? throw new ArgumentNullException(nameof(taskToPerform), "Задача для выполнения не указана");
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

        private readonly Func<Task>? _task;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public virtual async void Start()
        {
            if (_task == null)
            {
                return;
            }

            try
            {
                await _task.Invoke();
            }
            catch (Exception error)
            {
                IAsyncTaskError fail = new AclyAsyncTaskError(error);
                InvokeFailedEvent(fail);
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
            if (!Completed.TryInvoke(out Exception? userError))
            {
                Log.Error(userError);
            }
        }
        /// <summary>
        /// Вызвать событие обновления прогресса выполнения асинхронной задачи
        /// </summary>
        /// <param name="percent">Прогресс</param>
        protected void InvokeProgressUpdatedEvent(float percent)
        {
            if (!ProgressUpdated.TryInvoke(percent, out Exception? userError))
            {
                Log.Error(userError);
            }
        }
        /// <summary>
        /// Вызвать событие ошибки выполнения асинхронной задачи
        /// </summary>
        /// <param name="error">Ошибка</param>
        protected void InvokeFailedEvent(IAsyncTaskError error)
        {
            if (!Failed.TryInvoke(error, out Exception? userError))
            {
                Log.Error(userError);
            }
        }
        /// <summary>
        /// Вызвать событие ошибки выполнения асинхронной задачи
        /// </summary>
        /// <param name="error">Ошибка</param>
        protected void InvokeFailedEvent(Exception error)
        {
            InvokeFailedEvent(new AclyAsyncTaskError(error));
        }
        /// <summary>
        /// Вызвать событие ошибки выполнения асинхронной задачи
        /// </summary>
        /// <param name="error">Ошибка</param>
        protected void InvokeFailedEvent(ApiResponse error)
        {
            InvokeFailedEvent(new AclyAsyncTaskError(error));
        }

        #endregion
    }
}
