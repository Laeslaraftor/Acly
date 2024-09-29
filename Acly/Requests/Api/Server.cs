using Acly.Tasks;
using System;
using System.Threading.Tasks;

namespace Acly.Requests
{
	public partial class Api
	{
		/// <summary>
		/// Класс, предоставляющий возможность быстро и удобно отправлять Api запросы 
		/// </summary>
#pragma warning disable CA1724
		public class Server
#pragma warning restore CA1724
		{
			/// <summary>
			/// Создать класс для отправки запросов на сервер
			/// </summary>
			public Server() { }
			/// <summary>
			/// Создать класс для отправки запросов на сервер
			/// </summary>
			/// <param name="BaseUrl">Адрес сервера. Например, https://api.acly.ru</param>
			public Server(string BaseUrl)
			{
				this.BaseUrl = BaseUrl;
			}
			/// <summary>
			/// Создать класс для отправки запросов на сервер
			/// </summary>
			/// <param name="BaseUrl">Адрес сервера. Например, https://api.acly.ru</param>
			/// <param name="BaseFileExtension">
			/// Основной тип файла. Например: php; json; html.
			/// Если у файла нет типа то необходимо оставить это поле пустым или использовать <see cref="Get{T}(string, string, bool)"/> с последним аргументом false
			/// </param>
			public Server(string BaseUrl, string BaseFileExtension)
			{
				this.BaseUrl = BaseUrl;
				this.BaseFileExtension = BaseFileExtension;
			}
			/// <summary>
			/// Создать класс для отправки запросов на сервер
			/// </summary>
			/// <param name="BaseUrl">Адрес сервера. Например, https://api.acly.ru</param>
			/// <param name="BaseFileExtension">
			/// Основной тип файла. Например: php; json; html.
			/// Если у файла нет типа то необходимо оставить это поле пустым или использовать <see cref="Get{T}(string, string, bool)"/> с последним аргументом false
			/// </param>
			/// <param name="BaseParameters">
			/// Функция получения параметров, которые будут использоваться в каждом запросе.
			/// Пример выводимых параметров:
			/// <code>param=value&amp;param2=value2</code>
			/// </param>
			public Server(string BaseUrl, string BaseFileExtension, Func<Task<string>> BaseParameters)
			{
				this.BaseUrl = BaseUrl;
				this.BaseFileExtension = BaseFileExtension;
				this.BaseParameters = BaseParameters;
			}

			/// <summary>
			/// Маска запроса.
			/// Значение по умолчанию: {<see cref="BaseUrl"/>}/{Filename}{<see cref="BaseFileExtension"/>}{Parameters}.
			/// То есть: {0}/{1}{2}{3}.
			/// Здесь {Parameters} - это объединённые <see cref="BaseParameters"/> и параметры, указанные при вызове <see cref="GetUrl(string, string)"/>
			/// </summary>
			public string BaseUrlFormat
			{
				get
				{
					if (_BaseUrlFormat == null)
					{
						return "";
					}

					return _BaseUrlFormat;
				}
				set => _BaseUrlFormat = value;
			}
			/// <summary>
			/// Основной адрес запроса. Например, https://api.acly.ru
			/// </summary>
			public string BaseUrl
			{
				get
				{
					if (_BaseUrl == null)
					{
						return "";
					}

					return _BaseUrl;
				}
				set => _BaseUrl = value;
			}
			/// <summary>
			/// Основной тип файла. Например: php; json; html.
			/// Если у файла нет типа то необходимо оставить это поле пустым или использовать <see cref="Get{T}(string, string, bool)"/> с последним аргументом false
			/// </summary>
			public string BaseFileExtension
			{
				get
				{
					string? Result = _BaseFileExtension;

					if (Result == null)
					{
						return "";
					}
					if (Result.Length > 0)
					{
						Result = '.' + Result;
					}

					return Result;
				}
				set => _BaseFileExtension = value;
			}
			/// <summary>
			/// Функция получения параметров, которые будут использоваться в каждом запросе.
			/// Пример выводимых параметров:
			/// <code>param=value&amp;param2=value2</code>
			/// </summary>
			public Func<Task<string>>? BaseParameters { get; set; }

			private string? _BaseUrl;
			private string? _BaseFileExtension;
			private string? _BaseUrlFormat = "{0}/{1}{2}{3}";

			#region JSON запросы

			/// <summary>
			/// Отправить запрос на сервер и получить ответ через <see cref="Action{T}"/>
			/// </summary>
			/// <typeparam name="T">Тип в который будет конвертирован ответ сервера</typeparam>
			/// <param name="Filename">Название файла на сервере</param>
			/// <param name="Success">Действие при получении успешного результата</param>
			/// <param name="Fail">Действие при получении какой-либо ошибки</param>
			public void Get<T>(string Filename, Action<T> Success, Action<ApiResponse>? Fail = null) where T : ApiResponse
			{
				Get(Filename, "", Success, Fail);
			}
			/// <summary>
			/// Отправить запрос на сервер и получить ответ через <see cref="Action{T}"/>
			/// </summary>
			/// <typeparam name="T">Тип в который будет конвертирован ответ сервера</typeparam>
			/// <param name="Filename">Название файла на сервере</param>
			/// <param name="UseBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
			/// <param name="Success">Действие при получении успешного результата</param>
			/// <param name="Fail">Действие при получении какой-либо ошибки</param>
			public void Get<T>(string Filename, bool UseBaseFileExtension, Action<T> Success, Action<ApiResponse>? Fail = null) where T : ApiResponse
			{
				Get(Filename, "", UseBaseFileExtension, Success, Fail);
			}
			/// <summary>
			/// Отправить запрос на сервер и получить ответ через <see cref="Action{T}"/>
			/// </summary>
			/// <typeparam name="T">Тип в который будет конвертирован ответ сервера</typeparam>
			/// <param name="Filename">Название файла на сервере</param>
			/// <param name="Parameters">Параметры запроса</param>
			/// <param name="Success">Действие при получении успешного результата</param>
			/// <param name="Fail">Действие при получении какой-либо ошибки</param>
			public void Get<T>(string Filename, string? Parameters, Action<T> Success, Action<ApiResponse>? Fail = null) where T : ApiResponse
			{
				Get(Filename, Parameters, true, Success, Fail);
			}
			/// <summary>
			/// Отправить запрос на сервер и получить ответ через <see cref="Action{T}"/>
			/// </summary>
			/// <typeparam name="T">Тип в который будет конвертирован ответ сервера</typeparam>
			/// <param name="Filename">Название файла на сервере</param>
			/// <param name="Parameters">Параметры запроса</param>
			/// <param name="UseBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
			/// <param name="Success">Действие при получении успешного результата</param>
			/// <param name="Fail">Действие при получении какой-либо ошибки</param>
			/// <exception cref="ArgumentNullException">Название файла не указано</exception>
			public async void Get<T>(string Filename, string? Parameters, bool UseBaseFileExtension, Action<T> Success, Action<ApiResponse>? Fail = null) where T : ApiResponse
			{
				if (Filename == null)
				{
					throw new ArgumentNullException(nameof(Filename), "Название файла не указано");
				}

				T? Result = default;

				try
				{
					Result = await Get<T>(Filename, Parameters, UseBaseFileExtension);
				}
				catch (Exception Error)
				{
					Ajax.ExceptionToApiResponse(Error, Fail);
					return;
				}

				if (Result == default)
				{
					Result = Activator.CreateInstance<T>();
				}

				Success?.Invoke(Result);
			}

			/// <summary>
			/// Отправить запрос на сервер и получить ответ
			/// </summary>
			/// <typeparam name="T">Тип в который будет конвертирован ответ сервера</typeparam>
			/// <param name="Filename">Название файла на сервере</param>
			/// <returns>Ответ сервера, конвертированный в указанный тип</returns>
			public async Task<T> Get<T>(string Filename) where T : ApiResponse
			{
				return await Get<T>(Filename, "");
			}
			/// <summary>
			/// Отправить запрос на сервер и получить ответ
			/// </summary>
			/// <typeparam name="T">Тип в который будет конвертирован ответ сервера</typeparam>
			/// <param name="Filename">Название файла на сервере</param>
			/// <param name="UseBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
			/// <returns>Ответ сервера, конвертированный в указанный тип</returns>
			public async Task<T> Get<T>(string Filename, bool UseBaseFileExtension) where T : ApiResponse
			{
				return await Get<T>(Filename, "", UseBaseFileExtension);
			}
			/// <summary>
			/// Отправить запрос на сервер и получить ответ
			/// </summary>
			/// <typeparam name="T">Тип в который будет конвертирован ответ сервера</typeparam>
			/// <param name="Filename">Название файла на сервере</param>
			/// <param name="Parameters">Параметры запроса</param>
			/// <returns>Ответ сервера, конвертированный в указанный тип</returns>
			public async Task<T> Get<T>(string Filename, string? Parameters) where T : ApiResponse
			{
				return await Get<T>(Filename, Parameters, true);
			}
			/// <summary>
			/// Отправить запрос на сервер и получить ответ
			/// </summary>
			/// <typeparam name="T">Тип в который будет конвертирован ответ сервера</typeparam>
			/// <param name="Filename">Название файла на сервере</param>
			/// <param name="Parameters">Параметры запроса</param>
			/// <param name="UseBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
			/// <returns>Ответ сервера, конвертированный в указанный тип</returns>
			/// <exception cref="ArgumentNullException">Название файла не указано</exception>
			public async Task<T> Get<T>(string Filename, string? Parameters, bool UseBaseFileExtension) where T : ApiResponse
			{
				if (Filename == null)
				{
					throw new ArgumentNullException(nameof(Filename), "Название файла не указано");
				}

				string Url = await GetUrl(Filename, Parameters, UseBaseFileExtension);

				return await Ajax.GetJson<T>(Url);
			}

			/// <summary>
			/// Отправить запрос на сервер и получить ответ. Ошибки будут выведены как ответ сервера
			/// </summary>
			/// <typeparam name="T">Тип в который будет конвертирован ответ сервера</typeparam>
			/// <param name="Filename">Название файла на сервере</param>
			/// <returns>Ответ, конвертированный в указанный тип</returns>
			/// <exception cref="ArgumentNullException">Название файла не указано</exception>
			public async Task<T> GetSafe<T>(string Filename) where T : ApiResponse
			{
				return await GetSafe<T>(Filename, "");
			}
			/// <summary>
			/// Отправить запрос на сервер и получить ответ. Ошибки будут выведены как ответ сервера
			/// </summary>
			/// <typeparam name="T">Тип в который будет конвертирован ответ сервера</typeparam>
			/// <param name="Filename">Название файла на сервере</param>
			/// <param name="UseBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
			/// <returns>Ответ, конвертированный в указанный тип</returns>
			/// <exception cref="ArgumentNullException">Название файла не указано</exception>
			public async Task<T> GetSafe<T>(string Filename, bool UseBaseFileExtension) where T : ApiResponse
			{
				return await GetSafe<T>(Filename, "", UseBaseFileExtension);
			}
			/// <summary>
			/// Отправить запрос на сервер и получить ответ. Ошибки будут выведены как ответ сервера
			/// </summary>
			/// <typeparam name="T">Тип в который будет конвертирован ответ сервера</typeparam>
			/// <param name="Filename">Название файла на сервере</param>
			/// <param name="Parameters">Параметры запроса</param>
			/// <returns>Ответ, конвертированный в указанный тип</returns>
			/// <exception cref="ArgumentNullException">Название файла не указано</exception>
			public async Task<T> GetSafe<T>(string Filename, string? Parameters) where T : ApiResponse
			{
				return await GetSafe<T>(Filename, Parameters, true);
			}
			/// <summary>
			/// Отправить запрос на сервер и получить ответ. Ошибки будут выведены как ответ сервера
			/// </summary>
			/// <typeparam name="T">Тип в который будет конвертирован ответ сервера</typeparam>
			/// <param name="Filename">Название файла на сервере</param>
			/// <param name="Parameters">Параметры запроса</param>
			/// <param name="UseBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
			/// <returns>Ответ, конвертированный в указанный тип</returns>
			/// <exception cref="ArgumentNullException">Название файла не указано</exception>
			public async Task<T> GetSafe<T>(string Filename, string? Parameters, bool UseBaseFileExtension) where T : ApiResponse
			{
				if (Filename == null)
				{
					throw new ArgumentNullException(nameof(Filename), "Название файла не указано");
				}

				string Url = await GetUrl(Filename, Parameters, UseBaseFileExtension);

				try
				{
					return await Ajax.GetJson<T>(Url);
				}
				catch (Exception Error)
				{
					return (T)Ajax.ExceptionToApiResponse(Error);
				}
			}

			/// <summary>
			/// Отправить запрос на сервер и получить ответ
			/// </summary>
			/// <typeparam name="T">Тип в который будет конвертирован ответ сервера</typeparam>
			/// <param name="Filename">Название файла на сервере</param>
			/// <returns>Асинхронная задача запроса к серверу</returns>
			/// <exception cref="ArgumentNullException">Название файла не указано</exception>
			public IAsyncTask<T> GetAsync<T>(string Filename) where T : ApiResponse
			{
				return GetAsync<T>(Filename, "");
			}
			/// <summary>
			/// Отправить запрос на сервер и получить ответ
			/// </summary>
			/// <typeparam name="T">Тип в который будет конвертирован ответ сервера</typeparam>
			/// <param name="Filename">Название файла на сервере</param>
			/// <param name="UseBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
			/// <returns>Асинхронная задача запроса к серверу</returns>
			/// <exception cref="ArgumentNullException">Название файла не указано</exception>
			public IAsyncTask<T> GetAsync<T>(string Filename, bool UseBaseFileExtension) where T : ApiResponse
			{
				return GetAsync<T>(Filename, "", UseBaseFileExtension);
			}
			/// <summary>
			/// Отправить запрос на сервер и получить ответ
			/// </summary>
			/// <typeparam name="T">Тип в который будет конвертирован ответ сервера</typeparam>
			/// <param name="Filename">Название файла на сервере</param>
			/// <param name="Parameters">Параметры запроса</param>
			/// <returns>Асинхронная задача запроса к серверу</returns>
			/// <exception cref="ArgumentNullException">Название файла не указано</exception>
			public IAsyncTask<T> GetAsync<T>(string Filename, string? Parameters) where T : ApiResponse
			{
				return GetAsync<T>(Filename, Parameters, true);
			}
			/// <summary>
			/// Отправить запрос на сервер и получить ответ
			/// </summary>
			/// <typeparam name="T">Тип в который будет конвертирован ответ сервера</typeparam>
			/// <param name="Filename">Название файла на сервере</param>
			/// <param name="Parameters">Параметры запроса</param>
			/// <param name="UseBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
			/// <returns>Асинхронная задача запроса к серверу</returns>
			/// <exception cref="ArgumentNullException">Название файла не указано</exception>
			public IAsyncTask<T> GetAsync<T>(string Filename, string? Parameters, bool UseBaseFileExtension) where T : ApiResponse
			{
				if (Filename == null)
				{
					throw new ArgumentNullException(nameof(Filename), "Название файла не указано");
				}

				Task<string> Url = GetUrl(Filename, Parameters, UseBaseFileExtension);

				return Ajax.GetJsonAsync<T>(Url);
			}

			#endregion

			#region string запросы

			/// <summary>
			/// Отправить запрос на сервер и получить ответ
			/// </summary>
			/// <param name="Filename">Название файла на сервере</param>
			/// <returns>Ответ сервера</returns>
			public async Task<string> GetString(string Filename)
			{
				return await GetString(Filename, "");
			}
			/// <summary>
			/// Отправить запрос на сервер и получить ответ
			/// </summary>
			/// <param name="Filename">Название файла на сервере</param>
			/// <param name="UseBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
			/// <returns>Ответ сервера</returns>
			public async Task<string> GetString(string Filename, bool UseBaseFileExtension)
			{
				return await GetString(Filename, "", UseBaseFileExtension);
			}
			/// <summary>
			/// Отправить запрос на сервер и получить ответ
			/// </summary>
			/// <param name="Filename">Название файла на сервере</param>
			/// <param name="Parameters">Параметры запроса</param>
			/// <returns>Ответ сервера</returns>
			public async Task<string> GetString(string Filename, string? Parameters)
			{
				return await GetString(Filename, Parameters, true);
			}
			/// <summary>
			/// Отправить запрос на сервер и получить ответ
			/// </summary>
			/// <param name="Filename">Название файла на сервере</param>
			/// <param name="Parameters">Параметры запроса</param>
			/// <param name="UseBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
			/// <returns>Ответ сервера</returns>
			/// <exception cref="ArgumentNullException">Название файла не указано</exception>
			public async Task<string> GetString(string Filename, string? Parameters, bool UseBaseFileExtension)
			{
				if (Filename == null)
				{
					throw new ArgumentNullException(nameof(Filename), "Название файла не указано");
				}

				string Url = await GetUrl(Filename, Parameters, UseBaseFileExtension);

				return await Ajax.Get(Url);
			}

			/// <summary>
			/// Отправить запрос на сервер и получить ответ
			/// </summary>
			/// <param name="Filename">Название файла на сервере</param>
			/// <param name="Success">Действие при получении успешного результата</param>
			/// <param name="Fail">Действие при получении какой-либо ошибки</param>
			/// <returns>Ответ сервера</returns>
			public void GetString(string Filename, Action<string> Success, Action<ApiResponse>? Fail = null)
			{
				GetString(Filename, "", Success, Fail);
			}
			/// <summary>
			/// Отправить запрос на сервер и получить ответ
			/// </summary>
			/// <param name="Filename">Название файла на сервере</param>
			/// <param name="UseBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
			/// <param name="Success">Действие при получении успешного результата</param>
			/// <param name="Fail">Действие при получении какой-либо ошибки</param>
			/// <returns>Ответ сервера</returns>
			public void GetString(string Filename, bool UseBaseFileExtension, Action<string> Success, Action<ApiResponse>? Fail = null)
			{
				GetString(Filename, "", UseBaseFileExtension, Success, Fail);
			}
			/// <summary>
			/// Отправить запрос на сервер и получить ответ
			/// </summary>
			/// <param name="Filename">Название файла на сервере</param>
			/// <param name="Parameters">Параметры запроса</param>
			/// <param name="Success">Действие при получении успешного результата</param>
			/// <param name="Fail">Действие при получении какой-либо ошибки</param>
			/// <returns>Ответ сервера</returns>
			public void GetString(string Filename, string? Parameters, Action<string> Success, Action<ApiResponse>? Fail = null)
			{
				GetString(Filename, Parameters, true, Success, Fail);
			}
			/// <summary>
			/// Отправить запрос на сервер и получить ответ
			/// </summary>
			/// <param name="Filename">Название файла на сервере</param>
			/// <param name="Parameters">Параметры запроса</param>
			/// <param name="UseBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
			/// <param name="Success">Действие при получении успешного результата</param>
			/// <param name="Fail">Действие при получении какой-либо ошибки</param>
			/// <returns>Ответ сервера</returns>
			/// <exception cref="ArgumentNullException">Название файла не указано</exception>
			public async void GetString(string Filename, string? Parameters, bool UseBaseFileExtension, Action<string> Success, Action<ApiResponse>? Fail = null)
			{
				if (Filename == null)
				{
					throw new ArgumentNullException(nameof(Filename), "Название файла не указано");
				}

				string Url = await GetUrl(Filename, Parameters, UseBaseFileExtension);

				Ajax.Get(Url, Success, Fail);
			}

			/// <summary>
			/// Отправить запрос на сервер и получить ответ
			/// </summary>
			/// <param name="Filename">Название файла на сервере</param>
			/// <returns>Асинхронная задача запроса к серверу</returns>
			/// <exception cref="ArgumentNullException">Название файла не указано</exception>
			public IAsyncTask<string> GetStringAsync(string Filename)
			{
				return GetStringAsync(Filename, "");
			}
			/// <summary>
			/// Отправить запрос на сервер и получить ответ
			/// </summary>
			/// <param name="Filename">Название файла на сервере</param>
			/// <param name="UseBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
			/// <returns>Асинхронная задача запроса к серверу</returns>
			/// <exception cref="ArgumentNullException">Название файла не указано</exception>
			public IAsyncTask<string> GetStringAsync(string Filename, bool UseBaseFileExtension)
			{
				return GetStringAsync(Filename, "", UseBaseFileExtension);
			}
			/// <summary>
			/// Отправить запрос на сервер и получить ответ
			/// </summary>
			/// <param name="Filename">Название файла на сервере</param>
			/// <param name="Parameters">Параметры запроса</param>
			/// <returns>Асинхронная задача запроса к серверу</returns>
			/// <exception cref="ArgumentNullException">Название файла не указано</exception>
			public IAsyncTask<string> GetStringAsync(string Filename, string? Parameters)
			{
				return GetStringAsync(Filename, Parameters, true);
			}
			/// <summary>
			/// Отправить запрос на сервер и получить ответ
			/// </summary>
			/// <param name="Filename">Название файла на сервере</param>
			/// <param name="Parameters">Параметры запроса</param>
			/// <param name="UseBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
			/// <returns>Асинхронная задача запроса к серверу</returns>
			/// <exception cref="ArgumentNullException">Название файла не указано</exception>
			public IAsyncTask<string> GetStringAsync(string Filename, string? Parameters, bool UseBaseFileExtension)
			{
				if (Filename == null)
				{
					throw new ArgumentNullException(nameof(Filename), "Название файла не указано");
				}

				Task<string> Url = GetUrl(Filename, Parameters, UseBaseFileExtension);

				return Ajax.GetAsync(Url);
			}

			#endregion

			#region byte[] запросы

			/// <summary>
			/// Отправить запрос на сервер и получить данные
			/// </summary>
			/// <param name="Filename">Название файла на сервере</param>
			/// <returns>Данные с сервера</returns>
			public async Task<byte[]> GetBytes(string Filename)
			{
				return await GetBytes(Filename, "");
			}
			/// <summary>
			/// Отправить запрос на сервер и получить данные
			/// </summary>
			/// <param name="Filename">Название файла на сервере</param>
			/// <param name="UseBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
			/// <returns>Данные с сервера</returns>
			public async Task<byte[]> GetBytes(string Filename, bool UseBaseFileExtension)
			{
				return await GetBytes(Filename, "", UseBaseFileExtension);
			}
			/// <summary>
			/// Отправить запрос на сервер и получить данные
			/// </summary>
			/// <param name="Filename">Название файла на сервере</param>
			/// <param name="Parameters">Параметры запроса</param>
			/// <returns>Данные с сервера</returns>
			public async Task<byte[]> GetBytes(string Filename, string? Parameters)
			{
				return await GetBytes(Filename, Parameters, true);
			}
			/// <summary>
			/// Отправить запрос на сервер и получить данные
			/// </summary>
			/// <param name="Filename">Название файла на сервере</param>
			/// <param name="Parameters">Параметры запроса</param>
			/// <param name="UseBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
			/// <returns>Данные с сервера</returns>
			/// <exception cref="ArgumentNullException">Название файла не указано</exception>
			public async Task<byte[]> GetBytes(string Filename, string? Parameters, bool UseBaseFileExtension)
			{
				if (Filename == null)
				{
					throw new ArgumentNullException(nameof(Filename), "Название файла не указано");
				}

				string Url = await GetUrl(Filename, Parameters, UseBaseFileExtension);

				return await Ajax.GetBytes(Url);
			}

			/// <summary>
			/// Отправить запрос на сервер и получить данные
			/// </summary>
			/// <param name="Filename">Название файла на сервере</param>
			/// <param name="Success">Действие при получении успешного результата</param>
			/// <param name="Fail">Действие при получении какой-либо ошибки</param>
			/// <returns>Ответ сервера</returns>
			public void GetBytes(string Filename, Action<byte[]> Success, Action<ApiResponse>? Fail = null)
			{
				GetBytes(Filename, "", Success, Fail);
			}
			/// <summary>
			/// Отправить запрос на сервер и получить данные
			/// </summary>
			/// <param name="Filename">Название файла на сервере</param>
			/// <param name="UseBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
			/// <param name="Success">Действие при получении успешного результата</param>
			/// <param name="Fail">Действие при получении какой-либо ошибки</param>
			/// <returns>Ответ сервера</returns>
			public void GetBytes(string Filename, bool UseBaseFileExtension, Action<byte[]> Success, Action<ApiResponse>? Fail = null)
			{
				GetBytes(Filename, "", UseBaseFileExtension, Success, Fail);
			}
			/// <summary>
			/// Отправить запрос на сервер и получить данные
			/// </summary>
			/// <param name="Filename">Название файла на сервере</param>
			/// <param name="Parameters">Параметры запроса</param>
			/// <param name="Success">Действие при получении успешного результата</param>
			/// <param name="Fail">Действие при получении какой-либо ошибки</param>
			/// <returns>Ответ сервера</returns>
			public void GetBytes(string Filename, string? Parameters, Action<byte[]> Success, Action<ApiResponse>? Fail = null)
			{
				GetBytes(Filename, Parameters, true, Success, Fail);
			}
			/// <summary>
			/// Отправить запрос на сервер и получить данные
			/// </summary>
			/// <param name="Filename">Название файла на сервере</param>
			/// <param name="Parameters">Параметры запроса</param>
			/// <param name="UseBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
			/// <param name="Success">Действие при получении успешного результата</param>
			/// <param name="Fail">Действие при получении какой-либо ошибки</param>
			/// <returns>Ответ сервера</returns>
			/// <exception cref="ArgumentNullException">Название файла не указано</exception>
			public async void GetBytes(string Filename, string? Parameters, bool UseBaseFileExtension, Action<byte[]> Success, Action<ApiResponse>? Fail = null)
			{
				if (Filename == null)
				{
					throw new ArgumentNullException(nameof(Filename), "Название файла не указано");
				}

				string Url = await GetUrl(Filename, Parameters, UseBaseFileExtension);

				Ajax.GetBytes(Url, Success, Fail);
			}

			/// <summary>
			/// Отправить запрос на сервер и получить данные
			/// </summary>
			/// <param name="Filename">Название файла на сервере</param>
			/// <returns>Асинхронная задача запроса к серверу</returns>
			/// <exception cref="ArgumentNullException">Название файла не указано</exception>
			public IAsyncTask<byte[]> GetBytesAsync(string Filename)
			{
				return GetBytesAsync(Filename, "");
			}
			/// <summary>
			/// Отправить запрос на сервер и получить данные
			/// </summary>
			/// <param name="Filename">Название файла на сервере</param>
			/// <param name="UseBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
			/// <returns>Асинхронная задача запроса к серверу</returns>
			/// <exception cref="ArgumentNullException">Название файла не указано</exception>
			public IAsyncTask<byte[]> GetBytesAsync(string Filename, bool UseBaseFileExtension)
			{
				return GetBytesAsync(Filename, "", UseBaseFileExtension);
			}
			/// <summary>
			/// Отправить запрос на сервер и получить данные
			/// </summary>
			/// <param name="Filename">Название файла на сервере</param>
			/// <param name="Parameters">Параметры запроса</param>
			/// <returns>Асинхронная задача запроса к серверу</returns>
			/// <exception cref="ArgumentNullException">Название файла не указано</exception>
			public IAsyncTask<byte[]> GetBytesAsync(string Filename, string? Parameters)
			{
				return GetBytesAsync(Filename, Parameters, true);
			}
			/// <summary>
			/// Отправить запрос на сервер и получить данные
			/// </summary>
			/// <param name="Filename">Название файла на сервере</param>
			/// <param name="Parameters">Параметры запроса</param>
			/// <param name="UseBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
			/// <returns>Асинхронная задача запроса к серверу</returns>
			/// <exception cref="ArgumentNullException">Название файла не указано</exception>
			public IAsyncTask<byte[]> GetBytesAsync(string Filename, string? Parameters, bool UseBaseFileExtension)
			{
				if (Filename == null)
				{
					throw new ArgumentNullException(nameof(Filename), "Название файла не указано");
				}

				Task<string> Url = GetUrl(Filename, Parameters, UseBaseFileExtension);

				return Ajax.GetBytesAsync(Url);
			}

			#endregion

			#region Скачивание

			/// <summary>
			/// Скачать файл с сервера
			/// </summary>
			/// <param name="Filename">Название файла на сервере</param>
			/// <param name="FilePath">Путь сохранения файла</param>
			/// <exception cref="ArgumentNullException">Название или путь файла не указан</exception>
			public async Task Download(string Filename, string FilePath)
			{
				if (Filename == null)
				{
					throw new ArgumentNullException(nameof(Filename), "Название файла не указано");
				}
				if (FilePath == null)
				{
					throw new ArgumentNullException(nameof(FilePath), "Путь сохранения файла не указан");
				}

				string Url = await GetUrl(Filename);

				await Ajax.Download(Url, FilePath);
			}
			/// <summary>
			/// Скачать файл с сервера
			/// </summary>
			/// <param name="Filename">Название файла на сервере</param>
			/// <param name="FilePath">Путь сохранения файла</param>
			/// <param name="Success">Выполняется после успешного получения результата</param>
			/// <param name="Fail">Выполняется при получении какой-либо ошибки</param>
			/// <param name="ProgressUpdated">Выполняется при обновлении прогресса скачивания</param>
			/// <exception cref="ArgumentNullException">Название или путь файла не указан</exception>
			public async void Download(string Filename, string FilePath, Action Success, Action<Exception>? Fail = null, Action<float>? ProgressUpdated = null)
			{
				if (Filename == null)
				{
					throw new ArgumentNullException(nameof(Filename), "Название файла не указано");
				}
				if (FilePath == null)
				{
					throw new ArgumentNullException(nameof(FilePath), "Путь сохранения файла не указан");
				}

				string Url = await GetUrl(Filename);

				Ajax.Download(Url, FilePath, Success, Fail, ProgressUpdated);
			}
			/// <summary>
			/// Скачать файл с сервера
			/// </summary>
			/// <param name="Filename">Название файла на сервере</param>
			/// <param name="FilePath">Путь сохранения файла</param>
			/// <returns>Задача скачивания файла</returns>
			/// <exception cref="ArgumentNullException">Название или путь файла не указан</exception>
			public IAsyncTask DownloadAsync(string Filename, string FilePath)
			{
				if (Filename == null)
				{
					throw new ArgumentNullException(nameof(Filename), "Название файла не указано");
				}
				if (FilePath == null)
				{
					throw new ArgumentNullException(nameof(FilePath), "Путь сохранения файла не указан");
				}

				Task<string> Url = GetUrl(Filename);

				return Ajax.DownloadAsync(Url, FilePath);
			}

			#endregion

			#region Получение адреса запроса

			/// <summary>
			/// Получить адрес запроса
			/// </summary>
			/// <param name="Filename">Название файла на сервере</param>
			/// <returns>Адрес запроса</returns>
			public async Task<string> GetUrl(string Filename)
			{
				return await GetUrl(Filename, "");
			}
			/// <summary>
			/// Получить адрес запроса
			/// </summary>
			/// <param name="Filename">Название файла на сервере</param>
			/// <param name="UseBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
			/// <returns>Адрес запроса</returns>
			public async Task<string> GetUrl(string Filename, bool UseBaseFileExtension)
			{
				return await GetUrl(Filename, "", UseBaseFileExtension);
			}
			/// <summary>
			/// Получить адрес запроса
			/// </summary>
			/// <param name="Filename">Название файла на сервере</param>
			/// <param name="Parameters">Параметры запроса</param>
			/// <returns>Адрес запроса</returns>
			public async Task<string> GetUrl(string Filename, string? Parameters)
			{
				return await GetUrl(Filename, Parameters, true);
			}
			/// <summary>
			/// Получить адрес запроса
			/// </summary>
			/// <param name="Filename">Название файла на сервере</param>
			/// <param name="Parameters">Параметры запроса</param>
			/// <param name="UseBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
			/// <returns>Адрес запроса</returns>
			/// <exception cref="ArgumentNullException">Название файла не указано</exception>
			public async Task<string> GetUrl(string Filename, string? Parameters, bool UseBaseFileExtension)
			{
				if (Filename == null)
				{
					throw new ArgumentNullException(nameof(Filename), "Название файла не указано");
				}

				string ArgParams = "";

				if (Parameters != null)
				{
					ArgParams = Parameters;
				}

				string BaseParameters = await GetBaseParametersValue();
				string Params = Ajax.CombineUrlParameters(BaseParameters, ArgParams);
				string Extension = "";

				if (UseBaseFileExtension)
				{
					Extension = BaseFileExtension;
				}

				return string.Format(BaseUrlFormat, BaseUrl, Filename, Extension, Params);
			}

			private async Task<string> GetBaseParametersValue()
			{
				if (BaseParameters != null)
				{
					try
					{
						return await BaseParameters.Invoke();
					}
					catch { }
				}

				return "";
			}

			#endregion
		}
	}
}
