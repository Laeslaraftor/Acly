using System;
using System.Collections;

namespace Acly
{
    /// <summary>
    /// Интерфейс фильтра списка
    /// </summary>
    public interface ICollectionItemsFilter
    {
        /// <summary>
        /// Событие изменения фильтра
        /// </summary>
        public event EventHandler? FilterChanged;

        /// <summary>
        /// Проверить проходит ли элемент фильтры
        /// </summary>
        /// <param name="Collection">Коллекция, содержащая элемент</param>
        /// <param name="Item">Элемент, который надо проверить</param>
        /// <returns>Проходит ли элемент фильтры</returns>
        public bool Check(IEnumerable Collection, object Item);
    }
}
