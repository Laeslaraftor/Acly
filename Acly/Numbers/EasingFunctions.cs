using System;

namespace Acly.Numbers
{
    /// <summary>
    /// Класс с реализациями функций плавности
    /// </summary>
    public static class EasingFunctions
    {
        #region Функции плавности

        /// <summary>
        /// Линейная функция плавности
        /// </summary>
        /// <param name="k">Позиция на отрезке</param>
        /// <returns>Значение на позиции <paramref name="k"/></returns>
        public static float Linear(float k)
        {
            return k;
        }

        /// <summary>
        /// Квадратичная функция плавности
        /// </summary>
        public static class Quadratic
        {
            /// <summary>
            /// Режим входа в функцию плавности на позиции <paramref name="k"/>
            /// </summary>
            /// <param name="k">Позиция на отрезке</param>
            /// <returns>Значение на позиции <paramref name="k"/></returns>
            public static float In(float k)
            {
                return k * k;
            }

            /// <summary>
            /// Режим выхода в функцию плавности на позиции <paramref name="k"/>
            /// </summary>
            /// <param name="k">Позиция на отрезке</param>
            /// <returns>Значение на позиции <paramref name="k"/></returns>
            public static float Out(float k)
            {
                return k * (2f - k);
            }

            /// <summary>
            /// Режим входа и выхода в функцию плавности на позиции <paramref name="k"/>
            /// </summary>
            /// <param name="k">Позиция на отрезке</param>
            /// <returns>Значение на позиции <paramref name="k"/></returns>
            public static float InOut(float k)
            {
                if ((k *= 2f) < 1f) return 0.5f * k * k;
                return -0.5f * ((k -= 1f) * (k - 2f) - 1f);
            }
        };
        /// <summary>
        /// Кубическая функция плавности
        /// </summary>
        public static class Cubic
        {
            /// <summary>
            /// Режим входа в функцию плавности на позиции <paramref name="k"/>
            /// </summary>
            /// <param name="k">Позиция на отрезке</param>
            /// <returns>Значение на позиции <paramref name="k"/></returns>
            public static float In(float k)
            {
                return k * k * k;
            }

            /// <summary>
            /// Режим выхода в функцию плавности на позиции <paramref name="k"/>
            /// </summary>
            /// <param name="k">Позиция на отрезке</param>
            /// <returns>Значение на позиции <paramref name="k"/></returns>
            public static float Out(float k)
            {
                return 1f + ((k -= 1f) * k * k);
            }

            /// <summary>
            /// Режим входа и выхода в функцию плавности на позиции <paramref name="k"/>
            /// </summary>
            /// <param name="k">Позиция на отрезке</param>
            /// <returns>Значение на позиции <paramref name="k"/></returns>
            public static float InOut(float k)
            {
                if ((k *= 2f) < 1f) return 0.5f * k * k * k;
                return 0.5f * ((k -= 2f) * k * k + 2f);
            }
        };
        /// <summary>
        /// Квартичная функция плавности
        /// </summary>
        public static class Quartic
        {
            /// <summary>
            /// Режим входа в функцию плавности на позиции <paramref name="k"/>
            /// </summary>
            /// <param name="k">Позиция на отрезке</param>
            /// <returns>Значение на позиции <paramref name="k"/></returns>
            public static float In(float k)
            {
                return k * k * k * k;
            }

            /// <summary>
            /// Режим выхода в функцию плавности на позиции <paramref name="k"/>
            /// </summary>
            /// <param name="k">Позиция на отрезке</param>
            /// <returns>Значение на позиции <paramref name="k"/></returns>
            public static float Out(float k)
            {
                return 1f - ((k -= 1f) * k * k * k);
            }

            /// <summary>
            /// Режим входа и выхода в функцию плавности на позиции <paramref name="k"/>
            /// </summary>
            /// <param name="k">Позиция на отрезке</param>
            /// <returns>Значение на позиции <paramref name="k"/></returns>
            public static float InOut(float k)
            {
                if ((k *= 2f) < 1f) return 0.5f * k * k * k * k;
                return -0.5f * ((k -= 2f) * k * k * k - 2f);
            }
        };
        /// <summary>
        /// Квинтическая функция плавности
        /// </summary>
        public static class Quintic
        {
            /// <summary>
            /// Режим входа в функцию плавности на позиции <paramref name="k"/>
            /// </summary>
            /// <param name="k">Позиция на отрезке</param>
            /// <returns>Значение на позиции <paramref name="k"/></returns>
            public static float In(float k)
            {
                return k * k * k * k * k;
            }

            /// <summary>
            /// Режим выхода в функцию плавности на позиции <paramref name="k"/>
            /// </summary>
            /// <param name="k">Позиция на отрезке</param>
            /// <returns>Значение на позиции <paramref name="k"/></returns>
            public static float Out(float k)
            {
                return 1f + ((k -= 1f) * k * k * k * k);
            }

