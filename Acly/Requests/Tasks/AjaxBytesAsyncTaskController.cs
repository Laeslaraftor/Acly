using System;
using System.Threading.Tasks;

namespace Acly.Requests.Tasks
{
	/// <summary>
	/// Котроллер запроса на получение массива байтов
	/// </summary>
	public sealed class AjaxBytesAsyncTaskController : AjaxAsyncTaskControllerBase<byte[]>
	{
		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="Url"><inheritdoc/></param>
		public AjaxBytesAsyncTaskController(string Url) : base(Url)
		{
		}
		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="UrlTask"><inheritdoc/></param>
		public AjaxBytesAsyncTaskController(Task<string> UrlTask) : base(UrlTask)
		{
		}
		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="UrlFunc"><inheritdoc/></param>
		public AjaxBytesAsyncTaskController(Func<Task<string>> UrlFunc) : base(UrlFunc)
		{
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		protected override Action<string, Action<byte[]>, Action<float>, Action<ApiResponse>> GetRequestMethod()
		{
			return Ajax.GetBytes;
		}
	}
}
