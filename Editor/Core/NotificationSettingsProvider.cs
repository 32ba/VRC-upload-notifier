using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace VRCUploadNotifier.Core
{
    /// <summary>
    /// Settings provider for Project Settings window.
    /// Accessible via Edit > Project Settings > VRChat Upload Notifier.
    /// </summary>
    public class NotificationSettingsProvider : SettingsProvider
    {
        private const string SettingsPath = "Project/VRChat Upload Notifier";

        private NotificationSettingsProvider(string path, SettingsScope scope)
            : base(path, scope)
        {
        }

        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            return new NotificationSettingsProvider(SettingsPath, SettingsScope.Project)
            {
                label = "VRChat Upload Notifier",
                keywords = new HashSet<string>(new[]
                {
                    "VRChat", "Upload", "Notification", "Notifier", "SDK", "Avatar", "World"
                })
            };
        }

        public override void OnGUI(string searchContext)
        {
            var settings = NotificationSettings.Instance;

            EditorGUILayout.Space(10);

            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField("General", EditorStyles.boldLabel);
                EditorGUILayout.Space(5);

                settings.Enabled = EditorGUILayout.Toggle(
                    new GUIContent("Enable Notifications", "Master toggle for all notifications"),
                    settings.Enabled);

                using (new EditorGUI.DisabledGroupScope(!settings.Enabled))
                {
                    settings.PlaySound = EditorGUILayout.Toggle(
                        new GUIContent("Play Sound", "Play a sound when notifications are shown"),
                        settings.PlaySound);
                }
            }

            EditorGUILayout.Space(10);

            using (new EditorGUI.DisabledGroupScope(!settings.Enabled))
            {
                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    EditorGUILayout.LabelField("Notification Types", EditorStyles.boldLabel);
                    EditorGUILayout.Space(5);

                    settings.NotifyOnSuccess = EditorGUILayout.Toggle(
                        new GUIContent("Notify on Success", "Show notification when upload completes successfully"),
                        settings.NotifyOnSuccess);

                    settings.NotifyOnError = EditorGUILayout.Toggle(
                        new GUIContent("Notify on Error", "Show notification when upload fails"),
                        settings.NotifyOnError);
                }

                EditorGUILayout.Space(10);

                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    EditorGUILayout.LabelField("Content", EditorStyles.boldLabel);
                    EditorGUILayout.Space(5);

                    settings.ShowContentId = EditorGUILayout.Toggle(
                        new GUIContent("Show Content ID", "Include the VRChat Content ID in success notifications"),
                        settings.ShowContentId);
                }
            }

            EditorGUILayout.Space(20);

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Send Test Notification", GUILayout.Height(30)))
                {
                    UploadNotificationManager.SendTestNotification();
                }

                GUILayout.Space(10);

                if (GUILayout.Button("Reset to Defaults", GUILayout.Height(30)))
                {
                    if (EditorUtility.DisplayDialog(
                        "Reset Settings",
                        "Are you sure you want to reset all settings to their default values?",
                        "Reset",
                        "Cancel"))
                    {
                        settings.ResetToDefaults();
                    }
                }
            }

            EditorGUILayout.Space(20);

            DrawStatusInfo();
        }

        private void DrawStatusInfo()
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField("Status", EditorStyles.boldLabel);
                EditorGUILayout.Space(5);

                var platform = Application.platform.ToString();
                EditorGUILayout.LabelField("Platform:", platform);

                var notificationMethod = GetNotificationMethod();
                EditorGUILayout.LabelField("Notification Method:", notificationMethod);

#if VRC_SDK_VRCSDK3
                EditorGUILayout.LabelField("VRChat SDK:", "Detected ✓");
#else
                EditorGUILayout.HelpBox(
                    "VRChat SDK not detected. Install VRChat SDK 3.0+ to enable upload notifications.",
                    MessageType.Warning);
#endif
            }
        }

        private string GetNotificationMethod()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.OSXEditor:
                    return "macOS Notification Center (osascript)";
                case RuntimePlatform.WindowsEditor:
                    return "Windows Toast (PowerShell)";
                case RuntimePlatform.LinuxEditor:
                    return "notify-send (libnotify)";
                default:
                    return "Not supported";
            }
        }
    }
}