            /// <summary>
            /// Режим входа и выхода в функцию плавности на позиции <paramref name="k"/>
            /// </summary>
            /// <param name="k">Позиция на отрезке</param>
            /// <returns>Значение на позиции <paramref name="k"/></returns>
            public static float InOut(float k)
            {
                if ((k *= 2f) < 1f) return 0.5f * k * k * k * k * k;
                return 0.5f * ((k -= 2f) * k * k * k * k + 2f);
            }
        };
        /// <summary>
        /// Синусоидальная функция плавности
        /// </summary>
        public static class Sinusoidal
        {
            /// <summary>
            /// Режим входа в функцию плавности на позиции <paramref name="k"/>
            /// </summary>
            /// <param name="k">Позиция на отрезке</param>
            /// <returns>Значение на позиции <paramref name="k"/></returns>
            public static float In(float k)
            {
                return 1f - MathF.Cos(k * MathF.PI / 2f);
            }

            /// <summary>
            /// Режим выхода в функцию плавности на позиции <paramref name="k"/>
            /// </summary>
            /// <param name="k">Позиция на отрезке</param>
            /// <returns>Значение на позиции <paramref name="k"/></returns>
            public static float Out(float k)
            {
                return MathF.Sin(k * MathF.PI / 2f);
            }

            /// <summary>
            /// Режим входа и выхода в функцию плавности на позиции <paramref name="k"/>
            /// </summary>
            /// <param name="k">Позиция на отрезке</param>
            /// <returns>Значение на позиции <paramref name="k"/></returns>
            public static float InOut(float k)
            {
                return 0.5f * (1f - MathF.Cos(MathF.PI * k));
            }
        };
        /// <summary>
        /// Экспонентная функция плавности
        /// </summary>
        public static class Exponential
        {
            /// <summary>
            /// Режим входа в функцию плавности на позиции <paramref name="k"/>
            /// </summary>
            /// <param name="k">Позиция на отрезке</param>
            /// <returns>Значение на позиции <paramref name="k"/></returns>
            public static float In(float k)
            {
                return k == 0f ? 0f : MathF.Pow(1024f, k - 1f);
            }

            /// <summary>
            /// Режим выхода в функцию плавности на позиции <paramref name="k"/>
            /// </summary>
            /// <param name="k">Позиция на отрезке</param>
            /// <returns>Значение на позиции <paramref name="k"/></returns>
            public static float Out(float k)
            {
                return k == 1f ? 1f : 1f - MathF.Pow(2f, -10f * k);
            }

            /// <summary>
            /// Режим входа и выхода в функцию плавности на позиции <paramref name="k"/>
            /// </summary>
            /// <param name="k">Позиция на отрезке</param>
            /// <returns>Значение на позиции <paramref name="k"/></returns>
            public static float InOut(float k)
            {
                if (k == 0f) return 0f;
                if (k == 1f) return 1f;
                if ((k *= 2f) < 1f) return 0.5f * MathF.Pow(1024f, k - 1f);
                return 0.5f * (-MathF.Pow(2f, -10f * (k - 1f)) + 2f);
            }
        };
        /// <summary>
        /// Круговая функция плавности
        /// </summary>
        public static class Circular
        {
            /// <summary>
            /// Режим входа в функцию плавности на позиции <paramref name="k"/>
            /// </summary>
            /// <param name="k">Позиция на отрезке</param>
            /// <returns>Значение на позиции <paramref name="k"/></returns>
            public static float In(float k)
            {
                return 1f - MathF.Sqrt(1f - k * k);
            }

            /// <summary>
            /// Режим выхода в функцию плавности на позиции <paramref name="k"/>
            /// </summary>
            /// <param name="k">Позиция на отрезке</param>
            /// <returns>Значение на позиции <paramref name="k"/></returns>
            public static float Out(float k)
            {
                return MathF.Sqrt(1f - ((k -= 1f) * k));
            }

            /// <summary>
            /// Режим входа и выхода в функцию плавности на позиции <paramref name="k"/>
            /// </summary>
            /// <param name="k">Позиция на отрезке</param>
            /// <returns>Значение на позиции <paramref name="k"/></returns>
            public static float InOut(float k)
            {
                if ((k *= 2f) < 1f) return -0.5f * (MathF.Sqrt(1f - k * k) - 1);
                return 0.5f * (MathF.Sqrt(1f - (k -= 2f) * k) + 1f);
            }
        };
        /// <summary>
        /// Эластичная функция плавности
        /// </summary>
        public static class Elastic
        {
            /// <summary>
            /// Режим входа в функцию плавности на позиции <paramref name="k"/>
            /// </summary>
            /// <param name="k">Позиция на отрезке</param>
            /// <returns>Значение на позиции <paramref name="k"/></returns>
            public static float In(float k)
            {
                if (k == 0) return 0;
                if (k == 1) return 1;
                return -MathF.Pow(2f, 10f * (k -= 1f)) * MathF.Sin((k - 0.1f) * (2f * MathF.PI) / 0.4f);
            }

