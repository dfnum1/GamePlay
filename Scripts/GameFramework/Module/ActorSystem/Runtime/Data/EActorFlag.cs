/********************************************************************
生成日期:	5:11:2020  20:36
类    名: 	EActorFlag
作    者:	HappLI
描    述:	
*********************************************************************/
using Framework.DrawProps;

namespace Framework.ActorSystem.Runtime
{
    public enum EActorStatus
    {
        [Display("同步创建")] Create = 0,
        [Display("异步创建")] AsyncCreate,
        [Display("激活")] Active,
        [Display("显示")] Visible,
        [Display("隐藏")] Hide,
        [Display("阵亡")] Killed,
        [Display("复活")] Revive,
        [Display("删除")] Destroy,
        [Display("加载完成")] Loaded,
    }

    public enum EActorFlag
    {
        [Display("激活")]Active = 1 << 0,
        [Display("删除")]Destroy = 1 << 1,
        [Display("可见")]Visible = 1 << 2,
        [Display("死亡")]Killed = 1 << 3,
        [Display("逻辑")]Logic = 1 << 4,
        [Display("AI")]AI = 1 << 5,
        [Display("空间树")]Spatial = 1 << 6,
        [Display("可查找")]CollectAble = 1 << 7,
        [Display("调试")]Debug = 1 << 8,
        [Display("动态避障")]RVO = 1 << 9,
        [Display("服务器输入")]SvrSyncIn = 1 << 10,
        [Display("服务器输出")]SvrSyncOut = 1 << 11,
        [Display("Hud")]HudBar = 1 << 12,
        [Display("物品碰撞")]ColliderAble = 1 << 13,
        [Display("强制2D")]Facing2D = 1 << 14,
        [Disable]Default = EActorFlag.Visible | EActorFlag.Active | Spatial | CollectAble | RVO | Debug | HudBar,
    }
}
