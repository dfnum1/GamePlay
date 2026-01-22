/********************************************************************
生成日期:		1:11:2020 10:09
类    名: 	ParticleSortLayerSetterEditor
作    者:	HappLI
描    述:	特效排序编辑
*********************************************************************/
using UnityEditor;
using UnityEngine;

namespace Framework.ED
{
    public class ParticleSortLayerSetterEditor : Framework.ED.EditorWindowBase
    {
        public static ParticleSortLayerSetterEditor Instance { protected set; get; }


        public ParticleSortLayerSetterEditorLogic m_Logic;
        private GameObject m_pRoot = null;

        public Rect previewRect = new Rect(0, 0, 0, 0);
        string m_strMsgHelp = ""; 
        //-----------------------------------------------------
        [MenuItem("GamePlay/特效排序工具")]
        private static void Init()
        {
            if (EditorApplication.isCompiling)
            {
                EditorUtility.DisplayDialog("Waring", "Please wating compile over do it", "ok");
                return;
            }
            if (ParticleSortLayerSetterEditor.Instance == null)
            {
                ParticleSortLayerSetterEditor window = EditorWindow.GetWindow<ParticleSortLayerSetterEditor>();
                window.titleContent.text = "特效排序工具";
            }
        }
        //-----------------------------------------------------
        protected override void OnInnerDisable()
        {
            if (m_Logic != null) m_Logic.OnDisable();
        }
        //-----------------------------------------------------
        protected override void OnInnerEnable()
        {
            Instance = this;
            base.minSize = new Vector2(500, 160);

            m_Logic = new ParticleSortLayerSetterEditorLogic();
            if (m_Logic != null)
                m_Logic.OnEnable();
            m_strMsgHelp = "1.选择需要排序的特效文件的根目录\r\n2.选择排序层最低层级的单元\r\n3.点击排序，排序结束后，会自动计算新的最低层级\r\n4.特效层级有最大上限不能超过32767"; 
        }
        //-----------------------------------------------------
        public void setTargets(UnityEngine.Object[] targets)
        {
        }
        //-----------------------------------------------------
        protected override void OnInnerGUI()
        {
            if (m_Logic == null)
            {
                base.ShowNotification(new GUIContent("Init Failed."));
                return;
            }

            DrawHelpBox();
            m_Logic.OnDrawLayerPanel();
        }
        //-----------------------------------------------------
        private void DrawHelpBox()
        {
            Color color = GUI.color;
            GUI.color = Color.green;
            EditorGUILayout.HelpBox(m_strMsgHelp, MessageType.None);
            GUI.color = color;
            if(GUILayout.Button("说明文档"))
            {
                Application.OpenURL("https://docs.qq.com/doc/DTHppemVFZnJFdHRV");
            }
        }
    }
}
