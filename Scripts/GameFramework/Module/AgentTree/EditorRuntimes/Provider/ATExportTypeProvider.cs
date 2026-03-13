#if UNITY_EDITOR
using Framework.AT.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static Framework.AT.Editor.ExportAutoCode;

namespace Framework.AT.Editor
{
    public class ATExportTypeProvider : ScriptableObject, ISearchWindowProvider
    {
        public System.Action<System.Type, int> onSelect;
        int m_UserIndex = -1;
        //-----------------------------------------------------
        public static void PopSearch(System.Action<System.Type, int> callback, int userIndex = -1)
        {
            PopSearch(Event.current.mousePosition, callback, userIndex);
        }
        //-----------------------------------------------------
        public static void PopSearch(Vector2 mousePosition, System.Action<System.Type, int> callback, int userIndex = -1)
        {
            var provider = ScriptableObject.CreateInstance<Editor.ATExportTypeProvider>();
            provider.m_UserIndex = userIndex;
            provider.onSelect = (node, index) =>
            {
                if (callback != null) callback(node, index);
            };
            UnityEditor.Experimental.GraphView.SearchWindow.Open(new UnityEditor.Experimental.GraphView.SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition + Vector2.up * 25)), provider);
        }
        //-----------------------------------------------------
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var tree = new List<SearchTreeEntry>();
            tree.Add(new SearchTreeGroupEntry(new GUIContent("选择游状态")));
            var nodes = AgentTreeUtil.GetATClassTypes();
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    if(!node.Value.IsDefined(typeof(ATExportAttribute)))
                    {
                        continue;
                    }
                    var atNode = node.Value.GetCustomAttribute<ATExportAttribute>();
                    string name = atNode.nodeName;
                    if (string.IsNullOrEmpty(name)) name = node.Value.Name;
                    var menus = name.Split('/');
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
            if (entry.userData is KeyValuePair<int, System.Type> data)
            {
                onSelect?.Invoke(data.Value, m_UserIndex);
                return true;
            }
            return false;
        }
    }
}
#endif