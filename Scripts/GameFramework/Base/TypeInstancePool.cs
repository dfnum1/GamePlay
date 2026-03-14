/********************************************************************
生成日期:	5:11:2020  20:36
类    名: 	TypeInstancePool
作    者:	HappLI
描    述:	对象类型池子
*********************************************************************/
using Framework.Core;
using System.Collections.Generic;
using System.Diagnostics;

namespace Framework.Base
{
    [AT.Runtime.ATInteralExport("TypeObject", -14)]
    public abstract class TypeObject : IUserData
    {
        AFramework m_pFramework;
        public TypeObject()
        {
#if UNITY_EDITOR
            TypeInstancePool.CheckLegal(this.GetType());
#endif
        }
        //--------------------------------------------------------
        internal AFramework GetFramework()
        {
            return m_pFramework;
        }
        //--------------------------------------------------------
        internal FileSystem GetFileSystem()
        {
            if (m_pFramework == null) return null;
            return m_pFramework.GetFileSystem();
        }
        //--------------------------------------------------------
        internal void SetFramework(AFramework pFramework)
        {
            m_pFramework = pFramework;
        }
        public virtual void Destroy() { }
        //--------------------------------------------------------
        public void Free()
        {
            TypeInstancePool.Free(this);
        }
    }
    //--------------------------------------------------------
    public static class TypeInstancePool
    {
        const int POOL_COUNT = 128;
        static Dictionary<System.IntPtr, Stack<TypeObject>> ms_vPools = new Dictionary<System.IntPtr, Stack<TypeObject>>(16);
        //--------------------------------------------------------
        public static T Malloc<T>(AFramework pFramework) where T : TypeObject, new()
        {
#if UNITY_EDITOR
            ms_MallocInnter = typeof(T);
#endif
            if (pFramework!=null)
            {
                T newObj = pFramework.ShareCache.Malloc<T>();
#if UNITY_EDITOR
                ms_MallocInnter = null;
#endif
                return newObj;
            }
#if UNITY_EDITOR
            UnityEngine.Debug.LogError(typeof(T).Name + " new 操作不在框架内！！！");
#endif
            System.IntPtr handle = typeof(T).TypeHandle.Value;
            if (ms_vPools.TryGetValue(handle, out var pool) && pool.Count > 0)
            {
                var user = pool.Pop();
                user.SetFramework(pFramework);
#if UNITY_EDITOR
                ms_MallocInnter = null;
#endif
                return user as T;
            }
            T newT = new T();
            newT.SetFramework(pFramework);
#if UNITY_EDITOR
            ms_MallocInnter = null;
#endif
            return newT;
        }
        //--------------------------------------------------------
        public static void Free(this TypeObject pObj)
        {
            if (pObj == null) return;
            if (pObj.GetFramework() != null)
            {
                pObj.GetFramework().ShareCache.Free(pObj);
                return;
            }
            System.IntPtr handle = pObj.GetType().TypeHandle.Value;
            if (!ms_vPools.TryGetValue(handle, out var pool))
            {
                pool = new Stack<TypeObject>(POOL_COUNT);
                ms_vPools[handle] = pool;
            }
            pObj.Destroy();
            if (pool.Count < POOL_COUNT) pool.Push(pObj);
        }
#if UNITY_EDITOR
        static System.Type ms_MallocInnter = null;
        //------------------------------------------------------
        internal static void SetMallockInner(System.Type type)
        {
            ms_MallocInnter = type;
        }
        //------------------------------------------------------
        internal static void CheckLegal(System.Type type)
        {
            Debug.Assert(ms_MallocInnter == type, "禁止使用new " + type.Name + " 创建实例,请使用TypeInstancePool.Malloc<"+ type.Name + ">(pFramewrok) 创建!!!");
            if (ms_MallocInnter != type)
            {
                UnityEngine.Debug.Break();
            }
        }
#endif
    }
}