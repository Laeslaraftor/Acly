using Acly.Tokens;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Acly.Numbers
{
    /// <summary>
    /// Асинхронная анимация значения
    /// </summary>
    public class AsyncValueAnimation : ValueAnimation
    {
        private Token? _taskToken;
        private readonly Queue<Action> _framesToComplete = new();

        #region Анимация

        /// <summary>
        /// Запустить проигрывание анимации
        /// </summary>
        /// <param name="from">Начальное значение анимации</param>
        /// <param name="to">Конечное значение анимации</param>
        /// <param name="mode">Режим проигрывания анимации</param>
        protected override void StartAnimation(float from, float to, AnimationMode mode)
        {
            Token currentTaskToken = new();
            _taskToken = currentTaskToken;

            Ended += OnAnimationEnded;

            _framesToComplete.Clear();
            StartAnimationTask(currentTaskToken);

            base.StartAnimation(from, to, mode);
        }

        /// <summary>
        /// Выполнить кадр анимации в отдельном потоке
        /// </summary>
        /// <param name="from">Начальное значение анимации</param>
        /// <param name="to">Конечное значение анимации</param>
        /// <param name="frames">Количество кадров анимации</param>
        /// <param name="framesCompleted">Количество выполненных кадров</param>
        protected override void DoAnimationLoopFrame(float from, float to, int frames, int framesCompleted)
        {
            _framesToComplete.Enqueue(() =>
            {
                base.DoAnimationLoopFrame(from, to, frames, framesCompleted);
            });
        }

        #endregion

        #region Отдельный поток

        private async void StartAnimationTask(Token taskToken)
        {
            await Task.Run(async () =>
            {
                while (taskToken == _taskToken)
                {
                    AnimationTaskTick();
                    await Task.Delay(10);
                }

                Log.Message("Поток завершён");
            });
        }
        private void AnimationTaskTick()
        {
            if (_framesToComplete.TryDequeue(out Action frame))
            {
                frame?.Invoke();
            }
        }

        #endregion

        #region События

        private void OnAnimationEnded(ValueAnimation animation, AnimationMode mode)
        {
            Ended -= OnAnimationEnded;
            _taskToken = null;
        }

        #endregion
    }
}
