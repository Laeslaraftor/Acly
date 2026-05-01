using System;
using System.Collections.ObjectModel;

namespace Acly.Commands
{
    /// <summary>
    /// Шаблон команды
    /// </summary>
    [Serializable]
    public class CommandTemplate : IEquatable<Command>
    {
        /// <summary>
        /// Создать шаблон команды без параметров
        /// </summary>
        /// <param name="name">Название команды</param>
        public CommandTemplate(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name), "Название команды не указано");
            Parameters = new(Array.Empty<string>());
        }
        /// <summary>
        /// Создать шаблон команды
        /// </summary>
        /// <param name="name">Название команды</param>
        /// <param name="parameters">Параметры команды</param>
        public CommandTemplate(string name, params string[] parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters), "Параметры команды не указаны");
            }

            Name = name ?? throw new ArgumentNullException(nameof(name), "Название команды не указано");
            Parameters = new(parameters);
        }

        /// <summary>
        /// Название команды
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Параметры команды
        /// </summary>
        public ReadOnlyCollection<string> Parameters { get; private set; }

        #region Управление

        /// <summary>
        /// Получить шаблон команды
        /// </summary>
        /// <returns>Шаблон команды</returns>
        public override string ToString()
        {
            string result = "/" + Name;

            if (Parameters.Count > 0)
            {
                foreach (var parameter in Parameters)
                {
                    result += " [" + parameter + "]";
                }
            }

            return result;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="other"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public bool Equals(Command other)
        {
            if (other == null)
            {
                return false;
            }

            return Name == other.Name;
        }

        #endregion
    }
}
