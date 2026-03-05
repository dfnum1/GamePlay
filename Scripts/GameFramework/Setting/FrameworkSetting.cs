/********************************************************************
生成日期:	3:10:2019  15:03
类    名: 	AFrameworkSetting
作    者:	HappLI
描    述:	框架设置
*********************************************************************/
using Framework.AT.Runtime;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using Framework.ED;

#endif


#if USE_FIXEDMATH
using ExternEngine;
#else
using FFloat = System.Single;
using FMatrix4x4 = UnityEngine.Matrix4x4;
using FQuaternion = UnityEngine.Quaternion;
using FVector2 = UnityEngine.Vector2;
using FVector3 = UnityEngine.Vector3;
using FBounds = UnityEngine.Bounds;
using FRay = UnityEngine.Ray;
#endif

namespace Framework.Core
{
    [System.Serializable]
    public abstract class AFrameworkSettingData
    {
        [InspectorName("技能系统蓝图")]public AAgentTreeObject skillSystem;
        [InspectorName("Buff系统蓝图")] public AAgentTreeObject buffSystem;
    }
    public abstract class AFrameworkSetting : ScriptableObject
    {
        public abstract AFrameworkSettingData GetSetting();
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(AFrameworkSetting))]
    [EditorInitOnload]
    public class AFrameworkSettingEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            AFrameworkSetting setting = target as AFrameworkSetting;
            InspectorDrawUtil.DrawProperty(setting, System.Reflection.BindingFlags.Instance| System.Reflection.BindingFlags.Public| System.Reflection.BindingFlags.NonPublic);
            if (GUILayout.Button("保存"))
            {
                EditorUtility.SetDirty(setting);
                AssetDatabase.SaveAssetIfDirty(setting);
            }
        }
        //-----------------------------------------------------
        [MenuItem("Assets/Create/GamePlay/Gameplay全局配置")]
        public static void CreateGamePlaySetting()
        {
            var setting = CreateSetting(true);
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = setting;
        }
        //-----------------------------------------------------
        public static AFrameworkSetting CreateSetting(bool bPop = false)
        {
            var guids = AssetDatabase.FindAssets("t: AFrameworkSetting");
            if (guids.Length > 0)
            {
                if(bPop) EditorUtility.DisplayDialog("提示", "已存在Gameplay全局配置资源，请勿重复创建！", "确定");
                var setting = AssetDatabase.LoadAssetAtPath<AFrameworkSetting>(AssetDatabase.GUIDToAssetPath(guids[0]));
                return setting;
            }
            // 弹窗让用户选择保存路径
            string defaultName = "GlobalGameplaySetting.asset";
            string path = EditorUtility.SaveFilePanelInProject(
                "选择全局配置保存路径",
                defaultName,
                "asset",
                "请选择保存全局配置的路径"
            );
            if (string.IsNullOrEmpty(path))
                return null; // 用户取消

            var asset = Framework.ED.EditorUtils.CreateUnityScriptObject<Framework.Core.AFrameworkSetting>();
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssetIfDirty(asset);
            return asset;
        }
        //-----------------------------------------------------
        static Texture2D s_CustomIcon;
        static internal void OnEditorInitOnload()
        {
            s_CustomIcon = Framework.ED.EditorUtils.LoadEditorResource<Texture2D>("setting.png");
            EditorApplication.projectWindowItemOnGUI += OnProjectWindowItemGUI;
        }
        //-----------------------------------------------------
        static void OnProjectWindowItemGUI(string guid, Rect selectionRect)
        {
            if (s_CustomIcon == null) return;
            string path = AssetDatabase.GUIDToAssetPath(guid);
            var obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
            if (obj is AFrameworkSetting)
            {
                if (EditorGUIUtility.GetIconForObject(obj) != s_CustomIcon)
                    EditorGUIUtility.SetIconForObject(obj, s_CustomIcon);
            }
        }
    }
    //-----------------------------------------------------
    //! ProjectSettingsProvider
    //-----------------------------------------------------
    static class ProjectSettingsProvider
    {
        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider()
        {
            AFrameworkSetting setting = null;
            var guids = AssetDatabase.FindAssets("t: AFrameworkSetting");
            if (guids.Length > 0)
            {
                setting = AssetDatabase.LoadAssetAtPath<AFrameworkSetting>(AssetDatabase.GUIDToAssetPath(guids[0]));
            }


            var provider = new SettingsProvider("Project/Gameplay", SettingsScope.Project)
            {
                label = "Gameplay",
                guiHandler = (searchContext) =>
                {
                    if (setting == null)
                    {
                        if (GUILayout.Button("创建全局配置"))
                        {
                            setting= AFrameworkSettingEditor.CreateSetting();
                        }
                        return;
                    }

                    InspectorDrawUtil.DrawProperty(setting, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                    if (GUILayout.Button("保存"))
                    {
                        EditorUtility.SetDirty(setting);
                        AssetDatabase.SaveAssetIfDirty(setting);
                    }
                }
            };
            return provider;
        }
    }
#endif
}