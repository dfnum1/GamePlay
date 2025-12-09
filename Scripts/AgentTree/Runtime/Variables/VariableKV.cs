/********************************************************************
生成日期:	06:30:2025
类    名: 	VariableKV
作    者:	HappLI
描    述:	变量Key-Value存储类
*********************************************************************/
using Framework.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.AT.Runtime
{
    internal class VariableKV
    {
        System.Collections.Generic.Dictionary<short, bool>      m_vBools = null;
        System.Collections.Generic.Dictionary<short, int>       m_vInts = null;
        System.Collections.Generic.Dictionary<short, long>      m_vLongs = null;
        System.Collections.Generic.Dictionary<short, float>     m_vFloats = null;
        System.Collections.Generic.Dictionary<short, double>    m_vDoubles = null;
        System.Collections.Generic.Dictionary<short, string>    m_vStrings = null;
        System.Collections.Generic.Dictionary<short, Vector2>   m_vVec2s = null;
        System.Collections.Generic.Dictionary<short, Vector3>   m_vVec3s = null;
        System.Collections.Generic.Dictionary<short, Vector4>   m_vVec4s = null;
        System.Collections.Generic.Dictionary<short, ObjId>     m_vObjIds = null;
        System.Collections.Generic.Dictionary<short, Ray>       m_vRays = null;
        System.Collections.Generic.Dictionary<short, Color>     m_vColors = null;
        System.Collections.Generic.Dictionary<short, Quaternion> m_vQuaternions = null;
        System.Collections.Generic.Dictionary<short, Bounds>    m_vBounds = null;
        System.Collections.Generic.Dictionary<short, Rect>      m_vRects = null;
        System.Collections.Generic.Dictionary<short, Matrix4x4> m_vMatrixs = null;
        System.Collections.Generic.Dictionary<short, VariableUserData>  m_vUserDatas = null;
        //-----------------------------------------------------
        public void Clear()
        {
            m_vBools?.Clear();
            m_vInts?.Clear();
            m_vLongs?.Clear();
            m_vFloats?.Clear();
            m_vDoubles?.Clear();
            m_vStrings?.Clear();
            m_vVec2s?.Clear();
            m_vVec3s?.Clear();
            m_vVec4s?.Clear();
            m_vObjIds?.Clear();
            m_vRays?.Clear();
            m_vColors?.Clear();
            m_vQuaternions?.Clear();
            m_vBounds?.Clear();
            m_vRects?.Clear();
            m_vMatrixs?.Clear();
            m_vUserDatas?.Clear();
        }
        //-----------------------------------------------------
        public void SetBool(short key, bool value)
        {
            if (m_vBools == null) m_vBools = new System.Collections.Generic.Dictionary<short, bool>(2);
            m_vBools[key] = value;
        }
        //-----------------------------------------------------
        public bool GetBool(short key, bool defaultValue = false)
        {
            if (m_vBools != null && m_vBools.TryGetValue(key, out var val))
                return val;
            return defaultValue;
        }
        //-----------------------------------------------------
        public bool GetBool(short key, out bool value)
        {
            if (m_vBools != null && m_vBools.TryGetValue(key, out value))
                return true;
            value = false;
            return false;
        }
        //-----------------------------------------------------
        public void SetInt(short key, int value)
        {
            if (m_vInts == null) m_vInts = new System.Collections.Generic.Dictionary<short, int>(2);
            m_vInts[key] = value;
        }
        //-----------------------------------------------------
        public int GetInt(short key, int defaultValue = 0)
        {
            if (m_vInts != null && m_vInts.TryGetValue(key, out var val))
                return val;
            return defaultValue;
        }
        //-----------------------------------------------------
        public bool GetInt(short key, out int value)
        {
            if (m_vInts != null && m_vInts.TryGetValue(key, out value))
                return true;
            value = 0;
            return false;
        }
        //-----------------------------------------------------
        public void SetLong(short key, long value)
        {
            if (m_vLongs == null) m_vLongs = new Dictionary<short, long>(2);
            m_vLongs[key] = value;
        }
        //-----------------------------------------------------
        public long GetLong(short key, long defaultValue = 0)
        {
            if (m_vLongs != null && m_vLongs.TryGetValue(key, out var val))
                return val;
            return defaultValue;
        }
        //-----------------------------------------------------
        public bool GetLong(short key, out long value)
        {
            if (m_vLongs != null && m_vLongs.TryGetValue(key, out value))
                return true;
            value = 0;
            return false;
        }
        //-----------------------------------------------------
        public void SetFloat(short key, float value)
        {
            if (m_vFloats == null) m_vFloats = new System.Collections.Generic.Dictionary<short, float>(2);
            m_vFloats[key] = value;
        }
        //-----------------------------------------------------
        public float GetFloat(short key, float defaultValue = 0f)
        {
            if (m_vFloats != null && m_vFloats.TryGetValue(key, out var val))
                return val;
            return defaultValue;
        }
        //-----------------------------------------------------
        public bool GetFloat(short key, out float value)
        {
            if (m_vFloats != null && m_vFloats.TryGetValue(key, out value))
                return true;
            value = 0.0f;
            return false;
        }
        //-----------------------------------------------------
        public void SetDouble(short key, double value)
        {
            if (m_vDoubles == null) m_vDoubles = new Dictionary<short, double>(2);
            m_vDoubles[key] = value;
        }
        //-----------------------------------------------------
        public double GetDouble(short key, double defaultValue = 0)
        {
            if (m_vDoubles != null && m_vDoubles.TryGetValue(key, out var val))
                return val;
            return defaultValue;
        }
        //-----------------------------------------------------
        public bool GetDouble(short key, out double value)
        {
            if (m_vDoubles != null && m_vDoubles.TryGetValue(key, out value))
                return true;
            value = 0.0;
            return false;
        }
        //-----------------------------------------------------
        public void SetString(short key, string value)
        {
            if (m_vStrings == null) m_vStrings = new System.Collections.Generic.Dictionary<short, string>(2);
            m_vStrings[key] = value;
        }
        //-----------------------------------------------------
        public string GetString(short key, string defaultValue = null)
        {
            if (m_vStrings != null && m_vStrings.TryGetValue(key, out var val))
                return val;
            return defaultValue;
        }
        //-----------------------------------------------------
        public bool GetString(short key, out string value)
        {
            if (m_vStrings != null && m_vStrings.TryGetValue(key, out value))
                return true;
            value = null;
            return false;
        }
        //-----------------------------------------------------
        public void SetVec2(short key, Vector2 value)
        {
            if (m_vVec2s == null) m_vVec2s = new System.Collections.Generic.Dictionary<short, Vector2>(2);
            m_vVec2s[key] = value;
        }
        //-----------------------------------------------------
        public void SetVec2Int(short key, Vector2Int value)
        {
            if (m_vVec2s == null) m_vVec2s = new System.Collections.Generic.Dictionary<short, Vector2>(2);
            m_vVec2s[key] = value;
        }
        //-----------------------------------------------------
        public Vector2 GetVec2(short key, Vector2 defaultValue = default)
        {
            if (m_vVec2s != null && m_vVec2s.TryGetValue(key, out var val))
                return val;
            return defaultValue;
        }
        //-----------------------------------------------------
        public Vector2Int GetVec2Int(short key, Vector2Int defaultValue = default)
        {
            if (m_vVec2s != null && m_vVec2s.TryGetValue(key, out var val))
                return new Vector2Int((int)val.x, (int)val.y);
            return defaultValue;
        }
        //-----------------------------------------------------
        public bool GetVec2(short key, out Vector2 value)
        {
            if (m_vVec2s != null && m_vVec2s.TryGetValue(key, out value))
                return true;
            value = Vector2.zero;
            return false;
        }
        //-----------------------------------------------------
        public bool GetVec2Int(short key, out Vector2Int value)
        {
            Vector2 temp;
            if (m_vVec3s != null && m_vVec2s.TryGetValue(key, out temp))
            {
                value = new Vector2Int((int)temp.x, (int)temp.y);
                return true;
            }
            value = Vector2Int.zero;
            return false;
        }
        //-----------------------------------------------------
        public void SetVec3(short key, Vector3 value)
        {
            if (m_vVec3s == null) m_vVec3s = new System.Collections.Generic.Dictionary<short, Vector3>(2);
            m_vVec3s[key] = value;
        }
        //-----------------------------------------------------
        public void SetVec3Int(short key, Vector3Int value)
        {
            if (m_vVec3s == null) m_vVec3s = new System.Collections.Generic.Dictionary<short, Vector3>(2);
            m_vVec3s[key] = value;
        }
        //-----------------------------------------------------
        public Vector3 GetVec3(short key)
        {
            if (m_vVec3s != null && m_vVec3s.TryGetValue(key, out var val))
                return val;
            return Vector3.zero;
        }
        //-----------------------------------------------------
        public Vector3Int GetVec3Int(short key)
        {
            if (m_vVec3s != null && m_vVec3s.TryGetValue(key, out var val))
                return new Vector3Int((int)val.x, (int)val.y,(int)val.z);
            return Vector3Int.zero;
        }
        //-----------------------------------------------------
        public bool GetVec3(short key, out Vector3 value)
        {
            if (m_vVec3s != null && m_vVec3s.TryGetValue(key, out value))
                return true;
            value = Vector3.zero;
            return false;
        }
        //-----------------------------------------------------
        public bool GetVec3Int(short key, out Vector3Int value)
        {
            Vector3 temp;
            if (m_vVec3s != null && m_vVec3s.TryGetValue(key, out temp))
            {
                value = new Vector3Int((int)temp.x, (int)temp.y, (int)temp.z);
                return true;
            }
            value = Vector3Int.zero;
            return false;
        }
        //-----------------------------------------------------
        public void SetVec4(short key, Vector4 value)
        {
            if (m_vVec4s == null) m_vVec4s = new System.Collections.Generic.Dictionary<short, Vector4>(2);
            m_vVec4s[key] = value;
        }
        //-----------------------------------------------------
        public Vector4 GetVec4(short key)
        {
            if (m_vVec4s != null && m_vVec4s.TryGetValue(key, out var val))
                return val;
            return Vector4.one;
        }
        //-----------------------------------------------------
        public bool GetVec4(short key, out Vector4 value)
        {
            if (m_vVec4s != null && m_vVec4s.TryGetValue(key, out value))
                return true;
            value = Vector4.zero;
            return false;
        }
        //-----------------------------------------------------
        public void SetObjId(short key, ObjId value)
        {
            if (m_vObjIds == null) m_vObjIds = new System.Collections.Generic.Dictionary<short, ObjId>(2);
            m_vObjIds[key] = value;
        }
        //-----------------------------------------------------
        public ObjId GetObjId(short key, ObjId defaultValue = default)
        {
            if (m_vObjIds != null && m_vObjIds.TryGetValue(key, out var val))
                return val;
            return defaultValue;
        }
        //-----------------------------------------------------
        public bool GetObjId(short key, out ObjId value)
        {
            if (m_vObjIds != null && m_vObjIds.TryGetValue(key, out value))
                return true;
            value =new ObjId() { id =0};
            return false;
        }
        //-----------------------------------------------------
        public void SetRay(short key, Ray value)
        {
            if (m_vRays == null) m_vRays = new Dictionary<short, Ray>(2);
            m_vRays[key] = value;
        }
        //-----------------------------------------------------
        public Ray GetRay(short key, Ray defaultValue = default)
        {
            if (m_vRays != null && m_vRays.TryGetValue(key, out var val))
                return val;
            return defaultValue;
        }
        //-----------------------------------------------------
        public bool GetRay(short key, out Ray value)
        {
            if (m_vRays != null && m_vRays.TryGetValue(key, out value))
                return true;
            value = default;
            return false;
        }
        //-----------------------------------------------------
        public void SetColor(short key, Color value)
        {
            if (m_vColors == null) m_vColors = new Dictionary<short, Color>(2);
            m_vColors[key] = value;
        }
        //-----------------------------------------------------
        public Color GetColor(short key, Color defaultValue = default)
        {
            if (m_vColors != null && m_vColors.TryGetValue(key, out var val))
                return val;
            return defaultValue;
        }
        //-----------------------------------------------------
        public bool GetColor(short key, out Color value)
        {
            if (m_vColors != null && m_vColors.TryGetValue(key, out value))
                return true;
            value = Color.white;
            return false;
        }
        //-----------------------------------------------------
        public void SetQuaternion(short key, Quaternion value)
        {
            if (m_vQuaternions == null) m_vQuaternions = new Dictionary<short, Quaternion>(2);
            m_vQuaternions[key] = value;
        }
        //-----------------------------------------------------
        public Quaternion GetQuaternion(short key, Quaternion defaultValue = default)
        {
            if (m_vQuaternions != null && m_vQuaternions.TryGetValue(key, out var val))
                return val;
            return defaultValue;
        }
        //-----------------------------------------------------
        public bool GetQuaternion(short key, out Quaternion value)
        {
            if (m_vQuaternions != null && m_vQuaternions.TryGetValue(key, out value))
                return true;
            value = Quaternion.identity;
            return false;
        }
        //-----------------------------------------------------
        public void SetBounds(short key, Bounds value)
        {
            if (m_vBounds == null) m_vBounds = new Dictionary<short, Bounds>(2);
            m_vBounds[key] = value;
        }
        //-----------------------------------------------------
        public Bounds GetBounds(short key, Bounds defaultValue = default)
        {
            if (m_vBounds != null && m_vBounds.TryGetValue(key, out var val))
                return val;
            return defaultValue;
        }
        //-----------------------------------------------------
        public bool GetBounds(short key, out Bounds value)
        {
            if (m_vBounds != null && m_vBounds.TryGetValue(key, out value))
                return true;
            value = default;
            return false;
        }
        //-----------------------------------------------------
        public void SetRect(short key, Rect value)
        {
            if (m_vRects == null) m_vRects = new Dictionary<short, Rect>(2);
            m_vRects[key] = value;
        }
        //-----------------------------------------------------
        public Rect GetRect(short key, Rect defaultValue = default)
        {
            if (m_vRects != null && m_vRects.TryGetValue(key, out var val))
                return val;
            return defaultValue;
        }
        //-----------------------------------------------------
        public bool GetRect(short key, out Rect value)
        {
            if (m_vRects != null && m_vRects.TryGetValue(key, out value))
                return true;
            value = default;
            return false;
        }
        //-----------------------------------------------------
        public void SetMatrix(short key, Matrix4x4 value)
        {
            if (m_vMatrixs == null) m_vMatrixs = new Dictionary<short, Matrix4x4>(2);
            m_vMatrixs[key] = value;
        }
        //-----------------------------------------------------
        public Matrix4x4 GetMatrix(short key, Matrix4x4 defaultValue = default)
        {
            if (m_vMatrixs != null && m_vMatrixs.TryGetValue(key, out var val))
                return val;
            return defaultValue;
        }
        //-----------------------------------------------------
        public bool GetMatrix(short key, out Matrix4x4 value)
        {
            if (m_vMatrixs != null && m_vMatrixs.TryGetValue(key, out value))
                return true;
            value = Matrix4x4.identity;
            return false;
        }
        //-----------------------------------------------------
        public void SetUserData(short key, VariableUserData value)
        {
            if (m_vUserDatas == null) m_vUserDatas = new Dictionary<short, VariableUserData>(2);
            m_vUserDatas[key] = value;
        }
        //-----------------------------------------------------
        public VariableUserData GetUserData(short key)
        {
            if (m_vUserDatas != null && m_vUserDatas.TryGetValue(key, out var val))
                return val;
            return VariableUserData.DEF;
        }
        //-----------------------------------------------------
        public bool GetUserData(short key, out VariableUserData value)
        {
            value = VariableUserData.DEF;
            if (m_vUserDatas != null && m_vUserDatas.TryGetValue(key, out value))
                return true;
            return false;
        }
        //-----------------------------------------------------
        public void SetVariable(IVariable variable)
		{
            if (variable is VariableInt vInt)
                SetInt(vInt.GetGuid(), vInt.value);
            else if (variable is VariableLong vLong)
                SetLong(vLong.GetGuid(), vLong.value);
            else if (variable is VariableBool vBool)
                SetBool(vBool.GetGuid(), vBool.value);
            else if (variable is VariableFloat vFloat)
                SetFloat(vFloat.GetGuid(), vFloat.value);
            else if (variable is VariableDouble vDouble)
                SetDouble(vDouble.GetGuid(), vDouble.value);
            else if (variable is VariableString vString)
                SetString(vString.GetGuid(), vString.value);
            else if (variable is VariableVec2 vVec2)
                SetVec2(vVec2.GetGuid(), vVec2.value);
            else if (variable is VariableVec3 vVec3)
                SetVec3(vVec3.GetGuid(), vVec3.value);
            else if (variable is VariableVec4 vVec4)
                SetVec4(vVec4.GetGuid(), vVec4.value);
            else if (variable is VariableObjId vObjId)
                SetObjId(vObjId.GetGuid(), vObjId.value);
            else if (variable is VariableRay vRay)
                SetRay(vRay.GetGuid(), vRay.value);
            else if (variable is VariableColor vColor)
                SetColor(vColor.GetGuid(), vColor.value);
            else if (variable is VariableQuaternion vQuat)
                SetQuaternion(vQuat.GetGuid(), vQuat.value);
            else if (variable is VariableBounds vBounds)
                SetBounds(vBounds.GetGuid(), vBounds.value);
            else if (variable is VariableRect vRect)
                SetRect(vRect.GetGuid(), vRect.value);
            else if (variable is VariableMatrix vMatrix)
                SetMatrix(vMatrix.GetGuid(), vMatrix.value);
            else if (variable is VariableUserData vUserData)
                SetUserData(vUserData.GetGuid(), vUserData);
        }
    }
}