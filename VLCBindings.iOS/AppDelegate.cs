using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using LibVLCSharp.Forms.Shared;
using MediaManager.Library;
using UIKit;
using Xamarin.Forms;

namespace VLCBindings.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            LibVLCSharpFormsRenderer.Init();
            VLCCrossMediaManager.Current.Init();
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App(VLCCrossMediaManager.Current));

            //VLCCrossMediaManager.Current.PositionChanged += Current_PositionChanged;
            ////play
            //MessagingCenter.Instance.Subscribe<object, bool>(this, "PlayerMessage", (s, v) =>
            //{
            //    Device.BeginInvokeOnMainThread(() =>
            //    {
            //        if (v)
            //            VLCCrossMediaManager.Current.Play(new MediaItem("http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ElephantsDream.mp4"));
            //        else
            //            VLCCrossMediaManager.Current.Pause();
            //    });
            //});

            return base.FinishedLaunching(app, options);
        }
    }
}
