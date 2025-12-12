#if UNITY_EDITOR && USE_ACTORSYSTEM
/********************************************************************
生成日期:	11:03:2023
类    名: 	ActionEditorWindow
作    者:	HappLI
描    述:	表现图编辑窗口
*********************************************************************/
using Codice.Client.Common;
using Framework.ActorSystem.Runtime;
using Framework.AT.Editor;
using Framework.AT.Runtime;
using Framework.Core;
using Framework.Cutscene.Editor;
using Framework.Cutscene.Runtime;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if USE_FIXEDMATH
using ExternEngine;
#else
using UnityEngine;
using UnityEngine;
using FFloat = System.Single;
using FMatrix4x4 = UnityEngine.Matrix4x4;
using FQuaternion = UnityEngine.Quaternion;
using FVector2 = UnityEngine.Vector2;
using FVector3 = UnityEngine.Vector3;
#endif

namespace Framework.ActorSystem.Editor
{
    public class ActionEditorWindow : Framework.Cutscene.Editor.ACutsceneEditor, ICutsceneCallback, IActorSystemCallback
    {
        private const float EDGE_SNAP_OFFSET = 0.25f;
        enum EDragEdge
        {
            None = 0,
            Asset,
            Inspector,
            Timeline,
        }

        bool                            m_bShowActorSpatialDebug = false;
        bool                            m_bDebugAT = false;

        Rect                            m_PreviewRect;

        float                           m_fToolSize = 25;
        Rect                            m_InspectorRect;
        Vector2                         m_InspectorScoller;
        Rect                            m_TimelineRect;
        Vector2                         m_TimelineScoller;
        Rect                            m_AssetRect;
        Vector2                         m_AssetScoller;
        Rect                            m_ToolBarRect;

        EDragEdge m_eDragEdge = EDragEdge.None;

        float                           m_ViewLeftRate;
        float                           m_ViewRightRate;
        float                           m_ViewHeightRate;

        GUIStyle                        m_TileStyle;

        private Rect                    m_CutsceneAssetRect;
        private Rect                    m_CutsceneTimelineRect;
        private Rect                    m_CutsceneInspectorRect;
        CutsceneInstance                m_pCutsceneInstance = null;
        private bool                    m_bCutscenInspectorExpand = true;

        Framework.AT.Editor.AgentTreeWindow m_pSkillEditor = null;
        private Skill                   m_pDummySkill = new Skill();
        AActorComponent                  m_pActorComp = null;
        Actor                           m_pActor = null;
        Actor                           m_pTarget = null;

        List<ACutsceneLogic>            m_vCutsceneLogics = null;

