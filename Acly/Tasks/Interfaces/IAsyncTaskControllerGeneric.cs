namespace Acly.Tasks
{
	/// <summary>
	/// Интерфейс котроллера асинхронной задачи
	/// </summary>
	/// <typeparam name="TOutput">Тип выводимых данных</typeparam>
	public interface IAsyncTaskController<TOutput> : IAsyncTaskController
	{
		/// <summary>
		/// Вызывается сразу после окончания выполнения задачи
		/// </summary>
		public new event AsyncTaskComplete<TOutput> Completed;
	}
}
