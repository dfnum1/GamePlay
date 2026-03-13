/********************************************************************
生成日期:	06:30:2025
类    名: 	CustomEventObject
作    者:	HappLI
描    述:	蓝图自定义事件
*********************************************************************/

namespace Framework.AT.Runtime
{
    //-----------------------------------------------------
    //! CustomEventObject 
    //-----------------------------------------------------
    public class CustomEventObject : ACustomEventObject
    {
    }
#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(CustomEventObject))]
    public class AAgentTreeObjectEditor : ACustomEventObjectEditor
    {
    }
#endif
}