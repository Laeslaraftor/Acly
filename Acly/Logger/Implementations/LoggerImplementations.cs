using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Acly.Logger
{
    /// <summary>
    /// Класс для поиска реализаций <see cref="ILogger"/>
    /// </summary>
    public static class LoggerImplementations
    {
        /// <summary>
        /// Получить список типов, помеченных как реализация <see cref="ILogger"/>
        /// </summary>
        /// <returns>Список типов, помеченных как реализация <see cref="ILogger"/></returns>
        public static async Task<Type?> GetImplementationType()
        {
            IEnumerable<Type> types = await Helper.GetTypesWithAttribute<LoggerImplementation>();

            foreach (var type in types)
            {
                return type;
            }

            return null;
        }
    }
}
