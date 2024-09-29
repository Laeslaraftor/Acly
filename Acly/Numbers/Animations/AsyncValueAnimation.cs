using System.Threading.Tasks;
using System;
using Acly.Tokens;
using System.Collections.Generic;

namespace Acly.Numbers
{
	/// <summary>
	/// Асинхронная анимация значения
	/// </summary>
	public class AsyncValueAnimation : ValueAnimation
	{
		private Token? _TaskToken;
		private readonly Queue<Action> _FramesToComplete = new();

		#region Анимация

		/// <summary>
		/// Запустить проигрывание анимации
		/// </summary>
		/// <param name="From">Начальное значение анимации</param>
		/// <param name="To">Конечное значение анимации</param>
		/// <param name="Mode">Режим проигрывания анимации</param>
		protected override void StartAnimation(float From, float To, AnimationMode Mode)
		{
			Token CurrentTaskToken = new();
			_TaskToken = CurrentTaskToken;

			Ended += OnAnimationEnded;

			_FramesToComplete.Clear();
			StartAnimationTask(CurrentTaskToken);

			base.StartAnimation(From, To, Mode);
		}

		/// <summary>
		/// Выполнить кадр анимации в отдельном потоке
		/// </summary>
		/// <param name="From">Начальное значение анимации</param>
		/// <param name="To">Конечное значение анимации</param>
		/// <param name="Frames">Количество кадров анимации</param>
		/// <param name="FramesCompleted">Количество выполненных кадров</param>
		protected override void DoAnimationLoopFrame(float From, float To, int Frames, int FramesCompleted)
		{
			_FramesToComplete.Enqueue(() =>
			{
				base.DoAnimationLoopFrame(From, To, Frames, FramesCompleted);
			});
		}

		#endregion

		#region Отдельный поток

		private async void StartAnimationTask(Token TaskToken)
		{
			await Task.Run(async () =>
			{
				while (TaskToken == _TaskToken)
				{
					AnimationTaskTick();
					await Task.Delay(10);
				}

				Log.Message("Поток завершён");
			});
		}
		private void AnimationTaskTick()
		{
			if (_FramesToComplete.TryDequeue(out Action Frame))
			{
				Frame?.Invoke();
			}
		}

		#endregion

		#region События

		private void OnAnimationEnded(ValueAnimation Animation, AnimationMode Mode)
		{
			Ended -= OnAnimationEnded;
			_TaskToken = null;
		}

		#endregion
	}
}
