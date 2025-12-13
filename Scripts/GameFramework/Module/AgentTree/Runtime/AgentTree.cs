/********************************************************************
生成日期:	07:03:2025
类    名: 	AgentTree
作    者:	HappLI
描    述:	行为树
*********************************************************************/
using Framework.Core;
using System.Collections.Generic;
using UnityEngine;
namespace Framework.AT.Runtime
{
    public partial class AgentTree
    {
        delegate bool OnKeyEventDelegate(KeyCode key);
        AgentTreeManager                            m_pATManager = null;
        bool                                        m_bEnable = false;
        bool                                        m_bStarted = false;
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

        private HashSet<BaseNode>                   m_vMouseInputTask = null;

        LinkedList<IAgentTreeCallback>              m_vCallback = null;

        LinkedList<BaseNode>                        m_pCurrentExcuting = null;
        bool                                        m_bHasCustomTask = false;

        private Dictionary<int, IUserData>          m_OwnerClass = null;
        private Dictionary<int, IUserData>          m_OwnerParentClass = null;

        private Dictionary<KeyCode, LinkedList<BaseNode>>  m_vKeyListens = null;
#if UNITY_EDITOR
        private HashSet<KeyCode>                    m_vEditorEventKeys= new HashSet<KeyCode>(4);
#endif
        //-----------------------------------------------------
        internal AgentTree()
        {
            m_bEnable = false;
            m_bStarted = false;
            m_bHasCustomTask = false;
        }
        //-----------------------------------------------------
        public Camera GetMainCamera()
        {
            if (m_pATManager == null) return Camera.main;
            return m_pATManager.GetMainCamera();
        }
        //-----------------------------------------------------
        public bool GetNegScreenY()
        {
            if (m_pATManager == null) return false;
            return m_pATManager.GetNegScreenY();
        }
        //-----------------------------------------------------
        internal void SetATManager(AgentTreeManager pManager)
        {
            m_pATManager = pManager;
        }
        //------------------------------------------------------
        public T GetModule<T>() where T : AModule
        {
            if (m_pATManager == null) return null;
            return m_pATManager.GetFramework().GetModule<T>();
        }
        //------------------------------------------------------
        public IUserData GetOwnerClass(int hashCode)
        {
            if (m_OwnerClass == null) return null;
            IUserData userClass;
            if (m_OwnerClass.TryGetValue(hashCode, out userClass))
                return userClass;

            if (m_OwnerParentClass != null)
            {
                if (m_OwnerParentClass.TryGetValue(hashCode, out userClass)) return userClass;
            }

            int parent = ATRtti.GetClassParentTypeId(hashCode);
            while (parent != 0)
            {
                if (m_OwnerClass.TryGetValue(parent, out userClass))
                    return userClass;
                parent = ATRtti.GetClassParentTypeId(parent);
            }
            return null;
        }
        //------------------------------------------------------
        public void AddOwnerClass(IUserData pOwner, int hashCode = 0)
        {
            if (pOwner == null) return;
            if (hashCode == 0) hashCode = ATRtti.GetClassTypeId(pOwner.GetType());
            if (m_OwnerClass == null) m_OwnerClass = new Dictionary<int, IUserData>(2);
            if (m_OwnerParentClass == null) m_OwnerParentClass = new Dictionary<int, IUserData>(2);
            m_OwnerClass[hashCode] = pOwner;

            int parent = ATRtti.GetClassParentTypeId(hashCode);
            while (parent != 0)
            {
                m_OwnerParentClass[parent] = pOwner;
                parent = ATRtti.GetClassParentTypeId(parent);
            }
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
            agentTree.Init();

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
                    else if (task.type == (int)ETaskType.eMouseInput)
                    {
                        if (m_vMouseInputTask == null) m_vMouseInputTask = new HashSet<BaseNode>(2);
                        m_vMouseInputTask.Add(task);
                    }
                    else if (task.type == (int)ETaskType.eKeyInput)
                    {
                        int outport = task.GetOutportCount();
                        for(int j =0; j < outport; ++j)
                        {
                            int keyCode = GetOutportInt(task, j,-1);
                            if(keyCode>0)
                            {
                                if(m_vKeyListens == null)
                                {
                                    m_vKeyListens = new Dictionary<KeyCode, LinkedList<BaseNode>>(2);
                                }
                                KeyCode key = (KeyCode)keyCode;
                                if (!m_vKeyListens.TryGetValue(key, out var nodes))
                                {
                                    nodes = new LinkedList<BaseNode>();
                                    m_vKeyListens[key] = nodes;
                                }
                                nodes.AddLast(task);
                            }
                        }
                    }
                }
                if(m_vMouseInputTask!=null && m_vMouseInputTask.Count>0 && m_pATManager!=null)
                {
                    m_pATManager.AddMouseInputTask(this);
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
        }
        //-----------------------------------------------------
        public void Start()
        {
            if (m_bStarted)
                return;
            m_bStarted = true;
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
            Update(0);
        }
        //-----------------------------------------------------
        public void Exit()
        {
            if (m_pExitTask == null) return;
            if (m_vExecuting != null) m_vExecuting.Clear();
            else m_vExecuting = new LinkedList<BaseNode>();
            m_vExecuting.AddLast(m_pExitTask);
            Execute(m_vExecuting);
            m_pExitTask = null;
        }
        //-----------------------------------------------------
        public bool IsEnable()
        {
            return m_bEnable;
        }
        //-----------------------------------------------------
        public bool IsStarted()
        {
            return m_bStarted;
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
            if(m_bEnable) Execute(m_vExecuting);
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
                    m_vTickExecuting.AddFirst(m_pTickTask);
                }
            }

