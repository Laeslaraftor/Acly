using Acly.Platforms;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Acly.Player.Implementations
{
    /// <summary>
    /// Класс для поиска реализаций <see cref="ISimplePlayer"/>
    /// </summary>
    public static class SimplePlayerImplementations
    {
        private static IEnumerable<Type>? _implementations;

        /// <summary>
        /// Получить реализацию SimplePlayer для указанной платформы
        /// </summary>
        /// <param name="platform">Платформа</param>
        /// <returns>Тип с реализацией SimplePlayer</returns>
        public static async Task<Type?> GetPlatformImplementation(RuntimePlatform platform)
        {
            IEnumerable<Type> implementations = await GetImplementations();
            Type? result = null;

            foreach (var type in implementations)
            {
                foreach (var attribute in type.GetCustomAttributes<SimplePlayerImplementationAttribute>())
                {
                    if (attribute.Platform.HasFlag(platform))
                    {
                        result = type;
                        break;
                    }
                }
            }

            if (result?.IsImplementsInterface<ISimplePlayer>() == false)
            {
                throw new SimplePlayerImplementationException(result);
            }

            return result;
        }
        /// <summary>
        /// Получить реализацию SimplePlayer для текущей платформы
        /// </summary>
        /// <returns>Реализация SimplePlayer для текущей платформы</returns>
        public static async Task<Type?> GetCurrentPlatformImplementation()
        {
            return await GetPlatformImplementation(Platform.Current);
        }

        private static async Task<IEnumerable<Type>> GetImplementations()
        {
            if (_implementations != null)
            {
                return _implementations;
            }

            _implementations = await Helper.GetTypesWithAttribute<SimplePlayerImplementationAttribute>();

            return _implementations;
        }
    }
}
