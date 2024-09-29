namespace Acly.Requests
{
	/// <summary>
	/// Информация о файле для его скачивания
	/// </summary>
	public class DownloadFileInfo
	{
		/// <summary>
		/// Информация о файле для его скачивания
		/// </summary>
		/// <param name="Url">Адрес файла</param>
		/// <param name="OutputPath">Путь для скачивания файла</param>
		public DownloadFileInfo(string Url, string OutputPath)
		{
			this.Url = Url;
			this.OutputPath = OutputPath;
		}

		/// <summary>
		/// Адрес файла
		/// </summary>
		public string Url { get; private set; }
		/// <summary>
		/// Путь для скачивания файла
		/// </summary>
		public string OutputPath { get; private set; }
	}
}
