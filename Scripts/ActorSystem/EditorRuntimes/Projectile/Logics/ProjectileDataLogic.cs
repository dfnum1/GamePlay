#if UNITY_EDITOR
/********************************************************************
生成日期:	25:7:2019   14:35
类    名: 	ProjectileDataLogic
作    者:	HappLI
描    述:	飞行道具编辑器
*********************************************************************/
using Framework.ActorSystem.Runtime;
using Framework.ED;
using Mono.Cecil.Cil;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

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
        public override void OnSaveChanges()
        {
            if (m_pCurItem == null)
                return;
            var projectileDatas = GetOwner<ProjectileEditor>().GetProjectileDatas();
            if(projectileDatas == null)
            {
                EditorUtility.DisplayDialog("提示", "当前还没创建弹道的数据集对象，请先创建!", "好的");
                return;
            }

            string assetFile = System.IO.Path.GetDirectoryName(AssetDatabase.GetAssetPath(projectileDatas)).Replace("\\", "/");

            string saveFile = GetOwner<ProjectileEditor>().GetProjectileDatas().GetDataPath(m_pCurItem);
            if (string.IsNullOrEmpty(saveFile))
            {
                saveFile = EditorUtility.SaveFilePanelInProject("保存弹道数据", m_pCurItem.id.ToString(), "json", "", assetFile);
                if (saveFile != null) saveFile = saveFile.Replace("\\", "/");
                if (saveFile == null || !saveFile.StartsWith(assetFile))
                {
                    EditorUtility.DisplayDialog("提示", "请将弹道数据保存在\r\n" + assetFile + "\r\n目录或子目录下", "好的");
                    return;
                }
                string code = JsonUtility.ToJson(m_pCurItem, true);

                string dir = Path.GetDirectoryName(saveFile);
                if (!System.IO.Directory.Exists(dir))
                    System.IO.Directory.CreateDirectory(dir);

                projectileDatas.SetDataPath(m_pCurItem, saveFile);
            }
            {
                string code = JsonUtility.ToJson(m_pCurItem, true);
                FileStream fs = new FileStream(saveFile, System.IO.FileMode.OpenOrCreate);
                StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.UTF8);
                fs.Position = 0;
                fs.SetLength(0);
                writer.Write(code);
                writer.Close();
                GetOwner<ProjectileEditor>().GetActorManager().GetProjectileManager().AddProjectileData(m_pCurItem);
            }
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
            EditorGUI.BeginChangeCheck();

            uint newId = (uint)EditorGUILayout.DelayedIntField("Id", (int)m_pCurItem.id);
            if(newId != m_pCurItem.id)
            {
                var projMgr = GetOwner<ProjectileEditor>().GetActorManager().GetProjectileManager();
                var checkProj = projMgr.GetProjectileData(newId);
                if (checkProj == null)
                {
                    projMgr.RemoveProjectileData(m_pCurItem.id);
                    m_pCurItem.id = newId;
                    projMgr.AddProjectileData(m_pCurItem);
                }
                else
                {
                    EditorUtility.DisplayDialog("提示", $"该Id={newId}已被使用", "好的");
                }
            }

            ProjectileDataDrawer.OnInsepctor(m_pCurItem, GetRect().size);
            if(m_pCurItem != null && previewLogic.IsCanRefreshTest() && EditorGUI.EndChangeCheck())
                previewLogic.RefreshTest();

            EditorGUILayout.EndScrollView();
            GUILayout.EndArea();
        }
        //-----------------------------------------------------
        public override void New()
        {
            var projectileDatas = GetOwner<ProjectileEditor>().GetProjectileDatas();
            if (projectileDatas == null)
            {
                EditorUtility.DisplayDialog("提示", "当前还没创建弹道的数据集对象，请先创建!", "好的");
                return;
            }

            ProjectileData newData = new ProjectileData();
            newData.id = GetOwner<ProjectileEditor>().GetActorManager().GetProjectileManager().GeneratorNewID();
            GetOwner<ProjectileEditor>().OnSelectProjectileData(newData);
        }
    }
}
#endif