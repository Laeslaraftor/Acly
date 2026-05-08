using System;

namespace Acly
{
    /// <summary>
    /// Объект, логирующий свои события
    /// </summary>
    public class LogObject
    {
        /// <summary>
        /// Событие отправки исключения
        /// </summary>
        [field: NonSerialized]
        public event EventHandler<ExceptionSendEventArgs>? ExceptionSent;

        #region Управление

        /// <summary>
        /// Записать сообщение
        /// </summary>
        /// <param name="value">Значение сообщения</param>
        protected virtual void LogMessage(object? value)
        {
            if (value != null)
            {
                OnLogged(value);
            }
        }
        /// <summary>
        /// Записать предупреждение
        /// </summary>
        /// <param name="value">Значение предупреждения</param>
        protected virtual void LogWarning(object? value)
        {
            if (value != null)
            {
                OnLoggedWarning(value);
            }
        }
        /// <summary>
        /// Записать ошибку
        /// </summary>
        /// <param name="value">Значение ошибки</param>
        protected virtual void LogError(object? value)
        {
            if (value != null)
            {
                OnLoggedError(value);
            }
        }

        /// <summary>
        /// Попытаться выполнить действие и в случае неудачи записать исключение
        /// </summary>
        /// <param name="action">Действие, которое надо попытаться выполнить</param>
        protected void TryExecute(Action action)
        {
            try
            {
                action();
            }
            catch (Exception exception)
            {
                LogError(exception);
            }
        }
        /// <summary>
        /// Попытаться выполнить действие и в случае неудачи записать исключение
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">Действие, которое надо попытаться выполнить</param>
        /// <returns>Результат действия</returns>
        protected T? TryExecute<T>(Func<T> func)
        {
            try
            {
                return func();
            }
            catch (Exception exception)
            {
                LogError(exception);
            }

            return default;
        }

        #endregion

        #region События

        /// <summary>
        /// Событие записи сообщения
        /// </summary>
        /// <param name="value">Значение сообщения</param>
        protected virtual void OnLogged(object value)
        {
            Log.Error(value);
        }
        /// <summary>
        /// Событие записи предупреждения
        /// </summary>
        /// <param name="value">Значение предупреждения</param>
        protected virtual void OnLoggedWarning(object value)
        {
            Log.Warning(value);
        }
        /// <summary>
        /// Событие записи ошибки
        /// </summary>
        /// <param name="value">Значение ошибки</param>
        protected virtual void OnLoggedError(object value)
        {
            Log.Error(value);

            if (value is Exception exception)
            {
                OnExceptionSent(exception);
            }
        }
        /// <summary>
        /// Событие отправки исключения
        /// </summary>
        /// <param name="exception"></param>
        protected virtual void OnExceptionSent(Exception exception)
        {
            ExceptionSent?.Invoke(this, new(exception));
        }

        #endregion
    }
}
