using Acly.Tokens;
using System;
using System.Threading.Tasks;

namespace Acly.Numbers
{
    /// <summary>
    /// Анимация значения
    /// </summary>
    public class ValueAnimation
    {
        /// <summary>
        /// Создать анимацию значения
        /// </summary>
        public ValueAnimation()
        {
        }
        /// <summary>
        /// Создать анимацию значения
        /// </summary>
        /// <param name="to">От</param>
        public ValueAnimation(float to)
        {
            To = to;
        }
        /// <summary>
        /// Создать анимацию значения
        /// </summary>
        /// <param name="from">От</param>
        /// <param name="to">До</param>
        public ValueAnimation(float from, float to)
        {
            From = from;
            To = to;
        }

        /// <summary>
        /// Выполняется при обновлении кадра анимации
        /// </summary>
        public event ValueAnimationTick? Tick;
        /// <summary>
        /// Выполняется по окончании анимации
        /// </summary>
        public event ValueAnimationCompletion? Ended;

        /// <summary>
        /// Начальное значение анимации
        /// </summary>
        public float From { get; set; }
        /// <summary>
        /// Конечное значение анимации
        /// </summary>
        public float To { get; set; }
        /// <summary>
        /// Продолжительность анимации
        /// </summary>
        public TimeSpan Duration { get; set; }
        /// <summary>
        /// Количество кадров в секунду анимации
        /// </summary>
        public int FPS { get; set; } = 60;
        /// <summary>
        /// Функция плавности анимации
        /// </summary>
        public Easing Easing
        {
            get => field;
            set
            {
                field = value;
                EasingFunction = value.ToFunction();
            }
        } = Easing.Linear;
        /// <summary>
        /// Текущий режим анимации проигрывания анимации
        /// </summary>
        public AnimationMode? CurrentMode { get; protected set; }

        /// <summary>
        /// Установленная функция плавности
        /// </summary>
        protected Func<float, float> EasingFunction { get; private set; } = EasingFunctions.Linear;
        /// <summary>
        /// Токен выполнения анимации. Анимация выполняется пока установленный токен равен текущему токену
        /// </summary>
        protected Token? ProcessingToken { get; set; }

        #region Установка значений

        /// <summary>
        /// Установить начальное значение анимации
        /// </summary>
        public ValueAnimation SetFrom(float value)
        {
            From = value;
            return this;
        }
        /// <summary>
        /// Установить конечное значение анимации
        /// </summary>
        public ValueAnimation SetTo(float value)
        {
            To = value;
            return this;
        }
        /// <summary>
        /// Установить начальное и конечное значение анимации
        /// </summary>
        public ValueAnimation SetFromTo(float from, float to)
        {
            From = from;
            To = to;
            return this;
        }
        /// <summary>
        /// Установить продолжительность анимации
        /// </summary>
        public ValueAnimation SetDuration(TimeSpan time)
        {
            Duration = time;
            return this;
        }
        /// <summary>
        /// Установить событие при обновлении кадра
        /// </summary>
        public ValueAnimation SetTickEvent(ValueAnimationTick @event)
        {
            Tick += @event;
            return this;
        }
        /// <summary>
        /// Установить количество кадров в секунду
        /// </summary>
        public ValueAnimation SetFPS(int value)
        {
            FPS = value;
            return this;
        }
        /// <summary>
        /// Установить функцию плавности анимации
        /// </summary>
        public ValueAnimation SetEasing(Easing easing)
        {
            Easing = easing;
            return this;
        }

        #endregion

        #region Управление

        /// <summary>
        /// Запустить анимацию
        /// </summary>
        /// <param name="mode">Режим проигрывания анимации</param>
        public virtual ValueAnimation Start(AnimationMode mode = AnimationMode.Default)
        {
            CurrentMode = mode;

            if (mode == AnimationMode.Inverted)
            {
                StartAnimation(To, From, mode);
                return this;
            }

            StartAnimation(From, To, mode);
            return this;
        }
        /// <summary>
        /// Остановить проигрывание анимации
        /// </summary>
        /// <returns></returns>
        public virtual ValueAnimation Stop()
        {
            ProcessingToken = null;
            return this;
        }

        #endregion

        #region Анимация

        /// <summary>
        /// Запустить проигрывание анимации
        /// </summary>
        /// <param name="from">Начальное значение анимации</param>
        /// <param name="to">Конечное значение анимации</param>
        /// <param name="mode">Режим проигрывания анимации</param>
        protected virtual async void StartAnimation(float from, float to, AnimationMode mode)
        {
            Token token = new();
            ProcessingToken = token;

            int frames = Convert.ToInt32(Math.Ceiling(Duration.TotalSeconds * FPS));
            TimeSpan frameTime = TimeSpan.FromSeconds((float)1 / FPS);

            await StartAnimationLoop(from, to, frames, frameTime, token);

            InvokeEndedEvent(mode);
            CurrentMode = null;
        }

        /// <summary>
        /// Запуск цикла анимации
        /// </summary>
        /// <param name="from">Начальное значение анимации</param>
        /// <param name="fo">Конечное значение анимации</param>
        /// <param name="frames">Количество кадров анимации</param>
        /// <param name="frameTime">Продолжительность одного кадра</param>
        /// <param name="currentToken">Текущий токен. Анимация должна выполнятся пока <see cref="ProcessingToken"/> равен <paramref name="currentToken"/></param>
        protected virtual async Task StartAnimationLoop(float from, float fo, int frames, TimeSpan frameTime, Token currentToken)
        {
            int framesCompleted = 0;

            while (framesCompleted < frames)
            {
                if (ProcessingToken != currentToken)
                {
                    break;
                }

                framesCompleted++;

                DoAnimationLoopFrame(from, fo, frames, framesCompleted);

                await Task.Delay(frameTime);
            }
        }
        /// <summary>
        /// Выполнить кадр анимации
        /// </summary>
        /// <param name="from">Начальное значение анимации</param>
        /// <param name="to">Конечное значение анимации</param>
        /// <param name="frames">Количество кадров анимации</param>
        /// <param name="framesCompleted">Количество выполненных кадров</param>
        protected virtual void DoAnimationLoopFrame(float from, float to, int frames, int framesCompleted)
        {
            float percent = (float)framesCompleted / frames;
            float easingValue = EasingFunction(percent);
            float value = Helper.Lerp(from, to, easingValue);

            InvokeTickEvent(value);
        }

        #endregion

        #region Вызов событий

        /// <summary>
        /// Вызвать событие окончания анимации. Исключения игнорируются
        /// </summary>
        /// <param name="mode">Текущий режим проигрывания</param>
        protected void InvokeEndedEvent(AnimationMode mode)
        {
            try
            {
                Ended?.Invoke(this, mode);
            }
            catch (Exception error)
            {
                Log.Error(error);
            }
        }
        /// <summary>
        /// Вызвать событие обновления кадра анимации. Исключения игнорируются
        /// </summary>
        /// <param name="value">Значение кадра</param>
        protected void InvokeTickEvent(float value)
        {
            try
            {
                Tick?.Invoke(this, value);
            }
            catch (Exception error)
            {
                Log.Error(error);
            }
        }

        #endregion
    }
}
