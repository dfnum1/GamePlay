/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	ProjectileManager
作    者:	HappLI
描    述:	飞行道具管理器
*********************************************************************/
#if USE_FIXEDMATH
using ExternEngine;
#else
using FFloat = System.Single;
using FVector3 = UnityEngine.Vector3;
using FMatrix4x4 = UnityEngine.Matrix4x4;
#endif
using System;
using System.Collections.Generic;
using UnityEngine;
using Framework.ActorSystem.Runtime;
using Framework.AT.Runtime;
#if USE_SERVER
using Transform = ExternEngine.Transform;
#endif

namespace Framework.ActorSystem.Runtime
{
    //------------------------------------------------------
    internal class ProjectileManager : TypeObject
    {
        struct BoundProjectile
        {
            public ProjectileActor projectile;
            public int bound_count;
            public byte attackGroup;
            public Actor pTarget;
            public Actor hitActor;
        }
        public enum EEventType
        {
            Step,
            Attacker,
            Over,
            Hit,
        }
        struct Event
        {
            public Actor node_ptr;
            public Actor target_ptr;
            public EEventType type;
            public ProjectileActor pProjectile;
            public int eventId;

            public Vector3 runtime_pos;
            public Vector3 runtime_dir;
            public Vector3 runtime_hit;

            public AActorStateInfo stateParam;

            public Event(EEventType type, ProjectileActor pProjectile, Actor node_ptr, Actor target_ptr, int eventId, Vector3 runtime_hit, AActorStateInfo stateParam)
            {
                this.eventId = eventId;
                this.pProjectile = pProjectile;
                this.type = type;
                this.node_ptr = node_ptr;
                this.target_ptr = target_ptr;
                runtime_pos = pProjectile.GetPosition();
                runtime_dir = pProjectile.GetDirection();
                this.runtime_hit = runtime_hit;
                this.stateParam = stateParam;
            }
        }

        private ActorManager                        m_pActorManager = null;

        private Dictionary<int, ProjectileActor>    m_mRunningProjectile;

        Dictionary<uint, ProjectileData>            m_vDatas = null;

        private List<BoundProjectile>               m_vPrepareBounds = new List<BoundProjectile>(4);

        private List<Event>                         m_vTickEventTemps = new List<Event>(4);

        private List<ProjectileActor>               m_CatchTemp = null;

        List<ProjectileActor>                       m_vRangeExplodeProjectiles = null;
        List<ProjectileActor>                       m_vCounteractProjectiles = null;

