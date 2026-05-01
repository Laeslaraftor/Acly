using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Acly.JsonData.Implementations
{
    /// <summary>
    /// Класс для поиска реализации <see cref="IJsonConverter"/>
    /// </summary>
    public static class JsonImplementations
    {
        private static IEnumerable<Type>? _implementations;

        /// <summary>
        /// Получить тип объекта, помеченного как реализация <see cref="IJsonConverter"/>
        /// </summary>
        /// <returns>Тип объекта, помеченного как реализация <see cref="IJsonConverter"/></returns>
        public static async Task<Type?> GetConverterType()
        {
            IEnumerable<Type> implementations = await GetImplementations();

            foreach (var type in implementations)
            {
                return type;
            }

            return null;
        }

        private static async Task<IEnumerable<Type>> GetImplementations()
        {
            if (_implementations != null)
            {
                return _implementations;
            }

            _implementations = await Helper.GetTypesWithAttribute<JsonConverterImplementationAttribute>();

            return _implementations;
        }
    }
}
