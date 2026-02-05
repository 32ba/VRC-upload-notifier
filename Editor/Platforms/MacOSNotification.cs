using System.Diagnostics;
using UnityEngine;

namespace VRCUploadNotifier.Platforms
{
    /// <summary>
    /// macOS native notification implementation using osascript (AppleScript).
    /// </summary>
    public class MacOSNotification : INativeNotification
    {
        public bool IsSupported => Application.platform == RuntimePlatform.OSXEditor;

        public void Send(string title, string message, bool playSound = true)
        {
            if (!IsSupported)
            {
                return;
            }

            var escapedTitle = EscapeAppleScriptString(title);
            var escapedMessage = EscapeAppleScriptString(message);

            var soundClause = playSound ? " sound name \"Glass\"" : "";
            var script = $"display notification \"{escapedMessage}\" with title \"{escapedTitle}\"{soundClause}";

            var processInfo = new ProcessStartInfo
            {
                FileName = "osascript",
                Arguments = $"-e '{script}'",
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
                UnityEngine.Debug.LogWarning($"[VRCUploadNotifier] Failed to send macOS notification: {ex.Message}");
            }
        }

        private static string EscapeAppleScriptString(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            return input
                .Replace("\\", "\\\\")
                .Replace("\"", "\\\"")
                .Replace("\n", "\\n")
                .Replace("\r", "");
        }
    }
}
