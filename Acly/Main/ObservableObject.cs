using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Acly
{
    /// <summary>
    /// Базовый класс объекта с отслеживанием изменением полей
    /// </summary>
    [Serializable]
    public abstract class ObservableObject : INotifyPropertyChanging, INotifyPropertyChanged
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangingEventHandler? PropertyChanging;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Этот планировщик будет использоваться при вызове событий <see cref="PropertyChanged"/> и <see cref="PropertyChanging"/>.
        /// По умолчанию ссылается на <see cref="SharedDispatcher"/>
        /// </summary>
        protected virtual IDispatcher? Dispatcher => SharedDispatcher;

        #region Управление

        /// <summary>
        /// Выполнить действие через планировщик, если его нет, то действие будет выполнено как обычно
        /// </summary>
        /// <param name="action">Действие, которое надо выполнить</param>
        /// <exception cref="ArgumentNullException"></exception>
        protected async void Dispatch(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var dispatcher = Dispatcher;

            if (dispatcher == null)
            {
                action();
                return;
            }

            dispatcher.Dispatch(action);
        }
        /// <summary>
        /// Выполнить действие через планировщик, если его нет, то действие будет выполнено как обычно
        /// </summary>
        /// <param name="action">Действие, которое надо выполнить</param>
        /// <exception cref="ArgumentNullException"></exception>
        protected async Task DispatchAsync(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var dispatcher = Dispatcher;

            if (dispatcher == null)
            {
                action();
                return;
            }

            await dispatcher.DispatchAsync(action);
        }
        /// <summary>
        /// Вызвать событие
        /// </summary>
        /// <typeparam name="T">Тип делегата события</typeparam>
        /// <param name="eventDelegate">Метод события</param>
        /// <param name="args">Аргументы вызова</param>
        protected void Dispatch<T>(T? eventDelegate, params object?[]? args)
            where T : Delegate
        {
            if (eventDelegate != null)
            {
                Dispatch(() =>
                {
                    eventDelegate.DynamicInvoke(args);
                });
            }
        }
        /// <summary>
        /// Вызвать событие
        /// </summary>
        /// <typeparam name="T">Тип делегата события</typeparam>
        /// <param name="eventDelegate">Метод события</param>
        protected void Dispatch<T>(T? eventDelegate)
            where T : Delegate
        {
            Dispatch(eventDelegate, null);
        }
        /// <summary>
        /// Вызвать событие, в качестве аргумента которого будет предоставлен <see cref="EventArgs.Empty"/>
        /// </summary>
        /// <param name="eventHandler">Метод события</param>
        protected void Dispatch(EventHandler? eventHandler)
        {
            Dispatch(eventHandler, this, EventArgs.Empty);
        }

        #endregion

        #region События

        /// <summary>
        /// Событие изменения поля
        /// </summary>
        /// <param name="propertyName">Название поля, которое изменило своё значение</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventArgs args = GetPropertyChangedEventArgs(propertyName);
            OnPropertyChanged(args);
        }
        /// <summary>
        /// Событие изменения поля
        /// </summary>
        /// <param name="e">Аргументы события изменения поля</param>
        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            Dispatch(PropertyChanged, this, e);
        }
        /// <summary>
        /// Событие начала изменения поля
        /// </summary>
        /// <param name="propertyName">Название поля, которое начало менять значение</param>
        protected virtual void OnPropertyChanging(string propertyName)
        {
            var args = GetPropertyChangingEventArgs(propertyName); 
            OnPropertyChanging(args);
        }
        /// <summary>
        /// Событие начала изменения поля
        /// </summary>
        /// <param name="e">Аргументы события начала изменения поля</param>
        protected void OnPropertyChanging(PropertyChangingEventArgs e)
        {
            Dispatch(PropertyChanging, this, e);
        }

        #endregion

        #region Статика

        /// <summary>
        /// Глобальный экземпляр планировщика. 
        /// Этот планировщик будет использоваться при вызове событий <see cref="PropertyChanged"/> и <see cref="PropertyChanging"/>
        /// </summary>
        public static IDispatcher? SharedDispatcher { get; set; }

        private static readonly Dictionary<string, PropertyChangingEventArgs> _changingEventArgs = [];
        private static readonly Dictionary<string, PropertyChangedEventArgs> _changedEventArgs = [];

        /// <summary>
        /// Получить аргументы события начала изменения поля
        /// </summary>
        /// <param name="propertyName">Название изменяемого поля</param>
        /// <returns>Аргументы события начала изменения поля</returns>
        public static PropertyChangingEventArgs GetPropertyChangingEventArgs(string propertyName)
        {
            lock (_changingEventArgs)
            {
                return GetEventArgs(propertyName, _changingEventArgs, () => new(propertyName));
            }
        }
        /// <summary>
        /// Получить аргументы события изменения поля
        /// </summary>
        /// <param name="propertyName">Название изменённого поля</param>
        /// <returns>Аргументы события изменения поля</returns>
        public static PropertyChangedEventArgs GetPropertyChangedEventArgs(string propertyName)
        {
            lock (_changedEventArgs)
            {
                return GetEventArgs(propertyName, _changedEventArgs, () => new(propertyName));
            }
        }

        private static T GetEventArgs<T>(string propertyName, Dictionary<string, T> dictionary, Func<T> fabric)
        {
            if (!dictionary.TryGetValue(propertyName, out var args))
            {
                args = fabric();
                dictionary.Add(propertyName, args);
            }

            return args;
        }

        #endregion
    }
}
