/********************************************************************
生成日期:	5:11:2020  20:36
类    名: 	ESlotBindBit
作    者:	HappLI
描    述:	
*********************************************************************/
using Framework.DrawProps;

namespace Framework.ActorSystem.Runtime
{
    public enum ESlotBindBit
    {
        [Disable]
        None = 0,
        [Display("位置")]
        Position = 1 << 0,
        [Display("角度")]
        Rotation = 1 << 1,
        [Display("缩放")]
        Scale = 1 << 2,
        [Disable]
        All = Position | Rotation | Scale,
    }
}
