using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;

namespace Pandemonium.Models
{
    public class PlaylistItem: INotifyPropertyChanged, IDisposable
    {
        public PlaylistItem(string soundFile)
        {
            var fi = new System.IO.FileInfo(soundFile);
            if (!fi.Exists) throw new FileNotFoundException($"File '{soundFile}' does not exist or is not accessible.");

            Name = fi.Name.Remove(fi.Name.Length - fi.Extension.Length, fi.Extension.Length);

            SoundPlayer = new SoundPlayer(soundFile);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SoundFile)));
        }

        private SoundPlayer SoundPlayer { get; }

        private string _name = null;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            }
        }

        public int Volume
        {
            get => SoundPlayer.Volume;
            set
            {
                SoundPlayer.Volume = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Volume)));
            }
        }

        public float PlaybackSpeed
        {
            get => SoundPlayer.PlaybackSpeed;
            set
            {
                SoundPlayer.PlaybackSpeed = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PlaybackSpeed)));
            }
        }

        public SoundPlayer.PlayState State
        {
            get => SoundPlayer.State;
            set
            {
                SoundPlayer.State = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Paused)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(State)));
            }
        }

        public bool Paused
        {
            get => SoundPlayer.Paused;
            set
            {
                SoundPlayer.Paused = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Paused)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(State)));
            }
        }

        public string SoundFile { get => SoundPlayer.SoundFile; }

        public event PropertyChangedEventHandler PropertyChanged;


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // free unmanaged resources (unmanaged objects) and override a finalizer below.
                SoundPlayer.State = SoundPlayer.PlayState.Stopped;
                // set large fields to null.

                disposedValue = true;
            }
        }

        // override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~PlaylistItem()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
