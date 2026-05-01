using Acly.Requests;
using Acly.Tasks;
using Acly.Tokens;
using System;
using System.Threading.Tasks;

namespace Acly.Performing.Tasks
{
    /// <summary>
    /// Контроллер асинхронных задач загрузки
    /// </summary>
    public class LoadingAsyncTasksController : IAsyncTaskController
    {
        /// <summary>
        /// Создать экземпляр котроллера асинхронных задач загрузки
        /// </summary>
        /// <param name="tasksForPerform">Задачи для выполнения</param>
        /// <exception cref="ArgumentNullException">Задачи не установлены</exception>
        public LoadingAsyncTasksController(LoadingTasksList tasksForPerform)
        {
            if (tasksForPerform == null)
            {
                throw new ArgumentNullException(nameof(tasksForPerform), "Задачи для выполнения не установлены");
            }

            tasksForPerform.MakeReadOnly();

            _tasks = tasksForPerform;
        }

        /// <summary>
        /// Вызывается при изменении описания
        /// </summary>
        public event Action? DescriptionChanged;
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

        /// <summary>
        /// Описание текущего этапа загрузки
        /// </summary>
        public string? Description
        {
            get => _description;
            set
            {
                if (_description == value)
                {
                    return;
                }

                _description = value;
                DescriptionChanged?.Invoke();
            }
        }

        private readonly LoadingTasksList _tasks;
        private Token? _processingToken;
        private string? _description;

        #region Управление

        /// <summary>
        /// Начать выполнение
        /// </summary>
        public async void Start()
        {
            Token currentToken = new();
            _processingToken = currentToken;

            for (int i = 0; i < _tasks.Count; i++)
            {
                if (_processingToken != currentToken)
                {
                    break;
                }

                LoadingAsyncTask task = _tasks[i];
                IAsyncTaskError? result = await TryPerformTask(task, i);

                if (result != null)
                {
                    Failed?.Invoke(result);
                    return;
                }
            }

            Completed?.Invoke();
        }

        /// <summary>
        /// Прервать выполнение
        /// </summary>
        /// <param name="response">Сообщение</param>
        public void BreakWithError(Response response)
        {
            _processingToken = null;
            Failed?.Invoke(new AclyAsyncTaskError(new LoadingException(response)));
        }

        #endregion

        #region Выполнение задач

        private void UpdateProgress(int current, float percent)
        {
            float completed = Progress.FromAmountsRange(_tasks.Count, current, percent);
            ProgressUpdated?.Invoke(completed);
        }
        private async Task<IAsyncTaskError?> TryPerformTask(LoadingAsyncTask taskFunction, int taskIndex)
        {
            IAsyncTask? asyncTask = null;
            string? description = taskFunction.Description;

            TrySetDescription(description, 0);

            try
            {
                asyncTask = await taskFunction.GetAsyncTask();
            }
            catch (Exception error)
            {
                return new AclyAsyncTaskError(error);
            }

            if (asyncTask == null)
            {
                return null;
            }

            bool completed = false;
            IAsyncTaskError? result = null;
            bool failed = false;

            void OnCompleted()
            {
                completed = true;

                asyncTask.Completed -= OnCompleted;
                asyncTask.ProgressUpdated -= OnProgressUpdated;
                asyncTask.Failed -= OnFailed;
            }
            ;
            void OnProgressUpdated(float percent)
            {
                UpdateProgress(taskIndex, percent);
                TrySetDescription(description, percent);
            }
            ;
            void OnFailed(IAsyncTaskError info)
            {
                result = info;
                failed = true;
                OnCompleted();
            }
            ;

            asyncTask.Completed += OnCompleted;
            asyncTask.ProgressUpdated += OnProgressUpdated;
            asyncTask.Failed += OnFailed;

            while (!completed)
            {
                await Task.Delay(100);
            }

            if (!failed)
            {
                TrySetDescription(description, 1);
            }

            return result;
        }

        private bool TrySetDescription(string? value, float progress)
        {
            if (value == null)
            {
                return false;
            }

            try
            {
                Description = string.Format(value, Math.Round(progress * 100) + "%");
            }
            catch
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
