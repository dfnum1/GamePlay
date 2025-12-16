/********************************************************************
生成日期:	06:30:2025
类    名: 	UndoRedoSystem
作    者:	HappLI
描    述:	
*********************************************************************/
#if UNITY_EDITOR
using Framework.AT.Runtime;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;
using static UnityEngine.LightProbeProxyVolume;

namespace Framework.AT.Editor
{
    internal enum UndoRedoOperationType
    {
        All,
        Position,
        NodeChange,
        NodeEdge,
        NodeAdd,
        NodeDel,
    }
    internal class UndoRedoSystem
    {
        struct Snapshot
        {
            public UndoRedoOperationType type;
            public BaseNode pDummy;
            public AgentTreeData atData;
        }
        Stack<List<Snapshot>> m_vUndoList = new Stack<List<Snapshot>>();
        Stack<List<Snapshot>> m_vRedoList = new Stack<List<Snapshot>>();
        AgentTreeGraphView m_pGraphView;
        internal UndoRedoSystem(AgentTreeGraphView view)
        {
            m_pGraphView = view;
        }
        //-----------------------------------------------------
        public void RegisterUndo(GraphNode node, UndoRedoOperationType type, params GraphNode[] relevances)
        {
            AgentTreeData pData = new AgentTreeData();
            m_pGraphView.Save(pData);

            List<Snapshot> vSnapshot = new List<Snapshot>();
            Snapshot atSnap = new Snapshot()
            {
                type = UndoRedoOperationType.All,
                atData = pData
            };
            vSnapshot.Add(atSnap);
            m_vUndoList.Push(vSnapshot);
            return;
            if(type != UndoRedoOperationType.NodeAdd)
                node.Save();
            BaseNode dummyNode = (BaseNode)JsonUtility.FromJson(JsonUtility.ToJson(node.bindNode), node.bindNode.GetType());

            HashSet<int> vSets = new HashSet<int>();
            vSets.Add(node.bindNode.guid);
            List<Snapshot> snapshots = new List<Snapshot>();
            Snapshot snap = new Snapshot()
            {
                type = type,
                pDummy = dummyNode,
            };
            snapshots.Add(snap);

            if (dummyNode.nextActions!=null)
            {
                for(int i =0; i< dummyNode.nextActions.Length; ++i)
                {
                    var next = m_pGraphView.GetNode(dummyNode.nextActions[i]);
                    if(next!=null && !vSets.Contains(next.bindNode.guid))
                    {
                        vSets.Add(next.bindNode.guid);
                        BaseNode srcDummy = (BaseNode)JsonUtility.FromJson(JsonUtility.ToJson(next.bindNode), next.bindNode.GetType());
                        next.Save(srcDummy);
                        Snapshot tempSnap = new Snapshot()
                        {
                            type = type,
                            pDummy = srcDummy,
                        };
                        snapshots.Add(tempSnap);
                    }
                }
            }

            var inport = node.GetLink(true);
            if(inport!=null)
            {
                var connections = inport.bindPort.connections;
                foreach (var db in connections)
                {
                    if (db.output.source is LinkPort)
                    {
                        var output = (LinkPort)db.output.source;
                        if (vSets.Contains(output.graphNode.bindNode.guid))
                            continue;
                        vSets.Add(output.graphNode.bindNode.guid);
                        output.graphNode.Save();
                        BaseNode srcDummy = (BaseNode)JsonUtility.FromJson(JsonUtility.ToJson(output.graphNode.bindNode), output.graphNode.bindNode.GetType());
                        Snapshot srcSnap = new Snapshot()
                        {
                            type = type,
                            pDummy = srcDummy,
                        };
                        snapshots.Add(srcSnap);
                    }
                }
            }

            var nodePorts = dummyNode.GetInports(false);
            if(nodePorts!=null)
            {
                for (int i = 0; i < nodePorts.Length; ++i)
                {
                    if(nodePorts[i].dummyPorts !=null)
                    {
                        for (int j = 0; j < nodePorts[i].dummyPorts.Length; ++j)
                        {
                            var srcNode = m_pGraphView.GetNode(nodePorts[i].dummyPorts[j].guid);
                            if (srcNode != null)
                            {
                                if (vSets.Contains(srcNode.bindNode.guid))
                                    continue;
                                vSets.Add(srcNode.bindNode.guid);

                                srcNode.Save();
                                BaseNode srcDummy = (BaseNode)JsonUtility.FromJson(JsonUtility.ToJson(srcNode.bindNode), srcNode.bindNode.GetType());
                                Snapshot srcSnap = new Snapshot()
                                {
                                    type = type,
                                    pDummy = srcDummy,
                                };
                                snapshots.Add(srcSnap);
                            }
                        }
                    }
                }
            }

            if(relevances!=null)
            {
                foreach(var db in relevances)
                {
                    if (vSets.Contains(db.bindNode.guid))
                        continue;
                    vSets.Add(db.bindNode.guid);

                    db.Save();
                    BaseNode srcDummy = (BaseNode)JsonUtility.FromJson(JsonUtility.ToJson(db.bindNode), db.bindNode.GetType());
                    Snapshot srcSnap = new Snapshot()
                    {
                        type = type,
                        pDummy = srcDummy,
                    };
                    snapshots.Add(srcSnap);
                }
            }

            m_vUndoList.Push(snapshots);
        }
        //-----------------------------------------------------
        public bool HasChanges()
        {
            return m_vUndoList.Count>0 || m_vRedoList.Count > 0;
        }
        //-----------------------------------------------------
        void RefreshNode(GraphNode node, BaseNode pDummy)
        {
            {
                var bindPorts = pDummy.GetInports(false);
                var argvs = node.GetArvgs();
                if (argvs != null && bindPorts != null)
                {
                    for (int i = 0; i < argvs.Count && i < bindPorts.Length; ++i)
                    {
                        bindPorts[i].pVariable = m_pGraphView.GetVariable(bindPorts[i].varGuid);
                        argvs[i].nodePort = bindPorts[i];
                    }
                }
            }
            {
                var bindPorts = pDummy.GetOutports(false);
                var argvs = node.GetReturns();
                if (argvs != null && bindPorts != null)
                {
                    for (int i = 0; i < argvs.Count && i < bindPorts.Length; ++i)
                    {
                        bindPorts[i].pVariable = m_pGraphView.GetVariable(bindPorts[i].varGuid);
                        argvs[i].nodePort = bindPorts[i];
                    }
                }
            }
        }
        //-----------------------------------------------------
        public void Redo()
        {
            if (m_vRedoList.Count <= 0)
                return;

            List<GraphNode> vNodes = new List<GraphNode>();
            var snapshots = m_vRedoList.Pop();
            foreach (var db in snapshots)
            {
                if (db.type == UndoRedoOperationType.All)
                {
                    m_pGraphView.RefreshATDatas(db.atData);
                }
                else if (db.type == UndoRedoOperationType.NodeDel)
                {
                    //! To do: 删除节点
                    m_pGraphView.RemoveNode(db.pDummy.guid, false);
                }
                else if (db.type == UndoRedoOperationType.NodeAdd)
                {
                    //! To do: 添加节点
                    var node = m_pGraphView.AddNode(db.pDummy, false);
                    RefreshNode(node, db.pDummy);
                    vNodes.Add(node);
                }
                else if (db.type == UndoRedoOperationType.NodeChange ||
                    db.type == UndoRedoOperationType.NodeEdge)
                {
                    //! To do: 节点变更
                    //! To do: 节点连线变更
                    var node = m_pGraphView.GetNode(db.pDummy.guid);
                    if (node != null)
                    {
                        node.bindNode = db.pDummy;
                        RefreshNode(node, db.pDummy);
                        vNodes.Add(node);
                    }
                    else
                    {
                        node = m_pGraphView.AddNode(db.pDummy, false);
                        RefreshNode(node, db.pDummy);
                        vNodes.Add(node);
                    }
                }
                else if (db.type == UndoRedoOperationType.Position)
                {
                    //! To do: 节点位置变更
                    var node = m_pGraphView.GetNode(db.pDummy.guid);
                    if (node != null)
                    {
                        node.bindNode.posX = db.pDummy.posX;
                        node.bindNode.posY = db.pDummy.posY;
                        node.UpdatePosition();
                    }
                }
            }
            if (vNodes.Count > 0)
                m_pGraphView.UpdateNodeLinks(vNodes);
            m_vUndoList.Push(snapshots);
        }
        //-----------------------------------------------------
        public void Undo()
        {
            if (m_vUndoList.Count <= 0)
                return;

            List<GraphNode> vNodes = new List<GraphNode>();
            var snapshots = m_vUndoList.Pop();
            foreach (var db in snapshots)
            {
                if (db.type == UndoRedoOperationType.All)
                {
                    m_pGraphView.RefreshATDatas(db.atData);
                }
                else if (db.type == UndoRedoOperationType.NodeAdd)
                {
                    //! To do: 删除节点
                    m_pGraphView.RemoveNode(db.pDummy.guid, false);
                }
                else if(db.type == UndoRedoOperationType.NodeDel)
                {
                    //! To do: 添加节点
                    var node = m_pGraphView.AddNode(db.pDummy, false);
                    RefreshNode(node, db.pDummy);
                    vNodes.Add(node);
                }
                else if( db.type == UndoRedoOperationType.NodeChange ||
                    db.type == UndoRedoOperationType.NodeEdge)
                {
                    //! To do: 节点变更
                    //! To do: 节点连线变更
                    var node = m_pGraphView.GetNode(db.pDummy.guid);
                    if (node != null)
                    {
                        RefreshNode(node, db.pDummy);
                        vNodes.Add(node);
                    }
                }
                else if(db.type == UndoRedoOperationType.Position)
                {
                    //! To do: 节点位置变更
                    var node = m_pGraphView.GetNode(db.pDummy.guid);
                    if (node != null)
                    {
                        node.bindNode.posX = db.pDummy.posX;
                        node.bindNode.posY = db.pDummy.posY;
                        node.UpdatePosition();
                    }
                }
            }
            if (vNodes.Count > 0)
                m_pGraphView.UpdateNodeLinks(vNodes);
            m_vRedoList.Push(snapshots);
        }
    }
}

#endif