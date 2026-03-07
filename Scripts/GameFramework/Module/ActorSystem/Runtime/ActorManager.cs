/********************************************************************
生成日期:	06:30:2025
类    名: 	ActorManager
作    者:	HappLI
描    述:	Actor管理器
*********************************************************************/
using Framework.AT.Runtime;
using Framework.Core;
using Framework.Cutscene.Runtime;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Application;
using Framework.Base;
using System.IO;
using UnityEngine.Playables;
using Framework.AT.Editor;
using static UnityEngine.UI.CanvasScaler;






#if USE_FIXEDMATH
using ExternEngine;
#else
using FFloat = System.Single;
using FMatrix4x4 = UnityEngine.Matrix4x4;
using FQuaternion = UnityEngine.Quaternion;
using FVector2 = UnityEngine.Vector2;
using FVector3 = UnityEngine.Vector3;
using FBounds = UnityEngine.Bounds;
#endif
namespace Framework.ActorSystem.Runtime
{
    public interface IActorSystemCallback
    {
        bool OnActorSystemActorCallback(Actor pActor, EActorStatus eStatus, IVarData pTakeData = null);
        bool OnActorSystemActorAttrDirty(Actor pActor, byte attrType, FFloat oldValue, FFloat newValue, IVarData externVar = null);

        bool OnActorSystemActorHitFrame(HitFrameActor hitFrameActor);

    }
    [ATInteralExport("Actor系统/管理器",-1, icon: "ActorSystem/actormanager")]
    public class ActorManager : AModule
    {
        ProjectileManager                           m_ProjectileManager = null;
        bool                                        m_bEditMode = false;
        private List<IActorSystemCallback>          m_vCallbacks = null;
        int                                         m_nAutoGUID = 0;
        Dictionary<int, Actor>                      m_vNodes = new Dictionary<int, Actor>(128);
        Actor                                       m_pTail = null;
        Actor                                       m_pRoot = null; 
        List<Actor>                                 m_vDestroyList = new List<Actor>(16);

        float                                       m_fTerrainHeight = 0;
        int                                         m_nTerrainLayerMask = -1;

        Dictionary<int, AttrCoreData.AttrFormula>   m_AttrFormulas = null;
        Dictionary<int, AttrCoreData.AttrInfo>      m_AttrInfos = null;

        protected IntersetionParam                  m_IntersetionParam = null;
        HashSet<HitFrameActor>                      m_vHitFrameCaches;
        List<Actor>                                 m_CatchNodeList;
        HashSet<Actor>                              m_CatchNodeSet;

        private AgentTree                           m_pGlobalSkillAT = null;
        private AgentTree                           m_pGlobalBuffAT = null;

