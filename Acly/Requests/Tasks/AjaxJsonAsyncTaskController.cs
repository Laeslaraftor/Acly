using System;
using System.Threading.Tasks;

namespace Acly.Requests.Tasks
{
	/// <summary>
	/// Котроллер запроса на получение JSON
	/// </summary>
	public class AjaxJsonAsyncTaskController<T> : AjaxAsyncTaskControllerBase<T>
	{
		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="Url"><inheritdoc/></param>
		public AjaxJsonAsyncTaskController(string Url) : base(Url)
		{
		}
		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="UrlTask"><inheritdoc/></param>
		public AjaxJsonAsyncTaskController(Task<string> UrlTask) : base(UrlTask)
		{
		}
		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="UrlFunc"><inheritdoc/></param>
		public AjaxJsonAsyncTaskController(Func<Task<string>> UrlFunc) : base(UrlFunc)
		{
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		protected override Action<string, Action<T>, Action<float>, Action<ApiResponse>> GetRequestMethod()
		{
			return Ajax.GetJson;
		}
	}
}
