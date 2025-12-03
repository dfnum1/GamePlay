/********************************************************************
生成日期:	5:11:2020  20:36
类    名: 	AttackFrameParameter
作    者:	HappLI
描    述:	攻击帧参数数据
*********************************************************************/
using Framework.DrawProps;
using UnityEngine;

namespace Framework.ActorSystem.Runtime
{
    //------------------------------------------------------
    [System.Serializable]
    public class AttackFrameParameter : IContextData
    {
        [Display("受击硬直")]
        public float stuck_time_hit;
        [Display("受击计算朝向")]
        public bool target_direction_postion;
        //! on hit
        [Display("受击者受击动作类型")]
        public uint target_action_hit;
        [Display("击退力度")]
        public Vector3 hit_back_speed;
        [Display("击退时位移摩檫力", "<0时,不起作用")]
        public float hit_back_fraction = -1;
        [Display("击退时位移重力", "<0时,不起作用")]
        public float hit_back_gravity = -1f;

        [Display("受击持续时长", "如果为0且有动作，则取动作的时长")] public float target_duration_hit;
        [Display("受击特效缩放")] public float target_effect_hit_scale = 1.0f;
        [Display("受击特效"), StringView(typeof(GameObject))] public string target_effect_hit;
        [Display("受击特效偏移")] public Vector3 target_effect_hit_offset;
        [Display("受击特效挂点"), BindSlot] public string effect_hit_slot;

        [Display("声音")]
        public string sound_hit;

        [Display("伤害ID")]
        public uint damage;

        public void Reset()
        {
            target_action_hit = 0;
            stuck_time_hit = 0.0f;
            target_direction_postion = false;
            target_duration_hit = 0.0f;
            target_effect_hit_offset = Vector3.zero;

            target_effect_hit = "";
            sound_hit = "";
            damage = 0;
        }
    }
}