using System;
using System.Collections.Generic;
using System.Reflection;

namespace Acly
{
    /// <summary>
    /// Редактируемая коллекция
    /// </summary>
    /// <typeparam name="T">Тип данных списка</typeparam>
    public class EditableCollection<T> : ObservableList<T>, IEditableList
    {
        /// <summary>
        /// Создать новый экземпляр редактируемой коллекции
        /// </summary>
        public EditableCollection() : this(Activator.CreateInstance<T>)
        {
        }
        /// <summary>
        /// Создать новый экземпляр редактируемой коллекции
        /// </summary>
        public EditableCollection(IEnumerable<T> items) : this(Activator.CreateInstance<T>, items)
        {
        }
        /// <summary>
        /// Создать новый экземпляр редактируемой коллекции
        /// </summary>
        /// <param name="fabric">Фабрика элементов списка</param>
        public EditableCollection(Func<T> fabric)
        {
            _fabric = fabric;
            _editingItemSavedValues = GetEditableProperties();
        }
        /// <summary>
        /// Создать новый экземпляр редактируемой коллекции
        /// </summary>
        public EditableCollection(Func<T> fabric, IEnumerable<T> items) : base(items)
        {
            _fabric = fabric;
            _editingItemSavedValues = GetEditableProperties();
        }

