/********************************************************************
生成日期:	06:30:2025
类    名: 	AgentTreeUtil
作    者:	HappLI
描    述:	行为树工具类
*********************************************************************/
#if UNITY_EDITOR
using Framework.AT.Runtime;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using UnityEditor;
using UnityEngine;

namespace Framework.AT.Editor
{
    public class AgentTreeAttri
    {
        public string displayName;
        public string tips;
        public string strQueueName;
        public string strMenuName;
        public int actionType;
        public bool isCutsceneCustomEvent = false;
        public int cutsceneCusomtType = 0;

        public ATActionAttribute actionAttr;
        public ATIconAttribute iconAttr;
        public ATColorAttribute colorAttr;
        public List<ArgvAttribute> argvs = new List<ArgvAttribute>();
        public List<ArgvAttribute> returns = new List<ArgvAttribute>();
        public Dictionary<ArgvAttribute, LinkAttribute> linkAttributes = new Dictionary<ArgvAttribute, LinkAttribute>();

        public List<string> popArgvs = new List<string>();
        public List<string> popReturns = new List<string>();

        public System.Type nodeType;
        public ATTypeAttribute atTypeAttr;
        public ATFunctionAttribute functionAttr;
        public System.Type functionClassType;
        public System.Type graphNodeType;
    }
    internal class MenuTreeNode
    {
        public string Name { get; set; }
        public List<MenuTreeNode> Children { get; set; }
        public object UserData { get; set; } // 可以是AgentTreeAttri或null
        public Texture Icon { get; set; }
        public string Tips { get; set; }

        public MenuTreeNode(string name)
        {
            Name = name;
            Children = new List<MenuTreeNode>();
            UserData = null;
            Icon = null;
            Tips = null;
        }
    }
    public static class AgentTreeUtil
    {
        private static List<ECompareOpType> ms_vPopCompareOpTypes = new List<ECompareOpType>();
        private static List<EVariableType> ms_vPopEnumTypes = new List<EVariableType>();
        private static List<string> ms_vPopEnumTypeNames = new List<string>();
        private static Dictionary<long, AgentTreeAttri> ms_Attrs = null;
        private static List<AgentTreeAttri> ms_vLists = new List<AgentTreeAttri>();
        private static Dictionary<System.Type, System.Type> ms_vEditorNodeTypes = new Dictionary<Type, Type>();
        private static List<string> ms_vPops = new List<string>();
        static string ms_installPath = null;
        static List<MethodInfo> ms_vInitCall = new List<MethodInfo>();
        static MenuTreeNode ms_SearchMenuRoot = new MenuTreeNode("添加节点");

