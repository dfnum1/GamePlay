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

namespace Framework.AT.Editor
{
    internal enum UndoRedoOperationType
    {
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
        }
        Stack<List<Snapshot>> m_vUndoList = new Stack<List<Snapshot>>();
        Stack<List<Snapshot>> m_vRedoList = new Stack<List<Snapshot>>();
        AgentTreeGraphView m_pGraphView;
        internal UndoRedoSystem(AgentTreeGraphView view)
        {
            m_pGraphView = view;
        }
        //-----------------------------------------------------
        public void RegisterUndo(GraphNode node, UndoRedoOperationType type)
        {
            BaseNode dummyNode = (BaseNode)JsonUtility.FromJson(JsonUtility.ToJson(node.bindNode), node.bindNode.GetType());
            node.Save(dummyNode);

            List<Snapshot> snapshots = new List<Snapshot>();
            Snapshot snap = new Snapshot()
            {
                type = type,
                pDummy = dummyNode,
            };
            snapshots.Add(snap);

            if(dummyNode.nextActions!=null)
            {
                for(int i =0; i< dummyNode.nextActions.Length; ++i)
                {
                    var next = m_pGraphView.GetNode(dummyNode.nextActions[i]);
                    if(next!=null)
                    {
                        BaseNode srcDummy = (BaseNode)JsonUtility.FromJson(JsonUtility.ToJson(next.bindNode), next.bindNode.GetType());
                        next.Save(srcDummy);
                        Snapshot tempSnap = new Snapshot()
                        {
                            type = UndoRedoOperationType.NodeChange,
                            pDummy = srcDummy,
                        };
                        snapshots.Add(tempSnap);
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
                                BaseNode srcDummy = (BaseNode)JsonUtility.FromJson(JsonUtility.ToJson(srcNode.bindNode), srcNode.bindNode.GetType());
                                srcNode.Save(srcDummy);
                                Snapshot srcSnap = new Snapshot()
                                {
                                    type = UndoRedoOperationType.NodeChange,
                                    pDummy = srcDummy,
                                };
                                snapshots.Add(srcSnap);
                            }
                        }
                    }
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
        public void Redo()
        {

        }
        //-----------------------------------------------------
        public void Undo()
        {

        }
    }
}

#endif