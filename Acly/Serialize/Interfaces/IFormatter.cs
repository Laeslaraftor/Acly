namespace Acly.Serialize
{
    /// <summary>
    /// Интерфейс сериализатора/десериализатора
    /// </summary>
    public interface IFormatter
    {
        /// <summary>
        /// Сериализовать объект
        /// </summary>
        /// <param name="ObjectToSerialize">Объект для сериализации</param>
        /// <returns>Данные сериализованного объекта</returns>
        public byte[] Serialize(object ObjectToSerialize);
        /// <summary>
        /// Десериализовать объект
        /// </summary>
        /// <param name="SerializedData">Данные сериализованного объекта</param>
        /// <returns>Десериализованный объект</returns>
        public T Deserialize<T>(byte[] SerializedData);
    }
}
