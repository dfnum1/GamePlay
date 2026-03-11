/********************************************************************
生成日期:		11:03:2020
类    名: 	UIDrawUtils
作    者:	HappLI
描    述:	UI 绘制工具类
*********************************************************************/

#if UNITY_EDITOR
using System;
using System.Reflection;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

namespace Framework.ED
{
    public class UIDrawUtils
    {
        private static System.Reflection.MethodInfo ms_ApplayWireMateral;
        private static System.Type ms_realType;
        private static System.Reflection.PropertyInfo ms_property_handleWireMaterial;
        //------------------------------------------------------
        private static void InitType()
        {
            if (ms_realType == null)
            {
                ms_realType = System.Reflection.Assembly.GetAssembly(typeof(UnityEditor.Editor)).GetType("UnityEditor.HandleUtility");
                ms_property_handleWireMaterial = ms_realType.GetProperty("handleWireMaterial", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
                ms_ApplayWireMateral = ms_realType.GetMethod("ApplyWireMaterial", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static, null, new System.Type[0], null);
            }
        }
        //------------------------------------------------------
        public static Material handleWireMaterial
        {
            get
            {
                InitType();
                return (ms_property_handleWireMaterial.GetValue(null, null) as Material);
            }
        }
        //------------------------------------------------------
        private static Texture2D ms_gridTexture;
        public static Texture2D gridTexture
        {
            get
            {
                if (ms_gridTexture == null) ms_gridTexture = GenerateGridTexture(Color.gray, new Color(0.3f, 0.3f, 0.3f, 1f));
                return ms_gridTexture;
            }
        }
        //------------------------------------------------------
        private static Texture2D ms_crossTexture;
        public static Texture2D crossTexture
        {
            get
            {
                if (ms_crossTexture == null) ms_crossTexture = GenerateCrossTexture(Color.gray);
                return ms_crossTexture;
            }
        }
        //------------------------------------------------------
        public static Texture2D GenerateGridTexture(Color line, Color bg)
        {
            Texture2D tex = new Texture2D(64, 64);
            Color[] cols = new Color[64 * 64];
            for (int y = 0; y < 64; y++)
            {
                for (int x = 0; x < 64; x++)
                {
                    Color col = bg;
                    if (y % 16 == 0 || x % 16 == 0) col = Color.Lerp(line, bg, 0.65f);
                    if (y == 63 || x == 63) col = Color.Lerp(line, bg, 0.35f);
                    cols[(y * 64) + x] = col;
                }
            }
            tex.SetPixels(cols);
            tex.wrapMode = TextureWrapMode.Repeat;
            tex.filterMode = FilterMode.Bilinear;
            tex.name = "Grid";
            tex.Apply();
            return tex;
        }
        //------------------------------------------------------
        public static Texture2D GenerateCrossTexture(Color line)
        {
            Texture2D tex = new Texture2D(64, 64);
            Color[] cols = new Color[64 * 64];
            for (int y = 0; y < 64; y++)
            {
                for (int x = 0; x < 64; x++)
                {
                    Color col = line;
                    if (y != 31 && x != 31) col.a = 0;
                    cols[(y * 64) + x] = col;
                }
            }
            tex.SetPixels(cols);
            tex.wrapMode = TextureWrapMode.Clamp;
            tex.filterMode = FilterMode.Bilinear;
            tex.name = "Grid";
            tex.Apply();
            return tex;
        }
        //------------------------------------------------------
        public static void DrawColorBox(Rect rect, Color color)
        {
            handleWireMaterial.SetPass(0);
            GL.Color(color);
            GL.Begin(1);
            GL.Color(color);
            GL.Vertex3(rect.xMin, rect.yMin, 0f);
            GL.Color(color);
            GL.Vertex3(rect.xMin, rect.yMax, 0f);

            GL.Color(color);
            GL.Vertex3(rect.xMax, rect.yMin, 0f);
            GL.Color(color);
            GL.Vertex3(rect.xMax, rect.yMax, 0f);

            GL.Color(color);
            GL.Vertex3(rect.xMin, rect.yMin, 0f);
            GL.Color(color);
            GL.Vertex3(rect.xMax, rect.yMin, 0f);

            GL.Color(color);
            GL.Vertex3(rect.xMin, rect.yMax, 0f);
            GL.Color(color);
            GL.Vertex3(rect.xMax, rect.yMax, 0f);
            GL.End();
        }
        //------------------------------------------------------
        public static void DrawColorLine(Vector2 begin, Vector2 end, Color color)
        {
            handleWireMaterial.SetPass(0);
            GL.Begin(1);
            GL.Color(color);
            GL.Vertex3(begin.x, begin.y, 0);
            GL.Color(color);
            GL.Vertex3(end.x, end.y, 0);
            GL.End();
        }
        //------------------------------------------------------
        public static Texture2D ColorToTexture(int pxSize, Color col)
        {
            Color[] texCols = new Color[pxSize * pxSize];
            for (int px = 0; px < pxSize * pxSize; px++)
                texCols[px] = col;
            Texture2D tex = new Texture2D(pxSize, pxSize);
            tex.SetPixels(texCols);
            tex.Apply();
            return tex;
        }
        //------------------------------------------------------
        public static void ApplyWireMaterial()
        {
            if (ms_ApplayWireMateral != null) ms_ApplayWireMateral.Invoke(null, null);
            else handleWireMaterial.SetPass(0);
        }
        //-----------------------------------------------------
        public static void DrawLine(Vector3 start, Vector3 end, float thickness = 0.1f)
        {
#if UNITY_2020_3_OR_NEWER
            Handles.DrawLine(start, end, thickness);
#else
            Handles.DrawLine(start, end);
#endif
        }
        //-------------------------------------------
        public static void DrawCurveLineWithRail(float width, Vector2 vStart, Vector2 vEnd, Color color)
        {
            float dot = Mathf.Abs(Vector2.Dot((vEnd - vStart).normalized, Vector2.right));
            Vector2 vMid = (vEnd - vStart) * 0.05f;
            Vector2 startTan = vStart;
            startTan.x += 25f;
            startTan.y -= vMid.y;
            Vector2 endTan = vEnd;
            endTan.x -= 25f;
            endTan.y += vMid.y;
            Handles.DrawBezier(vStart, vEnd, startTan, endTan, color, null, width);
        }
        //------------------------------------------------------
        public static void DrawPolyLineWithRail(float width, Vector2 vStart, Vector2 vEnd, Vector2 offset, Color color, float arrow = 5)
        {
            Color backupcolor = Handles.color;
            Handles.color = color;
            Handles.DrawAAPolyLine(width, vStart + offset, vEnd + offset);
            Handles.DrawAAPolyLine(width, vStart, vStart + offset);
            Handles.DrawAAPolyLine(width, vEnd, vEnd + offset);
            Handles.DrawAAPolyLine(width, vEnd, vEnd + new Vector2(-arrow, -arrow));
            Handles.DrawAAPolyLine(width, vEnd, vEnd + new Vector2(arrow, -arrow));
            Handles.color = backupcolor;
        }
        //------------------------------------------------------
        public static void DrawPolyLineWithRail(float width, Vector2 vStart, Vector2 vEnd, Vector2 offset_start, Vector2 offset_end, Color color, float arrow = 5)
        {
            Color backupcolor = Handles.color;
            Handles.color = color;
            Handles.DrawAAPolyLine(width, vStart + offset_start, vEnd + offset_end);
            Handles.DrawAAPolyLine(width, vStart, vStart + offset_start);
            Handles.DrawAAPolyLine(width, vEnd, vEnd + offset_end);
            Handles.DrawAAPolyLine(width, vEnd, vEnd + new Vector2(-arrow, -arrow));
            Handles.DrawAAPolyLine(width, vEnd, vEnd + new Vector2(arrow, -arrow));
            Handles.color = backupcolor;
        }
        //------------------------------------------------------
        public static void DrawWireSemicircle(Vector3 origin, Vector3 direction, float radius, int angle, Vector3 axis)
        {
            Vector3 leftdir = Quaternion.AngleAxis(-angle / 2, axis) * direction;
            Vector3 rightdir = Quaternion.AngleAxis(angle / 2, axis) * direction;

            Vector3 currentP = origin + leftdir * radius;
            Vector3 oldP;
            if (angle != 360)
            {
                Handles.DrawLine(origin, currentP);
            }
            for (int i = 0; i < angle / 10; i++)
            {
                Vector3 dir = Quaternion.AngleAxis(10 * i, axis) * leftdir;
                oldP = currentP;
                currentP = origin + dir * radius;
                Handles.DrawLine(oldP, currentP);
            }
            oldP = currentP;
            currentP = origin + rightdir * radius;
            Handles.DrawLine(oldP, currentP);
            if (angle != 360)
            {
                Handles.DrawLine(currentP, origin);
            }

        }
        //------------------------------------------------------
        public static void DrawGrid(Rect rect, float zoom, Vector2 panOffset)
        {
            rect.position = Vector2.zero;

            Vector2 center = rect.size / 2f;
            Texture2D gridTex = gridTexture;
            Texture2D crossTex = crossTexture;

            // Offset from origin in tile units
            float xOffset = -(center.x * zoom + panOffset.x) / gridTex.width;
            float yOffset = ((center.y - rect.size.y) * zoom + panOffset.y) / gridTex.height;

            Vector2 tileOffset = new Vector2(xOffset, yOffset);

            // Amount of tiles
            float tileAmountX = Mathf.Round(rect.size.x * zoom) / gridTex.width;
            float tileAmountY = Mathf.Round(rect.size.y * zoom) / gridTex.height;

            Vector2 tileAmount = new Vector2(tileAmountX, tileAmountY);

            // Draw tiled background
            GUI.DrawTextureWithTexCoords(rect, gridTex, new Rect(tileOffset, tileAmount));
            GUI.DrawTextureWithTexCoords(rect, crossTex, new Rect(tileOffset + new Vector2(0.5f, 0.5f), tileAmount));
        }
        //-------------------------------------------
        public static void DrawHeader(string header)
        {
            InspectorDrawUtil.DrawHeader(header);
        }
        //-------------------------------------------
        public static T DrawField<T>(T curData, System.Action onDityCallback = null, params GUILayoutOption[] layoutOps)
        {
            return DrawField<T>(null, curData, onDityCallback, layoutOps);
        }
        //-------------------------------------------
        public static T DrawField<T>(string label, T curData, System.Action onDityCallback = null, params GUILayoutOption[] layoutOps)
        {
            object result = curData;
            bool changed = false;
            Type type = typeof(T);

            if (type == typeof(byte))
            {
                int v = string.IsNullOrEmpty(label) ? EditorGUILayout.IntField((byte)(object)curData, layoutOps) : EditorGUILayout.IntField(label, (byte)(object)curData, layoutOps);
                if ((byte)v != (byte)(object)curData) { changed = true; result = (byte)Mathf.Clamp(v, byte.MinValue, byte.MaxValue); }
            }
            else if (type == typeof(short))
            {
                int v = string.IsNullOrEmpty(label) ? EditorGUILayout.IntField((short)(object)curData, layoutOps) : EditorGUILayout.IntField(label, (short)(object)curData, layoutOps);
                if ((short)v != (short)(object)curData) { changed = true; result = (short)Mathf.Clamp(v, short.MinValue, short.MaxValue); }
            }
            else if (type == typeof(ushort))
            {
                int v = string.IsNullOrEmpty(label) ? EditorGUILayout.IntField((ushort)(object)curData, layoutOps) : EditorGUILayout.IntField(label, (ushort)(object)curData, layoutOps);
                if ((ushort)v != (ushort)(object)curData) { changed = true; result = (ushort)Mathf.Clamp(v, ushort.MinValue, ushort.MaxValue); }
            }
            else if (type == typeof(int))
            {
                int v = string.IsNullOrEmpty(label) ? EditorGUILayout.IntField((int)(object)curData, layoutOps) : EditorGUILayout.IntField(label, (int)(object)curData, layoutOps);
                if (v != (int)(object)curData) { changed = true; result = v; }
            }
            else if (type == typeof(uint))
            {
                long v = string.IsNullOrEmpty(label) ? EditorGUILayout.LongField((long)(uint)(object)curData, layoutOps) : EditorGUILayout.LongField(label, (long)(uint)(object)curData, layoutOps);
                if ((uint)v != (uint)(object)curData) { changed = true; result = (uint)Mathf.Max(0, v); }
            }
            else if (type == typeof(long))
            {
                long v = string.IsNullOrEmpty(label) ? EditorGUILayout.LongField((long)(object)curData, layoutOps) : EditorGUILayout.LongField(label, (long)(object)curData, layoutOps);
                if (v != (long)(object)curData) { changed = true; result = v; }
            }
            else if (type == typeof(ulong))
            {
                ulong v = string.IsNullOrEmpty(label) ? (ulong)EditorGUILayout.LongField((long)(object)curData, layoutOps) : (ulong)EditorGUILayout.LongField(label, (long)(object)curData, layoutOps);
                if (v != (ulong)(object)curData) { changed = true; result = v; }
            }
            else if (type == typeof(float))
            {
                float v = string.IsNullOrEmpty(label) ? EditorGUILayout.FloatField((float)(object)curData, layoutOps) : EditorGUILayout.FloatField(label, (float)(object)curData, layoutOps);
                if (!Mathf.Approximately(v, (float)(object)curData)) { changed = true; result = v; }
            }
            else if (type == typeof(double))
            {
                double v = string.IsNullOrEmpty(label) ? EditorGUILayout.DoubleField((double)(object)curData, layoutOps) : EditorGUILayout.DoubleField(label, (double)(object)curData, layoutOps);
                if (v != (double)(object)curData) { changed = true; result = v; }
            }
            else if (type == typeof(Vector2))
            {
                Vector2 v = string.IsNullOrEmpty(label) ? EditorGUILayout.Vector2Field(GUIContent.none, (Vector2)(object)curData, layoutOps) : EditorGUILayout.Vector2Field(label, (Vector2)(object)curData, layoutOps);
                if (v != (Vector2)(object)curData) { changed = true; result = v; }
            }
            else if (type == typeof(Vector3))
            {
                Vector3 v = string.IsNullOrEmpty(label) ? EditorGUILayout.Vector3Field(GUIContent.none, (Vector3)(object)curData, layoutOps) : EditorGUILayout.Vector3Field(label, (Vector3)(object)curData, layoutOps);
                if (v != (Vector3)(object)curData) { changed = true; result = v; }
            }
            else if (type == typeof(Vector4))
            {
                Vector4 v = string.IsNullOrEmpty(label) ? EditorGUILayout.Vector4Field(GUIContent.none, (Vector4)(object)curData, layoutOps) : EditorGUILayout.Vector4Field(label, (Vector4)(object)curData, layoutOps);
                if (v != (Vector4)(object)curData) { changed = true; result = v; }
            }
            else if (type == typeof(Rect))
            {
                Rect v = string.IsNullOrEmpty(label) ? EditorGUILayout.RectField((Rect)(object)curData, layoutOps) : EditorGUILayout.RectField(label, (Rect)(object)curData, layoutOps);
                if (v != (Rect)(object)curData) { changed = true; result = v; }
            }
            else if (type == typeof(Bounds))
            {
                Bounds v = string.IsNullOrEmpty(label) ? EditorGUILayout.BoundsField((Bounds)(object)curData, layoutOps) : EditorGUILayout.BoundsField(label, (Bounds)(object)curData, layoutOps);
                if (v != (Bounds)(object)curData) { changed = true; result = v; }
            }
            else if (type == typeof(BoundsInt))
            {
                BoundsInt v = string.IsNullOrEmpty(label) ? EditorGUILayout.BoundsIntField((BoundsInt)(object)curData, layoutOps) : EditorGUILayout.BoundsIntField(label, (BoundsInt)(object)curData, layoutOps);
                if (v != (BoundsInt)(object)curData) { changed = true; result = v; }
            }
            else if (type == typeof(Vector2Int))
            {
                Vector2Int v = string.IsNullOrEmpty(label) ? EditorGUILayout.Vector2IntField(GUIContent.none, (Vector2Int)(object)curData, layoutOps) : EditorGUILayout.Vector2IntField(label, (Vector2Int)(object)curData, layoutOps);
                if (v != (Vector2Int)(object)curData) { changed = true; result = v; }
            }
            else if (type == typeof(Vector3Int))
            {
                Vector3Int v = string.IsNullOrEmpty(label) ? EditorGUILayout.Vector3IntField(GUIContent.none, (Vector3Int)(object)curData, layoutOps) : EditorGUILayout.Vector3IntField(label, (Vector3Int)(object)curData, layoutOps);
                if (v != (Vector3Int)(object)curData) { changed = true; result = v; }
            }
            else if (type == typeof(Color))
            {
                Color v = string.IsNullOrEmpty(label) ? EditorGUILayout.ColorField((Color)(object)curData, layoutOps) : EditorGUILayout.ColorField(label, (Color)(object)curData, layoutOps);
                if (v != (Color)(object)curData) { changed = true; result = v; }
            }
            else
            {
                // fallback
                string cur = curData != null ? curData.ToString() : "";
                string last = cur;
                cur = string.IsNullOrEmpty(label)
                    ? EditorGUILayout.TextField(cur, layoutOps)
                    : EditorGUILayout.TextField(label, cur, layoutOps);
                if (cur != last) { changed = true; result = cur; }
            }

            if (changed && onDityCallback != null)
                onDityCallback();

            return (T)result;
        }
    }
    //------------------------------------------------------
    //!GUIViewportScope
    //------------------------------------------------------
    public struct GUIViewportScope : IDisposable
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
    //------------------------------------------------------
    //!GUIColorScope
    //------------------------------------------------------
    public struct GUIColorScope : IDisposable
    {
        Color m_backup;
        public GUIColorScope(Color color)
        {
            m_backup = GUI.color;
            GUI.color = color;
        }

