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
		/// <returns>Задача загрузки</returns>
		public static IAsyncTask Start<T>(params object[] Arguments) where T : ILoading
		{
			T Load = (T)Activator.CreateInstance(typeof(T), Arguments);

			IAsyncTask Task = Load.Start();
			SetEvents(Task);

			try
			{
				Started?.Invoke(Task);
			}
			catch (Exception Error)
			{
				Log.Error(Error);
			}

			return Task;
		}

		#region Управление

		private static void SetEvents(IAsyncTask Loading)
		{
			void OnCompleted()
			{
				RemoveEvents();
				Completed?.Invoke(Loading);
			}
			void OnProgressUpdated(float Percent)
			{
				ProgressUpdated?.Invoke(Loading, Percent);
			}
			void OnFailed(IAsyncTaskError Response)
			{
				RemoveEvents();
				Failed?.Invoke(Loading, Response);
			}
			void RemoveEvents()
			{
				Loading.Completed -= OnCompleted;
				Loading.ProgressUpdated -= OnProgressUpdated;
				Loading.Failed -= OnFailed;
			}

			Loading.Completed += OnCompleted;
			Loading.ProgressUpdated += OnProgressUpdated;
			Loading.Failed += OnFailed;
		}

		#endregion
	}
}
