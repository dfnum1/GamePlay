#if UNITY_EDITOR
using Framework.AT.Runtime;
using Framework.State.Runtime;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Framework.State.Editor
{
    public class GameModeLogicProvider : ScriptableObject, ISearchWindowProvider
    {
        public System.Action<System.Type, int> onSelect;
        //-----------------------------------------------------
        public static void Draw(GUIContent label, int classId, System.Action<int> callback)
        {
            var stateType = StateEditorUtil.GetStateWorldType(classId);
            string name = "未选择模式逻辑组件";
            Color useColor = GUI.color;
            Color color = GUI.color;
            if (stateType == null)
            {
                if (classId == 0) useColor = Color.yellow;
                else
                {
                    name = "模式逻辑组件丢失";
                    useColor = Color.red;
                }
            }
            else
            {
                name = stateType.Name;
                if (stateType.IsDefined(typeof(DecNameAttribute)))
                {
                    var attr = stateType.GetCustomAttribute<DecNameAttribute>();
                    if (attr != null && !string.IsNullOrEmpty(attr.name))
                        name = attr.name;
                }
            }
            GUILayout.BeginHorizontal();
            if (label != null && !string.IsNullOrEmpty(label.text)) GUILayout.Label(label, GUILayout.MaxWidth(GUI.skin.label.CalcSize(label).x + 10));
            GUI.color = useColor;
            if (GUILayout.Button(name))
            {
                var provider = ScriptableObject.CreateInstance<Editor.GameModeLogicProvider>();
                provider.onSelect = (node, clsId) =>
                {
                    if (callback != null) callback(clsId);
                };
                UnityEditor.Experimental.GraphView.SearchWindow.Open(new UnityEditor.Experimental.GraphView.SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition + Vector2.up * 25)), provider);
            }
            GUI.color = color;
            if (callback != null && GUILayout.Button(new GUIContent(Framework.ED.EditorUtils.LoadEditorResource<Texture2D>("Editor/clean.png"), ""), EditorStyles.toolbarButton, GUILayout.Width(20)))
            {
                if (EditorUtility.DisplayDialog("提示", "确定删除状态", "删除", "再想想"))
                {
                    callback(0);
                }
            }
            GUILayout.EndHorizontal();
        }
        //-----------------------------------------------------
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var tree = new List<SearchTreeEntry>();
            tree.Add(new SearchTreeGroupEntry(new GUIContent("选择游戏模式逻辑组件")));
            var nodes = StateEditorUtil.GetStateWorldTypes();
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    if (!node.Value.IsSubclassOf(typeof(AModeLogic)))
                        continue;
                    var menus = node.Key.Split('/');
                    for (int i = 0; i < menus.Length; ++i)
                    {
                        if (i == menus.Length - 1)
                        {
                            tree.Add(new SearchTreeEntry(new GUIContent(menus[i]))
                            {
                                level = 1 + i,
                                userData = node.Value
                            });
                        }
                        else
                            tree.Add(new SearchTreeGroupEntry(new GUIContent(menus[i])) { level = 1 + i });
                    }
                }
            }
            return tree;
        }
        //-----------------------------------------------------
        public bool OnSelectEntry(SearchTreeEntry entry, SearchWindowContext context)
        {
            if (entry.userData is System.Type data)
            {
                onSelect?.Invoke(data, StateEditorUtil.GetTypeClassId(data));
                return true;
            }
            return false;
        }
    }
}
#endif