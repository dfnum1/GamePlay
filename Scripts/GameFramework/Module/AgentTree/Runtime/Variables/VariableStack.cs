/********************************************************************
生成日期:	06:30:2025
类    名: 	VariableStack
作    者:	HappLI
描    述:	变量栈
*********************************************************************/
using Framework.Core;
using System;
using UnityEngine;

namespace Framework.AT.Runtime
{
    public class VariableStack
    {
        System.Collections.Generic.Stack<bool>          m_vBools = null;
        System.Collections.Generic.Stack<int>           m_vInts = null;
        System.Collections.Generic.Stack<long>          m_vLongs = null;
        System.Collections.Generic.Stack<float>         m_vFloats = null;
        System.Collections.Generic.Stack<double>        m_vDoubles = null;
        System.Collections.Generic.Stack<string>        m_vStrings = null;
        System.Collections.Generic.Stack<Vector2>       m_vVec2s = null;
        System.Collections.Generic.Stack<Vector3>       m_vVec3s = null;
        System.Collections.Generic.Stack<Vector4>       m_vVec4s = null;
        System.Collections.Generic.Stack<ObjId>         m_vObjIds = null;
        System.Collections.Generic.Stack<Ray>           m_vRays = null;
        System.Collections.Generic.Stack<Color>         m_vColors = null;
        System.Collections.Generic.Stack<Quaternion>    m_vQuaternions = null;
        System.Collections.Generic.Stack<Bounds>        m_vBounds = null;
        System.Collections.Generic.Stack<Rect>          m_vRects = null;
        System.Collections.Generic.Stack<Matrix4x4>     m_vMatrixs = null;
        System.Collections.Generic.Stack<IUserData>     m_vUserDatas = null;
        System.Collections.Generic.Stack<EVariableType> m_vTypes = null;
        byte                                            m_nCapacity = 2;
        //-----------------------------------------------------
        internal VariableStack()
        {

        }
        //-----------------------------------------------------
        public static VariableStack Malloc(int capacity = 2)
        {
            VariableStack list = VariablePool.GetVariableStack();
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
            m_vColors?.Clear();
            m_vBounds?.Clear();
            m_vMatrixs?.Clear();
            m_vRects?.Clear();
            m_vUserDatas?.Clear();
            m_vQuaternions?.Clear();
        }
        //-----------------------------------------------------
        public EVariableType GetVarType()
        {
            if (m_vTypes == null || m_vTypes.Count == 0) return EVariableType.eNone;
            return m_vTypes.Peek();
        }
        //-----------------------------------------------------
        public void PushBool(bool value)
        {
            if (m_vBools == null) m_vBools = new System.Collections.Generic.Stack<bool>(m_nCapacity);
            if (m_vTypes == null) m_vTypes = new System.Collections.Generic.Stack<EVariableType>(m_nCapacity);
            m_vTypes.Push(EVariableType.eBool);
            m_vBools.Push(value);
        }
        //-----------------------------------------------------
        public bool PopBool(out bool value)
        {
            if (m_vBools != null && m_vBools.Count > 0 && m_vTypes != null && m_vTypes.Count > 0)
            {
                var type = m_vTypes.Pop();
                if (type != EVariableType.eBool)
                {
                    Debug.LogError($"VariableStack: PopBool type mismatch, expected {EVariableType.eBool}, got {type}");
                }
                value = m_vBools.Pop();
                return true;
            }
            value = false;
            return false;
        }
        //-----------------------------------------------------
        public void PushInt(int value)
        {
            if (m_vInts == null) m_vInts = new System.Collections.Generic.Stack<int>(m_nCapacity);
            if (m_vTypes == null) m_vTypes = new System.Collections.Generic.Stack<EVariableType>(m_nCapacity);
            m_vTypes.Push(EVariableType.eInt);
            m_vInts.Push(value);
        }
        //-----------------------------------------------------
        public bool PopInt(out int value)
        {
            if (m_vInts != null && m_vInts.Count > 0 && m_vTypes != null && m_vTypes.Count > 0)
            {
                var type = m_vTypes.Pop();
                if (type != EVariableType.eInt)
                {
                    Debug.LogError($"VariableStack: PopInt type mismatch, expected {EVariableType.eInt}, got {type}");
                }
                value = m_vInts.Pop();
                return true;
            }
            value = 0;
            return false;
        }
        //-----------------------------------------------------
        public void PushLong(long value)
        {
            if (m_vLongs == null) m_vLongs = new System.Collections.Generic.Stack<long>(m_nCapacity);
            if (m_vTypes == null) m_vTypes = new System.Collections.Generic.Stack<EVariableType>(m_nCapacity);
            m_vTypes.Push(EVariableType.eLong);
            m_vLongs.Push(value);
        }
        //-----------------------------------------------------
        public bool PopLong(out long value)
        {
            if (m_vLongs != null && m_vLongs.Count > 0 && m_vTypes != null && m_vTypes.Count > 0)
            {
                var type = m_vTypes.Pop();
                if (type != EVariableType.eLong)
                {
                    Debug.LogError($"VariableStack: PopLong type mismatch, expected {EVariableType.eLong}, got {type}");
                }
                value = m_vLongs.Pop();
                return true;
            }
            value = 0;
            return false;
        }
        //-----------------------------------------------------
        public void PushFloat(float value)
        {
            if (m_vFloats == null) m_vFloats = new System.Collections.Generic.Stack<float>(m_nCapacity);
            if (m_vTypes == null) m_vTypes = new System.Collections.Generic.Stack<EVariableType>(m_nCapacity);
            m_vTypes.Push(EVariableType.eFloat);
            m_vFloats.Push(value);
        }
        //-----------------------------------------------------
        public bool PopFloat(out float value)
        {
            if (m_vFloats != null && m_vFloats.Count > 0 && m_vTypes != null && m_vTypes.Count > 0)
            {
                var type = m_vTypes.Pop();
                if (type != EVariableType.eFloat)
                {
                    Debug.LogError($"VariableStack: PopFloat type mismatch, expected {EVariableType.eFloat}, got {type}");
                }
                value = m_vFloats.Pop();
                return true;
            }
            value = 0.0f;
            return false;
        }
        //-----------------------------------------------------
        public void PushDouble(double value)
        {
            if (m_vDoubles == null) m_vDoubles = new System.Collections.Generic.Stack<double>(m_nCapacity);
            if (m_vTypes == null) m_vTypes = new System.Collections.Generic.Stack<EVariableType>(m_nCapacity);
            m_vTypes.Push(EVariableType.eDouble);
            m_vDoubles.Push(value);
        }
        //-----------------------------------------------------
        public bool PopDouble(out double value)
        {
            if (m_vDoubles != null && m_vDoubles.Count > 0 && m_vTypes != null && m_vTypes.Count > 0)
            {
                var type = m_vTypes.Pop();
                if (type != EVariableType.eDouble)
                {
                    Debug.LogError($"VariableStack: PopDouble type mismatch, expected {EVariableType.eDouble}, got {type}");
                }
                value = m_vDoubles.Pop();
                return true;
            }
            value = 0.0;
            return false;
        }
        //-----------------------------------------------------
        public void PushString(string value)
        {
            if (m_vStrings == null) m_vStrings = new System.Collections.Generic.Stack<string>(m_nCapacity);
            if (m_vTypes == null) m_vTypes = new System.Collections.Generic.Stack<EVariableType>(m_nCapacity);
            m_vTypes.Push(EVariableType.eString);
            m_vStrings.Push(value);
        }
        //-----------------------------------------------------
        public bool PopString(out string value)
        {
            if (m_vStrings != null && m_vStrings.Count > 0 && m_vTypes != null && m_vTypes.Count > 0)
            {
                var type = m_vTypes.Pop();
                if (type != EVariableType.eString)
                {
                    Debug.LogError($"VariableStack: PopString type mismatch, expected {EVariableType.eString}, got {type}");
                }
                value = m_vStrings.Pop();
                return true;
            }
            value = null;
            return false;
        }
        //-----------------------------------------------------
        public void PushVec2(Vector2 value)
        {
            if (m_vVec2s == null) m_vVec2s = new System.Collections.Generic.Stack<Vector2>(m_nCapacity);
            if (m_vTypes == null) m_vTypes = new System.Collections.Generic.Stack<EVariableType>(m_nCapacity);
            m_vTypes.Push(EVariableType.eVec2);
            m_vVec2s.Push(value);
        }
        //-----------------------------------------------------
        public bool PopVec2(out Vector2 value)
        {
            if (m_vVec2s != null && m_vVec2s.Count > 0 && m_vTypes != null && m_vTypes.Count > 0)
            {
                var type = m_vTypes.Pop();
                if (type != EVariableType.eVec2)
                {
                    Debug.LogError($"VariableStack: PopVec2 type mismatch, expected {EVariableType.eVec2}, got {type}");
                }
                value = m_vVec2s.Pop();
                return true;
            }
            value = Vector2.zero;
            return false;
        }
        //-----------------------------------------------------
        public void PushVec3(Vector3 value)
        {
            if (m_vVec3s == null) m_vVec3s = new System.Collections.Generic.Stack<Vector3>(m_nCapacity);
            if (m_vTypes == null) m_vTypes = new System.Collections.Generic.Stack<EVariableType>(m_nCapacity);
            m_vTypes.Push(EVariableType.eVec3);
            m_vVec3s.Push(value);
        }
        //-----------------------------------------------------
        public bool PopVec3(out Vector3 value)
        {
            if (m_vVec3s != null && m_vVec3s.Count > 0 && m_vTypes != null && m_vTypes.Count > 0)
            {
                var type = m_vTypes.Pop();
                if (type != EVariableType.eVec3)
                {
                    Debug.LogError($"VariableStack: PopVec3 type mismatch, expected {EVariableType.eVec3}, got {type}");
                }
                value = m_vVec3s.Pop();
                return true;
            }
            value = Vector3.zero;
            return false;
        }
        //-----------------------------------------------------
        public void PushVec4(Vector4 value)
        {
            if (m_vVec4s == null) m_vVec4s = new System.Collections.Generic.Stack<Vector4>(m_nCapacity);
            if (m_vTypes == null) m_vTypes = new System.Collections.Generic.Stack<EVariableType>(m_nCapacity);
            m_vTypes.Push(EVariableType.eVec4);
            m_vVec4s.Push(value);
        }
        //-----------------------------------------------------
        public bool PopVec4(out Vector4 value)
        {
            if (m_vVec4s != null && m_vVec4s.Count > 0 && m_vTypes != null && m_vTypes.Count > 0)
            {
                var type = m_vTypes.Pop();
                if (type != EVariableType.eVec4)
                {
                    Debug.LogError($"VariableStack: PopVec4 type mismatch, expected {EVariableType.eVec4}, got {type}");
                }
                value = m_vVec4s.Pop();
                return true;
            }
            value = Vector4.zero;
            return false;
        }
        //-----------------------------------------------------
        public void PushObjId(ObjId value)
        {
            if (m_vObjIds == null) m_vObjIds = new System.Collections.Generic.Stack<ObjId>(m_nCapacity);
            if (m_vTypes == null) m_vTypes = new System.Collections.Generic.Stack<EVariableType>(m_nCapacity);
            m_vTypes.Push(EVariableType.eObjId);
            m_vObjIds.Push(value);
        }
        //-----------------------------------------------------
        public bool PopObjId(out ObjId value)
        {
            if (m_vObjIds != null && m_vObjIds.Count > 0 && m_vTypes != null && m_vTypes.Count > 0)
            {
                var type = m_vTypes.Pop();
                if (type != EVariableType.eObjId)
                {
                    Debug.LogError($"VariableStack: PopObjId type mismatch, expected {EVariableType.eObjId}, got {type}");
                }
                value = m_vObjIds.Pop();
                return true;
            }
            value = new ObjId();
            return false;
        }
        //-----------------------------------------------------
        public void PushRay(Ray value)
        {
            if (m_vRays == null) m_vRays = new System.Collections.Generic.Stack<Ray>(m_nCapacity);
            if (m_vTypes == null) m_vTypes = new System.Collections.Generic.Stack<EVariableType>(m_nCapacity);
            m_vTypes.Push(EVariableType.eRay);
            m_vRays.Push(value);
        }
        //-----------------------------------------------------
        public bool PopRay(out Ray value)
        {
            if (m_vRays != null && m_vRays.Count > 0 && m_vTypes != null && m_vTypes.Count > 0)
            {
                var type = m_vTypes.Pop();
                if (type != EVariableType.eRay)
                {
                    Debug.LogError($"VariableStack: PopRay type mismatch, expected {EVariableType.eRay}, got {type}");
                }
                value = m_vRays.Pop();
                return true;
            }
            value = default;
            return false;
        }
        //-----------------------------------------------------
        public void PushBounds(Bounds value)
        {
            if (m_vBounds == null) m_vBounds = new System.Collections.Generic.Stack<Bounds>(m_nCapacity);
            if (m_vTypes == null) m_vTypes = new System.Collections.Generic.Stack<EVariableType>(m_nCapacity);
            m_vTypes.Push(EVariableType.eBounds);
            m_vBounds.Push(value);
        }
        //-----------------------------------------------------
        public bool PopBounds(out Bounds value)
        {
            if (m_vBounds != null && m_vBounds.Count > 0 && m_vTypes != null && m_vTypes.Count > 0)
            {
                var type = m_vTypes.Pop();
                if (type != EVariableType.eBounds)
                {
                    Debug.LogError($"VariableStack: PopBounds type mismatch, expected {EVariableType.eBounds}, got {type}");
                }
                value = m_vBounds.Pop();
                return true;
            }
            value = default;
            return false;
        }
        //-----------------------------------------------------
        public void PushMatrix(Matrix4x4 value)
        {
            if (m_vMatrixs == null) m_vMatrixs = new System.Collections.Generic.Stack<Matrix4x4>(m_nCapacity);
            if (m_vTypes == null) m_vTypes = new System.Collections.Generic.Stack<EVariableType>(m_nCapacity);
            m_vTypes.Push(EVariableType.eMatrix);
            m_vMatrixs.Push(value);
        }
        //-----------------------------------------------------
        public bool PopMatrix(out Matrix4x4 value)
        {
            if (m_vMatrixs != null && m_vMatrixs.Count > 0 && m_vTypes != null && m_vTypes.Count > 0)
            {
                var type = m_vTypes.Pop();
                if (type != EVariableType.eMatrix)
                {
                    Debug.LogError($"VariableStack: PopMatrix type mismatch, expected {EVariableType.eMatrix}, got {type}");
                }
                value = m_vMatrixs.Pop();
                return true;
            }
            value = Matrix4x4.identity;
            return false;
        }
        //-----------------------------------------------------
        public void PushRect(Rect value)
        {
            if (m_vRects == null) m_vRects = new System.Collections.Generic.Stack<Rect>(m_nCapacity);
            if (m_vTypes == null) m_vTypes = new System.Collections.Generic.Stack<EVariableType>(m_nCapacity);
            m_vTypes.Push(EVariableType.eRect);
            m_vRects.Push(value);
        }
        //-----------------------------------------------------
        public bool PopRect(out Rect value)
        {
            if (m_vRects != null && m_vRects.Count > 0 && m_vTypes != null && m_vTypes.Count > 0)
            {
                var type = m_vTypes.Pop();
                if (type != EVariableType.eRect)
                {
                    Debug.LogError($"VariableStack: PopRect type mismatch, expected {EVariableType.eRect}, got {type}");
                }
                value = m_vRects.Pop();
                return true;
            }
            value = default;
            return false;
        }
        //-----------------------------------------------------
        public void PushColor(Color value)
        {
            if (m_vColors == null) m_vColors = new System.Collections.Generic.Stack<Color>(m_nCapacity);
            if (m_vTypes == null) m_vTypes = new System.Collections.Generic.Stack<EVariableType>(m_nCapacity);
            m_vTypes.Push(EVariableType.eColor);
            m_vColors.Push(value);
        }
        //-----------------------------------------------------
        public bool PopColor(out Color value)
        {
            if (m_vColors != null && m_vColors.Count > 0 && m_vTypes != null && m_vTypes.Count > 0)
            {
                var type = m_vTypes.Pop();
                if (type != EVariableType.eColor)
                {
                    Debug.LogError($"VariableStack: PopColor type mismatch, expected {EVariableType.eColor}, got {type}");
                }
                value = m_vColors.Pop();
                return true;
            }
            value = default;
            return false;
        }
        //-----------------------------------------------------
        public void PushQuaternion(Quaternion value)
        {
            if (m_vQuaternions == null) m_vQuaternions = new System.Collections.Generic.Stack<Quaternion>(m_nCapacity);
            if (m_vTypes == null) m_vTypes = new System.Collections.Generic.Stack<EVariableType>(m_nCapacity);
            m_vTypes.Push(EVariableType.eQuaternion);
            m_vQuaternions.Push(value);
        }
        //-----------------------------------------------------
        public bool PopQuaternion(out Quaternion value)
        {
            if (m_vQuaternions != null && m_vQuaternions.Count > 0 && m_vTypes != null && m_vTypes.Count > 0)
            {
                var type = m_vTypes.Pop();
                if (type != EVariableType.eQuaternion)
                {
                    Debug.LogError($"VariableStack: PopQuaternion type mismatch, expected {EVariableType.eQuaternion}, got {type}");
                }
                value = m_vQuaternions.Pop();
                return true;
            }
            value = Quaternion.identity;
            return false;
        }
        //-----------------------------------------------------
        public void PushUserData(IUserData value)
        {
            if (m_vUserDatas == null) m_vUserDatas = new System.Collections.Generic.Stack<IUserData>(m_nCapacity);
            if (m_vTypes == null) m_vTypes = new System.Collections.Generic.Stack<EVariableType>(m_nCapacity);
            m_vTypes.Push(EVariableType.eUserData);
            m_vUserDatas.Push(value);
        }
        //-----------------------------------------------------
        public bool PopUserData(out IUserData value)
        {
            if (m_vUserDatas != null && m_vUserDatas.Count > 0 && m_vTypes != null && m_vTypes.Count > 0)
            {
                var type = m_vTypes.Pop();
                if (type != EVariableType.eUserData)
                {
                    Debug.LogError($"VariableStack: PopUserData type mismatch, expected {EVariableType.eUserData}, got {type}");
                }
                value = m_vUserDatas.Pop();
                return true;
            }
            value = null;
            return false;
        }
    }
}