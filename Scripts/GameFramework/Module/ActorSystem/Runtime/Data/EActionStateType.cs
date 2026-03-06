/********************************************************************
生成日期:	5:11:2020  20:36
类    名: 	EActionStateType
作    者:	HappLI
描    述:	
*********************************************************************/
using ExternEngine;
using Framework.AT.Runtime;
using Framework.DrawProps;
using UnityEngine;

namespace Framework.ActorSystem.Runtime
{
    [UnFilter]
    public enum EActionStateType : byte
    {
        [Display("无")] None = 0,
        [Display("待机")] Idle = 1,
        [Display("跑")] Run = 2,
        [Display("死亡")] Die = 4,

        [Display("跳跃")] Jump = 50,
        [Display("下落")] Fall = 51,

        [Display("效果组")] EffectGroup = 100,
        [Display("状态组")] StatusGroup,
        [Display("攻击组")] AttackGroup,
        [Display("受击组")] HurtGroup,

        [Disable] Count,
    }
    //------------------------------------------------------
    public enum EActionStateTag : byte
    {
        [Display("开始")] Start =0,
        [Display("开始循环")] Starting = 1,
        [Display("结束循环")] Ending = 2,
        [Display("结束")] End =3,
        [Display("循环")] Looping = 4,
    }
    //------------------------------------------------------
    public enum EActorGroundType : byte
    {
        [Display("地面")] Ground,
        [Display("空中")] Air,
        [Display("滑铲")] Lying,
        [Display("游泳")] Swimming,
        [Display("飞行")] Flying,
    };
    //------------------------------------------------------
    public enum ECollisionFlag : byte
    {
        UP = (1 << 0),
        DOWN = (1 << 1),
        SIDES = (1 << 2),
    };
    //------------------------------------------------------
    [AT.Runtime.ATType("Actor系统", ownerType:typeof(Actor))]
    public enum EActorATType : int
    {
        [AT.Runtime.ATAction("回调/攻击回调", true, false, true), AT.Runtime.ATIcon("ActorSystem/on_attack")]
        [Return("单位", typeof(Actor))]
        [Return("受击者", typeof(Actor))]
        [Return("技能", typeof(Skill))]
        onAttack = 300,

        [AT.Runtime.ATAction("回调/受击回调", true, false, true), AT.Runtime.ATIcon("ActorSystem/on_hit")]
        [Return("单位", typeof(Actor))]
        [Return("受击信息", typeof(HitFrameActor))]
        onHit,

        [AT.Runtime.ATAction("回调/死亡回调", true, false, true), AT.Runtime.ATIcon("ActorSystem/on_kill")]
        [Return("单位", typeof(Actor))]
        onKilled,

        [AT.Runtime.ATAction("回调/复活回调", true, false, true), AT.Runtime.ATIcon("ActorSystem/on_revive")]
        [Return("单位", typeof(Actor))]
        onRevive,

        [AT.Runtime.ATAction("回调/索敌回调", true, false, true), AT.Runtime.ATIcon("ActorSystem/on_lockTarget")]
        [Return("单位", typeof(Actor))]
        [Return("技能", typeof(Skill))]
        onLockTarget,

        [AT.Runtime.ATAction("回调/技能释放前置判定", true, false, true), AT.Runtime.ATIcon("ActorSystem/on_pretrigger_check")]
        [Return("单位", typeof(Actor))]
        [Return("技能", typeof(Skill))]
        onPreDoSkillCheck,

        [AT.Runtime.ATAction("回调/着陆回调",true, false, true), AT.Runtime.ATIcon("ActorSystem/on_ground")]
        [Return("单位", typeof(Actor))]
        onGround,

        [AT.Runtime.ATAction("回调/属性变更", true, false, true), AT.Runtime.ATIcon("ActorSystem/on_dirty_attr")]
        [Return("单位", typeof(Actor))]
        [Return("属性", typeof(byte), drawMethod: "DrawAttributePop")]
        [Return("之前值", typeof(FFloat))]
        [Return("新值", typeof(FFloat))]
        onDirtyAttribute,

        [AT.Runtime.ATAction("回调/移除Buff状态", true, false, true), AT.Runtime.ATIcon("ActorSystem/on_dirty_buff")]
        [Return("单位", typeof(Actor))]
        [Return("Buff", typeof(Buff))]
        [Return("移除状态", typeof(uint),drawMethod: "BuffStateDraw")]
        onRemoveBuffState,

        [AT.Runtime.ATAction("全局回调/新增Buff状态", true, false, true), AT.Runtime.ATIcon("ActorSystem/on_dirty_buff")]
        [Return("单位", typeof(Actor))]
        [Return("Buff", typeof(Buff))]
        [Return("新增状态", typeof(uint), drawMethod: "BuffStateDraw")]
        onAddBuffState,
    }
}