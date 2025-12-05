#if UNITY_EDITOR
/********************************************************************
生成日期:	25:7:2019   14:35
类    名: 	ProjectileDataLogic
作    者:	HappLI
描    述:	飞行道具编辑器
*********************************************************************/
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using Framework.Core;
using Framework.ED;
using Framework.Data;
using Framework.ActorSystem.Runtime;
using UnityEngine.UIElements;




#if USE_FIXEDMATH
using ExternEngine;
#else
using FFloat = System.Single;
using FVector3 = UnityEngine.Vector3;
using FVector2 = UnityEngine.Vector2;
using FQuaternion = UnityEngine.Quaternion;
using FMatrix4x4 = UnityEngine.Matrix4x4;
#endif

namespace Framework.ProjectileSystem.Editor
{
    public class ProjectTrack
    {
        public List<Vector3> vTracks;
        public ProjectileActor pProjectile;

        private bool m_bInited = false;
        public Vector3 backupSpeed = Vector3.zero;
        public Vector3 backupPosition = Vector3.zero;
        public Vector3 backupAcceleration = Vector3.zero;

        public Color trackColor = Color.white;
        public ProjectTrack(ProjectileActor pProj)
        {
            pProjectile = pProj;
            vTracks = new List<Vector3>();

            backupPosition = pProjectile.GetPosition();
            backupSpeed = pProjectile.GetSpeed();
            backupAcceleration = pProjectile.GetAcceleration();
        }
        public void Destroy()
        {
            if (pProjectile != null) pProjectile.Destroy();
        }
        public void TestTrack(Vector3 vStart, Vector3 vEnd)
        {
            if (pProjectile == null) return;
            if (pProjectile.GetProjectileData() == null)
                return;
            Color color = Handles.color;
            Handles.color = trackColor;

            var csvData = pProjectile.GetProjectileData();

            if (csvData.type == EProjectileType.TrackPath)
            {
                pProjectile.BuildTrackPathKeyframe(vStart, vEnd, csvData.speedLerp.x);
                pProjectile.DrawTrackPath(trackColor);
            }
            else
            {
                int nTrackTestCnt = 500;
                vTracks.Clear();
                vTracks.Add(vStart);
                pProjectile.SetPosition(backupPosition);
                pProjectile.SetSpeed(backupSpeed);
                pProjectile.SetAcceleration(backupAcceleration);
                pProjectile.SetRemainLifeTime(csvData.life_time);
                pProjectile.SetDelayTime((csvData.launch_delay + 0.0666f));
                pProjectile.SetSpeed(backupSpeed);
                pProjectile.ResetTrackStates();
                pProjectile.SetBounceTypeCount((int)csvData.speedLerp.x);
                int boundCount = 1000;
                if (csvData.type == EProjectileType.Bounce)
                    boundCount = (int)csvData.speedLerp.x;


                while (pProjectile.GetRemainLifeTime() >= 0 && vTracks.Count < 300 && nTrackTestCnt>0)
                {
                    if (vTracks.Count <= 0 || (vTracks[vTracks.Count - 1] - pProjectile.GetPosition()).sqrMagnitude > 0.1f)
                    {
                        vTracks.Add(pProjectile.GetPosition());
                        if (csvData.type == EProjectileType.Bounce)
                        {
                            if (vTracks.Count > 0 && vTracks[vTracks.Count-1].y <= 0.1f)
                                boundCount--;
                            if (boundCount <= 0)
                                break;
                        }
                    }
                    pProjectile.Update(0.0333333f);
                    nTrackTestCnt--;        
                }


                float len = 0;
                for (int i = 0; i < vTracks.Count - 1; ++i)
                {
                    Handles.color = trackColor;
                    Handles.DrawLine(vTracks[i], vTracks[i + 1]);

                    Handles.color = Color.green;
                    Vector3 toDir = vTracks[i + 1] - vTracks[i];
                    if (toDir.sqrMagnitude <= 0) toDir = Vector3.forward;
                    len += toDir.magnitude;
                    if (len >= 2)
                    {
                        Quaternion qt = Quaternion.LookRotation(toDir.normalized);
                        Handles.ArrowHandleCap(0, vTracks[i], qt, Mathf.Min(1.0f, HandleUtility.GetHandleSize(vTracks[i])), EventType.Repaint);
                        len = 0;
                    }
                }
            }
            Handles.color = color;
        }
    }

