namespace Acly.Performing
{
    /// <summary>
    /// Событие загрузки
    /// </summary>
    /// <param name="Loading">Загрузка, вызвавшая событие</param>
    /// <param name="Progress">Прогресс загрузки от 0 до 1</param>
    public delegate void LoadingProgressEvent(ILoading Loading, float Progress);
}
