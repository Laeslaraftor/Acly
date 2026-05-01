using System;

namespace Acly.Tokens
{
    /// <summary>
    /// Токен
    /// </summary>
    [Serializable]
    public struct Token : IEquatable<Token>
    {
        /// <summary>
        /// Создать токен
        /// </summary>
        public Token()
        {
            Key = Helper.RandomString(12);
        }
        /// <summary>
        /// Создать токен
        /// </summary>
        /// <param name="key">Значение токена</param>
        public Token(string key)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key), "Значение токена не задано");
        }

        /// <summary>
        /// Значение токена
        /// </summary>
        public string Key { get; private set; }

        #region Операторы

        /// <summary>
        /// Проверить равенство токенов
        /// </summary>
        /// <param name="t1">Первый токен</param>
        /// <param name="t2">Второй токен</param>
        /// <returns>Равны ли токены</returns>
        public static bool operator ==(Token t1, Token t2)
        {
            return t1.Key == t2.Key;
        }
        /// <summary>
        /// Проверить неравенство токенов
        /// </summary>
        /// <param name="t1">Первый токен</param>
        /// <param name="t2">Второй токен</param>
        /// <returns>Не равны ли токены</returns>
        public static bool operator !=(Token t1, Token t2)
        {
            return t1.Key != t2.Key;
        }

        /// <summary>
        /// Прибавить токен к строке
        /// </summary>
        /// <param name="value">Строка</param>
        /// <param name="t">Токен</param>
        /// <returns>Токен, прибавленный к строке</returns>
        public static string operator +(string value, Token t)
        {
            return value + t.Key;
        }
        /// <summary>
        /// Прибавить токен к строке
        /// </summary>
        /// <param name="value">Строка</param>
        /// <param name="t">Токен</param>
        /// <returns>Токен, прибавленный к строке</returns>
        public static string operator +(Token t, string value)
        {
            return t.Key + value;
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
            return obj is Token token &&
                   Key == token.Key;
        }
        /// <summary>
        /// Проверить идентичен ли токен
        /// </summary>
        /// <param name="other">Токен для сравнения</param>
        /// <returns>Идентичен ли токен</returns>
        public readonly bool Equals(Token other)
        {
            return other.Key == Key;
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
            return Key;
        }

        #endregion
    }
}
