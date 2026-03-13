/********************************************************************
生成日期:	06:30:2025
类    名: 	TransformShakeClip
作    者:	HappLI
描    述:	抖动用于模拟对象震动效果，使用简单的正弦函数来模拟震动效果。
*********************************************************************/
#if UNITY_EDITOR
using Framework.Cutscene.Editor;
#endif
using Framework.DrawProps;
using UnityEngine;
using UnityEngine.Playables;

namespace Framework.Cutscene.Runtime
{
    [System.Serializable, CutsceneClip("对象抖动Clip")]
    public class TransformShakeClip : IBaseClip
    {
        [Display("基本属性")] public BaseClipProp       baseProp;
        [Display("相机抖动")] public bool               useCamera = true;
        [Display("震动强度")] public Vector3            shakeIntense = new Vector3(0.1f, 0.25f,0.0f);
        [Display("震动频率")] public Vector3            shakeHertz = new Vector3(60,50,1);
        [Display("衰减曲线")] public AnimationCurve     decayCurve = AnimationCurve.Linear(0, 1, 1, 0);
        //-----------------------------------------------------
        public ACutsceneDriver CreateDriver()
        {
            return new TransformShakeDriver();
        }
        //-----------------------------------------------------
        public ushort GetIdType()
        {
            return (ushort)EClipType.eTransformShake;
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
        [AddInspector]
        public void OnCameraEditor()
        {
            if (GUILayout.Button("模拟"))
            {
            }
        }
#endif
    }
    //-----------------------------------------------------
    //相机抖动逻辑
    //-----------------------------------------------------
    public class TransformShakeDriver : ACutsceneDriver
    {
        private Camera m_pMainCamera = null;
        private Transform m_pTransform;
        private Vector3 m_TotalShake = Vector3.zero;
        System.Collections.Generic.List<ICutsceneObject> m_vObjects;
        private float m_fLastTime = 0;
        //-----------------------------------------------------
        public override void OnDestroy()
        {
#if UNITY_EDITOR
            if (IsEditorMode() && m_pMainCamera) LockUtil.RestoreCamera(m_pMainCamera);
#endif
            if (m_vObjects != null)
            {
                UnityEngine.Pool.ListPool<ICutsceneObject>.Release(m_vObjects);
                m_vObjects = null;
            }
            m_pMainCamera = null;
            m_pTransform = null;
            m_TotalShake = Vector3.zero;
            m_fLastTime = 0;
        }
        //-----------------------------------------------------
        public override bool OnClipEnter(CutsceneTrack pTrack, FrameData clip)
        {
            m_fLastTime = 0;
            m_TotalShake = Vector3.zero;
            var clipData = clip.clip.Cast<TransformShakeClip>();
            m_vObjects = pTrack.GetBindAllCutsceneObject(m_vObjects);
            if(clipData.useCamera)
            {
                var bindObj = pTrack.GetBindLastCutsceneObject();
                if (bindObj != null) m_pMainCamera = bindObj.GetCamera();
                if (m_pMainCamera == null) m_pMainCamera = Camera.main;
                if (m_pMainCamera)
                {
#if UNITY_EDITOR
                    if (IsEditorMode() && !ControllerRefUtil.IsControlling(m_pMainCamera)) LockUtil.RestoreCamera(m_pMainCamera);
#endif
                    m_pTransform = m_pMainCamera.transform;
#if UNITY_EDITOR
                    if (IsEditorMode()) LockUtil.BackupCamera(m_pMainCamera);
#endif
                }
            }
            else
            {
                m_pMainCamera = null;
                m_pTransform = null;
            }
            return true;
        }
        //-----------------------------------------------------
        public override bool OnClipLeave(CutsceneTrack pTrack, FrameData clip)
        {
            m_fLastTime = 0;
            if (clip.CanRestore())
            {
                var clipData = clip.clip.Cast<TransformShakeClip>();
                if(clipData.useCamera)
                {
                    if (ControllerRefUtil.ControllRef(m_pMainCamera) <= 0)
                    {
                        if (m_pTransform)
                        {
                            m_pTransform.position -= m_TotalShake;
                        }
#if UNITY_EDITOR
                        if (IsEditorMode() && m_pMainCamera) LockUtil.RestoreCamera(m_pMainCamera);
#endif
                    }
                }
                else
                {
                    if (m_vObjects != null)
                    {
                        foreach (var db in m_vObjects)
                        {
                            Vector3 pos = Vector3.zero;
                            if (db.GetParamPosition(ref pos))
                                db.SetParamPosition(pos - m_TotalShake);
                        }
                    }
                }

                if (m_vObjects != null)
                {
                    UnityEngine.Pool.ListPool<ICutsceneObject>.Release(m_vObjects);
                    m_vObjects = null;
                }
            }
            m_TotalShake = Vector3.zero;

            return true;
        }
        //-----------------------------------------------------
        public override bool OnFrameClip(CutsceneTrack pTrack, FrameData frameData)
        {
            if (frameData.eStatus == EPlayableStatus.Pause)
                return true;

            if (Mathf.Abs(m_fLastTime - frameData.subTime) <= 0.01f)
                return true;
            m_fLastTime = frameData.subTime;

            var clipData = frameData.clip.Cast<TransformShakeClip>();
            if (clipData.useCamera)
            {
                if (m_pTransform == null) return true;
                if (m_pTransform)
                {
                    float dampping = 1;
                    if (clipData.decayCurve != null && clipData.decayCurve.length > 0)
                    {
                        float maxTime = clipData.decayCurve[clipData.decayCurve.length - 1].time;
                        if (maxTime > 0)
                            dampping = clipData.decayCurve.Evaluate(frameData.subTime / frameData.clip.GetDuration() * maxTime);
                    }
                    float fShakeX = clipData.shakeIntense.x * ((float)Mathf.Sin(clipData.shakeHertz.x * frameData.subTime)) * dampping;
                    float fShakeY = clipData.shakeIntense.y * ((float)Mathf.Sin(clipData.shakeHertz.y * frameData.subTime)) * dampping;
                    float fShakeZ = clipData.shakeIntense.z * ((float)Mathf.Sin(clipData.shakeHertz.z * frameData.subTime)) * dampping;

                    var offset = fShakeX * m_pTransform.forward + fShakeY * m_pTransform.up + fShakeZ * m_pTransform.right;
                    m_TotalShake += offset;
                    m_pTransform.position += offset;
                }
            }
            else
            {
                if (m_vObjects != null && m_vObjects.Count > 0)
                {
                    float dampping = 1;
                    if (clipData.decayCurve != null && clipData.decayCurve.length > 0)
                    {
                        float maxTime = clipData.decayCurve[clipData.decayCurve.length - 1].time;
                        if (maxTime > 0)
                            dampping = clipData.decayCurve.Evaluate(frameData.subTime / frameData.clip.GetDuration() * maxTime);
                    }
                    float fShakeX = clipData.shakeIntense.x * ((float)Mathf.Sin(clipData.shakeHertz.x * frameData.subTime)) * dampping;
                    float fShakeY = clipData.shakeIntense.y * ((float)Mathf.Sin(clipData.shakeHertz.y * frameData.subTime)) * dampping;
                    float fShakeZ = clipData.shakeIntense.z * ((float)Mathf.Sin(clipData.shakeHertz.z * frameData.subTime)) * dampping;

                    var offset = fShakeX * m_pTransform.forward + fShakeY * m_pTransform.up + fShakeZ * m_pTransform.right;
                    m_TotalShake += offset;
                    foreach (var db in m_vObjects)
                    {
                        Vector3 pos = Vector3.zero;
                        if (db.GetParamPosition(ref pos))
                            db.SetParamPosition(pos + offset);
                    }
                }
                else
                {
                    m_vObjects = pTrack.GetBindAllCutsceneObject(m_vObjects);
                }
            }
            return true;
        }
    }
}