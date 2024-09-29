using Acly.Performing.Tasks;
using Acly.Requests;
using Acly.Tasks;
using System;

namespace Acly.Performing
{
	/// <summary>
	/// Базовый класс загрузки. Для создания своей загрузки необходимо унаследовать этот класс и перезаписать свойство <see cref="TasksForPerform"/>
	/// </summary>
	public abstract class LoadingBase : ILoading
	{
		/// <summary>
		/// Вызывается сразу после окончания выполнения задач
		/// </summary>
		public event Action<ILoading>? Completed;
		/// <summary>
		/// Вызывается при обновлении прогресса выполнения
		/// </summary>
		public event Action<ILoading, float>? ProgressUpdated;
		/// <summary>
		/// Вызывается если при выполнении задач произошла какая-то ошибка
		/// </summary>
		public event Action<ILoading, IAsyncTaskError>? Failed;

		/// <summary>
		/// Последовательность задач, которая будет выполняться во время загрузки.
		/// </summary>
		protected abstract LoadingTasksList TasksForPerform { get; }

		private LoadingAsyncTasksController? _Controller;
		private bool _Started;

		#region Управление

		/// <summary>
		/// Начать загрузку
		/// </summary>
		/// <returns>Задача загрузки</returns>
		public IAsyncTask Start()
		{
			if (_Started)
			{
				throw new InvalidOperationException("Загрузка уже была начата");
			}

			_Started = true;
			_Controller = new(TasksForPerform);

			_Controller.Completed += OnLoadingCompleted;
			_Controller.ProgressUpdated += OnLoadingProgressUpdated;
			_Controller.Failed += OnLoadingProgressFailed;

			return new AclyAsyncTask(_Controller);
		}

		/// <summary>
		/// Получить какое-либо значение
		/// </summary>
		/// <typeparam name="T">Тип получаемого значения</typeparam>
		/// <param name="Name">Название значения</param>
		/// <returns>Значение</returns>
		public virtual T? GetValue<T>(string Name)
		{
			return default;
		}

		#endregion

		#region Загрузка

		/// <summary>
		/// Прервать загрузку с ошибкой
		/// </summary>
		/// <param name="Response">Сообщение</param>
		protected void BreakWithError(Response Response)
		{
			_Controller?.BreakWithError(Response);
		}
		/// <summary>
		/// Прервать загрузку с ошибкой
		/// </summary>
		/// <param name="Error">Возникшее исключение</param>
		protected void BreakWithError(Exception Error)
		{
			BreakWithError(new Response(Error));
		}
		/// <summary>
		/// Прервать загрузку с ошибкой
		/// </summary>
		/// <param name="Code">Код ошибки</param>
		/// <param name="Text">Сообщение ошибки</param>
		protected void BreakWithError(string Code, string Text)
		{
			BreakWithError(new Response(Code, Text));
		}

		#endregion

		#region События

		private void OnLoadingCompleted()
		{
			RemoveEvents();
			Completed?.Invoke(this);
		}
		private void OnLoadingProgressUpdated(float Percent)
		{
			ProgressUpdated?.Invoke(this, Percent);
		}
		private void OnLoadingProgressFailed(IAsyncTaskError Response)
		{
			RemoveEvents();
			Failed?.Invoke(this, Response);
		}

		private void RemoveEvents()
		{
			if (_Controller == null)
			{
				return;
			}

			_Controller.Completed -= OnLoadingCompleted;
			_Controller.ProgressUpdated -= OnLoadingProgressUpdated;
			_Controller.Failed -= OnLoadingProgressFailed;
		}

		#endregion
	}
}
