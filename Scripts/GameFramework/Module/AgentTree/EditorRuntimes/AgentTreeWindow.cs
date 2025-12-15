/********************************************************************
生成日期:	06:30:2025
类    名: 	AgentTreeWindow
作    者:	HappLI
描    述:	行为树编辑器窗口
*********************************************************************/
#if UNITY_EDITOR
using Framework.AT.Runtime;
using Framework.ED;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Color = UnityEngine.Color;

namespace Framework.AT.Editor
{
    public class AgentTreeWindow : EditorWindowBase
    {
        GUIStyle                        m_TileStyle;
        AgentTree                       m_pAT;
        AgentTreeData                   m_pATData = null;
        System.Action<AgentTreeData>    m_onSave = null;
        //--------------------------------------------------------
        [MenuItem("Tools/GamePlay/蓝图脚本")]
        public static void Open()
        {
            if (EditorApplication.isCompiling)
            {
                EditorUtility.DisplayDialog("警告", "请等待编辑器完成编译再执行此功能", "确定");
                return;
            }
            AgentTreeWindow window = EditorWindow.GetWindow<AgentTreeWindow>();
            window.titleContent = new GUIContent("蓝图脚本");
        }
        //--------------------------------------------------------
        public static AgentTreeWindow Open(AgentTreeData atData, System.Object pObject, AgentTree pAT = null, System.Action<AgentTreeData> OnSave = null)
        {
            var editors = Resources.FindObjectsOfTypeAll<AgentTreeWindow>();
            if (editors!=null)
            {
                for(int i =0; i < editors.Length; ++i)
                {
                    if (editors[i].m_pATData == atData)
                    {
                        editors[i].m_pAT = pAT;
                        editors[i].m_onSave = OnSave;
                        editors[i].m_pATData = atData;
                        editors[i].OnChangeSelect(pObject); 
                        editors[i].Focus();
                        return editors[i];
                    }
                }
            }
            AgentTreeWindow window = EditorWindow.GetWindow<AgentTreeWindow>();
            window.titleContent = new GUIContent("蓝图脚本");
            window.m_pATData = atData;
            window.m_pAT = pAT;
            window.m_onSave = OnSave;
            window.OnChangeSelect(pObject);
            return window;
        }
        //--------------------------------------------------------
        public AgentTreeData GetATData()
        {
            return m_pATData;
        }
        //-----------------------------------------------------
        public void SetAT(AgentTree AT)
        {
            m_pAT = AT;
        }
        //-----------------------------------------------------
        public AgentTree GetAT()
        {
            return m_pAT;
        }
        //--------------------------------------------------------
        protected override void OnInnerEnable()
        {
            Editor.AgentTreeUtil.EditorInit();
            //    m_CutsceneManager = new CutsceneManager();
            //    m_pCutscene = new CutsceneInstance(m_CutsceneManager);
            //    m_pCutscene.SetEditorMode(true, this);
            //    m_pCutscene.SetGUID(DataUtils.EDITOR_INSTANCE_ID-1);
            //    DataUtils.AddRuntimeCutsceneInstance(m_pCutscene);
            this.minSize = new Vector2(600, 400);
            this.wantsMouseMove = true;
            this.wantsMouseEnterLeaveWindow = true;
            this.autoRepaintOnSceneChange = true;
            this.wantsLessLayoutEvents = true;

            m_TileStyle = new GUIStyle();
            m_TileStyle.fontSize = 20;
            m_TileStyle.normal.textColor = Color.white;
            m_TileStyle.alignment = TextAnchor.MiddleCenter;
        }
        //--------------------------------------------------------
        public Rect PreviewRect
        {
            get
            {
                return new Rect(0, 0, position.width, position.height);
            }
        }
        //--------------------------------------------------------
        protected override void OnInnerDisable()
        {
            base.OnInnerDisable();
       //     if(m_pCutscene!=null)
       //     {
       //         DataUtils.RemoveRuntimeCutsceneInstance(m_pCutscene);
       //         m_pCutscene.Destroy();
       //         m_pCutscene = null;
       //     }
        }
        //--------------------------------------------------------
        protected override void OnInnerUpdate()
        {
        }
        /*
        //--------------------------------------------------------
        public CutsceneManager GetCutsceneManager()
        {
            return m_CutsceneManager;
        }
        //--------------------------------------------------------
        public CutsceneInstance GetCutsceneInstance()
        {
            return m_pCutscene;
        }*/
        //--------------------------------------------------------
        public GUIStyle TileStyle
        {
            get { return m_TileStyle; }
        }
        //--------------------------------------------------------
        public override void SaveChanges()
        {
            base.SaveChanges();
            if (m_pAT != null)
            {
                bool bEnable = m_pAT.IsEnable();
                bool bStated = m_pAT.IsStarted();
                m_pAT.Create(m_pATData);
                if(bEnable) m_pAT.Enable(bEnable);
                if (bStated) m_pAT.Start();
            }
            if (m_onSave != null)
                m_onSave(m_pATData);
        }
        //--------------------------------------------------------
        internal bool OnGraphViewEvent(Event evt)
        {
            if (m_pAT != null)
                return m_pAT.EditorKeyEvent(evt);
            return false;
        }
        //--------------------------------------------------------
        protected override void OnInnerGUI()
        {
        }
        //-----------------------------------------------------
        protected override void OnInnerEvent(Event evt)
        {
            if(m_pAT!=null)
            {
                if(evt.type == EventType.KeyUp || evt.type == EventType.KeyDown)
                {
                    if (m_pAT.EditorKeyEvent(evt))
                    {
                        evt.Use();
                    }
                }
            }
        }
        //-----------------------------------------------------
        void OnNodeExecute(AgentTree pAT, BaseNode pNode)
        {
            var logics = GetLogics<AAgentTreeLogic>();
            foreach(var db in logics)
            {
                db.OnNotifyExecutedNode(pAT, pNode);
            }
        }
        //-----------------------------------------------------
        internal static void OnAgentTreeNodeExecute(AgentTree pAT, BaseNode pNode)
        {
            if (pAT == null) return;
            var editors = Resources.FindObjectsOfTypeAll<AgentTreeWindow>();
            if (editors != null)
            {
                for (int i = 0; i < editors.Length; ++i)
                {
                    if (editors[i].m_pATData == pAT.GetData())
                    {
                        editors[i].OnNodeExecute(pAT, pNode);
                    }
                }
            }
        }
    }
}

#endif