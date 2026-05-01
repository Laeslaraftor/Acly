using Acly.Requests;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Acly.Tasks
{
    /// <summary>
    /// Базовый класс контроллера асинхронной задачи
    /// </summary>
    public abstract class AsyncTaskControllerBase : IAsyncTaskController
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event Action? Completed;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event AsyncTaskProgress? ProgressUpdated;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event AsyncTaskFail? Failed;

        private bool _isFailed;
        private Thread? _taskThread;

        #region Управление

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public async void Start()
        {
            bool isCompleted = false;
            _taskThread = new(async () =>
            {
                try
                {
                    await StartTask();
                    isCompleted = true;
                }
                catch (Exception error)
                {
                    isCompleted = true;
                    Interrupt(error);
                }
            });
            _taskThread.Start();

            while (_taskThread.IsAlive || !isCompleted)
            {
                await Task.Delay(50);
            }

            if (_isFailed)
            {
                return;
            }

            SendProgress(1);
            Completed?.Invoke();
        }

        /// <summary>
        /// <inheritdoc cref="Start"/>
        /// </summary>
        /// <returns></returns>
        protected abstract Task StartTask();

        /// <summary>
        /// Отправить прогресс выполнения задачи
        /// </summary>
        /// <param name="progress">Прогресс выполнения задачи</param>
        protected void SendProgress(float progress)
        {
            ProgressUpdated?.Invoke(progress);
        }
        /// <summary>
        /// Прервать с ошибкой
        /// </summary>
        /// <param name="error">Ошибка</param>
        protected void Interrupt(Exception error)
        {
            if (_isFailed)
            {
                return;
            }

            _isFailed = true;
            Stop();
            Failed?.Invoke(new AclyAsyncTaskError(error));
        }
        /// <summary>
        /// Прервать с сообщением
        /// </summary>
        /// <param name="message">Сообщение</param>
        protected void Interrupt(Response message)
        {
            if (_isFailed)
            {
                return;
            }

            _isFailed = true;
            Stop();
            Failed?.Invoke(new AclyAsyncTaskError(message));
        }

        private void Stop()
        {
            if (_taskThread == null)
            {
                return;
            }

            try
            {
                _taskThread.Abort();
            }
            catch (Exception error)
            {
                Log.Error(error);
            }
        }

        #endregion
    }
}
