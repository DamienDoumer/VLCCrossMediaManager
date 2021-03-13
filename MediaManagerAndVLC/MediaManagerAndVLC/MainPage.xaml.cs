using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaManager;
using MediaManager.Library;
using MediaManager.Playback;
using Xamarin.Forms;

namespace MediaManagerAndVLC
{
    public partial class MainPage : ContentPage
    {
        private IMediaManager _mediaManager;
        private List<string> _mediaItems = new List<string>
        {
            "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ElephantsDream.mp4"
        };
        double GetProgressPercentFromCurrentMedia => ((_mediaManager.Position.TotalSeconds / _mediaManager.Duration.TotalSeconds) * 100);

        public MainPage(IMediaManager mediaManager)
        {
            _mediaManager = mediaManager;
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            _mediaManager.PositionChanged += _mediaManager_PositionChanged; ;
            _mediaManager.MediaItemFailed += _mediaManager_MediaItemFailed; ;
            _mediaManager.MediaItemFinished += _mediaManager_MediaItemFinished;
            _mediaManager.MediaItemChanged += _mediaManager_MediaItemChanged;
            _mediaManager.Play(_mediaItems);
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            _mediaManager.PositionChanged -= _mediaManager_PositionChanged; ;
            _mediaManager.MediaItemFailed -= _mediaManager_MediaItemFailed; ;
            _mediaManager.MediaItemFinished -= _mediaManager_MediaItemFinished; ;
            _mediaManager.MediaItemChanged -= _mediaManager_MediaItemChanged; ;
            base.OnDisappearing();
        }

        private void _mediaManager_MediaItemChanged(object sender, MediaManager.Media.MediaItemEventArgs e)
        {

        }

        private void _mediaManager_MediaItemFinished(object sender, MediaManager.Media.MediaItemEventArgs e)
        {
        }

        private void _mediaManager_MediaItemFailed(object sender, MediaManager.Media.MediaItemFailedEventArgs e)
        {
        }

        private void _mediaManager_PositionChanged(object sender, MediaManager.Playback.PositionChangedEventArgs e)
        {
            DurationLabel.Text = _mediaManager.Duration.ToString(@"mm\:ss");
            TotalPlayedLabel.Text = _mediaManager.Position.ToString(@"mm\:ss");
            SeekSlider.Value = GetProgressPercentFromCurrentMedia;
        }

        private async void SeekSlider_DragCompleted(object sender, EventArgs e)
        {
            var position = (SeekSlider.Value / 100) * _mediaManager.Duration.TotalSeconds;
            var currentPosition = TimeSpan.FromSeconds(position);
            await _mediaManager.SeekTo(currentPosition);
        }

        private void PlayListButton_Clicked(object sender, EventArgs e)
        {

        }

        private void MuteButton_Clicked(object sender, EventArgs e)
        {

        }

        private void PreviousTrackButton_Clicked(object sender, EventArgs e)
        {

        }

        private void MoveBackButton_Clicked(object sender, EventArgs e)
        {

        }

        private void PlayPauseButton_Clicked(object sender, EventArgs e)
        {
            if (_mediaManager.IsPlaying())
            {
                _mediaManager.Pause();
                PlayPauseButton.Text = "Play";
            }
            else
            {
                _mediaManager.Play();
                PlayPauseButton.Text = "Pause";
            }
        }

        private void MoveForwardButton_Clicked(object sender, EventArgs e)
        {

        }

        private void NextTrackButton_Clicked(object sender, EventArgs e)
        {

        }
    }
}
