/********************************************************************
生成日期:	06:30:2025
类    名: 	Actor
作    者:	HappLI
描    述:	Actor单位
*********************************************************************/
using Framework.ActorSystem.Editor;
using Framework.AT.Editor;
using Framework.Cutscene.Runtime;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Framework.ActorSystem.Runtime
{
    public class Actor : TypeObject, ICutsceneObject
    {
        public static Vector3                   INVAILD_POS = new Vector3(-9000, -9000, -9000);


        int                                     m_nInstanceID = 0;
        ActorManager                            m_pSytstem;
        List<AActorAgent>                       m_vAgents = null;
        Dictionary<System.Type, AActorAgent>    m_mTypeAgents = null;
        ActorParameter                          m_pActorParameter;

        WorldTransform                          m_Transform = new WorldTransform(Vector3.zero);
        IContextData                            m_pObjectAble;
        Transform                               m_pUnityTransform;
        private Dictionary<int, Component>      m_vComponents = null;

        ActorGraph                              m_pGraph = null;
        SkillSystem                             m_pSkillSystem = null;

        bool m_bCutsceneHold = false;
        protected ushort                        m_nFlags = (ushort)EActorFlag.Default;

        private int                             m_nFreezeCounter = 0;
        private float                           m_fFreezeDuration = 0;
        private float                           m_fDestoryDelta = 0;

        private Actor                           m_pNext = null;
        private Actor                           m_pPrev = null;
        //--------------------------------------------------------
        public ActorManager GetActorManager()
        {
            return m_pSytstem;
        }
        //--------------------------------------------------------
        internal void SetActorManager(ActorManager pSystem)
        {
            m_pSytstem = pSystem;
        }
        //--------------------------------------------------------
        public ActorParameter GetActorParameter()
        {
            if (m_pActorParameter == null) m_pActorParameter = TypeInstancePool.Malloc<ActorParameter>();
            return m_pActorParameter;
        }
        //--------------------------------------------------------
        internal void SetInstanceID(int nID)
        {
            m_nInstanceID = nID;
        }
        //--------------------------------------------------------
        public int GetInstanceID()
        {
            return m_nInstanceID;
        }
        //--------------------------------------------------------
        public void SetContextData(IContextData pData)
        {
            GetActorParameter().SetCfgData(pData);
        }
        //--------------------------------------------------------
        public IContextData GetContextData()
        {
            return GetActorParameter().GetCfgData();
        }
        //--------------------------------------------------------
        public void SetObjectAble(IContextData pObject)
        {
            if (m_pObjectAble == pObject)
            {
                m_Transform.bDirtyPos = true;
                m_Transform.bDirtyEuler = true;
                m_Transform.bDirtyScale = true;
                return;
            }

            if (m_pUnityTransform != null)
                m_pSytstem.DespawnInstance(m_pUnityTransform.gameObject);
            m_pUnityTransform = null;

            m_pObjectAble = null;
            if (IsDestroy())
            {
                return;
            }
            m_pObjectAble = pObject;

            GetActorGraph();
            if (m_vAgents != null)
            {
                for (int i = 0; i < m_vAgents.Count; ++i)
                {
                    m_vAgents[i].LoadedAble(pObject);
                }
            }

            if (pObject !=null)
            {
                if(pObject is ActorComponent)
                {
                    ActorComponent actorComp = pObject as ActorComponent;
                    m_pUnityTransform = actorComp.GetTransform();
                    if(actorComp.ActionGraphData!=null)
                        GetActorGraph().LoadActorGraph(actorComp.ActionGraphData, OnLoadActorGraph);
                }
                else if(pObject is Transform)
                {
                    m_pUnityTransform = pObject as Transform;
                }
                m_pSytstem.OnActorStatusCallback(this, EActorStatus.Loaded);
            }
            OnObjectAble(m_pObjectAble);
            UpdateTransform();
        }
        //--------------------------------------------------------
        public bool LoadActorGraph(string actorGraph)
        {
            return GetActorGraph().LoadGraph(actorGraph, OnLoadActorGraph);
        }
        //--------------------------------------------------------
        void OnLoadActorGraph(ActorGraphData graphData)
        {
            GetAgent<ActorGraphicAgent>(true).OnLoadActorGraphData(graphData);
        }
        //--------------------------------------------------------
        protected virtual void OnObjectAble(IContextData userData)
        {
        }
        //--------------------------------------------------------
        internal void OnConstruct()
        {
            if (m_pSkillSystem == null) m_pSkillSystem = TypeInstancePool.Malloc<SkillSystem>();
            m_pSkillSystem.SetActor(this);
            GetAgent<ActorGraphicAgent>(true);
            Reset();
        }
        //--------------------------------------------------------
        internal void Reset()
        {
            m_Transform.Clear();
            m_fDestoryDelta = 0;
            m_fFreezeDuration = 0;
            m_nFreezeCounter = 0;
            m_bCutsceneHold = false;
            m_nFlags = (ushort)EActorFlag.Default;
        }
        //--------------------------------------------------------
        internal void OnCreated()
        {
            if (m_pActorParameter != null)
                m_pActorParameter.SetActor(this);
        }
        //------------------------------------------------------
        internal Actor GetNext()
        {
            return m_pNext;
        }
        //------------------------------------------------------
        internal void SetNext(Actor pNode)
        {
            m_pNext = pNode;
        }
        //------------------------------------------------------
        internal Actor GetPrev()
        {
            return m_pPrev;
        }
        //------------------------------------------------------
        internal void SetPrev(Actor pNode)
        {
            m_pPrev = pNode;
        }
        //------------------------------------------------------
        public byte GetAttackGroup()
        {
            return GetActorParameter().GetAttackGroup();
        }
        //------------------------------------------------------
        public void SetAttackGroup(byte attackGroup)
        {
            GetActorParameter().SetAttackGroup(attackGroup);
        }
        //------------------------------------------------------
        public virtual bool CanAttackGroup(byte attackGroup)
        {
            return GetActorParameter().CanAttackGroup(attackGroup);
        }
        //--------------------------------------------------------
        public Vector3 GetPosition()
        {
            return m_Transform.GetPosition();
        }
        //--------------------------------------------------------
        public Vector3 GetLastPosition()
        {
            return m_Transform.GetLastPosition();
        }
        //--------------------------------------------------------
        public void SetPosition(Vector3 vPosition)
        {
            m_Transform.SetPosition(vPosition);
        }
        //--------------------------------------------------------
        public Vector3 GetEulerAngle()
        {
            return m_Transform.GetEulerAngle();
        }
        //--------------------------------------------------------
        public void SetEulerAngle(Vector3 vEulerAngle)
        {
            m_Transform.SetEulerAngle(vEulerAngle);
        }
        //--------------------------------------------------------
        public Matrix4x4 GetMatrix()
        {
            return m_Transform.GetMatrix();
        }
        //--------------------------------------------------------
        public void SetDirection(Vector3 vDirection)
        {
            if ((int)(vDirection.sqrMagnitude * 100) <= 0) return;
            m_Transform.SetDirection(vDirection);
        }
        //-------------------------------------------------
        public Vector3 GetDirection()
        {
            return m_Transform.GetDirection();
        }
        //-------------------------------------------------
        public Vector3 GetUp()
        {
            return m_Transform.GetUp();
        }
        //-------------------------------------------------
        public virtual void SetUp(Vector3 up)
        {
            m_Transform.SetUp(up);
        }
        //-------------------------------------------------
        public Vector3 GetRight()
        {
            return m_Transform.GetRight();
        }
        //-------------------------------------------------
        public Vector3 GetScale()
        {
            return m_Transform.GetScale();
        }
        //-------------------------------------------------
        public void SetScale(Vector3 scale)
        {
            m_Transform.SetScale(scale);
        }
        //--------------------------------------------------------
        public void SetTransfrom(Vector3 vPosition, Vector3 vEulerAngle, Vector3 vScale)
        {
            m_Transform.SetTransform(vPosition, vEulerAngle, vScale);
        }
        //------------------------------------------------------
        public bool IsFlag(EActorFlag flag)
        {
            return (m_nFlags & (ushort)flag) != 0;
        }
        //------------------------------------------------------
        public void SetFlag(EActorFlag flag, bool bSet)
        {
            if (bSet)
            {
                if (!IsFlag(flag))
                {
                    m_nFlags |= (ushort)flag;
                    OnFlagDirty(flag, true);
                }
            }
            else
            {
                if (IsFlag(flag))
                {
                    m_nFlags &= (ushort)(~(ushort)flag);
                    OnFlagDirty(flag, false);
                }
            }
        }
        //--------------------------------------------------------
        protected virtual void OnFlagDirty(EActorFlag flag, bool IsUsed)
        {
         //   if (m_pServerSync != null) m_pServerSync.OutSyncData(new SvrSyncData((int)EDefaultSyncType.NodeFlag, (int)flag, IsUsed ? 1 : 0));
            switch (flag)
            {
                case EActorFlag.Active:
                    {
                        if (IsUsed) m_pSytstem.OnActorStatusCallback(this, EActorStatus.Active);
                        return;
                    }
                case EActorFlag.Visible:
                    {
                        m_Transform.bDirtyPos = true;
                        m_pSytstem.OnActorStatusCallback(this, IsUsed ? EActorStatus.Visible : EActorStatus.Hide);
                        return;
                    }
                case EActorFlag.Killed:
                    {
                        ResetFreeze();
                        if (IsUsed) m_pSytstem.OnActorStatusCallback(this, EActorStatus.Killed);
                        else m_pSytstem.OnActorStatusCallback(this, EActorStatus.Revive);
                        return;
                    }
                case EActorFlag.Destroy:
                    {
                        if (IsUsed) m_pSytstem.OnActorStatusCallback(this, EActorStatus.Destroy); 
                        return;
                    }
            }
        }
        //-------------------------------------------------
        public virtual bool IsCanLogic()
        {
            return !IsFlag(EActorFlag.Killed) && IsFlag(EActorFlag.Logic) && !IsFlag(EActorFlag.Destroy) && IsFlag(EActorFlag.Active);
        }
        //------------------------------------------------------
        public void SetDelayDestroy(float fTime)
        {
            m_fDestoryDelta = fTime;
        }
        //------------------------------------------------------
        public float GetDelayDestroy()
        {
            return m_fDestoryDelta;
        }
        //------------------------------------------------------
        public void SetDestroy()
        {
            m_fDestoryDelta = 0;
            SetFlag(EActorFlag.Destroy, true);
        }
        //------------------------------------------------------
        public bool IsDestroy()
        {
            return IsFlag(EActorFlag.Destroy) || m_nInstanceID == 0;
        }
        //------------------------------------------------------
        public override void Destroy()
        {
            SetDestroy();
        }
        //------------------------------------------------------
        public void EnableHudBar(bool bHudBar)
        {
            SetFlag(EActorFlag.HudBar, bHudBar);
        }
        //------------------------------------------------------
        public bool IsEnableHudBar()
        {
            return IsFlag(EActorFlag.HudBar);
        }
        //------------------------------------------------------
        public void SetColliderAble<T>(bool bAble, bool isTrigger = false) where T : Collider
        {
#if !USE_SERVER
            if (IsColliderAble() != bAble)
            {
                if (m_pUnityTransform)
                {
                    if (bAble)
                    {
                        T collider = AddComponent<T>();
                        if (collider) collider.isTrigger = isTrigger;
                    }
                    EnableComponent<T>(bAble);
                }
            }
#endif
            SetFlag(EActorFlag.ColliderAble, bAble);
        }
        //------------------------------------------------------
        public bool IsColliderAble()
        {
            return IsFlag(EActorFlag.ColliderAble);
        }
        //------------------------------------------------------
        public void SetKilled(bool bVisible)
        {
            SetFlag(EActorFlag.Killed, bVisible);
        }
        //------------------------------------------------------
        public bool IsKilled()
        {
            return IsFlag(EActorFlag.Killed);
        }
        //------------------------------------------------------
        public bool IsDebug()
        {
            return IsFlag(EActorFlag.Debug);
        }
        //------------------------------------------------------
        public void SetDebug(bool bDebug)
        {
            SetFlag(EActorFlag.Debug, bDebug);
        }
        //------------------------------------------------------
        public void SetCollectAble(bool bVisible)
        {
            SetFlag(EActorFlag.CollectAble, bVisible);
        }
        //------------------------------------------------------
        public bool IsCollectAble()
        {
            return IsFlag(EActorFlag.CollectAble);
        }
        //------------------------------------------------------
        public void SetSpatial(bool bVisible)
        {
            SetFlag(EActorFlag.Spatial, bVisible);
        }
        //------------------------------------------------------
        public bool IsSpatial()
        {
            return IsFlag(EActorFlag.Spatial);
        }
        //------------------------------------------------------
        public void SetVisible(bool bVisible)
        {
            SetFlag(EActorFlag.Visible, bVisible);
        }
        //------------------------------------------------------
        public bool IsVisible()
        {
            return IsFlag(EActorFlag.Visible);
        }
        //------------------------------------------------------
        public void SetActived(bool bToggle)
        {
            SetFlag(EActorFlag.Active, bToggle);
        }
        //------------------------------------------------------
        public bool IsActived()
        {
            return IsFlag(EActorFlag.Active);
        }
        //------------------------------------------------------
        public void EnableLogic(bool bToggle)
        {
            SetFlag(EActorFlag.Logic, bToggle);
        }
        //------------------------------------------------------
        public bool IsLogicEnable()
        {
            return IsFlag(EActorFlag.Logic);
        }
        //------------------------------------------------------
        public void EnableAI(bool bToggle)
        {
            SetFlag(EActorFlag.AI, bToggle);
        }
        //------------------------------------------------------
        public bool IsEnableAI()
        {
            return IsFlag(EActorFlag.AI);
        }
        //------------------------------------------------------
        public void EnableRVO(bool bToggle)
        {
            SetFlag(EActorFlag.RVO, bToggle);
        }
        //------------------------------------------------------
        public bool IsEnableRVO()
        {
            return IsFlag(EActorFlag.RVO);
        }
        //--------------------------------------------------------
        public T GetAgent<T>(bool bAutoCreate = false) where T : AActorAgent, new()
        {
            if (m_mTypeAgents != null && m_mTypeAgents.TryGetValue(typeof(T), out var agent))
                return agent as T;

            T pAgent = null;
            if (bAutoCreate)
            {
                pAgent = AddAgent<T>();
                pAgent.Init();
                if (m_pObjectAble != null)
                {
                    pAgent.LoadedAble(m_pObjectAble);
                }
            }
            return pAgent;
        }
        //--------------------------------------------------------
        public void AddAgent(AActorAgent pAgent)
        {
            if (pAgent == null)
                return;
            if (m_vAgents == null)
            {
                m_vAgents = new List<AActorAgent>(2);
                m_mTypeAgents = new Dictionary<System.Type, AActorAgent>(2);
            }
            else
            {
                if (m_vAgents.Contains(pAgent)) return;
            }
            pAgent.SetActor(this);
            m_vAgents.Add(pAgent);
            m_mTypeAgents[pAgent.GetType()] = pAgent;
        }
        //--------------------------------------------------------
        public T AddAgent<T>() where T : AActorAgent, new()
        {
            if (m_vAgents == null)
            {
                m_vAgents = new List<AActorAgent>(2);
                m_mTypeAgents = new Dictionary<System.Type, AActorAgent>(2);
            }
            else
            {
                if (m_mTypeAgents.TryGetValue(typeof(T), out var pAgentThis))
                    return pAgentThis as T;
            }
            T pAgent = new T();
            pAgent.SetActor(this);
            m_vAgents.Add(pAgent);
            m_mTypeAgents[pAgent.GetType()] = pAgent;
            return pAgent;
        }
        //--------------------------------------------------------
        public void DelAgent(AActorAgent pAgent, bool bAutoDestroy = true)
        {
            if (pAgent == null) return;
            if (m_vAgents != null)
            {
                m_vAgents.Remove(pAgent);
            }
            pAgent.SetActor(null);
            if (bAutoDestroy) pAgent.Destroy();
        }
        //--------------------------------------------------------
        public ActorGraph GetActorGraph()
        {
            if (m_pGraph == null)
            {
                m_pGraph = TypeInstancePool.Malloc<ActorGraph>();
                m_pGraph.Init(this);
                m_pGraph.AddStartActionCallback(OnActionStartState);
                m_pGraph.AddEndActionCallback(OnActionEndState);
            }
            return m_pGraph;
        }
        //--------------------------------------------------------
        public ActorGraphData GetGraphData()
        {
            return GetActorGraph().GetGraphData();
        }
        //--------------------------------------------------------
        public void StartActionState(EActionStateType eType, uint nTag = 0, bool bForce = false, IContextData pStateParam = null)
        {
            ActorGraphicAgent pAgent = GetAgent<ActorGraphicAgent>();
            if (null == pAgent)
                return;
            var action = pAgent.GetActorAction(eType, nTag);
            if (action == null)
                return;
            if (action.GetPlayCutscene() != null)
            {
                GetActorGraph().Play(action.GetPlayCutscene(), action, pStateParam);
                return;
            }
            pAgent.PlayAnimation(eType, nTag, bForce);
        }
        //--------------------------------------------------------
        public void StartActionState(uint nActionTypeAndTag, bool bForce = false, IContextData pStateParam = null)
        {
            ActorGraphicAgent pAgent = GetAgent<ActorGraphicAgent>();
            if (null == pAgent)
                return;
            var action = pAgent.GetActorAction(nActionTypeAndTag);
            if (action == null)
                return;
            if (action.GetPlayCutscene() != null)
            {
                GetActorGraph().Play(action.GetPlayCutscene(), action);
                return;
            }
            pAgent.PlayAnimation(action, 0.1f, bForce);
        }
        //--------------------------------------------------------
        public void StartActionState(ActorAction pAction, float blendTime = 0.1f, bool bForce = false, IContextData pStateParam = null)
        {
            if (pAction == null)
                return;
            if (pAction.GetPlayCutscene() != null)
            {
                GetActorGraph().Play(pAction.GetPlayCutscene(), pAction, pStateParam);
                return;
            }
            ActorGraphicAgent pAgent = GetAgent<ActorGraphicAgent>();
            if (null == pAgent)
                return;
            pAgent.PlayAnimation(pAction, blendTime, bForce);
        }
        //--------------------------------------------------------
        public void StopActionState(EActionStateType eType, uint nTag = 0)
        {
            ActorGraphicAgent pAgent = GetAgent<ActorGraphicAgent>();
            if (null == pAgent)
                return;
            var action = pAgent.GetActorAction(eType, nTag);
            if (action != null && action.GetPlayCutscene() != null)
            {
                GetActorGraph().Stop(action);
                return;
            }

            pAgent.StopAnimation(eType, nTag);
        }
        //--------------------------------------------------------
        public void SetIdleType(EActionStateType eType, uint tag = 0)
        {
            ActorGraphicAgent pAgent = GetAgent<ActorGraphicAgent>();
            if (null == pAgent)
                return;
            pAgent.SetIdleType(eType, tag);
        }
        //--------------------------------------------------------
        public void RemoveActionState(EActionStateType eType, uint nTag = 0)
        {
            ActorGraphicAgent pAgent = GetAgent<ActorGraphicAgent>();
            if (null == pAgent)
                return;
            pAgent.RemoveActionState(eType, nTag);
        }
        //--------------------------------------------------------
        void OnActionEndState(ActorAction pState)
        {
            if (m_pSkillSystem != null)
                m_pSkillSystem.OnActionEndState(pState);
        }
        //--------------------------------------------------------
        void OnActionStartState(ActorAction pState)
        {
            if (m_pSkillSystem != null)
                m_pSkillSystem.OnActionStartState(pState);
        }
        //--------------------------------------------------------
        public void SetAttrs(byte[] attiTypes, int[] values)
        {
            GetActorParameter().SetAttrs(attiTypes, values);
        }
        //--------------------------------------------------------
        public void SetAttr(byte type, int value)
        {
            GetActorParameter().SetAttr(type, value);
        }
        //--------------------------------------------------------
        public int GetAttr(byte type, int defVal = 0)
        {
            return GetActorParameter().GetAttr(type, defVal);
        }
        //--------------------------------------------------------
        public void RemoveAttr(byte type)
        {
            GetActorParameter().RemoveAttr(type);
        }
        //--------------------------------------------------------
        public void AppendAttrs(byte[] attiTypes, int[] values)
        {
            GetActorParameter().AppendAttrs(attiTypes, values);
        }
        //--------------------------------------------------------
        public void AppendAttr(byte type, int value)
        {
            GetActorParameter().AppendAttr(type, value);
        }
        //--------------------------------------------------------
        public void SubAttrs(byte[] attiTypes, int[] values)
        {
            GetActorParameter().SubAttrs(attiTypes, values);
        }
        //--------------------------------------------------------
        public void SubAttr(byte type, int value, bool bLowerZero = false)
        {
            GetActorParameter().SubAttr(type, value, bLowerZero);
        }
        //--------------------------------------------------------
        public void ClearAttrs()
        {
            GetActorParameter().ClearAttrs();
        }
        //--------------------------------------------------------
        public ActorAction GetAction(EActionStateType eType, uint nTag)
        {
            ActorGraphicAgent pAgent = GetAgent<ActorGraphicAgent>();
            if (null == pAgent)
                return null;
            return pAgent.GetActorAction(eType, nTag);
        }
        //--------------------------------------------------------
        public ActorAction GetAction(string actionName)
        {
            ActorGraphicAgent pAgent = GetAgent<ActorGraphicAgent>();
            if (null == pAgent)
                return null;
            return pAgent.GetActorAction(actionName);
        }
        //--------------------------------------------------------
        public uint GetCurrentPlayActionStatePriority(uint layer)
        {
            ActorGraphicAgent pAgent = GetAgent<ActorGraphicAgent>();
            if (null == pAgent)
                return 0;

            return pAgent.GetCurrentPlayActionStatePriority(layer);
        }
        //--------------------------------------------------------
        public ActorAction GetCurrentPlayActionState(uint layer = 0)
        {
            ActorGraphicAgent pAgent = GetAgent<ActorGraphicAgent>();
            if (null == pAgent)
                return null;

            return pAgent.GetCurrentPlayActionState(layer);
        }
        //--------------------------------------------------------
        public AActorStateInfo GetStateParam()
        {
            if (m_pSkillSystem != null) return m_pSkillSystem.GetCurrentSkill();
            return null;
        }
        //--------------------------------------------------------
        public bool IsInAction(EActionStateType eType)
        {
            if (m_pGraph != null)
            {
                if (m_pGraph.IsInAction(eType))
                    return true;
            }
            ActorGraphicAgent pAgent = GetAgent<ActorGraphicAgent>();
            if (null == pAgent)
                return false;
            return pAgent.IsInAction(eType);
        }
        //--------------------------------------------------------
        public SkillSystem GetSkillSystem()
        {
            if (m_pSkillSystem == null) m_pSkillSystem = TypeInstancePool.Malloc<SkillSystem>();
            m_pSkillSystem.SetActor(this);
            return m_pSkillSystem;
        }
        //--------------------------------------------------------
        public bool IsAttacking()
        {
            if (m_pSkillSystem == null) return false;
            return m_pSkillSystem.GetCurrentSkill(false) != null;
        }
        //--------------------------------------------------------
        public IContextData GetObjectAble()
        {
            return m_pObjectAble;
        }
        //--------------------------------------------------------
        public T GetComponent<T>(bool bFindChild = false) where T : Component
        {
            if (m_pObjectAble == null || !(m_pObjectAble is MonoBehaviour)) return null;
            MonoBehaviour monoBehaviour = m_pObjectAble as MonoBehaviour;
            int hashCode = typeof(T).GetHashCode();
            if (m_vComponents == null)
            {
                m_vComponents = new Dictionary<int, Component>(4);
            }
            Component retCom;
            if (m_vComponents.TryGetValue(hashCode, out retCom))
                return retCom as T;

            retCom = monoBehaviour.GetComponent<T>();
            if (retCom == null && bFindChild)
            {
                retCom = monoBehaviour.GetComponentInChildren<T>();
            }
            m_vComponents[hashCode] = retCom;
            return retCom as T;
        }
        //--------------------------------------------------------
        public T AddComponent<T>(bool bNullNew = true, System.Type type = null) where T : Component
        {
            if (m_pObjectAble == null || !(m_pObjectAble is MonoBehaviour)) return null;
            MonoBehaviour monoBehaviour = m_pObjectAble as MonoBehaviour;
            if (type == null) type = typeof(T);
            int hashCode = type.GetHashCode();
            if (m_vComponents != null)
            {
                Component outCom;
                if (m_vComponents.TryGetValue(hashCode, out outCom))
                {
                    if (!bNullNew)
                    {
                        return outCom as T;
                    }
                }
            }
            T newComp = monoBehaviour.gameObject.AddComponent(type) as T;
            if (newComp == null) return null;
            if (m_vComponents == null) m_vComponents = new Dictionary<int, Component>(2);
            m_vComponents[hashCode] = newComp;
            return newComp;
        }
        //------------------------------------------------------
        public void EnableComponent<T>(bool bEnabled) where T : Component
        {
            if (m_vComponents == null) return;
            int hashCode = typeof(T).GetHashCode();
            Component retCom;
            if (m_vComponents.TryGetValue(hashCode, out retCom))
            {
                if (retCom is Behaviour) ((Behaviour)retCom).enabled = bEnabled;
                else if (retCom is Collider) ((Collider)retCom).enabled = bEnabled;
            }
        }
        //--------------------------------------------------------
        internal void Update(float fFrame)
        {
            if (IsFlag(EActorFlag.Destroy))
                return;
            if (m_pGraph != null)
            {
                m_pGraph.Update(fFrame);
            }
            if (m_vAgents != null)
            {
                for (int i = 0; i < m_vAgents.Count; ++i)
                    m_vAgents[i].Update(fFrame);
            }
            UpdateTransform();
            if (m_fDestoryDelta > 0)
            {
                m_fDestoryDelta -= fFrame;
                if (m_fDestoryDelta <= 0)
                {
                    SetDestroy();
                }
            }
        }
        //--------------------------------------------------------
        internal void FreeDestroy()
        {
            if (m_pGraph != null)
            {
                m_pGraph.Free();
                m_pGraph = null;
            }
            if (m_pSkillSystem != null)
            {
                m_pSkillSystem.Free();
                m_pSkillSystem = null;
            }
            if (m_vAgents != null)
            {
                for (int i = 0; i < m_vAgents.Count; ++i)
                    m_vAgents[i].Free();
                m_vAgents.Clear();
            }
            if (m_mTypeAgents != null) m_mTypeAgents.Clear();
            m_nInstanceID = 0;
            if(m_pActorParameter!=null)
            {
                m_pActorParameter.Free();
                m_pActorParameter = null;
            }
            m_Transform.Clear();
            if (m_vComponents != null) m_vComponents.Clear();
            if (m_pUnityTransform != null)
            {
                m_pSytstem.DespawnInstance(m_pUnityTransform.gameObject);
                m_pUnityTransform = null;
            }
            m_pObjectAble = null;
            m_pUnityTransform = null;
            m_pSytstem = null;
            Reset();
        }
        //--------------------------------------------------------
        public UnityEngine.Object GetUniyObject()
        {
            if (m_pUnityTransform == null) return null;
            return m_pUnityTransform.gameObject;
        }
        //--------------------------------------------------------
        public Transform GetUniyTransform()
        {
            return m_pUnityTransform;
        }
        //--------------------------------------------------------
        public Animator GetAnimator()
        {
            return GetComponent<Animator>(true);
        }
        //--------------------------------------------------------
        public Camera GetCamera()
        {
            return null;
        }
        //--------------------------------------------------------
        public bool SetParameter(EParamType type, CutsceneParam paramData)
        {
            switch (type)
            {
                case EParamType.ePlayAction:
                    {
                        var animationClip = paramData.ToUnityObject<AnimationClip>();
                        if (animationClip != null)
                        {
                            ActorGraphicAgent pAgent = GetAgent<ActorGraphicAgent>();
                            if (null == pAgent)
                                return false;
                            pAgent.PlayAnimation(paramData.userData, animationClip, paramData.ToInt(0));
                            pAgent.SetActionTime(paramData.userData, paramData.ToFloat(1));
                        }
                        else
                        {
                            string actionName = paramData.ToString();
                            if (string.IsNullOrEmpty(actionName))
                            {
                                EActionStateType eType = (EActionStateType)paramData.ToInt(0);
                                uint tag = (uint)paramData.ToInt(1);
                                StartActionState(eType, tag, false);
                            }
                            else
                            {
                                ActorGraphicAgent pAgent = GetAgent<ActorGraphicAgent>();
                                if (null == pAgent)
                                    return false;
                                pAgent.PlayAnimation(actionName);
                                pAgent.SetActionTime(actionName, paramData.ToFloat(0));
                            }
                        }
                    }
                    return true;
                case EParamType.eStopAction:
                    {
                        string actionName = paramData.ToString();
                        var animationClip = paramData.ToUnityObject<AnimationClip>();
                        if (animationClip != null)
                        {
                            ActorGraphicAgent pAgent = GetAgent<ActorGraphicAgent>();
                            if (null == pAgent)
                                return false;
                            pAgent.StopAnimation(paramData.userData);
                        }
                        else if (string.IsNullOrEmpty(actionName))
                        {
                            EActionStateType eType = (EActionStateType)paramData.ToInt(0);
                            uint tag = (uint)paramData.ToInt(1);
                            StopActionState(eType, tag);
                        }
                        else
                        {
                            ActorGraphicAgent pAgent = GetAgent<ActorGraphicAgent>();
                            if (null == pAgent)
                                return false;
                            pAgent.StopAnimation(actionName);
                        }
                    }
                    return true;
                case EParamType.eActionSpeed:
                    {
                        ActorGraphicAgent pAgent = GetAgent<ActorGraphicAgent>();
                        if (null == pAgent)
                            return false;
                        if (paramData.userData != null)
                        {
                            pAgent.SetActionSpeed((IContextData)paramData.userData, paramData.ToFloat(1));
                        }
                        else if (string.IsNullOrEmpty(paramData.strData))
                        {
                            pAgent.SetActionSpeed(paramData.strData, paramData.ToFloat(1));
                        }
                        else
                            pAgent.SetActionSpeed(paramData.ToFloat());
                    }
                    return true;
            }
            return false;
        }
        //--------------------------------------------------------
        public bool GetParameter(EParamType type, ref CutsceneParam paramData)
        {
            switch (type)
            {
                case EParamType.ePosition:
                    {
                        paramData.SetVector3(GetPosition());
                        return true;
                    }
                case EParamType.eEulerAngle:
                    {
                        paramData.SetVector3(GetEulerAngle());
                        return true;
                    }
                case EParamType.eQuraternion:
                    {
                        paramData.SetQuaternion(Quaternion.Euler(GetEulerAngle()));
                        return true;
                    }
                case EParamType.eScale:
                    {
                        paramData.SetVector3(GetScale());
                        return true;
                    }
                case EParamType.eHold:
                    {
                        paramData.SetBool(m_bCutsceneHold);
                        return true;
                    }
                case EParamType.eBindSlotMatrix:
                    {
                        var matrix = GetEventBindSlot(paramData.ToString(), -1);
                        paramData.SetMatrix(matrix);
                        return true;
                    }
            }
            return false;
        }
        //------------------------------------------------------
        protected void UpdateTransform()
        {
            if (IsFlag(EActorFlag.Visible))
            {
                bool bDirty = false;
                if (m_Transform.bDirtyPos)
                {
                    if (m_pUnityTransform != null)
                    {
                        m_pUnityTransform.position=m_Transform.GetPosition();
                    }
                    m_Transform.bDirtyPos = false;
                //    if (m_pServerSync != null) m_pServerSync.OutSyncData(new SvrSyncData((int)EDefaultSyncType.Position, m_Transform.GetPosition()));
                    OnDirtyPosition();
                    bDirty = true;
                }
                if (m_Transform.bDirtyEuler)
                {
                    if (m_pUnityTransform != null)
                    {
                        m_pUnityTransform.eulerAngles =m_Transform.GetEulerAngle();
                    }
                    m_Transform.bDirtyEuler = false;
               //     if (m_pServerSync != null) m_pServerSync.OutSyncData(new SvrSyncData((int)EDefaultSyncType.EulerAngle, m_Transform.GetEulerAngle()));
                    OnDirtyEulerAngle();
                    bDirty = true;
                }
                if (m_Transform.bDirtyScale)
                {
                    if (m_pUnityTransform != null)
                    {
                        m_pUnityTransform.localScale =m_Transform.GetScale();
                    }
                    m_Transform.bDirtyScale = false;
               //     if (m_pServerSync != null) m_pServerSync.OutSyncData(new SvrSyncData((int)EDefaultSyncType.Scale, m_Transform.GetScale()));
                    OnDirtyScale();
                    bDirty = true;
                }
            }
            else
            {
                if (m_pUnityTransform != null) m_pUnityTransform.position =INVAILD_POS;
            }
        }
        //------------------------------------------------------
        protected virtual void OnDirtyPosition() { }
        //------------------------------------------------------
        protected virtual void OnDirtyEulerAngle() { }
        //------------------------------------------------------
        protected virtual void OnDirtyScale() { }
        //------------------------------------------------------
        public bool IsCutscneHolded()
        {
#if USE_CUTSCENE
            return m_bCutsceneHold;
#else
            return false;
#endif
        }
        //------------------------------------------------------
        private void UpdateFreeze(float fFrameTime)
        {
            if (m_nFreezeCounter <= 0) return;
            if (m_fFreezeDuration > 0)
            {
                m_fFreezeDuration -= fFrameTime;
                if (m_fFreezeDuration <= 0)
                    ResetFreeze();
            }
        }
        //------------------------------------------------------
        public void ResetFreeze()
        {
            bool isFreezeCall = IsFreezed();
            m_nFreezeCounter = 0;
            m_fFreezeDuration = 0;
            if (isFreezeCall)
                OnFreezed(IsFreezed());
        }
        //------------------------------------------------------
        public void Freezed(bool bToggle, float fDuration)
        {
            bool isFreezeCall = IsFreezed();
            if (bToggle)
            {
                if (m_fFreezeDuration > 0)
                {
                    m_nFreezeCounter = 1;
                    if (fDuration > 0) m_fFreezeDuration = fDuration;
                }
                else
                {
                    m_fFreezeDuration = fDuration;
                    if (fDuration > 0) m_nFreezeCounter = 1;
                    else m_nFreezeCounter++;
                }
            }
            else
            {
                m_nFreezeCounter--;
                if (m_nFreezeCounter <= 0)
                {
                    m_nFreezeCounter = 0;
                    m_fFreezeDuration = 0;
                }
            }
            if (isFreezeCall != IsFreezed())
            {
                //if (m_pServerSync != null) m_pServerSync.OutSyncData(new SvrSyncData((int)EDefaultSyncType.Freeze, freeze));
                OnFreezed(IsFreezed());
            }
        }
        //------------------------------------------------------
        public bool IsFreezed()
        {
            return m_nFreezeCounter > 0;
        }
        //------------------------------------------------------
        protected virtual void OnFreezed(bool freeze)
        {
            
        }
        //------------------------------------------------------
        public virtual Matrix4x4 GetEventBindSlot(string strSlot, int bindSlot)
        {
            if (bindSlot == 0 || m_pUnityTransform == null) return GetMatrix();
            if (string.IsNullOrEmpty(strSlot)) return GetMatrix();
            if (strSlot.Equals("Root", StringComparison.OrdinalIgnoreCase)) return GetMatrix();
            if (strSlot.Equals("RootTop", StringComparison.OrdinalIgnoreCase))
            {
                Matrix4x4 temp = GetMatrix();
                ActorSystemUtil.OffsetPosition(ref temp, new Vector3(0, m_pActorParameter.GetModelHeight(), 0));
                return temp;
            }
            Matrix4x4 matrix = GetMatrix();

            ActorComponent actorComp = m_pObjectAble as ActorComponent;
            if (actorComp == null)
                return matrix;

            var tranform = actorComp.GetTransform();
            if (tranform)
            {
                if ((bindSlot & (int)ESlotBindBit.Rotation) != 0)
                {
                    matrix = tranform.localToWorldMatrix;
                    if ((bindSlot & (int)ESlotBindBit.Position) == 0) ActorSystemUtil.UpdatePosition(ref matrix, GetPosition());
                    if ((bindSlot & (int)ESlotBindBit.Scale) == 0) ActorSystemUtil.UpdateScale(ref matrix, GetScale());
                }
                else
                {
                    if ((bindSlot & (int)ESlotBindBit.Position) != 0) ActorSystemUtil.UpdatePosition(ref matrix, tranform.position);
                    if ((bindSlot & (int)ESlotBindBit.Scale) != 0) ActorSystemUtil.UpdateScale(ref matrix, tranform.localScale);
                }
            }

            return matrix;
        }
    }
}