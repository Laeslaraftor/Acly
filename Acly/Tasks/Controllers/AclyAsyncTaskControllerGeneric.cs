using System;
using System.Threading.Tasks;

namespace Acly.Tasks
{
    /// <summary>
    /// Стандартная реализация котроллера асинхронной задачи
    /// </summary>
    /// <typeparam name="TOutput">Тип выводимых данных</typeparam>
    /// <remarks>
    /// Создать экземпляр контроллера асинхронной задачи
    /// </remarks>
    /// <param name="taskToPerform">Асинхронная задача для выполнения</param>
    /// <exception cref="ArgumentNullException">Задача не указана</exception>
    public sealed class AclyAsyncTaskController<TOutput>(Func<Task<TOutput>> taskToPerform)
        : AclyAsyncTaskController, IAsyncTaskController<TOutput>
    {

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public new event AsyncTaskComplete<TOutput>? Completed;

        private readonly Func<Task<TOutput>> _task = taskToPerform
                ?? throw new ArgumentNullException(nameof(taskToPerform), "Задача для выполнения не указана");

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override async void Start()
        {
            TOutput? result = default;

            try
            {
                result = await _task.Invoke();
            }
            catch (Exception error)
            {
                IAsyncTaskError fail = new AclyAsyncTaskError(error);
                InvokeFailedEvent(fail);
                return;
            }

            InvokeProgressUpdatedEvent(1);
            InvokeCompletedEvent();
            Completed?.Invoke(result);
        }
    }
}
