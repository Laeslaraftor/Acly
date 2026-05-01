using Acly.Tasks;
using System;

namespace Acly.Performing
{
    /// <summary>
    /// Класс для управления загрузками
    /// </summary>
    public static class Loading
    {
        /// <summary>
        /// Вызывается когда начинается какая-либо загрузка
        /// </summary>
        public static event Action<IAsyncTask>? Started;
        /// <summary>
        /// Вызывается когда завершается какая-либо загрузка
        /// </summary>
        public static event Action<IAsyncTask>? Completed;
        /// <summary>
        /// Вызывается когда изменяется прогресс выполнения какой-либо загрузки
        /// </summary>
        public static event Action<IAsyncTask, float>? ProgressUpdated;
        /// <summary>
        /// Вызывается когда у какой-либо загрузки происходит ошибка
        /// </summary>
        public static event Action<IAsyncTask, IAsyncTaskError>? Failed;

        /// <summary>
        /// Начать загрузку
        /// </summary>
        /// <typeparam name="T">Тип объекта загрузки</typeparam>
        /// <param name="arguments">Аргументы для создания загрузки</param>
        /// <returns>Задача загрузки</returns>
        public static IAsyncTask Start<T>(params object[] arguments) where T : ILoading
        {
            return Start<T>(out var _, arguments);
        }
        /// <summary>
        /// Начать загрузку
        /// </summary>
        /// <typeparam name="T">Тип объекта загрузки</typeparam>
		/// <param name="loading">Начатая загрузка</param>
		/// <param name="arguments">Аргументы для создания загрузки</param>
        /// <returns>Задача загрузки</returns>
        public static IAsyncTask Start<T>(out T loading, params object[] arguments) where T : ILoading
        {
            loading = (T)Activator.CreateInstance(typeof(T), arguments);

            IAsyncTask task = loading.Start();
            SetEvents(task);

            try
            {
                Started?.Invoke(task);
            }
            catch (Exception error)
            {
                Log.Error(error);
            }

            return task;
        }

        #region Управление

        private static void SetEvents(IAsyncTask loading)
        {
            void OnCompleted()
            {
                RemoveEvents();
                Completed?.Invoke(loading);
            }
            void OnProgressUpdated(float percent)
            {
                ProgressUpdated?.Invoke(loading, percent);
            }
            void OnFailed(IAsyncTaskError response)
            {
                RemoveEvents();
                Failed?.Invoke(loading, response);
            }
            void RemoveEvents()
            {
                loading.Completed -= OnCompleted;
                loading.ProgressUpdated -= OnProgressUpdated;
                loading.Failed -= OnFailed;
            }

            loading.Completed += OnCompleted;
            loading.ProgressUpdated += OnProgressUpdated;
            loading.Failed += OnFailed;
        }

        #endregion
    }
}
