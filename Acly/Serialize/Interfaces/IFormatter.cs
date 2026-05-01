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
        /// <param name="objectToSerialize">Объект для сериализации</param>
        /// <returns>Данные сериализованного объекта</returns>
        public byte[] Serialize(object objectToSerialize);
        /// <summary>
        /// Десериализовать объект
        /// </summary>
        /// <param name="serializedData">Данные сериализованного объекта</param>
        /// <returns>Десериализованный объект</returns>
        public T Deserialize<T>(byte[] serializedData);
    }
}
