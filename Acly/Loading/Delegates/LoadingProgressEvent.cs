namespace Acly.Performing
{
    /// <summary>
    /// Событие загрузки
    /// </summary>
    /// <param name="loading">Загрузка, вызвавшая событие</param>
    /// <param name="progress">Прогресс загрузки от 0 до 1</param>
    public delegate void LoadingProgressEvent(ILoading loading, float progress);
}
