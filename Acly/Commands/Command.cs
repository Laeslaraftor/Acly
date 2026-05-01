using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace Acly.Commands
{
    /// <summary>
    /// Команда
    /// </summary>
    [Serializable]
    public struct Command : IEquatable<Command>
    {
        /// <summary>
        /// Команда
        /// </summary>
        /// <param name="value">Строка-команда</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public Command(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value), "Команда не указана");
            }
            if (value.Length == 0)
            {
                throw new ArgumentException("Команда не указана");
            }
            if (value[0] != '/')
            {
                throw new ArgumentException("Команда должна начинаться с /");
            }

            Name = string.Empty;
            Parameters = new(Array.Empty<string>());
            Parse(value);
        }

        /// <summary>
        /// Название команды
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Параметры команды
        /// </summary>
        public ReadOnlyCollection<string> Parameters { get; private set; }

        #region Установка

        private void Parse(string value)
        {
            List<string> parts = [];
            string currentPart = string.Empty;
            bool nowText = false;
            bool ignoreNext = false;
            bool superText = false;

            value = value[1..];

            foreach (var symbol in value)
            {
                if (symbol != '"' && !nowText && superText)
                {
                    superText = false;
                }
                if (symbol == '\\' && !ignoreNext && !superText)
                {
                    ignoreNext = true;
                    continue;
                }
                else if (symbol == '"' && !ignoreNext)
                {
                    if (nowText)
                    {
                        superText = false;
                    }

                    nowText = !nowText;
                    continue;
                }
                else if (symbol == ' ' && !nowText)
                {
                    parts.Add(currentPart.Trim());
                    currentPart = string.Empty;
                    continue;
                }
                else if (symbol == '@' && !nowText)
                {
                    superText = true;
                    continue;
                }

                currentPart += symbol;
                ignoreNext = false;
            }

            if (currentPart.Length > 0)
            {
                parts.Add(currentPart);
            }
            if (parts.Count == 0)
            {
                return;
            }

            Name = parts[0];
            parts.RemoveAt(0);
            Parameters = new(parts);
        }

        #endregion

        #region Операторы

        /// <summary>
        /// Равенство команд
        /// </summary>
        /// <param name="l">Команда 1</param>
        /// <param name="r">Команда 2</param>
        /// <returns>Равны ли команды</returns>
        public static bool operator ==(Command l, Command r)
        {
            return l.Equals(r);
        }
        /// <summary>
        /// Неравенство команд
        /// </summary>
        /// <param name="l">Команда 1</param>
        /// <param name="r">Команда 2</param>
        /// <returns>Неравны ли команды</returns>
        public static bool operator !=(Command l, Command r)
        {
            return !(l == r);
        }

        #endregion

        #region Управление

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        public readonly override string ToString()
        {
            string result = "Команда: " + Name;

            if (Parameters.Count > 0)
            {
                result += "; Параметры: ";
                string parameters = string.Empty;

                foreach (var parameter in Parameters)
                {
                    if (parameters.Length != 0)
                    {
                        parameters += ", ";
                    }

                    parameters += parameter;
                }

                result += parameters;
            }

            return result;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="obj"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public override readonly bool Equals(object obj)
        {
            return obj is Command other &&
                   Equals(other);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        public override readonly int GetHashCode()
        {
            return HashCode.Combine(Name, Parameters);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="other">Команда</param>
        /// <returns>Равны ли команды</returns>
        public readonly bool Equals(Command other)
        {
            if (Name != other.Name)
            {
                return false;
            }
            if (ReferenceEquals(Parameters, other.Parameters))
            {
                return true;
            }
            else if (Parameters == null || other.Parameters == null)
            {
                return false;
            }

            return Parameters.FullyEquals(other.Parameters);
        }

        #endregion

        #region Статика

        /// <summary>
        /// Попытаться создать команду
        /// </summary>
        /// <param name="value">Строка-команда</param>
        /// <param name="result">Команда (если не удалось создать, то пусто)</param>
        /// <returns>Удалось ли создать команду</returns>
        public static bool TryParse(string value, [NotNullWhen(true)] out Command? result)
        {
            result = null;

            try
            {
                result = new(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
