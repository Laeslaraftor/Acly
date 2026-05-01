using System;

namespace Acly.Requests
{
    /// <summary>
    /// Информация о файле для его скачивания
    /// </summary>
    /// <param name="url">Адрес файла</param>
    /// <param name="outputPath">Путь для скачивания файла</param>
    public struct DownloadFileInfo(string url, string outputPath)
        : IDownloadFileInfo, IEquatable<DownloadFileInfo>, IEquatable<IDownloadFileInfo?>
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string Url { get; set; } = url;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string OutputPath { get; set; } = outputPath;

        #region Управление

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="other"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public readonly bool Equals(DownloadFileInfo other)
        {
            return Equals((IDownloadFileInfo)other);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="other"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public readonly bool Equals(IDownloadFileInfo? other)
        {
            if (other == null)
            {
                return false;
            }

            return Url == other.Url &&
                   OutputPath == other.OutputPath;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="obj"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public override readonly bool Equals(object obj)
        {
            return obj is IDownloadFileInfo other &&
                   Equals(other);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        public override readonly int GetHashCode()
        {
            return HashCode.Combine(Url, OutputPath);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        public override readonly string ToString()
        {
            return $"{Url} -> {OutputPath}";
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="left"><inheritdoc/></param>
        /// <param name="right"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public static bool operator ==(DownloadFileInfo left, DownloadFileInfo right)
        {
            return left.Equals(right);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="left"><inheritdoc/></param>
        /// <param name="right"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public static bool operator !=(DownloadFileInfo left, DownloadFileInfo right)
        {
            return !(left == right);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="left"><inheritdoc/></param>
        /// <param name="right"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public static bool operator ==(IDownloadFileInfo left, DownloadFileInfo right)
        {
            return left?.Equals(right) == true;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="left"><inheritdoc/></param>
        /// <param name="right"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public static bool operator !=(IDownloadFileInfo left, DownloadFileInfo right)
        {
            return !(left == right);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="left"><inheritdoc/></param>
        /// <param name="right"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public static bool operator ==(DownloadFileInfo left, IDownloadFileInfo right)
        {
            return left.Equals(right);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="left"><inheritdoc/></param>
        /// <param name="right"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public static bool operator !=(DownloadFileInfo left, IDownloadFileInfo right)
        {
            return !(left == right);
        }

        #endregion
    }
}
