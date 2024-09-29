using Acly.JsonData;
using Acly.Requests.Tasks;
using Acly.Tasks;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Acly.Requests
{
	/// <summary>
	/// ����� ��� ������� ��������
	/// </summary>
	public static partial class Ajax
	{
		/// <summary>
		/// ����� �������� �������
		/// </summary>
		public static Timeout Timeout { get; set; } = new(60000);

		#region �������

		/// <summary>
		/// �������� ������ �� ���������� ������
		/// </summary>
		/// <param name="Url">����� ��� �������</param>
		/// <param name="Success">����������� ����� ��������� ��������� ����������</param>
		/// <param name="Fail">����������� ��� ��������� �����-���� ������</param>
		public static async void Get(string Url, Action<string> Success, Action<ApiResponse>? Fail = null)
		{
			try
			{
				string Result = await Get(Url);

				if (!Success.TryInvoke(Result, out Exception? UserError))
				{
#pragma warning disable CS8604
					Log.Error(UserError);
#pragma warning restore CS8604
				}
			}
			catch (Exception Error)
			{
				ExceptionToApiResponse(Error, Fail);
			}
		}
		/// <summary>
		/// �������� ������ �� ���������� ������
		/// </summary>
		/// <param name="Url">����� ��� �������</param>
		/// <param name="Success">����������� ����� ��������� ��������� ����������</param>
		/// <param name="Progress">����������� ���������� ��������� ���������� �������</param>
		/// <param name="Fail">����������� ��� ��������� �����-���� ������</param>
		public static async void Get(string Url, Action<string> Success, Action<float> Progress, Action<ApiResponse>? Fail = null)
		{
			using WebClient Client = GetWebClient();
			Client.DownloadProgressChanged += (s, e) =>
			{
				float Percent = (float)e.ProgressPercentage / 100;

				if (!Progress.TryInvoke(Percent, out Exception? Error))
				{
#pragma warning disable CS8604
					Log.Error(Error);
#pragma warning restore CS8604
				}
			};

			try
			{
				string Result = await Client.DownloadStringTaskAsync(Url);

				if (!Success.TryInvoke(Result, out Exception? UserError))
				{
#pragma warning disable CS8604
					Log.Error(UserError);
#pragma warning restore CS8604
				}
			}
			catch (Exception Error)
			{
				ExceptionToApiResponse(Error, Fail);
			}
		}
		/// <summary>
		/// �������� ������ �� ���������� ������
		/// </summary>
		/// <param name="Url">����� ��� �������</param>
		/// <returns>������ ������</returns>
		/// <exception cref="RequestException">�� ����� ������� ��������� �����-�� ������</exception>
		public static async Task<string> Get(string Url)
		{
			using HttpClient Web = new();
			Web.Timeout = TimeSpan.FromMinutes(1);

			Log.Message("������: " + Url);

			HttpResponseMessage Result = await Web.GetAsync(Url);
			string ResultText = await Result.Content.ReadAsStringAsync();

			if (!Result.IsSuccessStatusCode)
			{
				throw new RequestException(Url, Result.StatusCode);
			}

			return ResultText;
		}
		/// <summary>
		/// �������� ������ �� ���������� ������
		/// </summary>
		/// <param name="Url">����� ��� �������</param>
		/// <returns>������ �� ��������� ������</returns>
		/// <exception cref="RequestException">�� ����� ������� ��������� �����-�� ������</exception>
		public static IAsyncTask<string> GetAsync(string Url)
		{
			AjaxStringAsyncTaskController Controller = new(Url);
			return new AclyAsyncTask<string>(Controller);
		}
		/// <summary>
		/// �������� ������ �� ���������� ������
		/// </summary>
		/// <param name="Url">����� ��� �������</param>
		/// <returns>������ �� ��������� ������</returns>
		/// <exception cref="RequestException">�� ����� ������� ��������� �����-�� ������</exception>
		public static IAsyncTask<string> GetAsync(Task<string> Url)
		{
			AjaxStringAsyncTaskController Controller = new(Url);
			return new AclyAsyncTask<string>(Controller);
		}
		/// <summary>
		/// �������� ������ �� ���������� ������
		/// </summary>
		/// <param name="Url">����� ��� �������</param>
		/// <returns>������ �� ��������� ������</returns>
		/// <exception cref="RequestException">�� ����� ������� ��������� �����-�� ������</exception>
		public static IAsyncTask<string> GetAsync(Func<Task<string>> Url)
		{
			AjaxStringAsyncTaskController Controller = new(Url);
			return new AclyAsyncTask<string>(Controller);
		}

		/// <summary>
		/// �������� JSON ������ �� ���������� ������
		/// </summary>
		/// <typeparam name="T">��� � ������� ����� �������������� JSON ������</typeparam>
		/// <param name="Url">����� ��� �������</param>
		/// <param name="Success">����������� ����� ��������� ��������� ����������</param>
		/// <param name="Fail">����������� ��� ��������� �����-���� ������</param>
		public static async void GetJson<T>(string Url, Action<T> Success, Action<ApiResponse>? Fail = null)
		{
			try
			{
				T Result = await GetJson<T>(Url);

				if (!Success.TryInvoke(Result, out Exception? UserError))
				{
#pragma warning disable CS8604
					Log.Error(UserError);
#pragma warning restore CS8604
				}
			}
			catch (Exception Error)
			{
				ExceptionToApiResponse(Error, Fail);
			}
		}
		/// <summary>
		/// �������� JSON ������ �� ���������� ������
		/// </summary>
		/// <param name="Url">����� ��� �������</param>
		/// <param name="Success">����������� ����� ��������� ��������� ����������</param>
		/// <param name="Progress">����������� ���������� ��������� ���������� �������</param>
		/// <param name="Fail">����������� ��� ��������� �����-���� ������</param>
		public static async void GetJson<T>(string Url, Action<T> Success, Action<float> Progress, Action<ApiResponse>? Fail = null)
		{
			using WebClient Client = GetWebClient();
			Client.DownloadProgressChanged += (s, e) =>
			{
				float Percent = (float)e.ProgressPercentage / 100;

				if (!Progress.TryInvoke(Percent, out Exception? Error))
				{
#pragma warning disable CS8604
					Log.Error(Error);
#pragma warning restore CS8604
				}
			};

			void OnGetSuccess(T? Result)
			{
				try
				{
					Result ??= Activator.CreateInstance<T>();
				}
				catch (Exception Error)
				{
					Log.Error(Error);
					return;
				}
				
				Success?.Invoke(Result);		
			}			

			try
			{
				string Result = await Client.DownloadStringTaskAsync(Url);

				await OnGetRequestCompleted<T>(Result, OnGetSuccess, Fail);
			}
			catch (Exception Error)
			{
				ExceptionToApiResponse(Error, Fail);
			}
		}
		/// <summary>
		/// �������� JSON ������ �� ���������� ������
		/// </summary>
		/// <typeparam name="T">��� � ������� ����� �������������� JSON ������</typeparam>
		/// <param name="Url">����� ��� �������</param>
		/// <returns>������ ���������������� �� JSON ������</returns>
		/// <exception cref="JsonRequestException">�� ����� ����������� JSON ������ ��������� �����-�� ������</exception>
		public static async Task<T> GetJson<T>(string Url)
		{
			string Result = await Get(Url);
			T? ResultObject = default;

			void Success(T? Obj)
			{
				ResultObject = Obj;
			}
			void Fail(ApiResponse Response)
			{
				throw new JsonRequestException(Url, Response.Code, Response.Text);
			}

			await OnGetRequestCompleted<T>(Result, Success, Fail);

			ResultObject ??= Activator.CreateInstance<T>();

			return ResultObject;
		}
		/// <summary>
		/// �������� JSON ������ �� ���������� ������
		/// </summary>
		/// <typeparam name="T">��� � ������� ����� �������������� JSON ������</typeparam>
		/// <param name="Url">����� ��� �������</param>
		/// <returns>������ �� ��������� �������, ����������������� �� JSON ������</returns>
		/// <exception cref="JsonRequestException">�� ����� ����������� JSON ������ ��������� �����-�� ������</exception>
		public static IAsyncTask<T> GetJsonAsync<T>(string Url)
		{
			AjaxJsonAsyncTaskController<T> Controller = new(Url);
			return new AclyAsyncTask<T>(Controller);
		}
		/// <summary>
		/// �������� JSON ������ �� ���������� ������
		/// </summary>
		/// <typeparam name="T">��� � ������� ����� �������������� JSON ������</typeparam>
		/// <param name="Url">����� ��� �������</param>
		/// <returns>������ �� ��������� �������, ����������������� �� JSON ������</returns>
		/// <exception cref="JsonRequestException">�� ����� ����������� JSON ������ ��������� �����-�� ������</exception>
		public static IAsyncTask<T> GetJsonAsync<T>(Task<string> Url)
		{
			AjaxJsonAsyncTaskController<T> Controller = new(Url);
			return new AclyAsyncTask<T>(Controller);
		}
		/// <summary>
		/// �������� JSON ������ �� ���������� ������
		/// </summary>
		/// <typeparam name="T">��� � ������� ����� �������������� JSON ������</typeparam>
		/// <param name="Url">����� ��� �������</param>
		/// <returns>������ �� ��������� �������, ����������������� �� JSON ������</returns>
		/// <exception cref="JsonRequestException">�� ����� ����������� JSON ������ ��������� �����-�� ������</exception>
		public static IAsyncTask<T> GetJsonAsync<T>(Func<Task<string>> Url)
		{
			AjaxJsonAsyncTaskController<T> Controller = new(Url);
			return new AclyAsyncTask<T>(Controller);
		}

		/// <summary>
		/// �������� ������ ������ �� ���������� ������
		/// </summary>
		/// <param name="Url">����� ��� �������</param>
		/// <param name="Success">����������� ����� ��������� ��������� ����������</param>
		/// <param name="Fail">����������� ��� ��������� �����-���� ������</param>
		public static async void GetBytes(string Url, Action<byte[]> Success, Action<ApiResponse>? Fail = null)
		{
			try
			{
				byte[] Result = await GetBytes(Url);

				if (!Success.TryInvoke(Result, out Exception? UserError))
				{
#pragma warning disable CS8604
					Log.Error(UserError);
#pragma warning restore CS8604
				}
			}
			catch (Exception Error)
			{
				ExceptionToApiResponse(Error, Fail);
			}
		}
		/// <summary>
		/// �������� ������ ������ �� ���������� ������
		/// </summary>
		/// <param name="Url">����� ��� �������</param>
		/// <param name="Success">����������� ����� ��������� ��������� ����������</param>
		/// <param name="Progress">����������� ���������� ��������� ���������� �������</param>
		/// <param name="Fail">����������� ��� ��������� �����-���� ������</param>
		public static async void GetBytes(string Url, Action<byte[]> Success, Action<float> Progress, Action<ApiResponse>? Fail = null)
		{
			using WebClient Client = GetWebClient();
			Client.DownloadProgressChanged += (s, e) =>
			{
				float Percent = (float)e.ProgressPercentage / 100;

				if (!Progress.TryInvoke(Percent, out Exception? Error))
				{
#pragma warning disable CS8604
					Log.Error(Error);
#pragma warning restore CS8604
				}
			};

			try
			{
				byte[] Result = await Client.DownloadDataTaskAsync(Url);

				if (!Success.TryInvoke(Result, out Exception? UserError))
				{
#pragma warning disable CS8604
					Log.Error(UserError);
#pragma warning restore CS8604
				}
			}
			catch (Exception Error)
			{
				ExceptionToApiResponse(Error, Fail);
			}
		}
		/// <summary>
		/// �������� ������ ������ �� ���������� ������
		/// </summary>
		/// <param name="Url">����� ��� �������</param>
		/// <returns>������ ������</returns>
		public static async Task<byte[]> GetBytes(string Url)
		{
			using HttpClient Web = new();
			Web.Timeout = TimeSpan.FromMinutes(1);

			Log.Message("������: " + Url);

			return await Web.GetByteArrayAsync(Url);
		}
		/// <summary>
		/// �������� ������ ������ �� ���������� ������
		/// </summary>
		/// <param name="Url">����� ��� �������</param>
		/// <returns>������ �� ��������� ������� ������</returns>
		public static IAsyncTask<byte[]> GetBytesAsync(string Url)
		{
			AjaxBytesAsyncTaskController Controller = new(Url);
			return new AclyAsyncTask<byte[]>(Controller);
		}
		/// <summary>
		/// �������� ������ ������ �� ���������� ������
		/// </summary>
		/// <param name="Url">����� ��� �������</param>
		/// <returns>������ �� ��������� ������� ������</returns>
		public static IAsyncTask<byte[]> GetBytesAsync(Task<string> Url)
		{
			AjaxBytesAsyncTaskController Controller = new(Url);
			return new AclyAsyncTask<byte[]>(Controller);
		}
		/// <summary>
		/// �������� ������ ������ �� ���������� ������
		/// </summary>
		/// <param name="Url">����� ��� �������</param>
		/// <returns>������ �� ��������� ������� ������</returns>
		public static IAsyncTask<byte[]> GetBytesAsync(Func<Task<string>> Url)
		{
			AjaxBytesAsyncTaskController Controller = new(Url);
			return new AclyAsyncTask<byte[]>(Controller);
		}

		/// <summary>
		/// ������� ���� �� ���������� ������
		/// </summary>
		/// <param name="Url">����� ��� ���������� �����</param>
		/// <param name="Dir">���� ���������� �����</param>
		/// <param name="Success">����������� ����� ��������� ��������� ����������</param>
		/// <param name="Fail">����������� ��� ��������� �����-���� ������</param>
		/// <param name="ProgressUpdated">����������� ��� ���������� ��������� ����������</param>
		public static async void Download(string Url, string Dir, Action Success, Action<Exception>? Fail = null, Action<float>? ProgressUpdated = null)
		{
			try
			{
				await Download(Url, Dir, ProgressUpdated);

				if (!Success.TryInvoke(out Exception? UserError))
				{
#pragma warning disable CS8604
					Log.Error(UserError);
#pragma warning restore CS8604
				}
			}
			catch (Exception Error)
			{
				Fail?.Invoke(Error);
			}
		}
		/// <summary>
		/// ������� ���� �� ���������� ������
		/// </summary>
		/// <param name="FileInfo">���������� � �����: �����, ���� ����������</param>
		/// <param name="Success">����������� ����� ��������� ��������� ����������</param>
		/// <param name="Fail">����������� ��� ��������� �����-���� ������</param>
		/// <param name="ProgressUpdated">����������� ��� ���������� ��������� ����������</param>
		public static void Download(DownloadFileInfo FileInfo, Action Success, Action<Exception>? Fail = null, Action<float>? ProgressUpdated = null)
		{
			if (FileInfo == null)
			{
				throw new ArgumentNullException(nameof(FileInfo), "���������� � ����� �� �������");
			}

			Download(FileInfo.Url, FileInfo.OutputPath, Success, Fail, ProgressUpdated);
		}
		/// <summary>
		/// ������� ���� �� ���������� ������
		/// </summary>
		/// <param name="Url">����� ��� ���������� �����</param>
		/// <param name="Dir">���� ���������� �����</param>
		/// <param name="ProgressUpdated">����������� ��� ���������� ��������� ����������</param>
		public static async Task Download(string Url, string Dir, Action<float>? ProgressUpdated = null)
		{
			using WebClient Web = GetWebClient(Timeout.Infinity);
			Web.DownloadProgressChanged += (s, e) =>
			{
				float Percent = (float)e.ProgressPercentage / 100;
				ProgressUpdated?.Invoke(Percent);
			};

			Log.Message("���������� �����: " + Url + " �� ���� " + Dir);

			await Web.DownloadFileTaskAsync(Url, Dir);
		}
		/// <summary>
		/// ������� ���� �� ���������� ������
		/// </summary>
		/// <param name="FileInfo">���������� � �����: �����, ���� ����������</param>
		/// <param name="ProgressUpdated">����������� ��� ���������� ��������� ����������</param>
		public static Task Download(DownloadFileInfo FileInfo, Action<float>? ProgressUpdated = null)
		{
			if (FileInfo == null)
			{
				throw new ArgumentNullException(nameof(FileInfo), "���������� � ����� �� �������");
			}

			return Download(FileInfo.Url, FileInfo.OutputPath, ProgressUpdated);
		}
		/// <summary>
		/// ������� ���� �� ���������� ������
		/// </summary>
		/// <param name="Url">����� ��� ���������� �����</param>
		/// <param name="Dir">���� ���������� �����</param>
		/// <returns>����������� ������ ���������� �����</returns>
		public static IAsyncTask DownloadAsync(string Url, string Dir)
		{
			DownloadAsyncTaskController Controller = new(Url, Dir);
			return new AclyAsyncTask(Controller);
		}
		/// <summary>
		/// ������� ���� �� ���������� ������
		/// </summary>
		/// <param name="Url">����� ��� ���������� �����</param>
		/// <param name="Dir">���� ���������� �����</param>
		/// <returns>����������� ������ ���������� �����</returns>
		public static IAsyncTask DownloadAsync(Task<string> Url, string Dir)
		{
			DownloadAsyncTaskController Controller = new(Url, Dir);
			return new AclyAsyncTask(Controller);
		}
		/// <summary>
		/// ������� ���� �� ���������� ������
		/// </summary>
		/// <param name="Url">����� ��� ���������� �����</param>
		/// <param name="Dir">���� ���������� �����</param>
		/// <returns>����������� ������ ���������� �����</returns>
		public static IAsyncTask DownloadAsync(Func<Task<string>> Url, string Dir)
		{
			DownloadAsyncTaskController Controller = new(Url, Dir);
			return new AclyAsyncTask(Controller);
		}
		/// <summary>
		/// ������� �����
		/// </summary>
		/// <param name="Files">����� ��� ����������</param>
		/// <returns>����������� ������ ���������� �����</returns>
		public static IAsyncTask DownloadAsync(IEnumerable<DownloadFileInfo> Files)
		{
			if (Files == null)
			{
				throw new ArgumentNullException(nameof(Files), "����� ��� ���������� �� �������");
			}

			DownloadListTaskController Controller = new(Files);
			return new AclyAsyncTask(Controller);
		}

		#endregion

		#region ������ � ��������

		/// <summary>
		/// ���������� ��������� ������ � ���� ������.
		/// </summary>
		/// <param name="Parameters">��������� ������</param>
		/// <returns>��������� ������ � ����� ������</returns>
		/// <exception cref="ArgumentNullException">��������� �� �������</exception>
		public static string CombineUrlParameters(params string[] Parameters)
		{
			if (Parameters == null)
			{
				throw new ArgumentNullException("", "��������� �� �������");
			}
			if (Parameters.Length == 0)
			{
				return "";
			}

			string Result = "";

			foreach (var Param in Parameters)
			{
				if (Param == null)
				{
					continue;
				}
				if (Param.Length == 0)
				{
					continue;
				}

				string Value = Param;

				if (Param[0] == '?' || Param[0] == '&')
				{
					Value = Value[1..];
				}
				if (Result.Length > 0)
				{
					Result += '&';
				}

				Result += Value;
			}

			if (Result.Length == 0)
			{
				return "";
			}

			return '?' + Result;
		}

		#endregion

		#region ������� ������� ���������

		private static async Task OnGetRequestCompleted<T>(string Response, Action<T?> OnSuccess, Action<ApiResponse>? Fail = null)
		{
			T? Res = default;

			try
			{
				T Result = await Json.Convert<T>(Response);

				if (Result == null)
				{
					Fail?.Invoke(GetErrorResponse("EmptyResponse", "����������� ����� �������. ����������, ��������� ������� �����!"));
					return;
				}

				Res = Result;
			}
			catch (Exception Err)
			{
				Fail?.Invoke(GetErrorResponse("ResponseConvertError", Err.Message + " ������ ������: " + Response.Trim()));
			}

			OnSuccess?.Invoke(Res);
		}
		private static ApiResponse GetErrorResponse(string Code, string Text)
		{
			Code ??= "-1";

			Log.Error("������ �������: " + Text + " (" + Code + ")");

			return new ApiResponse
			{
				Code = Code,
				Text = Text
			};
		}

		#endregion

		#region ���������

		/// <summary>
		/// �������������� ���������� � <see cref="ApiResponse"/>
		/// </summary>
		/// <param name="Error">����������</param>
		/// <param name="Fail">�������� ����� ��������� ���������� �����������</param>
		public static void ExceptionToApiResponse(Exception Error, Action<ApiResponse>? Fail)
		{
			ApiResponse Result = ExceptionToApiResponse(Error);

			if (Fail == null)
			{
				return;
			}
			if (!Fail.TryInvoke(Result, out Exception? UserError))
			{
#pragma warning disable CS8604
				Log.Error(UserError);
#pragma warning restore CS8604
			}
		}
		/// <summary>
		/// �������������� ���������� � <see cref="ApiResponse"/>
		/// </summary>
		/// <param name="Error">����������</param>
		/// <returns><see cref="ApiResponse"/> � ����������� �� ����������</returns>
		public static ApiResponse ExceptionToApiResponse(Exception Error)
		{
			if (Error == null)
			{
				throw new ArgumentNullException(nameof(Error), "���������� �� ���� �������");
			}

			if (Error.GetType() == typeof(JsonRequestException))
			{
				JsonRequestException Err = (JsonRequestException)Error;

				return new()
				{
					Status = ApiResponseStatus.Error,
					Code = Err.Code,
					Text = Err.Response
				};
			}
			else if (Error.GetType() == typeof(RequestException))
			{
				RequestException Err = (RequestException)Error;

				return new()
				{
					Status = ApiResponseStatus.Error,
					Code = Err.Code.ToString()
				};
			}

			return new()
			{
				Status = ApiResponseStatus.Error,
				Code = Error.GetType().Name,
				Text = Error.Message
			};
		}

		#endregion

		#region ��������� WebClient

		/// <summary>
		/// �������� <see cref="WebClient"/> � ��������� <see cref="Timeout"/>
		/// </summary>
		/// <returns>���-������</returns>
		public static WebClient GetWebClient()
		{
			return new Client(Timeout);
		}
		/// <summary>
		/// �������� <see cref="WebClient"/> � ��������� ���������
		/// </summary>
		/// <param name="Timeout">��������</param>
		/// <returns>���-������</returns>
		public static WebClient GetWebClient(Timeout Timeout)
		{
			return new Client(Timeout);
		}

		#endregion
	}
}