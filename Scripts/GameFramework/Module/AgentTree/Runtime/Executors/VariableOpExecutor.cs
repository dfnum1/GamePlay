/********************************************************************
生成日期:	07:03:2025
类    名: 	VarOpExecutor
作    者:	HappLI
描    述:	+运算符执行器
*********************************************************************/
using UnityEngine;
namespace Framework.AT.Runtime
{
    internal class VarOpExecutor
    {
        //------------------------------------------------------
        public static bool OnExecute(AgentTree pAgent, BaseNode pNode)
        {
            int inportCnt = pNode.GetInportCount();
            int outportCnt = pNode.GetOutportCount();
            if (inportCnt != 3 || outportCnt <= 0)
            {
                Debug.LogError("condtion executor inport or outport count error!");
                return false;
            }
            var inputVarType = pAgent.GetInportVarType(pNode, 0);
            if (inputVarType == EVariableType.eNone)
            {
                Debug.LogError("condtion input var type error!");
                return false;
            }
            var conditionVarType = pAgent.GetInportVarType(pNode, 1);
            if (conditionVarType != EVariableType.eInt)
            {
                Debug.LogError("condtion op type error!");
                return false;
            }
            var opVarType = pAgent.GetInportVarType(pNode, 2);
            if (opVarType != inputVarType)
            {
                Debug.LogError("condtion op type type var error!");
                return false;
            }
            EOpType opType = (EOpType)pAgent.GetInportInt(pNode, 1, 0);

            switch (inputVarType)
            {
                case EVariableType.eInt:
                    {
                        int a = pAgent.GetInportInt(pNode, 0);
                        int b = pAgent.GetInportInt(pNode, 2);
                        int result = 0;
                        switch (opType)
                        {
                            case EOpType.eAdd: result = a + b; break;
                            case EOpType.eSub: result = a - b; break;
                            case EOpType.eMul: result = a * b; break;
                            case EOpType.eDiv: result = b == 0 ? 0 : a / b; break;
                            default: Debug.LogError($"Unsupported {opType}  for int"); return false;
                        }
                        pAgent.SetOutportInt(pNode, 0, result);
                        return true;
                    }
                case EVariableType.eLong:
                    {
                        var a = pAgent.GetInportLong(pNode, 0);
                        var b = pAgent.GetInportLong(pNode, 2);
                        long result = 0;
                        switch (opType)
                        {
                            case EOpType.eAdd: result = a + b; break;
                            case EOpType.eSub: result = a - b; break;
                            case EOpType.eMul: result = a * b; break;
                            case EOpType.eDiv: result = b == 0 ? 0 : a / b; break;
                            default: Debug.LogError($"Unsupported {opType}  for Long"); return false;
                        }
                        pAgent.SetOutportLong(pNode, 0, result);
                        return true;
                    }
                case EVariableType.eFloat:
                    {
                        float a = pAgent.GetInportFloat(pNode, 0);
                        float b = pAgent.GetInportFloat(pNode, 2);
                        float result = 0f;
                        switch (opType)
                        {
                            case EOpType.eAdd: result = a + b; break;
                            case EOpType.eSub: result = a - b; break;
                            case EOpType.eMul: result = a * b; break;
                            case EOpType.eDiv: result = Mathf.Approximately(b, 0f) ? 0f : a / b; break;
                            default: Debug.LogError($"Unsupported {opType}  for float"); return false;
                        }
                        pAgent.SetOutportFloat(pNode, 0, result);
                        return true;
                    }
                case EVariableType.eDouble:
                    {
                        var a = pAgent.GetInportDouble(pNode, 0);
                        var b = pAgent.GetInportDouble(pNode, 2);
                        double result = 0;
                        switch (opType)
                        {
                            case EOpType.eAdd: result = a + b; break;
                            case EOpType.eSub: result = a - b; break;
                            case EOpType.eMul: result = a * b; break;
                            case EOpType.eDiv: result = b != 0 ? 0f : a / b; break;
                            default: Debug.LogError($"Unsupported {opType}  for Double"); return false;
                        }
                        pAgent.SetOutportDouble(pNode, 0, result);
                        return true;
                    }
                case EVariableType.eBool:
                    {
                        Debug.LogError($"Unsupported {opType}  for bool"); return false;
                        return true;
                    }
                case EVariableType.eVec2:
                    {
                        Vector2 a = pAgent.GetInportVec2(pNode, 0);
                        Vector2 b = pAgent.GetInportVec2(pNode, 2);
                        Vector2 result = Vector2.zero;
                        switch (opType)
                        {
                            case EOpType.eAdd: result = a + b; break;
                            case EOpType.eSub: result = a - b; break;
                            case EOpType.eMul: result = Vector2.Scale(a, b); break;
                            case EOpType.eDiv:
                                result = new Vector2(
                                    Mathf.Approximately(b.x, 0f) ? 0f : a.x / b.x,
                                    Mathf.Approximately(b.y, 0f) ? 0f : a.y / b.y
                                );
                                break;
                            default: Debug.LogError($"Unsupported {opType}  for Vector2"); return false;
                        }
                        pAgent.SetOutportVec2(pNode, 0, result);
                        return true;
                    }
                case EVariableType.eVec3:
                    {
                        Vector3 a = pAgent.GetInportVec3(pNode, 0);
                        Vector3 b = pAgent.GetInportVec3(pNode, 2);
                        Vector3 result = Vector3.zero;
                        switch (opType)
                        {
                            case EOpType.eAdd: result = a + b; break;
                            case EOpType.eSub: result = a - b; break;
                            case EOpType.eMul: result = Vector3.Scale(a, b); break;
                            case EOpType.eDiv:
                                result = new Vector3(
                                    Mathf.Approximately(b.x, 0f) ? 0f : a.x / b.x,
                                    Mathf.Approximately(b.y, 0f) ? 0f : a.y / b.y,
                                    Mathf.Approximately(b.z, 0f) ? 0f : a.z / b.z
                                );
                                break;
                            default: Debug.LogError($"Unsupported {opType}  for Vector3"); return false;
                        }
                        pAgent.SetOutportVec3(pNode, 0, result);
                        return true;
                    }
                case EVariableType.eVec4:
                    {
                        Vector4 a = pAgent.GetInportVec4(pNode, 0);
                        Vector4 b = pAgent.GetInportVec4(pNode, 2);
                        Vector4 result = Vector4.zero;
                        switch (opType)
                        {
                            case EOpType.eAdd: result = a + b; break;
                            case EOpType.eSub: result = a - b; break;
                            case EOpType.eMul: result = Vector4.Scale(a, b); break;
                            case EOpType.eDiv:
                                result = new Vector4(
                                    Mathf.Approximately(b.x, 0f) ? 0f : a.x / b.x,
                                    Mathf.Approximately(b.y, 0f) ? 0f : a.y / b.y,
                                    Mathf.Approximately(b.z, 0f) ? 0f : a.z / b.z,
                                    Mathf.Approximately(b.w, 0f) ? 0f : a.w / b.w
                                );
                                break;
                            default: Debug.LogError($"Unsupported {opType}  for Vector4"); return false;
                        }
                        pAgent.SetOutportVec4(pNode, 0, result);
                        return true;
                    }
                case EVariableType.eString:
                    {
                        string a = pAgent.GetInportString(pNode, 0);
                        string b = pAgent.GetInportString(pNode, 2);
                        string result = "";
                        if (opType == EOpType.eAdd)
                            result = a + b;
                        else
                        {
                            Debug.LogError("String only supports Add (concat) operation.");
                            return false;
                        }
                        pAgent.SetOutportString(pNode, 0, result);
                        return true;
                    }
                case EVariableType.eColor:
                    {
                        var a = pAgent.GetInportColor(pNode, 0);
                        var b = pAgent.GetInportColor(pNode, 2);
                        Color result = Color.white;
                        switch (opType)
                        {
                            case EOpType.eAdd: result = a + b; break;
                            case EOpType.eSub: result = a - b; break;
                            case EOpType.eMul: result = a * b; break;
                            case EOpType.eDiv:
                                result = new Color(
                                    Mathf.Approximately(b.r, 0f) ? 0f : a.r / b.r,
                                    Mathf.Approximately(b.g, 0f) ? 0f : a.g / b.g,
                                    Mathf.Approximately(b.b, 0f) ? 0f : a.b / b.b,
                                    Mathf.Approximately(b.a, 0f) ? 0f : a.a / b.a
                                );
                                break;
                            default: Debug.LogError($"Unsupported {opType}  for Color"); return false;
                        }
                        pAgent.SetOutportColor(pNode, 0, result);
                        return true;
                    }
                case EVariableType.eRay:
                    {
                        Debug.LogError($"Unsupported {opType}  for Color"); return false;
                    }
                case EVariableType.eQuaternion:
                    {
                        var a = pAgent.GetInportQuaternion(pNode, 0);
                        var b = pAgent.GetInportQuaternion(pNode, 2);
                        Quaternion result = Quaternion.identity;
                        switch (opType)
                        {
                            case EOpType.eMul: result = a * b; break;
                            default: Debug.LogError($"Unsupported {opType} for Quaternion"); return false;
                        }
                        pAgent.SetOutportQuaternion(pNode, 0, result);
                        return true;
                    }
                case EVariableType.eRect:
                    {
                        var a = pAgent.GetInportRect(pNode, 0);
                        var b = pAgent.GetInportRect(pNode, 2);
                        Rect result = Rect.zero;
                        switch (opType)
                        {
                            case EOpType.eAdd: result.min = Vector3.Min(a.min, b.min); result.max = Vector3.Max(a.max, b.max); break;
                            default: Debug.LogError($"Unsupported {opType} for Quaternion"); return false;
                        }
                        pAgent.SetOutportRect(pNode, 0, result);
                        return true;
                    }
                case EVariableType.eBounds:
                    {
                        Debug.LogError($"Unsupported {opType} for Quaternion"); return false;
                    }
                case EVariableType.eMatrix:
                    {
                        var a = pAgent.GetInportMatrix(pNode, 0);
                        var b = pAgent.GetInportMatrix(pNode, 2);
                        Matrix4x4 result = Matrix4x4.identity;
                        switch (opType)
                        {
                            case EOpType.eMul: result = a * b; break;
                            default: Debug.LogError($"Unsupported {opType}  for Matrix4x4"); return false;
                        }
                        pAgent.SetOutportMatrix(pNode, 0, result);
                        return true;
                    }
                default:
                    Debug.LogError("Unsupported variable type for operation.");
                    return false;

            }
        }
    }
}