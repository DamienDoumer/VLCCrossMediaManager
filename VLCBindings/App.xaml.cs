using System;
using MediaManager;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VLCBindings
{
    public partial class App : Application
    {
        private IMediaManager _mediaManager;

        public App(IMediaManager mediaManager)
        {
            InitializeComponent();

            _mediaManager = mediaManager;
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
