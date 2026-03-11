/********************************************************************
生成日期:	1:29:2026  10:43
类    名: 	AGame
作    者:	HappLI
描    述:	游戏启动入口，提供框架运行时需要的接口实现，游戏开发者需要在游戏中创建一个GameObject，并挂载这个组件来启动框架。
*********************************************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace Framework.Core
{
    //-----------------------------------------------------
    public abstract class AGame : MonoBehaviour, IGame
    {
        [SerializeField] GamePlaySetting setting = null;
        [SerializeField] ScriptableObject[] datas = null;
        Transform m_pTransform = null;
        //--------------------------------------
        public abstract AFramework GetFramework();
        //--------------------------------------
        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            OnInnerAwake();
            var pFramework = GetFramework();
            if (pFramework != null)
            {
                pFramework.Init(this);
                pFramework.Awake();
            }
        }
        //--------------------------------------
        protected virtual void OnInnerAwake() { }
        //--------------------------------------
        void Start()
        {
            OnInnerStart();
            var pFramework = GetFramework();
            if (pFramework == null) return;
            pFramework.Start();
        }
        //--------------------------------------
        protected virtual void OnInnerStart() { }
        //--------------------------------------
        void Update()
        {
            OnInnerUpdate();
            var pFramework = GetFramework();
            if (pFramework == null) return;
            pFramework.Update(Time.deltaTime);
        }
        //--------------------------------------
        protected virtual void OnInnerUpdate() { }
        //--------------------------------------
        void LateUpdate()
        {
            OnInnerLateUpdate();
            var pFramework = GetFramework();
            if (pFramework == null) return;
            pFramework.LateUpdate(Time.deltaTime);
        }
        //--------------------------------------
        protected virtual void OnInnerLateUpdate() { }
        //--------------------------------------
        void FixedUpdate()
        {
            OnInnerFixedUpdate();
            var pFramework = GetFramework();
            if (pFramework == null) return;
            pFramework.FixedUpdate(Time.fixedDeltaTime);
        }
        //--------------------------------------
        protected virtual void OnInnerFixedUpdate() { }
        //--------------------------------------
        void OnDestroy()
        {
            OnInnerDestroy();
            var pFramework = GetFramework();
            if (pFramework == null) return;
            pFramework.Destroy();
        }
        //--------------------------------------
        protected virtual void OnInnerDestroy() { }
        //--------------------------------------
        public Coroutine BeginCoroutine(IEnumerator coroutine)
        {
            return StartCoroutine(coroutine);
        }
        //--------------------------------------
        public void EndAllCoroutine()
        {
            StopAllCoroutines();
        }
        //--------------------------------------
        public void EndCoroutine(Coroutine cortuine)
        {
            EndCoroutine(cortuine);
        }
        //--------------------------------------
        public void EndCoroutine(IEnumerator cortuine)
        {
            StopCoroutine(cortuine);
        }
        //--------------------------------------
        public ScriptableObject[] GetDatas()
        {
            return datas;
        }
        //--------------------------------------
        public AFrameworkSetting GetSetting()
        {
            return setting;
        }
        //--------------------------------------
        public Transform GetTransform()
        {
            if (m_pTransform == null) m_pTransform = this.transform;
            return m_pTransform;
        }
        //--------------------------------------
        public virtual RenderPipelineAsset GetURPAsset()
        {
            return null;
        }
        //--------------------------------------
        public virtual bool IsEditor()
        {
            return false;
        }
    }
}
