/********************************************************************
生成日期:	5:11:2020  20:36
类    名: 	ActorGraphicAgent
作    者:	HappLI
描    述:	动作表现类
*********************************************************************/
using Framework.Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Framework.ActorSystem.Runtime
{
    public class ActorGraphicAgent : AActorAgent
    {
        PlayableGraph                                       m_pPlayableGraph;
        GraphAnimationPlayer                                m_Player;
  //      Plugin.CGpuSkinMeshAgent                          m_pBakerSkin = null;
        int                                                 m_nPlayableId = 0;
        ActorGraphData                                      m_GraphData = null;
        Dictionary<uint, ActorAction>                       m_vActions = null;    //actionTag
        Dictionary<string, ActorAction>                     m_vActionByNames = null;
        System.Collections.Generic.List<ActorCommonAction>  m_vCommonActions = null;
        //--------------------------------------------------------
        protected override void OnLoadedAble(ActorContext component)
        {
       //     m_pBakerSkin = null;
            m_GraphData = null;
            m_vCommonActions = null;
            if (!component.IsValid())
                return;
            if(component.pContextData!=null)
            {
                if (component.pContextData is AActorComponent)
                    LoadActorCompent(component.pContextData as AActorComponent);
            }
            //    else if (component is Plugin.CGpuSkinMeshAgent)
            //       LoadBakeSkinCompent(component as Plugin.CGpuSkinMeshAgent);
        }
        //--------------------------------------------------------
        void LoadActorCompent(AActorComponent component)
        {
            var animator = GetComponent<Animator>(true);
            if (animator != null)
            {
                if (!m_pPlayableGraph.IsValid())
                    m_pPlayableGraph = UnityEngine.Playables.PlayableGraph.Create();
                m_pPlayableGraph.SetTimeUpdateMode(DirectorUpdateMode.Manual);
                var playable = UnityEngine.Playables.ScriptPlayable<GraphAnimationPlayer>.Create(m_pPlayableGraph, 1);
                m_Player = playable.GetBehaviour();
                m_nPlayableId = component.gameObject.GetInstanceID();

                UnityEngine.Playables.AnimationPlayableUtilities.Play(animator, m_Player.playable, m_pPlayableGraph);
                m_Player.SetMixLayer(1);
                m_Player.SetClipNum(4);
#if UNITY_EDITOR
                GraphPlayableUtil.CollectPlayable(component.gameObject, m_Player);
#endif
                //    m_pPlayableGraph.Play();

                var actorComponent = component as AActorComponent;
                if (actorComponent != null)
                {
                    m_vCommonActions = actorComponent.commonActions;
                    if (actorComponent.commonActions != null)
                    {
                        for (int i = 0; i < actorComponent.commonActions.Count; ++i)
                        {
                            var common = actorComponent.commonActions[i];
                            if (m_vActions == null) m_vActions = new Dictionary<uint, ActorAction>(64);
                            if (m_vActionByNames == null) m_vActionByNames = new Dictionary<string, ActorAction>(64);
                            m_vActions[common.GetActionKey()] = common;
                            m_vActionByNames[common.actioName] = common;
                            if (common.type == EActionStateType.Idle)
                            {
                                m_Player.SetDefaultClip(common.clip, common.layer, common, common.actioName);
                            }
                        }
                    }
                }
                m_pPlayableGraph.SetTimeUpdateMode(DirectorUpdateMode.Manual);
            }
        }
        //--------------------------------------------------------
        internal void OnLoadActorGraphData(ActorGraphData pGraphData)
        {
            if (m_GraphData == pGraphData)
                return;
            if(m_GraphData!=null && m_vActions!=null)
            {
                foreach(var db in m_GraphData.timelineActions)
                {
                    m_vActions.Remove(db.GetActionKey());
                    m_vActionByNames.Remove(db.actioName);
                }
            }
            m_GraphData = pGraphData;
            if (m_GraphData != null)
            {
                if (m_vActions == null) m_vActions = new Dictionary<uint, ActorAction>(m_GraphData.timelineActions.Count);
                if (m_vActionByNames == null) m_vActionByNames = new Dictionary<string, ActorAction>(m_GraphData.timelineActions.Count);
                foreach (var db in m_GraphData.timelineActions)
                {
                    m_vActions[db.GetActionKey()] = db;
                    m_vActionByNames[db.actioName] = db;
                }
            }
        }
        //--------------------------------------------------------
       // void LoadBakeSkinCompent(Plugin.CGpuSkinMeshAgent pAgent)
       // {
      //      m_pBakerSkin = pAgent;
      //  }
        //--------------------------------------------------------
        public PlayableGraph GetPlayableGraph()
        {
            return m_pPlayableGraph;
        }
        //--------------------------------------------------------
        public GraphAnimationPlayer GetGraphAnimationPlayer()
        {
            return m_Player;
        }
        //--------------------------------------------------------
        public void SetDefaultClip(AnimationClip clip)
        {
            if (m_Player != null)
                m_Player.SetDefaultClip(clip);
        }
        //--------------------------------------------------------
        public ActorAction GetActorAction(string actionName)
        {
            if (m_vActionByNames == null)
                return null;
            if (m_vActionByNames.TryGetValue(actionName, out var action))
                return action;
            return null;
        }
        //--------------------------------------------------------
        public ActorAction GetActorAction(EActionStateType eState, uint nTag)
        {
            return GetActorAction((uint)((int)eState << 16 | (int)nTag));
        }
        //--------------------------------------------------------
        public ActorAction GetActorAction(uint actionKey)
        {
            if (m_vActions == null)
                return null;
            if (m_vActions.TryGetValue(actionKey, out var action))
                return action;
            return null;
        }
        //--------------------------------------------------------
        public void PlayAnimation(string actioName, float blendTime = 0.1f, bool bForce = false)
        {
            var clip = GetActorAction(actioName);
            PlayAnimation(clip, blendTime,bForce);
        }
        //--------------------------------------------------------
        public void PlayAnimation(ActorAction pAction, float blendTime = 0.1f, bool bForce = false)
        {
            if (pAction == null)
                return;

          //  if (m_pBakerSkin != null)
          //  {
          //      m_pBakerSkin.Play(pAction.type, pAction.actionTag, bForce);
          //      return;
          //  }
            if (m_Player == null) return;
            if (!m_Player.IsStateExist(pAction))
            {
                if (pAction is ActorCommonAction)
                {
                    ActorCommonAction commonClip = pAction as ActorCommonAction;
                    m_Player.AddMotion(commonClip, commonClip.clip, commonClip.layer, commonClip.actioName);
                }
                else if (pAction is ActorTimelineAction)
                {
                    ActorTimelineAction commonClip = pAction as ActorTimelineAction;
                    //  m_Player.AddMotion(commonClip, commonClip.clip, commonClip.layer, commonClip.actioName);
                }
            }
            m_Player.CrossFade(pAction, 1.0f, blendTime, bForce);
        }
        //--------------------------------------------------------
        public void PlayAnimation(IUserData pOwner, AnimationClip animationClip, int layer, float blendTime = 0.1f, bool bForce = false)
        {
            if (animationClip == null || pOwner == null)
                return;
           // if (m_pBakerSkin != null)
           // {
           //     return;
           // }
            if (m_Player == null) return;
            if (!m_Player.IsStateExist(pOwner, animationClip))
            {
                m_Player.AddMotion(pOwner, animationClip, (uint)layer, animationClip.name);
            }
            m_Player.CrossFade(pOwner, 1.0f, blendTime, bForce);
        }
        //--------------------------------------------------------
        public bool IsPlaying(string actionName)
        {
            if (string.IsNullOrEmpty(actionName))
                return false;
            //if (m_pBakerSkin != null)
            //{
            //    return actionName.CompareTo(m_pBakerSkin.GetCurPlayName())==0;
            //}
            if (m_Player != null) return m_Player.IsPlaying(actionName);
            return false;
        }
        //--------------------------------------------------------
        public bool IsPlaying(IUserData pOwner)
        {
            if (pOwner == null)
                return false;
           // if (m_pBakerSkin != null)
           // {
           //     return false;
           // }
            if (m_Player != null) return m_Player.IsPlaying(pOwner);
            return false;
        }
        //--------------------------------------------------------
        public bool IsPlaying(AnimationClip pClip)
        {
            if (pClip == null)
                return false;
           // if (m_pBakerSkin != null)
            //{
           //     return false;
           // }
            if (m_Player != null) return m_Player.IsPlaying(pClip);
            return false;
        }
        //--------------------------------------------------------
        public void StopAnimation(string actioName,float stopLerp = 0.1f)
        {
           // if (m_pBakerSkin != null)
           // {
           //     m_pBakerSkin.Stop(actioName);
           //     return;
           // }
            if (m_Player != null)
                m_Player.Stop(actioName, stopLerp);
        }
        //--------------------------------------------------------
        public void StopAnimation(IUserData pUserData, float stopLerp = 0.1f)
        {
          //  if (m_pBakerSkin != null)
         //   {
          //      return;
         //   }
            if (m_Player != null)
                m_Player.Stop(pUserData, stopLerp);
        }
        //--------------------------------------------------------
        public void SetActionSpeed(float speed)
        {
           // if (m_pBakerSkin != null)
          //  {
          //      m_pBakerSkin.SetSpeed(speed);
          //      return;
          //  }
            if (m_Player != null)
                m_Player.SetSpeed(speed);
        }
        //--------------------------------------------------------
        public void SetActionSpeed(string stateName, float speed)
        {
          //  if (m_pBakerSkin != null)
         //   {
         //       m_pBakerSkin.SetSpeed(speed);
         //       return;
         //   }
            if (m_Player != null)
                m_Player.SetSpeed(stateName,speed);
        }
        //--------------------------------------------------------
        public void SetActionSpeed(IUserData pOwner, float speed)
        {
          //  if (m_pBakerSkin != null)
          //  {
          //      m_pBakerSkin.SetSpeed(speed);
          //      return;
          //  }
            if (m_Player != null)
                m_Player.SetSpeed(pOwner, speed);
        }
        //--------------------------------------------------------
        public void SetActionTime(float time, bool bOverDoned = true)
        {
          //  if (m_pBakerSkin != null)
           // {
           //     m_pBakerSkin.SetFixedTime(time);
           //     return;
           // }
            if (m_Player != null)
                m_Player.SetTime(time, bOverDoned);
        }
        //--------------------------------------------------------
        public void SetActionTime(string stateName, float time, bool bOverDoned = true)
        {
         //   if (m_pBakerSkin != null)
         //   {
          //      m_pBakerSkin.SetFixedTime(time);
          //      return;
          //  }
            if (m_Player != null)
                m_Player.SetTime(stateName,time, bOverDoned);
        }
        //--------------------------------------------------------
        public void SetActionTime(IUserData pOwner, float time, bool bOverDoned = true)
        {
            //if (m_pBakerSkin != null)
           // {
            //    m_pBakerSkin.SetFixedTime(time);
           //     return;
           // }
            if (m_Player != null)
                m_Player.SetTime(pOwner,time, bOverDoned);
        }
        //--------------------------------------------------------
        public void SetIdleType(EActionStateType eType, uint tag = 0, bool bForce = false)
        {
           // if (m_pBakerSkin != null)
           // {
           //     m_pBakerSkin.SetIdleType(eType, tag);
           //     return;
           // }
            if (m_GraphData == null)
                return;
            if (null == m_Player)
                return;
            var clip = GetActorAction(eType, tag);
            if (clip == null)
                return;
            if (!m_Player.IsStateExist(clip))
            {
                if (clip is ActorCommonAction)
                {
                    ActorCommonAction commonClip = clip as ActorCommonAction;
                    m_Player.SetDefaultClip(commonClip.clip, commonClip.layer, commonClip, commonClip.actioName);
                }
            }
        }
        //--------------------------------------------------------
        public void PlayAnimation(EActionStateType eType, uint tag =0, bool bForce = false)
        {
            var clip = GetActorAction(eType, tag);
            PlayAnimation(clip, 0.1f, bForce);
        }
        //--------------------------------------------------------
        public void StopAnimation(EActionStateType eType, uint tag = 0)
        {
         //   if (m_pBakerSkin != null)
         //   {
         //       m_pBakerSkin.Stop(eType, tag);
         //       return;
         //   }
            if (null == m_Player)
                return;
            if (m_GraphData == null)
                return;
            var clip = GetActorAction(eType, tag);
            if (clip == null)
                return;
            m_Player.Stop(clip);
        }
        //--------------------------------------------------------
        public void RemoveActionState(EActionStateType eType, uint nTag = 0)
        {
          //  if (m_pBakerSkin != null)
          //  {
          //      m_pBakerSkin.Stop(eType, nTag);
          //      return;
          //  }
            if (null == m_Player)
                return;
            if (m_GraphData == null)
                return;
            var clip = GetActorAction(eType, nTag);
            if (clip == null)
                return;
            m_Player.RemoveMotion(clip);
        }
        //--------------------------------------------------------
        public uint GetCurrentPlayActionStatePriority(uint layer)
        {
         //   if (m_pBakerSkin != null)
         //   {
         //       uint actionTag = m_pBakerSkin.GetCurPlayActionTag();
         //       if (actionTag == 0xffffffff)
         //           return 0;
         //       var action = GetActorAction(actionTag);
         //       if (action == null || action.layer != layer) return 0;
         //       return action.priority;
         //   }
        //    else
            {
                if (m_Player == null) return 0;
                var action = m_Player.GetPlayingAction(layer);
                if (action == null) return 0;
                if(action is ActorAction)
                {
                    return ((ActorAction)action).priority;
                }
            }
            return 0;
        }
        //--------------------------------------------------------
        public ActorAction GetCurrentPlayActionState(uint layer)
        {
          //  if (m_pBakerSkin != null)
         //   {
          //      uint actionTag = m_pBakerSkin.GetCurPlayActionTag();
          //      if (actionTag == 0xffffffff)
          //          return null;
          //      var action = GetActorAction(actionTag);
          //      if (action == null || action.layer != layer) return null;
          //      return action;
          //  }
          //  else
            {
                if (m_Player == null) return null;
                var action = m_Player.GetPlayingAction(layer);
                if (action == null) return null;
                if (action is ActorAction)
                {
                    return ((ActorAction)action);
                }
            }
            return null;
        }
        //--------------------------------------------------------
        public bool IsInAction(EActionStateType eType)
        {
          //  if (m_pBakerSkin != null)
          //  {
          //      uint actionTag = m_pBakerSkin.GetCurPlayActionTag();
          //      if (actionTag == 0xffffffff)
          //          return false;
          //      var action = GetActorAction(actionTag);
          //      if (action == null) return false;
          //      if (action.type == eType) return true;
          //      return false;
           // }
          /*  else*/ if (m_Player!=null)
            {
                var actions = m_Player.GetBindStates();
                if (actions == null) return false;
                foreach(var db in actions)
                {
                    if(db.Key is ActorAction)
                    {
                        if (((ActorAction)db.Key).type == eType)
                            return true;
                    }
                }
            }
            return false;
        }
        //--------------------------------------------------------
        protected override void OnUpdate(float fDelta)
        {
            base.OnUpdate(fDelta);
            if (!m_pPlayableGraph.IsValid())
                return;
            m_pPlayableGraph.Evaluate(fDelta);
        }
        //--------------------------------------------------------
        protected override void OnDestroy()
        {
            if (m_nPlayableId != 0)
            {
#if UNITY_EDITOR
                GraphPlayableUtil.UnCollectPlayable(m_nPlayableId);
#endif
            }
            m_nPlayableId = 0;
            if (m_Player != null)
            {
                m_Player.StopAll();
                m_Player.Destroy();
            }
            m_Player = null;
            if (m_pPlayableGraph.IsValid())
                m_pPlayableGraph.Destroy();
            m_GraphData = null;
       //     if (m_pBakerSkin != null) m_pBakerSkin.Destroy();
       //      m_pBakerSkin = null;
        }
	}
}