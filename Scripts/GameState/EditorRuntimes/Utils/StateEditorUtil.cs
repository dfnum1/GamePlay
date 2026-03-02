#if UNITY_EDITOR
using Framework.Base;
using Framework.Core;
using Framework.ED;
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
                    ms_PanelTileStyle = new GUIStyle(GUI.skin.label);
                    ms_PanelTileStyle.fontSize += 3;
                    ms_PanelTileStyle.normal.textColor = Color.white;
                    ms_PanelTileStyle.alignment = TextAnchor.MiddleCenter;
                    ms_PanelTileStyle.fontStyle = FontStyle.Bold;

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

                        if(tp.IsSubclassOf(typeof(AState)))
                        {
                            if (!tp.IsDefined(typeof(GameStateAttribute), false))
                                continue;
                        }
                        else if (tp.IsSubclassOf(typeof(AStateLogic)))
                        {
                            if (!tp.IsDefined(typeof(GameStateLogicAttribute), false))
                                continue;
                        }
                        else if (tp.IsSubclassOf(typeof(AMode)))
                        {
                            if (!tp.IsDefined(typeof(GameModeAttribute), false))
                                continue;
                        }
                        else if (tp.IsSubclassOf(typeof(AModeLogic)))
                        {
                            if (!tp.IsDefined(typeof(GameModeLogicAttribute), false))
                                continue;
                        }

                        var clsId = GetTypeClassId(tp);
                        ms_StateWorldTypes[name] = tp;
                        ms_StateWorldTypeIds[clsId] = tp;

                        ms_StateWorldTypeNames[clsId] = name;
                    }

                    if(tp.IsSubclassOf(typeof(AGameEditor)) && tp.IsDefined(typeof(StateCustomEditorAttribute)))
                    {
                        var attr = tp.GetCustomAttribute<StateCustomEditorAttribute>();
                        if (attr.type != null)
                        {
                            ms_StateWorldTypeEditorTypes[attr.type] = tp;
                        }
                    }
                }
            }
        }
        //-----------------------------------------------------
        internal static bool IsExistLogics(int clsId, List<GameStateLogicData> vLogics)
        {
            for (int j = 0; j < vLogics.Count; ++j)
            {
                if (vLogics[j].logicType == clsId)
                {
                    return true;
                }
            }
            return false;
        }
        //-----------------------------------------------------
        public static void DrawStateLogic(GameStateLogicData stateLogic, List<GameStateLogicData> vLogics = null, System.Action<int> action = null)
        {
            var enable = EditorGUILayout.Toggle("是否启用", stateLogic.enabled);
            if(enable!= stateLogic.enabled)
            {
                if (action != null) action(1);
                stateLogic.enabled = enable;
            }
            int index = -1;
            if (vLogics != null) index = vLogics.IndexOf(stateLogic);
            if (IsStateWorldType<AStateLogic>(stateLogic.logicType))
            {
                GameStateLogicProvider.Draw(new GUIContent("逻辑组件"), stateLogic, (clasId,indx) =>
                {
                    if(IsExistLogics(clasId, vLogics))
                    {
                        EditorUtility.DisplayDialog("提示", "该逻辑组件已被配置", "好的");
                    }
                    else
                    {
                        if(stateLogic.logicType != clasId)
                        {
                            if (action != null) action(1);
                            stateLogic.logicType = clasId;
                        }
                    }
                }, index, false);
            }
            else if (IsStateWorldType<AModeLogic>(stateLogic.logicType))
            {
                GameModeLogicProvider.Draw(new GUIContent("逻辑组件"), stateLogic, (clasId,indx) =>
                {
                    if (IsExistLogics(clasId, vLogics))
                    {
                        EditorUtility.DisplayDialog("提示", "该逻辑组件已被配置", "好的");
                    }
                    else
                    {
                        if (stateLogic.logicType != clasId)
                        {
                            if (action != null) action(1);
                            stateLogic.logicType = clasId;
                        }
                    }
                }, index, false);
            }
            else
            {
                EditorGUILayout.HelpBox("逻辑组件[" + stateLogic.logicType + "] 不存在!!!", MessageType.Error);
            }
            if(vLogics !=null && GUILayout.Button("删除逻辑"))
            {
                if(EditorUtility.DisplayDialog("提示", "确定删除该逻辑组件?","删除", "再想想"))
                {
                    vLogics.Remove(stateLogic);
                    if (action != null) action(2);
                }
            }
        }
        //-----------------------------------------------------
        public static List<GameStateLogicData> DrawStateLogics(AStateEditorLogic logic, string label, List<GameStateLogicData> vLogics)
        {
            if (vLogics == null) vLogics = new List<GameStateLogicData>();
            GUILayout.BeginHorizontal();
            GUILayout.Label(label);
            if(GUILayout.Button("添加逻辑"))
            {
                logic.UndoRegister(true);
                vLogics.Add(new GameStateLogicData());
            }
            GUILayout.EndHorizontal();

            for(int i =0; i < vLogics.Count; ++i)
            {
                GUILayout.BeginHorizontal();
                EditorGUI.indentLevel++;
                {
                    var clasType = vLogics[i];
                    if (!IsStateWorldType<AStateLogic>(vLogics[i].logicType))
                    {
                        clasType.logicType = 0;
                    }
                    GameStateLogicProvider.Draw(new GUIContent("逻辑[" + i + "]"), clasType, (clsId, index) => {
                        if (index >= 0 && index < vLogics.Count)
                        {
                            bool isExist = false;
                            for(int j =0; j < vLogics.Count; ++j)
                            {
                                if (vLogics[j].logicType == clsId)
                                {
                                    isExist = true;
                                    break;
                                }
                            }
                            if (!isExist)
                            {
                                if(vLogics[index].logicType != clsId)
                                {
                                    logic.UndoRegister(false);
                                    vLogics[index].logicType = clsId;
                                    logic.OnRefreshData(logic.GetWorldData());
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
                            logic.OnRefreshData(logic.GetWorldData());
                            --i;
                        }
                    }
                    if (i > 0)
                    {
                        if (GUILayout.Button("↑", GUILayout.Width(20)))
                        {
                            //! 交换位置
                            logic.UndoRegister(false);
                            var temp = vLogics[i - 1];
                            vLogics[i - 1] = vLogics[i];
                            vLogics[i] = temp;
                            logic.OnRefreshData(logic.GetWorldData());
                        }
                    }
                    if (i < vLogics.Count - 1)
                    {
                        if (GUILayout.Button("↓", GUILayout.Width(20)))
                        {
                            //! 交换位置
                            logic.UndoRegister(false);
                            var temp = vLogics[i + 1];
                            vLogics[i + 1] = vLogics[i];
                            vLogics[i] = temp;
                            logic.OnRefreshData(logic.GetWorldData());
                        }
                    }
                }
                EditorGUI.indentLevel--;
                GUILayout.EndHorizontal();
            }

            return vLogics;
        }
        //-----------------------------------------------------
        public static List<GameStateLogicData> DrawModeLogics(AStateEditorLogic logic, string label, List<GameStateLogicData> vLogics)
        {
            if (vLogics == null) vLogics = new List<GameStateLogicData>();
            GUILayout.BeginHorizontal();
            GUILayout.Label(label);
            if (GUILayout.Button("添加逻辑"))
            {
                logic.UndoRegister(true);
                vLogics.Add(new GameStateLogicData());
            }
            GUILayout.EndHorizontal();
            for (int i = 0; i < vLogics.Count; ++i)
            {
                GUILayout.BeginHorizontal();
                EditorGUI.indentLevel++;
                {
                    var clasType = vLogics[i];
                    if (!IsStateWorldType<AModeLogic>(vLogics[i].logicType))
                    {
                        clasType.logicType = 0;
                    }
                    GameModeLogicProvider.Draw(new GUIContent("逻辑[" + i + "]"), clasType, (clsId, index) => {
                        if (index >= 0 && index < vLogics.Count)
                        {
                            bool isExist = false;
                            for (int j = 0; j < vLogics.Count; ++j)
                            {
                                if (vLogics[j].logicType == clsId)
                                {
                                    isExist = true;
                                    break;
                                }
                            }
                            if (!isExist)
                            {
                                if (vLogics[index].logicType != clsId)
                                {
                                    logic.UndoRegister(false);
                                    vLogics[index].logicType = clsId;
                                    logic.OnRefreshData(logic.GetWorldData());
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
                            var temp = vLogics[i - 1];
                            vLogics[i - 1] = vLogics[i];
                            vLogics[i] = temp;
                            logic.OnRefreshData(logic.GetWorldData());
                        }
                    }
                    if (i < vLogics.Count - 1)
                    {
                        if (GUILayout.Button("↓", GUILayout.Width(20)))
                        {
                            //! 交换位置
                            logic.UndoRegister(false);
                            var temp = vLogics[i + 1];
                            vLogics[i + 1] = vLogics[i];
                            vLogics[i] = temp;
                            logic.OnRefreshData(logic.GetWorldData());
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