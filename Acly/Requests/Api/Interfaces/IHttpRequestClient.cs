using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Acly.Requests
{
    /// <summary>
    /// Интерфейс провайдера http клиента
    /// </summary>
    public interface IHttpRequestClient : IDisposable
    {
        /// <summary>
        /// Максимальное время ожидания ответа сервера
        /// </summary>
        public TimeSpan Timeout { get; set; }

        /// <summary>
        /// Сделать GET запрос по указанному адресу
        /// </summary>
        /// <param name="uri">Адрес запроса</param>
        /// <param name="progress">Обработчик прогресса</param>
        /// <returns>Полученный ответ</returns>
        public Task<string> Get(Uri uri, IProgress<double>? progress);
        /// <summary>
        /// Сделать GET запрос по указанному адресу
        /// </summary>
        /// <param name="uri">Адрес запроса</param>
        /// <param name="progress">Обработчик прогресса</param>
        /// <returns>Полученный ответ</returns>
        public Task<T> Get<T>(Uri uri, IProgress<double>? progress)
            where T : new();
        /// <summary>
        /// Сделать POST запрос по указанному адресу
        /// </summary>
        /// <param name="uri">Адрес запроса</param>
        /// <param name="progress">Обработчик прогресса</param>ъ
        /// <param name="content">Содержимое запроса</param>
        /// <returns>Полученный ответ</returns>
        public Task<string> Post(Uri uri, IProgress<double>? progress, HttpContent? content);
        /// <summary>
        /// Сделать POST запрос по указанному адресу
        /// </summary>
        /// <param name="uri">Адрес запроса</param>
        /// <param name="progress">Обработчик прогресса</param>ъ
        /// <param name="content">Содержимое запроса</param>
        /// <returns>Полученный ответ</returns>
        public Task<T> Post<T>(Uri uri, IProgress<double>? progress, HttpContent? content)
            where T : new();
        /// <summary>
        /// Сделать GET запрос по указанному адресу и скачать данные
        /// </summary>
        /// <param name="uri">Адрес запроса</param>
        /// <param name="progress">Обработчик прогресса</param>
        /// <param name="destination">Поток в который будут записываться скаченные данные</param>
        public Task GetDownload(Uri uri, IProgress<double>? progress, Stream destination);
        /// <summary>
        /// Сделать POST запрос по указанному адресу и скачать данные
        /// </summary>
        /// <param name="uri">Адрес запроса</param>
        /// <param name="progress">Обработчик прогресса</param>
        /// <param name="content">Содержимое запроса</param>
        /// <param name="destination">Поток в который будут записываться скаченные данные</param>
        public Task PostDownload(Uri uri, IProgress<double>? progress, HttpContent? content, Stream destination);
    }
}
