/********************************************************************
生成日期:	11:03:2023
类    名: 	ActorSystemUtil
作    者:	HappLI
描    述:	工具类
*********************************************************************/
using Framework.ActorSystem.Runtime;
using Framework.Cutscene.Runtime;
using System;
#if UNITY_EDITOR
using System.Reflection;
#endif
using UnityEngine;

namespace Framework.ActorSystem.Editor
{
    public class ActorSystemUtil
    {
        static System.Collections.Generic.List<ActorManager> ms_vActorManager = null;
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
#endif
    }
}