namespace VRCUploadNotifier.Platforms
{
    /// <summary>
    /// Interface for platform-specific native notification implementations.
    /// </summary>
    public interface INativeNotification
    {
        /// <summary>
        /// Gets whether this notification provider is supported on the current platform.
        /// </summary>
        bool IsSupported { get; }

        /// <summary>
        /// Sends a native notification.
        /// </summary>
        /// <param name="title">The notification title.</param>
        /// <param name="message">The notification message body.</param>
        /// <param name="playSound">Whether to play a sound with the notification.</param>
        void Send(string title, string message, bool playSound = true);
    }
}
