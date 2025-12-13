/********************************************************************
生成日期:	06:30:2025
类    名: 	Attribute
作    者:	HappLI
描    述:	变量
*********************************************************************/
using System;

namespace Framework.AT.Runtime
{
    //-----------------------------------------------------
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
    public class ATEditorInitializeAttribute : System.Attribute
    {
#if UNITY_EDITOR
        public string method;
#endif
        public ATEditorInitializeAttribute(string method="Init")
        {
#if UNITY_EDITOR
            this.method = method;
#endif
        }
    }
    //-----------------------------------------------------
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field, Inherited = false)]
    public class ATIconAttribute : System.Attribute
    {
#if UNITY_EDITOR
        public string name;
#endif
        public ATIconAttribute(string name)
        {
#if UNITY_EDITOR
            this.name = name;
#endif
        }
    }
    //-----------------------------------------------------
    public class ATColorAttribute : System.Attribute
    {
#if UNITY_EDITOR
        public UnityEngine.Color color;
        public bool node;
#endif
        public ATColorAttribute(string color, bool node = false)
        {
#if UNITY_EDITOR
            UnityEngine.ColorUtility.TryParseHtmlString(color, out this.color);
            this.node = node;
#endif
        }
    }
    //-----------------------------------------------------
    [AttributeUsage(AttributeTargets.Class| AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public class ATExportAttribute : System.Attribute
    {
#if UNITY_EDITOR
        public int guid;
        public string nodeName;
        public string icon;
#endif
        public ATExportAttribute(string nodeName = null, int guid = 0, string icon = null)
        {
#if UNITY_EDITOR
            this.guid = guid;
            this.nodeName = nodeName;
            this.icon = icon;
#endif
        }
    }
    //-----------------------------------------------------
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public class ATInteralExportAttribute : ATExportAttribute
    {
        public ATInteralExportAttribute(string nodeName = null, int guid = 0, string icon = null) : base(nodeName, guid, icon)
        {
        }
    }
    //-----------------------------------------------------
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public class ATClassAttribute : System.Attribute
    {
#if UNITY_EDITOR
        public string displayName;
        public System.Type classType;
#endif
        public ATClassAttribute(System.Type classType, string className)
        {
#if UNITY_EDITOR
            this.classType = classType;
            this.displayName = className;
#endif
        }
    }
    //-----------------------------------------------------
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public class ATMonoAttribute : System.Attribute
    {
#if UNITY_EDITOR
        public int guid;
        public string displayName;
        public System.Type type;
#endif
        public ATMonoAttribute(int guid, string displayName, Type type)
        {
#if UNITY_EDITOR
            this.guid = guid;
            this.displayName = displayName;
            this.type = type;
#endif
        }
    }
    //-----------------------------------------------------
    [AttributeUsage(AttributeTargets.Method)]
    public class ATFunctionAttribute : Attribute
    {
#if UNITY_EDITOR
        public string DisplayName { get; set; }
        public string ToolTips;
        public Type DecleType;
        public int guid;
        public string icon = "";
        public bool bFieldGet = false;
#endif
        public ATFunctionAttribute(int guid, string displayName, Type type, bool bFieldGet = false, string ToolTips = "", string icon = "")
        {
#if UNITY_EDITOR
            this.guid = guid;
            this.ToolTips = ToolTips;
            DisplayName = displayName;
            this.DecleType = type;
            this.bFieldGet = bFieldGet;
            this.icon = icon;
#endif
        }
#if UNITY_EDITOR
        internal ATActionAttribute ToAction()
        {
            ATActionAttribute attr = new ATActionAttribute(DisplayName,false,true,true,true);

            return attr;
        }
#endif
    }
    //-----------------------------------------------------
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class ATFunctionArgvAttribute : Attribute
    {
#if UNITY_EDITOR
        public Type DisplayType;
        public Type AlignType;
        public Type ArgvType;
        public string DisplayName;
        public string ToolTips;
        public bool bAutoDestroy = false;
        public bool bReturn = false;
        public bool bSeriable = true;
        public bool bShowEdit = true;
        public int ListElementByArgvIndex = -1;

        internal System.Object defaultValue = null;
        internal bool isExternAttrThis = false;
        internal bool isDelegateCall;
        internal bool isDelegateCallValid;
        internal System.Collections.Generic.List<ATFunctionArgvAttribute> vDelegateArgvs;
#endif
        public ATFunctionArgvAttribute(Type ArgvType, string DisplayName = "", bool bAutoDestroy = false, Type AlignType = null, Type DisplayType = null, string ToolTips = "", bool bReturn = false, int ListElementByArgvIndex = -1, bool bSeriable = true, bool bShowEdit = true)
        {
#if UNITY_EDITOR
            this.ArgvType = ArgvType;
            this.DisplayName = DisplayName;
            this.ToolTips = ToolTips;
            this.bAutoDestroy = bAutoDestroy;
            this.DisplayType = DisplayType;
            this.AlignType = AlignType;
            this.bReturn = bReturn;
            this.ListElementByArgvIndex = ListElementByArgvIndex;
            this.bSeriable = bSeriable;
            this.bShowEdit = bShowEdit;
            this.isExternAttrThis = false;
            this.isDelegateCall = false;
            this.isDelegateCallValid = false;

#endif
        }
        public ATFunctionArgvAttribute(Type ArgvType, string DisplayName, object DefauleValue, Type AlignType, Type DisplayType = null, string ToolTips = "", bool bReturn = false, int ListElementByArgvIndex = -1, bool bSeriable = true, bool bShowEdit = true)
        {
#if UNITY_EDITOR
            this.ArgvType = ArgvType;
            this.DisplayType = DisplayType;
            this.DisplayName = DisplayName;
            this.AlignType = AlignType;
            this.ToolTips = ToolTips;
            this.bAutoDestroy = false;
            this.DisplayType = DisplayType;
            this.bReturn = bReturn;
            this.ListElementByArgvIndex = ListElementByArgvIndex;
            this.bSeriable = bSeriable;
            this.bShowEdit = bShowEdit;
            this.isExternAttrThis = false;
            this.isDelegateCall = false;
            this.isDelegateCallValid = false;
#endif
        }
#if UNITY_EDITOR
        internal ArgvAttribute ToArgv()
        {
            return new ArgvAttribute(DisplayName, DisplayType, ArgvType, bShowEdit, defaultValue);
        }
#endif
    }
    //-----------------------------------------------------
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ATFunctionReturnAttribute : Attribute
    {
#if UNITY_EDITOR
        public Type DisplayType;
        public Type AlignType;
        public Type ReturnType;
        public string Name;
        public string ToolTips = "";
        public bool bAutoDestroy = false;
        public bool bSeriable = true;
        public bool bShowEdit = true;
        public byte bPropertySet = 0; //0=none, 1-get,2-set,3-getset
        public int ListElementByArgvIndex = -1;
#endif
        public ATFunctionReturnAttribute(Type ReturnType, Type AlignType = null, string name = "", bool bAutoDestroy = false, string ToolTips = "", int ListElementByArgvIndex = -1, bool bSeriable = true, bool bShowEdit = true, byte bPropertySet = 0)
        {
#if UNITY_EDITOR
            this.ReturnType = ReturnType;
            this.AlignType = AlignType;
            this.Name = name;
            this.ToolTips = ToolTips;
            this.bAutoDestroy = bAutoDestroy;
            this.ListElementByArgvIndex = ListElementByArgvIndex;
            this.bSeriable = bSeriable;
            this.bShowEdit = bShowEdit;
            this.bPropertySet = bPropertySet;
#endif
        }
        public ATFunctionReturnAttribute(Type ReturnType, string name, Type AlignType = null, Type DisplayType = null, string ToolTips = "", int ListElementByArgvIndex = -1, bool lbSeriable = true, bool bShowEdit = true, byte bPropertySet = 0)
        {
#if UNITY_EDITOR
            this.ReturnType = ReturnType;
            this.AlignType = AlignType;
            this.Name = name;
            this.ToolTips = ToolTips;
            this.bAutoDestroy = false;
            this.DisplayType = DisplayType;
            this.ListElementByArgvIndex = ListElementByArgvIndex;
            this.bSeriable = lbSeriable;
            this.bShowEdit = bShowEdit;
            this.bPropertySet = bPropertySet;
#endif
        }
#if UNITY_EDITOR
        internal ReturnAttribute ToArgv()
        {
            return new ReturnAttribute(Name, DisplayType, AlignType);
        }
#endif
    }
    //-----------------------------------------------------
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ATMethodAttribute : System.Attribute
    {
#if UNITY_EDITOR
        public string method;
        public string[] argvNames;
#endif
        public ATMethodAttribute(params string[] argvNames)
        {
#if UNITY_EDITOR
            this.method = null;
            this.argvNames = argvNames;
#endif
        }
        public ATMethodAttribute(string name, params string[] argvNames)
        {
#if UNITY_EDITOR
            this.method = name;
            this.argvNames = argvNames;
#endif
        }
    }
    //-----------------------------------------------------
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ATMethodArgvAttribute : System.Attribute
    {
#if UNITY_EDITOR
        public string argv;
        public System.Type displayType;
        public string displayTypeStr;
        public string drawerTypeStr;
#endif
        public ATMethodArgvAttribute(string argv, string displayType, string drawLabel = null)
        {
#if UNITY_EDITOR
            this.argv = argv;
            this.displayTypeStr = displayType;
            this.displayType = null;
            this.drawerTypeStr = drawLabel;
#endif
        }
        public ATMethodArgvAttribute(string argv, System.Type displayType, string drawLabel = null)
        {
#if UNITY_EDITOR
            this.argv = argv;
            this.displayTypeStr = null;
            this.displayType = displayType;
            this.drawerTypeStr = drawLabel;
#endif
        }
    }
    //-----------------------------------------------------
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class ATFieldAttribute : System.Attribute
    {
#if UNITY_EDITOR
        public string method;
        public bool bGet;
        public bool bSet;
#endif
        public ATFieldAttribute(bool bGet = true, bool bSet = false)
        {
#if UNITY_EDITOR
            this.method = null;
            this.bGet = bGet;
            this.bSet = bSet;
#endif
        }
        public ATFieldAttribute(string name, bool bGet = true, bool bSet = false)
        {
#if UNITY_EDITOR
            this.method = name;
            this.bGet = bGet;
            this.bSet = bSet;
#endif
        }
    }
    //-----------------------------------------------------
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    public class ATActionAttribute : System.Attribute
    {
#if UNITY_EDITOR
        public string name;
        public string tips;
        public bool isTask; // 是否为任务节点
        public bool hasInput;
        public bool hasOutput;
        public bool bShow;
        public bool allowMuti;
#endif
        public ATActionAttribute(string name, bool isTask = false, bool hasInput = true, bool hasOutput = true, bool bShow = true, bool allowMuti= true)
        {
#if UNITY_EDITOR
            this.name = name;
            this.tips = null;
            this.isTask = isTask;
            this.hasInput = hasInput;
            this.hasOutput = hasOutput;
            this.bShow = bShow;
            this.allowMuti = allowMuti;
#endif
        }
        public ATActionAttribute(string name, string tips, bool isTask = false, bool hasInput = true, bool hasOutput = true, bool bShow = true, bool allowMuti = true)
        {
#if UNITY_EDITOR
            this.name = name;
            this.tips = tips;
            this.isTask = isTask;
            this.hasInput = hasInput;
            this.hasOutput = hasOutput;
            this.bShow = bShow;
            this.allowMuti = allowMuti;
#endif
        }
    }
    //-----------------------------------------------------
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    public class ArgvAttribute : System.Attribute
    {
#if UNITY_EDITOR
        public string name;
        public string tips;
        public System.Type argvType;
        public object defValue; // 默认值
        public bool canEdit;
        public System.Type displayType;
        public EVariableType[] limitVarTypes;
#endif
        //-----------------------------------------------------
        public ArgvAttribute(string name, System.Type argvType, bool canEdit = false, System.Object defValue = null, params EVariableType[] limitTypes)
        {
#if UNITY_EDITOR
            this.name = name;
            this.tips = null;
            this.argvType = argvType;
            this.defValue = defValue;
            this.canEdit = canEdit;
            this.limitVarTypes = limitTypes;
#endif
        }
        //-----------------------------------------------------
        public ArgvAttribute(string name, System.Type argvType, System.Type displayType, bool canEdit = false, System.Object defValue = null, params EVariableType[] limitTypes)
        {
#if UNITY_EDITOR
            this.name = name;
            this.tips = null;
            this.argvType = argvType;
            this.displayType = displayType;
            this.defValue = defValue;
            this.canEdit = canEdit;
            this.limitVarTypes = limitTypes;
#endif
        }
        //-----------------------------------------------------
        public ArgvAttribute(string name, string tips, System.Type argvType, bool canEdit = false, System.Object defValue = null, params EVariableType[] limitTypes)
        {
#if UNITY_EDITOR
            this.name = name;
            this.tips = tips;
            this.argvType = argvType;
            this.defValue = defValue;
            this.canEdit = canEdit;
            this.limitVarTypes = limitTypes;
#endif
        }
        //-----------------------------------------------------
        public ArgvAttribute(string name, string tips, System.Type argvType, System.Type displayType, bool canEdit = false, System.Object defValue = null, params EVariableType[] limitTypes)
        {
#if UNITY_EDITOR
            this.name = name;
            this.tips = tips;
            this.argvType = argvType;
            this.displayType = displayType;
            this.defValue = defValue;
            this.canEdit = canEdit;
            this.limitVarTypes = limitTypes;
#endif
        }
#if UNITY_EDITOR
        //-----------------------------------------------------
        public T ToValue<T>(T defVal = default)
        {
            try
            {
                if (defValue == null)
                    return defVal;

                Type targetType = typeof(T);

                // 先特殊处理枚举
                if (targetType.IsEnum)
                {
                    // 支持名称和数字
                    if (Enum.TryParse(targetType, defValue.ToString(), true, out object enumVal))
                        return (T)enumVal;
                    // 尝试数字转枚举
                    if (int.TryParse(defValue.ToString(), out int intVal))
                        return (T)Enum.ToObject(targetType, intVal);
                    return defVal;
                }

                // 支持所有常见数值类型
                if (targetType == typeof(byte))
                    return (T)(object)byte.Parse(defValue.ToString());
                if (targetType == typeof(bool))
                    return (T)(object)defValue.ToString().Equals("true", StringComparison.OrdinalIgnoreCase);
                if (targetType == typeof(short))
                    return (T)(object)short.Parse(defValue.ToString());
                if (targetType == typeof(ushort))
                    return (T)(object)ushort.Parse(defValue.ToString());
                if (targetType == typeof(int))
                    return (T)(object)int.Parse(defValue.ToString());
                if (targetType == typeof(uint))
                    return (T)(object)uint.Parse(defValue.ToString());
                if (targetType == typeof(long))
                    return (T)(object)long.Parse(defValue.ToString());
                if (targetType == typeof(ulong))
                    return (T)(object)ulong.Parse(defValue.ToString());
                if (targetType == typeof(float))
                    return (T)(object)float.Parse(defValue.ToString());
                if (targetType == typeof(double))
                    return (T)(object)double.Parse(defValue.ToString());
                if (targetType == typeof(decimal))
                    return (T)(object)decimal.Parse(defValue.ToString());
                if (targetType == defValue.GetType())
                {
                    return (T)defValue;
                }
                // 其它类型尝试通用转换
                return (T)Convert.ChangeType(defValue, targetType);
            }
            catch
            {
                return defVal;
            }
        }
#endif
    }
    //-----------------------------------------------------
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    public class ReturnAttribute : System.Attribute
    {
#if UNITY_EDITOR
        public string name;
        public string tips;
        public System.Type argvType;
        public System.Type displayType;
#endif
        public ReturnAttribute(string name, System.Type argvType, System.Type displayType = null)
        {
#if UNITY_EDITOR
            this.name = name;
            this.tips = null;
            this.displayType = displayType;
            this.argvType = argvType;
#endif
        }
        //-----------------------------------------------------
        public ReturnAttribute(string name, string tips, System.Type argvType, System.Type displayType = null)
        {
#if UNITY_EDITOR
            this.name = name;
            this.tips = tips;
            this.argvType = argvType;
            this.displayType = displayType;
#endif
        }
    }
    //-----------------------------------------------------
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    public class LinkAttribute : System.Attribute
    {
#if UNITY_EDITOR
        public string name;
        public string tips;
        public bool linkIn;
#endif
        public LinkAttribute(string name, bool linkIn)
        {
#if UNITY_EDITOR
            this.name = name;
            this.tips = null;
            this.linkIn = linkIn;
#endif
        }
        //-----------------------------------------------------
        public LinkAttribute(string name, string tips, bool linkIn)
        {
#if UNITY_EDITOR
            this.name = name;
            this.tips = tips;
            this.linkIn = linkIn;
#endif
        }
    }
    //-----------------------------------------------------
    [AttributeUsage(AttributeTargets.Enum, AllowMultiple = true, Inherited = false)]
    public class ATTypeAttribute : System.Attribute
    {
#if UNITY_EDITOR
        public string name;
        public System.Type ownerType;
#endif
        public ATTypeAttribute(string name, System.Type ownerType = null)
        {
#if UNITY_EDITOR
            this.name = name;
            this.ownerType = ownerType;
#endif
        }
    }
}