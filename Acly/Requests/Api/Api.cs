using Acly.Tasks;
using System;
using System.Threading.Tasks;

namespace Acly.Requests
{
	/// <summary>
	/// Класс, предоставляющий возможность быстро и удобно отправлять Api запросы 
	/// </summary>
	public static partial class Api
	{
		/// <summary>
		/// Маска запроса.
		/// Значение по умолчанию: {<see cref="BaseUrl"/>}/{Filename}{<see cref="BaseFileExtension"/>}{Parameters}.
		/// То есть: {0}/{1}{2}{3}.
		/// Здесь {Parameters} - это объединённые <see cref="BaseParameters"/> и параметры, указанные при вызове <see cref="GetUrl(string, string)"/>
		/// </summary>
		public static string BaseUrlFormat
		{
			get => _Server.BaseUrlFormat;
			set => _Server.BaseUrlFormat = value;
		}
		/// <summary>
		/// Основной адрес запроса. Например, https://api.acly.ru
		/// </summary>
		public static string BaseUrl
		{
			get => _Server.BaseUrl;
			set => _Server.BaseUrl = value;
		}
		/// <summary>
		/// Основной тип файла. Например: php; json; html.
		/// Если у файла нет типа то необходимо оставить это поле пустым или использовать <see cref="Get{T}(string, string, bool)"/> с последним аргументом false
		/// </summary>
		public static string BaseFileExtension
		{
			get => _Server.BaseFileExtension;
			set => _Server.BaseFileExtension = value;
		}
		/// <summary>
		/// Функция получения параметров, которые будут использоваться в каждом запросе.
		/// Пример выводимых параметров:
		/// <code>param=value&amp;param2=value2</code>
		/// </summary>
		public static Func<Task<string>>? BaseParameters
		{
			get => _Server.BaseParameters;
			set => _Server.BaseParameters = value;
		}

		private readonly static Server _Server = new();

		#region JSON запросы

