using Acly.Performing.Tasks;
using Acly.Requests;
using Acly.Tasks;
using System;

namespace Acly.Performing
{
    /// <summary>
    /// Базовый класс загрузки. Для создания своей загрузки необходимо унаследовать этот класс и перезаписать свойство <see cref="TasksForPerform"/>
    /// </summary>
    public abstract class LoadingBase : ILoading
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event LoadingEvent? DescriptionChanged;
        /// <summary>
        /// Вызывается сразу после окончания выполнения задач
        /// </summary>
        public event LoadingEvent? Completed;
        /// <summary>
        /// Вызывается при обновлении прогресса выполнения
        /// </summary>
        public event LoadingProgressEvent? ProgressUpdated;
        /// <summary>
        /// Вызывается если при выполнении задач произошла какая-то ошибка
        /// </summary>
        public event LoadingFailEvent? Failed;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public virtual string? Description
        {
            get => field;
            set
            {
                if (value != field)
                {
                    field = value;
                    DescriptionChanged?.Invoke(this);
                }
            }
        }

        /// <summary>
        /// Последовательность задач, которая будет выполняться во время загрузки.
        /// </summary>
        protected abstract LoadingTasksList TasksForPerform { get; }

        private LoadingAsyncTasksController? _controller;
        private bool _started;

        #region Управление

        /// <summary>
        /// Начать загрузку
        /// </summary>
        /// <returns>Задача загрузки</returns>
        public IAsyncTask Start()
        {
            if (_started)
            {
                throw new InvalidOperationException("Загрузка уже была начата");
            }

            _started = true;
            _controller = new(TasksForPerform);

            _controller.Completed += OnLoadingCompleted;
            _controller.ProgressUpdated += OnLoadingProgressUpdated;
            _controller.Failed += OnLoadingProgressFailed;
            _controller.DescriptionChanged += OnLoadingDescriptionChanged;

            return new AclyAsyncTask(_controller);
        }

        /// <summary>
        /// Получить какое-либо значение
        /// </summary>
        /// <typeparam name="T">Тип получаемого значения</typeparam>
        /// <param name="name">Название значения</param>
        /// <returns>Значение</returns>
        public virtual T? GetValue<T>(string name)
        {
            return default;
        }

        #endregion

        #region Загрузка

        /// <summary>
        /// Прервать загрузку с ошибкой
        /// </summary>
        /// <param name="response">Сообщение</param>
        protected void BreakWithError(Response response)
        {
            _controller?.BreakWithError(response);
        }
        /// <summary>
        /// Прервать загрузку с ошибкой
        /// </summary>
        /// <param name="error">Возникшее исключение</param>
        protected void BreakWithError(Exception error)
        {
            BreakWithError(new Response(error));
        }
        /// <summary>
        /// Прервать загрузку с ошибкой
        /// </summary>
        /// <param name="code">Код ошибки</param>
        /// <param name="text">Сообщение ошибки</param>
        protected void BreakWithError(string code, string text)
        {
            BreakWithError(new Response(code, text));
        }
        /// <summary>
        /// Прервать загрузку с ошибкой
        /// </summary>
        /// <param name="error">Возникшее исключение</param>
		/// <param name="format">Формат сообщения об ошибке. {0} - сообщение ошибки, {1} - стек вызовов</param>
        protected void BreakWithError(Exception error, string format)
        {
            BreakWithError(new Response(error, format));
        }

        #endregion

        #region События

        private void OnLoadingCompleted()
        {
            RemoveEvents();
            Description = null;
            Completed?.Invoke(this);
        }
        private void OnLoadingProgressUpdated(float percent)
        {
            ProgressUpdated?.Invoke(this, percent);
        }
        private void OnLoadingProgressFailed(IAsyncTaskError response)
        {
            RemoveEvents();
            Failed?.Invoke(this, response);
        }
        private void OnLoadingDescriptionChanged()
        {
            Description = _controller?.Description;
        }

        private void RemoveEvents()
        {
            if (_controller == null)
            {
                return;
            }

            _controller.Completed -= OnLoadingCompleted;
            _controller.ProgressUpdated -= OnLoadingProgressUpdated;
            _controller.Failed -= OnLoadingProgressFailed;
            _controller.DescriptionChanged -= OnLoadingDescriptionChanged;
        }

        #endregion
    }
}
