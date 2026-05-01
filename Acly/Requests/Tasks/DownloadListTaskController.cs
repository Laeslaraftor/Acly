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
        /// <param name="files">Файлы для скачивания</param>
        public DownloadListTaskController(IEnumerable<DownloadFileInfo> files)
        {
            if (files == null)
            {
                throw new ArgumentNullException(nameof(files), "Файлы для скачивания не указаны");
            }

            _files = new(files);
            _count = _files.Count;
        }

        private readonly Queue<DownloadFileInfo> _files;
        private readonly int _count;

        #region Управление

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override async void Start()
        {
            int index = 0;

            while (_files.TryDequeue(out DownloadFileInfo file))
            {
                try
                {
                    await Ajax.Download(file, percent =>
                    {
                        OnProgressUpdated(index, percent);
                    });
                }
                catch (Exception error)
                {
                    InvokeFailedEvent(error);
                    break;
                }

                index++;
            }

            InvokeCompletedEvent();
        }

        #endregion

        #region События

        private void OnProgressUpdated(int index, float percent)
        {
            float outPercent = Progress.FromAmountsRange(_count, index, percent);
            InvokeProgressUpdatedEvent(outPercent);
        }

        #endregion
    }
}
