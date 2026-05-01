using System.Collections;

namespace Acly
{
    /// <summary>
    /// Конвертировать элемент коллекции
    /// </summary>
    /// <param name="value">Элемент коллекции, который необходимо конвертировать</param>
    /// <param name="index">Индекс конвертируемого элемента в коллекции</param>
    /// <param name="list">Коллекция, содержащая конвертируемый объект</param>
    /// <returns>Конвертированный объект</returns>
    public delegate object? ConvertCollectionValue(object? value, int index, IList list);
}