    [EditorBinder(typeof(ProjectileEditor), "m_LayerSize")]
    public class ProjectilePreview : AProjectileEditorLogic
    {
        string m_strPreviewEffect = null;
        GameObject m_pPreveObject = null;
        Actor m_pSimulateActor = null;
        Actor m_pTargetActor = null;
        GameObject m_pWaringObj = null;

        float m_PlayRuntime = 0;

        ProjectileData m_pCurrent = null;
        List<ProjectTrack> m_TestProjectile = new List<ProjectTrack>();

        TargetPreview m_Preview;
        GUIStyle m_PreviewStyle;
        //-----------------------------------------------------
        protected override void OnEnable()
        {
            m_TestProjectile = new List<ProjectTrack>();
            if (m_Preview == null) m_Preview = new TargetPreview(GetOwner());
            GameObject[] roots = new GameObject[1];
            roots[0] = new GameObject("EditorRoot");
            m_Preview.AddPreview(roots[0]);

            m_Preview.SetCamera(0.01f, 10000f, 60f);
            m_Preview.Initialize(roots);
            m_Preview.SetPreviewInstance(roots[0] as GameObject);
            m_Preview.OnDrawAfterCB = this.OnPreviewSceneDraw;
            m_Preview.bLeftMouseForbidMove = true;
            m_Preview.SetFloorTexture(ActorSystem.Editor.AssetUtil.GetFloorTexture());

            m_pSimulateActor = GetOwner<ProjectileEditor>().GetActorManager().CreateActor<Actor>(null);

            m_pTargetActor = GetOwner<ProjectileEditor>().GetActorManager().CreateActor<Actor>(null, null, 0);

            m_pTargetActor.SetDirection(-Vector3.forward);
            m_pTargetActor.SetPosition(Vector3.forward * 10);
            m_pSimulateActor.SetDirection(Vector3.forward);
            m_pSimulateActor.SetPosition(Vector3.zero);
        }
        //-----------------------------------------------------
        protected override void OnDisable()
        {
            Clear();
            if (m_pSimulateActor != null)
            {
                m_pSimulateActor.Destroy();
            }
            if (m_pTargetActor != null)
            {
                m_pTargetActor.Destroy();
            }
            if (m_Preview != null)
                m_Preview.Destroy();
            m_Preview = null;
        }
        //-----------------------------------------------------
        public void Clear()
        {
            ClearTarget();
        }
        //-----------------------------------------------------
        void ClearTarget()
        {
            GetOwner<ProjectileEditor>().GetActorManager().StopAllProjectiles();
            if (m_pWaringObj) ActorSystem.Editor.EditorUtil.Destroy(m_pWaringObj);
            m_pWaringObj = null;
            if (m_pPreveObject != null) ActorSystem.Editor.EditorUtil.Destroy(m_pPreveObject);
            m_pPreveObject = null;
        }
        //--------------------------------------------------------
        public void AddInstance(GameObject pAble,HideFlags hideFlag = HideFlags.HideAndDontSave)
        {
            if (m_Preview != null && pAble)
                m_Preview.AddPreview(pAble, hideFlag);
        }
        //-----------------------------------------------------
        public override void Reload()
        {
            RefreshTest();
        }
        //-----------------------------------------------------
        protected override void OnEvent(Event evt)
        {
            if(evt.type == EventType.KeyDown)
            {
                if(evt.keyCode == KeyCode.F5)
                {
                    RefreshTest();
                    Play(true);
                    evt.Use();
                }
            }
        }
        //-----------------------------------------------------
        public override void Play(bool bPlay)
        {
            m_PlayRuntime = 0;
            if (bPlay)
            {
                if(m_pCurrent != null)
                {
                    if (m_pSimulateActor != null) m_pSimulateActor.SetPosition(Vector3.zero);
                     Transform trackTrans = null;
                    if (m_pTargetActor != null && m_pTargetActor.GetObjectAble()!=null)
                        trackTrans = m_pTargetActor.GetUniyTransform();

                    RefreshTestProject(m_pSimulateActor, m_pCurrent, m_pTargetActor);
                    GetOwner<ProjectileEditor>().GetActorManager().LaunchProjectile(m_pCurrent, m_pSimulateActor, null, Vector3.up, Vector3.forward, m_pTargetActor, 0, 0, trackTrans);
                }
            }
        }
        //-----------------------------------------------------
        public bool isPlay()
        {
            return false;
        }
        //-----------------------------------------------------
        public override void OnChangeSelect(object pData)
        {
            if (pData == null) return;
            if(pData is ProjectileData)
            {
                OnChangeSelect((ProjectileData)pData);
            }
        }
        //-----------------------------------------------------
        public bool IsCanRefreshTest()
        {
            return m_pCurrent != null && m_pSimulateActor != null && m_pTargetActor != null;
        }
        //-----------------------------------------------------
        public void RefreshTest()
        {
            RefreshTestProject(m_pSimulateActor, m_pCurrent, m_pTargetActor);
        }
        //-----------------------------------------------------
        void RefreshTestProject(Actor pOwnerActor, ProjectileData pData, Actor targetNode)
        {
            for(int i = 0; i < m_TestProjectile.Count; ++i)
            {
                m_TestProjectile[i].Destroy();
            }

            if (pOwnerActor == null || targetNode == null)
                return;
            Transform pTrackTransform = null;
            if (targetNode != null)
                pTrackTransform = targetNode.GetUniyTransform();

            m_TestProjectile.Clear();
            Vector3 up = Vector3.up;
            Vector3 vDirection = pOwnerActor.GetDirection();
            Vector3 vPosition = pOwnerActor.GetPosition();
            Vector3 vRight = Vector3.Cross(vDirection, up);
            int damage_power = 0;
            uint track_frame_id = 0xffffffff;
            uint track_body_id = 0xffffffff;
            Transform pTrackSlot = null;
            FVector3 trackOffset = Vector3.zero;
            GetOwner<ProjectileEditor>().GetActorManager().TrackCheck(targetNode, vPosition, pData, pTrackTransform, ref pTrackSlot, ref damage_power, ref track_frame_id, ref track_body_id, ref trackOffset);

            if (pData.speedLows != null && pData.speedLows.Length > 0)
            {
                for (int i = 0; i < pData.speedLows.Length; ++i)
                {
                    ProjectileActor pProjectile = new ProjectileActor();
                    pProjectile.SetActorManager(GetOwner<ProjectileEditor>().GetActorManager());
                    pProjectile.SetContextData(pData);

                    pProjectile.Reset();
                    pProjectile.SetData(pData, pOwnerActor, targetNode, vPosition, vDirection, damage_power, track_frame_id, track_body_id);
                    pProjectile.SetSpeed(vPosition, vDirection, i);
                    pProjectile.SetSpeed(vDirection * pData.speedLows[i].z + up * pData.speedLows[i].y + vRight * pData.speedLows[i].x);
                    pProjectile.SetTrack(pTrackSlot, trackOffset);
                    pProjectile.SetDelayTime(pData.launch_delay + 0.0666f);


                    pProjectile.SetVisible(true);
                    pProjectile.SetActived(true);
                    pProjectile.EnableLogic(true);
                    pProjectile.SetSpatial(true);
                    pProjectile.SetCollectAble(false);

                    ProjectTrack trackProj = new ProjectTrack(pProjectile);
                    trackProj.trackColor = Color.red;
                    m_TestProjectile.Add(trackProj);
                }
                for (int i = 0; i < pData.speedUppers.Length; ++i)
                {
                    ProjectileActor pProjectile = new ProjectileActor();
                    pProjectile.SetActorManager(GetOwner<ProjectileEditor>().GetActorManager());
                    pProjectile.SetContextData(pData);

                    pProjectile.Reset();
                    pProjectile.SetData(pData, pOwnerActor, targetNode, vPosition, vDirection, damage_power, track_frame_id, track_body_id);
                    pProjectile.SetSpeed(vPosition, vDirection, i);
                    pProjectile.SetSpeed(vDirection * pData.speedUppers[i].z + up * pData.speedUppers[i].y + vRight * pData.speedUppers[i].x);
                    pProjectile.SetTrack(pTrackSlot, trackOffset);
                    pProjectile.SetDelayTime(pData.launch_delay + 0.0666f);


                    pProjectile.SetVisible(true);
                    pProjectile.SetActived(true);
                    pProjectile.EnableLogic(true);
                    pProjectile.SetSpatial(true);
                    pProjectile.SetCollectAble(false);

                    ProjectTrack trackProj = new ProjectTrack(pProjectile);
                    trackProj.trackColor = Color.yellow;
                    m_TestProjectile.Add(trackProj);
                }
            }
            else if (pData.life_time > 0)
            {
                ProjectileActor pProjectile = new ProjectileActor();
                pProjectile.SetActorManager(GetOwner<ProjectileEditor>().GetActorManager());
                pProjectile.SetContextData(pData);

                pProjectile.Reset();

                pProjectile.SetData(pData, pOwnerActor, targetNode, vPosition, vDirection, damage_power, track_frame_id, track_body_id);
                pProjectile.SetDelayTime(pData.launch_delay + 0.0666f);
                pProjectile.SetTrack(pTrackSlot, trackOffset);


                pProjectile.SetVisible(true);
                pProjectile.SetActived(true);
                pProjectile.EnableLogic(true);
                pProjectile.SetSpatial(true);
                pProjectile.SetCollectAble(false);
                //! call back
                m_TestProjectile.Add(new ProjectTrack(pProjectile));
            }
        }
        //-----------------------------------------------------
        void OnChangeSelect(ProjectileData  item)
        {
            if (item == m_pCurrent)
                return;
            ClearTarget();
            m_pCurrent = item;

            if (m_pCurrent == null) return;
            //creat test projectile
            RefreshTestProject(m_pSimulateActor, item, m_pTargetActor);

            GameObject pObj = AssetDatabase.LoadAssetAtPath<GameObject>(m_pCurrent.effect);
            if(pObj!=null)
            {
                m_strPreviewEffect = m_pCurrent.effect;
                m_pPreveObject = GameObject.Instantiate<GameObject>(pObj);
                ActorSystemUtil.ResetGameObject(m_pPreveObject, EResetType.All);
                AddInstance(m_pPreveObject, HideFlags.HideAndDontSave);
            }
            pObj = AssetDatabase.LoadAssetAtPath<GameObject>(m_pCurrent.waring_effect);
            if (pObj)
            {
                m_pWaringObj = GameObject.Instantiate<GameObject>(pObj);
                ActorSystemUtil.ResetGameObject(m_pWaringObj, EResetType.All);
                AddInstance(m_pWaringObj, HideFlags.HideAndDontSave);
            }
        }
        //-----------------------------------------------------
        protected override void OnUpdate(float fFrameTime)
        {
            if (fFrameTime >= 0.03333) fFrameTime = 0.03333f;
            m_PlayRuntime += fFrameTime;
           // UpdateParticle();
            if (m_pCurrent != null)
            {
                if (m_strPreviewEffect == null || m_strPreviewEffect.CompareTo(m_pCurrent.effect) != 0)
                {
                    m_strPreviewEffect = m_pCurrent.effect;
                    if (m_pPreveObject) Framework.ED.EditorUtils.Destroy(m_pPreveObject);
                    if (!string.IsNullOrEmpty(m_pCurrent.effect))
                    {
                        GameObject pObj = AssetDatabase.LoadAssetAtPath<GameObject>(m_pCurrent.effect);
                        if (pObj != null)
                        {
                            m_pPreveObject = GameObject.Instantiate<GameObject>(pObj);
                            ActorSystemUtil.ResetGameObject(m_pPreveObject, EResetType.All);
                            AddInstance(m_pPreveObject);
                        }
                    }    
                }
                //             if (preWaringEffectPrefab != null && preWaringEffectPrefab.CompareTo(projectile.waring_effect) != 0)
                //             {
                //                 if (m_pWaringObj) GameObject.DestroyImmediate(m_pWaringObj);
                //                 GameObject pObj = AssetDatabase.LoadAssetAtPath<GameObject>(m_pCurItem.waring_effect);
                //                 if (pObj != null)
                //                 {
                //                     m_pWaringObj = GameObject.Instantiate<GameObject>(pObj);
                //                     Base.Util.ResetGameObject(m_pWaringObj, Base.EResetType.All);
                //                 }
                //             }
            }
        }
        //-----------------------------------------------------
        void UpdateParticle()
        {
            if (Application.isPlaying) return;
            ParticleSystem[] systems = GameObject.FindObjectsOfType<ParticleSystem>();
            if (systems == null) return;
            for (int i = 0; i < systems.Length; ++i)
            {
                systems[i].Play();
                systems[i].Simulate(m_PlayRuntime);
            }
        }
        //-----------------------------------------------------
        public void Realod()
        {
            RefreshList();
        }
        //-----------------------------------------------------
        void RefreshList(bool bRealod = true)
        {
        }
        //-----------------------------------------------------
        protected override void OnGUI()
        {
            var logic = GetLogic<ProjectileDataListLogic>();
            bool isDataListActive = false;
            if (logic != null) isDataListActive = logic.IsActive();
            EditorGUI.BeginDisabledGroup(isDataListActive);
            DrawPreview(GetRect());
            EditorGUI.EndDisabledGroup();
        }
        //-----------------------------------------------------
        public void DrawPreview(Rect rc)
        {
            if (m_Preview != null && rc.width > 0 && rc.height > 0)
            {
                if (m_PreviewStyle == null)
                    m_PreviewStyle = new GUIStyle(EditorStyles.textField);
                m_Preview.OnPreviewGUI(rc, m_PreviewStyle);
            }
        }
        //-----------------------------------------------------
        public void OnSceneGUI(Event evt)
        {
#if !UNITY_5_1
            UnityEngine.Rendering.CompareFunction zTest = Handles.zTest;
            Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
#endif
            var projectiles = GetOwner<ProjectileEditor>().GetActorManager().GetRunningProjectile();
            if(projectiles!=null)
            {
                foreach (var db in projectiles)
                {
                    var csvData = db.Value.GetProjectileData();
                    if (csvData == null)
                        continue;
                    Vector3 vSpeedDirection = db.Value.GetSpeed();
                    if (vSpeedDirection.sqrMagnitude > 0.01f)
                        vSpeedDirection.Normalize();
                    else
                        vSpeedDirection = db.Value.GetDirection();
                    if (m_pCurrent.collisionType == EProjectileCollisionType.BOX)
                        RenderVolumeByColor(ref csvData.aabb_min, ref csvData.aabb_max, db.Value.GetPosition(), Quaternion.LookRotation(vSpeedDirection), Color.red, 1.0f, false);
                    else if (m_pCurrent.collisionType == EProjectileCollisionType.CAPSULE)
                        RenderSphereByColor(ref m_pCurrent.aabb_min.x, db.Value.GetPosition(), Quaternion.LookRotation(vSpeedDirection), "#ff0000ff", 1.0f, false);
                }
            }


            if (m_pCurrent != null)
            {
                Vector3 position = Vector3.zero;
                Quaternion rotation = Quaternion.identity;
                if(m_pPreveObject != null)
                {
                    position = m_pPreveObject.transform.position;
                    rotation = m_pPreveObject.transform.rotation;
                }
                if (m_pCurrent.collisionType == EProjectileCollisionType.BOX)
                    RenderVolumeByColor(ref m_pCurrent.aabb_min, ref m_pCurrent.aabb_max, position, rotation,Color.red, 1.0f, evt.shift);
                else if (m_pCurrent.collisionType == EProjectileCollisionType.CAPSULE)
                    RenderSphereByColor(ref m_pCurrent.aabb_min.x, position, rotation, "#ff0000ff", 1.0f, evt.shift);

                m_pSimulateActor.SetPosition(Handles.DoPositionHandle(m_pSimulateActor.GetPosition(), Quaternion.identity));
                m_pTargetActor.SetPosition(Handles.DoPositionHandle(m_pTargetActor.GetPosition(), Quaternion.identity));
                for(int i = 0; i < m_TestProjectile.Count; ++i)
                {
                    m_TestProjectile[i].TestTrack(m_pSimulateActor.GetPosition(), m_pTargetActor.GetPosition());
                }

                if (m_pCurrent.explode_range > 0)
                {
                    ActorSystemUtil.DrawBoundingBox(Vector3.zero, Vector3.one * m_pCurrent.explode_range, Matrix4x4.identity, Color.cyan, false);
                }
               // TestDrawLineTrace(m_pCurItem, m_pSimulateActor.GetPosition(), m_pSimulateActor.GetDirection(), 1/30f);

                if(m_pCurrent.type == EProjectileType.TrackPath)
                {
                    if(m_pCurrent.speedMaxs!=null && m_pCurrent.speedMaxs.Length>0 && m_pCurrent.accelerations.Length>0)
                    {
                        EProjectileParabolicType parabolicType = (EProjectileParabolicType)m_pCurrent.accelerations[0].y;
                        if (parabolicType == EProjectileParabolicType.StartEnd)
                        {
                            if (m_pCurrent.speedMaxs.Length == 2)
                            {
                                Handles.color = Color.green;
                                Vector3 leftCenter = m_pCurrent.speedMaxs[0] + m_pSimulateActor.GetPosition();
                                m_pCurrent.speedUppers[0] = Handles.DoPositionHandle(m_pCurrent.speedUppers[0] + leftCenter, Quaternion.identity) - leftCenter;
                                Handles.DrawLine(leftCenter, m_pCurrent.speedUppers[0] + leftCenter);
                                if (m_pCurrent.speedUppers[0].sqrMagnitude > 0)
                                    Handles.ArrowHandleCap(0, m_pCurrent.speedUppers[0] + leftCenter, Quaternion.LookRotation(m_pCurrent.speedUppers[0]), 0.1f, EventType.Repaint);

                                Handles.color = Color.red;
                                Vector3 rightCenter = m_pCurrent.speedMaxs[1] + m_pTargetActor.GetPosition();
                                m_pCurrent.speedLows[1] = Handles.DoPositionHandle(m_pCurrent.speedLows[1] + rightCenter, Quaternion.identity) - rightCenter;
                                Handles.DrawLine(rightCenter, m_pCurrent.speedLows[1] + rightCenter);
                                if (m_pCurrent.speedLows[1].sqrMagnitude > 0)
                                    Handles.ArrowHandleCap(0, m_pCurrent.speedLows[1] + rightCenter, Quaternion.LookRotation(m_pCurrent.speedLows[1]), 0.1f, EventType.Repaint);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < m_pCurrent.speedMaxs.Length; ++i)
                            {
                                Vector3 center = m_pCurrent.speedMaxs[i] + position;
                                float handleSize = Mathf.Min(1, HandleUtility.GetHandleSize(m_pCurrent.speedMaxs[i] + position));
                                Handles.SphereHandleCap(0, center, Quaternion.identity, handleSize, EventType.Repaint);
                                if (Event.current.shift)
                                {
                                    if(i >0)
                                    {
                                        Handles.color = Color.green;
                                        m_pCurrent.speedLows[i] = Handles.DoPositionHandle(m_pCurrent.speedLows[i] + center, Quaternion.identity) - center;
                                        Handles.DrawLine(center, m_pCurrent.speedLows[i] + center);
                                        if (m_pCurrent.speedLows[i].sqrMagnitude > 0)
                                            Handles.ArrowHandleCap(0, m_pCurrent.speedLows[i] + center, Quaternion.LookRotation(m_pCurrent.speedLows[i]), 0.1f, EventType.Repaint);
                                    }

                                    if (i < m_pCurrent.speedMaxs.Length-1)
                                    {
                                        Handles.color = Color.red;
                                        m_pCurrent.speedUppers[i] = Handles.DoPositionHandle(m_pCurrent.speedUppers[i] + center, Quaternion.identity) - center;
                                        Handles.DrawLine(center, m_pCurrent.speedUppers[i] + center);
                                        if (m_pCurrent.speedUppers[i].sqrMagnitude > 0)
                                            Handles.ArrowHandleCap(0, m_pCurrent.speedUppers[i] + center, Quaternion.LookRotation(m_pCurrent.speedUppers[i]), 0.1f, EventType.Repaint);
                                    }
                                }
                                else
                                {
                                    m_pCurrent.speedMaxs[i] = Handles.PositionHandle(center, Quaternion.identity) - position;
                                }
                            }
                            if (!Event.current.shift)
                            {
                                for (int i = 0; i < m_pCurrent.speedMaxs.Length; ++i)
                                {
                                    Vector3 point = m_pCurrent.speedMaxs[i] + position;
                                    Vector2 position2 = HandleUtility.WorldToGUIPoint(point);
                                    {
                                        GUILayout.BeginArea(new Rect(position2.x, position2.y, 100, 25));
                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("point[" + i + "]");
                                        if (GUILayout.Button("移除"))
                                        {
                                            m_pCurrent.RemoveTrackPoint(i);
                                            RefreshTest();
                                            break;
                                        }
                                        GUILayout.EndHorizontal();
                                        GUILayout.EndArea();
                                    }
                                    if (m_pCurrent.speedMaxs.Length > 1)
                                    {
                                        Vector3 insert = (m_pCurrent.speedMaxs[(i + 1) % m_pCurrent.speedMaxs.Length] + m_pCurrent.speedMaxs[i]) / 2 + position;
                                        {
                                            Vector2 insertgui = HandleUtility.WorldToGUIPoint(insert);
                                            GUILayout.BeginArea(new Rect(insertgui.x, insertgui.y, 40, 25));
                                            if (GUILayout.Button("插入"))
                                            {
                                                m_pCurrent.InsertTrackPoint((i + 1) % m_pCurrent.speedMaxs.Length, insert - position, Vector3.zero, Vector3.zero);
                                                RefreshTest();
                                                break;
                                            }
                                            GUILayout.EndArea();
                                        }
                                    }
                                }
                            }
                        }
                        
                    }
                }
            }
#if !UNITY_5_1
            Handles.zTest = zTest;
#endif
        }
        //------------------------------------------------------
        void RenderVolumeByColor(ref Vector3 minVolume, ref Vector3 maxVolume, Vector3 position, Quaternion rotation, Color dwColor, float fScale, bool bEditor = false)
        {
                Vector3 vCenter;
                Vector3 vHalf;
                vCenter = (minVolume + maxVolume) * 0.5f;
                vHalf = maxVolume - vCenter;

            ActorSystemUtil.DrawBoundingBox(vCenter, vHalf, Matrix4x4.TRS(position, rotation, Vector3.one), dwColor, false);
            if (bEditor)
            {
                minVolume = Handles.DoPositionHandle(minVolume + position, rotation) - position;
                maxVolume = Handles.DoPositionHandle(maxVolume + position, rotation) - position;
                minVolume = Vector3.Min(minVolume, maxVolume);
                maxVolume = Vector3.Max(minVolume, maxVolume);
            }
        }
        //--------------------------------------------------------
        void OnPreviewSceneDraw(int controllerId, Camera camera, Event evt)
        {
            OnSceneGUI(evt);
        }
        //------------------------------------------------------
        void RenderSphereByColor(ref float fRadius, Vector3 position, Quaternion rotation, string strColor, float fScale, bool bEditor = false)
        {
            Color dwColor;
#if !UNITY_5_1
            if (!ColorUtility.TryParseHtmlString(strColor, out dwColor))
                return;
#else
            dwColor = Color.white;
#endif
            Color color = Handles.color;
            Handles.color = dwColor;

#if !UNITY_5_1
            //Handles.SphereHandleCap(0, position, rotation, fRadius, EventType.Repaint);
            Handles.DrawWireDisc(position, rotation * Vector3.up, fRadius);
            Handles.DrawWireDisc(position, rotation * Vector3.right, fRadius);
            Handles.DrawWireDisc(position, rotation * Vector3.forward, fRadius);
#else
             //   Handles.CubeCap()
#endif

            if (bEditor)
            {
                fRadius = Handles.ScaleSlider(fRadius, position, rotation*Vector3.forward, rotation, HandleUtility.GetHandleSize(position), 0.1f);
            }

            Handles.color = color;
        }
    }
}
#endif