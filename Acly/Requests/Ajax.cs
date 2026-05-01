using Acly.Requests.Tasks;
using Acly.Tasks;
using System;
using System.Collections.Generic;
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
        public static TimeSpan Timeout
        {
            get => HttpRequestClient.Timeout;
            set => HttpRequestClient.Timeout = value;
        }

        private static HttpRequestClient HttpRequestClient
        {
            get
            {
                field ??= new()
                {
                    Timeout = TimeSpan.FromMinutes(1)
                };
                return field;
            }
        }
        private static Api.Server Server
        {
            get
            {
                field ??= new(HttpRequestClient);
                return field;
            }
        }

        #region Запросы

        /// <summary>
        /// Получить строку по указанному адресу
        /// </summary>
        /// <param name="url">Адрес для запроса</param>
        /// <param name="success">Выполняется после успешного получения результата</param>
        /// <param name="fail">Выполняется при получении какой-либо ошибки</param>
        public static void Get(string url, Action<string> success, Action<ApiResponse>? fail = null)
        {
            Server.GetString(url, success, fail);
        }
        /// <summary>
        /// Получить строку по указанному адресу
        /// </summary>
        /// <param name="url">Адрес для запроса</param>
        /// <param name="success">Выполняется после успешного получения результата</param>
        /// <param name="progress">Выполняется обновлении прогресса выполнения запроса</param>
        /// <param name="fail">Выполняется при получении какой-либо ошибки</param>
        public static void Get(string url, Action<string> success, Action<float> progress, Action<ApiResponse>? fail = null)
        {
            Server.GetString(url).Pin(success, progress, e => ResponseToApi(e, fail));
        }
        /// <summary>
        /// Получить строку по указанному адресу
        /// </summary>
        /// <param name="url">Адрес для запроса</param>
        /// <returns>Строка ответа</returns>
        /// <exception cref="RequestException">Во время запроса произошла какая-то ошибка</exception>
        public static async Task<string> Get(string url)
        {
            return await Server.GetString(url);
        }
        /// <summary>
        /// Получить строку по указанному адресу
        /// </summary>
        /// <param name="url">Адрес для запроса</param>
        /// <returns>Задача на получение строки</returns>
        /// <exception cref="RequestException">Во время запроса произошла какая-то ошибка</exception>
        public static IAsyncTask<string> GetAsync(string url)
        {
            return Server.GetString(url);
        }

        /// <summary>
        /// Получить JSON данные по указанному адресу
        /// </summary>
        /// <typeparam name="T">Тип в который будет конвертирована JSON строка</typeparam>
        /// <param name="url">Адрес для запроса</param>
        /// <param name="success">Выполняется после успешного получения результата</param>
        /// <param name="fail">Выполняется при получении какой-либо ошибки</param>
        public static void GetJson<T>(string url, Action<T> success, Action<ApiResponse>? fail = null)
            where T : new()
        {
            Server.Get(url, success, fail);
        }
        /// <summary>
        /// Получить JSON данные по указанному адресу
        /// </summary>
        /// <param name="url">Адрес для запроса</param>
        /// <param name="success">Выполняется после успешного получения результата</param>
        /// <param name="progress">Выполняется обновлении прогресса выполнения запроса</param>
        /// <param name="fail">Выполняется при получении какой-либо ошибки</param>
        public static void GetJson<T>(string url, Action<T> success, Action<float> progress, Action<ApiResponse>? fail = null)
            where T : new()
        {
            Server.Get<T>(url).Pin(success, progress, e => ResponseToApi(e, fail));
        }
        /// <summary>
        /// Получить JSON данные по указанному адресу
        /// </summary>
        /// <typeparam name="T">Тип в который будет конвертирована JSON строка</typeparam>
        /// <param name="url">Адрес для запроса</param>
        /// <returns>Объект конвертированный из JSON строки</returns>
        /// <exception cref="JsonRequestException">Во время конвертации JSON строки произошла какая-то ошибка</exception>
        public static async Task<T> GetJson<T>(string url)
            where T : new()
        {
            return await Server.Get<T>(url);
        }
        /// <summary>
        /// Получить JSON данные по указанному адресу
        /// </summary>
        /// <typeparam name="T">Тип в который будет конвертирована JSON строка</typeparam>
        /// <param name="url">Адрес для запроса</param>
        /// <returns>Задача на получение объекта, конвертированного из JSON строки</returns>
        /// <exception cref="JsonRequestException">Во время конвертации JSON строки произошла какая-то ошибка</exception>
        public static IAsyncTask<T> GetJsonAsync<T>(string url)
            where T : new()
        {
            return Server.Get<T>(url);
        }

        /// <summary>
        /// Получить массив байтов по указанному адресу
        /// </summary>
        /// <param name="url">Адрес для запроса</param>
        /// <param name="success">Выполняется после успешного получения результата</param>
        /// <param name="fail">Выполняется при получении какой-либо ошибки</param>
        public static void GetBytes(string url, Action<byte[]> success, Action<ApiResponse>? fail = null)
        {
            Server.GetBytes(url, success, r => ResponseToApi(r, fail));
        }
        /// <summary>
        /// Получить массив байтов по указанному адресу
        /// </summary>
        /// <param name="url">Адрес для запроса</param>
        /// <param name="success">Выполняется после успешного получения результата</param>
        /// <param name="progress">Выполняется обновлении прогресса выполнения запроса</param>
        /// <param name="fail">Выполняется при получении какой-либо ошибки</param>
        public static void GetBytes(string url, Action<byte[]> success, Action<float> progress, Action<ApiResponse>? fail = null)
        {
            Server.GetBytes(url).Pin(success, progress, e => ResponseToApi(e, fail));
        }
        /// <summary>
        /// Получить массив байтов по указанному адресу
        /// </summary>
        /// <param name="url">Адрес для запроса</param>
        /// <returns>Массив байтов</returns>
        public static async Task<byte[]> GetBytes(string url)
        {
            return await Server.GetBytes(url);
        }
        /// <summary>
        /// Получить массив байтов по указанному адресу
        /// </summary>
        /// <param name="url">Адрес для запроса</param>
        /// <returns>Задача на получение массива байтов</returns>
        public static IAsyncTask<byte[]> GetBytesAsync(string url)
        {
            return Server.GetBytes(url);
        }

        /// <summary>
        /// Скачать файл по указанному адресу
        /// </summary>
        /// <param name="url">Адрес для скачивания файла</param>
        /// <param name="filePath">Путь сохранения файла</param>
        /// <param name="success">Выполняется после успешного получения результата</param>
        /// <param name="fail">Выполняется при получении какой-либо ошибки</param>
        /// <param name="progressUpdated">Выполняется при обновлении прогресса скачивания</param>
        public static void Download(string url, string filePath, Action success, Action<ApiResponse>? fail = null, AsyncTaskProgress? progressUpdated = null)
        {
            Server.GetDownload(url, filePath, success, progressUpdated, r => ResponseToApi(r, fail));
        }
        /// <summary>
        /// Скачать файл по указанному адресу
        /// </summary>
        /// <param name="fileInfo">Информация о файле: адрес, путь скачивания</param>
        /// <param name="success">Выполняется после успешного получения результата</param>
        /// <param name="fail">Выполняется при получении какой-либо ошибки</param>
        /// <param name="progressUpdated">Выполняется при обновлении прогресса скачивания</param>
        public static void Download(DownloadFileInfo fileInfo, Action success, Action<ApiResponse>? fail = null, AsyncTaskProgress? progressUpdated = null)
        {
            Download(fileInfo.Url, fileInfo.OutputPath, success, fail, progressUpdated);
        }
        /// <summary>
        /// Скачать файл по указанному адресу
        /// </summary>
        /// <param name="url">Адрес для скачивания файла</param>
        /// <param name="filePath">Путь сохранения файла</param>
        /// <param name="progressUpdated">Выполняется при обновлении прогресса скачивания</param>
        public static async Task Download(string url, string filePath, AsyncTaskProgress? progressUpdated = null)
        {
            var task = Server.GetDownload(url, filePath);

            if (progressUpdated != null)
            {
                task.Pin(progressUpdated);
            }

            await task;
        }
        /// <summary>
        /// Скачать файл по указанному адресу
        /// </summary>
        /// <param name="fileInfo">Информация о файле: адрес, путь скачивания</param>
        /// <param name="progressUpdated">Выполняется при обновлении прогресса скачивания</param>
        public static Task Download(DownloadFileInfo fileInfo, AsyncTaskProgress? progressUpdated = null)
        {
            return Download(fileInfo.Url, fileInfo.OutputPath, progressUpdated);
        }
        /// <summary>
        /// Скачать файл по указанному адресу
        /// </summary>
        /// <param name="url">Адрес для скачивания файла</param>
        /// <param name="filePath">Путь сохранения файла</param>
        /// <returns>Асинхронная задача скачивания файла</returns>
        public static IAsyncTask DownloadAsync(string url, string filePath)
        {
            return Server.GetDownload(url, filePath);
        }
        /// <summary>
        /// Скачать файлы
        /// </summary>
        /// <param name="files">Файлы для скачивания</param>
        /// <returns>Асинхронная задача скачивания файла</returns>
        public static IAsyncTask DownloadAsync(IEnumerable<DownloadFileInfo> files)
        {
            if (files == null)
            {
                throw new ArgumentNullException(nameof(files), "Файлы для скачивания не указаны");
            }

            DownloadListTaskController Controller = new(files);
            return new AclyAsyncTask(Controller);
        }

        #endregion

        #region Работа с адресами

        /// <summary>
        /// Объединить параметры адреса в одну строку.
        /// </summary>
        /// <param name="parameters">Параметры адреса</param>
        /// <returns>Параметры адреса в одной строке</returns>
        /// <exception cref="ArgumentNullException">Параметры не указаны</exception>
        public static string CombineUrlParameters(params string[] parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters), "Параметры не указаны");
            }
            if (parameters.Length == 0)
            {
                return string.Empty;
            }

            string result = string.Empty;

            foreach (var parameter in parameters)
            {
                if (parameter == null)
                {
                    continue;
                }
                if (parameter.Length == 0)
                {
                    continue;
                }

                string value = parameter;

                if (parameter[0] == '?' || parameter[0] == '&')
                {
                    value = value[1..];
                }
                if (result.Length > 0)
                {
                    result += '&';
                }

                result += value;
            }

            if (result.Length != 0)
            {
                result = '?' + result;
            }

            return result;
        }

        #endregion

        #region События запроса получения

        private static void ResponseToApi(Response response, Action<ApiResponse>? handler)
        {
            if (handler == null)
            {
                return;
            }

            ApiResponse result = new(response.Exception)
            {
                Code = response.Code,
                Text = response.Text
            };

            if (!handler.TryInvoke(result, out var exception))
            {
                Log.Error(exception);
            }
        }

        #endregion

        #region Обработка

        /// <summary>
        /// Конвертировать исключение в <see cref="ApiResponse"/>
        /// </summary>
        /// <param name="error">Исключение</param>
        /// <param name="fail">Действие после получение результата конвертации</param>
        public static void ExceptionToApiResponse(Exception error, Action<ApiResponse>? fail)
        {
            ApiResponse result = ExceptionToApiResponse(error);

            if (fail == null)
            {
                return;
            }
            if (!fail.TryInvoke(result, out Exception? userError))
            {
                Log.Error(userError);
            }
        }
        /// <summary>
        /// Конвертировать исключение в <see cref="ApiResponse"/>
        /// </summary>
        /// <param name="error">Исключение</param>
        /// <returns><see cref="ApiResponse"/> с информацией об исключении</returns>
        public static ApiResponse ExceptionToApiResponse(Exception? error)
        {
            return ExceptionToApiResponse<ApiResponse>(error);
        }

        /// <summary>
        /// Получить исключение как ответ
        /// </summary>
        /// <typeparam name="T">Тип ответа</typeparam>
        /// <param name="error">Исключение, которое надо получить как ответ</param>
        /// <returns>Ответ с информацией об исключении (если доступно)</returns>
        public static ApiResponse ExceptionToResponse<T>(Exception? error)
        {
            if (typeof(ApiResponse).IsAssignableFrom(typeof(T)))
            {
                return ExceptionToApiResponse<ApiResponse>(error);
            }

            return new();
        }
        /// <summary>
        /// Конвертировать исключение в <see cref="ApiResponse"/>
        /// </summary>
        /// <param name="error">Конвертируемое исключение</param>
        /// <returns><see cref="ApiResponse"/> с информацией об исключении</returns>
        public static T ExceptionToApiResponse<T>(Exception? error)
            where T : ApiResponse, new()
        {
            if (error == null)
            {
                return new();
            }

            if (error is JsonRequestException jsonException)
            {
                return new()
                {
                    Status = ApiResponseStatus.Error,
                    Code = jsonException.Code,
                    Text = jsonException.Response
                };
            }
            else if (error is RequestException requestException)
            {
                return new()
                {
                    Status = ApiResponseStatus.Error,
                    Code = requestException.Code.ToString()
                };
            }

            return new()
            {
                Status = ApiResponseStatus.Error,
                Code = error.GetType().Name,
                Text = error.Message
            };
        }

        #endregion
    }
}