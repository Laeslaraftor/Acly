using Acly.Tasks;
using System;
using System.Threading.Tasks;

namespace Acly.Requests.Tasks
{
	/// <summary>
	/// Котроллер асинхронной задачи скачивания файла
	/// </summary>
	public sealed class DownloadAsyncTaskController : AjaxAsyncTaskControllerBase<string>
	{
		/// <summary>
		/// Создать котроллер для асинхронного скачивания файла
		/// </summary>
		/// <param name="Url">Адрес файла</param>
		/// <param name="Dir">Путь сохранения файла</param>
		public DownloadAsyncTaskController(string Url, string Dir) : base(Url)
		{
			if (Url == null)
			{
				throw new ArgumentNullException(nameof(Url), "Адрес файла не указан");
			}
			if (Dir == null)
			{
				throw new ArgumentNullException(nameof(Dir), "Путь сохранения файла не указан");
			}

			_Dir = Dir;
		}
		/// <summary>
		/// Создать котроллер для асинхронного скачивания файла
		/// </summary>
		/// <param name="Url">Адрес файла</param>
		/// <param name="Dir">Путь сохранения файла</param>
		public DownloadAsyncTaskController(Task<string> Url, string Dir) : base(Url)
		{
			if (Url == null)
			{
				throw new ArgumentNullException(nameof(Url), "Адрес файла не указан");
			}
			if (Dir == null)
			{
				throw new ArgumentNullException(nameof(Dir), "Путь сохранения файла не указан");
			}

			_Dir = Dir;
		}
		/// <summary>
		/// Создать котроллер для асинхронного скачивания файла
		/// </summary>
		/// <param name="Url">Адрес файла</param>
		/// <param name="Dir">Путь сохранения файла</param>
		public DownloadAsyncTaskController(Func<Task<string>> Url, string Dir) : base(Url)
		{
			if (Url == null)
			{
				throw new ArgumentNullException(nameof(Url), "Адрес файла не указан");
			}
			if (Dir == null)
			{
				throw new ArgumentNullException(nameof(Dir), "Путь сохранения файла не указан");
			}

			_Dir = Dir;
		}

		private readonly string _Dir;

		#region Управление

		private void Start(string Url, Action<string> Success, Action<float> Progress, Action<ApiResponse> Failed)
		{
			Ajax.Download(Url, _Dir, () =>
			{
				Success?.Invoke(_Dir);
			}, 
			Error =>
			{
				Failed?.Invoke(new(Error));
				Log.Error(Error);
			}, Progress);

		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		protected override Action<string, Action<string>, Action<float>, Action<ApiResponse>> GetRequestMethod()
		{
			return Start;
		}

		#endregion
	}
}
