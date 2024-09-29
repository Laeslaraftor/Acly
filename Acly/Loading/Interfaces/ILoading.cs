using Acly.Tasks;
using System;

namespace Acly.Performing
{
	/// <summary>
	/// Интерфейс реализации загрузки чего-либо
	/// </summary>
	public interface ILoading
	{
		/// <summary>
		/// Вызывается сразу после окончания выполнения задач
		/// </summary>
		public event Action<ILoading> Completed;
		/// <summary>
		/// Вызывается при обновлении прогресса выполнения
		/// </summary>
		public event Action<ILoading, float> ProgressUpdated;
		/// <summary>
		/// Вызывается если при выполнении задач произошла какая-то ошибка
		/// </summary>
		public event Action<ILoading, IAsyncTaskError> Failed;

		/// <summary>
		/// Начать загрузку
		/// </summary>
		/// <returns>Задача загрузки</returns>
		public IAsyncTask Start();

		/// <summary>
		/// Получить какое-либо значение
		/// </summary>
		/// <typeparam name="T">Тип получаемого значения</typeparam>
		/// <param name="Name">Название значения</param>
		/// <returns>Значение</returns>
		public T? GetValue<T>(string Name);
	}
}
