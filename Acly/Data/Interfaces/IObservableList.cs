using System.Collections.Generic;
using System.Collections.Specialized;

namespace Acly
{
    /// <summary>
    /// Список с отслеживанием изменений
    /// </summary>
    /// <typeparam name="T">Тип данных списка</typeparam>
    public interface IObservableList<T> : IList<T>, INotifyCollectionChanged
    {
    }
}
