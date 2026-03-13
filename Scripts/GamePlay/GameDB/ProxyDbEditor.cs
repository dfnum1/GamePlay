#if UNITY_EDITOR
/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	DBTypeAutoCode
作    者:	HappLI
描    述:	db 数据索引类型代码自动生成
*********************************************************************/

using Framework.AT.Editor;
using Framework.AT.Runtime;
using Framework.ED;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Framework.Db
{
    [EditorInitOnload]
    [ATDrawer(BaseATDrawerKey.Key_DrawProxyDbTypePop, "OnDrawProxyDbTypePop")]
    class ProxyDbEditor
    {
        class ClassBinaryCode
        {
            public int clsId;
            public System.Type classType = null;
        }
        static List<ClassBinaryCode> ms_ClassLists = null;
        static List<int> ms_vClsIds = new List<int>();
        static List<string> ms_vClsNames = new List<string>();
        //------------------------------------------------------
        static List<ClassBinaryCode> Collects()
        {
            if(ms_ClassLists == null)
            {
                ms_vClsIds.Clear();
                ms_vClsNames.Clear();
                ms_ClassLists = new List<ClassBinaryCode>();
                Assembly assembly = null;
                foreach (var ass in System.AppDomain.CurrentDomain.GetAssemblies())
                {
                    assembly = ass;

                    Type[] types = null;
                    try
                    {
                        assembly.GetTypes();
                    }
                    catch
                    {

                    }
                    if (types == null) continue;
                    for (int i = 0; i < types.Length; ++i)
                    {
                        Type tp = types[i];
                        if (tp == null) continue;
                        if (tp.IsSubclassOf(typeof(AProxyDB)))
                        {
                            var clsId = BuildHashCode(tp);
                            ms_vClsIds.Add(clsId);
                            ms_vClsNames.Add(Framework.ED.EditorUtils.GetDisplayName(tp));
                            ms_ClassLists.Add(new ClassBinaryCode() { clsId = clsId, classType = tp });
                        }
                    }
                }
            }
            
            return ms_ClassLists;
        }
        //------------------------------------------------------
        static void Build(string GeneratorCode = "Scripts/GameMain/Generators/NetWork")
        {
            EditorUtility.DisplayProgressBar("代码自动化", "DB、网络相关代码自动化...", 0.2f);
            ms_ClassLists = null;
            var ClassCodeMapping = Collects();
            string root = Path.Combine(Application.dataPath, GeneratorCode);
            EditorUtility.DisplayProgressBar("代码自动化", "DB、网络相关代码自动化...", 0.5f);
            BuildCode(Path.Combine(root, "DBTypeMapping.cs"), ClassCodeMapping);
            EditorUtility.DisplayProgressBar("代码自动化", "DB、网络相关代码自动化...", 0.7f);
            BuildDbDirtyCode(Path.Combine(root, "DBDirty"), ClassCodeMapping);

            EditorUtility.DisplayProgressBar("代码自动化", "DB、网络相关代码自动化...", 0.9f);
         //   MessageBuilder.DoBuilder(GeneratorCode);
            EditorUtility.ClearProgressBar();
        }
        //------------------------------------------------------
        static void BuildCode(string strPath, List<ClassBinaryCode> types)
        {
            string code = "//auto generator\r\n";
            code += "using Framework.Base;\r\n";
            code += "using Framework.Core;\r\n";
            code += "namespace Framework.Db\r\n";
            code += "{\r\n";
            code += "[Framework.Base.EditorSetupInit]\r\n";
            code += "\tpublic class DBTypeMapping \r\n";
            code += "\t{\r\n";

            code += "\t\tpublic static void Init()\r\n";
            code += "\t\t{\r\n";
            foreach (var db in types)
                code += "\t\t\tDBRtti.Register(" + db.clsId + ", typeof(" + db.classType.FullName.Replace("+", ".") + ")" + ", MallocTypeClass);\r\n";
            code += "\t\t}\r\n";

            code += "\t\tstatic AProxyDB MallocTypeClass(AFramework pFramework, int type)\r\n";
            code += "\t\t{\r\n";
            code += "\t\t\tswitch(type)\r\n";
            code += "\t\t\t{\r\n";
            foreach (var db in types)
                code += "\t\t\t\tcase " + db.clsId + ": return TypeInstancePool.Malloc<" + db.classType.FullName.Replace("+",".") + "(pFramework); break;\r\n";
            code += "\t\t\t}\r\n";
            code += "\t\t\treturn null;\r\n";
            code += "\t\t}\r\n";

            code += "\t}\r\n";
            code += "}\r\n";

            FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate);
            StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.UTF8);
            fs.Position = 0;
            fs.SetLength(0);
            writer.Write(code);
            writer.Close();
        }
        //------------------------------------------------------
        static void BuildDbDirtyCode(string strRoot, List<ClassBinaryCode> types)
        {
            if (!Directory.Exists(strRoot)) Directory.CreateDirectory(strRoot);
            foreach(var db in types)
            {
                string strPath = Path.Combine(strRoot, db.classType.Name + "_Dirty.cs");

                var fields = db.classType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
                if (fields == null || fields.Length <= 0)
                {
                    if (File.Exists(strPath)) File.Delete(strPath);
                    if(File.Exists(strPath + ".meta")) File.Delete(strPath + ".meta");
                    continue;
                }

                string code = "//auto generator\r\n";
                code += $"namespace {db.classType.Namespace.Replace("+",".")}\r\n";
                code += "{\r\n";
                code += $"\tpublic partial class {db.classType.Name}\r\n";
                code += "\t{\r\n";
                for(int i =0; i < fields.Length; ++i)
                {
                    string name = fields[i].Name;

                    if (fields[i].FieldType.ToString().StartsWith("System.Action"))
                    {
                        continue;
                    }

                    if(!name.StartsWith("m_"))
                    {
                        Debug.LogWarning($"{db.classType.Name} 的成员变量 \"{name}\" 没有以'm_'作用开始的命令方式，需改为\"m_{name}\"");
                        continue;
                    }

                    if (fields[i].FieldType.IsGenericType || fields[i].FieldType.IsArray)
                    {
                        continue;
                    }
                    if (!BuildPropSetCode(fields[i], out var strCompare, out var setCode))
                    {
                        Debug.LogWarning($"{db.classType.Name} 的成员变量 \"{name}\" 暂时不支持数据Dirty机制");
                        continue;
                    }

                    name= BuildPropName(fields[i], out var actionName);
                    if (i>0)
                        code += "\t\t//------------------------------------------------------\r\n";
                    if (!fields[i].IsDefined(typeof(DbFieldUnDirtyAttribute)))
                    {
                        code += "\t\tpublic System.Action<" + fields[i].FieldType.FullName + "> " + actionName + ";\r\n";
                    }
                    code += $"\t\tpublic {fields[i].FieldType.FullName} {name} \r\n";
                    code += "\t\t{\r\n";
                    code += "\t\t\tget{ return "+ fields[i].Name + "; }\r\n";
                    code += "\t\t\tset\r\n";
                    code += "\t\t\t{\r\n";
                    foreach (var sdb in strCompare)
                        code += $"\t\t\t\t{sdb}\r\n";
                    foreach(var sdb in setCode)
                        code += $"\t\t\t\t\t{sdb}\r\n";
                    if (!fields[i].IsDefined(typeof(DbFieldUnDirtyAttribute)))
                    {
                        code += $"\t\t\t\t{actionName}?.Invoke(value);\r\n";
                        code += "\t\t\t\tSetDirty();\r\n";
                    }

                    code += "\t\t\t}\r\n";
                    code += "\t\t}\r\n";
                }
                code += "\t}\r\n";
                code += "}\r\n";

                FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate);
                StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.UTF8);
                fs.Position = 0;
                fs.SetLength(0);
                writer.Write(code);
                writer.Close();
            }
        }
        //------------------------------------------------------
        static string BuildPropName(System.Reflection.FieldInfo field, out string actioName)
        {
            string name = field.Name;

            if (name.StartsWith("m_")) name = name.Substring("m_".Length);

            string nameTemp = name;
            if (field.FieldType == typeof(bool) ||
                field.FieldType == typeof(byte) ||
                field.FieldType == typeof(sbyte) ||
                field.FieldType == typeof(char) ||
                field.FieldType == typeof(short) ||
                field.FieldType == typeof(ushort) ||
                field.FieldType == typeof(int) ||
                field.FieldType == typeof(uint) ||
                field.FieldType == typeof(long) ||
                field.FieldType == typeof(ulong) ||
                field.FieldType == typeof(float) ||
                field.FieldType == typeof(double) ||
                field.FieldType == typeof(string))
            {
                if (nameTemp.Length > 1 && nameTemp.StartsWith("b", StringComparison.OrdinalIgnoreCase) && !nameTemp.StartsWith("bb", StringComparison.OrdinalIgnoreCase))
                    nameTemp = nameTemp.Substring(1);
                else if (nameTemp.Length > 2 && nameTemp.StartsWith("is", StringComparison.OrdinalIgnoreCase) && !nameTemp.StartsWith("isis", StringComparison.OrdinalIgnoreCase))
                    nameTemp = nameTemp.Substring(2);
                else if (nameTemp.Length > 1 && nameTemp.StartsWith("b", StringComparison.OrdinalIgnoreCase) && !nameTemp.StartsWith("bb", StringComparison.OrdinalIgnoreCase))
                    nameTemp = nameTemp.Substring(1);
                else if (nameTemp.Length > 1 && nameTemp.StartsWith("n", StringComparison.OrdinalIgnoreCase) && !nameTemp.StartsWith("nn", StringComparison.OrdinalIgnoreCase))
                    nameTemp = nameTemp.Substring(1);
                else if (nameTemp.Length > 1 && nameTemp.StartsWith("u", StringComparison.OrdinalIgnoreCase) && !nameTemp.StartsWith("uu", StringComparison.OrdinalIgnoreCase))
                    nameTemp = nameTemp.Substring(1);
                else if (nameTemp.Length > 1 && nameTemp.StartsWith("c", StringComparison.OrdinalIgnoreCase) && !nameTemp.StartsWith("cc", StringComparison.OrdinalIgnoreCase))
                    nameTemp = nameTemp.Substring(1);
                else if (nameTemp.Length > 1 && nameTemp.StartsWith("s", StringComparison.OrdinalIgnoreCase) && !nameTemp.StartsWith("ss", StringComparison.OrdinalIgnoreCase))
                    nameTemp = nameTemp.Substring(1);
                else if (nameTemp.Length > 1 && nameTemp.StartsWith("n", StringComparison.OrdinalIgnoreCase) && !nameTemp.StartsWith("nn", StringComparison.OrdinalIgnoreCase))
                    nameTemp = nameTemp.Substring(1);
                else if (nameTemp.Length > 1 && nameTemp.StartsWith("s", StringComparison.OrdinalIgnoreCase) && !nameTemp.StartsWith("ss", StringComparison.OrdinalIgnoreCase))
                    nameTemp = nameTemp.Substring(1);
                else if (nameTemp.Length > 2 && nameTemp.StartsWith("un", StringComparison.OrdinalIgnoreCase) && !nameTemp.StartsWith("unun", StringComparison.OrdinalIgnoreCase))
                    nameTemp = nameTemp.Substring(1);
                else if (nameTemp.Length > 1 && nameTemp.StartsWith("us", StringComparison.OrdinalIgnoreCase) && !nameTemp.StartsWith("usus", StringComparison.OrdinalIgnoreCase))
                    nameTemp = nameTemp.Substring(1);
                else if (nameTemp.Length > 1 && nameTemp.StartsWith("l", StringComparison.OrdinalIgnoreCase) && !nameTemp.StartsWith("ll", StringComparison.OrdinalIgnoreCase))
                    nameTemp = nameTemp.Substring(1);
                else if (nameTemp.Length > 1 && nameTemp.StartsWith("l", StringComparison.OrdinalIgnoreCase) && !nameTemp.StartsWith("ll", StringComparison.OrdinalIgnoreCase))
                    nameTemp = nameTemp.Substring(1);
                else if (nameTemp.Length > 1 && nameTemp.StartsWith("f", StringComparison.OrdinalIgnoreCase) && !nameTemp.StartsWith("ff", StringComparison.OrdinalIgnoreCase))
                    nameTemp = nameTemp.Substring(1);
                else if (nameTemp.Length>1 && nameTemp.StartsWith("d", StringComparison.OrdinalIgnoreCase) && !nameTemp.StartsWith("dd", StringComparison.OrdinalIgnoreCase))
                    nameTemp = nameTemp.Substring(1);
                else if (nameTemp.Length>"str".Length && nameTemp.StartsWith("str", StringComparison.OrdinalIgnoreCase) && !nameTemp.StartsWith("strstr", StringComparison.OrdinalIgnoreCase))
                    nameTemp = nameTemp.Substring(1);
                else if (nameTemp.Length > "m".Length && nameTemp.StartsWith("m", StringComparison.OrdinalIgnoreCase) && !nameTemp.StartsWith("mm", StringComparison.OrdinalIgnoreCase))
                    nameTemp = nameTemp.Substring(1);
                else if (nameTemp.Length > "v".Length && nameTemp.StartsWith("v", StringComparison.OrdinalIgnoreCase) && !nameTemp.StartsWith("vv", StringComparison.OrdinalIgnoreCase))
                    nameTemp = nameTemp.Substring(1);
                else if (nameTemp.Length > "map".Length && nameTemp.StartsWith("map", StringComparison.OrdinalIgnoreCase) && !nameTemp.StartsWith("mapmap", StringComparison.OrdinalIgnoreCase))
                    nameTemp = nameTemp.Substring(1);
                else if (nameTemp.Length > "set".Length && nameTemp.StartsWith("set", StringComparison.OrdinalIgnoreCase) && !nameTemp.StartsWith("setset", StringComparison.OrdinalIgnoreCase))
                    nameTemp = nameTemp.Substring(1);
                else if (nameTemp.Length > "list".Length && nameTemp.StartsWith("list", StringComparison.OrdinalIgnoreCase) && !nameTemp.StartsWith("listlist", StringComparison.OrdinalIgnoreCase))
                    nameTemp = nameTemp.Substring(1);
                else if (nameTemp.Length > "dic".Length && nameTemp.StartsWith("dic", StringComparison.OrdinalIgnoreCase) && !nameTemp.StartsWith("dicdic", StringComparison.OrdinalIgnoreCase))
                    nameTemp = nameTemp.Substring(1);
            }
            actioName = "On" + nameTemp.Substring(0, 1).ToUpper() + nameTemp.Substring(1) + "Dirty";
            return name;
        }
        //------------------------------------------------------
        static bool BuildPropSetCode(System.Reflection.FieldInfo field, out List<string> strCompare, out List<string> strSet)
        {
            strCompare = new List<string>();
            strSet = new List<string>();
            if (field.FieldType.IsGenericType || field.FieldType.IsArray)
            {
                return false;
            }
            if (field.FieldType == typeof(bool) ||
                field.FieldType == typeof(byte) ||
                field.FieldType == typeof(sbyte) ||
                field.FieldType == typeof(char) ||
                field.FieldType == typeof(short) ||
                field.FieldType == typeof(ushort) ||
                field.FieldType == typeof(int) ||
                field.FieldType == typeof(uint) ||
                field.FieldType == typeof(long) ||
                field.FieldType == typeof(ulong) ||
                field.FieldType == typeof(float) ||
                field.FieldType == typeof(double))
            {
                strCompare.Add("if(" + field.Name + " == value) return;");
                strSet.Add(field.Name + " = value;");
                return true;
            }
            if (field.FieldType == typeof(string))
            {
                strCompare.Add($"if(string.Compare({field.Name}, value) == 0) return;");
                strSet.Add(field.Name + " = value;");
                return true;
            }

            if (field.FieldType.IsValueType || field.FieldType.IsClass)
            {
                var method = field.FieldType.GetMethod("CompareTo", BindingFlags.Instance | BindingFlags.Public);
                if (method == null)
                {
                    Debug.LogWarning($"{field.DeclaringType.Name} 的成员变量 \"{field.Name}\" 必须提供比对函数public int CompareTo(ref {field.DeclaringType.Name} other);");
                    return false;
                }
                var parames = method.GetParameters();
                if (method.ReturnType != typeof(int) || parames.Length != 1 || parames[0].ParameterType != field.FieldType || !parames[0].IsIn)
                {
                    Debug.LogWarning($"{field.DeclaringType.Name} 的成员变量 \"{field.Name}\" 必须提供比对函数public int CompareTo(ref {field.DeclaringType.Name} other);");
                    return false;
                }

                strCompare.Add($"if({field.Name}.CompareTo(ref value) == 0) return;");
                strSet.Add(field.Name + " = value;");
                return true;
            }

            return strSet.Count > 0 && strCompare.Count>0;
        }
        //------------------------------------------------------
        internal static int BuildHashCode(System.Type type)
        {
            if (type == null) return 0;
            return BuildHashCode(type.FullName);
        }
        //------------------------------------------------------
        internal static int BuildHashCode(string typeName)
        {
            typeName = typeName.ToLower();
            int hash = Animator.StringToHash(typeName);
            if (hash >= 0 && hash <= 50000)
            {
                string baseName = typeName;
                string append = "";
                int tryCount = 0;
                while (hash >= -50000 && hash <= 50000 && tryCount < 1000)
                {
                    char ch = (char)('a' + (tryCount % 26));
                    append += ch;
                    hash = Animator.StringToHash(baseName + append);
                    ++tryCount;
                }
            }
            return hash;
        }
        //----------------------------------------
        internal static VisualElement OnDrawProxyDbTypePop(ArvgPort port, IVariable portValue, Action<IVariable> onValueChanged, int width)
        {
            if (portValue == null || portValue.GetVariableType() != EVariableType.eInt)
                return null;
            var ClassCodeMapping = Collects();
            bool canEdit = port.attri.canEdit;

            var intVar = (AT.Runtime.VariableInt)portValue;
            int curIndex = ms_vClsIds.IndexOf(intVar.value);
            if (curIndex < 0) curIndex = 0;

            var popup = new PopupField<string>(ms_vClsNames, curIndex)
            {
                style =
                {
                    width = width,
                    marginLeft = 4,
                    unityTextAlign = UnityEngine.TextAnchor.MiddleRight
                }
            };
            popup.SetEnabled(canEdit);

            popup.RegisterValueChangedCallback(evt =>
            {
                int idx = ms_vClsNames.IndexOf(evt.newValue);
                if (idx >= 0)
                {
                    intVar.value = ms_vClsIds[idx];
                    onValueChanged?.Invoke(intVar);
                }
            });

            return popup;
        }
        //------------------------------------------------------
        internal static void OnEditorInitOnload()
        {
            UserDBPreference setting = EditorFrameworkPreferences.GetSettings().GetCustom<UserDBPreference>();
            if (setting == null || !setting.autoCode)
                return;
            if (string.IsNullOrEmpty(setting.generatorCode))
            {
                Debug.LogWarning("请设置代码生成路径");
                return;
            }
            Build(setting.generatorCode);
        }
    }
    //------------------------------------------------------
    //! 用户db偏好设置
    //------------------------------------------------------
    [System.Serializable, CustomPreference("Preferences/GamePlay","用户Db")]
    internal class UserDBPreference
    {
        public bool autoCode = true;
        public string generatorCode = "";
        internal void OnPreferencesGUI()
        {
            autoCode = EditorGUILayout.Toggle("自动生成代码", autoCode);
            if (autoCode) generatorCode = EditorGUILayout.TextField("代码生成路径", generatorCode);
        }
    }
}
#endif