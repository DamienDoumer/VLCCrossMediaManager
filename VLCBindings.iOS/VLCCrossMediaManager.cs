using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using MediaManager;
using UIKit;

namespace VLCBindings.iOS
{
    public static class VLCCrossMediaManager
    {
        private static Lazy<IMediaManager> implementation = new Lazy<IMediaManager>(() => CreateMediaManager(), System.Threading.LazyThreadSafetyMode.PublicationOnly);
        public static VLCiOSMediaManagerImplementation VLCiOS => (VLCiOSMediaManagerImplementation)Current;

        public static IMediaManager Current
        {
            get
            {
                var ret = implementation.Value;
                if (ret == null)
                {
                    throw new NotImplementedException();
                }
                return ret;
            }
        }


        private static IMediaManager CreateMediaManager()
        {
            return new VLCiOSMediaManagerImplementation();
        }
    }
}
