using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Acly.Tasks
{
    /// <summary>
    /// Контроллер объединённых асинхронных задач. Последовательно выполняет каждую задачу
    /// </summary>
    /// <remarks>
    /// Создать новый экземпляр контроллера объединённых асинхронных задач
    /// </remarks>
    /// <param name="tasksGetters">Список методов для получения асинхронных задач</param>
    public class CombinedAsyncTaskControllers(IEnumerable<Func<IAsyncTask>> tasksGetters) : AsyncTaskControllerBase
    {
        private readonly List<Func<IAsyncTask>> _tasksGetters = [.. tasksGetters];

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        protected override async Task StartTask()
        {
            for (int i = 0; i < _tasksGetters.Count; i++)
            {
                var currentTask = _tasksGetters[i]();
                currentTask.ProgressUpdated += OnProgressUpdated;

                void OnProgressUpdated(float progress)
                {
                    progress = Acly.Progress.FromAmountsRange(_tasksGetters.Count, i, progress);
                }

                while (!currentTask.IsCompleted)
                {
                    await Task.Delay(50);
                }

                currentTask.ProgressUpdated -= OnProgressUpdated;

                if (currentTask.Error != null)
                {
                    Interrupt(currentTask.Error.Exception);
                    break;
                }
            }
        }
    }
}
