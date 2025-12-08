/********************************************************************
生成日期:	06:30:2025
类    名: 	VariableList
作    者:	HappLI
描    述:	变量列表存储类
*********************************************************************/
using Framework.Core;
using System.Collections.Generic;
using UnityEngine;


namespace Framework.AT.Runtime
{
    public class VariableList
    {
        struct TypeIndex
        {
            public EVariableType type;
            public byte index;
            public TypeIndex(EVariableType type, byte index)
            {
                this.type = type;
                this.index = index;
            }
        }
        List<bool>          m_vBools = null;
        List<int>           m_vInts = null;
        List<long>          m_vLongs = null;
        List<float>         m_vFloats = null;
        List<double>        m_vDoubles = null;
        List<string>        m_vStrings = null;
        List<Vector2>       m_vVec2s = null;
        List<Vector3>       m_vVec3s = null;
        List<Vector4>       m_vVec4s = null;
        List<Ray>           m_vRays = null;
        List<Ray2D>         m_vRay2Ds = null;
        List<Quaternion>    m_vQuaternions = null;
        List<Bounds>        m_vBounds = null;
        List<Rect>          m_vRects = null;
        List<Matrix4x4>     m_vMatrixs = null;
        List<ObjId>         m_vObjIds = null;
        List<IUserData>     m_vUserDatas = null;
        List<TypeIndex>     m_vTypes = null;
        byte                m_nCapacity = 2;
        //-----------------------------------------------------
        internal VariableList()
        {

        }
        //-----------------------------------------------------
        public static VariableList Malloc(int capacity =2)
        {
            VariableList list= VariablePool.GetVariableList();
            list.m_nCapacity = (byte)Mathf.Clamp(capacity, 1, 255);
            return list;
        }
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
            m_vTypes?.Clear();
            m_vRays?.Clear();
            m_vRay2Ds?.Clear();
            m_vQuaternions?.Clear();
            m_vBounds?.Clear();
            m_vRects?.Clear();
            m_vMatrixs?.Clear();
            m_vUserDatas?.Clear();
        }
        //-----------------------------------------------------
        public int GetVarCount()
        {
            if (m_vTypes == null) return 0;
            return m_vTypes.Count;
        }
        //-----------------------------------------------------
        public EVariableType GetVarType(int index)
        {
            if (index < 0 || m_vTypes == null || m_vTypes.Count == 0 || index >= m_vTypes.Count) return EVariableType.eNone;
            return m_vTypes[index].type;
        }
        //-----------------------------------------------------
        public void AddBool(bool value)
        {
            if (m_vBools == null) m_vBools = new List<bool>(m_nCapacity);
            if (m_vTypes == null) m_vTypes = new List<TypeIndex>(m_nCapacity);
            m_vTypes.Add(new TypeIndex(EVariableType.eBool, (byte)m_vBools.Count));
            m_vBools.Add(value);
        }
        //-----------------------------------------------------
        public void SetBool(int index, bool value)
        {
            if (index >= 0 && m_vBools != null && m_vBools.Count > 0 && m_vTypes != null && m_vTypes.Count > 0 && index < m_vTypes.Count)
            {
                var type = m_vTypes[index];
                if (type.type != EVariableType.eBool)
                {
                    Debug.LogError($"VariableList: SetBool type mismatch, expected {EVariableType.eBool}, got {type}");
                }
                if (type.index < 0 || type.index >= m_vBools.Count)
                {
                    Debug.LogError($"VariableList: SetBool index out of range, index={type.index}, count={m_vBools.Count}");
                    return;
                }
                m_vBools[type.index] = value;
            }
        }
        //-----------------------------------------------------
        public bool GetBool(int index, bool bDefault = false)
        {
            if (index >= 0 && m_vBools != null && m_vBools.Count > 0 && m_vTypes != null && m_vTypes.Count > 0 && index < m_vTypes.Count)
            {
                var type = m_vTypes[index];
                if (type.type != EVariableType.eBool)
                {
                    Debug.LogError($"VariableList: GetBool type mismatch, expected {EVariableType.eBool}, got {type}");
                }
                if (type.index < 0 || type.index >= m_vBools.Count)
                {
                    Debug.LogError($"VariableList: GetBool index out of range, index={type.index}, count={m_vBools.Count}");
                    return bDefault;
                }
                return m_vBools[type.index];
            }
            return bDefault;
        }
        //-----------------------------------------------------
        public List<bool> GetBools()
        {
            return m_vBools;
        }
        //-----------------------------------------------------
        public void AddInt(int value)
        {
            if (m_vInts == null) m_vInts = new List<int>(m_nCapacity);
            if (m_vTypes == null) m_vTypes = new List<TypeIndex>(m_nCapacity);
            m_vTypes.Add(new TypeIndex(EVariableType.eInt, (byte)m_vInts.Count));
            m_vInts.Add(value);
        }
        //-----------------------------------------------------
        public void SetInt(int index, int value)
        {
            if (index >= 0 && m_vInts != null && m_vInts.Count > 0 && m_vTypes != null && m_vTypes.Count > 0 && index < m_vTypes.Count)
            {
                var type = m_vTypes[index];
                if (type.type != EVariableType.eInt)
                {
                    Debug.LogError($"VariableList: SetInt type mismatch, expected {EVariableType.eInt}, got {type}");
                }
                if (type.index < 0 || type.index >= m_vInts.Count)
                {
                    Debug.LogError($"VariableList: SetInt index out of range, index={type.index}, count={m_vInts.Count}");
                    return;
                }
                m_vInts[type.index] = value;
            }
        }
        //-----------------------------------------------------
        public int GetInt(int index, int defaultValue = 0)
        {
            if (index >= 0 && m_vInts != null && m_vInts.Count > 0 && m_vTypes != null && m_vTypes.Count > 0 && index < m_vTypes.Count)
            {
                var type = m_vTypes[index];
                if (type.type != EVariableType.eInt)
                {
                    Debug.LogError($"VariableList: GetInt type mismatch, expected {EVariableType.eInt}, got {type}");
                }
                if (type.index < 0 || type.index >= m_vInts.Count)
                {
                    Debug.LogError($"VariableList: GetInt index out of range, index={type.index}, count={m_vInts.Count}");
                    return defaultValue;
                }
                return m_vInts[type.index];
            }
            return defaultValue;
        }
        //-----------------------------------------------------
        public List<int> GetInts()
        {
            return m_vInts;
        }
        //-----------------------------------------------------
        public void AddLong(long value)
        {
            if (m_vLongs == null) m_vLongs = new List<long>(m_nCapacity);
            if (m_vTypes == null) m_vTypes = new List<TypeIndex>(m_nCapacity);
            m_vTypes.Add(new TypeIndex(EVariableType.eLong, (byte)m_vLongs.Count));
            m_vLongs.Add(value);
        }
        //-----------------------------------------------------
        public void SetLong(int index, long value)
        {
            if (index >= 0 && m_vLongs != null && m_vLongs.Count > 0 && m_vTypes != null && m_vTypes.Count > 0 && index < m_vTypes.Count)
            {
                var type = m_vTypes[index];
                if (type.type != EVariableType.eLong)
                {
                    Debug.LogError($"VariableList: SetLong type mismatch, expected {EVariableType.eLong}, got {type}");
                }
                if (type.index < 0 || type.index >= m_vLongs.Count)
                {
                    Debug.LogError($"VariableList: SetLong index out of range, index={type.index}, count={m_vLongs.Count}");
                    return;
                }
                m_vLongs[type.index] = value;
            }
        }
        //-----------------------------------------------------
        public long GetLong(int index, long defaultValue = 0)
        {
            if (index >= 0 && m_vLongs != null && m_vLongs.Count > 0 && m_vTypes != null && m_vTypes.Count > 0 && index < m_vTypes.Count)
            {
                var type = m_vTypes[index];
                if (type.type != EVariableType.eLong)
                {
                    Debug.LogError($"VariableList: GetLong type mismatch, expected {EVariableType.eLong}, got {type}");
                }
                if (type.index < 0 || type.index >= m_vLongs.Count)
                {
                    Debug.LogError($"VariableList: GetLong index out of range, index={type.index}, count={m_vLongs.Count}");
                    return defaultValue;
                }
                return m_vLongs[type.index];
            }
            return defaultValue;
        }
        //-----------------------------------------------------
        public List<long> GetLongs()
        {
            return m_vLongs;
        }
        //-----------------------------------------------------
        public void AddFloat(float value)
        {
            if (m_vFloats == null) m_vFloats = new List<float>(m_nCapacity);
            if (m_vTypes == null) m_vTypes = new List<TypeIndex>(m_nCapacity);
            m_vTypes.Add(new TypeIndex(EVariableType.eFloat, (byte)m_vFloats.Count));
            m_vFloats.Add(value);
        }
        //-----------------------------------------------------
        public void SetFloat(int index, float value)
        {
            if (index >= 0 && m_vFloats != null && m_vFloats.Count > 0 && m_vTypes != null && m_vTypes.Count > 0 && index < m_vTypes.Count)
            {
                var type = m_vTypes[index];
                if (type.type != EVariableType.eFloat)
                {
                    Debug.LogError($"VariableList: SetFloat type mismatch, expected {EVariableType.eFloat}, got {type}");
                }
                if (type.index < 0 || type.index >= m_vFloats.Count)
                {
                    Debug.LogError($"VariableList: SetFloat index out of range, index={type.index}, count={m_vFloats.Count}");
                    return;
                }
                m_vFloats[type.index] = value;
            }
        }
        //-----------------------------------------------------
        public float GetFloat(int index, float defaultValue = 0f)
        {
            if (index >= 0 && m_vFloats != null && m_vFloats.Count > 0 && m_vTypes != null && m_vTypes.Count > 0 && index < m_vTypes.Count)
            {
                var type = m_vTypes[index];
                if (type.type != EVariableType.eFloat)
                {
                    Debug.LogError($"VariableList: GetFloat type mismatch, expected {EVariableType.eFloat}, got {type}");
                }
                if (type.index < 0 || type.index >= m_vFloats.Count)
                {
                    Debug.LogError($"VariableList: GetFloat index out of range, index={type.index}, count={m_vFloats.Count}");
                    return defaultValue;
                }
                return m_vFloats[type.index];
            }
            return defaultValue;
        }
        //-----------------------------------------------------
        public List<float> GetFloats()
        {
            return m_vFloats;
        }
        //-----------------------------------------------------
        public void AddDouble(double value)
        {
            if (m_vDoubles == null) m_vDoubles = new List<double>(m_nCapacity);
            if (m_vTypes == null) m_vTypes = new List<TypeIndex>(m_nCapacity);
            m_vTypes.Add(new TypeIndex(EVariableType.eDouble, (byte)m_vDoubles.Count));
            m_vDoubles.Add(value);
        }
        //-----------------------------------------------------
        public void SetDouble(int index, double value)
        {
            if (index >= 0 && m_vDoubles != null && m_vDoubles.Count > 0 && m_vTypes != null && m_vTypes.Count > 0 && index < m_vTypes.Count)
            {
                var type = m_vTypes[index];
                if (type.type != EVariableType.eDouble)
                {
                    Debug.LogError($"VariableList: SetDouble type mismatch, expected {EVariableType.eDouble}, got {type}");
                }
                if (type.index < 0 || type.index >= m_vDoubles.Count)
                {
                    Debug.LogError($"VariableList: SetDouble index out of range, index={type.index}, count={m_vDoubles.Count}");
                    return;
                }
                m_vDoubles[type.index] = value;
            }
        }
        //-----------------------------------------------------
        public double GetDouble(int index, double defaultValue = 0)
        {
            if (index >= 0 && m_vDoubles != null && m_vDoubles.Count > 0 && m_vTypes != null && m_vTypes.Count > 0 && index < m_vTypes.Count)
            {
                var type = m_vTypes[index];
                if (type.type != EVariableType.eDouble)
                {
                    Debug.LogError($"VariableList: GetDouble type mismatch, expected {EVariableType.eDouble}, got {type}");
                }
                if (type.index < 0 || type.index >= m_vDoubles.Count)
                {
                    Debug.LogError($"VariableList: GetDouble index out of range, index={type.index}, count={m_vDoubles.Count}");
                    return defaultValue;
                }
                return m_vDoubles[type.index];
            }
            return defaultValue;
        }
        //-----------------------------------------------------
        public List<double> GetDoubles()
        {
            return m_vDoubles;
        }
        //-----------------------------------------------------
        public void AddString(string value)
        {
            if (m_vStrings == null) m_vStrings = new List<string>(m_nCapacity);
            if (m_vTypes == null) m_vTypes = new List<TypeIndex>(m_nCapacity);
            m_vTypes.Add(new TypeIndex(EVariableType.eString, (byte)m_vStrings.Count));
            m_vStrings.Add(value);
        }
        //-----------------------------------------------------
        public void SetString(int index, string value)
        {
            if (index >= 0 && m_vStrings != null && m_vStrings.Count > 0 && m_vTypes != null && m_vTypes.Count > 0 && index < m_vTypes.Count)
            {
                var type = m_vTypes[index];
                if (type.type != EVariableType.eString)
                {
                    Debug.LogError($"VariableList: SetString type mismatch, expected {EVariableType.eString}, got {type}");
                }
                if (type.index < 0 || type.index >= m_vStrings.Count)
                {
                    Debug.LogError($"VariableList: SetString index out of range, index={type.index}, count={m_vStrings.Count}");
                    return;
                }
                m_vStrings[type.index] = value;
            }
        }
        //-----------------------------------------------------
        public string GetString(int index, string defaultValue = null)
        {
            if (index >= 0 && m_vStrings != null && m_vStrings.Count > 0 && m_vTypes != null && m_vTypes.Count > 0 && index < m_vTypes.Count)
            {
                var type = m_vTypes[index];
                if (type.type != EVariableType.eString)
                {
                    Debug.LogError($"VariableList: GetString type mismatch, expected {EVariableType.eString}, got {type}");
                }
                if (type.index < 0 || type.index >= m_vStrings.Count)
                {
                    Debug.LogError($"VariableList: GetString index out of range, index={type.index}, count={m_vStrings.Count}");
                    return defaultValue;
                }
                return m_vStrings[type.index];
            }
            return defaultValue;
        }
        //-----------------------------------------------------
        public List<string> GetStrings()
        {
            return m_vStrings;
        }
        //-----------------------------------------------------
        public void AddVec2(Vector2 value)
        {
            if (m_vVec2s == null) m_vVec2s = new List<Vector2>(m_nCapacity);
            if (m_vTypes == null) m_vTypes = new List<TypeIndex>(m_nCapacity);
            m_vTypes.Add(new TypeIndex(EVariableType.eVec2, (byte)m_vVec2s.Count));
            m_vVec2s.Add(value);
        }
        //-----------------------------------------------------
        public void SetVec2(int index, Vector2 value)
        {
            if (index >= 0 && m_vVec2s != null && m_vVec2s.Count > 0 && m_vTypes != null && m_vTypes.Count > 0 && index < m_vTypes.Count)
            {
                var type = m_vTypes[index];
                if (type.type != EVariableType.eVec2)
                {
                    Debug.LogError($"VariableList: SetVec2 type mismatch, expected {EVariableType.eVec2}, got {type}");
                }
                if (type.index < 0 || type.index >= m_vVec2s.Count)
                {
                    Debug.LogError($"VariableList: SetVec2 index out of range, index={type.index}, count={m_vVec2s.Count}");
                    return;
                }
                m_vVec2s[type.index] = value;
            }
        }
        //-----------------------------------------------------
        public Vector2 GetVec2(int index)
        {
            if (index >= 0 && m_vVec2s != null && m_vVec2s.Count > 0 && m_vTypes != null && m_vTypes.Count > 0 && index < m_vTypes.Count)
            {
                var type = m_vTypes[index];
                if (type.type != EVariableType.eVec2)
                {
                    Debug.LogError($"VariableList: GetVec2 type mismatch, expected {EVariableType.eVec2}, got {type}");
                }
                if (type.index < 0 || type.index >= m_vVec2s.Count)
                {
                    Debug.LogError($"VariableList: GetVec2 index out of range, index={type.index}, count={m_vVec2s.Count}");
                    return Vector2.zero;
                }
                return m_vVec2s[type.index];
            }
            return Vector2.zero;
        }
        //-----------------------------------------------------
        public List<Vector2> GetVec2s()
        {
            return m_vVec2s;
        }
        //-----------------------------------------------------
        public void AddVec3(Vector3 value)
        {
            if (m_vVec3s == null) m_vVec3s = new List<Vector3>(m_nCapacity);
            if (m_vTypes == null) m_vTypes = new List<TypeIndex>(m_nCapacity);
            m_vTypes.Add(new TypeIndex(EVariableType.eVec3, (byte)m_vVec3s.Count));
            m_vVec3s.Add(value);
        }
        //-----------------------------------------------------
        public void SetVec3(int index, Vector3 value)
        {
            if (index >= 0 && m_vVec3s != null && m_vVec3s.Count > 0 && m_vTypes != null && m_vTypes.Count > 0 && index < m_vTypes.Count)
            {
                var type = m_vTypes[index];
                if (type.type != EVariableType.eVec3)
                {
                    Debug.LogError($"VariableList: SetVec3 type mismatch, expected {EVariableType.eVec3}, got {type}");
                }
                if (type.index < 0 || type.index >= m_vVec3s.Count)
                {
                    Debug.LogError($"VariableList: SetVec3 index out of range, index={type.index}, count={m_vVec3s.Count}");
                    return;
                }
                m_vVec3s[type.index] = value;
            }
        }
        //-----------------------------------------------------
        public Vector3 GetVec3(int index)
        {
            if (index >= 0 && m_vVec3s != null && m_vVec3s.Count > 0 && m_vTypes != null && m_vTypes.Count > 0 && index < m_vTypes.Count)
            {
                var type = m_vTypes[index];
                if (type.type != EVariableType.eVec3)
                {
                    Debug.LogError($"VariableList: GetVec3 type mismatch, expected {EVariableType.eVec3}, got {type}");
                }
                if (type.index < 0 || type.index >= m_vVec3s.Count)
                {
                    Debug.LogError($"VariableList: GetVec3 index out of range, index={type.index}, count={m_vVec3s.Count}");
                    return Vector3.zero;
                }
                return m_vVec3s[type.index];
            }
            return Vector3.zero;
        }
        //-----------------------------------------------------
        public List<Vector3> GetVec3s()
        {
            return m_vVec3s;
        }
        //-----------------------------------------------------
        public void AddVec4(Vector4 value)
        {
            if (m_vVec4s == null) m_vVec4s = new List<Vector4>(m_nCapacity);
            if (m_vTypes == null) m_vTypes = new List<TypeIndex>(m_nCapacity);
            m_vTypes.Add(new TypeIndex(EVariableType.eVec4, (byte)m_vVec4s.Count));
            m_vVec4s.Add(value);
        }
        //-----------------------------------------------------
        public void SetVec4(int index, Vector4 value)
        {
            if (index >= 0 && m_vVec4s != null && m_vVec4s.Count > 0 && m_vTypes != null && m_vTypes.Count > 0 && index < m_vTypes.Count)
            {
                var type = m_vTypes[index];
                if (type.type != EVariableType.eVec4)
                {
                    Debug.LogError($"VariableList: SetVec4 type mismatch, expected {EVariableType.eVec4}, got {type}");
                }
                if (type.index < 0 || type.index >= m_vVec4s.Count)
                {
                    Debug.LogError($"VariableList: SetVec4 index out of range, index={type.index}, count={m_vVec4s.Count}");
                    return;
                }
                m_vVec4s[type.index] = value;
            }
        }
        //-----------------------------------------------------
        public Vector4 GetVec4(int index)
        {
            if (index >= 0 && m_vVec4s != null && m_vVec4s.Count > 0 && m_vTypes != null && m_vTypes.Count > 0 && index < m_vTypes.Count)
            {
                var type = m_vTypes[index];
                if (type.type != EVariableType.eVec4)
                {
                    Debug.LogError($"VariableList: GetVec4 type mismatch, expected {EVariableType.eVec4}, got {type}");
                }
                if (type.index < 0 || type.index >= m_vVec4s.Count)
                {
                    Debug.LogError($"VariableList: GetVec4 index out of range, index={type.index}, count={m_vVec4s.Count}");
                    return Vector4.zero;
                }
                return m_vVec4s[type.index];
            }
            return Vector4.zero;
        }
        //-----------------------------------------------------
        public List<Vector4> GetVec4s()
        {
            return m_vVec4s;
        }
        //-----------------------------------------------------
        public void AddObjId(ObjId value)
        {
            if (m_vObjIds == null) m_vObjIds = new List<ObjId>(m_nCapacity);
            if (m_vTypes == null) m_vTypes = new List<TypeIndex>(m_nCapacity);
            m_vTypes.Add(new TypeIndex(EVariableType.eObjId, (byte)m_vObjIds.Count));
            m_vObjIds.Add(value);
        }
        //-----------------------------------------------------
        public void SetObjId(int index, ObjId value)
        {
            if (index >= 0 && m_vObjIds != null && m_vObjIds.Count > 0 && m_vTypes != null && m_vTypes.Count > 0 && index < m_vTypes.Count)
            {
                var type = m_vTypes[index];
                if (type.type != EVariableType.eObjId)
                {
                    Debug.LogError($"VariableList: SetObjId type mismatch, expected {EVariableType.eObjId}, got {type}");
                }
                if (type.index < 0 || type.index >= m_vObjIds.Count)
                {
                    Debug.LogError($"VariableList: SetObjId index out of range, index={type.index}, count={m_vObjIds.Count}");
                    return;
                }
                m_vObjIds[type.index] = value;
            }
        }
        //-----------------------------------------------------
        public ObjId GetObjId(int index, ObjId defaultValue = default)
        {
            if (index >= 0 && m_vObjIds != null && m_vObjIds.Count > 0 && m_vTypes != null && m_vTypes.Count > 0 && index < m_vTypes.Count)
            {
                var type = m_vTypes[index];
                if (type.type != EVariableType.eObjId)
                {
                    Debug.LogError($"VariableList: GetObjId type mismatch, expected {EVariableType.eObjId}, got {type}");
                }
                if (type.index < 0 || type.index >= m_vObjIds.Count)
                {
                    Debug.LogError($"VariableList: GetObjId index out of range, index={type.index}, count={m_vObjIds.Count}");
                    return defaultValue;
                }
                return m_vObjIds[type.index];
            }
            return defaultValue;
        }
        //-----------------------------------------------------
        public List<ObjId> GetObjIds()
        {
            return m_vObjIds;
        }
        //-----------------------------------------------------
        public void AddRay(Ray value)
        {
            if (m_vRays == null) m_vRays = new List<Ray>(m_nCapacity);
            if (m_vTypes == null) m_vTypes = new List<TypeIndex>(m_nCapacity);
            m_vTypes.Add(new TypeIndex(EVariableType.eRay, (byte)m_vRays.Count));
            m_vRays.Add(value);
        }
        //-----------------------------------------------------
        public void SetRay(int index, Ray value)
        {
            if (index >= 0 && m_vRays != null && m_vRays.Count > 0 && m_vTypes != null && m_vTypes.Count > 0 && index < m_vTypes.Count)
            {
                var type = m_vTypes[index];
                if (type.type != EVariableType.eRay)
                {
                    Debug.LogError($"VariableList: SetRay type mismatch, expected {EVariableType.eRay}, got {type}");
                }
                if (type.index < 0 || type.index >= m_vRays.Count)
                {
                    Debug.LogError($"VariableList: SetRay index out of range, index={type.index}, count={m_vRays.Count}");
                    return;
                }
                m_vRays[type.index] = value;
            }
        }
        //-----------------------------------------------------
        public Ray GetRay(int index)
        {
            if (index >= 0 && m_vRays != null && m_vRays.Count > 0 && m_vTypes != null && m_vTypes.Count > 0 && index < m_vTypes.Count)
            {
                var type = m_vTypes[index];
                if (type.type != EVariableType.eRay)
                {
                    Debug.LogError($"VariableList: GetRay type mismatch, expected {EVariableType.eRay}, got {type}");
                }
                if (type.index < 0 || type.index >= m_vRays.Count)
                {
                    Debug.LogError($"VariableList: GetRay index out of range, index={type.index}, count={m_vRays.Count}");
                    return default;
                }
                return m_vRays[type.index];
            }
            return default;
        }
        //-----------------------------------------------------
        public List<Ray> GetRays()
        {
            return m_vRays;
        }
        //-----------------------------------------------------
        public void AddRay2D(Ray2D value)
        {
            if (m_vRay2Ds == null) m_vRay2Ds = new List<Ray2D>(m_nCapacity);
            if (m_vTypes == null) m_vTypes = new List<TypeIndex>(m_nCapacity);
            m_vTypes.Add(new TypeIndex(EVariableType.eRay2D, (byte)m_vRay2Ds.Count));
            m_vRay2Ds.Add(value);
        }
        //-----------------------------------------------------
        public void SetRay2D(int index, Ray2D value)
        {
            if (index >= 0 && m_vRay2Ds != null && m_vRay2Ds.Count > 0 && m_vTypes != null && m_vTypes.Count > 0 && index < m_vTypes.Count)
            {
                var type = m_vTypes[index];
                if (type.type != EVariableType.eRay2D)
                {
                    Debug.LogError($"VariableList: SetRay2D type mismatch, expected {EVariableType.eRay2D}, got {type}");
                }
                if (type.index < 0 || type.index >= m_vRay2Ds.Count)
                {
                    Debug.LogError($"VariableList: SetRay2D index out of range, index={type.index}, count={m_vRay2Ds.Count}");
                    return;
                }
                m_vRay2Ds[type.index] = value;
            }
        }
        //-----------------------------------------------------
        public Ray2D GetRay2D(int index)
        {
            if (index >= 0 && m_vRay2Ds != null && m_vRay2Ds.Count > 0 && m_vTypes != null && m_vTypes.Count > 0 && index < m_vTypes.Count)
            {
                var type = m_vTypes[index];
                if (type.type != EVariableType.eRay2D)
                {
                    Debug.LogError($"VariableList: GetRay2D type mismatch, expected {EVariableType.eRay2D}, got {type}");
                }
                if (type.index < 0 || type.index >= m_vRay2Ds.Count)
                {
                    Debug.LogError($"VariableList: GetRay2D index out of range, index={type.index}, count={m_vRay2Ds.Count}");
                    return default;
                }
                return m_vRay2Ds[type.index];
            }
            return default;
        }
        //-----------------------------------------------------
        public List<Ray2D> GetRay2Ds()
        {
            return m_vRay2Ds;
        }
        //-----------------------------------------------------
        public void AddQuaternion(Quaternion value)
        {
            if (m_vQuaternions == null) m_vQuaternions = new List<Quaternion>(m_nCapacity);
            if (m_vTypes == null) m_vTypes = new List<TypeIndex>(m_nCapacity);
            m_vTypes.Add(new TypeIndex(EVariableType.eQuaternion, (byte)m_vQuaternions.Count));
            m_vQuaternions.Add(value);
        }
        //-----------------------------------------------------
        public void SetQuaternion(int index, Quaternion value)
        {
            if (index >= 0 && m_vQuaternions != null && m_vQuaternions.Count > 0 && m_vTypes != null && m_vTypes.Count > 0 && index < m_vTypes.Count)
            {
                var type = m_vTypes[index];
                if (type.type != EVariableType.eQuaternion)
                {
                    Debug.LogError($"VariableList: SetQuaternion type mismatch, expected {EVariableType.eQuaternion}, got {type}");
                }
                if (type.index < 0 || type.index >= m_vQuaternions.Count)
                {
                    Debug.LogError($"VariableList: SetQuaternion index out of range, index={type.index}, count={m_vQuaternions.Count}");
                    return;
                }
                m_vQuaternions[type.index] = value;
            }
        }
        //-----------------------------------------------------
        public Quaternion GetQuaternion(int index)
        {
            if (index >= 0 && m_vQuaternions != null && m_vQuaternions.Count > 0 && m_vTypes != null && m_vTypes.Count > 0 && index < m_vTypes.Count)
            {
                var type = m_vTypes[index];
                if (type.type != EVariableType.eQuaternion)
                {
                    Debug.LogError($"VariableList: GetQuaternion type mismatch, expected {EVariableType.eQuaternion}, got {type}");
                }
                if (type.index < 0 || type.index >= m_vQuaternions.Count)
                {
                    Debug.LogError($"VariableList: GetQuaternion index out of range, index={type.index}, count={m_vQuaternions.Count}");
                    return Quaternion.identity;
                }
                return m_vQuaternions[type.index];
            }
            return Quaternion.identity;
        }
        //-----------------------------------------------------
        public List<Quaternion> GetQuaternions()
        {
            return m_vQuaternions;
        }
        //-----------------------------------------------------
        public void AddBounds(Bounds value)
        {
            if (m_vBounds == null) m_vBounds = new List<Bounds>(m_nCapacity);
            if (m_vTypes == null) m_vTypes = new List<TypeIndex>(m_nCapacity);
            m_vTypes.Add(new TypeIndex(EVariableType.eBounds, (byte)m_vBounds.Count));
            m_vBounds.Add(value);
        }
        //-----------------------------------------------------
        public void SetBounds(int index, Bounds value)
        {
            if (index >= 0 && m_vBounds != null && m_vBounds.Count > 0 && m_vTypes != null && m_vTypes.Count > 0 && index < m_vTypes.Count)
            {
                var type = m_vTypes[index];
                if (type.type != EVariableType.eBounds)
                {
                    Debug.LogError($"VariableList: SetBounds type mismatch, expected {EVariableType.eBounds}, got {type}");
                }
                if (type.index < 0 || type.index >= m_vBounds.Count)
                {
                    Debug.LogError($"VariableList: SetBounds index out of range, index={type.index}, count={m_vBounds.Count}");
                    return;
                }
                m_vBounds[type.index] = value;
            }
        }
        //-----------------------------------------------------
        public Bounds GetBounds(int index)
        {
            if (index >= 0 && m_vBounds != null && m_vBounds.Count > 0 && m_vTypes != null && m_vTypes.Count > 0 && index < m_vTypes.Count)
            {
                var type = m_vTypes[index];
                if (type.type != EVariableType.eBounds)
                {
                    Debug.LogError($"VariableList: GetBounds type mismatch, expected {EVariableType.eBounds}, got {type}");
                }
                if (type.index < 0 || type.index >= m_vBounds.Count)
                {
                    Debug.LogError($"VariableList: GetBounds index out of range, index={type.index}, count={m_vBounds.Count}");
                    return default;
                }
                return m_vBounds[type.index];
            }
            return default;
        }
        //-----------------------------------------------------
        public List<Bounds> GetBoundsList()
        {
            return m_vBounds;
        }
        //-----------------------------------------------------
        public void AddRect(Rect value)
        {
            if (m_vRects == null) m_vRects = new List<Rect>(m_nCapacity);
            if (m_vTypes == null) m_vTypes = new List<TypeIndex>(m_nCapacity);
            m_vTypes.Add(new TypeIndex(EVariableType.eRect, (byte)m_vRects.Count));
            m_vRects.Add(value);
        }
        //-----------------------------------------------------
        public void SetRect(int index, Rect value)
        {
            if (index >= 0 && m_vRects != null && m_vRects.Count > 0 && m_vTypes != null && m_vTypes.Count > 0 && index < m_vTypes.Count)
            {
                var type = m_vTypes[index];
                if (type.type != EVariableType.eRect)
                {
                    Debug.LogError($"VariableList: SetRect type mismatch, expected {EVariableType.eRect}, got {type}");
                }
                if (type.index < 0 || type.index >= m_vRects.Count)
                {
                    Debug.LogError($"VariableList: SetRect index out of range, index={type.index}, count={m_vRects.Count}");
                    return;
                }
                m_vRects[type.index] = value;
            }
        }
        //-----------------------------------------------------
        public Rect GetRect(int index)
        {
            if (index >= 0 && m_vRects != null && m_vRects.Count > 0 && m_vTypes != null && m_vTypes.Count > 0 && index < m_vTypes.Count)
            {
                var type = m_vTypes[index];
                if (type.type != EVariableType.eRect)
                {
                    Debug.LogError($"VariableList: GetRect type mismatch, expected {EVariableType.eRect}, got {type}");
                }
                if (type.index < 0 || type.index >= m_vRects.Count)
                {
                    Debug.LogError($"VariableList: GetRect index out of range, index={type.index}, count={m_vRects.Count}");
                    return default;
                }
                return m_vRects[type.index];
            }
            return default;
        }
        //-----------------------------------------------------
        public List<Rect> GetRects()
        {
            return m_vRects;
        }
        //-----------------------------------------------------
        public void AddMatrix(Matrix4x4 value)
        {
            if (m_vMatrixs == null) m_vMatrixs = new List<Matrix4x4>(m_nCapacity);
            if (m_vTypes == null) m_vTypes = new List<TypeIndex>(m_nCapacity);
            m_vTypes.Add(new TypeIndex(EVariableType.eMatrix, (byte)m_vMatrixs.Count));
            m_vMatrixs.Add(value);
        }
        //-----------------------------------------------------
        public void SetMatrix(int index, Matrix4x4 value)
        {
            if (index >= 0 && m_vMatrixs != null && m_vMatrixs.Count > 0 && m_vTypes != null && m_vTypes.Count > 0 && index < m_vTypes.Count)
            {
                var type = m_vTypes[index];
                if (type.type != EVariableType.eMatrix)
                {
                    Debug.LogError($"VariableList: SetMatrix type mismatch, expected {EVariableType.eMatrix}, got {type}");
                }
                if (type.index < 0 || type.index >= m_vMatrixs.Count)
                {
                    Debug.LogError($"VariableList: SetMatrix index out of range, index={type.index}, count={m_vMatrixs.Count}");
                    return;
                }
                m_vMatrixs[type.index] = value;
            }
        }
        //-----------------------------------------------------
        public Matrix4x4 GetMatrix(int index)
        {
            if (index >= 0 && m_vMatrixs != null && m_vMatrixs.Count > 0 && m_vTypes != null && m_vTypes.Count > 0 && index < m_vTypes.Count)
            {
                var type = m_vTypes[index];
                if (type.type != EVariableType.eMatrix)
                {
                    Debug.LogError($"VariableList: GetMatrix type mismatch, expected {EVariableType.eMatrix}, got {type}");
                }
                if (type.index < 0 || type.index >= m_vMatrixs.Count)
                {
                    Debug.LogError($"VariableList: GetMatrix index out of range, index={type.index}, count={m_vMatrixs.Count}");
                    return Matrix4x4.identity;
                }
                return m_vMatrixs[type.index];
            }
            return Matrix4x4.identity;
        }
        //-----------------------------------------------------
        public List<Matrix4x4> GetMatrixs()
        {
            return m_vMatrixs;
        }
        //-----------------------------------------------------
        public void AddUserData(IUserData value)
        {
            if (m_vUserDatas == null) m_vUserDatas = new List<IUserData>(m_nCapacity);
            if (m_vTypes == null) m_vTypes = new List<TypeIndex>(m_nCapacity);
            m_vTypes.Add(new TypeIndex(EVariableType.eUserData, (byte)m_vUserDatas.Count));
            m_vUserDatas.Add(value);
        }
        //-----------------------------------------------------
        public void SetUserData(int index, IUserData value)
        {
            if (index >= 0 && m_vUserDatas != null && m_vUserDatas.Count > 0 && m_vTypes != null && m_vTypes.Count > 0 && index < m_vTypes.Count)
            {
                var type = m_vTypes[index];
                if (type.type != EVariableType.eUserData)
                {
                    Debug.LogError($"VariableList: SetUserData type mismatch, expected {EVariableType.eUserData}, got {type}");
                }
                if (type.index < 0 || type.index >= m_vUserDatas.Count)
                {
                    Debug.LogError($"VariableList: SetUserData index out of range, index={type.index}, count={m_vUserDatas.Count}");
                    return;
                }
                m_vUserDatas[type.index] = value;
            }
        }
        //-----------------------------------------------------
        public IUserData GetUserData(int index, IUserData defaultValue = null)
        {
            if (index >= 0 && m_vUserDatas != null && m_vUserDatas.Count > 0 && m_vTypes != null && m_vTypes.Count > 0 && index < m_vTypes.Count)
            {
                var type = m_vTypes[index];
                if (type.type != EVariableType.eUserData)
                {
                    Debug.LogError($"VariableList: GetUserData type mismatch, expected {EVariableType.eUserData}, got {type}");
                }
                if (type.index < 0 || type.index >= m_vUserDatas.Count)
                {
                    Debug.LogError($"VariableList: GetUserData index out of range, index={type.index}, count={m_vUserDatas.Count}");
                    return defaultValue;
                }
                return m_vUserDatas[type.index];
            }
            return defaultValue;
        }
        //-----------------------------------------------------
        public List<IUserData> GetUserDatas()
        {
            return m_vUserDatas;
        }
        //-----------------------------------------------------
        internal void ChangeType(int index, EVariableType type, string defaultValue = null)
        {
            if (m_vTypes == null || index < 0 || index >= m_vTypes.Count)
                return;

            var oldTypeIndex = m_vTypes[index];
            int removedDataIndex = oldTypeIndex.index;

            // 1. 移除原类型数据
            switch (oldTypeIndex.type)
            {
                case EVariableType.eBool: m_vBools?.RemoveAt(removedDataIndex); break;
                case EVariableType.eInt: m_vInts?.RemoveAt(removedDataIndex); break;
                case EVariableType.eFloat: m_vFloats?.RemoveAt(removedDataIndex); break;
                case EVariableType.eString: m_vStrings?.RemoveAt(removedDataIndex); break;
                case EVariableType.eVec2: m_vVec2s?.RemoveAt(removedDataIndex); break;
                case EVariableType.eVec3: m_vVec3s?.RemoveAt(removedDataIndex); break;
                case EVariableType.eVec4: m_vVec4s?.RemoveAt(removedDataIndex); break;
                case EVariableType.eObjId: m_vObjIds?.RemoveAt(removedDataIndex); break;
                case EVariableType.eRay: m_vRays?.RemoveAt(removedDataIndex); break;
                case EVariableType.eRay2D: m_vRay2Ds?.RemoveAt(removedDataIndex); break;
                case EVariableType.eQuaternion: m_vQuaternions?.RemoveAt(removedDataIndex); break;
                case EVariableType.eBounds: m_vBounds?.RemoveAt(removedDataIndex); break;
                case EVariableType.eRect: m_vRects?.RemoveAt(removedDataIndex); break;
                case EVariableType.eMatrix: m_vMatrixs?.RemoveAt(removedDataIndex); break;
                case EVariableType.eUserData: m_vUserDatas?.RemoveAt(removedDataIndex); break;
                case EVariableType.eLong: m_vLongs?.RemoveAt(removedDataIndex); break;
                case EVariableType.eDouble: m_vDoubles?.RemoveAt(removedDataIndex); break;
                default: break;
            }

            // 2. 修正 m_vTypes 里同类型且 index > 被移除的 index 的 TypeIndex
            for (int i = 0; i < m_vTypes.Count; ++i)
            {
                if (i == index) continue;
                if (m_vTypes[i].type == oldTypeIndex.type && m_vTypes[i].index > removedDataIndex)
                {
                    m_vTypes[i] = new TypeIndex(m_vTypes[i].type, (byte)(m_vTypes[i].index - 1));
                }
            }

            // 3. 移除 m_vTypes 的该项
            m_vTypes.RemoveAt(index);

            // 4. 插入新类型的值（优先用defaultValue，否则用类型默认值）
            switch (type)
            {
                case EVariableType.eBool:
                    {
                        bool v = false;
                        if (!string.IsNullOrEmpty(defaultValue))
                            bool.TryParse(defaultValue, out v);
                        AddBool(v);
                    }
                    break;
                case EVariableType.eInt:
                    {
                        int v = 0;
                        if (!string.IsNullOrEmpty(defaultValue))
                            int.TryParse(defaultValue, out v);
                        AddInt(v);
                    }
                    break;
                case EVariableType.eFloat:
                    {
                        float v = 0;
                        if (!string.IsNullOrEmpty(defaultValue))
                            float.TryParse(defaultValue, out v);
                        AddFloat(v);
                    }
                    break;
                case EVariableType.eString:
                    {
                        AddString(defaultValue ?? string.Empty);
                    }
                    break;
                case EVariableType.eVec2:
                    {
                        Vector2 v = Vector2.zero;
                        if (!string.IsNullOrEmpty(defaultValue))
                        {
                            var split = defaultValue.Split('|');
                            if (split.Length >= 2)
                            {
                                float.TryParse(split[0], out v.x);
                                float.TryParse(split[1], out v.y);
                            }
                        }
                        AddVec2(v);
                    }
                    break;
                case EVariableType.eVec3:
                    {
                        Vector3 v = Vector3.zero;
                        if (!string.IsNullOrEmpty(defaultValue))
                        {
                            var split = defaultValue.Split('|');
                            if (split.Length >= 3)
                            {
                                float.TryParse(split[0], out v.x);
                                float.TryParse(split[1], out v.y);
                                float.TryParse(split[2], out v.z);
                            }
                        }
                        AddVec3(v);
                    }
                    break;
                case EVariableType.eVec4:
                    {
                        Vector4 v = Vector4.zero;
                        if (!string.IsNullOrEmpty(defaultValue))
                        {
                            var split = defaultValue.Split('|');
                            if (split.Length >= 4)
                            {
                                float.TryParse(split[0], out v.x);
                                float.TryParse(split[1], out v.y);
                                float.TryParse(split[2], out v.z);
                                float.TryParse(split[3], out v.w);
                            }
                        }
                        AddVec4(v);
                    }
                    break;
                case EVariableType.eObjId:
                    {
                        ObjId v = default;
                        if (!string.IsNullOrEmpty(defaultValue))
                            int.TryParse(defaultValue, out v.id);
                        AddObjId(v);
                    }
                    break;
                case EVariableType.eRay:
                    {
                        AddRay(default); // 可根据需求解析 defaultValue
                    }
                    break;
                case EVariableType.eRay2D:
                    {
                        AddRay2D(default);
                    }
                    break;
                case EVariableType.eQuaternion:
                    {
                        AddQuaternion(Quaternion.identity);
                    }
                    break;
                case EVariableType.eBounds:
                    {
                        AddBounds(default);
                    }
                    break;
                case EVariableType.eRect:
                    {
                        AddRect(default);
                    }
                    break;
                case EVariableType.eMatrix:
                    {
                        AddMatrix(Matrix4x4.identity);
                    }
                    break;
                case EVariableType.eUserData:
                    {
                        AddUserData(default);
                    }
                    break;
                case EVariableType.eLong:
                    {
                        AddLong(0);
                    }
                    break;
                case EVariableType.eDouble:
                    {
                        AddDouble(0);
                    }
                    break;
                default:
                    break;
            }
        }
        //-----------------------------------------------------
        internal void RemoveIndex(int index)
        {
            if (m_vTypes == null || index < 0 || index >= m_vTypes.Count)
                return;

            var typeIndex = m_vTypes[index];
            int dataIndex = typeIndex.index;

            // 1. 移除类型数据
            switch (typeIndex.type)
            {
                case EVariableType.eBool: m_vBools?.RemoveAt(dataIndex); break;
                case EVariableType.eInt: m_vInts?.RemoveAt(dataIndex); break;
                case EVariableType.eFloat: m_vFloats?.RemoveAt(dataIndex); break;
                case EVariableType.eString: m_vStrings?.RemoveAt(dataIndex); break;
                case EVariableType.eVec2: m_vVec2s?.RemoveAt(dataIndex); break;
                case EVariableType.eVec3: m_vVec3s?.RemoveAt(dataIndex); break;
                case EVariableType.eVec4: m_vVec4s?.RemoveAt(dataIndex); break;
                case EVariableType.eObjId: m_vObjIds?.RemoveAt(dataIndex); break;
                case EVariableType.eRay: m_vRays?.RemoveAt(dataIndex); break;
                case EVariableType.eRay2D: m_vRay2Ds?.RemoveAt(dataIndex); break;
                case EVariableType.eQuaternion: m_vQuaternions?.RemoveAt(dataIndex); break;
                case EVariableType.eBounds: m_vBounds?.RemoveAt(dataIndex); break;
                case EVariableType.eRect: m_vRects?.RemoveAt(dataIndex); break;
                case EVariableType.eMatrix: m_vMatrixs?.RemoveAt(dataIndex); break;
                case EVariableType.eUserData: m_vUserDatas?.RemoveAt(dataIndex); break;
                case EVariableType.eLong: m_vLongs?.RemoveAt(dataIndex); break;
                case EVariableType.eDouble: m_vDoubles?.RemoveAt(dataIndex); break;
                default: break;
            }

            // 2. 修正 m_vTypes 里同类型且 index > 被移除的 index 的 TypeIndex
            for (int i = 0; i < m_vTypes.Count; ++i)
            {
                if (i == index) continue;
                if (m_vTypes[i].type == typeIndex.type && m_vTypes[i].index > dataIndex)
                {
                    m_vTypes[i] = new TypeIndex(m_vTypes[i].type, (byte)(m_vTypes[i].index - 1));
                }
            }

            // 3. 移除 m_vTypes 的该项
            m_vTypes.RemoveAt(index);
        }
        //-----------------------------------------------------
        internal void SwapIndex(int index0, int index1)
        {
            if (m_vTypes == null || index0 < 0 || index1 < 0 || index0 >= m_vTypes.Count || index1 >= m_vTypes.Count || index0 == index1)
                return;

            var typeIndex0 = m_vTypes[index0];
            var typeIndex1 = m_vTypes[index1];

            if (typeIndex0.type == typeIndex1.type)
            {
                // 同类型，交换数据和TypeIndex.index
                switch (typeIndex0.type)
                {
                    case EVariableType.eBool:
                        if (m_vBools != null)
                        {
                            bool tmp = m_vBools[typeIndex0.index];
                            m_vBools[typeIndex0.index] = m_vBools[typeIndex1.index];
                            m_vBools[typeIndex1.index] = tmp;
                        }
                        break;
                    case EVariableType.eInt:
                        if (m_vInts != null)
                        {
                            int tmp = m_vInts[typeIndex0.index];
                            m_vInts[typeIndex0.index] = m_vInts[typeIndex1.index];
                            m_vInts[typeIndex1.index] = tmp;
                        }
                        break;
                    case EVariableType.eFloat:
                        if (m_vFloats != null)
                        {
                            float tmp = m_vFloats[typeIndex0.index];
                            m_vFloats[typeIndex0.index] = m_vFloats[typeIndex1.index];
                            m_vFloats[typeIndex1.index] = tmp;
                        }
                        break;
                    case EVariableType.eString:
                        if (m_vStrings != null)
                        {
                            string tmp = m_vStrings[typeIndex0.index];
                            m_vStrings[typeIndex0.index] = m_vStrings[typeIndex1.index];
                            m_vStrings[typeIndex1.index] = tmp;
                        }
                        break;
                    case EVariableType.eVec2:
                        if (m_vVec2s != null)
                        {
                            Vector2 tmp = m_vVec2s[typeIndex0.index];
                            m_vVec2s[typeIndex0.index] = m_vVec2s[typeIndex1.index];
                            m_vVec2s[typeIndex1.index] = tmp;
                        }
                        break;
                    case EVariableType.eVec3:
                        if (m_vVec3s != null)
                        {
                            Vector3 tmp = m_vVec3s[typeIndex0.index];
                            m_vVec3s[typeIndex0.index] = m_vVec3s[typeIndex1.index];
                            m_vVec3s[typeIndex1.index] = tmp;
                        }
                        break;
                    case EVariableType.eVec4:
                        if (m_vVec4s != null)
                        {
                            Vector4 tmp = m_vVec4s[typeIndex0.index];
                            m_vVec4s[typeIndex0.index] = m_vVec4s[typeIndex1.index];
                            m_vVec4s[typeIndex1.index] = tmp;
                        }
                        break;
                    case EVariableType.eObjId:
                        if (m_vObjIds != null)
                        {
                            ObjId tmp = m_vObjIds[typeIndex0.index];
                            m_vObjIds[typeIndex0.index] = m_vObjIds[typeIndex1.index];
                            m_vObjIds[typeIndex1.index] = tmp;
                        }
                        break;
                    case EVariableType.eRay:
                        if (m_vRays != null)
                        {
                            Ray tmp = m_vRays[typeIndex0.index];
                            m_vRays[typeIndex0.index] = m_vRays[typeIndex1.index];
                            m_vRays[typeIndex1.index] = tmp;
                        }
                        break;
                    case EVariableType.eRay2D:
                        if (m_vRay2Ds != null)
                        {
                            Ray2D tmp = m_vRay2Ds[typeIndex0.index];
                            m_vRay2Ds[typeIndex0.index] = m_vRay2Ds[typeIndex1.index];
                            m_vRay2Ds[typeIndex1.index] = tmp;
                        }
                        break;
                    case EVariableType.eQuaternion:
                        if (m_vQuaternions != null)
                        {
                            Quaternion tmp = m_vQuaternions[typeIndex0.index];
                            m_vQuaternions[typeIndex0.index] = m_vQuaternions[typeIndex1.index];
                            m_vQuaternions[typeIndex1.index] = tmp;
                        }
                        break;
                    case EVariableType.eBounds:
                        if (m_vBounds != null)
                        {
                            Bounds tmp = m_vBounds[typeIndex0.index];
                            m_vBounds[typeIndex0.index] = m_vBounds[typeIndex1.index];
                            m_vBounds[typeIndex1.index] = tmp;
                        }
                        break;
                    case EVariableType.eRect:
                        if (m_vRects != null)
                        {
                            Rect tmp = m_vRects[typeIndex0.index];
                            m_vRects[typeIndex0.index] = m_vRects[typeIndex1.index];
                            m_vRects[typeIndex1.index] = tmp;
                        }
                        break;
                    case EVariableType.eMatrix:
                        if (m_vMatrixs != null)
                        {
                            Matrix4x4 tmp = m_vMatrixs[typeIndex0.index];
                            m_vMatrixs[typeIndex0.index] = m_vMatrixs[typeIndex1.index];
                            m_vMatrixs[typeIndex1.index] = tmp;
                        }
                        break;
                    case EVariableType.eUserData:
                        if (m_vUserDatas != null)
                        {
                            IUserData tmp = m_vUserDatas[typeIndex0.index];
                            m_vUserDatas[typeIndex0.index] = m_vUserDatas[typeIndex1.index];
                            m_vUserDatas[typeIndex1.index] = tmp;
                        }
                        break;
                    case EVariableType.eLong:
                        if (m_vLongs != null)
                        {
                            long tmp = m_vLongs[typeIndex0.index];
                            m_vLongs[typeIndex0.index] = m_vLongs[typeIndex1.index];
                            m_vLongs[typeIndex1.index] = tmp;
                        }
                        break;
                    case EVariableType.eDouble:
                        if (m_vDoubles != null)
                        {
                            double tmp = m_vDoubles[typeIndex0.index];
                            m_vDoubles[typeIndex0.index] = m_vDoubles[typeIndex1.index];
                            m_vDoubles[typeIndex1.index] = tmp;
                        }
                        break;
                    default:
                        break;
                }
                // 交换TypeIndex.index
                m_vTypes[index0] = new TypeIndex(typeIndex0.type, typeIndex1.index);
                m_vTypes[index1] = new TypeIndex(typeIndex1.type, typeIndex0.index);
            }
            else
            {
                // 不同类型，只交换TypeIndex，不动数据
                m_vTypes[index0] = typeIndex1;
                m_vTypes[index1] = typeIndex0;
            }
        }
        //-----------------------------------------------------
        public bool AddVariable(IVariable value)
        {
            if (value is VariableBool varB) AddBool(varB.value);
            else if (value is VariableInt varI) AddInt(varI.value);
            else if (value is VariableFloat varF) AddFloat(varF.value);
            else if (value is VariableVec2 varVec2) AddVec2(varVec2.value);
            else if (value is VariableVec3 varVec3) AddVec3(varVec3.value);
            else if (value is VariableVec4 varVec4) AddVec4(varVec4.value);
            else if (value is VariableString varStr) AddString(varStr.value);
            else if (value is VariableObjId varObj) AddObjId(varObj.value);
            else if (value is VariableRay varRay) AddRay(varRay.value);
            else if (value is VariableRay2D varRay2D) AddRay2D(varRay2D.value);
            else if (value is VariableQuaternion varQuat) AddQuaternion(varQuat.value);
            else if (value is VariableBounds varBounds) AddBounds(varBounds.value);
            else if (value is VariableRect varRect) AddRect(varRect.value);
            else if (value is VariableMatrix varMatrix) AddMatrix(varMatrix.value);
            else if (value is VariableUserData varUserData) AddUserData(varUserData.pUser);
            else if (value is VariableLong varLongData) AddLong(varLongData.value);
            else if (value is VariableDouble varDoubleData) AddDouble(varDoubleData.value);
            else return false;
            return true;
        }
        //-----------------------------------------------------
        public bool AddVariable(VariableList value)
        {
            if (value == null || value.GetVarCount() == 0)
                return false;

            int count = value.GetVarCount();
            for (int i = 0; i < count; i++)
            {
                AddVariable(value, i);
            }
            return true;
        }
        //-----------------------------------------------------
        public bool AddVariable(VariableList value, int index)
        {
            if (value == null || value.GetVarCount() == 0)
                return false;

            int count = value.GetVarCount();
            if (index < 0 || index >= count)
                return false;

            var type = value.GetVarType(index);
            switch (type)
            {
                case EVariableType.eBool:
                    AddBool(value.GetBool(index));
                    break;
                case EVariableType.eInt:
                    AddInt(value.GetInt(index));
                    break;
                case EVariableType.eFloat:
                    AddFloat(value.GetFloat(index));
                    break;
                case EVariableType.eString:
                    AddString(value.GetString(index));
                    break;
                case EVariableType.eVec2:
                    AddVec2(value.GetVec2(index));
                    break;
                case EVariableType.eVec3:
                    AddVec3(value.GetVec3(index));
                    break;
                case EVariableType.eVec4:
                    AddVec4(value.GetVec4(index));
                    break;
                case EVariableType.eObjId:
                    AddObjId(value.GetObjId(index));
                    break;
                case EVariableType.eRay:
                    AddRay(value.GetRay(index));
                    break;
                case EVariableType.eRay2D:
                    AddRay2D(value.GetRay2D(index));
                    break;
                case EVariableType.eQuaternion:
                    AddQuaternion(value.GetQuaternion(index));
                    break;
                case EVariableType.eBounds:
                    AddBounds(value.GetBounds(index));
                    break;
                case EVariableType.eRect:
                    AddRect(value.GetRect(index));
                    break;
                case EVariableType.eMatrix:
                    AddMatrix(value.GetMatrix(index));
                    break;
                case EVariableType.eUserData:
                    AddUserData(value.GetUserData(index));
                    break;
                case EVariableType.eLong:
                    AddLong(value.GetLong(index));
                    break;
                case EVariableType.eDouble:
                    AddDouble(value.GetDouble(index));
                    break;
                default:
                    // 跳过未知类型
                    break;
            }
            return true;
        }
        //-----------------------------------------------------
        public bool AddVariable(string value)
        {
            AddString(value);
            return true;
        }
        //-----------------------------------------------------
        public bool AddVariable<T>(T value) where T : struct
        {
            if (value is byte byVal) AddInt(byVal);
            else if (value is short sVal) AddInt(sVal);
            else if (value is ushort usVal) AddInt(usVal);
            else if (value is int intVal) AddInt(intVal);
            else if (value is uint uintVal) AddInt((int)uintVal);
            else if (value is float floatVal) AddFloat(floatVal);
            else if (value is bool boolVal) AddBool(boolVal);
            else if (value is Vector2 v2Val) AddVec2(v2Val);
            else if (value is Vector3 v3Val) AddVec3(v3Val);
            else if (value is Vector4 v4Val) AddVec4(v4Val);
            else if (value is ObjId objId) AddObjId(objId);
            else if (value is Ray rayVal) AddRay(rayVal);
            else if (value is Ray2D ray2DVal) AddRay2D(ray2DVal);
            else if (value is Quaternion quatVal) AddQuaternion(quatVal);
            else if (value is Bounds boundsVal) AddBounds(boundsVal);
            else if (value is Rect rectVal) AddRect(rectVal);
            else if (value is Matrix4x4 matrixVal) AddMatrix(matrixVal);
            else if (value is IUserData userDataVal) AddUserData(userDataVal);
            else if (value is long userLong) AddLong(userLong);
            else if (value is double userDouble) AddDouble(userDouble);
            else return false;
            return true;
        }
    }
}