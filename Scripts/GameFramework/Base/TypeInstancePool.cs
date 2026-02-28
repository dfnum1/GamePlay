/********************************************************************
生成日期:	5:11:2020  20:36
类    名: 	TypeInstancePool
作    者:	HappLI
描    述:	对象类型池子
*********************************************************************/
using System.Collections.Generic;

namespace Framework.Core
{
    public abstract class TypeObject : IUserData
    {
        AFramework m_pFramework;
        internal AFramework GetFramework()
        {
            return m_pFramework;
        }
        internal void SetFramework(AFramework pFramework)
        {
            m_pFramework = pFramework;
        }
        public virtual void Destroy() { }
    }
    //--------------------------------------------------------
    public static class TypeInstancePool
    {
        const int POOL_COUNT = 128;
        static Dictionary<System.IntPtr, Stack<TypeObject>> ms_vPools = new Dictionary<System.IntPtr, Stack<TypeObject>>(16);
        //--------------------------------------------------------
        public static T Malloc<T>(AFramework pFramework = null) where T : TypeObject, new()
        {
            if(pFramework!=null)
            {
                return pFramework.FrameworkShareCache.Malloc<T>();
            }
            System.IntPtr handle = typeof(T).TypeHandle.Value;
            if (ms_vPools.TryGetValue(handle, out var pool) && pool.Count > 0)
            {
                var user = pool.Pop();
                user.SetFramework(pFramework);
                return user as T;
            }
            T newT = new T();
            newT.SetFramework(pFramework);
            return newT;
        }
        //--------------------------------------------------------
        public static void Free(this TypeObject pObj, AFramework pFramework = null)
        {
            if (pFramework != null)
            {
                pFramework.FrameworkShareCache.Free(pObj);
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
    }
}