        public void Dispose()
        {
            GUI.color = m_backup;
        }
    }
    //------------------------------------------------------
    //!GUIIndentScope
    //------------------------------------------------------
    public struct GUIIndentScope : IDisposable
    {
        public GUIIndentScope(Color color)
        {
            EditorGUI.indentLevel++;
        }

        public void Dispose()
        {
            EditorGUI.indentLevel--;
        }
    }
    //------------------------------------------------------
    //!GUILabelWidthScope
    //------------------------------------------------------
    public struct GUILabelWidthScope : IDisposable
    {
        float m_backup;
        public GUILabelWidthScope(float width)
        {
            m_backup = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = width;
        }

        public void Dispose()
        {
            EditorGUIUtility.labelWidth = m_backup;
        }
    }
    //------------------------------------------------------
    //!GUILayoutHorizontalScope
    //------------------------------------------------------
    public struct GUILayoutHorizontalScope : IDisposable
    {
        public GUILayoutHorizontalScope(params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal(options);
        }

        public void Dispose()
        {
            GUILayout.EndHorizontal();
        }
    }
    //------------------------------------------------------
    //!GUIHorizontalIndentScope
    //------------------------------------------------------
    public struct GUIHorizontalIndentScope : IDisposable
    {
        public GUIHorizontalIndentScope(params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal(options);
            EditorGUI.indentLevel++;
        }

        public void Dispose()
        {
            EditorGUI.indentLevel--;
            GUILayout.EndHorizontal();
        }
    }
    //------------------------------------------------------
    //!GUIHorizontalIndentScope
    //------------------------------------------------------
    public struct HandleZtestScope : IDisposable
    {
        UnityEngine.Rendering.CompareFunction old;
        public HandleZtestScope(UnityEngine.Rendering.CompareFunction f)
        {
            old = Handles.zTest;
            Handles.zTest = f;
        }

        public void Dispose()
        {
            Handles.zTest = old;
        }
    }
    //------------------------------------------------------
    //!HandleColorScope
    //------------------------------------------------------
    public struct HandleColorScope : IDisposable
    {
        Color old;
        public HandleColorScope(Color f)
        {
            old = Handles.color;
            Handles.color = f;
        }

        public void Dispose()
        {
            Handles.color = old;
        }
    }
}
#endif
