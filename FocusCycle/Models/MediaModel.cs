using System.IO;
using System.Windows.Media;

namespace FocusCycle.Models
{
    class MediaModel : IDisposable
    {
        private Stream? _stream;
        private string? _tempDirectory;
        private string? _tempFile;
        private readonly MediaPlayer _mediaPlayer;

        public double Volume
        {
            get => _mediaPlayer.Volume;
            set
            {
                if (_mediaPlayer.Volume == value) return;
                if (value < 0.0 || value > 1.0)
                    throw new ArgumentException("Значение громкости не должен быть ниже 0 и выше 1");
                _mediaPlayer.Volume = value;
            }
        }

        public void Play()
        {
            _stream ??= InitializeStream();

            if (!File.Exists(_tempFile))
            {
                if (!Directory.Exists(_tempDirectory))
                {
                    _tempDirectory ??= Path.Combine(Path.GetTempPath(), "FocusCycle");
                    Directory.CreateDirectory(_tempDirectory);
                }
                _tempFile ??= Path.Combine(_tempDirectory, "notification.wav");
                using (FileStream fs = File.Create(_tempFile))
                    _stream.CopyTo(fs);
                _mediaPlayer.Open(new Uri(_tempFile));
            }
            _mediaPlayer.Play();
        }

        public void Stop() => _mediaPlayer.Stop();

        public void Dispose()
        {
            _mediaPlayer.Stop();
            _mediaPlayer.MediaEnded -= _mediaPlayer_MediaEnded;
            _mediaPlayer.Close();
            if(Directory.Exists(_tempDirectory))
                Directory.Delete(_tempDirectory, true);
        }

        public MediaModel()
        {
            _mediaPlayer = new MediaPlayer();
            _mediaPlayer.MediaEnded += _mediaPlayer_MediaEnded;
        }

        private Stream InitializeStream() => App
            .GetResourceStream((Uri)App.Current.Resources["Notification"])
            .Stream;


        private void _mediaPlayer_MediaEnded(object? sender, EventArgs e)
        {
            _mediaPlayer.Position = TimeSpan.Zero;
            _mediaPlayer.Play();
        }

    }
}
