using Acly.Tasks;
using System;
using System.Threading.Tasks;

namespace Acly.Requests.Tasks
{
    internal sealed class RequestAsyncTaskController<T> : AsyncTaskControllerBase<T>
    {
        public RequestAsyncTaskController(T defaultValue, Func<IProgress<double>, Task<T>> requestMethod)
            : this(() => defaultValue, requestMethod)
        {
        }
        public RequestAsyncTaskController(Func<T> defaultValueFactory, Func<IProgress<double>, Task<T>> requestMethod)
        {
            _defaultValueFactory = defaultValueFactory;
            _requestMethod = requestMethod;
            _progressHandler = new(SendProgress);
        }

        private readonly Func<T> _defaultValueFactory;
        private readonly Func<IProgress<double>, Task<T>> _requestMethod;
        private readonly Progress<double> _progressHandler;

        #region Управление

        protected override async Task<T> StartTask()
        {
            try
            {
                return await _requestMethod(_progressHandler);
            }
            catch (Exception error)
            {
                Interrupt(error);
            }

            return _defaultValueFactory();
        }

        #endregion

        #region Статика

        public static IAsyncTask<T> CreateTask(T defaultValue, Func<IProgress<double>, Task<T>> requestMethod)
        {
            return new AclyAsyncTask<T>(new RequestAsyncTaskController<T>(defaultValue, requestMethod));
        }
        public static IAsyncTask<T> CreateTask(Func<T> defaultValueFactory, Func<IProgress<double>, Task<T>> requestMethod)
        {
            return new AclyAsyncTask<T>(new RequestAsyncTaskController<T>(defaultValueFactory, requestMethod));
        }

        #endregion
    }
}
