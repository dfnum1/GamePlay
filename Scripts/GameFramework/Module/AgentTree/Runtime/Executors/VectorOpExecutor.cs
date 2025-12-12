/********************************************************************
生成日期:	07:03:2025
类    名: 	VectorOpExecutor
作    者:	HappLI
描    述:	向量运算执行器
*********************************************************************/
using UnityEngine;
namespace Framework.AT.Runtime
{
    internal class VectorOpExecutor
    {
        public static bool OnExecutor(AgentTree pAgent, BaseNode pNode)
        {
            int inportCnt = pNode.GetInportCount();
            int outportCnt = pNode.GetOutportCount();
            if (inportCnt < 1 || outportCnt <= 0)
            {
                Debug.LogError("condtion executor inport or outport count error!");
                return false;
            }
            var varType1 = pAgent.GetInportVarType(pNode, 0);
            if (varType1 == EVariableType.eNone)
            {
                Debug.LogError("condtion input var type error!");
                return false;
            }
            var varType2 = EVariableType.eNone;
            if(inportCnt>1) varType2 = pAgent.GetInportVarType(pNode, 1);
            var outputType = pAgent.GetOutportVarType(pNode, 0);
            switch ((EActionType)pNode.type)
            {
                case EActionType.eDotVariable:
                    {
                        if (varType2 != varType1 || outputType!= EVariableType.eFloat)
                        {
                            Debug.LogError("condtion op type error!");
                            return false;
                        }
                        switch (varType1)
                        {
                            case EVariableType.eVec2:
                                {
                                    Vector2 a = pAgent.GetInportVec2(pNode, 0);
                                    Vector2 b = pAgent.GetInportVec2(pNode, 1);
                                    float result = Vector2.Dot(a, b);
                                    pAgent.SetOutportFloat(pNode, 0, result);
                                    return true;
                                }
                            case EVariableType.eVec3:
                                {
                                    Vector3 a = pAgent.GetInportVec3(pNode, 0);
                                    Vector3 b = pAgent.GetInportVec3(pNode, 1);
                                    float result = Vector3.Dot(a, b);
                                    pAgent.SetOutportFloat(pNode, 0, result);
                                    return true;
                                }
                            case EVariableType.eVec4:
                                {
                                    Vector4 a = pAgent.GetInportVec4(pNode, 0);
                                    Vector4 b = pAgent.GetInportVec4(pNode, 1);
                                    float result = Vector4.Dot(a, b);
                                    pAgent.SetOutportFloat(pNode, 0, result);
                                    return true;
                                }
                            default:
                                Debug.LogError("Dot only supports Vec2/Vec3/Vec4");
                                return true;
                        }
                    }
                    break;
                case EActionType.eCrossVariable:
                    {
                        if (varType2 != varType1)
                        {
                            Debug.LogError("condtion op type error!");
                            return false;
                        }
                        switch (varType1)
                        {
                            case EVariableType.eVec2:
                                {
                                    if(outputType!= EVariableType.eFloat)
                                    {
                                        Debug.LogError("condtion outport not float!");
                                        return false;
                                    }
                                    Vector2 a = pAgent.GetInportVec2(pNode, 0);
                                    Vector2 b = pAgent.GetInportVec2(pNode, 1);
                                    float result = a.x * b.y - a.y * b.x;
                                    pAgent.SetOutportFloat(pNode, 0, result);
                                    return true;
                                }
                            case EVariableType.eVec3:
                                {
                                    if (outputType != EVariableType.eVec3)
                                    {
                                        Debug.LogError("condtion outport not Vec3!");
                                        return false;
                                    }
                                    Vector3 a = pAgent.GetInportVec3(pNode, 0);
                                    Vector3 b = pAgent.GetInportVec3(pNode, 1);
                                    Vector3 result = Vector3.Cross(a, b);
                                    pAgent.SetOutportVec3(pNode, 0, result);
                                    return true;
                                }
                            default:
                                Debug.LogError("Cross only supports Vec2/Vec3");
                                return true;
                        }
                    }
                    break;
                case EActionType.eDistanceVariable:
                    {
                        if (varType2 != varType1)
                        {
                            Debug.LogError("condtion op type error!");
                            return false;
                        }
                        switch (varType1)
                        {
                            case EVariableType.eVec2:
                                {
                                    if (outputType != EVariableType.eFloat)
                                    {
                                        Debug.LogError("condtion outport not float!");
                                        return false;
                                    }
                                    Vector2 a = pAgent.GetInportVec2(pNode, 0);
                                    Vector2 b = pAgent.GetInportVec2(pNode, 1);
                                    float result = Vector2.Distance(a, b);
                                    pAgent.SetOutportFloat(pNode, 0, result);
                                    return true;
                                }
                            case EVariableType.eVec3:
                                {
                                    if (outputType != EVariableType.eFloat)
                                    {
                                        Debug.LogError("condtion outport not float!");
                                        return false;
                                    }
                                    Vector3 a = pAgent.GetInportVec3(pNode, 0);
                                    Vector3 b = pAgent.GetInportVec3(pNode, 1);
                                    float result = Vector3.Distance(a, b);
                                    pAgent.SetOutportFloat(pNode, 0, result);
                                    return true;
                                }
                            case EVariableType.eVec4:
                                {
                                    if (outputType != EVariableType.eFloat)
                                    {
                                        Debug.LogError("condtion outport not float!");
                                        return false;
                                    }
                                    Vector4 a = pAgent.GetInportVec4(pNode, 0);
                                    Vector4 b = pAgent.GetInportVec4(pNode, 1);
                                    float result = (a - b).magnitude;
                                    pAgent.SetOutportFloat(pNode, 0, result);
                                    return true;
                                }
                            default:
                                Debug.LogError("Distance only supports Vec2/Vec3/Vec4");
                                return true;
                        }
                    }
                    break;
                case EActionType.eLerp:
                    {
                        if (varType2 != varType1 || outputType != varType1)
                        {
                            Debug.LogError("condtion op type error!");
                            return false;
                        }
                        switch (varType1)
                        {
                            case EVariableType.eVec2:
                                {
                                    Vector2 a = pAgent.GetInportVec2(pNode, 0);
                                    Vector2 b = pAgent.GetInportVec2(pNode, 1);
                                    Vector2 result = Vector2.Lerp(a, b, Time.deltaTime * pAgent.GetInportFloat(pNode, 2));
                                    pAgent.SetOutportVec2(pNode, 0, result);
                                    return true;
                                }
                            case EVariableType.eVec3:
                                {
                                    Vector3 a = pAgent.GetInportVec3(pNode, 0);
                                    Vector3 b = pAgent.GetInportVec3(pNode, 1);
                                    Vector3 result = Vector3.Lerp(a, b, Time.deltaTime* pAgent.GetInportFloat(pNode, 2));
                                    pAgent.SetOutportVec3(pNode, 0, result);
                                    return true;
                                }
                            case EVariableType.eVec4:
                                {
                                    Vector4 a = pAgent.GetInportVec4(pNode, 0);
                                    Vector4 b = pAgent.GetInportVec4(pNode, 1);
                                    Vector4 result = Vector4.Lerp(a, b, Time.deltaTime * pAgent.GetInportFloat(pNode, 2));
                                    pAgent.SetOutportVec4(pNode, 0, result);
                                    return true;
                                }
                            case EVariableType.eQuaternion:
                                {
                                    Quaternion a = pAgent.GetInportQuaternion(pNode, 0);
                                    Quaternion b = pAgent.GetInportQuaternion(pNode, 1);
                                    Quaternion result = Quaternion.Lerp(a, b, Time.deltaTime * pAgent.GetInportFloat(pNode, 2));
                                    pAgent.SetOutportQuaternion(pNode, 0, result);
                                    return true;
                                }
                            case EVariableType.eColor:
                                {
                                    Color a = pAgent.GetInportColor(pNode, 0);
                                    Color b = pAgent.GetInportColor(pNode, 1);
                                    Color result = Color.Lerp(a, b, Time.deltaTime * pAgent.GetInportFloat(pNode, 2));
                                    pAgent.SetOutportColor(pNode, 0, result);
                                    return true;
                                }
                            default:
                                Debug.LogError("Lerp only supports Vec2/Vec3/Vec4/Color/Quaternion");
                                return true;
                        }
                    }
                    break;
                case EActionType.eSlerp:
                    {
                        if (varType2 != varType1 || varType1 != outputType)
                        {
                            Debug.LogError("condtion op type error!");
                            return false;
                        }
                        switch (varType1)
                        {
                            case EVariableType.eVec3:
                                {
                                    Vector3 a = pAgent.GetInportVec3(pNode, 0);
                                    Vector3 b = pAgent.GetInportVec3(pNode, 1);
                                    Vector3 result = Vector3.Slerp(a, b, Time.deltaTime * pAgent.GetInportFloat(pNode, 2));
                                    pAgent.SetOutportVec3(pNode, 0, result);
                                    return true;
                                }
                            case EVariableType.eQuaternion:
                                {
                                    Quaternion a = pAgent.GetInportQuaternion(pNode, 0);
                                    Quaternion b = pAgent.GetInportQuaternion(pNode, 1);
                                    Quaternion result = Quaternion.Slerp(a, b, Time.deltaTime * pAgent.GetInportFloat(pNode, 2));
                                    pAgent.SetOutportQuaternion(pNode, 0, result);
                                    return true;
                                }
                            default:
                                Debug.LogError("Lerp only supports Vec3/Quaternion");
                                return true;
                        }
                    }
                    break;
                case EActionType.eQuaternionToEuler:
                    {
                        if(varType1 == EVariableType.eQuaternion && outputType == EVariableType.eVec3)
                        {
                            Quaternion a = pAgent.GetInportQuaternion(pNode, 0);
                            pAgent.SetOutportVec3(pNode, 0, a.eulerAngles);
                            return true;
                        }
                        else
                            Debug.LogError("Lerp only supports Quaternion");
                        return true;
                    }
                case EActionType.eEulerToQuaternion:
                    {
                        if (varType1 == EVariableType.eVec3 && outputType == EVariableType.eQuaternion)
                        {
                            var a = pAgent.GetInportVec3(pNode, 0);
                            pAgent.SetOutportQuaternion(pNode, 0, Quaternion.Euler(a));
                            return true;
                        }
                        else
                            Debug.LogError("Lerp only supports Quaternion");
                        return true;
                    }
                case EActionType.eMatrixToTRS:
                    {
                        if(inportCnt < 3)
                        {
                            Debug.LogError("eMatrixToTRS need 3 Vec3 outports!");
                            return true;
                        }
                        if (varType1 == EVariableType.eMatrix && outputType == EVariableType.eVec3 && outputType == pAgent.GetOutportVarType(pNode, 1) && outputType == pAgent.GetOutportVarType(pNode, 2))
                        {
                            var a = pAgent.GetInportMatrix(pNode, 0);
                            
                            pAgent.SetOutportVec3(pNode, 0, a.GetPosition());
                            pAgent.SetOutportVec3(pNode, 1, a.rotation.eulerAngles);
                            pAgent.SetOutportVec3(pNode, 2, BaseUtil.GetScale(a));
                            return true;
                        }
                        else
                            Debug.LogError("Lerp only supports Matrix");
                        return true;
                    }
                case EActionType.eTRSToMatrix:
                    {
                        if (inportCnt < 3)
                        {
                            Debug.LogError("eMatrixToTRS need 3 Vec3 outports!");
                            return true;
                        }
                        if(inportCnt<3)
                        {
                            Debug.LogError("eTRSToMatrix need 3 Vec3 inports!");
                            return true;
                        }
                        if (varType1 == EVariableType.eVec3 && varType1 == pAgent.GetInportVarType(pNode, 1) && varType1 == pAgent.GetInportVarType(pNode, 2) &&
                            outputType == EVariableType.eMatrix )
                        {
                            var a = pAgent.GetInportMatrix(pNode, 0);
                            pAgent.SetOutportMatrix(pNode, 0, Matrix4x4.TRS(pAgent.GetInportVec3(pNode, 0), Quaternion.Euler(pAgent.GetInportVec3(pNode, 1)), pAgent.GetInportVec3(pNode, 2)));
                            return true;
                        }
                        else
                            Debug.LogError("Lerp only supports Matrix");
                        return true;
                    }
                case EActionType.eMatrixMultiplyPoint:
                    {
                        if (inportCnt < 2)
                        {
                            Debug.LogError("eMatrixMultiplyPoint need inports[matrix vec3]!");
                            return true;
                        }
                        if (varType1 == EVariableType.eMatrix && EVariableType.eVec3 == pAgent.GetInportVarType(pNode, 1) &&
                            outputType == EVariableType.eVec3)
                        {
                            var a = pAgent.GetInportMatrix(pNode, 0);
                            var b = pAgent.GetInportVec3(pNode, 10);
                            pAgent.SetOutportVec3(pNode, 0, a.MultiplyPoint(b));
                            return true;
                        }
                        else
                            Debug.LogError("Lerp only supports Matrix");
                        return true;
                    }
                case EActionType.eMatrixMultiplyPoint3x4:
                    {
                        if (inportCnt < 2)
                        {
                            Debug.LogError("eMatrixMultiplyPoint3x4 need inports[matrix vec3]!");
                            return true;
                        }
                        if (varType1 == EVariableType.eMatrix && EVariableType.eVec3 == pAgent.GetInportVarType(pNode, 1) &&
                            outputType == EVariableType.eVec3)
                        {
                            var a = pAgent.GetInportMatrix(pNode, 0);
                            var b = pAgent.GetInportVec3(pNode, 10);
                            pAgent.SetOutportVec3(pNode, 0, a.MultiplyPoint3x4(b));
                            return true;
                        }
                        else
                            Debug.LogError("Lerp only supports Matrix");
                        return true;
                    }
                case EActionType.eMatrixMultiplyVector:
                    {
                        if (inportCnt < 2)
                        {
                            Debug.LogError("eMatrixMultiplyVector need inports[matrix vec3]!");
                            return true;
                        }
                        if (varType1 == EVariableType.eMatrix && EVariableType.eVec3 == pAgent.GetInportVarType(pNode, 1) &&
                            outputType == EVariableType.eVec3)
                        {
                            var a = pAgent.GetInportMatrix(pNode, 0);
                            var b = pAgent.GetInportVec3(pNode, 10);
                            pAgent.SetOutportVec3(pNode, 0, a.MultiplyVector(b));
                            return true;
                        }
                        else
                            Debug.LogError("Lerp only supports Matrix");
                        return true;
                    }
                case EActionType.eScreenToWorldPosition:
                    {
                        if(inportCnt<2)
                        {
                            Debug.LogError("eScreenToWorldPosition need inports[vec3 float]!");
                            return true;
                        }
                        if (varType1 == EVariableType.eVec2 && EVariableType.eFloat == pAgent.GetInportVarType(pNode, 1) &&
                            outputType == EVariableType.eVec3)
                        {
                            var a = pAgent.GetInportVec2(pNode, 0);
                            Camera mainCamera = pAgent.GetMainCamera();
                            if (mainCamera == null)
                            {
                                Debug.LogError("Main camera is null!");
                                return true;
                            }
                            if (pAgent.GetNegScreenY())
                                a.y = mainCamera.pixelHeight - a.y;
                            Vector3 worldPos = BaseUtil.RayHitPos(mainCamera.ScreenPointToRay(a),pAgent.GetInportFloat(pNode, 1,0));
                            pAgent.SetOutportVec3(pNode,0, worldPos);
                            return true;
                        }
                        else
                            Debug.LogError("Lerp only supports vec3");
                        return true;
                    }
                case EActionType.eWorldToScreenPosition:
                    {
                        if (varType1 == EVariableType.eVec3 &&
                            outputType == EVariableType.eVec2)
                        {
                            var a = pAgent.GetInportVec3(pNode, 0);
                            Camera mainCamera = pAgent.GetMainCamera();
                            if (mainCamera == null)
                            {
                                Debug.LogError("Main camera is null!");
                                return true;
                            }
                            Vector2 screenPos = mainCamera.WorldToScreenPoint(a);
                            if(pAgent.GetNegScreenY()) screenPos.y = mainCamera.pixelHeight - screenPos.y;
                            pAgent.SetOutportVec2(pNode, 0, screenPos);
                            return true;
                        }
                        else
                            Debug.LogError("Lerp only supports vec3");
                        return true;
                    }
                case EActionType.eCheckWorldPosInView:
                    {
                        if (inportCnt < 2)
                        {
                            Debug.LogError("eMatrixMultiplyVector need inports[vec3 float]!");
                            return true;
                        }
                        if (varType1 == EVariableType.eVec3 && EVariableType.eFloat == pAgent.GetInportVarType(pNode, 1) &&
                            outputType == EVariableType.eBool)
                        {
                            var a = pAgent.GetInportVec3(pNode, 0);
                            Camera mainCamera = pAgent.GetMainCamera();
                            if (mainCamera == null)
                            {
                                Debug.LogError("Main camera is null!");
                                return true;
                            }
                            bool bInview = BaseUtil.PositionInView(mainCamera.cullingMatrix, a, 1 + pAgent.GetInportFloat(pNode, 1));
                            pAgent.SetOutportBool(pNode, 0, bInview);
                            return true;
                        }
                        else
                            Debug.LogError("Lerp only supports inports[vec3 float] outpots[bool]");
                        return true;
                    }
                default:
                    Debug.LogError("Unsupported variable type for operation.");
                    return false;
            }
            return true;
        }
    }
}