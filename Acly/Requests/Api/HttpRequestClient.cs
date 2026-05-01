using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Acly.Requests
{
    /// <summary>
    /// Класс с базовой реализацией <see cref="IHttpRequestClient"/>
    /// </summary>
    public class HttpRequestClient(HttpClient client) : Disposable, IHttpRequestClient
    {
        /// <summary>
        /// Создать новый экземпляр клиента отправки http запросов
        /// </summary>
#pragma warning disable CA2000 // Ликвидировать объекты перед потерей области
        public HttpRequestClient() : this(new())
#pragma warning restore CA2000 // Ликвидировать объекты перед потерей области
        {
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public TimeSpan Timeout
        {
            get => field;
            set
            {
                if (field != value)
                {
                    OnPropertyChanging(nameof(Timeout));
                    field = value;
                    HttpClient.Timeout = value;
                    OnPropertyChanged(nameof(Timeout));
                }
            }
        } = System.Threading.Timeout.InfiniteTimeSpan;

        /// <summary>
        /// Http клиент, который используется для отправки запросов
        /// </summary>
        protected virtual HttpClient HttpClient { get; } = client;

        #region Управление

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="isDisposing"><inheritdoc/></param>
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            HttpClient.Dispose();
        }

        #endregion

        #region Запросы

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="uri"><inheritdoc/></param>
        /// <param name="progress"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public async Task<string> Get(Uri uri, IProgress<double>? progress)
        {
            Log.Message("GET: " + uri);
            return await HttpClient.GetString(uri, progress);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="uri"><inheritdoc/></param>
        /// <param name="progress"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public async Task<T> Get<T>(Uri uri, IProgress<double>? progress)
            where T : new()
        {
            Log.Message("GET: " + uri);
            return await HttpClient.GetApi<T>(uri, progress);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="uri"><inheritdoc/></param>
        /// <param name="progress"><inheritdoc/></param>
        /// <param name="content"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public async Task<string> Post(Uri uri, IProgress<double>? progress, HttpContent? content)
        {
            Log.Message("POST: " + uri);
            return await HttpClient.PostString(uri, progress, content);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="uri"><inheritdoc/></param>
        /// <param name="progress"><inheritdoc/></param>
        /// <param name="content"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public async Task<T> Post<T>(Uri uri, IProgress<double>? progress, HttpContent? content)
            where T : new()
        {
            Log.Message("POST: " + uri);
            return await HttpClient.PostApi<T>(uri, progress, content);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="uri"><inheritdoc/></param>
        /// <param name="progress"><inheritdoc/></param>
        /// <param name="destination"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public async Task GetDownload(Uri uri, IProgress<double>? progress, Stream destination)
        {
            Log.Message("GET downloading: " + uri);
            await HttpClient.ReadAsync(HttpMethod.Get, uri, destination, progress, null);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="uri"><inheritdoc/></param>
        /// <param name="progress"><inheritdoc/></param>
        /// <param name="content"><inheritdoc/></param>
        /// <param name="destination"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public async Task PostDownload(Uri uri, IProgress<double>? progress, HttpContent? content, Stream destination)
        {
            Log.Message("POST downloading: " + uri);
            await HttpClient.ReadAsync(HttpMethod.Post, uri, destination, progress, content);
        }

        #endregion
    }
}
