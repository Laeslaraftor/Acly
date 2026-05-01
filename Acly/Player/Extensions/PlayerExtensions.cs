using System;

namespace Acly.Player.Extensions
{
    /// <summary>
    /// Класс с методами расширения для плеера
    /// </summary>
    public static class PlayerExtensions
    {
        extension(ISimplePlayer player)
        {
            /// <summary>
            /// Проигрывается ли сейчас аудио
            /// </summary>
            public bool IsPlaying => player.State == SimplePlayerState.Playing;
            /// <summary>
            /// Установлен ли источник аудио
            /// </summary>
            public bool SourceSetted => player.Source != null;

            /// <summary>
            /// Переключить состояние плеера воспроизвести/пауза
            /// </summary>
            /// <returns>Новое состояние плеера</returns>
            /// <exception cref="ArgumentNullException"></exception>
            public SimplePlayerState SwitchState()
            {
                if (player == null)
                {
                    throw new ArgumentNullException(nameof(player), "Плеер не указан");
                }

                if (player.State != SimplePlayerState.Playing)
                {
                    player.Play();
                    return player.State;
                }

                player.Pause();

                return player.State;
            }
            /// <summary>
            /// Скопировать значения основных параметров из другого плеера
            /// </summary>
            /// <param name="reference">Плеер, значения которого будут использоваться</param>
            /// <exception cref="ArgumentNullException"></exception>
            public void CopyValues(ISimplePlayer reference)
            {
                if (player == null)
                {
                    throw new ArgumentNullException(nameof(player));
                }
                if (reference == null)
                {
                    throw new ArgumentNullException(nameof(player));
                }

                player.AutoPlay = reference.AutoPlay;
                player.Loop = reference.Loop;
                player.Speed = reference.Speed;
                player.Volume = reference.Volume;
            }
        }
    }
}
