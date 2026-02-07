// Stub types for VRChat SDK — only what VRCUploadNotifier actually references.

using System;

// VRCSdkControlPanel lives in the global namespace in the actual VRC SDK.
public class VRCSdkControlPanel
{
    public static bool TryGetBuilder<T>(out T builder) where T : class
    {
        builder = default;
        return false;
    }
}

namespace VRC.SDKBase.Editor.Api
{
    public interface IVRCSdkAvatarBuilderApi
    {
        event EventHandler<string> OnSdkUploadSuccess;
        event EventHandler<string> OnSdkUploadError;
    }

    public interface IVRCSdkWorldBuilderApi
    {
        event EventHandler<string> OnSdkUploadSuccess;
        event EventHandler<string> OnSdkUploadError;
    }
}
