/********************************************************************
生成日期:	06:30:2025
类    名: 	Actor
作    者:	HappLI
描    述:	Actor单位
*********************************************************************/
using System.Collections.Generic;
using TagLib.Id3v2;
using UnityEngine;
namespace Framework.ActorSystem.Runtime
{
    public class ActorParameter : TypeObject
    {
        Actor m_pActor;
        protected byte m_nAttackGroup = 0;
        public ActorParameter()
        {
            m_pActor = null;
        }
        //--------------------------------------------------------
        public void SetActor(Actor pActor)
        {
            m_pActor = pActor;
        }
        //--------------------------------------------------------
        public byte GetAttackGroup()
        {
            return m_nAttackGroup;
        }
        //------------------------------------------------------
        public void SetAttackGroup(byte attackGroup)
        {
            m_nAttackGroup = attackGroup;
        }
        //------------------------------------------------------
        public virtual bool CanAttackGroup(byte attackGroup)
        {
            return m_nAttackGroup != attackGroup;
        }
        //--------------------------------------------------------
        public ActorManager GetActorManager()
        {
            return m_pActor?.GetActorManager();
        }
        //--------------------------------------------------------
        public float GetModelHeight()
        {
            return 0;
        }
        //--------------------------------------------------------
        internal void Update(float fFrame)
        {
        }
        //--------------------------------------------------------
        public override void Destroy()
        {
        }
    }
}