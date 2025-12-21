/********************************************************************
生成日期:	06:30:2025
类    名: 	ParallelConditionNode
作    者:	HappLI
描    述:	并行条件节点编辑器
*********************************************************************/
#if UNITY_EDITOR
using Framework.AT.Runtime;
using Framework.ED;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Framework.AT.Editor
{
    [EditorBindNode(typeof(ParallelCondition))]
    public class ParallelConditionNode : GraphNode
    {
        Dictionary<ArvgPort, List<VisualElement>> m_Conditions = new Dictionary<ArvgPort, List<VisualElement>>();
        List<ArgvAttribute> m_vAgvs = new List<ArgvAttribute>();
        List<ECompareOpType> m_vOps = new List<ECompareOpType>();
        public ParallelConditionNode(AgentTreeGraphView pAgent, BaseNode pNode, bool bUpdatePos = true) : base(pAgent, pNode, bUpdatePos)
        {
        }
        //------------------------------------------------------
        protected override void OnInit()
        {
            var attri = GetAttri();
            if (attri == null)
                return;
            BuildArgvAttrs();
        }
        //------------------------------------------------------
        void BuildArgvAttrs()
        {
            var attri = GetAttri();
            m_vAgvs.Clear();
            m_vOps.Clear();
            ParallelCondition node = this.bindNode as ParallelCondition;
            if (node.opTypes == null)
                return;
            for (int i =0; i < node.opTypes.Length; ++i)
            {
                ArgvAttribute attr = new ArgvAttribute("条件" + i,typeof(IVariable),true);
                m_vAgvs.Add(attr);
                m_vAgvs.Add(attr);
                m_vOps.Add(node.opTypes[i]);
            }
        }
        //------------------------------------------------------
        protected override void OnStart()
        {
            var buttonBar = new VisualElement();
            buttonBar.style.flexDirection = FlexDirection.Row;
            buttonBar.style.justifyContent = Justify.Center;
            buttonBar.style.marginTop = 4;
            buttonBar.style.marginBottom = 4;

            var btn = new Button(() =>
            {
                OnAddCondtion();
            })
            {
                text = "新增条件"
            };
            btn.style.height = 22;
            btn.style.unityFontStyleAndWeight = FontStyle.Bold;

            buttonBar.Add(btn); 

            this.titleButtonContainer.Add(buttonBar);
        }
        //------------------------------------------------------
        void OnAddCondtion()
        {
            ParallelCondition node = this.bindNode as ParallelCondition;
            List<ECompareOpType> vOps = new List<ECompareOpType>();
            if (node.opTypes != null) vOps.AddRange(node.opTypes);
            vOps.Add(ECompareOpType.eEqual);

            ArgvAttribute attr = new ArgvAttribute("条件" + vOps.Count.ToString(), typeof(IVariable),true);
            m_vAgvs.Add(attr);
            m_vAgvs.Add(attr);
            m_vOps.Add(ECompareOpType.eEqual);
            m_pGraphView.RegisterUndo(this, UndoRedoOperationType.NodeChange);

            CheckPorts();
            BuildPorts();
            this.MarkDirtyRepaint();
        }
        //------------------------------------------------------
        public override void Save(BaseNode pDummy = null)
        {
            base.Save(pDummy);
            ParallelCondition node = this.bindNode as ParallelCondition;
            List<NodePort> vPorts = new List<NodePort>();
            foreach(var db in m_vArgvPorts)
            {
                vPorts.Add(db.nodePort);
            }
            node.argvs = vPorts.ToArray();
            node.opTypes = m_vOps.ToArray();
        }
        //------------------------------------------------------
        protected override void CreateInports()
        {
            if (m_vAgvs.Count <= 0)
                return;

            var inputVars = bindNode.GetInports(true, m_vAgvs.Count);
            if (inputVars != null)
            {
                for (int i = 0; i < m_vAgvs.Count; ++i)
                {
                    var nodePort = inputVars[i];

                    nodePort.pVariable = m_pGraphView.CreateVariable(m_vAgvs[i], nodePort.varGuid);
                    nodePort.varGuid = nodePort.pVariable.GetGuid();
                    inputVars[i] = nodePort;

                    if (nodePort.pVariable == null)
                    {
                        m_vArgvPorts.Add(null);
                        continue;
                    }
                    ArvgPort port = new ArvgPort();
                    port.grapNode = this;
                    port.nodePort = nodePort;
                    port.attri = m_vAgvs[i];
                    port.curDummyAttri = GetDummyLinkPortAttri(port);

                    var opIndex = GetOpCompareIndex(i);
                    if (opIndex >= 0 && opIndex < m_vArgvPorts.Count)
                    {
                        if (m_vArgvPorts[opIndex].curDummyAttri == null) m_vArgvPorts[opIndex].curDummyAttri = port.curDummyAttri;
                        else port.curDummyAttri = m_vArgvPorts[opIndex].curDummyAttri;
                    }
                    port.isInput = true;
                    port.slotIndex = i;
                    m_vArgvPorts.Add(port);
                    m_pGraphView.AddPort(port);
                }
            }
        }
        //------------------------------------------------------
        protected override void CreateOutports()
        {
        }
        //------------------------------------------------------
        protected override void BuildArvPort()
        {
            this.inputContainer.Clear();
            int opIndex = 0;
            for (int i = 0; i < m_vArgvPorts.Count; ++i)
            {
                if (!m_Conditions.TryGetValue(m_vArgvPorts[i], out var visualElements))
                {
                    visualElements = new List<VisualElement>();
                    m_Conditions.Add(m_vArgvPorts[i], visualElements);
                }

                if (i % 2 == 0)
                {
                    int thisIndex = opIndex;
                    var btn = new Button(() =>
                    {
                        if (EditorUtility.DisplayDialog("提示", "确定要移除该条件吗", "移除", "再想想"))
                        {
                            RemoveCondition(thisIndex);
                        }
                    })
                    {
                        text = "条件" + (opIndex+1).ToString()
                    };
                    btn.style.height = 22;
                    btn.style.backgroundColor = Color.gray;
                    btn.style.flexDirection= FlexDirection.Row;
                    btn.style.unityFontStyleAndWeight = FontStyle.Bold;
                    this.inputContainer.Add(btn);

                    visualElements.Add(btn);
                }
                var port = m_vArgvPorts[i];
                var inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(ArvgPort));
                inputPort.tooltip = port.attri.tips;
                if (inputPort.tooltip == null) inputPort.tooltip = "";
                if (inputPort.tooltip.Length > 0) inputPort.tooltip += "\r\n";
                inputPort.tooltip += "端口变量GUID:" + port.GetVariableGuid();
                string portName = (i % 2 == 0) ? "比较值1" : "比较值2";
                inputPort.portName = portName;
                inputPort.portColor = EditorPreferences.GetTypeColor(port.GetVariable().GetType());
                inputPort.source = port;
                inputPort.style.marginRight = 4;

                var fieldRoot = new VisualElement();
                fieldRoot.style.flexDirection = FlexDirection.Row;
                inputPort.Add(fieldRoot);

                this.inputContainer.Add(inputPort);

                m_vArgvPorts[i].fieldRoot = fieldRoot;
                m_vArgvPorts[i].bindPort = inputPort;
                DrawPortValue(port);

                visualElements.Add(inputPort);

                if (i%2==0)
                {  
                    var popup = new PopupField<AT.Runtime.ECompareOpType>("操作条件",
                      AgentTreeUtil.GetPopCompareOpTypes(),
                      m_vOps[opIndex],
                      t => EditorUtils.GetEnumDisplayName(t),
                      t => EditorUtils.GetEnumDisplayName(t)
                    );
                    popup.userData = opIndex;
                    popup.RegisterValueChangedCallback(evt =>
                    {
                        m_pGraphView.RegisterUndo(this, UndoRedoOperationType.NodeChange);
                        int index = (int)popup.userData;
                        m_vOps[index] = (ECompareOpType)evt.newValue;
                    });
                    this.inputContainer.Add(popup);

                    visualElements.Add(popup);
                    opIndex++;
                }
            }
        }
        //------------------------------------------------------
        protected override void BuildReturnPort()
        {
            base.BuildReturnPort();
        }
        //------------------------------------------------------
        protected override void OnReBuildPortContain(ArvgPort port)
        {
            base.OnReBuildPortContain(port);
            int syncIndex = GetOpCompareIndex(port.slotIndex);
            if (syncIndex < 0 || syncIndex >= m_vAgvs.Count)
                return;

            var port1 = m_vArgvPorts[syncIndex];
            port1.eEnumType = port.eEnumType;
            port1.curDummyAttri = port.curDummyAttri;
            var val = m_pGraphView.ChangeVariable(port1.GetVariableGuid(), port.eEnumType);
            port1.bindPort.portColor = EditorPreferences.GetTypeColor(val.GetType());
            port1.nodePort.pVariable = val;
            ReBuildPortContain(port1);
        }
        //------------------------------------------------------
        int GetOpCompareIndex(int port)
        {
            int syncIndex = -1;
            if (port % 2 == 0)
            {
                syncIndex = port + 1;
            }
            else syncIndex = port - 1;
            if (syncIndex < 0 || syncIndex >= m_vAgvs.Count)
                return syncIndex;
            return syncIndex;
        }
        //------------------------------------------------------
        void RemoveCondition(int index)
        {
            if(index>=0 && index < m_vOps.Count)
            {
                m_pGraphView.RegisterUndo(this, UndoRedoOperationType.NodeChange);
                m_vOps.RemoveAt(index);
                {
                    var port = m_vArgvPorts[index];
                    if(port.bindPort!=null)
                    {
                        var connections = port.bindPort.connections;
                        if (connections != null)
                        {
                            foreach (var con in connections)
                            {
                                m_pGraphView.RemoveElement(con);
                            }
                        }
                    }
                    m_pGraphView.DelPort(port);
                    if(m_Conditions.TryGetValue(port, out var list))
                    {
                        foreach(var db in list)
                        {
                            this.inputContainer.Remove(db);
                        }
                        m_Conditions.Remove(port);
                    }
                }
                {
                    var port = m_vArgvPorts[index+1];
                    if (port.bindPort != null)
                    {
                        var connections = port.bindPort.connections;
                        if (connections != null)
                        {
                            foreach (var con in connections)
                            {
                                m_pGraphView.RemoveElement(con);
                            }
                        }
                        m_pGraphView.DelPort(port);
                    }
                    if (m_Conditions.TryGetValue(port, out var list))
                    {
                        foreach (var db in list)
                        {
                            this.inputContainer.Remove(db);
                        }
                        m_Conditions.Remove(port);
                    }
                }
                m_vArgvPorts.RemoveAt(index);
                m_vArgvPorts.RemoveAt(index);
                m_vAgvs.RemoveAt(index);
                m_vAgvs.RemoveAt(index);

                foreach(var db in m_Conditions)
                {
                    var list = db.Value;
                    for(int i =0; i < list.Count; ++i)
                    {
                        if (list[i] is Button)
                        {
                            ((Button)list[i]).text = "条件" + (m_vArgvPorts.IndexOf(db.Key)+1).ToString();
                        }
                    }
                }
            }
        }
    }
}
#endif