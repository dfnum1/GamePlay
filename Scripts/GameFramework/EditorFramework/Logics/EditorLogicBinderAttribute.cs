/********************************************************************
生成日期:		11:06:2020
类    名: 	EditorLogicBinderAttribute
作    者:	HappLI
描    述:	编辑器逻辑绑定器
*********************************************************************/
#if UNITY_EDITOR
using System;

namespace Framework.ED
{
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true)]
    public class EditorBinderAttribute : System.Attribute
    {
        public System.Type bindType;
        public int order;
        public string rectMethod;
        public EditorBinderAttribute(System.Type bindType, int order = 0)
        {
            this.bindType = bindType;
            this.order = order;
            this.rectMethod = null;
        }
        public EditorBinderAttribute(System.Type bindType, string rectMethod, int order = 0)
        {
            this.bindType = bindType;
            this.order = order;
            this.rectMethod = rectMethod;
        }
    }
    //-----------------------------------------------------
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true)]
    public class EditorInitOnloadAttribute : System.Attribute
    {
        public string callMethod;
        public EditorInitOnloadAttribute(string callMethod= "OnEditorInitOnload")
        {
            this.callMethod = callMethod;
        }
    }
    //-----------------------------------------------------
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CustomPreferenceAttribute : Attribute
    {
        public string key = "";
        public string header = "";
        public string method = "OnPreferencesGUI";
        public CustomPreferenceAttribute(string key, string header = "", string method = "OnPreferencesGUI")
        {
            this.key = key;
            this.header = header;
            this.method = method;
        }
    }
}

#endif