            /// <summary>
            /// Режим выхода в функцию плавности на позиции <paramref name="k"/>
            /// </summary>
            /// <param name="k">Позиция на отрезке</param>
            /// <returns>Значение на позиции <paramref name="k"/></returns>
            public static float Out(float k)
            {
                if (k == 0) return 0;
                if (k == 1) return 1;
                return MathF.Pow(2f, -10f * k) * MathF.Sin((k - 0.1f) * (2f * MathF.PI) / 0.4f) + 1f;
            }

            /// <summary>
            /// Режим входа и выхода в функцию плавности на позиции <paramref name="k"/>
            /// </summary>
            /// <param name="k">Позиция на отрезке</param>
            /// <returns>Значение на позиции <paramref name="k"/></returns>
            public static float InOut(float k)
            {
                if ((k *= 2f) < 1f) return -0.5f * MathF.Pow(2f, 10f * (k -= 1f)) * MathF.Sin((k - 0.1f) * (2f * MathF.PI) / 0.4f);
                return MathF.Pow(2f, -10f * (k -= 1f)) * MathF.Sin((k - 0.1f) * (2f * MathF.PI) / 0.4f) * 0.5f + 1f;
            }
        };
        /// <summary>
        /// Возвратная функция плавности
        /// </summary>
        public static class Back
        {
            private const float s = 1.70158f;
            private const float s2 = 2.5949095f;

            /// <summary>
            /// Режим входа в функцию плавности на позиции <paramref name="k"/>
            /// </summary>
            /// <param name="k">Позиция на отрезке</param>
            /// <returns>Значение на позиции <paramref name="k"/></returns>
            public static float In(float k)
            {
                return k * k * ((s + 1f) * k - s);
            }

            /// <summary>
            /// Режим выхода в функцию плавности на позиции <paramref name="k"/>
            /// </summary>
            /// <param name="k">Позиция на отрезке</param>
            /// <returns>Значение на позиции <paramref name="k"/></returns>
            public static float Out(float k)
            {
                return (k -= 1f) * k * ((s + 1f) * k + s) + 1f;
            }

            /// <summary>
            /// Режим входа и выхода в функцию плавности на позиции <paramref name="k"/>
            /// </summary>
            /// <param name="k">Позиция на отрезке</param>
            /// <returns>Значение на позиции <paramref name="k"/></returns>
            public static float InOut(float k)
            {
                if ((k *= 2f) < 1f) return 0.5f * (k * k * ((s2 + 1f) * k - s2));
                return 0.5f * ((k -= 2f) * k * ((s2 + 1f) * k + s2) + 2f);
            }
        };
        /// <summary>
        /// Подпрыгивающая функция плавности
        /// </summary>
        public static class Bounce
        {
            /// <summary>
            /// Режим входа в функцию плавности на позиции <paramref name="k"/>
            /// </summary>
            /// <param name="k">Позиция на отрезке</param>
            /// <returns>Значение на позиции <paramref name="k"/></returns>
            public static float In(float k)
            {
                return 1f - Out(1f - k);
            }

            /// <summary>
            /// Режим выхода в функцию плавности на позиции <paramref name="k"/>
            /// </summary>
            /// <param name="k">Позиция на отрезке</param>
            /// <returns>Значение на позиции <paramref name="k"/></returns>
            public static float Out(float k)
            {
                if (k < (1f / 2.75f))
                {
                    return 7.5625f * k * k;
                }
                else if (k < (2f / 2.75f))
                {
                    return 7.5625f * (k -= (1.5f / 2.75f)) * k + 0.75f;
                }
                else if (k < (2.5f / 2.75f))
                {
                    return 7.5625f * (k -= (2.25f / 2.75f)) * k + 0.9375f;
                }
                else
                {
                    return 7.5625f * (k -= (2.625f / 2.75f)) * k + 0.984375f;
                }
            }

            /// <summary>
            /// Режим входа и выхода в функцию плавности на позиции <paramref name="k"/>
            /// </summary>
            /// <param name="k">Позиция на отрезке</param>
            /// <returns>Значение на позиции <paramref name="k"/></returns>
            public static float InOut(float k)
            {
                if (k < 0.5f) return In(k * 2f) * 0.5f;
                return Out(k * 2f - 1f) * 0.5f + 0.5f;
            }
        };

