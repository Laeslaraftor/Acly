using System;

namespace Acly.Tasks
{
    /// <summary>
    /// Базовый класс асинхронной задачи, объединяющей задачи
    /// </summary>
    public abstract class CombinedTasksBase : IAsyncTask
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
        /// <param name="Progress">Прогресс от 0 до 1</param>
        protected void InvokeProgressUpdatedEvent(float Progress) => ProgressUpdated?.Invoke(Progress);
        /// <summary>
        /// Вызвать событие провала задачи
        /// </summary>
        /// <param name="Error">Информация о провале</param>
        protected void InvokeFailedEvent(IAsyncTaskError Error) => Failed?.Invoke(Error);

        #endregion
    }
}
