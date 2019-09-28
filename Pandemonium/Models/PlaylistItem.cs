using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;

namespace Pandemonium.Models
{
    public class PlaylistItem: INotifyPropertyChanged
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
    }
}
