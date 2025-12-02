#if UNITY_EDITOR
using Framework.ActorSystem.Runtime;
using Framework.ED;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Framework.ActorSystem.Editor
{
    public static class EditorUtil
    {
        //-----------------------------------------------------
        public static int CombineHash(this int h1, int h2)
        {
            return h1 ^ (int)(h2 + 0x9e3779b9 + (h1 << 6) + (h1 >> 2)); // Similar to c++ boost::hash_combine
        }
        //-----------------------------------------------------
        public static bool DrawButton(Texture image, string tooltip, GUIStyle style, float width =0)
        {
            if(width<=0)
                return GUILayout.Button(new GUIContent(image, tooltip),  style);
            else
                return GUILayout.Button(new GUIContent(image, tooltip),style, GUILayout.Width(width));
        }
        //-----------------------------------------------------
        public static bool DrawToggle(bool value, Texture image, string tooltip, GUIStyle style, float width = 0)
        {
            if (width <= 0)
                return GUILayout.Toggle(value, new GUIContent(image, tooltip), style);
            else
                return GUILayout.Toggle(value, new GUIContent(image, tooltip), style, GUILayout.Width(width));
        }
        //-----------------------------------------------------
        public static void DrawPolygonAA(Color color, Vector3[] vertices)
        {
            var prevColor = Handles.color;
            Handles.color = color;
            Handles.DrawAAConvexPolygon(vertices);
            Handles.color = prevColor;
        }
        //-----------------------------------------------------
        public static void DrawLine(Vector3 p1, Vector3 p2, Color color)
        {
            var c = Handles.color;
            Handles.color = color;
            Handles.DrawLine(p1, p2);
            Handles.color = c;
        }
        //-----------------------------------------------------
        public static void DrawDottedLine(Vector3 p1, Vector3 p2, float segmentsLength, Color col)
        {
            UIDrawUtils.ApplyWireMaterial();

            GL.Begin(GL.LINES);
            GL.Color(col);

            var length = Vector3.Distance(p1, p2); // ignore z component
            var count = Mathf.CeilToInt(length / segmentsLength);
            for (var i = 0; i < count; i += 2)
            {
                GL.Vertex((Vector3.Lerp(p1, p2, i * segmentsLength / length)));
                GL.Vertex((Vector3.Lerp(p1, p2, (i + 1) * segmentsLength / length)));
            }

            GL.End();
        }
        //-----------------------------------------------------
        public static void ShadowLabel(Rect rect, GUIContent content, GUIStyle style, Color textColor, Color shadowColor)
        {
            var shadowRect = rect;
            shadowRect.xMin += 2.0f;
            shadowRect.yMin += 2.0f;
            style.normal.textColor = shadowColor;
            style.hover.textColor = shadowColor;
            GUI.Label(shadowRect, content, style);

            style.normal.textColor = textColor;
            style.hover.textColor = textColor;
            GUI.Label(rect, content, style);
        }
        //-----------------------------------------------------
        public static uint DrawActionAndTag(uint stateAndTag, string name = null, bool isHorizontal = false, GUILayoutOption[] op = null)
        {
            GetActionTypeAndTag(stateAndTag, out var eType, out var tag);
            if (isHorizontal)
                GUILayout.BeginHorizontal(op);
            eType = (EActionStateType)InspectorDrawUtil.PopEnum(name, eType, null);
            tag = (ushort)EditorGUILayout.IntField((int)tag, GUILayout.Width(80));
            stateAndTag = BuildActionKey(eType, tag);
            if (isHorizontal)
                GUILayout.EndHorizontal();
            return stateAndTag;
        }
        //-----------------------------------------------------
        public static EActionStateType DrawActionNoTag(EActionStateType type, string name = null, GUILayoutOption[] op = null)
        {
            type = (EActionStateType)InspectorDrawUtil.PopEnum(name, type, null);
            return type;
        }
        //-----------------------------------------------------
        public static void GetActionTypeAndTag(uint key, out EActionStateType type, out ushort tag)
        {
            type = (EActionStateType)(int)(key >> 16);
            tag = (ushort)(key & 0xffff);
        }
        //-----------------------------------------------------
        public static uint BuildActionKey(EActionStateType type, uint tag)
        {
            return ((uint)type << 16) | tag;
        }
        //-----------------------------------------------------
        public static void Destroy(GameObject pIns)
        {
            if (pIns == null) return;
            if (Application.isPlaying) GameObject.Destroy(pIns);
            else GameObject.DestroyImmediate(pIns);
        }
    }
#if USE_ACTORSYSTEM
    public static class AssetUtil
    {
        static string ms_installPath = null;
        public static string BuildInstallPath()
        {
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
        public static Texture2D GetFloorTexture()
        {
            string install = BuildInstallPath();
            if (string.IsNullOrEmpty(install)) return null;
            string groundPath = install + "/ground.png";
            if (!File.Exists(groundPath))
                return null;
            return UnityEditor.AssetDatabase.LoadAssetAtPath<Texture2D>(groundPath);
        }
    }
#endif // USE_ACTORSYSTEM
    struct GUIViewportScope : IDisposable
    {
        bool m_open;
        public GUIViewportScope(Rect position)
        {
            m_open = false;
            if (Event.current.type == EventType.Repaint || Event.current.type == EventType.Layout)
            {
                GUI.BeginClip(position, -position.min, Vector2.zero, false);
                m_open = true;
            }
        }

        public void Dispose()
        {
            CloseScope();
        }

        void CloseScope()
        {
            if (m_open)
            {
                GUI.EndClip();
                m_open = false;
            }
        }
    }
}
#endif