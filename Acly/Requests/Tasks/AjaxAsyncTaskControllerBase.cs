using Acly.Tasks;
using System;
using System.Threading.Tasks;

namespace Acly.Requests.Tasks
{
	/// <summary>
	/// Базовый класс котроллера запросов
	/// </summary>
	/// <typeparam name="T">Возвращаемый тип</typeparam>
#pragma warning disable CA1012
	public abstract class AjaxAsyncTaskControllerBase<T> : AclyAsyncTaskController, IAsyncTaskController<T>
#pragma warning restore CA1012
	{
		/// <summary>
		/// Создать котроллер запросов
		/// </summary>
		/// <param name="Url">Адрес запроса</param>
		public AjaxAsyncTaskControllerBase(string Url)
		{
			if (Url == null)
			{
				throw new ArgumentNullException(nameof(Url), "Адрес указан");
			}

			_Url = Url;
		}
		/// <summary>
		/// Создать котроллер запросов
		/// </summary>
		/// <param name="UrlTask">Задача для получения запроса</param>
		public AjaxAsyncTaskControllerBase(Task<string> UrlTask)
		{
			if (UrlTask == null)
			{
				throw new ArgumentNullException(nameof(UrlTask), "Адрес указан");
			}

			_UrlTask = UrlTask;
		}
		/// <summary>
		/// Создать котроллер запросов
		/// </summary>
		/// <param name="UrlFunc">Асинхронная функция для получения адреса</param>
		public AjaxAsyncTaskControllerBase(Func<Task<string>> UrlFunc)
		{
			if (UrlFunc == null)
			{
				throw new ArgumentNullException(nameof(UrlFunc), "Асинхронная функция для получения адреса не указана");
			}

			_UrlTask = UrlFunc.Invoke();
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public new event AsyncTaskComplete<T>? Completed;
		
		private readonly string? _Url;
		private readonly Task<string>? _UrlTask;

		#region Управление

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public override async void Start()
		{
			var Method = GetRequestMethod();
			string Address = await GetUrl();

			Method(Address, OnDownloadCompleted, OnProgressUpdatedEvent, OnDownloadFailed);
		}

		/// <summary>
		/// Получить метод запроса
		/// </summary>
		/// <returns>Метод запроса</returns>
		protected abstract Action<string, Action<T>, Action<float>, Action<ApiResponse>> GetRequestMethod();

		#endregion

		#region Адрес

		/// <summary>
		/// Получить адрес запроса
		/// </summary>
		/// <returns>Адрес запроса</returns>
		protected async Task<string> GetUrl()
		{
			if (_UrlTask != null)
			{
				return await _UrlTask;
			}

#pragma warning disable CS8603
			return _Url;
#pragma warning restore CS8603
		}

		#endregion

		#region События

		private void OnProgressUpdatedEvent(float Progress)
		{
			InvokeProgressUpdatedEvent(Progress);
		}
		private void OnDownloadCompleted(T Data)
		{
			InvokeCompletedEvent();
			Completed?.Invoke(Data);
		}
		private void OnDownloadFailed(ApiResponse Error)
		{
			InvokeFailedEvent(new AclyAsyncTaskError(Error));
		}

		#endregion
	}
}
