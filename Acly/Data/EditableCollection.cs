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
        public EditableCollection() : this(() => Activator.CreateInstance<T>())
        {
        }
        /// <summary>
        /// Создать новый экземпляр редактируемой коллекции
        /// </summary>
        public EditableCollection(IEnumerable<T> Items) : this(() => Activator.CreateInstance<T>(), Items)
        {
        }
        /// <summary>
        /// Создать новый экземпляр редактируемой коллекции
        /// </summary>
        /// <param name="Fabric">Фабрика элементов списка</param>
        public EditableCollection(Func<T> Fabric)
        {
            _Fabric = Fabric;
            _EditingItemSavedValues = GetEditableProperties();
        }
        /// <summary>
        /// Создать новый экземпляр редактируемой коллекции
        /// </summary>
        public EditableCollection(Func<T> Fabric, IEnumerable<T> Items) : base(Items)
        {
            _Fabric = Fabric;
            _EditingItemSavedValues = GetEditableProperties();
        }

        /// <summary>
        /// Событие добавления, удаления, перемещения объекта списка
        /// </summary>
        [field: NonSerialized] public event EventHandler<CollectionItemEventArgs<T>>? ItemChanged;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool CanAddNew => !IsReadOnly;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool CanCancelEdit => CurrentEditItem != null;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool CanRemove => !IsReadOnly && Count > 0;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public object? CurrentAddItem => CurrentAdd;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public object? CurrentEditItem => CurrentEdit;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool IsAddingNew => CurrentAddItem != null;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool IsEditingItem => CurrentEditItem != null;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public NewItemPosition NewItemPosition { get; set; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="Index"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public override T this[int Index]
        {
            get => base[Index];
            set
            {
                var Item = this[Index];

                if (Item?.Equals(value) == true)
                {
                    return;
                }

                base[Index] = value;

                InvokeItemChanged(CollectionItemAction.Remove, Item);
            }
        }

        /// <summary>
        /// <inheritdoc cref="CurrentAddItem"/>
        /// </summary>
        public T? CurrentAdd { get; private set; }
        /// <summary>
        /// <inheritdoc cref="CurrentEditItem"/>
        /// </summary>
        protected T? CurrentEdit { get; private set; }

        [field: NonSerialized] private readonly Func<T> _Fabric;
        [field: NonSerialized] private readonly Dictionary<PropertyInfo, object?> _EditingItemSavedValues;

        #region Управление

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="Item"><inheritdoc/></param>
        public override void Add(T Item)
        {
            base.Add(Item);
            InvokeItemChanged(CollectionItemAction.Add, Item);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="Index"><inheritdoc/></param>
        /// <param name="Item"><inheritdoc/></param>
        public override void Insert(int Index, T Item)
        {
            List<T> MovedItems = new(Count - Index);

            for (int i = Index; i < Count; i++)
            {
                MovedItems.Add(this[i]);
            }

            base.Insert(Index, Item);

            foreach (var MovedItem in MovedItems)
            {
                InvokeItemChanged(CollectionItemAction.Move, MovedItem);
            }

            MovedItems.Clear();
            MovedItems.Capacity = 0;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void Clear()
        {
            List<T> TempItems = new(this);

            base.Clear();

            foreach (var Item in TempItems)
            {
                InvokeItemChanged(CollectionItemAction.Remove, Item);
            }

            TempItems.Clear();
            TempItems.Capacity = 0;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="Item"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public override bool Remove(T Item)
        {
            if (base.Remove(Item))
            {
                InvokeItemChanged(CollectionItemAction.Remove, Item);
                return true;
            }

            return false;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="Index"><inheritdoc/></param>
        public override void RemoveAt(int Index)
        {
            var Item = this[Index];

            base.RemoveAt(Index);
            InvokeItemChanged(CollectionItemAction.Remove, Item);
        }

        #endregion

        #region Редактирование

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        public object? AddNew()
        {
            var Item = _Fabric();
            CurrentAdd = Item;

            return Item;
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
        /// <param name="Item"><inheritdoc/></param>
        public void EditItem(object Item)
        {
            if (Item is not T TypedItem)
            {
                throw new ArgumentException($"Недопустимый тип объекта!", nameof(Item));
            }

            SaveValues(TypedItem);
            CurrentEdit = TypedItem;
        }

        void IEditableList.Remove(object Item)
        {
            if (Item is not T TypedItem)
            {
                throw new ArgumentException($"Недопустимый тип объекта!", nameof(Item));
            }

            Remove(TypedItem);
        }

        private void SaveValues(T Item)
        {
            foreach (var Property in _EditingItemSavedValues.Keys)
            {
                _EditingItemSavedValues[Property] = Property.GetValue(Item);
            }
        }
        private void RestoreValues(T Item)
        {
            foreach (var Info in _EditingItemSavedValues)
            {
                Info.Key.SetValue(Item, Info.Value);
            }
        }

        #endregion

        #region События

        /// <summary>
        /// Вызвать событие изменения элемента списка
        /// </summary>
        /// <param name="Action"><inheritdoc cref="CollectionItemAction"/></param>
        /// <param name="Item">Изменённый элемент списка</param>
        protected void InvokeItemChanged(CollectionItemAction Action, T Item)
        {
            ItemChanged?.Invoke(this, new(Action, Item));
        }

        #endregion

        #region Статика

        private static Dictionary<PropertyInfo, object?> GetEditableProperties()
        {
            Dictionary<PropertyInfo, object?> Result = new();

            foreach (var Property in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (Property.CanRead && Property.CanWrite)
                {
                    Result.Add(Property, null);
                }
            }

            return Result;
        }

        #endregion
    }
}
