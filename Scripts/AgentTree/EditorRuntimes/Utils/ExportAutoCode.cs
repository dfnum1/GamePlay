/********************************************************************
生成日期:	06:30:2025
类    名: 	IconUtils
作    者:	HappLI
描    述:	
*********************************************************************/
#if UNITY_EDITOR
using Framework.AT.Runtime;
using Framework.Core;
using Framework.DrawProps;
using System;
using System.Collections.Generic;
using System.IO;
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
            public class ExportMethodInfo
            {
                public int guid;
                public string memberName;
                public MemberInfo info;

                public System.Type DeclaringType
                {
                    get { return info.DeclaringType; }
                }
                public T GetCustomAttribute<T>() where T : Attribute
                {
                    return info.GetCustomAttribute(typeof(T)) as T;
                }
                public T GetCustomAttribute<T>(bool inherit) where T : Attribute
                {
                    return info.GetCustomAttribute(typeof(T), inherit) as T;
                }
                public T[] GetCustomAttributes<T>() where T : Attribute
                {
                    return info.GetCustomAttributes(typeof(T)) as T[];
                }
                public T[] GetCustomAttributes<T>(bool inherit) where T : Attribute
                {
                    return info.GetCustomAttributes(typeof(T), inherit) as T[];
                }
                public bool IsDefined(System.Type attrType)
                {
                    return info.IsDefined(attrType);
                }
            }

            public string exportPath;

            public int guid = 0;
            public Type type;
            public ATExportAttribute exportAttr;
            public Dictionary<string, int> methodCount = new Dictionary<string, int>();
            public Dictionary<string, ExportMethodInfo> methods = new Dictionary<string, ExportMethodInfo>();
        }
        static Dictionary<string, ExportInfo> ms_vExports = new Dictionary<string, ExportInfo>();
        static HashSet<System.Type> ms_vRefTypes = new HashSet<Type>();
        public static void ExportATMothed()
        {
            ms_vExports.Clear();
            ms_vRefTypes.Clear();
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
                    if (tp.IsDefined(typeof(ATExportAttribute), false))
                    {
                        var exportAttr = tp.GetCustomAttribute<ATExportAttribute>();
                        string typeName = GetTypeName(tp);
                        MethodInfo[] meths = types[i].GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
                        for (int m = 0; m < meths.Length; ++m)
                        {
                            if (meths[m].IsDefined(typeof(ATMethodAttribute), false))
                            {
                                if (!CheckMethodCanExport(meths[m]))
                                {
                                    Debug.LogWarning(typeName +  " Method " + meths[m].Name + " can not export!");
                                    continue;
                                }
                                ATMethodAttribute attr = (ATMethodAttribute)meths[m].GetCustomAttribute(typeof(ATMethodAttribute));
                                ExportInfo exportData;
                                if (!ms_vExports.TryGetValue(typeName, out exportData))
                                {
                                    exportData = new ExportInfo();
                                    exportData.guid = exportAttr.guid;
                                    if (exportData.guid ==0)
                                    {
                                        exportData.guid = Animator.StringToHash(typeName);
                                    }
                                    exportData.exportPath = "../ATExports/" + tp.FullName.Replace("+", "_") + ".cs";
                                    exportData.type = tp;
                                    exportData.exportAttr = exportAttr;
                                    ms_vExports.Add(typeName, exportData);
                                }
                                if(!exportData.methodCount.TryGetValue(meths[m].Name, out var methodCnt))
                                {
                                    methodCnt = 0;
                                }
                                methodCnt++;
                                exportData.methodCount[meths[m].Name] = methodCnt;

                                ExportInfo.ExportMethodInfo exportMth = new ExportInfo.ExportMethodInfo();
                                exportMth.info = meths[m];
                                if (methodCnt <= 1) exportMth.memberName = meths[m].Name;
                                else exportMth.memberName = meths[m].Name + "_" + (methodCnt - 1);
                                exportMth.guid = Animator.StringToHash(typeName + ":" + exportMth.memberName);
                                exportData.methods[exportMth.memberName] =exportMth;
                            }
                        }
                    }
                }
            }
            foreach(var db in ms_vExports)
            {
                ExportCode(db.Value);
            }
            ExportATClasses();
            ExportCallEnter();
        }
        //-----------------------------------------------------
        static void ExportCode(ExportInfo export)
        {
            StringBuilder code = new StringBuilder();
            code.AppendLine("//auto generated");
            code.AppendLine("namespace " + export.type.Namespace.Replace("+", "."));
            code.AppendLine("{");
            code.AppendLine($"\tpublic class {export.type.FullName.Replace("+", "_")}");
            code.AppendLine("\t{");
            code.AppendLine("\t\tpublic static bool DoAction(AgentTree pAgentTree, BaseNode pNode)");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tint actionType = pNode.type;");
            code.AppendLine("\t\t\tswitch(actionType)");
            foreach (var db in export.methods)
            {
                code.AppendLine("\t\t\tcase " + db.Value.guid + ":" + "//" + db.Value.info.Name);
                code.AppendLine("\t\t\t{");
                if(db.Value.info is MethodInfo)
                {
                    MethodInfo method = db.Value.info as MethodInfo;

                    string functionHead = "public static bool AT_" + db.Value.memberName + "(";
                    functionHead += "VariableUserData pUserClass";
                    foreach (var parm in method.GetParameters())
                    {
                        getp
                        functionHead += 
                    }
                    functionHead += ")";
                    if (method.IsStatic)
                    {

                    }
                    else
                    {

                    }
                }
                code.AppendLine("\t\t\t\treturn true;");
                code.AppendLine("\t\t\t}");

            }
            code.AppendLine("\t\t\treturn false;");
            code.AppendLine("\t\t}");
            code.AppendLine("\t}");
            code.AppendLine("}");

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
                code.AppendLine($"\t\t\tms_vIdTypes[{db.Value.guid}] = typeof({db.Value.type.FullName.Replace("+", ".")});");
                code.AppendLine($"\t\t\tms_vTypeIds[typeof({db.Value.type.FullName.Replace("+", ".")})] = {db.Value.guid};");
            }
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
            code.AppendLine("\t\tpublic static bool DoAction(AgentTree pAgentTree, BaseNode pNode)");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tif(pNode == null || pNode.GetInportCount()<=0) return true;");
            code.AppendLine("\t\t\tint classId = pAgentTree.GetInportInt(pNode, 0);");
            code.AppendLine("\t\t\tswitch(classId)");
            foreach (var db in ms_vExports)
            {
                code.Append("\t\t\tcase " + db.Value.guid + ":");
                code.Append($"return {db.Value.type.FullName.Replace("+", "_")}.DoAction(pAgentTree, pNode);");
                code.AppendLine("//" + db.Key);
            }
            code.AppendLine("\t\t\treturn false;");
            code.AppendLine("\t\t}");
            code.AppendLine("\t}");
            code.AppendLine("}");
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
                    if (!IsUserDataType(parameters[i].ParameterType))
                        ms_vRefTypes.Add(parameters[i].ParameterType);
                }
            }



            return true;
        }
        //-----------------------------------------------------
        static string GetTypeName(System.Type type)
        {
            return type.FullName.Replace("+", ".");
        }
        //-----------------------------------------------------
        static bool IsUserDataType(System.Type type)
        {
            if ((type.IsClass || type.IsValueType))
            {
                if (type.GetType().GetInterface(typeof(IUserData).FullName.Replace("+", ".")) != null)
                    return true;
            }
            return false;
        }
        //-----------------------------------------------------
        static System.Type ConvertTypeToATType(System.Type type)
        {
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
            else if (type == typeof(Ray2D)) return typeof(VariableRay2D);
            else if (type == typeof(Rect)) return typeof(VariableRect);
            else if (type == typeof(Bounds)) return typeof(VariableBounds);
            else if (type == typeof(Matrix4x4)) return typeof(VariableMatrix);
            else if (type == typeof(IUserData)) return typeof(VariableUserData);
            else if (IsUserDataType(type)) return typeof(VariableUserData);
            return null;
        }
        //-----------------------------------------------------
        static bool SupportExportATType(System.Type type)
        {
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