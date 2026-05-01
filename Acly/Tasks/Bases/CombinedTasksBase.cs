using System;

namespace Acly.Tasks
{
    /// <summary>
    /// Базовый класс асинхронной задачи, объединяющей задачи
    /// </summary>
    public abstract class CombinedTasksBase : Disposable, IAsyncTask
    {
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
        /// <inheritdoc/>
        /// </summary>
        public bool IsCompleted { get; protected set; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IAsyncTaskError? Error { get; protected set; }
        /// <summary>
        /// Порядковый номер текущей задачи
        /// </summary>
        public int TaskIndex { get; protected set; } = -1;

        #region Управление

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="isDisposing"><inheritdoc/></param>
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            Error = null;
        }

        #endregion

        #region События

        /// <summary>
        /// Вызвать событие успешного завершения задачи
        /// </summary>
        protected void InvokeCompletedEvent() => Completed?.Invoke();
        /// <summary>
        /// Вызвать событие изменения прогресса задачи
        /// </summary>
        /// <param name="progress">Прогресс от 0 до 1</param>
        protected void InvokeProgressUpdatedEvent(float progress) => ProgressUpdated?.Invoke(progress);
        /// <summary>
        /// Вызвать событие провала задачи
        /// </summary>
        /// <param name="error">Информация о провале</param>
        protected void InvokeFailedEvent(IAsyncTaskError error) => Failed?.Invoke(error);

        #endregion
    }
}
