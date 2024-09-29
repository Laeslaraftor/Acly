using Acly.Tasks;
using System;
using System.Collections.Generic;

namespace Acly.Requests.Tasks
{
	/// <summary>
	/// Котроллер задачи скачивания списка файлов
	/// </summary>
	public class DownloadListTaskController : AclyAsyncTaskController
	{
		/// <summary>
		/// Котроллер задачи скачивания списка файлов
		/// </summary>
		/// <param name="Files">Файлы для скачивания</param>
		public DownloadListTaskController(IEnumerable<DownloadFileInfo> Files)
		{
			if (Files == null)
			{
				throw new ArgumentNullException(nameof(Files), "Файлы для скачивания не указаны");
			}

			_Files = new(Files);
			_Count = _Files.Count;
		}

		private readonly Queue<DownloadFileInfo> _Files;
		private readonly int _Count;

		#region Управление
		
		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public override async void Start()
		{
			int Index = 0;

			while (_Files.TryDequeue(out DownloadFileInfo File))
			{
				try
				{
					await Ajax.Download(File, Percent =>
					{
						OnProgressUpdated(Index, Percent);
					});
				}
				catch (Exception Error)
				{
					InvokeFailedEvent(Error);
					break;
				}

				Index++;
			}

			InvokeCompletedEvent();
		}

		#endregion

		#region События

		private void OnProgressUpdated(int Index, float Percent)
		{
			float OutPercent = Progress.FromAmountsRange(_Count, Index, Percent);
			InvokeProgressUpdatedEvent(OutPercent);
		}

		#endregion
	}
}
