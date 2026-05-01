using System;

namespace Acly.Tasks
{
    /// <summary>
    /// Результат выполнения асинхронной задачи
    /// </summary>
    public readonly struct AsyncTaskAwaitResult(bool isSuccess, IAsyncTaskError? error)
        : IEquatable<AsyncTaskAwaitResult>
    {
        /// <summary>
        /// Создать результат асинхронной задачи
        /// </summary>
        public AsyncTaskAwaitResult(bool isSuccess)
            : this(isSuccess, null)
        {
        }
        /// <summary>
        /// Создать результат асинхронной задачи
        /// </summary>
        public AsyncTaskAwaitResult(IAsyncTaskError error)
            : this(false, error)
        {
        }

        /// <summary>
        /// Успешно ли выполнена задача
        /// </summary>
        public bool IsSuccess { get; } = isSuccess;
        /// <summary>
        /// Ошибка, возникшая при выполнении задачи (если есть)
        /// </summary>
        public IAsyncTaskError? Error { get; } = error;

        #region Управление

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="obj"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public override bool Equals(object obj)
        {
            return obj is AsyncTaskAwaitResult other &&
                   Equals(other);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="other"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public bool Equals(AsyncTaskAwaitResult other)
        {
            return IsSuccess == other.IsSuccess &&
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
        public static bool operator ==(AsyncTaskAwaitResult left, AsyncTaskAwaitResult right)
        {
            return left.Equals(right);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="left"><inheritdoc/></param>
        /// <param name="right"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public static bool operator !=(AsyncTaskAwaitResult left, AsyncTaskAwaitResult right)
        {
            return !(left == right);
        }

        #endregion
    }
}
