/********************************************************************
生成日期:	5:11:2020  20:36
类    名: 	ActorTransformLogic
作    者:	HappLI
描    述:	移动逻辑
*********************************************************************/
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
namespace Framework.ActorSystem.Runtime
{
    public class ActorTransformLogic : AActorAgent
    {
        protected FVector3  m_PositionOffset = FVector3.zero;
        protected FVector3  m_Speed = FVector3.zero;
        protected FFloat    m_fGravity = 0.0f;
        protected bool      m_bUseGravity = true;
        protected FFloat    m_fFraction = 0.0f;
        //--------------------------------------------------------
        public void Reset()
        {
            m_fFraction = 0;
            m_bUseGravity = true;
            m_fGravity = ActorSystemUtil.GTRAVITY_VALUE;

            m_PositionOffset = FVector3.zero;
            m_Speed = FVector3.zero;
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
        //--------------------------------------------------------
        protected override void OnUpdate(float fDelta)
        {
            if (m_pActor.IsKilled())
                return;
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
            if (Physics.Raycast(m_pActor.GetPosition(), Vector3.down, out hit, Mathf.Max(m_PositionOffset.y*1.25f), m_pActor.GetActorManager().GetTerrainLayerMask()))
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