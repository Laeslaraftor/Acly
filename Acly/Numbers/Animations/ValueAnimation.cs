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
		public ValueAnimation() { }
		/// <summary>
		/// Создать анимацию значения
		/// </summary>
		/// <param name="To">От</param>
		public ValueAnimation(float To)
		{
			this.To = To;
		}
		/// <summary>
		/// Создать анимацию значения
		/// </summary>
		/// <param name="From">От</param>
		/// <param name="To">До</param>
		public ValueAnimation(float From, float To)
		{
			this.From = From;
			this.To = To;
		}

		/// <summary>
		/// Выполняется при обновлении кадра анимации
		/// </summary>
		public event Action<ValueAnimation, float>? Tick;
		/// <summary>
		/// Выполняется по окончании анимации
		/// </summary>
		public event Action<ValueAnimation, AnimationMode>? Ended;

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
			get => _Easing;
			set
			{
				_Easing = value;
				EasingFunction = value.ToFunction();
			}
		}
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

		private Easing _Easing = Easing.Linear;

		#region Установка значений

		/// <summary>
		/// Установить начальное значение анимации
		/// </summary>
		public ValueAnimation SetFrom(float Value)
		{
			From = Value;
			return this;
		}
		/// <summary>
		/// Установить конечное значение анимации
		/// </summary>
		public ValueAnimation SetTo(float Value)
		{
			To = Value;
			return this;
		}
		/// <summary>
		/// Установить начальное и конечное значение анимации
		/// </summary>
		public ValueAnimation SetFromTo(float From, float To)
		{
			this.From = From;
			this.To = To;
			return this;
		}
		/// <summary>
		/// Установить продолжительность анимации
		/// </summary>
		public ValueAnimation SetDuration(TimeSpan Time)
		{
			Duration = Time;
			return this;
		}
		/// <summary>
		/// Установить событие при обновлении кадра
		/// </summary>
		public ValueAnimation SetTickEvent(Action<ValueAnimation, float> Event)
		{
			Tick += Event;
			return this;
		}
		/// <summary>
		/// Установить количество кадров в секунду
		/// </summary>
		public ValueAnimation SetFPS(int Value)
		{
			FPS = Value;
			return this;
		}
		/// <summary>
		/// Установить функцию плавности анимации
		/// </summary>
		public ValueAnimation SetEasing(Easing Easing)
		{
			this.Easing = Easing;
			return this;
		}

		#endregion

		#region Управление

		/// <summary>
		/// Запустить анимацию
		/// </summary>
		/// <param name="Mode">Режим проигрывания анимации</param>
		public virtual ValueAnimation Start(AnimationMode Mode = AnimationMode.Default)
		{
			CurrentMode = Mode;

			if (Mode == AnimationMode.Inverted)
			{
				StartAnimation(To, From, Mode);
				return this;
			}

			StartAnimation(From, To, Mode);
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
		/// <param name="From">Начальное значение анимации</param>
		/// <param name="To">Конечное значение анимации</param>
		/// <param name="Mode">Режим проигрывания анимации</param>
		protected virtual async void StartAnimation(float From, float To, AnimationMode Mode)
		{
			Token Token = new();
			ProcessingToken = Token;

			int Frames = Convert.ToInt32(Math.Ceiling(Duration.TotalSeconds * FPS));
			TimeSpan FrameTime = TimeSpan.FromSeconds((float)1 / FPS);

			await StartAnimationLoop(From, To, Frames, FrameTime, Token);

			InvokeEndedEvent(Mode);
			CurrentMode = null;
		}

		/// <summary>
		/// Запуск цикла анимации
		/// </summary>
		/// <param name="From">Начальное значение анимации</param>
		/// <param name="To">Конечное значение анимации</param>
		/// <param name="Frames">Количество кадров анимации</param>
		/// <param name="FrameTime">Продолжительность одного кадра</param>
		/// <param name="CurrentToken">Текущий токен. Анимация должна выполнятся пока <see cref="ProcessingToken"/> равен <paramref name="CurrentToken"/></param>
		protected virtual async Task StartAnimationLoop(float From, float To, int Frames, TimeSpan FrameTime, Token CurrentToken)
		{
			int FramesCompleted = 0;

			while (FramesCompleted < Frames)
			{
				if (ProcessingToken != CurrentToken)
				{
					break;
				}

				FramesCompleted++;

				DoAnimationLoopFrame(From, To, Frames, FramesCompleted);

				await Task.Delay(FrameTime);
			}
		}
		/// <summary>
		/// Выполнить кадр анимации
		/// </summary>
		/// <param name="From">Начальное значение анимации</param>
		/// <param name="To">Конечное значение анимации</param>
		/// <param name="Frames">Количество кадров анимации</param>
		/// <param name="FramesCompleted">Количество выполненных кадров</param>
		protected virtual void DoAnimationLoopFrame(float From, float To, int Frames, int FramesCompleted)
		{
			float Percent = (float)FramesCompleted / Frames;
			float EasingValue = EasingFunction(Percent);
			float Value = Helper.Lerp(From, To, EasingValue);

			InvokeTickEvent(Value);
		}

		#endregion

		#region Вызов событий

		/// <summary>
		/// Вызвать событие окончания анимации. Исключения игнорируются
		/// </summary>
		/// <param name="Mode">Текущий режим проигрывания</param>
		protected void InvokeEndedEvent(AnimationMode Mode)
		{
			try
			{
				Ended?.Invoke(this, Mode);
			}
			catch (Exception Error)
			{
				Log.Error(Error);
			}
		}
		/// <summary>
		/// Вызвать событие обновления кадра анимации. Исключения игнорируются
		/// </summary>
		/// <param name="Value">Значение кадра</param>
		protected void InvokeTickEvent(float Value)
		{
			try
			{
				Tick?.Invoke(this, Value);
			}
			catch (Exception Error)
			{
				Log.Error(Error);
			}
		}

		#endregion
	}
}
