#if UNITY_EDITOR
using Framework.State.Runtime;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
namespace Framework.State.Editor
{
    public class StateEditorUtil
    {
        static string ms_installPath = null;
        public static string BuildInstallPath()
        {
            if (string.IsNullOrEmpty(ms_installPath))
            {
                var scripts = UnityEditor.AssetDatabase.FindAssets("t:Script GameWorldEditor");
                if (scripts.Length > 0)
                {
                    ms_installPath = System.IO.Path.GetDirectoryName(UnityEditor.AssetDatabase.GUIDToAssetPath(scripts[0])).Replace("\\", "/");
                }
            }
            return ms_installPath;
        }
        //-----------------------------------------------------
        internal static int GetTypeClassId(System.Type type)
        {
            string typeName = type.FullName.Replace("+", ".");
            typeName = typeName.ToLower();
            int hash = Animator.StringToHash(typeName);
            return hash;
        }
        //-----------------------------------------------------
        private static GUIStyle ms_PanelTileStyle = null;
        public static GUIStyle panelTitleStyle
        {
            get
            {
                if (ms_PanelTileStyle == null)
                {
                    ms_PanelTileStyle = new GUIStyle();
                    ms_PanelTileStyle.fontSize = 13;
                    ms_PanelTileStyle.normal.textColor = Color.white;
                    ms_PanelTileStyle.alignment = TextAnchor.MiddleCenter;

                }
                return ms_PanelTileStyle;
            }
        }
        //-----------------------------------------------------
        static bool ms_bInitTypeed = false;
        private static Dictionary<string, System.Type> ms_StateWorldTypes = new Dictionary<string, System.Type>();
        private static Dictionary<int, System.Type> ms_StateWorldTypeIds = new Dictionary<int, System.Type>();
        //-----------------------------------------------------
        public static System.Type GetStateWorldType(int id)
        {
            InitTypes();
            if (ms_StateWorldTypeIds.TryGetValue(id, out var worldType))
                return worldType;
            return null;
        }
        //-----------------------------------------------------
        public static System.Type GetStateWorldType(string name)
        {
            InitTypes();
            if (ms_StateWorldTypes.TryGetValue(name, out var worldType))
                return worldType;
            return null;
        }
        //-----------------------------------------------------
        public static Dictionary<string,System.Type> GetStateWorldTypes()
        {
            InitTypes();
            return ms_StateWorldTypes;
        }
        //-----------------------------------------------------
        public static Dictionary<int, System.Type> GetStateWorldIdTypes()
        {
            InitTypes();
            return ms_StateWorldTypeIds;
        }
        //-----------------------------------------------------
        internal static void ReInitTypes()
        {
            ms_bInitTypeed = false;
        }
        //-----------------------------------------------------
        internal static void InitTypes()
        {
            if (ms_bInitTypeed)
                return;
            ms_bInitTypeed = true;
            ms_StateWorldTypes.Clear();
            ms_StateWorldTypeIds.Clear();
            foreach (var ass in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] types = null;
                try
                {
                    types = ass.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    types = ex.Types; // 部分可用类型
                                      // 可选：输出警告
                    UnityEngine.Debug.LogWarning($"加载程序集 {ass.FullName} 时部分类型无法加载: {ex}");
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogWarning($"加载程序集 {ass.FullName} 时发生异常: {ex}");
                    continue;
                }
                for (int i = 0; i < types.Length; ++i)
                {
                    Type tp = types[i];
                    if (tp == null || tp.IsAbstract) continue;
                    if (tp.IsSubclassOf(typeof(AState)) ||
                        tp.IsSubclassOf(typeof(AStateLogic)) ||
                        tp.IsSubclassOf(typeof(AMode)) ||
                        tp.IsSubclassOf(typeof(AModeLogic)))
                    {
                        DecNameAttribute attr = tp.GetCustomAttribute<DecNameAttribute>();
                        string name = tp.Name;
                        if (attr != null && !string.IsNullOrEmpty(name))
                            name = attr.name;
                        ms_StateWorldTypes[name] = tp;
                        ms_StateWorldTypeIds[GetTypeClassId(tp)] = tp;
                    }
                }
            }
        }
    }
}
#endif