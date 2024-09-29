namespace Acly.Tasks
{
	/// <summary>
	/// Интерфейс реализации асинхронной задачи
	/// </summary>
	/// <typeparam name="TOutput">Тип выводимых данных</typeparam>
	public interface IAsyncTask<TOutput> : IAsyncTask
	{
		/// <summary>
		/// Вызывается сразу после окончания выполнения задачи
		/// </summary>
		public new event AsyncTaskComplete<TOutput> Completed;

		/// <summary>
		/// Результат выполнения асинхронной задачи
		/// </summary>
		public TOutput? Result { get; }
	}
}