        //------------------------------------------------------
        public ProjectileManager()
        {
        }
        //------------------------------------------------------
        ~ProjectileManager()
        {
            m_CatchTemp = null;
            m_vPrepareBounds = null;
            m_mRunningProjectile = null;
        }
        //------------------------------------------------------
        public void Awake(ActorManager actorMgr)
        {
            m_pActorManager = actorMgr;
            m_CatchTemp = new List<ProjectileActor>(16);
            if (m_vTickEventTemps == null) m_vTickEventTemps = new List<Event>(4);
            m_vTickEventTemps.Clear();
             m_mRunningProjectile = new Dictionary<int, ProjectileActor>(16);
            if (m_vPrepareBounds == null) m_vPrepareBounds = new List<BoundProjectile>(4);
        }
        //------------------------------------------------------
        public void SetProjectileDatas(ProjectileDatas projectileDatas)
        {
            if (projectileDatas == null)
                return;
            if (m_vDatas == null) m_vDatas = new Dictionary<uint, ProjectileData>();
            m_vDatas.Clear();
            projectileDatas.GetDatas(m_vDatas);
        }
        //------------------------------------------------------
        public ProjectileData GetProjectileData(uint nId)
        {
            if (m_vDatas == null)
                return null;
            ProjectileData outData;
            if (m_vDatas.TryGetValue(nId, out outData))
                return outData;
            return null;
        }
        //------------------------------------------------------
        public override void Destroy()
        {
            Clear();
        }
        //------------------------------------------------------
        public void Clear()
        {
            m_vPrepareBounds.Clear();
            m_vTickEventTemps.Clear();
            StopAllProjectiles();
            if (m_vCounteractProjectiles != null) m_vCounteractProjectiles.Clear();
            if (m_vRangeExplodeProjectiles != null) m_vRangeExplodeProjectiles.Clear();
        }
        //------------------------------------------------------
        public void StopProjectileByOwner(Actor pNode, float fLaucherTime)
        {
            if (pNode == null) return;
            foreach(var db in m_mRunningProjectile)
            {
                ProjectileActor pProjectile = db.Value;
                if(pProjectile.GetOwnerActor() == pNode)
                {
                    if(pProjectile.GetDelayTime()>0 || pProjectile.GetRunningTime() <= fLaucherTime)
                    {
                        pProjectile.SetRemainLifeTime(0);
                    }
                }
            }
        }
        //------------------------------------------------------
        public void Update(float fFrameTime)
        {
            for (int i = 0; i < m_vPrepareBounds.Count; ++i)
            {
                DoBoundLaunchProjectile(m_vPrepareBounds[i]);
            }
            m_vPrepareBounds.Clear();

            if (m_vCounteractProjectiles == null) m_vCounteractProjectiles = new List<ProjectileActor>(4);
            m_vCounteractProjectiles.Clear();
            if (m_vRangeExplodeProjectiles == null) m_vRangeExplodeProjectiles = new List<ProjectileActor>(4);
            m_vRangeExplodeProjectiles.Clear();

            ProjectileActor pProjectile = null;
            foreach (var db in m_mRunningProjectile)
            {
                pProjectile = db.Value;
                if(pProjectile.IsDestroy() || pProjectile.IsKilled())
                {
                    m_CatchTemp.Add(pProjectile);
                    continue;
                }
                if(pProjectile.CheckStoped(this,fFrameTime))
                {
                    m_CatchTemp.Add(pProjectile);
                    continue;
                }

                if(pProjectile.CheckIntersection(this, out var bCounteract, out var bExplode, out var bHitScene))
                {
                    if (bCounteract)
                        m_vCounteractProjectiles.Add(pProjectile);
                    if (bExplode)
                        m_vRangeExplodeProjectiles.Add(pProjectile);
                 //   DoProjectileHit(pProjectile, pAttacker, 0, ref m_AttackDataArray, false, bHitScene, pProjectile.IsPenetrable() ? pProjectile.GetRemainHitCount() : 1);
                }
            }

            for (int i = 0; i < m_vTickEventTemps.Count; ++i)
            {
                Event evt = m_vTickEventTemps[i];
                switch(evt.type)
                {
                    case EEventType.Attacker:
                        {
                            DoAttackEvent(evt);
                        }
                        break;
                    case EEventType.Hit:
                        {
                            DoHitEvent(evt);
                        }
                        break;
                    case EEventType.Step:
                        {
                            DoStepEvent(evt);
                        }
                        break;
                    case EEventType.Over:
                        {
                            DoOverEvent(evt);
                        }
                        break;
                }
            }
            m_vTickEventTemps.Clear();

            for (int i = 0; i < m_CatchTemp.Count; ++i)
            {
                StopProjectile(m_CatchTemp[i]);
            }
            m_CatchTemp.Clear();
        }
        //------------------------------------------------------
        internal void AddEvent(EEventType type, ProjectileActor pProjectile)
        {
         //   var projectileData = pProjectile.GetProjectileData();
         //   if (projectileData == null)
        //        return;
         //   m_vTickEventTemps.Add(new Event(type, pProjectile, pProjectile.GetOwnerActor(), null, projectileData.StepEventID, pProjectile.GetPosition(), pProjectile.GetStateParam()));
        }
        //------------------------------------------------------
        public void TrackCheck(Actor pTargetActor, FVector3 vPosition, ProjectileData pData, Transform pTrackTransform, ref Transform pTrackSlot, ref int damage_power, ref uint track_frame_id, ref uint track_body_id, ref FVector3 trackOffset)
        {
            if (m_pActorManager == null)
                return;
            damage_power = 0;
            track_frame_id = 0xffffffff;
            track_body_id = 0xffffffff;
            pTrackSlot = null;
            trackOffset = FVector3.zero;
            if (pData == null) return;
            if (ProjectileData.IsTrack(pData.type))
            {
                if (pTrackTransform == null)
                {
                    if (pData.track_target_slot != null && pData.track_target_slot.Length > 0)
                    {
                        int randomIndex = 0;
                        if (m_pActorManager != null) randomIndex = m_pActorManager.GetRandom(0, pData.track_target_slot.Length);
                        else randomIndex = UnityEngine.Random.Range(0, pData.track_target_slot.Length);

                        pTrackSlot = pTargetActor.GetEventBindSlot(pData.track_target_slot[randomIndex], out var slotOffset);
                        if (pTrackSlot == null)
                            pTrackSlot = pTargetActor.GetUniyTransform();
                        else
                            trackOffset = slotOffset;
                    }
                    else
                    {
                        pTrackSlot = pTargetActor.GetUniyTransform();
                    }
                }
                else
                    pTrackSlot = pTrackTransform;
            }
        }
        //------------------------------------------------------
        public int LaunchProjectile(uint dwProjectileTableID, Actor pOwnerActor, AActorStateInfo stateParam,
        FVector3 vPosition, FVector3 vDirection, Actor targetNode = null, int dwAssignedID = 0,
        float fDelta =0, Transform pTrackTransform = null, List<IUserData> vResults = null)
        {
            if (pOwnerActor == null) return 0;
            ProjectileData pData = GetProjectileData(dwProjectileTableID);
            if (pData == null)
            {
                Debug.LogError("飞行道具" + pData.id + "  数据不存在!");
                return 0;
            }
            return LaunchProjectile(pData, pOwnerActor, stateParam, vPosition, vDirection, targetNode, dwAssignedID, fDelta, pTrackTransform, vResults);
        }
        //------------------------------------------------------
        public int LaunchProjectile(ProjectileData pData, Actor pOwnerActor, AActorStateInfo stateParam,
        FVector3 vPosition, FVector3 vDirection, Actor targetNode = null, int dwAssignedID = 0,
        float fDelta = 0, Transform pTrackTransform = null, List<IUserData> vResults = null)
        {
            if (pOwnerActor == null || pData == null) return 0;
            int dwID = 0;
            uint dwProjectileTableID = pData.id;
            if(m_pActorManager == null)
            {
                Debug.LogError("Actor系统没有赋值:m_pActorManager 为空!");
                return dwID;
            }
#if UNITY_EDITOR
            if (pData.life_time <= 0)
            {
                Debug.LogError("飞行道具" + pData.id + " 没生命时长!");
                return dwID;
            }
#endif
#if UNITY_EDITOR
            if (pData.hit_count <= 0)
            {
                Debug.LogError("飞行道具" + pData.id + " 没有攻击次数!");
                return dwID;
            }
#endif
            if (targetNode != null && (targetNode.IsFlag(EActorFlag.Killed) || targetNode.IsDestroy()))
            {
#if UNITY_EDITOR
                Debug.LogWarning("飞行道具" + pData.id + " 没发射成功，因为目标已阵亡!");
#endif
                return dwID;
            }
            if (pData.speedLows != null && pData.speedLows.Length > 1) dwAssignedID = 0;

            FVector3 up = FVector3.up;

            int damage_power = 0;
            uint track_frame_id = 0xffffffff;
            uint track_body_id = 0xffffffff;
            Transform pTrackSlot = null;
            FVector3 trackOffset = FVector3.zero;
            TrackCheck(targetNode, vPosition, pData, pTrackTransform, ref pTrackSlot, ref damage_power, ref track_frame_id, ref track_body_id, ref trackOffset);
            if (pData.type != EProjectileType.TrackPath && pData.speedLows != null && pData.speedLows.Length > 0)
            {
                if (pData.speedLows.Length > 1) dwAssignedID = 0;
                for (int i = 0; i < pData.speedLows.Length; ++i)
                {
                    ProjectileActor pProjectile = m_pActorManager.CreateActor<ProjectileActor>(pData, null, (int)dwAssignedID);
                    pProjectile.Reset();
                    pProjectile.SetData(pData, pOwnerActor, targetNode, vPosition, vDirection, damage_power, track_frame_id, track_body_id);
                    pProjectile.SetSpeed(vPosition, vDirection, i, pOwnerActor.IsFlag(EActorFlag.Facing2D));
                    pProjectile.SetTrack(pTrackSlot, trackOffset);
                    pProjectile.SetDelayTime((FFloat)(fDelta + pData.launch_delay + 0.0666f));
                    pProjectile.SetStateParam(stateParam);

                    m_mRunningProjectile.Add(pProjectile.GetInstanceID(), pProjectile);
                    if (vResults != null) vResults.Add(pProjectile);

                    pProjectile.SetVisible(true);
                    pProjectile.SetActived(true);
                    pProjectile.EnableLogic(true);
                    pProjectile.SetSpatial(true);
                    pProjectile.SetCollectAble(false);

                    //! call back
                    OnLaunchProjectileCallback(pProjectile);
                }
            }
            else if (pData.life_time > 0)
            {
                if (pData.type == EProjectileType.TrackPath)
                {
                    if (!pData.IsValidTrackPath())
                    {
                        return dwID;
                    }
                }

                ProjectileActor pProjectile = m_pActorManager.CreateActor<ProjectileActor>(pData);
                pProjectile.Reset();

                pProjectile.SetData(pData, pOwnerActor, targetNode, vPosition, vDirection, damage_power, track_frame_id, track_body_id);
                pProjectile.SetStateParam(stateParam);
                pProjectile.SetDelayTime((FFloat)(fDelta + pData.launch_delay + 0.0666f));
                pProjectile.SetTrack(pTrackSlot, trackOffset);

                if (pData.type == EProjectileType.TrackPath)
                {
                    pProjectile.BuildTrackPathKeyframe(vPosition, pProjectile.GetTargetPosition());
                }

                m_mRunningProjectile.Add(pProjectile.GetInstanceID(), pProjectile);
                if (vResults != null) vResults.Add(pProjectile);

                pProjectile.SetVisible(true);
                pProjectile.SetActived(true);
                pProjectile.EnableLogic(true);
                pProjectile.SetSpatial(true);
                pProjectile.SetCollectAble(false);

                //! call back
                OnLaunchProjectileCallback(pProjectile);
            }
            return dwID;
        }
        //------------------------------------------------------
        uint DoBoundLaunchProjectile(BoundProjectile bound)
        {
            if(m_pActorManager == null)
            {
                Debug.LogError("Actor系统没有赋值:m_pActorManager 为空!");
                return 0xffffffff;
            }
            uint dwID = 0xffffffff;
            var csvData = bound.projectile.GetProjectileData();
            if (csvData == null)
                return dwID;
            if (bound.pTarget == null) return dwID;
            if (bound.pTarget.IsFlag(EActorFlag.Killed)) return dwID;
            float dist = (bound.pTarget.GetPosition() - bound.projectile.GetPosition()).sqrMagnitude;
            if (dist <= 1) return dwID;
            FVector3 up = FVector3.up;

            int damage_power = 0;
            uint track_frame_id = 0xffffffff;
            uint track_body_id = 0xffffffff;
            Transform pTrackSlot = null;
            FVector3 trackOffset = FVector3.zero;
            FVector3 vPosition = bound.projectile.GetPosition();
            FVector3 vDirection = (bound.pTarget.GetPosition() - bound.projectile.GetPosition()).normalized;
            TrackCheck(bound.pTarget, vPosition, bound.projectile.GetProjectileData(), null, ref pTrackSlot, ref damage_power, ref track_frame_id, ref track_body_id, ref trackOffset);
            if(pTrackSlot!=null) vDirection = (pTrackSlot.position - bound.projectile.GetPosition()).normalized;
            if (csvData.speedLows != null && csvData.speedLows.Length > 0)
            {
                for (int i = 0; i < csvData.speedLows.Length; ++i)
                {
                    ProjectileActor pProjectile = m_pActorManager.CreateActor<ProjectileActor>(csvData);
                    pProjectile.Reset();
                    pProjectile.SetData(csvData, bound.projectile.GetOwnerActor(), bound.pTarget, vPosition, vDirection, damage_power, track_frame_id, track_body_id);
                    pProjectile.SetBoundSpeed(vPosition, vDirection, i);
                    pProjectile.SetTrack(pTrackSlot, trackOffset);
                    pProjectile.SetOffsetEulerAngle(bound.projectile.GetOffsetEulerAngle());
                    pProjectile.SetStateParam(bound.projectile.GetStateParam());
                    pProjectile.SetBindOwnerSlot(bound.projectile.GetBindOwnerSlot());
                    pProjectile.SetRemainHitCount(bound.projectile.GetRemainHitCount()+1);
                    if (bound.hitActor != null) pProjectile.RecodeHit(bound.hitActor.GetInstanceID(), 10);
                    pProjectile.CopyBoundeds(bound.projectile);

                    pProjectile.SetBoundStartActor(bound.hitActor);
                    pProjectile.SetBoundProjectile(true);
                    pProjectile.SetRemainBoundCount(bound.projectile.GetRemainBoundCount());
                    pProjectile.SetDamagePower(bound.projectile.GetDamagePower());

                    if (bound.attackGroup !=0xff) pProjectile.SetAttackGroup(bound.attackGroup);
                    if (bound.projectile.IsBoundInvert())
                    {
                        if (bound.bound_count%2==0)
                        {
                         //   if (csvData.bound_damage_id > 0)
                         //   {
                         //       pProjectile.SetDamageId((uint)csvData.bound_damage_id);
                         //   }
                         //   else pProjectile.SetDamageId((uint)csvData.damage);
                        }
                        else
                        {
                            pProjectile.SetDamageId((uint)csvData.damage);
                        }
                    }
                    else
                    {
                      //  if (csvData.bound_damage_id > 0)
                      //  {
                      //      pProjectile.SetDamageId((uint)csvData.bound_damage_id);
                      //  }
                    }
                    if (bound.bound_count>=0 && (csvData.bound_flag & (int)EBoundFlag.BoundDamageAdd) != 0)
                        pProjectile.SetDamageId(pProjectile.GetDamageID() + (uint)bound.bound_count);


                    m_mRunningProjectile.Add(pProjectile.GetInstanceID(), pProjectile);

                    pProjectile.SetVisible(true);
                    pProjectile.SetActived(true);
                    pProjectile.EnableLogic(true);
                    pProjectile.SetSpatial(true);
                    pProjectile.SetCollectAble(false);
                    //! call back
                    OnLaunchProjectileCallback(pProjectile);
                }
            }
            else if (csvData.life_time > 0)
            {
                ProjectileActor pProjectile = m_pActorManager.CreateActor<ProjectileActor>(csvData);
                pProjectile.Reset();

                pProjectile.SetData(csvData, bound.projectile.GetOwnerActor(), bound.pTarget, vPosition, vDirection, damage_power, track_frame_id, track_body_id);
                pProjectile.SetStateParam(bound.projectile.GetStateParam());
                pProjectile.SetOffsetEulerAngle(bound.projectile.GetOffsetEulerAngle());
                pProjectile.SetTrack(pTrackSlot, trackOffset);
                pProjectile.SetBindOwnerSlot(bound.projectile.GetBindOwnerSlot());
                pProjectile.SetRemainHitCount(bound.projectile.GetRemainHitCount());
                pProjectile.SetRemainBoundCount(bound.projectile.GetRemainBoundCount());
                pProjectile.SetBoundStartActor(bound.hitActor);
                pProjectile.SetBoundProjectile(true);
                pProjectile.SetDamagePower(bound.projectile.GetDamagePower());
                if (bound.hitActor != null) pProjectile.RecodeHit(bound.hitActor.GetInstanceID(), 10);
                pProjectile.CopyBoundeds(bound.projectile);

                if (bound.attackGroup != 0xff) pProjectile.SetAttackGroup(bound.attackGroup);
                if (bound.projectile.IsBoundInvert())
                {
                    if (bound.bound_count % 2 == 0)
                    {
                       // if (csvData.bound_damage_id > 0)
                       // {
                       //     pProjectile.SetDamageId((uint)csvData.bound_damage_id);
                       // }
                       // else pProjectile.SetDamageId( (uint)csvData.damage);
                        
                    }
                    else
                    {
                        pProjectile.SetDamageId((uint)csvData.damage);
                    }
                }
                else
                {
                   // if (csvData.bound_damage_id > 0)
                   // {
                   //     pProjectile.SetDamageId((uint)csvData.bound_damage_id);
                   // }
                }
				if (bound.bound_count>=0 && (csvData.bound_flag & (int)EBoundFlag.BoundDamageAdd) != 0)
                	pProjectile.SetDamageId(pProjectile.GetDamageID() + (uint)bound.bound_count);
                m_mRunningProjectile.Add(pProjectile.GetInstanceID(), pProjectile);

                pProjectile.SetVisible(true);
                pProjectile.SetActived(true);
                pProjectile.EnableLogic(true);
                pProjectile.SetSpatial(true);
                pProjectile.SetCollectAble(false);
                //! call back
                OnLaunchProjectileCallback(pProjectile);
            }
            return dwID;
        }
        //------------------------------------------------------
        void OnLaunchProjectileCallback(ProjectileActor pProjectile)
        {
#if !USE_SERVER
            var projectileData = pProjectile.GetProjectileData();
            if (projectileData == null)
                return;
            if (!string.IsNullOrEmpty(projectileData.waring_effect))
            {
                pProjectile.TestFinalDropPos(Time.deltaTime);
            }
#endif
        }
        //------------------------------------------------------
        public uint BoundLaunchProjectile(ProjectileActor projectile, Actor pTarget, Actor pHitActor, int boundCnt, byte boundAttackGroup=0xff, bool bImmdeate = true)
        {
            uint dwID = 0xffffffff;
            if (pTarget == null) return dwID;
            if (projectile.GetRemainBoundCount() <= 0) return dwID;
            float dist = (pTarget.GetPosition() - projectile.GetPosition()).sqrMagnitude;
            if (dist <= 1) return dwID;

       //     projectile.delta = 0.1f;
            projectile.SetRemainLifeTime(0.1f);
            projectile.AddBounded(pTarget);

            BoundProjectile bound = new BoundProjectile();
            bound.projectile = projectile;
            bound.pTarget = pTarget;
            bound.hitActor = pHitActor;
            bound.bound_count = boundCnt;
            bound.attackGroup = boundAttackGroup;
            if (bImmdeate)
            {
                return DoBoundLaunchProjectile(bound);
            }
            else
            {
                m_vPrepareBounds.Add(bound);
            }
            return dwID;
        }
        //------------------------------------------------------
        public ProjectileActor FindProjectile(int nLaunchID)
        {
            ProjectileActor pProjectile = null;
            if (m_mRunningProjectile.TryGetValue(nLaunchID, out pProjectile))
                return pProjectile;
            return null;
        }
        //------------------------------------------------------
        public Dictionary<int, ProjectileActor> GetRunningProjectile()
        {
            return m_mRunningProjectile;
        }
        //------------------------------------------------------
        public void DoProjectileHit(ProjectileActor pProjectile, Actor pAttacker, uint dwSkillLevel, ref HashSet<HitFrameActor> attack_data_array, bool bUseAttackData, bool bHitScene, int hitCount = 1, bool bExplode = false)
        {
            pProjectile.SubHitCount(hitCount);
            pProjectile.ResetHitStepDelta();
        //    float fAppendOnHitActionRate = 0f;
          //  AttackFrameUtil.CU_PlayOnHitActorActions(pAttacker as Actor, dwSkillLevel, pProjectile.projectile, EVolumeType.Attack, fAppendOnHitActionRate,
          //      pProjectile.position, pProjectile.direction, ref attack_data_array, bUseAttackData);

            if (pAttacker == null)
                pAttacker = pProjectile.GetOwnerActor();

            var projectileData = pProjectile.GetProjectileData();
            if (projectileData == null)
                return;

            foreach(var attackArray in attack_data_array )
            {
             //   if (projectileData.HitEventID>0)
             //       m_vTickEventTemps.Add(new Event(EEventType.Hit, pProjectile, attackArray.target_ptr, null, projectileData.HitEventID, attackArray.hit_position, pProjectile.GetStateParam()));

             //   if (projectileData.AttackEventID > 0)
             //       m_vTickEventTemps.Add(new Event(EEventType.Attacker, pProjectile, pAttacker, attackArray.target_ptr, projectileData.AttackEventID, pProjectile.GetPosition(), pProjectile.GetStateParam()));
            }

            if (pProjectile.IsTrackEnd() || bHitScene)
                pProjectile.SetRemainLifeTime(0);
        }
        //------------------------------------------------------
        public void DoProjectileCounteract(ProjectileActor pProjectile)
        {
            pProjectile.SubHitCount(1);
        }
        //------------------------------------------------------
        void DoStepEvent(Event pEvt)
        {
            //if (pEvt.node_ptr == null || pEvt.eventId<=0)
            //    return;
            //if (m_pFramework == null) return;
            //m_pFramework.eventSystem.Begin();
            //m_pFramework.eventSystem.ATuserData = pEvt.node_ptr;
            //m_pFramework.eventSystem.pParentNode = pEvt.pProjectile.GetOwnerActor();
            //m_pFramework.eventSystem.bCalcAxisOffset = true;
            //m_pFramework.eventSystem.TriggerEventPos = pEvt.runtime_pos;
            //m_pFramework.eventSystem.TriggerEventRealPos = pEvt.runtime_pos;
            //m_pFramework.eventSystem.TriggerActorActionStateParam = pEvt.stateParam;
            //m_pFramework.eventSystem.TriggerActorDir = pEvt.node_ptr.GetDirection();
            //m_pFramework.eventSystem.OnEventCallback += OnOverEventCallback;
            //m_pFramework.OnTriggerEvent(pEvt.eventId, false);
            //m_pFramework.eventSystem.End();
        }
        //------------------------------------------------------
        void DoHitEvent(Event pEvt)
        {
            //if (pEvt.node_ptr == null || pEvt.eventId <= 0) return;
            //if (m_pFramework == null) return;
            //m_pFramework.eventSystem.Begin();
            //m_pFramework.eventSystem.ATuserData = pEvt.node_ptr;
            //m_pFramework.eventSystem.pTargetNode = pEvt.target_ptr;
            //m_pFramework.eventSystem.pParentNode = pEvt.pProjectile.GetOwnerActor();
            //m_pFramework.eventSystem.bCalcAxisOffset = true;
            //m_pFramework.eventSystem.TriggerEventPos = pEvt.runtime_hit;
            //m_pFramework.eventSystem.TriggerEventRealPos = pEvt.runtime_hit;
            //m_pFramework.eventSystem.TriggerActorActionStateParam = pEvt.stateParam;
            //m_pFramework.eventSystem.TriggerActorDir = pEvt.node_ptr.GetDirection();
            //m_pFramework.eventSystem.OnEventCallback += OnOverEventCallback;
            //m_pFramework.OnTriggerEvent(pEvt.eventId, false);
            //m_pFramework.eventSystem.End();
        }
        //------------------------------------------------------
        void DoAttackEvent(Event pEvt)
        {
            //if (pEvt.node_ptr == null || pEvt.eventId <= 0) return;
            //if (m_pFramework == null) return;
            //m_pFramework.eventSystem.Begin();
            //m_pFramework.eventSystem.ATuserData = pEvt.node_ptr;
            //m_pFramework.eventSystem.pTargetNode = pEvt.target_ptr;
            //m_pFramework.eventSystem.pParentNode = pEvt.pProjectile.GetOwnerActor();
            //m_pFramework.eventSystem.bCalcAxisOffset = true;
            //m_pFramework.eventSystem.TriggerEventPos = pEvt.runtime_pos;
            //m_pFramework.eventSystem.TriggerEventRealPos = pEvt.runtime_pos;
            //m_pFramework.eventSystem.TriggerActorActionStateParam = pEvt.stateParam;
            //m_pFramework.eventSystem.TriggerActorDir = pEvt.node_ptr.GetDirection();
            //m_pFramework.eventSystem.OnEventCallback += OnOverEventCallback;
            //m_pFramework.OnTriggerEvent(pEvt.eventId, false);
            //m_pFramework.eventSystem.End();
        }
        //------------------------------------------------------
        void DoOverEvent(Event pEvt)
        {
            //if (pEvt.node_ptr == null || pEvt.eventId <= 0) return;
            //if (m_pFramework == null) return;
            //m_pFramework.eventSystem.Begin();
            //m_pFramework.eventSystem.ATuserData = pEvt.node_ptr;
            //m_pFramework.eventSystem.pTargetNode = pEvt.target_ptr;
            //m_pFramework.eventSystem.pParentNode = pEvt.pProjectile.GetOwnerActor();
            //m_pFramework.eventSystem.bCalcAxisOffset = true;
            //m_pFramework.eventSystem.TriggerEventPos = pEvt.runtime_pos;
            //m_pFramework.eventSystem.TriggerEventRealPos = pEvt.runtime_pos;
            //m_pFramework.eventSystem.TriggerActorActionStateParam = pEvt.stateParam;
            //m_pFramework.eventSystem.TriggerActorDir = pEvt.node_ptr.GetDirection();
            //m_pFramework.eventSystem.OnEventCallback += OnOverEventCallback;
            //m_pFramework.OnTriggerEvent(pEvt.eventId, false);
            //m_pFramework.eventSystem.End();
        }
        //------------------------------------------------------
        //void OnOverEventCallback(BaseEvent param, ref uint usedFlag)
        //{
        //}
        //------------------------------------------------------
        public void StopProjectile(ProjectileActor pProjectile, bool bRemoved = true)
        {
            var projectileData = pProjectile.GetProjectileData();
            if (projectileData == null) return;
        //    if(projectileData.OverEventID>0)
          //      m_vTickEventTemps.Add(new Event(EEventType.Over, pProjectile, pProjectile.GetOwnerActor(), null, projectileData.OverEventID, pProjectile.GetPosition(), pProjectile.GetStateParam()));

            if (bRemoved)
            {
                m_mRunningProjectile.Remove(pProjectile.GetInstanceID());
            }
            pProjectile.Reset();
            pProjectile.SetDestroy();
        }
        //------------------------------------------------------
        public void StopProjectile(int dwLaunchID, bool bRemoved = true)
        {
            ProjectileActor pProjectile;
            if (m_mRunningProjectile.TryGetValue(dwLaunchID, out pProjectile))
                StopProjectile(pProjectile, bRemoved);
        }
        //------------------------------------------------------
        public void StopAllProjectiles()
        {
            m_vTickEventTemps.Clear();
            foreach (var db in m_mRunningProjectile)
            {
                db.Value.Reset();
                db.Value.SetDestroy();
            }
            m_CatchTemp.Clear();
            m_mRunningProjectile.Clear();
        }
        //------------------------------------------------------
        public void DelayStopProjectile(ProjectileActor pProjectile, FFloat fDelayDuration)
        {
            pProjectile.SetDelayStopDelta(fDelayDuration);
            pProjectile.SetDelayStopDuration(fDelayDuration);

            var vHoldRoles = pProjectile.GetHoldRoles();
            if (vHoldRoles != null)
            {
                Actor target;
                for (int i = 0; i < vHoldRoles.Count; ++i)
                {
                    target = vHoldRoles[i];
                    target.SetSpeedXZ(Vector3.zero);
                }
            }
        }
        //------------------------------------------------------
        internal void OnActorStatusCallback(Actor pActor, EActorStatus eStatus, IContextData pTakeData = null)
        {
        }
    }
}

