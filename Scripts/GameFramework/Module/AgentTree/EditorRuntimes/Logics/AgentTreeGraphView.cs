/********************************************************************
生成日期:	06:30:2025
类    名: 	AgentTreeGraphView
作    者:	HappLI
描    述:	行为树视图绘制视图
*********************************************************************/
#if UNITY_EDITOR
using Framework.AT.Runtime;
using Framework.ED;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

namespace Framework.AT.Editor
{
    public class AgentTreeGraphView : GraphView
    {
        System.Object m_pObject;
        AgentTreeData m_pAgentTreeData = null;
        AEditorLogic m_pOwnerEditorLogic;
        Dictionary<BaseNode, GraphNode> m_vNodes = new Dictionary<BaseNode, GraphNode>();
        Dictionary<short, GraphNode> m_vGuidNodes = new Dictionary<short, GraphNode>();
        Dictionary<short, IVariable> m_vVariables = new Dictionary<short, IVariable>();
        Dictionary<long, ArvgPort> m_vPorts = new Dictionary<long, ArvgPort>();

        bool m_bIniting = false;
        bool m_bRegisterUndo = true;
        private UndoRedoSystem m_undoRedoSystem;
        private bool m_isPerformingUndoRedo = false;
        public AgentTreeGraphView(AEditorLogic pOwner)
        {
            m_pOwnerEditorLogic = pOwner;
            m_undoRedoSystem = new UndoRedoSystem(this);
            // 允许对Graph进行Zoom in/out
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            // 允许拖拽Content
            this.AddManipulator(new ContentDragger());
            // 允许Selection里的内容
            this.AddManipulator(new SelectionDragger());
            // GraphView允许进行框选
            this.AddManipulator(new RectangleSelector());

            // 添加右键菜单
            this.AddManipulator(new ContextualMenuManipulator((ContextualMenuPopulateEvent evt) =>
            {
                evt.menu.InsertAction(0, "撤销", (a) =>
                {
                    Undo();
                }, DropdownMenuAction.AlwaysEnabled);
                evt.menu.InsertAction(1, "重做", (a) =>
                {
                    Redo();
                }, DropdownMenuAction.AlwaysEnabled);
                evt.menu.InsertAction(2, "保存", (a) =>
                {
                    m_pOwnerEditorLogic.GetOwner().SaveChanges();
                });
                if (this.selection != null && this.selection.Count == 1 && this.selection.First() is GraphNode)
                {
                    var node = this.selection.First() as GraphNode;
                    if (!node.GetAttri().isCutsceneCustomEvent && node.GetAttri().functionClassType != null)
                    {
                        evt.menu.InsertAction(3, "函数定位", (a) =>
                        {
                            OnLocationNodeFunction(node);
                        });
                    }
                    evt.menu.InsertAction(3, "设置备注名", (a) =>
                    {
                        ReNodeName(node, "设置备注名");
                    });
                }
            }));

            this.RegisterCallback<KeyDownEvent>(evt =>
            {
                var window = m_pOwnerEditorLogic.GetOwner<AgentTreeWindow>();
                if (window != null && window.OnGraphViewEvent(evt.imguiEvent))
                {
                    evt.StopPropagation(); // 阻止事件继续传递
                }
                else
                {
                    if ((evt.ctrlKey || evt.commandKey) && evt.keyCode == KeyCode.S)
                    {
                        m_pOwnerEditorLogic.GetOwner().SaveChanges();
                        evt.StopPropagation();
                    }
                    else if ((evt.ctrlKey || evt.commandKey) && evt.keyCode == KeyCode.Z)
                    {
                        Undo();
                        evt.StopPropagation();
                    }
                    else if ((evt.ctrlKey || evt.commandKey) && evt.keyCode == KeyCode.Y)
                    {
                        Redo();
                        evt.StopPropagation();
                    }
                }

            });
            this.RegisterCallback<KeyUpEvent>(evt =>
            {
                var window = m_pOwnerEditorLogic.GetOwner<AgentTreeWindow>();
                if (window != null && window.OnGraphViewEvent(evt.imguiEvent))
                {
                    evt.StopPropagation();
                }
            });

            var menuWindowProvider = (AgentTreeSearcher)ScriptableObject.CreateInstance<AgentTreeSearcher>();
            menuWindowProvider.ownerGraphView = this;
            menuWindowProvider.OnSelectEntryHandler = OnMenuSelectEntry;
            nodeCreationRequest += context =>
            {
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), menuWindowProvider);
            };
            this.graphViewChanged += OnGraphViewChanged;

            this.viewTransformChanged += OnViewTransformChanged;

