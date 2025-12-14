using System;
using System.Collections;

namespace Acly
{
    /// <summary>
    /// Интерфейс изменяемого списка
    /// </summary>
#pragma warning disable CA1010 // Также необходимо реализовать универсальный интерфейс
    public interface IEditableList : IEnumerable
#pragma warning restore CA1010 // Также необходимо реализовать универсальный интерфейс
    {
        /// <summary>
        /// Событие добавления, удаления, перемещения объекта списка
        /// </summary>
        public event EventHandler<CollectionItemEventArgs>? ItemChanged;

        /// <summary>
        /// Возвращает значение, указывающее, можно ли добавить новый элемент в коллекцию.
        /// </summary>
        public bool CanAddNew { get; }
        /// <summary>
        /// Возвращает значение, указывающее, может ли представление коллекции отклонить отложенные изменения и восстановить исходные значения изменяемого объекта.
        /// </summary>
        public bool CanCancelEdit { get; }
        /// <summary>
        /// Возвращает значение, указывающее, можно ли удалить элемент из коллекции.
        /// </summary>
        public bool CanRemove { get; }
        /// <summary>
        /// Возвращает элемент, добавляемый во время текущей транзакции добавления.
        /// </summary>
        public object? CurrentAddItem { get; }
        /// <summary>
        /// Возвращает изменяемый элемент коллекции.
        /// </summary>
        public object? CurrentEditItem { get; }
        /// <summary>
        /// Возвращает значение, указывающее, выполняется ли в данный момент транзакция добавления.
        /// </summary>
        public bool IsAddingNew { get; }
        /// <summary>
        /// Возвращает значение, указывающее, выполняется ли в данный момент транзакция изменения.
        /// </summary>
        public bool IsEditingItem { get; }
        /// <summary>
        /// Возвращает или задает положение местозаполнителя нового элемента в представлении коллекции.
        /// </summary>
        public NewItemPosition NewItemPosition { get; set; }

        /// <summary>
        /// Добавляет новый элемент в коллекцию.
        /// </summary>
        public object? AddNew();
        /// <summary>
        /// Добавляет новый элемент в коллекцию.
        /// </summary>
        /// <param name="Item">Объект, который надо добавить</param>
        public void AddNew(object? Item);
        /// <summary>
        /// Завершает транзакцию изменения и, если это возможно, восстанавливает исходное значение для элемента.
        /// </summary>
        public void CancelEdit();
        /// <summary>
        /// Завершает транзакцию добавления и отменяет ожидающий новый элемент.
        /// </summary>
        public void CancelNew();
        /// <summary>
        /// Завершает транзакцию изменения и сохраняет отложенные изменения.
        /// </summary>
        public void CommitEdit();
        /// <summary>
        /// Завершает транзакцию добавления и сохраняет ожидаемый новый элемент.
        /// </summary>
        public void CommitNew();
        /// <summary>
        /// Начинает транзакцию изменения заданного элемента.
        /// </summary>
        public void EditItem(object Item);
        /// <summary>
        /// Удаляет заданный элемент из коллекции.
        /// </summary>
        public void Remove(object Item);
        /// <summary>
        /// Удаляет элемент в указанной позиции из коллекции.
        /// </summary>
        public void RemoveAt(int Index);
        /// <summary>
        /// Задать значение по индексу
        /// </summary>
        /// <param name="Item">Новое значение</param>
        /// <param name="Index">Индекс значения</param>
        public void SetValue(object? Item, int Index);
    }
}
