using MediaManager.Volume;

namespace MediaManagerAndVLC.iOS.VLCMediaManager
{
    public class VLCVolumeManager : VolumeManagerBase, IVolumeManager
    {
        protected VLCiOSMediaManagerImplementation MediaManager => VLCCrossMediaManager.VLCiOS;
        public LibVLCSharp.Shared.MediaPlayer Player => MediaManager?.Player;

        public override event VolumeChangedEventHandler VolumeChanged;
        public override int CurrentVolume { get; set; }
        public override int MaxVolume { get; set; }
        public override float Balance { get; set; }
        public override bool Muted
        {
            get => Player?.Mute ?? false;
            set
            {
                Player.Mute = value;
                int.TryParse((Player.Volume * 100).ToString(), out var vol);
                VolumeChanged?.Invoke(this, new VolumeChangedEventArgs(vol, value));
            }
        }
    }
}
