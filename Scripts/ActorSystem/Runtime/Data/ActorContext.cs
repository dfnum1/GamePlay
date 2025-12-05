/********************************************************************
生成日期:	06:30:2025
类    名: 	ActorContext
作    者:	HappLI
描    述:	Actor单位上下文
*********************************************************************/
using Framework.AT.Runtime;
using UnityEngine;
namespace Framework.ActorSystem.Runtime
{
    //--------------------------------------------------------
    public struct ActorContext : IUserData
    {
        public IContextData pContextData { get; private set; }
        public UnityEngine.Object pUnityObject { get; private set; }
        public UnityEngine.GameObject pUnityGameObject { get; private set; }
        public UnityEngine.Transform pUnityTransform { get; private set; }
        public static ActorContext NULL = new ActorContext();
        //--------------------------------------------------------
        public T CastContextData<T>() where T : class, IContextData
        {
            if (pContextData == null) return null;
            return pContextData as T;
        }
        //--------------------------------------------------------
        public bool IsValid()
        {
            return pUnityObject != null || pContextData != null || pUnityTransform != null || pUnityGameObject!=null;
        }
        //--------------------------------------------------------
        internal void SetContextData(Actor pActor, IContextData pData)
        {
            if (pActor == null)
            {
                Debug.LogAssertion("ActorContext pActor is null");
                return;
            }
            Clear(pActor);
            pContextData = pData;
            if (pData == null) return;
            if(pData is AActorComponent)
            {
                AActorComponent component = pData as AActorComponent;
                pUnityTransform = component.transform;
                pUnityGameObject = component.gameObject;
                pUnityObject = component;
            }
            else
            {
                Debug.LogWarning("Actor SetContextData pData 不是 AActorComponent 类型");
            }
        }
        //--------------------------------------------------------
        internal void SetContextData(Actor pActor, Transform pTransfrom)
        {
            if (pActor == null)
            {
                Debug.LogAssertion("ActorContext pActor is null");
                return;
            }
            Clear(pActor);
            this.pUnityTransform = pTransfrom;
            this.pUnityObject = pTransfrom;
            if (pTransfrom)
                pUnityGameObject = pTransfrom.gameObject;
        }
        //--------------------------------------------------------
        internal void SetContextData(Actor pActor, GameObject pGO)
        {
            if (pActor == null)
                return;
            Clear(pActor);
            if (pGO == null) return;
            this.pUnityTransform = pGO.transform;
            this.pUnityObject = pGO;
            pUnityGameObject = pGO;
        }
        //--------------------------------------------------------
        internal void SetContextData(Actor pActor, UnityEngine.Object pObj)
        {
            if (pActor == null)
                return;
            Clear(pActor);
            this.pUnityObject = pObj;
            if (pObj == null)
                return;
            if(pObj is AActorComponent)
            {
                AActorComponent component = pObj as AActorComponent;
                pUnityTransform = component.transform;
                pUnityGameObject = component.gameObject;
            }
            else if (pObj is GameObject)
            {
                pUnityGameObject = pObj as GameObject;
                pUnityTransform = pUnityGameObject.transform;
            }
            else if (pObj is Transform)
            {
                pUnityTransform = pObj as Transform;
                pUnityGameObject = pUnityTransform.gameObject;
            }
        }
        //--------------------------------------------------------
        internal void Clear(Actor pActor)
        {
            if(pActor != null)
            {
                if (pUnityGameObject) pActor.GetActorManager().DespawnInstance(pUnityGameObject);
            }
            else
            {
                Debug.LogAssertion("ActorContext Clear pActor is null");
            }
            pContextData = null;
            pUnityObject = null;
            pUnityTransform = null;
        }
        //--------------------------------------------------------
        internal void ClearNoDespawn()
        {
            pContextData = null;
            pUnityObject = null;
            pUnityGameObject = null;
            pUnityTransform = null;
        }
    }
}