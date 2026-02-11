/********************************************************************
生成日期:	06:30:2025
类    名: 	Variables
作    者:	HappLI
描    述:	变量
*********************************************************************/
using Framework.Core;
using System;
using System.Linq;
using UnityEngine;
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
namespace Framework.AT.Runtime
{
    [System.Serializable]
    public  struct ObjId
    {
        public byte userType;
        public int id;
        public ObjId(int id, byte userType =0)
        {
            this.userType = userType;
            this.id = id;
        }
        public static ObjId DEF => new ObjId { id = 0, userType =0 };
        public bool IsEqual(ObjId other)
        {
            return this.userType == other.userType && this.id == other.id;
        }
    }
    public enum EVariableType : byte
    {
        [DrawProps.Disable]eNone = 0,
        [InspectorName("整数")]eInt = 1,
        [InspectorName("浮点数")] eFloat = 2,
        [InspectorName("字符串")] eString = 3,
        [InspectorName("Bool")] eBool = 4,
        [InspectorName("长整形")] eLong,
        [InspectorName("双浮点")] eDouble,
        [InspectorName("二维向量")] eVec2,
        [InspectorName("三维向量")] eVec3,
        [InspectorName("四维向量")] eVec4,
        [InspectorName("ObjId")] eObjId,
        [InspectorName("Color")] eColor,
        [InspectorName("Ray")] eRay,
        [InspectorName("Quaternion")] eQuaternion,
        [InspectorName("Rect")] eRect,
        [InspectorName("Bounds")] eBounds,
        [InspectorName("Matrix")] eMatrix,
        [InspectorName("UserData")] eUserData,
    }
    //-----------------------------------------------------
    public interface IVariable
    {
        EVariableType GetVariableType();
        short GetGuid();
    }
    //-----------------------------------------------------
    [System.Serializable]
    public struct VariableInt : IVariable
    {
        public short guid;
        public int value;
        //-----------------------------------------------------
        public VariableInt(int value)
        {
            guid = 0;
            this.value = value;
        }
        //-----------------------------------------------------
        public EVariableType GetVariableType()
        {
            return EVariableType.eInt;
        }
        //-----------------------------------------------------
        public short GetGuid() { return guid; }
    }
    //-----------------------------------------------------
    [System.Serializable]
    public struct VariableLong : IVariable
    {
        public short guid;
        public long value;
        //-----------------------------------------------------
        public VariableLong(int value)
        {
            guid = 0;
            this.value = value;
        }
        //-----------------------------------------------------
        public EVariableType GetVariableType()
        {
            return EVariableType.eLong;
        }
        //-----------------------------------------------------
        public short GetGuid() { return guid; }
    }
    //-----------------------------------------------------
    [System.Serializable]
    public struct VariableDouble : IVariable
    {
        public short guid;
        public double value;
        //-----------------------------------------------------
        public VariableDouble(int value)
        {
            guid = 0;
            this.value = value;
        }
        //-----------------------------------------------------
        public EVariableType GetVariableType()
        {
            return EVariableType.eDouble;
        }
        //-----------------------------------------------------
        public short GetGuid() { return guid; }
    }
    //-----------------------------------------------------
    [System.Serializable]
    public struct VariableFloat : IVariable
    {
        public short guid;
        public float value;
        //-----------------------------------------------------
        public VariableFloat(float value)
        {
            guid = 0;
            this.value = value;
        }
        //-----------------------------------------------------
        public EVariableType GetVariableType()
        {
            return EVariableType.eFloat;
        }
        //-----------------------------------------------------
        public short GetGuid() { return guid; }
#if USE_FIXEDMATH
        //-----------------------------------------------------
        public static implicit operator FFloat(VariableFloat value)
        {
            return value.value;
        }
#endif
    }
    //-----------------------------------------------------
    [System.Serializable]
    public struct VariableVec2 : IVariable
    {
        public short guid;
        public Vector2 value;
        //-----------------------------------------------------
        public VariableVec2(Vector2 value)
        {
            guid = 0;
            this.value = value;
        }
        //-----------------------------------------------------
        public VariableVec2(float value1, float value2)
        {
            guid = 0;
            this.value = new Vector2(value1, value2);
        }
        //-----------------------------------------------------
        public EVariableType GetVariableType()
        {
            return EVariableType.eVec2;
        }
        //-----------------------------------------------------
        public short GetGuid() { return guid; }
#if USE_FIXEDMATH
        //-----------------------------------------------------
        public static implicit operator FVector2(VariableVec2 value)
        {
            return new FVector2(value.value.x, value.value.y);
        }
#endif
    }
    //-----------------------------------------------------
    [System.Serializable]
    public struct VariableVec3 : IVariable
    {
        public short guid;
        public Vector3 value;
        //-----------------------------------------------------
        public VariableVec3(Vector3 value)
        {
            guid = 0;
            this.value = value;
        }
        //-----------------------------------------------------
        public VariableVec3(float value1, float value2, float value3)
        {
            guid = 0;
            this.value = new Vector3(value1, value2, value3);
        }
        //-----------------------------------------------------
        public EVariableType GetVariableType()
        {
            return EVariableType.eVec3;
        }
        //-----------------------------------------------------
        public short GetGuid() { return guid; }
#if USE_FIXEDMATH
        //-----------------------------------------------------
        public static implicit operator FVector3(VariableVec3 value)
        {
            return new FVector3(value.value.x, value.value.y, value.value.z);
        }
#endif
    }
    //-----------------------------------------------------
    [System.Serializable]
    public struct VariableVec4 : IVariable
    {
        public short guid;
        public FVector4 value;
        //-----------------------------------------------------
        public VariableVec4(Vector4 value)
        {
            guid = 0;
            this.value = value;
        }
        //-----------------------------------------------------
        public VariableVec4(float value1, float value2, float value3, float value4)
        {
            guid = 0;
            this.value = new FVector4(value1, value2, value3, value4);
        }
        //-----------------------------------------------------
        public EVariableType GetVariableType()
        {
            return EVariableType.eVec4;
        }
        //-----------------------------------------------------
        public short GetGuid() { return guid; }
    }
    //-----------------------------------------------------
    [System.Serializable]
    public struct VariableColor : IVariable
    {
        public short guid;
        public Color value;
        //-----------------------------------------------------
        public VariableColor(Vector4 value)
        {
            guid = 0;
            this.value = value;
        }
        //-----------------------------------------------------
        public VariableColor(float r, float g, float b, float a)
        {
            guid = 0;
            this.value = new Color(r, g, b, a);
        }
        //-----------------------------------------------------
        public EVariableType GetVariableType()
        {
            return EVariableType.eColor;
        }
        //-----------------------------------------------------
        public short GetGuid() { return guid; }
    }
    //-----------------------------------------------------
    [System.Serializable]
    public struct VariableString : IVariable
    {
        public short guid;
        public string value;
        //-----------------------------------------------------
        public VariableString(string value)
        {
            guid = 0;
            this.value = value;
        }
        //-----------------------------------------------------
        public EVariableType GetVariableType()
        {
            return EVariableType.eString;
        }
        //-----------------------------------------------------
        public short GetGuid() { return guid; }
    }
    //-----------------------------------------------------
    [System.Serializable]
    public struct VariableBool : IVariable
    {
        public short guid;
        public bool value;
        //-----------------------------------------------------
        public VariableBool(bool value)
        {
            guid = 0;
            this.value = value;
        }
        //-----------------------------------------------------
        public EVariableType GetVariableType()
        {
            return EVariableType.eBool;
        }
        //-----------------------------------------------------
        public short GetGuid() { return guid; }
    }
    //-----------------------------------------------------
    [System.Serializable]
    public struct VariableRay : IVariable
    {
        public short guid;
        public FRay value;
        //-----------------------------------------------------
        public VariableRay(FRay value)
        {
            guid = 0;
            this.value = value;
        }
        //-----------------------------------------------------
        public EVariableType GetVariableType()
        {
            return EVariableType.eRay;
        }
        //-----------------------------------------------------
        public short GetGuid() { return guid; }
    }
    //-----------------------------------------------------
    [System.Serializable]
    public struct VariableQuaternion : IVariable
    {
        public short guid;
        public FQuaternion value;
        //-----------------------------------------------------
        public VariableQuaternion(FQuaternion value)
        {
            guid = 0;
            this.value = value;
        }
        //-----------------------------------------------------
        public EVariableType GetVariableType()
        {
            return EVariableType.eQuaternion;
        }
        //-----------------------------------------------------
        public short GetGuid() { return guid; }
    }
    //-----------------------------------------------------
    [System.Serializable]
    public struct VariableBounds : IVariable
    {
        public short guid;
        public FBounds value;
        //-----------------------------------------------------
        public VariableBounds(FBounds value)
        {
            guid = 0;
            this.value = value;
        }
        //-----------------------------------------------------
        public EVariableType GetVariableType()
        {
            return EVariableType.eBounds;
        }
        //-----------------------------------------------------
        public short GetGuid() { return guid; }
    }
    //-----------------------------------------------------
    [System.Serializable]
    public struct VariableRect : IVariable
    {
        public short guid;
        public FRect value;
        //-----------------------------------------------------
        public VariableRect(FRect value)
        {
            guid = 0;
            this.value = value;
        }
        //-----------------------------------------------------
        public EVariableType GetVariableType()
        {
            return EVariableType.eRect;
        }
        //-----------------------------------------------------
        public short GetGuid() { return guid; }
    }
    //-----------------------------------------------------
    [System.Serializable]
    public struct VariableMatrix : IVariable
    {
        public short guid;
        public FMatrix4x4 value;
        //-----------------------------------------------------
        public VariableMatrix(FMatrix4x4 value)
        {
            guid = 0;
            this.value = value;
        }
        //-----------------------------------------------------
        public EVariableType GetVariableType()
        {
            return EVariableType.eMatrix;
        }
        //-----------------------------------------------------
        public short GetGuid() { return guid; }
    }
    //-----------------------------------------------------
    [System.Serializable]
    public struct VariableUserData : IVariable
    {
        public short guid;
        public int value;
        [System.NonSerialized]
        public IUserData pPointer;
        //-----------------------------------------------------
        public VariableUserData(int value)
        {
            guid = 0;
            this.pPointer = null;
            this.value = value;
        }
        //-----------------------------------------------------
        public EVariableType GetVariableType()
        {
            return EVariableType.eUserData;
        }
        //-----------------------------------------------------
        public short GetGuid() { return guid; }

