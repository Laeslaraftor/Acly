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
	/// Класс для быстрых запросов
	/// </summary>
	public static partial class Ajax
	{
		/// <summary>
		/// Время ожидания запроса
		/// </summary>
		public static Timeout Timeout { get; set; } = new(60000);

		#region Запросы

		/// <summary>
		/// Получить строку по указанному адресу
		/// </summary>
		/// <param name="Url">Адрес для запроса</param>
		/// <param name="Success">Выполняется после успешного получения результата</param>
		/// <param name="Fail">Выполняется при получении какой-либо ошибки</param>
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
		/// Получить строку по указанному адресу
		/// </summary>
		/// <param name="Url">Адрес для запроса</param>
		/// <param name="Success">Выполняется после успешного получения результата</param>
		/// <param name="Progress">Выполняется обновлении прогресса выполнения запроса</param>
		/// <param name="Fail">Выполняется при получении какой-либо ошибки</param>
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
		/// Получить строку по указанному адресу
		/// </summary>
		/// <param name="Url">Адрес для запроса</param>
		/// <returns>Строка ответа</returns>
		/// <exception cref="RequestException">Во время запроса произошла какая-то ошибка</exception>
		public static async Task<string> Get(string Url)
		{
			using HttpClient Web = new();
			Web.Timeout = TimeSpan.FromMinutes(1);

			Log.Message("Запрос: " + Url);

			HttpResponseMessage Result = await Web.GetAsync(Url);
			string ResultText = await Result.Content.ReadAsStringAsync();

			if (!Result.IsSuccessStatusCode)
			{
				throw new RequestException(Url, Result.StatusCode);
			}

			return ResultText;
		}
		/// <summary>
		/// Получить строку по указанному адресу
		/// </summary>
		/// <param name="Url">Адрес для запроса</param>
		/// <returns>Задача на получение строки</returns>
		/// <exception cref="RequestException">Во время запроса произошла какая-то ошибка</exception>
		public static IAsyncTask<string> GetAsync(string Url)
		{
			AjaxStringAsyncTaskController Controller = new(Url);
			return new AclyAsyncTask<string>(Controller);
		}
		/// <summary>
		/// Получить строку по указанному адресу
		/// </summary>
		/// <param name="Url">Адрес для запроса</param>
		/// <returns>Задача на получение строки</returns>
		/// <exception cref="RequestException">Во время запроса произошла какая-то ошибка</exception>
		public static IAsyncTask<string> GetAsync(Task<string> Url)
		{
			AjaxStringAsyncTaskController Controller = new(Url);
			return new AclyAsyncTask<string>(Controller);
		}
		/// <summary>
		/// Получить строку по указанному адресу
		/// </summary>
		/// <param name="Url">Адрес для запроса</param>
		/// <returns>Задача на получение строки</returns>
		/// <exception cref="RequestException">Во время запроса произошла какая-то ошибка</exception>
		public static IAsyncTask<string> GetAsync(Func<Task<string>> Url)
		{
			AjaxStringAsyncTaskController Controller = new(Url);
			return new AclyAsyncTask<string>(Controller);
		}

		/// <summary>
		/// Получить JSON данные по указанному адресу
		/// </summary>
		/// <typeparam name="T">Тип в который будет конвертирована JSON строка</typeparam>
		/// <param name="Url">Адрес для запроса</param>
		/// <param name="Success">Выполняется после успешного получения результата</param>
		/// <param name="Fail">Выполняется при получении какой-либо ошибки</param>
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
		/// Получить JSON данные по указанному адресу
		/// </summary>
		/// <param name="Url">Адрес для запроса</param>
		/// <param name="Success">Выполняется после успешного получения результата</param>
		/// <param name="Progress">Выполняется обновлении прогресса выполнения запроса</param>
		/// <param name="Fail">Выполняется при получении какой-либо ошибки</param>
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
		/// Получить JSON данные по указанному адресу
		/// </summary>
		/// <typeparam name="T">Тип в который будет конвертирована JSON строка</typeparam>
		/// <param name="Url">Адрес для запроса</param>
		/// <returns>Объект конвертированный из JSON строки</returns>
		/// <exception cref="JsonRequestException">Во время конвертации JSON строки произошла какая-то ошибка</exception>
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
		/// Получить JSON данные по указанному адресу
		/// </summary>
		/// <typeparam name="T">Тип в который будет конвертирована JSON строка</typeparam>
		/// <param name="Url">Адрес для запроса</param>
		/// <returns>Задача на получение объекта, конвертированного из JSON строки</returns>
		/// <exception cref="JsonRequestException">Во время конвертации JSON строки произошла какая-то ошибка</exception>
		public static IAsyncTask<T> GetJsonAsync<T>(string Url)
		{
			AjaxJsonAsyncTaskController<T> Controller = new(Url);
			return new AclyAsyncTask<T>(Controller);
		}
		/// <summary>
		/// Получить JSON данные по указанному адресу
		/// </summary>
		/// <typeparam name="T">Тип в который будет конвертирована JSON строка</typeparam>
		/// <param name="Url">Адрес для запроса</param>
		/// <returns>Задача на получение объекта, конвертированного из JSON строки</returns>
		/// <exception cref="JsonRequestException">Во время конвертации JSON строки произошла какая-то ошибка</exception>
		public static IAsyncTask<T> GetJsonAsync<T>(Task<string> Url)
		{
			AjaxJsonAsyncTaskController<T> Controller = new(Url);
			return new AclyAsyncTask<T>(Controller);
		}
		/// <summary>
		/// Получить JSON данные по указанному адресу
		/// </summary>
		/// <typeparam name="T">Тип в который будет конвертирована JSON строка</typeparam>
		/// <param name="Url">Адрес для запроса</param>
		/// <returns>Задача на получение объекта, конвертированного из JSON строки</returns>
		/// <exception cref="JsonRequestException">Во время конвертации JSON строки произошла какая-то ошибка</exception>
		public static IAsyncTask<T> GetJsonAsync<T>(Func<Task<string>> Url)
		{
			AjaxJsonAsyncTaskController<T> Controller = new(Url);
			return new AclyAsyncTask<T>(Controller);
		}

		/// <summary>
		/// Получить массив байтов по указанному адресу
		/// </summary>
		/// <param name="Url">Адрес для запроса</param>
		/// <param name="Success">Выполняется после успешного получения результата</param>
		/// <param name="Fail">Выполняется при получении какой-либо ошибки</param>
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
		/// Получить массив байтов по указанному адресу
		/// </summary>
		/// <param name="Url">Адрес для запроса</param>
		/// <param name="Success">Выполняется после успешного получения результата</param>
		/// <param name="Progress">Выполняется обновлении прогресса выполнения запроса</param>
		/// <param name="Fail">Выполняется при получении какой-либо ошибки</param>
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
		/// Получить массив байтов по указанному адресу
		/// </summary>
		/// <param name="Url">Адрес для запроса</param>
		/// <returns>Массив байтов</returns>
		public static async Task<byte[]> GetBytes(string Url)
		{
			using HttpClient Web = new();
			Web.Timeout = TimeSpan.FromMinutes(1);

			Log.Message("Запрос: " + Url);

			return await Web.GetByteArrayAsync(Url);
		}
		/// <summary>
		/// Получить массив байтов по указанному адресу
		/// </summary>
		/// <param name="Url">Адрес для запроса</param>
		/// <returns>Задача на получение массива байтов</returns>
		public static IAsyncTask<byte[]> GetBytesAsync(string Url)
		{
			AjaxBytesAsyncTaskController Controller = new(Url);
			return new AclyAsyncTask<byte[]>(Controller);
		}
		/// <summary>
		/// Получить массив байтов по указанному адресу
		/// </summary>
		/// <param name="Url">Адрес для запроса</param>
		/// <returns>Задача на получение массива байтов</returns>
		public static IAsyncTask<byte[]> GetBytesAsync(Task<string> Url)
		{
			AjaxBytesAsyncTaskController Controller = new(Url);
			return new AclyAsyncTask<byte[]>(Controller);
		}
		/// <summary>
		/// Получить массив байтов по указанному адресу
		/// </summary>
		/// <param name="Url">Адрес для запроса</param>
		/// <returns>Задача на получение массива байтов</returns>
		public static IAsyncTask<byte[]> GetBytesAsync(Func<Task<string>> Url)
		{
			AjaxBytesAsyncTaskController Controller = new(Url);
			return new AclyAsyncTask<byte[]>(Controller);
		}

		/// <summary>
		/// Скачать файл по указанному адресу
		/// </summary>
		/// <param name="Url">Адрес для скачивания файла</param>
		/// <param name="Dir">Путь сохранения файла</param>
		/// <param name="Success">Выполняется после успешного получения результата</param>
		/// <param name="Fail">Выполняется при получении какой-либо ошибки</param>
		/// <param name="ProgressUpdated">Выполняется при обновлении прогресса скачивания</param>
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
		/// Скачать файл по указанному адресу
		/// </summary>
		/// <param name="FileInfo">Информация о файле: адрес, путь скачивания</param>
		/// <param name="Success">Выполняется после успешного получения результата</param>
		/// <param name="Fail">Выполняется при получении какой-либо ошибки</param>
		/// <param name="ProgressUpdated">Выполняется при обновлении прогресса скачивания</param>
		public static void Download(DownloadFileInfo FileInfo, Action Success, Action<Exception>? Fail = null, Action<float>? ProgressUpdated = null)
		{
			if (FileInfo == null)
			{
				throw new ArgumentNullException(nameof(FileInfo), "Информация о файле не указана");
			}

			Download(FileInfo.Url, FileInfo.OutputPath, Success, Fail, ProgressUpdated);
		}
		/// <summary>
		/// Скачать файл по указанному адресу
		/// </summary>
		/// <param name="Url">Адрес для скачивания файла</param>
		/// <param name="Dir">Путь сохранения файла</param>
		/// <param name="ProgressUpdated">Выполняется при обновлении прогресса скачивания</param>
		public static async Task Download(string Url, string Dir, Action<float>? ProgressUpdated = null)
		{
			using WebClient Web = GetWebClient(Timeout.Infinity);
			Web.DownloadProgressChanged += (s, e) =>
			{
				float Percent = (float)e.ProgressPercentage / 100;
				ProgressUpdated?.Invoke(Percent);
			};

			Log.Message("Скачивание файла: " + Url + " по пути " + Dir);

			await Web.DownloadFileTaskAsync(Url, Dir);
		}
		/// <summary>
		/// Скачать файл по указанному адресу
		/// </summary>
		/// <param name="FileInfo">Информация о файле: адрес, путь скачивания</param>
		/// <param name="ProgressUpdated">Выполняется при обновлении прогресса скачивания</param>
		public static Task Download(DownloadFileInfo FileInfo, Action<float>? ProgressUpdated = null)
		{
			if (FileInfo == null)
			{
				throw new ArgumentNullException(nameof(FileInfo), "Информация о файле не указана");
			}

			return Download(FileInfo.Url, FileInfo.OutputPath, ProgressUpdated);
		}
		/// <summary>
		/// Скачать файл по указанному адресу
		/// </summary>
		/// <param name="Url">Адрес для скачивания файла</param>
		/// <param name="Dir">Путь сохранения файла</param>
		/// <returns>Асинхронная задача скачивания файла</returns>
		public static IAsyncTask DownloadAsync(string Url, string Dir)
		{
			DownloadAsyncTaskController Controller = new(Url, Dir);
			return new AclyAsyncTask(Controller);
		}
		/// <summary>
		/// Скачать файл по указанному адресу
		/// </summary>
		/// <param name="Url">Адрес для скачивания файла</param>
		/// <param name="Dir">Путь сохранения файла</param>
		/// <returns>Асинхронная задача скачивания файла</returns>
		public static IAsyncTask DownloadAsync(Task<string> Url, string Dir)
		{
			DownloadAsyncTaskController Controller = new(Url, Dir);
			return new AclyAsyncTask(Controller);
		}
		/// <summary>
		/// Скачать файл по указанному адресу
		/// </summary>
		/// <param name="Url">Адрес для скачивания файла</param>
		/// <param name="Dir">Путь сохранения файла</param>
		/// <returns>Асинхронная задача скачивания файла</returns>
		public static IAsyncTask DownloadAsync(Func<Task<string>> Url, string Dir)
		{
			DownloadAsyncTaskController Controller = new(Url, Dir);
			return new AclyAsyncTask(Controller);
		}
		/// <summary>
		/// Скачать файлы
		/// </summary>
		/// <param name="Files">Файлы для скачивания</param>
		/// <returns>Асинхронная задача скачивания файла</returns>
		public static IAsyncTask DownloadAsync(IEnumerable<DownloadFileInfo> Files)
		{
			if (Files == null)
			{
				throw new ArgumentNullException(nameof(Files), "Файлы для скачивания не указаны");
			}

			DownloadListTaskController Controller = new(Files);
			return new AclyAsyncTask(Controller);
		}

		#endregion

		#region Работа с адресами

		/// <summary>
		/// Объединить параметры адреса в одну строку.
		/// </summary>
		/// <param name="Parameters">Параметры адреса</param>
		/// <returns>Параметры адреса в одной строке</returns>
		/// <exception cref="ArgumentNullException">Параметры не указаны</exception>
		public static string CombineUrlParameters(params string[] Parameters)
		{
			if (Parameters == null)
			{
				throw new ArgumentNullException("", "Параметры не указаны");
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

		#region События запроса получения

		private static async Task OnGetRequestCompleted<T>(string Response, Action<T?> OnSuccess, Action<ApiResponse>? Fail = null)
		{
			T? Res = default;

			try
			{
				T Result = await Json.Convert<T>(Response);

				if (Result == null)
				{
					Fail?.Invoke(GetErrorResponse("EmptyResponse", "Неожиданный ответ сервера. Пожалуйста, повторите попытку позже!"));
					return;
				}

				Res = Result;
			}
			catch (Exception Err)
			{
				Fail?.Invoke(GetErrorResponse("ResponseConvertError", Err.Message + " Сервер вернул: " + Response.Trim()));
			}

			OnSuccess?.Invoke(Res);
		}
		private static ApiResponse GetErrorResponse(string Code, string Text)
		{
			Code ??= "-1";

			Log.Error("Ошибка запроса: " + Text + " (" + Code + ")");

			return new ApiResponse
			{
				Code = Code,
				Text = Text
			};
		}

		#endregion

		#region Обработка

		/// <summary>
		/// Конвертировать исключение в <see cref="ApiResponse"/>
		/// </summary>
		/// <param name="Error">Исключение</param>
		/// <param name="Fail">Действие после получение результата конвертации</param>
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
		/// Конвертировать исключение в <see cref="ApiResponse"/>
		/// </summary>
		/// <param name="Error">Исключение</param>
		/// <returns><see cref="ApiResponse"/> с информацией об исключении</returns>
		public static ApiResponse ExceptionToApiResponse(Exception Error)
		{
			if (Error == null)
			{
				throw new ArgumentNullException(nameof(Error), "Исключение не было указано");
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

		#region Получение WebClient

		/// <summary>
		/// Получить <see cref="WebClient"/> с задержкой <see cref="Timeout"/>
		/// </summary>
		/// <returns>Веб-клиент</returns>
		public static WebClient GetWebClient()
		{
			return new Client(Timeout);
		}
		/// <summary>
		/// Получить <see cref="WebClient"/> с указанной задержкой
		/// </summary>
		/// <param name="Timeout">Задержка</param>
		/// <returns>Веб-клиент</returns>
		public static WebClient GetWebClient(Timeout Timeout)
		{
			return new Client(Timeout);
		}

		#endregion
	}
}