        #endregion

        #region Работа с функциями плавности

        /// <summary>
        /// Получить функцию плавность указанного типа
        /// </summary>
        /// <param name="easingFunction">Тип функции плавности</param>
        /// <returns>Функция плавности</returns>
        public static Func<float, float> GetEasingFunction(Easing easingFunction)
        {
            // Quad
            if (easingFunction == Easing.EaseInQuad)
            {
                return Quadratic.In;
            }

            if (easingFunction == Easing.EaseOutQuad)
            {
                return Quadratic.Out;
            }

            if (easingFunction == Easing.EaseInOutQuad)
            {
                return Quadratic.InOut;
            }

            // Cubic
            if (easingFunction == Easing.EaseInCubic)
            {
                return Cubic.In;
            }

            if (easingFunction == Easing.EaseOutCubic)
            {
                return Cubic.Out;
            }

            if (easingFunction == Easing.EaseInOutCubic)
            {
                return Cubic.InOut;
            }

            // Quart
            if (easingFunction == Easing.EaseInQuart)
            {
                return Quartic.In;
            }

            if (easingFunction == Easing.EaseOutQuart)
            {
                return Quartic.Out; ;
            }

            if (easingFunction == Easing.EaseInOutQuart)
            {
                return Quartic.InOut; ;
            }

            // Quint
            if (easingFunction == Easing.EaseInQuint)
            {
                return Quintic.In;
            }

            if (easingFunction == Easing.EaseOutQuint)
            {
                return Quintic.Out;
            }

            if (easingFunction == Easing.EaseInOutQuint)
            {
                return Quintic.InOut;
            }

            // Sine
            if (easingFunction == Easing.EaseInSine)
            {
                return Sinusoidal.In;
            }

            if (easingFunction == Easing.EaseOutSine)
            {
                return Sinusoidal.Out;
            }

            if (easingFunction == Easing.EaseInOutSine)
            {
                return Sinusoidal.InOut;
            }

            // Expo
            if (easingFunction == Easing.EaseInExpo)
            {
                return Exponential.In;
            }

            if (easingFunction == Easing.EaseOutExpo)
            {
                return Exponential.Out;
            }

            if (easingFunction == Easing.EaseInOutExpo)
            {
                return Exponential.InOut;
            }

            // CirC
            if (easingFunction == Easing.EaseInCirc)
            {
                return Circular.In;
            }

            if (easingFunction == Easing.EaseOutCirc)
            {
                return Circular.Out;
            }

            if (easingFunction == Easing.EaseInOutCirc)
            {
                return Circular.InOut;
            }

            // Linear
            if (easingFunction == Easing.Linear)
            {
                return Linear;
            }

            //  Bounce
            if (easingFunction == Easing.EaseInBounce)
            {
                return Bounce.In;
            }

            if (easingFunction == Easing.EaseOutBounce)
            {
                return Bounce.Out;
            }

            if (easingFunction == Easing.EaseInOutBounce)
            {
                return Bounce.InOut;
            }

            // Back
            if (easingFunction == Easing.EaseInBack)
            {
                return Back.In;
            }

            if (easingFunction == Easing.EaseOutBack)
            {
                return Back.Out;
            }

            if (easingFunction == Easing.EaseInOutBack)
            {
                return Back.InOut;
            }

            // Elastic
            if (easingFunction == Easing.EaseInElastic)
            {
                return Elastic.In;
            }

            if (easingFunction == Easing.EaseOutElastic)
            {
                return Elastic.Out;
            }

            if (easingFunction == Easing.EaseInOutElastic)
            {
                return Elastic.InOut;
            }

            return Linear;
        }
        /// <summary>
        /// Получить значение от 0 до 1 указанной функции плавности на позиции t
        /// </summary>
        /// <param name="easingFunction">Тип функции плавности</param>
        /// <param name="t">Позиция</param>
        /// <returns>Значение от 0 до 1</returns>
        public static float GetEasingFunction(Easing easingFunction, float t)
        {
            return GetEasingFunction(easingFunction)(t);
        }

        /// <summary>
        /// Получить функцию плавности
        /// </summary>
        /// <param name="easing">Тип функции плавности</param>
        /// <returns>Функция плавности</returns>
        public static Func<float, float> ToFunction(this Easing easing)
        {
            return GetEasingFunction(easing);
        }
        /// <summary>
        /// Получить значение от 0 до 1 на позиции t
        /// </summary>
        /// <param name="easing">Тип функции плавности</param>
        /// <param name="t">Позиция</param>
        /// <returns>Значение от 0 до 1</returns>
        public static float ToFunction(this Easing easing, float t)
        {
            return GetEasingFunction(easing)(t);
        }

        #endregion
    }
}