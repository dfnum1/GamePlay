/********************************************************************
生成日期:	5:11:2020  20:36
类    名: 	EActionStateType
作    者:	HappLI
描    述:	
*********************************************************************/
using Framework.DrawProps;

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
    [AT.Runtime.ATEvent("AT事件", ownerType:typeof(AActorComponent))]
    public enum EActorATType : int
    {
        [AT.Runtime.ATEvent("onGround")] onGround = 60000,
        [AT.Runtime.ATEvent("onHit")] onHit = 60001,
        [AT.Runtime.ATEvent("onAttack")] onAttack = 60002,
    }
}