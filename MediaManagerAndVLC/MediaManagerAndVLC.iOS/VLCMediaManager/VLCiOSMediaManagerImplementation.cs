using System;
using LibVLCSharp.Shared;
using MediaManager;
using MediaManager.Library;
using MediaManager.Media;
using MediaManager.Notifications;
using MediaManager.Platforms.Apple.Media;
using MediaManager.Playback;
using MediaManager.Player;
using MediaManager.Volume;

namespace MediaManagerAndVLC.iOS.VLCMediaManager
{
    public class VLCiOSMediaManagerImplementation : MediaManagerBase, IMediaManager<LibVLCSharp.Shared.MediaPlayer>
    {
        public VLCiOSMediaPlayer VLCiOSMediaPlayer => (VLCiOSMediaPlayer)MediaPlayer;
        public LibVLCSharp.Shared.MediaPlayer Player => VLCiOSMediaPlayer?.Player;

        //Shadow the other event handlers, since they are hidden and not usable by derived members
        public new event StateChangedEventHandler StateChanged;
        public new event BufferedChangedEventHandler BufferedChanged;
        public new event PositionChangedEventHandler PositionChanged;

        public new event MediaItemFinishedEventHandler MediaItemFinished;
        public new event MediaItemChangedEventHandler MediaItemChanged;
        public new event MediaItemFailedEventHandler MediaItemFailed;

        private VLCiOSMediaPlayer _player;
        private IMediaPlayer _mediaPlayer;
        public override IMediaPlayer MediaPlayer
        {
            get
            {
                if (_mediaPlayer == null)
                {
                    _player = new VLCiOSMediaPlayer();
                    _mediaPlayer = _player;
                    AttachPlayerEvents();
                }

                return _mediaPlayer;
            }
            set => SetProperty(ref _mediaPlayer, value);
        }

        private IMediaExtractor _extractor;
        public override IMediaExtractor Extractor
        {
            get
            {
                if (_extractor == null)
                    _extractor = new AppleMediaExtractor();
                return _extractor;
            }
            set => SetProperty(ref _extractor, value);
        }

        private IVolumeManager _volume;
        public override IVolumeManager Volume
        {
            get
            {
                if (_volume == null)
                    _volume = new VLCVolumeManager();
                return _volume;
            }
            set => SetProperty(ref _volume, value);
        }

        private INotificationManager _notification;
        public override INotificationManager Notification
        {
            get
            {
                if (_notification == null)
                    _notification = new VLCiOSNotificationManager();

                return _notification;
            }
            set => SetProperty(ref _notification, value);
        }

        public override TimeSpan Position => TimeSpan.FromMilliseconds(VLCiOSMediaPlayer.Player.Time);
        public override TimeSpan Duration => TimeSpan.FromMilliseconds(VLCiOSMediaPlayer.Player.Length);

        //TODO later Use VLC Player's "SetRate" method
        public override float Speed { get; set; }
        //TODO later, Already implemented inn iOS MediaImplementation (Code repetition)
        public override bool KeepScreenOn { get; set; }

        /// <summary>
        /// NOTE: We attach player events here instead of doing it in the player
        /// its self as what is properly done with other players because,
        /// The event calls are internal, and we had the shadow them in other to invoke them here.
        /// </summary>
        void AttachPlayerEvents()
        {
            Player.Muted += PlayerOnMuted;
            Player.Unmuted += PlayerOnUnmuted;
            Player.TimeChanged += OnTimeChanged;
            Player.EncounteredError += OnEncounteredError;
            Player.PositionChanged += OnPositionChanged;
            Player.LengthChanged += OnLengthChanged;
            Player.EndReached += OnEndReached;
            Player.Paused += OnPaused;
            Player.PausableChanged += OnPausableChanged;
            Player.Playing += OnPlaying;
            Player.MediaChanged += OnMediaChanged;
            Player.Stopped += OnStopped;
            Player.Buffering += OnBuffering;
        }

        private void OnBuffering(object sender, MediaPlayerBufferingEventArgs e)
        {
            _state = MediaPlayerState.Buffering;
        }

        private void OnStopped(object sender, EventArgs e)
        {
            _state = MediaPlayerState.Stopped;
        }

        private void OnMediaChanged(object sender, MediaPlayerMediaChangedEventArgs e)
        {
            //VLCiOSMediaPlayer.Initialize();
            //VLCiOSMediaPlayer.Dispose();
            //_mediaPlayer = null;
            //var p = MediaPlayer;
            MediaItemChanged?.Invoke(Player, new MediaItemEventArgs(new MediaItem
            {
                MediaUri = e.Media.Mrl,
            }));
            //_mediaPlayer = null;
            //var p = MediaPlayer;
        }

        void DettachPlayerEvents()
        {
            Player.Muted -= PlayerOnMuted;
            Player.Unmuted -= PlayerOnUnmuted;
            Player.TimeChanged -= OnTimeChanged;
            Player.EncounteredError -= OnEncounteredError;
            Player.PositionChanged -= OnPositionChanged;
            Player.LengthChanged -= OnLengthChanged;
            Player.EndReached -= OnEndReached;
            Player.Paused -= OnPaused;
            Player.PausableChanged -= OnPausableChanged;
            Player.Playing -= OnPlaying;
            Player.MediaChanged -= OnMediaChanged;
            Player.Stopped -= OnStopped;
            Player.Buffering -= OnBuffering;
        }

        private void PlayerOnUnmuted(object sender, EventArgs e)
        {
            var m = Player.Mute;
        }

        private void PlayerOnMuted(object sender, EventArgs e)
        {
            var m = Player.Mute;
        }

        #region Player events
        private void OnEncounteredError(object sender, EventArgs e)
        {
            _state = MediaPlayerState.Failed;
            MediaItemFailed?.Invoke(Player, new MediaItemFailedEventArgs(Queue.Current, 
                new Exception(e.ToString()), "VLC Player failed to play item."));
        }

        protected virtual void OnPausableChanged(object sender, MediaPlayerPausableChangedEventArgs e)
        {
            ;
            //throw new NotImplementedException();
        }

        private void OnPlaying(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            _state = MediaPlayerState.Playing;
        }

        protected virtual void OnPaused(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            _state = MediaPlayerState.Paused;
        }

        protected virtual void OnEndReached(object sender, EventArgs e)
        {
            MediaItemFinished?.Invoke(Player, new MediaItemEventArgs(Queue.Current));
        }

        protected virtual void OnLengthChanged(object sender, MediaPlayerLengthChangedEventArgs e)
        {
            // throw new NotImplementedException();
            ;
        }

        private void OnPositionChanged(object sender, LibVLCSharp.Shared.MediaPlayerPositionChangedEventArgs e)
        {
            PositionChanged?.Invoke(this, new PositionChangedEventArgs(TimeSpan.FromMilliseconds(Player.Time)));
        }

        protected virtual void OnTimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            ;
            //throw new NotImplementedException();
        }

        #endregion

        public override void Dispose()
        {
            DettachPlayerEvents();
            base.Dispose();
        }
    }
}
