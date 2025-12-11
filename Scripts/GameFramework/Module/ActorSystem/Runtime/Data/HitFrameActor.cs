/********************************************************************
生成日期:	5:11:2020  20:36
类    名: 	HitFrameActor
作    者:	HappLI
描    述:	命中帧数据
*********************************************************************/
using Framework.AT.Runtime;
using Framework.Core;
using Framework.DrawProps;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.ActorSystem.Runtime
{
    //------------------------------------------------------
    public enum EHitType
    {
        [Display("未知")]
        Unknown = 0,
        [Display("爆炸")]
        Explode = 1,
        [Display("弹射")]
        Bound = 2,
        [Display("贯穿")]
        MutiHit = 3,
    }
    //------------------------------------------------------
    [ATInteralExport("Actor系统/命中帧数据", -3, icon: "ActorSystem/hit_frame_actor")]
    public struct HitFrameActor : IUserData
    {
        [ATField("攻击者",true,false)]public Actor attack_ptr;
        [ATField("受击者",true,false)]public Actor target_ptr;
        public AttackFrameParameter frameParameter;
        public AActorStateInfo attack_state_param;
        public AActorStateInfo target_state_param;
        [ATField("受击位置",true,false)]public Vector3 hit_position;
        [ATField("受击朝向", true,false)]public Vector3 hit_direction;
        public uint damage_id;
        public ushort damage_level;
        public IContextData skill_data;
        public byte projectileClassify;
        public int damage_power;
        public uint hit_body_part;
        [ATField("受击类型",true,false)]public EHitType hitType;
        public int mul_hit_cnt;
        public int attacker_target_count;
        public int hitType_take_data_0;
        public int hitType_take_data_1;
        [ATField("是否打到场景", true, false)] public bool bHitScene;
        public HitFrameActor(uint damage_id, Actor attacker, Actor targeter, AttackFrameParameter frameParameter, Vector3 hit_position, Vector3 hit_direction, byte projectileClassify = 0,
            AActorStateInfo attack_state_param = null, AActorStateInfo target_state_param = null/*,
            AFrameClip attack_frame = null, AFrameClip target_frame = null*/)
        {
            this.damage_id = damage_id;
            this.projectileClassify = 0;
            this.damage_level = 1;
            this.hit_position = hit_position;
            this.hit_direction = hit_direction;
            this.attack_state_param = attack_state_param;
            this.target_state_param = target_state_param;
            this.damage_power = 1;
            this.attack_ptr = attacker;
            this.target_ptr = targeter;
            //   this.attack_frame = attack_frame;
            //   this.target_frame = target_frame;
            this.skill_data = null;
            this.hitType = EHitType.Unknown;
            this.hitType_take_data_0 = 0;
            this.hitType_take_data_1 = 0;
            this.attacker_target_count = 0;
            this.hit_body_part = 0;
            this.mul_hit_cnt = 1;
            this.bHitScene = false;
            this.frameParameter = frameParameter;
        }
        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 31 + (attack_ptr != null ? attack_ptr.GetInstanceID() : 0);
            hash = hash * 31 + (target_ptr != null ? target_ptr.GetInstanceID() : 0);
            hash = hash * 31 + damage_id.GetHashCode();
            return hash;
        }
        public bool Equals(HitFrameActor other)
        {
            return attack_ptr == other.attack_ptr && target_ptr == other.target_ptr && damage_id == other.damage_id;
        }

        public static HitFrameActor DEFAULT = new HitFrameActor(0, null, null, null, Vector3.zero, Vector3.zero);
    }
}