            Execute(m_vTickExecuting);
            Execute(m_vExecuting);
            m_pCurrentExcuting = null;

            if (m_vKeyListens != null)
            {
                foreach(var db in m_vKeyListens)
                {
                    if (Input.GetKeyDown(db.Key) || Input.GetKeyUp(db.Key)) 
                        KeyInputEvent(db.Value);
                }
            }

            return IsKeepUpdate();
        }
#if UNITY_EDITOR
        //-----------------------------------------------------
        internal bool EditorKeyEvent(Event evt)
        {
            bool bDo = false;
            if (m_vKeyListens != null)
            {
                bool OnEventCheck(KeyCode key)
                {
                    return m_vEditorEventKeys.Contains(key);
                }

                if (evt.type == EventType.KeyDown || evt.type == EventType.KeyUp)
                {
                    if (evt.type == EventType.KeyDown) m_vEditorEventKeys.Add(evt.keyCode);
                    else m_vEditorEventKeys.Remove(evt.keyCode);
                    foreach (var db in m_vKeyListens)
                    {
                        if (evt.keyCode == db.Key)
                        {
                            if (KeyInputEvent(db.Value, OnEventCheck))
                                bDo = true;
                        }
                    }
                }
            }
            return bDo;
        }
#endif
        //-----------------------------------------------------
        bool IsKeepUpdate()
        {
            return m_bHasCustomTask;
        }
        //-----------------------------------------------------
        void Execute(LinkedList<BaseNode> vList)
        {
            m_pCurrentExcuting = vList;
            if (vList == null)
                return;
            bool executedAny;
            int executeCount = 0;
            do
            {
                executedAny = false;
                var node = vList.First;
                while (node != null)
                {
                    executeCount++;
                    var next = node.Next;
                    var curNode = node.Value;
                    bool bSucceed = OnExecute(curNode);
                    if (bSucceed)
                        AddExecuted(curNode);
                    if (bSucceed)
                    {
                        vList.Remove(node);
                        executedAny = true;
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
            } while (executedAny && vList.Count > 0 && executeCount<1000);
            if(executeCount>=1000)
            {
                UnityEngine.Debug.LogWarning("有死循环的风险！！！");
            }
        }
        //------------------------------------------------------
        bool KeyInputEvent(LinkedList<BaseNode> vNodes, OnKeyEventDelegate onCheck = null)
        {
            if (vNodes == null)
                return false;
            bool bDo = false;
            foreach (var db in vNodes)
            {
                if (!db.HasFlag(ENodeFlag.Enable)) continue;
                int inport = db.GetOutportCount();
                bool bCanFire = true;
                for (int i = 0; i < inport; ++i)
                {
                    var key = this.GetOutportInt(db, i, -1);
                    if (key <= 0)
                    {
                        continue;
                    }
                    if(onCheck!=null)
                    {
                        if(!onCheck((KeyCode)key))
                        {
                            bCanFire = false;
                            break;
                        }
                    }
                    else if(!Input.GetKey((KeyCode)key))
                    {
                        bCanFire = false;
                        break;
                    }
                }
                if (bCanFire)
                {
                    if (m_vExecuting == null) m_vExecuting = new LinkedList<BaseNode>();
                    m_vExecuting.AddFirst(db);
                    bDo = true;
                }
            }
            return bDo;
        }
        //------------------------------------------------------
        public bool MouseInputEvent(EATMouseType mouseType, TouchInput.TouchData touchData)
        {
            if (m_vMouseInputTask == null)
                return false;
            VariableList argvs = VariableList.Malloc(5);
            argvs.AddInt((int)mouseType);
            argvs.AddInt((int)touchData.touchID);
            argvs.AddVec2(touchData.position);
            argvs.AddVec2(touchData.lastPosition);
            argvs.AddVec2(touchData.deltaPosition);
            argvs.AddBool(touchData.isUITouched);
            foreach (var db in m_vMouseInputTask)
            {
                ExecuteNode(db, argvs, false);
            }
            argvs.Release();
            return false;
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
#if UNITY_EDITOR
                Editor.AgentTreeWindow.OnAgentTreeNodeExecute(this, pNode);
#endif
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
                case (short)EActionType.eSlerp:
                case (short)EActionType.eQuaternionToEuler:
                case (short)EActionType.eEulerToQuaternion:
                case (short)EActionType.eMatrixToTRS:
                case (short)EActionType.eTRSToMatrix:
                case (short)EActionType.eMatrixMultiplyPoint:
                case (short)EActionType.eMatrixMultiplyPoint3x4:
                case (short)EActionType.eMatrixMultiplyVector:
                case (short)EActionType.eScreenToWorldPosition:
                case (short)EActionType.eWorldToScreenPosition:
                case (short)EActionType.eCheckWorldPosInView:
                    return VectorOpExecutor.OnExecutor(this, pNode);
                case (short)EActionType.eCondition: return ConditionExecutor.OnExecute(this, pNode);
            }
            if (m_vCallback != null)
            {
                for (var callback = m_vCallback.First; callback != null; callback = callback.Next)
                {
                    if (callback.Value.OnATExecutedNode(this, pNode))
                    {
                        return true;
                    }
                }
            }
            if (m_pATManager != null)
            {
                if (m_pATManager.OnNotifyExecutedNode(this, pNode))
                    return true;
            }
            return ATCallHandler.DoAction(this,pNode);
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
            m_bStarted = false;
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
            if (m_vMouseInputTask != null) m_vMouseInputTask.Clear();
            if (m_vKeyListens != null) m_vKeyListens.Clear();
            m_pExitTask = null;
            m_pTickTask = null;
            m_pStartTask = null;
            m_pCurrentExcuting = null;
            m_bHasCustomTask = false;
#if UNITY_EDITOR
            m_vEditorEventKeys.Clear();
#endif
        }
        //-----------------------------------------------------
        internal void Destroy()
        {
            Exit();
            Clear();
            if (m_pATManager != null) m_pATManager.OnDestroyAgentTree(this);
            m_pATManager = null;
            if (m_vCallback != null) m_vCallback.Clear();
            if (m_OwnerClass != null) m_OwnerClass.Clear();
            if (m_OwnerParentClass != null) m_OwnerParentClass.Clear();
        }
    }
}