/********************************************************************
生成日期:	06:30:2025
类    名: 	GraphNode
作    者:	HappLI
描    述:	基础的节点绘制
*********************************************************************/
#if UNITY_EDITOR
using Framework.AT.Runtime;
using Framework.Core;
using Framework.DrawProps;
using Framework.ED;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Framework.AT.Editor
{
    public class GraphNode : Node
    {
        public AT.Runtime.BaseNode bindNode;
        public GroupNode ownerGroup;

        protected LinkPort m_pLinkIn;
        protected LinkPort m_pLinkOut;

        protected Dictionary<ArvgPort, LinkPort> m_vOtherLinks = new Dictionary<ArvgPort, LinkPort>();

        protected List<ArvgPort> m_vArgvPorts = new List<ArvgPort>();
        protected List<ArvgPort> m_vReturnPorts = new List<ArvgPort>();

        private bool m_isFlashing = false;
        private Color m_lastBgColor = Color.red;
        private double m_flashEndTime = 0;
        private int m_nExternPortCount = 0;

        protected HashSet<ArvgPort> m_vChangePorts = new HashSet<ArvgPort>();

        private Vector2 m_dragStartPos;
        private bool m_isDragging = false;
        private bool m_bUndoRegister = false;

        protected AgentTreeGraphView m_pGraphView;
        public GraphNode(AgentTreeGraphView pAgent, AT.Runtime.BaseNode pNode, bool bUpdatePos = true)
        {
            m_pGraphView = pAgent;
            bindNode = pNode;
            var attri = GetAttri();
            UpdateTitle();

            OnInit();

            AddIcon();
            if (attri != null)
            {
                AddTitlePorts(attri.actionAttr.hasInput, attri.actionAttr.hasOutput);
            }

            CheckPorts();
            BuildPorts();

            UpdatePosition();

            //! refresh
            bool expandLock = this.expanded;
            if(expandLock)
            {
                this.expanded = false;
                this.expanded = true;
            }

           // this.title += "[" + bindNode.guid + "]";
           this.tooltip= "GUID: " + bindNode.guid;

            this.titleContainer.style.fontSize = 50;
            this.titleContainer.style.unityFontStyleAndWeight = FontStyle.Bold;
            if (attri.colorAttr != null)
            {
                if (attri.colorAttr.color.a > 0)
                {
                    if(attri.colorAttr.node)
                        this.style.backgroundColor = attri.colorAttr.color;
                    else
                        this.titleContainer.style.backgroundColor = attri.colorAttr.color;
                }
            }

            this.RegisterCallback<MouseDownEvent>(evt =>
            {
                // 只响应左键
                if (evt.button == 0)
                {
                    OnDragMouseDown();
                }
            });

            OnStart();
        }
        //------------------------------------------------------
        protected virtual void OnStart()
        {

        }
        //------------------------------------------------------
        public void UpdateTitle()
        {
            var attri = GetAttri();
            if (attri != null)
            {
                this.title = attri.displayName;
            }
            else this.title = "Unknown Node";
            if (bindNode!=null && !string.IsNullOrEmpty(bindNode.name))
            {
                this.title += "[" + bindNode.name + "]";
            }
        }
        //------------------------------------------------------
        protected void AddIcon()
        {
            var attri = GetAttri();
            if (attri == null)
                return;
            if (attri.iconAttr != null)
            {
                var icon = AgentTreeUtil.LoadIcon(attri.iconAttr.name);
                if (icon != null)
                {
                    var img = new Image
                    {
                        image = icon,
                        style =
                        {
                            width = 32,
                            height = 32,
                            marginLeft = 10,
                            marginTop = 4,
                        }
                    };
                    // 添加到标题栏或主容器
                    this.titleContainer.Insert(0, img);
                }
            }
        }
        //------------------------------------------------------
        protected void AddPortIcon(ArvgPort port, Texture icon)
        {
            if (icon == null) return;
            var img = new Image
            {
                image = icon,
                style =
                {
                    width = 20,
                    height = 20,
                    marginLeft = 10,
                    marginTop = 4,
                }
            };
            port.bindPort.Add(img);
        }
        //------------------------------------------------------
        protected virtual void OnInit()
        {

        }
        //------------------------------------------------------
        public void OnDragMouseDown()
        {
            m_dragStartPos = this.GetPosition().position;
            m_isDragging = true;
            m_bUndoRegister = false;
        }
        //------------------------------------------------------
        public void OnDragMouseUp()
        {
            if (m_isDragging)
            {
                m_isDragging = false;
                Vector2 endPos = this.GetPosition().position;
                if ((endPos - m_dragStartPos).sqrMagnitude > 0.1f)
                {
                    m_bUndoRegister = true;
                    m_pGraphView.RegisterUndo(this, UndoRedoOperationType.Position,true);
                    m_bUndoRegister = false;
                }
            }
        }
        //------------------------------------------------------
        public void UpdatePosition()
        {
            var rect = this.GetPosition();
            this.SetPosition(new Rect(bindNode.posX * 0.01f, bindNode.posY * 0.01f, rect.width, rect.height));
        }
        //------------------------------------------------------
        public void UpdateSize()
        {
        //    this.style.width = GetWidth();
         //   this.style.height = GetHeight();

            // 让子容器自适应
            this.inputContainer.style.flexGrow = 1;
            this.outputContainer.style.flexGrow = 1;
            this.titleContainer.style.height = 30;

            // 触发布局和重绘
            this.MarkDirtyRepaint();
            this.inputContainer.MarkDirtyRepaint();
            this.outputContainer.MarkDirtyRepaint();
            this.titleContainer.MarkDirtyRepaint();
        }
        //------------------------------------------------------
        public void ClearLinks(bool bClear = true)
        {
            ClearLinkIn(bClear);
            ClearLinkOut(bClear);
            ClearOtherLink(bClear); 
            foreach (var db in m_vArgvPorts)
            {
                var connections = db.bindPort.connections;
                if (connections != null)
                {
                    foreach (var con in connections)
                    {
                        m_pGraphView.RemoveElement(con);
                    }
                }
            }

            foreach (var db in m_vReturnPorts)
            {
                if (db.bindPort != null)
                {
                    var connects = db.bindPort.connections;
                    if (connects != null)
                    {
                        foreach (var edge in connects)
                        {
                            m_pGraphView.RemoveElement(edge);
                        }
                    }
                }
            }
        }
        //------------------------------------------------------
        public void ClearLinkIn(bool bClear = true)
        {
            if (m_pLinkIn != null)
            {
                var connections = m_pLinkIn.bindPort.connections;
                if (connections != null)
                {
                    foreach (var db in connections)
                    {
                        m_pGraphView.RemoveElement(db);
                    }
                }
                if(bClear) m_pLinkIn.bindPort.Clear();
            }
        }
        //------------------------------------------------------
        public void ClearLinkOut(bool bClear = true)
        {
            if (m_pLinkOut != null)
            {
                var connections = m_pLinkOut.bindPort.connections;
                if (connections != null)
                {
                    foreach (var db in connections)
                    {
                        m_pGraphView.RemoveElement(db);
                    }
                }
                if (bClear) m_pLinkOut.bindPort.Clear();
            }
        }
        //------------------------------------------------------
        public void ClearOtherLink(bool bClear = true)
        {
            foreach (var db in m_vOtherLinks)
            {
                var connections = db.Value.bindPort.connections;
                if (connections != null)
                {
                    foreach (var con in connections)
                    {
                        m_pGraphView.RemoveElement(con);
                    }
                }
                if (bClear) db.Value.bindPort.Clear();
            }
        }
        //------------------------------------------------------
        public void Release()
        {
            ClearLinkIn();
            ClearOtherLink();

            foreach (var db in m_vArgvPorts)
            {
                m_pGraphView.DelPort(db);
            }


            foreach (var db in m_vReturnPorts)
            {
                if (db.refReturnPort != null)
                {
                    AddPortIcon(db.refReturnPort, EditorGUIUtility.IconContent("console.erroricon").image);
                    db.refReturnPort.bindPort.portName = "!ERROR!";
                    var connects = db.refReturnPort.bindPort.connections;
                    if(connects!=null)
                    {
                        foreach (var edge in connects)
                        {
                            m_pGraphView.RemoveElement(edge);
                        }
                    }
                }
                if (db.bindPort != null)
                {
                    var connects = db.bindPort.connections;
                    if (connects != null)
                    {
                        foreach (var edge in connects)
                        {
                            m_pGraphView.RemoveElement(edge);
                        }
                    }
                }
                m_pGraphView.DelPort(db);
            }

            m_vArgvPorts.Clear();
            m_vReturnPorts.Clear();
            m_vChangePorts.Clear();
        }
        //------------------------------------------------------
        public LinkPort GetLink(bool isInput)
        {
            if (isInput) return m_pLinkIn;
            return m_pLinkOut;
        }
        //------------------------------------------------------
        public ArvgPort GetArvg(int index)
        {
            if (m_vArgvPorts == null || index < 0 || index >= m_vArgvPorts.Count)
                return m_vArgvPorts[index];
            return null;
        }
        //------------------------------------------------------
        public int IndexOfInport(ArvgPort port)
        {
            return m_vArgvPorts.IndexOf(port);
        }
        //------------------------------------------------------
        public List<ArvgPort> GetArvgs()
        {
            return m_vArgvPorts;
        }
        //------------------------------------------------------
        public ArvgPort GetArvg(short guid)
        {
            if (m_vArgvPorts == null)
                return null;
            for (int i = 0; i < m_vArgvPorts.Count; ++i)
            {
                if (m_vArgvPorts[i].GetVariableGuid() == guid)
                    return m_vArgvPorts[i];
            }
            return null;
        }
        //------------------------------------------------------
        public ArvgPort GetReturn(int index)
        {
            if (m_vReturnPorts == null || index < 0 || index >= m_vReturnPorts.Count)
                return m_vReturnPorts[index];
            return null;
        }
        //------------------------------------------------------
        public int IndexOfOutport(ArvgPort port)
        {
            return m_vReturnPorts.IndexOf(port);
        }
        //-----------------------------------------------------
        public List<ArvgPort> GetReturns()
        {
            return m_vReturnPorts;
        }
        //------------------------------------------------------
        public Dictionary<ArvgPort, LinkPort> GetOtherLinks()
        {
            return m_vOtherLinks;
        }
        //------------------------------------------------------
        public ArvgPort GetReturn(short guid)
        {
            for(int i =0; i < m_vReturnPorts.Count; ++i)
            {
                if (m_vReturnPorts[i].GetVariableGuid() == guid)
                    return m_vReturnPorts[i];
            }
            return null;
        }
        //------------------------------------------------------
        public void OnNotifyExcuted()
        {
            if(m_vArgvPorts!=null)
            {
                foreach (var db in m_vArgvPorts)
                {
                    UpdatePortRuntimeValue(db);
                }
            }
            if (m_vReturnPorts != null)
            {
                foreach (var db in m_vReturnPorts)
                {
                    if (m_vOtherLinks.ContainsKey(db))
                        continue;
                    UpdatePortRuntimeValue(db);
                    if (db.bindPort == null)
                        return;
                    if (db.bindPort.connections != null)
                    {
                        foreach (var connect in db.bindPort.connections)
                        {
                            if (connect.input.source is ArvgPort)
                            {
                                ArvgPort inPort = connect.input.source as ArvgPort;
                                if (inPort.grapNode != null && inPort.grapNode is NewVariableNode)
                                {
                                    inPort.grapNode.FlashRed();
                                    inPort.grapNode.OnNotifyExcuted();
                                }
                            }
                        }
                    }
                }
            }
        }
        //------------------------------------------------------
        public void FlashRed(float duration = 1f)
        {
            if (m_isFlashing) return;
            m_isFlashing = true;
            m_lastBgColor = this.style.backgroundColor.value;
            this.style.backgroundColor = new Color(1f, 0.2f, 0.2f, 1f); // 红色

            m_flashEndTime = duration;
        }
        //------------------------------------------------------
        public void Update(float deltaTime)
        {
            if(m_vChangePorts.Count>0)
            {
                foreach(var port in m_vChangePorts)
                {
                    OnArgvPortVarTypeChanged(port);
                    ReBuildPortContain(port);
                    OnReBuildPortContain(port);
                }
                UpdateSize();
                m_vChangePorts.Clear();
            }
            if (m_isFlashing && m_flashEndTime > 0)
            {
                m_flashEndTime -= deltaTime;
                if (m_flashEndTime <= 0)
                {
                    this.style.backgroundColor = m_lastBgColor;
                    m_isFlashing = false;
                }
            }
        }
        //------------------------------------------------------
        public virtual void Save(BaseNode pDummy = null)
        {
            BaseNode saveNode = pDummy;
            if (saveNode == null) saveNode = bindNode;
            if(m_bUndoRegister)
            {
                saveNode.posX = (int)(this.m_dragStartPos.x * 100);
                saveNode.posY = (int)(this.m_dragStartPos.y * 100);
            }
            else
            {
                saveNode.posX = (int)(this.GetPosition().x * 100);
                saveNode.posY = (int)(this.GetPosition().y * 100);
            }
            List<short> nextActions = new List<short>();
            if(m_pLinkOut!=null)
            {
                var connections = m_pLinkOut.bindPort.connections;
                if(connections!=null)
                {
                    foreach(var db in connections)
                    {
                        if(db.input.source is LinkPort)
                        {
                            var input = (LinkPort)db.input.source;
                            if(!nextActions.Contains(input.graphNode.bindNode.guid)) nextActions.Add(input.graphNode.bindNode.guid);
                        }
                    }
                }
            }
            saveNode.nextActions = nextActions.ToArray();

            foreach(var db in m_vOtherLinks)
            {
                if (db.Value.argvPort == null)
                    continue;
                var pVar = m_pGraphView.GetVariable(db.Value.argvPort.GetVariableGuid());
                if (pVar == null)
                    continue;
                if(pVar is VariableInt)
                {
                    VariableInt pStr = (VariableInt)pVar;
                    List<int> vDo = new List<int>();
                    var connections = db.Value.bindPort.connections;
                    if (connections != null)
                    {
                        foreach (var con in connections)
                        {
                            if (con.input.source is LinkPort)
                            {
                                var input = (LinkPort)con.input.source;
                                if (!vDo.Contains(input.graphNode.bindNode.guid)) vDo.Add(input.graphNode.bindNode.guid);
                            }
                        }
                    }
                    if (vDo.Count > 0)
                        pStr.value = vDo[0];
                    else pStr.value = -1;
                        m_pGraphView.UpdateVariable(pStr);
                }
            }

            if(m_vArgvPorts!=null)
            {
                foreach(var db in m_vArgvPorts)
                {
                    db.RefreshDummyPorts();
                }

                var inputVars = saveNode.GetInports(false);
                if(inputVars!=null)
                {
                    for (int i = 0; i < m_vArgvPorts.Count && i < inputVars.Length; ++i)
                    {
                        inputVars[i] = m_vArgvPorts[i].nodePort;
                        if (m_vOtherLinks.ContainsKey(m_vArgvPorts[i]))
                            continue;
                        m_pGraphView.UpdatePortVariableDefault(m_vArgvPorts[i]);
                    }
                }
            }
            if (m_vReturnPorts != null)
            {
                var portVars = saveNode.GetOutports(false);
                if (portVars != null)
                {
                    for (int i = 0; i < m_vReturnPorts.Count && i < portVars.Length; ++i)
                    {
                        portVars[i] = m_vReturnPorts[i].nodePort;
                        if (m_vOtherLinks.ContainsKey(m_vReturnPorts[i]))
                            continue;
                        m_pGraphView.UpdatePortVariableDefault(m_vReturnPorts[i]);
                    }
                }
            }
        }
        //------------------------------------------------------
        public virtual void AddTitlePorts(bool hasIn, bool hasOut)
        {
            // 创建左侧 input port
            if(hasIn)
            {
                m_pLinkIn = new LinkPort();
                m_pLinkIn.graphNode = this;

                var inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, UnityEditor.Experimental.GraphView.Port.Capacity.Multi, typeof(LinkPort));
                inputPort.portName = ""; // 不显示名字
                inputPort.style.marginRight = 4;
                inputPort.style.backgroundImage = IconUtils.linkOuter;
                inputPort.style.backgroundColor = Color.clear;
                inputPort.style.width = 16;
                inputPort.style.height = 16;
                inputPort.portColor = EditorPreferences.GetSettings().linkLineColor;
                inputPort.source = m_pLinkIn;
                titleContainer.Insert(0, inputPort);
                m_pLinkIn.bindPort = inputPort;
            }


            // 创建右侧 output port
            if(hasOut)
            {
                m_pLinkOut = new LinkPort();
                m_pLinkOut.graphNode = this;
                var outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, UnityEditor.Experimental.GraphView.Port.Capacity.Multi, typeof(LinkPort));
                outputPort.portName = ""; // 不显示名字
                outputPort.style.marginLeft = 4;
                outputPort.style.backgroundImage = IconUtils.linkOuter;
                outputPort.style.backgroundColor = Color.clear;
                outputPort.style.width = 16;
                outputPort.style.height = 16;
                outputPort.portColor = EditorPreferences.GetSettings().linkLineColor;
                outputPort.source = m_pLinkOut;
                titleContainer.Add(outputPort);
                m_pLinkOut.bindPort = outputPort;
            }
        }
        //------------------------------------------------------
        public AgentTreeAttri GetAttri()
        {
            if (bindNode == null) return null;
            int customType = 0;
            if(bindNode is AT.Runtime.CustomEvent)
            {
                customType = ((AT.Runtime.CustomEvent)bindNode).eventType;
            }
            return AgentTreeUtil.GetAttri(bindNode.type, customType);
        }
        //------------------------------------------------------
        public bool CanChangeValue(ArvgPort port)
        {
            return port.attri.canEdit;// && !m_pGraphView.isDebugPort;
        }
        //------------------------------------------------------
        protected virtual void OnArgvPortChanging(ArvgPort port)
        {
            m_pGraphView.RegisterUndo(this, UndoRedoOperationType.NodeChange);
        }
        //------------------------------------------------------
        protected virtual void OnArgvPortChanged(ArvgPort port)
        {
            if (port == null || port.bindPort == null)
                return;
        }
        //------------------------------------------------------
        protected void ClearPortVarEle(ArvgPort port)
        {
            if (port.fieldRoot != null)
            {
                if (port.fieldElement != null)
                    port.fieldRoot.Remove(port.fieldElement);

                if (port.enumPopFieldElement != null)
                    port.bindPort.Remove(port.enumPopFieldElement);

                port.enumPopFieldElement = null;
                port.fieldElement = null;
                port.fieldRoot.Clear();
            }
        }
        //------------------------------------------------------
        protected virtual void OnArgvPortVarTypeChanged(ArvgPort port)
        {
            if (port == null || port.bindPort == null)
                return;

            port.curDummyAttri = GetDummyLinkPortAttri(port);
            var val = m_pGraphView.ChangeVariable(port.GetVariableGuid(), port.eEnumType);
            // 需要重新赋值回去（struct）
            port.bindPort.portColor = EditorPreferences.GetTypeColor(val.GetType());
            port.nodePort.pVariable = val;
        }
        //------------------------------------------------------
        public void ChangeVariableType(ArvgPort port, Runtime.EVariableType newType)
        {
            var val2 = m_pGraphView.ChangeVariable(port.GetVariableGuid(), newType);
            port.nodePort.pVariable = val2;
        }
        //------------------------------------------------------
        protected ArgvAttribute GetDummyLinkPortAttri(ArvgPort port)
        {
            return GetDummyLinkPortAttri(port.nodePort);
        }
        //------------------------------------------------------
        protected virtual ArgvAttribute GetDummyLinkPortAttri(NodePort port)
        {
            if (port.dummyPorts != null && port.dummyPorts.Length > 0)
            {
                for (int i = 0; i < port.dummyPorts.Length; ++i)
                {
                    var dummyNode = m_pGraphView.GetBaseNode(port.dummyPorts[i].guid);
                    if (dummyNode == null)
                    {
                        continue;
                    }
                    AgentTreeAttri attri = null;
                    if (dummyNode != null && dummyNode.type == (int)EActionType.eGetVariable)
                    {
                        var ports = dummyNode.GetOutports(false);
                        if(ports!=null && ports.Length>0)
                        {
                            var castDummyNode = m_pGraphView.GetVarOwnerBaseNode(ports[0].varGuid);
                            if (castDummyNode != null)
                                dummyNode = castDummyNode;
                        }
                        attri = AgentTreeUtil.GetAttri(dummyNode);
                    }
                    else
                        attri = AgentTreeUtil.GetAttri(dummyNode);
                    if (attri == null)
                        continue;
                    List<ArgvAttribute> slotAttrs = null;
                    if (port.dummyPorts[i].type == 0)
                    {
                        slotAttrs = attri.argvs;
                    }
                    else
                    {
                        slotAttrs = attri.returns;
                    }


                    if (slotAttrs != null && port.dummyPorts[i].slotIndex < slotAttrs.Count)
                    {
                        return slotAttrs[port.dummyPorts[i].slotIndex];
                    }
                }
            }
            return null;
        }
        //------------------------------------------------------
        public void CheckPorts()
        {
            m_vChangePorts.Clear();
            foreach (var db in m_vArgvPorts)
                m_pGraphView.DelPort(db);

            foreach (var db in m_vReturnPorts)
                m_pGraphView.DelPort(db);

            m_vArgvPorts.Clear();
            m_vReturnPorts.Clear();
            this.inputContainer.Clear();
            this.outputContainer.Clear();

            CreateInports();
            CreateOutports();
        }
        //------------------------------------------------------
        protected virtual void CreateInports()
        {
            var attr = GetAttri();
            if (attr == null)
                return;
            if (attr.argvs.Count > 0)
            {
                var inputVars = bindNode.GetInports(true, attr.argvs.Count);
                if(inputVars!=null)
                {
                    for (int i = 0; i < attr.argvs.Count; ++i)
                    {
                        var nodePort = inputVars[i];

                        nodePort.pVariable = m_pGraphView.CreateVariable(attr.argvs[i], nodePort.varGuid);
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
                        port.attri = attr.argvs[i];
                        port.curDummyAttri = GetDummyLinkPortAttri(port);
                        port.isInput = true;
                        port.slotIndex = i;
                        m_vArgvPorts.Add(port);
                        m_pGraphView.AddPort(port);
                    }
                }
            }
            else
                bindNode.GetInports(true, 0);

        }
        //------------------------------------------------------
        protected virtual void CreateOutports()
        {
            var attr = GetAttri();
            if (attr == null)
                return;

            if (attr.returns.Count > 0)
            {
                var outportVar = bindNode.GetOutports(true, attr.returns.Count);
                for (int i = 0; i < attr.returns.Count; ++i)
                {
                    var nodePort = outportVar[i];
                    nodePort.pVariable = m_pGraphView.CreateVariable(attr.returns[i], nodePort.varGuid);
                    nodePort.varGuid = nodePort.pVariable.GetGuid();
                    outportVar[i] = nodePort;
                    if (nodePort.pVariable == null)
                    {
                        m_vReturnPorts.Add(null);
                        continue;
                    }
                    ArvgPort port = new ArvgPort();
                    port.grapNode = this;
                    port.nodePort = nodePort;
                    port.attri = attr.returns[i];
                    port.curDummyAttri = GetDummyLinkPortAttri(nodePort);
                    port.isInput = false;
                    port.slotIndex = i;
                    m_vReturnPorts.Add(port);
                    m_pGraphView.AddPort(port);
                }
            }
            else
                bindNode.GetOutports(true, 0);
        }
        //------------------------------------------------------
        protected void BuildPorts()
        {
            BuildArvPort();
            BuildReturnPort();
        }
        //------------------------------------------------------
        protected virtual void BuildArvPort()
        {
            this.inputContainer.Clear();
            for(int i =0; i < m_vArgvPorts.Count; ++i)
            {
                var port = m_vArgvPorts[i];
                var inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(ArvgPort));
                inputPort.tooltip = port.attri.tips;
                if (inputPort.tooltip == null) inputPort.tooltip = "";
                if (inputPort.tooltip.Length > 0) inputPort.tooltip += "\r\n";
                inputPort.tooltip += "端口变量GUID:" + port.GetVariableGuid();
                if (port.GetVariable().GetVariableType() == EVariableType.eUserData)
                {
                    inputPort.portName = "";
                    inputPort.tooltip += "\r\n变量名:" + port.attri.name;
                }
                else
                    inputPort.portName = port.attri.name;
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
            }
        }
        //------------------------------------------------------
        protected virtual void BuildReturnPort()
        {
            var attris = GetAttri();
            this.outputContainer.Clear();
            for (int i = 0; i < m_vReturnPorts.Count; ++i)
            {
                var port = m_vReturnPorts[i];
                if (attris.linkAttributes.TryGetValue(attris.returns[i], out var linkAgvAttr))
                {
                    LinkPort linkPort = new LinkPort();
                    linkPort.graphNode = this;
                    linkPort.argvPort = port;
                    var outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, UnityEditor.Experimental.GraphView.Port.Capacity.Single, typeof(LinkPort));
                    outputPort.tooltip = linkAgvAttr.tips;
                    if (outputPort.tooltip == null) outputPort.tooltip = "";
                    if (outputPort.tooltip.Length > 0) outputPort.tooltip += "\r\n";
                    outputPort.tooltip += "端口变量GUID:" + port.GetVariableGuid();
                    if (port.GetVariable().GetVariableType() == EVariableType.eUserData)
                    {
                        outputPort.portName = "";
                        outputPort.tooltip += "\r\n变量名:" + port.attri.name;
                    }
                    else
                        outputPort.portName = port.attri.name;

                    outputPort.style.marginLeft = 4;
                    outputPort.style.marginTop = (m_vReturnPorts.Count+m_vArgvPorts.Count-1)*16;
                    outputPort.style.backgroundImage = IconUtils.linkOuter1;
                    outputPort.style.backgroundColor = Color.clear;
                    outputPort.style.width = 16;
                    outputPort.style.height = 16;
                    outputPort.portColor = EditorPreferences.GetSettings().linkLine1Color;
                    outputPort.source = linkPort;
                    linkPort.bindPort  = outputPort;
                    m_vOtherLinks[port] = linkPort;

                    port.bindPort = outputPort;
                    this.outputContainer.Add(outputPort);
                }
                else
                {
                    var inputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(ArvgPort));
                    inputPort.tooltip = port.attri.tips;
                    if (inputPort.tooltip == null) inputPort.tooltip = "";
                    if (inputPort.tooltip.Length > 0) inputPort.tooltip += "\r\n";
                    inputPort.tooltip += "端口变量GUID:" + port.GetVariableGuid();
                    if (port.GetVariable().GetVariableType() == EVariableType.eUserData)
                    {
                        inputPort.portName = "";
                        inputPort.tooltip += "\r\n变量名:" + port.attri.name;
                    }
                    else
                        inputPort.portName = port.attri.name;
                    inputPort.portColor = EditorPreferences.GetTypeColor(port.GetVariable().GetType());
                    inputPort.source = port;
                    inputPort.style.marginRight = 4;

                    var fieldRoot = new VisualElement();
                    fieldRoot.style.flexDirection = FlexDirection.Row;
                    inputPort.Add(fieldRoot);
                    this.inputContainer.Add(inputPort);

                    m_vReturnPorts[i].fieldRoot = fieldRoot;
                    m_vReturnPorts[i].bindPort = inputPort;
                    DrawPortValue(port);

                    inputPort.AddManipulator(new ContextualMenuManipulator(evt =>
                    {
                        evt.menu.AppendAction("变量引用", action =>
                        {
                            AT.Runtime.ActionNode pAction = new AT.Runtime.ActionNode();
                            pAction.type = (int)EActionType.eGetVariable;
                            pAction.guid = m_pGraphView.GeneratorGUID();
                            var nodePorts = pAction.GetOutports(true, 1);
                            nodePorts[0].varGuid = (inputPort.source as ArvgPort).GetVariableGuid();
                            pAction.nextActions = null;

                            var windowPos = inputPort.GetPosition() ;
                            var nodePos = this.GetPosition().position;
                            pAction.posX = (int)((windowPos.x + nodePos.x+ 300) * 100);
                            pAction.posY = (int)((windowPos.y + nodePos.y) * 100);
                            m_pGraphView.AddNode(pAction);
                        });
                    }));
                }
            }
        }
        //------------------------------------------------------
        public void ReBuildPortContain(ArvgPort port)
        {
            var dummyAttr = GetDummyLinkPortAttri(port);
            bool bDirty = (port.curDummyAttri != dummyAttr);
            port.curDummyAttri = dummyAttr;
            ClearPortVarEle(port);
            DrawPortValue(port);

            if (bDirty)
                OnReBuildPortContain(port);
        }
        //------------------------------------------------------
        protected virtual void OnReBuildPortContain(ArvgPort port) { }
        //------------------------------------------------------
        protected virtual void DrawPortValue(ArvgPort port)
        {
            ArgvAttribute attri = port.attri;
            if (port.curDummyAttri != null) attri = port.curDummyAttri;
            bool hasDummy = (port.nodePort.dummyPorts != null && port.nodePort.dummyPorts.Length > 0);
            if (port.fieldRoot!=null)
            {
                if (hasDummy || !port.attri.canEdit)
                    port.fieldRoot.SetEnabled(false);
                else
                    port.fieldRoot.SetEnabled(true);
            }

            if (attri.argvType == typeof(AT.Runtime.IVariable))
            {
                short varGuid = port.GetVariableGuid();
                var portVariable = m_pGraphView.GetVariable(varGuid);
                if(portVariable!=null)
                {
                    port.eEnumType = portVariable.GetVariableType();
                }

                InnerDrawPortValue(port);
                bool bDrawEnumPopVarType = true;
                if(port.slotIndex>0)
                {
                    //// 如果是输入端口的第一个参数，且类型是变量类型，则不显示枚举类型选择
                    //if (port.isInput && m_vArgvPorts[0].attri.argvType == typeof(AT.Runtime.IVariable))
                    //{
                    //    bDrawEnumPopVarType = false;
                    //}
                }
                if(bDrawEnumPopVarType && port.isInput)
                {
                    if (attri.limitVarTypes != null && attri.limitVarTypes.Length>0)
                    {
                        var popList = new List<AT.Runtime.EVariableType>(attri.limitVarTypes);
                        if (!popList.Contains(port.eEnumType))
                            port.eEnumType = popList[0];
                        var popup = new PopupField<AT.Runtime.EVariableType>(
                            popList,
                            port.eEnumType,
                            t => EditorUtils.GetEnumDisplayName(t),
                            t => EditorUtils.GetEnumDisplayName(t)
                        );
                        popup.style.width = 80;
                        popup.style.marginLeft = 4;
                        popup.style.unityTextAlign = TextAnchor.MiddleRight;

                        popup.SetEnabled(!hasDummy && port.fieldRoot.enabledSelf);

                        port.enumPopFieldElement = popup;

                        popup.RegisterValueChangedCallback(evt =>
                        {
                            OnArgvPortChanging(port);
                            port.eEnumType = evt.newValue;
                            for (int i = 0; i < m_vArgvPorts.Count; ++i)
                            {
                                m_vArgvPorts[i].eEnumType = port.eEnumType;
                            }
                            m_vArgvPorts.Add(port);
                        });

                        port.bindPort.Add(popup);
                    }
                    else
                    {
                        var popup = new PopupField<AT.Runtime.EVariableType>(
                            AgentTreeUtil.GetPopEnumTypes(),
                            port.eEnumType,
                            t => EditorUtils.GetEnumDisplayName(t),
                            t => EditorUtils.GetEnumDisplayName(t)
                        );
                        popup.style.width = 80;
                        popup.style.marginLeft = 4;
                        popup.style.unityTextAlign = TextAnchor.MiddleRight;
                        popup.SetEnabled( !hasDummy && port.fieldRoot.enabledSelf );
                        popup.RegisterValueChangedCallback(evt =>
                        {
                            OnArgvPortChanging(port);
                            // 更新变量值
                            if ((AT.Runtime.EVariableType)evt.newValue != Runtime.EVariableType.eNone)
                            {
                                port.eEnumType = (AT.Runtime.EVariableType)evt.newValue;
                                m_vChangePorts.Add(port);
                            }
                        });
                        port.enumPopFieldElement = popup;
                        port.bindPort.Add(popup);
                    }
                }
                return;
            }
            InnerDrawPortValue(port);
        }
        //------------------------------------------------------
        protected virtual void InnerDrawPortValue(ArvgPort port, int insertIndex = -1)
        {
            ArgvAttribute attri = port.attri;
            if (port.curDummyAttri != null) attri = port.curDummyAttri;
            if (port.fieldElement!=null)
            {
                port.fieldRoot.Remove(port.fieldElement);
            }
            port.fieldRoot.style.minHeight = 0;
            port.bindPort.style.marginTop = 0;

            short varGuid = port.GetVariableGuid();
            var portVariable = m_pGraphView.GetVariable(varGuid);
            if (portVariable == null)
                return;

            if(!string.IsNullOrEmpty(attri.methodDrawer))
            {
                var drawElement = AgentTreeUtil.DrawCustomElement(attri.methodDrawer, port, portVariable, (retObj) =>
                {
                    if (CanChangeValue(port))
                    {
                        OnArgvPortChanging(port);
                        m_pGraphView.UpdateVariable(portVariable);
                        OnArgvPortChanged(port);
                    }
                },120);
                if(drawElement!=null)
                {
                    port.fieldRoot.Add(drawElement);
                    port.fieldElement = drawElement;
                    return;
                }
            }

            if (portVariable is AT.Runtime.VariableInt && attri.argvType.IsEnum)
            {
                var intVar = (AT.Runtime.VariableInt)portVariable;
                var popup = CreateEnumPopupFieldWithDisplay(
                    attri.argvType,
                    intVar.value,
                    v => {
                        if (CanChangeValue(port))
                        {
                            OnArgvPortChanging(port);
                            intVar.value = v;
                            OnArgvPortChanged(port);
                            m_pGraphView.UpdateVariable(intVar);
                        }
                    },
                     port.attri.canEdit
                );

                // 设置运行时变量的值
                var runtimeVar = portVariable;// m_pGraphView.GetRuntimeVariable(port);
                if (runtimeVar != null && runtimeVar is AT.Runtime.VariableInt runtimeInt)
                {
                    int idx = popup.choices.IndexOf(
                        Framework.ED.EditorUtils.GetEnumDisplayName(attri.argvType, runtimeInt.value)
                    );
                    if (idx >= 0)
                        popup.index = idx;
                }

                port.fieldRoot.Add(popup);
                port.fieldElement = popup;
            }
            else if (portVariable is AT.Runtime.VariableInt intVar)
            {
                DrawField<IntegerField, int>(
                    intVar, port, intVar.value,
                    () => new IntegerField { style = { width = 120, marginLeft = 4, unityTextAlign = TextAnchor.MiddleRight } },
                    (f, v) => f.value = v,
                    f => f.value,
                    (f, cb) => f.RegisterValueChangedCallback(cb)
                );
            }
            else if (portVariable is AT.Runtime.VariableObjId objIdVar)
            {
                DrawField<IntegerField, int>(
                    objIdVar, port, objIdVar.value.id,
                    () => new IntegerField { style = { width = 120, marginLeft = 4, unityTextAlign = TextAnchor.MiddleRight } },
                    (f, v) => f.value = v,
                    f => f.value,
                    (f, cb) => f.RegisterValueChangedCallback(cb)
                );
            }
            else if (portVariable is AT.Runtime.VariableBool boolVar)
            {
                DrawField<Toggle, bool>(
                    boolVar, port, boolVar.value,
                    () => new Toggle { style = { width = 120, marginLeft = 4, unityTextAlign = TextAnchor.MiddleRight } },
                    (f, v) => f.value = v,
                    f => f.value,
                    (f, cb) => f.RegisterValueChangedCallback(cb)
                );
            }
            else if (portVariable is AT.Runtime.VariableFloat floatVar)
            {
                DrawField<FloatField, float>(
                    floatVar, port, floatVar.value,
                    () => new FloatField { style = { width = 120, marginLeft = 4, unityTextAlign = TextAnchor.MiddleRight } },
                    (f, v) => f.value = v,
                    f => f.value,
                    (f, cb) => f.RegisterValueChangedCallback(cb)
                );
            }
            else if (portVariable is AT.Runtime.VariableString strVar)
            {
                DrawField<TextField, string>(
                    strVar, port, strVar.value,
                    () => new TextField { style = { width = 120, marginLeft = 4, unityTextAlign = TextAnchor.MiddleRight } },
                    (f, v) => f.value = v,
                    f => f.value,
                    (f, cb) => f.RegisterValueChangedCallback(cb)
                );
            }
            else if (portVariable is AT.Runtime.VariableVec2 vec2Var)
            {
                DrawField<Vector2Field, Vector2>(
                    vec2Var, port, vec2Var.value,
                    () => new Vector2Field { style = { width = 120, marginLeft = 4, unityTextAlign = TextAnchor.MiddleRight } },
                    (f, v) => f.value = v,
                    f => f.value,
                    (f, cb) => f.RegisterValueChangedCallback(cb)
                );
            }
            else if (portVariable is AT.Runtime.VariableVec3 vec3Var)
            {
                DrawField<Vector3Field, Vector3>(
                    vec3Var, port, vec3Var.value,
                    () => new Vector3Field { style = { width = 200, marginLeft = 4, unityTextAlign = TextAnchor.MiddleRight } },
                    (f, v) => f.value = v,
                    f => f.value,
                    (f, cb) => f.RegisterValueChangedCallback(cb)
                );
            }
            else if (portVariable is AT.Runtime.VariableVec4 vec4Var)
            {
                DrawField<Vector4Field, Vector4>(
                    vec4Var, port, vec4Var.value,
                    () => new Vector4Field { style = { width = 260, marginLeft = 4, unityTextAlign = TextAnchor.MiddleRight } },
                    (f, v) => f.value = v,
                    f => f.value,
                    (f, cb) => f.RegisterValueChangedCallback(cb)
                );
            }
            else if (portVariable is AT.Runtime.VariableLong longVar)
            {
                DrawField<LongField, long>(
                    longVar, port, longVar.value,
                    () => new LongField { style = { width = 120, marginLeft = 4, unityTextAlign = TextAnchor.MiddleRight } },
                    (f, v) => f.value = v,
                    f => f.value,
                    (f, cb) => f.RegisterValueChangedCallback(cb)
                );
            }
            else if (portVariable is AT.Runtime.VariableDouble doubleVar)
            {
                DrawField<DoubleField, double>(
                    doubleVar, port, doubleVar.value,
                    () => new DoubleField { style = { width = 120, marginLeft = 4, unityTextAlign = TextAnchor.MiddleRight } },
                    (f, v) => f.value = v,
                    f => f.value,
                    (f, cb) => f.RegisterValueChangedCallback(cb)
                );
            }
            else if (portVariable is AT.Runtime.VariableColor colorVar)
            {
                DrawField<ColorField, Color>(
                    colorVar, port, colorVar.value,
                    () => new ColorField { style = { width = 120, marginLeft = 4, unityTextAlign = TextAnchor.MiddleRight } },
                    (f, v) => f.value = v,
                    f => f.value,
                    (f, cb) => f.RegisterValueChangedCallback(cb)
                );
            }
            else if (portVariable is AT.Runtime.VariableRect rectVar)
            {
                DrawField<RectField, Rect>(
                    rectVar, port, rectVar.value,
                    () => new RectField { style = { width = 120, marginLeft = 4, unityTextAlign = TextAnchor.MiddleRight } },
                    (f, v) => f.value = v,
                    f => f.value,
                    (f, cb) => f.RegisterValueChangedCallback(cb)
                );
                port.bindPort.style.marginTop = 5;
            }
            else if (portVariable is AT.Runtime.VariableQuaternion drawVar)
            {
                VariableVec4 tempVec = new VariableVec4();
                tempVec.value = new Vector4(drawVar.value.x, drawVar.value.y, drawVar.value.z, drawVar.value.w);
                DrawField<Vector4Field, Vector4>(
                    drawVar, port, tempVec.value,
                    () => new Vector4Field { style = { width = 260, marginLeft = 4, unityTextAlign = TextAnchor.MiddleRight } },
                    (f, v) => { f.value = v; drawVar.value = new Quaternion(v.x, v.y, v.z, v.w); },
                    f => f.value,
                    (f, cb) => f.RegisterValueChangedCallback(cb)
                );
            }
            else if (portVariable is AT.Runtime.VariableRay rayVar)
            {
                m_nExternPortCount += 1;
                // Ray没有内置UI控件，使用两个Vector3Field分别编辑origin和direction
                var container = new VisualElement { style = { flexDirection = FlexDirection.Column } };

                var originField = new Vector3Field()
                {
                    value = rayVar.value.origin,
                    style = { width = 150, marginLeft = 4, unityTextAlign = TextAnchor.MiddleLeft }
                };
                var dirField = new Vector3Field()
                {
                    value = rayVar.value.direction,
                    style = { width = 150, marginLeft = 4, unityTextAlign = TextAnchor.MiddleLeft }
                };

                originField.SetEnabled(port.attri.canEdit);
                dirField.SetEnabled(port.attri.canEdit);

                // 设置运行时值
                var runtimeVar = portVariable;// m_pGraphView.GetRuntimeVariable(port);
                if (runtimeVar is AT.Runtime.VariableRay runtimeRay)
                {
                    originField.value = runtimeRay.value.origin;
                    dirField.value = runtimeRay.value.direction;
                }

                originField.RegisterValueChangedCallback(evt =>
                {
                    if (CanChangeValue(port))
                    {
                        OnArgvPortChanging(port);
                        rayVar.value.origin = evt.newValue;
                        OnArgvPortChanged(port);
                        m_pGraphView.UpdateVariable(rayVar);
                    }
                });
                dirField.RegisterValueChangedCallback(evt =>
                {
                    if (CanChangeValue(port))
                    {
                        OnArgvPortChanging(port);
                        rayVar.value.direction = evt.newValue;
                        OnArgvPortChanged(port);
                        m_pGraphView.UpdateVariable(rayVar);
                    }
                });
                container.Add(originField);
                container.Add(dirField);

                port.fieldRoot.Add(container);
                port.fieldElement = container;
                port.bindPort.style.marginTop = 10;
                port.fieldRoot.style.minHeight = 50;
                port.bindPort.tooltip += "\r\n射线起始位置和射线方向";
            }
            else if (portVariable is AT.Runtime.VariableBounds boundsVar)
            {
                m_nExternPortCount += 1;
                // Bounds没有内置UI控件，使用两个Vector3Field分别编辑center和size
                var container = new VisualElement { style = { flexDirection = FlexDirection.Column } };

                var centerField = new Vector3Field()
                {
                    value = boundsVar.value.center,
                    style = { width = 120, marginLeft = 4, unityTextAlign = TextAnchor.MiddleLeft }
                };
                var sizeField = new Vector3Field()
                {
                    value = boundsVar.value.size,
                    style = { width = 120, marginLeft = 4, unityTextAlign = TextAnchor.MiddleLeft }
                };

                centerField.SetEnabled(port.attri.canEdit);
                sizeField.SetEnabled(port.attri.canEdit);

                var runtimeVar = portVariable;// m_pGraphView.GetRuntimeVariable(port);
                if (runtimeVar is AT.Runtime.VariableBounds runtimeBounds)
                {
                    centerField.value = runtimeBounds.value.center;
                    sizeField.value = runtimeBounds.value.size;
                }

                centerField.RegisterValueChangedCallback(evt =>
                {
                    if (CanChangeValue(port))
                    {
                        OnArgvPortChanging(port);
                        boundsVar.value.center = evt.newValue;
                        OnArgvPortChanged(port);
                        m_pGraphView.UpdateVariable(boundsVar);
                    }
                });
                sizeField.RegisterValueChangedCallback(evt =>
                {
                    if (CanChangeValue(port))
                    {
                        OnArgvPortChanging(port);
                        boundsVar.value.size = evt.newValue;
                        OnArgvPortChanged(port);
                        m_pGraphView.UpdateVariable(boundsVar);
                    }
                });
                container.Add(centerField);
                container.Add(sizeField);
                port.fieldRoot.Add(container);
                port.fieldElement = container;
                port.bindPort.style.marginTop = 10;
                port.fieldRoot.style.minHeight = 50;
                port.bindPort.tooltip += "\r\n包围盒中心点和包围盒大小";

            }
            else if (portVariable is AT.Runtime.VariableMatrix matrixVar)
            {
                m_nExternPortCount += 3;
                // 用四个 Vector4Field 分别编辑 Matrix4x4 的四行
                var container = new VisualElement { style = { flexDirection = FlexDirection.Column } };

                Vector4 GetRow(Matrix4x4 m, int row)
                {
                    switch (row)
                    {
                        case 0: return new Vector4(m.m00, m.m01, m.m02, m.m03);
                        case 1: return new Vector4(m.m10, m.m11, m.m12, m.m13);
                        case 2: return new Vector4(m.m20, m.m21, m.m22, m.m23);
                        case 3: return new Vector4(m.m30, m.m31, m.m32, m.m33);
                        default: return Vector4.zero;
                    }
                }
                void SetRow(ref Matrix4x4 m, int row, Vector4 v)
                {
                    switch (row)
                    {
                        case 0: m.m00 = v.x; m.m01 = v.y; m.m02 = v.z; m.m03 = v.w; break;
                        case 1: m.m10 = v.x; m.m11 = v.y; m.m12 = v.z; m.m13 = v.w; break;
                        case 2: m.m20 = v.x; m.m21 = v.y; m.m22 = v.z; m.m23 = v.w; break;
                        case 3: m.m30 = v.x; m.m31 = v.y; m.m32 = v.z; m.m33 = v.w; break;
                    }
                }

                var fields = new Vector4Field[4];
                for (int i = 0; i < 4; ++i)
                {
                    fields[i] = new Vector4Field()
                    {
                        value = GetRow(matrixVar.value, i),
                        style = { width = 260, marginLeft = 4, unityTextAlign = TextAnchor.MiddleLeft }
                    };
                    fields[i].SetEnabled(port.attri.canEdit);
                    int rowIdx = i;
                    fields[i].RegisterValueChangedCallback(evt =>
                    {
                        if (CanChangeValue(port))
                        {
                            OnArgvPortChanging(port);
                            var mat = matrixVar.value;
                            SetRow(ref mat, rowIdx, evt.newValue);
                            matrixVar.value = mat;
                            OnArgvPortChanged(port);
                            m_pGraphView.UpdateVariable(matrixVar);
                        }
                    });
                    container.Add(fields[i]);
                }

                // 设置运行时值
                var runtimeVar = portVariable;// m_pGraphView.GetRuntimeVariable(port);
                if (runtimeVar is AT.Runtime.VariableMatrix runtimeMatrix)
                {
                    for (int i = 0; i < 4; ++i)
                        fields[i].value = GetRow(runtimeMatrix.value, i);
                }

                port.fieldRoot.Add(container);
                port.fieldElement = container;
                port.bindPort.style.marginTop = 50;
                port.fieldRoot.style.minHeight = 120;
            }
            else if (portVariable is AT.Runtime.VariableUserData)
            {
                var userVar = (AT.Runtime.VariableUserData)portVariable;
                var popup = CreateClassPopupFieldWithDisplay(
                    userVar.value,
                    userVar.pPointer,
                    v => {
                        if (CanChangeValue(port))
                        {
                            OnArgvPortChanging(port);
                            userVar.value = ATRtti.GetClassTypeId(v);
                            OnArgvPortChanged(port);
                            m_pGraphView.UpdateVariable(userVar);
                        }
                    },
                     port.attri.canEdit
                );

                // 设置运行时变量的值
                var runtimeVar = portVariable;// m_pGraphView.GetRuntimeVariable(port);
                if (runtimeVar != null && runtimeVar is AT.Runtime.VariableInt runtimeInt)
                {
                    var popupField = popup.Q<PopupField<string>>();
                    System.Type type = ATRtti.GetClassType(runtimeInt.value);
                    int idx = 0;
                    if(type!=null)
                    {
                        popupField.choices.IndexOf(type.FullName.Replace("+", "."));
                    }
                    if (idx >= 0)
                        popupField.index = idx;
                }

                port.fieldRoot.Add(popup);
                port.fieldElement = popup;
            }
        }
        //-----------------------------------------------------
        public static PopupField<string> CreateEnumPopupFieldWithDisplay(
        Type enumType,
        int curValue,
        Action<int> onValueChanged,
        bool canEdit = true,
        int width = 120)
        {
            Array enumValues = Enum.GetValues(enumType);
            List<string> displayNames = new List<string>();
            List<int> valueList = new List<int>();
            foreach (var val in enumValues)
            {
                string name = Enum.GetName(enumType, val);
                FieldInfo fi = enumType.GetField(name);
                string displayName = name;
                if (fi != null && fi.IsDefined(typeof(DisplayAttribute), false))
                {
                    var attr = fi.GetCustomAttribute<DisplayAttribute>();
                    if (!string.IsNullOrEmpty(attr.displayName))
                        displayName = attr.displayName;
                }
                displayNames.Add(displayName);
                valueList.Add(Convert.ToInt32(val));
            }

            int curIndex = valueList.IndexOf(curValue);
            if (curIndex < 0) curIndex = 0;

            var popup = new PopupField<string>(displayNames, curIndex)
            {
                style =
            {
                width = width,
                marginLeft = 4,
                unityTextAlign = UnityEngine.TextAnchor.MiddleRight
            }
            };
            popup.SetEnabled(canEdit);

            popup.RegisterValueChangedCallback(evt =>
            {
                int idx = displayNames.IndexOf(evt.newValue);
                if (idx >= 0)
                {
                    onValueChanged?.Invoke(valueList[idx]);
                }
            });

            return popup;
        }
        //-----------------------------------------------------
        VisualElement CreateClassPopupFieldWithDisplay(
        int typeId,
        IUserData pPointer,
        Action<System.Type> onValueChanged,
        bool canEdit = true,
        int width = 120)
        {
            var nameIds = AgentTreeUtil.GetATClassTypes(typeId);
            List<string> displayNames = nameIds.Keys.ToList();
            List<int> valueList = nameIds.Values.ToList();
            int curIndex = valueList.IndexOf(typeId);
            if (curIndex < 0) curIndex = 0;

            var popup = new PopupField<string>(displayNames, curIndex)
            {
                style =
                {
                    width = width,
                    marginLeft = 4,
                    unityTextAlign = UnityEngine.TextAnchor.MiddleRight
                }
            };
            popup.SetEnabled(canEdit);

            bool isRestore = false;
            popup.RegisterValueChangedCallback(evt =>
            {
                int idx = displayNames.IndexOf(evt.newValue);
                if (idx >= 0)
                {
                    System.Type lastType = ATRtti.GetClassType(typeId);
                    System.Type type = ATRtti.GetClassType(valueList[idx]);
                    if (ATRtti.HasCommonConcreteBaseType(lastType, type))
                    {
                        onValueChanged?.Invoke(type);
                        var iconImage = popup.Q<Image>();
                        if (iconImage != null)
                        {
                            if (type != null && type.IsDefined(typeof(ATExportAttribute)))
                            {
                                ATExportAttribute atAtrr = type.GetCustomAttribute<ATExportAttribute>();
                                if (atAtrr != null && !string.IsNullOrEmpty(atAtrr.icon))
                                {
                                    var icon = AgentTreeUtil.LoadIcon(atAtrr.icon);
                                    iconImage.image = icon;
                                }
                            }
                        }
                    }
                    else
                    {
                        isRestore = true;
                        popup.index = curIndex;
                        if (isRestore)
                            this.m_pGraphView.ShowNotificationError("该节点在类[" + type.Name + "]中不能用!!!");
                        isRestore = false;
                    }
                }
            });

            Texture2D icon = null;
            var type = ATRtti.GetClassType(typeId);
            if (type!=null && type.IsDefined(typeof(ATExportAttribute)))
            {
                ATExportAttribute atAtrr = type.GetCustomAttribute<ATExportAttribute>();
                if(atAtrr!=null && !string.IsNullOrEmpty(atAtrr.icon))
                {
                    icon = AgentTreeUtil.LoadIcon(atAtrr.icon);
                }
            }

            // 创建横向容器
            var container = new VisualElement();
            container.style.flexDirection = FlexDirection.Row;
            // 添加popup
            container.Add(popup);

            // 添加icon
            if (icon != null)
            {
                var img = new Image
                {
                    image = icon,
                    style =
                    {
                        width = 16,
                        height = 16,
                        marginRight = 4,
                        marginTop = 2,
                        marginLeft = 2,
                    }
                };
                container.Add(img);
            }

            return container;
        }
        //-----------------------------------------------------
        private void DrawField<TField, TValue>(
                IVariable portVariable,
                ArvgPort port,
                TValue value,
                Func<TField> fieldFactory,
                Action<TField, TValue> setValue,
                Func<TField, TValue> getValue,
                Action<TField, EventCallback<ChangeEvent<TValue>>> registerCallback)
                where TField : VisualElement, new()
        {
            var field = fieldFactory();
            setValue(field, value);

            field.userData = port;
            field.SetEnabled(port.attri.canEdit);

            // 设置运行时值
            var runtimeVar = portVariable;// m_pGraphView.GetRuntimeVariable(port);
            if (runtimeVar != null && runtimeVar is TValue runtimeValue)
                setValue(field, runtimeValue);

            registerCallback(field, evt =>
            {
                if (CanChangeValue(port))
                {
                    OnArgvPortChanging(port);
                    // 赋值
                    setValue(field, evt.newValue);
                    var valueField = portVariable.GetType().GetField("value", System.Reflection.BindingFlags.Public| System.Reflection.BindingFlags.Instance| System.Reflection.BindingFlags.NonPublic);
                    valueField?.SetValue(portVariable, evt.newValue);
                    OnArgvPortChanged(port);
                    m_pGraphView.UpdateVariable(portVariable);
                }
            });

            port.fieldRoot.Add(field);
            port.fieldElement = field;
        }
        //-----------------------------------------------------
        protected virtual void UpdatePortRuntimeValue(ArvgPort port)
        {
            RefreshPortValue(port, false,true);
        }
        //-----------------------------------------------------
        public virtual void SetPortDefalueValue(ArvgPort port)
        {
            RefreshPortValue(port, true,false);
        }
        //-----------------------------------------------------
        protected virtual void RefreshPortValue(ArvgPort port, bool bSetDefault, bool bRuntime)
        {
            if (port.fieldElement == null)
                return;
            short varGuid = port.GetVariableGuid();
            var portVariable = m_pGraphView.GetVariable(varGuid);
            if (portVariable == null)
                return;

            //      if (port.attri.canEdit)
            //          return;

            if (portVariable is AT.Runtime.VariableInt)
            {
                if (port.attri.argvType.IsEnum)
                {
                    if (port.fieldElement is EnumField)
                    {
                        if(bSetDefault) m_pGraphView.UpdatePortVariableDefault(port);
                        IVariable runtimeVar = null;
                        if (bRuntime) runtimeVar = m_pGraphView.GetRuntimeVariable(port);
                        if(runtimeVar == null) runtimeVar = m_pGraphView.GetVariable(varGuid);
                        if (runtimeVar != null && runtimeVar is AT.Runtime.VariableInt)
                        {
                            EnumField enumField = (EnumField)port.fieldElement;
                            enumField.value = (Enum)Enum.ToObject(port.attri.argvType, ((AT.Runtime.VariableInt)runtimeVar).value);
                        }
                    }
                }
                else
                {
                    if (port.fieldElement is IntegerField)
                    {
                        if (bSetDefault) m_pGraphView.UpdatePortVariableDefault(port);
                        IVariable runtimeVar = null;
                        if (bRuntime) runtimeVar = m_pGraphView.GetRuntimeVariable(port);
                        if (runtimeVar == null) runtimeVar = m_pGraphView.GetVariable(varGuid);
                        if (runtimeVar != null && runtimeVar is AT.Runtime.VariableInt)
                        {
                            IntegerField eleField = (IntegerField)port.fieldElement;
                            eleField.value = ((AT.Runtime.VariableInt)runtimeVar).value;
                        }
                    }
                }
            }
            else if (portVariable is AT.Runtime.VariableObjId)
            {
                if (port.fieldElement is IntegerField)
                {
                    if (bSetDefault) m_pGraphView.UpdatePortVariableDefault(port);
                    IVariable runtimeVar = null;
                    if (bRuntime) runtimeVar = m_pGraphView.GetRuntimeVariable(port);
                    if (runtimeVar == null) runtimeVar = m_pGraphView.GetVariable(varGuid);
                    if (runtimeVar != null && runtimeVar is AT.Runtime.VariableObjId)
                    {
                        IntegerField eleField = (IntegerField)port.fieldElement;
                        eleField.value = ((AT.Runtime.VariableObjId)runtimeVar).value.id;
                    }
                }
            }
            else if (portVariable is AT.Runtime.VariableBool)
            {
                if (port.fieldElement is Toggle)
                {
                    if (bSetDefault) m_pGraphView.UpdatePortVariableDefault(port);
                    IVariable runtimeVar = null;
                    if (bRuntime) runtimeVar = m_pGraphView.GetRuntimeVariable(port);
                    if (runtimeVar == null) runtimeVar = m_pGraphView.GetVariable(varGuid);
                    if (runtimeVar != null && runtimeVar is AT.Runtime.VariableBool)
                    {
                        Toggle eleField = (Toggle)port.fieldElement;
                        eleField.value = ((AT.Runtime.VariableBool)runtimeVar).value;
                    }
                }
            }
            else if (portVariable is AT.Runtime.VariableFloat)
            {
                if (port.fieldElement is FloatField)
                {
                    if (bSetDefault) m_pGraphView.UpdatePortVariableDefault(port);
                    IVariable runtimeVar = null;
                    if (bRuntime) runtimeVar = m_pGraphView.GetRuntimeVariable(port);
                    if (runtimeVar == null) runtimeVar = m_pGraphView.GetVariable(varGuid);
                    if (runtimeVar != null && runtimeVar is AT.Runtime.VariableFloat)
                    {
                        FloatField eleField = (FloatField)port.fieldElement;
                        eleField.value = ((AT.Runtime.VariableFloat)runtimeVar).value;
                    }
                }
            }
            else if (portVariable is AT.Runtime.VariableString)
            {
                if (port.fieldElement is TextField)
                {
                    if (bSetDefault) m_pGraphView.UpdatePortVariableDefault(port);
                    IVariable runtimeVar = null;
                    if (bRuntime) runtimeVar = m_pGraphView.GetRuntimeVariable(port);
                    if (runtimeVar == null) runtimeVar = m_pGraphView.GetVariable(varGuid);
                    if (runtimeVar != null && runtimeVar is AT.Runtime.VariableString)
                    {
                        TextField eleField = (TextField)port.fieldElement;
                        eleField.value = ((AT.Runtime.VariableString)runtimeVar).value;
                    }
                }
            }
            else if (portVariable is AT.Runtime.VariableVec2)
            {
                if (port.fieldElement is Vector2Field)
                {
                    if (bSetDefault) m_pGraphView.UpdatePortVariableDefault(port);
                    IVariable runtimeVar = null;
                    if (bRuntime) runtimeVar = m_pGraphView.GetRuntimeVariable(port);
                    if (runtimeVar == null) runtimeVar = m_pGraphView.GetVariable(varGuid);
                    if (runtimeVar != null && runtimeVar is AT.Runtime.VariableVec2)
                    {
                        Vector2Field eleField = (Vector2Field)port.fieldElement;
                        eleField.value = ((AT.Runtime.VariableVec2)runtimeVar).value;
                    }
                }
            }
            else if (portVariable is AT.Runtime.VariableVec3)
            {
                if (port.fieldElement is Vector3Field)
                {
                    if (bSetDefault) m_pGraphView.UpdatePortVariableDefault(port);
                    IVariable runtimeVar = null;
                    if (bRuntime) runtimeVar = m_pGraphView.GetRuntimeVariable(port);
                    if (runtimeVar == null) runtimeVar = m_pGraphView.GetVariable(varGuid);
                    if (runtimeVar != null && runtimeVar is AT.Runtime.VariableVec3)
                    {
                        Vector3Field eleField = (Vector3Field)port.fieldElement;
                        eleField.value = ((AT.Runtime.VariableVec3)runtimeVar).value;
                    }
                }
            }
            else if (portVariable is AT.Runtime.VariableVec4)
            {
                if (port.fieldElement is Vector4Field)
                {
                    if (bSetDefault) m_pGraphView.UpdatePortVariableDefault(port);
                    IVariable runtimeVar = null;
                    if (bRuntime) runtimeVar = m_pGraphView.GetRuntimeVariable(port);
                    if (runtimeVar == null) runtimeVar = m_pGraphView.GetVariable(varGuid);
                    if (runtimeVar != null && runtimeVar is AT.Runtime.VariableVec4)
                    {
                        Vector4Field eleField = (Vector4Field)port.fieldElement;
                        eleField.value = ((AT.Runtime.VariableVec4)runtimeVar).value;
                    }
                }
            }
            else if (portVariable is AT.Runtime.VariableLong)
            {
                if (port.fieldElement is LongField)
                {
                    if (bSetDefault) m_pGraphView.UpdatePortVariableDefault(port);
                    IVariable runtimeVar = null;
                    if (bRuntime) runtimeVar = m_pGraphView.GetRuntimeVariable(port);
                    if (runtimeVar == null) runtimeVar = m_pGraphView.GetVariable(varGuid);
                    if (runtimeVar != null && runtimeVar is AT.Runtime.VariableLong)
                    {
                        LongField eleField = (LongField)port.fieldElement;
                        eleField.value = ((AT.Runtime.VariableLong)runtimeVar).value;
                    }
                }
            }
            else if (portVariable is AT.Runtime.VariableDouble)
            {
                if (port.fieldElement is DoubleField)
                {
                    if (bSetDefault) m_pGraphView.UpdatePortVariableDefault(port);
                    IVariable runtimeVar = null;
                    if (bRuntime) runtimeVar = m_pGraphView.GetRuntimeVariable(port);
                    if (runtimeVar == null) runtimeVar = m_pGraphView.GetVariable(varGuid);
                    if (runtimeVar != null && runtimeVar is AT.Runtime.VariableDouble)
                    {
                        DoubleField eleField = (DoubleField)port.fieldElement;
                        eleField.value = ((AT.Runtime.VariableDouble)runtimeVar).value;
                    }
                }
            }
            else if (portVariable is AT.Runtime.VariableColor)
            {
                if (port.fieldElement is ColorField)
                {
                    if (bSetDefault) m_pGraphView.UpdatePortVariableDefault(port);
                    IVariable runtimeVar = null;
                    if (bRuntime) runtimeVar = m_pGraphView.GetRuntimeVariable(port);
                    if (runtimeVar == null) runtimeVar = m_pGraphView.GetVariable(varGuid);
                    if (runtimeVar != null && runtimeVar is AT.Runtime.VariableColor)
                    {
                        ColorField eleField = (ColorField)port.fieldElement;
                        eleField.value = ((AT.Runtime.VariableColor)runtimeVar).value;
                    }
                }
            }
            else if (portVariable is AT.Runtime.VariableRay)
            {
                // fieldElement为VisualElement，包含两个Vector3Field
                if (port.fieldElement is VisualElement container && container.childCount == 2)
                {
                    if (bSetDefault) m_pGraphView.UpdatePortVariableDefault(port);
                    IVariable runtimeVar = null;
                    if (bRuntime) runtimeVar = m_pGraphView.GetRuntimeVariable(port);
                    if (runtimeVar == null) runtimeVar = m_pGraphView.GetVariable(varGuid);
                    if (runtimeVar is AT.Runtime.VariableRay runtimeRay)
                    {
                        if (container[0] is Vector3Field originField)
                            originField.value = runtimeRay.value.origin;
                        if (container[1] is Vector3Field dirField)
                            dirField.value = runtimeRay.value.direction;
                    }
                }
            }
            else if (portVariable is AT.Runtime.VariableBounds)
            {
                // fieldElement为VisualElement，包含两个Vector3Field
                if (port.fieldElement is VisualElement container && container.childCount == 2)
                {
                    if (bSetDefault) m_pGraphView.UpdatePortVariableDefault(port);
                    IVariable runtimeVar = null;
                    if (bRuntime) runtimeVar = m_pGraphView.GetRuntimeVariable(port);
                    if (runtimeVar == null) runtimeVar = m_pGraphView.GetVariable(varGuid);
                    if (runtimeVar is AT.Runtime.VariableBounds runtimeBounds)
                    {
                        if (container[0] is Vector3Field centerField)
                            centerField.value = runtimeBounds.value.center;
                        if (container[1] is Vector3Field sizeField)
                            sizeField.value = runtimeBounds.value.size;
                    }
                }
            }
            else if (portVariable is AT.Runtime.VariableMatrix)
            {
                // fieldElement为VisualElement，包含4个Vector4Field，分别对应Matrix4x4的四行
                if (port.fieldElement is VisualElement container && container.childCount == 4)
                {
                    if (bSetDefault) m_pGraphView.UpdatePortVariableDefault(port);
                    IVariable runtimeVar = null;
                    if (bRuntime) runtimeVar = m_pGraphView.GetRuntimeVariable(port);
                    if (runtimeVar == null) runtimeVar = m_pGraphView.GetVariable(varGuid);
                    if (runtimeVar is AT.Runtime.VariableMatrix runtimeMatrix)
                    {
                        // 辅助方法：获取Matrix4x4的某一行
                        Vector4 GetRow(Matrix4x4 m, int row)
                        {
                            switch (row)
                            {
                                case 0: return new Vector4(m.m00, m.m01, m.m02, m.m03);
                                case 1: return new Vector4(m.m10, m.m11, m.m12, m.m13);
                                case 2: return new Vector4(m.m20, m.m21, m.m22, m.m23);
                                case 3: return new Vector4(m.m30, m.m31, m.m32, m.m33);
                                default: return Vector4.zero;
                            }
                        }
                        for (int i = 0; i < 4; ++i)
                        {
                            if (container[i] is Vector4Field rowField)
                                rowField.value = GetRow(runtimeMatrix.value, i);
                        }
                    }
                }
            }
            else if (portVariable is AT.Runtime.VariableRect)
            {
                // Rect 使用 RectField
                if (port.fieldElement is RectField rectField)
                {
                    if (bSetDefault) m_pGraphView.UpdatePortVariableDefault(port);
                    IVariable runtimeVar = null;
                    if (bRuntime) runtimeVar = m_pGraphView.GetRuntimeVariable(port);
                    if (runtimeVar == null) runtimeVar = m_pGraphView.GetVariable(varGuid);
                    if (runtimeVar is AT.Runtime.VariableRect runtimeRect)
                    {
                        rectField.value = runtimeRect.value;
                    }
                }
            }
        }

    }
}
#endif