using Acly.Player;

namespace Acly.Test.Player
{
    [SimplePlayerImplementation(Platforms.RuntimePlatform.Windows)]
    public class TestPlayer : ITestPlayer
    {
        public TimeSpan Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public TimeSpan Duration => throw new NotImplementedException();

        public float Speed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float Volume { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool Loop { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool IsPlaying => throw new NotImplementedException();

        public bool SourceSetted => throw new NotImplementedException();

        public bool AutoPlay { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public SimplePlayerState State => throw new NotImplementedException();

        public event Action<SimplePlayerState>? StateChanged;
        public event Action? SourceChanged;
        public event Action? SourceEnded;

        public float[] GetSpectrumData(int Size, SpectrumWindow Window = SpectrumWindow.Rectangular)
        {
            throw new NotImplementedException();
        }

        public float[] GetSpectrumData(int Size, int SmoothAmount, SpectrumWindow Window = SpectrumWindow.Rectangular)
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Play()
        {
            throw new NotImplementedException();
        }

        public void Release()
        {
            throw new NotImplementedException();
        }

        public Task SetSource(byte[] Data)
        {
            throw new NotImplementedException();
        }

        public Task SetSource(Stream SourceStream)
        {
            throw new NotImplementedException();
        }

        public Task SetSource(string SourceUrl)
        {
            throw new NotImplementedException();
        }

        public Task SetSource(object Data)
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
