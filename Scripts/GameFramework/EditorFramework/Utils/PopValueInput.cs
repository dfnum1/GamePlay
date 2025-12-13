/********************************************************************
生成日期:		11:07:2020
类    名: 	PopValueInput
作    者:	HappLI
描    述:	输入框
*********************************************************************/
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Framework.ED
{
    public class PopValueInput : EditorWindow
    {
        private object m_binder;
        private object m_value;
        static PopValueInput ms_Instance = null;

        public System.Action<object, object> editCallback;
        //--------------------------------------------------------
        public static void Show(string title, object binder, object value, System.Action<object, object> callback = null)
        {
            if (ms_Instance != null)
            {
                ms_Instance.editCallback = callback;
                ms_Instance.m_binder = binder;
                ms_Instance.m_value = value;
                ms_Instance.titleContent = new GUIContent(title);

                if (value != null && value.GetType() == typeof(string))
                    ms_Instance.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 80);
                else
                    ms_Instance.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 50);

                ms_Instance.ShowUtility();
                ms_Instance.Focus();
                return;
            }
            var window = ScriptableObject.CreateInstance<PopValueInput>();
            window.m_binder = binder;
            window.m_value = value;
            window.editCallback = callback;
            ms_Instance = window;
            window.titleContent = new GUIContent(title);
            if (value!=null && value.GetType() == typeof(string))
                window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 80);
            else
                window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 50);

            window.ShowUtility();
            window.Focus();
        }
        //--------------------------------------------------------
        void OnLostFocus()
        {
            this.Focus(); // 失去焦点时重新获取
        }
        //--------------------------------------------------------
        private void OnDisable()
        {
            ms_Instance = null;
        }
        //--------------------------------------------------------
        void OnGUI()
        {
            if(m_value!=null)
            {
                if(m_value.GetType() == typeof(bool))
                {
                    m_value = EditorGUILayout.Toggle((bool)m_value);
                }
                else if (m_value.GetType() == typeof(byte))
                {
                    m_value = (byte)EditorGUILayout.IntField((byte)m_value);
                }
                else if (m_value.GetType() == typeof(short))
                {
                    m_value = (short)EditorGUILayout.IntField((short)m_value);
                }
                else if (m_value.GetType() == typeof(ushort))
                {
                    m_value = (ushort)EditorGUILayout.IntField((ushort)m_value);
                }
                else if (m_value.GetType() == typeof(int))
                {
                    m_value = EditorGUILayout.IntField((int)m_value);
                }
                else if (m_value.GetType() == typeof(uint))
                {
                    m_value = (uint)EditorGUILayout.IntField((int)m_value);
                }
                else if (m_value.GetType() == typeof(float))
                {
                    m_value = (float)EditorGUILayout.FloatField((float)m_value);
                }
                else if (m_value.GetType() == typeof(double))
                {
                    m_value = (double)EditorGUILayout.DoubleField((double)m_value);
                }
                else if (m_value.GetType() == typeof(long))
                {
                    m_value = (long)EditorGUILayout.LongField((long)m_value);
                }
                else if (m_value.GetType() == typeof(Rect))
                {
                    m_value = (Rect)EditorGUILayout.RectField((Rect)m_value);
                }
                else if (m_value.GetType() == typeof(RectInt))
                {
                    m_value = (RectInt)EditorGUILayout.RectIntField((RectInt)m_value);
                }
                else if (m_value.GetType() == typeof(Color))
                {
                    m_value = (Color)EditorGUILayout.ColorField((Color)m_value);
                }
                else if (m_value.GetType() == typeof(Vector2))
                {
                    m_value = (Vector2)EditorGUILayout.Vector2Field("",(Vector2)m_value);
                }
                else if (m_value.GetType() == typeof(Vector2Int))
                {
                    m_value = (Vector2Int)EditorGUILayout.Vector2IntField("",(Vector2Int)m_value);
                }
                else if (m_value.GetType() == typeof(Vector3))
                {
                    m_value = (Vector3)EditorGUILayout.Vector3Field("", (Vector3)m_value);
                }
                else if (m_value.GetType() == typeof(Vector2Int))
                {
                    m_value = (Vector3Int)EditorGUILayout.Vector3IntField("", (Vector3Int)m_value);
                }
                else if (m_value.GetType() == typeof(Vector4))
                {
                    m_value = (Vector4)EditorGUILayout.Vector4Field("", (Vector4)m_value);
                }
                else if (m_value.GetType() == typeof(Bounds))
                {
                    m_value = (Bounds)EditorGUILayout.BoundsField("", (Bounds)m_value);
                }
                else if (m_value.GetType() == typeof(BoundsInt))
                {
                    m_value = (BoundsInt)EditorGUILayout.BoundsIntField("", (BoundsInt)m_value);
                }
                else if (m_value.GetType().IsSubclassOf(typeof(UnityEngine.Object)))
                {
                    m_value = (UnityEngine.Object)EditorGUILayout.ObjectField("", (UnityEngine.Object)m_value, m_value.GetType(), true);
                }
                else if (m_value.GetType() == typeof(string))
                {
                    m_value = EditorGUILayout.TextField("", m_value.ToString(), GUILayout.Height(40));
                }
                else
                {
                    EditorGUILayout.LabelField(m_value.GetType().ToString() + " 不支持编辑");
                }
            }
            else
            {
                EditorGUILayout.LabelField(" 不支持编辑");
            }
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("确定"))
            {
                if (editCallback != null) editCallback(m_binder, m_value);
                    Close();
            }
            if (GUILayout.Button("取消"))
            {
                Close();
            }
            GUILayout.EndHorizontal();
        }
    }
}
#endif