		/// <summary>
		/// Отправить запрос на сервер и получить ответ через <see cref="Action{T}"/>
		/// </summary>
		/// <typeparam name="T">Тип в который будет конвертирован ответ сервера</typeparam>
		/// <param name="Filename">Название файла на сервере</param>
		/// <param name="Success">Действие при получении успешного результата</param>
		/// <param name="Fail">Действие при получении какой-либо ошибки</param>
		public static void Get<T>(string Filename, Action<T> Success, Action<ApiResponse>? Fail = null) where T : ApiResponse
		{
			_Server.Get(Filename, Success, Fail);
		}
		/// <summary>
		/// Отправить запрос на сервер и получить ответ через <see cref="Action{T}"/>
		/// </summary>
		/// <typeparam name="T">Тип в который будет конвертирован ответ сервера</typeparam>
		/// <param name="Filename">Название файла на сервере</param>
		/// <param name="UseBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
		/// <param name="Success">Действие при получении успешного результата</param>
		/// <param name="Fail">Действие при получении какой-либо ошибки</param>
		public static void Get<T>(string Filename, bool UseBaseFileExtension, Action<T> Success, Action<ApiResponse>? Fail = null) where T : ApiResponse
		{
			_Server.Get(Filename, UseBaseFileExtension, Success, Fail);
		}
		/// <summary>
		/// Отправить запрос на сервер и получить ответ через <see cref="Action{T}"/>
		/// </summary>
		/// <typeparam name="T">Тип в который будет конвертирован ответ сервера</typeparam>
		/// <param name="Filename">Название файла на сервере</param>
		/// <param name="Parameters">Параметры запроса</param>
		/// <param name="Success">Действие при получении успешного результата</param>
		/// <param name="Fail">Действие при получении какой-либо ошибки</param>
		public static void Get<T>(string Filename, string? Parameters, Action<T> Success, Action<ApiResponse>? Fail = null) where T : ApiResponse
		{
			_Server.Get(Filename, Parameters, Success, Fail);
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
		public static void Get<T>(string Filename, string? Parameters, bool UseBaseFileExtension, Action<T> Success, Action<ApiResponse>? Fail = null) where T : ApiResponse
		{
			_Server.Get(Filename, Parameters, UseBaseFileExtension, Success, Fail);
		}

		/// <summary>
		/// Отправить запрос на сервер и получить ответ
		/// </summary>
		/// <typeparam name="T">Тип в который будет конвертирован ответ сервера</typeparam>
		/// <param name="Filename">Название файла на сервере</param>
		/// <returns>Ответ сервера, конвертированный в указанный тип</returns>
		public static async Task<T> Get<T>(string Filename) where T : ApiResponse
		{
			return await _Server.Get<T>(Filename);
		}
		/// <summary>
		/// Отправить запрос на сервер и получить ответ
		/// </summary>
		/// <typeparam name="T">Тип в который будет конвертирован ответ сервера</typeparam>
		/// <param name="Filename">Название файла на сервере</param>
		/// <param name="UseBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
		/// <returns>Ответ сервера, конвертированный в указанный тип</returns>
		public static async Task<T> Get<T>(string Filename, bool UseBaseFileExtension) where T : ApiResponse
		{
			return await _Server.Get<T>(Filename, UseBaseFileExtension);
		}
		/// <summary>
		/// Отправить запрос на сервер и получить ответ
		/// </summary>
		/// <typeparam name="T">Тип в который будет конвертирован ответ сервера</typeparam>
		/// <param name="Filename">Название файла на сервере</param>
		/// <param name="Parameters">Параметры запроса</param>
		/// <returns>Ответ сервера, конвертированный в указанный тип</returns>
		public static async Task<T> Get<T>(string Filename, string? Parameters) where T : ApiResponse
		{
			return await _Server.Get<T>(Filename, Parameters);
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
		public static async Task<T> Get<T>(string Filename, string? Parameters, bool UseBaseFileExtension) where T : ApiResponse
		{
			return await _Server.Get<T>(Filename, Parameters, UseBaseFileExtension);
		}

		/// <summary>
		/// Отправить запрос на сервер и получить ответ. Ошибки будут выведены как ответ сервера
		/// </summary>
		/// <typeparam name="T">Тип в который будет конвертирован ответ сервера</typeparam>
		/// <param name="Filename">Название файла на сервере</param>
		/// <returns>Ответ, конвертированный в указанный тип</returns>
		/// <exception cref="ArgumentNullException">Название файла не указано</exception>
		public static async Task<T> GetSafe<T>(string Filename) where T : ApiResponse
		{
			return await _Server.GetSafe<T>(Filename);
		}
		/// <summary>
		/// Отправить запрос на сервер и получить ответ. Ошибки будут выведены как ответ сервера
		/// </summary>
		/// <typeparam name="T">Тип в который будет конвертирован ответ сервера</typeparam>
		/// <param name="Filename">Название файла на сервере</param>
		/// <param name="UseBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
		/// <returns>Ответ, конвертированный в указанный тип</returns>
		/// <exception cref="ArgumentNullException">Название файла не указано</exception>
		public static async Task<T> GetSafe<T>(string Filename, bool UseBaseFileExtension) where T : ApiResponse
		{
			return await _Server.GetSafe<T>(Filename, UseBaseFileExtension);
		}
		/// <summary>
		/// Отправить запрос на сервер и получить ответ. Ошибки будут выведены как ответ сервера
		/// </summary>
		/// <typeparam name="T">Тип в который будет конвертирован ответ сервера</typeparam>
		/// <param name="Filename">Название файла на сервере</param>
		/// <param name="Parameters">Параметры запроса</param>
		/// <returns>Ответ, конвертированный в указанный тип</returns>
		/// <exception cref="ArgumentNullException">Название файла не указано</exception>
		public static async Task<T> GetSafe<T>(string Filename, string? Parameters) where T : ApiResponse
		{
			return await _Server.GetSafe<T>(Filename, Parameters);
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
		public static async Task<T> GetSafe<T>(string Filename, string? Parameters, bool UseBaseFileExtension) where T : ApiResponse
		{
			return await _Server.GetSafe<T>(Filename, Parameters, UseBaseFileExtension);
		}

		/// <summary>
		/// Отправить запрос на сервер и получить ответ
		/// </summary>
		/// <typeparam name="T">Тип в который будет конвертирован ответ сервера</typeparam>
		/// <param name="Filename">Название файла на сервере</param>
		/// <returns>Асинхронная задача запроса к серверу</returns>
		/// <exception cref="ArgumentNullException">Название файла не указано</exception>
		public static IAsyncTask<T> GetAsync<T>(string Filename) where T : ApiResponse
		{
			return _Server.GetAsync<T>(Filename);
		}
		/// <summary>
		/// Отправить запрос на сервер и получить ответ
		/// </summary>
		/// <typeparam name="T">Тип в который будет конвертирован ответ сервера</typeparam>
		/// <param name="Filename">Название файла на сервере</param>
		/// <param name="UseBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
		/// <returns>Асинхронная задача запроса к серверу</returns>
		/// <exception cref="ArgumentNullException">Название файла не указано</exception>
		public static IAsyncTask<T> GetAsync<T>(string Filename, bool UseBaseFileExtension) where T : ApiResponse
		{
			return _Server.GetAsync<T>(Filename, UseBaseFileExtension);
		}
		/// <summary>
		/// Отправить запрос на сервер и получить ответ
		/// </summary>
		/// <typeparam name="T">Тип в который будет конвертирован ответ сервера</typeparam>
		/// <param name="Filename">Название файла на сервере</param>
		/// <param name="Parameters">Параметры запроса</param>
		/// <returns>Асинхронная задача запроса к серверу</returns>
		/// <exception cref="ArgumentNullException">Название файла не указано</exception>
		public static IAsyncTask<T> GetAsync<T>(string Filename, string? Parameters) where T : ApiResponse
		{
			return _Server.GetAsync<T>(Filename, Parameters);
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
		public static IAsyncTask<T> GetAsync<T>(string Filename, string? Parameters, bool UseBaseFileExtension) where T : ApiResponse
		{
			return _Server.GetAsync<T>(Filename, Parameters, UseBaseFileExtension);
		}

		#endregion

		#region string запросы

		/// <summary>
		/// Отправить запрос на сервер и получить ответ
		/// </summary>
		/// <param name="Filename">Название файла на сервере</param>
		/// <returns>Ответ сервера</returns>
		public static async Task<string> GetString(string Filename)
		{
			return await _Server.GetString(Filename);
		}
		/// <summary>
		/// Отправить запрос на сервер и получить ответ
		/// </summary>
		/// <param name="Filename">Название файла на сервере</param>
		/// <param name="UseBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
		/// <returns>Ответ сервера</returns>
		public static async Task<string> GetString(string Filename, bool UseBaseFileExtension)
		{
			return await _Server.GetString(Filename, UseBaseFileExtension);
		}
		/// <summary>
		/// Отправить запрос на сервер и получить ответ
		/// </summary>
		/// <param name="Filename">Название файла на сервере</param>
		/// <param name="Parameters">Параметры запроса</param>
		/// <returns>Ответ сервера</returns>
		public static async Task<string> GetString(string Filename, string? Parameters)
		{
			return await _Server.GetString(Filename, Parameters);
		}
		/// <summary>
		/// Отправить запрос на сервер и получить ответ
		/// </summary>
		/// <param name="Filename">Название файла на сервере</param>
		/// <param name="Parameters">Параметры запроса</param>
		/// <param name="UseBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
		/// <returns>Ответ сервера</returns>
		/// <exception cref="ArgumentNullException">Название файла не указано</exception>
		public static async Task<string> GetString(string Filename, string? Parameters, bool UseBaseFileExtension)
		{
			return await _Server.GetString(Filename, Parameters, UseBaseFileExtension);
		}

		/// <summary>
		/// Отправить запрос на сервер и получить ответ
		/// </summary>
		/// <param name="Filename">Название файла на сервере</param>
		/// <param name="Success">Действие при получении успешного результата</param>
		/// <param name="Fail">Действие при получении какой-либо ошибки</param>
		/// <returns>Ответ сервера</returns>
		public static void GetString(string Filename, Action<string> Success, Action<ApiResponse>? Fail = null)
		{
			_Server.GetString(Filename, Success, Fail);
		}
		/// <summary>
		/// Отправить запрос на сервер и получить ответ
		/// </summary>
		/// <param name="Filename">Название файла на сервере</param>
		/// <param name="UseBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
		/// <param name="Success">Действие при получении успешного результата</param>
		/// <param name="Fail">Действие при получении какой-либо ошибки</param>
		/// <returns>Ответ сервера</returns>
		public static void GetString(string Filename, bool UseBaseFileExtension, Action<string> Success, Action<ApiResponse>? Fail = null)
		{
			_Server.GetString(Filename, UseBaseFileExtension, Success, Fail);
		}
		/// <summary>
		/// Отправить запрос на сервер и получить ответ
		/// </summary>
		/// <param name="Filename">Название файла на сервере</param>
		/// <param name="Parameters">Параметры запроса</param>
		/// <param name="Success">Действие при получении успешного результата</param>
		/// <param name="Fail">Действие при получении какой-либо ошибки</param>
		/// <returns>Ответ сервера</returns>
		public static void GetString(string Filename, string? Parameters, Action<string> Success, Action<ApiResponse>? Fail = null)
		{
			_Server.GetString(Filename, Parameters, Success, Fail);
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
		public static void GetString(string Filename, string? Parameters, bool UseBaseFileExtension, Action<string> Success, Action<ApiResponse>? Fail = null)
		{
			_Server.GetString(Filename, Parameters, UseBaseFileExtension, Success, Fail);
		}

		/// <summary>
		/// Отправить запрос на сервер и получить ответ
		/// </summary>
		/// <param name="Filename">Название файла на сервере</param>
		/// <returns>Асинхронная задача запроса к серверу</returns>
		/// <exception cref="ArgumentNullException">Название файла не указано</exception>
		public static IAsyncTask<string> GetStringAsync(string Filename)
		{
			return _Server.GetStringAsync(Filename);
		}
		/// <summary>
		/// Отправить запрос на сервер и получить ответ
		/// </summary>
		/// <param name="Filename">Название файла на сервере</param>
		/// <param name="UseBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
		/// <returns>Асинхронная задача запроса к серверу</returns>
		/// <exception cref="ArgumentNullException">Название файла не указано</exception>
		public static IAsyncTask<string> GetStringAsync(string Filename, bool UseBaseFileExtension)
		{
			return _Server.GetStringAsync(Filename, UseBaseFileExtension);
		}
		/// <summary>
		/// Отправить запрос на сервер и получить ответ
		/// </summary>
		/// <param name="Filename">Название файла на сервере</param>
		/// <param name="Parameters">Параметры запроса</param>
		/// <returns>Асинхронная задача запроса к серверу</returns>
		/// <exception cref="ArgumentNullException">Название файла не указано</exception>
		public static IAsyncTask<string> GetStringAsync(string Filename, string? Parameters)
		{
			return _Server.GetStringAsync(Filename, Parameters);
		}
		/// <summary>
		/// Отправить запрос на сервер и получить ответ
		/// </summary>
		/// <param name="Filename">Название файла на сервере</param>
		/// <param name="Parameters">Параметры запроса</param>
		/// <param name="UseBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
		/// <returns>Асинхронная задача запроса к серверу</returns>
		/// <exception cref="ArgumentNullException">Название файла не указано</exception>
		public static IAsyncTask<string> GetStringAsync(string Filename, string? Parameters, bool UseBaseFileExtension)
		{
			return _Server.GetStringAsync(Filename, Parameters, UseBaseFileExtension);
		}

		#endregion

		#region byte[] запросы

		/// <summary>
		/// Отправить запрос на сервер и получить данные
		/// </summary>
		/// <param name="Filename">Название файла на сервере</param>
		/// <returns>Данные с сервера</returns>
		public static async Task<byte[]> GetBytes(string Filename)
		{
			return await _Server.GetBytes(Filename);
		}
		/// <summary>
		/// Отправить запрос на сервер и получить данные
		/// </summary>
		/// <param name="Filename">Название файла на сервере</param>
		/// <param name="UseBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
		/// <returns>Данные с сервера</returns>
		public static async Task<byte[]> GetBytes(string Filename, bool UseBaseFileExtension)
		{
			return await _Server.GetBytes(Filename, UseBaseFileExtension);
		}
		/// <summary>
		/// Отправить запрос на сервер и получить данные
		/// </summary>
		/// <param name="Filename">Название файла на сервере</param>
		/// <param name="Parameters">Параметры запроса</param>
		/// <returns>Данные с сервера</returns>
		public static async Task<byte[]> GetBytes(string Filename, string? Parameters)
		{
			return await _Server.GetBytes(Filename, Parameters);
		}
		/// <summary>
		/// Отправить запрос на сервер и получить данные
		/// </summary>
		/// <param name="Filename">Название файла на сервере</param>
		/// <param name="Parameters">Параметры запроса</param>
		/// <param name="UseBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
		/// <returns>Данные с сервера</returns>
		/// <exception cref="ArgumentNullException">Название файла не указано</exception>
		public static async Task<byte[]> GetBytes(string Filename, string? Parameters, bool UseBaseFileExtension)
		{
			return await _Server.GetBytes(Filename, Parameters, UseBaseFileExtension);
		}

		/// <summary>
		/// Отправить запрос на сервер и получить данные
		/// </summary>
		/// <param name="Filename">Название файла на сервере</param>
		/// <param name="Success">Действие при получении успешного результата</param>
		/// <param name="Fail">Действие при получении какой-либо ошибки</param>
		/// <returns>Ответ сервера</returns>
		public static void GetBytes(string Filename, Action<byte[]> Success, Action<ApiResponse>? Fail = null)
		{
			_Server.GetBytes(Filename, Success, Fail);
		}
		/// <summary>
		/// Отправить запрос на сервер и получить данные
		/// </summary>
		/// <param name="Filename">Название файла на сервере</param>
		/// <param name="UseBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
		/// <param name="Success">Действие при получении успешного результата</param>
		/// <param name="Fail">Действие при получении какой-либо ошибки</param>
		/// <returns>Ответ сервера</returns>
		public static void GetBytes(string Filename, bool UseBaseFileExtension, Action<byte[]> Success, Action<ApiResponse>? Fail = null)
		{
			_Server.GetBytes(Filename, UseBaseFileExtension, Success, Fail);
		}
		/// <summary>
		/// Отправить запрос на сервер и получить данные
		/// </summary>
		/// <param name="Filename">Название файла на сервере</param>
		/// <param name="Parameters">Параметры запроса</param>
		/// <param name="Success">Действие при получении успешного результата</param>
		/// <param name="Fail">Действие при получении какой-либо ошибки</param>
		/// <returns>Ответ сервера</returns>
		public static void GetBytes(string Filename, string? Parameters, Action<byte[]> Success, Action<ApiResponse>? Fail = null)
		{
			_Server.GetBytes(Filename, Parameters, Success, Fail);
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
		public static void GetBytes(string Filename, string? Parameters, bool UseBaseFileExtension, Action<byte[]> Success, Action<ApiResponse>? Fail = null)
		{
			_Server.GetBytes(Filename, Parameters, UseBaseFileExtension, Success, Fail);
		}

		/// <summary>
		/// Отправить запрос на сервер и получить данные
		/// </summary>
		/// <param name="Filename">Название файла на сервере</param>
		/// <returns>Асинхронная задача запроса к серверу</returns>
		/// <exception cref="ArgumentNullException">Название файла не указано</exception>
		public static IAsyncTask<byte[]> GetBytesAsync(string Filename)
		{
			return _Server.GetBytesAsync(Filename);
		}
		/// <summary>
		/// Отправить запрос на сервер и получить данные
		/// </summary>
		/// <param name="Filename">Название файла на сервере</param>
		/// <param name="UseBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
		/// <returns>Асинхронная задача запроса к серверу</returns>
		/// <exception cref="ArgumentNullException">Название файла не указано</exception>
		public static IAsyncTask<byte[]> GetBytesAsync(string Filename, bool UseBaseFileExtension)
		{
			return _Server.GetBytesAsync(Filename, UseBaseFileExtension);
		}
		/// <summary>
		/// Отправить запрос на сервер и получить данные
		/// </summary>
		/// <param name="Filename">Название файла на сервере</param>
		/// <param name="Parameters">Параметры запроса</param>
		/// <returns>Асинхронная задача запроса к серверу</returns>
		/// <exception cref="ArgumentNullException">Название файла не указано</exception>
		public static IAsyncTask<byte[]> GetBytesAsync(string Filename, string? Parameters)
		{
			return _Server.GetBytesAsync(Filename, Parameters);
		}
		/// <summary>
		/// Отправить запрос на сервер и получить данные
		/// </summary>
		/// <param name="Filename">Название файла на сервере</param>
		/// <param name="Parameters">Параметры запроса</param>
		/// <param name="UseBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
		/// <returns>Асинхронная задача запроса к серверу</returns>
		/// <exception cref="ArgumentNullException">Название файла не указано</exception>
		public static IAsyncTask<byte[]> GetBytesAsync(string Filename, string? Parameters, bool UseBaseFileExtension)
		{
			return _Server.GetBytesAsync(Filename, Parameters, UseBaseFileExtension);
		}

		#endregion

		#region Скачивание

		/// <summary>
		/// Скачать файл с сервера
		/// </summary>
		/// <param name="Filename">Название файла на сервере</param>
		/// <param name="FilePath">Путь сохранения файла</param>
		/// <exception cref="ArgumentNullException">Название или путь файла не указан</exception>
		public static Task Download(string Filename, string FilePath)
		{
			return _Server.Download(Filename, FilePath);
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
		public static void Download(string Filename, string FilePath, Action Success, Action<Exception>? Fail = null, Action<float>? ProgressUpdated = null)
		{
			_Server.Download(Filename, FilePath, Success, Fail, ProgressUpdated);
		}
		/// <summary>
		/// Скачать файл с сервера
		/// </summary>
		/// <param name="Filename">Название файла на сервере</param>
		/// <param name="FilePath">Путь сохранения файла</param>
		/// <returns>Задача скачивания файла</returns>
		/// <exception cref="ArgumentNullException">Название или путь файла не указан</exception>
		public static IAsyncTask DownloadAsync(string Filename, string FilePath)
		{
			return _Server.DownloadAsync(Filename, FilePath);
		}

		#endregion

		#region Получение адреса запроса

		/// <summary>
		/// Получить адрес запроса
		/// </summary>
		/// <param name="Filename">Название файла на сервере</param>
		/// <returns>Адрес запроса</returns>
		public static async Task<string> GetUrl(string Filename)
		{
			return await GetUrl(Filename, "");
		}
		/// <summary>
		/// Получить адрес запроса
		/// </summary>
		/// <param name="Filename">Название файла на сервере</param>
		/// <param name="UseBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
		/// <returns>Адрес запроса</returns>
		public static async Task<string> GetUrl(string Filename, bool UseBaseFileExtension)
		{
			return await GetUrl(Filename, "", UseBaseFileExtension);
		}
		/// <summary>
		/// Получить адрес запроса
		/// </summary>
		/// <param name="Filename">Название файла на сервере</param>
		/// <param name="Parameters">Параметры запроса</param>
		/// <returns>Адрес запроса</returns>
		public static async Task<string> GetUrl(string Filename, string? Parameters)
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
		public static async Task<string> GetUrl(string Filename, string? Parameters, bool UseBaseFileExtension)
		{
			if (Filename == null)
			{
				throw new ArgumentNullException(nameof(Filename), "Название файла не указано");
			}

			return await _Server.GetUrl(Filename, Parameters, UseBaseFileExtension);
		}

		#endregion
	}
}
