using Acly.Requests;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Acly.Tasks
{
    /// <summary>
    /// Класс с методами расширения для асинхронных задач
    /// </summary>
#pragma warning disable CA1708 // Идентификаторы должны отличаться не только регистром
    public static class TaskExtensions
#pragma warning restore CA1708 // Идентификаторы должны отличаться не только регистром
    {
        private const string _EmptyResultErrorCode = "TaskEmptyErrorInfo";
        private const string _EmptyResultErrorMessage = "Произошла ошибка, но информация об ошибке не предоставлена";

        extension(AsyncTaskAwaitResult result)
        {
            /// <summary>
            /// Вызвать обработчик в зависимости от результата выполнения задачи
            /// </summary>
            /// <param name="success">Обработчик успешного выполнения задачи</param>
            /// <param name="error">Обработчик провального выполнения задачи</param>
            public void InvokeCallback(Action? success, Action<ApiResponse>? error = null)
            {
                if (success == null && error == null)
                {
                    return;
                }
                if (result.IsSuccess)
                {
                    if (success != null && !success.TryInvoke(out var exception))
                    {
                        Log.Error(exception);
                    }

                    return;
                }
                if (result.Error != null)
                {
                    var response = Ajax.ExceptionToApiResponse(result.Error.Exception);

                    if (!error.TryInvoke(response, out var exception))
                    {
                        Log.Error(exception);
                    }

                    return;
                }

                ApiResponse falloffResponse = new()
                {
                    Status = ApiResponseStatus.Error,
                    Code = _EmptyResultErrorCode,
                    Text = _EmptyResultErrorMessage
                };

                if (!error.TryInvoke(falloffResponse, out var invokeException))
                {
                    Log.Error(invokeException);
                }
            }
        }
        extension<T>(AsyncTaskAwaitResult<T> result)
        {
            /// <summary>
            /// Вызвать обработчик в зависимости от результата выполнения задачи
            /// </summary>
            /// <param name="success">Обработчик успешного выполнения задачи</param>
            /// <param name="error">Обработчик провального выполнения задачи</param>
            public void InvokeCallback(Action<T>? success, Action<ApiResponse>? error = null)
            {
                if (success == null && error == null)
                {
                    return;
                }
                if (result.IsSuccess)
                {
                    if (success != null && !success.TryInvoke(result.Result!, out var exception))
                    {
                        Ajax.ExceptionToApiResponse(exception, error);
                        Log.Error(exception);
                    }

                    return;
                }
                if (error == null)
                {
                    return;
                }
                if (result.Error != null)
                {
                    var response = Ajax.ExceptionToApiResponse(result.Error.Exception);

                    if (!error.TryInvoke(response, out var exception))
                    {
                        Log.Error(exception);
                    }

                    return;
                }

                ApiResponse falloffResponse = new()
                {
                    Status = ApiResponseStatus.Error,
                    Code = _EmptyResultErrorCode,
                    Text = _EmptyResultErrorMessage
                };

                if (!error.TryInvoke(falloffResponse, out var invokeException))
                {
                    Log.Error(invokeException);
                }
            }
        }
        extension(IAsyncTask asyncTask)
        {
            /// <summary>
            /// Получает объект типа awaiter, используемый для данного объекта Task.
            /// </summary>
            /// <returns></returns>
            public TaskAwaiter<AsyncTaskAwaitResult> GetAwaiter()
            {
                var taskSource = new TaskCompletionSource<AsyncTaskAwaitResult>();
                var task = taskSource.Task;

                void SetResult()
                {
                    var error = asyncTask.Error;
                    taskSource.SetResult(new(error == null, error));
                }

                asyncTask.Pin(SetResult, _ => SetResult());

                return task.GetAwaiter();
            }
            /// <summary>
            /// Добавить обработчики задачи.
            /// После завершения задачи все обработчики будут сняты.
            /// </summary>
            /// <param name="completionHandler">Обработчик завершения задачи</param>
            /// <param name="progressHandler">Обработчик прогресса выполнения задачи</param>
            /// <param name="failHandler">Обработчик провала задачи</param>
            public void Pin(Action? completionHandler, AsyncTaskProgress? progressHandler, Action<Response>? failHandler)
            {
                if (completionHandler == null &&
                    progressHandler == null &&
                    failHandler == null)
                {
                    return;
                }
                if (asyncTask.IsCompleted)
                {
                    if (asyncTask.Error != null)
                    {
                        OnFailed(asyncTask.Error);
                        return;
                    }

                    OnCompleted();
                    return;
                }

                void Clear()
                {
                    asyncTask.Completed -= OnCompleted;
                    asyncTask.Failed -= OnFailed;

                    if (progressHandler != null)
                    {
                        asyncTask.ProgressUpdated -= progressHandler;
                    }
                }
                void OnCompleted()
                {
                    Clear();

                    if (completionHandler != null && !completionHandler.TryInvoke(out var exception))
                    {
                        Log.Error(exception);
                    }
                }
                void OnFailed(IAsyncTaskError error)
                {
                    Clear();

                    if (failHandler != null && !failHandler.TryInvoke(error.Response, out var exception))
                    {
                        Log.Error(exception);
                    }
                }

                asyncTask.Completed += OnCompleted;
                asyncTask.Failed += OnFailed;

                if (progressHandler != null)
                {
                    asyncTask.ProgressUpdated += progressHandler;
                }
            }
            /// <summary>
            /// Добавить обработчики задачи.
            /// После завершения задачи все обработчики будут сняты.
            /// </summary>
            /// <param name="completionHandler">Обработчик завершения задачи</param>
            /// <param name="progressHandler">Обработчик прогресса выполнения задачи</param>
            /// <param name="failHandler">Обработчик провала задачи</param>
            public void Pin(Action? completionHandler, Action<float>? progressHandler, Action<Response>? failHandler)
            {
                void OnProgress(float progress)
                {
                    if (!progressHandler.TryInvoke(progress, out var exception))
                    {
                        Log.Error(exception);
                    }
                }

                AsyncTaskProgress? newProgressHandler = null;

                if (progressHandler != null)
                {
                    newProgressHandler = OnProgress;
                }

                asyncTask.Pin(completionHandler, newProgressHandler, failHandler);
            }
            /// <summary>
            /// Добавить обработчики задачи.
            /// После завершения задачи все обработчики будут сняты.
            /// </summary>
            /// <param name="completionHandler">Обработчик завершения задачи</param>
            /// <param name="failHandler">Обработчик провала задачи</param>
            public void Pin(Action? completionHandler, Action<Response>? failHandler)
            {
                asyncTask.Pin(completionHandler, (AsyncTaskProgress?)null, failHandler);
            }
            /// <summary>
            /// Добавить обработчики задачи.
            /// После завершения задачи обработчик будет снят.
            /// </summary>
            /// <param name="completionHandler">Обработчик завершения задачи</param>
            public void Pin(Action completionHandler)
            {
                asyncTask.Pin(completionHandler, null);
            }
            /// <summary>
            /// Добавить обработчики задачи.
            /// После завершения задачи обработчик будет снят.
            /// </summary>
            /// <param name="failHandler">Обработчик провала задачи</param>
            public void Pin(Action<Response> failHandler)
            {
                asyncTask.Pin(null, failHandler);
            }
            /// <summary>
            /// Добавить обработчики задачи.
            /// После завершения задачи обработчик будет снят.
            /// </summary>
            /// <param name="progressHandler">Обработчик прогресса выполнения задачи</param>
            public void Pin(AsyncTaskProgress progressHandler)
            {
                asyncTask.Pin(null, progressHandler, null);
            }
        }
        extension<T>(IAsyncTask<T> asyncTask)
        {
            /// <summary>
            /// Получает объект типа awaiter, используемый для данного объекта Task.
            /// </summary>
            /// <returns></returns>
            public TaskAwaiter<AsyncTaskAwaitResult<T>> GetAwaiter()
            {
                var taskSource = new TaskCompletionSource<AsyncTaskAwaitResult<T>>();
                var task = taskSource.Task;

                void SetResult()
                {
                    var error = asyncTask.Error;
                    taskSource.SetResult(new(error == null, asyncTask.Result, error));
                }

                asyncTask.Pin(SetResult, _ => SetResult());

                return task.GetAwaiter();
            }
            /// <summary>
            /// Добавить обработчики задачи.
            /// После завершения задачи все обработчики будут сняты.
            /// </summary>
            /// <param name="completionHandler">Обработчик завершения задачи</param>
            /// <param name="progressHandler">Обработчик прогресса выполнения задачи</param>
            /// <param name="failHandler">Обработчик провала задачи</param>
            public void Pin(Action<T>? completionHandler, AsyncTaskProgress? progressHandler, Action<Response>? failHandler)
            {
                asyncTask.Pin(() =>
                {
                    completionHandler?.Invoke(asyncTask.Result!);
                }, progressHandler, failHandler);
            }
            /// <summary>
            /// Добавить обработчики задачи.
            /// После завершения задачи все обработчики будут сняты.
            /// </summary>
            /// <param name="completionHandler">Обработчик завершения задачи</param>
            /// <param name="progressHandler">Обработчик прогресса выполнения задачи</param>
            /// <param name="failHandler">Обработчик провала задачи</param>
            public void Pin(Action<T>? completionHandler, Action<float>? progressHandler, Action<Response>? failHandler)
            {
                void OnProgress(float progress)
                {
                    if (!progressHandler.TryInvoke(progress, out var exception))
                    {
                        Log.Error(exception);
                    }
                }

                AsyncTaskProgress? newProgressHandler = null;

                if (progressHandler != null)
                {
                    newProgressHandler = OnProgress;
                }

                asyncTask.Pin(completionHandler, newProgressHandler, failHandler);
            }
            /// <summary>
            /// Добавить обработчики задачи.
            /// После завершения задачи все обработчики будут сняты.
            /// </summary>
            /// <param name="completionHandler">Обработчик завершения задачи</param>
            /// <param name="failHandler">Обработчик провала задачи</param>
            public void Pin(Action<T>? completionHandler, Action<Response>? failHandler)
            {
                asyncTask.Pin(completionHandler, (AsyncTaskProgress?)null, failHandler);
            }
            /// <summary>
            /// Добавить обработчики задачи.
            /// После завершения задачи обработчик будет снят.
            /// </summary>
            /// <param name="completionHandler">Обработчик завершения задачи</param>
            public void Pin(Action<T> completionHandler)
            {
                asyncTask.Pin(completionHandler, null);
            }
        }
    }
}
