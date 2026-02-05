using System.Diagnostics;
using UnityEngine;

namespace VRCUploadNotifier.Platforms
{
    /// <summary>
    /// Windows native notification implementation using PowerShell Toast notifications.
    /// Requires Windows 10 or later (which is required by VRChat anyway).
    /// </summary>
    public class WindowsNotification : INativeNotification
    {
        public bool IsSupported => Application.platform == RuntimePlatform.WindowsEditor;

        public void Send(string title, string message, bool playSound = true)
        {
            if (!IsSupported)
            {
                return;
            }

            var escapedTitle = EscapePowerShellString(title);
            var escapedMessage = EscapePowerShellString(message);

            var audioXml = playSound
                ? "<audio src=\"ms-winsoundevent:Notification.Default\"/>"
                : "<audio silent=\"true\"/>";

            var toastXml = $@"
<toast>
    <visual>
        <binding template=""ToastText02"">
            <text id=""1"">{System.Security.SecurityElement.Escape(title)}</text>
            <text id=""2"">{System.Security.SecurityElement.Escape(message)}</text>
        </binding>
    </visual>
    {audioXml}
</toast>";

            var script = $@"
[Windows.UI.Notifications.ToastNotificationManager, Windows.UI.Notifications, ContentType = WindowsRuntime] | Out-Null
[Windows.Data.Xml.Dom.XmlDocument, Windows.Data.Xml.Dom.XmlDocument, ContentType = WindowsRuntime] | Out-Null
$xml = New-Object Windows.Data.Xml.Dom.XmlDocument
$xml.LoadXml(@'{toastXml.Replace("'", "''")}'@)
$toast = [Windows.UI.Notifications.ToastNotification]::new($xml)
[Windows.UI.Notifications.ToastNotificationManager]::CreateToastNotifier('Unity').Show($toast)
";

            var processInfo = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{script.Replace("\"", "\\\"")}\"",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            try
            {
                var process = Process.Start(processInfo);
                // Don't wait for PowerShell to complete - let it run asynchronously
                // to avoid blocking the Unity main thread
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogWarning($"[VRCUploadNotifier] Failed to send Windows notification: {ex.Message}");
            }
        }

        private static string EscapePowerShellString(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            return input
                .Replace("`", "``")
                .Replace("\"", "`\"")
                .Replace("$", "`$")
                .Replace("\n", "`n")
                .Replace("\r", "");
        }
    }
}
