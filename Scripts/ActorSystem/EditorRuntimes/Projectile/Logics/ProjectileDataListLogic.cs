#if UNITY_EDITOR
/********************************************************************
生成日期:	25:7:2019   14:35
类    名: 	ProjectileDataLogic
作    者:	HappLI
描    述:	飞行道具编辑器
*********************************************************************/
using Framework.ActorSystem.Runtime;
using Framework.Cutscene.Editor;
using Framework.ED;
using Mono.Cecil.Cil;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;
using static Framework.ActorSystem.Editor.AssetDrawLogic;

namespace Framework.ProjectileSystem.Editor
{
    [EditorBinder(typeof(ProjectileEditor), "m_DataListSize",-1)]
    public class ProjectileDataListLogic : AProjectileEditorLogic
    {
        public class ProjectileItem : TreeAssetView.ItemData
        {
            public ProjectileData pData;
            public override Color itemColor()
            {
                return Color.white;
            }
        }
        TreeAssetView m_pTreeView = null;
        //-----------------------------------------------------
        protected override void OnEnable()
        {
            base.OnEnable();
            m_pTreeView = new TreeAssetView(new string[] { "弹道列表" });
            m_pTreeView.OnCellDraw += OnDrawItem;
            m_pTreeView.OnSelectChange = OnItemSelect;
            m_pTreeView.OnItemDoubleClick = OnItemSelect;
            Refresh();
        }
        //-----------------------------------------------------
        void Refresh()
        {
            var datas = GetOwner<ProjectileEditor>().GetActorManager().GetProjectileManager().GetDatas();
            m_pTreeView.BeginTreeData();
            foreach(var db in datas)
            {
                ProjectileItem item = new ProjectileItem();
                item.id = (int)db.Key;
                item.pData = db.Value;
                m_pTreeView.AddData(item);
            }
            m_pTreeView.EndTreeData();
        }
        //-----------------------------------------------------
        protected override void OnDisable()
        {
        }
        //-----------------------------------------------------
        protected override void OnActive()
        {
            if (IsActive())
                Refresh();
        }
        //-----------------------------------------------------
        protected override void OnGUI()
        {
            m_pTreeView.GetColumn(0).width = GetRect().width;
            m_pTreeView.OnGUI(GetRect());
        }
        //--------------------------------------------------------
        void OnItemSelect(TreeAssetView.ItemData item)
        {
            var data = item as ProjectileItem;
            this.Active(false);
            GetOwner<ProjectileEditor>()?.OnSelectProjectileData(data.pData);
        }
        //-----------------------------------------------------
        bool OnDrawItem(TreeAssetView.RowArgvData rowData)
        {
            var data = rowData.itemData.data;
            if (rowData.column == 0)
            {
                string name = data.name;
                if (string.IsNullOrEmpty(name))
                    name = data.id.ToString();
                else
                {
                    name += "[" + data.id + "]";
                }
                EditorGUI.LabelField(rowData.rowRect, name);
            }
            return true;
        }
    }
}
#endif