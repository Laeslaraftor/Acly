using Acly.Numbers;
using Acly.Platforms;
using Acly.Player.Implementations;
using Acly.Tokens;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Acly.Player
{
	/// <summary>
	/// Класс для управления SimplePlayer
	/// </summary>
	public static class SimplePlayer
	{
		/// <summary>
		/// Вызывается при изменении состояния плеера
		/// </summary>
		public static event SimplePlayerStateEvent? StateChanged;
		/// <summary>
		/// Вызывается при смене источника
		/// </summary>
		public static event SimplePlayerEvent? SourceChanged;
		/// <summary>
		/// Вызывается при окончании источника
		/// </summary>
		public static event SimplePlayerEvent? SourceEnded;

		/// <summary>
		/// Текущая позиция плеера
		/// </summary>
		public static TimeSpan Position
		{
			get
			{
				if (Player == null)
				{
					return TimeSpan.Zero;
				}

				return Player.Position;
			}
			set
			{
				if (Player != null)
				{
					Player.Position = value;
				}
			}
		}
		/// <summary>
		/// Продолжительность источника
		/// </summary>
		public static TimeSpan Duration
		{
			get
			{
				if (Player == null)
				{
					return TimeSpan.Zero;
				}

				return Player.Duration;
			}
		}
		/// <summary>
		/// Скорость проигрывания
		/// </summary>
		public static float Speed
		{
			get
			{
				if (Player == null)
				{
					return 1;
				}

				return Player.Speed;
			}
			set
			{
				if (Player != null)
				{
					Player.Speed = value;
				}
			}
		}
		/// <summary>
		/// Громкость плеера
		/// </summary>
		public static float Volume
		{
			get
			{
				if (Player == null)
				{
					return 1;
				}

				return Player.Volume;
			}
			set
			{
				if (Player != null)
				{
					Player.Volume = value;
				}
			}
		}
		/// <summary>
		/// Начать ли заново по окончании источника
		/// </summary>
		public static bool Loop
		{
			get
			{
				if (Player == null)
				{
					return false;
				}

				return Player.Loop;
			}
			set
			{
				if (Player != null)
				{
					Player.Loop = value;
				}
			}
		}
		/// <summary>
		/// Проигрывается ли сейчас источник
		/// </summary>
		public static bool IsPlaying
		{
			get
			{
				if (Player == null)
				{
					return false;
				}

				return Player.IsPlaying;
			}
		}
		/// <summary>
		/// Установлен ли источник
		/// </summary>
		public static bool SourceSetted
		{
			get
			{
				if (Player == null)
				{
					return false;
				}

				return Player.SourceSetted;
			}
		}
		/// <summary>
		/// Начинать ли проигрывание после смены источника
		/// </summary>
		public static bool AutoPlay
		{
			get
			{
				if (Player == null)
				{
					return false;
				}

				return Player.AutoPlay;
			}
			set
			{
				if (Player != null)
				{
					Player.AutoPlay = value;
				}
			}
		}

		/// <summary>
		/// Текущее состояние плеера
		/// </summary>
		public static SimplePlayerState State
		{
			get
			{
				if (Player == null)
				{
					return SimplePlayerState.Stopped;
				}

				return Player.State;
			}
		}
		/// <summary>
		/// Папка для временных файлов плеера
		/// </summary>
		public static string TempFolder
		{
			get
			{
				if (_TempFolder == null)
				{
					return DefaultTempFolder;
				}

				return _TempFolder;
			}
			set => _TempFolder = value;
		}
		/// <summary>
		/// Реализация <see cref="ISimplePlayer"/>, полученная после инициализации <see cref="SimplePlayer"/>
		/// </summary>
		public static ISimplePlayer? Implementation => Player;

		private static ISimplePlayer? Player
		{
			get
			{
				if (!_IsInitialized)
				{
					InitializeAsync();
					throw new InvalidOperationException(nameof(SimplePlayer) + " не инициализирован");
				}
				if (_Player == null)
				{
					InitializeAsync();
					return null;
				}
				
				return _Player;
			}
		}
		private static string DefaultTempFolder
		{
			get
			{
				string Folder = Path.Combine(Environment.CurrentDirectory, "Temp");

				if (!Directory.Exists(Folder))
				{
					Directory.CreateDirectory(Folder);
				}

				return Folder;
			}
		}
		private static ISimplePlayer? _Player;
		private static string? _TempFolder;

		private static bool _IsInitialized;
		private static bool _IsInitializing;

		private static ValueAnimation? _PauseAnimation;
		private static Token? _DemoToken;

		private static readonly TimeSpan _DefaultFadeDuration = TimeSpan.FromSeconds(0.5);

		#region Получение плеера текущей платформы

		/// <summary>
		/// Получить экземпляр плеера для текущей платформы
		/// </summary>
		/// <returns>Экземпляр плеера</returns>
		public static async Task<ISimplePlayer?> GetCurrentPlatformImplementation()
		{
			return await GetPlatformImplementation(Platform.Current);
		}
		/// <summary>
		/// Получить экземпляр плеера для указанной платформы
		/// </summary>
		/// <param name="Platform">Платформа</param>
		/// <returns>Экземпляр плеера</returns>
		/// <exception cref="SimplePlayerNoImplementation">Для указанной платформы нет реализаций</exception>
		/// <exception cref="SimplePlayerIncorrectImplementationCreateMethod">Неправильный метод, указанный для получения экземпляра</exception>
		public static async Task<ISimplePlayer?> GetPlatformImplementation(RuntimePlatform Platform)
		{
			Type? Implementation = await SimplePlayerImplementations.GetPlatformImplementation(Platform);

			if (Implementation == null)
			{
				throw new SimplePlayerNoImplementation(Platform);
			}

			SimplePlayerImplementationAttribute Attribute = Implementation.GetCustomAttribute<SimplePlayerImplementationAttribute>();

			if (Attribute.Method == null)
			{
				return (ISimplePlayer)Activator.CreateInstance(Implementation);
			}

			MethodInfo[] Methods = Implementation.GetMethods();

			foreach (var Method in Methods)
			{
				if (Method.Name != Attribute.Method)
				{
					continue;
				}
				if (Method.GetParameters().Length > 0)
				{
					throw new SimplePlayerIncorrectImplementationCreateMethod(Implementation, Method.Name);
				}
				if (Method.ReturnType == typeof(void))
				{
					throw new SimplePlayerIncorrectImplementationCreateMethod(Implementation, Method.Name, nameof(ISimplePlayer));
				}
				if (Method.ReturnType != typeof(ISimplePlayer))
				{
					throw new SimplePlayerIncorrectImplementationCreateMethod(Implementation, Method.Name, Method.ReturnType.Name, nameof(ISimplePlayer));
				}

				return (ISimplePlayer)Method.Invoke(Implementation, null);
			}

			return null;
		}

		#endregion

		#region Установка

		/// <summary>
		/// Установить источник из массива байтов аудиофайла
		/// </summary>
		/// <param name="Data">Массив байтов аудиофайла</param>
		public static async Task SetSource(byte[] Data)
		{
			if (Player == null)
			{
				return;
			}

			await Player.SetSource(Data);
		}
		/// <summary>
		/// Установить источник из <see cref="Stream"/>
		/// </summary>
		/// <param name="SourceStream">Источник аудио</param>
		public static async Task SetSource(Stream SourceStream)
		{
			if (Player == null)
			{
				return;
			}

			await Player.SetSource(SourceStream);
		}
        /// <summary>
        /// Установить источник из URL адреса
        /// </summary>
        /// <param name="SourceUrl">URL адрес аудио</param>
        public static async Task SetSource(string SourceUrl)
        {
            if (Player == null)
            {
                return;
            }

            await Player.SetSource(SourceUrl);
        }
        /// <summary>
        /// Установить источник из объекта
        /// </summary>
        /// <param name="Data">Объект для установки аудиофайла</param>
        public static async Task<bool> TrySetSource<T>(T Data)
        {
            if (Player == null)
            {
                return false;
            }

			try
			{
				ISimplePlayer<T> GenericPlayer = (ISimplePlayer<T>)Player;
                await GenericPlayer.SetSource(Data);

				return true;
            }
			catch (Exception Error)
			{
				Log.Error(Error);
			}

			return false;
        }

        /// <summary>
        /// Выполнить первоначальную установку, чтоб дальнейшем можно было без задержек начать использовать плеер
        /// </summary>
        public static async Task Initialize()
		{
			if (_IsInitializing)
			{
				throw new InvalidOperationException(nameof(SimplePlayer) + " уже инициализируется. Пожалуйста, подождите");
			}

			_IsInitializing = true;

			try
			{
				_Player = await GetCurrentPlatformImplementation();
			}
			catch (Exception Error)
			{
				Log.Error(Error);
			}

			if (_Player != null)
			{
				_Player.StateChanged += OnPlayerStateChanged;
				_Player.SourceChanged += OnPlayerSourceChanged;
				_Player.SourceEnded += OnPlayerSourceEnded;
			}

			_IsInitialized = true;
			_IsInitializing = false;
		}

		private static async void InitializeAsync()
		{
			await Initialize();
		}

		#endregion

		#region Управление

		/// <summary>
		/// Поставить на паузу
		/// </summary>
		public static void Pause()
		{
			Player?.Pause();
		}
		/// <summary>
		/// Остановить плеер
		/// </summary>
		public static void Stop()
		{
			Player?.Stop();
		}
		/// <summary>
		/// Включить / возобновить проигрывание
		/// </summary>
		public static void Play()
		{
			RemoveAnimation();
			_DemoToken = null;
			Volume = 1;

			Player?.Play();
		}
		/// <summary>
		/// Начать проигрывание с указанной позиции
		/// </summary>
		/// <param name="Position">Позиция для проигрывания</param>
		public static void Play(TimeSpan Position)
		{
			SimplePlayer.Position = Position;
			Play();
		}
		/// <summary>
		/// Начать проигрывание с указанной позиции
		/// </summary>
		/// <param name="Seconds">Позиция для проигрывания в секундах</param>
		public static void Play(float Seconds)
		{
			Play(TimeSpan.FromSeconds(Seconds));
		}

		/// <summary>
		/// Получить данные спектра указанного размера с применением указанного FFT окна
		/// </summary>
		/// <param name="Size">Размер требуемых данных спектра</param>
		/// <param name="Window">FFT окно</param>
		/// <returns>Данные спектра</returns>
		public static float[] GetSpectrumData(int Size, SpectrumWindow Window = SpectrumWindow.Rectangular)
		{
			if (Player == null)
			{
				return new float[Size];
			}

			return Player.GetSpectrumData(Size, Window);
		}
		/// <summary>
		/// Получить данные спектра указанного размера со сглаживанием и применением указанного FFT окна
		/// </summary>
		/// <param name="Size">Размер требуемых данных спектра</param>
		/// <param name="SmoothAmount">Степень сглаживания. 
		/// Например, пусть степень сглаживания будет равна 2, то значение arr[i] будет равно среднему арифметическому значений от arr[i - 2] до arr[i + 2] включительно
		/// </param>
		/// <param name="Window">FFT окно</param>
		/// <returns>Данные спектра со сглаживанием</returns>
		public static float[] GetSpectrumData(int Size, int SmoothAmount, SpectrumWindow Window = SpectrumWindow.Rectangular)
		{
			if (Player == null)
			{
				return new float[Size];
			}

			return Player.GetSpectrumData(Size, SmoothAmount, Window);
		}
		/// <summary>
		/// Получить готовые к использованию данные спектра указанного размера со сглаживанием и применением указанного FFT окна
		/// </summary>
		/// <param name="Size">Размер требуемых данных спектра</param>
		/// <param name="Window">FFT окно</param>
		/// <returns>Готовые к использованию данные спектра</returns>
		public static float[] GetFilteredSpectrumData(int Size, SpectrumWindow Window = SpectrumWindow.Rectangular)
		{
			if (Player == null)
			{
				return new float[Size];
			}

			return Player.GetFilteredSpectrumData(Size, Window);
		}
		/// <summary>
		/// Получить готовые к использованию данные спектра со сглаживанием указанного размера со сглаживанием и применением указанного FFT окна
		/// </summary>
		/// <param name="Size">Размер требуемых данных спектра</param>
		/// <param name="SmoothAmount">Степень сглаживания. 
		/// Например, пусть степень сглаживания будет равна 2, то значение arr[i] будет равно среднему арифметическому значений от arr[i - 2] до arr[i + 2] включительно
		/// </param>
		/// <param name="Window">FFT окно</param>
		/// <returns>Готовые к использованию данные спектра</returns>
		public static float[] GetFilteredSpectrumData(int Size, int SmoothAmount, SpectrumWindow Window = SpectrumWindow.Rectangular)
		{
			if (Player == null)
			{
				return new float[Size];
			}

			return Player.GetFilteredSpectrumData(Size, SmoothAmount, Window);
		}
        /// <summary>
        /// Получить сглаженные данные спектра. Таки данные не должны быть резкими
        /// </summary>
        /// <param name="Size">Размер требуемых данных спектра</param>
        /// <param name="DecreaseSize">Размер уменьшения</param>
        /// <param name="DecreaseMultiplier">Множитель уменьшения. Чем дольше, тем быстрее уменьшается</param>
        /// <param name="Window">FFT окно</param>
        /// <returns>Данные спектра</returns>
        public static float[] GetBufferedSpectrumData(int Size, float DecreaseSize = 0.005f, float DecreaseMultiplier = 1.2f, SpectrumWindow Window = SpectrumWindow.Rectangular)
		{
			float[] Data = GetSpectrumData(Size, Window);
			Data = SpectrumBandBuffer.ApplyBuffer(Data, DecreaseSize, DecreaseMultiplier);

			return Data;
		}
        /// <summary>
        /// Получить сглаженные данные спектра. Таки данные не должны быть резкими
        /// </summary>
        /// <param name="Size">Размер требуемых данных спектра</param>
		/// <param name="SmoothAmount">Степень сглаживания. 
		/// Например, пусть степень сглаживания будет равна 2, то значение arr[i] будет равно среднему арифметическому значений от arr[i - 2] до arr[i + 2] включительно
		/// </param>
        /// <param name="DecreaseSize">Размер уменьшения</param>
        /// <param name="DecreaseMultiplier">Множитель уменьшения. Чем дольше, тем быстрее уменьшается</param>
        /// <param name="Window">FFT окно</param>
        /// <returns>Данные спектра</returns>
        public static float[] GetBufferedSpectrumData(int Size, int SmoothAmount, float DecreaseSize = 0.005f, float DecreaseMultiplier = 1.2f, SpectrumWindow Window = SpectrumWindow.Rectangular)
        {
            float[] Data = GetSpectrumData(Size, SmoothAmount, Window);
            Data = SpectrumBandBuffer.ApplyBuffer(Data, DecreaseSize, DecreaseMultiplier);

            return Data;
        }

        #endregion

        #region Дополнительное управление

        /// <summary>
        /// Включить демо-проигрывание. Такое проигрывается даёт прослушать небольшой отрывок источника. Этот отрывок зациклен
        /// </summary>
        public static void PlayDemo()
		{
			TimeSpan StartPosition = TimeSpan.FromMinutes(1);
			TimeSpan Duration = SimplePlayer.Duration - StartPosition - TimeSpan.FromSeconds(30);

			PlayDemo(StartPosition, Duration);
		}
		/// <summary>
		/// Включить демо-проигрывание с указанным временем начала и продолжительностью.
		/// Такое проигрывается даёт прослушать небольшой отрывок источника. Этот отрывок зациклен
		/// </summary>
		/// <param name="StartPosition">Время начала проигрывание</param>
		/// <param name="Duration">Продолжительность отрывка</param>
		public static void PlayDemo(TimeSpan StartPosition, TimeSpan Duration)
		{
			RemoveAnimation();
			Play(StartPosition);

			_DemoToken = new();
			DemoTimerTick(_DemoToken.Value, StartPosition, Duration);
		}

		/// <summary>
		/// Плавное возобновление проигрывания
		/// </summary>
		/// <param name="Duration">Продолжительность возобновления</param>
		public static void FadePlay(TimeSpan Duration)
		{
			RemoveAnimation();

			Volume = 0;

			if (_DemoToken == null)
			{
				Play();
			}
			else
			{
				Player?.Play();
			}

			_PauseAnimation = GetAnimation(0, 1, Duration, Easing.Linear, Val =>
			{
				Volume = Val;
			});
		}
		/// <summary>
		/// Плавное возобновление проигрывания
		/// </summary>
		public static void FadePlay()
		{
			FadePlay(_DefaultFadeDuration);
		}

		/// <summary>
		/// Плавная остановка проигрывания
		/// </summary>
		/// <param name="Duration">Продолжительность затухания</param>
		/// <param name="Paused">Действие после окончания затухания</param>
		public static void FadePause(TimeSpan Duration, Action? Paused = null)
		{
			RemoveAnimation();

			float StartVolume = Volume;

			_PauseAnimation = GetAnimation(Volume, 0, Duration, Easing.Linear, Val =>
			{
				Volume = Val;
			}, () =>
			{
				Pause();
				Volume = StartVolume;
				Paused?.Invoke();
			});
		}
		/// <summary>
		/// Плавная остановка проигрывания
		/// </summary>
		/// <param name="Paused">Действие после окончания затухания</param>
		public static void FadePause(Action? Paused = null)
		{
			FadePause(_DefaultFadeDuration, Paused);
		}

		/// <summary>
		/// Плавная остановка проигрывания через замедление скорости
		/// </summary>
		/// <param name="Duration">Продолжительность замедления</param>
		/// <param name="Paused">Действие после окончания замедления</param>
		public static void PitchPause(TimeSpan Duration, Action? Paused = null)
		{
			RemoveAnimation();

			float StartSpeed = Speed;

			_PauseAnimation = GetAnimation(StartSpeed, 0, Duration, Easing.Linear, Val =>
			{
				Speed = Val;
			}, () =>
			{
				Pause();
				Speed = StartSpeed;
				Paused?.Invoke();
			});
		}
		/// <summary>
		/// Плавная остановка проигрывания через замедление скорости
		/// </summary>
		/// <param name="Paused">Действие после окончания замедления</param>
		public static void PitchPause(Action? Paused = null)
		{
			PitchPause(_DefaultFadeDuration, Paused);
		}

		private static async void DemoTimerTick(Token Token, TimeSpan StartPosition, TimeSpan Duration)
		{
			await Task.Delay(Duration);

			if (Token != _DemoToken)
			{
				return;
			}

			SimplePlayerState StartState = State;

			FadePause(() =>
			{
				Position = StartPosition;

				if (StartState == SimplePlayerState.Playing)
				{
					Player?.Play();
				}
			});

			DemoTimerTick(Token, StartPosition, Duration);
		}

		#endregion

		#region Анимация значения

		private static void RemoveAnimation()
		{
			if (_PauseAnimation == null)
			{
				return;
			}

			_PauseAnimation.Stop();
			_PauseAnimation = null;
		}
		private static ValueAnimation GetAnimation(float From, float To, TimeSpan Duration, Easing Easing, Action<float> Updated, Action? Completed = null)
		{
			ValueAnimation? Anim = _PauseAnimation;
			Anim ??= new();

			Anim.SetFrom(From).SetTo(To).SetDuration(Duration).SetEasing(Easing).SetTickEvent((a, v) =>
			{
				Updated?.Invoke(v);
			});
			Anim.Ended += (a, m) =>
			{
				Completed?.Invoke();
			};

			return Anim.Start();
		}

		#endregion

		#region События

		private static void OnPlayerStateChanged(ISimplePlayer Player, SimplePlayerState State) => StateChanged?.Invoke(Player, State);
		private static void OnPlayerSourceChanged(ISimplePlayer Player) => SourceChanged?.Invoke(Player);
        private static void OnPlayerSourceEnded(ISimplePlayer Player) => SourceEnded?.Invoke(Player);

        #endregion
    }
}
