using System;
using System.Threading.Tasks;

namespace Acly.Requests.Tasks
{
	/// <summary>
	/// Котроллер запроса на получение строки
	/// </summary>
	public class AjaxStringAsyncTaskController : AjaxAsyncTaskControllerBase<string>
	{
		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="Url"><inheritdoc/></param>
		public AjaxStringAsyncTaskController(string Url) : base(Url)
		{
		}
		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="UrlTask"><inheritdoc/></param>
		public AjaxStringAsyncTaskController(Task<string> UrlTask) : base(UrlTask)
		{
		}
		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="UrlFunc"><inheritdoc/></param>
		public AjaxStringAsyncTaskController(Func<Task<string>> UrlFunc) : base(UrlFunc)
		{
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		protected override Action<string, Action<string>, Action<float>, Action<ApiResponse>> GetRequestMethod()
		{
			return Ajax.Get;
		}
	}
}