        Vector3 m_ActorPosition         = Vector3.zero;
        Vector3 m_ActorEulerAngle       = Vector3.zero;
        Vector3 m_TargetPosition        = Vector3.forward*3;
        Vector3 m_TargetEulerAngle      = Vector3.zero;
        //--------------------------------------------------------
        public Actor Actor
        {
            get { return m_pActor; }
        }
        //--------------------------------------------------------
        public Actor Target
        {
            get { return m_pTarget; }
        }
        //--------------------------------------------------------
        [MenuItem("Tools/GamePlay/动作编辑器")]
        public static void Open()
        {
            if (EditorApplication.isCompiling)
            {
                EditorUtility.DisplayDialog("警告", "请等待编辑器完成编译再执行此功能", "确定");
                return;
            }
            ActionEditorWindow window = EditorWindow.GetWindow<ActionEditorWindow>();
            window.titleContent = new GUIContent("动作编辑器");
        }
        //--------------------------------------------------------
        public static void OpenTarget(AActorComponent pActor)
        {
            if (EditorApplication.isCompiling)
            {
                EditorUtility.DisplayDialog("警告", "请等待编辑器完成编译再执行此功能", "确定");
                return;
            }
            ActionEditorWindow window = EditorWindow.GetWindow<ActionEditorWindow>();
            window.titleContent = new GUIContent("动作编辑器-" + pActor.gameObject.name);
            window.m_pActorComp = pActor;
        }
        //--------------------------------------------------------
        protected override void OnInnerEnable()
        {
            GetActorManager().InitializeSpatialIndex(new Bounds(new Vector3(0,0,0), new Vector3(10, 10, 10)));
            GetActorManager().RegisterCallback(this);
            RefreshProjectileDatas();

            this.minSize = new Vector2(400, 300);
            this.wantsMouseMove = true;
            this.wantsMouseEnterLeaveWindow = true;
            this.autoRepaintOnSceneChange = true;
            this.wantsLessLayoutEvents = true;

            m_ViewLeftRate = 0.25f;
            m_ViewRightRate = 0.75f;
            m_ViewHeightRate = 0.65f;

            m_TileStyle = new GUIStyle();
            m_TileStyle.fontSize = 20;
            m_TileStyle.normal.textColor = Color.white;
            m_TileStyle.alignment = TextAnchor.MiddleCenter;
#if USE_CUTSCENE
            RegisterLogic<Framework.Cutscene.Editor.AssetDrawLogic>().InitRectMethod(this.GetType(), "m_CutsceneAssetRect");
            RegisterLogic<Framework.Cutscene.Editor.TimelineDrawLogic>().InitRectMethod(this.GetType(), "m_CutsceneTimelineRect");
            RegisterLogic<Framework.Cutscene.Editor.InspectorDrawLogic>().InitRectMethod(this.GetType(), "m_CutsceneInspectorRect");
            RegisterLogic<Framework.Cutscene.Editor.UndoLogic>();
            m_vCutsceneLogics = GetLogics<ACutsceneLogic>();
            RegisterLogic<Framework.Cutscene.Editor.TimelineDrawLogic>().SetSelfRepaint(false);
#endif
        }
        //--------------------------------------------------------
        internal void RefreshProjectileDatas()
        {
            string[] guideDatas = AssetDatabase.FindAssets("t:AProjectileDatas");
            if (guideDatas != null && guideDatas.Length > 0)
            {
                var projectDatas = AssetDatabase.LoadAssetAtPath<AProjectileDatas>(AssetDatabase.GUIDToAssetPath(guideDatas[0]));
                AProjectileDatas.RefreshDatas(projectDatas, false);
                GetActorManager().SetProjectileDatas(projectDatas);
            }
        }
        //--------------------------------------------------------
        internal Skill GetDummySkill()
        {
            return m_pDummySkill;
        }
        //--------------------------------------------------------
        protected override void OnInnerDisable()
        {
            if (m_pSkillEditor != null) m_pSkillEditor.Close();
            m_pSkillEditor = null;
        }
        //--------------------------------------------------------
        protected override void OnInnerDestroy()
        {
            if (m_pActor != null) m_pActor.Destroy();
            m_pActor = null;
            if (m_pTarget != null) m_pTarget.Destroy();
            m_pTarget = null;
        }
        //--------------------------------------------------------
        protected override void OnStart()
        {
            m_pActor = null;
            if (m_pActorComp)
            {
                var pInstance = GameObject.Instantiate<GameObject>(m_pActorComp.gameObject);
                var pComp = pInstance.GetComponent<AActorComponent>();
                pComp.SetBindPrefab(m_pActorComp.gameObject);

                Actor pActorInstance = null;
                pActorInstance = GetActorManager().CreateActor(null, null, -9999);
                pActorInstance.SetAttackGroup(0);
                pActorInstance.OnCreated();
                pActorInstance.SetObjectAble(pComp);
                pActorInstance.SetActived(true);
                pActorInstance.SetVisible(true);
                pActorInstance.EnableLogic(true);
                pActorInstance.EnableRVO(false);
                pActorInstance.SetAttackGroup(0);
                SelectActor(pActorInstance);
                m_pActor = pActorInstance;
                m_pActor.SetPosition(m_ActorPosition);
                m_pActor.SetEulerAngle(m_ActorEulerAngle);
#if USE_CUTSCENE
                m_pCutsceneInstance = pActorInstance.GetActorGraph().GetCutsceneInstance();
                m_pCutsceneInstance.RegisterCallback(this);
                pActorInstance.GetActorGraph().SetAutoUpdate(false);
#endif
                m_pDummySkill.SetSkillSystem(m_pActor.GetSkillSystem());
                m_pDummySkill.SetLevel(1);
                m_pDummySkill.SetActived(true);

                m_pActor.GetActorGraph().AddStartActionCallback(OnStartAction);
                m_pActor.GetActorGraph().AddChangeActionCallback(OnChangeAction);
            }

            if (m_pTarget==null)
            {
                m_pTarget = GetActorManager().CreateActor(null, null, -9998);
                m_pTarget.OnCreated();
                m_pTarget.SetPosition(m_TargetPosition);
                m_pTarget.SetEulerAngle(m_TargetEulerAngle);
                m_pTarget.SetBound(Vector3.one * -0.5f, Vector3.one * 0.5f);
                GameObject pInst = GameObject.CreatePrimitive(PrimitiveType.Cube);
                m_pTarget.SetObjectAble(Framework.ED.EditorUtils.AddUnityScriptComponent<AActorComponent>(pInst));
            }
            m_pTarget.SetActived(true);
            m_pTarget.SetVisible(true);
            m_pTarget.EnableLogic(true);
            m_pTarget.EnableRVO(false);
            m_pTarget.SetAttackGroup(1);
            if (m_pActor != null)
            {
                m_pActor.GetSkillSystem().AddLockTarget(m_pTarget);
                m_pActor.GetSkillSystem().AddSkill(m_pDummySkill, ESkillType.eInitiative);

            }
        }
        //--------------------------------------------------------
        protected override void OnInnerUpdate()
        {
            RefreshLayout();
#if USE_CUTSCENE
            if (m_pCutsceneInstance != null)
            {
                m_pCutsceneInstance.BindData(m_pActor);
            }
#endif           
            this.ForceRepaint();
        }
        //--------------------------------------------------------
        public Rect InspectorRect
        {
            get { return m_InspectorRect; }
        }
        //--------------------------------------------------------
        public Rect AssetRect
        {
            get { return m_AssetRect; }
        }
        //--------------------------------------------------------
        public Rect PreviewRect
        {
            get { return m_PreviewRect; }
        }
        //--------------------------------------------------------
        public Rect TimelineRect
        {
            get { return m_TimelineRect; }
        }
        //--------------------------------------------------------
        public Rect ToolBarRect
        {
            get { return m_ToolBarRect; }
        }
        //--------------------------------------------------------
        public GUIStyle TileStyle
        {
            get { return m_TileStyle; }
        }
        //--------------------------------------------------------
        public void OpenSkillEditor()
        {
            var actor = GetLogic<AssetDrawLogic>().GetActor();
            if (actor == null)
                return;
            var actorComp = actor.GetObjectAble();
            AActorComponent component = actorComp.CastContextData<AActorComponent>();
            if(m_pActorComp!=null)
            {
                component = m_pActorComp;
            }
            else
            {
                var prefab = PrefabUtility.GetCorrespondingObjectFromSource<GameObject>(component.gameObject);
                if (prefab != null)
                {
                    component = prefab.GetComponent<AActorComponent>();
                }
            }
            if (component == null)
                return;

            if (m_pSkillEditor != null)
            {
                if (m_pSkillEditor.GetATData() == component.ATData)
                {
                    m_pSkillEditor.Focus();
                    return;
                }
                m_pSkillEditor.Close();
            }
            if(m_pActor!=null)
            {
                m_pActor.GetAgent<ActorAgentTree>()?.LoadAT(component.ATData);
            }
             m_pSkillEditor = AgentTreeWindow.Open(component.ATData, component, m_pActor.GetAgent<ActorAgentTree>().GetAT(), (data) =>{
                 component.ATData = data;
             });
        }
        //--------------------------------------------------------
        protected override void OnInnerEvent(Event evt)
        {
            if(m_bDebugAT && m_pActor!=null && (evt.type == EventType.KeyUp|| evt.type == EventType.KeyDown))
            {
                var actorAT = m_pActor.GetAgent<ActorAgentTree>();
                if (actorAT != null && actorAT.GetAT() != null)
                {
                    if (actorAT.GetAT().EditorKeyEvent(evt))
                    {
                        evt.Use();
                    }
                }
            }

            if (evt.control && evt.type == EventType.KeyDown && evt.keyCode == KeyCode.Z)
            {
                evt.Use();
            }
        }
        //--------------------------------------------------------
        protected override void OnInnerGUI()
        {
            base.OnInnerGUI();
            ProcessDragEdge(Event.current);
        }
        //--------------------------------------------------------
        protected override void OnAfterInnerGUI()
        {
            var processManipulators = Event.current.type != EventType.Repaint && Event.current.type != EventType.Layout;
            bool bRepaint = Event.current.type == EventType.Repaint;
            //! top split toobar line
            if (bRepaint)
                Framework.ED.UIDrawUtils.DrawColorLine(new Vector2(0, m_fToolSize), m_ToolBarRect.size, Color.grey);
            bool bDragOver = false;
            //! asset line
            {
                Color lineColor = Color.grey;
                var rect = new Rect(m_AssetRect.xMax-10, m_AssetRect.yMin, 10, m_AssetRect.height);
                EditorGUIUtility.AddCursorRect(rect, MouseCursor.SplitResizeLeftRight);
                if (CheckEdgeDrag(EDragEdge.Asset, rect))
                {
                    lineColor = Color.yellow;
                    bDragOver = true;
                }
                if (bRepaint)
                    Framework.ED.UIDrawUtils.DrawColorLine(new Vector2(m_AssetRect.xMax, m_AssetRect.yMin), new Vector2(m_AssetRect.xMax, m_AssetRect.yMax), lineColor);
            }
            //! inspector
            {
                Color lineColor = Color.grey;
                var rect = new Rect(m_InspectorRect.xMin, m_InspectorRect.yMin, 10, m_InspectorRect.height);
                EditorGUIUtility.AddCursorRect(rect, MouseCursor.SplitResizeLeftRight);
                if (CheckEdgeDrag(EDragEdge.Inspector, rect))
                {
                    lineColor = Color.yellow;
                    bDragOver = true;
                }
                if (bRepaint)
                    Framework.ED.UIDrawUtils.DrawColorLine(new Vector2(m_InspectorRect.xMin + 0.01f, m_InspectorRect.yMin), new Vector2(m_InspectorRect.xMin + 0.01f, m_InspectorRect.yMax), lineColor);
            }
            //! timeline split line
            {
                Color lineColor = Color.grey;
                var rect = new Rect(m_TimelineRect.xMin, m_TimelineRect.yMin, m_TimelineRect.width, 10);
                EditorGUIUtility.AddCursorRect(rect, MouseCursor.SplitResizeUpDown);
                if (CheckEdgeDrag(EDragEdge.Timeline, rect))
                {
                    lineColor = Color.yellow;
                    bDragOver = true;
                }
                if (bRepaint)
                {
#if USE_CUTSCENE
                    Framework.ED.UIDrawUtils.DrawColorLine(new Vector2(m_TimelineRect.xMin, m_TimelineRect.yMin + 0.01f), new Vector2(m_bCutscenInspectorExpand?m_InspectorRect.xMin:m_TimelineRect.xMax, m_TimelineRect.yMin + 0.01f), lineColor);
#else
                    UIDrawUtils.DrawColorLine(new Vector2(m_TimelineRect.xMin, m_TimelineRect.yMin + 0.01f), new Vector2(m_TimelineRect.xMax, m_TimelineRect.yMin + 0.01f), lineColor);
#endif
                }
            }

#if USE_CUTSCENE
            if(GUI.Button(new Rect(m_CutsceneInspectorRect.xMax-20, m_CutsceneInspectorRect.yMin, 20, 20),"U"))
            {
                m_bCutscenInspectorExpand = !m_bCutscenInspectorExpand;
            }
#endif
        }
        //--------------------------------------------------------
        bool CheckEdgeDrag(EDragEdge type, Rect region)
        {
            if (m_eDragEdge == type) return true;
            if (region.Contains(Event.current.mousePosition))
            {
                if (Event.current.type == EventType.MouseDrag)
                    m_eDragEdge = type;
                return true;
            }
            return false;
        }
        //--------------------------------------------------------
        void ProcessDragEdge(Event evt)
        {
            if (m_eDragEdge == EDragEdge.None) return;
            if (evt.type == EventType.MouseUp)
            {
                m_eDragEdge = EDragEdge.None;
                return;
            }
            if (evt.button == 0)
            {
                if (evt.type != EventType.MouseDrag)
                    return;

                switch (m_eDragEdge)
                {
                    case EDragEdge.Asset:
                        {
                            m_ViewLeftRate += Event.current.delta.x / this.position.width;
                            evt.Use();
                        }
                        break;
                    case EDragEdge.Inspector:
                        {
                            m_ViewRightRate += Event.current.delta.x / this.position.width;
                            evt.Use();
                        }
                        break;
                    case EDragEdge.Timeline:
                        {
                            m_ViewHeightRate += Event.current.delta.y / (this.position.height - m_fToolSize);
                            evt.Use();
                        }
                        break;
                }
            }
        }
        //--------------------------------------------------------
        void RefreshLayout()
        {
            m_ViewLeftRate = Mathf.Clamp(m_ViewLeftRate, 0.1f, 0.5f);
            m_ViewRightRate = Mathf.Clamp(m_ViewRightRate, 0.55f, 0.8f);
            m_ViewHeightRate = Mathf.Clamp(m_ViewHeightRate, 0.25f, 0.9f);
            m_PreviewRect.x = this.position.width * m_ViewLeftRate;
            m_PreviewRect.y = m_fToolSize;
            m_PreviewRect.width = this.position.width * m_ViewRightRate - m_PreviewRect.x;
            m_PreviewRect.height = (this.position.height - m_fToolSize) * m_ViewHeightRate;

            m_AssetRect.y = m_InspectorRect.y = m_PreviewRect.y;
            m_AssetRect.width = m_PreviewRect.xMin;
            m_InspectorRect.x = m_PreviewRect.xMax;
            m_InspectorRect.width = this.position.width - m_PreviewRect.xMax;
            m_InspectorRect.height = m_AssetRect.height = m_PreviewRect.height;

            m_TimelineRect.yMin = m_PreviewRect.yMax;
            m_TimelineRect.height = this.position.height - m_PreviewRect.yMax;
            m_TimelineRect.width = this.position.width;

            m_ToolBarRect.position = Vector2.zero;
            m_ToolBarRect.size = new Vector2(this.position.width, m_fToolSize);


            m_CutsceneAssetRect.x = m_TimelineRect.x;
            m_CutsceneAssetRect.y = m_TimelineRect.y;
            m_CutsceneAssetRect.width = 200;
            m_CutsceneAssetRect.height = m_TimelineRect.height;

            m_CutsceneTimelineRect.x = m_CutsceneAssetRect.xMax;
            m_CutsceneTimelineRect.y = m_TimelineRect.y;
            m_CutsceneTimelineRect.width = m_InspectorRect.xMin- m_CutsceneAssetRect.xMax;
            m_CutsceneTimelineRect.height = m_TimelineRect.height;

            if(m_bCutscenInspectorExpand)
            {
                m_CutsceneInspectorRect.x = m_InspectorRect.x;
                m_CutsceneInspectorRect.y = m_InspectorRect.y;
                m_CutsceneInspectorRect.width = m_InspectorRect.width;
                m_CutsceneInspectorRect.height = this.position.height- m_InspectorRect.y;
            }
            else
            {
                m_CutsceneInspectorRect.x = m_InspectorRect.x;
                m_CutsceneInspectorRect.y = m_TimelineRect.y;
                m_CutsceneInspectorRect.width = m_InspectorRect.width;
                m_CutsceneInspectorRect.height = m_TimelineRect.height;
            }
        }
        //--------------------------------------------------------
        void OnChangeAction(ActorAction pAction)
        {
            m_pDummySkill.SetActionTypeAndTag(pAction.type, pAction.actionTag);
            var actors = GetLogics();
            for (int i = 0; i < actors.Count; ++i)
            {
                if (actors[i] is ActionEditorLogic)
                {
                    (actors[i] as ActionEditorLogic).OnChangeAction(pAction);
                }
            }
        }
        //--------------------------------------------------------
        void OnStartAction(ActorAction pAction)
        {
            m_pDummySkill.SetActionTypeAndTag(pAction.type, pAction.actionTag);
            var actors = GetLogics();
            for (int i = 0; i < actors.Count; ++i)
            {
                if (actors[i] is ActionEditorLogic)
                {
                    (actors[i] as ActionEditorLogic).OnStartAction(pAction);
                }
            }
        }
        //--------------------------------------------------------
        public void SelectActor(Actor pActor)
        {
            var actors = GetLogics();
            for(int i =0; i < actors.Count; ++i)
            {
                if (actors[i] is ActionEditorLogic)
                {
                    (actors[i] as ActionEditorLogic).OnSelectActor(pActor);
                }
            }
        }
        //--------------------------------------------------------
        public ActorManager GetActorManager()
        {
            return GetEditorGame().GetModule<ActorManager>();
        }
        //--------------------------------------------------------
        public bool IsShowSpatialActorDebug()
        {
            return m_bShowActorSpatialDebug;
        }
        //--------------------------------------------------------
        public void ShowSpatialActorDebug()
        {
            m_bShowActorSpatialDebug = !m_bShowActorSpatialDebug;
        }
        //--------------------------------------------------------
        public bool IsDebugAT()
        {
            return m_bDebugAT;
        }
        //--------------------------------------------------------
        public void DebugAT(bool bDebug)
        {
            m_bDebugAT = bDebug;
        }
        //--------------------------------------------------------
        public void OnPreviewDraw(int controllerId, Camera camera, Event evt)
        {
            AgentTreeManager mgr = GetEditorGame().GetModule<AgentTreeManager>();
            if(mgr!=null)
            {
                mgr.SetMainCamera(camera);
                mgr.SetNegScreenY(true);
            }
            if (m_vCutsceneLogics != null)
            {
                foreach (var logic in m_vCutsceneLogics)
                {
                    logic.OnPreviewDraw(controllerId, camera, evt);
                }
            }
            if(evt.shift)
            {
                Handles.Label(m_ActorPosition, "模拟位置", EditorStyles.boldLabel);
                Handles.Label(m_TargetPosition, "模拟敌方位置", EditorStyles.boldLabel);
                if (Tools.current == Tool.Rotate)
                {
                    m_ActorEulerAngle = Handles.DoRotationHandle(Quaternion.Euler(m_ActorEulerAngle),m_ActorPosition).eulerAngles;
                    m_TargetEulerAngle = Handles.DoRotationHandle(Quaternion.Euler(m_TargetEulerAngle), m_TargetPosition).eulerAngles;
                }
                else
                {
                    m_ActorPosition = Handles.DoPositionHandle(m_ActorPosition, Quaternion.identity);
                    m_TargetPosition = Handles.DoPositionHandle(m_TargetPosition, Quaternion.identity);
                }
            }
            if(m_bShowActorSpatialDebug)
            {
#if !UNITY_5_1
                UnityEngine.Rendering.CompareFunction zTest = Handles.zTest;
                Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
#endif
                GetActorManager().DrawDebug(false);
#if !UNITY_5_1
                Handles.zTest = zTest;
#endif
            }

            if(m_bDebugAT && m_pActor!=null)
            {
                if (evt.type == EventType.MouseDown)
                {
                    var actorAT = m_pActor.GetAgent<ActorAgentTree>();
                    if (actorAT != null && actorAT.GetAT() != null)
                    {
                        TouchInput.TouchData touch = TouchInput.TouchData.DEF;
                        touch.position = evt.mousePosition;
                        touch.lastPosition = evt.mousePosition;
                        touch.deltaPosition = evt.delta;
                        touch.isUITouched = false;
                        if (actorAT.GetAT().MouseInputEvent(EATMouseType.Begin, touch))
                        {
                            evt.Use();
                        }
                    }
                }
                if (evt.type == EventType.MouseUp)
                {
                    var actorAT = m_pActor.GetAgent<ActorAgentTree>();
                    if (actorAT != null && actorAT.GetAT() != null)
                    {
                        TouchInput.TouchData touch = TouchInput.TouchData.DEF;
                        touch.position = evt.mousePosition;
                        touch.lastPosition = evt.mousePosition;
                        touch.deltaPosition = evt.delta;
                        touch.isUITouched = false;
                        if (actorAT.GetAT().MouseInputEvent(EATMouseType.End, touch))
                        {
                            evt.Use();
                        }
                    }
                }
                if (evt.type == EventType.MouseDrag)
                {
                    var actorAT = m_pActor.GetAgent<ActorAgentTree>();
                    if (actorAT != null && actorAT.GetAT() != null)
                    {
                        TouchInput.TouchData touch = TouchInput.TouchData.DEF;
                        touch.position = evt.mousePosition;
                        touch.lastPosition = evt.mousePosition;
                        touch.deltaPosition = evt.delta;
                        touch.isUITouched = false;
                        if (actorAT.GetAT().MouseInputEvent(EATMouseType.Move, touch))
                        {
                            evt.Use();
                        }
                    }
                }
            }
            
        }
        //--------------------------------------------------------
        public override bool IsRuntimeOpenPlayingCutscene()
        {
            return false;
        }
        //--------------------------------------------------------
        public override void OpenRuntimePlayingCutscene(CutsceneInstance pInstance)
        {
            m_pCutsceneInstance = pInstance;
        }
        //--------------------------------------------------------
        public override CutsceneInstance GetCutsceneInstance()
        {
            return m_pCutsceneInstance;
        }
        //--------------------------------------------------------
        public override void OpenAgentTreeEdit()
        {
        }
        //--------------------------------------------------------
        public override void SaveAgentTreeData()
        {
        }
        //--------------------------------------------------------
        public override AT.Editor.AgentTreeWindow GetAgentTreeWindow()
        {
            return null;
        }
        //--------------------------------------------------------
        public override void OnSetTime(float time)
        {
            var logic = GetLogic<Framework.Cutscene.Editor.TimelineDrawLogic>();
            if (logic == null)
                return;
            logic.SetCurrentTime(time);
        }
        //--------------------------------------------------------
        public void OnCutsceneStatus(int cutsceneInstanceId, EPlayableStatus status)
        {
            if(status == EPlayableStatus.Start || status == EPlayableStatus.Create)
            {
                if (m_pActor != null)
                {
                    m_pActor.SetPosition(m_ActorPosition);
                    m_pActor.SetEulerAngle(m_ActorEulerAngle);
                    m_pActor.SetSpeed(Vector3.zero);
                }
                if (m_pTarget != null)
                {
                    m_pTarget.SetPosition(m_TargetPosition);
                    m_pTarget.SetEulerAngle(m_TargetEulerAngle);
                    m_pTarget.SetSpeed(Vector3.zero);

                }
            }
            if(status == EPlayableStatus.Create)
            {
                if (m_pActor != null)
                {
                    if (m_pTarget != null)
                    {
                        m_pActor.GetSkillSystem().AddLockTarget(m_pTarget, true);
                        m_pActor.GetSkillSystem().DoSkill(m_pDummySkill);
                    }
                }
            }
        }
        //--------------------------------------------------------
        public bool OnCutscenePlayableCreateClip(CutscenePlayable playable, CutsceneTrack track, IBaseClip clip)
        {
            return false;
        }
        //--------------------------------------------------------
        public bool OnCutscenePlayableDestroyClip(CutscenePlayable playable, CutsceneTrack track, IBaseClip clip)
        {
            return false;
        }
        //--------------------------------------------------------
        public bool OnCutscenePlayableFrameClip(CutscenePlayable playable, FrameData frameData)
        {
            return false;
        }
        //--------------------------------------------------------
        public bool OnCutscenePlayableFrameClipEnter(CutscenePlayable playable, CutsceneTrack track, FrameData frameData)
        {
            return false;
        }
        //--------------------------------------------------------
        public bool OnCutscenePlayableFrameClipLeave(CutscenePlayable playable, CutsceneTrack track, FrameData frameData)
        {
            return false;
        }
        //--------------------------------------------------------
        public bool OnCutsceneEventTrigger(CutscenePlayable pPlayable, CutsceneTrack pTrack, IBaseEvent pEvent)
        {
            return false;
        }
        //--------------------------------------------------------
        public bool OnAgentTreeExecute(AgentTree pAgentTree, BaseNode pNode)
        {
            return false;
        }
        //--------------------------------------------------------
        public bool OnLoadAsset(string name, Action<UnityEngine.Object> onLoaded, bool bAsync = true)
        {
            return false;
        }
        //--------------------------------------------------------
        public bool OnUnloadAsset(UnityEngine.Object pAsset)
        {
            return false;
        }
        //--------------------------------------------------------
        public bool OnSpawnInstance(string name, Action<GameObject> onLoaded, bool bAsync = true)
        {
            return false;
        }
        //--------------------------------------------------------
        public bool OnDespawnInstance(GameObject pInstance, string name = null)
        {
            return false;
        }
        //--------------------------------------------------------
        public bool OnActorSystemActorCallback(Actor pActor, EActorStatus eStatus, IContextData pTakeData = null)
        {
            if(eStatus == EActorStatus.Loaded)
            {
                var uniObj = pActor.GetUniyTransform();
                if(uniObj!=null)
                {
                    var logics = GetLogics();
                    for (int i = 0; i < logics.Count; ++i)
                    {
                        if (logics[i] is ActionEditorLogic)
                        {
                            (logics[i] as ActionEditorLogic).OnSpwanGameObejct(uniObj.gameObject);
                        }
                    }
                }
            }
            else if (eStatus == EActorStatus.Create)
            {
                var contextData = pActor.GetContextData();
                if (contextData != null)
                {
                    if (pActor is ProjectileActor)
                    {
                        ProjectileActor projectorActor = pActor as ProjectileActor;
                        if (contextData is ProjectileData)
                        {
                            ProjectileData projData = (ProjectileData)contextData;
                            if (!string.IsNullOrEmpty(projData.effect))
                            {
                                var prefabInst = ActorSystemUtil.EditLoadUnityObject(projData.effect) as GameObject;
                                if (prefabInst != null)
                                {
                                    var projectileObj = GameObject.Instantiate(prefabInst);
                                    AActorComponent pComp = projectileObj.GetComponent<AActorComponent>();
                                    if (pComp == null) pComp = Framework.ED.EditorUtils.AddUnityScriptComponent<AActorComponent>(projectileObj);
                                    projectorActor.SetObjectAble(pComp);
                                }
                            }
                        }
                    }

                }
            }
            return false;
        }
        //--------------------------------------------------------
        public bool OnActorSystemActorAttrDirty(Actor pActor, byte attrType, FFloat oldValue, FFloat newValue, IContextData externVar = null)
        {
            return false;
        }
        //--------------------------------------------------------
        public bool OnActorSystemActorHitFrame(HitFrameActor hitFrameActor)
        {
            return false;
        }
    }
}

#endif