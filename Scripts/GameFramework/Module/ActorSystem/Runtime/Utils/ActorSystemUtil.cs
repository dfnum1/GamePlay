/********************************************************************
生成日期:	11:03:2023
类    名: 	ActorSystemUtil
作    者:	HappLI
描    述:	工具类
*********************************************************************/
using Framework.Cutscene.Runtime;
using System;
#if UNITY_EDITOR
using System.Reflection;
using UnityEditor.Presets;

#endif
using UnityEngine;

namespace Framework.ActorSystem.Runtime
{
    public enum EResetType
    {
        Local,
        World,
        All,
    }
    public class ActorSystemUtil
    {
        public const float GTRAVITY_VALUE = 9.8f;
#if UNITY_EDITOR
        static System.Collections.Generic.HashSet<ActorManager> ms_vActorManager = null;
#endif
        //-----------------------------------------------------
        internal static void Register(ActorManager actorMgr)
        {
#if UNITY_EDITOR
            if (ms_vActorManager == null)
                ms_vActorManager = new System.Collections.Generic.HashSet<ActorManager>(2);
            ms_vActorManager.Add(actorMgr);
#endif
        }
        //-----------------------------------------------------
        internal static void Unregister(ActorManager actorMgr)
        {
#if UNITY_EDITOR
            if (ms_vActorManager == null) return;
            ms_vActorManager.Remove(actorMgr);
#endif
        }
        //-----------------------------------------------------
        internal static void RefreshProjectileDatas(AProjectileDatas projectiles)
        {
#if UNITY_EDITOR
            if (ms_vActorManager == null) return;
            foreach(var db in ms_vActorManager)
            {
                db.SetProjectileDatas(projectiles);
            }
#endif
        }
        //------------------------------------------------------
#if UNITY_EDITOR
        //-----------------------------------------------------
        public static UnityEngine.Object EditLoadUnityObject(string file)
        {
            return Framework.ED.EditorUtils.EditLoadUnityObject(file);
        }
        //------------------------------------------------------
        public static void DrawBoundingBox(Vector3 vCenter, Vector3 vHalf, Matrix4x4 mWorld, Color dwColor, bool bGizmos)
        {
            DrawBoundingBoxLine(vCenter, vHalf, mWorld, new Vector3(1.0f, 1.0f, 1.0f), new Vector3(-1.0f, 1.0f, 1.0f), dwColor, bGizmos);
            DrawBoundingBoxLine(vCenter, vHalf, mWorld, new Vector3(-1.0f, 1.0f, 1.0f), new Vector3(-1.0f, 1.0f, -1.0f), dwColor, bGizmos);
            DrawBoundingBoxLine(vCenter, vHalf, mWorld, new Vector3(-1.0f, 1.0f, -1.0f), new Vector3(1.0f, 1.0f, -1.0f), dwColor, bGizmos);
            DrawBoundingBoxLine(vCenter, vHalf, mWorld, new Vector3(1.0f, 1.0f, -1.0f), new Vector3(1.0f, 1.0f, 1.0f), dwColor, bGizmos);

            DrawBoundingBoxLine(vCenter, vHalf, mWorld, new Vector3(1.0f, -1.0f, 1.0f), new Vector3(-1.0f, -1.0f, 1.0f), dwColor, bGizmos);
            DrawBoundingBoxLine(vCenter, vHalf, mWorld, new Vector3(-1.0f, -1.0f, 1.0f), new Vector3(-1.0f, -1.0f, -1.0f), dwColor, bGizmos);
            DrawBoundingBoxLine(vCenter, vHalf, mWorld, new Vector3(-1.0f, -1.0f, -1.0f), new Vector3(1.0f, -1.0f, -1.0f), dwColor, bGizmos);
            DrawBoundingBoxLine(vCenter, vHalf, mWorld, new Vector3(1.0f, -1.0f, -1.0f), new Vector3(1.0f, -1.0f, 1.0f), dwColor, bGizmos);

            DrawBoundingBoxLine(vCenter, vHalf, mWorld, new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, -1.0f, 1.0f), dwColor, bGizmos);
            DrawBoundingBoxLine(vCenter, vHalf, mWorld, new Vector3(-1.0f, 1.0f, 1.0f), new Vector3(-1.0f, -1.0f, 1.0f), dwColor, bGizmos);
            DrawBoundingBoxLine(vCenter, vHalf, mWorld, new Vector3(1.0f, 1.0f, -1.0f), new Vector3(1.0f, -1.0f, -1.0f), dwColor, bGizmos);
            DrawBoundingBoxLine(vCenter, vHalf, mWorld, new Vector3(-1.0f, 1.0f, -1.0f), new Vector3(-1.0f, -1.0f, -1.0f), dwColor, bGizmos);
        }
        //-----------------------------------------------------
        static void DrawBoundingBoxLine(Vector3 vCenter, Vector3 vHalf, Matrix4x4 mWorld, Vector3 vStart, Vector3 vEnd, Color dwColor, bool bGizmos)
        {
            Vector3 v1 = new Vector3(vHalf.x * vStart.x, vHalf.y * vStart.y, vHalf.z * vStart.z);
            Vector3 v2 = new Vector3(vHalf.x * vEnd.x, vHalf.y * vEnd.y, vHalf.z * vEnd.z);
            v1 = v1 + vCenter;
            v2 = v2 + vCenter;
            v1 = mWorld.MultiplyPoint(v1);
            v2 = mWorld.MultiplyPoint(v2);
            DrawLine(v1, v2, dwColor, bGizmos);
        }
        //------------------------------------------------------
        static void DrawLine(Vector3 start, Vector3 end, Color dwColor, bool bGizmos)
        {
            if (bGizmos)
            {
                Color colr = UnityEngine.Gizmos.color;
                UnityEngine.Gizmos.color = dwColor;
                UnityEngine.Gizmos.DrawLine(start, end);
                UnityEngine.Gizmos.color = colr;
            }
            else
            {
#if UNITY_EDITOR
                Color colr = UnityEditor.Handles.color;
                UnityEditor.Handles.color = dwColor;
                UnityEditor.Handles.DrawLine(start, end);
                UnityEditor.Handles.color = colr;
#endif
            }
        }
#endif
    }
}