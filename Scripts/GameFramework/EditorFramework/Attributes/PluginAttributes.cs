/********************************************************************
生成日期:		1:11:2020 10:06
类    名: 	ObjectTypeAttribute
作    者:	HappLI
描    述:	对象类型属性
*********************************************************************/
using System;

namespace Framework.DrawProps
{  
    //-----------------------------------------------------
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class EditorLoaderAttribute : System.Attribute
    {
#if UNITY_EDITOR
        public string method;
#endif
        public EditorLoaderAttribute(string method)
        {
#if UNITY_EDITOR
            this.method = method;
#endif
        }
    }
    //-----------------------------------------------------
    [AttributeUsage(AttributeTargets.Class)]
    public class ExternPlayAudioAttribute : Attribute
    {
#if UNITY_EDITOR
        public string strMethod = "Play";
#endif
        public ExternPlayAudioAttribute(string strMethod = null)
        {
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(strMethod)) return;
            this.strMethod = strMethod;
#endif
        }
    }
}