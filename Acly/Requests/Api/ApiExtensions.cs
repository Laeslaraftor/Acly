using Acly.Tasks;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Acly.Requests
{
    /// <summary>
    /// Класс с методами расширения для api
    /// </summary>
    public static class ApiExtensions
    {
        extension(Api.ServerBase server)
        {
            #region POST запросы

            /// <summary>
            /// Сделать POST запрос и получить ответ в виде JSON
            /// </summary>
            /// <typeparam name="T">Тип ответа</typeparam>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="content">Содержимое для отправки на сервер</param>
            /// <returns>Ответ сервера, конвертированный в указанный тип</returns>
            public IAsyncTask<T> Post<T>(string filename, object? content)
                where T : new()
            {
                return server.Post<T>(filename, content, null, true);
            }
            /// <summary>
            /// Сделать POST запрос и получить ответ в виде JSON
            /// </summary>
            /// <typeparam name="T">Тип ответа</typeparam>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="content">Содержимое для отправки на сервер</param>
			/// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="Api.ServerBase.BaseFileExtension"/>)</param>
            /// <returns>Ответ сервера, конвертированный в указанный тип</returns>
            public IAsyncTask<T> Post<T>(string filename, object? content, bool useBaseFileExtension)
                where T : new()
            {
                return server.Post<T>(filename, content, null, useBaseFileExtension);
            }
            /// <summary>
            /// Сделать POST запрос и получить ответ в виде JSON
            /// </summary>
            /// <typeparam name="T">Тип ответа</typeparam>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="content">Содержимое для отправки на сервер</param>
            /// <param name="parameters">Параметры запроса</param>
            /// <returns>Ответ сервера, конвертированный в указанный тип</returns>
            public IAsyncTask<T> Post<T>(string filename, object? content, string? parameters)
                where T : new()
            {
                return server.Post<T>(filename, content, parameters, true);
            }

            /// <summary>
            /// Сделать POST запрос и гарантированно получить ответ в виде JSON
            /// </summary>
            /// <typeparam name="T">Тип ответа</typeparam>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="content">Содержимое для отправки на сервер</param>
            /// <returns>Ответ сервера, конвертированный в указанный тип</returns>
            public async Task<T> PostSafe<T>(string filename, object? content)
                where T : new()
            {
                return await server.PostSafe<T>(filename, content, null, true);
            }
            /// <summary>
            /// Сделать POST запрос и гарантированно получить ответ в виде JSON
            /// </summary>
            /// <typeparam name="T">Тип ответа</typeparam>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="content">Содержимое для отправки на сервер</param>
			/// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="Api.ServerBase.BaseFileExtension"/>)</param>
            /// <returns>Ответ сервера, конвертированный в указанный тип</returns>
            public async Task<T> PostSafe<T>(string filename, object? content, bool useBaseFileExtension)
                where T : new()
            {
                return await server.PostSafe<T>(filename, content, null, useBaseFileExtension);
            }
            /// <summary>
            /// Сделать POST запрос и гарантированно получить ответ в виде JSON
            /// </summary>
            /// <typeparam name="T">Тип ответа</typeparam>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="content">Содержимое для отправки на сервер</param>
            /// <param name="parameters">Параметры запроса</param>
            /// <returns>Ответ сервера, конвертированный в указанный тип</returns>
            public async Task<T> PostSafe<T>(string filename, object? content, string? parameters)
                where T : new()
            {
                return await server.PostSafe<T>(filename, content, parameters, true);
            }
            /// <summary>
            /// Сделать POST запрос и гарантированно получить ответ в виде JSON
            /// </summary>
            /// <typeparam name="T">Тип ответа</typeparam>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="content">Содержимое для отправки на сервер</param>
            /// <param name="parameters">Параметры запроса</param>
            /// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="Api.ServerBase.BaseFileExtension"/>)</param>
            /// <returns>Ответ сервера, конвертированный в указанный тип</returns>
            public async Task<T> PostSafe<T>(string filename, object? content, string? parameters, bool useBaseFileExtension)
                where T : new()
            {
                var result = await server.Post<T>(filename, content, parameters, useBaseFileExtension);

                if (result.Result != null)
                {
                    return result.Result;
                }

                return (T)(object)Ajax.ExceptionToResponse<T>(result.Error?.Exception);
            }

            /// <summary>
            /// Сделать POST запрос и получить ответ в виде JSON
            /// </summary>
            /// <typeparam name="T">Тип ответа</typeparam>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="content">Содержимое для отправки на сервер</param>
            /// <param name="success">Обработчик результата</param>
            /// <param name="error">Обработчик ошибки</param>
            public void Post<T>(string filename, object? content, Action<T>? success, Action<ApiResponse>? error = null)
                where T : new()
            {
                server.Post(filename, content, null, true, success, error);
            }
            /// <summary>
            /// Сделать POST запрос и получить ответ в виде JSON
            /// </summary>
            /// <typeparam name="T">Тип ответа</typeparam>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="content">Содержимое для отправки на сервер</param>
			/// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="Api.ServerBase.BaseFileExtension"/>)</param>
            /// <param name="success">Обработчик результата</param>
            /// <param name="error">Обработчик ошибки</param>
            public void Post<T>(string filename, object? content, bool useBaseFileExtension, Action<T>? success, Action<ApiResponse>? error = null)
                where T : new()
            {
                server.Post(filename, content, null, useBaseFileExtension, success, error);
            }
            /// <summary>
            /// Сделать POST запрос и получить ответ в виде JSON
            /// </summary>
            /// <typeparam name="T">Тип ответа</typeparam>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="content">Содержимое для отправки на сервер</param>
            /// <param name="parameters">Параметры запроса</param>
            /// <param name="success">Обработчик результата</param>
            /// <param name="error">Обработчик ошибки</param>
            public void Post<T>(string filename, object? content, string? parameters, Action<T>? success, Action<ApiResponse>? error = null)
                where T : new()
            {
                server.Post(filename, content, parameters, true, success, error);
            }
            /// <summary>
            /// Сделать POST запрос и получить ответ в виде JSON
            /// </summary>
            /// <typeparam name="T">Тип ответа</typeparam>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="content">Содержимое для отправки на сервер</param>
            /// <param name="parameters">Параметры запроса</param>
            /// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="Api.ServerBase.BaseFileExtension"/>)</param>
            /// <param name="success">Обработчик результата</param>
            /// <param name="error">Обработчик ошибки</param>
            public async void Post<T>(string filename, object? content, string? parameters, bool useBaseFileExtension, Action<T>? success, Action<ApiResponse>? error = null)
                where T : new()
            {
                var result = await server.Post<T>(filename, content, parameters, useBaseFileExtension);
                result.InvokeCallback(success, error);
            }

            /// <summary>
            /// Сделать POST запрос и получить ответ в виде строки
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="content">Содержимое для отправки на сервер</param>
            /// <returns>Ответ сервера в виде строки</returns>
            public IAsyncTask<string> PostString(string filename, object? content)
            {
                return server.PostString(filename, content, null, true);
            }
            /// <summary>
            /// Сделать POST запрос и получить ответ в виде строки
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="content">Содержимое для отправки на сервер</param>
			/// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="Api.ServerBase.BaseFileExtension"/>)</param>
            /// <returns>Ответ сервера в виде строки</returns>
            public IAsyncTask<string> PostString(string filename, object? content, bool useBaseFileExtension)
            {
                return server.PostString(filename, content, null, useBaseFileExtension);
            }
            /// <summary>
            /// Сделать POST запрос и получить ответ в виде строки
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="content">Содержимое для отправки на сервер</param>
			/// <param name="parameters">Параметры запроса</param>
            /// <returns>Ответ сервера в виде строки</returns>
            public IAsyncTask<string> PostString(string filename, object? content, string? parameters)
            {
                return server.PostString(filename, content, parameters, true);
            }

            /// <summary>
            /// Сделать POST запрос и получить ответ в виде строки
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="content">Содержимое для отправки на сервер</param>
            /// <param name="success">Обработчик результата</param>
            /// <param name="error">Обработчик ошибки</param>
            public void PostString(string filename, object? content, Action<string>? success, Action<ApiResponse>? error = null)
            {
                server.PostString(filename, content, null, true, success, error);
            }
            /// <summary>
            /// Сделать POST запрос и получить ответ в виде строки
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="content">Содержимое для отправки на сервер</param>
			/// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="Api.ServerBase.BaseFileExtension"/>)</param>
            /// <param name="success">Обработчик результата</param>
            /// <param name="error">Обработчик ошибки</param>
            public void PostString(string filename, object? content, bool useBaseFileExtension, Action<string>? success, Action<ApiResponse>? error = null)
            {
                server.PostString(filename, content, null, useBaseFileExtension, success, error);
            }
            /// <summary>
            /// Сделать POST запрос и получить ответ в виде строки
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="content">Содержимое для отправки на сервер</param>
			/// <param name="parameters">Параметры запроса</param>
            /// <param name="success">Обработчик результата</param>
            /// <param name="error">Обработчик ошибки</param>
            public void PostString(string filename, object? content, string? parameters, Action<string>? success, Action<ApiResponse>? error = null)
            {
                server.PostString(filename, content, parameters, true, success, error);
            }
            /// <summary>
            /// Сделать POST запрос и получить ответ в виде строки
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="content">Содержимое для отправки на сервер</param>
            /// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="Api.ServerBase.BaseFileExtension"/>)</param>
			/// <param name="parameters">Параметры запроса</param>
            /// <param name="success">Обработчик результата</param>
            /// <param name="error">Обработчик ошибки</param>
            public async void PostString(string filename, object? content, string? parameters, bool useBaseFileExtension, Action<string>? success, Action<ApiResponse>? error = null)
            {
                var result = await server.PostString(filename, content, parameters, useBaseFileExtension);
                result.InvokeCallback(success, error);
            }

            /// <summary>
            /// Сделать POST запрос и получить ответ в виде массива байтов
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="content">Содержимое для отправки на сервер</param>
            /// <returns>Ответ сервера в виде массива байтов</returns>
            public IAsyncTask<byte[]> PostBytes(string filename, object? content)
            {
                return server.PostBytes(filename, content, null, true);
            }
            /// <summary>
            /// Сделать POST запрос и получить ответ в виде массива байтов
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="content">Содержимое для отправки на сервер</param>
			/// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="Api.ServerBase.BaseFileExtension"/>)</param>
            /// <returns>Ответ сервера в виде массива байтов</returns>
            public IAsyncTask<byte[]> PostBytes(string filename, object? content, bool useBaseFileExtension)
            {
                return server.PostBytes(filename, content, null, useBaseFileExtension);
            }
            /// <summary>
            /// Сделать POST запрос и получить ответ в виде массива байтов
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="content">Содержимое для отправки на сервер</param>
			/// <param name="parameters">Параметры запроса</param>
            /// <returns>Ответ сервера в виде массива байтов</returns>
            public IAsyncTask<byte[]> PostBytes(string filename, object? content, string? parameters)
            {
                return server.PostBytes(filename, content, parameters, true);
            }

            /// <summary>
            /// Сделать POST запрос и получить ответ в виде массива байтов
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="content">Содержимое для отправки на сервер</param>
            /// <param name="success">Обработчик результата</param>
            /// <param name="error">Обработчик ошибки</param>
            public void PostBytes(string filename, object? content, Action<byte[]>? success, Action<ApiResponse>? error = null)
            {
                server.PostBytes(filename, content, null, true, success, error);
            }
            /// <summary>
            /// Сделать POST запрос и получить ответ в виде массива байтов
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="content">Содержимое для отправки на сервер</param>
			/// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="Api.ServerBase.BaseFileExtension"/>)</param>
            /// <param name="success">Обработчик результата</param>
            /// <param name="error">Обработчик ошибки</param>
            public void PostBytes(string filename, object? content, bool useBaseFileExtension, Action<byte[]>? success, Action<ApiResponse>? error = null)
            {
                server.PostBytes(filename, content, null, useBaseFileExtension, success, error);
            }
            /// <summary>
            /// Сделать POST запрос и получить ответ в виде массива байтов
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="content">Содержимое для отправки на сервер</param>
			/// <param name="parameters">Параметры запроса</param>
            /// <param name="success">Обработчик результата</param>
            /// <param name="error">Обработчик ошибки</param>
            public void PostBytes(string filename, object? content, string? parameters, Action<byte[]>? success, Action<ApiResponse>? error = null)
            {
                server.PostBytes(filename, content, parameters, true, success, error);
            }
            /// <summary>
            /// Сделать POST запрос и получить ответ в виде массива байтов
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="content">Содержимое для отправки на сервер</param>
            /// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="Api.ServerBase.BaseFileExtension"/>)</param>
			/// <param name="parameters">Параметры запроса</param>
            /// <param name="success">Обработчик результата</param>
            /// <param name="error">Обработчик ошибки</param>
            public async void PostBytes(string filename, object? content, string? parameters, bool useBaseFileExtension, Action<byte[]>? success, Action<ApiResponse>? error = null)
            {
                var result = await server.PostBytes(filename, content, parameters, useBaseFileExtension);
                result.InvokeCallback(success, error);
            }

            /// <summary>
            /// Сделать POST запрос и получить ответ в виде сырых данных
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="content">Содержимое для отправки на сервер</param>
            /// <param name="destination">Поток в который будут записаны загруженные данные</param>
            public IAsyncTask PostDownload(string filename, object? content, Stream destination)
            {
                return server.PostDownload(filename, content, destination, null, true);
            }
            /// <summary>
            /// Сделать POST запрос и получить ответ в виде сырых данных
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="content">Содержимое для отправки на сервер</param>
            /// <param name="destination">Поток в который будут записаны загруженные данные</param>
			/// <param name="parameters">Параметры запроса</param>
            public IAsyncTask PostDownload(string filename, object? content, Stream destination, string? parameters)
            {
                return server.PostDownload(filename, content, destination, parameters, true);
            }
            /// <summary>
            /// Сделать POST запрос и получить ответ в виде сырых данных
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="content">Содержимое для отправки на сервер</param>
            /// <param name="destination">Поток в который будут записаны загруженные данные</param>
			/// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="Api.ServerBase.BaseFileExtension"/>)</param>
            public IAsyncTask PostDownload(string filename, object? content, Stream destination, bool useBaseFileExtension)
            {
                return server.PostDownload(filename, content, destination, null, useBaseFileExtension);
            }

            /// <summary>
            /// Сделать POST запрос и записать ответ в файл
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="content">Содержимое для отправки на сервер</param>
            /// <param name="filePath">Путь к файлу в который будут записаны полученные данные. 
            /// Если файл не существует, то он будет создан. Если файл уже существует, то он будет удалён и создан новый.</param>
            public IAsyncTask PostDownload(string filename, object? content, string filePath)
            {
                return server.PostDownload(filename, content, filePath, null, true);
            }
            /// <summary>
            /// Сделать POST запрос и записать ответ в файл
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="content">Содержимое для отправки на сервер</param>
            /// <param name="filePath">Путь к файлу в который будут записаны полученные данные. 
            /// Если файл не существует, то он будет создан. Если файл уже существует, то он будет удалён и создан новый.</param>
            /// <param name="parameters">Параметры запроса</param>
            public IAsyncTask PostDownload(string filename, object? content, string filePath, string? parameters)
            {
                return server.PostDownload(filename, content, filePath, parameters, true);
            }
            /// <summary>
            /// Сделать POST запрос и записать ответ в файл
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="content">Содержимое для отправки на сервер</param>
            /// <param name="filePath">Путь к файлу в который будут записаны полученные данные. 
            /// Если файл не существует, то он будет создан. Если файл уже существует, то он будет удалён и создан новый.</param>
            /// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="Api.ServerBase.BaseFileExtension"/>)</param>
            public IAsyncTask PostDownload(string filename, object? content, string filePath, bool useBaseFileExtension)
            {
                return server.PostDownload(filename, content, filePath, null, useBaseFileExtension);
            }
            /// <summary>
            /// Сделать POST запрос и записать ответ в файл
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="content">Содержимое для отправки на сервер</param>
            /// <param name="filePath">Путь к файлу в который будут записаны полученные данные. 
            /// Если файл не существует, то он будет создан. Если файл уже существует, то он будет удалён и создан новый.</param>
			/// <param name="parameters">Параметры запроса</param>
			/// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="Api.ServerBase.BaseFileExtension"/>)</param>
            public IAsyncTask PostDownload(string filename, object? content, string filePath, string? parameters, bool useBaseFileExtension)
            {
                using var file = Api.ServerBase.UseFile(filePath);
                return server.PostDownload(filename, content, file, parameters, useBaseFileExtension);
            }

            /// <summary>
            /// Сделать POST запрос и получить ответ в виде сырых данных
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="content">Содержимое для отправки на сервер</param>
            /// <param name="destination">Поток в который будут записаны загруженные данные</param>
            /// <param name="success">Обработчик результата</param>
            /// <param name="progress">Обработчик прогресса загрузки</param>
            /// <param name="error">Обработчик ошибки</param>
            public IAsyncTask PostDownload(string filename, object? content, Stream destination, Action? success, AsyncTaskProgress? progress, Action<Response>? error = null)
            {
                return server.PostDownload(filename, content, destination, null, true, success, progress, error);
            }
            /// <summary>
            /// Сделать POST запрос и получить ответ в виде сырых данных
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="content">Содержимое для отправки на сервер</param>
            /// <param name="destination">Поток в который будут записаны загруженные данные</param>
            /// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="Api.ServerBase.BaseFileExtension"/>)</param>
            /// <param name="success">Обработчик результата</param>
            /// <param name="progress">Обработчик прогресса загрузки</param>
            /// <param name="error">Обработчик ошибки</param>
            public IAsyncTask PostDownload(string filename, object? content, Stream destination, bool useBaseFileExtension, Action? success, AsyncTaskProgress? progress, Action<Response>? error = null)
            {
                return server.PostDownload(filename, content, destination, null, useBaseFileExtension, success, progress, error);
            }
            /// <summary>
            /// Сделать POST запрос и получить ответ в виде сырых данных
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="content">Содержимое для отправки на сервер</param>
            /// <param name="destination">Поток в который будут записаны загруженные данные</param>
            /// <param name="parameters">Параметры запроса</param>
            /// <param name="success">Обработчик результата</param>
            /// <param name="progress">Обработчик прогресса загрузки</param>
            /// <param name="error">Обработчик ошибки</param>
            public IAsyncTask PostDownload(string filename, object? content, Stream destination, string? parameters, Action? success, AsyncTaskProgress? progress, Action<Response>? error = null)
            {
                return server.PostDownload(filename, content, destination, parameters, true, success, progress, error);
            }
            /// <summary>
            /// Сделать POST запрос и получить ответ в виде сырых данных
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="content">Содержимое для отправки на сервер</param>
            /// <param name="destination">Поток в который будут записаны загруженные данные</param>
            /// <param name="parameters">Параметры запроса</param>
            /// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="Api.ServerBase.BaseFileExtension"/>)</param>
            /// <param name="success">Обработчик результата</param>
            /// <param name="progress">Обработчик прогресса загрузки</param>
            /// <param name="error">Обработчик ошибки</param>
            public IAsyncTask PostDownload(string filename, object? content, Stream destination, string? parameters, bool useBaseFileExtension, Action? success, AsyncTaskProgress? progress, Action<Response>? error = null)
            {
                var downloading = server.PostDownload(filename, content, destination, parameters, useBaseFileExtension);
                server.HandleAsyncTask(downloading, success, progress, error);

                return downloading;
            }

            /// <summary>
            /// Сделать POST запрос и записать ответ в файл
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="content">Содержимое для отправки на сервер</param>
            /// <param name="filePath">Путь к файлу в который будут записаны полученные данные. 
            /// Если файл не существует, то он будет создан. Если файл уже существует, то он будет удалён и создан новый.</param>
            /// <param name="success">Обработчик результата</param>
            /// <param name="progress">Обработчик прогресса загрузки</param>
            /// <param name="error">Обработчик ошибки</param>
            public IAsyncTask PostDownload(string filename, object? content, string filePath, Action? success, AsyncTaskProgress? progress, Action<Response>? error = null)
            {
                return server.PostDownload(filename, content, filePath, null, true, success, progress, error);
            }
            /// <summary>
            /// Сделать POST запрос и записать ответ в файл
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="content">Содержимое для отправки на сервер</param>
            /// <param name="filePath">Путь к файлу в который будут записаны полученные данные. 
            /// Если файл не существует, то он будет создан. Если файл уже существует, то он будет удалён и создан новый.</param>
            /// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="Api.ServerBase.BaseFileExtension"/>)</param>
            /// <param name="success">Обработчик результата</param>
            /// <param name="progress">Обработчик прогресса загрузки</param>
            /// <param name="error">Обработчик ошибки</param>
            public IAsyncTask PostDownload(string filename, object? content, string filePath, bool useBaseFileExtension, Action? success, AsyncTaskProgress? progress, Action<Response>? error = null)
            {
                return server.PostDownload(filename, content, filePath, null, useBaseFileExtension, success, progress, error);
            }
            /// <summary>
            /// Сделать POST запрос и записать ответ в файл
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="content">Содержимое для отправки на сервер</param>
            /// <param name="filePath">Путь к файлу в который будут записаны полученные данные. 
            /// Если файл не существует, то он будет создан. Если файл уже существует, то он будет удалён и создан новый.</param>
            /// <param name="parameters">Параметры запроса</param>
            /// <param name="success">Обработчик результата</param>
            /// <param name="progress">Обработчик прогресса загрузки</param>
            /// <param name="error">Обработчик ошибки</param>
            public IAsyncTask PostDownload(string filename, object? content, string filePath, string? parameters, Action? success, AsyncTaskProgress? progress, Action<Response>? error = null)
            {
                return server.PostDownload(filename, content, filePath, parameters, true, success, progress, error);
            }
            /// <summary>
            /// Сделать POST запрос и записать ответ в файл
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="content">Содержимое для отправки на сервер</param>
            /// <param name="filePath">Путь к файлу в который будут записаны полученные данные. 
            /// Если файл не существует, то он будет создан. Если файл уже существует, то он будет удалён и создан новый.</param>
            /// <param name="parameters">Параметры запроса</param>
            /// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="Api.ServerBase.BaseFileExtension"/>)</param>
            /// <param name="success">Обработчик результата</param>
            /// <param name="progress">Обработчик прогресса загрузки</param>
            /// <param name="error">Обработчик ошибки</param>
            public IAsyncTask PostDownload(string filename, object? content, string filePath, string? parameters, bool useBaseFileExtension, Action? success, AsyncTaskProgress? progress, Action<Response>? error = null)
            {
                using var file = Api.ServerBase.UseFile(filePath);
                return server.PostDownload(filename, content, file, parameters, useBaseFileExtension, success, progress, error);
            }

            #endregion

            #region GET запросы

            /// <summary>
            /// Сделать GET запрос и получить ответ в виде JSON
            /// </summary>
            /// <typeparam name="T">Тип ответа</typeparam>
            /// <param name="filename">Название файла на сервере</param>
            /// <returns>Ответ сервера, конвертированный в указанный тип</returns>
            public IAsyncTask<T> Get<T>(string filename)
                where T : new()
            {
                return server.Get<T>(filename, null, true);
            }
            /// <summary>
            /// Сделать GET запрос и получить ответ в виде JSON
            /// </summary>
            /// <typeparam name="T">Тип ответа</typeparam>
            /// <param name="filename">Название файла на сервере</param>
			/// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="Api.ServerBase.BaseFileExtension"/>)</param>
            /// <returns>Ответ сервера, конвертированный в указанный тип</returns>
            public IAsyncTask<T> Get<T>(string filename, bool useBaseFileExtension)
                where T : new()
            {
                return server.Get<T>(filename, null, useBaseFileExtension);
            }
            /// <summary>
            /// Сделать GET запрос и получить ответ в виде JSON
            /// </summary>
            /// <typeparam name="T">Тип ответа</typeparam>
            /// <param name="filename">Название файла на сервере</param>
			/// <param name="parameters">Параметры запроса</param>
            /// <returns>Ответ сервера, конвертированный в указанный тип</returns>
            public IAsyncTask<T> Get<T>(string filename, string? parameters)
                where T : new()
            {
                return server.Get<T>(filename, parameters, true);
            }

            /// <summary>
            /// Сделать GET запрос и гарантированно получить ответ в виде JSON
            /// </summary>
            /// <typeparam name="T">Тип ответа</typeparam>
            /// <param name="filename">Название файла на сервере</param>
            /// <returns>Ответ сервера, конвертированный в указанный тип</returns>
            public async Task<T> GetSafe<T>(string filename)
                where T : new()
            {
                return await server.GetSafe<T>(filename, null, true);
            }
            /// <summary>
            /// Сделать GET запрос и гарантированно получить ответ в виде JSON
            /// </summary>
            /// <typeparam name="T">Тип ответа</typeparam>
            /// <param name="filename">Название файла на сервере</param>
			/// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="Api.ServerBase.BaseFileExtension"/>)</param>
            /// <returns>Ответ сервера, конвертированный в указанный тип</returns>
            public async Task<T> GetSafe<T>(string filename, bool useBaseFileExtension)
                where T : new()
            {
                return await server.GetSafe<T>(filename, null, useBaseFileExtension);
            }
            /// <summary>
            /// Сделать GET запрос и гарантированно получить ответ в виде JSON
            /// </summary>
            /// <typeparam name="T">Тип ответа</typeparam>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="parameters">Параметры запроса</param>
            /// <returns>Ответ сервера, конвертированный в указанный тип</returns>
            public async Task<T> GetSafe<T>(string filename, string? parameters)
                where T : new()
            {
                return await server.GetSafe<T>(filename, parameters, true);
            }
            /// <summary>
            /// Сделать GET запрос и гарантированно получить ответ в виде JSON
            /// </summary>
            /// <typeparam name="T">Тип ответа</typeparam>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="parameters">Параметры запроса</param>
            /// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="Api.ServerBase.BaseFileExtension"/>)</param>
            /// <returns>Ответ сервера, конвертированный в указанный тип</returns>
            public async Task<T> GetSafe<T>(string filename, string? parameters, bool useBaseFileExtension)
                where T : new()
            {
                var result = await server.Get<T>(filename, parameters, useBaseFileExtension);

                if (result.Result != null)
                {
                    return result.Result;
                }

                return (T)(object)Ajax.ExceptionToResponse<T>(result.Error?.Exception);
            }

            /// <summary>
            /// Сделать GET запрос и получить ответ в виде JSON
            /// </summary>
            /// <typeparam name="T">Тип ответа</typeparam>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="success">Обработчик результата</param>
            /// <param name="error">Обработчик ошибки</param>
            public void Get<T>(string filename, Action<T>? success, Action<ApiResponse>? error = null)
                where T : new()
            {
                server.Get(filename, null, true, success, error);
            }
            /// <summary>
            /// Сделать GET запрос и получить ответ в виде JSON
            /// </summary>
            /// <typeparam name="T">Тип ответа</typeparam>
            /// <param name="filename">Название файла на сервере</param>
			/// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="Api.ServerBase.BaseFileExtension"/>)</param>
            /// <param name="success">Обработчик результата</param>
            /// <param name="error">Обработчик ошибки</param>
            public void Get<T>(string filename, bool useBaseFileExtension, Action<T>? success, Action<ApiResponse>? error = null)
                where T : new()
            {
                server.Get(filename, null, useBaseFileExtension, success, error);
            }
            /// <summary>
            /// Сделать GET запрос и получить ответ в виде JSON
            /// </summary>
            /// <typeparam name="T">Тип ответа</typeparam>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="parameters">Параметры запроса</param>
            /// <param name="success">Обработчик результата</param>
            /// <param name="error">Обработчик ошибки</param>
            public void Get<T>(string filename, string? parameters, Action<T>? success, Action<ApiResponse>? error = null)
                where T : new()
            {
                server.Get(filename, parameters, true, success, error);
            }
            /// <summary>
            /// Сделать GET запрос и получить ответ в виде JSON
            /// </summary>
            /// <typeparam name="T">Тип ответа</typeparam>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="parameters">Параметры запроса</param>
            /// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="Api.ServerBase.BaseFileExtension"/>)</param>
            /// <param name="success">Обработчик результата</param>
            /// <param name="error">Обработчик ошибки</param>
            public async void Get<T>(string filename, string? parameters, bool useBaseFileExtension, Action<T>? success, Action<ApiResponse>? error = null)
                where T : new()
            {
                var result = await server.Get<T>(filename, parameters, useBaseFileExtension);
                result.InvokeCallback(success, error);
            }

            /// <summary>
            /// Сделать GET запрос и получить ответ в виде строки
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <returns>Ответ сервера в виде строки</returns>
            public IAsyncTask<string> GetString(string filename)
            {
                return server.GetString(filename, null, true);
            }
            /// <summary>
            /// Сделать GET запрос и получить ответ в виде строки
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
			/// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="Api.ServerBase.BaseFileExtension"/>)</param>
            /// <returns>Ответ сервера в виде строки</returns>
            public IAsyncTask<string> GetString(string filename, bool useBaseFileExtension)
            {
                return server.GetString(filename, null, useBaseFileExtension);
            }
            /// <summary>
            /// Сделать GET запрос и получить ответ в виде строки
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
			/// <param name="parameters">Параметры запроса</param>
            /// <returns>Ответ сервера в виде строки</returns>
            public IAsyncTask<string> GetString(string filename, string? parameters)
            {
                return server.GetString(filename, parameters, true);
            }

            /// <summary>
            /// Сделать GET запрос и получить ответ в виде строки
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="success">Обработчик результата</param>
            /// <param name="error">Обработчик ошибки</param>
            public void GetString(string filename, Action<string>? success, Action<ApiResponse>? error = null)
            {
                server.GetString(filename, null, true, success, error);
            }
            /// <summary>
            /// Сделать GET запрос и получить ответ в виде строки
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
			/// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="Api.ServerBase.BaseFileExtension"/>)</param>
            /// <param name="success">Обработчик результата</param>
            /// <param name="error">Обработчик ошибки</param>
            public void GetString(string filename, bool useBaseFileExtension, Action<string>? success, Action<ApiResponse>? error = null)
            {
                server.GetString(filename, null, useBaseFileExtension, success, error);
            }
            /// <summary>
            /// Сделать GET запрос и получить ответ в виде строки
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
			/// <param name="parameters">Параметры запроса</param>
            /// <param name="success">Обработчик результата</param>
            /// <param name="error">Обработчик ошибки</param>
            public void GetString(string filename, string? parameters, Action<string>? success, Action<ApiResponse>? error = null)
            {
                server.GetString(filename, parameters, true, success, error);
            }
            /// <summary>
            /// Сделать GET запрос и получить ответ в виде строки
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="Api.ServerBase.BaseFileExtension"/>)</param>
			/// <param name="parameters">Параметры запроса</param>
            /// <param name="success">Обработчик результата</param>
            /// <param name="error">Обработчик ошибки</param>
            public async void GetString(string filename, string? parameters, bool useBaseFileExtension, Action<string>? success, Action<ApiResponse>? error = null)
            {
                var result = await server.GetString(filename, parameters, useBaseFileExtension);
                result.InvokeCallback(success, error);
            }

            /// <summary>
            /// Сделать GET запрос и получить ответ в виде массива байтов
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <returns>Ответ сервера в виде массива байтов</returns>
            public IAsyncTask<byte[]> GetBytes(string filename)
            {
                return server.GetBytes(filename, null, true);
            }
            /// <summary>
            /// Сделать GET запрос и получить ответ в виде массива байтов
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
			/// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="Api.ServerBase.BaseFileExtension"/>)</param>
            /// <returns>Ответ сервера в виде массива байтов</returns>
            public IAsyncTask<byte[]> GetBytes(string filename, bool useBaseFileExtension)
            {
                return server.GetBytes(filename, null, useBaseFileExtension);
            }
            /// <summary>
            /// Сделать GET запрос и получить ответ в виде массива байтов
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
			/// <param name="parameters">Параметры запроса</param>
            /// <returns>Ответ сервера в виде массива байтов</returns>
            public IAsyncTask<byte[]> GetBytes(string filename, string? parameters)
            {
                return server.GetBytes(filename, parameters, true);
            }

            /// <summary>
            /// Сделать GET запрос и получить ответ в виде массива байтов
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="success">Обработчик результата</param>
            /// <param name="error">Обработчик ошибки</param>
            public void GetBytes(string filename, Action<byte[]>? success, Action<ApiResponse>? error = null)
            {
                server.GetBytes(filename, null, true, success, error);
            }
            /// <summary>
            /// Сделать GET запрос и получить ответ в виде массива байтов
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
			/// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="Api.ServerBase.BaseFileExtension"/>)</param>
            /// <param name="success">Обработчик результата</param>
            /// <param name="error">Обработчик ошибки</param>
            public void GetBytes(string filename, bool useBaseFileExtension, Action<byte[]>? success, Action<ApiResponse>? error = null)
            {
                server.GetBytes(filename, null, useBaseFileExtension, success, error);
            }
            /// <summary>
            /// Сделать GET запрос и получить ответ в виде массива байтов
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
			/// <param name="parameters">Параметры запроса</param>
            /// <param name="success">Обработчик результата</param>
            /// <param name="error">Обработчик ошибки</param>
            public void GetBytes(string filename, string? parameters, Action<byte[]>? success, Action<ApiResponse>? error = null)
            {
                server.GetBytes(filename, parameters, true, success, error);
            }
            /// <summary>
            /// Сделать GET запрос и получить ответ в виде массива байтов
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="Api.ServerBase.BaseFileExtension"/>)</param>
			/// <param name="parameters">Параметры запроса</param>
            /// <param name="success">Обработчик результата</param>
            /// <param name="error">Обработчик ошибки</param>
            public async void GetBytes(string filename, string? parameters, bool useBaseFileExtension, Action<byte[]>? success, Action<ApiResponse>? error = null)
            {
                var result = await server.GetBytes(filename, parameters, useBaseFileExtension);
                result.InvokeCallback(success, error);
            }

            /// <summary>
            /// Сделать GET запрос и получить ответ в виде сырых данных
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="destination">Поток в который будут записаны загруженные данные</param>
            public IAsyncTask GetDownload(string filename, Stream destination)
            {
                return server.GetDownload(filename, destination, null, true);
            }
            /// <summary>
            /// Сделать GET запрос и получить ответ в виде сырых данных
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="destination">Поток в который будут записаны загруженные данные</param>
			/// <param name="parameters">Параметры запроса</param>
            public IAsyncTask GetDownload(string filename, Stream destination, string? parameters)
            {
                return server.GetDownload(filename, destination, parameters, true);
            }
            /// <summary>
            /// Сделать GET запрос и получить ответ в виде сырых данных
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="destination">Поток в который будут записаны загруженные данные</param>
			/// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="Api.ServerBase.BaseFileExtension"/>)</param>
            public IAsyncTask GetDownload(string filename, Stream destination, bool useBaseFileExtension)
            {
                return server.GetDownload(filename, destination, null, useBaseFileExtension);
            }

            /// <summary>
            /// Сделать GET запрос и записать ответ в файл
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="filePath">Путь к файлу в который будут записаны полученные данные. 
            /// Если файл не существует, то он будет создан. Если файл уже существует, то он будет удалён и создан новый.</param>
            public IAsyncTask GetDownload(string filename, string filePath)
            {
                return server.GetDownload(filename, filePath, null, true);
            }
            /// <summary>
            /// Сделать GET запрос и записать ответ в файл
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="filePath">Путь к файлу в который будут записаны полученные данные. 
            /// Если файл не существует, то он будет создан. Если файл уже существует, то он будет удалён и создан новый.</param>
            /// <param name="parameters">Параметры запроса</param>
            public IAsyncTask GetDownload(string filename, string filePath, string? parameters)
            {
                return server.GetDownload(filename, filePath, parameters, true);
            }
            /// <summary>
            /// Сделать GET запрос и записать ответ в файл
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="filePath">Путь к файлу в который будут записаны полученные данные. 
            /// Если файл не существует, то он будет создан. Если файл уже существует, то он будет удалён и создан новый.</param>
            /// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="Api.ServerBase.BaseFileExtension"/>)</param>
            public IAsyncTask GetDownload(string filename, string filePath, bool useBaseFileExtension)
            {
                return server.GetDownload(filename, filePath, null, useBaseFileExtension);
            }
            /// <summary>
            /// Сделать GET запрос и записать ответ в файл
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="filePath">Путь к файлу в который будут записаны полученные данные. 
            /// Если файл не существует, то он будет создан. Если файл уже существует, то он будет удалён и создан новый.</param>
			/// <param name="parameters">Параметры запроса</param>
			/// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="Api.ServerBase.BaseFileExtension"/>)</param>
            public IAsyncTask GetDownload(string filename, string filePath, string? parameters, bool useBaseFileExtension)
            {
                using var file = Api.ServerBase.UseFile(filePath);
                return server.GetDownload(filename, file, parameters, useBaseFileExtension);
            }

            /// <summary>
            /// Сделать GET запрос и получить ответ в виде сырых данных
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="destination">Поток в который будут записаны загруженные данные</param>
            /// <param name="success">Обработчик результата</param>
            /// <param name="progress">Обработчик прогресса загрузки</param>
            /// <param name="error">Обработчик ошибки</param>
            public IAsyncTask GetDownload(string filename, Stream destination, Action? success, AsyncTaskProgress? progress, Action<Response>? error = null)
            {
                return server.GetDownload(filename, destination, null, true, success, progress, error);
            }
            /// <summary>
            /// Сделать GET запрос и получить ответ в виде сырых данных
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="destination">Поток в который будут записаны загруженные данные</param>
            /// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="Api.ServerBase.BaseFileExtension"/>)</param>
            /// <param name="success">Обработчик результата</param>
            /// <param name="progress">Обработчик прогресса загрузки</param>
            /// <param name="error">Обработчик ошибки</param>
            public IAsyncTask GetDownload(string filename, Stream destination, bool useBaseFileExtension, Action? success, AsyncTaskProgress? progress, Action<Response>? error = null)
            {
                return server.GetDownload(filename, destination, null, useBaseFileExtension, success, progress, error);
            }
            /// <summary>
            /// Сделать GET запрос и получить ответ в виде сырых данных
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="destination">Поток в который будут записаны загруженные данные</param>
            /// <param name="parameters">Параметры запроса</param>
            /// <param name="success">Обработчик результата</param>
            /// <param name="progress">Обработчик прогресса загрузки</param>
            /// <param name="error">Обработчик ошибки</param>
            public IAsyncTask GetDownload(string filename, Stream destination, string? parameters, Action? success, AsyncTaskProgress? progress, Action<Response>? error = null)
            {
                return server.GetDownload(filename, destination, parameters, true, success, progress, error);
            }
            /// <summary>
            /// Сделать GET запрос и получить ответ в виде сырых данных
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="destination">Поток в который будут записаны загруженные данные</param>
            /// <param name="parameters">Параметры запроса</param>
            /// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="Api.ServerBase.BaseFileExtension"/>)</param>
            /// <param name="success">Обработчик результата</param>
            /// <param name="progress">Обработчик прогресса загрузки</param>
            /// <param name="error">Обработчик ошибки</param>
            public IAsyncTask GetDownload(string filename, Stream destination, string? parameters, bool useBaseFileExtension, Action? success, AsyncTaskProgress? progress, Action<Response>? error = null)
            {
                var downloading = server.GetDownload(filename, destination, parameters, useBaseFileExtension);
                server.HandleAsyncTask(downloading, success, progress, error);

                return downloading;
            }

            /// <summary>
            /// Сделать GET запрос и записать ответ в файл
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="filePath">Путь к файлу в который будут записаны полученные данные. 
            /// Если файл не существует, то он будет создан. Если файл уже существует, то он будет удалён и создан новый.</param>
            /// <param name="success">Обработчик результата</param>
            /// <param name="progress">Обработчик прогресса загрузки</param>
            /// <param name="error">Обработчик ошибки</param>
            public IAsyncTask GetDownload(string filename, string filePath, Action? success, AsyncTaskProgress? progress, Action<Response>? error = null)
            {
                return server.GetDownload(filename, filePath, null, true, success, progress, error);
            }
            /// <summary>
            /// Сделать GET запрос и записать ответ в файл
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="filePath">Путь к файлу в который будут записаны полученные данные. 
            /// Если файл не существует, то он будет создан. Если файл уже существует, то он будет удалён и создан новый.</param>
            /// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="Api.ServerBase.BaseFileExtension"/>)</param>
            /// <param name="success">Обработчик результата</param>
            /// <param name="progress">Обработчик прогресса загрузки</param>
            /// <param name="error">Обработчик ошибки</param>
            public IAsyncTask GetDownload(string filename, string filePath, bool useBaseFileExtension, Action? success, AsyncTaskProgress? progress, Action<Response>? error = null)
            {
                return server.GetDownload(filename, filePath, null, useBaseFileExtension, success, progress, error);
            }
            /// <summary>
            /// Сделать GET запрос и записать ответ в файл
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="filePath">Путь к файлу в который будут записаны полученные данные. 
            /// Если файл не существует, то он будет создан. Если файл уже существует, то он будет удалён и создан новый.</param>
            /// <param name="parameters">Параметры запроса</param>
            /// <param name="success">Обработчик результата</param>
            /// <param name="progress">Обработчик прогресса загрузки</param>
            /// <param name="error">Обработчик ошибки</param>
            public IAsyncTask GetDownload(string filename, string filePath, string? parameters, Action? success, AsyncTaskProgress? progress, Action<Response>? error = null)
            {
                return server.GetDownload(filename, filePath, parameters, true, success, progress, error);
            }
            /// <summary>
            /// Сделать GET запрос и записать ответ в файл
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="filePath">Путь к файлу в который будут записаны полученные данные. 
            /// Если файл не существует, то он будет создан. Если файл уже существует, то он будет удалён и создан новый.</param>
            /// <param name="parameters">Параметры запроса</param>
            /// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="Api.ServerBase.BaseFileExtension"/>)</param>
            /// <param name="success">Обработчик результата</param>
            /// <param name="progress">Обработчик прогресса загрузки</param>
            /// <param name="error">Обработчик ошибки</param>
            public IAsyncTask GetDownload(string filename, string filePath, string? parameters, bool useBaseFileExtension, Action? success, AsyncTaskProgress? progress, Action<Response>? error = null)
            {
                using var file = Api.ServerBase.UseFile(filePath);
                return server.GetDownload(filename, file, parameters, useBaseFileExtension, success, progress, error);
            }

            #endregion

            #region Дополнительно

            private void HandleAsyncTask(IAsyncTask task, Action? success, AsyncTaskProgress? progress, Action<Response>? error)
            {
                bool cleared = false;

                if (task.IsCompleted)
                {
                    Clear();
                }

                void Clear()
                {
                    if (cleared)
                    {
                        return;
                    }

                    cleared = true;
                    task.Completed -= OnCompleted;
                    task.Failed -= OnFailed;

                    if (progress != null)
                    {
                        task.ProgressUpdated -= progress;
                    }
                    if (!task.IsCompleted)
                    {
                        return;
                    }
                    if (task.Error != null)
                    {
                        error?.Invoke(task.Error.Response);
                        return;
                    }

                    success?.Invoke();
                }
                void OnCompleted()
                {
                    Clear();
                }
                void OnFailed(IAsyncTaskError error)
                {
                    Clear();
                }

                if (progress != null)
                {
                    task.ProgressUpdated += progress;
                }

                task.Completed += Clear;
                task.Failed += OnFailed;
            }
            private static FileStream UseFile(string filePath)
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                return File.Open(filePath, FileMode.OpenOrCreate);
            }

            #endregion
        }
    }
}