        /// <summary>
        /// Событие добавления, удаления, перемещения объекта списка
        /// </summary>
        [field: NonSerialized] public event EventHandler<CollectionItemEventArgs<T>>? ItemChanged;
        event EventHandler<CollectionItemEventArgs>? IEditableList.ItemChanged
        {
            add
            {
                if (value != null)
                {
                    _changeEventHandlers.Add(value);
                }
            }
            remove
            {
                if (value != null)
                {
                    _changeEventHandlers.Remove(value);
                }
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool CanAddNew => !IsReadOnly;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool CanCancelEdit
        {
            get => field;
            private set
            {
                if (field != value)
                {
                    OnPropertyChanging(nameof(CanCancelEdit));
                    field = value;
                    OnPropertyChanged(nameof(CanCancelEdit));
                }
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool CanRemove
        {
            get => field;
            private set
            {
                if (field != value)
                {
                    OnPropertyChanging(nameof(CanRemove));
                    field = value;
                    OnPropertyChanged(nameof(CanRemove));
                }
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public object? CurrentAddItem
        {
            get => field;
            private set
            {
                if (!Equals(field, value))
                {
                    OnPropertyChanging(nameof(CurrentAddItem));
                    field = value;
                    IsAddingNew = value != null;
                    OnPropertyChanged(nameof(CurrentAddItem));
                }
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public object? CurrentEditItem
        {
            get => field;
            private set
            {
                if (!Equals(field, value))
                {
                    OnPropertyChanging(nameof(CurrentEditItem));
                    field = value;
                    IsEditingItem = value != null;
                    CanCancelEdit = value != null;
                    OnPropertyChanged(nameof(CurrentEditItem));
                }
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool IsAddingNew
        {
            get => field;
            private set
            {
                if (field != value)
                {
                    OnPropertyChanging(nameof(IsAddingNew));
                    field = value;
                    OnPropertyChanged(nameof(IsAddingNew));
                }
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool IsEditingItem
        {
            get => field;
            private set
            {
                if (field != value)
                {
                    OnPropertyChanging(nameof(IsEditingItem));
                    field = value;
                    OnPropertyChanged(nameof(IsEditingItem));
                }
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public NewItemPosition NewItemPosition
        {
            get => field;
            set
            {
                if (field != value)
                {
                    OnPropertyChanging(nameof(NewItemPosition));
                    field = value;
                    OnPropertyChanged(nameof(NewItemPosition));
                }
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="index"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public override T this[int index]
        {
            get => base[index];
            set
            {
                var item = this[index];

                if (item?.Equals(value) == true)
                {
                    return;
                }

                base[index] = value;

                if (item != null)
                {
                    InvokeItemChanged(CollectionItemAction.Remove, item);
                }
                if (value != null)
                {
                    InvokeItemChanged(CollectionItemAction.Add, value);
                }
            }
        }
        /// <summary>
        /// <inheritdoc cref="CurrentAddItem"/>
        /// </summary>
        public T? CurrentAdd
        {
            get => field;
            private set
            {
                if (!Equals(field, value))
                {
                    OnPropertyChanging(nameof(CurrentAdd));
                    field = value;
                    CurrentAddItem = value;
                    OnPropertyChanged(nameof(CurrentAdd));
                }
            }
        }
        /// <summary>
        /// <inheritdoc cref="CurrentEditItem"/>
        /// </summary>
        public T? CurrentEdit
        {
            get => field;
            private set
            {
                if (!Equals(field, value))
                {
                    OnPropertyChanging(nameof(CurrentEdit));
                    field = value;
                    CurrentEditItem = value;
                    OnPropertyChanged(nameof(CurrentEdit));
                }
            }
        }

        object? IEditableList.this[int index]
        {
            get => this[index];
            set
            {
                if (value != null && !value.GetType().IsNullable() && value is not T)
                {
                    throw new ArgumentException("Недопустимый тип объекта!", nameof(value));
                }

                this[index] = (T)value!;
            }
        }

        [field: NonSerialized] private readonly Func<T> _fabric;
        [field: NonSerialized] private readonly Dictionary<PropertyInfo, object?> _editingItemSavedValues;
        [field: NonSerialized] private readonly List<EventHandler<CollectionItemEventArgs>> _changeEventHandlers = [];

        #region Управление

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="item"><inheritdoc/></param>
        public override void Add(T item)
        {
            base.Add(item);
            InvokeItemChanged(CollectionItemAction.Add, item);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="index"><inheritdoc/></param>
        /// <param name="item"><inheritdoc/></param>
        public override void Insert(int index, T item)
        {
            base.Insert(index, item);

            HandleMovedItems(index + 1, Count);
            InvokeItemChanged(CollectionItemAction.Add, item);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void Clear()
        {
            List<T> tempItems = [.. this];

            base.Clear();

            foreach (var item in tempItems)
            {
                InvokeItemChanged(CollectionItemAction.Remove, item);
            }

            tempItems.Clear();
            tempItems.Capacity = 0;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="item"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public override bool Remove(T item)
        {
            int itemIndex = IndexOf(item);

            if (base.Remove(item))
            {
                HandleMovedItems(itemIndex, Count);
                InvokeItemChanged(CollectionItemAction.Remove, item);
                return true;
            }

            return false;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="index"><inheritdoc/></param>
        public override void RemoveAt(int index)
        {
            if (0 > index || index >= Count)
            {
                return;
            }

            var item = this[index];

            base.RemoveAt(index);

            HandleMovedItems(index, Count);
            InvokeItemChanged(CollectionItemAction.Remove, item);
        }

        private void HandleMovedItems(int startIndex, int count)
        {
            for (int i = startIndex; i < count; i++)
            {
                InvokeItemChanged(CollectionItemAction.Move, this[i]);
            }
        }

        #endregion

        #region Редактирование

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        public object? AddNew()
        {
            var item = _fabric();
            CurrentAdd = item;

            return item;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="item"><inheritdoc/></param>
        public void AddNew(object? item)
        {
            if (item != null && !item.GetType().IsNullable() && item is not T)
            {
                throw new ArgumentException($"Недопустимый тип объекта! Требуется: {typeof(T).FullName}, получено: {item.GetType().FullName}", nameof(item));
            }

            Add((T)item!);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void CancelEdit()
        {
            if (CurrentEdit == null)
            {
                return;
            }

            RestoreValues(CurrentEdit);
            CurrentEdit = default;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void CancelNew()
        {
            CurrentAdd = default;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void CommitEdit()
        {
            CurrentEdit = default;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void CommitNew()
        {
            if (CurrentAdd == null)
            {
                return;
            }
            if (NewItemPosition == NewItemPosition.AtBeginning && Count > 0)
            {
                Insert(0, CurrentAdd);
                return;
            }

            Add(CurrentAdd);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="item"><inheritdoc/></param>
        public void EditItem(object item)
        {
            if (item is not T typedItem)
            {
                throw new ArgumentException("Недопустимый тип объекта!", nameof(item));
            }

            SaveValues(typedItem);
            CurrentEdit = typedItem;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="item"><inheritdoc/></param>
        /// <param name="index"><inheritdoc/></param>
        /// <exception cref="ArgumentException"></exception>
        public void SetValue(object? item, int index)
        {
            if (item != null && !item.GetType().IsNullable() && item is not T)
            {
                throw new ArgumentException("Недопустимый тип объекта!", nameof(item));
            }

            this[index] = (T)item!;
        }

        bool IEditableList.Contains(object? item)
        {
            if (item != null && !item.GetType().IsNullable() && item is not T)
            {
                return false;
            }

            return Contains((T)item!);
        }
        void IEditableList.Remove(object item)
        {
            if (item is not T typedItem)
            {
                throw new ArgumentException("Недопустимый тип объекта!", nameof(item));
            }

            Remove(typedItem);
        }

        private void SaveValues(T item)
        {
            foreach (var property in _editingItemSavedValues.Keys)
            {
                _editingItemSavedValues[property] = property.GetValue(item);
            }
        }
        private void RestoreValues(T item)
        {
            foreach (var info in _editingItemSavedValues)
            {
                info.Key.SetValue(item, info.Value);
            }
        }

        #endregion

        #region События

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="oldValue"><inheritdoc/></param>
        /// <param name="newValue"><inheritdoc/></param>
        protected override void OnCountChanged(int oldValue, int newValue)
        {
            base.OnCountChanged(oldValue, newValue);
            CanRemove = !IsReadOnly && newValue > 0;
        }

        /// <summary>
        /// Вызвать событие изменения элемента списка
        /// </summary>
        /// <param name="action"><inheritdoc cref="CollectionItemAction"/></param>
        /// <param name="item">Изменённый элемент списка</param>
        protected void InvokeItemChanged(CollectionItemAction action, T item)
        {
            CollectionItemEventArgs args = new(action, item!);

            foreach (var handler in _changeEventHandlers)
            {
                handler(this, args);
            }

            ItemChanged?.Invoke(this, new(action, item));
        }

        #endregion

        #region Статика

        private static Dictionary<PropertyInfo, object?> GetEditableProperties()
        {
            Dictionary<PropertyInfo, object?> result = [];

            foreach (var property in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (property.CanRead && property.CanWrite)
                {
                    result.Add(property, null);
                }
            }

            return result;
        }

        #endregion
    }
}
