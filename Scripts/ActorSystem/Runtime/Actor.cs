/********************************************************************
生成日期:	06:30:2025
类    名: 	Actor
作    者:	HappLI
描    述:	Actor单位
*********************************************************************/
using Framework.AT.Runtime;
using Framework.Core;
using Framework.Cutscene.Runtime;
using System;
using System.Collections.Generic;
using UnityEngine;
#if USE_FIXEDMATH
using ExternEngine;
#else
using FFloat = System.Single;
using FMatrix4x4 = UnityEngine.Matrix4x4;
using FQuaternion = UnityEngine.Quaternion;
using FVector2 = UnityEngine.Vector2;
using FVector3 = UnityEngine.Vector3;
#endif
namespace Framework.ActorSystem.Runtime
{
    //--------------------------------------------------------
    //! Actor
    //--------------------------------------------------------
    [ATExport("Actor",1)]
    public class Actor : TypeActor, ICutsceneObject
    {
        public static FVector3 INVAILD_POS = new FVector3(-9000, -9000, -9000);


        int                                     m_nInstanceID = 0;
        protected ActorManager                  m_pSytstem;
        List<AActorAgent>                       m_vAgents = null;
        Dictionary<System.Type, AActorAgent>    m_mTypeAgents = null;
        ActorParameter                          m_pActorParameter;

