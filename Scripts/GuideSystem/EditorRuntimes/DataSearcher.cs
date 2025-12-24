/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	DataSearcher
作    者:	
描    述:	数据搜索器
*********************************************************************/
#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using static Framework.Guide.Editor.GuideSystemEditor;
namespace Framework.Guide.Editor
{
    //------------------------------------------------------
    public partial class DataSearcher : GuideSearcher
    {
        //------------------------------------------------------
        protected override bool OnDragItem(AssetTree.ItemData item)
        {
            return false;
        }
        //------------------------------------------------------
        protected override bool OnDrawItem(Rect cellRect, AssetTree.TreeItemData item, int column, bool bSelected, bool focused)
        {
            AssetTree.ItemData itemData = item.data as AssetTree.ItemData;
            ItemEvent guideItem = itemData as ItemEvent;
            GuideSystemEditor.DataParam dataParam = (GuideSystemEditor.DataParam)guideItem.param;
            item.displayName = "";
            if (dataParam.Data.Tag > 0 && dataParam.Data.Tag < ushort.MaxValue)
            {
                item.displayName += "[Tag=" + dataParam.Data.Tag + "]";
            }
            item.displayName += dataParam.Data.Name + "[Id=" + dataParam.Data.Guid + "]";

            GUIContent content = new GUIContent(item.displayName, "存储文件:" + dataParam.Data.strFile);
            GUI.Label(new Rect(cellRect.x, cellRect.y, cellRect.width - 40, cellRect.height), content);
            if(GUI.Button(new Rect(cellRect.xMax-60, cellRect.y,40, cellRect.height), "移除"))
            {
                if (EditorUtility.DisplayDialog("提示", "确认是否要移除本引导组?", "移除", "取消"))
                {
                    GuideSystem.getInstance().datas.Remove(item.id);
                    if (!string.IsNullOrEmpty(dataParam.Data.strFile))
                    {
                        string path = dataParam.Data.strFile.Replace("\\", "/");
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                        string metaPath = path + ".meta";
                        if (System.IO.File.Exists(metaPath))
                        {
                            System.IO.File.Delete(metaPath);
                        }
                    }
                    Search(m_assetTree.searchString);
                }
            }
            if (GUI.Button(new Rect(cellRect.xMax - 110, cellRect.y, 50, cellRect.height), "重命名"))
            {
                RenamePopup.Show(dataParam.Data, dataParam.Data.Name);
            }
            if (GUI.Button(new Rect(cellRect.xMax - 20, cellRect.y, 20, cellRect.height), "☝"))
            {
                if(!string.IsNullOrEmpty(dataParam.Data.strFile))
                    EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<Object>(dataParam.Data.strFile));
            }
            if (dataParam.Data.Tag > 0 && dataParam.Data.Tag < ushort.MaxValue && GuideSystem.getInstance().IsTrigged(dataParam.Data.Tag))
            {
                GUI.DrawTexture(new Rect(cellRect.xMax - 135, cellRect.y, cellRect.height, cellRect.height), GuideEditorResources.LoadTexture("complete.png"));
            }
            return true;
        }
        //------------------------------------------------------
        protected override void OnSawpDatas()
        {
            if (GuideSystem.getInstance().datas == null) return;
            Dictionary<int, GuideGroup> datas = GuideSystem.getInstance().datas;
            datas.Clear();
            List<AssetTree.ItemData> vItems = m_assetTree.GetDatas();
            for(int i = 0; i < vItems.Count; ++i)
            {
                ItemEvent temp = vItems[i] as ItemEvent;
                GuideSystemEditor.DataParam dataParam = (GuideSystemEditor.DataParam)temp.param;
                datas[vItems[i].id] = dataParam.Data;
            }
        }
        //------------------------------------------------------
        protected override void OnSearch(string query)
        {
            if (GuideSystem.getInstance().datas == null) return;
            bool includeNodeFinder = false;
            string queryName = query;
            if(query.StartsWith("t:"))
            {
                includeNodeFinder = true;
                query = query.Substring(2);
            }
            GuideSystemEditor pEditor = GuideSystemEditor.Instance;
            List<GuideGroup> vGuides = new List<GuideGroup>();
            foreach (var db in GuideSystem.getInstance().datas)
            {
                bool bQuerty= false;
                if (includeNodeFinder)
                {
                    db.Value.Init(false);
                    var nodes = db.Value.GetAllNodes();
                    if (nodes == null)
                        continue;
                    foreach(var nd in nodes)
                    {
                        string name = nd.Value.Name;
                        if (name == null) name = "";
                        if (GuideSystemEditor.NodeTypes.TryGetValue(nd.Value.GetEnumType(), out var attrNode))
                        {
                            name += attrNode.strQueueName;
                        }
                        if (string.IsNullOrEmpty(name))
                            continue;

                        if (IsQuery(query, name))
                        {
                            bQuerty = true;
                            break;
                        }
                    }
                }
                else
                {
                    bQuerty = IsQuery(query, db.Value.Guid + db.Value.Name);
                }
                if (!bQuerty) continue;
                vGuides.Add(db.Value);
            }
            vGuides.Sort((a0, a1) =>
            {
                long key0 = ((a0.Tag > 0 && a0.Tag < ushort.MaxValue) ? a0.Tag : 0) * 100000 + a0.Guid;
                long key1 = ((a1.Tag > 0 && a1.Tag < ushort.MaxValue) ? a1.Tag : 0) * 100000 + a1.Guid;
                if (key0 < key1) return -1;
                else if (key0 > key1) return 1;
                return 0;
            });
                
            foreach (var db in vGuides)
            {
                GuideSystemEditor.DataParam param = new GuideSystemEditor.DataParam();
                param.Data = db;

                ItemEvent item = new ItemEvent();
                item.param = param;
                item.callback = pEditor.LoadData;

                item.id = db.Guid;
                item.name = db.Name + "[Id=" + db.Guid + "]" + queryName;
                if(db.Tag>=0 && db.Tag < ushort.MaxValue)
                {
                    item.name += "[Tag=" + db.Tag + "]";
                }
                m_assetTree.AddData(item);
            }
        }
        //------------------------------------------------------
        protected override void OnClose()
        {
            GuideSystemEditor.Instance.Save(false);
        }
    }
}
#endif