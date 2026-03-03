/********************************************************************
生成日期:	06:30:2025
类    名: 	TweenClip
作    者:	HappLI
描    述:	Tween样式
*********************************************************************/
#if UNITY_EDITOR
using Framework.Cutscene.Editor;
using UnityEditor;
#endif
using Framework.DrawProps;
using UnityEngine;
using UnityEngine.Playables;
using static Framework.Cutscene.Runtime.TweenClip;

namespace Framework.Cutscene.Runtime
{
    [System.Serializable, CutsceneClip("TweenClip")]
    public class TweenClip : IBaseClip
    {
        [Display("基本属性")] public BaseClipProp baseProp;
        [Display("晃动方式")] public EQuationType quationType = EQuationType.RTG_EASE_IN;
        [Display("晃动类型")] public EEaseType easeType = EEaseType.RTG_LINEAR;

        [Display("位置")] public bool usePos = true;
        [Display("目标位置"), StateByField("usePos", "true")] public Vector3 targetPos;

        [Display("朝向")] public bool useRot = false;
        [Display("目标角度"), StateByField("useRot", "true")] public Vector3 targetEuler;

        [Display("缩放")] public bool useScale = false;
        [Display("目标缩放"), StateByField("useScale", "true")] public Vector3 targetScale;
        [Display("回归")] public bool backMirror = false;
        //-----------------------------------------------------
        public ACutsceneDriver CreateDriver()
        {
            return new TweenDriver();
        }
        //-----------------------------------------------------
        public ushort GetIdType()
        {
            return (ushort)EClipType.eTween;
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
        public void OnSceneView(SceneView sceneView)
        {
            if (baseProp.ownerTrackObject != null)
            {
                var binder = baseProp.ownerTrackObject.GetBindLastCutsceneObject();
                if (binder != null)
                {
                }
            }
        }
#endif
    }
    //-----------------------------------------------------
    //Tween剪辑逻辑
    //-----------------------------------------------------
    public class TweenDriver : ACutsceneDriver
    {
        private bool m_bGetPos = false;
        private bool m_bGetRot = false;
        private bool m_bGetScale = false;
        private Vector3 m_OriPosition = Vector3.one;
        private Quaternion m_OriRotation = Quaternion.identity;
        private Vector3 m_OriScale = Vector3.one;
        private ICutsceneObject m_pHold;
        RtgEasing m_Easing = null;
        //-----------------------------------------------------
        public override void OnDestroy()
        {
            if (m_pHold != null) m_pHold.SetParamHold(false);
            m_pHold = null;
        }
        //-----------------------------------------------------
        void CheckObject(CutsceneTrack pTrack)
        {
            if (m_pHold == null)
            {
                m_pHold = pTrack.GetBindLastCutsceneObject();
            }
            if (m_pHold != null)
            {
                if(!m_bGetPos) m_bGetPos = m_pHold.GetParamPosition(ref m_OriPosition);
                if (!m_bGetRot) m_bGetRot = m_pHold.GetParamQuaternion(ref m_OriRotation);
                if (!m_bGetScale) m_bGetScale = m_pHold.GetParamScale(ref m_OriScale);
            }
        }
        //-----------------------------------------------------
        public override bool OnClipEnter(CutsceneTrack pTrack, FrameData frameData)
        {
            m_bGetPos = false;
            m_bGetRot = false;
            m_bGetScale = false;

            CheckObject(pTrack);
            if (m_pHold == null) return true;
            m_pHold.SetParamHold(true);
            var clipData = frameData.clip.Cast<TweenClip>();
            m_Easing = CutsceneKit.CreateTweenEasing(clipData.easeType);
            return true;
        }
        //-----------------------------------------------------
        public override bool OnClipLeave(CutsceneTrack pTrack, FrameData clip)
        {
            if(m_pHold!=null)
            {
                if (clip.CanRestore() || clip.IsLeaveIn())
                {
                    if (m_pHold != null)
                    {
                        if (m_bGetPos) m_pHold.SetParamPosition(m_OriPosition);
                        if (m_bGetRot) m_pHold.SetParamQuaternion(m_OriRotation);
                        if (m_bGetScale) m_pHold.SetParamScale(m_OriScale);
                    }
                }
                m_pHold.SetParamHold(false);
            }
            return true;
        }
        //-----------------------------------------------------
        public override bool OnFrameClip(CutsceneTrack pTrack, FrameData frameData)
        {
            CheckObject(pTrack);
            if (m_pHold == null)
                return true;
            var clipData = frameData.clip.Cast<TweenClip>();
            if (m_pHold != null)
            {
                float subTime = frameData.subTime;
                float duration = clipData.GetDuration();
                bool backDo = false;
                if (clipData.backMirror)
                {
                    duration /= 2;
                    if(subTime>= duration)
                    {
                        subTime -= duration;
                        backDo = true;
                    }
                }
                m_pHold.SetParamHold(true);
                if(m_bGetPos && clipData.usePos)
                {
                    Vector3 result = m_OriPosition;
                    if(backDo)
                    {
                        if (clipData.quationType == EQuationType.RTG_EASE_IN)
                        {
                            result.x = m_Easing.EaseIn(subTime, clipData.targetPos.x, result.x - clipData.targetPos.x, duration);
                            result.y = m_Easing.EaseIn(subTime, clipData.targetPos.y,  result.y - clipData.targetPos.y, duration);
                            result.z = m_Easing.EaseIn(subTime, clipData.targetPos.z, result.z - clipData.targetPos.z, duration);
                        }
                        else if (clipData.quationType == EQuationType.RTG_EASE_OUT)
                        {
                            result.x = m_Easing.EaseOut(subTime, clipData.targetPos.x, result.x - clipData.targetPos.x, duration);
                            result.y = m_Easing.EaseOut(subTime, clipData.targetPos.y, result.y - clipData.targetPos.y, duration);
                            result.z = m_Easing.EaseOut(subTime, clipData.targetPos.z, result.z - clipData.targetPos.z, duration);
                        }
                        else
                        {
                            result.x = m_Easing.EaseInOut(subTime, clipData.targetPos.x, result.x - clipData.targetPos.x, duration);
                            result.y = m_Easing.EaseInOut(subTime, clipData.targetPos.y, result.y - clipData.targetPos.y, duration);
                            result.z = m_Easing.EaseInOut(subTime, clipData.targetPos.z, result.z - clipData.targetPos.z, duration);
                        }
                    }
                    else
                    {
                        if (clipData.quationType == EQuationType.RTG_EASE_IN)
                        {
                            result.x = m_Easing.EaseIn(subTime, result.x, clipData.targetPos.x - result.x, duration);
                            result.y = m_Easing.EaseIn(subTime, result.y, clipData.targetPos.y - result.y, duration);
                            result.z = m_Easing.EaseIn(subTime, result.z, clipData.targetPos.z - result.z, duration);
                        }
                        else if (clipData.quationType == EQuationType.RTG_EASE_OUT)
                        {
                            result.x = m_Easing.EaseOut(subTime, result.x, clipData.targetPos.x - result.x, duration);
                            result.y = m_Easing.EaseOut(subTime, result.y, clipData.targetPos.y - result.y, duration);
                            result.z = m_Easing.EaseOut(subTime, result.z, clipData.targetPos.z - result.z, duration);
                        }
                        else
                        {
                            result.x = m_Easing.EaseInOut(subTime, result.x, clipData.targetPos.x - result.x, duration);
                            result.y = m_Easing.EaseInOut(subTime, result.y, clipData.targetPos.y - result.y, duration);
                            result.z = m_Easing.EaseInOut(subTime, result.z, clipData.targetPos.z - result.z, duration);
                        }
                    }
                        m_pHold.SetParamPosition(result);
                }
                if (m_bGetScale && clipData.useScale)
                {
                    Vector3 result = m_OriScale;
                    if (backDo)
                    {
                        if (clipData.quationType == EQuationType.RTG_EASE_IN)
                        {
                            result.x = m_Easing.EaseIn(subTime, clipData.targetScale.x,result.x - clipData.targetScale.x, duration);
                            result.y = m_Easing.EaseIn(subTime, clipData.targetScale.y,result.y - clipData.targetScale.y, duration);
                            result.z = m_Easing.EaseIn(subTime, clipData.targetScale.z,result.z - clipData.targetScale.z, duration);
                        }
                        else if (clipData.quationType == EQuationType.RTG_EASE_OUT)
                        {
                            result.x = m_Easing.EaseOut(subTime, clipData.targetScale.x, result.x - clipData.targetScale.x, duration);
                            result.y = m_Easing.EaseOut(subTime, clipData.targetScale.y, result.y - clipData.targetScale.y, duration);
                            result.z = m_Easing.EaseOut(subTime, clipData.targetScale.z, result.z - clipData.targetScale.z, duration);
                        }
                        else
                        {
                            result.x = m_Easing.EaseInOut(subTime, clipData.targetScale.x, result.x - clipData.targetScale.x, duration);
                            result.y = m_Easing.EaseInOut(subTime, clipData.targetScale.y, result.y - clipData.targetScale.y, duration);
                            result.z = m_Easing.EaseInOut(subTime, clipData.targetScale.z, result.z - clipData.targetScale.z, duration);
                        }
                    }
                    else
                    {
                        if (clipData.quationType == EQuationType.RTG_EASE_IN)
                        {
                            result.x = m_Easing.EaseIn(subTime, result.x, clipData.targetScale.x - result.x, duration);
                            result.y = m_Easing.EaseIn(subTime, result.y, clipData.targetScale.y - result.y, duration);
                            result.z = m_Easing.EaseIn(subTime, result.z, clipData.targetScale.z - result.z, duration);
                        }
                        else if (clipData.quationType == EQuationType.RTG_EASE_OUT)
                        {
                            result.x = m_Easing.EaseOut(subTime, result.x, clipData.targetScale.x - result.x, duration);
                            result.y = m_Easing.EaseOut(subTime, result.y, clipData.targetScale.y - result.y, duration);
                            result.z = m_Easing.EaseOut(subTime, result.z, clipData.targetScale.z - result.z, duration);
                        }
                        else
                        {
                            result.x = m_Easing.EaseInOut(subTime, result.x, clipData.targetScale.x - result.x, duration);
                            result.y = m_Easing.EaseInOut(subTime, result.y, clipData.targetScale.y - result.y, duration);
                            result.z = m_Easing.EaseInOut(subTime, result.z, clipData.targetScale.z - result.z, duration);
                        }
                    }
                        m_pHold.SetParamScale(result);
                }
                if (m_bGetRot && clipData.useRot)
                {
                    Quaternion result = m_OriRotation;
                    Quaternion target = Quaternion.Euler(clipData.targetEuler);
                    if (backDo)
                    {
                        if (clipData.quationType == EQuationType.RTG_EASE_IN)
                        {
                            result.x = m_Easing.EaseIn(subTime, target.x,result.x- target.x, duration);
                            result.y = m_Easing.EaseIn(subTime, target.y,result.y- target.y, duration);
                            result.z = m_Easing.EaseIn(subTime, target.z,result.z- target.z, duration);
                            result.w = m_Easing.EaseIn(subTime, target.w,result.w- target.w, duration);
                        }
                        else if (clipData.quationType == EQuationType.RTG_EASE_OUT)
                        {
                            result.x = m_Easing.EaseOut(subTime, target.x, result.x - target.x, duration);
                            result.y = m_Easing.EaseOut(subTime, target.y, result.y - target.y, duration);
                            result.z = m_Easing.EaseOut(subTime, target.z, result.z - target.z, duration);
                            result.w = m_Easing.EaseOut(subTime, target.w, result.w - target.w, duration);
                        }
                        else
                        {
                            result.x = m_Easing.EaseInOut(subTime, target.x, result.x - target.x, duration);
                            result.y = m_Easing.EaseInOut(subTime, target.y, result.y - target.y, duration);
                            result.z = m_Easing.EaseInOut(subTime, target.z, result.z - target.z, duration);
                            result.w = m_Easing.EaseInOut(subTime, target.w, result.w - target.w, duration);
                        }
                    }
                    else
                    {
                        if (clipData.quationType == EQuationType.RTG_EASE_IN)
                        {
                            result.x = m_Easing.EaseIn(subTime, result.x, target.x - result.x, duration);
                            result.y = m_Easing.EaseIn(subTime, result.y, target.y - result.y, duration);
                            result.z = m_Easing.EaseIn(subTime, result.z, target.z - result.z, duration);
                            result.w = m_Easing.EaseIn(subTime, result.w, target.w - result.w, duration);
                        }
                        else if (clipData.quationType == EQuationType.RTG_EASE_OUT)
                        {
                            result.x = m_Easing.EaseOut(subTime, result.x, target.x - result.x, duration);
                            result.y = m_Easing.EaseOut(subTime, result.y, target.y - result.y, duration);
                            result.z = m_Easing.EaseOut(subTime, result.z, target.z - result.z, duration);
                            result.w = m_Easing.EaseOut(subTime, result.w, target.w - result.w, duration);
                        }
                        else
                        {
                            result.x = m_Easing.EaseInOut(subTime, result.x, target.x - result.x, duration);
                            result.y = m_Easing.EaseInOut(subTime, result.y, target.y - result.y, duration);
                            result.z = m_Easing.EaseInOut(subTime, result.z, target.z - result.z, duration);
                            result.w = m_Easing.EaseInOut(subTime, result.w, target.w - result.w, duration);
                        }
                    }
                        
                    m_pHold.SetParamQuaternion(result);
                }
            }
            return true;
        }
    }
}