        public static VariableUserData DEF = new VariableUserData() { guid =0, value =0, pPointer = null };
        //-----------------------------------------------------
        public bool IsEqual(VariableUserData other)
        {
            return this.value == other.value && this.pPointer == other.pPointer;
        }
    }
    //-----------------------------------------------------
    [System.Serializable]
    public struct VariableObjId : IVariable
    {
        public short guid;
        public ObjId value;
        //-----------------------------------------------------
        public VariableObjId(ObjId value)
        {
            guid = 0;
            this.value = value;
        }
        //-----------------------------------------------------
        public EVariableType GetVariableType()
        {
            return EVariableType.eObjId;
        }
        //-----------------------------------------------------
        public short GetGuid() { return guid; }
    }
    //-----------------------------------------------------
    [System.Serializable]
    struct VaribaleSerizlizeData
    {
        [System.Serializable]
        public struct Item
        {
            public byte type;
            public string value;
        }
        public Item[] items;
    }
    //-----------------------------------------------------
    [System.Serializable]
    public class Variables
    {
        [UnityEngine.SerializeField]string serializeData;
        [System.NonSerialized]public VariableList variables;
        //-----------------------------------------------------
        public int GetVarCount()
        {
            if (variables == null) return 0;
            return variables.GetVarCount();
        }
        //-----------------------------------------------------
        public EVariableType GetVarType(int index)
        {
            if (variables == null) return EVariableType.eNone;
            return variables.GetVarType(index);
        }
        //-----------------------------------------------------
        public bool GetBool(int index, bool defVal =false)
        {
            if (variables == null) return defVal;
            return variables.GetBool(index, defVal);
        }
        //-----------------------------------------------------
        public int GetInt(int index, int defVal = 0)
        {
            if (variables == null) return defVal;
            return variables.GetInt(index, defVal);
        }
        //-----------------------------------------------------
        public float GetFloat(int index, float defVal = 0)
        {
            if (variables == null) return defVal;
            return variables.GetFloat(index, defVal);
        }
        //-----------------------------------------------------
        public ObjId GetObjId(int index)
        {
            if (variables == null) return ObjId.DEF;
            return variables.GetObjId(index);
        }
        //-----------------------------------------------------
        public FVector2 GetVec2(int index)
        {
            if (variables == null) return FVector2.zero;
            return variables.GetVec2(index);
        }
        //-----------------------------------------------------
        public FVector3 GetVec3(int index)
        {
            if (variables == null) return FVector3.zero;
            return variables.GetVec3(index);
        }
        //-----------------------------------------------------
        public FVector4 GetVec4(int index)
        {
            if (variables == null) return FVector4.zero;
            return variables.GetVec4(index);
        }
        //-----------------------------------------------------
        public string GetString(int index, string defValue = null)
        {
            if (variables == null) return defValue;
            return variables.GetString(index, defValue);
        }
        //-----------------------------------------------------
        public bool Deserialize()
        {
            if (string.IsNullOrEmpty(serializeData))
                return false;
            VaribaleSerizlizeData data = UnityEngine.JsonUtility.FromJson<VaribaleSerizlizeData>(serializeData);
            if (data.items != null)
            {
                variables = VariableList.Malloc(data.items.Length);
                for (int i = 0; i < data.items.Length; ++i)
                {
                    var item = data.items[i];
                    switch ((EVariableType)item.type)
                    {
                        case EVariableType.eInt:
                            {
                                int v = 0;
                                int.TryParse(item.value, out v);
                                variables.AddInt(v);
                            }
                            break;
                        case EVariableType.eFloat:
                            {
                                float v = 0;
                                float.TryParse(item.value, out v);
                                variables.AddFloat(v);
                            }
                            break;
                        case EVariableType.eString:
                            {
                                variables.AddString(item.value);
                            }
                            break;
                        case EVariableType.eBool:
                            {
                                bool v = false;
                                bool.TryParse(item.value, out v);
                                variables.AddBool(v);
                            }
                            break;
                        case EVariableType.eVec2:
                            {
                                var splitVal = item.value.Split('|');
                                if(splitVal.Length >=2)
                                {
                                    float x = 0, y = 0;
                                    float.TryParse(splitVal[0], out x);
                                    float.TryParse(splitVal[1], out y);
                                    variables.AddVec2(new Vector2(x, y));
                                }
                            }
                            break;
                        case EVariableType.eVec3:
                            {
                                var splitVal = item.value.Split('|');
                                if (splitVal.Length >= 3)
                                {
                                    float x = 0, y = 0, z =0;
                                    float.TryParse(splitVal[0], out x);
                                    float.TryParse(splitVal[1], out y);
                                    float.TryParse(splitVal[2], out z);
                                    variables.AddVec3(new Vector3(x, y, z));
                                }
                            }
                            break;
                            case EVariableType.eVec4:
                            {
                                var splitVal = item.value.Split('|');
                                if (splitVal.Length >= 4)
                                {
                                    float x = 0, y = 0, z = 0, w = 0;
                                    float.TryParse(splitVal[0], out x);
                                    float.TryParse(splitVal[1], out y);
                                    float.TryParse(splitVal[2], out z);
                                    float.TryParse(splitVal[3], out w);
                                    variables.AddVec4(new Vector4(x, y, z,w));
                                }
                            }
                            break;
                        case EVariableType.eObjId:
                            {
                                int v = 0;
                                int.TryParse(item.value, out v);
                                ObjId obj = new ObjId();
                                JsonUtility.FromJsonOverwrite(item.value, obj);
                                variables.AddObjId(obj);
                            }
                            break;
                        case EVariableType.eLong:
                            {
                                long v = 0;
                                long.TryParse(item.value, out v);
                                variables.AddLong(v);
                            }
                            break;
                        case EVariableType.eDouble:
                            {
                                double v = 0;
                                double.TryParse(item.value, out v);
                                variables.AddDouble(v);
                            }
                            break;
                        case EVariableType.eColor:
                            {
                                Color v = Color.white;
                                ColorUtility.TryParseHtmlString(item.value, out v);
                                variables.AddColor(v);
                            }
                            break;
                        case EVariableType.eRay:
                            {
                                var splitVal = item.value.Split('|');
                                if (splitVal.Length == 6)
                                {
                                    Ray ray = new Ray();
                                    float x = 0, y = 0, z = 0, dx = 0,dy=0,dz=0;
                                    float.TryParse(splitVal[0], out x);
                                    float.TryParse(splitVal[1], out y);
                                    float.TryParse(splitVal[2], out z);
                                    float.TryParse(splitVal[3], out dx);
                                    float.TryParse(splitVal[4], out dy);
                                    float.TryParse(splitVal[5], out dz);
                                    ray.origin = new Vector3(x, y, z);
                                    ray.direction = new Vector3(dx,dy,dz);
                                    variables.AddRay(ray);
                                }
                            }
                            break;
                        case EVariableType.eQuaternion:
                            {
                                var splitVal = item.value.Split('|');
                                if (splitVal.Length == 4)
                                {
                                    Quaternion rot = Quaternion.identity;
                                    float.TryParse(splitVal[0], out rot.x);
                                    float.TryParse(splitVal[1], out rot.y);
                                    float.TryParse(splitVal[2], out rot.z);
                                    float.TryParse(splitVal[3], out rot.w);
                                    variables.AddQuaternion(rot);
                                }
                            }
                            break;
                        case EVariableType.eRect:
                            {
                                var splitVal = item.value.Split('|');
                                if (splitVal.Length == 4)
                                {
                                    Rect rot = Rect.zero;
                                    float x, y, z, w;
                                    float.TryParse(splitVal[0], out x);
                                    float.TryParse(splitVal[1], out y);
                                    float.TryParse(splitVal[2], out z);
                                    float.TryParse(splitVal[3], out w);
                                    rot.x = x;
                                    rot.y = y;
                                    rot.width = z;
                                    rot.height = w;
                                    variables.AddRect(rot);
                                }
                            }
                            break;
                        case EVariableType.eBounds:
                            {
                                var splitVal = item.value.Split('|');
                                if (splitVal.Length == 6)
                                {
                                    float x0, y0, z0;
                                    float x1, y1, z1;
                                    float.TryParse(splitVal[0], out x0);
                                    float.TryParse(splitVal[1], out y0);
                                    float.TryParse(splitVal[2], out z0);
                                    float.TryParse(splitVal[3], out x1);
                                    float.TryParse(splitVal[4], out y1);
                                    float.TryParse(splitVal[5], out z1);
                                    variables.AddBounds(new Bounds(new Vector3(x0,y0,z0), new Vector3(x1,y1,z1)));
                                }
                            }
                            break;
                        case EVariableType.eMatrix:
                            {
                                var splitVal = item.value.Split('|');
                                if (splitVal.Length == 16)
                                {
                                    Matrix4x4 mat = Matrix4x4.identity;
                                    float.TryParse(splitVal[0], out mat.m00);
                                    float.TryParse(splitVal[1], out mat.m10);
                                    float.TryParse(splitVal[2], out mat.m20);
                                    float.TryParse(splitVal[3], out mat.m30);

                                    float.TryParse(splitVal[0], out mat.m01);
                                    float.TryParse(splitVal[1], out mat.m11);
                                    float.TryParse(splitVal[2], out mat.m21);
                                    float.TryParse(splitVal[3], out mat.m31);

                                    float.TryParse(splitVal[0], out mat.m02);
                                    float.TryParse(splitVal[1], out mat.m12);
                                    float.TryParse(splitVal[2], out mat.m22);
                                    float.TryParse(splitVal[3], out mat.m32);

                                    float.TryParse(splitVal[0], out mat.m03);
                                    float.TryParse(splitVal[1], out mat.m13);
                                    float.TryParse(splitVal[2], out mat.m23);
                                    float.TryParse(splitVal[3], out mat.m33);
                                    variables.AddMatrix(mat);
                                }
                            }
                            break;
                        case EVariableType.eUserData:
                            {
                                int v = 0;
                                int.TryParse(item.value, out v);
                                variables.AddUserData(new VariableUserData() { value = v });
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            else
                variables = null;
                return true;
        }
        //-----------------------------------------------------
        public void Serialize()
        {
#if UNITY_EDITOR
            VaribaleSerizlizeData data = new VaribaleSerizlizeData();
            if (variables != null)
            {
                data.items = new VaribaleSerizlizeData.Item[variables.GetVarCount()];
                for (int i = 0; i < data.items.Length; ++i)
                {
                    var type = variables.GetVarType(i);
                    var item = new VaribaleSerizlizeData.Item();
                    item.type = (byte)type;
                    switch (type)
                    {
                        case EVariableType.eInt:
                            item.value = variables.GetInt(i).ToString();
                            break;
                        case EVariableType.eFloat:
                            item.value = variables.GetFloat(i).ToString("F3");
                            break;
                        case EVariableType.eString:
                            item.value = variables.GetString(i);
                            break;
                        case EVariableType.eBool:
                            item.value = variables.GetBool(i).ToString();
                            break;
                        case EVariableType.eVec2:
                            {
                                var v = variables.GetVec2(i);
                                item.value = $"{v.x.ToString("F3")}|{v.y.ToString("F3")}";
                            }
                            break;
                        case EVariableType.eVec3:
                            {
                                var v = variables.GetVec3(i);
                                item.value = $"{v.x.ToString("F3")}|{v.y.ToString("F3")}|{v.z.ToString("F3")}";
                            }
                            break;
                        case EVariableType.eVec4:
                            {
                                var v = variables.GetVec4(i);
                                item.value = $"{v.x.ToString("F3")}|{v.y.ToString("F3")}|{v.z.ToString("F3")}|{v.w.ToString("F3")}";
                            }
                            break;
                        case EVariableType.eObjId:
                            {
                                var v = variables.GetObjId(i);
                                item.value = JsonUtility.ToJson(v);
                            }
                            break;
                        case EVariableType.eLong:
                            item.value = variables.GetLongs()[i].ToString();
                            break;
                        case EVariableType.eDouble:
                            item.value = variables.GetDoubles()[i].ToString("F6");
                            break;
                        case EVariableType.eColor:
                            {
                                var c = variables.GetColors()[i];
                                item.value = $"#{ColorUtility.ToHtmlStringRGBA(c)}";
                            }
                            break;
                        case EVariableType.eRay:
                            {
                                var r = variables.GetRays()[i];
                                item.value = $"{r.origin.x.ToString("F3")}|{r.origin.y.ToString("F3")}|{r.origin.z.ToString("F3")}|{r.direction.x.ToString("F3")}|{r.direction.y.ToString("F3")}|{r.direction.z.ToString("F3")}";
                            }
                            break;
                        case EVariableType.eQuaternion:
                            {
                                var q = variables.GetQuaternions()[i];
                                item.value = $"{q.x.ToString("F3")}|{q.y.ToString("F3")}|{q.z.ToString("F3")}|{q.w.ToString("F3")}";
                            }
                            break;
                        case EVariableType.eRect:
                            {
                                var rect = variables.GetRects()[i];
                                item.value = $"{rect.x.ToString("F3")}|{rect.y.ToString("F3")}|{rect.width.ToString("F3")}|{rect.height.ToString("F3")}";
                            }
                            break;
                        case EVariableType.eBounds:
                            {
                                var b = variables.GetBoundsList()[i];
                                item.value = $"{b.center.x.ToString("F3")}|{b.center.y.ToString("F3")}|{b.center.z.ToString("F3")}|{b.size.x.ToString("F3")}|{b.size.y.ToString("F3")}|{b.size.z.ToString("F3")}";
                            }
                            break;
                        case EVariableType.eMatrix:
                            {
                                var m = variables.GetMatrixs()[i];
                                item.value =
                                    $"{m.m00.ToString("F3")}|{m.m10.ToString("F3")}|{m.m20.ToString("F3")}|{m.m30.ToString("F3")}|"
                                    + $"{m.m01.ToString("F3")}|{m.m11.ToString("F3")}|{m.m21.ToString("F3")}|{m.m31.ToString("F3")}|"
                                    + $"{m.m02.ToString("F3")}|{m.m12.ToString("F3")}|{m.m22.ToString("F3")}|{m.m32.ToString("F3")}|"
                                    + $"{m.m03.ToString("F3")}|{m.m13.ToString("F3")}|{m.m23.ToString("F3")}|{m.m33.ToString("F3")}";
                            }
                            break;
                        case EVariableType.eUserData:
                            item.value = variables.GetUserDatas()[i].value.ToString();
                            break;
                    }
                    data.items[i] = item;
                }
            }
            else
            {
                data.items = new VaribaleSerizlizeData.Item[0];
            }
            serializeData = UnityEngine.JsonUtility.ToJson(data);
#else
            Debug.LogError("un support out editor");
#endif
        }
    }
    //-----------------------------------------------------
    public static class VariableUtil
    {
        public static IVariable CreateVariable(EVariableType type, short guid)
        {
            switch (type)
            {
                case EVariableType.eInt:
                    return new VariableInt { value = 0, guid = guid };
                case EVariableType.eFloat:
                    return new VariableFloat { value = 0f, guid = guid };
                case EVariableType.eString:
                    return new VariableString { value = string.Empty, guid = guid };
                case EVariableType.eBool:
                    return new VariableBool { value = false, guid = guid };
                case EVariableType.eLong:
                    return new VariableLong { value = 0, guid = guid };
                case EVariableType.eDouble:
                    return new VariableDouble { value = 0, guid = guid };
                case EVariableType.eVec2:
                    return new VariableVec2 { value = Vector2.zero, guid = guid };
                case EVariableType.eVec3:
                    return new VariableVec3 { value = Vector3.zero, guid = guid };
                case EVariableType.eVec4:
                    return new VariableVec4 { value = Vector4.zero, guid = guid };
                case EVariableType.eObjId:
                    return new VariableObjId { value = new ObjId(), guid = guid };
                case EVariableType.eColor:
                    return new VariableColor { value = Vector4.zero, guid = guid };
                case EVariableType.eRay:
                    return new VariableRay { value = new Ray(Vector3.zero, Vector3.forward), guid = guid };
                case EVariableType.eQuaternion:
                    return new VariableQuaternion { value = Quaternion.identity, guid = guid };
                case EVariableType.eRect:
                    return new VariableRect { value = new Rect(0, 0, 0, 0), guid = guid };
                case EVariableType.eBounds:
                    return new VariableBounds { value = new Bounds(Vector3.zero, Vector3.zero), guid = guid };
                case EVariableType.eMatrix:
                    return new VariableMatrix { value = Matrix4x4.identity, guid = guid };
                case EVariableType.eUserData:
                    return new VariableUserData { value = 0, pPointer = null, guid = guid };
                default:
                    return null;
            }
        }
        //-----------------------------------------------------
        public static IVariable CreateVariable<T>(T value, short guid = 0)
        {
            if (value is byte byVal)
                return new VariableInt { value = byVal, guid = guid };
            if (value is short sVal)
                return new VariableInt { value = sVal, guid = guid };
            if (value is ushort usVal)
                return new VariableInt { value = usVal, guid = guid };
            if (value is int intVal)
                return new VariableInt { value = intVal, guid = guid };
            if (value is long longVal)
                return new VariableLong { value = longVal, guid = guid };
            if (value is double doubleVal)
                return new VariableDouble { value = (long)doubleVal, guid = guid };
            if (value is float floatVal)
                return new VariableFloat { value = floatVal, guid = guid };
            if (value is string strVal)
                return new VariableString { value = strVal, guid = guid };
            if (value is bool boolVal)
                return new VariableBool { value = boolVal, guid = guid };
            if (value is Vector2 v2Val)
                return new VariableVec2 { value = v2Val, guid = guid };
            if (value is Vector3 v3Val)
                return new VariableVec3 { value = v3Val, guid = guid };
            if (value is Vector4 v4Val)
                return new VariableVec4 { value = v4Val, guid = guid };
            if (value is ObjId objId)
                return new VariableObjId { value = objId, guid = guid };
            if (value is Color colorVal)
                return new VariableColor { value = new Vector4(colorVal.r, colorVal.g, colorVal.b, colorVal.a), guid = guid };
            if (value is Ray rayVal)
                return new VariableRay { value = rayVal, guid = guid };
            if (value is Quaternion quatVal)
                return new VariableQuaternion { value = quatVal, guid = guid };
            if (value is Rect rectVal)
                return new VariableRect { value = rectVal, guid = guid };
            if (value is Bounds boundsVal)
                return new VariableBounds { value = boundsVal, guid = guid };
            if (value is Matrix4x4 matVal)
                return new VariableMatrix { value = matVal, guid = guid };
            if (value is VariableUserData userDataVal)
                return new VariableUserData { value = userDataVal.value, pPointer = userDataVal.pPointer, guid = guid };
            return null;
        }
        //-----------------------------------------------------
#if UNITY_EDITOR
        internal static Type GetVariableCsType(EVariableType type)
        {
            switch (type)
            {
                case EVariableType.eBool: return typeof(bool);
                case EVariableType.eInt: return typeof(int);
                case EVariableType.eFloat: return typeof(float);
                case EVariableType.eString: return typeof(string);
                case EVariableType.eLong: return typeof(long);
                case EVariableType.eDouble: return typeof(double);
                case EVariableType.eVec2: return typeof(Vector2);
                case EVariableType.eVec3: return typeof(Vector3);
                case EVariableType.eVec4: return typeof(Vector4);
                case EVariableType.eObjId: return typeof(ObjId);
                case EVariableType.eColor: return typeof(Color);
                case EVariableType.eRay: return typeof(Ray);
                case EVariableType.eQuaternion: return typeof(Quaternion);
                case EVariableType.eRect: return typeof(Rect);
                case EVariableType.eBounds: return typeof(Bounds);
                case EVariableType.eMatrix: return typeof(Matrix4x4);
                case EVariableType.eUserData: return typeof(VariableUserData);
                default: return null;
            }
        }
        //-----------------------------------------------------
        internal static IVariable CreateVariable(ArgvAttribute attri, short guid)
        {
            if(attri.argvType.GetInterface(typeof(IVariable).FullName.Replace("+", "."))!=null)
            {
                IVariable pVar =(IVariable)Activator.CreateInstance(attri.argvType);
                var guideField = attri.argvType.GetField("guid", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                if (guideField != null)
                    guideField.SetValue(pVar, guid);
                if(pVar is VariableUserData)
                {
                    VariableUserData pUserData = (VariableUserData)pVar;
                    pUserData.value = ATRtti.GetClassTypeId(attri.argvType);
                    return pUserData;
                }
                return pVar;
            }
            if (attri.argvType == typeof(bool)) return new VariableBool { value = attri.ToValue<bool>(false), guid = guid };
            else if (attri.argvType == typeof(byte) ||
                attri.argvType == typeof(char) ||
                attri.argvType == typeof(sbyte) ||
                attri.argvType.IsEnum ||
                attri.argvType == typeof(short) ||
                attri.argvType == typeof(ushort) ||
                attri.argvType == typeof(int) ||
                attri.argvType == typeof(uint))
            {
                return new VariableInt { value = attri.ToValue<int>(0), guid = guid };
            }
            else if (attri.argvType.IsEnum)
            {
                if (attri.defValue != null)
                {
                    if (Enum.TryParse(attri.argvType, attri.defValue.ToString(), true, out object enumVal))
                    {
                        return new VariableInt { value = Convert.ToInt32(enumVal), guid = guid };
                    }
                    else
                    {
                        return new VariableInt { value = attri.ToValue<int>(0), guid = guid };
                    }
                }
                else
                    return new VariableInt { value = attri.ToValue<int>(0), guid = guid };
            }
            else if (attri.argvType == typeof(long))
                return new VariableLong { value = attri.ToValue<long>(0), guid = guid };
            else if (attri.argvType == typeof(double))
                return new VariableDouble { value = (long)attri.ToValue<double>(0), guid = guid };
            else if (attri.argvType == typeof(float))
                return new VariableFloat { value = attri.ToValue<float>(0), guid = guid };
            else if (attri.argvType == typeof(string))
                return new VariableString { value = attri.defValue != null ? attri.defValue.ToString() : "", guid = guid };
            else if (attri.argvType == typeof(Vector2))
                return new VariableVec2 { value = attri.ToValue<Vector2>(Vector2.zero), guid = guid };
            else if (attri.argvType == typeof(Vector3))
                return new VariableVec3 { value = attri.ToValue<Vector3>(Vector3.zero), guid = guid };
            else if (attri.argvType == typeof(Vector4))
                return new VariableVec4 { value = attri.ToValue<Vector4>(Vector4.zero), guid = guid };
            else if (attri.argvType == typeof(ObjId))
                return new VariableObjId { value = new ObjId(), guid = guid };
            else if (attri.argvType == typeof(Color))
                return new VariableColor { value = attri.ToValue<Vector4>(Vector4.zero), guid = guid };
            else if (attri.argvType == typeof(Ray))
                return new VariableRay { value = attri.ToValue<Ray>(new Ray(Vector3.zero, Vector3.forward)), guid = guid };
            else if (attri.argvType == typeof(Quaternion))
                return new VariableQuaternion { value = attri.ToValue<Quaternion>(Quaternion.identity), guid = guid };
            else if (attri.argvType == typeof(Rect))
                return new VariableRect { value = attri.ToValue<Rect>(new Rect(0, 0, 0, 0)), guid = guid };
            else if (attri.argvType == typeof(Bounds))
                return new VariableBounds { value = attri.ToValue<Bounds>(new Bounds(Vector3.zero, Vector3.zero)), guid = guid };
            else if (attri.argvType == typeof(Matrix4x4))
                return new VariableMatrix { value = attri.ToValue<Matrix4x4>(Matrix4x4.identity), guid = guid };
            else if (attri.argvType == typeof(VariableUserData) || attri.argvType.GetInterfaces().Contains(typeof(IUserData)))
            {
                var pVar = new VariableUserData { value = attri.ToValue<int>(0), pPointer = null, guid = guid };
                pVar.value = ATRtti.GetClassTypeId(attri.argvType);
                return pVar;
            }
            else if (attri.argvType == typeof(IVariable))
                return new VariableInt { value = 0, guid = guid };
            else if (attri.argvType == typeof(BaseNode))
                return new VariableInt { value = 0, guid = guid };

            UnityEngine.Debug.LogError($"Unsupported variable type: {attri.argvType}");
            return null;
        }
#endif
    }
}