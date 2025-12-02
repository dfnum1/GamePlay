#if UNITY_EDITOR
/********************************************************************
生成日期:	25:7:2019   14:35
类    名: 	ProjectileDataLogic
作    者:	HappLI
描    述:	飞行道具编辑器
*********************************************************************/
using UnityEditor;
using UnityEngine;
using Framework.ED;
using Framework.ActorSystem.Runtime;

namespace Framework.ProjectileSystem.Editor
{
    [EditorBinder(typeof(ProjectileEditor), "m_InspecSize")]
    public class ProjectileDataLogic : AProjectileEditorLogic
    {
        public class ProjectileItem : TreeAssetView.ItemData
        {
            public int editorID = 0;
            public ProjectileData pData;
            public override Color itemColor()
            {
                return Color.white;
            }
        }
        ProjectileData m_pCurItem;
        Vector2 m_Scroll = Vector2.zero;

        bool m_bDirtyRefresh = false;
        //-----------------------------------------------------
        protected override void OnEnable()
        {
            base.OnEnable();
        }
        //-----------------------------------------------------
        protected override void OnDisable()
        {
            Clear();
        }
        //--------------------------------------------------------
        protected override void OnStart()
        {
            if (m_pCurItem == null)
                New();
        }
        //-----------------------------------------------------
        public void Clear()
        {
            ClearTarget();
        }
        //-----------------------------------------------------
        void ClearTarget()
        {
            m_pCurItem = null;
        }
        //-----------------------------------------------------
        public override void OnChangeSelect(object pData)
        {
            if (pData == null) return;
            if (!(pData is ProjectileData))
                return;
            ProjectileData item = pData as ProjectileData;
            OnChangeSelect(item);
        }
        //-----------------------------------------------------
        void OnChangeSelect(ProjectileData  item)
        {
            if (item == m_pCurItem)
                return;
            ClearTarget();
            m_pCurItem = item;
        }
        //-----------------------------------------------------
        public void SaveData()
        {
        }
        //-----------------------------------------------------
        public void Realod()
        {
            RefreshList();
        }
        //-----------------------------------------------------
        void RefreshList(bool bRealod = true)
        {
        }
        //-----------------------------------------------------
        protected override void OnGUI()
        {
            if (m_pCurItem == null) return;
            GUILayout.BeginArea(GetRect());
            var previewLogic = GetLogic<ProjectilePreview>();
            if (m_pCurItem!=null && previewLogic.IsCanRefreshTest() && GUILayout.Button("刷新预览曲线", new GUILayoutOption[] { GUILayout.Width(GetRect().size.x - 6) }))
            {
                previewLogic.RefreshTest();
            }
            m_Scroll = EditorGUILayout.BeginScrollView(m_Scroll);
            ProjectileDataDrawer.OnInsepctor(m_pCurItem, GetRect().size);
            EditorGUILayout.EndScrollView();
            GUILayout.EndArea();
        }
        //-----------------------------------------------------
        public override void New()
        {
            ProjectileData newData = new ProjectileData();
            newData.id = 0;

            GetOwner<ProjectileEditor>().OnSelectProjectileData(newData);
        }
    }
}
#endif