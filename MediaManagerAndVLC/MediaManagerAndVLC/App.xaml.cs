using System;
using MediaManager;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MediaManagerAndVLC
{
    public partial class App : Application
    {
        private IMediaManager _mediaManager;

        public App(IMediaManager mediaManager)
        {
            _mediaManager = mediaManager;
            InitializeComponent();

            MainPage = new MainPage(_mediaManager);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
