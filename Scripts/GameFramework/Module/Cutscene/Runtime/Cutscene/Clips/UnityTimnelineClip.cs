/********************************************************************
生成日期:	06:30:2025
类    名: 	TimnelineClip
作    者:	HappLI
描    述:	UnityTimelineClip
*********************************************************************/
#if UNITY_EDITOR
using Framework.Cutscene.Editor;
using UnityEditor;
#endif
using Framework.DrawProps;
using UnityEngine;
using System.Collections.Generic;
using System;
using Framework.AT.Runtime;
using UnityEngine.Playables;
using Framework.Core;

namespace Framework.Cutscene.Runtime
{
    [System.Serializable, CutsceneClip("UnityTimelineClip")]
    public class UnityTimnelineClip : IBaseClip
    {
        [Display("基本属性")] public BaseClipProp baseProp;
        [Display("Timeline资源"), StringViewPlugin("OnDrawSelectPrefabInspector")] public string timeline = "";
        [Display("挂点名"), RowFieldInspector("OnSelectBindSlot")] public string bindNode; // 可为空，表示根节点
        [Display("位置偏移"), DisplayNameByField("bindNode", "null", "位置")] public Vector3 position;
        [Display("旋转偏移"), DisplayNameByField("bindNode", "null", "旋转")] public Vector3 rotate;
        [Display("缩放")] public Vector3 scale = Vector3.one;
        [Display("异步加载")] public bool asyncLoad = true;
        [Display("绑定轨道")] public bool bindTrack = false;
        [Display("识别ID"), StateByField("bindTrack", "true")] public int objId;
        [Display("轴同步")] public bool syncAxis = false; // 是否同步轴向
        //-----------------------------------------------------
        public ACutsceneDriver CreateDriver()
        {
            return new UnityTimelineDriver();
        }
        //-----------------------------------------------------
        public ushort GetIdType()
        {
            return (ushort)EClipType.eUnityTimeline;
        }
        //-----------------------------------------------------
        public float GetDuration()
        {
            return baseProp.duration;
        }
        //-----------------------------------------------------
        public EClipEdgeType GetEndEdgeType()
        {
            return baseProp.endEdgeType;
        }
        //-----------------------------------------------------
        public string GetName()
        {
            return baseProp.name;
        }
        //-----------------------------------------------------
        public ushort GetRepeatCount()
        {
            return baseProp.repeatCnt;
        }
        //-----------------------------------------------------
        public float GetTime()
        {
            return baseProp.time;
        }
        //-----------------------------------------------------
        public float GetBlend(bool bIn)
        {
            return baseProp.GetBlend(bIn);
        }

#if UNITY_EDITOR
        //-----------------------------------------------------
        [AddInspector]
        public void OnEditor()
        {
            if (baseProp.ownerTrackObject != null)
            {
                var drivers = baseProp.ownerTrackObject.GetDrivers(this);
                if (drivers != null)
                {
                    foreach (var db in drivers)
                    {
                        if (db is UnityTimelineDriver)
                        {
                            UnityTimelineDriver particleDirver = ((UnityTimelineDriver)db);
                            var gameObject = particleDirver.GetInstance();
                            if (gameObject)
                            {
                                if (GUILayout.Button("同步时长"))
                                {
                                    PlayableDirector director = gameObject.GetComponent<PlayableDirector>();
                                    if(director == null) director = gameObject.GetComponentInChildren<PlayableDirector>();
                                    if(director)
                                    {
                                        baseProp.duration = (float)director.duration;
                                    }
                                }

                                if (Selection.activeGameObject != gameObject)
                                {
                                    if (string.IsNullOrEmpty(bindNode))
                                    {
                                        gameObject.transform.position = position;
                                        gameObject.transform.eulerAngles = rotate;
                                        gameObject.transform.localScale = scale;
                                    }
                                    else
                                    {
                                        gameObject.transform.localPosition = position;
                                        gameObject.transform.localEulerAngles = rotate;
                                        gameObject.transform.localScale = scale;
                                    }
                                }

                                if (GUILayout.Button("设置当前位置信息"))
                                {
                                    if (string.IsNullOrEmpty(bindNode))
                                    {
                                        position = gameObject.transform.position;
                                        rotate = gameObject.transform.eulerAngles;
                                        scale = gameObject.transform.localScale;
                                    }
                                    else
                                    {
                                        position = gameObject.transform.localPosition;
                                        rotate = gameObject.transform.localEulerAngles;
                                        scale = gameObject.transform.localScale;
                                    }
                                }

                                if (GUILayout.Button("定位选择"))
                                {
                                    Selection.activeObject = gameObject;
                                    if (SceneView.lastActiveSceneView) SceneView.lastActiveSceneView.LookAt(gameObject.transform.position);
                                }
                            }
                            break;
                        }
                    }
                }
            }
        }
        //-----------------------------------------------------
        [NonSerialized] List<string> m_vBindPops = new List<string>();
        [NonSerialized] private Transform m_pLastTransfrom = null;
        public void OnSelectBindSlot(System.Object pOwner, System.Reflection.FieldInfo fieldInfo)
        {
            if (fieldInfo.Name != "bindNode")
                return;
            if (baseProp.ownerTrackObject == null)
                return;
            var bindObj = baseProp.ownerTrackObject.GetBindLastCutsceneObject();
            var objTrans = bindObj?.GetUniyTransform();

            if (m_pLastTransfrom != objTrans)
            {
                m_pLastTransfrom = objTrans;
                m_vBindPops.Clear();
                m_vBindPops.Add("");
                if (objTrans)
                {
                    AddAllChildPaths(objTrans, "", m_vBindPops);
                }
            }
            if (objTrans == null)
                return;

            int nSelect = m_vBindPops.IndexOf(bindNode);
            if (nSelect < 0 && string.IsNullOrEmpty(bindNode)) nSelect = 0;
            nSelect = EditorGUILayout.Popup("", nSelect, m_vBindPops.ToArray(), new GUILayoutOption[] { GUILayout.Width(80) });
            if (nSelect >= 0 && nSelect < m_vBindPops.Count)
            {
                bindNode = m_vBindPops[nSelect];
            }
        }
        //-----------------------------------------------------
        private void AddAllChildPaths(Transform parent, string parentPath, List<string> pathList)
        {
            foreach (Transform child in parent)
            {
                string path = string.IsNullOrEmpty(parentPath) ? child.name : parentPath + "/" + child.name;
                pathList.Add(path);
                AddAllChildPaths(child, path, pathList);
            }
        }
        //-----------------------------------------------------
        void SetDefault()
        {
            if (Camera.main == null)
                return;
            // 获取视野中心点与地表 y=0 的交点
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            float t = 0f;
            if (ray.direction.y != 0f)
            {
                t = -ray.origin.y / ray.direction.y;
            }
            Vector3 groundPos = ray.origin + ray.direction * t;

            position = groundPos;
        }
#endif		
    }
    //-----------------------------------------------------
    //UnityTimeline剪辑逻辑
    //-----------------------------------------------------
    public class UnityTimelineDriver : ACutsceneDriver, ICutsceneObject
    {
        private InstanceAble m_Instance;
        private Transform m_BindTrans;
        private PlayableDirector m_PlayableDirector;
        private Vector3 m_BindOffset;
        private Vector3 m_BindRotate;
        private Vector3 m_Scale;
        private bool m_bSyncAxis = true;
        private string m_lastBindNode = null;
        //-----------------------------------------------------
        public override void OnDestroy()
        {
            DestroyInstnace();
            m_lastBindNode = null;
            m_PlayableDirector = null;
        }
        //-----------------------------------------------------
        public InstanceAble GetInstance()
        {
            return m_Instance;
        }
        //-----------------------------------------------------
        void OnInstance(InstanceAble pObj)
        {
            if (pObj == null)
                return;

            if (IsDestroyed())
            {
                DestroyInstnace();
                return;
            }
            if (pObj == m_Instance)
                return;
            // 实例化特效
            m_Instance = pObj;
            m_Instance.SetActive(true);
            UpdateTransform();
            m_PlayableDirector = null;
            if (m_bSyncAxis)
            {
                m_PlayableDirector = m_Instance.GetComponent<PlayableDirector>();
                if (m_PlayableDirector == null)
                    m_PlayableDirector = m_Instance.GetComponentInChildren<PlayableDirector>();
                if(m_PlayableDirector)
                {
                    m_PlayableDirector.timeUpdateMode = DirectorUpdateMode.Manual;
                    m_PlayableDirector.time = 0;
                }
            }
        }
        //-----------------------------------------------------
        void UpdateTransform()
        {
            if (m_Instance == null)
                return;
            if (m_BindTrans != null)
            {
                m_Instance.transform.SetParent(m_BindTrans, false);
            }
            m_Instance.transform.position = Vector3.zero;
            m_Instance.transform.eulerAngles = Vector3.zero;

            m_Instance.transform.localPosition = m_BindOffset;
            m_Instance.transform.localScale = m_Scale;
            m_Instance.transform.localEulerAngles = m_BindRotate;
        }
        //-----------------------------------------------------
        void CheckBindTransform(CutsceneTrack pTrack, UnityTimnelineClip parClip)
        {
            if (string.IsNullOrEmpty(parClip.bindNode))
            {
                m_lastBindNode = null;
                m_BindTrans = null;
                if (m_Instance) m_Instance.transform.SetParent(null);
                return;
            }

            if (m_BindTrans != null && parClip.bindNode.CompareTo(m_lastBindNode) == 0)
            {
                return;
            }

            m_BindTrans = null;
            List<ICutsceneObject> vObjs = null;
            vObjs = pTrack.GetBindAllCutsceneObject(vObjs);
            if (vObjs == null || vObjs.Count == 0)
            {
                Debug.LogWarning("UnityTimnelineClip: No bind objects found for particle effect.");
                return;
            }
            foreach (var db in vObjs)
            {
                var go = db.GetUniyObject() as GameObject;
                if (go != null && !string.IsNullOrEmpty(parClip.bindNode))
                {
                    var node = CutsceneKit.Find(go.transform, parClip.bindNode);
                    if (node != null)
                        m_BindTrans = node;
                    else
                        m_BindTrans = go.transform;
                }
                else if (go != null && go.name.CompareTo(parClip.bindNode) == 0)
                {
                    m_BindTrans = go.transform;
                }
            }
            UpdateTransform();
            if (m_BindTrans != null)
                m_lastBindNode = parClip.bindNode;
        }
        //-----------------------------------------------------
        public override bool OnClipEnter(CutsceneTrack pTrack, FrameData clip)
        {
            UnityTimnelineClip parClip = clip.clip.Cast<UnityTimnelineClip>();
            if (parClip == null || string.IsNullOrEmpty(parClip.timeline))
                return false;

            m_BindRotate = parClip.rotate;
            m_BindOffset = parClip.position;
            m_Scale = parClip.scale;
            m_bSyncAxis = parClip.syncAxis;

            // 获取挂点
            CheckBindTransform(pTrack, parClip);

            SpawnInstance(parClip.timeline, OnInstance, parClip.asyncLoad);

            if (parClip.bindTrack) pTrack.BindTrackData(new ObjId(parClip.objId), this);
            return true;
        }
        //-----------------------------------------------------
        public override bool OnClipLeave(CutsceneTrack pTrack, FrameData clip)
        {
            if (clip.CanRestore() || clip.IsLeaveIn())
            {
                pTrack.RemoveObject(this);
                DestroyInstnace();
            }
            return true;
        }
        //-----------------------------------------------------
        public override bool OnFrameClip(CutsceneTrack pTrack, FrameData frameData)
        {
            UnityTimnelineClip parClip = frameData.clip.Cast<UnityTimnelineClip>();
            CheckBindTransform(pTrack, parClip);
            if (frameData.eStatus == EPlayableStatus.Start || IsEditorMode())
            {
                if (parClip.syncAxis)
                {
                    if(m_PlayableDirector)
                    {
                        m_PlayableDirector.timeUpdateMode = DirectorUpdateMode.Manual;
                        m_PlayableDirector.time = frameData.subTime;
                        m_PlayableDirector.Evaluate();
                    }
                }
#if UNITY_EDITOR
                if (frameData.eStatus != EPlayableStatus.Start && IsEditorMode())
                {
                    //! can edit
                    if (Selection.activeGameObject != m_Instance)
                    {
                        UpdateTransform();
                    }
                }
                else
#endif
                    UpdateTransform();
            }
            return true;
        }
        //-----------------------------------------------------
        private void DestroyInstnace()
        {
            if (m_Instance != null)
            {
                DespawnInstance(m_Instance);
                m_Instance = null;
            }
            m_PlayableDirector = null;
        }

