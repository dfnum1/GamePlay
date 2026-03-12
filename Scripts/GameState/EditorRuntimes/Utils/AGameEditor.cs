/********************************************************************
生成日期:		11:07:2025
类    名: 	AGameEditor
作    者:	HappLI
描    述:	游戏数据体编辑器
*********************************************************************/
#if UNITY_EDITOR
using Framework.Core;
using UnityEditor;
using Framework.ED;
using UnityEngine;
using Framework.State.Runtime;

namespace Framework.State.Editor
{
    //------------------------------------------------------------
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true)]
    public class StateCustomEditorAttribute : System.Attribute
    {
#if UNITY_EDITOR
        public System.Type type;
#endif
        public StateCustomEditorAttribute(System.Type type)
        {
#if UNITY_EDITOR
            this.type = type;
#endif
        }
    }
    //------------------------------------------------------------
    public class AGameEditor
    {
        public delegate void OnToolBarMenu(System.Object data);
        protected AGameCfgData m_pData;
        protected EditorWindow m_pEditor;
        private ED.UndoHandler m_Undo;
        internal System.Action<string, OnToolBarMenu, System.Object> OnAddToolBar;
        internal System.Action<string> OnRemoveToolBar;
        //------------------------------------------------
        internal void SetData(AGameCfgData pData)
        {
            m_pData = pData;
        }
        //------------------------------------------------
        public AFramework GetFramework()
        {
            if (m_pEditor == null || !(m_pEditor is EditorWindowBase))
                return null;
            return ((EditorWindowBase)m_pEditor).GetEditorGame();
        }
        //------------------------------------------------
        internal void SetEditor(EditorWindow pData)
        {
            m_pEditor = pData;
        }
        //------------------------------------------------
        public void ShowNotification(string message, float duration = 1)
        {
            if (m_pEditor == null || !(m_pEditor is EditorWindowBase))
                return;
            ((EditorWindowBase)m_pEditor).ShowNotification(message, duration);
        }
        //------------------------------------------------
        public void ShowNotificationWarning(string message, float duration = 1)
        {
            if (m_pEditor == null || !(m_pEditor is EditorWindowBase))
                return;
            ((EditorWindowBase)m_pEditor).ShowNotificationWarning(message, duration);
        }
        //------------------------------------------------
        public void ShowNotificationError(string message, float duration = 1)
        {
            if (m_pEditor == null || !(m_pEditor is EditorWindowBase))
                return;
            ((EditorWindowBase)m_pEditor).ShowNotificationError(message, duration);
        }
        //------------------------------------------------
        internal void SetUndoHandle(ED.UndoHandler handler)
        {
            m_Undo = handler;
        }
        //------------------------------------------------
        public T GetData<T>() where T : AGameCfgData
        {
            return m_pData as T;
        }
        //------------------------------------------------
        public virtual void OnInspectorGUI()
        {
            Framework.ED.InspectorDrawUtil.DrawProperty(m_pData, null);
        }
        //------------------------------------------------
        public virtual void OnRefreshData(object pData)
        {

        }
        //------------------------------------------------
        protected void RegisterUndo(System.Object pData, bool bDirty = false)
        {
            if (m_Undo == null) return;
            m_Undo.RegisterUndoData(pData);
        }
        //------------------------------------------------
        protected void AddToolBar(string name, OnToolBarMenu func, System.Object pData = null)
        {
            if (OnAddToolBar != null) OnAddToolBar(name, func, pData);
        }
        //------------------------------------------------
        protected void RemoveToolBar(string name)
        {
            if (OnRemoveToolBar != null) OnRemoveToolBar(name);
        }
        //------------------------------------------------
        public virtual void OnUndoAction(System.Object pObj, bool bDirty)
        {

        }
        //------------------------------------------------
        public virtual void OnUpdate(float deltaTime)
        {

        }
        //------------------------------------------------
        public virtual void OnSceneView(SceneView view)
        {
        }
        //------------------------------------------------
        public virtual void OnEvent(Event evt)
        {

        }
        //------------------------------------------------
        public virtual void OnGUI(Rect rect)
        {

        }
        //------------------------------------------------
        public virtual bool OnInspectorGUI(Rect rect)
        {
            return false;
        }
        //------------------------------------------------
        public virtual void OnAssetMenu(GenericMenu menu)
        {
        }
        //------------------------------------------------
        public virtual void OnPreviewEnable(Framework.ED.TargetPreview preview)
        {

        }
        //------------------------------------------------
        public virtual void OnPreviewDisable(Framework.ED.TargetPreview preview)
        {

        }
        //------------------------------------------------
        public virtual void OnPreviewView(Framework.ED.TargetPreview preview)
        {
        }
        //------------------------------------------------
        public virtual void OnSaveChanges()
        {

        }
        //------------------------------------------------
        public virtual void OnRemoveWorldItem(IGameWorldItem item)
        {

        }
        //------------------------------------------------
        protected void DestroyObj(UnityEngine.Object obj)
        {
            if (obj == null) return;
            Framework.ED.EditorUtils.Destroy(obj);
        }
        //------------------------------------------------
        protected Mesh CreateSquareMesh(float size = 1f)
        {
            Mesh mesh = new Mesh();
            float half = size * 0.5f;
            Vector3[] vertices = new Vector3[]
            {
                new Vector3(-half, 0, -half),
                new Vector3(-half, 0,  half),
                new Vector3( half, 0,  half),
                new Vector3( half, 0, -half)
            };
            int[] triangles = new int[]
            {
                0, 1, 2,
                0, 2, 3
            };
            Vector2[] uv = new Vector2[]
            {
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(1, 0)
            };
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;
            mesh.RecalculateNormals();
            return mesh;
        }
        //------------------------------------------------
        protected Material CreateTransparentMaterial()
        {
            Shader shader = null;
            bool isURP = UnityEngine.Rendering.GraphicsSettings.currentRenderPipeline != null &&
                         UnityEngine.Rendering.GraphicsSettings.currentRenderPipeline.GetType().Name.Contains("Universal");

            if (isURP)
            {
                // URP半透Shader
                shader = Shader.Find("Universal Render Pipeline/Unlit");
            }
            else
            {
                // 标准半透Shader
                shader = Shader.Find("Unlit/Transparent");
                if (shader == null)
                    shader = Shader.Find("Legacy Shaders/Transparent/Diffuse");
            }

            Material pMaterial = new Material(shader);
            pMaterial.color = new Color(0, 1, 0, 0.3f); // 半透明绿色
            pMaterial.SetFloat("_Surface", 1); // 如果是URP Unlit，设置为透明
            pMaterial.SetFloat("_Blend", 3);   // 混合模式（可选，视Shader而定）
            pMaterial.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);   // 混合模式（可选，视Shader而定）
            pMaterial.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);   // 混合模式（可选，视Shader而定）
            return pMaterial;
        }
    }
}
#endif