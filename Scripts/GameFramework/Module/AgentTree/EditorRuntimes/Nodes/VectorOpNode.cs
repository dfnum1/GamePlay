/********************************************************************
生成日期:	06:30:2025
类    名: 	VectorOpNode
作    者:	HappLI
描    述:	向量运算节点
*********************************************************************/
#if UNITY_EDITOR
using Framework.AT.Runtime;

namespace Framework.AT.Editor
{
    [NodeBind(EActionType.eDotVariable)]
    [NodeBind(EActionType.eCrossVariable)]
    [NodeBind(EActionType.eDistanceVariable)]
    [NodeBind(EActionType.eLerp)]
    public class VectorOpNode : GraphNode
    {
        public VectorOpNode(AgentTreeGraphView pAgent, BaseNode pNode, bool bUpdatePos = true) : base(pAgent, pNode, bUpdatePos)
        {
        }
        //------------------------------------------------------
        protected override void OnArgvPortChanged(ArvgPort port)
        {
            base.OnArgvPortChanged(port);
        }
        //------------------------------------------------------
        protected override void CreateInports()
        {
            base.CreateInports();
            if (2 < m_vArgvPorts.Count)
            {
                m_vArgvPorts[1].curDummyAttri = m_vArgvPorts[0].curDummyAttri;
            }
        }
        //------------------------------------------------------
        protected override void BuildArvPort()
        {
            base.BuildArvPort();
            if (2 < m_vArgvPorts.Count)
            {
                m_vArgvPorts[1].bindPort.portColor = m_vArgvPorts[0].bindPort.portColor;
            }
        }
        //------------------------------------------------------
        protected override void CreateOutports()
        {
            base.CreateOutports();
            if (m_vArgvPorts.Count>0 && 0 < m_vReturnPorts.Count && m_vReturnPorts[0].attri.argvType == typeof(IVariable))
            {
                m_vReturnPorts[0].curDummyAttri = m_vArgvPorts[0].curDummyAttri;
            }
        }
        //------------------------------------------------------
        protected override void BuildReturnPort()
        {
            base.BuildReturnPort();
            if (m_vArgvPorts.Count > 0 && 0 < m_vReturnPorts.Count && m_vReturnPorts[0].attri.argvType == typeof(IVariable))
            {
                m_vReturnPorts[0].bindPort.portColor = m_vArgvPorts[0].bindPort.portColor;
            }
        }
        //------------------------------------------------------
        protected override void OnReBuildPortContain(ArvgPort port)
        {
            if(port.slotIndex == 0)
            {
                if (1 < m_vArgvPorts.Count)
                {
                    ChangeVariableType(m_vArgvPorts[1], port.eEnumType);
                    var port2 = m_vArgvPorts[1].bindPort;
                    // 需要重新赋值回去（struct）
                    m_vArgvPorts[1].bindPort.portColor = port.bindPort.portColor;
                    m_vArgvPorts[1].curDummyAttri = port.curDummyAttri;
                    ClearPortVarEle(m_vArgvPorts[1]);
                    InnerDrawPortValue(m_vArgvPorts[1], 2);
                }
            }
            if (port.slotIndex == 1)
            {
                if (1 < m_vArgvPorts.Count)
                {
                    ChangeVariableType(m_vArgvPorts[0], port.eEnumType);
                    var port2 = m_vArgvPorts[0].bindPort;
                    // 需要重新赋值回去（struct）
                    m_vArgvPorts[0].bindPort.portColor = port.bindPort.portColor;
                    m_vArgvPorts[0].curDummyAttri = port.curDummyAttri;
                    ClearPortVarEle(m_vArgvPorts[0]);
                    InnerDrawPortValue(m_vArgvPorts[0], 2);
                }
            }
            if (port.slotIndex == 0 || port.slotIndex == 1)
            {
                if (0 < m_vReturnPorts.Count && m_vReturnPorts[0].attri.argvType == typeof(IVariable))
                {
                    ChangeVariableType(m_vReturnPorts[0], port.eEnumType);
                    var port2 = m_vReturnPorts[0].bindPort;
                    // 需要重新赋值回去（struct）
                    m_vReturnPorts[0].bindPort.portColor = port.bindPort.portColor;
                    m_vReturnPorts[0].curDummyAttri = port.curDummyAttri;
                    ClearPortVarEle(m_vReturnPorts[0]);
                    InnerDrawPortValue(m_vReturnPorts[0], 2);
                }
            }
        }
    }
}
#endif