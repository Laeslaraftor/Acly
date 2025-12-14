using System.Collections;

namespace Acly
{
    /// <summary>
    /// Интерфейс конвертера значений списков
    /// </summary>
    /// <typeparam name="T1">Тип значения 1</typeparam>
    /// <typeparam name="T2">Тип значения 2</typeparam>
    public interface ICollectionValueConverter<T1, T2>
    {
        /// <summary>
        /// Конвертировать значение типа 1 в значение типа 2
        /// </summary>
        /// <param name="Value">Значение типа 1</param>
        /// <param name="Index">Индекс значения в первом списке. По этому же индексу будет записано конвертированное значение во второй список</param>
        /// <param name="FirstCollection">Список, содержащий конвертируемое значение</param>
        /// <param name="SecondCollection">Список в который будет записано конвертированное значение</param>
        /// <returns>Конвертированное значение типа 2</returns>
        public T2 Convert(T1 Value, int Index, IList FirstCollection, IList SecondCollection);
        /// <summary>
        /// Конвертировать значение типа 2 в значение типа 1
        /// </summary>
        /// <param name="Value">Значение типа 2</param>
        /// <param name="Index">Индекс значения во втором списке. По этому же индексу будет записано конвертированное значение в первый список</param>
        /// <param name="FirstCollection">Список в который будет записано конвертированное значение</param>
        /// <param name="SecondCollection">Список, содержащий конвертируемое значение</param>
        /// <returns>Конвертированное значение типа 1</returns>
        public T1 ConvertBack(T2 Value, int Index, IList FirstCollection, IList SecondCollection);
    }
}
