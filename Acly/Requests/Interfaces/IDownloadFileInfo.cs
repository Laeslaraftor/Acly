namespace Acly.Requests
{
    /// <summary>
    /// Интерфейс информации о файле для его скачивания
    /// </summary>
    public interface IDownloadFileInfo
    {
        /// <summary>
        /// Адрес файла
        /// </summary>
        public string Url { get; }
        /// <summary>
        /// Путь для скачивания файла
        /// </summary>
        public string OutputPath { get; }
    }
}
