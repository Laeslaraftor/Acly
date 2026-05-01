using Acly.Requests.Tasks;
using Acly.Tasks;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Acly.Requests
{
    public partial class Api
    {
        /// <summary>
        /// Http Api сервер
        /// </summary>
        public sealed class Server : ServerBase
        {
            /// <summary>
			/// Создать класс для отправки запросов на сервер
			/// </summary>
            /// <param name="timeout"><inheritdoc cref="IHttpRequestClient.Timeout"/></param>
			public Server(TimeSpan timeout)
                : this(null)
            {
                _httpClientProvider.Timeout = timeout;
            }
            /// <summary>
			/// Создать класс для отправки запросов на сервер
			/// </summary>
            /// <param name="httpClientProvider">Провайдер http клиента</param>
			public Server(IHttpRequestClient? httpClientProvider = null)
                : base()
            {
                _httpClientProvider = httpClientProvider ?? new HttpRequestClient();
            }
            /// <summary>
            /// Создать класс для отправки запросов на сервер
            /// </summary>
            /// <param name="baseUrl">Адрес сервера. Например, https://api.acly.ru</param>
            /// <param name="httpClientProvider">Провайдер http клиента</param>
            public Server(string baseUrl, IHttpRequestClient? httpClientProvider = null)
                : base(baseUrl)
            {
                _httpClientProvider = httpClientProvider ?? new HttpRequestClient();
            }
            /// <summary>
            /// Создать класс для отправки запросов на сервер
            /// </summary>
            /// <param name="baseUrl">Адрес сервера. Например, https://api.acly.ru</param>
            /// <param name="baseFileExtension">
            /// Основной тип файла. Например: php; json; html.
            /// Если у файла нет типа то необходимо оставить это поле пустым или использовать <see cref="Get{T}(string, string, bool)"/> с последним аргументом false
            /// </param>
            /// <param name="httpClientProvider">Провайдер http клиента</param>
            public Server(string baseUrl, string baseFileExtension, IHttpRequestClient? httpClientProvider = null)
                : base(baseUrl, baseFileExtension)
            {
                _httpClientProvider = httpClientProvider ?? new HttpRequestClient();
            }
            /// <summary>
            /// Создать класс для отправки запросов на сервер
            /// </summary>
            /// <param name="baseUrl">Адрес сервера. Например, https://api.acly.ru</param>
            /// <param name="baseFileExtension">
            /// Основной тип файла. Например: php; json; html.
            /// Если у файла нет типа то необходимо оставить это поле пустым или использовать <see cref="Get{T}(string, string, bool)"/> с последним аргументом false
            /// </param>
            /// <param name="baseParameters">
            /// Функция получения параметров, которые будут использоваться в каждом запросе.
            /// Пример выводимых параметров:
            /// <code>param=value&amp;param2=value2</code>
            /// </param>
            /// <param name="httpClientProvider">Провайдер http клиента</param>
            public Server(string baseUrl, string baseFileExtension, Func<Task<string>> baseParameters, IHttpRequestClient? httpClientProvider = null)
                : base(baseUrl, baseFileExtension, baseParameters)
            {
                _httpClientProvider = httpClientProvider ?? new HttpRequestClient();
            }

            private readonly IHttpRequestClient _httpClientProvider;

            #region Управление

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            /// <param name="isDisposing"><inheritdoc/></param>
            protected override void Dispose(bool isDisposing)
            {
                base.Dispose(isDisposing);
                _httpClientProvider.Dispose();
            }

            #endregion

            #region Запросы

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            /// <typeparam name="T"><inheritdoc/></typeparam>
            /// <param name="filename"><inheritdoc/></param>
            /// <param name="content"><inheritdoc/></param>
            /// <param name="parameters"><inheritdoc/></param>
            /// <param name="useBaseFileExtension"><inheritdoc/></param>
            /// <returns><inheritdoc/></returns>
            public override IAsyncTask<T> Post<T>(string filename, object? content, string? parameters, bool useBaseFileExtension)
            {
                return RequestAsyncTaskController<T>.CreateTask(() => new(), async progress =>
                {
                    var url = await GetUrl(filename, parameters, useBaseFileExtension);
                    return await _httpClientProvider.Post<T>(url, progress, content);
                });
            }
            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            /// <param name="filename"><inheritdoc/></param>
            /// <param name="content"><inheritdoc/></param>
            /// <param name="parameters"><inheritdoc/></param>
            /// <param name="useBaseFileExtension"><inheritdoc/></param>
            /// <returns><inheritdoc/></returns>
            public override IAsyncTask<string> PostString(string filename, object? content, string? parameters, bool useBaseFileExtension)
            {
                return RequestAsyncTaskController<string>.CreateTask(string.Empty, async progress =>
                {
                    var url = await GetUrl(filename, parameters, useBaseFileExtension);
                    return await _httpClientProvider.Post(url, progress, content);
                });
            }
            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            /// <param name="filename"><inheritdoc/></param>
            /// <param name="content"><inheritdoc/></param>
            /// <param name="destination"><inheritdoc/></param>
            /// <param name="parameters"><inheritdoc/></param>
            /// <param name="useBaseFileExtension"><inheritdoc/></param>
            /// <returns><inheritdoc/></returns>
            public override IAsyncTask PostDownload(string filename, object? content, Stream destination, string? parameters, bool useBaseFileExtension)
            {
                return RequestAsyncTaskController<bool>.CreateTask(false, async progress =>
                {
                    var url = await GetUrl(filename, parameters, useBaseFileExtension);
                    await _httpClientProvider.PostDownload(url, progress, content, destination);

                    return true;
                });
            }
            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            /// <param name="filename"><inheritdoc/></param>
            /// <param name="content"><inheritdoc/></param>
            /// <param name="parameters"><inheritdoc/></param>
            /// <param name="useBaseFileExtension"><inheritdoc/></param>
            /// <returns><inheritdoc/></returns>
            public override IAsyncTask<byte[]> PostBytes(string filename, object? content, string? parameters, bool useBaseFileExtension)
            {
                return RequestAsyncTaskController<byte[]>.CreateTask(() => [], async progress =>
                {
                    using MemoryStream memory = new();
                    var url = await GetUrl(filename, parameters, useBaseFileExtension);
                    await _httpClientProvider.PostDownload(url, progress, content, memory);

                    return memory.ToArray();
                });
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            /// <typeparam name="T"><inheritdoc/></typeparam>
            /// <param name="filename"><inheritdoc/></param>
            /// <param name="parameters"><inheritdoc/></param>
            /// <param name="useBaseFileExtension"><inheritdoc/></param>
            /// <returns><inheritdoc/></returns>
            public override IAsyncTask<T> Get<T>(string filename, string? parameters, bool useBaseFileExtension)
            {
                return RequestAsyncTaskController<T>.CreateTask(() => new(), async progress =>
                {
                    var url = await GetUrl(filename, parameters, useBaseFileExtension);
                    return await _httpClientProvider.Get<T>(url, progress);
                });
            }
            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            /// <param name="filename"><inheritdoc/></param>
            /// <param name="parameters"><inheritdoc/></param>
            /// <param name="useBaseFileExtension"><inheritdoc/></param>
            /// <returns><inheritdoc/></returns>
            public override IAsyncTask<string> GetString(string filename, string? parameters, bool useBaseFileExtension)
            {
                return RequestAsyncTaskController<string>.CreateTask(string.Empty, async progress =>
                {
                    var url = await GetUrl(filename, parameters, useBaseFileExtension);
                    return await _httpClientProvider.Get(url, progress);
                });
            }
            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            /// <param name="filename"><inheritdoc/></param>
            /// <param name="destination"><inheritdoc/></param>
            /// <param name="parameters"><inheritdoc/></param>
            /// <param name="useBaseFileExtension"><inheritdoc/></param>
            /// <returns><inheritdoc/></returns>
            public override IAsyncTask GetDownload(string filename, Stream destination, string? parameters, bool useBaseFileExtension)
            {
                return RequestAsyncTaskController<bool>.CreateTask(false, async progress =>
                {
                    var url = await GetUrl(filename, parameters, useBaseFileExtension);
                    await _httpClientProvider.GetDownload(url, progress, destination);

                    return true;
                });
            }
            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            /// <param name="filename"><inheritdoc/></param>
            /// <param name="parameters"><inheritdoc/></param>
            /// <param name="useBaseFileExtension"><inheritdoc/></param>
            /// <returns><inheritdoc/></returns>
            public override IAsyncTask<byte[]> GetBytes(string filename, string? parameters, bool useBaseFileExtension)
            {
                return RequestAsyncTaskController<byte[]>.CreateTask(() => [], async progress =>
                {
                    using MemoryStream memory = new();
                    var url = await GetUrl(filename, parameters, useBaseFileExtension);
                    await _httpClientProvider.GetDownload(url, progress, memory);

                    return memory.ToArray();
                });
            }

            #endregion
        }
    }
}
