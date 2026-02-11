/********************************************************************
生成日期:	5:11:2020  20:36
类    名: 	ActorTransformLogic
作    者:	HappLI
描    述:	移动逻辑
*********************************************************************/
#if USE_FIXEDMATH
using ExternEngine;
using UnityEngine;
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
    public class ActorTransformLogic : AActorAgent
    {
        protected FVector3          m_PositionOffset = FVector3.zero;
        protected FVector3          m_Speed = FVector3.zero;
        protected FFloat            m_fGravity = 0.0f;
        protected bool              m_bUseGravity = true;
        protected FFloat            m_fFraction = 0.0f;

        protected bool              m_bTurning = false;
        protected FVector2          m_fTurningDelta = FVector2.zero;
        FQuaternion                 m_OriginRotation;
        FQuaternion                 m_TargetRotation;
        FQuaternion                 m_CurRotation;
        //--------------------------------------------------------
        protected override void OnInit()
        {
            m_fFraction = 0;
            m_bUseGravity = true;
            m_fGravity = ActorSystemUtil.GTRAVITY_VALUE;

            m_PositionOffset = FVector3.zero;
            m_Speed = FVector3.zero;

            m_fTurningDelta = FVector2.zero;
            m_bTurning = false;
            m_OriginRotation = FQuaternion.identity;
            m_TargetRotation = FQuaternion.identity;
            m_CurRotation = FQuaternion.identity;
        }
        //------------------------------------------------------
        void SetEulerAngleImmediately(FVector3 vEulerAngle)
        {
            m_bTurning = false;
            m_TargetRotation.eulerAngles = vEulerAngle;
            m_OriginRotation.eulerAngles = vEulerAngle;
            m_CurRotation = m_OriginRotation;
            m_pActor.SetTransformEulerAngle(vEulerAngle);
        }
        //------------------------------------------------------
        public void SetEulerAngle(FVector3 vEulerAngle, bool bImmediately=false)
        {
            if(bImmediately)
            {
                SetEulerAngleImmediately(vEulerAngle);
                return;
            }
            bool bFacing = m_pActor.IsFlag(EActorFlag.Facing2D);
            if (bFacing)
            {
                SetDirection(BaseUtil.EulersAngleToDirection(vEulerAngle));
                return;
            }
            m_bTurning = GetTurnTime() > 0;
            if (m_bTurning)
            {
                m_OriginRotation = m_CurRotation;
                m_TargetRotation.eulerAngles = vEulerAngle;
                m_fTurningDelta.x = 0.0f;
                m_fTurningDelta.y = GetTurnTime();
            }
            else
            {
                m_TargetRotation.eulerAngles = vEulerAngle;
                m_OriginRotation.eulerAngles = vEulerAngle;
                m_CurRotation = m_OriginRotation;
                m_pActor.SetTransformEulerAngle(vEulerAngle);
            }
        }
        //------------------------------------------------------
        public void SetDirectionImmediately(FVector3 vDir)
        {
            m_bTurning = false;
            BaseUtil.CU_GetQuaternionFromDirection(vDir, m_pActor.GetUp(), ref m_TargetRotation);
            m_OriginRotation = m_TargetRotation;
            m_CurRotation = m_OriginRotation;
            m_pActor.SetTransformEulerAngle(m_OriginRotation.eulerAngles);
        }
        //------------------------------------------------------
        public void SetDirection(FVector3 vDir)
        {
            SetDirection(vDir, 0);
        }
        //------------------------------------------------------
        public void SetDirection(FVector3 vDir, FFloat turnTime, bool replaceTurnTime = true)
        {
            if (vDir.sqrMagnitude <= 0) return;
            if(turnTime <=0)
            {
                SetDirectionImmediately(vDir);
                return;
            }

            bool bFacing = m_pActor.IsFlag(EActorFlag.Facing2D);
            if (bFacing)
            {
#if USE_FIXEDMATH
                FFloat dotVal = FMath.Dot(vDir, FVector3.right);
#else
                FFloat dotVal = Vector3.Dot(vDir, FVector3.right);
#endif
                if (dotVal <= 0)
                    vDir = FVector3.back;
                else
                    vDir = FVector3.forward;
            }

            if (vDir.sqrMagnitude > 0) vDir = vDir.normalized;
            if (GetFinalDirection() == vDir) return;

            m_bTurning = GetTurnTime() > 0 || turnTime > 0;
            BaseUtil.CU_GetQuaternionFromDirection(vDir, GetFinalUp(), ref m_TargetRotation);
            if (m_bTurning || turnTime > 0)
            {
                m_OriginRotation = m_CurRotation;
                if (replaceTurnTime || m_fTurningDelta.y <= 0)
                {
                    m_fTurningDelta.x = 0;
                    if (turnTime > 0) m_fTurningDelta.y = turnTime;
                    else m_fTurningDelta.y = GetTurnTime();
                }

                m_bTurning = true;
            }
            else
            {
                BaseUtil.CU_GetQuaternionFromDirection(vDir, GetFinalUp(), ref m_TargetRotation);
                m_OriginRotation = m_TargetRotation;
                m_CurRotation = m_OriginRotation;
                m_pActor.SetTransformEulerAngle(m_CurRotation.eulerAngles);
            }
        }
        //------------------------------------------------------
        public void SetUp(FVector3 vUp)
        {
            if (vUp.sqrMagnitude <= 0) return;
            if (vUp.sqrMagnitude > 0) vUp = vUp.normalized;
            SetUp(vUp,0);
        }
        //------------------------------------------------------
        public void SetUp(FVector3 vUp, FFloat turnTime, bool replaceTurnTime = true)
        {
            if (vUp.sqrMagnitude <= 0) return;
            if (vUp.sqrMagnitude > 0) vUp = vUp.normalized;

            if (GetFinalUp() == vUp) return;

            m_bTurning = GetTurnTime() > 0 || turnTime > 0;
            BaseUtil.CU_GetQuaternionFromDirection(GetFinalDirection(), vUp, ref m_TargetRotation);
            if (m_bTurning || turnTime > 0)
            {
                m_OriginRotation = m_CurRotation;
                if (replaceTurnTime || m_fTurningDelta.y <=0)
                {
                    m_fTurningDelta.x = 0;
                    if (turnTime > 0) m_fTurningDelta.y = turnTime;
                    else m_fTurningDelta.y = GetTurnTime();
                }
                m_bTurning = true;
            }
            else
            {
                BaseUtil.CU_GetQuaternionFromDirection(GetFinalDirection(), vUp, ref m_TargetRotation);
                m_OriginRotation = m_TargetRotation;
                m_CurRotation = m_OriginRotation;
                m_pActor.SetTransformEulerAngle(m_OriginRotation.eulerAngles);
            }
        }
        //------------------------------------------------------
        public FVector3 GetFinalDirection()
        {
            return m_TargetRotation * FVector3.forward;
        }
        //------------------------------------------------------
        public FVector3 GetFinalEulerAngle()
        {
            return m_TargetRotation.eulerAngles;
        }
        //------------------------------------------------------
        public FVector3 GetFinalRight()
        {
            return m_TargetRotation * FVector3.right;
        }
        //------------------------------------------------------
        public FVector3 GetFinalUp()
        {
            return m_TargetRotation * FVector3.up;
        }
        //------------------------------------------------------
        public FFloat GetTurnTime()
        {
            return 0.1f;
        }
        //------------------------------------------------------
        public bool HasSpeedXZ()
        {
#if USE_FIXEDMATH
            return FMath.Abs(m_Speed.x) > 0.1f || FMath.Abs(m_Speed.z) > 0.1f;
#else
            return System.Math.Abs(m_Speed.x) > 0.1f || System.Math.Abs(m_Speed.z) > 0.1f;
#endif
        }
        //------------------------------------------------------
        public void SetFarction(FFloat fFraction)
        {
            m_fFraction = fFraction;
        }
        //------------------------------------------------------
        public FFloat GetFarction()
        {
            return m_fFraction;
        }
        //------------------------------------------------------
        public void SetGravity(FFloat fGravity)
        {
            m_fGravity = fGravity;
        }
        //------------------------------------------------------
        public FFloat GetGravity()
        {
            return m_fGravity;
        }
        //------------------------------------------------------
        public void EnableGravity(bool bEnable)
        {
            m_bUseGravity = bEnable;
        }
        //------------------------------------------------------
        public FVector3 GetSpeed() 
        {
            return m_Speed;
        }
        //------------------------------------------------------
        public void SetSpeed(FVector3 vSpeed)
        {
            m_Speed = vSpeed;
        }
        //------------------------------------------------------
        public void SetSpeedXZ(FVector3 vSpeed)
        {
            m_Speed.x = vSpeed.x;
            m_Speed.z = vSpeed.z;
        }
        //------------------------------------------------------
        public void SetSpeedXZ(FFloat fSpeedX, FFloat fSpeedZ)
        {
            m_Speed.x = fSpeedX;
            m_Speed.z = fSpeedZ;
        }
        //------------------------------------------------------
        public void SetSpeedY(FFloat fSpeed)
        {
            m_Speed.y = fSpeed;
        }
        //--------------------------------------------------------
        public FFloat GetRunSpeed()
        {
            return m_pActor.GetActorParameter().GetSpeed();
        }
        //-------------------------------------------------
        protected override void OnUpdate(float fDelta)
        {
            if (m_pActor.IsKilled())
                return;

            if (m_bTurning)
            {
                m_fTurningDelta.x += fDelta;
                if (m_fTurningDelta.y > 0 && m_fTurningDelta.x < m_fTurningDelta.y)
                {
#if USE_FIXEDMATH
                    FFloat fFactor = FMath.Clamp01(m_fTurningDelta.x / m_fTurningDelta.y);
#else
                    FFloat fFactor = Mathf.Clamp01(m_fTurningDelta.x / m_fTurningDelta.y);
#endif
                    m_CurRotation = FQuaternion.Lerp(m_OriginRotation, m_TargetRotation, fFactor);
                }
                else
                {
                    m_fTurningDelta = FVector2.zero;
                    m_bTurning = false;
                    m_OriginRotation = m_TargetRotation;
                    m_CurRotation = m_TargetRotation;
                }
                m_pActor.SetTransformEulerAngle(m_CurRotation.eulerAngles);
            }

            if (!m_pActor.IsFlag(EActorFlag.Logic))
                return;

            FVector3 pushSpeed = FVector3.zero;
            //if (IsEnableRVO() && m_fRVOPushForce > 0)
            //{
            //    pushSpeed = GetWorld().ComputerNewVelocity(this, new FVector3(m_Speed.x, 0.0f, m_Speed.z), m_fRVOPushForce, Base.ConfigUtil.timeHorizon, Base.ConfigUtil.timeHorizonObst, out var isColission);
            //}

            if (m_bUseGravity)
            {
                m_Speed.y -= m_fGravity * fDelta;// * fTime * 0.5f;
            }

            //! apply fraction
            m_Speed.x = APPLY_FRACTION(m_Speed.x, m_fFraction, fDelta);
            m_Speed.z = APPLY_FRACTION(m_Speed.z, m_fFraction, fDelta);
            m_PositionOffset.x = (m_Speed.x + pushSpeed.x) * fDelta * GetRunSpeed();
            m_PositionOffset.z = (m_Speed.z + pushSpeed.z) * fDelta * GetRunSpeed();
            m_PositionOffset.y = (m_Speed.y + pushSpeed.y) * fDelta;

            RaycastHit hit;
            if (Physics.Raycast(m_pActor.GetPosition(), FVector3.down, out hit, Mathf.Max(m_PositionOffset.y*1.25f), m_pActor.GetActorManager().GetTerrainLayerMask()))
            {
                m_PositionOffset.y = hit.point.y;
            }
            else
            {
                Vector3 pos = m_PositionOffset + m_pActor.GetPosition();
                if (pos.y <= m_pActor.GetActorManager().GetTerrainHeight())
                    m_PositionOffset.y = 0;
            }
            m_pActor.OffsetPosition(m_PositionOffset);
            m_PositionOffset = FVector3.zero;
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