/********************************************************************
生成日期:	11:07:2025
类    名: 	GameWorldHandler
作    者:	HappLI
描    述:	游戏世界句柄
*********************************************************************/
using Framework.Core;
using Framework.State.Editor;
using System;
using System.Collections.Generic;
namespace Framework.State.Runtime
{
    public class GameWorldHandler
    {
        public delegate TypeObject OnMallocTypeObject(int typeId);
#if UNITY_EDITOR
        static System.Type                          ms_MallocInnter = null;
#endif
        static Dictionary<int, OnMallocTypeObject>  ms_MallocHandles = new Dictionary<int, OnMallocTypeObject>(128);
        public static void Register(int callId, OnMallocTypeObject callFunction)
        {
            if (callId == 0 && callFunction == null)
                return;
            ms_MallocHandles[callId] = callFunction;
        }
        //-----------------------------------------------------
        internal static void CheckInnerMalloc(System.Type type)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Assert(ms_MallocInnter != type, "禁止使用new " + type.Name + " 创建实例!!!");
#endif
        }
        //-----------------------------------------------------
        internal static T Malloc<T>(int typeId) where T : TypeObject
        {
            if (ms_MallocHandles.TryGetValue(typeId, out var handle))
            {
#if UNITY_EDITOR
                ms_MallocInnter = typeof(T);
#endif
                var handleObj = handle(typeId);
#if UNITY_EDITOR
                ms_MallocInnter = null;
#endif
                if (handleObj == null) return null;
                return handleObj as T;
            }
#if UNITY_EDITOR
            var type = StateEditorUtil.GetStateWorldType(typeId);
            if (type == null) return null;
            return Activator.CreateInstance(type) as T;
#else
            return null;
#endif
        }
    }
}

