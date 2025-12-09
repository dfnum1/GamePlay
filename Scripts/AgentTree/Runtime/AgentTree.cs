/********************************************************************
生成日期:	07:03:2025
类    名: 	AgentTree
作    者:	HappLI
描    述:	行为树
*********************************************************************/
using Framework.Core;
using System.Collections.Generic;
namespace Framework.AT.Runtime
{
    public partial class AgentTree
    {
        AgentTreeManager                            m_pATManager = null;
        bool                                        m_bEnable = false;
        AgentTreeData                               m_pData = null;
        VariableKV                                  m_vRuntimeVariables = null;
        LinkedList<BaseNode>                        m_vExecuting = null;
        EnterTask                                   m_pStartTask = null;
        EnterTask                                   m_pExitTask = null;

        HashSet<short>                              m_vExecuted = null;
        private Dictionary<short, long>             m_vNodeExecTime = null;
        private long                                m_execOrder = 0;

        EnterTask                                   m_pTickTask = null;
        LinkedList<BaseNode>                        m_vTickExecuting = null;

        LinkedList<IAgentTreeCallback>              m_vCallback = null;

        LinkedList<BaseNode>                        m_pCurrentExcuting = null;
        bool                                        m_bHasCustomTask = false;
        //-----------------------------------------------------
        internal AgentTree()
        {
            m_bEnable = false;
            m_bHasCustomTask = false;
        }
        //-----------------------------------------------------
        internal void SetATManager(AgentTreeManager pManager)
        {
            m_pATManager = pManager;
        }
        //-----------------------------------------------------
        internal AgentTreeData GetData()
        {
            return m_pData;
        }
        //-----------------------------------------------------
        public BaseNode GetNode(short guid)
        {
            if (m_pData == null) return null;
            return m_pData.GetNode(guid);
        }
        //-----------------------------------------------------
        public IUserData FindUserClass(int hashCode)
        {
            return null;
        }
        //-----------------------------------------------------
        VariableKV GetRuntimeVariable()
        {
            if (m_vRuntimeVariables == null) m_vRuntimeVariables = VariablePool.Malloc();
            return m_vRuntimeVariables;
        }
        //-----------------------------------------------------
        public bool Create(AgentTreeData agentTree)
        {
            Clear();

            if (agentTree == null)
                return false;

            m_bHasCustomTask = true;
            m_pData = agentTree;
            if(agentTree.tasks!=null)
            {
                for (int i = 0; i < agentTree.tasks.Length; ++i)
                {
                    var task = agentTree.tasks[i];
                    if (task.type == (int)ETaskType.eExit)
                    {
                        m_pExitTask = task;
                        continue;
                    }
                    else if (task.type == (int)ETaskType.eTick)
                    {
                        m_pTickTask = task;
                        continue;
                    }
                    else if (task.type == (int)ETaskType.eStart)
                    {
                        m_pStartTask = task;
                    }
                    else
                        m_bHasCustomTask = true;
                }
            }

            return true;
        }
        //-----------------------------------------------------
        public void Enable(bool bEnable)
        {
            if (m_bEnable == bEnable)
                return;
            m_bEnable = bEnable;
            if (m_bEnable)
            {
                if (m_pStartTask != null)
                {
                    if (m_vExecuting == null) m_vExecuting = new LinkedList<BaseNode>();
                    else m_vExecuting.Clear();
                    m_vExecuting.AddFirst(m_pStartTask);
                }

                if (m_pTickTask != null)
                {
                    if (m_vTickExecuting == null) m_vTickExecuting = new LinkedList<BaseNode>();
                    else m_vTickExecuting.Clear();
                    m_vTickExecuting.AddFirst(m_pTickTask);
                }
            }
            else
            {
                if (m_pExitTask != null)
                {
                    if (m_vExecuting != null) m_vExecuting.Clear();
                    else m_vExecuting = new LinkedList<BaseNode>();
                    m_vExecuting.AddLast(m_pExitTask);
                    Execute(m_vExecuting);
                }
            }
        }
        //-----------------------------------------------------
        public bool IsEnable()
        {
            return m_bEnable;
        }
        //-----------------------------------------------------
        public int GetTaskArgvCount(int type, out int argvType)
        {
            argvType = 0;
            return 0;
        }
        //-----------------------------------------------------
        public bool ExecuteTask(int type, VariableList vArgvs = null, bool bAutoReleaseAgvs = true)
        {
            if (!m_bEnable || m_pData == null || m_pData.tasks == null)
            {
                if (bAutoReleaseAgvs && vArgvs != null) vArgvs.Release();
                return false;
            }
            if (type <= (int)ETaskType.eExit)
            {
                if (bAutoReleaseAgvs && vArgvs != null) vArgvs.Release();
                return false;
            }
            bool bHasType = false;
            for (int i = 0; i < m_pData.tasks.Length; ++i)
            {
                var task = m_pData.tasks[i];
                if (task.type == type)
                {
                    bHasType = true;
                    ExecuteNode(task, vArgvs, false);
                }
            }
            if (bAutoReleaseAgvs && vArgvs != null) vArgvs.Release();
            return bHasType;
        }
        //-----------------------------------------------------
        public bool ExecuteNode(BaseNode pNode, VariableList vArgvs = null, bool bAutoReleaseAgvs = true)
        {
            if (pNode == null) return false;
            if (m_vExecuting == null) m_vExecuting = new LinkedList<BaseNode>();
            if (vArgvs != null)
            {
                var ports = pNode.GetOutports();
                int portCnt = pNode.GetOutportCount();
                if(!pNode.IsTask())
                {
                    ports = pNode.GetInports();
                    portCnt = pNode.GetInportCount();
                }
                for (int j = 0; j < portCnt && j < vArgvs.GetVarCount(); ++j)
                {
                    var port = ports[j];
                    if (port.pVariable == null)
                        continue;
                    switch (port.pVariable.GetVariableType())
                    {
                        case EVariableType.eInt:
                            SetInt(port.varGuid, vArgvs.GetInt(j));
                            break;
                        case EVariableType.eBool:
                            SetBool(port.varGuid, vArgvs.GetBool(j));
                            break;
                        case EVariableType.eFloat:
                            SetFloat(port.varGuid, vArgvs.GetFloat(j));
                            break;
                        case EVariableType.eString:
                            SetString(port.varGuid, vArgvs.GetString(j));
                            break;
                        case EVariableType.eVec2:
                            SetVec2(port.varGuid, vArgvs.GetVec2(j));
                            break;
                        case EVariableType.eVec3:
                            SetVec3(port.varGuid, vArgvs.GetVec3(j));
                            break;
                        case EVariableType.eVec4:
                            SetVec4(port.varGuid, vArgvs.GetVec4(j));
                            break;
                        case EVariableType.eObjId:
                            SetObjId(port.varGuid, vArgvs.GetObjId(j));
                            break;
                        case EVariableType.eRay:
                            SetRay(port.varGuid, vArgvs.GetRay(j));
                            break;
                        case EVariableType.eColor:
                            SetColor(port.varGuid, vArgvs.GetColor(j));
                            break;
                        case EVariableType.eQuaternion:
                            SetQuaternion(port.varGuid, vArgvs.GetQuaternion(j));
                            break;
                        case EVariableType.eBounds:
                            SetBounds(port.varGuid, vArgvs.GetBounds(j));
                            break;
                        case EVariableType.eRect:
                            SetRect(port.varGuid, vArgvs.GetRect(j));
                            break;
                        case EVariableType.eMatrix:
                            SetMatrix(port.varGuid, vArgvs.GetMatrix(j));
                            break;
                        case EVariableType.eUserData:
                            var varVar = vArgvs.GetUserData(j);
                            SetUserData(port.varGuid, varVar.pPointer);
                            break;
                    }
                }
            }
            m_vExecuting.AddFirst(pNode);
            return true;
        }
        //-----------------------------------------------------
        internal bool Update(float deltaTime)
        {
            if (!m_bEnable) return true;
            if (m_vTickExecuting != null && m_vTickExecuting.Count <= 0)
            {
                if (m_pTickTask != null)
                {
                    m_vTickExecuting.AddFirst(m_pStartTask);
                }
            }

            Execute(m_vTickExecuting);
            Execute(m_vExecuting);
            m_pCurrentExcuting = null;

            return IsKeepUpdate();
        }
        //-----------------------------------------------------
        bool IsKeepUpdate()
        {
            if (m_bHasCustomTask) return true;
            if (m_pExitTask != null || m_pTickTask != null) return true;
            if (m_vExecuting != null && m_vExecuting.Count > 0) return true;
            if (m_vTickExecuting != null && m_vTickExecuting.Count > 0) return true;
            return false;
        }
        //-----------------------------------------------------
        void Execute(LinkedList<BaseNode> vList)
        {
            m_pCurrentExcuting = vList;
            if (vList == null)
                return;
            for (var node = vList.First; node != null;)
            {
                var next = node.Next;
                var curNode = node.Value;
                bool bSucceed = OnExecute(curNode);
                if (bSucceed)
                    AddExecuted(curNode);
                if (bSucceed)
                {
                    vList.Remove(node);
                    var nexts = curNode.GetNexts(m_pData);
                    if (nexts != null)
                    {
                        for (int i = 0; i < nexts.Length; ++i)
                        {
                            if (nexts[i] == null) continue;
                            if (vList.Contains(nexts[i])) continue;
                            vList.AddLast(nexts[i]);
                        }
                    }
                }
                else
                {
                    if (IsOverAction(curNode.type))
                    {
                        AddExecuted(curNode);
                        vList.Remove(node);
                    }
                }
                node = next;
            }
        }
        //-----------------------------------------------------
        public void PushDoNode(BaseNode currentNode, short nodeGuid)
        {
            if (m_pData == null || m_pCurrentExcuting == null)
                return;
            var node = m_pData.GetNode(nodeGuid);
            if (node == null)
                return;
            if (!IsOverAction(currentNode.type))
                m_pCurrentExcuting.Remove(currentNode);
            m_pCurrentExcuting.AddLast(node);
        }
        //-----------------------------------------------------
        bool IsOverAction(int actionType)
        {
            return actionType == (short)EActionType.eCondition;
        }
        //-----------------------------------------------------
        private bool OnExecute(BaseNode pNode)
        {
            if (pNode.IsTask())
                return true;
            switch (pNode.type)
            {
                case (short)EActionType.eNewVariable:
                case (short)EActionType.eGetVariable: return true;
                case (short)EActionType.eOpVariable:
                    return VarOpExecutor.OnExecute(this, pNode);
                case (short)EActionType.eDotVariable:
                case (short)EActionType.eCrossVariable:
                case (short)EActionType.eDistanceVariable:
                case (short)EActionType.eLerp:
                    return VectorOpExecutor.OnExecutor(this, pNode);
                case (short)EActionType.eCondition: return ConditionExecutor.OnExecute(this, pNode);
            }
            if (m_vCallback != null)
            {
                for (var callback = m_vCallback.First; callback != null; callback = callback.Next)
                {
                    if(callback.Value.OnNotifyExecutedNode(this, pNode))
                    {
                        return true;
                    }
                }
            }
            if (m_pATManager != null)
            {
                return m_pATManager.OnNotifyExecutedNode(this, pNode);
            }
            return false;
        }
        //-----------------------------------------------------
        public void RegisterCallback(IAgentTreeCallback pCallback)
        {
            if (m_vCallback == null) m_vCallback = new LinkedList<IAgentTreeCallback>();
            if (!m_vCallback.Contains(pCallback))
                m_vCallback.AddLast(pCallback);
        }
        //-----------------------------------------------------
        public void UnregisterCallback(IAgentTreeCallback pCallback)
        {
            if (m_vCallback == null) return;
            if (m_vCallback.Contains(pCallback))
                m_vCallback.Remove(pCallback);
        }
        //-----------------------------------------------------
        public bool IsExecuted(short guid)
        {
            if (m_vExecuted == null) return false;
            return m_vExecuted.Contains(guid);
        }
        //-----------------------------------------------------
        void AddExecuted(BaseNode pNode)
        {
            if (m_vExecuted == null) m_vExecuted = new HashSet<short>(m_pData.GetNodeCnt());
            m_vExecuted.Add(pNode.guid);
            m_execOrder++;
            if (m_vNodeExecTime == null) m_vNodeExecTime = new Dictionary<short, long>(m_pData.GetNodeCnt());
            m_vNodeExecTime[pNode.guid] = m_execOrder;
        }
        //-----------------------------------------------------
        void Clear()
        {
            Enable(false);
            m_pData = null;
            if (m_vRuntimeVariables != null)
            {
                m_vRuntimeVariables.Release();
                m_vRuntimeVariables = null;
            }
            if (m_vExecuting != null) m_vExecuting.Clear();
            if (m_vNodeExecTime != null) m_vNodeExecTime.Clear();
            if (m_vTickExecuting != null) m_vTickExecuting.Clear();
            if (m_vExecuted != null) m_vExecuted.Clear();
            m_pExitTask = null;
            m_pTickTask = null;
            m_pStartTask = null;
            m_pCurrentExcuting = null;
            m_bHasCustomTask = false;
        }
        //-----------------------------------------------------
        internal void Destroy()
        {
            Clear();
            m_pATManager = null;
            if (m_vCallback != null) m_vCallback.Clear();
        }
    }
}