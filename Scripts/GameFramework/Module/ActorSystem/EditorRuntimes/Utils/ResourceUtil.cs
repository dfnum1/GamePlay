#if UNITY_EDITOR
using Framework.ActorSystem.Runtime;
using Framework.ED;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Framework.ActorSystem.Editor
{
    public static class AssetUtil
    {
        static string ms_installPath = null;
        public static string BuildInstallPath()
        {
            ms_installPath = Framework.ED.EditorUtils.GetInstallEditorResourcePath();
            if (string.IsNullOrEmpty(ms_installPath))
            {
                var scripts = UnityEditor.AssetDatabase.FindAssets("t:Script ActorEditorWindow");
                if (scripts.Length > 0)
                {
                    if (scripts.Length > 0)
                    {
                        string installPath = System.IO.Path.GetDirectoryName(UnityEditor.AssetDatabase.GUIDToAssetPath(scripts[0])).Replace("\\", "/");

                        string installPath1 = Path.Combine(installPath, "EditorResources").Replace("\\", "/");
                        if (!System.IO.Directory.Exists(installPath1))
                        {
                            installPath1 = Path.Combine(Path.GetDirectoryName(installPath), "EditorResources").Replace("\\", "/");
                        }
                        if (System.IO.Directory.Exists(installPath1))
                        {
                            ms_installPath = installPath1;
                        }
                    }
                }
            }
            return ms_installPath;
        }
        //-----------------------------------------------------
        public static Texture2D LoadTexture(string name)
        {
            if(!name.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                name += ".png";
            string install = BuildInstallPath();
            if (string.IsNullOrEmpty(install)) return null;
            string groundPath = install + "/"+ name;
            if (!File.Exists(groundPath))
                return null;
            return UnityEditor.AssetDatabase.LoadAssetAtPath<Texture2D>(groundPath);
        }
    }
    //-----------------------------------------------------
    //!ActorSystemEngineInit
    //-----------------------------------------------------
    [InitializeOnLoad]
    public class ActorSystemEngineInit
    {
        static Texture2D s_CustomIcon;
        static Texture2D s_ProjecitleIcon;
        static Texture2D s_ActorAttriIcon;
        static ActorSystemEngineInit()
        {
            s_ActorAttriIcon = AssetUtil.LoadTexture("ActorSystem/actor_attris.png");
            s_ProjecitleIcon = AssetUtil.LoadTexture("ActorSystem/Projectile.png");
            EditorApplication.projectWindowItemOnGUI += OnProjectWindowItemGUI;

        }
        //-----------------------------------------------------
        static void OnProjectWindowItemGUI(string guid, Rect selectionRect)
        {
            if (s_ProjecitleIcon == null && s_CustomIcon==null) return;
            string path = AssetDatabase.GUIDToAssetPath(guid);
            var obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
            if (obj is AProjectileDatas)
            {
                if(s_ProjecitleIcon!=null)
                {
                    if (EditorGUIUtility.GetIconForObject(obj) != s_ProjecitleIcon)
                        EditorGUIUtility.SetIconForObject(obj, s_ProjecitleIcon);
                }
            }
            else if (obj is AActorAttrDatas)
            {
                if (s_ActorAttriIcon != null)
                {
                    if (EditorGUIUtility.GetIconForObject(obj) != s_ActorAttriIcon)
                        EditorGUIUtility.SetIconForObject(obj, s_ActorAttriIcon);
                }
            }
        }
    }
}
#endif