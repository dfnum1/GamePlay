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
        //-----------------------------------------------------
        public static void ResetGameObject(GameObject gameObject, EResetType type = EResetType.Local)
        {
            if (gameObject == null) return;
            ResetGameObject(gameObject.transform, type);
        }
        //-----------------------------------------------------
        public static void ResetGameObject(Transform pTrans, EResetType type = EResetType.Local)
        {
            if (pTrans == null) return;
            if (type == EResetType.World || type == EResetType.All)
            {
                pTrans.position = Vector3.zero;
                pTrans.rotation = Quaternion.identity;
                pTrans.eulerAngles = Vector3.zero;
            }

            if (type == EResetType.Local || type == EResetType.All)
            {
                pTrans.localPosition = Vector3.zero;
                pTrans.localRotation = Quaternion.identity;
                pTrans.localEulerAngles = Vector3.zero;
            }
        }
        //-----------------------------------------------------
        static public void UpdatePosition(ref Matrix4x4 mtWorld, Vector3 position)
        {
            Vector4 colum = mtWorld.GetColumn(3);
            colum.x = position.x;
            colum.y = position.y;
            colum.z = position.z;
            mtWorld.SetColumn(3, colum);
        }
        //-----------------------------------------------------
        static public void OffsetPosition(ref Matrix4x4 mtWorld, Vector3 offset)
        {
            if (offset.sqrMagnitude <= 0) return;
            Vector4 colum = mtWorld.GetColumn(3);
            colum.x += offset.x;
            colum.y += offset.y;
            colum.z += offset.z;
            mtWorld.SetColumn(3, colum);
        }
        //-----------------------------------------------------
        static public void UpdateScale(ref Matrix4x4 mtWorld, Vector3 scale)
        {
            mtWorld.m00 = scale.x;
            mtWorld.m11 = scale.y;
            mtWorld.m22 = scale.z;
        }
        //-----------------------------------------------------
        static public Vector3 GetPosition(Matrix4x4 mtWorld)
        {
            return mtWorld.GetColumn(3);
        }
        //------------------------------------------------------
        static public Vector3 RoateAround(Vector3 anchor, Vector3 point, Quaternion rot)
        {
            return rot * (point - anchor) + anchor;
        }
        //------------------------------------------------------
#if UNITY_EDITOR
        private static System.Reflection.MethodInfo ms_pLoadUnityPlugin = null;
        //-----------------------------------------------------
        static void Init()
        {
            if(ms_pLoadUnityPlugin == null)
            {
                foreach (var ass in System.AppDomain.CurrentDomain.GetAssemblies())
                {
                    Type[] types = ass.GetTypes();
                    for (int i = 0; i < types.Length; ++i)
                    {
                        Type tp = types[i];
                        if (tp.IsDefined(typeof(CutsceneEditorLoaderAttribute), false))
                        {
                            var clipAttri = tp.GetCustomAttribute<CutsceneEditorLoaderAttribute>();
                            ms_pLoadUnityPlugin = tp.GetMethod(clipAttri.method, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                            break;
                        }
                    }
                }
            }
        }
        //-----------------------------------------------------
        public static UnityEngine.Object EditLoadUnityObject(string file)
        {
            Init();
            if (ms_pLoadUnityPlugin != null)
            {
                var returnObj = ms_pLoadUnityPlugin.Invoke(null, new object[] { file });
                if (returnObj != null && returnObj is UnityEngine.Object)
                    return returnObj as UnityEngine.Object;
            }
            return UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(file);
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