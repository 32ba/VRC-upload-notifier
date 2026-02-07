// Stub types for VRC.SDKBase.Editor assembly (from com.vrchat.base asmdef).

using System;

// Global namespace — matches real VRC SDK
public interface IVRCSdkControlPanelBuilder
{
    void RegisterBuilder(object panel);
}

public partial class VRCSdkControlPanel
{
    public static bool TryGetBuilder<T>(out T builder) where T : VRC.SDKBase.Editor.IVRCSdkBuilderApi
    {
        builder = default;
        return false;
    }
}

namespace VRC.SDKBase.Editor
{
    public interface IVRCSdkBuilderApi : IVRCSdkControlPanelBuilder
    {
        event EventHandler<object> OnSdkBuildStart;
        event EventHandler<string> OnSdkBuildProgress;
        event EventHandler<string> OnSdkBuildFinish;
        event EventHandler<string> OnSdkBuildSuccess;
        event EventHandler<string> OnSdkBuildError;
        event EventHandler OnSdkUploadStart;
        event EventHandler<(string status, float percentage)> OnSdkUploadProgress;
        event EventHandler<string> OnSdkUploadFinish;
        event EventHandler<string> OnSdkUploadSuccess;
        event EventHandler<string> OnSdkUploadError;
        void CancelUpload();
    }
}
