#if UNITY_EDITOR
using UnityEngine;

namespace Framework.State.Editor
{
    public class WarEditorUtil
    {
        static string ms_installPath = null;
        public static string BuildInstallPath()
        {
            if (string.IsNullOrEmpty(ms_installPath))
            {
                var scripts = UnityEditor.AssetDatabase.FindAssets("t:Script BattleWorldEditor");
                if (scripts.Length > 0)
                {
                    ms_installPath = System.IO.Path.GetDirectoryName(UnityEditor.AssetDatabase.GUIDToAssetPath(scripts[0])).Replace("\\", "/");
                }
            }
            return ms_installPath;
        }
        //-----------------------------------------------------
        private static GUIStyle ms_PanelTileStyle = null;
        public static GUIStyle panelTitleStyle
        {
            get
            {
                if (ms_PanelTileStyle == null)
                {
                    ms_PanelTileStyle = new GUIStyle();
                    ms_PanelTileStyle.fontSize = 13;
                    ms_PanelTileStyle.normal.textColor = Color.white;
                    ms_PanelTileStyle.alignment = TextAnchor.MiddleCenter;

                }
                return ms_PanelTileStyle;
            }
        }
    }
}
#endif