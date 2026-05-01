using Acly.Numbers;
using Acly.Platforms;
using Acly.Player.Extensions;
using Acly.Player.Implementations;
using Acly.Tokens;
using System;
using System.Collections.Generic;
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
        /// <inheritdoc cref="ISimplePlayer.Source"/>
        /// </summary>
        public static object? Source => Player?.Source;

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
                if (_tempFolder == null)
                {
                    return DefaultTempFolder;
                }

                return _tempFolder;
            }
            set => _tempFolder = value;
        }
        /// <summary>
        /// Реализация <see cref="ISimplePlayer"/>, полученная после инициализации <see cref="SimplePlayer"/>
        /// </summary>
        public static ISimplePlayer? Implementation => Player;

        private static ISimplePlayer? Player
        {
            get
            {
                if (!_isInitialized)
                {
                    InitializeAsync();
                    throw new InvalidOperationException(nameof(SimplePlayer) + " не инициализирован");
                }
                if (_player == null)
                {
                    InitializeAsync();
                    return null;
                }

                return _player;
            }
            set
            {
                if (_player != null)
                {
                    _player.StateChanged -= OnPlayerStateChanged;
                    _player.SourceChanged -= OnPlayerSourceChanged;
                    _player.SourceEnded -= OnPlayerSourceEnded;
                }
                if (value != null)
                {
                    value.StateChanged += OnPlayerStateChanged;
                    value.SourceChanged += OnPlayerSourceChanged;
                    value.SourceEnded += OnPlayerSourceEnded;
                }

                _player = value;
            }
        }
        private static string DefaultTempFolder
        {
            get
            {
                if (field == null)
                {
                    string folder = Path.Combine(Environment.CurrentDirectory, "Temp");

                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }

                    field = folder;
                }

                return field;
            }
        }

        private static readonly TimeSpan _defaultFadeDuration = TimeSpan.FromSeconds(0.5);
        private static readonly Dictionary<ISimplePlayer, List<ValueAnimation>> _animations = [];
        private static ISimplePlayer? _player;
        private static ISimplePlayer? _secondPlayer;
        private static string? _tempFolder;
        private static bool _isInitialized;
        private static bool _isInitializing;
        private static Token? _demoToken;

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
            Type? implementation = await SimplePlayerImplementations.GetPlatformImplementation(Platform)
                ?? throw new SimplePlayerNoImplementation(Platform);
            SimplePlayerImplementationAttribute attribute = implementation.GetCustomAttribute<SimplePlayerImplementationAttribute>();

            if (string.IsNullOrEmpty(attribute.Method))
            {
                return (ISimplePlayer)Activator.CreateInstance(implementation);
            }

            MethodInfo[] methods = implementation.GetMethods();

            foreach (var method in methods)
            {
                if (method.Name != attribute.Method)
                {
                    continue;
                }
                if (method.GetParameters().Length > 0)
                {
                    throw new SimplePlayerIncorrectImplementationCreateMethod(implementation, method.Name);
                }
                if (method.ReturnType == typeof(void))
                {
                    throw new SimplePlayerIncorrectImplementationCreateMethod(implementation, method.Name, nameof(ISimplePlayer));
                }
                if (!typeof(ISimplePlayer).IsAssignableFrom(method.ReturnType))
                {
                    throw new SimplePlayerIncorrectImplementationCreateMethod(implementation, method.Name, method.ReturnType.Name, nameof(ISimplePlayer));
                }

                return (ISimplePlayer)method.Invoke(implementation, null);
            }

            return null;
        }

        #endregion

        #region Установка

        /// <summary>
        /// Установить источник из объекта
        /// </summary>
        /// <param name="source">Объект для установки аудиофайла</param>
        public static async Task<bool> SetSource<T>(T source)
        {
            var player = Player;

            if (player == null || player is not ISimplePlayer<T> typedPlayer)
            {
                return false;
            }

            try
            {
                await typedPlayer.SetSource(source);
                return true;
            }
            catch (Exception error)
            {
                Log.Error(error);
            }

            return false;
        }

        /// <summary>
        /// Выполнить первоначальную установку, чтоб дальнейшем можно было без задержек начать использовать плеер
        /// </summary>
        public static async Task Initialize()
        {
            if (_isInitializing)
            {
                throw new InvalidOperationException(nameof(SimplePlayer) + " уже инициализируется. Пожалуйста, подождите");
            }

            _isInitializing = true;

            try
            {
                Player = await GetCurrentPlatformImplementation();
            }
            catch (Exception error)
            {
                Log.Error(error);
            }

            _isInitialized = true;
            _isInitializing = false;
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
            _demoToken = null;
            Volume = 1;

            Player?.Play();
        }
        /// <summary>
        /// Начать проигрывание с указанной позиции
        /// </summary>
        /// <param name="position">Позиция для проигрывания</param>
        public static void Play(TimeSpan position)
        {
            Position = position;
            Play();
        }
        /// <summary>
        /// Начать проигрывание с указанной позиции
        /// </summary>
        /// <param name="seconds">Позиция для проигрывания в секундах</param>
        public static void Play(float seconds)
        {
            Play(TimeSpan.FromSeconds(seconds));
        }

        /// <summary>
        /// Получить данные спектра указанного размера с применением указанного FFT окна
        /// </summary>
        /// <param name="size">Размер требуемых данных спектра</param>
        /// <param name="window">FFT окно</param>
        /// <returns>Данные спектра</returns>
        public static float[] GetSpectrumData(int size, SpectrumWindow window = SpectrumWindow.Rectangular)
        {
            if (Player == null)
            {
                return [];
            }

            return Player.GetSpectrumData(size, window);
        }
        /// <summary>
        /// Получить данные спектра указанного размера со сглаживанием и применением указанного FFT окна
        /// </summary>
        /// <param name="size">Размер требуемых данных спектра</param>
        /// <param name="smoothAmount">Степень сглаживания. 
        /// Например, пусть степень сглаживания будет равна 2, то значение arr[i] будет равно среднему арифметическому значений от arr[i - 2] до arr[i + 2] включительно
        /// </param>
        /// <param name="window">FFT окно</param>
        /// <returns>Данные спектра со сглаживанием</returns>
        public static float[] GetSpectrumData(int size, int smoothAmount, SpectrumWindow window = SpectrumWindow.Rectangular)
        {
            if (Player == null)
            {
                return [];
            }

            return Player.GetSpectrumData(size, smoothAmount, window);
        }
        /// <summary>
        /// Получить готовые к использованию данные спектра указанного размера со сглаживанием и применением указанного FFT окна
        /// </summary>
        /// <param name="size">Размер требуемых данных спектра</param>
        /// <param name="window">FFT окно</param>
        /// <returns>Готовые к использованию данные спектра</returns>
        public static float[] GetFilteredSpectrumData(int size, SpectrumWindow window = SpectrumWindow.Rectangular)
        {
            if (Player == null)
            {
                return [];
            }

            return Player.GetFilteredSpectrumData(size, window);
        }
        /// <summary>
        /// Получить готовые к использованию данные спектра со сглаживанием указанного размера со сглаживанием и применением указанного FFT окна
        /// </summary>
        /// <param name="size">Размер требуемых данных спектра</param>
        /// <param name="smoothAmount">Степень сглаживания. 
        /// Например, пусть степень сглаживания будет равна 2, то значение arr[i] будет равно среднему арифметическому значений от arr[i - 2] до arr[i + 2] включительно
        /// </param>
        /// <param name="window">FFT окно</param>
        /// <returns>Готовые к использованию данные спектра</returns>
        public static float[] GetFilteredSpectrumData(int size, int smoothAmount, SpectrumWindow window = SpectrumWindow.Rectangular)
        {
            if (Player == null)
            {
                return [];
            }

            return Player.GetFilteredSpectrumData(size, smoothAmount, window);
        }
        /// <summary>
        /// Получить сглаженные данные спектра. Таки данные не должны быть резкими
        /// </summary>
        /// <param name="size">Размер требуемых данных спектра</param>
        /// <param name="decreaseSize">Размер уменьшения</param>
        /// <param name="decreaseMultiplier">Множитель уменьшения. Чем дольше, тем быстрее уменьшается</param>
        /// <param name="window">FFT окно</param>
        /// <returns>Данные спектра</returns>
        public static float[] GetBufferedSpectrumData(int size, float decreaseSize = 0.005f, float decreaseMultiplier = 1.2f, SpectrumWindow window = SpectrumWindow.Rectangular)
        {
            float[] data = GetSpectrumData(size, window);
            data = SpectrumBandBuffer.ApplyBuffer(data, decreaseSize, decreaseMultiplier);

            return data;
        }
        /// <summary>
        /// Получить сглаженные данные спектра. Таки данные не должны быть резкими
        /// </summary>
        /// <param name="size">Размер требуемых данных спектра</param>
		/// <param name="smoothAmount">Степень сглаживания. 
		/// Например, пусть степень сглаживания будет равна 2, то значение arr[i] будет равно среднему арифметическому значений от arr[i - 2] до arr[i + 2] включительно
		/// </param>
        /// <param name="decreaseSize">Размер уменьшения</param>
        /// <param name="decreaseMultiplier">Множитель уменьшения. Чем дольше, тем быстрее уменьшается</param>
        /// <param name="window">FFT окно</param>
        /// <returns>Данные спектра</returns>
        public static float[] GetBufferedSpectrumData(int size, int smoothAmount, float decreaseSize = 0.005f, float decreaseMultiplier = 1.2f, SpectrumWindow window = SpectrumWindow.Rectangular)
        {
            float[] data = GetSpectrumData(size, smoothAmount, window);
            data = SpectrumBandBuffer.ApplyBuffer(data, decreaseSize, decreaseMultiplier);

            return data;
        }

        #endregion

        #region Дополнительное управление

        /// <summary>
        /// Включить демо-проигрывание. Такое проигрывается даёт прослушать небольшой отрывок источника. Этот отрывок зациклен
        /// </summary>
        public static void PlayDemo()
        {
            TimeSpan startPosition = TimeSpan.FromMinutes(1);
            TimeSpan duration = Duration - startPosition - TimeSpan.FromSeconds(30);

            PlayDemo(startPosition, duration);
        }
        /// <summary>
        /// Включить демо-проигрывание с указанным временем начала и продолжительностью.
        /// Такое проигрывается даёт прослушать небольшой отрывок источника. Этот отрывок зациклен
        /// </summary>
        /// <param name="startPosition">Время начала проигрывание</param>
        /// <param name="duration">Продолжительность отрывка</param>
        public static void PlayDemo(TimeSpan startPosition, TimeSpan duration)
        {
            var currentPlayer = Player;

            if (currentPlayer == null)
            {
                return;
            }

            RemoveAnimation();
            Play(startPosition);

            _demoToken = new();
            DemoTimerTick(currentPlayer, _demoToken.Value, startPosition, duration);
        }

        /// <summary>
        /// Плавное возобновление проигрывания
        /// </summary>
        /// <param name="duration">Продолжительность возобновления</param>
        public static void FadePlay(TimeSpan duration)
        {
            RemoveAnimation();

            var currentPlayer = Player;

            if (currentPlayer == null)
            {
                return;
            }

            _demoToken = null;
            currentPlayer.Volume = 0;
            currentPlayer.Play();

            var animation = GetAnimation(0, 1, duration, Easing.Linear, value =>
            {
                currentPlayer.Volume = value;
            });

            AddAnimation(animation);
        }
        /// <summary>
        /// Плавное возобновление проигрывания
        /// </summary>
        public static void FadePlay()
        {
            FadePlay(_defaultFadeDuration);
        }

        /// <summary>
        /// Плавная остановка проигрывания
        /// </summary>
        /// <param name="duration">Продолжительность затухания</param>
        /// <param name="paused">Действие после окончания затухания</param>
        public static void FadePause(TimeSpan duration, Action? paused = null)
        {
            RemoveAnimation();

            float startVolume = Volume;
            var currentPlayer = Player;

            if (currentPlayer == null)
            {
                return;
            }

            var animation = GetAnimation(Volume, 0, duration, Easing.Linear, value =>
            {
                currentPlayer.Volume = value;
            }, () =>
            {
                currentPlayer.Pause();
                currentPlayer.Volume = startVolume;
                paused?.Invoke();
            });

            AddAnimation(animation);
        }
        /// <summary>
        /// Плавная остановка проигрывания
        /// </summary>
        /// <param name="paused">Действие после окончания затухания</param>
        public static void FadePause(Action? paused = null)
        {
            FadePause(_defaultFadeDuration, paused);
        }

        /// <summary>
        /// Плавная остановка проигрывания через замедление скорости
        /// </summary>
        /// <param name="duration">Продолжительность замедления</param>
        /// <param name="paused">Действие после окончания замедления</param>
        public static void PitchPause(TimeSpan duration, Action? paused = null)
        {
            RemoveAnimation();

            var currentPlayer = Player;
            float startSpeed = Speed;

            if (currentPlayer == null)
            {
                return;
            }

            var animation = GetAnimation(startSpeed, 0, duration, Easing.Linear, value =>
            {
                currentPlayer.Speed = value;
            }, () =>
            {
                Pause();
                currentPlayer.Speed = startSpeed;
                paused?.Invoke();
            });

            AddAnimation(animation);
        }
        /// <summary>
        /// Плавная остановка проигрывания через замедление скорости
        /// </summary>
        /// <param name="paused">Действие после окончания замедления</param>
        public static void PitchPause(Action? paused = null)
        {
            PitchPause(_defaultFadeDuration, paused);
        }

        /// <summary>
        /// Плавно сменить источник плеера
        /// </summary>
        /// <param name="sourceSetter">Установщик источника</param>
        /// <param name="startPosition">Начальная позиция проигрывания</param>
        /// <param name="duration">Продолжительность плавного перехода</param>
        /// <exception cref="InvalidOperationException"></exception>
        public static async void SmoothSwitchSource(Func<ISimplePlayer, Task> sourceSetter, TimeSpan startPosition, TimeSpan duration)
        {
            if (Player == null)
            {
                throw new InvalidOperationException("Невозможно плавно сменить источник, так как плеер не был создан");
            }
            if (sourceSetter == null)
            {
                throw new ArgumentNullException(nameof(sourceSetter));
            }

            _secondPlayer ??= await GetCurrentPlatformImplementation();

            if (_secondPlayer == null)
            {
                throw new InvalidOperationException("Не удалось начать плавную смену источника, так как не удалось создать второй экземпляр плеера");
            }

            bool autoPlayEnabled = Player.AutoPlay;
            float volume = Player.Volume;
            _secondPlayer.CopyValues(Player);
            _secondPlayer.AutoPlay = false;

            await sourceSetter(_secondPlayer);

            var currentPlayer = Player;
            Player = _secondPlayer;
            _secondPlayer = currentPlayer;

            currentPlayer = Player;
            var SecondPlayer = _secondPlayer;

            void VolumeAnimationTick(ValueAnimation animation, float value)
            {
                currentPlayer.Volume = value;
                SecondPlayer.Volume = volume - value;
            }
            void VolumeAnimationEnded(ValueAnimation animation, AnimationMode mode)
            {
                SecondPlayer.Stop();
            }

            ValueAnimation animation = new(0, volume)
            {
                Duration = duration
            };
            animation.Tick += VolumeAnimationTick;
            animation.Ended += VolumeAnimationEnded;

            currentPlayer.Volume = 0;
            currentPlayer.Position = startPosition;

            currentPlayer.Play();
            animation.Start();
        }

        /// <summary>
        /// Переключиться на другой экземпляр плеера, при этом сохранив текущие действия.
        /// Например, если начать плавную остановку проигрывания и переключить экземпляр,
        /// то плавная остановка продолжится на том экземпляре на котором была начала
        /// </summary>
        public static async void SwitchInstances()
        {
            _secondPlayer ??= await GetCurrentPlatformImplementation();

            if (_secondPlayer == null)
            {
                throw new InvalidOperationException("Невозможно переключиться на другой экземпляр, так как не удалось создать второй экземпляр плеера");
            }

            var currentPlayer = Player;
            Player = _secondPlayer;
            _secondPlayer = currentPlayer;
        }

        private static async void DemoTimerTick(ISimplePlayer player, Token token, TimeSpan startPosition, TimeSpan duration)
        {
            await Task.Delay(duration);

            if (token != _demoToken || Player != player)
            {
                return;
            }

            SimplePlayerState startState = player.State;

            FadePause(() =>
            {
                player.Position = startPosition;

                if (startState == SimplePlayerState.Playing)
                {
                    player.Play();
                }
            });

            DemoTimerTick(player, token, startPosition, duration);
        }

        #endregion

        #region Анимация значения

        private static void AddAnimation(ValueAnimation animation)
        {
            var currentPlayer = Player;

            if (currentPlayer == null)
            {
                return;
            }
            if (!_animations.TryGetValue(currentPlayer, out var animationsList))
            {
                animationsList = [];
                _animations.Add(currentPlayer, animationsList);
            }

            animationsList.Add(animation);
        }
        private static void RemoveAnimation()
        {
            var currentPlayer = Player;

            if (currentPlayer == null)
            {
                return;
            }

            if (_animations.TryGetValue(currentPlayer, out var animations))
            {
                foreach (var animation in animations)
                {
                    animation.Stop();
                }

                animations.Clear();
            }
        }
        private static ValueAnimation GetAnimation(float from, float to, TimeSpan duration, Easing easing, Action<float> updated, Action? completed = null)
        {
            ValueAnimation animation = new()
            {
                From = from,
                To = to,
                Duration = duration,
                Easing = easing
            };

            animation.Tick += OnAnimationTick;
            animation.Ended += OnAnimationEnded;

            void OnAnimationTick(ValueAnimation animation, float value)
            {
                updated?.Invoke(value);
            }
            void OnAnimationEnded(ValueAnimation animation, AnimationMode mode)
            {
                animation.Tick -= OnAnimationTick;
                animation.Ended -= OnAnimationEnded;
                completed?.Invoke();
            }

            return animation.Start();
        }

        #endregion

        #region События

        private static void OnPlayerStateChanged(ISimplePlayer player, SimplePlayerState state) => StateChanged?.Invoke(player, state);
        private static void OnPlayerSourceChanged(ISimplePlayer player) => SourceChanged?.Invoke(player);
        private static void OnPlayerSourceEnded(ISimplePlayer player) => SourceEnded?.Invoke(player);

        #endregion
    }
}