        private ISpatialWorld                       m_pSpatialIndex;
        private ESpatialIndexType                   m_eSpatialIndexType = ESpatialIndexType.Octree;
        private bool                                m_isSpatialIndexEnabled = true;
        FBounds                                     m_SpatialBounds = SpatialIndexFactory.DefaultWorldBounds;
        //-----------------------------------------------------
        public bool IsEditorMode()
        {
            return m_bEditMode;
        }
        //-----------------------------------------------------
        protected override void OnInit()
        {
            ActorSystemUtil.Register(this);
            m_fTerrainHeight = 0;
        }
        //-----------------------------------------------------        
        protected override void OnStart()
        {
            BuildAgentTrees();
        }
        //-----------------------------------------------------        
        public AttrCoreData.AttrFormula GetAttrFormula(int formula)
        {
            if (m_AttrFormulas == null) return null;
            if (m_AttrFormulas.TryGetValue(formula, out var attrFormula))
                return attrFormula;
            return null;
        }
        //-----------------------------------------------------        
        public AttrCoreData.AttrInfo GetAttrInfo(int type)
        {
            if (m_AttrInfos == null) return AttrCoreData.AttrInfo.DEF;
            if (m_AttrInfos.TryGetValue(type, out var attrInfo))
                return attrInfo;
            return AttrCoreData.AttrInfo.DEF;
        }
        //-----------------------------------------------------        
        [ATMethod("设置空间大小")]
        public void InitializeSpatialIndex(FBounds? worldBounds = null)
        {
            FBounds bounds = worldBounds ?? m_SpatialBounds;
            if (m_pSpatialIndex != null)
            {
                m_pSpatialIndex.Dispose();
            }

            m_pSpatialIndex = SpatialIndexFactory.CreateIndex(m_eSpatialIndexType, bounds);
        }
        //-----------------------------------------------------
        [ATMethod("设置空间划分类型")]
        public void SetSpatialIndexType(ESpatialIndexType indexType)
        {
            if (m_eSpatialIndexType == indexType)
            {
                return;
            }

            m_eSpatialIndexType = indexType;
            InitializeSpatialIndex();

            if (m_isSpatialIndexEnabled && m_pSpatialIndex != null)
            {
                foreach (var actor in m_vNodes.Values)
                {
                    m_pSpatialIndex.AddActor(actor);
                }
            }
        }
        //-----------------------------------------------------        
        [ATMethod("设置空间划分开关")]
        public void SetSpatialIndexEnabled(bool enabled)
        {
            m_isSpatialIndexEnabled = enabled;
        }
        //-----------------------------------------------------        
        [ATMethod("获取空间划分类型")]
        public ESpatialIndexType GetSpatialIndexType()
        {
            return m_eSpatialIndexType;
        }
        //-----------------------------------------------------        
        [ATMethod("是否开始空间划分")]
        public bool IsSpatialIndexEnabled()
        {
            return m_isSpatialIndexEnabled;
        }
        //-----------------------------------------------------
        [ATMethod("设置地形碰撞层Mask")]
        public void SetTerrainLayerMask(int layerMask)
        {
            m_nTerrainLayerMask = layerMask;
        }
        //-----------------------------------------------------
        [ATMethod("获取地形碰撞层Mask")]
        public int GetTerrainLayerMask()
        {
            return m_nTerrainLayerMask;
        }
        //-----------------------------------------------------
        [ATMethod("设置地表最低高度")]
        public void SetTerrainHeight(float layerMask)
        {
            m_fTerrainHeight = layerMask;
        }
        //-----------------------------------------------------
        [ATMethod("获取地表最低高度")]
        public float GetTerrainHeight()
        {
            return m_fTerrainHeight;
        }
        //-----------------------------------------------------
        [ATMethod("浮点区间随机")]
        public float GetRandom(float lower, float upper)
        {
            if(lower<upper) return UnityEngine.Random.Range(lower, upper);
            return UnityEngine.Random.Range(upper, lower);
        }
        //-----------------------------------------------------
        [ATMethod("整数区间随机")]
        public int GetRandom(int lower, int upper)
        {
            if (lower < upper) return UnityEngine.Random.Range(lower, upper);
            return UnityEngine.Random.Range(upper, lower);
        }
        //-----------------------------------------------------
        public bool IsPause()
        {
            return false;
        }
        //-----------------------------------------------------
        public bool IsLogicLock()
        {
            return false;
        }
        //-----------------------------------------------------
        public IntersetionParam GetIntersetionParam()
        {
            if (m_IntersetionParam == null) m_IntersetionParam = new IntersetionParam();
            m_IntersetionParam.Check();
            return m_IntersetionParam;
        }
        //-----------------------------------------------------
        internal CutsceneInstance CreateCutsceneInstance()
        {
            return new CutsceneInstance(GetFramework().GetModule<CutsceneManager>());
        }
        //-----------------------------------------------------
        [ATMethod("同步创建Actor")]
        public Actor CreateActor(IActorContextData pData, IVarData userVariable = null, int actorId = 0)
        {
            return InnerCreateActor<Actor>(actorId, pData, false, userVariable);
        }
        //-----------------------------------------------------
        [ATMethod("异步创建Actor")]
        public Actor AsyncCreateActor(IActorContextData pData, IVarData userVariable = null, int actorId =0)
        {
            return InnerCreateActor<Actor>(actorId, pData, true, userVariable);
        }
        //-----------------------------------------------------
        public T CreateActor<T>(IActorContextData pData, IVarData userVariable = null, int actorId = 0) where T : Actor, new()
        {
            return InnerCreateActor<T>(actorId, pData, false, userVariable);
        }
        //-----------------------------------------------------
        public T AsyncCreateActor<T>(IActorContextData pData, IVarData userVariable = null, int actorId=0) where T : Actor, new()
        {
            return InnerCreateActor<T>(actorId, pData, true, userVariable);
        }
        //-----------------------------------------------------
        T InnerCreateActor<T>(int actorId, IActorContextData pData, bool bAsync, IVarData userVariable = null) where T : Actor, new()
        {
            T pActor = null;
            if (pActor == null) pActor = TypeInstancePool.Malloc<T>(GetFramework());
            if (pActor == null) return null;
            pActor.SetActorManager(this);
            pActor.OnConstruct();

            if (actorId == 0)
            {
                m_nAutoGUID++;
                actorId = m_nAutoGUID;
            }
            else
            {
                if (m_vNodes.ContainsKey(actorId))
                {
                    m_nAutoGUID++;
                    actorId = m_nAutoGUID;
                }
                else
                    m_nAutoGUID = Mathf.Max(m_nAutoGUID, actorId);
            }
            pActor.SetContextData(pData);
            pActor.SetInstanceID(actorId);
            AddActor(pActor);

            if(pData!=null)
            {
                string modelFile = pData.GetAssetFile();
                if(!File.Exists(modelFile))
                {
                    var op = GetFileSystem().SpawnInstance(modelFile, OnSpawnInstance, bAsync);
                    if(op!=null) op.SetUserData(0, pActor);
                }
            }

            OnActorStatusCallback(pActor, bAsync ? EActorStatus.AsyncCreate : EActorStatus.Create, userVariable);
            pActor.OnCreated();
            return pActor;
        }
        //-----------------------------------------------------
        void OnSpawnInstance(InstanceOperator instOp, bool check)
        {
            var pActor = instOp.GetUserData<Actor>(0);
            if (check)
            {
                instOp.SetUsed(!pActor.IsDestroy());
                return;
            }
            var able = instOp.GetInstanceAble();
            if(able!=null) pActor.SetObjectAble(instOp.GetObject());
            else pActor.SetObjectAble(instOp.GetObject());
        }
        //-----------------------------------------------------
        [ATMethod("根据ID获取Actor")]
        public Actor GetActor(int id)
        {
            if (m_vNodes == null) return null;
            if (m_vNodes.TryGetValue(id, out var actor))
                return actor;
            return null;
        }
        //-----------------------------------------------------
        public T GetActor<T>(int id) where T : Actor
        {
            if (m_vNodes == null) return null;
            if (m_vNodes.TryGetValue(id, out var actor))
                return actor as T;
            return null;
        }
        //-----------------------------------------------------
        void AddActor(Actor pNode)
        {
            if (pNode == null) return;
            if (m_pRoot == null)
                m_pRoot = pNode;
            if (m_pTail == null)
                m_pTail = m_pRoot;
            else
            {
                m_pTail.SetNext(pNode);
                pNode.SetPrev(m_pTail);
            }

            m_pTail = pNode;

            m_vNodes.Add(pNode.GetInstanceID(), pNode);
            
            if (pNode.IsSpatial()&& m_isSpatialIndexEnabled && m_pSpatialIndex != null)
            {
                m_pSpatialIndex.AddActor(pNode);
            }
        }
        //-----------------------------------------------------
        void RemoveNode(Actor pNode, bool bRemoveMaps = true)
        {
            var prev = pNode.GetPrev();
            var next = pNode.GetNext();
            if (prev != null)
                prev.SetNext(next);
            if (next != null)
                next.SetPrev(prev);

            if (m_pRoot == pNode)
                m_pRoot = next;
            if (m_pTail == pNode)
                m_pTail = prev;

            if (bRemoveMaps)
                m_vNodes.Remove(pNode.GetInstanceID());
            
            if (m_isSpatialIndexEnabled && m_pSpatialIndex != null)
            {
                m_pSpatialIndex.RemoveActor(pNode);
            }
            
            pNode.FreeDestroy();
        }
        //-----------------------------------------------------
        internal ProjectileManager GetProjectileManager()
        {
            if(m_ProjectileManager == null)
            {
                m_ProjectileManager = TypeInstancePool.Malloc<ProjectileManager>(GetFramework());
                m_ProjectileManager.Awake(this);
            }
            return m_ProjectileManager;
        }
        //-----------------------------------------------------
        public void RegisterCallback(IActorSystemCallback callback)
        {
            if (callback == null) return;
            if (m_vCallbacks == null)
                m_vCallbacks = new List<IActorSystemCallback>(1);
            if (!m_vCallbacks.Contains(callback))
                m_vCallbacks.Add(callback);
        }
        //-----------------------------------------------------
        public void UnregisterCallback(IActorSystemCallback callback)
        {
            if (callback == null) return;
            if (m_vCallbacks == null) return;
            if (m_vCallbacks.Contains(callback))
                m_vCallbacks.Remove(callback);
        }
        //------------------------------------------------------
        public HashSet<HitFrameActor> GetHitFrameActorCaches()
        {
            if (m_vHitFrameCaches == null) m_vHitFrameCaches = new HashSet<HitFrameActor>(2);
            return m_vHitFrameCaches;
        }
        //------------------------------------------------------
        public void SetProjectileDatas(AProjectileDatas projectileDatas)
        {
            if (projectileDatas == null)
                return;
            GetProjectileManager().SetProjectileDatas(projectileDatas);
        }
        //------------------------------------------------------
        [ATMethod("停掉所有指定对象在时间内的弹道")]
        public void StopProjectileByOwner(Actor pActor, float fLaucherTime)
        {
            if (m_ProjectileManager == null) return;
            m_ProjectileManager.StopProjectileByOwner(pActor, fLaucherTime);
        }
        //------------------------------------------------------
        public int LaunchProjectile(uint dwProjectileTableID, Actor pOwnerActor, AActorStateInfo stateParam,
        Vector3 vPosition, Vector3 vDirection, Actor targetNode = null, int dwAssignedID = 0,
        float fDelta = 0, Transform pTrackTransform = null, List<Actor> vResults = null)
        {
            return GetProjectileManager().LaunchProjectile(dwProjectileTableID, pOwnerActor, stateParam, vPosition, vDirection, targetNode, dwAssignedID, fDelta, pTrackTransform, vResults);
        }
        //------------------------------------------------------
        public int LaunchProjectile(ProjectileData pData, Actor pOwnerActor, AActorStateInfo stateParam,
               Vector3 vPosition, Vector3 vDirection, Actor targetNode = null, int dwAssignedID = 0,
               float fDelta = 0, Transform pTrackTransform = null, List<Actor> vResults = null)
        {
            return GetProjectileManager().LaunchProjectile(pData, pOwnerActor, stateParam, vPosition, vDirection, targetNode, dwAssignedID, fDelta, pTrackTransform, vResults);
        }
        //------------------------------------------------------
        public void TrackCheck(Actor pTargetActor, Vector3 vPosition, ProjectileData pData, Transform pTrackTransform, ref Transform pTrackSlot, ref int damage_power, ref uint track_frame_id, ref uint track_body_id, ref FVector3 trackOffset)
        {
            if (m_ProjectileManager == null) return;
            m_ProjectileManager.TrackCheck(pTargetActor, vPosition, pData, pTrackTransform, ref pTrackSlot, ref damage_power, ref track_frame_id, ref track_body_id, ref trackOffset);
        }
        //------------------------------------------------------
        [ATMethod("停掉所有弹道")]
        public void StopAllProjectiles()
        {
            if (m_ProjectileManager == null) return;
            m_ProjectileManager.StopAllProjectiles();
        }
        //------------------------------------------------------
        internal Dictionary<int,ProjectileActor> GetRunningProjectile()
        {
            if (m_ProjectileManager == null) return null;
            return m_ProjectileManager.GetRunningProjectile();
        }
        //------------------------------------------------------
        public List<Actor> GetCatchActorList()
        {
            if (m_CatchNodeList == null) m_CatchNodeList = new List<Actor>(2);
            m_CatchNodeList.Clear();
            return m_CatchNodeList;
        }
        //------------------------------------------------------
        public HashSet<Actor> GetCatchActorSet()
        {
            if (m_CatchNodeSet == null) m_CatchNodeSet = new HashSet<Actor>(2);
            m_CatchNodeSet.Clear();
            return m_CatchNodeSet;
        }
        //-----------------------------------------------------
        internal bool OnActorStatusCallback(Actor pActor, EActorStatus eStatus, IVarData pTakeData = null)
        {
            if (pActor == null)
                return false;
            if(m_ProjectileManager!=null)
            {
                m_ProjectileManager.OnActorStatusCallback(pActor, eStatus, pTakeData);
            }

            if(eStatus == EActorStatus.Killed)
            {
                OnTaskGlobalAT((int)EActorATType.onKilled, pActor);
            }
            else if (eStatus == EActorStatus.Revive)
            {
                OnTaskGlobalAT((int)EActorATType.onRevive, pActor);
            }
            if(m_vCallbacks!=null)
            {
                foreach (var db in m_vCallbacks)
                {
                    if (db.OnActorSystemActorCallback(pActor, eStatus, pTakeData))
                        return true;
                }
            }

            return false;
        }
        //-----------------------------------------------------
        internal bool OnActorAttriDirtyCallback(Actor pActor, byte attrType, float oldValue, float newValue, IVarData pTakeData = null)
        {
            if (pActor == null)
                return false;

            var argv = VariableList.Malloc(GetFramework());
            argv.AddUserData(pActor);
            argv.AddInt(attrType);
            argv.AddFloat(oldValue);
            argv.AddFloat(newValue);
            OnTaskGlobalAT((int)EActorATType.onDirtyAttribute, argv);

            if (m_vCallbacks!=null)
            {
                foreach (var db in m_vCallbacks)
                {
                    if (db.OnActorSystemActorAttrDirty(pActor, attrType, oldValue, newValue, pTakeData))
                        return true;
                }
            }

            return false;
        }
        //-----------------------------------------------------
        internal bool OnActorSystemActorHitFrame(HitFrameActor hitFrame)
        {
            if(hitFrame.attack_state_param!=null)
            {
                if (hitFrame.target_ptr != null)
                {
                    var pFormula = GetAttrFormula(hitFrame.attack_state_param.GetAttrFormulaType());
                    if (pFormula != null)
                    {
                        FFloat finalValue = AttrFormulaUtil.CalcAttrFormula(pFormula, hitFrame.attack_ptr, hitFrame.target_ptr);
                        hitFrame.target_ptr.SubAttr(pFormula.applayAttr, finalValue);
                    }
                }
            }
            if(hitFrame.target_ptr!=null)
            {
                ActorAgentTree pAT = hitFrame.target_ptr.GetAgent<ActorAgentTree>(false);
                if (pAT != null)
                    pAT.OnHit(hitFrame);
            }
            OnTaskGlobalAT((int)EActorATType.onHitFrame, hitFrame.attack_ptr,hitFrame.target_ptr, hitFrame);

            if (m_vCallbacks == null)
                return false;
            foreach (var db in m_vCallbacks)
            {
                if (db.OnActorSystemActorHitFrame(hitFrame))
                    return true;
            }
            return false;
        }
        //-----------------------------------------------------
        protected override void OnUpdate(FFloat fFrame)
        {
            if (m_ProjectileManager != null)
                m_ProjectileManager.Update(fFrame);
            if (m_pRoot != null)
            {
                int index = 0;
                Actor pNode = m_pRoot;
                while (pNode != null)
                {
                    if (pNode.IsFlag(EActorFlag.Destroy))
                    {
                        m_vDestroyList.Add(pNode);
                    }
                    //else if (pNode.IsFlag(EActorFlag.Spatial))
                    //{
                    //    m_pWorldKDTree.Set(pNode, index++, false);
                    //    //  m_pWorldSpatial.AddNode(pNode);
                    //}
                    pNode = pNode.GetNext();
                }
             //   m_pWorldKDTree.Rebuild();

                Actor destroyNode;
                for (int i = 0; i < m_vDestroyList.Count; ++i)
                {
                    destroyNode = m_vDestroyList[i];
                    m_vNodes.Remove(destroyNode.GetInstanceID());
                    RemoveNode(destroyNode, false);
                }
                m_vDestroyList.Clear();

                pNode = m_pRoot;
                while (pNode != null)
                {
                    pNode.Update(fFrame);
               //     m_WorldTriggers.TriggerCheck(pNode);
                    pNode = pNode.GetNext();
                }
              //  m_WorldTriggers.Update();
            }
            else
            {
                Actor destroyNode;
                for (int i = 0; i < m_vDestroyList.Count; ++i)
                {
                    destroyNode = m_vDestroyList[i];
                    m_vNodes.Remove(destroyNode.GetInstanceID());
                    RemoveNode(destroyNode, false);
                }
                m_vDestroyList.Clear();
            }

            if (m_pGlobalSkillAT != null) m_pGlobalSkillAT.Update(fFrame);
            if (m_pGlobalBuffAT != null) m_pGlobalBuffAT.Update(fFrame);
        }
        //------------------------------------------------------
        public long GetRunTime()
        {
            return GetFramework().GetRunTime();
        }
        //------------------------------------------------------
        public long GetRunUnScaleTime()
        {
            return GetFramework().GetRunUnScaleTime();
        }
        //-----------------------------------------------------
        void BuildAgentTrees()
        {
            if(m_pGlobalSkillAT!=null) m_pGlobalSkillAT.Free();
            m_pGlobalSkillAT = null;
            if(m_pGlobalBuffAT != null) m_pGlobalBuffAT.Free();
             m_pGlobalBuffAT = null;
            if (GetFramework().gameStartup == null)
                return;

            var setting = GetFramework().gameStartup.GetSetting();
            if (setting == null || setting.GetSetting() == null)
                return;
            var settingData = setting.GetSetting();
            if (settingData.skillSystem != null && settingData.skillSystem.atData != null && settingData.skillSystem.atData.enable)
            {
                var pAT = AgentTreePool.MallocAgentTree(settingData.skillSystem.atData, GetFramework());
                if(pAT!=null)
                {
                    m_pGlobalSkillAT = pAT;
                    m_pGlobalSkillAT.Enable(true);
                    m_pGlobalSkillAT.Start();
                }
            }
            if (settingData.buffSystem != null && settingData.buffSystem.atData!=null && settingData.buffSystem.atData.enable)
            {
                var pAT = AgentTreePool.MallocAgentTree(settingData.buffSystem.atData, GetFramework());
                if (pAT != null)
                {
                    m_pGlobalBuffAT = pAT;
                    m_pGlobalBuffAT.Enable(true);
                    m_pGlobalBuffAT.Start();
                }
            }
        }
        //-----------------------------------------------------
        public void DrawDebug(bool bGizmos)
        {
            if (m_isSpatialIndexEnabled && m_pSpatialIndex != null)
            {
                m_pSpatialIndex.DebugDraw(bGizmos);
            }
        }
        //-----------------------------------------------------
        [ATMethod("删除所有Actor对象")]
        public void Clear()
        {           // if (m_pWorldKDTree != null) m_pWorldKDTree.Clear();
           // if (m_WorldTriggers != null) m_WorldTriggers.Clear();
            if (m_pSpatialIndex != null)
            {
                m_pSpatialIndex.Clear();
            }
            if (m_pRoot != null)
            {
                Actor pNext = null;
                Actor pNode = m_pRoot;
                while (pNode != null)
                {
                    pNext = pNode.GetNext();
                    pNode.FreeDestroy();
                    pNode = pNext;
                }
            }
            m_vNodes.Clear();
            m_vDestroyList.Clear();
            m_pRoot = null;
            m_pTail = null;
            m_nAutoGUID = 0;
            if (m_ProjectileManager != null)
                m_ProjectileManager.Clear();
        }
        //-----------------------------------------------------
        protected override void OnDestroy()
        {
            Clear();

            if (m_pGlobalSkillAT != null) m_pGlobalSkillAT.Free();
            m_pGlobalSkillAT = null;
            if (m_pGlobalBuffAT != null) m_pGlobalBuffAT.Free();
            m_pGlobalBuffAT = null;

            if (m_vCallbacks!=null) m_vCallbacks.Clear();
            if (m_ProjectileManager != null) m_ProjectileManager.Destroy();
            
            if (m_pSpatialIndex != null)
            {
                m_pSpatialIndex.Dispose();
                m_pSpatialIndex = null;
            }
            
            ActorSystemUtil.Unregister(this);
        }
        //-----------------------------------------------------        
        public void UpdateActorSpatialIndex(Actor actor)
        {
            if (m_isSpatialIndexEnabled && m_pSpatialIndex != null && actor != null)
            {
                m_pSpatialIndex.UpdateActor(actor);
            }
        }
        //-----------------------------------------------------
        public List<Actor> QueryActorsInBounds(WorldBoundBox boundingBox, Actor pIngore = null, List<Actor> vResults = null)
        {
            if (vResults == null)
            {
                vResults = GetCatchActorList();
                vResults.Clear();
            }
            if (!m_isSpatialIndexEnabled || m_pSpatialIndex == null)
            {
                foreach (var db in m_vNodes)
                {
                    if (db.Value == pIngore)
                        continue;
                    vResults.Add(db.Value);
                }
                return vResults;
            }            
            m_pSpatialIndex.QueryActorsInBounds(boundingBox, vResults, pIngore);
            return vResults;
        }
        //-----------------------------------------------------
        public List<Actor> QueryActorsByRay(FRay ray, float maxDistance, Actor pIngore = null, List<Actor> vResults = null)
        {
            if (vResults == null)
            {
                vResults = GetCatchActorList();
                vResults.Clear();
            }
            if (!m_isSpatialIndexEnabled || m_pSpatialIndex == null)
            {
                foreach (var db in m_vNodes)
                {
                    if (db.Value == pIngore)
                        continue;
                    vResults.Add(db.Value);
                }
                return vResults;
            }
            m_pSpatialIndex.QueryActorsByRay(ray, maxDistance, vResults, pIngore);
            return vResults;
        }
        //-----------------------------------------------------
        public List<Actor> QueryNearestActors(FVector3 position, float maxDistance = 1000, Actor pIngore = null, List<Actor> vResults = null)
        {
            if (vResults == null)
            {
                vResults = GetCatchActorList();
                vResults.Clear();
            }
            if (!m_isSpatialIndexEnabled || m_pSpatialIndex == null)
            {
                foreach (var db in m_vNodes)
                {
                    if (db.Value == pIngore)
                        continue;
                    vResults.Add(db.Value);
                }
                return vResults;
            }
            m_pSpatialIndex.QueryActorsAtPosition(position, maxDistance, vResults, pIngore);
            return vResults;
        }
        //-----------------------------------------------------
        internal void OnTaskGlobalAT(int taskType, VariableList argvs,bool autoRelease = true)
        {
            if (m_pGlobalBuffAT != null)
            {
                m_pGlobalBuffAT.ExecuteTask(taskType, argvs, false);
                m_pGlobalBuffAT.Free();
            }

            if (m_pGlobalSkillAT != null)
            {
                m_pGlobalSkillAT.ExecuteTask(taskType, argvs, false);
            }
            if(autoRelease && argvs != null) argvs.Release();
        }
        //-----------------------------------------------------
        internal void OnTaskGlobalAT(int taskType)
        {
            OnTaskGlobalAT(taskType, null);
        }
        //-----------------------------------------------------
        internal void OnTaskGlobalAT(int taskType, IUserData data1)
        {
            VariableList argvs = VariableList.Malloc(GetFramework(),1);
            argvs.AddUserData(data1);
            OnTaskGlobalAT(taskType, argvs);
        }
        //-----------------------------------------------------
        internal void OnTaskGlobalAT(int taskType, IUserData data1, IUserData data2)
        {
            VariableList argvs = VariableList.Malloc(GetFramework(), 1);
            argvs.AddUserData(data1);
            argvs.AddUserData(data2);
            OnTaskGlobalAT(taskType, argvs);
        }       
        //-----------------------------------------------------
        internal void OnTaskGlobalAT(int taskType, IUserData data1, IUserData data2, IUserData data3)
        {
            VariableList argvs = VariableList.Malloc(GetFramework(), 1);
            argvs.AddUserData(data1);
            argvs.AddUserData(data2);
            argvs.AddUserData(data3);
            OnTaskGlobalAT(taskType, argvs);
        }
    }
}