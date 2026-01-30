#if USE_CUTSCENE
/********************************************************************
生成日期:	06:30:2025
类    名: 	ActorProjectileClip
作    者:	HappLI
描    述:	Actor弹道剪辑
*********************************************************************/
using UnityEngine;
using System.Collections.Generic;
using Framework.DrawProps;
using Framework.Cutscene.Runtime;
using Framework.Core;
using System;

#if UNITY_EDITOR
using Framework.ProjectileSystem.Editor;
using Framework.ActorSystem.Editor;
using Framework.ED;
using System.Reflection;
using UnityEditor;
using Framework.Cutscene.Editor;
#endif
namespace Framework.ActorSystem.Runtime
{
    [System.Serializable, CutsceneClip("Actor/弹道Clip", typeof(Actor))]
    public class ActorProjectileClip : IBaseClip
    {
        [Display("基本属性")] public BaseClipProp baseProp;

        [BindSlot("OnCollectBindSlot"), Display("触发绑点")]
        public string                               bindSlot = "";

        [Display("偏移")]
        public Vector3                              offset = Vector3.zero;

        [Display("角度偏移")]
        public Vector3                              localRotate = Vector3.zero;

        [Display("弹道库"),RowFieldInspector("OnDrawProjectileLibrary")]
        public uint                                 projectileLibrary = 0;
        [StateByField("projectileLibrary","0",true)]
        public ActorSystem.Runtime.ProjectileData   projecitleData = null;
        //-----------------------------------------------------
        public ACutsceneDriver CreateDriver()
        {
            return new ProjecitleDriver();
        }
        //-----------------------------------------------------
        public ushort GetIdType()
        {
            return (ushort)EActorCutsceneClipType.eActorProjectileClip;
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
        public void OnSceneView(SceneView sceneView)
        {
            if (baseProp.ownerTrackObject == null)
                return;
        }
        //-----------------------------------------------------
        internal void OnCollectBindSlot()
        {
            if (baseProp.ownerTrackObject == null)
                return;
            var bindObj = baseProp.ownerTrackObject.GetBindLastCutsceneObject();
            if (bindObj == null)
                return;
            Actor pActor = bindObj as Actor;
            if (pActor == null)
                return;

            var actorAble = pActor.GetObjectAble();
            AActorComponent actorComp = actorAble.CastContextData<AActorComponent>();
            if (actorComp == null)
                return;

            InspectorDrawUtil.BindSlots.Clear();
            if (actorComp.slots!=null)
            {
                foreach(var db in actorComp.slots)
                {
                    InspectorDrawUtil.BindSlots.Add(db.name);
                }
            }
        }
        //-----------------------------------------------------
        [System.NonSerialized]bool m_bExpandEditor = false;
        [AddInspector]
        internal void OnInspector()
        {
            if (this.projectileLibrary != 0)
            {
                if (GUILayout.Button("设置弹道原本时长"))
                {
                    var projData = ProjecitleProvider.GetProjectileData(projectileLibrary);
                    if (projData != null)
                    {
                        this.baseProp.duration = Mathf.Min(50, projData.life_time);
                    }
                }
            }
            float width = GUILayoutUtility.GetLastRect().width;
            GUILayout.BeginHorizontal();
            m_bExpandEditor = EditorGUILayout.Foldout(m_bExpandEditor, "编辑");
            if (GUILayout.Button("编辑器"))
            {
                m_bExpandEditor = false;
                if (this.projectileLibrary != 0)
                    ProjectileEditor.EditorProjectile(this.projectileLibrary);
                else
                {
                    ProjectileEditor.EditorProjectile(this.projecitleData);
                }
            }
            GUILayout.EndHorizontal();
            if (m_bExpandEditor)
            {
                if (this.projectileLibrary != 0)
                {
                    var projData = ProjecitleProvider.GetProjectileData(projectileLibrary);
                    if(projData!=null) 
                        Framework.ProjectileSystem.Editor.ProjectileDataDrawer.OnInsepctor(projData, new Vector2(width, 0));
                    else
                        GUILayout.Label("弹道库中不存在该弹道[" + projectileLibrary + "]");
                }
                else
                {
                    Framework.ProjectileSystem.Editor.ProjectileDataDrawer.OnInsepctor(this.projecitleData, new Vector2(width, 0));
                }
            }
        }
        //-----------------------------------------------------
        internal void OnDrawProjectileLibrary(System.Object pOwner, System.Reflection.FieldInfo fieldInfo)
        {
            if(GUILayout.Button("..."))
            {
                ActorSystem.Editor.ProjecitleProvider.Show((proj) =>
                {
                    this.projectileLibrary = proj.id;
                    this.baseProp.duration = Mathf.Min(50, proj.life_time);
                });
            }
        }
#endif
    }
    //-----------------------------------------------------
    //ProjecitleDriver
    //-----------------------------------------------------
    public class ProjecitleDriver : ACutsceneDriver
    {
        //-----------------------------------------------------
        public override void OnDestroy()
        {
        }
        //-----------------------------------------------------
        public override bool OnClipEnter(CutsceneTrack pTrack, FrameData frameData)
        {
            ActorProjectileClip projectileClip = frameData.clip.Cast<ActorProjectileClip>();
            var pBinder = pTrack.GetBindLastCutsceneObject();
            if (pBinder == null)
                return true;
            if (!(pBinder is Actor))
            {
                return true;
            }
            Actor pOwner = pBinder as Actor;
            Actor pTarget = null;

            ProjectileData projectileData = null;
            if (projectileClip.projectileLibrary != 0)
            {
                projectileData = pOwner.GetActorManager().GetProjectileManager().GetProjectileData(projectileClip.projectileLibrary);
            }
            else projectileData = projectileClip.projecitleData;
            if (projectileData == null)
            {
                UnityEngine.Debug.LogWarning("projectile is null!!!");
                return true;
            }

            Vector3 vEventPos = pOwner.GetPosition() + projectileClip.offset;
            if (!string.IsNullOrEmpty(projectileClip.bindSlot) && pOwner != null)
            {
                var bindSlot = pOwner.GetEventBindSlot(projectileClip.bindSlot);
                if (bindSlot != null)
                {
                    vEventPos = bindSlot.GetPosition();
                    if (projectileClip.offset.sqrMagnitude > 0)
                    {
                        var rotation = bindSlot.rotation;
                        vEventPos += (rotation*Vector3.forward) * projectileClip.offset.z;
                        vEventPos += (rotation * Vector3.right) * projectileClip.offset.x;
                        vEventPos += (rotation * Vector3.up) * projectileClip.offset.y;
                    }
                }
            }

            List<Actor> vLockTargets = null;
            var skillSystem = pOwner.GetSkillSystem();
            if (skillSystem != null)
            {
                vLockTargets = skillSystem.GetLockTargets();
            }
            AActorStateInfo stateParam = pOwner.GetStateParam();

            AActorStateInfo targetStateParam = null;
            if (pTarget!=null)
            {
                targetStateParam = pTarget.GetStateParam();
            }

            float fDelayGap = 0;
            var cacheList = pOwner.GetActorManager().GetCatchActorList();
            if (vLockTargets == null)
                pOwner.GetActorManager().LaunchProjectile(projectileData, pOwner, stateParam, vEventPos, pOwner.GetDirection(), pTarget, 0, 0, null, cacheList);
            else
            {
                foreach(var db in vLockTargets)
                    pOwner.GetActorManager().LaunchProjectile(projectileData, pOwner, stateParam, vEventPos, pOwner.GetDirection(), db, 0, 0, null, cacheList);
            }
            for (int p = 0; p < cacheList.Count; ++p)
            {
                ProjectileActor pProj = (ProjectileActor)cacheList[p];
                pProj.SetLifeTime(projectileClip.GetDuration());
                pProj.SetRemainLifeTime(projectileClip.GetDuration()-0.1f);
                pProj.SetOffsetEulerAngle(projectileClip.localRotate);
                pProj.SetBindOwnerSlot(projectileClip.bindSlot);
                pProj.SetStartBindOffset(projectileClip.offset);
                if (fDelayGap > 0)
                {
                    pProj.SetVisible(false);
                }
            }
            return true;
        }
    }
}
#endif