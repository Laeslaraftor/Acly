using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Acly
{
    /// <summary>
    /// Список с отслеживанием изменений
    /// </summary>
    /// <typeparam name="T">Тип данных списка</typeparam>
    [Serializable]
    public class ObservableList<T> : ObservableObject, IObservableList<T>, IReadOnlyCollection<T>, IReadOnlyList<T>
    {
        /// <summary>
        /// Создать новый экземпляр отслеживаемого списка
        /// </summary>
        public ObservableList()
        {
            _list = [];
        }
        /// <summary>
        /// Создать новый экземпляр отслеживаемого списка
        /// </summary>
        /// <param name="items">Список объектов</param>
        public ObservableList(IEnumerable<T> items)
        {
            _list = [.. items];
            Count = _list.Count;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        [field: NonSerialized] public event NotifyCollectionChangedEventHandler? CollectionChanged;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="index"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public virtual T this[int index]
        {
            get => _list[index];
            set
            {
                var item = _list[index];

                if (item?.Equals(value) == true)
                {
                    return;
                }

                _list[index] = value;
                InvokeReplace(value, item, index);
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public int Count
        {
            get => field;
            private set
            {
                if (field != value)
                {
                    OnPropertyChanging(nameof(Count));
                    var oldValue = field;
                    field = value;
                    OnCountChanged(oldValue, value);
                    OnPropertyChanged(nameof(Count));
                }
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public virtual bool IsReadOnly { get; }

        private readonly List<T> _list;

        #region Управление

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="item"><inheritdoc/></param>
        public virtual void Add(T item)
        {
            _list.Add(item);
            InvokeAdd(item);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="index"><inheritdoc/></param>
        /// <param name="item"><inheritdoc/></param>
        public virtual void Insert(int index, T item)
        {
            _list.Insert(index, item);
            InvokeInsert(item, index);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="item"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public virtual bool Remove(T item)
        {
            int index = IndexOf(item);

            if (index >= 0 && _list.Remove(item))
            {
                InvokeRemove(item, index);
                return true;
            }

            return false;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="index"><inheritdoc/></param>
        public virtual void RemoveAt(int index)
        {
            T item = _list[index];

            _list.RemoveAt(index);
            InvokeRemove(item, index);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public virtual void Clear()
        {
            _list.Clear();
            InvokeClear();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="item"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public int IndexOf(T item) => _list.IndexOf(item);
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="item"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public bool Contains(T item) => _list.Contains(item);

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="array"><inheritdoc/></param>
        /// <param name="arrayIndex"><inheritdoc/></param>
        public void CopyTo(T[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);

        private void UpdateCount()
        {
            Count = _list.Count;
        }

        #endregion

        #region Перечисление

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        public virtual IEnumerator<T> GetEnumerator() => _list.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();

        #endregion

        #region События

        /// <summary>
        /// Вызвать событие изменения коллекции
        /// </summary>
        /// <param name="args">Данные события</param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            Dispatch(CollectionChanged, this, args);
        }
        /// <summary>
        /// Событие изменения количества элементов в коллекции
        /// </summary>
        /// <param name="oldValue">Прошлое количество элементов</param>
        /// <param name="newValue">Текущее количество элементов</param>
        protected virtual void OnCountChanged(int oldValue, int newValue)
        {
        }

        private void InvokeReplace(T? newItem, T? oldItem, int index)
        {
            OnCollectionChanged(
                new(NotifyCollectionChangedAction.Replace, newItem, oldItem, index));
        }
        private void InvokeAdd(T? newItem)
        {
            InvokeInsert(newItem, Count - 1);
        }
        private void InvokeRemove(T? newItem, int index)
        {
            OnCollectionChanged(
                new(NotifyCollectionChangedAction.Remove, newItem, index));
            UpdateCount();
        }
        private void InvokeInsert(T? newItem, int index)
        {
            OnCollectionChanged(
                new(NotifyCollectionChangedAction.Add, newItem, index));
            UpdateCount();
        }
        private void InvokeClear()
        {
            OnCollectionChanged(new(NotifyCollectionChangedAction.Reset));
            UpdateCount();
        }

        #endregion
    }
}
