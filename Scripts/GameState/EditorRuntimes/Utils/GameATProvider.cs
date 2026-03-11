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
    public class GameATProvider : ScriptableObject, ISearchWindowProvider
    {
        List<GameAgentData> m_vAgents;
        public System.Action<GameAgentData, int> onSelect;
        private int m_nUserIndex =-1;
        //-----------------------------------------------------
        public static void Draw(List<GameAgentData> vAgents, string label, int agentId, System.Action<GameAgentData, int> callback, int userIndex = -1)
        {
            Draw(vAgents, new GUIContent(label), agentId, callback, userIndex);
        }
        //-----------------------------------------------------
        public static void Draw(List<GameAgentData> vAgents, GUIContent label, int agentId, System.Action<GameAgentData, int> callback, int userIndex=-1)
        {
            GameAgentData agentData = null;
            if (vAgents!=null)
            {
                for(int i =0; i < vAgents.Count; ++i)
                {
                    if (vAgents[i].agentId == agentId)
                    {
                        agentData = vAgents[i];
                        break;
                    }
                }
            }
            string name = "";
            Color useColor = GUI.color;
            Color color = GUI.color;
            if (agentData == null)
            {
                if (agentId == 0) useColor = Color.yellow;
                else
                {
                    name = "蓝图脚本丢失";
                    useColor = Color.red;
                }
            }
            else
            {
                name = agentData.name + "[" + agentData.agentId + "]";
            }
            GUILayout.BeginHorizontal();
            if (label != null && !string.IsNullOrEmpty(label.text)) GUILayout.Label(label, GUILayout.MaxWidth(GUI.skin.label.CalcSize(label).x + 10));
            GUI.color = useColor;
            if (GUILayout.Button(name))
            {
                PopSearch(vAgents,callback, userIndex);
            }
            GUI.color = color;
            if (callback != null && GUILayout.Button(new GUIContent(Framework.ED.EditorUtils.LoadEditorResource<Texture2D>("Editor/clean.png"), ""), EditorStyles.toolbarButton, GUILayout.Width(20)))
            {
                if (EditorUtility.DisplayDialog("提示", "确定删除蓝图脚本", "删除", "再想想"))
                {
                    callback(null, userIndex);
                }
            }
            GUILayout.EndHorizontal();
        }
        //-----------------------------------------------------
        public static void PopSearch(List<GameAgentData> vAgents, System.Action<GameAgentData, int> callback, int useIndex=-1)
        {
            PopSearch(vAgents,Event.current.mousePosition, callback, useIndex);
        }
        //-----------------------------------------------------
        public static void PopSearch(List<GameAgentData> vAgents, Vector2 mousePosition, System.Action<GameAgentData, int> callback, int useIndex = -1)
        {
            var provider = ScriptableObject.CreateInstance<Editor.GameATProvider>();
            provider.m_nUserIndex = useIndex;
            provider.m_vAgents = vAgents;
            provider.onSelect = (node, clsId) =>
            {
                if (callback != null) callback(node, provider.m_nUserIndex);
            };
            UnityEditor.Experimental.GraphView.SearchWindow.Open(new UnityEditor.Experimental.GraphView.SearchWindowContext(GUIUtility.GUIToScreenPoint(mousePosition + Vector2.up * 25)), provider);
        }
        //-----------------------------------------------------
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var tree = new List<SearchTreeEntry>();
            tree.Add(new SearchTreeGroupEntry(new GUIContent("选择游戏蓝图脚本")));
            if (m_vAgents != null)
            {
                foreach (var node in m_vAgents)
                {
                    var menus = node.name.Split('/');
                    for (int i = 0; i < menus.Length; ++i)
                    {
                        if (i == menus.Length - 1)
                        {
                            tree.Add(new SearchTreeEntry(new GUIContent(menus[i]))
                            {
                                level = 1 + i,
                                userData = node
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
            if (entry.userData is GameAgentData data)
            {
                onSelect?.Invoke(data, m_nUserIndex);
                return true;
            }
            return false;
        }
    }
}
#endif