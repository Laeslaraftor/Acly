namespace Acly.Tasks
{
    /// <summary>
    /// Изменение прогресса выполнения асинхронной задачи
    /// </summary>
    /// <param name="Progress">Прогресс выполнения от 0 до 1</param>
    public delegate void AsyncTaskProgress(float Progress);
}
