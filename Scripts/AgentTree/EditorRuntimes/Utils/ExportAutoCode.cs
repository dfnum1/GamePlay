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
using UnityEditor;
using UnityEngine;

namespace Framework.AT.Editor
{
    public class ExportAutoCode
    {
        public class ExportInfo
        {
            public struct ExportMethodInfo
            {
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
                            }
                        }
                    }
                }
            }
            foreach(var db in ms_vExports)
            {
                ExportCode(db.Value);
            }
        }
        //-----------------------------------------------------
        static void ExportCode(ExportInfo export)
        {
            foreach(var db in export.methods)
            {

            }
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