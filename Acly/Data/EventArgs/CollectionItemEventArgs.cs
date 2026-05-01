using System;

namespace Acly
{
    /// <summary>
    /// Информация события изменения элемента списка
    /// </summary>
    /// <typeparam name="T">Тип элемента списка</typeparam>
    /// <remarks>
    /// Создать новый экземпляр информации события изменения элемента списка
    /// </remarks>
    /// <param name="action"><inheritdoc cref="Action"/></param>
    /// <param name="item"><inheritdoc cref="Item"/></param>
    public class CollectionItemEventArgs<T>(CollectionItemAction action, T item) : EventArgs
    {

        /// <summary>
        /// <inheritdoc cref="CollectionItemAction"/>
        /// </summary>
        public CollectionItemAction Action { get; } = action;
        /// <summary>
        /// Изменённый элемент списка
        /// </summary>
        public T Item { get; } = item;
    }
    /// <summary>
    /// Информация события изменения элемента списка
    /// </summary>
    /// <remarks>
    /// <inheritdoc/>
    /// </remarks>
    public class CollectionItemEventArgs(CollectionItemAction action, object item)
        : CollectionItemEventArgs<object>(action, item)
    {
    }
}
