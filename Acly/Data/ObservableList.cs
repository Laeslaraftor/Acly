using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Acly
{
    /// <summary>
    /// Список с отслеживанием изменений
    /// </summary>
    /// <typeparam name="T">Тип данных списка</typeparam>
    [Serializable]
    public class ObservableList<T> : IObservableList<T>
    {
        /// <summary>
        /// Создать новый экземпляр отслеживаемого списка
        /// </summary>
        public ObservableList()
        {
            _List = new();
        }
        /// <summary>
        /// Создать новый экземпляр отслеживаемого списка
        /// </summary>
        /// <param name="Items">Список объектов</param>
        public ObservableList(IEnumerable<T> Items)
        {
            _List = new(Items);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        [field: NonSerialized] public event PropertyChangedEventHandler? PropertyChanged;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        [field: NonSerialized] public event NotifyCollectionChangedEventHandler? CollectionChanged;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="Index"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public virtual T this[int Index]
        {
            get => _List[Index];
            set
            {
                var Item = _List[Index];

                if (Item?.Equals(value) == true)
                {
                    return;
                }

                _List[Index] = value;
                InvokeReplace(value, Item, Index);
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public int Count => _List.Count;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public virtual bool IsReadOnly { get; }

        private readonly List<T> _List;

        #region Управление
        
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="Item"><inheritdoc/></param>
        public virtual void Add(T Item)
        {
            _List.Add(Item);
            InvokeAdd(Item);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="Index"><inheritdoc/></param>
        /// <param name="Item"><inheritdoc/></param>
        public virtual void Insert(int Index, T Item)
        {
            _List.Insert(Index, Item);
            InvokeInsert(Item, Index);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="Item"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public virtual bool Remove(T Item)
        {
            int Index = IndexOf(Item);

            if (Index >= 0)
            {
                _List.Remove(Item);
                InvokeRemove(Item, Index);
                return true;
            }

            return false;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="Index"><inheritdoc/></param>
        public virtual void RemoveAt(int Index)
        {
            T Item = _List[Index];

            _List.RemoveAt(Index);
            InvokeRemove(Item, Index);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public virtual void Clear()
        {
            _List.Clear();
            InvokeClear();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="Item"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public int IndexOf(T Item) => _List.IndexOf(Item);
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="Item"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public bool Contains(T Item) => _List.Contains(Item);

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="Array"><inheritdoc/></param>
        /// <param name="ArrayIndex"><inheritdoc/></param>
        public void CopyTo(T[] Array, int ArrayIndex) => _List.CopyTo(Array, ArrayIndex);

        #endregion

        #region Перечисление

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        public IEnumerator<T> GetEnumerator() => _List.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _List.GetEnumerator();

        #endregion

        #region События

        /// <summary>
        /// Вызывать событие изменения поля
        /// </summary>
        /// <param name="PropertyName">Изменённое поле</param>
        protected void InvokePropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new(PropertyName));
        }
        /// <summary>
        /// Вызвать событие изменения коллекции
        /// </summary>
        /// <param name="Args">Данные события</param>
        protected void InvokeCollectionChanged(NotifyCollectionChangedEventArgs Args)
        {
            CollectionChanged?.Invoke(this, Args);
            InvokePropertyChanged(nameof(Count));
        }

        private void InvokeReplace(T? NewItem, T? OldItem, int Index)
        {
            InvokeCollectionChanged( 
                new(NotifyCollectionChangedAction.Replace, NewItem, OldItem, Index));
        }
        private void InvokeAdd(T? NewItem)
        {
            InvokeInsert(NewItem, Count - 1);
        }
        private void InvokeRemove(T? NewItem, int Index)
        {
            InvokeCollectionChanged(
                new(NotifyCollectionChangedAction.Remove, NewItem, Index));
        }
        private void InvokeInsert(T? NewItem, int Index)
        {
            InvokeCollectionChanged(
                new(NotifyCollectionChangedAction.Add, NewItem, Index));
        }
        private void InvokeClear()
        {
            InvokeCollectionChanged(new(NotifyCollectionChangedAction.Reset));
        }

        #endregion
    }
}
