using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Acly
{
    /// <summary>
    /// Список объединяющий несколько списков
    /// </summary>
    /// <typeparam name="TList">Тип списков</typeparam>
    /// <typeparam name="T">Тип элемента списка</typeparam>
    public class UnitedCollection<TList, T> : Disposable, IReadOnlyList<T>, INotifyCollectionChanged
        where TList : IReadOnlyList<T>, INotifyCollectionChanged
    {
        /// <summary>
        /// Создать новый экземпляр объединяющего списка
        /// </summary>
        /// <param name="collections">Список списков для объединения</param>
        /// <param name="filter">Фильтр элементов</param>
        public UnitedCollection(IEnumerable<TList> collections, ICollectionItemsFilter? filter = null)
            : this([.. collections], filter)
        {

        }
        /// <summary>
        /// Создать новый экземпляр объединяющего списка
        /// </summary>
        /// <param name="collections">Список списков для объединения</param>
        public UnitedCollection(params TList[] collections)
            : this([.. collections], null)
        {

        }
        /// <summary>
        /// Создать новый экземпляр объединяющего списка
        /// </summary>
        /// <param name="collections">Список списков для объединения</param>
        /// <param name="filter">Фильтр элементов</param>
        public UnitedCollection(ICollectionItemsFilter? filter, params TList[] collections)
            : this([.. collections], filter)
        {

        }
        /// <summary>
        /// Создать новый экземпляр объединяющего списка
        /// </summary>
        /// <param name="collections">Список списков для объединения</param>
        /// <param name="filter">Фильтр элементов</param>
        public UnitedCollection(IList<TList> collections, ICollectionItemsFilter? filter = null)
        {
            Filter = filter;
            Collections = new(collections);

            InitializeCollections();

            filter?.FilterChanged += OnFilterChanged;
            Count = _combinedItems.Count;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="index"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public T this[int index] => _combinedItems[index];
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
                    field = value;
                    OnPropertyChanged(nameof(Count));
                }
            }
        }
        /// <summary>
        /// Список коллекций элементов
        /// </summary>
        public ReadOnlyCollection<TList> Collections { get; }
        /// <summary>
        /// Фильтр элементов
        /// </summary>
        public ICollectionItemsFilter? Filter { get; }

        private readonly List<T> _combinedItems = [];
        private readonly Dictionary<TList, List<T>> _filteredItems = [];

        #region Управление

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="isDisposing"><inheritdoc/></param>
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            Filter?.FilterChanged -= OnFilterChanged;

            foreach (var collection in Collections)
            {
                collection.CollectionChanged -= OnSourceCollectionChanged;
            }

            _filteredItems.Clear();
            _combinedItems.Clear();
        }

        private void InitializeCollections()
        {
            foreach (var collection in Collections)
            {
                collection.CollectionChanged -= OnSourceCollectionChanged;
                collection.CollectionChanged += OnSourceCollectionChanged;
                ProcessCollectionItems(collection);
            }

            UpdateCombinedItems();
        }
        private void ProcessCollectionItems(TList collection)
        {
            if (!_filteredItems.TryGetValue(collection, out var filtered))
            {
                filtered = [];
                _filteredItems.Add(collection, filtered);
            }

            filtered.Clear();

            foreach (var item in collection)
            {
                if (Filter == null || Filter.Check(collection, item!))
                {
                    filtered.Add(item);
                }
            }
        }
        private void UpdateCombinedItems()
        {
            _combinedItems.Clear();

            foreach (var filtered in _filteredItems.Values)
            {
                _combinedItems.AddRange(filtered);
            }
        }
        private int GetCombinedIndexForCollectionItem(TList collection, int collectionIndex)
        {
            int combinedIndex = 0;

            foreach (var list in Collections)
            {
                if (list.Equals(collection))
                {
                    return combinedIndex + collectionIndex;
                }

                if (_filteredItems.TryGetValue(list, out var filtered))
                {
                    combinedIndex += filtered.Count;
                }
            }

            return -1;
        }

        #endregion

        #region Перечисление

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        public IEnumerator<T> GetEnumerator() => _combinedItems.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        #region События

        private void OnSourceCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender is not TList collection ||
                !_filteredItems.TryGetValue(collection, out var filteredItems))
            {
                return;
            }

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null)
                    {
                        List<T> newFilteredItems = [];

                        foreach (T newItem in e.NewItems)
                        {
                            if (Filter == null || Filter.Check(collection, newItem!))
                            {
                                newFilteredItems.Add(newItem);
                            }
                        }

                        if (newFilteredItems.Count > 0)
                        {
                            int insertIndex;

                            if (e.NewStartingIndex < filteredItems.Count)
                            {
                                insertIndex = e.NewStartingIndex;
                            }
                            else
                            {
                                insertIndex = filteredItems.Count;
                            }

                            filteredItems.InsertRange(insertIndex, newFilteredItems);
                            UpdateCombinedItems();

                            var combinedIndex = GetCombinedIndexForCollectionItem(collection, insertIndex);

                            if (combinedIndex >= 0)
                            {
                                OnCollectionChanged(NotifyCollectionChangedAction.Add,
                                    newFilteredItems, combinedIndex);
                            }
                        }
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems != null)
                    {
                        List<T> removedItems = [];
                        int firstItemIndex = -1;

                        foreach (T oldItem in e.OldItems)
                        {
                            var index = filteredItems.IndexOf(oldItem);

                            if (index >= 0)
                            {
                                firstItemIndex = index;
                                removedItems.Add(filteredItems[index]);
                                filteredItems.RemoveAt(index);
                            }
                        }

                        if (removedItems.Count > 0)
                        {
                            UpdateCombinedItems();
                            OnCollectionChanged(NotifyCollectionChangedAction.Remove, removedItems, firstItemIndex);
                        }
                    }
                    break;

                case NotifyCollectionChangedAction.Replace:
                    if (e.NewItems != null && e.OldItems != null)
                    {
                        var oldIndex = filteredItems.IndexOf((T)e.OldItems[0]);

                        if (oldIndex >= 0)
                        {
                            var oldItem = filteredItems[oldIndex];
                            filteredItems.RemoveAt(oldIndex);
                            var newItem = (T)e.NewItems[0];

                            if (Filter == null || Filter.Check(collection, newItem))
                            {
                                filteredItems.Insert(oldIndex, newItem);
                                UpdateCombinedItems();

                                var combinedIndex = GetCombinedIndexForCollectionItem(collection, oldIndex);

                                if (combinedIndex >= 0)
                                {
                                    OnCollectionChanged(NotifyCollectionChangedAction.Replace,
                                        newItem: newItem, oldItem: oldItem, index: combinedIndex);
                                }
                            }
                            else
                            {
                                UpdateCombinedItems();
                                OnCollectionChanged(NotifyCollectionChangedAction.Remove,
                                    new[] { oldItem });
                            }
                        }
                        else
                        {
                            var newItem = (T)e.NewItems[0];

                            if (Filter == null || Filter.Check(collection, newItem!))
                            {
                                var insertIndex = e.NewStartingIndex < filteredItems.Count
                                    ? e.NewStartingIndex
                                    : filteredItems.Count;

                                filteredItems.Insert(insertIndex, newItem);
                                UpdateCombinedItems();

                                var combinedIndex = GetCombinedIndexForCollectionItem(collection, insertIndex);

                                if (combinedIndex >= 0)
                                {
                                    OnCollectionChanged(NotifyCollectionChangedAction.Add,
                                        new[] { newItem }, combinedIndex);
                                }
                            }
                        }
                    }
                    break;

                case NotifyCollectionChangedAction.Move:
                    // Note: This is a simplified implementation
                    // A full implementation would need to handle filtering in move operations
                    UpdateCombinedItems();
                    OnCollectionChanged(NotifyCollectionChangedAction.Reset);
                    break;

                case NotifyCollectionChangedAction.Reset:
                    ProcessCollectionItems(collection);
                    UpdateCombinedItems();
                    OnCollectionChanged(NotifyCollectionChangedAction.Reset);
                    break;
            }
        }

        private void OnFilterChanged(object? sender, EventArgs e)
        {
            foreach (var collection in Collections)
            {
                ProcessCollectionItems(collection);
            }

            UpdateCombinedItems();
            OnCollectionChanged(NotifyCollectionChangedAction.Reset);
        }

        /// <summary>
        /// Вызвать событие изменения коллекции
        /// </summary>
        /// <param name="action">Действие, изменившее коллекцию</param>
        /// <param name="changedItems">Список изменённых элементов коллекции</param>
        /// <param name="index">Индекс изменённого элемента</param>
        /// <param name="oldItem">Старый элемент коллекции</param>
        /// <param name="newItem">Новый элемент коллекции</param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedAction action, IList? changedItems = null,
            int index = -1, object? oldItem = null, object? newItem = null)
        {
            if (changedItems != null)
            {
                NotifyCollectionChangedEventArgs args;

                if (action == NotifyCollectionChangedAction.Add ||
                    action == NotifyCollectionChangedAction.Remove)
                {
                    args = new(action, changedItems, index);
                }
                else
                {
                    args = new(action, changedItems);
                }

                Dispatch(CollectionChanged, this, args);
            }
            else if (oldItem != null && newItem != null && index >= 0)
            {
                Dispatch(CollectionChanged, this, new NotifyCollectionChangedEventArgs(action, newItem, oldItem, index));
            }
            else
            {
                Dispatch(CollectionChanged, this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }

            Count = _combinedItems.Count;
        }

        #endregion
    }
}
