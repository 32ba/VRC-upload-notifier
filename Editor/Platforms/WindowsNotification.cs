using System.Diagnostics;
using System.Text;
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

            var escapedTitle = System.Security.SecurityElement.Escape(title);
            var escapedMessage = System.Security.SecurityElement.Escape(message);

            var audioXml = playSound
                ? "<audio src='ms-winsoundevent:Notification.Default'/>"
                : "<audio silent='true'/>";

            var xmlStr = "<toast><visual><binding template='ToastText02'>"
                + $"<text id='1'>{escapedTitle}</text>"
                + $"<text id='2'>{escapedMessage}</text>"
                + $"</binding></visual>{audioXml}</toast>";

            // Escape single quotes for PowerShell single-quoted string
            var psXml = xmlStr.Replace("'", "''");

            var script =
                "[Windows.UI.Notifications.ToastNotificationManager, Windows.UI.Notifications, ContentType = WindowsRuntime] | Out-Null\n"
                + "[Windows.Data.Xml.Dom.XmlDocument, Windows.Data.Xml.Dom.XmlDocument, ContentType = WindowsRuntime] | Out-Null\n"
                + "$xml = New-Object Windows.Data.Xml.Dom.XmlDocument\n"
                + $"$xml.LoadXml('{psXml}')\n"
                + "$toast = [Windows.UI.Notifications.ToastNotification]::new($xml)\n"
                + "[Windows.UI.Notifications.ToastNotificationManager]::CreateToastNotifier('Unity').Show($toast)\n";

            // Use -EncodedCommand (Base64) to avoid all escaping issues
            var encoded = System.Convert.ToBase64String(Encoding.Unicode.GetBytes(script));

            var processInfo = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-NoProfile -ExecutionPolicy Bypass -EncodedCommand {encoded}",
                UseShellExecute = false,
                CreateNoWindow = true
            };

            try
            {
                Process.Start(processInfo);
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogWarning($"[VRCUploadNotifier] Failed to send Windows notification: {ex.Message}");
            }
        }
    }
}
