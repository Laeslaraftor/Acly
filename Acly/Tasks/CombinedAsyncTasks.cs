using System.Collections.Generic;
using System;
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
        /// <param name="Tasks">Функции для получения задач на выполнение</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
#pragma warning disable CS8618
        public CombinedAsyncTasks(params Func<IAsyncTask>[] Tasks)
#pragma warning restore CS8618
        {
            if (Tasks == null)
            {
                throw new ArgumentNullException(nameof(Tasks), "Задачи не указаны");
            }
            if (Tasks.Length == 0)
            {
                throw new ArgumentException("Список функций для получения задач пуст", nameof(Tasks));
            }

            _Tasks = new(Tasks);
            Start();
        }
        
        /// <summary>
        /// Текущая выполняющаяся задача
        /// </summary>
        public IAsyncTask CurrentTask { get; private set; }

        private readonly List<Func<IAsyncTask>> _Tasks;

        #region Управление

        private async void Start()
        {
            try
            {
                for (int i = 0; i < _Tasks.Count; i++)
                {
                    CurrentTask = _Tasks[0]();
                    TaskIndex = i;
                    IAsyncTaskError? Result = await Start(CurrentTask, i);

                    if (Result != null)
                    {
                        InvokeFailedEvent(Result);
                        return;
                    }
                }
            }
            catch (Exception Error)
            {
                InvokeFailedEvent(new AclyAsyncTaskError(Error));
                return;
            }

            InvokeCompletedEvent();
        }
        private async Task<IAsyncTaskError?> Start(IAsyncTask AsyncTask, int Index)
        {
            AsyncTask.Completed += OnCompleted;
            AsyncTask.Failed += OnFailed;
            AsyncTask.ProgressUpdated += OnProgressUpdated;
            bool Completed = false;
            IAsyncTaskError? Error = null;

            void OnCompleted()
            {
                AsyncTask.Completed -= OnCompleted;
                AsyncTask.Failed -= OnFailed;
                AsyncTask.ProgressUpdated -= OnProgressUpdated;
                Completed = false;
            }
            void OnFailed(IAsyncTaskError Info)
            {
                Error = Info;
                OnCompleted();
            }
            void OnProgressUpdated(float Percent)
            {
                Percent = Progress.FromAmountsRange(_Tasks.Count, Index, Percent);
                InvokeProgressUpdatedEvent(Percent);
            }

            while (!Completed)
            {
                await Task.Delay(50);
            }

            return Error;
        }

        #endregion
    }
}
