using System;

namespace Framework.Base
{
    //-----------------------------------------------------
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
    public class EditorSetupInitAttribute : System.Attribute
    {
#if UNITY_EDITOR
        public string method;
#endif
        public EditorSetupInitAttribute(string method = "Init")
        {
#if UNITY_EDITOR
            this.method = method;
#endif
        }
    }
}