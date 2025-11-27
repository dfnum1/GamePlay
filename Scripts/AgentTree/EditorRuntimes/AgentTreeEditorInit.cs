#if UNITY_EDITOR
using Framework.AT.Runtime;
using System.IO;
using UnityEditor;
using UnityEngine;
namespace Framework.AT.Editor
{
    [InitializeOnLoad]
    public class AgentTreeEditorInit
    {
        static Texture2D s_CustomIcon;
        static AgentTreeEditorInit()
        {
            s_CustomIcon = EditorResources.LoadTexture("AgentTree.png");
            EditorApplication.projectWindowItemOnGUI += OnProjectWindowItemGUI;
        }
        //-----------------------------------------------------
        static void OnProjectWindowItemGUI(string guid, Rect selectionRect)
        {
            if (s_CustomIcon == null) return;
            string path = AssetDatabase.GUIDToAssetPath(guid);
            var obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
            if (obj is AgentTreeObject)
            {
                if (EditorGUIUtility.GetIconForObject(obj) != s_CustomIcon)
                    EditorGUIUtility.SetIconForObject(obj, s_CustomIcon);
            }
        }
    }
}
#endif