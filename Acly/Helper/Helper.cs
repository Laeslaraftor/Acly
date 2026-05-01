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

        private const string _symbols = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890";

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
        /// <param name="length">Длина случайной строки</param>
        /// <returns>Случайная строка</returns>
        public static string RandomString(int length)
        {
            return RandomString(length, _symbols);
        }
        /// <summary>
        /// Случайная строка длиной в 6 символов, составленная из указанных символов
        /// </summary>
        /// <param name="symbols">Символы для составления строки</param>
        /// <returns>Случайная строка</returns>
        public static string RandomString(string symbols)
        {
            return RandomString(6, symbols);
        }
        /// <summary>
        /// Случайная строка указанной длины из указанных символов
        /// </summary>
        /// <param name="length">Длина строки</param>
        /// <param name="symbols">Символы для составления строки</param>
        /// <returns>Случайная строка</returns>
        /// <exception cref="ArgumentNullException">Символы для составления строки не указаны</exception>
        /// <exception cref="ArgumentException">Длина строки не может быть отрицательной</exception>
        public static string RandomString(int length, string symbols)
        {
            if (symbols == null)
            {
                throw new ArgumentNullException(nameof(symbols), "Символы для составления строки не указаны");
            }
            if (symbols.Length == 0)
            {
                throw new ArgumentException("Количество символов для составления случайной строки должно быть больше нуля", nameof(symbols));
            }
            if (length < 0)
            {
                throw new ArgumentException("Длина строки не может быть отрицательной!", nameof(length));
            }

            string result = string.Empty;

            for (int i = 0; i < length; i++)
            {
                int index = Random.Next(0, symbols.Length - 1);
                result += symbols[index];
            }

            return result;
        }
        /// <summary>
        /// Случайная строка длиной в 6 символов, составленная из указанных символов
        /// </summary>
        /// <param name="symbols">Символы для составления строки</param>
        /// <returns>Случайная строка</returns>
        public static string RandomString(char[] symbols)
        {
            return RandomString(6, symbols);
        }
        /// <summary>
        /// Случайная строка указанной длины из указанных символов
        /// </summary>
        /// <param name="length">Длина строки</param>
        /// <param name="symbols">Символы для составления строки</param>
        /// <returns>Случайная строка</returns>
        public static string RandomString(int length, char[] symbols)
        {
            return RandomString(length, symbols.ToString(true));
        }

        /// <summary>
        /// Проверить является ли введённая почта верной
        /// </summary>
        /// <param name="email">Почта</param>
        /// <returns>Верна ли почта</returns>
        public static bool IsValidEmail(string email)
        {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email), "Почта не указана");
            }

            email = email.Trim();

            if (email.EndsWith('.'))
            {
                return false;
            }
            try
            {
                MailAddress address = new(email);
                return address.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// <inheritdoc cref="LerpUnclamped(float, float, float)"/>
        /// </summary>
        public static float Lerp(float a, float b, float t)
        {
            t = Math.Clamp(t, 0, 1);
            return LerpUnclamped(a, b, t);
        }
        /// <summary>
        /// <inheritdoc cref="LerpUnclamped(float, float, float)"/>
        /// </summary>
        public static double Lerp(double a, double b, float t)
        {
            t = Math.Clamp(t, 0, 1);
            return LerpUnclamped(a, b, t);
        }
        /// <summary>
        /// <inheritdoc cref="LerpUnclamped(float, float, float)"/>
        /// </summary>
        public static double Lerp(float a, float b, double t)
        {
            t = Math.Clamp(t, 0, 1);
            return LerpUnclamped(a, b, t);
        }
        /// <summary>
        /// <inheritdoc cref="LerpUnclamped(float, float, float)"/>
        /// </summary>
        public static double Lerp(double a, double b, double t)
        {
            t = Math.Clamp(t, 0, 1);
            return LerpUnclamped(a, b, t);
        }
        /// <summary>
        /// a + (b - a) * t
        /// </summary>
        public static float LerpUnclamped(float a, float b, float t)
        {
            return a + (b - a) * t;
        }
        /// <summary>
        /// <inheritdoc cref="LerpUnclamped(float, float, float)"/>
        /// </summary>
        public static double LerpUnclamped(double a, double b, float t)
        {
            return a + (b - a) * t;
        }
        /// <summary>
        /// <inheritdoc cref="LerpUnclamped(float, float, float)"/>
        /// </summary>
        public static double LerpUnclamped(double a, double b, double t)
        {
            return a + (b - a) * t;
        }
        /// <summary>
        /// <inheritdoc cref="LerpUnclamped(float, float, float)"/>
        /// </summary>
        public static double LerpUnclamped(float a, float b, double t)
        {
            return a + (b - a) * t;
        }

        /// <summary>
        /// Получить значение к рамках от 0 до 1
        /// </summary>
        /// <param name="value">Значение</param>
        /// <returns>Значение к рамках от 0 до 1</returns>
        public static float Clamp01(float value)
        {
            return Clamp(value, 0, 1);
        }
        /// <summary>
        /// Получить значение к рамках от <paramref name="minimum"/> до <paramref name="maximum"/>
        /// </summary>
        /// <param name="value">Значение</param>
        /// <param name="minimum">Минимальное значение</param>
        /// <param name="maximum">Максимальное значение</param>
        /// <returns>Значение к рамках от <paramref name="minimum"/> до <paramref name="maximum"/></returns>
        public static float Clamp(float value, float minimum, float maximum)
        {
            return MathF.Min(MathF.Max(value, minimum), maximum);
        }
        /// <summary>
        /// Получить значение к рамках от 0 до 1
        /// </summary>
        /// <param name="value">Значение</param>
        /// <returns>Значение к рамках от 0 до 1</returns>
        public static double Clamp01(double value)
        {
            return Clamp(value, 0, 1);
        }
        /// <summary>
        /// Получить значение к рамках от <paramref name="minimum"/> до <paramref name="maximum"/>
        /// </summary>
        /// <param name="value">Значение</param>
        /// <param name="minimum">Минимальное значение</param>
        /// <param name="maximum">Максимальное значение</param>
        /// <returns>Значение к рамках от <paramref name="minimum"/> до <paramref name="maximum"/></returns>
        public static double Clamp(double value, double minimum, double maximum)
        {
            return Math.Min(Math.Max(value, minimum), maximum);
        }
        /// <summary>
        /// Получить значение к рамках от 0 до 1
        /// </summary>
        /// <param name="value">Значение</param>
        /// <returns>Значение к рамках от 0 до 1</returns>
        public static int Clamp01(int value)
        {
            return Clamp(value, 0, 1);
        }
        /// <summary>
        /// Получить значение к рамках от <paramref name="minimum"/> до <paramref name="maximum"/>
        /// </summary>
        /// <param name="value">Значение</param>
        /// <param name="minimum">Минимальное значение</param>
        /// <param name="maximum">Максимальное значение</param>
        /// <returns>Значение к рамках от <paramref name="minimum"/> до <paramref name="maximum"/></returns>
        public static int Clamp(int value, int minimum, int maximum)
        {
            return Math.Min(Math.Max(value, minimum), maximum);
        }
        /// <summary>
        /// Получить значение к рамках от 0 до 1
        /// </summary>
        /// <param name="value">Значение</param>
        /// <returns>Значение к рамках от 0 до 1</returns>
        public static long Clamp01(long value)
        {
            return Clamp(value, 0, 1);
        }
        /// <summary>
        /// Получить значение к рамках от <paramref name="minimum"/> до <paramref name="maximum"/>
        /// </summary>
        /// <param name="value">Значение</param>
        /// <param name="minimum">Минимальное значение</param>
        /// <param name="maximum">Максимальное значение</param>
        /// <returns>Значение к рамках от <paramref name="minimum"/> до <paramref name="maximum"/></returns>
        public static long Clamp(long value, long minimum, long maximum)
        {
            return Math.Min(Math.Max(value, minimum), maximum);
        }
    }
}
