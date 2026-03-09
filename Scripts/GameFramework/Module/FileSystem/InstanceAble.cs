/********************************************************************
生成日期:	1:11:2020 10:05
类    名: 	FileSystem
作    者:	HappLI
描    述:	文件操作系统
*********************************************************************/
using Framework.ActorSystem.Runtime;
using Framework.Base;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Core
{
    //------------------------------------------------------
    public enum EInstanceCallbackType
    {
        eEnable,
        eDisiable,
        eRecyled,
        eDestroy,
    }
    //------------------------------------------------------
    public interface IInstanceAbleCallback
    {
        void OnInstanceCallback(InstanceAble pAble, EInstanceCallbackType eType);
    }
    //------------------------------------------------------
    public class InstanceAble : MonoBehaviour, IContextData
    {
        internal static Action<string, GameObject, InstanceAble> OnRealDestroyLinster;
        internal static Action<string, GameObject, InstanceAble> OnDestroyLinster;
        internal static Action<string, GameObject, InstanceAble> OnRecyleLinster;
        internal static Action<string, GameObject, InstanceAble> OnPoolStartLinster;

        AFramework                          m_pFramework;
        Transform                           m_pTransform;
        GameObject                          m_pObject;

        private Dictionary<int, Component>  m_vComponents = null;
        private int                         m_nDefaultLayerFlag = 0;
        GameObject                          m_pPrefab = null;
        string                              m_strPrefabPath = null;
        private List<IInstanceAbleCallback> m_vCallbacks;
        //------------------------------------------------------
        public Transform GetTransform()
        {
            if (m_pTransform == null) m_pTransform = this.transform;
            return m_pTransform;
        }
        //------------------------------------------------------
        public GameObject GetGameObject()
        {
            if (m_pObject == null) m_pObject = this.gameObject;
            return m_pObject;
        }
        //------------------------------------------------------
        public GameObject Prefab
        {
            get
            {
                return m_pPrefab;
            }
        }
        //------------------------------------------------------
        public void SetPosition(Vector3 postion, bool bLocal = false)
        {
            if (bLocal)
                GetTransform().localPosition = postion;
            else
                GetTransform().position = postion;
        }
        //------------------------------------------------------
        public Vector3 GetPosition(bool bLocal = false)
        {
            if (bLocal)
                return GetTransform().localPosition;
            else
                return GetTransform().position;
        }
        //------------------------------------------------------
        public void SetRotation(Quaternion rot, bool bLocal = false)
        {
            if (bLocal)
                GetTransform().rotation = rot;
            else
                GetTransform().localRotation = rot;
        }
        //------------------------------------------------------
        public Quaternion GetRotation(bool bLocal = false)
        {
            if (bLocal)
                return GetTransform().localRotation;
            else
                return GetTransform().rotation;
        }
        //------------------------------------------------------
        public void SetForward(Vector3 forward)
        {
            GetTransform().forward = forward;
        }
        //------------------------------------------------------
        public Vector3 GetForward()
        {
            return GetTransform().forward;
        }
        //------------------------------------------------------
        public void SetUp(Vector3 up)
        {
            GetTransform().up = up;
        }
        //------------------------------------------------------
        public Vector3 GetUp()
        {
            return GetTransform().up;
        }
        //------------------------------------------------------
        public void SetEulerAngle(Vector3 eulerAngles, bool bLocal = false)
        {
            if (bLocal)
                GetTransform().localEulerAngles = eulerAngles;
            else
                GetTransform().eulerAngles = eulerAngles;
        }
        //------------------------------------------------------
        public Vector3 GetEulerAngle(bool bLocal = false)
        {
            if (bLocal)
                return GetTransform().localEulerAngles;
            else
                return GetTransform().eulerAngles;
        }
        //------------------------------------------------------
        public void SetScale(Vector3 scale)
        {
            GetTransform().localScale = scale;
        }
        //------------------------------------------------------
        public Vector3 GetScale(bool bLocal = true)
        {
            if (bLocal) return GetTransform().localScale;
            return GetTransform().lossyScale;
        }
        //------------------------------------------------------
        public void SetTransform(Matrix4x4 matrix, bool bLocal = false)
        {
            if(bLocal) GetTransform().SetLocalPositionAndRotation(matrix.GetColumn(3), matrix.rotation);
            else GetTransform().SetPositionAndRotation(matrix.GetColumn(3), matrix.rotation);
            GetTransform().localScale = matrix.lossyScale;
        }
        //------------------------------------------------------
        public void SetTransform(Vector3 position, Quaternion rot, Vector3 scale, bool bLocal = false)
        {
            if (bLocal) GetTransform().SetLocalPositionAndRotation(position, rot);
            else GetTransform().SetPositionAndRotation(position, rot);
            GetTransform().localScale = scale;
        }
        //------------------------------------------------------
        internal void SetLockInfo(string prefabPath, GameObject prefabObj)
        {
            m_pPrefab = prefabObj;
            m_strPrefabPath = prefabPath;
        }
        //------------------------------------------------------
        public void RegisterCallback(IInstanceAbleCallback callback)
        {
            if (m_vCallbacks == null) m_vCallbacks = new List<IInstanceAbleCallback>(2);
            if (m_vCallbacks.Contains(callback)) return;
            m_vCallbacks.Add(callback);
        }
        //------------------------------------------------------
        public void UnRegisterCallback(IInstanceAbleCallback callback)
        {
            if (m_vCallbacks == null) return;
            m_vCallbacks.Remove(callback);
        }
        //------------------------------------------------------
        public void SetActive(bool bActive)
        {
            GetGameObject().SetActive(bActive);
        }
        //------------------------------------------------------
        public void Destroy(float delayTime = 0)
        {

        }
        //------------------------------------------------------
        public virtual void RecyleDestroy(int recyleMax = 2)
        {
#if UNITY_EDITOR
            if (m_pFramework == null || !AFramework.isStartup)
            {
                RealDestroy();
                return;
            }
#endif
#if !USE_SERVER
            m_pFramework.GetFileSystem().DeSpawnInstance(this, recyleMax);
#endif
        }
        //------------------------------------------------------
        internal void RealDestroy()
        {
            if (OnDestroyLinster != null)
                OnDestroyLinster(m_strPrefabPath, m_pPrefab, this);

            if (gameObject)
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    GameObject.DestroyImmediate(gameObject);
                else
                    GameObject.Destroy(gameObject);
#else
                GameObject.Destroy(gameObject);
#endif
            }
        }
        //------------------------------------------------------
        public T GetBehaviour<T>() where T : Component
        {
            int hashCode = typeof(T).GetHashCode();
            if (m_vComponents == null)
            {
                m_vComponents = new Dictionary<int, Component>(4);
            }
            Component retCom;
            if (m_vComponents.TryGetValue(hashCode, out retCom))
                return retCom as T;
            retCom = GetComponent<T>();
            m_vComponents[hashCode] = retCom;
            return retCom as T;
        }
        //------------------------------------------------------
        public T AddBehaviour<T>(System.Type type) where T : Component
        {
            int hashCode = type.GetHashCode();
            if (m_vComponents != null)
            {
                Component outCom;
                if (m_vComponents.TryGetValue(hashCode, out outCom))
                {
                    return outCom as T;
                }
            }
            T newComp = m_pObject.AddComponent(type) as T;
            if (newComp == null) return null;
            if (m_vComponents == null) m_vComponents = new Dictionary<int, Component>(2);
            m_vComponents.Add(hashCode, newComp);
            return newComp;
        }
        //------------------------------------------------------
        protected void OnDestroy()
        {
            if (OnRealDestroyLinster != null)
                OnRealDestroyLinster(m_strPrefabPath, m_pPrefab, this);
            DoCallback(EInstanceCallbackType.eDestroy);
        }
        //------------------------------------------------------
        protected void DoCallback(EInstanceCallbackType type)
        {
            if (m_pFramework != null)
                m_pFramework.GetFileSystem()?.OnInstanceCallback(this,type);
            if (m_vCallbacks != null)
            {
                for (int i = 0; i < m_vCallbacks.Count; ++i)
                {
                    m_vCallbacks[i].OnInstanceCallback(this, type);
                }
            }
        }
    }
}