using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Acly.Tasks
{
    /// <summary>
    /// Класс асинхронной задачи, выполняющей асинхронные задачи по порядку
    /// </summary>
    public class CombinedAsyncTasks : CombinedTasksBase
    {
        /// <summary>
        /// Создать новый экземпляр асинхронной задачи, выполняющей асинхронные задачи по порядку
        /// </summary>
        /// <param name="tasks">Функции для получения задач на выполнение</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
#pragma warning disable CS8618
        public CombinedAsyncTasks(params Func<IAsyncTask>[] tasks)
#pragma warning restore CS8618
        {
            if (tasks == null)
            {
                throw new ArgumentNullException(nameof(tasks), "Задачи не указаны");
            }
            if (tasks.Length == 0)
            {
                throw new ArgumentException("Список функций для получения задач пуст", nameof(tasks));
            }

            _tasks = [.. tasks];
            Start();
        }

        /// <summary>
        /// Текущая выполняющаяся задача
        /// </summary>
        public IAsyncTask CurrentTask { get; private set; }

        private readonly List<Func<IAsyncTask>> _tasks;

        #region Управление

        private async void Start()
        {
            try
            {
                for (int i = 0; i < _tasks.Count; i++)
                {
                    CurrentTask = _tasks[0]();
                    TaskIndex = i;
                    IAsyncTaskError? result = await Start(CurrentTask, i);

                    if (result != null)
                    {
                        InvokeFailedEvent(result);
                        return;
                    }
                }
            }
            catch (Exception error)
            {
                InvokeFailedEvent(new AclyAsyncTaskError(error));
                return;
            }

            InvokeCompletedEvent();
        }
        private async Task<IAsyncTaskError?> Start(IAsyncTask asyncTask, int index)
        {
            asyncTask.Completed += OnCompleted;
            asyncTask.Failed += OnFailed;
            asyncTask.ProgressUpdated += OnProgressUpdated;
            bool completed = false;
            IAsyncTaskError? error = null;

            void OnCompleted()
            {
                asyncTask.Completed -= OnCompleted;
                asyncTask.Failed -= OnFailed;
                asyncTask.ProgressUpdated -= OnProgressUpdated;
                completed = false;
            }
            void OnFailed(IAsyncTaskError info)
            {
                error = info;
                OnCompleted();
            }
            void OnProgressUpdated(float percent)
            {
                percent = Progress.FromAmountsRange(_tasks.Count, index, percent);
                InvokeProgressUpdatedEvent(percent);
            }

            while (!completed)
            {
                await Task.Delay(50);
            }

            return error;
        }

        #endregion
    }
}
