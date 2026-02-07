// Stub types for UnityEditor — only what VRCUploadNotifier actually references.

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
    public static class EditorPrefs
    {
        public static bool GetBool(string key, bool defaultValue) => defaultValue;
        public static void SetBool(string key, bool value) { }
    }

    public enum SettingsScope
    {
        User = 0,
        Project = 1
    }

    public class SettingsProvider
    {
        public SettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) { }
        public string label { get; set; }
        public IEnumerable<string> keywords { get; set; }
        public virtual void OnGUI(string searchContext) { }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class SettingsProviderAttribute : Attribute { }

    public static class EditorGUILayout
    {
        public static void Space(float width = 0) { }
        public static void LabelField(string label, GUIStyle style) { }
        public static void LabelField(string label, string label2) { }
        public static bool Toggle(GUIContent label, bool value) => value;
        public static void HelpBox(string message, MessageType type) { }

        public class VerticalScope : IDisposable
        {
            public VerticalScope(GUIStyle style, params GUILayoutOption[] options) { }
            public void Dispose() { }
        }

        public class HorizontalScope : IDisposable
        {
            public HorizontalScope(params GUILayoutOption[] options) { }
            public void Dispose() { }
        }
    }

    public static class EditorStyles
    {
        public static GUIStyle helpBox => null;
        public static GUIStyle boldLabel => null;
    }

    public static class EditorGUI
    {
        public class DisabledGroupScope : IDisposable
        {
            public DisabledGroupScope(bool disabled) { }
            public void Dispose() { }
        }
    }

    public static class EditorUtility
    {
        public static bool DisplayDialog(string title, string message, string ok, string cancel) => false;
    }

    public enum MessageType
    {
        Warning
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class InitializeOnLoadAttribute : Attribute { }

    public static class EditorApplication
    {
        public delegate void CallbackFunction();
        public static CallbackFunction delayCall;
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class MenuItem : Attribute
    {
        public MenuItem(string itemName) { }
    }
}
