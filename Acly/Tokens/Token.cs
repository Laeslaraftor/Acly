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
		/// <param name="Key">Значение токена</param>
		public Token(string Key)
		{
			if (Key == null)
			{
				throw new ArgumentNullException(nameof(Key), "Значение токена не задано");
			}

			this.Key = Key;
		}

		/// <summary>
		/// Значение токена
		/// </summary>
		public string Key { get; private set; }

		#region Операторы

		/// <summary>
		/// Проверить равенство токенов
		/// </summary>
		/// <param name="T1">Первый токен</param>
		/// <param name="T2">Второй токен</param>
		/// <returns>Равны ли токены</returns>
		public static bool operator ==(Token T1, Token T2)
		{
			return T1.Key == T2.Key;
		}
		/// <summary>
		/// Проверить неравенство токенов
		/// </summary>
		/// <param name="T1">Первый токен</param>
		/// <param name="T2">Второй токен</param>
		/// <returns>Не равны ли токены</returns>
		public static bool operator !=(Token T1, Token T2)
		{
			return T1.Key != T2.Key;
		}

		/// <summary>
		/// Прибавить токен к строке
		/// </summary>
		/// <param name="Value">Строка</param>
		/// <param name="T">Токен</param>
		/// <returns>Токен, прибавленный к строке</returns>
		public static string operator +(string Value, Token T)
		{
			return Value + T.Key;
		}
		/// <summary>
		/// Прибавить токен к строке
		/// </summary>
		/// <param name="Value">Строка</param>
		/// <param name="T">Токен</param>
		/// <returns>Токен, прибавленный к строке</returns>
		public static string operator +(Token T, string Value)
		{
			return T.Key + Value;
		}

		#endregion

		#region Другое

		/// <summary>
		/// Проверить идентичен ли токен указанному объекту
		/// </summary>
		/// <param name="Obj">Объект для сравнения</param>
		/// <returns>Идентичен ли объект</returns>
		public readonly override bool Equals(object? Obj)
		{
			return Obj is Token token &&
				   Key == token.Key;
		}
		/// <summary>
		/// Проверить идентичен ли токен
		/// </summary>
		/// <param name="Other">Токен для сравнения</param>
		/// <returns>Идентичен ли токен</returns>
		public readonly bool Equals(Token Other)
		{
			return Other.Key == Key;
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
