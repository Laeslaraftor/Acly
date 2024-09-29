using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace Acly
{
    /// <summary>
    /// Список с отслеживанием изменений
    /// </summary>
    /// <typeparam name="T">Тип данных списка</typeparam>
    [Serializable]
    public class ObservableList<T> : IList<T>, INotifyCollectionChanged
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
        [field: NonSerialized] public event NotifyCollectionChangedEventHandler? CollectionChanged;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="Index"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public T this[int Index]
        {
            get => _List[Index];
            set => _List[Index] = value;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public int Count => _List.Count;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool IsReadOnly { get; }

        private readonly List<T> _List;

        #region Управление
        
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="Item"><inheritdoc/></param>
        public void Add(T Item)
        {
            _List.Add(Item);
            InvokeChanged();
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="Index"><inheritdoc/></param>
        /// <param name="Item"><inheritdoc/></param>
        public void Insert(int Index, T Item)
        {
            _List.Insert(Index, Item);
            InvokeChanged();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="Item"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public bool Remove(T Item)
        {
            bool Result = _List.Remove(Item);

            if (Result)
            {
                InvokeChanged();
            }

            return Result;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="Index"><inheritdoc/></param>
        public void RemoveAt(int Index)
        {
            _List.RemoveAt(Index);
            InvokeChanged();
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Clear()
        {
            _List.Clear();
            InvokeChanged();
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

        private void InvokeChanged()
        {
            CollectionChanged?.Invoke(this, new(NotifyCollectionChangedAction.Reset));
        }

        #endregion
    }
}
