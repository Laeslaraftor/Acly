namespace Acly.Tasks
{
    /// <summary>
    /// Успешное завершение асинхронной задачи с результатом
    /// </summary>
    /// <typeparam name="T">Тип данных результата</typeparam>
    /// <param name="Result">Результат выполнения асинхронной задачи</param>
    public delegate void AsyncTaskComplete<T>(T Result);
}
