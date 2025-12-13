/********************************************************************
生成日期:	06:30:2025
类    名: 	EActionType
作    者:	HappLI
描    述:	内置的行为类型
*********************************************************************/
using Framework.DrawProps;
using UnityEngine;

namespace Framework.AT.Runtime
{
    public enum EATMouseType
    {
        [Display("无")] None,
        [Display("按下")]Begin,
        [Display("移动")] Move,
        [Display("滚动")] Wheel,
        [Display("弹起")] End,
    }
    //-----------------------------------------------------
    [ATType("任务")]
    public enum ETaskType
    {
        [ATAction("开始", true, false, true,allowMuti:false), ATIcon("AT/at_enter_start")]
        eStart = 1,//任务开始

        [ATAction("Tick", true, false, true, allowMuti: false), ATIcon("AT/at_enter_tick")]
        eTick = 2,

        [ATAction("退出", true, false, true, allowMuti: false), ATIcon("AT/at_enter_exit")]
        eExit = 3,

        [ATAction("键盘输入", true, false, true), ATIcon("AT/at_key_input")]
        [Argv("按键1", typeof(KeyCode), true)]
        [Argv("按键2", typeof(KeyCode), true)]
        [Argv("按键3", typeof(KeyCode), true)]
        [Argv("按键4", typeof(KeyCode), true)]
        eKeyInput = 4,

        [ATAction("鼠标输入", true, false, true), ATIcon("AT/at_mouse_input")]
        [Argv("状态", typeof(EATMouseType), false)]
        [Argv("输入Id", typeof(int), false)]
        [Argv("当前屏幕坐标", typeof(Vector2), false)]
        [Argv("上次屏幕坐标", typeof(Vector2), false)]
        [Argv("差值坐标", typeof(Vector2), false)]
        [Argv("是否点击UI",typeof(bool), false)]
        eMouseInput = 5,

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
        [Argv("参数1", typeof(IVariable), true,null,EVariableType.eFloat, EVariableType.eColor, EVariableType.eVec2, EVariableType.eVec3, EVariableType.eQuaternion)]
        [Argv("参数2", typeof(IVariable), true,null,EVariableType.eFloat, EVariableType.eColor, EVariableType.eVec2, EVariableType.eVec3, EVariableType.eQuaternion)]
        [Argv("速度", typeof(float), true)]
        [Return("结果", typeof(IVariable))]
        eLerp,

        [ATAction("过渡Slerp")]
        [Argv("参数1", typeof(IVariable), true, null, EVariableType.eVec3, EVariableType.eQuaternion)]
        [Argv("参数2", typeof(IVariable), true, null, EVariableType.eVec3, EVariableType.eQuaternion)]
        [Argv("速度", typeof(float), true)]
        [Return("结果", typeof(IVariable))]
        eSlerp,

        [ATAction("四元素转欧拉角")]
        [Argv("欧拉角", typeof(VariableQuaternion), true)]
        [Return("四元素", typeof(VariableVec3))]
        eQuaternionToEuler,

        [ATAction("欧拉角转四元素")]
        [Argv("欧拉角", typeof(VariableVec3), true)]
        [Return("四元素", typeof(VariableQuaternion))]
        eEulerToQuaternion,

        [ATAction("矩阵转TRS")]
        [Argv("欧拉角", typeof(VariableMatrix), true)]
        [Return("位置", typeof(VariableVec3))]
        [Return("欧拉角", typeof(VariableVec3))]
        [Return("缩放", typeof(VariableVec3))]
        eMatrixToTRS,

        [ATAction("TRS转矩阵")]
        [Argv("位置", typeof(VariableVec3),true)]
        [Argv("欧拉角", typeof(VariableVec3), true)]
        [Argv("缩放", typeof(VariableVec3), true)]
        [Return("欧拉角", typeof(VariableMatrix))]
        eTRSToMatrix,

        [ATAction("MultiplyPoint")]
        [Argv("矩阵", typeof(VariableMatrix), true)]
        [Argv("坐标位置", typeof(VariableVec3), true)]
        [Return("矩阵下的空间坐标", typeof(VariableVec3))]
        eMatrixMultiplyPoint,

        [ATAction("MultiplyPoint3x4")]
        [Argv("矩阵", typeof(VariableMatrix), true)]
        [Argv("坐标位置", typeof(VariableVec3), true)]
        [Return("矩阵下的空间坐标", typeof(VariableVec3))]
        eMatrixMultiplyPoint3x4,

        [ATAction("MultiplyVector")]
        [Argv("矩阵", typeof(VariableMatrix), true)]
        [Argv("坐标位置", typeof(VariableVec3), true)]
        [Return("矩阵下的空间坐标", typeof(VariableVec3))]
        eMatrixMultiplyVector,

        [ATAction("屏幕坐标转世界坐标")]
        [Argv("屏幕坐标", typeof(VariableVec2), true)]
        [Argv("地表高度", typeof(VariableFloat), true)]
        [Return("世界坐标", typeof(VariableVec3))]
        eScreenToWorldPosition,

        [ATAction("世界坐标转屏幕坐标")]
        [Argv("世界坐标", typeof(VariableVec3), true)]
        [Return("屏幕坐标", typeof(VariableVec2))]
        eWorldToScreenPosition,

        [ATAction("检测世界坐标点是否在屏幕中")]
        [Argv("屏幕坐标", typeof(VariableVec3), true)]
        [Argv("阀值", typeof(VariableFloat), true)]
        [Return("是否在视野", typeof(VariableBool))]
        eCheckWorldPosInView,

        [ATAction("新建变量", false, false, false)]
        [Argv("变量", typeof(IVariable), true)]
        [Return("输出", typeof(IVariable))]
        eNewVariable = 998,//新建变量

        [DrawProps.Disable]
        eCutsceneCustomEvent = 999,//Cutscene自定义事件

        [DrawProps.Disable]
        eCustomBegin = 1000,//自定义事件开始
    }
}