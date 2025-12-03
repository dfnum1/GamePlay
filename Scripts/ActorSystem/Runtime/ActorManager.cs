/********************************************************************
生成日期:	06:30:2025
类    名: 	ActorManager
作    者:	HappLI
描    述:	Actor管理器
*********************************************************************/
using Framework.Cutscene.Runtime;
using System.Collections.Generic;
using UnityEngine;
namespace Framework.ActorSystem.Runtime
{
    public interface IActorSystemCallback
    {
        bool OnActorSystemLoadAsset(string name, System.Action<UnityEngine.Object> onLoaded, bool bAsync = true);
        bool OnActorSystemUnloadAsset(UnityEngine.Object pAsset);

        bool OnActorSystemSpawnInstance(string name, System.Action<UnityEngine.GameObject> onLoaded, bool bAsync = true);
        bool OnActorSystemDespawnInstance(GameObject pInstance, string name = null);

        bool OnActorSystemActorCallback(Actor pActor, EActorStatus eStatus, IContextData pTakeData = null);
        bool OnActorSystemActorAttrDirty(Actor pActor, byte attrType, int oldValue, int newValue, IContextData externVar = null);

        bool OnActorSystemActorHitFrame(HitFrameActor hitFrameActor);

    }
    public class ActorManager
    {
        bool                                    m_isInitialized = false;
        CutsceneManager                         m_CutsceneManager = null;
        ProjectileManager                       m_ProjectileManager = null;
        bool                                    m_bEditMode = false;
        private List<IActorSystemCallback>      m_vCallbacks = null;
        int                                     m_nAutoGUID = 0;
        Dictionary<int, Actor>                  m_vNodes = new Dictionary<int, Actor>(128);
        Actor                                   m_pTail = null;
        Actor                                   m_pRoot = null; 
        List<Actor>                             m_vDestroyList = new List<Actor>(16);

        int                                     m_nTerrainLayerMask = -1;
        protected long                          m_lRuntime = 0;
        protected long                          m_lRuntimeUnScale = 0;

