using System;

namespace Acly.Tasks
{
    /// <summary>
    /// Результат выполнения асинхронной задачи
    /// </summary>
    public readonly struct AsyncTaskAwaitResult<T>(bool isSuccess, T? result, IAsyncTaskError? error)
        : IEquatable<AsyncTaskAwaitResult<T>>
    {
        /// <summary>
        /// Создать результат асинхронной задачи
        /// </summary>
        public AsyncTaskAwaitResult(T result)
            : this(true, result, null)
        {
        }
        /// <summary>
        /// Создать результат асинхронной задачи
        /// </summary>
        public AsyncTaskAwaitResult(IAsyncTaskError error)
            : this(false, default, error)
        {
        }

        /// <summary>
        /// Успешно ли выполнена задача
        /// </summary>
        public bool IsSuccess { get; } = isSuccess;
        /// <summary>
        /// Результат выполненной задачи
        /// </summary>
        public T? Result { get; } = result;
        /// <summary>
        /// Ошибка, возникшая при выполнении задачи (если есть)
        /// </summary>
        public IAsyncTaskError? Error { get; } = error;

        #region Управление

        /// <summary>
        /// Получить результат выполнения задачи.
        /// Если во время выполнения задачи возникло исключение, то именно оно будет выброшено.
        /// </summary>
        /// <returns>Результат выполнения задачи</returns>
        public T GetCompletionResult()
        {
            if (Error != null)
            {
                throw Error.Exception;
            }

            return Result!;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="obj"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public override bool Equals(object obj)
        {
            return obj is AsyncTaskAwaitResult<T> other &&
                   Equals(other);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="other"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public bool Equals(AsyncTaskAwaitResult<T> other)
        {
            return IsSuccess == other.IsSuccess &&
                   Equals(Result, other.Result) &&
                   Error == other.Error;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(IsSuccess, Error);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="left"><inheritdoc/></param>
        /// <param name="right"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public static bool operator ==(AsyncTaskAwaitResult<T> left, AsyncTaskAwaitResult<T> right)
        {
            return left.Equals(right);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="left"><inheritdoc/></param>
        /// <param name="right"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public static bool operator !=(AsyncTaskAwaitResult<T> left, AsyncTaskAwaitResult<T> right)
        {
            return !(left == right);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="result"><inheritdoc/></param>
        public static implicit operator AsyncTaskAwaitResult<T>(T result)
        {
            return new(result);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="result"><inheritdoc/></param>
        public static implicit operator T(AsyncTaskAwaitResult<T> result)
        {
            return result.GetCompletionResult();
        }

        #endregion
    }
}
