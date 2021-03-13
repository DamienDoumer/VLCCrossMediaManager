using System;
using MediaManager;

namespace MediaManagerAndVLC.iOS.VLCMediaManager
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
