using Acly.Tasks;

namespace Acly.Performing
{
    /// <summary>
    /// Интерфейс реализации загрузки чего-либо
    /// </summary>
    public interface ILoading
    {
        /// <summary>
        /// Вызывается при изменении описания
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
        /// Описание текущего этапа загрузки
        /// </summary>
        public string? Description { get; }

        #region Управление

        /// <summary>
        /// Начать загрузку
        /// </summary>
        /// <returns>Задача загрузки</returns>
        public IAsyncTask Start();

        /// <summary>
        /// Получить какое-либо значение
        /// </summary>
        /// <typeparam name="T">Тип получаемого значения</typeparam>
        /// <param name="name">Название значения</param>
        /// <returns>Значение</returns>
        public T? GetValue<T>(string name);

        #endregion
    }
}
