using UnityEditor;

namespace VRCUploadNotifier.Core
{
    /// <summary>
    /// Settings data for VRChat Upload Notifier.
    /// Stored in EditorPrefs for persistence across Unity sessions.
    /// </summary>
    public class NotificationSettings
    {
        private const string PrefsPrefix = "VRCUploadNotifier.";

        private const string EnabledKey = PrefsPrefix + "Enabled";
        private const string NotifyOnSuccessKey = PrefsPrefix + "NotifyOnSuccess";
        private const string NotifyOnErrorKey = PrefsPrefix + "NotifyOnError";
        private const string PlaySoundKey = PrefsPrefix + "PlaySound";
        private const string ShowContentIdKey = PrefsPrefix + "ShowContentId";

        private static NotificationSettings _instance;

        public static NotificationSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new NotificationSettings();
                }
                return _instance;
            }
        }

        /// <summary>
        /// Master toggle for all notifications.
        /// </summary>
        public bool Enabled
        {
            get => EditorPrefs.GetBool(EnabledKey, true);
            set => EditorPrefs.SetBool(EnabledKey, value);
        }

        /// <summary>
        /// Whether to notify on successful uploads.
        /// </summary>
        public bool NotifyOnSuccess
        {
            get => EditorPrefs.GetBool(NotifyOnSuccessKey, true);
            set => EditorPrefs.SetBool(NotifyOnSuccessKey, value);
        }

        /// <summary>
        /// Whether to notify on failed uploads.
        /// </summary>
        public bool NotifyOnError
        {
            get => EditorPrefs.GetBool(NotifyOnErrorKey, true);
            set => EditorPrefs.SetBool(NotifyOnErrorKey, value);
        }

        /// <summary>
        /// Whether to play a sound with notifications.
        /// </summary>
        public bool PlaySound
        {
            get => EditorPrefs.GetBool(PlaySoundKey, true);
            set => EditorPrefs.SetBool(PlaySoundKey, value);
        }

        /// <summary>
        /// Whether to include the Content ID in success notifications.
        /// </summary>
        public bool ShowContentId
        {
            get => EditorPrefs.GetBool(ShowContentIdKey, true);
            set => EditorPrefs.SetBool(ShowContentIdKey, value);
        }

        /// <summary>
        /// Resets all settings to their default values.
        /// </summary>
        public void ResetToDefaults()
        {
            Enabled = true;
            NotifyOnSuccess = true;
            NotifyOnError = true;
            PlaySound = true;
            ShowContentId = true;
        }
    }
}
