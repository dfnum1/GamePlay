/********************************************************************
生成日期:	06:30:2025
类    名: 	IconUtils
作    者:	HappLI
描    述:	
*********************************************************************/
#if UNITY_EDITOR
using Framework.AT.Runtime;
using Framework.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Framework.AT.Editor
{
    public class ExportAutoCode
    {
        public class ExportInfo
        {
            public class ExportMemberInfo
            {
                public int guid;
                public string memberName;
                public string displayName;
                public MemberInfo info;
                public ATMethodAttribute attr;
                public ATFieldAttribute fieldAttr;
            }

            public string exportPath;

            public int guid = 0;
            public Type type;
            public ATExportAttribute exportAttr;
            public Dictionary<string, int> methodCount = new Dictionary<string, int>();
            public Dictionary<string, ExportMemberInfo> methods = new Dictionary<string, ExportMemberInfo>();
        }
        static Dictionary<string, ExportInfo> ms_vExports = new Dictionary<string, ExportInfo>();
        static HashSet<System.Type> ms_vRefTypes = new HashSet<Type>();
        static string EXPORT_PATH = "../GamePlay/Generators/";
        //-----------------------------------------------------
        [MenuItem("GamePlay/编译蓝图脚本")]
        public static void ExportInternalAT()
        {
            ExportATMothed(true);
        }
        //-----------------------------------------------------
        static void ExportATMothed(bool bInternal)
        {
            ms_vExports.Clear();
            ms_vRefTypes.Clear();
            ms_vRefTypes.Add(typeof(IUserData));
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
                    ATExportAttribute exportAttr = null;
                 //   if(bInternal)
                 //   {
                 //       if (tp.IsDefined(typeof(ATInteralExportAttribute), false)) exportAttr = tp.GetCustomAttribute<ATInteralExportAttribute>();
                 //   }
                 //   else
                    {
                        if (tp.IsDefined(typeof(ATExportAttribute), false)) exportAttr = tp.GetCustomAttribute<ATExportAttribute>();
                    }

                    if(IsUserDataType(tp))
                    {
                        ms_vRefTypes.Add(tp);
                    }

                    if (exportAttr!=null)
                    {
                        string typeName = GetTypeName(tp);
                        MethodInfo[] meths = types[i].GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
                        for (int m = 0; m < meths.Length; ++m)
                        {
                            if (meths[m].IsDefined(typeof(ATMethodAttribute), false))
                            {
                                if (!CheckMethodCanExport(meths[m]))
                                {
                                    Debug.LogWarning(typeName + " Method " + meths[m].Name + " can not export!");
                                    continue;
                                }
                                ATMethodAttribute attr = (ATMethodAttribute)meths[m].GetCustomAttribute(typeof(ATMethodAttribute));
                                ExportInfo exportData;
                                if (!ms_vExports.TryGetValue(typeName, out exportData))
                                {
                                    exportData = new ExportInfo();
                                    exportData.guid = exportAttr.guid;
                                    if (exportData.guid == 0)
                                    {
                                        exportData.guid = BuildHashCode(tp);
                                    }
                                    exportData.exportPath = EXPORT_PATH + tp.FullName.Replace("+", "_").Replace(".", "_") + ".cs";
                                    exportData.type = tp;
                                    exportData.exportAttr = exportAttr;
                                    ms_vExports.Add(typeName, exportData);
                                }
                                if (!exportData.methodCount.TryGetValue(meths[m].Name, out var methodCnt))
                                {
                                    methodCnt = 0;
                                }
                                methodCnt++;
                                exportData.methodCount[meths[m].Name] = methodCnt;

                                ExportInfo.ExportMemberInfo exportMth = new ExportInfo.ExportMemberInfo();
                                exportMth.info = meths[m];
                                exportMth.attr = attr;
                                exportMth.displayName = attr.method;
                                if (methodCnt <= 1) exportMth.memberName = meths[m].Name;
                                else exportMth.memberName = meths[m].Name + "_" + (methodCnt - 1);
                                exportMth.guid = ATRtti.BuildHashCode(typeName + ":" + exportMth.memberName);
                                if (string.IsNullOrEmpty(exportMth.displayName))
                                    exportMth.displayName = exportMth.memberName;
                                exportData.methods[exportMth.memberName] = exportMth;
                            }
                        }
                        FieldInfo[] fields = types[i].GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
                        for (int m = 0; m < fields.Length; ++m)
                        {
                            if (fields[m].IsDefined(typeof(ATFieldAttribute), false))
                            {
                                if (!CheckCanExport(fields[m]))
                                {
                                    Debug.LogWarning(typeName + " Filed " + fields[m].Name + " can not export!");
                                    continue;
                                }
                                ATFieldAttribute attr = (ATFieldAttribute)fields[m].GetCustomAttribute(typeof(ATFieldAttribute));
                                if(!attr.bGet && !attr.bSet)
                                {
                                    continue;
                                }
                                ExportInfo exportData;
                                if (!ms_vExports.TryGetValue(typeName, out exportData))
                                {
                                    exportData = new ExportInfo();
                                    exportData.guid = exportAttr.guid;
                                    if (exportData.guid == 0)
                                    {
                                        exportData.guid = BuildHashCode(tp);
                                    }
                                    exportData.exportPath = EXPORT_PATH + tp.FullName.Replace("+", "_").Replace(".", "_") + ".cs";
                                    exportData.type = tp;
                                    exportData.exportAttr = exportAttr;
                                    ms_vExports.Add(typeName, exportData);
                                }
                                if (!exportData.methodCount.TryGetValue(fields[m].Name, out var methodCnt))
                                {
                                    methodCnt = 0;
                                }
                                methodCnt++;
                                exportData.methodCount[fields[m].Name] = methodCnt;

                                ExportInfo.ExportMemberInfo exportMth = new ExportInfo.ExportMemberInfo();
                                exportMth.info = fields[m];
                                exportMth.fieldAttr = attr;
                                exportMth.attr = null;
                                exportMth.displayName = attr.method;
                                if (methodCnt <= 1) exportMth.memberName = fields[m].Name;
                                else exportMth.memberName = fields[m].Name + "_" + (methodCnt - 1);
                                exportMth.guid = 0;
                                if (string.IsNullOrEmpty(exportMth.displayName))
                                    exportMth.displayName = exportMth.memberName;
                                exportData.methods[exportMth.memberName] = exportMth;
                            }
                        }
                    }
                }
            }

            string epxortRoot = Application.dataPath + "/" + EXPORT_PATH;
            if(Directory.Exists(epxortRoot))
            {
                Directory.Delete(epxortRoot, true);
            }

            foreach (var db in ms_vExports)
            {
                if(bInternal)
                {
                    if (!(db.Value.exportAttr is ATInteralExportAttribute))
                        continue;
                }
                ExportCode(db.Value);
            }
            ExportRegisterCode(bInternal);
        }
        //-----------------------------------------------------
        static void ExportCode(ExportInfo export)
        {
            string typeClassName = export.type.FullName.Replace("+", "_").Replace(".", "_");
            string oriTypeClassName = export.type.Name.Replace("+", ".");
            StringBuilder methodCode = new StringBuilder();
            StringBuilder code = new StringBuilder();
            code.AppendLine("//auto generated");
            code.AppendLine("using Framework.AT.Runtime;");
            code.AppendLine("namespace " + export.type.Namespace.Replace("+", "."));
            code.AppendLine("{");
            code.AppendLine("#if UNITY_EDITOR");
            code.AppendLine($"\t[ATClass(typeof({GetTypeName(export.type)}),\"{export.exportAttr.nodeName}\")]");
            code.AppendLine("#endif");
            code.AppendLine($"\tpublic class {typeClassName}");
            code.AppendLine("\t{");
            code.AppendLine("##FUNCTION##");
            if(!export.type.IsSubclassOf(typeof(AModule)))
            {
                code.AppendLine("\t\tstatic bool CheckUserClassPointer(ref VariableUserData pUserClass, AgentTree pAgentTree, BaseNode pNode)");
                code.AppendLine("\t\t{");
                code.AppendLine("\t\t\tif(pUserClass.pPointer == null) pUserClass.pPointer = pAgentTree.GetOwnerClass(pUserClass.value);");
                code.AppendLine("\t\t\tif(pUserClass.pPointer == null) return false;");
                code.AppendLine("\t\t\treturn true;");
                code.AppendLine("\t\t}");
            }

            code.AppendLine("\t\tpublic static bool DoAction(VariableUserData pUserClass, AgentTree pAgentTree, BaseNode pNode)");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tint actionType = pNode.type;");
            code.AppendLine("\t\t\tswitch(actionType)");
            code.AppendLine("\t\t\t{");
            foreach (var db in export.methods)
            {
                if(db.Value.info is MethodInfo)
                {
                    ExportMethod(code, methodCode, export,db.Value);
                }
                else if (db.Value.info is FieldInfo)
                {
                    ExportField(code, methodCode, export, db.Value);
                }
            }
            code.AppendLine("\t\t\t}");
            code.AppendLine("\t\t\treturn true;");
            code.AppendLine("\t\t}");
            code.AppendLine("\t}");
            code.AppendLine("}");

            code.Replace("##FUNCTION##", methodCode.ToString());
            string fullPath = Application.dataPath + "/" + export.exportPath;
            if (!Directory.Exists(Path.GetDirectoryName(fullPath)))
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate);
            StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.UTF8);
            fs.Position = 0;
            fs.SetLength(0);
            writer.Write(code);
            writer.Close();
        }
        //-----------------------------------------------------
        static void ExportMethod(StringBuilder code, StringBuilder methodCode, ExportInfo export, ExportInfo.ExportMemberInfo info)
        {
            code.AppendLine("\t\t\tcase " + info.guid + ":" + "//" + info.info.Name);
            code.AppendLine("\t\t\t{");
            bool isModule = export.type.IsSubclassOf(typeof(AModule));
            MethodInfo method = info.info as MethodInfo;
            string typeClassName = export.type.FullName.Replace("+", "_").Replace(".", "_");
            string oriTypeClassName = export.type.Name.Replace("+", ".");
            Type retType = null;
            bool hasReturn = false;
            if (method.ReturnType != null && method.ReturnType != typeof(void))
            {
                retType = ConvertTypeToATType(method.ReturnType);
                hasReturn = true;
            }

            Dictionary<string, string> vDraws = new Dictionary<string, string>();
            if (method.IsDefined(typeof(ATArgvDrawerAttribute)))
            {
                var drawAttrs = method.GetCustomAttributes<ATArgvDrawerAttribute>();
                foreach (var db in drawAttrs)
                    vDraws[db.argv] = db.drawerMethod;
            }

            string functionName = "AT_" + info.memberName;

            string paramOutAttrs = "";
            string outParamSetPorts = "";

            int paramOutIndex = hasReturn?1:0;
            string functionCall = "";
            string functionAttributes = $"\t\t[ATFunction({info.guid},\"{info.displayName}\",typeof({GetTypeName(export.type)}),false)]\r\n";
            string functionHead = "\t\tstatic bool " + functionName + "(";
            if (method.IsStatic)
            {
                string callArgv = "";
                for (int i = 0; i < method.GetParameters().Length; ++i)
                {
                    var parm = method.GetParameters()[i];
                    string paramName = parm.Name;
                    if (info.attr.argvNames != null && i < info.attr.argvNames.Length)
                        paramName = info.attr.argvNames[i];
                    var paramType = ConvertTypeToATType(parm.ParameterType);

                    vDraws.TryGetValue(parm.Name, out var drawMethod);

                    if (parm.IsIn || parm.ParameterType.IsByRef) callArgv += "ref ";
                    else if (parm.IsOut) callArgv += "out var ";
                    callArgv += $"{parm.Name}";

                    if(!parm.IsOut)
                    {
                        if (!functionHead.EndsWith("(")) functionHead += ",";
                        functionHead += $"{GetTypeName(parm.ParameterType)} {parm.Name}";
                        if (i < method.GetParameters().Length - 1)
                        {
                            callArgv += ",";
                        }
                    }
                    if (i < method.GetParameters().Length - 1)
                    {
                        callArgv += ",";
                    }

                    if(parm.IsOut || parm.IsIn || parm.ParameterType.IsByRef)
                    {
                        if(!string.IsNullOrEmpty(drawMethod))
                            paramOutAttrs += $"\t\t[ATFunctionReturn(typeof({GetTypeName(paramType)}), \"{paramName}\", null,typeof({GetTypeName(parm.ParameterType)}),drawMethod:\"{drawMethod}\")]\r\n";
                        else
                            paramOutAttrs += $"\t\t[ATFunctionReturn(typeof({GetTypeName(paramType)}), \"{paramName}\", null,typeof({GetTypeName(parm.ParameterType)}))]\r\n";

                        string castLabel = "";
                        if (parm.ParameterType.IsEnum)
                        {
                            castLabel = "(int)";
                        }
                        outParamSetPorts += $"\t\t\tpAgentTree.{ConvertTypeToATSetOutPortFunction(parm.ParameterType)}(pNode, {paramOutIndex}, {castLabel}{parm.Name});";
                        paramOutIndex++;
                    }
                    if (!parm.IsOut)
                    {
                        if(!string.IsNullOrEmpty(drawMethod))
                            functionAttributes += $"\t\t[ATFunctionArgv(typeof({GetTypeName(paramType)}),\"{paramName}\",false, null,typeof({GetTypeName(parm.ParameterType)}),drawMethod:\"{drawMethod}\")]\r\n";
                        else
                            functionAttributes += $"\t\t[ATFunctionArgv(typeof({GetTypeName(paramType)}),\"{paramName}\",false, null,typeof({GetTypeName(parm.ParameterType)}))]\r\n";
                    }
                }
                functionCall += $"\t\t\t";
                if (hasReturn)
                {
                    string castLabel = "";
                    if (method.ReturnType.IsEnum)
                    {
                        castLabel = "(int)";
                    }

                    functionCall += $"pAgentTree.{ConvertTypeToATSetOutPortFunction(method.ReturnType)}(pNode, {0}, {castLabel}{GetTypeName(export.type)}.{method.Name}({callArgv}));";
                }
                else
                    functionCall += $"{GetTypeName(export.type)}.{method.Name}({callArgv});";
            }
            else
            {
                functionAttributes += $"\t\t[ATFunctionArgv(typeof({typeof(VariableUserData).Name}),\"{export.type.Name}\",false, null,typeof({GetTypeName(export.type)}))]\r\n";

                string callArgv = "";
                functionHead += $"{export.type.Name} pPointerThis";
                for (int i = 0; i < method.GetParameters().Length; ++i)
                {
                    var parm = method.GetParameters()[i];
                    string paramName = parm.Name;
                    if (info.attr.argvNames != null && i < info.attr.argvNames.Length)
                        paramName = info.attr.argvNames[i];
                    var paramType = ConvertTypeToATType(parm.ParameterType);

                    if (parm.IsIn || parm.ParameterType.IsByRef) callArgv += "ref ";
                    else if (parm.IsOut) callArgv += "out var ";
                    callArgv += $"{parm.Name}";

                    vDraws.TryGetValue(parm.Name, out var drawMethod);

                    if (parm.IsOut || parm.IsIn || parm.ParameterType.IsByRef)
                    {
                        if (!string.IsNullOrEmpty(drawMethod))
                            paramOutAttrs += $"\t\t[ATFunctionReturn(typeof({GetTypeName(parm.ParameterType)}), \"{paramName}\", null,typeof({GetTypeName(parm.ParameterType)}),drawMethod:\"{drawMethod}\")]\r\n";
                        else
                            paramOutAttrs += $"\t\t[ATFunctionReturn(typeof({GetTypeName(parm.ParameterType)}), \"{paramName}\", null,typeof({GetTypeName(parm.ParameterType)}))]\r\n";

                        string castLabel = "";
                        if (parm.ParameterType.IsEnum)
                        {
                            castLabel = "(int)";
                        }
                        outParamSetPorts += $"\t\t\tpAgentTree.{ConvertTypeToATSetOutPortFunction(parm.ParameterType)}(pNode, {paramOutIndex}, {castLabel}{parm.Name});";
                        paramOutIndex++;
                    }
                    if(!parm.IsOut)
                    {
                        functionHead += $",{GetTypeName(parm.ParameterType)} {parm.Name}";
                        if (!string.IsNullOrEmpty(drawMethod))
                            functionAttributes += $"\t\t[ATFunctionArgv(typeof({GetTypeName(paramType)}),\"{paramName}\",false, null,typeof({GetTypeName(parm.ParameterType)}),drawMethod:\"{drawMethod}\")]\r\n";
                        else
                            functionAttributes += $"\t\t[ATFunctionArgv(typeof({GetTypeName(paramType)}),\"{paramName}\",false, null,typeof({GetTypeName(parm.ParameterType)}))]\r\n";
                    }
                    if (i < method.GetParameters().Length - 1)
                    {
                        callArgv += ",";
                    }
                }
                functionCall += $"\t\t\t";
                if (hasReturn)
                {
                    string castLabel = "";
                    if (method.ReturnType.IsEnum)
                    {
                        castLabel = "(int)";
                    }
                    functionCall += $"pAgentTree.{ConvertTypeToATSetOutPortFunction(method.ReturnType)}(pNode, {0}, {castLabel}pPointerThis.{method.Name}({callArgv}));";
                }
                else
                    functionCall += $"pPointerThis.{method.Name}({callArgv});";
            }
            if (hasReturn || paramOutAttrs.Length > 0)
            {
                if (!functionHead.EndsWith("(")) functionHead += ",";
                functionHead += $"AgentTree pAgentTree, BaseNode pNode";
            }
            if (hasReturn)
            {
                functionAttributes += $"\t\t[ATFunctionReturn(typeof({GetTypeName(retType)}), \"pReturn\", null,typeof({GetTypeName(method.ReturnType)}))]\r\n";
            }
            if (paramOutAttrs.Length>0)
            {
                functionAttributes += paramOutAttrs;
            }
            functionHead += ")";
            if (functionAttributes.EndsWith("\r\n"))
                functionAttributes = functionAttributes.Substring(0, functionAttributes.Length - "\r\n".Length);

            if(outParamSetPorts.Length>0)
            {
                functionCall += $"\r\n";
                functionCall += outParamSetPorts;
            }

            methodCode.AppendLine("#if UNITY_EDITOR");
            methodCode.AppendLine(functionAttributes);
            methodCode.AppendLine("#endif");
            methodCode.AppendLine(functionHead);
            methodCode.AppendLine("\t\t{");
            methodCode.AppendLine(functionCall);
            methodCode.AppendLine("\t\t\treturn true;");
            methodCode.AppendLine("\t\t}");


            if (!method.IsStatic && !isModule)
            {
                //  code.AppendLine("\t\t\t\tif(pUserClass == null) return true;");
                code.AppendLine("\t\t\t\tif(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;");
                code.AppendLine($"\t\t\t\tif(pNode.GetInportCount() <= {method.GetParameters().Length}) return true;");
                code.AppendLine($"\t\t\t\tif(!(pUserClass.pPointer is {oriTypeClassName})) return true;");
            }
            else
                code.AppendLine($"\t\t\t\tif(pNode.GetInportCount() <= {method.GetParameters().Length}) return true;");
            if(isModule)
            {
                code.AppendLine($"\t\t\t\t{export.type} pModulePointer = pAgentTree.GetModule<{export.type}>();");
                code.AppendLine($"\t\t\t\tif(pModulePointer == null) return true;");
            }

            bool bAddDot = true;
            if (isModule)
            {
                code.Append($"\t\t\t\treturn {functionName}(pModulePointer");
                bAddDot = false;
            }
            else if (!method.IsStatic)
            {
                code.Append($"\t\t\t\treturn {functionName}(({oriTypeClassName})pUserClass.pPointer");
                bAddDot = false;
            }
            else
            {
                code.Append($"\t\t\t\treturn {functionName}(");
            }
            int paramIndex = 1;
            for (int i = 0; i < method.GetParameters().Length; ++i)
            {
                var parm = method.GetParameters()[i];
                if (parm.IsOut)
                    continue;
                var paramType = ConvertTypeToATType(parm.ParameterType);
                string castLabel = "";
                if (parm.ParameterType.IsEnum) castLabel = "(" + GetTypeName(parm.ParameterType) + ")";

                if (!bAddDot)
                {
                    code.Append(",");
                    bAddDot = true;
                }
                code.Append($"{castLabel}pAgentTree.{ConvertTypeToATGetInPortFunction(parm.ParameterType)}(pNode,{paramIndex})");
                if (i < method.GetParameters().Length - 1) code.Append(",");
                paramIndex++;
            }
            if (hasReturn)
            {
                if (!bAddDot)
                {
                    code.Append(",");
                    bAddDot = true;
                }
                if (paramIndex > 1) code.Append($", ");
                code.Append($"pAgentTree, pNode");
            }

            code.AppendLine($");");

            code.AppendLine("\t\t\t}");
        }
        //-----------------------------------------------------
        static void ExportField(StringBuilder code, StringBuilder methodCode, ExportInfo export, ExportInfo.ExportMemberInfo info)
        {
            FieldInfo field = info.info as FieldInfo;
            string typeClassName = export.type.FullName.Replace("+", "_").Replace(".", "_");
            string oriTypeClassName = export.type.Name.Replace("+", ".");
            bool isModule = export.type.IsSubclassOf(typeof(AModule));

            var paramType = ConvertTypeToATType(field.FieldType);

            string functionNameGet = "AT_Get_" + info.memberName;
            string functionNameSet = "AT_Set_" + info.memberName;
            int getGuid = ATRtti.BuildHashCode(GetTypeName(export.type) + ".Get_" + info.memberName );
            int setGuid = ATRtti.BuildHashCode(GetTypeName(export.type) + ".Set_" + info.memberName );

            string castLabel = "";
            if (field.FieldType.IsEnum)
            {
                castLabel = "(int)";
            }

            string drawMethod = null;
            if(field.IsDefined(typeof(ATArgvDrawerAttribute)))
            {
                var methodAttr = field.GetCustomAttribute<ATArgvDrawerAttribute>();
                drawMethod = methodAttr.drawerMethod;
            }

            string functionCallGet = "";
            string functionCallSet = "";
            string functionAttributesGet = $"\t\t[ATFunction({getGuid},\"{info.displayName}\",typeof({GetTypeName(export.type)}),false)]\r\n";
            string functionAttributesSet = $"\t\t[ATFunction({setGuid},\"{info.displayName}\",typeof({GetTypeName(export.type)}),false)]\r\n";
            string functionHeadGet = "\t\tstatic bool " + functionNameGet + "(";
            string functionHeadSet = "\t\tstatic bool " + functionNameSet + "(";
            if (field.IsStatic)
            {
                if (string.IsNullOrEmpty(drawMethod))
                {
                    functionAttributesGet += $"\t\t[ATFunctionArgv(typeof({GetTypeName(paramType)}),\"{field.Name}\",false, null,typeof({GetTypeName(field.FieldType)}))]\r\n";
                    functionAttributesSet += $"\t\t[ATFunctionArgv(typeof({GetTypeName(paramType)}),\"{field.Name}\",false, null,typeof({GetTypeName(field.FieldType)}))]\r\n";
                }
                else
                {
                    functionAttributesGet += $"\t\t[ATFunctionArgv(typeof({GetTypeName(paramType)}),\"{field.Name}\",false, null,typeof({GetTypeName(field.FieldType)}),drawMethod:\"{drawMethod}\")]\r\n";
                    functionAttributesSet += $"\t\t[ATFunctionArgv(typeof({GetTypeName(paramType)}),\"{field.Name}\",false, null,typeof({GetTypeName(field.FieldType)}),drawMethod:\"{drawMethod}\")]\r\n";
                }

                functionCallGet += $"\t\t\tpAgentTree.{ConvertTypeToATSetOutPortFunction(field.FieldType)}(pNode, 0, {castLabel}{GetTypeName(export.type)}.{field.Name});";
                functionCallSet += $"\t\t\t{GetTypeName(export.type)}.{field.Name}={field.Name};";

                functionHeadGet += $"AgentTree pAgentTree, BaseNode pNode";
                functionHeadSet += $"{GetTypeName(field.FieldType)} {field.Name}";
            }
            else
            {
                functionAttributesGet += $"\t\t[ATFunctionArgv(typeof({typeof(VariableUserData).Name}),\"{export.type.Name}\",false, null,typeof({GetTypeName(field.FieldType)}))]\r\n";
                functionAttributesSet += $"\t\t[ATFunctionArgv(typeof({typeof(VariableUserData).Name}),\"{export.type.Name}\",false, null,typeof({GetTypeName(field.FieldType)}))]\r\n";

                if (string.IsNullOrEmpty(drawMethod))
                {
                    functionAttributesGet += $"\t\t[ATFunctionArgv(typeof({GetTypeName(paramType)}),\"{field.Name}\",false, null,typeof({GetTypeName(field.FieldType)}))]\r\n";
                    functionAttributesSet += $"\t\t[ATFunctionArgv(typeof({GetTypeName(paramType)}),\"{field.Name}\",false, null,typeof({GetTypeName(field.FieldType)}))]\r\n";
                }
                else
                {
                    functionAttributesGet += $"\t\t[ATFunctionArgv(typeof({GetTypeName(paramType)}),\"{field.Name}\",false, null,typeof({GetTypeName(field.FieldType)}),drawMethod:\"{drawMethod}\")]\r\n";
                    functionAttributesSet += $"\t\t[ATFunctionArgv(typeof({GetTypeName(paramType)}),\"{field.Name}\",false, null,typeof({GetTypeName(field.FieldType)}),drawMethod:\"{drawMethod}\")]\r\n";
                }

                functionCallGet += $"\t\t\tpAgentTree.{ConvertTypeToATSetOutPortFunction(field.FieldType)}(pNode, 0, {castLabel}pPointerThis.{field.Name});";
                functionCallSet += $"\t\t\tpPointerThis.{field.Name}={field.Name};";

                functionHeadGet += $"{oriTypeClassName} pPointerThis, AgentTree pAgentTree, BaseNode pNode";
                functionHeadSet += $"{oriTypeClassName} pPointerThis,{GetTypeName(field.FieldType)} {field.Name}";
            }

            functionHeadGet += ")";
            functionHeadSet += ")";
            if (functionAttributesGet.EndsWith("\r\n"))
                functionAttributesGet = functionAttributesGet.Substring(0, functionAttributesGet.Length - "\r\n".Length);

            if (functionAttributesSet.EndsWith("\r\n"))
                functionAttributesSet = functionAttributesSet.Substring(0, functionAttributesSet.Length - "\r\n".Length);

            if(info.fieldAttr.bGet)
            {
                methodCode.AppendLine("#if UNITY_EDITOR");
                methodCode.AppendLine(functionAttributesGet);
                methodCode.AppendLine("#endif");
                methodCode.AppendLine(functionHeadGet);
                methodCode.AppendLine("\t\t{");
                methodCode.AppendLine(functionCallGet);
                methodCode.AppendLine("\t\t\treturn true;");
                methodCode.AppendLine("\t\t}");


                code.AppendLine("\t\t\tcase " + getGuid + ":" + "//" + info.info.Name + " get");
                code.AppendLine("\t\t\t{");

                if (!field.IsStatic && !isModule)
                {
                    code.AppendLine("\t\t\t\tif(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;");
                    code.AppendLine($"\t\t\t\tif(pNode.GetInportCount() <= {1}) return true;");
                    code.AppendLine($"\t\t\t\tif(!(pUserClass.pPointer is {oriTypeClassName})) return true;");
                }
                else
                    code.AppendLine($"\t\t\t\tif(pNode.GetInportCount() <= {1}) return true;");
                if (isModule)
                {
                    code.AppendLine($"\t\t\t\t{export.type} pModulePointer = pAgentTree.GetModule<{export.type}>();");
                    code.AppendLine($"\t\t\t\tif(pModulePointer == null) return true;");
                }

                if (isModule)
                {
                    code.Append($"\t\t\t\treturn {functionNameGet}(pModulePointer, pAgentTree, pNode");
                }
                if (!field.IsStatic)
                {
                    code.Append($"\t\t\t\treturn {functionNameGet}(({oriTypeClassName})pUserClass.pPointer, pAgentTree, pNode");
                }
                else
                {
                    code.Append($"\t\t\t\treturn {functionNameGet}(pAgentTree, pNode");
                }

                code.AppendLine($");");
                code.AppendLine("\t\t\t}");
            }

            if (info.fieldAttr.bSet && !export.type.IsValueType)
            {
                methodCode.AppendLine("#if UNITY_EDITOR");
                methodCode.AppendLine(functionAttributesSet);
                methodCode.AppendLine("#endif");
                methodCode.AppendLine(functionHeadSet);
                methodCode.AppendLine("\t\t{");
                methodCode.AppendLine(functionCallSet);
                methodCode.AppendLine("\t\t\treturn true;");
                methodCode.AppendLine("\t\t}");

                code.AppendLine("\t\t\tcase " + setGuid + ":" + "//" + info.info.Name + " set");
                code.AppendLine("\t\t\t{");

                if (!field.IsStatic && !isModule)
                {
                    code.AppendLine("\t\t\t\tif(!CheckUserClassPointer(ref pUserClass, pAgentTree, pNode)) return true;");
                    code.AppendLine($"\t\t\t\tif(pNode.GetInportCount() <= {1}) return true;");
                    code.AppendLine($"\t\t\t\tif(!(pUserClass.pPointer is {oriTypeClassName})) return true;");
                }
                else
                    code.AppendLine($"\t\t\t\tif(pNode.GetInportCount() <= {1}) return true;");
                if (isModule)
                {
                    code.AppendLine($"\t\t\t\t{export.type} pModulePointer = pAgentTree.GetModule<{export.type}>();");
                    code.AppendLine($"\t\t\t\tif(pModulePointer == null) return true;");
                }
                if (isModule)
                {
                    code.Append($"\t\t\t\treturn {functionNameGet}(pModulePointer,");
                }
                else if (!field.IsStatic)
                {
                    code.Append($"\t\t\t\treturn {functionNameSet}(({oriTypeClassName})pUserClass.pPointer,");
                }
                else
                {
                    code.Append($"\t\t\t\treturn {functionNameSet}(");
                }

                code.Append($"{castLabel}pAgentTree.{ConvertTypeToATGetInPortFunction(field.FieldType)}(pNode,{1})");
                code.AppendLine($");");
                code.AppendLine("\t\t\t}");
            }
        }
        //-----------------------------------------------------
        static void ExportCallEnter()
        {
            StringBuilder code = new StringBuilder();
            code.AppendLine("//auto generated");
            code.AppendLine("using System.Collections.Generic;");
            code.AppendLine("namespace Framework.AT.Runtime");
            code.AppendLine("{");
            code.AppendLine("\tpublic class ATRtti");
            code.AppendLine("\t{");
            code.AppendLine("\t\tstatic Dictionary<int, System.Type> ms_vIdTypes = null;");
            code.AppendLine("\t\tstatic Dictionary<System.Type, int> ms_vTypeIds = null;");
            code.AppendLine("\t\t//-----------------------------------------------------");
            code.AppendLine("\t\tstatic void Init()");
            code.AppendLine("\t\t{");
            code.AppendLine($"\t\t\tif(ms_vIdTypes != null) return;");
            code.AppendLine($"\t\t\tif(ms_vIdTypes == null) ms_vIdTypes = new Dictionary<int, System.Type>({ms_vExports.Count});");
            code.AppendLine($"\t\t\tif(ms_vTypeIds == null) ms_vTypeIds = new Dictionary<System.Type,int>({ms_vExports.Count});");
            code.AppendLine($"\t\t\tms_vIdTypes.Clear();");
            code.AppendLine($"\t\t\tms_vTypeIds.Clear();");
            foreach (var db in ms_vExports)
            {
                code.AppendLine($"\t\t\tRegister({db.Value.guid}, typeof({db.Value.type.FullName.Replace("+", ".")}));");
            }
            code.AppendLine("\t\t}");
            code.AppendLine("\t\t//-----------------------------------------------------");
            code.AppendLine("\t\tpublic static void Register(int typeId, System.Type type)");
            code.AppendLine("\t\t{");
            code.AppendLine($"\t\t\tif(ms_vIdTypes == null) ms_vIdTypes = new Dictionary<int, System.Type>(128);");
            code.AppendLine($"\t\t\tif(ms_vTypeIds == null) ms_vTypeIds = new Dictionary<System.Type,int>(128);");
            code.AppendLine($"\t\t\tms_vIdTypes[typeId] = type;");
            code.AppendLine($"\t\t\tms_vTypeIds[type] = typeId;");
            code.AppendLine("\t\t}");
            code.AppendLine("\t\t//-----------------------------------------------------");
            code.AppendLine("\t\tpublic static System.Type GetClassType(int typeId)");
            code.AppendLine("\t\t{");
            code.AppendLine($"\t\t\tInit();");
            code.AppendLine($"\t\t\tif(ms_vIdTypes.TryGetValue(typeId, out var classType)) return classType;");
            code.AppendLine($"\t\t\treturn null;");
            code.AppendLine("\t\t}");
            code.AppendLine("\t\t//-----------------------------------------------------");
            code.AppendLine("\t\tpublic static int GetClassTypeId(System.Type type)");
            code.AppendLine("\t\t{");
            code.AppendLine($"\t\t\tInit();");
            code.AppendLine($"\t\t\tif(ms_vTypeIds.TryGetValue(type, out var typeId)) return typeId;");
            code.AppendLine($"\t\t\treturn 0;");
            code.AppendLine("\t\t}");
            code.AppendLine("\t}");
            code.AppendLine("}");

            string fullPath = Application.dataPath + "/" + EXPORT_PATH + "ATRtti.cs";
            if (!Directory.Exists(Path.GetDirectoryName(fullPath)))
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate);
            StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.UTF8);
            fs.Position = 0;
            fs.SetLength(0);
            writer.Write(code);
            writer.Close();
        }
        //-----------------------------------------------------
        static void ExportATClasses()
        {
            StringBuilder code = new StringBuilder();
            code.AppendLine("//auto generated");
            code.AppendLine("namespace Framework.AT.Runtime");
            code.AppendLine("{");
            code.AppendLine("\tpublic class ATCallHandler");
            code.AppendLine("\t{");
            code.AppendLine("\t\tpublic delegate bool OnActionDelegate(VariableUserData pCall, AgentTree pAgentTree, BaseNode pNode);");
            code.AppendLine("\t\tstatic Dictionary<int, OnActionDelegate> ms_CallHandles = new Dictionary<int, OnActionDelegate>(128);");
            code.AppendLine("\t\tpublic static void RegisterHandler(int callId, OnActionDelegate callFunction)");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tms_CallHandles[callId] = callFunction;");
            code.AppendLine("\t\t}");
            code.AppendLine("\t\t//-----------------------------------------------------");
            code.AppendLine("\t\tpublic static bool DoAction(AgentTree pAgentTree, BaseNode pNode)");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tif(pNode == null || pNode.GetInportCount()<=0) return true;");
            code.AppendLine("\t\t\tvar pUserClasser = pAgentTree.GetInportUserData(pNode, 0);");
            code.AppendLine("\t\t\tswitch(pUserClasser.value)");
            code.AppendLine("\t\t\t{");
            foreach (var db in ms_vExports)
            {
                code.Append("\t\t\tcase " + db.Value.guid + ":");
                code.Append($"return {db.Value.type.Namespace.Replace("+",".")}.{db.Value.type.FullName.Replace(".", "_")}.DoAction(pUserClasser,pAgentTree, pNode);");
                code.AppendLine("//" + db.Key);
            }
            code.AppendLine("\t\t\t}");
            code.AppendLine("\t\t\tif(ms_CallHandles.TryGetValue(pUserClasser.value, out var callFunction))");
            code.AppendLine("\t\t\t{");
            code.AppendLine("\t\t\treturn callFunction(pUserClasser, pAgentTree, pNode);");
            code.AppendLine("\t\t\t}");
            code.AppendLine("\t\t\treturn false;");
            code.AppendLine("\t\t}");
            code.AppendLine("\t}");
            code.AppendLine("}");

            string fullPath = Application.dataPath + "/" + EXPORT_PATH + "ATCallHandler.cs";
            if (!Directory.Exists(Path.GetDirectoryName(fullPath)))
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate);
            StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.UTF8);
            fs.Position = 0;
            fs.SetLength(0);
            writer.Write(code);
            writer.Close();
        }
        //-----------------------------------------------------
        static void ExportRegisterCode(bool bInternal)
        {
            StringBuilder code = new StringBuilder();
            code.AppendLine("//auto generated");
            code.AppendLine("namespace Framework.AT.Runtime");
            code.AppendLine("{");
            code.AppendLine("\t[ATEditorInitialize]");
            if (bInternal) code.AppendLine("\tinternal class ATRegisterInternalHandler");
            else code.AppendLine("\tinternal class ATRegisterHandler");
            code.AppendLine("\t{");
            code.AppendLine("\t\t//-----------------------------------------------------");
            code.AppendLine("\t\tpublic static void Init()");
            code.AppendLine("\t\t{");
            foreach (var db in ms_vExports)
            {
                string function = $"{db.Value.type.Namespace.Replace("+", ".")}.{db.Value.type.FullName.Replace(".", "_")}.DoAction";
                string parentTypeId = "0";
                if(db.Value.type.BaseType!=null)
                {
                    if(IsUserDataType(db.Value.type.BaseType))
                    {
                        parentTypeId = BuildHashCode(db.Value.type.BaseType) + "/*" + GetTypeName(db.Value.type.BaseType) + "*/";
                    }
                }
                code.AppendLine($"\t\t\tRegister({db.Value.guid}, typeof({db.Value.type.FullName.Replace("+", ".")}),{function},{parentTypeId});");
            }
            code.AppendLine("\t\t}");
            code.AppendLine("\t\t//-----------------------------------------------------");
            code.AppendLine("\t\tpublic static void Register(int typeId, System.Type type, ATCallHandler.OnActionDelegate onFunction, int parentTypeId =0)");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tATRtti.Register(typeId,type,parentTypeId);");
            code.AppendLine("\t\t\tATCallHandler.RegisterHandler(typeId,onFunction);");
            code.AppendLine("\t\t}");
            code.AppendLine("\t}");
            code.AppendLine("}");

            string fullPath = Application.dataPath + "/" + EXPORT_PATH + "ATRegisterHandler.cs";
            if (!Directory.Exists(Path.GetDirectoryName(fullPath)))
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate);
            StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.UTF8);
            fs.Position = 0;
            fs.SetLength(0);
            writer.Write(code);
            writer.Close();
        }
        //-----------------------------------------------------
        static bool CheckMethodCanExport(MethodInfo method)
        {
            if(!SupportExportATType(method.ReturnType))
            {
                return false;
            }
            var parameters = method.GetParameters();
            if(parameters !=null && parameters.Length>0)
            {
                for(int i=0; i < parameters.Length; ++i)
                {
                    if (!SupportExportATType(parameters[i].ParameterType))
                        return false;
                }
            }
            if (IsUserDataType(method.ReturnType)) ms_vRefTypes.Add(method.ReturnType);
            if (parameters != null && parameters.Length > 0)
            {
                for (int i = 0; i < parameters.Length; ++i)
                {
                    if (IsUserDataType(parameters[i].ParameterType))
                        ms_vRefTypes.Add(parameters[i].ParameterType);
                }
            }
            return true;
        }
        //-----------------------------------------------------
        static bool CheckCanExport(FieldInfo method)
        {
            if (!SupportExportATType(method.FieldType))
            {
                return false;
            }
            return true;
        }
        //------------------------------------------------------
        static int BuildHashCode(System.Type type)
        {
            return ATRtti.BuildHashCode(type);
        }
        //-----------------------------------------------------
        static string GetTypeName(System.Type type)
        {
            type = GetParameterType(type);
            string name = type.FullName.Replace("+", ".");
            if (name.EndsWith("&")) name = name.Substring(0, name.Length - 1);
            return name;
        }
        //-----------------------------------------------------
        static bool IsUserDataType(System.Type type)
        {
            if (type.IsClass || type.IsValueType)
            {
                if (type.GetType().GetInterface(typeof(IUserData).FullName.Replace("+", ".")) != null)
                    return true;
            }
            return type == typeof(IUserData) || type.GetInterfaces().Contains(typeof(IUserData));
        }
        //-----------------------------------------------------
        static System.Type ConvertTypeToATType(System.Type type)
        {
            type = GetParameterType(type);
            if (type == typeof(bool)) return typeof(VariableBool);
            else if (type == typeof(byte)) return typeof(VariableInt);
            else if (type == typeof(sbyte)) return typeof(VariableInt);
            else if (type == typeof(ushort)) return typeof(VariableInt);
            else if (type == typeof(short)) return typeof(VariableInt);
            else if (type == typeof(int)) return typeof(VariableInt);
            else if (type == typeof(uint)) return typeof(VariableInt);
            else if (type == typeof(float)) return typeof(VariableFloat);
            else if (type == typeof(double)) return typeof(VariableDouble);
            else if (type == typeof(long)) return typeof(VariableLong);
            else if (type == typeof(ulong)) return typeof(VariableLong);
            else if (type == typeof(Vector2)) return typeof(VariableVec2);
            else if (type == typeof(Vector2Int)) return typeof(VariableVec2);
            else if (type == typeof(Vector3)) return typeof(VariableVec3);
            else if (type == typeof(Vector3Int)) return typeof(VariableVec3);
            else if (type == typeof(Vector4)) return typeof(VariableVec4);
            else if (type == typeof(Quaternion)) return typeof(VariableQuaternion);
            else if (type == typeof(Ray)) return typeof(VariableRay);
            else if (type == typeof(Color)) return typeof(VariableColor);
            else if (type == typeof(Rect)) return typeof(VariableRect);
            else if (type == typeof(Bounds)) return typeof(VariableBounds);
            else if (type == typeof(Matrix4x4)) return typeof(VariableMatrix);
            else if (type == typeof(ObjId)) return typeof(VariableObjId);
            else if (type == typeof(string)) return typeof(VariableString);
            else if (type == typeof(IUserData)) return typeof(VariableUserData);
            else if (IsUserDataType(type)) return typeof(VariableUserData);
            else if (type.IsEnum) return typeof(VariableInt);
            return null;
        }
        //-----------------------------------------------------
        static string ConvertTypeToATGetInPortFunction(System.Type type)
        {
            type = GetParameterType(type);
            if (type == typeof(bool)) return "GetInportBool";
            else if (type == typeof(char)) return "GetInportChar";
            else if (type == typeof(byte)) return "GetInportByte";
            else if (type == typeof(sbyte)) return "GetInportSbyte";
            else if (type == typeof(ushort)) return "GetInportUshort";
            else if (type == typeof(short)) return "GetInportShort";
            else if (type == typeof(int)) return "GetInportInt";
            else if (type == typeof(uint)) return "GetInportUint";
            else if (type == typeof(float)) return "GetInportFloat";
            else if (type == typeof(double)) return "GetInportDouble";
            else if (type == typeof(long)) return "GetInportLong";
            else if (type == typeof(ulong)) return "GetInportUlong";
            else if (type == typeof(Vector2)) return "GetInportVec2";
            else if (type == typeof(Vector2Int)) return "GetInportVec2Int";
            else if (type == typeof(Vector3)) return "GetInportVec3";
            else if (type == typeof(Vector3Int)) return "GetInportVec3Int";
            else if (type == typeof(Vector4)) return "GetInportVec4";
            else if (type == typeof(Quaternion)) return "GetInportQuaternion";
            else if (type == typeof(Ray)) return "GetInportRay";
            else if (type == typeof(Color)) return "GetInportColor";
            else if (type == typeof(Rect)) return "GetInportRect";
            else if (type == typeof(Bounds)) return "GetInportBounds";
            else if (type == typeof(Matrix4x4)) return "GetInportMatrix";
            else if (type == typeof(string)) return "GetInportString";
            else if (type == typeof(IUserData)) return "GetInportUserData<"+GetTypeName(type) +">";
            else if (type == typeof(ObjId)) return "GetInportObjId";
            else if (IsUserDataType(type)) return "GetInportUserData<"+GetTypeName(type) +">";
            else if (type.IsEnum) return "GetInportInt";
            return null;
        }
        //-----------------------------------------------------
        static string ConvertTypeToATGetOutPortFunction(System.Type type)
        {
            if (type == typeof(bool)) return "GetOutportBool";
            else if (type == typeof(char)) return "GetOutportChar";
            else if (type == typeof(byte)) return "GetOutportByte";
            else if (type == typeof(sbyte)) return "GetOutportSbyte";
            else if (type == typeof(ushort)) return "GetOutportUshort";
            else if (type == typeof(short)) return "GetOutportShort";
            else if (type == typeof(int)) return "GetOutportInt";
            else if (type == typeof(uint)) return "GetOutportUint";
            else if (type == typeof(float)) return "GetOutportFloat";
            else if (type == typeof(double)) return "GetOutportDouble";
            else if (type == typeof(long)) return "GetOutportLong";
            else if (type == typeof(ulong)) return "GetOutportUlong";
            else if (type == typeof(Vector2)) return "GetOutportVec2";
            else if (type == typeof(Vector2Int)) return "GetOutportVec2Int";
            else if (type == typeof(Vector3)) return "GetOutportVec3";
            else if (type == typeof(Vector3Int)) return "GetOutportVec3Int";
            else if (type == typeof(Vector4)) return "GetOutportVec4";
            else if (type == typeof(Quaternion)) return "GetOutportQuaternion";
            else if (type == typeof(Ray)) return "GetOutportRay";
            else if (type == typeof(Color)) return "GetOutportColor";
            else if (type == typeof(Rect)) return "GetOutportRect";
            else if (type == typeof(Bounds)) return "GetOutportBounds";
            else if (type == typeof(Matrix4x4)) return "GetOutportMatrix";
            else if (type == typeof(IUserData)) return "GetOutportUserData";
            else if (type == typeof(string)) return "GetOutportString";
            else if (IsUserDataType(type)) return "GetOutportUserData";
            else if (type.IsEnum) return "GetOutportInt";
            return null;
        }
        //-----------------------------------------------------
        static string ConvertTypeToATSetOutPortFunction(System.Type type)
        {
            if (type == typeof(bool))               return "SetOutportBool";
            else if (type == typeof(char))          return "SetOutportChar";
            else if (type == typeof(byte))          return "SetOutportByte";
            else if (type == typeof(sbyte))         return "SetOutportSbyte";
            else if (type == typeof(ushort))        return "SetOutportUshort";
            else if (type == typeof(short))         return "SetOutportShort";
            else if (type == typeof(int))           return "SetOutportInt";
            else if (type == typeof(uint))          return "SetOutportUint";
            else if (type == typeof(float))         return "SetOutportFloat";
            else if (type == typeof(double))        return "SetOutportDouble";
            else if (type == typeof(long))          return "SetOutportLong";
            else if (type == typeof(ulong))         return "SetOutportUlong";
            else if (type == typeof(Vector2))       return "SetOutportVec2";
            else if (type == typeof(Vector2Int))    return "SetOutportVec2Int";
            else if (type == typeof(Vector3))       return "SetOutportVec3";
            else if (type == typeof(Vector3Int))    return "SetOutportVec3Int";
            else if (type == typeof(Vector4))       return "SetOutportVec4";
            else if (type == typeof(Quaternion))    return "SetOutportQuaternion";
            else if (type == typeof(Ray))           return "SetOutportRay";
            else if (type == typeof(Color))         return "SetOutportColor";
            else if (type == typeof(Rect))          return "SetOutportRect";
            else if (type == typeof(Bounds))        return "SetOutportBounds";
            else if (type == typeof(Matrix4x4))     return "SetOutportMatrix";
            else if (type == typeof(IUserData))     return "SetOutportUserData";
            else if (type == typeof(string))        return "SetOutportString";
            else if (IsUserDataType(type))          return "SetOutportUserData";
            else if (type.IsEnum)                   return "SetOutportInt";
            return null;
        }
        //-----------------------------------------------------
        static Type GetParameterType(System.Type type)
        {
            if ((type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)))
            {
                return Nullable.GetUnderlyingType(type);
            }
            return type;
        }
        //-----------------------------------------------------
        static bool SupportExportATType(System.Type type)
        {
            type = GetParameterType(type);

            if (type == typeof(void)
                || type == typeof(bool)
                || type == typeof(byte)
                || type == typeof(sbyte)
                || type == typeof(ushort)
                || type == typeof(short)
                || type == typeof(int)
                || type == typeof(uint)
                || type == typeof(float)
                || type == typeof(double)
                || type == typeof(string)
                || type == typeof(long)
                || type == typeof(ulong)
                || type == typeof(Vector2)
                || type == typeof(Vector2Int)
                || type == typeof(Vector3)
                || type == typeof(Vector3Int)
                || type == typeof(Vector4)
                || type == typeof(Quaternion)
                || type == typeof(Ray)
                || type == typeof(Ray2D)
                || type == typeof(Rect)
                || type == typeof(Bounds)
                || type == typeof(Matrix4x4)
                || type == typeof(ObjId)
                || type.IsEnum)
            {
                return true;
            }
            if (type == typeof(IUserData))
                return true;

            if (IsUserDataType(type))
            {
                return true;
            }
            return false;
        }
    }
}

#endif