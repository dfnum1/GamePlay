#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Framework.Guide.Editor
{

    [Framework.Guide.GuideDisplayType("OnDrawNetCode", "引导GUID", false)]
    public class GuideProvider
    {
        static Dictionary<int, GuideGroup> ms_vCodes = new Dictionary<int, GuideGroup>();
        static string ms_LastMd5 = null;
        public static void InitData()
        {
            string[] guideDatas = AssetDatabase.FindAssets("t:GuideDatas");
            if (guideDatas != null && guideDatas.Length > 0)
            {
                var guideCsv = AssetDatabase.LoadAssetAtPath<GuideDatas>(AssetDatabase.GUIDToAssetPath(guideDatas[0]));
                if(guideCsv!=null)
                {
                    guideCsv.Init();
                    ms_vCodes = guideCsv.allDatas;
                }
            }
        }

        public static void OnDrawNetCode(GUIContent label, object pData, System.Reflection.FieldInfo fieldInfo)
        {
            if (fieldInfo == null || fieldInfo.FieldType != typeof(int)) return;
            if (ms_vCodes.Count <= 0)
            {
                InitData();
            }
            string codeName = "";
            var objData = fieldInfo.GetValue(pData);
            int codeId = 0;
            if (objData != null) codeId = (int)objData;
            if (ms_vCodes.TryGetValue(codeId, out var info))
            {
                codeName = info.Name;
                if (info.Tag >= 1 && info.Tag < 65535)
                    codeName += $" [Tag:{info.Tag}]";
            }

            Color color = GUI.color;
            if (string.IsNullOrEmpty(codeName))
            {
                codeName = "NONE";
                GUI.color = Color.red;
            }
            float labelWidthBack = EditorGUIUtility.labelWidth;
            if (!string.IsNullOrEmpty(label.text))
            {
                EditorGUIUtility.labelWidth = Mathf.Min(40, GUI.skin.textField.CalcSize(label).x + 5);
            }
            else
                EditorGUIUtility.labelWidth = 1;
            try
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.TextField(label, codeName);
                GUI.color = color;
                EditorGUIUtility.labelWidth = labelWidthBack;
                if (GUILayout.Button("...", GUILayout.Width(15)))
                {
                    var provider = ScriptableObject.CreateInstance<GuideSearch>();
                    provider.onSelected = (info) =>
                    {
                        if (fieldInfo != null && pData != null)
                        {
                            fieldInfo.SetValue(pData, info);
                        }
                    };
                    SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)), provider);
                }
                EditorGUILayout.EndHorizontal();
            }
            catch { }
        }

        public class GuideSearch : ScriptableObject, ISearchWindowProvider
        {
            public System.Action<int> onSelected;

            public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
            {
                InitData();
                var tree = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("消息Id"), 0)
            };

                // 按Index排序
                foreach (var info in ms_vCodes)
                {
                    string display = $"{info.Value.Name}";
                    if(info.Value.Tag>=1 && info.Value.Tag< 65535)
                        display += $" [Tag:{info.Value.Tag}]";
                    var entry = new SearchTreeEntry(new GUIContent(display))
                    {
                        level = 1,
                        userData = info.Key
                    };
                    tree.Add(entry);
                }
                return tree;
            }

            public bool OnSelectEntry(SearchTreeEntry entry, SearchWindowContext context)
            {
                if (entry.userData is int)
                {
                    onSelected?.Invoke((int)entry.userData);
                    return true;
                }
                return false;
            }
        }
    }
}
#endif