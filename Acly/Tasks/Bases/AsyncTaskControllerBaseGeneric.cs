using Acly.Requests;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Acly.Tasks
{
    /// <summary>
    /// Базовый класс контроллера асинхронной задачи, которая возвращает результат
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AsyncTaskControllerBase<T> : IAsyncTaskController<T>
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
        event AsyncTaskComplete<T> IAsyncTaskController<T>.Completed
        {
            add => _completeHandlers.Add(value);
            remove => _completeHandlers.Remove(value);
        }

        private readonly List<AsyncTaskComplete<T>> _completeHandlers = [];
        private bool _isFailed;
        private Thread? _taskThread;

        #region Управление

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public async void Start()
        {
            bool isCompleted = false;
            T? result = default;
            _taskThread = new(async () =>
            {
                try
                {
                    result = await StartTask();
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
            InvokeCompletedEvents(result!);
        }

        /// <summary>
        /// <inheritdoc cref="Start"/>
        /// </summary>
        /// <returns></returns>
        protected abstract Task<T> StartTask();

        /// <summary>
        /// Отправить прогресс выполнения задачи
        /// </summary>
        /// <param name="progress">Прогресс выполнения задачи</param>
        protected void SendProgress(float progress)
        {
            ProgressUpdated?.Invoke(progress);
        }
        /// <summary>
        /// Отправить прогресс выполнения задачи
        /// </summary>
        /// <param name="progress">Прогресс выполнения задачи</param>
        protected void SendProgress(double progress)
        {
            SendProgress((float)progress);
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

        #region События

        private void InvokeCompletedEvents(T result)
        {
            Completed?.Invoke();

            List<AsyncTaskComplete<T>> Handlers = [.. _completeHandlers];

            foreach (var Handler in Handlers)
            {
                Handler(result);
            }
        }

        #endregion
    }
}
