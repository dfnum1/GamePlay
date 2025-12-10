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
using Color = UnityEngine.Color;

namespace Framework.AT.Editor
{
    public class AgentTreeWindow : EditorWindowBase
    {
        GUIStyle                        m_TileStyle;
        AgentTreeData                   m_pATData = null;
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
        public static AgentTreeWindow Open(AgentTreeData atData, System.Object pObject)
        {
            AgentTreeWindow[] editors = EditorWindow.FindObjectsOfType<AgentTreeWindow>();
            if(editors!=null)
            {
                for(int i =0; i < editors.Length; ++i)
                {
                    if (editors[i].m_pATData == atData)
                    {
                        editors[i].m_pATData = atData;
                        editors[i].OnChangeSelect(pObject); 
                        editors[i].Focus();
                        return editors[i];
                    }
                }
            }
            AgentTreeWindow window = EditorWindow.GetWindow<AgentTreeWindow>();
            window.titleContent = new GUIContent("行为树");
            window.m_pCurrentObj = pObject;
            window.m_pATData = atData;
            window.OnChangeSelect(pObject);
            return window;
        }
        //--------------------------------------------------------
        public AgentTreeData GetATData()
        {
            return m_pATData;
        }
        //--------------------------------------------------------
        protected override void OnInnerEnable()
        {
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
        }
        //--------------------------------------------------------
        protected override void OnInnerGUI()
        {
        }
        //-----------------------------------------------------
        protected override void OnInnerEvent(Event evt)
        {
        }
    }
}

#endif