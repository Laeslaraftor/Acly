using System.Collections;

namespace Acly
{
    /// <summary>
    /// Конвертировать элемент коллекции
    /// </summary>
    /// <param name="Value">Элемент коллекции, который необходимо конвертировать</param>
    /// <param name="Index">Индекс конвертируемого элемента в коллекции</param>
    /// <param name="List">Коллекция, содержащая конвертируемый объект</param>
    /// <returns>Конвертированный объект</returns>
    public delegate object? ConvertCollectionValue(object? Value, int Index, IList List);
}
