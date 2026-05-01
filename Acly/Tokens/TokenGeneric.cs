using System;

namespace Acly.Tokens
{
    /// <summary>
    /// Токен
    /// </summary>
    [Serializable]
    public struct Token<T> : IEquatable<Token<T>>
    {
        /// <summary>
        /// Создать токен
        /// </summary>
        /// <param name="key">Значение токена</param>
        public Token(T key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key), "Значение токена не задано");
            }

            Key = key;
        }

        /// <summary>
        /// Значение токена
        /// </summary>
        public T Key { get; private set; }

        #region Операторы

        /// <summary>
        /// Проверить равенство токенов
        /// </summary>
        /// <param name="t1">Первый токен</param>
        /// <param name="t2">Второй токен</param>
        /// <returns>Равны ли токены</returns>
        public static bool operator ==(Token<T> t1, Token<T> t2)
        {
            return t1.Key?.Equals(t2) == true;
        }
        /// <summary>
        /// Проверить неравенство токенов
        /// </summary>
        /// <param name="t1">Первый токен</param>
        /// <param name="t2">Второй токен</param>
        /// <returns>Не равны ли токены</returns>
        public static bool operator !=(Token<T> t1, Token<T> t2)
        {
            return !(t1 == t2);
        }

        /// <summary>
        /// Прибавить токен к строке
        /// </summary>
        /// <param name="value">Строка</param>
        /// <param name="t">Токен</param>
        /// <returns>Токен, прибавленный к строке</returns>
        public static string operator +(string value, Token<T> t)
        {
            if (t.Key == null)
            {
                return value;
            }

            return value + t.Key.ToString();
        }
        /// <summary>
        /// Прибавить токен к строке
        /// </summary>
        /// <param name="value">Строка</param>
        /// <param name="t">Токен</param>
        /// <returns>Токен, прибавленный к строке</returns>
        public static string operator +(Token<T> t, string value)
        {
            if (t.Key == null)
            {
                return value;
            }

            return t.Key.ToString() + value;
        }

        #endregion

        #region Другое

        /// <summary>
        /// Проверить идентичен ли токен указанному объекту
        /// </summary>
        /// <param name="obj">Объект для сравнения</param>
        /// <returns>Идентичен ли объект</returns>
        public readonly override bool Equals(object? obj)
        {
            return obj is Token<T> token &&
                   Key?.Equals(token.Key) == true;
        }
        /// <summary>
        /// Проверить идентичен ли токен
        /// </summary>
        /// <param name="other">Токен для сравнения</param>
        /// <returns>Идентичен ли токен</returns>
        public readonly bool Equals(Token<T> other)
        {
            return other == this;
        }

        /// <summary>
        /// Получить хэш-код
        /// </summary>
        /// <returns>Хэш-код</returns>
        public readonly override int GetHashCode()
        {
            return HashCode.Combine(Key);
        }
        /// <summary>
        /// Преобразовать токен в строку
        /// </summary>
        /// <returns>Токен как строка</returns>
        public readonly override string ToString()
        {
            if (Key == null)
            {
                return string.Empty;
            }

            return Key.ToString();
        }

        #endregion
    }
}
