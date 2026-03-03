#if UNITY_EDITOR
/********************************************************************
生成日期:	11:9:2025
类    名: 	AssetSearchProvider
作    者:	HappLI
描    述:	资源选择器
*********************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Framework.Cutscene.Editor
{
    public class AssetSearchProvider : ScriptableObject, ISearchWindowProvider
	{
		public class AssetInfo
		{
			public string codeName;
			public string file;
		}
		private System.Type m_assetType;
		private List<SearchTreeEntry> _entries;
		private Action<string, string> _onSelect;
		static string ms_lastMd5 = null;
        static Dictionary<string, AssetInfo> ms_vCodeNames = new();
        static bool ms_bLoaded = false;
		public static Dictionary<string, AssetInfo> GetAssets(System.Type type)
		{
			List<int> unExistVideos = new List<int>();
			var assets = AssetDatabase.FindAssets("t:" + type.Name);
			for (int i = 0; i < assets.Length; ++i)
			{
				string path = AssetDatabase.GUIDToAssetPath(assets[i]);
				string name = Path.GetFileNameWithoutExtension(path);
				AssetInfo info = new AssetInfo();
				info.codeName = name;
				info.file = path;
				ms_vCodeNames[name] = info;
            }
			return ms_vCodeNames;
		}
        //------------------------------------------------------
		public static AssetInfo GetByCodeName(string videoCodeName, System.Type type)
		{
            GetAssets(type);
			if (ms_vCodeNames.TryGetValue(videoCodeName, out var videoInfo))
				return videoInfo;
			return null;
        }
        //------------------------------------------------------
        public void Init(System.Type assetType, Action<string, string> onSelect)
		{
			m_assetType = assetType;
            _onSelect = onSelect;
			_entries = null; // 重新加载
		}
		//------------------------------------------------------
		public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
		{
			if (_entries != null)
				return _entries;

			_entries = new List<SearchTreeEntry>
			{
				new SearchTreeGroupEntry(new GUIContent(m_assetType.Name), 0)
			};
			var assetList = GetAssets(m_assetType);

			// 路径分级缓存，key为完整路径，value为SearchTreeEntry
			var groupDict = new Dictionary<string, SearchTreeEntry>
				{
					{ "", _entries[0] } // 根节点
				};

			if (assetList != null)
			{
				foreach (var info in assetList)
				{
					var pathParts = info.Value.codeName.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
					string curPath = "";
					int level = 1;

					// 逐级创建分组
					for (int i = 0; i < pathParts.Length - 1; i++)
					{
						string part = pathParts[i];
						string parentPath = curPath;
						curPath = string.IsNullOrEmpty(curPath) ? part : $"{curPath}/{part}";

						if (!groupDict.ContainsKey(curPath))
						{
							var groupEntry = new SearchTreeGroupEntry(new GUIContent(part), level);
							_entries.Add(groupEntry);
							groupDict[curPath] = groupEntry;
						}
						level++;
					}

					// 添加节点
					string prefabName = pathParts[pathParts.Length - 1];
					var entry = new SearchTreeEntry(new GUIContent(prefabName))
					{
						level = level,
						userData = info.Value // 选择时返回prefabName
					};
					_entries.Add(entry);
				}
			}

			return _entries;
		}
		//------------------------------------------------------
		public bool OnSelectEntry(SearchTreeEntry entry, SearchWindowContext context)
		{
			if (entry.userData is AssetInfo name)
			{
				_onSelect?.Invoke(name.codeName, name.file);
				return true;
			}
			return false;
		}
	}
}
#endif