        //-----------------------------------------------------
        public UnityEngine.Object GetUniyObject()
        {
            return m_Instance;
        }
        //-----------------------------------------------------
        public Transform GetUniyTransform()
        {
            if (m_Instance == null) return null;
            return m_Instance.transform;
        }
        //-----------------------------------------------------
        public Animator GetAnimator()
        {
            return null;
        }
        //-----------------------------------------------------
        public Camera GetCamera()
        {
            return null;
        }
        //-----------------------------------------------------
        public bool SetParameter(EParamType type, CutsceneParam paramData)
        {
            if (m_Instance == null)
                return false;
            switch (type)
            {
                case EParamType.ePosition: m_Instance.transform.position = paramData.ToVector3(); return true;
                case EParamType.eEulerAngle: m_Instance.transform.eulerAngles = paramData.ToVector3(); return true;
                case EParamType.eQuraternion: m_Instance.transform.rotation = paramData.ToQuaternion(); return true;
                case EParamType.eScale: m_Instance.transform.localScale = paramData.ToVector3(); return true;
            }
            return false;
        }
        //-----------------------------------------------------
        public bool GetParameter(EParamType type, ref CutsceneParam paramData)
        {
            if (m_Instance == null)
                return false;
            switch (type)
            {
                case EParamType.ePosition: paramData.SetVector3(m_Instance.transform.position); return true;
                case EParamType.eEulerAngle: paramData.SetVector3(m_Instance.transform.eulerAngles); return true;
                case EParamType.eQuraternion: paramData.SetQuaternion(m_Instance.transform.rotation); return true;
                case EParamType.eScale: paramData.SetVector3(m_Instance.transform.localScale); return true;
            }
            return false;
        }
        //-----------------------------------------------------
        public void Destroy()
        {
        }
    }
}