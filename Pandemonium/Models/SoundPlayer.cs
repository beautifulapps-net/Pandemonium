using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IrrKlang;

namespace Pandemonium.Models
{
    public class SoundPlayer:ISoundStopEventReceiver
    {
        public enum PlayState
        {
            Playing,
            Paused,
            Stopped
        }

        public delegate void SoundStopped(SoundPlayer sender);
        public event SoundStopped Stopped;
        private bool _hasStopped = false;

        private static ISoundEngine SoundEngine { get; }

        static SoundPlayer()
        {
            SoundEngine = new ISoundEngine();
        }

        public SoundPlayer(string soundFile)
            : this(soundFile, true, true)
        { }

        public SoundPlayer(string soundFile, bool playLooped, bool startPaused)
        {
            SoundFile = soundFile;
            Reload(playLooped, startPaused);
        }

        public void Reload()
        {
            Reload(Looped, Paused);
            Volume = 100;
        }

        public void Reload(bool playLooped, bool startPaused)
        {
            _hasStopped = false;
            Sound = SoundEngine.Play2D(SoundFile, playLooped, startPaused);
        }

        public string SoundFile { get; }

        private ISound Sound { get; set; }


        public bool Paused
        {
            get{return Sound.Paused;}
            set{Sound.Paused = value;}
        }

        public PlayState State
        {
            get
            {
                if (_hasStopped)
                {
                    return PlayState.Stopped;
                }

                return Sound.Paused ? PlayState.Paused : PlayState.Playing;
            }
            set
            {
                if(value== PlayState.Paused)
                {
                    Sound.Paused = true;
                }
                else if(value== PlayState.Playing)
                {
                    Sound.Paused = false;
                }
                else if (value == PlayState.Stopped)
                {
                    Sound.Stop();
                }
            }
        }
        public float PlaybackSpeed
        {
            get { return Sound.PlaybackSpeed; }
            set { Sound.PlaybackSpeed = value; }
        }

        public bool Looped
        {
            get { return Sound.Looped; }
            set { Sound.Looped = value; }
        }

        private int _volume = 100;
        public int Volume
        {
            get
            {
                return _volume;
            }
            set
            {
                if ((value < 0) || (value > 100)) return;

                _volume = value;
                var result = _volume / 100f;
                Sound.Volume = result;
            }
        }

        void ISoundStopEventReceiver.OnSoundStopped(ISound sound, StopEventCause reason, object userData)
        {
            _hasStopped = true;
            Stopped?.Invoke(this);
        }

    }
}
