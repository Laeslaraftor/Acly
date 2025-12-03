namespace Acly.Execution
{
    /// <summary>
    /// Типы данных в байт-коде
    /// </summary>
#pragma warning disable CA1028 // Хранилище перечислений должно относиться к типу Int32
    public enum CodeDataType : byte
#pragma warning restore CA1028 // Хранилище перечислений должно относиться к типу Int32
    {
        /// <summary>
        /// Булево значение
        /// </summary>
        Bool,
        /// <summary>
        /// 8 битное число (байт)
        /// </summary>
        Byte,
        /// <summary>
        /// 16 битное число
        /// </summary>
        Short,
        /// <summary>
        /// 32 битное число
        /// </summary>
        Int32,
        /// <summary>
        /// 64 битное число
        /// </summary>
        Int64,
        /// <summary>
        /// <inheritdoc cref="System.Double"/>
        /// </summary>
        Double,
        /// <summary>
        /// <inheritdoc cref="System.Single"/>
        /// </summary>
        Float,
        /// <summary>
        /// Символ
        /// </summary>
        Char,
        /// <summary>
        /// Строка
        /// </summary>
        String
    }
}
