/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	VariableUtil
作    者:	HappLI
描    述:	变量工具
*********************************************************************/
using System.Collections.Generic;
using UnityEngine;
using Framework.Core;


#if USE_FIXEDMATH
using ExternEngine;
#else
using FFloat = System.Single;
using FMatrix4x4 = UnityEngine.Matrix4x4;
using FQuaternion = UnityEngine.Quaternion;
using FVector2 = UnityEngine.Vector2;
using FVector3 = UnityEngine.Vector3;
using FBounds = UnityEngine.Bounds;
using FRay = UnityEngine.Ray;
#endif

namespace Framework.Base
{
    public static class VariableUtil
    {
        //------------------------------------------------------
        static public bool ToBool(this IUserData data)
        {
            if (data == null) return false;
            if (data is ByteVar) return ((ByteVar)data).boolVal;
            if (data is Value1Var) return ((Value1Var)data).boolVal;
            return false;
        }
        //------------------------------------------------------
        static public short ToShort(this IUserData data)
        {
            if (data == null) return 0;
            if (data is Value1Var) return (short)((Value1Var)data).shortVal0;
            else if (data is Value2Var) return ((Value2Var)data).shortVal0;
            return 0;
        }
        //------------------------------------------------------
        static public int ToInt(this IUserData data)
        {
            if (data == null) return 0;
            if (data is Value1Var) return ((Value1Var)data).intVal;
            else if (data is Value2Var) return ((Value2Var)data).intVal0;
            else if (data is Value3Var) return ((Value3Var)data).intVal0;
            return 0;
        }
        //------------------------------------------------------
        static public uint ToUInt(this IUserData data)
        {
            if (data == null) return 0xffffffff;
            if (data is Value1Var) return ((Value1Var)data).uintVal;
            else if (data is Value2Var) return (uint)((Value2Var)data).intVal0;
            else if (data is Value3Var) return (uint)((Value3Var)data).intVal0;
            return 0xffffffff;
        }
        //------------------------------------------------------
        static public float ToFloat(this IUserData data)
        {
            if (data == null) return 0;
            if (data is Value1Var) return ((Value1Var)data).floatVal;
            else if (data is Value2Var) return ((Value2Var)data).floatVal0;
            else if (data is Value3Var) return ((Value3Var)data).floatVal0;
            return 0;
        }
        //------------------------------------------------------
        static public float ToLong(this IUserData data)
        {
            if (data == null) return 0;
            if (data is Value2Var) return ((Value2Var)data).longValue;
            else if (data is Value3Var) return ((Value3Var)data).longValue;
            return 0;
        }
        //------------------------------------------------------
        static public Vector2 ToVec2(this IUserData data)
        {
            if (data == null) return Vector2.zero;
            if (data is Value2Var)
            {
                Value2Var v3 = (Value2Var)data;
                return new Vector2(v3.floatVal0, v3.floatVal1);
            }
            else if (data is Value3Var)
            {
                Value3Var v3 = (Value3Var)data;
                return new Vector2(v3.floatVal0, v3.floatVal1);
            }
            return Vector2.zero;
        }
        //------------------------------------------------------
        static public Vector2Int ToVec2Int(this IUserData data)
        {
            if (data == null) return Vector2Int.zero;
            if (data is Value2Var)
            {
                Value2Var v3 = (Value2Var)data;
                return new Vector2Int(v3.intVal0, v3.intVal1);
            }
            else if (data is Value3Var)
            {
                Value3Var v3 = (Value3Var)data;
                return new Vector2Int(v3.intVal0, v3.intVal1);
            }
            return Vector2Int.zero;
        }
        //------------------------------------------------------
        static public Vector3 ToVec3(this IUserData data)
        {
            if (data == null) return Vector3.zero;
            if (data is Value3Var)
            {
                Value3Var v3 = (Value3Var)data;
                return new Vector3(v3.floatVal0, v3.floatVal1, v3.floatVal2);
            }
            return Vector3.zero;
        }
        //------------------------------------------------------
        static public Vector3Int ToVec3Int(this IUserData data)
        {
            if (data == null) return Vector3Int.zero;
            if (data is Value3Var)
            {
                Value3Var v3 = (Value3Var)data;
                return new Vector3Int(v3.intVal0, v3.intVal1, v3.intVal2);
            }
            return Vector3Int.zero;
        }
        //------------------------------------------------------
        static public string ToString(this IUserData data)
        {
            if (data == null) return string.Empty;
            if (data is StringVar) return ((StringVar)data).strValue;
            return string.Empty;
        }
        //------------------------------------------------------
        public static List<FVector3> CacheFVector3List(AFramework pFramework,int capcity =4)
        {
            if (pFramework == null) return new List<FVector3>(capcity);
            return pFramework.ShareCache.CacheFVector3List(capcity);
        }
        //------------------------------------------------------
        public static List<Vector3> CacheVector3List(AFramework pFramework, int capcity = 4)
        {
            if (pFramework == null) return new List<Vector3>(capcity);
            return pFramework.ShareCache.CacheVector3List(capcity);
        }
        //-----------------------------------------------------
        public static List<int> CacheIntList(AFramework pFramework, int capcity = 4)
        {
            if (pFramework == null) return new List<int>(capcity);
            return pFramework.ShareCache.CacheIntList(capcity);
        }
        //-----------------------------------------------------
        public static List<float> CacheFloatList(AFramework pFramework, int capcity = 4)
        {
            if (pFramework == null) return new List<float>(capcity);
            return pFramework.ShareCache.CacheFloatList(capcity);
        }
        //-----------------------------------------------------
        public static List<long> CacheLongList(AFramework pFramework, int capcity = 4)
        {
            if (pFramework == null) return new List<long>(capcity);
            return pFramework.ShareCache.CacheLongList(capcity);
        }
        //-----------------------------------------------------
        public static List<double> CacheDoubleList(AFramework pFramework, int capcity = 4)
        {
            if (pFramework == null) return new List<double>(capcity);
            return pFramework.ShareCache.CacheDoubleList(capcity);
        }
        //-----------------------------------------------------
        public static HashSet<int> CacheIntSet(AFramework pFramework, int capcity = 4)
        {
            if (pFramework == null) return new HashSet<int>(capcity);
            return pFramework.ShareCache.CacheIntSet(capcity);
        }
        //-----------------------------------------------------
        public static HashSet<float> CacheFloatSet(AFramework pFramework, int capcity = 4)
        {
            if (pFramework == null) return new HashSet<float>(capcity);
            return pFramework.ShareCache.CacheFloatSet(capcity);
        }
        //-----------------------------------------------------
        public static HashSet<long> CacheLongSet(AFramework pFramework, int capcity = 4)
        {
            if (pFramework == null) return new HashSet<long>(capcity);
            return pFramework.ShareCache.CacheLongSet(capcity);
        }
        //-----------------------------------------------------
        public static HashSet<double> CacheDoubleSet(AFramework pFramework, int capcity = 4)
        {
            if (pFramework == null) return new HashSet<double>(capcity);
            return pFramework.ShareCache.CacheDoubleSet(capcity);
        }
        //-----------------------------------------------------
        public static List<IUserData> CacheDataList(AFramework pFramework, int capcity = 4)
        {
            if (pFramework == null) return new List<IUserData>(capcity);
            return pFramework.ShareCache.CacheDataList(capcity);
        }
        //-----------------------------------------------------
        public static HashSet<IUserData> CacheDataHashSet(AFramework pFramework, int capcity = 4)
        {
            if (pFramework == null) return new HashSet<IUserData>(capcity);
            return pFramework.ShareCache.CacheDataHashSet(capcity);
        }
#if USE_FIXEDMATH
        //------------------------------------------------------
        public static List<FVector3> ListVectorToF(this List<Vector3> vList, AFramework pFramework)
        {
            if (vList == null || vList.Count<=0)
                return null;
            List<FVector3> vResult = null;
            if (pFramework != null) vResult = pFramework.ShareCache.CacheFVector3List(vList.Count);
            else vResult = new List<FVector3>(vList.Count);
            for(int i =0; i < vList.Count; ++i)
            {
                vResult.Add(vList[i]);
            }
            return vResult;
        }
        //------------------------------------------------------
        public static List<Vector3> ListFVectorTo(this List<FVector3> vList, AFramework pFramework)
        {
            if (vList == null || vList.Count <= 0)
                return null;
            List<Vector3> vResult = null;
            if (pFramework != null) vResult = pFramework.ShareCache.CacheVector3List(vList.Count);
            else vResult = new List<Vector3>(vList.Count);
            for (int i = 0; i < vList.Count; ++i)
            {
                vResult.Add(vList[i]);
            }
            return vResult;
        }
#else
        //------------------------------------------------------
        public static List<FVector3> ListVectorToF(this List<Vector3> vList, AFramework pFramework)
        {
            return vList;
        }
        //------------------------------------------------------
        public static List<Vector3> ListFVectorTo(this List<FVector3> vList, AFramework pFramework)
        {
            return vList;
        }
#endif
    }
}