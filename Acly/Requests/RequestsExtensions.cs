using Acly.JsonData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Acly.Requests
{
    /// <summary>
    /// Методы расширения для запросов
    /// </summary>
#pragma warning disable CA1708 // Идентификаторы должны отличаться не только регистром
    public static class RequestsExtensions
#pragma warning restore CA1708 // Идентификаторы должны отличаться не только регистром
    {
        extension(HttpClient client)
        {
            /// <summary>
            /// Сделать GET запрос и получить строку
            /// </summary>
            /// <param name="uri">Адрес запроса</param>
            /// <param name="progress">Обработчик прогресса запроса</param>
            /// <returns>Ответ запроса</returns>
            public async Task<string> GetString(Uri uri, IProgress<double>? progress)
            {
                using var request = await client.ReadAsync(HttpMethod.Get, uri, progress);
                return request.ReadAsString() ?? string.Empty;
            }
            /// <summary>
            /// Сделать GET запрос по указанному адресу
            /// </summary>
            /// <param name="uri">Адрес запроса</param>
            /// <param name="progress">Обработчик прогресса</param>
            /// <returns>Полученный ответ</returns>
            public async Task<T> GetApi<T>(Uri uri, IProgress<double>? progress)
                where T : new()
            {
                using var request = await client.ReadAsync(HttpMethod.Get, uri, progress);
                return await request.ReadAsApiResponse<T>();
            }
            /// <summary>
            /// Сделать POST запрос и получить строку
            /// </summary>
            /// <param name="uri">Адрес запроса</param>
            /// <param name="content">Содержимое запроса</param>
            /// <param name="progress">Обработчик прогресса запроса</param>
            /// <returns>Ответ запроса</returns>
            public async Task<string> PostString(Uri uri, IProgress<double>? progress, HttpContent? content)
            {
                using var request = await client.ReadAsync(HttpMethod.Post, uri, progress, content);
                return request.ReadAsString() ?? string.Empty;
            }
            /// <summary>
            /// Сделать POST запрос по указанному адресу
            /// </summary>
            /// <param name="uri">Адрес запроса</param>
            /// <param name="progress">Обработчик прогресса</param>ъ
            /// <param name="content">Содержимое запроса</param>
            /// <returns>Полученный ответ</returns>
            public async Task<T> PostApi<T>(Uri uri, IProgress<double>? progress, HttpContent? content)
                where T : new()
            {
                using var request = await client.ReadAsync(HttpMethod.Post, uri, progress, content);
                return await request.ReadAsApiResponse<T>();
            }
            /// <summary>
            /// Прочитать поток с отслеживанием прогресса
            /// </summary>
            /// <param name="method">Тип запроса</param>
            /// <param name="uri">Адрес запроса</param>
            /// <param name="destination">Поток в который будет записан результат запроса</param>
            /// <param name="progress">Обработчик прогресса</param>
            /// <param name="content">Содержимое запроса</param>
            /// <returns>Поток с полученными данными</returns>
            public async Task<HttpClientReadResult> ReadAsync(HttpMethod method, Uri uri, Stream destination, IProgress<double>? progress, HttpContent? content = null)
            {
                using HttpRequestMessage request = new(method, uri)
                {
                    Content = content,
                };

                foreach (var header in client.DefaultRequestHeaders)
                {
                    request.Headers.Add(header.Key, header.Value);
                }

                request.Headers.Authorization = client.DefaultRequestHeaders.Authorization;

                using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                var encoding = response.GetEncoding();
                await response.ReadAsync(destination, progress);

                return new(encoding, destination, response.StatusCode, response.IsSuccessStatusCode);
            }
            /// <summary>
            /// Прочитать поток с отслеживанием прогресса
            /// </summary>
            /// <param name="method">Тип запроса</param>
            /// <param name="uri">Адрес запроса</param>
            /// <param name="progress">Обработчик прогресса</param>
            /// <param name="content">Содержимое запроса</param>
            /// <returns>Поток с полученными данными</returns>
            public async Task<HttpClientReadResult> ReadAsync(HttpMethod method, Uri uri, IProgress<double>? progress, HttpContent? content = null)
            {
                MemoryStream memory = new();
                return await client.ReadAsync(method, uri, memory, progress, content);
            }
        }
        extension(HttpClientReadResult httpClientReadResult)
        {
            /// <summary>
            /// Прочитать HTTP ответ как <see cref="ApiResponse"/>
            /// </summary>
            /// <typeparam name="T">Тип api ответа</typeparam>
            /// <returns>Ответ api</returns>
            public async Task<T> ReadAsApiResponse<T>() where T : new()
            {
                var content = httpClientReadResult.ReadAsString();

                if (!string.IsNullOrEmpty(content))
                {
                    try
                    {
                        return await Json.Convert<T>(content);
                    }
                    catch (Exception error)
                    {
                        Log.Error(error);
                    }
                }

                if (typeof(ApiResponse).IsAssignableFrom(typeof(T)))
                {
                    return (T)(object)new ApiResponse()
                    {
                        Status = httpClientReadResult.IsSuccessStatusCode ? ApiResponseStatus.Success : ApiResponseStatus.Error,
                        Code = httpClientReadResult.StatusCode.ToString(),
                        Text = content ?? string.Empty
                    };
                }

                return new();
            }
        }
        extension(HttpResponseMessage message)
        {
            /// <summary>
            /// Прочитать HTTP ответ 
            /// </summary>
            /// <param name="destination">Поток в который будет записан ответ</param>
            /// <param name="progress">Прогресс чтения ответа</param>
            /// <param name="bufferSize">Размер временного буфера для чтения и записи ответа</param>
            /// <param name="cancellationToken">Токен отмены</param>
            public async Task ReadAsync(Stream destination, IProgress<double>? progress = null, int bufferSize = 4096, CancellationToken? cancellationToken = null)
            {
                var token = cancellationToken == null ? CancellationToken.None : cancellationToken.Value;
                var contentLength = message.Content.Headers.ContentLength;

                using var download = await message.Content.ReadAsStreamAsync();

                if (progress == null || !contentLength.HasValue)
                {
                    await download.CopyToAsync(destination);
                    return;
                }

                var doubleLength = (double)contentLength.Value;
                var relativeProgress = new Progress<long>(totalBytes => progress.Report(totalBytes / doubleLength));
                await download.CopyToAsync(destination, bufferSize, relativeProgress, token);

                progress.Report(1);
            }
            /// <summary>
            /// Получить кодировку HTTP сообщения
            /// </summary>
            /// <returns></returns>
            public Encoding GetEncoding()
            {
                var charset = message.Content.Headers.ContentType?.CharSet;
                return !string.IsNullOrEmpty(charset) ? Encoding.GetEncoding(charset) : Encoding.UTF8;
            }
        }
        extension(HttpContent _)
        {
            /// <summary>
            /// Получить <see cref="HttpContent"/> из объекта
            /// </summary>
            /// <param name="content">Объект из которого надо сделать содержимое http запроса</param>
            /// <returns>Содержимое http запроса</returns>
            public static async Task<HttpContent?> FromObject(object? content)
            {
                if (content == null)
                {
                    return null;
                }
                if (content is byte[] data)
                {
                    return new ByteArrayContent(data);
                }
                else if (content is Stream stream)
                {
                    return new StreamContent(stream);
                }
                else if (content is IEnumerable<KeyValuePair<string, string>> values)
                {
                    return new FormUrlEncodedContent(values);
                }
                else if (content is IEnumerable<KeyValuePair<string, object?>> objectValues)
                {
                    Dictionary<string, string?> textValues = [];

                    foreach (var info in objectValues)
                    {
                        if (info.Value is string str)
                        {
                            textValues.Add(info.Key, str);
                            continue;
                        }
                        else if (info.Value != null)
                        {
                            var jsonValue = await Json.Convert(info.Value);
                            textValues.Add(info.Key, jsonValue);
                            continue;
                        }

                        textValues.Add(info.Key, null);
                    }

                    return new FormUrlEncodedContent(textValues);
                }

                string json;

                if (content is string textContent)
                {
                    json = textContent;
                }
                else
                {
                    json = await Json.Convert(content);
                }

                return new StringContent(json, Encoding.UTF8, Json.MediaType);
            }
        }
        extension(IHttpRequestClient provider)
        {
            /// <summary>
            /// Сделать GET запрос по указанному адресу
            /// </summary>
            /// <param name="url">Адрес запроса</param>
            /// <param name="progress">Обработчик прогресса</param>
            /// <returns>Полученный ответ</returns>
            public async Task<string> Get(string url, IProgress<double>? progress)
            {
                return await provider.Get(new(url, UriKind.Absolute), progress);
            }
            /// <summary>
            /// Сделать GET запрос по указанному адресу
            /// </summary>
            /// <param name="url">Адрес запроса</param>
            /// <param name="progress">Обработчик прогресса</param>
            /// <returns>Полученный ответ</returns>
            public async Task<T> Get<T>(string url, IProgress<double>? progress)
                where T : new()
            {
                return await provider.Get<T>(new(url, UriKind.Absolute), progress);
            }
            /// <summary>
            /// Сделать POST запрос по указанному адресу
            /// </summary>
            /// <param name="url">Адрес запроса</param>
            /// <param name="progress">Обработчик прогресса</param>ъ
            /// <param name="content">Содержимое запроса</param>
            /// <returns>Полученный ответ</returns>
            public async Task<string> Post(string url, IProgress<double>? progress, HttpContent? content)
            {
                return await provider.Post(new(url, UriKind.Absolute), progress, content);
            }
            /// <summary>
            /// Сделать POST запрос по указанному адресу
            /// </summary>
            /// <param name="url">Адрес запроса</param>
            /// <param name="progress">Обработчик прогресса</param>ъ
            /// <param name="content">Содержимое запроса</param>
            /// <returns>Полученный ответ</returns>
            public async Task<string> Post(string url, IProgress<double>? progress, object? content)
            {
                return await provider.Post(new Uri(url, UriKind.Absolute), progress, content);
            }
            /// <summary>
            /// Сделать POST запрос по указанному адресу
            /// </summary>
            /// <param name="url">Адрес запроса</param>
            /// <param name="progress">Обработчик прогресса</param>ъ
            /// <param name="content">Содержимое запроса</param>
            /// <returns>Полученный ответ</returns>
            public async Task<string> Post(Uri url, IProgress<double>? progress, object? content)
            {
                HttpContent? httpContent = null;

                if (content != null)
                {
                    httpContent = await HttpContent.FromObject(content);
                }

                return await provider.Post(url, progress, httpContent);
            }
            /// <summary>
            /// Сделать POST запрос по указанному адресу
            /// </summary>
            /// <param name="url">Адрес запроса</param>
            /// <param name="progress">Обработчик прогресса</param>ъ
            /// <param name="content">Содержимое запроса</param>
            /// <returns>Полученный ответ</returns>
            public async Task<T> Post<T>(string url, IProgress<double>? progress, HttpContent? content)
                where T : new()
            {
                return await provider.Post<T>(new(url, UriKind.Absolute), progress, content);
            }
            /// <summary>
            /// Сделать POST запрос по указанному адресу
            /// </summary>
            /// <param name="url">Адрес запроса</param>
            /// <param name="progress">Обработчик прогресса</param>ъ
            /// <param name="content">Содержимое запроса</param>
            /// <returns>Полученный ответ</returns>
            public async Task<T> Post<T>(string url, IProgress<double>? progress, object? content)
                where T : new()
            {
                return await provider.Post<T>(new Uri(url, UriKind.Absolute), progress, content);
            }
            /// <summary>
            /// Сделать POST запрос по указанному адресу
            /// </summary>
            /// <param name="url">Адрес запроса</param>
            /// <param name="progress">Обработчик прогресса</param>ъ
            /// <param name="content">Содержимое запроса</param>
            /// <returns>Полученный ответ</returns>
            public async Task<T> Post<T>(Uri url, IProgress<double>? progress, object? content)
                where T : new()
            {
                HttpContent? httpContent = null;

                if (content != null)
                {
                    httpContent = await HttpContent.FromObject(content);
                }

                return await provider.Post<T>(url, progress, httpContent);
            }
            /// <summary>
            /// Сделать GET запрос по указанному адресу и скачать данные
            /// </summary>
            /// <param name="url">Адрес запроса</param>
            /// <param name="progress">Обработчик прогресса</param>
            /// <param name="destination">Поток в который будут записываться скаченные данные</param>
            public async Task GetDownload(string url, IProgress<double>? progress, Stream destination)
            {
                await provider.GetDownload(new(url, UriKind.Absolute), progress, destination);
            }
            /// <summary>
            /// Сделать POST запрос по указанному адресу и скачать данные
            /// </summary>
            /// <param name="url">Адрес запроса</param>
            /// <param name="progress">Обработчик прогресса</param>
            /// <param name="content">Содержимое запроса</param>
            /// <param name="destination">Поток в который будут записываться скаченные данные</param>
            public async Task PostDownload(string url, IProgress<double>? progress, HttpContent? content, Stream destination)
            {
                await provider.PostDownload(new(url, UriKind.Absolute), progress, content, destination);
            }
            /// <summary>
            /// Сделать POST запрос по указанному адресу и скачать данные
            /// </summary>
            /// <param name="url">Адрес запроса</param>
            /// <param name="progress">Обработчик прогресса</param>
            /// <param name="content">Содержимое запроса</param>
            /// <param name="destination">Поток в который будут записываться скаченные данные</param>
            public async Task PostDownload(string url, IProgress<double>? progress, object? content, Stream destination)
            {
                await provider.PostDownload(new Uri(url, UriKind.Absolute), progress, content, destination);
            }
            /// <summary>
            /// Сделать POST запрос по указанному адресу и скачать данные
            /// </summary>
            /// <param name="url">Адрес запроса</param>
            /// <param name="progress">Обработчик прогресса</param>
            /// <param name="content">Содержимое запроса</param>
            /// <param name="destination">Поток в который будут записываться скаченные данные</param>
            public async Task PostDownload(Uri url, IProgress<double>? progress, object? content, Stream destination)
            {
                HttpContent? httpContent = null;

                if (content != null)
                {
                    httpContent = await HttpContent.FromObject(content);
                }

                await provider.PostDownload(url, progress, httpContent, destination);
            }
        }
        extension(ISendable sendable)
        {
            /// <summary>
            /// Отправить данные на сервер
            /// </summary>
            /// <param name="data">Данные</param>
            /// <exception cref="ArgumentNullException">Сервер или данные не указаны</exception>
            public void Send(byte[] data)
            {
                if (sendable == null)
                {
                    throw new ArgumentNullException(nameof(sendable), "Сервер не указан");
                }
                if (data == null)
                {
                    throw new ArgumentNullException(nameof(data), "Данные для отправки не указаны");
                }

                sendable.Send(data, 0, data.Length);
            }
            /// <summary>
            /// Отправить сообщение с кодировкой <see cref="Encoding.UTF8"/>
            /// </summary>
            /// <param name="message">Сообщение</param>
            /// <exception cref="ArgumentNullException">Сервер или сообщение не указано</exception>
            public void SendText(string message)
            {
                if (sendable == null)
                {
                    throw new ArgumentNullException(nameof(sendable), "Сервер не указан");
                }
                if (message == null)
                {
                    throw new ArgumentNullException(nameof(message), "Сообщение не указано");
                }

                sendable.SendText(message, Encoding.UTF8);
            }
            /// <summary>
            /// Отправить сообщение с указанной кодировкой
            /// </summary>
            /// <param name="message">Сообщение</param>
            /// <param name="encoding">Кодировка сообщения</param>
            /// <exception cref="ArgumentNullException">Сервер, сообщение, или кодировка не указана</exception>
            public void SendText(string message, Encoding encoding)
            {
                if (sendable == null)
                {
                    throw new ArgumentNullException(nameof(sendable), "Сервер не указан");
                }
                if (message == null)
                {
                    throw new ArgumentNullException(nameof(message), "Сообщение не указано");
                }
                if (encoding == null)
                {
                    throw new ArgumentNullException(nameof(encoding), "Кодировка не указана");
                }

                byte[] data = encoding.GetBytes(message);
                sendable.Send(data, 0, data.Length);
            }
        }
        extension(Socket socket)
        {
            /// <summary>
            /// Отправить асинхронно данные сокету
            /// </summary>
            /// <param name="data">Данные</param>
            /// <param name="offset">Смещение</param>
            /// <param name="length">Размер данных</param>
            /// <exception cref="ArgumentNullException">Сокет не указан</exception>
            public void SendAsync(byte[] data, int offset, int length)
            {
                if (socket == null)
                {
                    throw new ArgumentNullException(nameof(socket), "Сокет не указан");
                }

                using SocketAsyncEventArgs args = new();
                args.SetBuffer(data, offset, length);

                socket.SendAsync(args);
            }
            /// <summary>
            /// Проверить подключен ли сокет к серверу
            /// </summary>
            /// <returns>Подключен ли сокет</returns>
            public bool IsConnected()
            {
                if (socket == null)
                {
                    throw new ArgumentNullException(nameof(socket), "Сокет не указан");
                }

                bool poll = socket.Poll(1000, SelectMode.SelectRead);
                bool data = socket.Available == 0;

                return !(poll && data);
            }
        }
        extension(Socket server)
        {
            /// <summary>
            /// Отправить сообщение с кодировкой <see cref="Encoding.UTF8"/>
            /// </summary>
            /// <param name="message">Сообщение</param>
            /// <exception cref="ArgumentNullException">Сервер или сообщение не указано</exception>
            public int SendText(string message)
            {
                if (server == null)
                {
                    throw new ArgumentNullException(nameof(server), "Сервер не указан");
                }
                if (message == null)
                {
                    throw new ArgumentNullException(nameof(message), "Сообщение не указано");
                }

                return server.SendText(message, Encoding.UTF8);
            }
            /// <summary>
            /// Отправить сообщение с указанной кодировкой
            /// </summary>
            /// <param name="message">Сообщение</param>
            /// <param name="encoding">Кодировка сообщения</param>
            /// <exception cref="ArgumentNullException">Сервер, сообщение, или кодировка не указана</exception>
            public int SendText(string message, Encoding encoding)
            {
                if (server == null)
                {
                    throw new ArgumentNullException(nameof(server), "Сервер не указан");
                }
                if (message == null)
                {
                    throw new ArgumentNullException(nameof(message), "Сообщение не указано");
                }
                if (encoding == null)
                {
                    throw new ArgumentNullException(nameof(encoding), "Кодировка не указана");
                }

                byte[] data = encoding.GetBytes(message);
                return server.Send(data);
            }
        }
    }
}
