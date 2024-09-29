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
		/// <param name="Key">Значение токена</param>
		public Token(T Key)
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
		public T Key { get; private set; }

		#region Операторы

		/// <summary>
		/// Проверить равенство токенов
		/// </summary>
		/// <param name="T1">Первый токен</param>
		/// <param name="T2">Второй токен</param>
		/// <returns>Равны ли токены</returns>
		public static bool operator ==(Token<T> T1, Token<T> T2)
		{
			return T1.Key?.Equals(T2) == true;
		}
		/// <summary>
		/// Проверить неравенство токенов
		/// </summary>
		/// <param name="T1">Первый токен</param>
		/// <param name="T2">Второй токен</param>
		/// <returns>Не равны ли токены</returns>
		public static bool operator !=(Token<T> T1, Token<T> T2)
		{
			return !(T1 == T2);
		}

		/// <summary>
		/// Прибавить токен к строке
		/// </summary>
		/// <param name="Value">Строка</param>
		/// <param name="T">Токен</param>
		/// <returns>Токен, прибавленный к строке</returns>
		public static string operator +(string Value, Token<T> T)
		{
			if (T.Key == null)
			{
				return Value;
			}

			return Value + T.Key.ToString();
		}
		/// <summary>
		/// Прибавить токен к строке
		/// </summary>
		/// <param name="Value">Строка</param>
		/// <param name="T">Токен</param>
		/// <returns>Токен, прибавленный к строке</returns>
		public static string operator +(Token<T> T, string Value)
		{
			if (T.Key == null)
			{
				return Value;
			}

			return T.Key.ToString() + Value;
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
			return Obj is Token<T> token &&
				   Key?.Equals(token.Key) == true;
		}
		/// <summary>
		/// Проверить идентичен ли токен
		/// </summary>
		/// <param name="Other">Токен для сравнения</param>
		/// <returns>Идентичен ли токен</returns>
		public readonly bool Equals(Token<T> Other)
		{
			return Other == this;
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
				return "";
			}

			return Key.ToString();
		}

		#endregion
	}
}
