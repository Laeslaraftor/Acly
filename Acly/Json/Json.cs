using Acly.JsonData.Implementations;
using System;
using System.Threading.Tasks;

namespace Acly.JsonData
{
    /// <summary>
    /// Класс для работы с JSON данными
    /// </summary>
    public static class Json
    {
        /// <summary>
        /// Тип медиа
        /// </summary>
        public const string MediaType = "application/json";

        private static IJsonConverter? _customConverter;

        #region Управление

        /// <summary>
        /// Конвертировать JSON в объект указанного типа
        /// </summary>
        /// <typeparam name="T">Тип объекта в который будет конвертирован JSON</typeparam>
        /// <param name="json">JSON строка</param>
        /// <returns>Конвертированные объект</returns>
        public static async Task<T> Convert<T>(string json)
        {
            IJsonConverter converter = await GetConverter();
            return converter.Convert<T>(json);
        }
        /// <summary>
        /// Конвертировать объект в JSON строку
        /// </summary>
        /// <param name="obj">Объект для конвертации в строку</param>
        /// <returns>JSON строка</returns>
        public static async Task<string> Convert(object obj)
        {
            IJsonConverter converter = await GetConverter();
            return converter.Convert(obj);
        }

        #endregion

        #region Получение реализации

        private static async Task<IJsonConverter> GetConverter()
        {
            _customConverter ??= await GetCustomImplementation();

            if (_customConverter == null)
            {
                return DefaultJsonConverter.Instance;
            }

            return _customConverter;
        }
        private static async Task<IJsonConverter?> GetCustomImplementation()
        {
            Type? implementation = await JsonImplementations.GetConverterType();

            if (implementation == null)
            {
                return null;
            }
            if (!implementation.IsImplementsInterface<IJsonConverter>())
            {
                throw new JsonConverterImplementationException(implementation);
            }

            return (IJsonConverter)Activator.CreateInstance(implementation);
        }

        #endregion
    }
}
