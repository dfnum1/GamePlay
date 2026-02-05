/********************************************************************
生成日期:	3:10:2019  15:03
类    名: 	AFramework
作    者:	HappLI
描    述:	框架基类
*********************************************************************/
using Framework.ActorSystem.Editor;
using Framework.ActorSystem.Runtime;
using Framework.AT.Runtime;
using Framework.Cutscene.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Framework.Core
{
    public abstract class AFramework : IAgentTreeCallback, ICutsceneCallback, IActorSystemCallback
    {
        private static AFramework ms_pMainFramework;
#if UNITY_EDITOR
        private static AFramework ms_pEditorFramework;
        public static AFramework editorFramework { get { return ms_pEditorFramework; } }
#endif
        public bool         IsEditorMode { get { return m_pGame == null || m_pGame.IsEditor(); } }
        public static bool  isStartup { get { return ms_pMainFramework != null && ms_pMainFramework.m_bStarted; } }
        private IGame       m_pGame;
        public IGame        gameStartup { get { return m_pGame; } }

        private bool m_bInited = false;
        private bool m_bAwaked = false;
        private bool m_bStarted = false;
        protected long m_lRuntime = 0;
        protected long m_lRuntimeUnScale = 0;

        private Dictionary<int, AModule>        m_vTypeModdules = new Dictionary<int, AModule>(32);
        private List<AModule>                   m_vModdules = new List<AModule>(32);
        private List<IUpdate>                   m_vAllUpdates = new List<IUpdate>(32);
        private Dictionary<int, IFixedUpdate>   m_vAllFixedUpdates = new Dictionary<int, IFixedUpdate>(32);
        private Dictionary<int, ILateUpdate>    m_vAllLateUpdates = new Dictionary<int, ILateUpdate>(32);
        private Dictionary<int, ITouchInput>    m_vAllTouchInputs = new Dictionary<int, ITouchInput>(32);
        private Dictionary<int, IKeyInput>      m_vAllKeyInputs = new Dictionary<int, IKeyInput>(32);
        private Dictionary<int, IPause>         m_vAllPauses = new Dictionary<int, IPause>(32);
        //------------------------------------------------------
        public void Init(IGame game)
        {
#if UNITY_EDITOR
            if (game.IsEditor())
                ms_pEditorFramework = this;
#endif
            ms_pMainFramework = this;
            if (m_bInited)
                return;
            if (m_pGame != null)
                return;

            AddModule<TouchInput>();
            var atMgr = AddModule<AT.Runtime.AgentTreeManager>(); atMgr.RegisterCallback(this);
            var cutsceneMgr = AddModule<Cutscene.Runtime.CutsceneManager>(); cutsceneMgr.RegisterCallback(this);
            var actorMgr = AddModule<ActorSystem.Runtime.ActorManager>(); actorMgr.RegisterCallback(this);

            m_bAwaked = false;
            m_bStarted = false;

            m_pGame = game;

            OnInit();
            m_bInited = true;
            for (int i = 0; i < m_vModdules.Count; ++i)
            {
                m_vModdules[i].Init(this);
            }
        }
        //------------------------------------------------------
        public void Awake()
        {
            if (m_bAwaked)
                return;
            m_bAwaked = true;
            for (int i = 0; i < m_vModdules.Count; ++i)
            {
                m_vModdules[i].Awake();
            }
            for (int i = 0; i < m_vModdules.Count; ++i)
            {
                RegisterFunction(m_vModdules[i]);
            }
            OnAwake();
        }
        //------------------------------------------------------
        protected abstract void OnInit();
        //------------------------------------------------------
        protected abstract void OnAwake();
        //------------------------------------------------------
        public void Start()
        {
            if (m_bStarted)
                return;
            m_bStarted = true;
            for (int i = 0; i < m_vModdules.Count; ++i)
            {
                m_vModdules[i].Start();
            }
            OnStart();
        }
        protected abstract void OnStart();
        //------------------------------------------------------
        public T GetModule<T>() where T : AModule
        {
            if (m_vTypeModdules.TryGetValue(typeof(T).GetHashCode(), out var module))
                return module as T;
            return null;
        }
        //------------------------------------------------------
        protected T AddModule<T>() where T : AModule, new()
        {
            int hash = typeof(T).GetHashCode();
            if (m_vTypeModdules.TryGetValue(hash, out var existModule))
                return existModule as T;
            for (int i = 0; i < m_vModdules.Count; ++i)
            {
                if (m_vModdules[i] is T)
                    return m_vModdules[i] as T;
            }
            T moduel = new T();

            if (m_bInited)
                moduel.Init(this);
            if (m_bAwaked)
                moduel.Awake();
            if (m_bStarted)
                moduel.Start();

            m_vTypeModdules[hash] = moduel;
            OnAddModule(moduel);

            if (m_bStarted)
                RegisterFunction(moduel);
            m_vModdules.Add(moduel);
            return moduel;
        }
        //------------------------------------------------------
        public void RegisterModule(AModule module)
        {
            //! is inited,do init
            if (m_pGame != null)
                module.Init(this);
            int hashCode = module.GetType().GetHashCode();
            if (m_vTypeModdules.ContainsKey(hashCode))
                return;

            OnAddModule(module);
            RegisterFunction(module);
            m_vModdules.Add(module);
            m_vTypeModdules[hashCode] = module;
        }
        //------------------------------------------------------
        public void UnRegisterModule(AModule module)
        {
            int hashCode = module.GetType().GetHashCode();
            m_vTypeModdules.Remove(hashCode);
            UnRegisterFunction(module);
            m_vModdules.Remove(module);
            OnRemoveModule(module);
        }
        //------------------------------------------------------
        protected virtual void OnAddModule(AModule module)
        {
        }
        //------------------------------------------------------
        protected virtual void OnRemoveModule(AModule module)
        {
        }
        //------------------------------------------------------
        public Coroutine BeginCoroutine(IEnumerator enumerator)
        {
            if (this.m_pGame == null)
                return null;
            return this.m_pGame.BeginCoroutine(enumerator);
        }
        //------------------------------------------------------
        public void EndCoroutine(Coroutine enumerator)
        {
            if (this.m_pGame == null) return;
            this.m_pGame.EndCoroutine(enumerator);
        }
        //------------------------------------------------------
        public void EndCoroutine(IEnumerator enumerator)
        {
            if (this.m_pGame == null) return;
            this.m_pGame.EndCoroutine(enumerator);
        }
        //-------------------------------------------------
        public void OnTouchBegin(TouchInput.TouchData touch)
        {
            //  AgentTreeManager.getInstance().ExecuteEvent
            foreach (var db in m_vAllTouchInputs)
                db.Value.OnTouchBegin(touch);
        }
        //-------------------------------------------------
        public void OnTouchMove(TouchInput.TouchData touch)
        {
            foreach (var db in m_vAllTouchInputs)
                db.Value.OnTouchMove(touch);
        }
        //-------------------------------------------------
        public void OnTouchWheel(float wheel, Vector2 mouse)
        {
            foreach (var db in m_vAllTouchInputs)
                db.Value.OnTouchWheel(wheel, mouse);

        }
        //-------------------------------------------------
        public void OnTouchEnd(TouchInput.TouchData touch)
        {
            foreach (var db in m_vAllTouchInputs)
                db.Value.OnTouchEnd(touch);
        }
        //------------------------------------------------------
        public float GetDeltaTime()
        {
            return Time.deltaTime;
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
        //------------------------------------------------------
        public void RegisterFunction(IUserData pointer, int hashCode = 0)
        {
            if (pointer == null) return;
            if (hashCode == 0) hashCode = pointer.GetType().GetHashCode();

            AModule modulePtr = pointer as AModule;
            if (modulePtr != null)
            {
                var update = pointer as IUpdate;
                if (update != null) m_vAllUpdates.Add(update);
            }

            IFixedUpdate fixedUpdate = pointer as IFixedUpdate;
            if (fixedUpdate != null) m_vAllFixedUpdates[hashCode]= fixedUpdate;


            ILateUpdate lateUpdate = pointer as ILateUpdate;
            if (lateUpdate != null) m_vAllLateUpdates[hashCode] = lateUpdate;


            IPause pauseCB = pointer as IPause;
            if (pauseCB != null) m_vAllPauses[hashCode] = pauseCB;

            ITouchInput touchCb = pointer as ITouchInput;
            if (touchCb != null)
                m_vAllTouchInputs[hashCode] = touchCb;

            OnRegisterFunction(pointer, hashCode);
        }
        //------------------------------------------------------
        protected virtual void OnRegisterFunction(IUserData pointer, int hashCode = 0) { }
        //------------------------------------------------------
        public void UnRegisterFunction(IUserData pointer, int hashCode = 0)
        {
            if (pointer == null) return;
            if (hashCode == 0) hashCode = pointer.GetType().GetHashCode();

            var update = pointer as IUpdate;
            if (update != null) m_vAllUpdates.Remove(update);

            IFixedUpdate fixedUpdate = pointer as IFixedUpdate;
            if (fixedUpdate != null)
                m_vAllFixedUpdates.Remove(hashCode);


            ILateUpdate lateUpdate = pointer as ILateUpdate;
            if (lateUpdate != null)
                m_vAllLateUpdates.Remove(hashCode);

            IPause pauseCB = pointer as IPause;
            if (pauseCB != null)
                m_vAllPauses.Remove(hashCode);

            ITouchInput touchCb = pointer as ITouchInput;
            if (touchCb != null)
                m_vAllTouchInputs.Remove(hashCode);

            OnUnRegisterFunction(pointer, hashCode);
        }
        //-------------------------------------------------
        protected virtual void OnUnRegisterFunction(IUserData pointer, int hashCode = 0) { }
        //------------------------------------------------------
        public void Update(float fFrameTime)
        {
            if (!m_bInited || !m_bStarted || !m_bAwaked)
                return;

#if USE_SERVER
            m_pExterTimer.DoUpdate(fFrameTime);
            m_lRuntime += (int)(fFrameTime * 1000);
            m_lRuntimeUnScale = m_pExterTimer.realtimeSinceStartup;
#else
            m_lRuntime = (int)(Time.time * 1000);
            m_lRuntimeUnScale = (int)(Time.unscaledTime * 1000);
#endif
            bool bLockFrame = false;// IsPause() || IsLogicLock();

            if (!bLockFrame)
            {
                for (int i = 0; i < m_vModdules.Count; ++i)
                {
                    m_vModdules[i].Update(fFrameTime);
                }
                for (int i = 0; i < m_vAllUpdates.Count; i++)
                    m_vAllUpdates[i].Update(fFrameTime);
                OnUpdate(fFrameTime);
            }
        }
        //------------------------------------------------------
        protected virtual void OnUpdate(float fTime)
        {

        }
        //------------------------------------------------------
        public void LateUpdate()
        {
            if (!m_bInited || !m_bStarted || !m_bAwaked)
                return;
            float fDelta = Time.deltaTime;
            foreach (var db in m_vAllLateUpdates)
                db.Value.LateUpdate(fDelta);

            OnLateUpdate(fDelta);

        }
        protected virtual void OnLateUpdate(float fFrameTime) { }
        //------------------------------------------------------
        public void FixedUpdate()
        {
            float fixedDeltaTime = Time.fixedDeltaTime;
            foreach (var db in m_vAllFixedUpdates)
                db.Value.FixedUpdate(fixedDeltaTime);
            OnFixedUpdate(fixedDeltaTime);
        }
        //------------------------------------------------------
        protected virtual void OnFixedUpdate(float fFrameTime) { }
        //------------------------------------------------------
        public void Destroy()
        {
            m_vAllLateUpdates.Clear();
            m_vAllFixedUpdates.Clear();
            m_vAllUpdates.Clear();

            m_vAllTouchInputs.Clear();
            m_vAllKeyInputs.Clear();

            m_vAllPauses.Clear();

            OnDestroy();

            for (int i = 0; i < m_vModdules.Count; ++i)
            {
                m_vModdules[i].Destroy();
            }
            m_vModdules.Clear();
        }
        //------------------------------------------------------
        protected abstract void OnDestroy();
        #region ModelCallback
        //------------------------------------------------------
        public virtual bool OnLoadAsset(string name, Action<UnityEngine.Object> onLoaded, bool bAsync = true)
        {
            var obj = Framework.ED.EditorUtils.EditLoadUnityObject<UnityEngine.Object>(name);
            if (onLoaded != null) onLoaded(obj);
            return obj!=null;
        }
        //------------------------------------------------------
        public virtual bool OnUnloadAsset(UnityEngine.Object pAsset)
        {
            return false;
        }
        //------------------------------------------------------
        public virtual bool OnSpawnInstance(string name, Action<GameObject> onLoaded, bool bAsync = true)
        {
            var obj = Framework.ED.EditorUtils.EditLoadUnityObject<UnityEngine.GameObject>(name);
            if (onLoaded != null && obj) onLoaded(GameObject.Instantiate(obj));

            return obj!=null;
        }
        //------------------------------------------------------
        public virtual bool OnDespawnInstance(GameObject pInstance, string name = null)
        {
            Framework.ED.EditorUtils.Destroy(pInstance);
            return true;
        }
        //------------------------------------------------------
        public virtual void OnCutsceneStatus(int cutsceneInstanceId, EPlayableStatus status)
        {
        }
        //------------------------------------------------------
        public virtual bool OnCutscenePlayableCreateClip(CutscenePlayable playable, CutsceneTrack track, IBaseClip clip)
        {
            return false;
        }
        //------------------------------------------------------
        public virtual bool OnCutscenePlayableDestroyClip(CutscenePlayable playable, CutsceneTrack track, IBaseClip clip)
        {
            return false;
        }
        //------------------------------------------------------
        public virtual bool OnCutscenePlayableFrameClip(CutscenePlayable playable, FrameData frameData)
        {
            return false;
        }
        //------------------------------------------------------
        public virtual bool OnCutscenePlayableFrameClipEnter(CutscenePlayable playable, CutsceneTrack track, FrameData frameData)
        {
            return false;
        }
        //------------------------------------------------------
        public virtual bool OnCutscenePlayableFrameClipLeave(CutscenePlayable playable, CutsceneTrack track, FrameData frameData)
        {
            return false;
        }
        //------------------------------------------------------
        public virtual bool OnCutsceneEventTrigger(CutscenePlayable pPlayablle, CutsceneTrack pTrack, IBaseEvent pEvent)
        {
            return false;
        }
        //------------------------------------------------------
        public virtual bool OnAgentTreeExecute(AgentTree pAgentTree, BaseNode pNode)
        {
            return false;
        }
        //------------------------------------------------------
        public virtual bool OnActorSystemActorCallback(Actor pActor, EActorStatus eStatus, IContextData pTakeData = null)
        {
            return false;
        }
        //------------------------------------------------------
        public virtual bool OnActorSystemActorAttrDirty(Actor pActor, byte attrType, float oldValue, float newValue, IContextData externVar = null)
        {
            return false;
        }
        //------------------------------------------------------
        public virtual bool OnActorSystemActorHitFrame(HitFrameActor hitFrameActor)
        {
            return false;
        }
        //------------------------------------------------------
        public bool OnATExecutedNode(AgentTree pAgentTree, BaseNode pNode)
        {
            return false;
        }
        //------------------------------------------------------
        public virtual void OnSimpleFindPath(Actor pActor, Vector3 toPos, float fSpeed, System.Action<List<Vector3>, float> onCallback)
        {
            Debug.LogWarning("业务层没有实现基于寻路的路径移动，请联系程序实现业务");
        }
        #endregion
    }
}