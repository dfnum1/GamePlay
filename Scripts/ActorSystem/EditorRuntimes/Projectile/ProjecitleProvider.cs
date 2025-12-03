#if UNITY_EDITOR
using Framework.ActorSystem.Runtime;
using Framework.Cutscene.Editor;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Framework.ActorSystem.Editor
{
    public class ProjecitleProvider
    {
        static Dictionary<uint, ProjectileData> ms_vCodes = new Dictionary<uint, ProjectileData>();
        static void InitData()
        {
            ms_vCodes.Clear();
            string[] guides = AssetDatabase.FindAssets("t:ProjectileDatas");
            if (guides != null && guides.Length > 0)
            {
                var data = AssetDatabase.LoadAssetAtPath<ProjectileDatas>(AssetDatabase.GUIDToAssetPath(guides[0]));
                if(data != null)
                {
                    ProjectileDatas.RefreshDatas(data);
                    if(data.projectiles!=null)
                    {
                        foreach(var db in data.projectiles)
                        {
                            ms_vCodes[db.id] = db;
                        }
                    }
                }
            }
        }

        public static ProjectileData GetProjectileData(uint id)
        {
            InitData();
            if (ms_vCodes.TryGetValue(id, out var projData))
                return projData;
            return null;
        }
        public static void Show(Action<ProjectileData> onSelected)
        {
            var provider = ScriptableObject.CreateInstance<ProjectileSearch>();
            provider.onSelected = onSelected;
            SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)), provider);
        }
        class ProjectileSearch : ScriptableObject, ISearchWindowProvider
        {
            public System.Action<ProjectileData> onSelected;

            public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
            {
                InitData();
                var tree = new List<SearchTreeEntry>
                {
                    new SearchTreeGroupEntry(new GUIContent("弹道库"), 0)
                };

                // 按Index排序
                foreach (var info in ms_vCodes)
                {
                    string display = info.Value.id.ToString();
                    if(info.Value.desc!=null) display += $"{info.Value.desc}";
                    var entry = new SearchTreeEntry(new GUIContent(display))
                    {
                        level = 1,
                        userData = info.Value
                    };
                    tree.Add(entry);
                }
                return tree;
            }

            public bool OnSelectEntry(SearchTreeEntry entry, SearchWindowContext context)
            {
                if (entry.userData is ProjectileData)
                {
                    onSelected?.Invoke((ProjectileData)entry.userData);
                    return true;
                }
                return false;
            }
        }
    }
}
#endif