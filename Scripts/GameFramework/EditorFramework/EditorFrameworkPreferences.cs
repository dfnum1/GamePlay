/********************************************************************
生成日期:	06:30:2025
类    名: 	EditorFrameworkPreferences
作    者:	HappLI
描    述:	编辑器偏好设置
*********************************************************************/
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using Framework.ED;
using Framework.DrawProps;
using System.Reflection;

namespace Framework.ED
{
    public class EditorFrameworkPreferences
    {
        /// <summary> The last key we checked. This should be the one we modify </summary>
        private static string lastKey => GetProjectKeyPrefix() + "Settings";
        private static string GetProjectKeyPrefix()
        {
            // 用项目路径的 hash 作为前缀，保证唯一性
            string projectPath = Application.dataPath; // 例如 D:/MyProject/Assets
            int hash = projectPath.GetHashCode();
            return $"FrameworkSetting.{hash}.";
        }
        private static Dictionary<Type, Color> typeColors = new Dictionary<Type, Color>();
        private static Dictionary<string, Settings> settings = new Dictionary<string, Settings>();

        [System.Serializable]
        public class Settings : ISerializationCallbackReceiver
        {
            [Display("代码自动化")]public bool autoCodeGen = true;
            [Display("二进制序列化代码生成路径")] public string binaryGeneratedPatch = "";
            [Display("配置表c#代码生成路径")] public string tableCsGeneratedPatch = "";
            [Display("配置表c++代码生成路径")] public string tableCppGeneratedPatch = "";
            [Display("配置表c#服务器代码生成路径")] public string tableCsServerGeneratedPatch = "";

            public void OnAfterDeserialize()
            {
            }

            public void OnBeforeSerialize()
            {
            }
        }

        /// <summary> Get settings of current active editor </summary>
        public static Settings GetSettings()
        {
            if (!settings.ContainsKey(lastKey)) VerifyLoaded();
            return settings[lastKey];
        }

//#if UNITY_2019_1_OR_NEWER
        [SettingsProvider]
        public static SettingsProvider CreateActorSystemSettingsProvider() {
            SettingsProvider provider = new SettingsProvider("Preferences/GamePlay", SettingsScope.User) {
                guiHandler = (searchContext) => { PreferencesGUI(); },
            };
            return provider;
        }
//#endif

//#if !UNITY_2019_1_OR_NEWER
//        [PreferenceItem("AT Editor")]
//#endif
        public static void PreferencesGUI()
        {
            VerifyLoaded();
            Settings settings = EditorFrameworkPreferences.settings[lastKey];
            SystemSettingsGUI(lastKey, settings);
            if (GUILayout.Button(new GUIContent("Set Default", "Reset all values to default"), GUILayout.Width(120)))
            {
                ResetPrefs();
            }
        }

        public static void OpenUserPreferences()
        {
            SettingsService.OpenUserPreferences("Preferences/GamePlay");
        }

        /// <summary> Load prefs if they exist. Create if they don't </summary>
        private static Settings LoadPrefs()
        {
            // Create settings if it doesn't exist
            if (!EditorPrefs.HasKey(lastKey))
            {
                EditorPrefs.SetString(lastKey, JsonUtility.ToJson(new Settings()));
            }
            return JsonUtility.FromJson<Settings>(EditorPrefs.GetString(lastKey));
        }

        /// <summary> Delete all prefs </summary>
        public static void ResetPrefs()
        {
            if (EditorPrefs.HasKey(lastKey)) EditorPrefs.DeleteKey(lastKey);
            if (settings.ContainsKey(lastKey)) settings.Remove(lastKey);
            typeColors = new Dictionary<Type, Color>();
            VerifyLoaded();

            RepaintAll();
        }

        static void RepaintAll()
        {
            /*
            var editors = Resources.FindObjectsOfTypeAll<ACutsceneEditor>();
            if (editors != null)
            {
                for (int i = 0; i < editors.Length; ++i)
                    editors[i].Repaint();
            }*/
        }

        /// <summary> Save preferences in EditorPrefs </summary>
        private static void SavePrefs(string key, Settings settings)
        {
            EditorPrefs.SetString(key, JsonUtility.ToJson(settings));
        }

        /// <summary> Check if we have loaded settings for given key. If not, load them </summary>
        private static void VerifyLoaded()
        {
            if (!settings.ContainsKey(lastKey)) settings.Add(lastKey, LoadPrefs());
        }

        private static void SystemSettingsGUI(string key, Settings settings)
        {
            EditorGUILayout.LabelField("System", EditorStyles.boldLabel);
            bool bChange = false;
            var fields = settings.GetType().GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            for (int i = 0; i < fields.Length; ++i)
            {
                string name = fields[i].Name;
                if(fields[i].IsDefined(typeof(DisplayAttribute),false))
                {
                    name = fields[i].GetCustomAttribute<DisplayAttribute>().displayName;
                }
                if (fields[i].FieldType == typeof(Color))
                {
                    Color col = (Color)fields[i].GetValue(settings);
                    EditorGUI.BeginChangeCheck();
                    col = EditorGUILayout.ColorField(name, col);
                    if (EditorGUI.EndChangeCheck())
                    {
                        bChange = true;
                        fields[i].SetValue(settings, col);
                    }
                }
                else if (fields[i].FieldType == typeof(bool))
                {
                    bool col = (bool)fields[i].GetValue(settings);
                    EditorGUI.BeginChangeCheck();
                    col = EditorGUILayout.Toggle(name, col);
                    if (EditorGUI.EndChangeCheck())
                    {
                        bChange = true;
                        fields[i].SetValue(settings, col);
                    }
                }
                else if (fields[i].FieldType == typeof(float))
                {
                    float col = (float)fields[i].GetValue(settings);
                    EditorGUI.BeginChangeCheck();
                    col = EditorGUILayout.FloatField(name, col);
                    if (EditorGUI.EndChangeCheck())
                    {
                        bChange = true;
                        fields[i].SetValue(settings, col);
                    }
                }
                else if (fields[i].FieldType == typeof(int))
                {
                    int col = (int)fields[i].GetValue(settings);
                    EditorGUI.BeginChangeCheck();
                    col = EditorGUILayout.IntField(name, col);
                    if (EditorGUI.EndChangeCheck())
                    {
                        bChange = true;
                        fields[i].SetValue(settings, col);
                    }
                }
                else if (fields[i].FieldType == typeof(string))
                {
                    string col = (string)fields[i].GetValue(settings);
                    EditorGUI.BeginChangeCheck();
                    col = EditorGUILayout.TextField(name, col);
                    if (EditorGUI.EndChangeCheck())
                    {
                        bChange = true;
                        fields[i].SetValue(settings, col);
                    }
                }
                else if (fields[i].FieldType == typeof(Vector2))
                {
                    Vector2 col = (Vector2)fields[i].GetValue(settings);
                    EditorGUI.BeginChangeCheck();
                    col = EditorGUILayout.Vector2Field(name, col);
                    if (EditorGUI.EndChangeCheck())
                    {
                        bChange = true;
                        fields[i].SetValue(settings, col);
                    }
                }
                else if (fields[i].FieldType == typeof(Vector3))
                {
                    Vector3 col = (Vector3)fields[i].GetValue(settings);
                    EditorGUI.BeginChangeCheck();
                    col = EditorGUILayout.Vector3Field(name, col);
                    if (EditorGUI.EndChangeCheck())
                    {
                        bChange = true;
                        fields[i].SetValue(settings, col);
                    }
                }
            }
            if (bChange) SavePrefs(key, settings);

            EditorGUILayout.Space();
        }
    }
}
#endif