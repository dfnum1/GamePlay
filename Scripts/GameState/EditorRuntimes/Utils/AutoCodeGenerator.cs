#if UNITY_EDITOR
using Framework.ED;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
namespace Framework.State.Editor
{
    public class AutoCodeGenerator
    {
        static string INTER_EXPORT_PATH = "../Packages/GamePlay/Scripts/GamePlay/GameState/Generators/";
        internal static void AutoCode(string root)
        {
            var stateTypes = StateEditorUtil.GetStateWorldIdTypes();
            Dictionary<int, System.Type> innerStates = new Dictionary<int, Type>();
            Dictionary<int, System.Type> outerStates = new Dictionary<int, Type>();
            foreach (var db in stateTypes)
            {
                if (EditorUtils.IsGamePlayInnerType(db.Value))
                    innerStates[db.Key] = db.Value;
                else
                    outerStates[db.Key] = db.Value;
            }
            if(!string.IsNullOrEmpty(root))AutoCode(root, outerStates, "GameStateTypeRegistry");
            AutoCode(INTER_EXPORT_PATH, outerStates, "GameStateInnerTypeRegistry");
        }
        //-----------------------------------------------------
        internal static void AutoCode(string root, Dictionary<int, System.Type> stateTypes, string className)
        {
            StringBuilder code = new StringBuilder();
            code.AppendLine("// This code is auto-generated. Do not modify.");
            code.AppendLine("using Framework.Core;");
            code.AppendLine("namespace Framework.State.Runtime");
            code.AppendLine("{");
            code.AppendLine("\t[Framework.Base.EditorSetupInit]");
            code.AppendLine("    public static class " + className);
            code.AppendLine("    {");
            code.AppendLine("        public static void Init()");
            code.AppendLine("        {");
            foreach (var kvp in stateTypes)
            {
                code.AppendLine($"            GameWorldHandler.Register({kvp.Key}, MallocTypeClass);");
            }
            code.AppendLine("        }");
            if(stateTypes.Count>0)
            {
                code.AppendLine("        //--------------------------------------------------------");
                code.AppendLine("        static TypeObject MallocTypeClass(int id)");
                code.AppendLine("        {");
                code.AppendLine("            switch(id)");
                code.AppendLine("            {");
                foreach (var kvp in stateTypes)
                {
                    code.AppendLine($"                case {kvp.Key}: return TypeInstancePool.Malloc<{kvp.Value.FullName.Replace("+", ".")}>();");
                }
                code.AppendLine("                default: return null;");
                code.AppendLine("            }");
                code.AppendLine("        }");
            }

            code.AppendLine("    }");
            code.AppendLine("}");

            if (!Directory.Exists(root))
                Directory.CreateDirectory(root);
            string filePath = System.IO.Path.Combine(root, className + ".cs");
            FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate);
            StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.UTF8);
            fs.Position = 0;
            fs.SetLength(0);
            writer.Write(code);
            writer.Close();
        }
    }
}
#endif