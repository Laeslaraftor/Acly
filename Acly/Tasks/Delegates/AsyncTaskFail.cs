namespace Acly.Tasks
{
    /// <summary>
    /// Возникновение ошибки во время выполнения асинхронной задачи
    /// </summary>
    /// <param name="error">Информация о возникшей ошибке</param>
    public delegate void AsyncTaskFail(IAsyncTaskError error);
}
