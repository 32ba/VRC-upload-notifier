using System.Diagnostics;
using UnityEngine;

namespace VRCUploadNotifier.Platforms
{
    /// <summary>
    /// Linux native notification implementation using notify-send (libnotify).
    /// Works with most desktop environments (GNOME, KDE, XFCE, etc.).
    /// </summary>
    public class LinuxNotification : INativeNotification
    {
        public bool IsSupported => Application.platform == RuntimePlatform.LinuxEditor;

        public void Send(string title, string message, bool playSound = true)
        {
            if (!IsSupported)
            {
                return;
            }

            var escapedTitle = EscapeShellString(title);
            var escapedMessage = EscapeShellString(message);

            var processInfo = new ProcessStartInfo
            {
                FileName = "notify-send",
                Arguments = $"\"{escapedTitle}\" \"{escapedMessage}\" --urgency=normal --app-name=Unity",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            try
            {
                using (var process = Process.Start(processInfo))
                {
                    process?.WaitForExit(5000);
                }
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogWarning($"[VRCUploadNotifier] Failed to send Linux notification: {ex.Message}");
            }
        }

        private static string EscapeShellString(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            return input
                .Replace("\\", "\\\\")
                .Replace("\"", "\\\"")
                .Replace("$", "\\$")
                .Replace("`", "\\`")
                .Replace("\n", " ")
                .Replace("\r", "");
        }
    }
}
