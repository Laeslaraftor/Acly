using System;
using System.Threading.Tasks;

namespace Acly.Requests
{
	public static partial class Api
	{
		/// <summary>
		/// Шаблон запроса для быстрых запросов на сервер
		/// </summary>
		public class Request
		{
			/// <summary>
			/// Создать шаблон запроса
			/// </summary>
			/// <param name="Filename">Название файла на сервере</param>
			public Request(string Filename)
			{
				this.Filename = Filename;
			}
			/// <summary>
			/// Создать шаблон запроса
			/// </summary>
			/// <param name="Filename">Название файла на сервере</param>
			/// <param name="Parameters">Параметры запроса</param>
			public Request(string Filename, string Parameters)
			{
				this.Filename = Filename;
				this.Parameters = Parameters;
			}
			/// <summary>
			/// Создать шаблон запроса
			/// </summary>
			/// <param name="Filename">Название файла на сервере</param>
			/// <param name="UseBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
			public Request(string Filename, bool UseBaseFileExtension)
			{
				this.Filename = Filename;
				this.UseBaseFileExtension = UseBaseFileExtension;
			}
			/// <summary>
			/// Создать шаблон запроса
			/// </summary>
			/// <param name="Filename">Название файла на сервере</param>
			/// <param name="Parameters">Параметры запроса</param>
			/// <param name="UseBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
			public Request(string Filename, string Parameters, bool UseBaseFileExtension)
			{
				this.Filename = Filename;
				this.Parameters = Parameters;
				this.UseBaseFileExtension = UseBaseFileExtension;
			}

			/// <summary>
			/// Название файла на сервере
			/// </summary>
			public string Filename { get; set; }
			/// <summary>
			/// Параметры запроса
			/// </summary>
			public string? Parameters { get; set; }
			/// <summary>
			/// Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)
			/// </summary>
			public bool UseBaseFileExtension { get; set; } = true;

			#region Запросы

			/// <summary>
			/// Отправить запрос на сервер и получить ответ через <see cref="Action{T}"/>
			/// </summary>
			/// <typeparam name="T">Тип в который будет конвертирован ответ сервера</typeparam>
			/// <param name="Success">Действие при получении успешного результата</param>
			/// <param name="Fail">Действие при получении какой-либо ошибки</param>
			/// <exception cref="ArgumentNullException">Название файла не указано</exception>
			public void Get<T>(Action<T> Success, Action<ApiResponse>? Fail = null) where T : ApiResponse
			{
				if (Filename == null)
				{
					throw new ArgumentNullException(nameof(Filename), "Название файла не указано");
				}

				Api.Get(Filename, Parameters, UseBaseFileExtension, Success, Fail);
			}
			/// <summary>
			/// Отправить запрос на сервер и получить ответ
			/// </summary>
			/// <typeparam name="T">Тип в который будет конвертирован ответ сервера</typeparam>
			/// <returns>Ответ сервера, конвертированный в указанный тип</returns>
			/// <exception cref="ArgumentNullException">Название файла не указано</exception>
			public async Task<T> Get<T>() where T : ApiResponse
			{
				if (Filename == null)
				{
					throw new ArgumentNullException(nameof(Filename), "Название файла не указано");
				}

				return await Api.Get<T>(Filename, Parameters, UseBaseFileExtension);
			}
			/// <summary>
			/// Отправить запрос на сервер и получить ответ. Ошибки будут выведены как ответ сервера
			/// </summary>
			/// <typeparam name="T">Тип в который будет конвертирован ответ сервера</typeparam>
			/// <returns>Ответ, конвертированный в указанный тип</returns>
			/// <exception cref="ArgumentNullException">Название файла не указано</exception>
			public async Task<T> GetSafe<T>() where T : ApiResponse
			{
				if (Filename == null)
				{
					throw new ArgumentNullException(nameof(Filename), "Название файла не указано");
				}

				return await Api.GetSafe<T>(Filename, Parameters, UseBaseFileExtension);
			}

			#endregion
		}
	}
}
