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
            add => _CompleteHandlers.Add(value);
            remove => _CompleteHandlers.Remove(value);
        }

        private readonly List<AsyncTaskComplete<T>> _CompleteHandlers = new();
        private bool _IsFailed;
        private Thread? _TaskThread;

        #region Управление

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public async void Start()
        {
            bool IsCompleted = false;
            T? Result = default;
            _TaskThread = new(async () =>
            {
                try
                {
                    Result = await StartTask();
                    IsCompleted = true;
                }
                catch (Exception Error)
                {
                    IsCompleted = true;
                    Interrupt(Error);
                }
            });
            _TaskThread.Start();

            while (_TaskThread.IsAlive || !IsCompleted)
            {
                await Task.Delay(50);
            }

            if (_IsFailed)
            {
                return;
            }

            SendProgress(1);
#pragma warning disable CS8604
            InvokeCompletedEvents(Result);
#pragma warning restore CS8604
        }

        /// <summary>
        /// <inheritdoc cref="Start"/>
        /// </summary>
        /// <returns></returns>
        protected abstract Task<T> StartTask();

        /// <summary>
        /// Отправить прогресс выполнения задачи
        /// </summary>
        /// <param name="Progress">Прогресс выполнения задачи</param>
        protected void SendProgress(float Progress)
        {
            ProgressUpdated?.Invoke(Progress);
        }
        /// <summary>
        /// Прервать с ошибкой
        /// </summary>
        /// <param name="Error">Ошибка</param>
        protected void Interrupt(Exception Error)
        {
            if (_IsFailed)
            {
                return;
            }

            _IsFailed = true;
            Stop();
            Failed?.Invoke(new AclyAsyncTaskError(Error));
        }
        /// <summary>
        /// Прервать с сообщением
        /// </summary>
        /// <param name="Message">Сообщение</param>
        protected void Interrupt(Response Message)
        {
            if (_IsFailed)
            {
                return;
            }

            _IsFailed = true;
            Stop();
            Failed?.Invoke(new AclyAsyncTaskError(Message));
        }

        private void Stop()
        {
            if (_TaskThread == null)
            {
                return;
            }

            try
            {
                _TaskThread.Abort();
            }
            catch
            {
            }
        }

        #endregion

        #region События

        private void InvokeCompletedEvents(T Result)
        {
            Completed?.Invoke();

            List<AsyncTaskComplete<T>> Handlers = new(_CompleteHandlers);

            foreach (var Handler in Handlers)
            {
                Handler(Result);
            }
        }

        #endregion
    }
}
