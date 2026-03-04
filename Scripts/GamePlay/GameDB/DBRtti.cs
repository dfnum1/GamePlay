/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	DBRtti
作    者:	HappLI
描    述:	DB RTTI
*********************************************************************/
using Framework.Core;
using System;
using System.Collections.Generic;
namespace Framework.Db
{
    public static class DBRtti
    {
        public delegate AProxyDB MallocProxyDBEvent(AFramework pFramework, int type);
        static Dictionary<int, System.Type> ms_vIdTypes;
        static Dictionary<System.Type, int> ms_vTypeIds;
        static Dictionary<int, MallocProxyDBEvent> ms_vMalloc;
        //------------------------------------------------------
        public static void Register(int id, System.Type type, MallocProxyDBEvent mallocFunc)
        {
            if (ms_vIdTypes == null) ms_vIdTypes = new Dictionary<int, Type>(8);
            ms_vIdTypes[id] = type;
            if (ms_vTypeIds == null) ms_vTypeIds = new Dictionary<Type,int>(8);
            ms_vTypeIds[type] = id;
            if(mallocFunc !=null)
            {
                if (ms_vMalloc == null) ms_vMalloc = new Dictionary<int, MallocProxyDBEvent>(8);
                ms_vMalloc[id] = mallocFunc;
            }
        }
        //------------------------------------------------------
        internal static System.Type GetType(int id)
        {
            if (ms_vIdTypes == null) return null;
            if (ms_vIdTypes.TryGetValue(id, out var type))
                return type;
            return null;
        }
        //------------------------------------------------------
        internal static int GetTypeId(System.Type type)
        {
            if (ms_vTypeIds == null) return -1;
            if (ms_vTypeIds.TryGetValue(type, out var id))
                return id;
            return -1;
        }
        //------------------------------------------------------
        internal static T Malloc<T>(User pUser, int type) where T : AProxyDB
        {
            if (ms_vMalloc != null)
            {
                if (ms_vMalloc.TryGetValue(type, out var mallockFunc))
                {
#if UNITY_EDITOR
                    ms_MallocInnter = typeof(T);
#endif
                    var handleObj = mallockFunc(pUser.GetFramework(), type);
#if UNITY_EDITOR
                    ms_MallocInnter = null;
#endif
                    if (handleObj == null) return null;
                    handleObj.Init(pUser);
                    return handleObj as T;
                }
            }
#if UNITY_EDITOR
            var typeType= GetType(type);
            if (typeType == null) return null;
            return (T)Activator.CreateInstance(typeType);
#endif
        }
#if UNITY_EDITOR
        static System.Type ms_MallocInnter = null;
        //------------------------------------------------------
        internal static void CheckLegal(System.Type type)
        {
            UnityEngine.Debug.Assert(ms_MallocInnter == type, "禁止使用new " + type.Name + " 创建实例!!!");
        }
#endif
    }
}
