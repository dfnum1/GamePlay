/********************************************************************
生成日期:	12:03:2025
类    名: 	SearchTypeProvider
作    者:	HappLI
描    述:	
*********************************************************************/
#if UNITY_EDITOR
using Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Framework.Cutscene.Editor
{
    public class SearchTypeProvider : ScriptableObject, ISearchWindowProvider
    {
        // 可选：回调，选中类型后执行
        public Action<Type> OnTypeSelected;

        // 缓存所有类型
        private List<Type> m_AllTypes;

        // 构造类型列表
        private void BuildTypeList()
        {
            var baseType = typeof(IUserData);
            // 这里只列出所有非抽象、非泛型的 public 类型
            m_AllTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => baseType.IsAssignableFrom(t) && t != baseType && t.IsPublic && !t.IsAbstract && !t.IsGenericType)
                .OrderBy(t => t.FullName)
                .ToList();
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            if (m_AllTypes == null)
                BuildTypeList();

            var tree = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("所有类型"), 0)
            };
            // 用于记录已添加的分组路径，避免重复
            var groupLevels = new Dictionary<string, int>();

            foreach (var type in m_AllTypes)
            {
                // 按 . 分割命名空间和类名
                var parts = type.FullName.Split('.');
                string path = "";
                int level = 1;

                // 构建分组
                for (int i = 0; i < parts.Length - 1; ++i)
                {
                    path = path == "" ? parts[i] : path + "." + parts[i];
                    if (!groupLevels.ContainsKey(path))
                    {
                        tree.Add(new SearchTreeGroupEntry(new GUIContent(parts[i]), level));
                        groupLevels[path] = level;
                    }
                    level++;
                }

                // 添加类型条目
                tree.Add(new SearchTreeEntry(new GUIContent(parts.Last()))
                {
                    level = level,
                    userData = type
                });
            }
            return tree;
        }
        public bool OnSelectEntry(SearchTreeEntry entry, SearchWindowContext context)
        {
            if (entry.userData is Type type)
            {
                OnTypeSelected?.Invoke(type);
                return true;
            }
            return false;
        }
        public static void Show(Action<Type> onTypeSelected)
        {
            var provider = CreateInstance<SearchTypeProvider>();
            provider.OnTypeSelected = onTypeSelected;
            SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)), provider);
        }
    }
}
#endif