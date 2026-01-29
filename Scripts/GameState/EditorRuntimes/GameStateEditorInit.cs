/********************************************************************
生成日期:	06:30:2025
类    名: 	CutsceneEditor
作    者:	HappLI
描    述:	过场动作编辑器
*********************************************************************/
#if UNITY_EDITOR
using Framework.ED;
using Framework.State.Runtime;
using System.IO;
using UnityEditor;
using UnityEngine;
using Color = UnityEngine.Color;

namespace Framework.State.Editor
{
    [InitializeOnLoad]
    public class GameStateEditorInit
    {
        static Texture2D s_CustomIcon;
        static GameStateEditorInit()
        {
            gameWorldIcon();
            StateEditorUtil.ReInitTypes();
            EditorApplication.projectWindowItemOnGUI += OnProjectWindowItemGUI;

            //! auto code
            // 你可以自定义生成文件路径
            string path = EditorPreferences.GetSettings().generatorCodePath;
            if (!string.IsNullOrEmpty(path))
            {
                AutoCodeGenerator.AutoCode(path);
            }
            else
            {
                AutoCodeGenerator.AutoCode(null);
                Debug.LogError("请先在编辑器[Edit -> Preferences... -> GameWorld]设置中配置代码生成路径");
                return;
            }
        }
        //-----------------------------------------------------
        public static Texture2D gameWorldIcon()
        {
            if(s_CustomIcon == null) s_CustomIcon = Framework.ED.EditorUtils.LoadEditorResource<Texture2D>("GameWorld/gameworld.png");
            return s_CustomIcon;
        }
        //-----------------------------------------------------
        static void OnProjectWindowItemGUI(string guid, Rect selectionRect)
        {
            if (s_CustomIcon == null) return;
            string path = AssetDatabase.GUIDToAssetPath(guid);
            var obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
            if (obj is AGameWorldObject)
            {
                //      Rect iconRect = new Rect(selectionRect.x + 2, selectionRect.y + 2, 16, 16);
                //       GUI.DrawTexture(iconRect, s_CustomIcon, ScaleMode.ScaleToFit);
                if (EditorGUIUtility.GetIconForObject(obj) != s_CustomIcon)
                    EditorGUIUtility.SetIconForObject(obj, s_CustomIcon);
            }
        }
    }
}

#endif