using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AVFoundation;
using Foundation;
using LibVLCSharp.Shared;
using MediaManager.Library;
using MediaManager.Player;
using MediaManager.Video;
using UIKit;
using Xamarin.Forms;
using IVideoView = MediaManager.Video.IVideoView;

namespace VLCBindings.iOS
{
    public class VLCiOSMediaPlayer : MediaPlayerBase, IMediaPlayer<LibVLCSharp.Shared.MediaPlayer>
    {
        public override IVideoView VideoView { get; set; }
        public VLCiOSMediaManagerImplementation MediaManager => VLCCrossMediaManager.VLCiOS;

        LibVLCSharp.Shared.MediaPlayer _player;

        public LibVLCSharp.Shared.MediaPlayer Player
        {
            get
            {
                if (_player == null)
                    Initialize();
                return _player;
            }
            set => SetProperty(ref _player, value);
        }

        public VLCiOSMediaPlayer()
        {
        }


        protected virtual void Initialize()
        {
            var libVLC = new LibVLC(enableDebugLogs: true);
            Player = new LibVLCSharp.Shared.MediaPlayer(libVLC);
            //Note: Implement this so that when the user switchs away from your app to an app like "Camera, Or audio recording app, that records media", and iOS interuptions
            //Will be handled safely
            AVAudioSession.Notifications.ObserveInterruption(ToneInterruptionListener);
            Device.BeginInvokeOnMainThread(() =>
            {
                UIApplication.SharedApplication.BeginReceivingRemoteControlEvents();
            });
        }

        //NOte: this method is from iOSMediaPlayer, and normally should just be inherited to create another player
        protected virtual async void ToneInterruptionListener(object sender, AVAudioSessionInterruptionEventArgs interruptArgs)
        {
            switch (interruptArgs.InterruptionType)
            {
                case AVAudioSessionInterruptionType.Began:
                    await MediaManager.Pause();
                    break;
                case AVAudioSessionInterruptionType.Ended:
                    if (interruptArgs.Option == AVAudioSessionInterruptionOptions.ShouldResume)
                    {
                        await MediaManager.Play();
                    }
                    break;
            }
        }

        public override void UpdateVideoAspect(VideoAspectMode videoAspectMode)
        {
            //throw new NotImplementedException();
        }

        public override void UpdateShowPlaybackControls(bool showPlaybackControls)
        {
            //throw new NotImplementedException();
        }

        public override Task Play()
        {
            Player.SetPause(false);
            return Task.CompletedTask;
        }

        public override Task SeekTo(TimeSpan position)
        {
            //throw new NotImplementedException();
            var totalMilliSeconds = position.TotalMilliseconds;
            Player.Time = Convert.ToInt64(totalMilliSeconds);

            return Task.CompletedTask;
        }

        public override Task Stop()
        {
            Player.Stop();
            return Task.CompletedTask;
        }

        protected override void Dispose(bool disposing)
        {
            Player.Dispose();
            base.Dispose();
        }

        public override void UpdateVideoPlaceholder(object value)
        {
            //throw new NotImplementedException();
        }

        public override Task Pause()
        {
            Player.SetPause(true);
            return Task.CompletedTask;
        }

        public override async Task Play(IMediaItem mediaItem)
        {
            InvokeBeforePlaying(this, new MediaPlayerEventArgs(mediaItem, this));
            Device.BeginInvokeOnMainThread(() =>
            {
                var libVLC = new LibVLC(enableDebugLogs: true);
                Player.Play(new Media(libVLC, new Uri(mediaItem.MediaUri)));
            });
            //Player = new LibVLCSharp.Shared.MediaPlayer(new Media);
            InvokeAfterPlaying(this, new MediaPlayerEventArgs(mediaItem, this));
        }

        //private async Task<Stream> GetStreamFromUrl(string url, Dictionary<string, string> headers)
        //{
        //    byte[] data;

        //    using (var client = new System.Net.Http.HttpClient())
        //    {
        //        if (headers != null)
        //        {
        //            foreach (var header in headers)
        //            {
        //                client.DefaultRequestHeaders.Add(header.Key, header.Value);
        //            }
        //        }

        //        data = await client.GetByteArrayAsync(url);
        //    }

        //    return new MemoryStream(data);
        //}

        public override Task Play(IMediaItem mediaItem, TimeSpan startAt, TimeSpan? stopAt = null)
        {
            //TODO: Play with Player's "Position" and "Time" and "Length"
            //This percentage is between 0 and 1
            //var currentPositionPercentage = e.Position;
            //var percentageTime = new decimal((currentPositionPercentage / 1)) * new decimal(Player.Length);
            //var timeSpanReached = TimeSpan.FromMilliseconds(Convert.ToInt64(percentageTime));
            throw new NotImplementedException();
        }
    }
}
