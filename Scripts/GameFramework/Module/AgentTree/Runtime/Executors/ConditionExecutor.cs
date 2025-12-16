/********************************************************************
生成日期:	07:03:2025
类    名: 	ConditionExecutor
作    者:	HappLI
描    述:	条件判断执行器
*********************************************************************/
using UnityEngine;
namespace Framework.AT.Runtime
{
    internal class ConditionExecutor
    {
        //------------------------------------------------------
        public static bool OnExecute(AgentTree pAgent, BaseNode pNode)
        {
            int inportCnt = pNode.GetInportCount();
            if (inportCnt != 3)
                return false;
            var inputVarType = pAgent.GetInportVarType(pNode,0);
            if (inputVarType == EVariableType.eNone)
            {
                Debug.LogError("condtion executor input type var error!");
                return false;
            }
            var conditionVarType = pAgent.GetInportVarType(pNode, 1);
            if(conditionVarType != EVariableType.eInt)
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
            ECompareOpType opType = (ECompareOpType)pAgent.GetInportInt(pNode, 1, 0);
            return OnExecute(pAgent, pNode, 0, 2, opType);
        }
        //------------------------------------------------------
        public static bool OnExecute(AgentTree pAgent, BaseNode pNode, int leftPort, int rightPort, ECompareOpType opType)
        {
            int inportCnt = pNode.GetInportCount();
            if(leftPort<0 || rightPort<0 || leftPort>= inportCnt || rightPort>= inportCnt)
            {
                Debug.LogError("condition compare op type port count error!!!");
                return false;
            }
            var inputVarType = pAgent.GetInportVarType(pNode, leftPort);
            if (inputVarType == EVariableType.eNone)
            {
                Debug.LogError("condtion executor input type var error!");
                return false;
            }
            var opVarType = pAgent.GetInportVarType(pNode, rightPort);
            if (opVarType != inputVarType)
            {
                Debug.LogError("condtion op type type var error!");
                return false;
            }
            int nDoSucceed = 0;
            switch (inputVarType)
            {
                case EVariableType.eBool:
                    {
                        if (opType == ECompareOpType.eEqual)
                        {
                            nDoSucceed = pAgent.GetInportBool(pNode, leftPort) == pAgent.GetInportBool(pNode, rightPort) ? 1 : 2;
                        }
                        else if (opType == ECompareOpType.eNotEqual) nDoSucceed = pAgent.GetInportBool(pNode, leftPort) != pAgent.GetInportBool(pNode, rightPort) ? 1 : 2;
                        else return true;
                    }
                    break;
                case EVariableType.eInt:
                    {
                        if (opType == ECompareOpType.eEqual) nDoSucceed = pAgent.GetInportInt(pNode, leftPort) == pAgent.GetInportInt(pNode, rightPort) ? 1 : 2;
                        else if (opType == ECompareOpType.eNotEqual) nDoSucceed = pAgent.GetInportInt(pNode, leftPort) != pAgent.GetInportInt(pNode, rightPort) ? 1 : 2;
                        else if (opType == ECompareOpType.eGreaterThan) nDoSucceed = pAgent.GetInportInt(pNode, leftPort) > pAgent.GetInportInt(pNode, rightPort) ? 1 : 2;
                        else if (opType == ECompareOpType.eGreaterThanOrEqual) nDoSucceed = pAgent.GetInportInt(pNode, leftPort) >= pAgent.GetInportInt(pNode, rightPort) ? 1 : 2;
                        else if (opType == ECompareOpType.eLessThan) nDoSucceed = pAgent.GetInportInt(pNode, leftPort) < pAgent.GetInportInt(pNode, rightPort) ? 1 : 2;
                        else if (opType == ECompareOpType.eLessThanOrEqual) nDoSucceed = pAgent.GetInportInt(pNode, leftPort) <= pAgent.GetInportInt(pNode, rightPort) ? 1 : 2;
                        else if (opType == ECompareOpType.eXor) nDoSucceed = ((pAgent.GetInportInt(pNode, leftPort) & pAgent.GetInportInt(pNode, rightPort)) != 0) ? 1 : 2;
                        else return true;
                    }
                    break;
                case EVariableType.eLong:
                    {
                        if (opType == ECompareOpType.eEqual) nDoSucceed = pAgent.GetInportLong(pNode, leftPort) == pAgent.GetInportLong(pNode, rightPort) ? 1 : 2;
                        else if (opType == ECompareOpType.eNotEqual) nDoSucceed = pAgent.GetInportLong(pNode, leftPort) != pAgent.GetInportLong(pNode, rightPort) ? 1 : 2;
                        else if (opType == ECompareOpType.eGreaterThan) nDoSucceed = pAgent.GetInportLong(pNode, leftPort) > pAgent.GetInportLong(pNode, rightPort) ? 1 : 2;
                        else if (opType == ECompareOpType.eGreaterThanOrEqual) nDoSucceed = pAgent.GetInportLong(pNode, leftPort) >= pAgent.GetInportLong(pNode, rightPort) ? 1 : 2;
                        else if (opType == ECompareOpType.eLessThan) nDoSucceed = pAgent.GetInportLong(pNode, leftPort) < pAgent.GetInportLong(pNode, rightPort) ? 1 : 2;
                        else if (opType == ECompareOpType.eLessThanOrEqual) nDoSucceed = pAgent.GetInportLong(pNode, leftPort) <= pAgent.GetInportLong(pNode, rightPort) ? 1 : 2;
                        else if (opType == ECompareOpType.eXor) nDoSucceed = ((pAgent.GetInportLong(pNode, leftPort) & pAgent.GetInportLong(pNode, rightPort)) != 0) ? 1 : 2;
                        else return true;
                    }
                    break;
                case EVariableType.eFloat:
                    {
                        if (opType == ECompareOpType.eEqual) nDoSucceed = Mathf.Abs(pAgent.GetInportFloat(pNode, leftPort) - pAgent.GetInportFloat(pNode, rightPort)) <= 0.0001f ? 1 : 2;
                        else if (opType == ECompareOpType.eNotEqual) nDoSucceed = Mathf.Abs(pAgent.GetInportFloat(pNode, leftPort) - pAgent.GetInportFloat(pNode, rightPort)) > 0.0001f ? 1 : 2;
                        else if (opType == ECompareOpType.eGreaterThan) nDoSucceed = pAgent.GetInportFloat(pNode, leftPort) > pAgent.GetInportFloat(pNode, rightPort) ? 1 : 2;
                        else if (opType == ECompareOpType.eGreaterThanOrEqual) nDoSucceed = pAgent.GetInportFloat(pNode, leftPort) >= pAgent.GetInportFloat(pNode, rightPort) ? 1 : 2;
                        else if (opType == ECompareOpType.eLessThan) nDoSucceed = pAgent.GetInportFloat(pNode, leftPort) < pAgent.GetInportFloat(pNode, rightPort) ? 1 : 2;
                        else if (opType == ECompareOpType.eLessThanOrEqual) nDoSucceed = pAgent.GetInportFloat(pNode, leftPort) <= pAgent.GetInportFloat(pNode, rightPort) ? 1 : 2;
                        else return true;
                    }
                    break;
                case EVariableType.eDouble:
                    {
                        if (opType == ECompareOpType.eEqual) nDoSucceed = (pAgent.GetInportDouble(pNode, leftPort) == pAgent.GetInportDouble(pNode, rightPort)) ? 1 : 2;
                        else if (opType == ECompareOpType.eNotEqual) nDoSucceed = pAgent.GetInportDouble(pNode, leftPort) != pAgent.GetInportDouble(pNode, rightPort) ? 1 : 2;
                        else if (opType == ECompareOpType.eGreaterThan) nDoSucceed = pAgent.GetInportDouble(pNode, leftPort) > pAgent.GetInportDouble(pNode, rightPort) ? 1 : 2;
                        else if (opType == ECompareOpType.eGreaterThanOrEqual) nDoSucceed = pAgent.GetInportDouble(pNode, leftPort) >= pAgent.GetInportDouble(pNode, rightPort) ? 1 : 2;
                        else if (opType == ECompareOpType.eLessThan) nDoSucceed = pAgent.GetInportDouble(pNode, leftPort) < pAgent.GetInportDouble(pNode, rightPort) ? 1 : 2;
                        else if (opType == ECompareOpType.eLessThanOrEqual) nDoSucceed = pAgent.GetInportDouble(pNode, leftPort) <= pAgent.GetInportDouble(pNode, rightPort) ? 1 : 2;
                        else return true;
                    }
                    break;
                case EVariableType.eString:
                    {
                        if (opType == ECompareOpType.eEqual) nDoSucceed = pAgent.GetInportString(pNode, leftPort).CompareTo(pAgent.GetInportString(pNode, rightPort)) == 0 ? 1 : 2;
                        else if (opType == ECompareOpType.eNotEqual) nDoSucceed = pAgent.GetInportString(pNode, leftPort).CompareTo(pAgent.GetInportString(pNode, rightPort)) != 0 ? 1 : 2;
                        else if (opType == ECompareOpType.eContains) nDoSucceed = pAgent.GetInportString(pNode, leftPort).Contains(pAgent.GetInportString(pNode, rightPort)) ? 1 : 2;
                        else return true;
                    }
                    break;
                case EVariableType.eVec2:
                    {
                        if (opType == ECompareOpType.eEqual) nDoSucceed = pAgent.GetInportVec2(pNode, leftPort).Equals(pAgent.GetInportVec2(pNode, rightPort)) ? 1 : 2;
                        else if (opType == ECompareOpType.eNotEqual) nDoSucceed = !pAgent.GetInportVec2(pNode, leftPort).Equals(pAgent.GetInportVec2(pNode, rightPort)) ? 1 : 2;
                        else return true;
                    }
                    break;
                case EVariableType.eVec3:
                    {
                        if (opType == ECompareOpType.eEqual) nDoSucceed = pAgent.GetInportVec3(pNode, leftPort).Equals(pAgent.GetInportVec3(pNode, rightPort)) ? 1 : 2;
                        else if (opType == ECompareOpType.eNotEqual) nDoSucceed = !pAgent.GetInportVec3(pNode, leftPort).Equals(pAgent.GetInportVec3(pNode, rightPort)) ? 1 : 2;
                        else return true;
                    }
                    break;
                case EVariableType.eVec4:
                    {
                        if (opType == ECompareOpType.eEqual) nDoSucceed = pAgent.GetInportVec4(pNode, leftPort).Equals(pAgent.GetInportVec4(pNode, rightPort)) ? 1 : 2;
                        else if (opType == ECompareOpType.eNotEqual) nDoSucceed = !pAgent.GetInportVec4(pNode, leftPort).Equals(pAgent.GetInportVec4(pNode, rightPort)) ? 1 : 2;
                        else return true;
                    }
                    break;
                case EVariableType.eObjId:
                    {
                        if (opType == ECompareOpType.eEqual) nDoSucceed = pAgent.GetInportObjId(pNode, leftPort).IsEqual(pAgent.GetInportObjId(pNode, rightPort)) ? 1 : 2;
                        else if (opType == ECompareOpType.eNotEqual) nDoSucceed = !pAgent.GetInportObjId(pNode, leftPort).IsEqual(pAgent.GetInportObjId(pNode, rightPort)) ? 1 : 2;
                        else return true;
                    }
                    break;
                case EVariableType.eColor:
                    {
                        if (opType == ECompareOpType.eEqual) nDoSucceed = pAgent.GetInportColor(pNode, leftPort).Equals(pAgent.GetInportColor(pNode, rightPort)) ? 1 : 2;
                        else if (opType == ECompareOpType.eNotEqual) nDoSucceed = !pAgent.GetInportColor(pNode, leftPort).Equals(pAgent.GetInportColor(pNode, rightPort)) ? 1 : 2;
                        else return true;
                    }
                    break;
                case EVariableType.eRay:
                    {
                        if (opType == ECompareOpType.eEqual) nDoSucceed = RayEquals(pAgent.GetInportRay(pNode, leftPort),pAgent.GetInportRay(pNode, rightPort)) ? 1 : 2;
                        else if (opType == ECompareOpType.eNotEqual) nDoSucceed = !RayEquals(pAgent.GetInportRay(pNode, leftPort),pAgent.GetInportRay(pNode, rightPort)) ? 1 : 2;
                        else return true;
                    }
                    break;
                case EVariableType.eQuaternion:
                    {
                        if (opType == ECompareOpType.eEqual) nDoSucceed = pAgent.GetInportQuaternion(pNode, leftPort).Equals(pAgent.GetInportQuaternion(pNode, rightPort)) ? 1 : 2;
                        else if (opType == ECompareOpType.eNotEqual) nDoSucceed = !pAgent.GetInportQuaternion(pNode, leftPort).Equals(pAgent.GetInportQuaternion(pNode, rightPort)) ? 1 : 2;
                        else return true;
                    }
                    break;
                case EVariableType.eRect:
                    {
                        if (opType == ECompareOpType.eEqual) nDoSucceed = pAgent.GetInportRect(pNode, leftPort).Equals(pAgent.GetInportRect(pNode, rightPort)) ? 1 : 2;
                        else if (opType == ECompareOpType.eNotEqual) nDoSucceed = !pAgent.GetInportRect(pNode, leftPort).Equals(pAgent.GetInportRect(pNode, rightPort)) ? 1 : 2;
                        else return true;
                    }
                    break;
                case EVariableType.eBounds:
                    {
                        if (opType == ECompareOpType.eEqual) nDoSucceed = pAgent.GetInportBounds(pNode, leftPort).Equals(pAgent.GetInportBounds(pNode, rightPort)) ? 1 : 2;
                        else if (opType == ECompareOpType.eNotEqual) nDoSucceed = !pAgent.GetInportBounds(pNode, leftPort).Equals(pAgent.GetInportBounds(pNode, rightPort)) ? 1 : 2;
                        else return true;
                    }
                    break;
                case EVariableType.eMatrix:
                    {
                        if (opType == ECompareOpType.eEqual) nDoSucceed = pAgent.GetInportMatrix(pNode, leftPort).Equals(pAgent.GetInportMatrix(pNode, rightPort)) ? 1 : 2;
                        else if (opType == ECompareOpType.eNotEqual) nDoSucceed = !pAgent.GetInportMatrix(pNode, leftPort).Equals(pAgent.GetInportMatrix(pNode, rightPort)) ? 1 : 2;
                        else return true;
                    }
                    break;
                case EVariableType.eUserData:
                    {
                        if (opType == ECompareOpType.eEqual) nDoSucceed = pAgent.GetInportUserData(pNode, leftPort).IsEqual(pAgent.GetInportUserData(pNode, rightPort)) ? 1 : 2;
                        else if (opType == ECompareOpType.eNotEqual) nDoSucceed = !pAgent.GetInportUserData(pNode, leftPort).IsEqual(pAgent.GetInportUserData(pNode, rightPort)) ? 1 : 2;
                        else return true;
                    }
                    break;
            }
            if (nDoSucceed == 2)
            {
                //! else check false
                var outports = pNode.GetOutports();
                if (outports != null && outports.Length > 0)
                {
                    int failedDoNode = pAgent.GetOutportInt(pNode, outports.Length - 1);
                    if (failedDoNode != 0)
                    {
                        pAgent.PushDoNode(pNode, (short)failedDoNode);
                    }
                }
            }
            return nDoSucceed == 1;
        }
        //-----------------------------------------------------
        public static bool RayEquals(Ray a, Ray b, float epsilon = 0.0001f)
        {
            return Vector3.Distance(a.origin, b.origin) < epsilon &&
                   Vector3.Distance(a.direction, b.direction) < epsilon;
        }
    }
}