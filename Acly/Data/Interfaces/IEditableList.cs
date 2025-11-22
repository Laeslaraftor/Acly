namespace Acly
{
    /// <summary>
    /// Интерфейс изменяемого списка
    /// </summary>
    public interface IEditableList
    {
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
    }
}
