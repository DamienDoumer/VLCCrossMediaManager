using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaManager;
using MediaManager.Library;
using Xamarin.Forms;

namespace VLCBindings
{
    public partial class MainPage : ContentPage
    {
        private IMediaManager _mediaManager;
        public MainPage(IMediaManager mediaManager)
        {
            _mediaManager = mediaManager;
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            //play
            _mediaManager.Pause();
            //   MessagingCenter.Instance.Send<object, bool>(this, "PlayerMessage", false);
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            //play
            _mediaManager.Play(new MediaItem("http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ElephantsDream.mp4"));
            //MessagingCenter.Instance.Send<object, bool>(this, "PlayerMessage", true);
        }
    }
}
