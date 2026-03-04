/********************************************************************
生成日期:	3:10:2019  15:03
类    名: 	AModule
作    者:	HappLI
描    述:	基础模块类
*********************************************************************/
using ExternEngine;
using Framework.Base;

namespace Framework.Core
{
    public abstract class AModule : IUserData
    {
        protected AFramework m_pFramework;
        public void Init(AFramework pFramwork)
        {
            if (m_pFramework == pFramwork)
                return;
            m_pFramework = pFramwork;
            OnInit();
        }
        //-------------------------------------------------
        public void Awake()
        {
            OnAwake();
        }
        //-------------------------------------------------
        public void Start()
        {
            OnStart();
        }
        //-------------------------------------------------
        public void Update(FFloat fFrame)
        {
            OnUpdate(fFrame);
        }
        //-------------------------------------------------
        protected virtual void OnUpdate(FFloat fFrame) { }
        //-------------------------------------------------
        public AFramework GetFramework()
        {
            return m_pFramework;
        }
        //-------------------------------------------------
        public FileSystem GetFileSystem()
        {
            return m_pFramework.GetFileSystem();
        }
        //-------------------------------------------------
        protected virtual void OnStart() { }
        //-------------------------------------------------
        protected virtual void OnInit() { }
        protected virtual void OnAwake() { }
        //-------------------------------------------------
        public void Destroy()
        {
            OnDestroy();
        }
        //-------------------------------------------------
        protected virtual void OnDestroy() { }
    }
}