            this.RegisterCallback<MouseDownEvent>(OnMouseDownEvent);
            this.RegisterCallback<MouseUpEvent>(OnMouseUpEvent);
        }
        //-----------------------------------------------------
        void OnMouseDownEvent(MouseDownEvent evt)
        {
            if (evt.button == 0)
            {
                foreach (var node in this.selection)
                {
                    if (node is Framework.AT.Editor.GraphNode graphNode)
                    {
                        graphNode.OnDragMouseDown();
                    }
                }
            }
        }
        //-----------------------------------------------------
        void OnMouseUpEvent(MouseUpEvent evt)
        {
            if (evt.button == 0)
            {
                foreach (var node in this.selection)
                {
                    if (node is Framework.AT.Editor.GraphNode graphNode)
                    {
                        graphNode.OnDragMouseUp();
                    }
                }
            }
        }
        //-----------------------------------------------------
        private GraphViewChange OnGraphViewChanged(GraphViewChange change)
        {
            if (change.edgesToCreate != null)
            {
                foreach (var edge in change.edgesToCreate)
                {
                    OnEdgeConnected(edge);
                }
            }
            if (change.elementsToRemove != null)
            {
                foreach (var element in change.elementsToRemove)
                {
                    if (element is GraphNode node)
                    {
                        RemoveNode(node.bindNode);
                    }
                    else if (element is Edge)
                    {
                        OnEdgeDisconnected((Edge)element);
                    }
                }
            }
            return change;
        }
        //-----------------------------------------------------
        private void OnViewTransformChanged(GraphView graphView)
        {

        }
        //-----------------------------------------------------
        void ReNodeName(GraphNode node, string title)
        {
            if (node.bindNode.name == null)
                node.bindNode.name = "";
            PopValueInput.Show(title, node, node.bindNode.name, (data, value) =>
            {
                node.bindNode.name = "";
                if (value == null) return;
                node.bindNode.name = value.ToString();
                node.UpdateTitle();
            });
        }
        //-----------------------------------------------------
        void OnLocationNodeFunction(GraphNode pNode)
        {
            var attri = pNode.GetAttri();
            if (attri == null || attri.functionClassType == null)
            {
                ShowNotification("无法定位函数地址");
                return;
            }
            System.Type type = attri.functionClassType;
            var guid = attri.functionAttr.guid;


            string[] guids = AssetDatabase.FindAssets($"{type.Name} t:Script");
            string filePath = null;
            if (guids.Length == 1)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            }
            else
            {
                foreach (var g in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(g);
                    if (string.IsNullOrEmpty(path) || !path.EndsWith(".cs"))
                        continue;
                    var lines = System.IO.File.ReadAllLines(path);
                    foreach (var line in lines)
                    {
                        if (line.Contains($"class {type.Name}") || line.Contains($"struct {type.Name}"))
                        {
                            if (string.IsNullOrEmpty(type.Namespace) || lines.Any(l => l.Contains($"namespace {type.Namespace}")))
                            {
                                filePath = path;
                                break;
                            }
                        }
                    }
                    if (filePath != null)
                        break;
                }
            }


            if (string.IsNullOrEmpty(filePath))
            {
                ShowNotificationError("找不到类型对应的脚本文件路径");
                return;
            }

            int targetLine = 1;
            try
            {
                var lines = System.IO.File.ReadAllLines(filePath);
                string guidStr = $"[ATFunction({guid}";
                for (int i = 0; i < lines.Length; ++i)
                {
                    if (lines[i].Contains(guidStr))
                    {
                        targetLine = i + 1;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowNotificationError("读取脚本文件失败: " + ex.Message);
                return;
            }

            UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(filePath, targetLine);
        }
        //-----------------------------------------------------
        void OnEdgeConnected(Edge edge)
        {
            var outputPort = edge.output;
            var inputPort = edge.input;
            if (inputPort != null && inputPort.source != null && inputPort.source is ArvgPort)
            {
                ArvgPort arvPort = inputPort.source as ArvgPort;
                RegisterUndo(arvPort.grapNode, UndoRedoOperationType.NodeEdge);

                arvPort.fieldRoot.SetEnabled(false);
                if (arvPort.enumPopFieldElement != null)
                    arvPort.enumPopFieldElement.SetEnabled(arvPort.fieldRoot.enabledSelf);

                if (outputPort.source is ArvgPort)
                    arvPort.AddDummyPort(outputPort.source as ArvgPort);

                if (arvPort.attri.argvType == typeof(IVariable))
                {
                    //! 重建arvPort.fieldRoot 下的节点
                    arvPort.grapNode.ReBuildPortContain(arvPort);
                }

            }
            else if(inputPort !=null && inputPort.source!=null && inputPort.source is LinkPort)
            {
                LinkPort arvPort = inputPort.source as LinkPort;
                RegisterUndo(arvPort.graphNode, UndoRedoOperationType.NodeEdge);
            }
        }
        //-----------------------------------------------------
        void OnEdgeDisconnected(Edge edge)
        {
            var outputPort = edge.output;
            var inputPort = edge.input;
            if (inputPort != null && inputPort.source != null && inputPort.source is ArvgPort)
            {
                ArvgPort arvPort = inputPort.source as ArvgPort;

                RegisterUndo(arvPort.grapNode, UndoRedoOperationType.NodeEdge);

                if (arvPort.nodePort.dummyPorts != null && outputPort.source is ArvgPort)
                {
                    var outPort = (ArvgPort)outputPort.source;
                    arvPort.RemoveDummyPort(outPort);
                }
                arvPort.fieldRoot.SetEnabled(arvPort.attri.canEdit);
                if (arvPort.enumPopFieldElement != null)
                    arvPort.enumPopFieldElement.SetEnabled(arvPort.fieldRoot.enabledSelf);
                arvPort.grapNode.ReBuildPortContain(arvPort);
            }
            else if (inputPort != null && inputPort.source != null && inputPort.source is LinkPort)
            {
                LinkPort arvPort = inputPort.source as LinkPort;
                RegisterUndo(arvPort.graphNode, UndoRedoOperationType.NodeEdge);
            }
        }
        //-----------------------------------------------------
        private bool OnMenuSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            BaseNode newNode = null;
            if (searchTreeEntry.userData is AgentTreeAttri)
            {
                AgentTreeAttri attri = searchTreeEntry.userData as AgentTreeAttri;

                if (attri.isCutsceneCustomEvent)
                {
                    CustomEvent pAction = new CustomEvent();
                    pAction.type = attri.actionType;
                    pAction.eventType = attri.cutsceneCusomtType;
                    pAction.guid = GeneratorGUID();
                    pAction.nextActions = null;
                    newNode = pAction;
                }
                else if (attri.actionAttr.isTask)
                {
                    if (!attri.actionAttr.allowMuti && HasTaskNode((ETaskType)attri.actionType))
                    {
                        m_pOwnerEditorLogic.GetOwner().ShowNotification(new GUIContent("Task node already exists in the tree."), 2.0f);
                        return false;
                    }
                    EnterTask pTask = new EnterTask();
                    pTask.type = attri.actionType;
                    pTask.guid = GeneratorGUID();
                    pTask.nextActions = null;
                    newNode = pTask;
                }
                else
                {
                    AT.Runtime.ActionNode pAction = new AT.Runtime.ActionNode();
                    pAction.type = attri.actionType;
                    pAction.guid = GeneratorGUID();
                    pAction.nextActions = null;
                    newNode = pAction;
                }
            }
            //??????
            //else if (searchTreeEntry.userData is EventAttriData)
            //{
            //    EventAttriData attri = searchTreeEntry.userData as EventAttriData;
            //}
            else if (searchTreeEntry.userData is ArvgPort)
            {
                ArvgPort arcgPort = searchTreeEntry.userData as ArvgPort;
                if (!arcgPort.isInput)
                {
                    AT.Runtime.ActionNode pAction = new AT.Runtime.ActionNode();
                    pAction.type = (int)EActionType.eGetVariable;
                    pAction.guid = GeneratorGUID();
                    var nodePorts = pAction.GetOutports(true, 1);
                    nodePorts[0].varGuid = arcgPort.GetVariableGuid();
                    pAction.nextActions = null;
                    newNode = pAction;
                }
            }
            if (newNode == null)
                return false;


            Vector2 windowPos = context.screenMousePosition;
            if (m_pOwnerEditorLogic.GetOwner() != null)
                windowPos -= m_pOwnerEditorLogic.GetOwner().position.position;
            Vector2 graphPos = this.contentViewContainer.WorldToLocal(windowPos);
            newNode.posX = (int)((graphPos.x) * 100);
            newNode.posY = (int)((graphPos.y) * 100);
            var viewNode = AddNode(newNode);

            return true;
        }
        //--------------------------------------------------------
        public void OnEnable(AEditorLogic logic)
        {
            m_pOwnerEditorLogic = logic;
        }
        //-----------------------------------------------------
        public AgentTreeData GetATData()
        {
            return m_pAgentTreeData;
        }
        //-----------------------------------------------------
        public void ShowNotification(string content, float duration = 1)
        {
            m_pOwnerEditorLogic.GetOwner().ShowNotification(new GUIContent(content), duration);
        }
        //-----------------------------------------------------
        public void ShowNotificationError(string content, float duration = 1)
        {
            var icon = EditorGUIUtility.IconContent("console.erroricon").image;
            m_pOwnerEditorLogic.GetOwner().ShowNotification(new GUIContent(content, icon), duration);
        }
        //-----------------------------------------------------
        public void ShowNotificationWarning(string content, float duration = 1)
        {
            var icon = EditorGUIUtility.IconContent("console.warnicon").image;
            m_pOwnerEditorLogic.GetOwner().ShowNotification(new GUIContent(content, icon), duration);
        }
        //-----------------------------------------------------
        public AgentTree GetCurrentRuntimeAgentTree()
        {
            return m_pOwnerEditorLogic.GetOwner<AgentTreeWindow>().GetAT();
            // if (cutsceneInstance == null)
            return null;
            // return cutsceneInstance.GetAgentTree();
        }
        //-----------------------------------------------------
        public bool IsPlaying()
        {
            var pAT = GetCurrentRuntimeAgentTree();
            if (pAT == null) return false;
            return pAT.IsStarted();
        }
        //-----------------------------------------------------
        public IVariable GetRuntimeVariable(ArvgPort port)
        {
            var agentTree = GetCurrentRuntimeAgentTree();
            if (agentTree == null) return null;
            var returnVal = agentTree.GetVariable(port.GetVariableGuid(), true);
            if (returnVal == null && port.nodePort.dummyPorts != null && port.nodePort.dummyPorts.Length > 0)
            {
                var dummyPort = agentTree.GetDummyPort(port.grapNode.bindNode, port.slotIndex, port.isInput);
                if (dummyPort.IsValid())
                {
                    NodePort[] ports = null;
                    if (dummyPort.type == 0) ports = dummyPort.pNode.GetInports();
                    else ports = dummyPort.pNode.GetOutports();
                    if (ports != null)
                        returnVal = agentTree.GetVariable(ports[dummyPort.slotIndex].varGuid, true);
                }
            }
            return returnVal;
        }
        //-----------------------------------------------------
        public System.Object GetEditOwnerObject()
        {
            return m_pObject;
        }
        //-----------------------------------------------------
        public void SetAgentTree(AgentTreeData pAgentTree, System.Object pOwner)
        {
            //var cutscene = GetCurrentCutscene();
            //cutscene.CreateAgentTree(pObj.GetCutsceneGraph());
            //var agentTree = cutscene.GetAgentTree();
            //if (agentTree != null)
            //    agentTree.RegisterCallback(this);
            if (m_pAgentTreeData != null && m_undoRedoSystem.HasChanges())
            {
                if (EditorUtility.DisplayDialog("提示", "当前有更改，是否需要保存当前后，再加载呢", "保存", "直接加载"))
                {
                    Save(m_pAgentTreeData);
                }
            }
            m_bIniting = true;
            m_pObject = pOwner;
            m_pAgentTreeData = pAgentTree;
            m_vVariables = pAgentTree.GetVariableGUIDs();
            if (pAgentTree.tasks != null)
            {
                for (int i = 0; i < pAgentTree.tasks.Length; ++i)
                {
                    AddNode(pAgentTree.tasks[i]);
                }
            }
            if (pAgentTree.actions != null)
            {
                for (int i = 0; i < pAgentTree.actions.Length; ++i)
                {
                    AddNode(pAgentTree.actions[i]);
                }
            }
            if (pAgentTree.events != null)
            {
                for (int i = 0; i < pAgentTree.events.Length; ++i)
                {
                    AddNode(pAgentTree.events[i]);
                }
            }
            CreateLinkLine();
            m_bIniting = false;
        }
        //--------------------------------------------------------
        internal void RefreshATDatas(AgentTreeData pData)
        {
            m_bIniting = true;
            Dictionary<int, GraphNode> vNodes = new Dictionary<int, GraphNode>();
            foreach(var db in m_vGuidNodes)
            {
                vNodes[db.Key] = db.Value;
            }
            m_vGuidNodes.Clear();
            m_vNodes.Clear();
            m_vVariables = pData.GetVariableGUIDs();
            if (pData.tasks != null)
            {
                for (int i = 0; i < pData.tasks.Length; ++i)
                {
                    if (vNodes.TryGetValue(pData.tasks[i].guid, out var graphNode))
                    {
                        m_vNodes.Add(pData.tasks[i], graphNode);
                        m_vGuidNodes[pData.tasks[i].guid] = graphNode;
                    }
                    else
                        AddNode(pData.tasks[i]);
                    vNodes.Remove(pData.tasks[i].guid);
                }
            }
            if (pData.actions != null)
            {
                for (int i = 0; i < pData.actions.Length; ++i)
                {
                    if (vNodes.TryGetValue(pData.actions[i].guid, out var graphNode))
                    {
                        m_vNodes.Add(pData.actions[i], graphNode);
                        m_vGuidNodes[pData.actions[i].guid] = graphNode;
                    }
                    else
                        AddNode(pData.actions[i]);
                    vNodes.Remove(pData.actions[i].guid);
                }
            }
            if (pData.events != null)
            {
                for (int i = 0; i < pData.events.Length; ++i)
                {
                    if (vNodes.TryGetValue(pData.events[i].guid, out var graphNode))
                    {
                        m_vNodes.Add(pData.events[i], graphNode);
                        m_vGuidNodes[pData.events[i].guid] = graphNode;
                    }
                    else
                        AddNode(pData.events[i]);
                    vNodes.Remove(pData.events[i].guid);
                }
            }

            //remove old nodes
            foreach (var db in vNodes)
            {
                db.Value.Release();
                this.RemoveElement(db.Value);
            }
            CreateLinkLine();
            m_bIniting = false;
        }
        //--------------------------------------------------------
        public GraphNode AddNode(BaseNode pNode, bool bUndo =true)
        {
            if (m_vNodes.TryGetValue(pNode, out var viewNode))
                return viewNode;

            int customType = 0;
            if (pNode is CustomEvent)
            {
                customType = ((CustomEvent)pNode).eventType;
            }
            GraphNode graphNode = null;
            var attri = AgentTreeUtil.GetAttri(pNode.type, customType);
            if (attri != null && attri.graphNodeType != null)
            {
                graphNode = (GraphNode)Activator.CreateInstance(attri.graphNodeType, this, pNode, true);
            }
            else
                graphNode = new GraphNode(this, pNode);

            if (bUndo)
            {
                RegisterUndo(graphNode, UndoRedoOperationType.NodeAdd);
            }

            AddElement(graphNode);
            graphNode.UpdatePosition();
            m_vNodes.Add(pNode, graphNode);
            m_vGuidNodes[pNode.guid] = graphNode;
            return graphNode;
        }
        //--------------------------------------------------------
        public GraphNode GetNode(BaseNode pNode)
        {
            if (m_vNodes.TryGetValue(pNode, out var graphNode))
            {
                return graphNode;
            }
            return null;
        }
        //--------------------------------------------------------
        internal bool HasTaskNode(ETaskType type)
        {
            foreach (var db in m_vNodes)
            {
                if (db.Value.bindNode is EnterTask)
                {
                    if (((EnterTask)db.Value.bindNode).type == (int)type)
                        return true;
                }
            }
            return false;
        }
        //--------------------------------------------------------
        public GraphNode GetNode(short guid)
        {
            if (m_vGuidNodes.TryGetValue(guid, out var graphNode))
            {
                return graphNode;
            }
            return null;
        }
        //--------------------------------------------------------
        public void RemoveNode(AT.Runtime.BaseNode pNode, bool bUndo = true)
        {
            if (m_vNodes.TryGetValue(pNode, out var graphNode))
            {
                if (bUndo)
                {
                    RegisterUndo(graphNode, UndoRedoOperationType.NodeDel);
                }
                graphNode.Release();
                this.RemoveElement(graphNode);
                m_vNodes.Remove(pNode);
                m_vGuidNodes.Remove(pNode.guid);
            }
        }
        //--------------------------------------------------------
        public void RemoveNode(short guid, bool bUndo = true)
        {
            if (m_vGuidNodes.TryGetValue(guid, out var graphNode))
            {
                if (bUndo)
                {
                    RegisterUndo(graphNode, UndoRedoOperationType.NodeDel);
                }
                graphNode.Release();
                this.RemoveElement(graphNode);
                m_vNodes.Remove(graphNode.bindNode);
                m_vGuidNodes.Remove(guid);
            }
        }
        //--------------------------------------------------------
        public void OnDisable()
        {

        }
        //--------------------------------------------------------
        public void OnGUI(Rect rect)
        {
        }
        //--------------------------------------------------------
        public void OnSaveChanges()
        {
            if (m_pAgentTreeData == null)
                return;

            UpdatePortsVariableDefault();
            Save(m_pAgentTreeData);
        }
        //-----------------------------------------------------
        internal void RegisterUndo(GraphNode pNode, UndoRedoOperationType type)
        {
            if (m_bIniting) return;
            m_bRegisterUndo = true;
          //  m_undoRedoSystem.RegisterUndo(pNode, type);
        }
        //-----------------------------------------------------
        public void Undo()
        {
            if (m_isPerformingUndoRedo) return;

            m_undoRedoSystem.Undo();

            m_isPerformingUndoRedo = true;
            try
            {
            }
            finally
            {
                m_isPerformingUndoRedo = false;
            }
        }
        //-----------------------------------------------------
        public void Redo()
        {
            if (m_isPerformingUndoRedo) return;

            m_undoRedoSystem.Redo();

            m_isPerformingUndoRedo = true;
            try
            {
            }
            finally
            {
                m_isPerformingUndoRedo = false;
            }
        }
        //-----------------------------------------------------
        public void Save(AgentTreeData pData, bool bReInit = true)
        {
            Dictionary<System.Type, List<BaseNode>> vNodes = new Dictionary<Type, List<BaseNode>>();
            foreach (var db in m_vNodes)
            {
                var graphNode = db.Value;

                graphNode.Save();
                if(!vNodes.TryGetValue(graphNode.bindNode.GetType(), out var vList))
                {
                    vList = new List<BaseNode>();
                    vNodes[graphNode.bindNode.GetType()] = vList;
                }
                vList.Add(graphNode.bindNode);
            }

            var fields = pData.GetType().GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            for(int i =0; i < fields.Length; ++i)
            {
                if (fields[i].FieldType.IsArray)
                {
                    System.Type dataType = fields[i].FieldType.GetElementType();
                    vNodes.TryGetValue(dataType, out var vList);
                    if (vList == null || vList.Count <= 0)
                        fields[i].SetValue(pData, null);
                    else
                    {
                        Array arr = Array.CreateInstance(dataType, vList.Count);
                        for (int j = 0; j < vList.Count; ++j)
                        {
                            arr.SetValue(vList[j], j);
                        }
                        fields[i].SetValue(pData, arr);
                    }
                }
            }
            CollectAllNodeVariables();
            var guidVars = pData.GetVariableGUIDs();
            if (guidVars != m_vVariables)
            {
                pData.SetVariables(m_vVariables);
            }
            pData.Serialize(false);
            if(bReInit) pData.Init(true);
        }
        //-----------------------------------------------------
        public void CollectAllNodeVariables()
        {
            // 1. 收集所有节点端口上的变量
            HashSet<short> usedGuids = new HashSet<short>();
            foreach (var nodePair in m_vNodes)
            {
                var graphNode = nodePair.Value;
                // 输入端口
                var inPorts = graphNode.GetArvgs();
                if (inPorts != null)
                {
                    foreach (var port in inPorts)
                    {
                        var variable = port?.GetVariable();
                        if (variable != null)
                            usedGuids.Add(variable.GetGuid());
                    }
                }
                // 输出端口
                var outPorts = graphNode.GetReturns();
                if (outPorts != null)
                {
                    foreach (var port in outPorts)
                    {
                        var variable = port?.GetVariable();
                        if (variable != null)
                            usedGuids.Add(variable.GetGuid());
                    }
                }
            }

            // 2. 保留 m_vVariables 中被节点引用的变量，移除未被引用的
            var allGuids = m_vVariables.Keys.ToList();
            foreach (var guid in allGuids)
            {
                if (!usedGuids.Contains(guid))
                    m_vVariables.Remove(guid);
            }

            // 3. 补充节点引用但 m_vVariables 中没有的变量
            foreach (var guid in usedGuids)
            {
                if (!m_vVariables.ContainsKey(guid))
                {
                    // 尝试从节点端口获取变量并加入
                    foreach (var nodePair in m_vNodes)
                    {
                        var graphNode = nodePair.Value;
                        var inPorts = graphNode.GetArvgs();
                        if (inPorts != null)
                        {
                            foreach (var port in inPorts)
                            {
                                var variable = port?.GetVariable();
                                if (variable != null && variable.GetGuid() == guid)
                                {
                                    m_vVariables[guid] = variable;
                                    break;
                                }
                            }
                        }
                        var outPorts = graphNode.GetReturns();
                        if (outPorts != null)
                        {
                            foreach (var port in outPorts)
                            {
                                var variable = port?.GetVariable();
                                if (variable != null && variable.GetGuid() == guid)
                                {
                                    m_vVariables[guid] = variable;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        //------------------------------------------------------
        public override List<Port> GetCompatiblePorts(Port startAnchor, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();
            if (startAnchor.source != null)
            {
                foreach (var port in ports.ToList())
                {
                    if (startAnchor.node == port.node ||
                        startAnchor.direction == port.direction ||
                        startAnchor.portType != port.portType)
                    {
                        continue;
                    }

                    if (port.source == null)
                        continue;
                    if (port.source.GetType() != startAnchor.source.GetType())
                        continue;

                    if (port.source is ArvgPort)
                    {
                        var srcPort = (ArvgPort)port.source;
                        var starPort = (ArvgPort)startAnchor.source;

                        if (srcPort.GetVariable() == null || starPort.GetVariable() == null)
                            continue;

                        if (srcPort.GetVariableGuid() == starPort.GetVariableGuid())
                            continue;
                        if (GetVariableOwnerNode(srcPort.GetVariableGuid()) == GetVariableOwnerNode(starPort.GetVariableGuid()))
                            continue;

                        if (srcPort.GetVariable().GetType() != starPort.GetVariable().GetType())
                            continue;

                        if (srcPort.GetVariable() is VariableUserData)
                        {
                            VariableUserData userDataSrc = (VariableUserData)srcPort.GetVariable();
                            VariableUserData userDataStar = (VariableUserData)starPort.GetVariable();
                            if (userDataSrc.value != userDataStar.value)
                            {
                                if (!ATRtti.IsSubOfTypeId(userDataStar.value, userDataSrc.value))
                                    continue;
                            }
                        }
                    }
                    else if (port.source is LinkPort)
                    {
                    }
                    else
                        continue;
                    compatiblePorts.Add(port);
                }
            }

            return compatiblePorts;
        }
        //-----------------------------------------------------
        void CreateLinkLine()
        {
            UpdateNodeLinks(m_vNodes.Values.ToList());
            return;
            foreach (var db in m_vNodes)
            {
                var graphNode = db.Value;
                var linkOutPort = graphNode.GetLink(false);
                if (linkOutPort != null && graphNode.bindNode.nextActions != null)
                {
                    for (int i = 0; i < graphNode.bindNode.nextActions.Length; ++i)
                    {
                        GraphNode linkNode = GetNode(graphNode.bindNode.nextActions[i]);
                        if (linkNode == null)
                            continue;
                        if (linkNode == graphNode)
                            continue;

                        var linkInPort = linkNode.GetLink(true);
                        if (linkInPort != null)
                        {
                            var edge = new Edge
                            {
                                output = linkOutPort.bindPort,
                                input = linkInPort.bindPort
                            };
                            edge.edgeControl.inputColor = edge.edgeControl.outputColor = EditorPreferences.GetSettings().linkLineColor;
                            edge.edgeControl.capRadius = EditorPreferences.GetSettings().linkLineWidth;
                            linkOutPort.bindPort.Connect(edge);
                            linkInPort.bindPort.Connect(edge);
                            AddElement(edge);
                        }
                    }
                }

                var otherLinks = graphNode.GetOtherLinks();
                foreach (var othLink in otherLinks)
                {
                    var pVar = othLink.Key.GetVariable();
                    if (pVar is VariableInt)
                    {
                        VariableInt pStr = (VariableInt)pVar;
                        //if(!string.IsNullOrEmpty(pStr.value))
                        {
                            //   string[] splits = pStr.value.Split(new char[] { ';', '|' });
                            //  for(int i =0; i < splits.Length; ++i)
                            {
                                short guidTemp = (short)pStr.value;
                                //       if (short.TryParse(splits[i], out var guidTemp))
                                {
                                    GraphNode linkNode = GetNode(guidTemp);
                                    if (linkNode == null)
                                        continue;
                                    if (linkNode == graphNode)
                                        continue;

                                    var linkInPort = linkNode.GetLink(true);
                                    if (linkInPort != null)
                                    {
                                        var edge = new Edge
                                        {
                                            output = othLink.Value.bindPort,
                                            input = linkInPort.bindPort
                                        };
                                        edge.edgeControl.inputColor = edge.edgeControl.outputColor = EditorPreferences.GetSettings().linkLineColor;
                                        edge.edgeControl.capRadius = EditorPreferences.GetSettings().linkLineWidth;
                                        othLink.Value.bindPort.Connect(edge);
                                        linkInPort.bindPort.Connect(edge);
                                        AddElement(edge);
                                    }
                                }
                            }
                        }
                    }
                }

                var inports = graphNode.GetArvgs();
                if (inports != null)
                {
                    for (int i = 0; i < inports.Count; ++i)
                    {
                        int dummyCnt = inports[i].GetDummyCnt();
                        for (int j = 0; j < dummyCnt; ++j)
                        {
                            long linkKey = inports[i].GetLinkPortKey(j);
                            var linkInPort = GetPort(linkKey);
                            if (linkInPort != null && linkInPort.grapNode != graphNode)
                            {
                                var edge = new Edge
                                {
                                    output = linkInPort.bindPort,
                                    input = inports[i].bindPort
                                };
                                edge.edgeControl.capRadius = EditorPreferences.GetSettings().linkLineWidth;
                                linkInPort.bindPort.Connect(edge);
                                inports[i].bindPort.Connect(edge);
                                AddElement(edge);
                            }
                        }
                    }
                }
            }
        }
        //-----------------------------------------------------
        public void UpdateNodeLinks(List<GraphNode> vNodes)
        {
            foreach (var db in vNodes)
            {
                var graphNode = db;
                graphNode.ClearLinks();
                var linkOutPort = graphNode.GetLink(false);
                if (linkOutPort != null && graphNode.bindNode.nextActions != null && graphNode.bindNode.nextActions.Length>0)
                {
                    for (int i = 0; i < graphNode.bindNode.nextActions.Length; ++i)
                    {
                        GraphNode linkNode = GetNode(graphNode.bindNode.nextActions[i]);
                        if (linkNode == null)
                            continue;
                        if (linkNode == graphNode)
                            continue;

                        var linkInPort = linkNode.GetLink(true);
                        if (linkInPort != null)
                        {
                            var edge = new Edge
                            {
                                output = linkOutPort.bindPort,
                                input = linkInPort.bindPort
                            };
                            edge.edgeControl.inputColor = edge.edgeControl.outputColor = EditorPreferences.GetSettings().linkLineColor;
                            edge.edgeControl.capRadius = EditorPreferences.GetSettings().linkLineWidth;
                            linkOutPort.bindPort.Connect(edge);
                            linkInPort.bindPort.Connect(edge);
                            AddElement(edge);
                        }
                    }
                }
                var otherLinks = graphNode.GetOtherLinks();
                foreach (var othLink in otherLinks)
                {
                    var pVar = othLink.Key.GetVariable();
                    if (pVar is VariableInt)
                    {
                        VariableInt pStr = (VariableInt)pVar;

                        short guidTemp = (short)pStr.value;
                        {
                            GraphNode linkNode = GetNode(guidTemp);
                            if (linkNode == null)
                                continue;
                            if (linkNode == graphNode)
                                continue;

                            var linkInPort = linkNode.GetLink(true);
                            if (linkInPort != null)
                            {
                                var edge = new Edge
                                {
                                    output = othLink.Value.bindPort,
                                    input = linkInPort.bindPort
                                };
                                edge.edgeControl.inputColor = edge.edgeControl.outputColor = EditorPreferences.GetSettings().linkLineColor;
                                edge.edgeControl.capRadius = EditorPreferences.GetSettings().linkLineWidth;
                                othLink.Value.bindPort.Connect(edge);
                                linkInPort.bindPort.Connect(edge);
                                AddElement(edge);
                            }
                        }
                    }
                }

                var inports = graphNode.GetArvgs();
                if (inports != null)
                {
                    for (int i = 0; i < inports.Count; ++i)
                    {
                        int dummyCnt = inports[i].GetDummyCnt();
                        for (int j = 0; j < dummyCnt; ++j)
                        {
                            long linkKey = inports[i].GetLinkPortKey(j);
                            var linkInPort = GetPort(linkKey);
                            if (linkInPort != null && linkInPort.grapNode != graphNode)
                            {
                                var edge = new Edge
                                {
                                    output = linkInPort.bindPort,
                                    input = inports[i].bindPort
                                };
                                edge.edgeControl.capRadius = EditorPreferences.GetSettings().linkLineWidth;
                                linkInPort.bindPort.Connect(edge);
                                inports[i].bindPort.Connect(edge);
                                AddElement(edge);
                            }
                        }
                    }
                }
            }
        }
        //------------------------------------------------------
        public void OnEvent(Rect rect, Event e)
        {
        }
        //-----------------------------------------------------
        public void OnUpdate(float fTime)
        {
            foreach (var db in m_vNodes)
            {
                db.Value.Update(fTime);
            }
            if(m_bRegisterUndo)
            {
                m_undoRedoSystem.RegisterUndo(null, UndoRedoOperationType.All);
                m_bRegisterUndo = false;
            }
        }
        //-----------------------------------------------------
        public void CreateActionNode(object val)
        {

        }
        //-----------------------------------------------------
        void Repaint()
        {
            m_pOwnerEditorLogic.Repaint();
        }
        //------------------------------------------------------
        internal short GeneratorGUID()
        {
            short guid = 1;
            int maxStack = 1000000;
            while (m_vGuidNodes.ContainsKey(guid))
            {
                guid++;
                maxStack--;
                if (maxStack <= 0) break;
            }
            return guid;
        }
        //-----------------------------------------------------
        public List<ArvgPort> GetArvgPorts()
        {
            List<ArvgPort> vPorts = new List<ArvgPort>();
            foreach (var db in m_vNodes)
            {
                var ports = db.Value.GetArvgs();
                if (ports != null)
                    vPorts.AddRange(ports);

                ports = db.Value.GetReturns();
                if (ports != null)
                    vPorts.AddRange(ports);
            }
            return vPorts;
        }
        //-----------------------------------------------------
        public IVariable CreateVariable(ArgvAttribute pAttr, short guid = 0)
        {
            if (guid != 0)
            {
                if (m_vVariables.TryGetValue(guid, out var pExistVar))
                    return pExistVar;
            }
            guid = GeneratorVarGUID();
            IVariable pVar = VariableUtil.CreateVariable(pAttr, guid);
            m_vVariables[guid] = pVar;
            return pVar;
        }
        //------------------------------------------------------
        public void UpdateVariable(IVariable variable)
        {
            // 如果行为树已经启用，则不允许添加新的变量
            //if (isPlaying || m_bLastPlaying)
            //    return;
            m_vVariables[variable.GetGuid()] = variable;
        }
        //-----------------------------------------------------
        public IVariable ChangeVariable(short guid, EVariableType newType)
        {
            m_vVariables.Remove(guid);
            IVariable val = VariableUtil.CreateVariable(newType, guid);
            m_vVariables[guid] = val;
            return val;
        }
        //-----------------------------------------------------
        public IVariable GetVariable(short guid)
        {
            if (m_vVariables.TryGetValue(guid, out var variable))
            {
                return variable;
            }
            return null;
        }
        //-----------------------------------------------------
        public void UpdatePortVariableDefault(ArvgPort port)
        {
            if (!port.attri.canEdit)
            {
                var portVar = GetVariable(port.GetVariableGuid());
                if (portVar != null)
                {
                    portVar = VariableUtil.CreateVariable(port.attri, port.GetVariableGuid());
                    m_vVariables[port.GetVariableGuid()] = portVar;
                }
            }
        }
        //-----------------------------------------------------
        public void UpdatePortsVariableDefault()
        {
            foreach (var db in m_vNodes)
            {
                var argvPorts = db.Value.GetArvgs();
                if (argvPorts != null)
                {
                    foreach (var sub in argvPorts)
                    {
                        db.Value.SetPortDefalueValue(sub);
                    }
                }
                var returnPort = db.Value.GetReturns();
                if (returnPort != null)
                {
                    foreach (var sub in returnPort)
                    {
                        db.Value.SetPortDefalueValue(sub);
                    }
                }
            }
        }
        //------------------------------------------------------
        public GraphNode GetVariableOwnerNode(short guid)
        {
            foreach (var db in m_vNodes)
            {
                if (db.Value.bindNode.type == (ushort)EActionType.eGetVariable)
                    continue;
                var argvPorts = db.Value.GetArvgs();
                if (argvPorts != null)
                {
                    foreach (var sub in argvPorts)
                    {
                        if (sub.GetVariableGuid() == guid)
                            return db.Value;
                    }
                }
                var returnPort = db.Value.GetReturns();
                if (returnPort != null)
                {
                    foreach (var sub in returnPort)
                    {
                        if (sub.GetVariableGuid() == guid)
                            return db.Value;
                    }
                }
            }
            return null;
        }
        //------------------------------------------------------
        public void AddPort(ArvgPort port)
        {
            m_vPorts[port.GetKey()] = port;
        }
        //-----------------------------------------------------
        public void DelPort(ArvgPort port)
        {
            m_vPorts.Remove(port.GetKey());
            if (port.GetVariable() != null)
                RemoveVariable(port.GetVariable().GetGuid());
        }
        //-----------------------------------------------------
        public ArvgPort GetPort(long key)
        {
            if (m_vPorts.TryGetValue(key, out var port))
            {
                return port;
            }
            return null;
        }
        //-----------------------------------------------------
        public void RemoveVariable(short guid)
        {
            if (m_vVariables.ContainsKey(guid))
            {
                m_vVariables.Remove(guid);
            }
        }
        //------------------------------------------------------
        public short GeneratorVarGUID()
        {
            short guid = 1;
            int maxStack = 1000000;
            while (m_vVariables.ContainsKey(guid))
            {
                guid++;
                maxStack--;
                if (maxStack <= 0) break;
            }
            return guid;
        }
        //-----------------------------------------------------
        public bool OnNotifyExecutedNode(AgentTree pAgentTree, BaseNode pNode)
        {
            if (m_vGuidNodes.TryGetValue(pNode.guid, out var graphNode) && graphNode != null)
            {
                graphNode.OnNotifyExcuted();
                graphNode.FlashRed(1f);
            }
            return false;
        }
        //-----------------------------------------------------
        public override void AddToSelection(ISelectable selectable)
        {
            base.AddToSelection(selectable);
        }
        //-----------------------------------------------------
        public override void RemoveFromSelection(ISelectable selectable)
        {
            base.RemoveFromSelection(selectable);
        }
    }
}

#endif