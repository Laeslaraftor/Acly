using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Acly.Requests
{
    /// <summary>
    /// Результат запроса <see cref="HttpClient"/>
    /// </summary>
    public readonly struct HttpClientReadResult(Encoding encoding, Stream content, HttpStatusCode statusCode, bool isSuccessStatusCode)
        : IDisposable, IEquatable<HttpClientReadResult>
    {
        /// <summary>
        /// Результат запроса <see cref="HttpClient"/>
        /// </summary>
        public HttpClientReadResult(Encoding encoding, Stream content, HttpResponseMessage response)
#pragma warning disable CA1062 // Проверить аргументы или открытые методы
            : this(encoding, content, response.StatusCode, response.IsSuccessStatusCode)
#pragma warning restore CA1062 // Проверить аргументы или открытые методы
        {
        }

        /// <summary>
        /// Кодировка ответа
        /// </summary>
        public Encoding Encoding { get; } = encoding;
        /// <summary>
        /// Содержимое ответа
        /// </summary>
        public Stream Content { get; } = content;
        /// <summary>
        /// Код ответа
        /// </summary>
        public HttpStatusCode StatusCode { get; } = statusCode;
        /// <summary>
        /// Является ли код ответа успешным
        /// </summary>
        public bool IsSuccessStatusCode { get; } = isSuccessStatusCode;

        #region Управление

        /// <summary>
        /// Прочитать ответ как массив байтов
        /// </summary>
        /// <returns>Ответ как массив байтов</returns>
        public byte[]? ReadAsByteArray()
        {
            if (Encoding == null || Content == null)
            {
                return null;
            }
            if (Content is MemoryStream memory)
            {
                return memory.ToArray();
            }

            using MemoryStream stream = new();
            Content.Position = 0;
            Content.CopyTo(stream);

            return stream.ToArray();
        }
        /// <summary>
        /// Прочитать содержимое ответа как строку с учётом кодировки
        /// </summary>
        /// <returns>Ответ как строка</returns>
        public string? ReadAsString()
        {
            if (Encoding == null)
            {
                return null;
            }

            var content = ReadAsByteArray();

            return content == null ? null : Encoding.GetString(content);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public async void Dispose()
        {
            if (Content != null)
            {
                await Content.DisposeAsync();
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="other"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public readonly bool Equals(HttpClientReadResult other)
        {
            return Encoding == other.Encoding &&
                   Content == other.Content;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="obj"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public override readonly bool Equals(object obj)
        {
            return obj is HttpClientReadResult other &&
                   Equals(other);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        public override readonly int GetHashCode()
        {
            return HashCode.Combine(Encoding, Content);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="left"><inheritdoc/></param>
        /// <param name="right"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public static bool operator ==(HttpClientReadResult left, HttpClientReadResult right)
        {
            return left.Equals(right);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="left"><inheritdoc/></param>
        /// <param name="right"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public static bool operator !=(HttpClientReadResult left, HttpClientReadResult right)
        {
            return !(left == right);
        }

        #endregion
    }
}
