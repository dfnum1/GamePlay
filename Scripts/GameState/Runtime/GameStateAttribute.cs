/********************************************************************
生成日期:	11:07:2025
类    名: 	GameStateAttributes
作    者:	HappLI
描    述:	属性定义
*********************************************************************/

namespace Framework.State.Runtime
{
    //------------------------------------------------------------
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true)]
    public class GameStateAttribute : System.Attribute
    {
#if UNITY_EDITOR
        public string name;
#endif
        public GameStateAttribute(string name)
        {
#if UNITY_EDITOR
            this.name = name;
#endif
        }
    }
    //------------------------------------------------------------
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple=true)]
    public class GameStateLogicAttribute : System.Attribute
    {
#if UNITY_EDITOR
        public string name;
        public int order;
        public System.Type[] limitStates;
#endif
        public GameStateLogicAttribute(string name, int order = 0)
        {
#if UNITY_EDITOR
            this.name = name;
            this.order = order;
            limitStates = null;
#endif
        }
        //------------------------------------------------------------
        public GameStateLogicAttribute(string name, System.Type[] limitStateTypes, int order = 0)
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
    public class GameModeAttribute : System.Attribute
    {
#if UNITY_EDITOR
        public string name;
        public System.Type[] limitStates;
#endif
        public GameModeAttribute(string name)
        {
#if UNITY_EDITOR
            this.name = name;
            this.limitStates = null;
#endif
        }
        public GameModeAttribute(string name, System.Type[] limitStateTypes)
        {
#if UNITY_EDITOR
            this.name = name;
            this.limitStates = limitStateTypes;
#endif
        }
    }
    //------------------------------------------------------------
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true)]
    public class GameModeLogicAttribute : System.Attribute
    {
#if UNITY_EDITOR
        public string name;
        public int order;
        public System.Type[] limitModes;
#endif
        public GameModeLogicAttribute(string name, int order=0)
        {
#if UNITY_EDITOR
            this.name = name;
            this.order = order;
            limitModes = null;
#endif
        }
        //------------------------------------------------------------
        public GameModeLogicAttribute(string name, System.Type[] limitModeTypes, int order = 0)
        {
#if UNITY_EDITOR
            this.name = name;
            this.order = order;
            limitModes = limitModeTypes;
#endif
        }
    }
}

