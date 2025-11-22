using System;

namespace Acly
{
    /// <summary>
    /// Информация события изменения элемента списка
    /// </summary>
    /// <typeparam name="T">Тип элемента списка</typeparam>
    public class CollectionItemEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Создать новый экземпляр информации события изменения элемента списка
        /// </summary>
        /// <param name="Action"><inheritdoc cref="Action"/></param>
        /// <param name="Item"><inheritdoc cref="Item"/></param>
        public CollectionItemEventArgs(CollectionItemAction Action, T Item)
        {
            this.Action = Action;
            this.Item = Item;
        }

        /// <summary>
        /// <inheritdoc cref="CollectionItemAction"/>
        /// </summary>
        public CollectionItemAction Action { get; }
        /// <summary>
        /// Изменённый элемент списка
        /// </summary>
        public T Item { get; }
    }
    /// <summary>
    /// Информация события изменения элемента списка
    /// </summary>
    public class CollectionItemEventArgs : CollectionItemEventArgs<object>
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public CollectionItemEventArgs(CollectionItemAction Action, object Item) : base(Action, Item)
        {
        }
    }
}