        protected IntersetionParam              m_IntersetionParam = null;
        HashSet<HitFrameActor>                  m_vHitFrameCaches;
        List<Actor>                             m_CatchNodeList;
        //-----------------------------------------------------
        public bool IsEditorMode()
        {
            return m_bEditMode;
        }
        //-----------------------------------------------------
        public void Init(CutsceneManager cutsceneMgr)
        {
            if (m_isInitialized)
                return;
            m_isInitialized = true;
            m_CutsceneManager = cutsceneMgr;
        }
        //-----------------------------------------------------
        public void SetTerrainLayerMask(int layerMask)
        {
            m_nTerrainLayerMask = layerMask;
        }
        //-----------------------------------------------------
        public int GetTerrainLayerMask()
        {
            return m_nTerrainLayerMask;
        }
        //-----------------------------------------------------
        public float GetRandom(float lower, float upper)
        {
            if(lower<upper) return UnityEngine.Random.Range(lower, upper);
            return UnityEngine.Random.Range(upper, lower);
        }
        //-----------------------------------------------------
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
            return m_IntersetionParam;
        }
        //-----------------------------------------------------
        internal CutsceneInstance CreateCutsceneInstance()
        {
            if(!m_isInitialized)
            {
                UnityEngine.Debug.LogError("no initialized");
                return null;
            }
            return new CutsceneInstance(m_CutsceneManager);
        }
        //-----------------------------------------------------
        public Actor CreateActor(IContextData pData, IContextData userVariable = null, int nodeID = 0)
        {
            return InnerCreateActor<Actor>(0, pData, false, userVariable);
        }
        //-----------------------------------------------------
        public Actor AsyncCreateActor(int nodeID, IContextData pData, IContextData userVariable = null)
        {
            return InnerCreateActor<Actor>(0, pData, true, userVariable);
        }
        //-----------------------------------------------------
        public T CreateActor<T>(IContextData pData, IContextData userVariable = null, int nodeID = 0) where T : Actor, new()
        {
            return InnerCreateActor<T>(0, pData, false, userVariable);
        }
        //-----------------------------------------------------
        public T AsyncCreateActor<T>(int nodeID, IContextData pData, IContextData userVariable = null) where T : Actor, new()
        {
            return InnerCreateActor<T>(0, pData, true, userVariable);
        }
        //-----------------------------------------------------
        T InnerCreateActor<T>(int nodeID, IContextData pData, bool bAsync, IContextData userVariable = null) where T : Actor, new()
        {
            if (!m_isInitialized)
            {
                UnityEngine.Debug.LogError("no initialized");
                return null;
            }
            T pActor = null;
            if (pActor == null) pActor = TypeInstancePool.Malloc<T>();
            if (pActor == null) return null;
            pActor.SetActorManager(this);
            pActor.OnConstruct();

            if (nodeID == 0)
            {
                m_nAutoGUID++;
                nodeID = m_nAutoGUID;
            }
            else
            {
                if (m_vNodes.ContainsKey(nodeID))
                {
                    m_nAutoGUID++;
                    nodeID = m_nAutoGUID;
                }
                else
                    m_nAutoGUID = Mathf.Max(m_nAutoGUID, nodeID);
            }
            pActor.SetContextData(pData);
            pActor.SetInstanceID(nodeID);
            AddActor(pActor);

            OnActorStatusCallback(pActor, bAsync ? EActorStatus.AsyncCreate : EActorStatus.Create, userVariable);
            pActor.OnCreated();
            return pActor;
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
        }
        //-----------------------------------------------------
        public void RemoveNode(Actor pNode, bool bRemoveMaps = true)
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
            pNode.FreeDestroy();
        }
        //-----------------------------------------------------
        public int CalcNeighbors(Actor pActor, float range, ref List<Actor> vNodes)
        {
            return 0;
        }
        //-----------------------------------------------------
        internal ProjectileManager GetProjectileManager()
        {
            if(m_ProjectileManager == null)
            {
                m_ProjectileManager = TypeInstancePool.Malloc<ProjectileManager>();
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
        public void SetProjectileDatas(ProjectileDatas projectileDatas)
        {
            if (projectileDatas == null)
                return;
            GetProjectileManager().SetProjectileDatas(projectileDatas);
        }
        //------------------------------------------------------
        public void StopProjectileByOwner(Actor pNode, float fLaucherTime)
        {
            if (m_ProjectileManager == null) return;
            m_ProjectileManager.StopProjectileByOwner(pNode, fLaucherTime);
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
        public void TrackCheck(Actor pTargetActor, Vector3 vPosition, ProjectileData pData, Transform pTrackTransform, ref Transform pTrackSlot, ref int damage_power, ref uint track_frame_id, ref uint track_body_id, ref Vector3 trackOffset)
        {
            if (m_ProjectileManager == null) return;
            m_ProjectileManager.TrackCheck(pTargetActor, vPosition, pData, pTrackTransform, ref pTrackSlot, ref damage_power, ref track_frame_id, ref track_body_id, ref trackOffset);
        }
        //------------------------------------------------------
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
            return m_CatchNodeList;
        }
        //-----------------------------------------------------
        internal bool OnActorStatusCallback(Actor pActor, EActorStatus eStatus, IContextData pTakeData = null)
        {
            if (m_vCallbacks == null || pActor == null)
                return false;
            if(m_ProjectileManager!=null)
            {
                m_ProjectileManager.OnActorStatusCallback(pActor, eStatus, pTakeData);
            }
            foreach(var db in m_vCallbacks)
            {
                if (db.OnActorSystemActorCallback(pActor, eStatus, pTakeData))
                    return true;
            }
            return false;
        }
        //-----------------------------------------------------
        internal bool OnActorAttriDirtyCallback(Actor pActor, byte attrType, int oldValue, int newValue, IContextData pTakeData = null)
        {
            if (m_vCallbacks == null || pActor == null)
                return false;
            foreach (var db in m_vCallbacks)
            {
                if (db.OnActorSystemActorAttrDirty(pActor, attrType, oldValue, newValue, pTakeData))
                    return true;
            }
            return false;
        }
        //-----------------------------------------------------
        internal bool OnActorSystemActorHitFrame(HitFrameActor hitFrame)
        {
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
        public bool LoadAsset(string file, System.Action<UnityEngine.Object> onCallback, bool bAsync = true)
        {
            if (string.IsNullOrEmpty(file))
                return false;
#if UNITY_EDITOR
            if (m_vCallbacks == null || m_vCallbacks.Count <= 0)
            {
                var asset = ActorSystemUtil.EditLoadUnityObject(file);
                if (asset != null)
                {
                    if (onCallback != null) onCallback(asset);
                    return true;
                }
            }
#endif

            if (m_vCallbacks == null || m_vCallbacks.Count <= 0)
            {
#if UNITY_EDITOR
                if (Application.isPlaying)
                    Debug.LogError("CutsceneManager: No callbacks registered to load asset " + file);
#else
                Debug.LogError("CutsceneManager: No callbacks registered to load asset " + file);
#endif
                return false;
            }
            m_vCallbacks[0].OnActorSystemLoadAsset(file, onCallback, bAsync);
            return true;
        }
        //-----------------------------------------------------
        public void UnloadAsset(UnityEngine.Object pAsset)
        {
            if (pAsset == null)
                return;
            if (m_vCallbacks == null || m_vCallbacks.Count <= 0)
            {
#if UNITY_EDITOR
                if (!m_bEditMode && Application.isPlaying)
                    Debug.LogError("ActorManager: No callbacks registered to UnloadAsset");
#else
                Debug.LogError("ActorManager: No callbacks registered to UnloadAsset ");
#endif
                return;
            }
            m_vCallbacks[0].OnActorSystemUnloadAsset(pAsset);
        }
        //-----------------------------------------------------
        public bool SpawnInstance(string file, System.Action<UnityEngine.GameObject> onCallback, bool bAsync = true)
        {
            if (string.IsNullOrEmpty(file))
                return false;
#if UNITY_EDITOR
            if (m_vCallbacks == null || m_vCallbacks.Count <= 0)
            {
                var obj = ActorSystemUtil.EditLoadUnityObject(file);
                if (obj == null || !(obj is UnityEngine.GameObject))
                    return false;
                if (onCallback != null) onCallback(GameObject.Instantiate(obj as GameObject));
                return true;
            }
#endif
            if (m_vCallbacks == null || m_vCallbacks.Count <= 0)
            {
#if UNITY_EDITOR
                if (Application.isPlaying)
                    Debug.LogError("ActorManager: No callbacks registered to spawn instance " + file);
#else
                Debug.LogError("ActorManager: No callbacks registered to spawn instance " + file);
#endif
                return false;
            }
            m_vCallbacks[0].OnActorSystemSpawnInstance(file, onCallback, bAsync);
            return true;
        }
        //-----------------------------------------------------
        public void DespawnInstance(GameObject pInstance, string name = null)
        {
            if (pInstance == null)
                return;
#if UNITY_EDITOR
            if (m_vCallbacks == null || m_vCallbacks.Count <= 0)
            {
                if (Application.isPlaying) GameObject.Destroy(pInstance);
                else GameObject.DestroyImmediate(pInstance);
                return;
            }
#endif
            if (m_vCallbacks == null || m_vCallbacks.Count <= 0)
            {
#if UNITY_EDITOR
                if (!m_bEditMode && Application.isPlaying)
                    Debug.LogWarning("ActorManager: No callbacks registered to despawn instance " + name);
#else
                Debug.LogWarning("ActorManager: No callbacks registered to despawn instance " + name);
#endif
                if (pInstance) GameObject.Destroy(pInstance);
                return;
            }
            m_vCallbacks[0].OnActorSystemDespawnInstance(pInstance, name);
        }
        //-----------------------------------------------------
        public void Update(float fFrame)
        {
            if(!m_isInitialized)
            {
                UnityEngine.Debug.LogWarning("not initialized");
                return;
            }
            m_lRuntime = (int)(Time.time * 1000);
            m_lRuntimeUnScale = (int)(Time.unscaledTime * 1000);
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
        }
        //------------------------------------------------------
        public long GetRunTime()
        {
            return m_lRuntime;
        }
        //------------------------------------------------------
        public long GetRunUnScaleTime()
        {
            return m_lRuntimeUnScale;
        }
        //-----------------------------------------------------
        public void Clear()
        {
           // if (m_pWorldKDTree != null) m_pWorldKDTree.Clear();
           // if (m_WorldTriggers != null) m_WorldTriggers.Clear();
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
            m_lRuntime = 0;
            m_lRuntimeUnScale = 0;
            if (m_ProjectileManager != null)
                m_ProjectileManager.Clear();
        }
        //-----------------------------------------------------
        public void Shutdown()
        {
            Clear();
            if(m_vCallbacks!=null) m_vCallbacks.Clear();
            if (m_ProjectileManager != null) m_ProjectileManager.Destroy();
        }
    }
}