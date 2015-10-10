using System;
using WMPLib;

namespace MusicTimeCore
{
    public class AudioManager : IDisposable
    {
        private readonly WindowsMediaPlayer _mainPlayer;
        
        public int Volume
        {
            get { return _mainPlayer.settings.volume; }
            set { _mainPlayer.settings.volume = value; }
        }

        public bool Mute
        {
            get { return _mainPlayer.settings.mute; }
            set { _mainPlayer.settings.mute = value; }
        }

        public string StatusString
        {
            get { return _mainPlayer.status; }
        }

        public IWMPMedia CurrentMedia
        {
            get { return _mainPlayer.currentMedia; }
        }
        
        public AudioManager()
        {
            _mainPlayer = new WindowsMediaPlayerClass();
        }

        public void Play(string uri)
        {
            _mainPlayer.controls.stop();
            _mainPlayer.URL = uri;
        }

        public void Stop()
        {
            _mainPlayer.controls.stop();
        }

        public void Previous()
        {
            _mainPlayer.controls.previous();
        }

        public void Next()
        {
            _mainPlayer.controls.next();
        }

        

        public void Dispose()
        {
            _mainPlayer.close();
        }

    }
}
