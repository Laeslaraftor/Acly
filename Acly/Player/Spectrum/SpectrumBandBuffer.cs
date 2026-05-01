using System;

namespace Acly.Player
{
    /// <summary>
    /// Работа со спектром
    /// </summary>
    public static class SpectrumBandBuffer
    {
        private static float[]? _buffer;
        private static float[]? _bufferDecrease;

        /// <summary>
        /// Применить буфер для массива. Применяя этот буфер, значения, находящиеся в массиве уменьшаются, пока не станут равны нулю.
        /// Чем дольше значения уменьшаются тем сильнее начинают уменьшаться. С каждым уменьшением скорость умножается на множитель уменьшения.
        /// </summary>
        /// <param name="samples">Массив для уменьшения</param>
        /// <param name="decreaseSize">Начальная скорость уменьшения</param>
        /// <param name="decreaseMultiplier">Множитель уменьшения</param>
        /// <returns>Плавно уменьшенный буфер</returns>
        public static float[] ApplyBuffer(float[] samples, float decreaseSize = 0.005f, float decreaseMultiplier = 1.2f)
        {
            return ApplyBuffer(samples, ref _buffer, ref _bufferDecrease, decreaseSize, decreaseMultiplier);
        }
        /// <summary>
        /// Применить буфер для массива. Применяя этот буфер, значения, находящиеся в массиве уменьшаются, пока не станут равны нулю.
        /// Чем дольше значения уменьшаются тем сильнее начинают уменьшаться. С каждым уменьшением скорость умножается на множитель уменьшения.
        /// </summary>
        /// <param name="samples">Массив для уменьшения</param>
        /// <param name="buffer">Буфер</param>
        /// <param name="bufferDecrease">Буфер уменьшения</param>
        /// <param name="decreaseSize">Начальная скорость уменьшения</param>
        /// <param name="decreaseMultiplier">Множитель уменьшения</param>
        /// <returns>Плавно уменьшенный буфер</returns>
        public static float[] ApplyBuffer(float[] samples, ref float[]? buffer, ref float[]? bufferDecrease, float decreaseSize = 0.005f, float decreaseMultiplier = 1.2f)
        {
            if (samples == null)
            {
                throw new ArgumentNullException(nameof(samples));
            }

            if (buffer == null || samples.Length != buffer.Length || bufferDecrease == null)
            {
                buffer = samples;
                bufferDecrease = new float[samples.Length];
                return samples;
            }

            for (int i = 0; i < samples.Length; i++)
            {
                if (samples[i] > buffer[i])
                {
                    buffer[i] = samples[i];
                    bufferDecrease[i] = decreaseSize;
                }
                else if (samples[i] < buffer[i])
                {
                    buffer[i] = MathF.Max(buffer[i] - bufferDecrease[i], 0);
                    bufferDecrease[i] *= decreaseMultiplier;
                }
            }

            return buffer;
        }
    }
}
