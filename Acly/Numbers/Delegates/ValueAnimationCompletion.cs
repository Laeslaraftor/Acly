namespace Acly.Numbers
{
    /// <summary>
    /// Событие окончания анимации
    /// </summary>
    /// <param name="animation">Анимация, которая вызвала событие</param>
    /// <param name="mode">Режим проигрывания анимации</param>
    public delegate void ValueAnimationCompletion(ValueAnimation animation, AnimationMode mode);
}
