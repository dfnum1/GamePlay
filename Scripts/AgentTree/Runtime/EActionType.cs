/********************************************************************
生成日期:	06:30:2025
类    名: 	EActionType
作    者:	HappLI
描    述:	内置的行为类型
*********************************************************************/
namespace Framework.AT.Runtime
{
    //-----------------------------------------------------
    [ATType("任务")]
    public enum ETaskType
    {
        [ATAction("开始", true, false, true), ATIcon("at_enter_start")]
        eStart = 1,//任务开始

        [ATAction("Tick", true, false, true), ATIcon("at_enter_tick")]
        eTick = 2,

        [ATAction("退出", true, false, true), ATIcon("at_enter_exit")]
        eExit = 3,

        eTaskEndId = 100,//任务开始
    }
    //-----------------------------------------------------
    [ATType("常规")]
    public enum EActionType
    {
        [DrawProps.Disable]eActionBegin = 101,

        [ATAction("变量获取", false, false, false, false)]
        [Return("GUID",typeof(int))]
        eGetVariable = eActionBegin+1,//获取变量

        [ATAction("条件判断")]
        [Argv("参数1", typeof(IVariable), true)]
        [Argv("符号", typeof(ECompareOpType), true)]
        [Argv("参数2", typeof(IVariable), true)]
        [Link("不成立", false)]
        eCondition ,//条件

        [ATAction("运算")]
        [Argv("参数1", typeof(IVariable), true)]
        [Argv("符号", typeof(EOpType), true, null)]
        [Argv("参数2", typeof(IVariable), true)]
        [Return("结果", typeof(IVariable))]
        eOpVariable,

        [ATAction("Dot")]
        [Argv("参数1", typeof(IVariable), true, null, EVariableType.eVec2, EVariableType.eVec3)]
        [Argv("参数2", typeof(IVariable), true, null, EVariableType.eVec2, EVariableType.eVec3)]
        [Return("结果", typeof(float))]
        eDotVariable,

        [ATAction("Cross")]
        [Argv("参数1", typeof(IVariable), true,null,EVariableType.eVec2, EVariableType.eVec3)]
        [Argv("参数2", typeof(IVariable), true, null, EVariableType.eVec2, EVariableType.eVec3)]
        [Return("结果", typeof(IVariable))]
        eCrossVariable,

        [ATAction("坐标距离")]
        [Argv("参数1", typeof(IVariable), true, null,EVariableType.eVec2, EVariableType.eVec3)]
        [Argv("参数2", typeof(IVariable), true, null,EVariableType.eVec2, EVariableType.eVec3)]
        [Return("结果", typeof(float))]
        eDistanceVariable,

        [ATAction("过渡Lerp")]
        [Argv("参数1", typeof(IVariable), true,null,EVariableType.eFloat, EVariableType.eVec2, EVariableType.eVec3)]
        [Argv("参数2", typeof(IVariable), true,null,EVariableType.eFloat, EVariableType.eVec2, EVariableType.eVec3)]
        [Argv("速度", typeof(float), true)]
        [Return("结果", typeof(IVariable))]
        eLerp,

        [ATAction("新建变量", false, false, false)]
        [Argv("变量", typeof(IVariable), true)]
        [Return("输出", typeof(IVariable))]
        eNewVariable,//新建变量

        [DrawProps.Disable]
        eCutsceneCustomEvent = 999,//Cutscene自定义事件

        [DrawProps.Disable]
        eCustomBegin = 1000,//自定义事件开始
    }
}