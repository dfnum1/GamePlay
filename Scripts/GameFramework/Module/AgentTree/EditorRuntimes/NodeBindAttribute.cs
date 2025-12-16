/********************************************************************
生成日期:	06:30:2025
类    名: 	NodeBindAttribute
作    者:	HappLI
描    述:	节点绑定属性
*********************************************************************/
#if UNITY_EDITOR
using Framework.AT.Runtime;
using System;

namespace Framework.AT.Editor
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class NodeBindAttribute : System.Attribute
    {
        public int actionType;
        public int customType;
        public NodeBindAttribute(int actionType,int customType = 0)
        {
            this.actionType = actionType;
            this.customType = customType;
        }
        public NodeBindAttribute(EActionType actionType, int customType = 0)
        {
            this.actionType = (int)actionType;
            this.customType = customType;
        }
    }
    //-----------------------------------------------------
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class EditorBindNodeAttribute : System.Attribute
    {
        public System.Type nodeType;
        public EditorBindNodeAttribute(System.Type nodeType)
        {
            this.nodeType = nodeType;
        }
    }
}
#endif