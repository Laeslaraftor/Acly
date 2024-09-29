using System;
using System.Net.Mail;

namespace Acly
{
	/// <summary>
	/// Класс со вспомогательными методами и методами расширения
	/// </summary>
	public static partial class Helper
	{
		/// <summary>
		/// Экземпляр <see cref="System.Random"/>
		/// </summary>
		public readonly static Random Random = new();

		private const string _Symbols = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890";

		/// <summary>
		/// Случайная строка длиной в 6 символов
		/// </summary>
		/// <returns>Случайная строка</returns>
		public static string RandomString()
		{
			return RandomString(6);
		}
		/// <summary>
		/// Случайная строка указанной длины
		/// </summary>
		/// <param name="Length">Длина случайной строки</param>
		/// <returns>Случайная строка</returns>
		public static string RandomString(int Length)
		{
			return RandomString(Length, _Symbols);
		}
		/// <summary>
		/// Случайная строка длиной в 6 символов, составленная из указанных символов
		/// </summary>
		/// <param name="Symbols">Символы для составления строки</param>
		/// <returns>Случайная строка</returns>
		public static string RandomString(string Symbols)
		{
			return RandomString(6, Symbols);
		}
		/// <summary>
		/// Случайная строка указанной длины из указанных символов
		/// </summary>
		/// <param name="Length">Длина строки</param>
		/// <param name="Symbols">Символы для составления строки</param>
		/// <returns>Случайная строка</returns>
		/// <exception cref="ArgumentNullException">Символы для составления строки не указаны</exception>
		/// <exception cref="ArgumentException">Длина строки не может быть отрицательной</exception>
		public static string RandomString(int Length, string Symbols)
		{
			if (Symbols == null)
			{
				throw new ArgumentNullException(nameof(Symbols), "Символы для составления строки не указаны");
			}
			if (Symbols.Length == 0)
			{
				throw new ArgumentException("Количество символов для составления случайной строки должно быть больше нуля", nameof(Symbols));
			}
			if (Length < 0)
			{
				throw new ArgumentException("Длина строки не может быть отрицательной!", nameof(Length));
			}

			string Result = "";

			for (int i = 0; i < Length; i++)
			{
				int Index = Random.Next(0, Symbols.Length - 1);
				Result += Symbols[Index];
			}

			return Result;
		}
		/// <summary>
		/// Случайная строка длиной в 6 символов, составленная из указанных символов
		/// </summary>
		/// <param name="Symbols">Символы для составления строки</param>
		/// <returns>Случайная строка</returns>
		public static string RandomString(char[] Symbols)
		{
			return RandomString(6, Symbols);
		}
		/// <summary>
		/// Случайная строка указанной длины из указанных символов
		/// </summary>
		/// <param name="Length">Длина строки</param>
		/// <param name="Symbols">Символы для составления строки</param>
		/// <returns>Случайная строка</returns>
		public static string RandomString(int Length, char[] Symbols)
		{
			return RandomString(Length, Symbols.ToString(true));
		}

		/// <summary>
		/// Проверить является ли введённая почта верной
		/// </summary>
		/// <param name="Email">Почта</param>
		/// <returns>Верна ли почта</returns>
		public static bool IsValidEmail(string Email)
		{
			if (Email == null)
			{
				throw new ArgumentNullException(nameof(Email), "Почта не указана");
			}

			Email = Email.Trim();

			if (Email.EndsWith("."))
			{
				return false;
			}
			try
			{
				MailAddress Address = new MailAddress(Email);
				return Address.Address == Email;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// a + (b - a) * t;
		/// </summary>
		public static float Lerp(float A, float B, float T)
		{
			T = Math.Clamp(T, 0, 1);
			return A + (B - A) * T;
		}

		/// <summary>
		/// Получить значение к рамках от 0 до 1
		/// </summary>
		/// <param name="Value">Значение</param>
		/// <returns>Значение к рамках от 0 до 1</returns>
		public static float Clamp01(float Value)
		{
			return Clamp(Value, 0, 1);
		}
		/// <summary>
		/// Получить значение к рамках от <paramref name="Min"/> до <paramref name="Max"/>
		/// </summary>
		/// <param name="Value">Значение</param>
		/// <param name="Min">Минимальное значение</param>
		/// <param name="Max">Максимальное значение</param>
		/// <returns>Значение к рамках от <paramref name="Min"/> до <paramref name="Max"/></returns>
		public static float Clamp(float Value, float Min, float Max)
		{
			return MathF.Min(MathF.Max(Value, Min), Max);
		}
		/// <summary>
		/// Получить значение к рамках от 0 до 1
		/// </summary>
		/// <param name="Value">Значение</param>
		/// <returns>Значение к рамках от 0 до 1</returns>
		public static double Clamp01(double Value)
		{
			return Clamp(Value, 0, 1);
		}
		/// <summary>
		/// Получить значение к рамках от <paramref name="Min"/> до <paramref name="Max"/>
		/// </summary>
		/// <param name="Value">Значение</param>
		/// <param name="Min">Минимальное значение</param>
		/// <param name="Max">Максимальное значение</param>
		/// <returns>Значение к рамках от <paramref name="Min"/> до <paramref name="Max"/></returns>
		public static double Clamp(double Value, double Min, double Max)
		{
			return Math.Min(Math.Max(Value, Min), Max);
		}
		/// <summary>
		/// Получить значение к рамках от 0 до 1
		/// </summary>
		/// <param name="Value">Значение</param>
		/// <returns>Значение к рамках от 0 до 1</returns>
		public static int Clamp01(int Value)
		{
			return Clamp(Value, 0, 1);
		}
		/// <summary>
		/// Получить значение к рамках от <paramref name="Min"/> до <paramref name="Max"/>
		/// </summary>
		/// <param name="Value">Значение</param>
		/// <param name="Min">Минимальное значение</param>
		/// <param name="Max">Максимальное значение</param>
		/// <returns>Значение к рамках от <paramref name="Min"/> до <paramref name="Max"/></returns>
		public static int Clamp(int Value, int Min, int Max)
		{
			return Math.Min(Math.Max(Value, Min), Max);
		}
		/// <summary>
		/// Получить значение к рамках от 0 до 1
		/// </summary>
		/// <param name="Value">Значение</param>
		/// <returns>Значение к рамках от 0 до 1</returns>
		public static long Clamp01(long Value)
		{
			return Clamp(Value, 0, 1);
		}
		/// <summary>
		/// Получить значение к рамках от <paramref name="Min"/> до <paramref name="Max"/>
		/// </summary>
		/// <param name="Value">Значение</param>
		/// <param name="Min">Минимальное значение</param>
		/// <param name="Max">Максимальное значение</param>
		/// <returns>Значение к рамках от <paramref name="Min"/> до <paramref name="Max"/></returns>
		public static long Clamp(long Value, long Min, long Max)
		{
			return Math.Min(Math.Max(Value, Min), Max);
		}
	}
}
