/********************************************************************
生成日期:	5:11:2020  20:36
类    名: 	ActorTransformLogic
作    者:	HappLI
描    述:	移动逻辑
*********************************************************************/
#if USE_FIXEDMATH
using ExternEngine;
#else
using Framework.AT.Runtime;
using Framework.Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FFloat = System.Single;
using FMatrix4x4 = UnityEngine.Matrix4x4;
using FQuaternion = UnityEngine.Quaternion;
using FVector2 = UnityEngine.Vector2;
using FVector3 = UnityEngine.Vector3;
#endif
namespace Framework.ActorSystem.Runtime
{
    public class RunAlongPathAgnet : AActorAgent
    {
        protected EActionStateType  m_eRunActionStateType = EActionStateType.Run;
        protected uint              m_nRunActionStateTag = 0;
        protected bool              m_bRunningAlongPathPoint = false;
        protected bool              m_bRunningAlongPathPlay = false;
        protected bool              m_bRunAlongPathPointEnsureSucceed = false;
        protected bool              m_bRunAlongPathPointUpdateDirection = false;
        protected FFloat            m_fRunAlongPathPointSpeed = 0.0f;
        protected FFloat            m_fRunAlongPathPointTime = 0.0f;
        protected FFloat            m_fRunAlongPathPointDelta = 0.0f;
        protected FVector3          m_LocalRunAlongPoisiotn = FVector3.zero;
        protected Vector3Track      m_pPathPointTrack = new Vector3Track();
        //--------------------------------------------------------
        protected override void OnInit()
        {
            m_eRunActionStateType = EActionStateType.Run;
            m_nRunActionStateTag = 0;
            m_bRunningAlongPathPlay = false;
            m_bRunningAlongPathPoint = false;
            m_bRunAlongPathPointEnsureSucceed = false;
            m_bRunAlongPathPointUpdateDirection = false;
            m_fRunAlongPathPointSpeed = 0.0f;
            m_fRunAlongPathPointDelta = 0.0f;
            m_fRunAlongPathPointTime = 0.0f;
            m_LocalRunAlongPoisiotn = FVector3.zero;
            m_pPathPointTrack.Clear();
            m_pActor.SetSpeedXZ(FVector3.zero);
        }
        //--------------------------------------------------------
        public FFloat GetRunSpeed()
        {
            return m_pActor.GetActorParameter().GetSpeed();
        }
        //-------------------------------------------------
        public void SetRunActionType(EActionStateType eType, uint tag =0)
        {
            m_eRunActionStateType = eType;
            m_nRunActionStateTag = tag;
        }
        //-------------------------------------------------
        public FFloat RunTo(FVector3 toPos, FFloat fSpeed = 0)
        {
            return RunAlongPathPoint(m_pActor.GetPosition(), toPos, fSpeed);
        }
        //-------------------------------------------------
        public void NavRunTo(FVector3 toPos, FFloat fSpeed = 0)
        {
            if (m_pActor.GetFramework() == null)
                return;
            m_pActor.GetFramework().OnSimpleFindPath(m_pActor, toPos, fSpeed, OnSimpleFindPath);
        }
        //-------------------------------------------------
        void OnSimpleFindPath(List<FVector3> vPathPoint, FFloat fSpeed)
        {
            RunAlongPathPoint(vPathPoint, fSpeed);
        }
        //-------------------------------------------------
        public FFloat RunAlongPathPoint(List<FVector3> vPathPoint, FFloat fSpeed = 0, bool bEnsureSucceed = false, bool bUpdateDirection = true)
        {
            if (fSpeed <= 0) fSpeed = Mathf.Max(0.1f, GetRunSpeed());
            if (vPathPoint.Count > 1)
            {
                StopRunAlongPathPoint();
                m_bRunningAlongPathPoint = true;
                m_bRunningAlongPathPlay = true;;
                m_bRunAlongPathPointEnsureSucceed = bEnsureSucceed;
                m_bRunAlongPathPointUpdateDirection = bUpdateDirection;
                m_fRunAlongPathPointSpeed = fSpeed;
                m_fRunAlongPathPointDelta = 0.0f;
                m_LocalRunAlongPoisiotn = FVector3.zero;
                m_pPathPointTrack.Clear();
                m_pPathPointTrack.AddKeyPoint(0.0f, vPathPoint[0]);

                FFloat fPathLength = 0.0f;
                for (int i = 1; i < vPathPoint.Count; i++)
                {
                    FVector3 vStart = vPathPoint[i - 1];
                    FVector3 vEnd = vPathPoint[i];
                    FFloat fPathSegmentLength = (vEnd - vStart).magnitude;
                    if (fPathSegmentLength <= 0) continue;
                    fPathLength += fPathSegmentLength;
                    FFloat fTime = fPathLength / m_fRunAlongPathPointSpeed;
                    m_pPathPointTrack.AddKeyPoint(fTime, vEnd);
                }

                m_fRunAlongPathPointTime = fPathLength / m_fRunAlongPathPointSpeed;
                OnRunAlongPathPoint(m_pPathPointTrack);
            }
            else
            {
                StopRunAlongPathPoint();
                m_fRunAlongPathPointTime = -1.0f;
            }

            return m_fRunAlongPathPointTime;
        }
        //------------------------------------------------------
        public FFloat RunAlongPathPoint(FVector3 srcPos, FVector3 toPos, FFloat fSpeed, bool bEnsureSucceed = false, bool bUpdateDirection = true)
        {
            FFloat fPathLength = (toPos - srcPos).magnitude;
            if (fPathLength > 0)
            {
                StopRunAlongPathPoint();
                if (fSpeed <= 0) fSpeed = Mathf.Max(0.1f, GetRunSpeed());
                m_bRunningAlongPathPoint = true;
                m_bRunningAlongPathPlay = true;
                m_bRunAlongPathPointEnsureSucceed = bEnsureSucceed;
                m_bRunAlongPathPointUpdateDirection = bUpdateDirection;
                m_fRunAlongPathPointSpeed = fSpeed;
                m_fRunAlongPathPointDelta = 0.0f;
                m_LocalRunAlongPoisiotn = FVector3.zero;
                m_pPathPointTrack.Clear();
                m_pPathPointTrack.AddKeyPoint(0.0f, srcPos);

                FFloat fTime = fPathLength / m_fRunAlongPathPointSpeed;
                m_pPathPointTrack.AddKeyPoint(fTime, toPos);
                m_fRunAlongPathPointTime = fTime;
                OnRunAlongPathPoint(m_pPathPointTrack);
            }
            else
            {
                StopRunAlongPathPoint();
                m_fRunAlongPathPointTime = -1.0f;
            }

            return m_fRunAlongPathPointTime;
        }
        //------------------------------------------------------
        protected virtual void OnRunAlongPathPoint(Vector3Track vPathPoint) 
        {
        }
        //------------------------------------------------------
        public void StopRunAlongPathPoint()
        {
            if (m_bRunningAlongPathPoint)
            {
                OnStopAlongPathPoint();

                m_bRunningAlongPathPlay = false;
                m_bRunningAlongPathPoint = false;
                m_bRunAlongPathPointEnsureSucceed = false;
                m_bRunAlongPathPointUpdateDirection = false;
                m_fRunAlongPathPointSpeed = 0.0f;
                m_fRunAlongPathPointDelta = 0.0f;
                m_fRunAlongPathPointTime = 0.0f;
                m_LocalRunAlongPoisiotn = FVector3.zero;
                m_pPathPointTrack.Clear();
                m_pActor.SetSpeedXZ(FVector3.zero);
            }
        }
        //-------------------------------------------------
        public void PauseRunAlongPathPoint()
        {
            if (m_bRunningAlongPathPoint)
                m_bRunningAlongPathPlay = false;
        }
        //-------------------------------------------------
        public void ResumeRunAlongPathPoint()
        {
            if (m_bRunningAlongPathPoint)
                m_bRunningAlongPathPlay = true;
        }
        //-------------------------------------------------
        protected virtual void OnStopAlongPathPoint()
        {
        }
        //-------------------------------------------------
        public bool IsRunAlongPathPlaying()
        {
            return m_bRunningAlongPathPlay && m_bRunningAlongPathPoint;
        }
        //-------------------------------------------------
        protected override void OnUpdate(float fDelta)
        {
            if (m_pActor.IsKilled())
                return;

            UpdateRunAlongPathPoint(fDelta);
        }
        //--------------------------------------------------------
        protected void UpdateRunAlongPathPoint(FFloat fFrame)
        {
            if (m_bRunningAlongPathPoint)
            {
                if (m_bRunningAlongPathPlay) m_fRunAlongPathPointDelta += fFrame;
                FVector3 vTargetPosition, vToPosition;
                bool bOk = m_pPathPointTrack.Evaluate(m_fRunAlongPathPointDelta, out vTargetPosition, out vToPosition);
                if (!bOk)
                {
                    StopRunAlongPathPoint();
                    return;
                }
                FVector3 vPostionOffset = vTargetPosition - m_pActor.GetPosition();
                if (m_bRunAlongPathPointEnsureSucceed)
                {
                    m_pActor.SetPosition(vTargetPosition);
                }
                else
                {
                    FFloat fOffset = vPostionOffset.magnitude;
                    if (fOffset > 0.01f)
                    {
                        FVector3 vSpeedVector = vPostionOffset / fOffset;
                        vSpeedVector.y = 0.0f;
                        m_pActor.SetSpeedXZ(vSpeedVector * m_fRunAlongPathPointSpeed);
                    }
                }
                if (m_bRunAlongPathPointUpdateDirection)
                {
                    vPostionOffset.y = 0;
                    bool bForward = vPostionOffset.x > 0f;
                    if (m_pActor.IsFlag(EActorFlag.Facing2D))
                        m_pActor.SetDirection(bForward ? FVector3.right : (-FVector3.right), 0.1f, false);
                    else
                    {
                        vPostionOffset = vToPosition - m_pActor.GetPosition();
                        vPostionOffset.y = 0;
                        m_pActor.SetDirection(vPostionOffset.normalized, 0.1f, false);
                    }
                }
                if (m_fRunAlongPathPointDelta >= m_fRunAlongPathPointTime)
                    StopRunAlongPathPoint();
            }
        }
        //------------------------------------------------------
        public static FFloat APPLY_FRACTION(FFloat speed, FFloat fraction, FFloat time)
        {
#if USE_FIXEDMATH
            if (FMath.Abs(fraction) > 0.01f && FMath.Abs(speed) > 0.01f)
#else
            if (System.Math.Abs(fraction) > 0.01f && System.Math.Abs(speed) > 0.01f)
#endif
            {
                FFloat temp_speed = speed;
                if (temp_speed > 0f)
                    temp_speed -= fraction * time;
                else
                    temp_speed += fraction * time;
                if (temp_speed * speed < 0f)
                    speed = 0;
                else
                    speed = temp_speed;
            }
            return speed;
        }
        //--------------------------------------------------------
        protected override void OnDestroy()
        {
        }
	}
}