        protected WorldTransform                m_Transform = new WorldTransform(FVector3.zero);
        protected WorldBoundBox                 m_BoundBox = new WorldBoundBox();
        ActorContext                            m_pObjectAble = new ActorContext();
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
        [ATMethod]
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
        void DestroyObjectAble()
        {
            m_pObjectAble.Clear(this);
            if (m_vComponents != null) m_vComponents.Clear();
        }
        //--------------------------------------------------------
        public void SetObjectContext(IContextData pObject)
        {
            if (m_pObjectAble.pContextData == pObject)
            {
                m_Transform.bDirtyPos = true;
                m_Transform.bDirtyEuler = true;
                m_Transform.bDirtyScale = true;
                return;
            }

            DestroyObjectAble();
            m_pObjectAble.SetContextData(this, pObject);
            DoObjectAble();
        }
        //--------------------------------------------------------
        public void SetObjectAble(UnityEngine.Object pObject)
        {
            if (m_pObjectAble.pUnityObject == pObject)
            {
                m_Transform.bDirtyPos = true;
                m_Transform.bDirtyEuler = true;
                m_Transform.bDirtyScale = true;
                return;
            }

            if(pObject is AActorComponent)
            {
                SetObjectContext((AActorComponent)pObject);
                return;
            }

            DestroyObjectAble();
            m_pObjectAble.SetContextData(this, pObject);
            DoObjectAble();
        }
        //--------------------------------------------------------
        public void SetObjectAble(UnityEngine.Transform pTransform)
        {
            if (m_pObjectAble.pUnityTransform == pTransform)
            {
                m_Transform.bDirtyPos = true;
                m_Transform.bDirtyEuler = true;
                m_Transform.bDirtyScale = true;
                return;
            }

            DestroyObjectAble();
            m_pObjectAble.SetContextData(this, pTransform);
            DoObjectAble();
        }
        //--------------------------------------------------------
        public void SetObjectAble(UnityEngine.GameObject pGo)
        {
            if (m_pObjectAble.pUnityGameObject == pGo)
            {
                m_Transform.bDirtyPos = true;
                m_Transform.bDirtyEuler = true;
                m_Transform.bDirtyScale = true;
                return;
            }

            DestroyObjectAble();
            m_pObjectAble.SetContextData(this, pGo);
            DoObjectAble();
        }
        //--------------------------------------------------------
        void DoObjectAble()
        {
            if (IsDestroy())
            {
                return;
            }
            m_Transform.bDirtyPos = true;
            m_Transform.bDirtyEuler = true;
            m_Transform.bDirtyScale = true;
            GetActorGraph();
            if (m_vAgents != null)
            {
                for (int i = 0; i < m_vAgents.Count; ++i)
                {
                    m_vAgents[i].LoadedAble(m_pObjectAble);
                }
            }

            if (m_pObjectAble.pUnityObject != null && m_pObjectAble.pUnityObject is AActorComponent)
            {
                var actorComp = m_pObjectAble.pUnityObject as AActorComponent;
                if (actorComp.ActionGraphData != null)
                    GetActorGraph().LoadActorGraph(actorComp.ActionGraphData, OnLoadActorGraph);
            }
            OnObjectAble(m_pObjectAble);
            UpdateTransform();
            m_pSytstem.OnActorStatusCallback(this, EActorStatus.Loaded);
        }
        //--------------------------------------------------------
        protected void BreakObjectAble()
        {
            m_pObjectAble.ClearNoDespawn();
        }
        //--------------------------------------------------------
        public bool LoadActorGraph(string actorGraph)
        {
            return GetActorGraph().LoadGraph(actorGraph, OnLoadActorGraph);
        }
        //--------------------------------------------------------
        void OnLoadActorGraph(ActorGraphData graphData)
        {
            if(graphData!=null)
            {
                SetBound(graphData.boundBox.min, graphData.boundBox.max);
            }
            GetAgent<ActorGraphicAgent>(true).OnLoadActorGraphData(graphData);
        }
        //--------------------------------------------------------
        protected virtual void OnObjectAble(ActorContext userData)
        {
        }
        //--------------------------------------------------------
        internal void OnConstruct()
        {
            Reset();
            SetActived(false);
            SetVisible(false);
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
            OnReset();
        }
        //--------------------------------------------------------
        protected virtual void OnReset() { }
        //--------------------------------------------------------
        internal void OnCreated()
        {
            if (m_pActorParameter != null)
                m_pActorParameter.SetActor(this);
            UpdateTransform();
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
        [ATMethod("获取攻击组")]
        public byte GetAttackGroup()
        {
            return GetActorParameter().GetAttackGroup();
        }
        //------------------------------------------------------
        [ATMethod("设置攻击组")]
        public void SetAttackGroup(byte attackGroup)
        {
            GetActorParameter().SetAttackGroup(attackGroup);
        }
        //------------------------------------------------------
        [ATMethod("是否可攻击")]
        public virtual bool CanAttackGroup(byte attackGroup)
        {
            return GetActorParameter().CanAttackGroup(attackGroup);
        }
        //--------------------------------------------------------
        [ATMethod("设置速度")]
        public void SetSpeed(FVector3 speed)
        {
            GetAgent<ActorTransformLogic>(true).SetSpeed(speed);
        }
        //--------------------------------------------------------
        [ATMethod("设置速度XZ")]
        public void SetSpeedXZ(FVector3 speed)
        {
            GetAgent<ActorTransformLogic>(true).SetSpeedXZ(speed);
        }
        //--------------------------------------------------------
        [ATMethod("获取当前速度")]
        public FVector3 GetSpeed()
        {         
            return GetAgent<ActorTransformLogic>(true).GetSpeed();
        }
        //--------------------------------------------------------
        [ATMethod("设置Y速度")]
        public void SetSpeedY(FFloat fSpeedY)
        {
            GetAgent<ActorTransformLogic>(true).SetSpeedY(fSpeedY);
        }
        //--------------------------------------------------------
        [ATMethod("设置摩檫力")]
        public void SetFraction(FFloat fValue)
        {
            GetAgent<ActorTransformLogic>(true).SetFarction(fValue);
        }
        //--------------------------------------------------------
        [ATMethod("获取摩檫力")]
        public FFloat GetFraction()
        {
            return GetAgent<ActorTransformLogic>(true).GetFarction();
        }
        //------------------------------------------------------
        [ATMethod("启用重力")]
        public void EnableGravity(bool bEnable)
        {
            GetAgent<ActorTransformLogic>(true).EnableGravity(bEnable);
        }
        //--------------------------------------------------------
        [ATMethod("设置重力")]
        public void SetGravity(FFloat fValue)
        {
            GetAgent<ActorTransformLogic>(true).SetGravity(fValue);
        }
        //--------------------------------------------------------
        [ATMethod("获取重力")]
        public FFloat GetGravity()
        {
            return GetAgent<ActorTransformLogic>(true).GetGravity();
        }
        //--------------------------------------------------------
        [ATMethod("获取当前位置")]
        public FVector3 GetPosition()
        {
            return m_Transform.GetPosition();
        }
        //--------------------------------------------------------
        [ATMethod("获取上一次位置")]
        public FVector3 GetLastPosition()
        {
            return m_Transform.GetLastPosition();
        }
        //--------------------------------------------------------
        [ATMethod("设置位置")]
        public void SetPosition(FVector3 vPosition)
        {
            m_Transform.SetPosition(vPosition);
        }
        //--------------------------------------------------------
        [ATMethod("位置偏移")]
        public void OffsetPosition(FVector3 vOffset)
        {
            m_Transform.SetPosition(vOffset + m_Transform.GetPosition());
        }
        //--------------------------------------------------------
        [ATMethod("获取角度")]
        public FVector3 GetEulerAngle()
        {
            return m_Transform.GetEulerAngle();
        }
        //--------------------------------------------------------
        [ATMethod("设置角度")]
        public void SetEulerAngle(FVector3 vEulerAngle)
        {
            m_Transform.SetEulerAngle(vEulerAngle);
        }
        //--------------------------------------------------------
        public Matrix4x4 GetMatrix()
        {
            return m_Transform.GetMatrix();
        }
        //-------------------------------------------------
        public WorldBoundBox GetBounds()
        {
            return m_BoundBox;
        }
        //--------------------------------------------------------
        public void SetBound(FVector3 min, FVector3 max)
        {
            m_BoundBox.Set(min, max);
        }
        //--------------------------------------------------------
        [ATMethod("设置方向")]
        public void SetDirection(FVector3 vDirection)
        {
            if ((int)(vDirection.sqrMagnitude * 100) <= 0) return;
            m_Transform.SetDirection(vDirection);
        }
        //-------------------------------------------------
        [ATMethod("获取方向")]
        public FVector3 GetDirection()
        {
            return m_Transform.GetDirection();
        }
        //-------------------------------------------------
        [ATMethod("获取Up朝向")]
        public FVector3 GetUp()
        {
            return m_Transform.GetUp();
        }
        //-------------------------------------------------
        [ATMethod("设置Up朝向")]
        public virtual void SetUp(FVector3 up)
        {
            m_Transform.SetUp(up);
        }
        //-------------------------------------------------
        [ATMethod("获取Right朝向")]
        public FVector3 GetRight()
        {
            return m_Transform.GetRight();
        }
        //-------------------------------------------------
        [ATMethod("获取缩放")]
        public FVector3 GetScale()
        {
            return m_Transform.GetScale();
        }
        //-------------------------------------------------
        [ATMethod("设置缩放")]
        public void SetScale(FVector3 scale)
        {
            m_Transform.SetScale(scale);
        }
        //--------------------------------------------------------
        [ATMethod("设置位置角度缩放")]
        public void SetTransfrom(FVector3 vPosition, FVector3 vEulerAngle, FVector3 vScale)
        {
            m_Transform.SetTransform(vPosition, vEulerAngle, vScale);
        }
        //------------------------------------------------------
        [ATMethod("是否拥有标志")]
        public bool IsFlag(EActorFlag flag)
        {
            return (m_nFlags & (ushort)flag) != 0;
        }
        //------------------------------------------------------
        [ATMethod("设置标志")]
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
            if (m_pSytstem == null)
                return;
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
        [ATMethod("是否开启逻辑")]
        public virtual bool IsCanLogic()
        {
            return !IsFlag(EActorFlag.Killed) && IsFlag(EActorFlag.Logic) && !IsFlag(EActorFlag.Destroy) && IsFlag(EActorFlag.Active);
        }
        //--------------------------------------------------------
        [ATMethod("是否不可见")]
        public bool IsInvincible()
        {
            return false;
        }
        //------------------------------------------------------
        [ATMethod("设置延迟删除时间")]
        public void SetDelayDestroy(float fTime)
        {
            m_fDestoryDelta = fTime;
        }
        //------------------------------------------------------
        [ATMethod("获取延迟删除时间")]
        public float GetDelayDestroy()
        {
            return m_fDestoryDelta;
        }
        //------------------------------------------------------
        [ATMethod("删除")]
        public void SetDestroy()
        {
            m_fDestoryDelta = 0;
            SetFlag(EActorFlag.Destroy, true);
        }
        //------------------------------------------------------
        [ATMethod("是否删除")]
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
        [ATMethod("HUD开关")]
        public void EnableHudBar(bool bHudBar)
        {
            SetFlag(EActorFlag.HudBar, bHudBar);
        }
        //------------------------------------------------------
        [ATMethod("是否开启HUD")]
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
                if (m_pObjectAble.pUnityGameObject)
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
        [ATMethod("是否拥有物理碰撞")]
        public bool IsColliderAble()
        {
            return IsFlag(EActorFlag.ColliderAble);
        }
        //------------------------------------------------------
        [ATMethod]
        public void SetKilled(bool bKilled)
        {
            SetFlag(EActorFlag.Killed, bKilled);
        }
        //------------------------------------------------------
        [ATMethod]
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
        [ATMethod]
        public void SetVisible(bool bVisible)
        {
            SetFlag(EActorFlag.Visible, bVisible);
        }
        //------------------------------------------------------
        [ATMethod]
        public bool IsVisible()
        {
            return IsFlag(EActorFlag.Visible);
        }
        //------------------------------------------------------
        [ATMethod]
        public void SetActived(bool bToggle)
        {
            SetFlag(EActorFlag.Active, bToggle);
        }
        //------------------------------------------------------
        [ATMethod]
        public bool IsActived()
        {
            return IsFlag(EActorFlag.Active);
        }
        //------------------------------------------------------
        [ATMethod]
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
        [ATMethod]
        public void EnableAI(bool bToggle)
        {
            SetFlag(EActorFlag.AI, bToggle);
        }
        //------------------------------------------------------
        [ATMethod]
        public bool IsEnableAI()
        {
            return IsFlag(EActorFlag.AI);
        }
        //------------------------------------------------------
        [ATMethod]
        public void EnableRVO(bool bToggle)
        {
            SetFlag(EActorFlag.RVO, bToggle);
        }
        //------------------------------------------------------
        [ATMethod]
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
                if (m_pObjectAble.IsValid())
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
        [ATMethod]
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
        [ATMethod]
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
        [ATMethod]
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
        [ATMethod]
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
        [ATMethod]
        public void SetIdleType(EActionStateType eType, uint tag = 0)
        {
            ActorGraphicAgent pAgent = GetAgent<ActorGraphicAgent>();
            if (null == pAgent)
                return;
            pAgent.SetIdleType(eType, tag);
        }
        //--------------------------------------------------------
        [ATMethod]
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
        public void SetAttrs(byte[] attiTypes, FFloat[] values)
        {
            GetActorParameter().SetAttrs(attiTypes, values);
        }
        //--------------------------------------------------------
        [ATMethod]
        public void SetAttr(byte type, FFloat value)
        {
            GetActorParameter().SetAttr(type, value);
        }
        //--------------------------------------------------------
        [ATMethod]
        public FFloat GetAttr(byte type, FFloat defVal = 0)
        {
            return GetActorParameter().GetAttr(type, defVal);
        }
        //--------------------------------------------------------
        [ATMethod]
        public void RemoveAttr(byte type)
        {
            GetActorParameter().RemoveAttr(type);
        }
        //--------------------------------------------------------
        public void AppendAttrs(byte[] attiTypes, FFloat[] values)
        {
            GetActorParameter().AppendAttrs(attiTypes, values);
        }
        //--------------------------------------------------------
        [ATMethod]
        public void AppendAttr(byte type, FFloat value)
        {
            GetActorParameter().AppendAttr(type, value);
        }
        //--------------------------------------------------------
        public void SubAttrs(byte[] attiTypes, FFloat[] values)
        {
            GetActorParameter().SubAttrs(attiTypes, values);
        }
        //--------------------------------------------------------
        [ATMethod]
        public void SubAttr(byte type, int value, bool bLowerZero = false)
        {
            GetActorParameter().SubAttr(type, value, bLowerZero);
        }
        //--------------------------------------------------------
        [ATMethod]
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
        [ATMethod]
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
        [ATMethod]
        public bool IsAttacking()
        {
            if (m_pSkillSystem == null) return false;
            return m_pSkillSystem.GetCurrentSkill(false) != null;
        }
        //--------------------------------------------------------
        public ActorContext GetObjectAble()
        {
            return m_pObjectAble;
        }
        //--------------------------------------------------------
        public T GetComponent<T>(bool bFindChild = false) where T : Component
        {
            if (m_pObjectAble.pUnityGameObject == null ) return null;
            int hashCode = typeof(T).GetHashCode();
            if (m_vComponents == null)
            {
                m_vComponents = new Dictionary<int, Component>(4);
            }
            Component retCom;
            if (m_vComponents.TryGetValue(hashCode, out retCom))
                return retCom as T;

            retCom = m_pObjectAble.pUnityGameObject.GetComponent<T>();
            if (retCom == null && bFindChild)
            {
                retCom = m_pObjectAble.pUnityGameObject.GetComponentInChildren<T>();
            }
            m_vComponents[hashCode] = retCom;
            return retCom as T;
        }
        //--------------------------------------------------------
        public T AddComponent<T>(bool bNullNew = true, System.Type type = null) where T : Component
        {
            if (m_pObjectAble.pUnityGameObject == null) return null;
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
            T newComp = m_pObjectAble.pUnityGameObject.AddComponent(type) as T;
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
            OnUpdate(fFrame);
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
        protected virtual void OnUpdate(float fFrame)
        {

        }
        //--------------------------------------------------------
        public UnityEngine.Object GetUniyObject()
        {
            return m_pObjectAble.pUnityObject;
        }
        //--------------------------------------------------------
        public Transform GetUniyTransform()
        {
            return m_pObjectAble.pUnityTransform;
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
                    if (m_pObjectAble.pUnityTransform)
                    {
                        m_pObjectAble.pUnityTransform.position=m_Transform.GetPosition();
                    }
                    m_Transform.bDirtyPos = false;
                    //    if (m_pServerSync != null) m_pServerSync.OutSyncData(new SvrSyncData((int)EDefaultSyncType.Position, m_Transform.GetPosition()));
                    ActorManager actorManager = GetActorManager();
                    if (actorManager != null)
                    {
                        actorManager.UpdateActorSpatialIndex(this);
                    }
                    OnDirtyPosition();
                    bDirty = true;
                }
                if (m_Transform.bDirtyEuler)
                {
                    if (m_pObjectAble.pUnityTransform)
                    {
                        m_pObjectAble.pUnityTransform.eulerAngles =m_Transform.GetEulerAngle();
                    }
                    m_Transform.bDirtyEuler = false;
               //     if (m_pServerSync != null) m_pServerSync.OutSyncData(new SvrSyncData((int)EDefaultSyncType.EulerAngle, m_Transform.GetEulerAngle()));
                    OnDirtyEulerAngle();
                    bDirty = true;
                }
                if (m_Transform.bDirtyScale)
                {
                    if (m_pObjectAble.pUnityTransform)
                    {
                        m_pObjectAble.pUnityTransform.localScale =m_Transform.GetScale();
                    }
                    m_Transform.bDirtyScale = false;
               //     if (m_pServerSync != null) m_pServerSync.OutSyncData(new SvrSyncData((int)EDefaultSyncType.Scale, m_Transform.GetScale()));
                    OnDirtyScale();
                    bDirty = true;
                }
                if(bDirty)
                    m_BoundBox.SetTransform(m_Transform.GetMatrix());
            }
            else
            {
                if (m_pObjectAble.pUnityTransform) m_pObjectAble.pUnityTransform.position =INVAILD_POS;
            }
        }
        //------------------------------------------------------
        protected virtual void OnDirtyPosition() 
        {
        }
        //------------------------------------------------------
        protected virtual void OnDirtyEulerAngle() { }
        //------------------------------------------------------
        protected virtual void OnDirtyScale() { }
        //------------------------------------------------------
        [ATMethod]
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
        [ATMethod]
        public void ResetFreeze()
        {
            bool isFreezeCall = IsFreezed();
            m_nFreezeCounter = 0;
            m_fFreezeDuration = 0;
            if (isFreezeCall)
                OnFreezed(IsFreezed());
        }
        //------------------------------------------------------
        [ATMethod]
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
        [ATMethod]
        public bool IsFreezed()
        {
            return m_nFreezeCounter > 0;
        }
        //------------------------------------------------------
        protected virtual void OnFreezed(bool freeze)
        {
            
        }
        //------------------------------------------------------
        public virtual bool IsIntersecition(Actor pNode)
        {
            if (pNode == null)
                return false;
            WorldBoundBox bound = pNode.GetBounds();
            if (IntersetionUtil.CU_LineOBBIntersection(m_pSytstem.GetIntersetionParam(), GetLastPosition(), GetPosition(), bound.GetCenter(), bound.GetHalf(), pNode.GetMatrix()) ||
                IntersetionUtil.CU_OBBOBBIntersection(m_pSytstem.GetIntersetionParam(), bound.GetCenter(), bound.GetHalf(), pNode.GetMatrix(), GetBounds().GetCenter(), GetBounds().GetHalf(), GetMatrix()))
                return true;
            return false;
        }
        //------------------------------------------------------
        public virtual bool IsIntersecition(Matrix4x4 mtTrans, FVector3 vCenter, FVector3 vHalf)
        {
            if (IntersetionUtil.CU_LineOBBIntersection(m_pSytstem.GetIntersetionParam(), GetLastPosition(), GetPosition(), vCenter, vHalf, mtTrans))
            {
                return true;
            }
            if (IntersetionUtil.CU_OBBOBBIntersection(m_pSytstem.GetIntersetionParam(), vCenter, vHalf, mtTrans, GetBounds().GetCenter(), GetBounds().GetHalf(), GetMatrix()))
                return true;
            return false;
        }
        //------------------------------------------------------
        public virtual bool IsIntersecition(Matrix4x4 mtTrans, float radius)
        {
            FVector3 vTransCenter = ActorSystemUtil.GetPosition(mtTrans);
            if (IntersetionUtil.CU_LineSphereIntersection(m_pSytstem.GetIntersetionParam(), GetLastPosition(), GetPosition(), vTransCenter, radius))
            {
                return true;
            }
            if (IntersetionUtil.CU_SphereAABBInstersection(vTransCenter, radius, GetBounds().GetMin(true), GetBounds().GetMax(true)))
                return true;
            return false;
        }
        //------------------------------------------------------
        public virtual Matrix4x4 GetEventBindSlot(string strSlot, int bindSlot = (int)ESlotBindBit.All)
        {
            if (bindSlot == 0 || m_pObjectAble.pUnityTransform == null) return GetMatrix();
            if (string.IsNullOrEmpty(strSlot)) return GetMatrix();
            if (strSlot.Equals("Root", StringComparison.OrdinalIgnoreCase)) return GetMatrix();
            if (strSlot.Equals("RootTop", StringComparison.OrdinalIgnoreCase))
            {
                Matrix4x4 temp = GetMatrix();
                ActorSystemUtil.OffsetPosition(ref temp, new FVector3(0, m_pActorParameter.GetModelHeight(), 0));
                return temp;
            }
            Matrix4x4 matrix = GetMatrix();

            if (m_pObjectAble.pUnityObject == null) return matrix;
            AActorComponent actorComp = m_pObjectAble.pUnityObject as AActorComponent;
            if (actorComp == null)
                return matrix;

            Transform slotTransform = null;
            slotTransform = actorComp.GetSlot(strSlot,out var slotOffset);
            if(slotTransform == null) slotTransform = actorComp.GetTransform();
            if (slotTransform)
            {
                if ((bindSlot & (int)ESlotBindBit.Rotation) != 0)
                {
                    matrix = slotTransform.localToWorldMatrix;
                    if ((bindSlot & (int)ESlotBindBit.Position) == 0) ActorSystemUtil.UpdatePosition(ref matrix, GetPosition()+ slotOffset);
                    if ((bindSlot & (int)ESlotBindBit.Scale) == 0) ActorSystemUtil.UpdateScale(ref matrix, GetScale());
                }
                else
                {
                    if ((bindSlot & (int)ESlotBindBit.Position) != 0) ActorSystemUtil.UpdatePosition(ref matrix, slotTransform.position + slotOffset);
                    if ((bindSlot & (int)ESlotBindBit.Scale) != 0) ActorSystemUtil.UpdateScale(ref matrix, slotTransform.localScale);
                }
            }

            return matrix;
        }
        //------------------------------------------------------
        public virtual Transform GetEventBindSlot(string strSlot, out FVector3 slotOffset)
        {
            slotOffset = FVector3.zero;
            if (m_pObjectAble.pUnityObject == null) return null;
            if (string.IsNullOrEmpty(strSlot)) return null;
            if (strSlot.Equals("Root", StringComparison.OrdinalIgnoreCase)) return m_pObjectAble.pUnityTransform;
            Matrix4x4 matrix = GetMatrix();
            AActorComponent actorComp = m_pObjectAble.pUnityObject as AActorComponent;
            if (actorComp == null)
                return m_pObjectAble.pUnityTransform;

            return actorComp.GetSlot(strSlot,out slotOffset);
        }
        //--------------------------------------------------------
        internal void FreeDestroy()
        {
            OnDestroy();
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
            m_pObjectAble.Clear(this);
            m_pSytstem = null;
            Reset();
        }
        //--------------------------------------------------------
        protected virtual void OnDestroy()
        { 
        }
        //--------------------------------------------------------
        public override int GetHashCode()
        {
            return m_nInstanceID;
        }
#if UNITY_EDITOR
        //--------------------------------------------------------
        public virtual void DrawDebug()
        {
            UnityEditor.Handles.Label(GetPosition(), this.GetType().Name + ":" + GetInstanceID().ToString() + " guid:" + GetActorParameter().GetGUID());
            ActorSystemUtil.DrawBoundingBox(m_BoundBox.GetCenter(), m_BoundBox.GetHalf(), GetMatrix(), Color.green, true);
        }
#endif
    }
}