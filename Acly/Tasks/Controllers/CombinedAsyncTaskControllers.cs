using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Acly.Tasks
{
    /// <summary>
    /// Контроллер объединённых асинхронных задач. Последовательно выполняет каждую задачу
    /// </summary>
    public class CombinedAsyncTaskControllers : AsyncTaskControllerBase
    {
        /// <summary>
        /// Создать новый экземпляр контроллера объединённых асинхронных задач
        /// </summary>
        /// <param name="TasksGetters">Список методов для получения асинхронных задач</param>
        public CombinedAsyncTaskControllers(IEnumerable<Func<IAsyncTask>> TasksGetters)
        {
            _TasksGetters = new(TasksGetters);
        }

        private readonly List<Func<IAsyncTask>> _TasksGetters;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        protected override async Task StartTask()
        {
            for (int i = 0; i < _TasksGetters.Count; i++)
            {
                var CurrentTask = _TasksGetters[i]();
                CurrentTask.ProgressUpdated += OnProgressUpdated;

                void OnProgressUpdated(float Progress)
                {
                    Progress = Acly.Progress.FromAmountsRange(_TasksGetters.Count, i, Progress);
                }

                while (!CurrentTask.IsCompleted)
                {
                    await Task.Delay(50);
                }

                CurrentTask.ProgressUpdated -= OnProgressUpdated;

                if (CurrentTask.Error != null)
                {
                    Interrupt(CurrentTask.Error.Exception);
                    break;
                }
            }
        }
    }
}
