using Newtonsoft.Json;
using System;

namespace Acly.JsonData.Implementations
{
    /// <summary>
    /// JsonConverter по умолчанию
    /// </summary>
    public class DefaultJsonConverter : IJsonConverter
    {
        /// <summary>
        /// Конвертировать JSON строку в указанные объект
        /// </summary>
        /// <typeparam name="T">Объект в который будет конвертирована JSON строка</typeparam>
        /// <param name="json">JSON строка</param>
        /// <returns>Конвертированный объект</returns>
        public T Convert<T>(string json)
        {
            T? result = JsonConvert.DeserializeObject<T>(json);
            result ??= Activator.CreateInstance<T>();

            return result;
        }
        /// <summary>
        /// Конвертировать объект в JSON строку
        /// </summary>
        /// <param name="obj">Объект для конвертации</param>
        /// <returns>JSON строка</returns>
        public string Convert(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        #region Статика

        /// <summary>
        /// Глобальный экземпляр JSON конвертера по умолчанию
        /// </summary>
        public static readonly DefaultJsonConverter Instance = new();

        #endregion
    }
}
