/********************************************************************
生成日期:	5:11:2020  20:36
类    名: 	EActorFlag
作    者:	HappLI
描    述:	
*********************************************************************/
namespace Framework.ActorSystem.Runtime
{
    public enum EActorStatus
    {
        Create = 0,
        AsyncCreate,
        Active,
        Visible,
        Hide,
        Killed,
        Revive,
        Destroy,
        Loaded,
    }

    public enum EActorFlag
    {
        Active = 1 << 0,
        Destroy = 1 << 1,
        Visible = 1 << 2,
        Killed = 1 << 3,
        Logic = 1 << 4,
        AI = 1 << 5,
        Spatial = 1 << 6,
        CollectAble = 1 << 7,
        Debug = 1 << 8,
        RVO = 1 << 9,
        SvrSyncIn = 1 << 10,
        SvrSyncOut = 1 << 11,
        HudBar = 1 << 12,
        ColliderAble = 1 << 13,
        Facing2D = 1 << 14,
        Default = EActorFlag.Visible | EActorFlag.Active | Spatial | CollectAble | RVO | Debug | HudBar,
    }
}