        public static string BuildInstallPath()
        {
            ms_installPath = Framework.ED.EditorUtils.GetInstallEditorResourcePath();
            if (string.IsNullOrEmpty(ms_installPath))
            {
                var scripts = UnityEditor.AssetDatabase.FindAssets("t:Script CutsceneEditor");
                if (scripts.Length > 0)
                {
                    string installPath = System.IO.Path.GetDirectoryName(UnityEditor.AssetDatabase.GUIDToAssetPath(scripts[0])).Replace("\\", "/");

                    installPath = Path.Combine(installPath, "EditorResources").Replace("\\", "/");
                    if (System.IO.Directory.Exists(installPath))
                    {
                        ms_installPath = installPath;
                    }
                }
            }
            return ms_installPath;
        }
        //-----------------------------------------------------
        public static Texture2D LoadIcon(string icon)
        {
            Texture2D tex = null;
            string path = AgentTreeUtil.BuildInstallPath();
            if (!string.IsNullOrEmpty(path))
            {
                tex = AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(path, icon + ".png"));
            }
            if (tex == null)
            {
                tex = EditorGUIUtility.LoadRequired(icon) as Texture2D;
            }
            return tex;
        }
        //-----------------------------------------------------
        internal static void Init(bool bForce = false)
        {
            if (bForce || ms_Attrs == null)
            {
                ms_vEditorNodeTypes.Clear();
                ms_vInitCall.Clear();
                ms_vPopEnumTypeNames.Clear();
                ms_vPopEnumTypes.Clear();
                ms_vPopCompareOpTypes.Clear();

                foreach (Enum v in Enum.GetValues(typeof(AT.Runtime.ECompareOpType)))
                {
                    string strName = Enum.GetName(typeof(ECompareOpType), v);
                    FieldInfo fi = typeof(AT.Runtime.ECompareOpType).GetField(strName);
                    if (fi.IsDefined(typeof(Framework.DrawProps.DisableAttribute)))
                    {
                        continue;
                    }
                    ms_vPopCompareOpTypes.Add((ECompareOpType)v);
                }

                foreach (Enum v in Enum.GetValues(typeof(EVariableType)))
                {
                    string strName = Enum.GetName(typeof(EVariableType), v);
                    FieldInfo fi = typeof(EVariableType).GetField(strName);
                    if (fi.IsDefined(typeof(Framework.DrawProps.DisableAttribute)))
                    {
                        continue;
                    }
                    if (fi.IsDefined(typeof(Framework.DrawProps.DisplayAttribute)))
                    {
                        strName = fi.GetCustomAttribute<Framework.DrawProps.DisplayAttribute>().displayName;
                    }
                    if (fi.IsDefined(typeof(InspectorNameAttribute)))
                    {
                        strName = fi.GetCustomAttribute<InspectorNameAttribute>().displayName;
                    }
                    ms_vPopEnumTypes.Add((EVariableType)v);
                    ms_vPopEnumTypeNames.Add(strName);
                }
                ms_Attrs = new Dictionary<long, AgentTreeAttri>();
                ms_vLists = new List<AgentTreeAttri>();
                ms_vPops = new List<string>();

                Dictionary<long, System.Type> vGraphNodeTypes = new Dictionary<long, Type>();
                foreach (var ass in System.AppDomain.CurrentDomain.GetAssemblies())
                {
                    Type[] types = null;
                    try
                    {
                        types = ass.GetTypes();
                    }
                    catch (ReflectionTypeLoadException ex)
                    {
                        types = ex.Types; // 部分可用类型
                                          // 可选：输出警告
                        UnityEngine.Debug.LogWarning($"加载程序集 {ass.FullName} 时部分类型无法加载: {ex}");
                    }
                    catch (Exception ex)
                    {
                        UnityEngine.Debug.LogWarning($"加载程序集 {ass.FullName} 时发生异常: {ex}");
                        continue;
                    }
                    for (int i = 0; i < types.Length; ++i)
                    {
                        Type tp = types[i];
                        if (tp == null) continue;
                        if (tp.IsDefined(typeof(EditorBindNodeAttribute), false))
                        {
                            ms_vEditorNodeTypes[tp.GetCustomAttribute<EditorBindNodeAttribute>().nodeType] =tp;
                        }
                        if (tp.IsDefined(typeof(NodeBindAttribute), false))
                        {
                            NodeBindAttribute[] atTypeAttrs = (NodeBindAttribute[])tp.GetCustomAttributes<NodeBindAttribute>();
                            for (int j = 0; j < atTypeAttrs.Length; ++j)
                            {
                                long key = ((long)atTypeAttrs[j].actionType) << 32 | (long)atTypeAttrs[j].customType;
                                vGraphNodeTypes[key] = tp;
                            }
                        }
                        if (tp.IsDefined(typeof(ATEditorInitializeAttribute), false))
                        {
                            ATEditorInitializeAttribute initAttri = tp.GetCustomAttribute<ATEditorInitializeAttribute>();
                            if(!string.IsNullOrEmpty(initAttri.method))
                            {
                                var initCall = tp.GetMethod(initAttri.method, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
                                if (initCall != null)
                                    ms_vInitCall.Add(initCall);
                            }
                        }
                        if (tp.IsDefined(typeof(ATNodeAttribute), false))
                        {
                            var atNodeAttr = tp.GetCustomAttribute<ATNodeAttribute>();
                            AgentTreeAttri attr = new AgentTreeAttri();
                            attr.functionClassType = tp;
                            ATActionAttribute atAction = new ATActionAttribute(atNodeAttr.nodeName, false);
                            attr.actionAttr = atAction;
                            if (tp.IsDefined(typeof(ATIconAttribute))) attr.iconAttr = tp.GetCustomAttribute<ATIconAttribute>();
                            else attr.iconAttr = new ATIconAttribute(atNodeAttr.icon);
                            if (tp.IsDefined(typeof(ATColorAttribute))) attr.colorAttr = tp.GetCustomAttribute<ATColorAttribute>();

                            attr.cutsceneCusomtType = 0;
                            attr.isCutsceneCustomEvent = false;
                            attr.actionType = ATRtti.BuildHashCode(tp);
                            attr.nodeType = tp;
                            attr.displayName = atNodeAttr.nodeName;
                            attr.strQueueName = atNodeAttr.nodeName + tp.Name.ToString() + ED.EditorUtils.PinYin(atNodeAttr.nodeName);
                            attr.strMenuName = atNodeAttr.nodeName;
                            long key = ((long)attr.actionType) << 32 | (long)attr.cutsceneCusomtType;
                            if (ms_Attrs.TryGetValue(key, out var attrD))
                            {
                                Debug.LogError(tp.Name + " 存在重复定义:" + tp.FullName.ToString());
                            }
                            else
                            {
                                ms_Attrs.Add(key, attr);
                                ms_vLists.Add(attr);
                                ms_vPops.Add(attr.displayName);
                            }
                        }
                    }
                }
                foreach (var ass in System.AppDomain.CurrentDomain.GetAssemblies())
                {
                    Type[] types = null;
                    try
                    {
                        types = ass.GetTypes();
                    }
                    catch (ReflectionTypeLoadException ex)
                    {
                        types = ex.Types; // 部分可用类型
                                          // 可选：输出警告
                        UnityEngine.Debug.LogWarning($"加载程序集 {ass.FullName} 时部分类型无法加载: {ex}");
                    }
                    catch (Exception ex)
                    {
                        UnityEngine.Debug.LogWarning($"加载程序集 {ass.FullName} 时发生异常: {ex}");
                        continue;
                    }
                    for (int i = 0; i < types.Length; ++i)
                    {
                        Type tp = types[i];
                        if (tp == null) continue;
                        if(tp.IsDefined(typeof(ATClassAttribute)))
                        {
                            ATClassAttribute classAT = tp.GetCustomAttribute<ATClassAttribute>();
                            if (classAT.classType == null)
                                continue;
                            var iconAttr = tp.GetCustomAttribute<ATIconAttribute>(false);
                            var colorAttr = tp.GetCustomAttribute<ATColorAttribute>(false);
                            ATExportAttribute atExport = classAT.classType.GetCustomAttribute<ATExportAttribute>(false);
                            if(atExport!=null)
                            {
                                if(!string.IsNullOrEmpty(atExport.icon))
                                {
                                    iconAttr = new ATIconAttribute(atExport.icon);
                                }
                            }

                            var methods = tp.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                            foreach(var method in methods)
                            {
                                if (method.IsDefined(typeof(ATFunctionAttribute), false))
                                {
                                    ATFunctionAttribute atTypeAttr = method.GetCustomAttribute<ATFunctionAttribute>();
                                    AgentTreeAttri attr = new AgentTreeAttri();
                                    attr.functionClassType = tp;
                                    attr.functionAttr = atTypeAttr;
                                    attr.actionAttr = atTypeAttr.ToAction();
                                    if (method.IsDefined(typeof(ATIconAttribute))) attr.iconAttr = method.GetCustomAttribute<ATIconAttribute>();
                                    else attr.iconAttr = iconAttr;

                                    if (method.IsDefined(typeof(ATColorAttribute))) attr.colorAttr = method.GetCustomAttribute<ATColorAttribute>();
                                    else attr.colorAttr = colorAttr;

                                    attr.cutsceneCusomtType = 0;
                                    attr.isCutsceneCustomEvent = false;
                                    attr.actionType = atTypeAttr.guid;
                                    attr.displayName = atTypeAttr.DisplayName;
                                    attr.strQueueName = classAT.displayName + atTypeAttr.DisplayName + tp.Name.ToString() + method.Name + ED.EditorUtils.PinYin(atTypeAttr.DisplayName);
                                    attr.strMenuName = classAT.displayName + "/" + atTypeAttr.DisplayName;
                                    if (attr.actionAttr.isTask)
                                    {
                                        attr.actionAttr.hasInput = false;
                                    }
                                    if (method.IsDefined(typeof(ATFunctionArgvAttribute), false))
                                    {
                                        ATFunctionArgvAttribute[] argvs = (ATFunctionArgvAttribute[])method.GetCustomAttributes<ATFunctionArgvAttribute>();
                                        if (argvs != null)
                                        {
                                            for(int j =0; j < argvs.Length; ++j)
                                                attr.argvs.Add(argvs[j].ToArgv());
                                        }
                                    }
                                    if (method.IsDefined(typeof(ATFunctionReturnAttribute), false))
                                    {
                                        ATFunctionReturnAttribute[] returns = (ATFunctionReturnAttribute[])method.GetCustomAttributes<ATFunctionReturnAttribute>();
                                        if (returns != null)
                                        {
                                            for (int j = 0; j < returns.Length; ++j)
                                            {
                                                attr.returns.Add(new ArgvAttribute(returns[j].Name, returns[j].DisplayType));
                                            }
                                        }
                                    }
                                    long key = ((long)attr.actionType) << 32 | (long)attr.cutsceneCusomtType;
                                    if (vGraphNodeTypes.TryGetValue(key, out var graphNodeType))
                                    {
                                        attr.graphNodeType = graphNodeType;
                                    }
                                    else
                                    {
                                        if (attr.actionAttr.isTask)
                                            attr.graphNodeType = typeof(TaskGraphNode);
                                        else
                                            attr.graphNodeType = typeof(GraphNode);
                                    }
                                    if (ms_Attrs.TryGetValue(key, out var attrD))
                                    {
                                        Debug.LogError(tp.Name + " 存在重复定义:" + tp.FullName.ToString() + "." + method.Name);
                                    }
                                    else
                                    {
                                        foreach (var db in attr.argvs)
                                        {
                                            attr.popArgvs.Add(db.name);
                                        }
                                        foreach (var db in attr.returns)
                                        {
                                            attr.popReturns.Add(db.name);
                                        }
                                        ms_Attrs.Add(key, attr);
                                        ms_vLists.Add(attr);
                                        ms_vPops.Add(attr.displayName);
                                    }
                                }
                            }
                            
                        }
                        else if (tp.IsDefined(typeof(ATTypeAttribute), false))
                        {
                            ATTypeAttribute atTypeAttr = tp.GetCustomAttribute<ATTypeAttribute>();
                            foreach (Enum v in Enum.GetValues(tp))
                            {
                                string strName = Enum.GetName(tp, v);
                                FieldInfo fi = tp.GetField(strName);
                                if (fi.IsDefined(typeof(Framework.DrawProps.DisableAttribute)))
                                {
                                    continue;
                                }
                                if (!fi.IsDefined(typeof(ATActionAttribute)))
                                {
                                    continue;
                                }
                                var actionAttr = fi.GetCustomAttribute<ATActionAttribute>();
                                    int flagValue = Convert.ToInt32(v);
 
                                    strName = actionAttr.name;
                                AgentTreeAttri attr = new AgentTreeAttri();
                                attr.cutsceneCusomtType = 0;
                                attr.atTypeAttr = atTypeAttr;
                                attr.isCutsceneCustomEvent = false;
                                attr.actionAttr = actionAttr;
                                attr.iconAttr = fi.GetCustomAttribute<ATIconAttribute>();
                                attr.colorAttr = fi.GetCustomAttribute<ATColorAttribute>();
                                attr.actionType = flagValue;
                                attr.displayName = strName;
                                attr.strQueueName = strName + v.ToString() + ED.EditorUtils.PinYin(strName);
                                if (!string.IsNullOrEmpty(atTypeAttr.name))
                                {
                                    attr.strMenuName = atTypeAttr.name + "/" + attr.displayName;
                                    attr.strQueueName += "/" + atTypeAttr.name;
                                }
                                else
                                    attr.strMenuName = strName;
                                if (attr.actionAttr.isTask)
                                {
                                    attr.actionAttr.hasInput = false;
                                }
                                if (fi.IsDefined(typeof(ArgvAttribute), false))
                                {
                                    ArgvAttribute[] argvs = (ArgvAttribute[])fi.GetCustomAttributes<ArgvAttribute>();
                                    if (argvs != null)
                                    {
                                        if (attr.actionAttr.isTask)
                                            attr.returns.AddRange(argvs);
                                        else
                                            attr.argvs.AddRange(argvs);
                                    }
                                }
                                if (fi.IsDefined(typeof(ReturnAttribute), false))
                                {
                                    ReturnAttribute[] returns = (ReturnAttribute[])fi.GetCustomAttributes<ReturnAttribute>();
                                    if (returns != null)
                                    {
                                        for(int j =0; j < returns.Length; ++j)
                                        {
                                            attr.returns.Add(new ArgvAttribute(returns[j].name, returns[j].argvType));
                                        }
                                    }
                                }
                                if (fi.IsDefined(typeof(LinkAttribute), false))
                                {
                                    LinkAttribute[] returns = (LinkAttribute[])fi.GetCustomAttributes<LinkAttribute>();
                                    if (returns != null)
                                    {
                                        for (int j = 0; j < returns.Length; ++j)
                                        {
                                            ArgvAttribute arvgs = new ArgvAttribute(returns[j].name, typeof(int), false);
                                            arvgs.tips = returns[j].tips;
                                            attr.linkAttributes[arvgs] = returns[j];
                                            attr.returns.Add(arvgs);
                                        }
                                    }
                                }
                                long key = ((long)attr.actionType) << 32 | (long)attr.cutsceneCusomtType;
                                if (vGraphNodeTypes.TryGetValue(key, out var graphNodeType))
                                {
                                    attr.graphNodeType = graphNodeType;
                                }
                                else
                                {
                                    if(attr.actionAttr.isTask)
                                        attr.graphNodeType = typeof(TaskGraphNode);
                                    else
                                        attr.graphNodeType = typeof(GraphNode);
                                }
                                if (ms_Attrs.TryGetValue(key, out var attrD))
                                {
                                    Debug.LogError(tp.Name + " 存在重复定义:" + tp.Name + "." + v.ToString() + "=" + flagValue);
                                }
                                else
                                {
                                    foreach(var db in attr.argvs)
                                    {
                                        attr.popArgvs.Add(db.name);
                                    }
                                    foreach (var db in attr.returns)
                                    {
                                        attr.popReturns.Add(db.name);
                                    }
                                    ms_Attrs.Add(key, attr);
                                    ms_vLists.Add(attr);
                                    ms_vPops.Add(attr.displayName);
                                }
                            }
                        }
                    }
                }
                /*
                //! 添加custom data 自定义的事件
                var eventLists = CustomAgentUtil.GetEventList();
                foreach ( var db in eventLists)
                {
                    AgentTreeAttri attr = new AgentTreeAttri();
                    attr.isCutsceneCustomEvent = true;
                    attr.cutsceneCusomtType = (int)db.customType;
                    attr.actionAttr = new ATActionAttribute(db.name, false);
                    attr.actionType = (int)EActionType.eCutsceneCustomEvent;
                    attr.displayName = db.name;
                    attr.strMenuName = "自定义/" + db.name;
                    attr.strQueueName = db.name;
                    if (db.inputVariables!=null)
                    {
                        for(int j =0; j < db.inputVariables.Length; ++j)
                        {
                            var input = db.inputVariables[j];
                            attr.argvs.Add(new ArgvAttribute(input.name, VariableUtil.GetVariableCsType(input.type), input.canEdit, input.defaultValue));
                        }
                    }
                    if (db.outputVariables != null)
                    {
                        for (int j = 0; j < db.outputVariables.Length; ++j)
                        {
                            var input = db.outputVariables[j];
                            attr.returns.Add(new ArgvAttribute(input.name, VariableUtil.GetVariableCsType(input.type), input.canEdit, input.defaultValue));
                        }
                    }
                    long key = ((long)attr.actionType) << 32 | (long)attr.cutsceneCusomtType;
                    if (ms_Attrs.TryGetValue(key, out var attrD))
                    {
                        Debug.LogError(db.name + " 存在重复定义:" + attr.cutsceneCusomtType);
                    }
                    else
                    {
                        foreach (var temp in attr.argvs)
                        {
                            attr.popArgvs.Add(temp.name);
                        }
                        foreach (var temp in attr.returns)
                        {
                            attr.popReturns.Add(temp.name);
                        }
                        ms_Attrs.Add(key, attr);
                        ms_vLists.Add(attr);
                        ms_vPops.Add(attr.displayName);
                    }
                }*/

                ms_SearchMenuRoot = new MenuTreeNode("添加节点");
                foreach (var item in AgentTreeUtil.GetAttrs())
                {
                    if (!item.actionAttr.bShow)
                        continue;

                    string menuPath = item.strMenuName ?? item.displayName;
                    var pathParts = menuPath.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);

                    // 加载图标
                    Texture icon = null;
                    if (item.iconAttr != null)
                        icon = AgentTreeUtil.LoadIcon(item.iconAttr.name);

                    // 将菜单项添加到树形结构中
                    AddToTreeNode(ms_SearchMenuRoot, pathParts, item, icon);
                }
                SortMenuTree(ms_SearchMenuRoot);
            }
        }
        //-----------------------------------------------------
        internal static void EditorInit()
        {
            Init();
            bool bHasTips = false;
            foreach (var db in ms_vInitCall)
            {
                db.Invoke(null, null);
                if (db.DeclaringType.Name.Contains("ATRegisterInternalHandler"))
                    continue;
                bHasTips = true;
            }
            if (!bHasTips)
                return;
            UnityEngine.Debug.LogWarning("请注意，以下AT句柄务必需要在游戏框架中调用，编辑器模式下自动调用！！！");
            UnityEngine.Debug.LogWarning("请注意，以下AT句柄务必需要在游戏框架中调用，编辑器模式下自动调用！！！");
            UnityEngine.Debug.LogWarning("请注意，以下AT句柄务必需要在游戏框架中调用，编辑器模式下自动调用！！！");
            foreach (var db in ms_vInitCall)
            {
                if (db.DeclaringType.Name.Contains("ATRegisterInternalHandler"))
                    continue;
                UnityEngine.Debug.LogWarning(db.DeclaringType.FullName.ToString() + "." + db.Name);
            }
        }
        //-----------------------------------------------------
        internal static Type GetEditorNodeType(BaseNode pNode)
        {
            Init();
            int customType = 0;
            if (pNode is CustomEvent)
            {
                customType = ((CustomEvent)pNode).eventType;
            }
            var attri = AgentTreeUtil.GetAttri(pNode.type, customType);
            if (attri != null && attri.graphNodeType != null) return attri.graphNodeType;
            ms_vEditorNodeTypes.TryGetValue(pNode.GetType(), out var editorType);
            return editorType;
        }
        //-----------------------------------------------------
        public static List<string> GetPops()
        {
            Init();
            return ms_vPops;
        }
        //-----------------------------------------------------
        public static List<AgentTreeAttri> GetAttrs()
        {
            Init();
            return ms_vLists;
        }
        //-----------------------------------------------------
        internal static MenuTreeNode GetMenuRoot()
        {
            Init();
            return ms_SearchMenuRoot;
        }
        //-----------------------------------------------------
        public static AgentTreeAttri GetAttri(int type, int customType)
        {
            Init();
            long key = ((long)type) << 32 | (long)customType;
            if (ms_Attrs.TryGetValue(key, out var tempAttr))
                return tempAttr;
            return null;
        }
        //-----------------------------------------------------
        internal static AgentTreeAttri GetAttri(BaseNode bindNode)
        {
            if (bindNode == null) return null;
            int customType = 0;
            if (bindNode is AT.Runtime.CustomEvent)
            {
                customType = ((AT.Runtime.CustomEvent)bindNode).eventType;
            }
            return AgentTreeUtil.GetAttri(bindNode.type, customType);
        }
        //-----------------------------------------------------
        public static List<ECompareOpType> GetPopCompareOpTypes()
        {
            Init();
            return ms_vPopCompareOpTypes;
        }
        //-----------------------------------------------------
        public static List<EVariableType> GetPopEnumTypes()
        {
            Init();
            return ms_vPopEnumTypes;
        }
        //-----------------------------------------------------
        public static List<string> GetPopEnumTypeNames()
        {
            return ms_vPopEnumTypeNames;
        }
        //-----------------------------------------------------
        // 将菜单项添加到树形结构
        static void AddToTreeNode(MenuTreeNode parent, string[] pathParts, AgentTreeAttri userData, Texture icon)
        {
            if (pathParts.Length == 0)
                return;

            // 查找当前路径部分对应的子节点
            string currentName = pathParts[0];
            MenuTreeNode currentNode = parent.Children.Find(node => node.Name == currentName);

            if (currentNode == null)
            {
                // 创建新节点
                currentNode = new MenuTreeNode(currentName);
                parent.Children.Add(currentNode);
            }

            if (pathParts.Length == 1)
            {
                currentNode.Icon = icon;
                // 这是叶节点，存储用户数据
                currentNode.UserData = userData;
                currentNode.Tips = userData.ToString(); // 假设userData有ToString()方法
            }
            else
            {
                // 递归处理剩余路径
                string lastPath = "";
                for(int i =0; i < pathParts.Length;++i)
                {
                    lastPath += pathParts[i];
                    if (i < pathParts.Length - 1) lastPath += "/";
                }
                string[] remainingPath = new string[pathParts.Length - 1];
                Array.Copy(pathParts, 1, remainingPath, 0, remainingPath.Length);
                AddToTreeNode(currentNode, remainingPath, userData, icon);
            }
        }
        //-----------------------------------------------------
        // 对菜单树进行排序
        static void SortMenuTree(MenuTreeNode node)
        {
            // 先对子节点进行排序
            node.Children.Sort((a, b) =>
            {
                // 首先按是否为叶节点排序（非叶节点在前）
                bool aIsLeaf = a.UserData != null;
                bool bIsLeaf = b.UserData != null;
                if (aIsLeaf != bIsLeaf)
                    return aIsLeaf ? 1 : -1;

                // 然后按名称排序
                return a.Name.CompareTo(b.Name);
            });

            // 递归排序每个子节点
            foreach (var child in node.Children)
            {
                SortMenuTree(child);
            }
        }
    }
}

#endif