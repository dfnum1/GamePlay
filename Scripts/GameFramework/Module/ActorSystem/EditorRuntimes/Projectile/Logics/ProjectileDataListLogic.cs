#if UNITY_EDITOR
/********************************************************************
生成日期:	25:7:2019   14:35
类    名: 	ProjectileDataLogic
作    者:	HappLI
描    述:	飞行道具编辑器
*********************************************************************/
using Framework.ActorSystem.Runtime;
using Framework.ED;
using UnityEditor;
using UnityEngine;

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
            m_pTreeView = new TreeAssetView(new string[] { "弹道列表","" });
            m_pTreeView.OnCellDraw += OnDrawItem;
          //  m_pTreeView.OnSelectChange = OnItemSelect;
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
            var rect = GetRect();
            m_pTreeView.searchString = GUI.TextField(new Rect(rect.x, rect.y, rect.width, 20), m_pTreeView.searchString);
            m_pTreeView.GetColumn(0).width = rect.width-50;
            m_pTreeView.GetColumn(1).width = 50;
            m_pTreeView.OnGUI(new Rect(rect.x, rect.y+20, rect.width, rect.height-20));
        }
        //-----------------------------------------------------
        protected override void OnEvent(Event evt)
        {
            if(evt.type == EventType.KeyUp)
            {
                if(evt.keyCode == KeyCode.Escape)
                {
                    Active(false);
                }
            }    
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
            var data = rowData.itemData.data as ProjectileItem;
            if (rowData.column == 0)
            {
                string name = data.pData.desc;
                if (string.IsNullOrEmpty(name))
                    name = data.id.ToString();
                else
                {
                    name += "[" + data.pData.id + "]";
                }
                rowData.itemData.data.name = name;
                EditorGUI.LabelField(rowData.rowRect, name);
            }
            else if (rowData.column == 1)
            {
                float offsetX = rowData.rowRect.x;
                string strFile = GetOwner<ProjectileEditor>().GetProjectileDatas()?.GetDataPath(data.pData);
                if(!string.IsNullOrEmpty(strFile))
                {
                    if (GUI.Button(new Rect(offsetX, rowData.rowRect.y, 20, rowData.rowRect.height), "☝"))
                    {
                        EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<Object>(strFile));
                    }
                    offsetX += 20;
                }

                if (GUI.Button(new Rect(offsetX, rowData.rowRect.y, 20, rowData.rowRect.height), "X"))
                {
                    if (EditorUtility.DisplayDialog("提示", "确认是否要删除本弹道数据?", "删除", "取消"))
                    {
                        GetOwner<ProjectileEditor>().GetActorManager().GetProjectileManager().RemoveProjectileData(data.pData.id);
                        string path = strFile.Replace("\\", "/");
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                        string metaPath = path + ".meta";
                        if (System.IO.File.Exists(metaPath))
                        {
                            System.IO.File.Delete(metaPath);
                        }
                        Refresh();
                    }
                }
            }
            return true;
        }
    }
}
#endif