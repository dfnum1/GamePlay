/********************************************************************
生成日期:	11:07:2025
类    名: 	GameStateAttributes
作    者:	HappLI
描    述:	属性定义
*********************************************************************/

using System.IO;

namespace Framework.State.Runtime
{
    //------------------------------------------------------------
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true)]
    public class DecNameAttribute : System.Attribute
    {
#if UNITY_EDITOR
        public string name;
#endif
        public DecNameAttribute(string name)
        {
#if UNITY_EDITOR
            this.name = name;
#endif
        }
    }
    //------------------------------------------------------------
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true)]
    public class GameStateAttribute : DecNameAttribute
    {
        public GameStateAttribute(string name) : base(name)
        {
        }
    }
    //------------------------------------------------------------
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple=true)]
    public class GameStateLogicAttribute : DecNameAttribute
    {
#if UNITY_EDITOR
        public int order;
        public System.Type[] limitStates;
#endif
        public GameStateLogicAttribute(string name, int order = 0) : base(name)
        {
#if UNITY_EDITOR
            this.name = name;
            this.order = order;
            limitStates = null;
#endif
        }
        //------------------------------------------------------------
        public GameStateLogicAttribute(string name, System.Type[] limitStateTypes, int order = 0) : base(name)
        {
#if UNITY_EDITOR
            this.name = name;
            this.order = order;
            this.limitStates = limitStateTypes;
#endif
        }
    }
    //------------------------------------------------------------
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true)]
    public class GameModeAttribute : DecNameAttribute
    {
#if UNITY_EDITOR
        public System.Type[] limitStates;
#endif
        public GameModeAttribute(string name) : base(name)
        {
#if UNITY_EDITOR
            this.name = name;
            this.limitStates = null;
#endif
        }
        public GameModeAttribute(string name, System.Type[] limitStateTypes) : base(name)
        {
#if UNITY_EDITOR
            this.name = name;
            this.limitStates = limitStateTypes;
#endif
        }
    }
    //------------------------------------------------------------
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true)]
    public class GameModeLogicAttribute : DecNameAttribute
    {
#if UNITY_EDITOR
        public int order;
        public System.Type[] limitModes;
#endif
        public GameModeLogicAttribute(string name, int order=0) : base(name)
        {
#if UNITY_EDITOR
            this.name = name;
            this.order = order;
            limitModes = null;
#endif
        }
        //------------------------------------------------------------
        public GameModeLogicAttribute(string name, System.Type[] limitModeTypes, int order = 0) : base(name)
        {
#if UNITY_EDITOR
            this.name = name;
            this.order = order;
            limitModes = limitModeTypes;
#endif
        }
    }
    //------------------------------------------------------------
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true)]
    public class StateIconAttribute : System.Attribute
    {
#if UNITY_EDITOR
        public string name;
#endif
        public StateIconAttribute(string name)
        {
#if UNITY_EDITOR
            string suffix = Path.GetExtension(name);
            if(string.IsNullOrEmpty(suffix))
            {
                name += ".png";
            }
            this.name = name;
#endif
        }
    }
}

