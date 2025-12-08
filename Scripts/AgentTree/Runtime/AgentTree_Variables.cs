/********************************************************************
生成日期:	07:03:2025
类    名: 	AgentTree
作    者:	HappLI
描    述:	行为树
*********************************************************************/
using Framework.Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
namespace Framework.AT.Runtime
{
    public partial class AgentTree
    {
        #region prop_method
        //-----------------------------------------------------
        /// <summary>
        /// 该方法存在封箱拆箱，建议只在编辑器下使用
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="bRuntime"></param>
        /// <returns></returns>
        public IVariable GetVariable(short guid, bool bRuntime = false)
        {
            if (m_pData == null)
                return null;
            if (m_vRuntimeVariables != null)
            {
                var varNode = m_pData.GetVariable(guid);
                if (varNode != null)
                {
                    switch (varNode.GetVariableType())
                    {
                        case EVariableType.eInt:
                            {
                                if (m_vRuntimeVariables.GetInt(guid, out var retVal)) return new VariableInt { guid = guid, value = retVal };
                                if (bRuntime) return null; break;
                            }
                        case EVariableType.eBool:
                            {
                                if (m_vRuntimeVariables.GetBool(guid, out var retVal)) return new VariableBool { guid = guid, value = retVal };
                                if (bRuntime) return null; break;
                            }
                        case EVariableType.eFloat:
                            {
                                if (m_vRuntimeVariables.GetFloat(guid, out var retVal)) return new VariableFloat { guid = guid, value = retVal };
                                if (bRuntime) return null; break;
                            }
                        case EVariableType.eString:
                            {
                                if (m_vRuntimeVariables.GetString(guid, out var retVal)) return new VariableString { guid = guid, value = retVal };
                                if (bRuntime) return null; break;
                            }
                        case EVariableType.eVec2:
                            {
                                if (m_vRuntimeVariables.GetVec2(guid, out var retVal)) return new VariableVec2 { guid = guid, value = retVal };
                                if (bRuntime) return null; break;
                            }
                        case EVariableType.eVec3:
                            {
                                if (m_vRuntimeVariables.GetVec3(guid, out var retVal)) return new VariableVec3 { guid = guid, value = retVal };
                                if (bRuntime) return null; break;
                            }
                        case EVariableType.eVec4:
                            {
                                if (m_vRuntimeVariables.GetVec4(guid, out var retVal)) return new VariableVec4 { guid = guid, value = retVal };
                                if (bRuntime) return null; break;
                            }
                        case EVariableType.eObjId:
                            {
                                if (m_vRuntimeVariables.GetObjId(guid, out var retVal)) return new VariableObjId { guid = guid, value = retVal };
                                if (bRuntime) return null; break;
                            }
                        case EVariableType.eRay:
                            {
                                if (m_vRuntimeVariables.GetRay(guid, out var retVal)) return new VariableRay { guid = guid, value = retVal };
                                if (bRuntime) return null; break;
                            }
                        case EVariableType.eRay2D:
                            {
                                if (m_vRuntimeVariables.GetRay2D(guid, out var retVal)) return new VariableRay2D { guid = guid, value = retVal };
                                if (bRuntime) return null; break;
                            }
                        case EVariableType.eQuaternion:
                            {
                                if (m_vRuntimeVariables.GetQuaternion(guid, out var retVal)) return new VariableQuaternion { guid = guid, value = retVal };
                                if (bRuntime) return null; break;
                            }
                        case EVariableType.eBounds:
                            {
                                if (m_vRuntimeVariables.GetBounds(guid, out var retVal)) return new VariableBounds { guid = guid, value = retVal };
                                if (bRuntime) return null; break;
                            }
                        case EVariableType.eRect:
                            {
                                if (m_vRuntimeVariables.GetRect(guid, out var retVal)) return new VariableRect { guid = guid, value = retVal };
                                if (bRuntime) return null; break;
                            }
                        case EVariableType.eMatrix:
                            {
                                if (m_vRuntimeVariables.GetMatrix(guid, out var retVal)) return new VariableMatrix { guid = guid, value = retVal };
                                if (bRuntime) return null; break;
                            }
                        case EVariableType.eUserData:
                            {
                                if (m_vRuntimeVariables.GetUserData(guid, out var retVal)) return new VariableUserData { guid = guid, value = retVal };
                                if (bRuntime) return null; break;
                            }
                        default: if (bRuntime) return null; break;
                    }
                }
            }
            if (!bRuntime) return null;
            return m_pData.GetVariable(guid);
        }
        //-----------------------------------------------------
        public EVariableType GetInportVarType(BaseNode pNode, int index)
        {
            if (m_pData == null)
                return EVariableType.eNone;
            var ports = pNode.GetInports();
            if (index < 0 || ports == null || index >= ports.Length)
                return EVariableType.eNone;
            if (ports[index].pVariable != null)
            {
                return ports[index].pVariable.GetVariableType();
            }
            var pVar = m_pData.GetVariable(ports[index].varGuid);
            if (pVar == null) return EVariableType.eNone;
            ports[index].pVariable = pVar;
            return pVar.GetVariableType();
        }
        //-----------------------------------------------------
        public EVariableType GetOutportVarType(BaseNode pNode, int index)
        {
            if (m_pData == null)
                return EVariableType.eNone;
            var ports = pNode.GetOutports();
            if (index < 0 || ports == null || index >= ports.Length)
                return EVariableType.eNone;
            if (ports[index].pVariable != null)
            {
                return ports[index].pVariable.GetVariableType();
            }
            var pVar = m_pData.GetVariable(ports[index].varGuid);
            if (pVar == null) return EVariableType.eNone;
            ports[index].pVariable = pVar;
            return pVar.GetVariableType();
        }
        //-----------------------------------------------------
        internal DummyPort GetDummyPort(BaseNode pNode, int index, bool bInport)
        {
            if (index < 0 || m_pData == null) return DummyPort.DEF;
            NodePort[] ports = bInport ? pNode.GetInports() : pNode.GetOutports();
            if (ports == null || index >= ports.Length) return DummyPort.DEF;
            DummyPort[] dummyPorts = ports[index].dummyPorts;
            if (dummyPorts == null || dummyPorts.Length <= 0) return DummyPort.DEF;

            DummyPort latest = DummyPort.DEF;
            long latestTime = long.MinValue;
            for (int i = 0; i < dummyPorts.Length; ++i)
            {
                var dummy = dummyPorts[i];
                bool isExecuted = IsExecuted(dummy.guid);
                bool bForceUsed = false;
                if(!isExecuted)
                {
                    if (dummy.pNode == null)
                    {
                        dummy.pNode = m_pData.GetNode(dummy.guid);
                    }
                    if (dummy.pNode == null)
                        continue;

                    if (dummy.pNode.type == (int)EActionType.eNewVariable)
                    {
                        isExecuted = true;
                        bForceUsed = true;
                    }
                }

                long execTime = 1;
                if (bForceUsed || (isExecuted && m_vNodeExecTime!=null && m_vNodeExecTime.TryGetValue(dummy.guid, out execTime)))
                {
                    if (execTime > latestTime)
                    {
                        latestTime = execTime;
                        latest = dummy;
                    }
                }
            }
            if (latest.guid!=0)
            {
                var dummy = latest;
                if (dummy.pNode == null)
                {
                    dummy.pNode = m_pData.GetNode(dummy.guid);
                }
                if(dummy.IsValid())
                {
                    if(dummy.pNode.type == (int)EActionType.eNewVariable)
                    {
                        // if new variable node, get inports
                        var tempPorts = dummy.pNode.GetInports();
                        if(tempPorts != null && dummy.slotIndex >=0 && dummy.slotIndex < tempPorts.Length)
                        {
                            return dummy;
                        }
                    }
                }
                return dummy;
            }
            return DummyPort.DEF;
        }
        //-----------------------------------------------------
        public bool SetInportBool(BaseNode pNode, int index, bool bValue)
        {
            var inports = pNode.GetInports();
            if (index < 0 || inports == null || index >= inports.Length)
                return false;

            return SetBool(inports[index].varGuid, bValue);
        }
        //-----------------------------------------------------
        public bool SetInportInt(BaseNode pNode, int index, int nValue)
        {
            var inports = pNode.GetInports();
            if (index < 0 || inports == null || index >= inports.Length)
                return false;

            return SetInt(inports[index].varGuid, nValue);
        }
        //-----------------------------------------------------
        public bool SetInportObjId(BaseNode pNode, int index, ObjId objId)
        {
            var inports = pNode.GetInports();
            if (index < 0 || inports == null || index >= inports.Length)
                return false;

            return SetObjId(inports[index].varGuid, objId);
        }
        //-----------------------------------------------------
        public bool SetInportFloat(BaseNode pNode, int index, float fValue)
        {
            var inports = pNode.GetInports();
            if (index < 0 || inports == null || index >= inports.Length)
                return false;

            return SetFloat(inports[index].varGuid, fValue);
        }
        //-----------------------------------------------------
        public bool SetInportString(BaseNode pNode, int index, string strValue)
        {
            var inports = pNode.GetInports();
            if (index < 0 || inports == null || index >= inports.Length)
                return false;

            return SetString(inports[index].varGuid, strValue);
        }
        //-----------------------------------------------------
        public bool SetInportVec2(BaseNode pNode, int index, Vector2 vecValue)
        {
            var inports = pNode.GetInports();
            if (index < 0 || inports == null || index >= inports.Length)
                return false;

            return SetVec2(inports[index].varGuid, vecValue);
        }
        //-----------------------------------------------------
        public bool SetInportVec3(BaseNode pNode, int index, Vector3 vecValue)
        {
            var inports = pNode.GetInports();
            if (index < 0 || inports == null || index >= inports.Length)
                return false;

            return SetVec3(inports[index].varGuid, vecValue);
        }
        //-----------------------------------------------------
        public bool SetInportVec4(BaseNode pNode, int index, Vector4 vecValue)
        {
            var inports = pNode.GetInports();
            if (index < 0 || inports == null || index >= inports.Length)
                return false;

            return SetVec4(inports[index].varGuid, vecValue);
        }
        //-----------------------------------------------------
        public bool SetOutportBool(BaseNode pNode, int index, bool bValue)
        {
            var outports = pNode.GetOutports();
            if (index < 0 || outports == null || index >= outports.Length)
                return false;

            return SetBool(outports[index].varGuid, bValue);
        }
        //-----------------------------------------------------
        public bool SetOutportInt(BaseNode pNode, int index, int nValue)
        {
            var outports = pNode.GetOutports();
            if (index < 0 || outports == null || index >= outports.Length)
                return false;

            return SetInt(outports[index].varGuid, nValue);
        }
        //-----------------------------------------------------
        public bool SetOutportObjId(BaseNode pNode, int index, ObjId objValue)
        {
            var outports = pNode.GetOutports();
            if (index < 0 || outports == null || index >= outports.Length)
                return false;

            return SetObjId(outports[index].varGuid, objValue);
        }
        //-----------------------------------------------------
        public bool SetOutportFloat(BaseNode pNode, int index, float fValue)
        {
            var outports = pNode.GetOutports();
            if (index < 0 || outports == null || index >= outports.Length)
                return false;

            return SetFloat(outports[index].varGuid, fValue);
        }
        //-----------------------------------------------------
        public bool SetOutportString(BaseNode pNode, int index, string strValue)
        {
            var outports = pNode.GetOutports();
            if (index < 0 || outports == null || index >= outports.Length)
                return false;

            return SetString(outports[index].varGuid, strValue);
        }
        //-----------------------------------------------------
        public bool SetOutportVec2(BaseNode pNode, int index, Vector2 vecValue)
        {
            var outports = pNode.GetOutports();
            if (index < 0 || outports == null || index >= outports.Length)
                return false;

            return SetVec2(outports[index].varGuid, vecValue);
        }
        //-----------------------------------------------------
        public bool SetOutportVec3(BaseNode pNode, int index, Vector3 vecValue)
        {
            var outports = pNode.GetOutports();
            if (index < 0 || outports == null || index >= outports.Length)
                return false;

            return SetVec3(outports[index].varGuid, vecValue);
        }
        //-----------------------------------------------------
        public bool SetOutportVec4(BaseNode pNode, int index, Vector4 vecValue)
        {
            var outports = pNode.GetOutports();
            if (index < 0 || outports == null || index >= outports.Length)
                return false;

            return SetVec4(outports[index].varGuid, vecValue);
        }
        //-----------------------------------------------------
        public bool GetInportBool(BaseNode pNode, int index, bool defValue = false)
        {
            var inports = pNode.GetInports();
            if (index < 0 || inports == null || index >= inports.Length)
                return defValue;

            var nodePort = inports[index];
            DummyPort dummyPort = GetDummyPort(pNode, index, true);
            if (dummyPort.IsValid())
            {
                NodePort[] ports = null;
                if (dummyPort.type == 0) ports = pNode.GetInports();
                else ports = pNode.GetOutports();
                if (dummyPort.slotIndex >= 0 && ports != null && dummyPort.slotIndex < ports.Length)
                {
                    return GetBool(ports[dummyPort.slotIndex].varGuid, defValue);
                }
            }

            return GetBool(nodePort.varGuid, defValue);
        }
        /*
        //-----------------------------------------------------
        public IVariable GetInportVar(BaseNode pNode, int index)
        {
            var inports = pNode.GetInports();
            if (index < 0 || inports == null || index >= inports.Length)
                return null;
            var nodePort = inports[index];
            DummyPort dummyPort = GetDummyPort(pNode, index, true);
            if (dummyPort.IsValid())
            {
                NodePort[] ports = null;
                if (dummyPort.type == 0) ports = dummyPort.pNode.GetInports();
                else ports = dummyPort.pNode.GetOutports();
                if (dummyPort.slotIndex >= 0 && ports != null && dummyPort.slotIndex < ports.Length)
                {
                    return GetVariable(ports[dummyPort.slotIndex].varGuid,true);
                }
            }

            return GetVariable(nodePort.varGuid,true);
        }
        */
        //-----------------------------------------------------
        public int GetInportInt(BaseNode pNode, int index, int defValue = 0)
        {
            var inports = pNode.GetInports();
            if (index < 0 || inports == null || index >= inports.Length)
                return defValue;

            var nodePort = inports[index];
            DummyPort dummyPort = GetDummyPort(pNode, index, true);
            if (dummyPort.IsValid())
            {
                NodePort[] ports = null;
                if(dummyPort.pNode.type == (int)EActionType.eNewVariable)
                {
                    ports = dummyPort.pNode.GetInports();
                }
                else
                {
                    if (dummyPort.type == 0) ports = dummyPort.pNode.GetInports();
                    else ports = dummyPort.pNode.GetOutports();
                }
                if (dummyPort.slotIndex >= 0 && ports != null && dummyPort.slotIndex < ports.Length)
                {
                    return GetInt(ports[dummyPort.slotIndex].varGuid, defValue);
                }
            }

            return GetInt(nodePort.varGuid, defValue);
        }
        //-----------------------------------------------------
        public ObjId GetInportObjId(BaseNode pNode, int index)
        {
            var inports = pNode.GetInports();
            if (index < 0 || inports == null || index >= inports.Length)
                return new ObjId();

            var nodePort = inports[index];
            DummyPort dummyPort = GetDummyPort(pNode, index, true);
            if (dummyPort.IsValid())
            {
                NodePort[] ports = null;
                if (dummyPort.type == 0) ports = dummyPort.pNode.GetInports();
                else ports = dummyPort.pNode.GetOutports();
                if (dummyPort.slotIndex >= 0 && ports != null && dummyPort.slotIndex < ports.Length)
                {
                    return GetObjId(ports[dummyPort.slotIndex].varGuid);
                }
            }

            return GetObjId(nodePort.varGuid);
        }
        //-----------------------------------------------------
        public float GetInportFloat(BaseNode pNode, int index, float defValue = 0.0f)
        {
            var inports = pNode.GetInports();
            if (index < 0 || inports == null || index >= inports.Length)
                return defValue;

            var nodePort = inports[index];
            DummyPort dummyPort = GetDummyPort(pNode, index, true);
            if (dummyPort.IsValid())
            {
                NodePort[] ports = null;
                if (dummyPort.type == 0) ports = pNode.GetInports();
                else ports = pNode.GetOutports();
                if (dummyPort.slotIndex >= 0 && ports != null && dummyPort.slotIndex < ports.Length)
                {
                    return GetFloat(ports[dummyPort.slotIndex].varGuid, defValue);
                }
            }

            return GetFloat(nodePort.varGuid, defValue);
        }
        //-----------------------------------------------------
        public string GetInportString(BaseNode pNode, int index, string defValue = null)
        {
            var inports = pNode.GetInports();
            if (index < 0 || inports == null || index >= inports.Length)
                return defValue;

            var nodePort = inports[index];
            DummyPort dummyPort = GetDummyPort(pNode, index, true);
            if (dummyPort.IsValid())
            {
                NodePort[] ports = null;
                if (dummyPort.type == 0) ports = pNode.GetInports();
                else ports = pNode.GetOutports();
                if (dummyPort.slotIndex >= 0 && ports != null && dummyPort.slotIndex < ports.Length)
                {
                    return GetString(ports[dummyPort.slotIndex].varGuid, defValue);
                }
            }

            return GetString(nodePort.varGuid, defValue);
        }
        //-----------------------------------------------------
        public Vector2 GetInportVec2(BaseNode pNode, int index)
        {
            var inports = pNode.GetInports();
            if (index < 0 || inports == null || index >= inports.Length)
                return Vector2.zero;

            var nodePort = inports[index];
            DummyPort dummyPort = GetDummyPort(pNode, index, true);
            if (dummyPort.IsValid())
            {
                NodePort[] ports = null;
                if (dummyPort.type == 0) ports = pNode.GetInports();
                else ports = pNode.GetOutports();
                if (dummyPort.slotIndex >= 0 && ports != null && dummyPort.slotIndex < ports.Length)
                {
                    return GetVec2(ports[dummyPort.slotIndex].varGuid);
                }
            }

            return GetVec2(nodePort.varGuid);
        }
        //-----------------------------------------------------
        public Vector3 GetInportVec3(BaseNode pNode, int index)
        {
            var inports = pNode.GetInports();
            if (index < 0 || inports == null || index >= inports.Length)
                return Vector2.zero;

            var nodePort = inports[index];
            DummyPort dummyPort = GetDummyPort(pNode, index, true);
            if (dummyPort.IsValid())
            {
                NodePort[] ports = null;
                if (dummyPort.type == 0) ports = pNode.GetInports();
                else ports = pNode.GetOutports();
                if (dummyPort.slotIndex >= 0 && ports != null && dummyPort.slotIndex < ports.Length)
                {
                    return GetVec3(ports[dummyPort.slotIndex].varGuid);
                }
            }

            return GetVec3(nodePort.varGuid);
        }
        //-----------------------------------------------------
        public Vector4 GetInportVec4(BaseNode pNode, int index)
        {
            var inports = pNode.GetInports();
            if (index < 0 || inports == null || index >= inports.Length)
                return Vector4.zero;

            var nodePort = inports[index];
            DummyPort dummyPort = GetDummyPort(pNode, index, true);
            if (dummyPort.IsValid())
            {
                NodePort[] ports = null;
                if (dummyPort.type == 0) ports = pNode.GetInports();
                else ports = pNode.GetOutports();
                if (dummyPort.slotIndex >= 0 && ports != null && dummyPort.slotIndex < ports.Length)
                {
                    return GetVec4(ports[dummyPort.slotIndex].varGuid);
                }
            }

            return GetVec4(nodePort.varGuid);
        }
        /*
        //-----------------------------------------------------
        public IVariable GetOutportVar(BaseNode pNode, int index)
        {
            var ports = pNode.GetOutports();
            if (index < 0 || ports == null || index >= ports.Length)
                return null;

            return GetVariable(ports[index].varGuid);
        }*/
        //-----------------------------------------------------
        public bool GetOutportBool(BaseNode pNode, int index, bool defValue = false)
        {
            var ports = pNode.GetOutports();
            if (index < 0 || ports == null || index >= ports.Length)
                return defValue;

            return GetBool(ports[index].varGuid, defValue);
        }
        //-----------------------------------------------------
        public int GetOutportInt(BaseNode pNode, int index, int defValue = 0)
        {
            var ports = pNode.GetOutports();
            if (index < 0 || ports == null || index >= ports.Length)
                return defValue;

            return GetInt(ports[index].varGuid, defValue);
        }
        //-----------------------------------------------------
        public ObjId GetOutportObjId(BaseNode pNode, int index)
        {
            var ports = pNode.GetOutports();
            if (index < 0 || ports == null || index >= ports.Length)
                return new ObjId();

            return GetObjId(ports[index].varGuid);
        }
        //-----------------------------------------------------
        public float GetOutportFloat(BaseNode pNode, int index, float defValue = 0.0f)
        {
            var ports = pNode.GetOutports();
            if (index < 0 || ports == null || index >= ports.Length)
                return defValue;

            return GetFloat(ports[index].varGuid, defValue);
        }
        //-----------------------------------------------------
        public string GetOutportString(BaseNode pNode, int index, string defValue = null)
        {
            var ports = pNode.GetOutports();
            if (index < 0 || ports == null || index >= ports.Length)
                return defValue;

            return GetString(ports[index].varGuid, defValue);
        }
        //-----------------------------------------------------
        public Vector2 GetOutportVec2(BaseNode pNode, int index)
        {
            var ports = pNode.GetOutports();
            if (index < 0 || ports == null || index >= ports.Length)
                return Vector2.zero;

            return GetVec2(ports[index].varGuid);
        }
        //-----------------------------------------------------
        public Vector3 GetOutportVec3(BaseNode pNode, int index)
        {
            var ports = pNode.GetOutports();
            if (index < 0 || ports == null || index >= ports.Length)
                return Vector2.zero;

            return GetVec3(ports[index].varGuid);
        }
        //-----------------------------------------------------
        public Vector4 GetOutportVec4(BaseNode pNode, int index)
        {
            var ports = pNode.GetOutports();
            if (index < 0 || ports == null || index >= ports.Length)
                return Vector4.zero;

            return GetVec4(ports[index].varGuid);
        }
        //-----------------------------------------------------
        public bool SetBool(short guid, bool bValue)
        {
            if (m_pData == null)
                return false;
            var varNode = m_pData.GetVariable(guid);
            if (varNode == null)
            {
                Debug.LogError("guid:" + guid + "  vairable is null");
                return false;
            }
            if (varNode.GetVariableType() == EVariableType.eBool)
            {
                GetRuntimeVariable().SetBool(guid, bValue);
            }
            else
            {
                Debug.LogError("guid:" + guid + "  vairable type is " + varNode.GetVariableType().ToString());
                return false;
            }
            return true;
        }
        //-----------------------------------------------------
        public bool SetInt(short guid, int nValue)
        {
            if (m_pData == null)
                return false;
            var varNode = m_pData.GetVariable(guid);
            if (varNode == null)
            {
                Debug.LogError("guid:" + guid + "  vairable is null");
                return false;
            }
            if (varNode.GetVariableType() == EVariableType.eInt)
            {
                GetRuntimeVariable().SetInt(guid, nValue);
            }
            else
            {
                Debug.LogError("guid:" + guid + "  vairable type is " + varNode.GetVariableType().ToString());
                return false;
            }
            return true;
        }
        //-----------------------------------------------------
        public bool SetObjId(short guid, ObjId objId)
        {
            if (m_pData == null)
                return false;
            var varNode = m_pData.GetVariable(guid);
            if (varNode == null)
            {
                Debug.LogError("guid:" + guid + "  vairable is null");
                return false;
            }
            if (varNode.GetVariableType() == EVariableType.eObjId)
            {
                GetRuntimeVariable().SetObjId(guid, objId);
            }
            else
            {
                Debug.LogError("guid:" + guid + "  vairable type is " + varNode.GetVariableType().ToString());
                return false;
            }
            return true;
        }
        //-----------------------------------------------------
        public bool SetFloat(short guid, float fValue)
        {
            if (m_pData == null)
                return false;
            var varNode = m_pData.GetVariable(guid);
            if (varNode == null)
            {
                Debug.LogError("guid:" + guid + "  vairable is null");
                return false;
            }
            if (varNode.GetVariableType() == EVariableType.eFloat)
            {
                GetRuntimeVariable().SetFloat(guid, fValue);
            }
            else if (varNode.GetVariableType() == EVariableType.eVec2)
            {
                GetRuntimeVariable().SetVec2(guid, new Vector2(fValue, fValue));
            }
            else if (varNode.GetVariableType() == EVariableType.eVec3)
            {
                GetRuntimeVariable().SetVec3(guid, new Vector3(fValue, fValue, fValue));
            }
            else if (varNode.GetVariableType() == EVariableType.eVec4)
            {
                GetRuntimeVariable().SetVec4(guid, new Vector4(fValue, fValue, fValue, fValue));
            }
            else
            {
                Debug.LogError("guid:" + guid + "  vairable type is " + varNode.GetVariableType().ToString());
                return false;
            }
            return true;
        }
        //-----------------------------------------------------
        public bool SetString(short guid, string strValue)
        {
            if (m_pData == null)
                return false;
            var varNode = m_pData.GetVariable(guid);
            if (varNode == null)
            {
                Debug.LogError("guid:" + guid + "  vairable is null");
                return false;
            }
            if (varNode.GetVariableType() == EVariableType.eString)
            {
                GetRuntimeVariable().SetString(guid, strValue);
            }
            else
            {
                Debug.LogError("guid:" + guid + "  vairable type is " + varNode.GetVariableType().ToString());
                return false;
            }
            return true;
        }
        //-----------------------------------------------------
        public bool SetVec2(short guid, Vector2 vecValue)
        {
            if (m_pData == null)
                return false;
            var varNode = m_pData.GetVariable(guid);
            if (varNode == null)
            {
                Debug.LogError("guid:" + guid + "  vairable is null");
                return false;
            }
            if (varNode.GetVariableType() == EVariableType.eVec2)
            {
                GetRuntimeVariable().SetVec2(guid, vecValue);
            }
            else if (varNode.GetVariableType() == EVariableType.eVec3)
            {
                GetRuntimeVariable().SetVec3(guid, vecValue);
            }
            else if (varNode.GetVariableType() == EVariableType.eVec4)
            {
                GetRuntimeVariable().SetVec4(guid, vecValue);
            }
            else if (varNode.GetVariableType() == EVariableType.eFloat)
            {
                GetRuntimeVariable().SetFloat(guid, vecValue.x);
            }
            else
            {
                Debug.LogError("guid:" + guid + "  vairable type is " + varNode.GetVariableType().ToString());
                return false;
            }
            return true;
        }
        //-----------------------------------------------------
        public bool SetVec3(short guid, Vector3 vecValue)
        {
            if (m_pData == null)
                return false;
            var varNode = m_pData.GetVariable(guid);
            if (varNode == null)
            {
                Debug.LogError("guid:" + guid + "  vairable is null");
                return false;
            }
            if (varNode.GetVariableType() == EVariableType.eVec3)
            {
                GetRuntimeVariable().SetVec3(guid, vecValue);
            }
            else if (varNode.GetVariableType() == EVariableType.eVec2)
            {
                GetRuntimeVariable().SetVec2(guid, vecValue);
            }
            else if (varNode.GetVariableType() == EVariableType.eVec4)
            {
                GetRuntimeVariable().SetVec4(guid, vecValue);
            }
            else if (varNode.GetVariableType() == EVariableType.eFloat)
            {
                GetRuntimeVariable().SetFloat(guid, vecValue.x);
            }
            else
            {
                Debug.LogError("guid:" + guid + "  vairable type is " + varNode.GetVariableType().ToString());
                return false;
            }
            return true;
        }
        //-----------------------------------------------------
        public bool SetVec4(short guid, Vector4 vecValue)
        {
            if (m_pData == null)
                return false;
            var varNode = m_pData.GetVariable(guid);
            if (varNode == null)
            {
                Debug.LogError("guid:" + guid + "  vairable is null");
                return false;
            }
            if (varNode.GetVariableType() == EVariableType.eVec4)
            {
                GetRuntimeVariable().SetVec4(guid, vecValue);
            }
            else if (varNode.GetVariableType() == EVariableType.eVec3)
            {
                GetRuntimeVariable().SetVec3(guid, vecValue);
            }
            else if (varNode.GetVariableType() == EVariableType.eVec2)
            {
                GetRuntimeVariable().SetVec2(guid, vecValue);
            }
            else if (varNode.GetVariableType() == EVariableType.eFloat)
            {
                GetRuntimeVariable().SetFloat(guid, vecValue.x);
            }
            else
            {
                Debug.LogError("guid:" + guid + "  vairable type is " + varNode.GetVariableType().ToString());
                return false;
            }
            return true;
        }
        //-----------------------------------------------------
        public bool SetRay(short guid, Ray ray)
        {
            if (m_pData == null)
                return false;
            var varNode = m_pData.GetVariable(guid);
            if (varNode == null)
            {
                Debug.LogError("guid:" + guid + "  vairable is null");
                return false;
            }
            if (varNode.GetVariableType() == EVariableType.eRay)
            {
                GetRuntimeVariable().SetRay(guid, ray);
            }
            else
            {
                Debug.LogError("guid:" + guid + "  vairable type is " + varNode.GetVariableType().ToString());
                return false;
            }
            return true;
        }
        //-----------------------------------------------------
        public bool SetRay2D(short guid, Ray2D ray)
        {
            if (m_pData == null)
                return false;
            var varNode = m_pData.GetVariable(guid);
            if (varNode == null)
            {
                Debug.LogError("guid:" + guid + "  vairable is null");
                return false;
            }
            if (varNode.GetVariableType() == EVariableType.eRay2D)
            {
                GetRuntimeVariable().SetRay2D(guid, ray);
            }
            else
            {
                Debug.LogError("guid:" + guid + "  vairable type is " + varNode.GetVariableType().ToString());
                return false;
            }
            return true;
        }
        //-----------------------------------------------------
        public bool SetQuaternion(short guid, Quaternion quaternion)
        {
            if (m_pData == null)
                return false;
            var varNode = m_pData.GetVariable(guid);
            if (varNode == null)
            {
                Debug.LogError("guid:" + guid + "  vairable is null");
                return false;
            }
            if (varNode.GetVariableType() == EVariableType.eQuaternion)
            {
                GetRuntimeVariable().SetQuaternion(guid, quaternion);
            }
            else if (varNode.GetVariableType() == EVariableType.eVec4)
            {
                GetRuntimeVariable().SetVec4(guid, new Vector4(quaternion.x, quaternion.y,quaternion.z,quaternion.w));
            }
            else
            {
                Debug.LogError("guid:" + guid + "  vairable type is " + varNode.GetVariableType().ToString());
                return false;
            }
            return true;
        }
        //-----------------------------------------------------
        public bool SetBounds(short guid, Bounds bounds)
        {
            if (m_pData == null)
                return false;
            var varNode = m_pData.GetVariable(guid);
            if (varNode == null)
            {
                Debug.LogError("guid:" + guid + "  vairable is null");
                return false;
            }
            if (varNode.GetVariableType() == EVariableType.eBounds)
            {
                GetRuntimeVariable().SetBounds(guid, bounds);
            }
            else
            {
                Debug.LogError("guid:" + guid + "  vairable type is " + varNode.GetVariableType().ToString());
                return false;
            }
            return true;
        }
        //-----------------------------------------------------
        public bool SetRect(short guid, Rect rect)
        {
            if (m_pData == null)
                return false;
            var varNode = m_pData.GetVariable(guid);
            if (varNode == null)
            {
                Debug.LogError("guid:" + guid + "  vairable is null");
                return false;
            }
            if (varNode.GetVariableType() == EVariableType.eRect)
            {
                GetRuntimeVariable().SetRect(guid, rect);
            }
            else
            {
                Debug.LogError("guid:" + guid + "  vairable type is " + varNode.GetVariableType().ToString());
                return false;
            }
            return true;
        }
        //-----------------------------------------------------
        public bool SetMatrix(short guid, Matrix4x4 matrix)
        {
            if (m_pData == null)
                return false;
            var varNode = m_pData.GetVariable(guid);
            if (varNode == null)
            {
                Debug.LogError("guid:" + guid + "  vairable is null");
                return false;
            }
            if (varNode.GetVariableType() == EVariableType.eMatrix)
            {
                GetRuntimeVariable().SetMatrix(guid, matrix);
            }
            else
            {
                Debug.LogError("guid:" + guid + "  vairable type is " + varNode.GetVariableType().ToString());
                return false;
            }
            return true;
        }
        //-----------------------------------------------------
        public bool SetUserData(short guid, IUserData userData)
        {
            if (m_pData == null)
                return false;
            var varNode = m_pData.GetVariable(guid);
            if (varNode == null)
            {
                Debug.LogError("guid:" + guid + "  vairable is null");
                return false;
            }
            if (varNode.GetVariableType() == EVariableType.eUserData)
            {
                GetRuntimeVariable().SetUserData(guid, userData);
            }
            else
            {
                Debug.LogError("guid:" + guid + "  vairable type is " + varNode.GetVariableType().ToString());
                return false;
            }
            return true;
        }
        //-----------------------------------------------------
        public bool GetBool(short guid, bool bDefalue = false)
        {
            if (m_pData == null)
                return bDefalue;
            var varNode = m_pData.GetVariable(guid);
            if (varNode == null)
            {
                Debug.LogError("guid:" + guid + "  vairable is null");
                return bDefalue;
            }
            if (m_vRuntimeVariables != null && m_vRuntimeVariables.GetBool(guid, out var bVal))
                return bVal;

            if (varNode.GetVariableType() == EVariableType.eBool)
            {
                return ((VariableBool)varNode).value;
            }
            else
            {
                Debug.LogError("guid:" + guid + "  vairable type is " + varNode.GetVariableType().ToString());
                return bDefalue;
            }
        }
        //-----------------------------------------------------
        public int GetInt(short guid, int nDefalue = 0)
        {
            if (m_pData == null)
                return nDefalue;
            var varNode = m_pData.GetVariable(guid);
            if (varNode == null)
            {
                Debug.LogError("guid:" + guid + "  vairable is null");
                return nDefalue;
            }
            if (m_vRuntimeVariables != null && m_vRuntimeVariables.GetInt(guid, out var retVal))
                return retVal;

            if (varNode.GetVariableType() == EVariableType.eInt)
            {
                return ((VariableInt)varNode).value;
            }
            else
            {
                Debug.LogError("guid:" + guid + "  vairable type is " + varNode.GetVariableType().ToString());
                return nDefalue;
            }
        }
        //-----------------------------------------------------
        public ObjId GetObjId(short guid)
        {
            if (m_pData == null)
                return new ObjId();
            var varNode = m_pData.GetVariable(guid);
            if (varNode == null)
            {
                Debug.LogError("guid:" + guid + "  vairable is null");
                return new ObjId();
            }
            if (m_vRuntimeVariables != null && m_vRuntimeVariables.GetObjId(guid, out var retVal))
                return retVal;

            if (varNode.GetVariableType() == EVariableType.eObjId)
            {
                return ((VariableObjId)varNode).value;
            }
            else
            {
                Debug.LogError("guid:" + guid + "  vairable type is " + varNode.GetVariableType().ToString());
                return new ObjId();
            }
        }
        //-----------------------------------------------------
        public float GetFloat(short guid, float fDefalue = 0)
        {
            if (m_pData == null)
                return fDefalue;
            var varNode = m_pData.GetVariable(guid);
            if (varNode == null)
            {
                Debug.LogError("guid:" + guid + "  vairable is null");
                return fDefalue;
            }
            if (m_vRuntimeVariables != null && m_vRuntimeVariables.GetFloat(guid, out var retVal))
                return retVal;

            if (varNode.GetVariableType() == EVariableType.eFloat)
            {
                return ((VariableFloat)varNode).value;
            }
            else
            {
                Debug.LogError("guid:" + guid + "  vairable type is " + varNode.GetVariableType().ToString());
                return fDefalue;
            }
        }
        //-----------------------------------------------------
        public string GetString(short guid, string strDefalue = null)
        {
            if (m_pData == null)
                return strDefalue;
            var varNode = m_pData.GetVariable(guid);
            if (varNode == null)
            {
                Debug.LogError("guid:" + guid + "  vairable is null");
                return strDefalue;
            }
            if (m_vRuntimeVariables != null && m_vRuntimeVariables.GetString(guid, out var retVal))
                return retVal;

            if (varNode.GetVariableType() == EVariableType.eString)
            {
                return ((VariableString)varNode).value;
            }
            else
            {
                Debug.LogError("guid:" + guid + "  vairable type is " + varNode.GetVariableType().ToString());
                return strDefalue;
            }
        }
        //-----------------------------------------------------
        public Vector2 GetVec2(short guid)
        {
            if (m_pData == null)
                return Vector2.zero;
            var varNode = m_pData.GetVariable(guid);
            if (varNode == null)
            {
                Debug.LogError("guid:" + guid + "  vairable is null");
                return Vector2.zero;
            }
            if (m_vRuntimeVariables != null && m_vRuntimeVariables.GetVec2(guid, out var retVal))
                return retVal;

            if (varNode.GetVariableType() == EVariableType.eVec2)
            {
                return ((VariableVec2)varNode).value;
            }
            else
            {
                Debug.LogError("guid:" + guid + "  vairable type is " + varNode.GetVariableType().ToString());
                return Vector2.zero;
            }
        }
        //-----------------------------------------------------
        public Vector3 GetVec3(short guid)
        {
            if (m_pData == null)
                return Vector3.zero;
            var varNode = m_pData.GetVariable(guid);
            if (varNode == null)
            {
                Debug.LogError("guid:" + guid + "  vairable is null");
                return Vector3.zero;
            }
            if (m_vRuntimeVariables != null && m_vRuntimeVariables.GetVec3(guid, out var retVal))
                return retVal;

            if (varNode.GetVariableType() == EVariableType.eVec3)
            {
                return ((VariableVec3)varNode).value;
            }
            else
            {
                Debug.LogError("guid:" + guid + "  vairable type is " + varNode.GetVariableType().ToString());
                return Vector3.zero;
            }
        }
        //-----------------------------------------------------
        public Vector4 GetVec4(short guid)
        {
            if (m_pData == null)
                return Vector4.zero;
            var varNode = m_pData.GetVariable(guid);
            if (varNode == null)
            {
                Debug.LogError("guid:" + guid + "  vairable is null");
                return Vector4.zero;
            }
            if (m_vRuntimeVariables != null && m_vRuntimeVariables.GetVec4(guid, out var retVal))
                return retVal;

            if (varNode.GetVariableType() == EVariableType.eVec4)
            {
                return ((VariableVec4)varNode).value;
            }
            else
            {
                Debug.LogError("guid:" + guid + "  vairable type is " + varNode.GetVariableType().ToString());
                return Vector4.zero;
            }
        }
        //-----------------------------------------------------
        public Ray GetRay(short guid)
        {
            if (m_pData == null)
                return new Ray();
            var varNode = m_pData.GetVariable(guid);
            if (varNode == null)
            {
                Debug.LogError("guid:" + guid + "  vairable is null");
                return new Ray();
            }
            if (m_vRuntimeVariables != null && m_vRuntimeVariables.GetRay(guid, out var retVal))
                return retVal;

            if (varNode.GetVariableType() == EVariableType.eRay)
            {
                return ((VariableRay)varNode).value;
            }
            else
            {
                Debug.LogError("guid:" + guid + "  vairable type is " + varNode.GetVariableType().ToString());
                return new Ray();
            }
        }
        //-----------------------------------------------------
        public Ray2D GetRay2D(short guid)
        {
            if (m_pData == null)
                return new Ray2D();
            var varNode = m_pData.GetVariable(guid);
            if (varNode == null)
            {
                Debug.LogError("guid:" + guid + "  vairable is null");
                return new Ray2D();
            }
            if (m_vRuntimeVariables != null && m_vRuntimeVariables.GetRay2D(guid, out var retVal))
                return retVal;

            if (varNode.GetVariableType() == EVariableType.eRay)
            {
                return ((VariableRay2D)varNode).value;
            }
            else
            {
                Debug.LogError("guid:" + guid + "  vairable type is " + varNode.GetVariableType().ToString());
                return new Ray2D();
            }
        }
        //-----------------------------------------------------
        public Quaternion GetQuaternion(short guid)
        {
            if (m_pData == null)
                return Quaternion.identity;
            var varNode = m_pData.GetVariable(guid);
            if (varNode == null)
            {
                Debug.LogError("guid:" + guid + "  vairable is null");
                return Quaternion.identity;
            }
            if (m_vRuntimeVariables != null && m_vRuntimeVariables.GetQuaternion(guid, out var retVal))
                return retVal;

            if (varNode.GetVariableType() == EVariableType.eQuaternion)
            {
                return ((VariableQuaternion)varNode).value;
            }
            else
            {
                Debug.LogError("guid:" + guid + "  vairable type is " + varNode.GetVariableType().ToString());
                return Quaternion.identity;
            }
        }
        //-----------------------------------------------------
        public Bounds GetBounds(short guid)
        {
            if (m_pData == null)
                return new Bounds();
            var varNode = m_pData.GetVariable(guid);
            if (varNode == null)
            {
                Debug.LogError("guid:" + guid + "  vairable is null");
                return new Bounds();
            }
            if (m_vRuntimeVariables != null && m_vRuntimeVariables.GetBounds(guid, out var retVal))
                return retVal;

            if (varNode.GetVariableType() == EVariableType.eBounds)
            {
                return ((VariableBounds)varNode).value;
            }
            else
            {
                Debug.LogError("guid:" + guid + "  vairable type is " + varNode.GetVariableType().ToString());
                return new Bounds();
            }
        }
        //-----------------------------------------------------
        public Rect GetRect(short guid)
        {
            if (m_pData == null)
                return Rect.zero;
            var varNode = m_pData.GetVariable(guid);
            if (varNode == null)
            {
                Debug.LogError("guid:" + guid + "  vairable is null");
                return Rect.zero;
            }
            if (m_vRuntimeVariables != null && m_vRuntimeVariables.GetRect(guid, out var retVal))
                return retVal;

            if (varNode.GetVariableType() == EVariableType.eRect)
            {
                return ((VariableRect)varNode).value;
            }
            else
            {
                Debug.LogError("guid:" + guid + "  vairable type is " + varNode.GetVariableType().ToString());
                return Rect.zero;
            }
        }
        //-----------------------------------------------------
        public Matrix4x4 GetMatrix(short guid)
        {
            if (m_pData == null)
                return Matrix4x4.identity;
            var varNode = m_pData.GetVariable(guid);
            if (varNode == null)
            {
                Debug.LogError("guid:" + guid + "  vairable is null");
                return Matrix4x4.identity;
            }
            if (m_vRuntimeVariables != null && m_vRuntimeVariables.GetMatrix(guid, out var retVal))
                return retVal;

            if (varNode.GetVariableType() == EVariableType.eMatrix)
            {
                return ((VariableMatrix)varNode).value;
            }
            else
            {
                Debug.LogError("guid:" + guid + "  vairable type is " + varNode.GetVariableType().ToString());
                return Matrix4x4.identity;
            }
        }
        //-----------------------------------------------------
        public IUserData GetUserData(short guid)
        {
            if (m_pData == null)
                return null;
            var varNode = m_pData.GetVariable(guid);
            if (varNode == null)
            {
                Debug.LogError("guid:" + guid + "  vairable is null");
                return null;
            }
            if (m_vRuntimeVariables != null && m_vRuntimeVariables.GetUserData(guid, out var retVal))
                return retVal;

            if (varNode.GetVariableType() == EVariableType.eUserData)
            {
                return ((VariableUserData)varNode).value;
            }
            else
            {
                Debug.LogError("guid:" + guid + "  vairable type is " + varNode.GetVariableType().ToString());
                return null;
            }
        }
        #endregion
    }
}