// Stub types for UnityEngine — only what VRCUploadNotifier actually references.
// These are never shipped; they exist solely to satisfy the compiler at build time.

namespace UnityEngine
{
    public enum RuntimePlatform
    {
        OSXEditor,
        WindowsEditor,
        LinuxEditor
    }

    public static class Application
    {
        public static RuntimePlatform platform => default;
    }

    public static class Debug
    {
        public static void Log(object message) { }
        public static void LogWarning(object message) { }
    }

    public class GUIContent
    {
        public GUIContent(string text, string tooltip) { }
    }

    public class GUIStyle { }

    public class GUILayoutOption { }

    public class GUILayout
    {
        public static bool Button(string text, params GUILayoutOption[] options) => false;
        public static void Space(float pixels) { }
        public static GUILayoutOption Height(float height) => null;
    }
}
