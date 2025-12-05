#if UNITY_EDITOR
/********************************************************************
生成日期:	25:7:2019   14:35
类    名: 	ProjectileEditor
作    者:	HappLI
描    述:	飞行道具编辑器
*********************************************************************/
using Framework.ActorSystem.Editor;
using Framework.ActorSystem.Runtime;
using Framework.AT.Runtime;
using Framework.Cutscene.Runtime;
using Framework.ED;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

#if USE_FIXEDMATH
using ExternEngine;
#else
using UnityEngine;
using FFloat = System.Single;
using FMatrix4x4 = UnityEngine.Matrix4x4;
using FQuaternion = UnityEngine.Quaternion;
using FVector2 = UnityEngine.Vector2;
using FVector3 = UnityEngine.Vector3;
#endif

namespace Framework.ProjectileSystem.Editor
{
    public class ProjectileEditor : EditorWindowBase, ICutsceneCallback, IActorSystemCallback
    {
        public static ProjectileEditor Instance { protected set; get; }

        static float GapTop = 50f;
        static float GapBottom = 0;

        public List<string> m_vBoneList = new List<string>();

        AProjectileDatas m_ProjectileDatas = null;

        private uint m_nEditProjectileId = 0xffffffff;

        Rect m_LayerSize = new Rect();
        Rect m_InspecSize = new Rect();
        Rect m_DataListSize = new Rect();
        CutsceneManager m_CutsceneManager = null;
        ActorManager    m_pActorManager;
        byte m_DragSplitGap = 0;
        TimelinePanel m_Timeline = new TimelinePanel();
        //-----------------------------------------------------
        [MenuItem("Tools/GamePlay/飞行道具编辑器 _F7")]
        internal static void StartEditor()
        {
            if (Instance == null)
                EditorWindow.GetWindow<ProjectileEditor>();
            Instance.titleContent = new GUIContent("飞行道具编辑器");
            Instance.Focus();
        }
        //-----------------------------------------------------
        public static void EditorProjectile(ProjectileData projectileData)
        {
            StartEditor();
            Instance.OnChangeSelect(projectileData);
        }
        //-----------------------------------------------------
        public static void EditorProjectile(uint projectileId)
        {
            StartEditor();
            Instance.m_nEditProjectileId = projectileId;
        }
        //-----------------------------------------------------
        public AProjectileDatas GetProjectileDatas()
        {
            return m_ProjectileDatas;
        }
        //-----------------------------------------------------
        public void AppEdiorSetup(Camera camera)
        {
            OnReLoad();
        }
        //-----------------------------------------------------
        static public void OnSceneFunc(SceneView sceneView)
        {
            Instance.OnSceneGUI(sceneView);
        }
        //-----------------------------------------------------
        private void OnSceneGUI(SceneView sceneView)
        {
        }
        //-----------------------------------------------------
        public void SetEditorTarget(UnityEngine.Object value, bool bInspectorOPen = true)
        {
            if (value == null) return;
        }
        //-----------------------------------------------------
        public ActorManager GetActorManager()
        {
            return m_pActorManager;
        }
        //-----------------------------------------------------
        protected override void OnInnerDisable()
        {
            Instance = null;

            //             if (EditorApplication.isPlaying)
            //                 EditorApplication.isPlaying = false;

            m_pActorManager.Shutdown();
            SceneView.duringSceneGui -= OnSceneFunc;
        }
        //-----------------------------------------------------
        protected override void OnInnerEnable()
        {
            Instance = this;

            //! 加载飞行道具配置数据
            string[] guideDatas = AssetDatabase.FindAssets("t:AProjectileDatas");
            if (guideDatas != null && guideDatas.Length > 0)
            {
                m_ProjectileDatas = AssetDatabase.LoadAssetAtPath<AProjectileDatas>(AssetDatabase.GUIDToAssetPath(guideDatas[0]));
            }
            else
            {
                EditorUtility.DisplayDialog("提示", "没有创建飞行道具数据集，请先创建!!", "好的");
                string savePath = EditorUtility.SaveFilePanelInProject("创建弹道数据集", "AProjectileDatas", "asset", "用于管理零散的弹道配置数据文件", Application.dataPath);
                if(string.IsNullOrEmpty(savePath))
                {
                    return;
                }
                AProjectileDatas projData = Framework.ED.EditorUtils.CreateUnityScriptObject<AProjectileDatas>();
                projData.name = "AProjectileDatas";
                AssetDatabase.CreateAsset(projData, savePath);
                EditorUtility.SetDirty(projData);
                AssetDatabase.SaveAssetIfDirty(projData);
                m_ProjectileDatas = AssetDatabase.LoadAssetAtPath<AProjectileDatas>(savePath);
            }
            if (m_ProjectileDatas != null)
            {
                AProjectileDatas.RefreshDatas(m_ProjectileDatas,false);
            }

            m_CutsceneManager = new CutsceneManager();
            m_CutsceneManager.SetEditorMode(true);
            m_pActorManager = new ActorManager();
            m_pActorManager.Init(m_CutsceneManager);
            m_pActorManager.SetProjectileDatas(m_ProjectileDatas);
            m_pActorManager.RegisterCallback(this);

            GetLogic<ProjectileDataListLogic>()?.Active(false);

            base.minSize = new Vector2(850f, 320f);

            SceneView.duringSceneGui += OnSceneFunc;
        }
        //-----------------------------------------------------
        protected override void OnStart()
        {
            base.OnStart();
            if(m_nEditProjectileId!=0xffffffff)
            {
                var projectileData = m_pActorManager.GetProjectileManager().GetProjectileData(m_nEditProjectileId);
                if(projectileData!=null)
                {
                    OnChangeSelect(projectileData);
                }
                m_nEditProjectileId = 0xffffffff;
            }
        }
        //-----------------------------------------------------
        protected override void OnInnerUpdate()
        {
            if (m_pActorManager != null)
                m_pActorManager.Update(m_pTimer.deltaTime);

            if (m_CutsceneManager != null)
                m_CutsceneManager.Update(m_pTimer.deltaTime);

            this.ForceRepaint();
        }
        //-----------------------------------------------------
        protected override void OnInnerGUI()
        {
            float width = 450;
            ProjectileDataListLogic dataLogic = GetLogic<ProjectileDataListLogic>();
            m_DataListSize = new Rect(0,GapTop, dataLogic.IsActive()?200:0, position.height - GapTop - GapBottom);
            m_LayerSize = new Rect(0, GapTop, this.position.width- width, position.height - GapTop - GapBottom);
            m_InspecSize = new Rect(m_LayerSize.xMax+5, GapTop, width, position.height - GapTop - GapBottom);


            GUILayout.BeginHorizontal();
            DrawToolPanel();
            GUILayout.EndHorizontal();
        }
        //-----------------------------------------------------
        protected override void OnInnerEvent(Event evt)
        {
            if(evt.type == EventType.MouseDown)
            {
                if(m_DragSplitGap == 0)
                {
                    if (new Rect(position.x, GapTop, m_LayerSize.x, m_LayerSize.y).Contains(evt.mousePosition))
                    {
                        m_DragSplitGap = 1;
                        evt.Use();
                    }
                    else if (new Rect(position.x - m_InspecSize.x, GapTop, m_InspecSize.x, m_InspecSize.y).Contains(evt.mousePosition))
                    {
                        m_DragSplitGap = 2;
                        evt.Use();
                    }
                }
            }
            else if(evt.type == EventType.MouseDrag)
            {
                if(m_DragSplitGap == 1)
                {
                    m_LayerSize.x += evt.delta.x;
                    EditorGUIUtility.AddCursorRect(position, MouseCursor.SlideArrow);
                    evt.Use();
                }
                else if (m_DragSplitGap == 2)
                {
                    m_InspecSize.x += evt.delta.x;
                    EditorGUIUtility.AddCursorRect(position, MouseCursor.SlideArrow);
                    evt.Use();
                }
            }
            else if(evt.type == EventType.MouseUp)
            {
                if (m_DragSplitGap != 0) evt.Use();
                m_DragSplitGap = 0;
            }
            else if(evt.type == EventType.KeyDown)
            {
                if (evt.control && evt.keyCode == KeyCode.O)
                {
                    GetLogic<ProjectileDataListLogic>()?.Active(true);
                    evt.Use();
                }
                if (evt.control && evt.keyCode == KeyCode.N)
                {
                    var logics = GetLogics();
                    foreach (var db in logics)
                    {
                        if (db is AProjectileEditorLogic)
                        {
                            ((AProjectileEditorLogic)db).New();
                        }
                    }
                    evt.Use();
                }
                if (evt.control && evt.keyCode == KeyCode.S)
                {
                    SaveChanges();
                    evt.Use();
                }
                if (evt.keyCode == KeyCode.F6)
                {
                    Stop();
                    var logics = GetLogics();
                    foreach (var db in logics)
                    {
                        if (db is AProjectileEditorLogic)
                        {
                            ((AProjectileEditorLogic)db).Reload();
                        }
                    }
                    evt.Use();
                }
                if (evt.control && evt.keyCode == KeyCode.F5)
                {
                    Play();
                    evt.Use();
                }
            }
        }
        //-----------------------------------------------------
        public void OnReLoadAssetData()
        {

        }
        //-----------------------------------------------------
        private void OnReLoad()
        {
            OnReLoadAssetData();
        }
        //-----------------------------------------------------
        public void OnDestroy()
        {
        }
        //-----------------------------------------------------
        private void DrawToolPanel()
        {
            if (GUILayout.Button(new GUIContent("打开", "快捷键:ctrl+o"), new GUILayoutOption[] { GUILayout.Width(80f), GUILayout.Height(45f) }))
            {
                GetLogic<ProjectileDataListLogic>()?.Active(true);
            }
            if (GUILayout.Button(new GUIContent("新建", "快捷键:ctrl+n"), new GUILayoutOption[] { GUILayout.Width(80f), GUILayout.Height(45f) }))
            {
                var logics = GetLogics();
                foreach (var db in logics)
                {
                    if (db is AProjectileEditorLogic)
                    {
                        ((AProjectileEditorLogic)db).New();
                    }
                }
            }
            if (GUILayout.Button(new GUIContent("保存", "快捷键:ctrl+s"), new GUILayoutOption[] { GUILayout.Width(80f), GUILayout.Height(45f) }))
            {
                SaveChanges();
            }
            if (GUILayout.Button(new GUIContent("刷新", "快捷键:F6"), new GUILayoutOption[] { GUILayout.Width(80f), GUILayout.Height(45f) }))
            {
                Stop();
                var logics = GetLogics();
                foreach (var db in logics)
                {
                    if (db is AProjectileEditorLogic)
                    {
                        ((AProjectileEditorLogic)db).Reload();
                    }
                }
            }
            if (GUILayout.Button(new GUIContent("模拟", "快捷键:ctrl+F5"), new GUILayoutOption[] { GUILayout.Width(80f), GUILayout.Height(45f) }))
            {
                Play();
            }
            //if (GUILayout.Button("停止", new GUILayoutOption[] { GUILayout.Width(80f), GUILayout.Height(45f) }))
            //{
            //    Stop();
            //}
            if (m_ProjectileDatas!=null && GUILayout.Button("提交", new GUILayoutOption[] { GUILayout.Width(80f), GUILayout.Height(45f) }))
            {
                AProjectileDatas.RefreshDatas(m_ProjectileDatas, true);
                string  dir = System.IO.Path.GetDirectoryName(AssetDatabase.GetAssetPath(m_ProjectileDatas));
                Framework.ED.EditorUtils.CommitGit(dir);
            }
            if (GUILayout.Button("退出", new GUILayoutOption[] { GUILayout.Width(80f), GUILayout.Height(45f) }))
            {
                base.Close();
            }
            GUILayout.BeginArea(new Rect(position.width - 200,0,200,45));
            GUILayout.BeginHorizontal();
            GUILayout.Label("SpeedSacle:", new GUILayoutOption[] { GUILayout.Width(80) });
            m_pTimer.m_currentSnap = GUILayout.HorizontalSlider(m_pTimer.m_currentSnap, 0.15f, 2f, new GUILayoutOption[] { GUILayout.Width(80f) });
            m_pTimer.m_currentSnap = Mathf.Clamp(EditorGUILayout.FloatField(m_pTimer.m_currentSnap, new GUILayoutOption[] { GUILayout.Width(20) }), 0.15f, 2);
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }
        //-----------------------------------------------------
        public void Play()
        {
            var logics = GetLogics();
            foreach (var db in logics)
            {
                if (db is AProjectileEditorLogic)
                {
                    ((AProjectileEditorLogic)db).Play(true);
                }
            }
        }
        //-----------------------------------------------------
        public void Stop()
        {
            var logics = GetLogics();
            foreach (var db in logics)
            {
                if (db is AProjectileEditorLogic)
                {
                    ((AProjectileEditorLogic)db).Play(false);
                }
            }
        }
        //-----------------------------------------------------
        public void OnSelectProjectileData(ProjectileData pData)
        {
            var logics = GetLogics();
            foreach(var db in logics)
            {
                if(db is AProjectileEditorLogic)
                {
                    ((AProjectileEditorLogic)db).OnChangeSelect(pData);
                }
            }
        }
        //-----------------------------------------------------
        public void OnCutsceneLoadAsset(string name, Action<UnityEngine.Object> onLoaded, bool bAsync = true)
        {
        }
        //-----------------------------------------------------
        public void OnCutsceneUnloadAsset(UnityEngine.Object pAsset)
        {
        }
        //-----------------------------------------------------
        public void OnCutsceneSpawnInstance(string name, Action<GameObject> onLoaded, bool bAsync = true)
        {
        }
        //-----------------------------------------------------
        public void OnCutsceneDespawnInstance(GameObject pInstance, string name = null)
        {
        }
        //-----------------------------------------------------
        public void OnCutsceneStatus(int cutsceneInstanceId, EPlayableStatus status)
        {
        }
        //-----------------------------------------------------
        public bool OnCutscenePlayableCreateClip(CutscenePlayable playable, CutsceneTrack track, IBaseClip clip)
        {
            return false;
        }
        //-----------------------------------------------------
        public bool OnCutscenePlayableDestroyClip(CutscenePlayable playable, CutsceneTrack track, IBaseClip clip)
        {
            return false;
        }
        //-----------------------------------------------------
        public bool OnCutscenePlayableFrameClip(CutscenePlayable playable, FrameData frameData)
        {
            return false;
        }
        //-----------------------------------------------------
        public bool OnCutscenePlayableFrameClipEnter(CutscenePlayable playable, CutsceneTrack track, FrameData frameData)
        {
            return false;
        }
        //-----------------------------------------------------
        public bool OnCutscenePlayableFrameClipLeave(CutscenePlayable playable, CutsceneTrack track, FrameData frameData)
        {
            return false;
        }
        //-----------------------------------------------------
        public bool OnCutsceneEventTrigger(CutscenePlayable pPlayablle, CutsceneTrack pTrack, IBaseEvent pEvent)
        {
            return false;
        }
        //-----------------------------------------------------
        public bool OnAgentTreeExecute(AgentTree pAgentTree, BaseNode pNode)
        {
            return false;
        }
        //-----------------------------------------------------
        public bool OnActorSystemLoadAsset(string name, Action<UnityEngine.Object> onLoaded, bool bAsync = true)
        {
            var obj = ActorSystemUtil.EditLoadUnityObject(name);
            if (obj == null)
                return false;
            if (onLoaded != null) onLoaded(obj);
            return true;
        }
        //-----------------------------------------------------
        public bool OnActorSystemUnloadAsset(UnityEngine.Object pAsset)
        {
            return false;
        }
        //-----------------------------------------------------
        public bool OnActorSystemSpawnInstance(string name, Action<GameObject> onLoaded, bool bAsync = true)
        {
            var obj = ActorSystemUtil.EditLoadUnityObject(name);
            if (obj == null || !(obj is UnityEngine.GameObject))
                return false;
            var prefabInst = obj as GameObject;
            if (onLoaded != null) onLoaded(GameObject.Instantiate(prefabInst));
            return true;
        }
        //-----------------------------------------------------
        public bool OnActorSystemDespawnInstance(GameObject pInstance, string name = null)
        {
            if(pInstance!=null)
            {
                EditorUtil.Destroy(pInstance);
            }
            return false;
        }
        //-----------------------------------------------------
        public bool OnActorSystemActorCallback(Actor pActor, EActorStatus eStatus, IContextData pTakeData = null)
        {
            if (eStatus == EActorStatus.Loaded)
            {
                var uniObj = pActor.GetUniyTransform();
                if (uniObj != null)
                {
                    ProjectilePreview preview = GetLogic<ProjectilePreview>();
                    preview.AddInstance(uniObj.gameObject);
                }
            }
            else if(eStatus == EActorStatus.Create)
            {
                var contextData = pActor.GetContextData();
                if(contextData!=null)
                {
                    if(pActor is ProjectileActor)
                    {
                        ProjectileActor projectorActor = pActor as ProjectileActor;
                        if (contextData is ProjectileData)
                        {
                            ProjectileData projData = (ProjectileData)contextData;
                            if (!string.IsNullOrEmpty(projData.effect))
                            {
                                var prefabInst = ActorSystemUtil.EditLoadUnityObject(projData.effect) as GameObject;
                                if(prefabInst!=null)
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
        //-----------------------------------------------------
        public bool OnActorSystemActorAttrDirty(Actor pActor, byte attrType, FFloat oldValue, FFloat newValue, IContextData externVar = null)
        {
            return false;
        }
        //-----------------------------------------------------
        public bool OnActorSystemActorHitFrame(HitFrameActor hitFrameActor)
        {
            return false;
        }
    }
}
#endif