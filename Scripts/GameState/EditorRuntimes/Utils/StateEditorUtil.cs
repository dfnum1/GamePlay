#if UNITY_EDITOR
using Framework.Core;
using Framework.State.Runtime;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
namespace Framework.State.Editor
{
    internal struct MenuContextData
    {
        public System.Object bindData;
        public Vector2 mousePosition;
        public MenuContextData(System.Object bind, Vector2 pos)
        {
            this.bindData = bind;
            this.mousePosition = pos;
        }
    }
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
        private static Dictionary<int, string> ms_StateWorldTypeNames = new Dictionary<int, string>();
        private static Dictionary<System.Type, System.Type> ms_StateWorldTypeEditorTypes = new Dictionary<Type, Type>();
        //-----------------------------------------------------
        public static System.Type GetStateWorldType(int id)
        {
            InitTypes();
            if (ms_StateWorldTypeIds.TryGetValue(id, out var worldType))
                return worldType;
            return null;
        }
        //-----------------------------------------------------
        public static bool IsStateWorldType<T>(int id) where T : TypeObject
        {
            var type = GetStateWorldType(id);
            if (type == null) return false;
            return type.IsSubclassOf(typeof(T));
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
        public static string GetStateWorldTypeName(int id, string defName ="未知名称")
        {
            InitTypes();
            if (ms_StateWorldTypeNames.TryGetValue(id, out var name))
                return name;
            var clsType = GetStateWorldType(id);
            if (clsType != null) return clsType.Name;
            return defName;
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
        internal static System.Type GetTypeEditorType(System.Type type)
        {
            InitTypes();
            if (ms_StateWorldTypeEditorTypes.TryGetValue(type, out var editorType))
                return editorType;
            return null;
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
            ms_StateWorldTypeNames.Clear();
            ms_StateWorldTypeEditorTypes.Clear();
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
                        tp.IsSubclassOf(typeof(AMode))  ||
                        tp.IsSubclassOf(typeof(AModeLogic)) ||
                        tp.IsSubclassOf(typeof(AGameCfgData)))
                    {
                        DecNameAttribute attr = tp.GetCustomAttribute<DecNameAttribute>();
                        string name = tp.Name;
                        if (attr != null && !string.IsNullOrEmpty(name))
                            name = attr.name;

                        var clsId = GetTypeClassId(tp);
                        ms_StateWorldTypes[name] = tp;
                        ms_StateWorldTypeIds[clsId] = tp;

                        ms_StateWorldTypeNames[clsId] = name;
                    }

                    if(tp.IsSubclassOf(typeof(AGameEditor)) && tp.IsDefined(typeof(CustomEditor)))
                    {
                        var attr = tp.GetCustomAttribute<CustomEditor>();
                        var field = attr.GetType().GetField("m_InspectedType", BindingFlags.Instance | BindingFlags.NonPublic);
                        if (field != null)
                        {
                            var typeObj = field.GetValue(attr);
                            if (typeObj != null && typeObj is Type)
                            {
                                ms_StateWorldTypeEditorTypes[(Type)typeObj] = tp;
                            }
                        }
                    }
                }
            }
        }
        //-----------------------------------------------------
        public static List<int> DrawStateLogics(AStateEditorLogic logic, string label, List<int> vLogics)
        {
            if (vLogics == null) vLogics = new List<int>();
            GUILayout.BeginHorizontal();
            GUILayout.Label(label);
            if(GUILayout.Button("添加逻辑"))
            {
                logic.UndoRegister(true);
                vLogics.Add(0);
            }
            GUILayout.EndHorizontal();

            for(int i =0; i < vLogics.Count; ++i)
            {
                GUILayout.BeginHorizontal();
                EditorGUI.indentLevel++;
                {
                    int clasType = vLogics[i];
                    if (!IsStateWorldType<AStateLogic>(vLogics[i]))
                    {
                        clasType = 0;
                    }
                    GameStateLogicProvider.Draw(new GUIContent("逻辑[" + i + "]"), clasType, (clsId, index) => {
                        if (index >= 0 && index < vLogics.Count)
                        {
                            if (!vLogics.Contains(clsId))
                            {
                                if(vLogics[index] != clsId)
                                {
                                    logic.UndoRegister(false);
                                    vLogics[index] = clsId;
                                }
                            }
                            else
                                EditorUtility.DisplayDialog("提示", "逻辑状态已存在！！", "好的");
                        }
                    },i);
                    if (GUILayout.Button("-", GUILayout.Width(20)))
                    {
                        if (EditorUtility.DisplayDialog("提示", "确定是要移除？", "移除", "再想想"))
                        {
                            logic.UndoRegister(true);
                            vLogics.RemoveAt(i);
                            --i;
                        }
                    }
                    if (i > 0)
                    {
                        if (GUILayout.Button("↑", GUILayout.Width(20)))
                        {
                            //! 交换位置
                            logic.UndoRegister(false);
                            int temp = vLogics[i - 1];
                            vLogics[i - 1] = vLogics[i];
                            vLogics[i] = temp;
                        }
                    }
                    if (i < vLogics.Count - 1)
                    {
                        if (GUILayout.Button("↓", GUILayout.Width(20)))
                        {
                            //! 交换位置
                            logic.UndoRegister(false);
                            int temp = vLogics[i + 1];
                            vLogics[i + 1] = vLogics[i];
                            vLogics[i] = temp;
                        }
                    }
                }
                EditorGUI.indentLevel--;
                GUILayout.EndHorizontal();
            }

            return vLogics;
        }
        //-----------------------------------------------------
        public static List<int> DrawModeLogics(AStateEditorLogic logic, string label, List<int> vLogics)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label);
            if (GUILayout.Button("添加逻辑"))
            {
                logic.UndoRegister(true);
                vLogics.Add(0);
            }
            GUILayout.EndHorizontal();

            for (int i = 0; i < vLogics.Count; ++i)
            {
                GUILayout.BeginHorizontal();
                EditorGUI.indentLevel++;
                {
                    int clasType = vLogics[i];
                    if (!IsStateWorldType<AModeLogic>(vLogics[i]))
                    {
                        clasType = 0;
                    }
                    GameModeLogicProvider.Draw(new GUIContent("逻辑[" + i + "]"), clasType, (clsId, index) => {
                        if (vLogics == null) vLogics = new List<int>();
                        if (index >= 0 && index < vLogics.Count)
                        {
                            if (!vLogics.Contains(clsId))
                            {
                                if (vLogics[index] != clsId)
                                {
                                    logic.UndoRegister(false);
                                    vLogics[index] = clsId;
                                }
                            }
                            else
                                EditorUtility.DisplayDialog("提示", "逻辑状态已存在！！", "好的");
                        }
                    },i);
                    if (GUILayout.Button("-", GUILayout.Width(20)))
                    {
                        if (EditorUtility.DisplayDialog("提示", "确定是要移除？", "移除", "再想想"))
                        {
                            logic.UndoRegister(true);
                            vLogics.RemoveAt(i);
                            --i;
                        }
                    }
                    if (i > 0)
                    {
                        if (GUILayout.Button("↑", GUILayout.Width(20)))
                        {
                            //! 交换位置
                            logic.UndoRegister(false);
                            int temp = vLogics[i - 1];
                            vLogics[i - 1] = vLogics[i];
                            vLogics[i] = temp;
                        }
                    }
                    if (i < vLogics.Count - 1)
                    {
                        if (GUILayout.Button("↓", GUILayout.Width(20)))
                        {
                            //! 交换位置
                            logic.UndoRegister(false);
                            int temp = vLogics[i + 1];
                            vLogics[i + 1] = vLogics[i];
                            vLogics[i] = temp;
                        }
                    }
                }
                EditorGUI.indentLevel--;
                GUILayout.EndHorizontal();
            }

            return vLogics;
        }
    }
}
#endif