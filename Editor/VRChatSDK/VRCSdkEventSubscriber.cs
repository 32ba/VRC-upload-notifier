#if VRC_SDK_VRCSDK3
using System;
using UnityEditor;
using VRC.SDKBase.Editor.Api;
using VRCUploadNotifier.Core;

namespace VRCUploadNotifier.VRChatSDK
{
    /// <summary>
    /// Subscribes to VRChat SDK upload events and triggers notifications.
    /// Automatically initializes when the Unity Editor loads.
    /// </summary>
    [InitializeOnLoad]
    public static class VRCSdkEventSubscriber
    {
        private static bool _isInitialized;
        private static int _retryCount;
        private const int MaxRetries = 10;

        static VRCSdkEventSubscriber()
        {
            // Delay initialization to allow SDK to initialize first
            EditorApplication.delayCall += TryInitialize;
        }

        private static void TryInitialize()
        {
            if (_isInitialized)
            {
                return;
            }

            var avatarSubscribed = TrySubscribeAvatarBuilder();
            var worldSubscribed = TrySubscribeWorldBuilder();

            if (avatarSubscribed || worldSubscribed)
            {
                _isInitialized = true;
                UnityEngine.Debug.Log("[VRCUploadNotifier] Successfully subscribed to VRChat SDK events.");
            }
            else
            {
                _retryCount++;
                if (_retryCount < MaxRetries)
                {
                    // Retry later - SDK Panel might not be initialized yet
                    EditorApplication.delayCall += TryInitialize;
                }
            }
        }

        private static bool TrySubscribeAvatarBuilder()
        {
            try
            {
                if (!VRCSdkControlPanel.TryGetBuilder<IVRCSdkAvatarBuilderApi>(out var avatarBuilder))
                {
                    return false;
                }

                // Unsubscribe first to prevent duplicate handlers
                avatarBuilder.OnSdkUploadSuccess -= OnAvatarUploadSuccess;
                avatarBuilder.OnSdkUploadError -= OnAvatarUploadError;

                // Subscribe to events
                avatarBuilder.OnSdkUploadSuccess += OnAvatarUploadSuccess;
                avatarBuilder.OnSdkUploadError += OnAvatarUploadError;

                return true;
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogWarning($"[VRCUploadNotifier] Failed to subscribe to Avatar builder: {ex.Message}");
                return false;
            }
        }

        private static bool TrySubscribeWorldBuilder()
        {
            try
            {
                if (!VRCSdkControlPanel.TryGetBuilder<IVRCSdkWorldBuilderApi>(out var worldBuilder))
                {
                    return false;
                }

                // Unsubscribe first to prevent duplicate handlers
                worldBuilder.OnSdkUploadSuccess -= OnWorldUploadSuccess;
                worldBuilder.OnSdkUploadError -= OnWorldUploadError;

                // Subscribe to events
                worldBuilder.OnSdkUploadSuccess += OnWorldUploadSuccess;
                worldBuilder.OnSdkUploadError += OnWorldUploadError;

                return true;
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogWarning($"[VRCUploadNotifier] Failed to subscribe to World builder: {ex.Message}");
                return false;
            }
        }

        private static void OnAvatarUploadSuccess(object sender, string contentId)
        {
            UploadNotificationManager.NotifyUploadSuccess("Avatar", contentId);
        }

        private static void OnAvatarUploadError(object sender, string errorMessage)
        {
            UploadNotificationManager.NotifyUploadError("Avatar", errorMessage);
        }

        private static void OnWorldUploadSuccess(object sender, string contentId)
        {
            UploadNotificationManager.NotifyUploadSuccess("World", contentId);
        }

        private static void OnWorldUploadError(object sender, string errorMessage)
        {
            UploadNotificationManager.NotifyUploadError("World", errorMessage);
        }

        /// <summary>
        /// Manually reinitialize the event subscriptions.
        /// Call this if the SDK Control Panel was opened after initial load.
        /// </summary>
        [MenuItem("Tools/VRChat Upload Notifier/Reinitialize Event Subscription")]
        public static void Reinitialize()
        {
            _isInitialized = false;
            _retryCount = 0;
            TryInitialize();

            if (_isInitialized)
            {
                UnityEngine.Debug.Log("[VRCUploadNotifier] Event subscriptions reinitialized successfully.");
            }
            else
            {
                UnityEngine.Debug.LogWarning("[VRCUploadNotifier] Failed to reinitialize. Make sure the VRChat SDK Control Panel has been opened at least once.");
            }
        }
    }
}
#endif
