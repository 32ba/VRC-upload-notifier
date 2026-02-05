using UnityEngine;
using VRCUploadNotifier.Platforms;

namespace VRCUploadNotifier.Core
{
    /// <summary>
    /// Facade for managing upload notifications across platforms.
    /// Handles platform detection and notification dispatch.
    /// </summary>
    public static class UploadNotificationManager
    {
        private static INativeNotification _notificationProvider;

        private static INativeNotification NotificationProvider
        {
            get
            {
                if (_notificationProvider == null)
                {
                    _notificationProvider = CreateNotificationProvider();
                }
                return _notificationProvider;
            }
        }

        private static INativeNotification CreateNotificationProvider()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.OSXEditor:
                    return new MacOSNotification();
                case RuntimePlatform.WindowsEditor:
                    return new WindowsNotification();
                case RuntimePlatform.LinuxEditor:
                    return new LinuxNotification();
                default:
                    Debug.LogWarning($"[VRCUploadNotifier] Unsupported platform: {Application.platform}");
                    return new NullNotification();
            }
        }

        /// <summary>
        /// Notifies that an upload completed successfully.
        /// </summary>
        /// <param name="contentType">The type of content (Avatar/World).</param>
        /// <param name="contentId">The VRChat content ID.</param>
        public static void NotifyUploadSuccess(string contentType, string contentId)
        {
            var settings = NotificationSettings.Instance;

            if (!settings.Enabled || !settings.NotifyOnSuccess)
            {
                return;
            }

            var title = $"VRChat {contentType} Upload Complete";
            var message = settings.ShowContentId && !string.IsNullOrEmpty(contentId)
                ? $"Upload successful!\nID: {contentId}"
                : "Upload successful!";

            Debug.Log($"[VRCUploadNotifier] {title}: {message.Replace("\n", " ")}");

            if (NotificationProvider.IsSupported)
            {
                NotificationProvider.Send(title, message, settings.PlaySound);
            }
        }

        /// <summary>
        /// Notifies that an upload failed.
        /// </summary>
        /// <param name="contentType">The type of content (Avatar/World).</param>
        /// <param name="errorMessage">The error message.</param>
        public static void NotifyUploadError(string contentType, string errorMessage)
        {
            var settings = NotificationSettings.Instance;

            if (!settings.Enabled || !settings.NotifyOnError)
            {
                return;
            }

            var title = $"VRChat {contentType} Upload Failed";
            var message = !string.IsNullOrEmpty(errorMessage)
                ? $"Error: {TruncateMessage(errorMessage, 100)}"
                : "Upload failed. Check the console for details.";

            Debug.LogWarning($"[VRCUploadNotifier] {title}: {message.Replace("\n", " ")}");

            if (NotificationProvider.IsSupported)
            {
                NotificationProvider.Send(title, message, settings.PlaySound);
            }
        }

        /// <summary>
        /// Sends a test notification to verify the system is working.
        /// </summary>
        public static void SendTestNotification()
        {
            var settings = NotificationSettings.Instance;
            var title = "VRChat Upload Notifier";
            var message = "Test notification - everything is working!";

            Debug.Log($"[VRCUploadNotifier] Sending test notification...");

            if (NotificationProvider.IsSupported)
            {
                NotificationProvider.Send(title, message, settings.PlaySound);
                Debug.Log($"[VRCUploadNotifier] Test notification sent.");
            }
            else
            {
                Debug.LogWarning($"[VRCUploadNotifier] Notifications are not supported on this platform: {Application.platform}");
            }
        }

        private static string TruncateMessage(string message, int maxLength)
        {
            if (string.IsNullOrEmpty(message) || message.Length <= maxLength)
            {
                return message;
            }
            return message.Substring(0, maxLength - 3) + "...";
        }

        /// <summary>
        /// Null implementation for unsupported platforms.
        /// </summary>
        private class NullNotification : INativeNotification
        {
            public bool IsSupported => false;
            public void Send(string title, string message, bool playSound = true) { }
        }
    }
}
