#if UNITY_EDITOR
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

public class AssemblyComplier
{
    static ApiCompatibilityLevel apiCompatibilityLevel = ApiCompatibilityLevel.NET_Standard;// ApiCompatibilityLevel.NET_Unity_4_8;
    static float ms_DllComplierWeight = 10f;
    static string ms_OutputDir = Application.dataPath + "/../Plugins/GamePlay/";
    static string ms_InputDir = Application.dataPath + "/../Packages/GamePlay/";
    static Dictionary<string, string> ms_CopyDir = new Dictionary<string,string>();
    static Dictionary<string, string> ms_CopyFile = new Dictionary<string, string>();
    static HashSet<string> ms_vPackageDlls = new HashSet<string>();
    static float ms_fProgress = 0f;
    static string ms_strProcessTile = "";
    [MenuItem("GamePlay/编译发布Package",false, 1000)]
    static void CompileAssembly()
    {
        ms_vPackageDlls.Clear();
   //     ms_vPackageDlls.Add("agenttree");
        ms_vPackageDlls.Add("guidesystem");
        ms_vPackageDlls.Add("gameframework");
        ms_vPackageDlls.Add("gamestate");
        ms_vPackageDlls.Add("gameeditor");
        ms_vPackageDlls.Add("externalengine");
        //      ms_vPackageDlls.Add("cutscene");
        //     ms_vPackageDlls.Add("actorsystem");

        ms_CopyDir.Clear();
        ms_CopyFile.Clear();
        ms_CopyDir["Editor Resources"] = "Editor Resources";
        ms_CopyDir["Package Resources"]= "Package Resources";
        ms_CopyDir["Tests"]= "Tests";
        ms_CopyDir["Scripts/GamePlay"]= "Runtime";
        ms_CopyDir["Scripts/Plugins"] = "Runtime/Plugins";
        ms_CopyFile["package.json"] = "package.json";
        ms_CopyFile["ValidationExceptions.json"] = "ValidationExceptions.json";
        ms_CopyFile["CHANGELOG.md"] = "CHANGELOG.md";
        ms_CopyFile["LICENSE.md"] = "LICENSE.md";
        ms_fProgress = 0;
        ms_strProcessTile = "编译编辑态dll";
        OnEditorUpdate(0, true);
        ms_DllComplierWeight = 80.0f / (ms_vPackageDlls.Count*2);

        BuildEditorDll((result) => {
            if(result)
            {
                ms_strProcessTile = "编译运行态dll";
                BuildDll((resutl1) => {
                    CopyPackageDatas();
                    OnEditorUpdate(100, true);
                    EditorUtility.DisplayDialog("提示", (result && resutl1) ? "DLL编译成功" : "DLL编译失败", "确定");
                });
            }
            else
            {
                EditorUtility.DisplayDialog("提示", "DLL编译失败", "确定");
                OnEditorUpdate(100, true);
            }
        });
    }
    //--------------------------------------------------------
    [MenuItem("GamePlay/编译发布Package", true, 1000)]
    bool IsValidMenu()
    {
        return !Framework.ED.EditorUtils.IsInstallFromPackage();
    }
    //--------------------------------------------------------
    static void OnEditorUpdate(float process, bool bAppend = true)
    {
        if (bAppend)
            ms_fProgress += process;
        else ms_fProgress = Mathf.Max(ms_fProgress, process);
            EditorUtility.DisplayProgressBar(ms_strProcessTile, "", Mathf.Clamp01(ms_fProgress / 100.0f));
        if (ms_fProgress >= 100.0f)
        {
            EditorUtility.ClearProgressBar();
        }
    }
    //--------------------------------------------------------
    static void CopyPackageDatas()
    {
        ms_strProcessTile = "复制Package资源文件";
        OnEditorUpdate(80,false);
        foreach (var db in ms_CopyDir)
        {
            Framework.ED.EditorUtils.CopyDir(ms_InputDir + db.Key, ms_OutputDir+db.Value);
            if(File.Exists(ms_InputDir+db.Key + ".meta"))
            {
                Framework.ED.EditorUtils.CopyFile(ms_InputDir + db.Key + ".meta", ms_OutputDir + db.Value + ".meta");
            }
        }
        foreach (var db in ms_CopyFile)
        {
            Framework.ED.EditorUtils.CopyFile(ms_InputDir + db.Key, ms_OutputDir + db.Value);
            if (File.Exists(ms_InputDir + db.Key + ".meta"))
            {
                Framework.ED.EditorUtils.CopyFile(ms_InputDir + db.Key + ".meta", ms_OutputDir + db.Value + ".meta");
            }
        }

        ms_strProcessTile = "生成GamePlay 程序集的引用关系";
        OnEditorUpdate(85, false);
        //GamePlay.asmdef
        //帮我把这个文件中的references 内容替换为editorRefs 和 runtimeRefs 的内容
        string[] editorDlls = new string[] {
            ms_OutputDir + "Runtime/Plugins/ExternEngineEditor.dll",
            ms_OutputDir + "Runtime/Plugins/GameFrameworkEditor.dll",
            //ms_OutputDir + "Runtime/Plugins/AgentTreeEditor.dll",
            //ms_OutputDir + "Runtime/Plugins/CutsceneEditor.dll",
            //ms_OutputDir + "Runtime/Plugins/ActorSystemEditor.dll",
            ms_OutputDir + "Runtime/Plugins/GameStateEditor.dll",
            ms_OutputDir + "Runtime/Plugins/GuideSystemEditor.dll"
        };
        string[] runtimeDlls = new string[] {
            ms_OutputDir + "Runtime/Plugins/ExternEngine.dll",
            ms_OutputDir + "Runtime/Plugins/GameFramework.dll",
            //ms_OutputDir + "Runtime/Plugins/AgentTree.dll",
            //ms_OutputDir + "Runtime/Plugins/Cutscene.dll",
            //ms_OutputDir + "Runtime/Plugins/ActorSystem.dll",
            ms_OutputDir + "Runtime/Plugins/GameState.dll",
            ms_OutputDir + "Runtime/Plugins/GuideSystem.dll"
        };
        string[] editorRefs = GetDllGuids(editorDlls);
        string[] runtimeRefs = GetDllGuids(runtimeDlls);
        string asmdefPath = ms_OutputDir + "Runtime/GamePlay.asmdef";
        if (File.Exists(asmdefPath))
        {
            try
            {
                string content = File.ReadAllText(asmdefPath);

                // 构造新的 references 字段内容
                var allRefs = editorRefs.Concat(runtimeRefs)
                    .Select(r => $"    \"{r}\"")
                    .ToArray();
                string refsBlock = "\"references\": [\n" + string.Join(",\n", allRefs) + "\n  ]";

                // 用正则替换 references 字段
                string pattern = "\"references\"\\s*:\\s*\\[[^\\]]*\\]";
                string replaced = System.Text.RegularExpressions.Regex.Replace(
                    content, pattern, refsBlock, System.Text.RegularExpressions.RegexOptions.Multiline);

                File.WriteAllText(asmdefPath, replaced);
                UnityEngine.Debug.Log("已用正则替换 references 字段: " + asmdefPath);
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogError("asmdef references 替换失败: " + ex.Message);
            }
        }
        ms_strProcessTile = "生成调试mdb";
        OnEditorUpdate(95, false);
        //! 将Plugins/GamePlay/Runtime/Plugins 目录下的pdb 文件，Editor.pdb 结尾的调用Tools~/pdb2mdb.exe 转换为 mdb 文件。其他的pdb文件直接删除
        string frameworkDir = ms_OutputDir + "Runtime/Plugins/";
        string pdb2mdbExe = GetPdb2MonoExePath();
        if (Directory.Exists(frameworkDir) && File.Exists(pdb2mdbExe))
        {
            var pdbFiles = Directory.GetFiles(frameworkDir, "*.pdb", SearchOption.TopDirectoryOnly);
            foreach (var pdb in pdbFiles)
            {
                string fileName = Path.GetFileName(pdb);
                string dllPath = Path.ChangeExtension(pdb, ".dll");
                bool isEditorPdb = false;
                if (fileName.EndsWith("Editor.pdb", System.StringComparison.OrdinalIgnoreCase) && File.Exists(dllPath))
                {
                    isEditorPdb = true;
                    /*
                    var process = new Process();
                    process.StartInfo.FileName = pdb2mdbExe;
                    process.StartInfo.Arguments = $"\"{dllPath}\"";
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();
                    if (process.ExitCode != 0)
                    {
                        UnityEngine.Debug.LogError($"pdb2mdb 转换失败: {pdb}\n{error}");
                    }
                    else
                    {
                        UnityEngine.Debug.Log($"pdb2mdb 转换成功: {pdb}\n{output}");
                    }*/
                }
                // 删除所有pdb
                try
                {
                    if(!isEditorPdb)
                        File.Delete(pdb);
                }
                catch (System.Exception ex)
                {
                    UnityEngine.Debug.LogError($"删除PDB失败: {pdb}\n{ex.Message}");
                }
            }
        }
        OnEditorUpdate(100, false);
    }
    //--------------------------------------------------------
    static string GetDllGuid(string dllPath)
    {
        string metaPath = dllPath + ".meta";
        if (!File.Exists(metaPath))
            return null;
        var lines = File.ReadAllLines(metaPath);
        foreach (var line in lines)
        {
            if (line.StartsWith("guid:"))
            {
                return "GUID:" + line.Substring(5).Trim();
            }
        }
        return null;
    }
    //--------------------------------------------------------
    static string[] GetDllGuids(string[] dllPaths)
    {
        var guids = new List<string>();
        foreach (var dll in dllPaths)
        {
            var guid = GetDllGuid(dll);
            if (!string.IsNullOrEmpty(guid))
                guids.Add(guid);
        }
        return guids.ToArray();
    }
    //--------------------------------------------------------
    static string GetPdb2MonoExePath()
    {
        string unityRoot = EditorApplication.applicationContentsPath;
        string[] possiblePaths = new string[] {
            Path.Combine(unityRoot, "MonoBleedingEdge", "lib", "mono", "4.5", "pdb2mdb.exe"),
            Path.Combine(unityRoot, "MonoBleedingEdge", "lib", "mono", "2.0", "pdb2mdb.exe"),
            Path.Combine(unityRoot, "MonoBleedingEdge", "bin", "pdb2mdb.exe"),
            Path.Combine(unityRoot, "MonoBleedingEdge", "pdb2mdb.exe")
        };
        string pdb2mdbExe = null;
        foreach (var path in possiblePaths)
        {
            if (File.Exists(path))
            {
                pdb2mdbExe = path;
                break;
            }
        }
        if(pdb2mdbExe == null)
            pdb2mdbExe = ms_InputDir + "Tools~/pdb2mdb.exe";
        return pdb2mdbExe;
    }
    //--------------------------------------------------------
    static void BuildEditorDll(System.Action<bool> onCallback)
    {
        ComplierExternalDll((bExternSucceed) => {
            if(bExternSucceed)
            {
                Framework.ED.EditorUtils.CopyFile(ms_OutputDir + "Runtime/Plugins/ExternEngineEditor.dll", ms_InputDir + "Scripts/Plugins/ExternEngineEditor.dll");
            }
            ComplierDll((bSucceed) => {
                OnEditorUpdate(ms_DllComplierWeight);
                if (bSucceed)
                {
                    OnEditorUpdate(ms_DllComplierWeight);
                    ComplierDll((succeed1) =>
                    {
                        OnEditorUpdate(ms_DllComplierWeight);
                        if (succeed1)
                        {
                            ComplierDll((succeed2) =>
                            {
                                if (succeed2)
                                {
                                    ComplierDll((succeed3) =>
                                    {
                                        onCallback(succeed3);
                                    },
                                    ms_InputDir + "Scripts/GameEditor", ms_OutputDir + "Runtime/Plugins/GameEditor.dll", "UNITY_EDITOR",
                                    ms_OutputDir + "Runtime/Plugins/ExternEngineEditor.dll",
                                    ms_OutputDir + "Runtime/Plugins/GameFrameworkEditor.dll",
                                    ms_OutputDir + "Runtime/Plugins/GameStateEditor.dll");
                                }
                                else
                                    onCallback(succeed2);
                            },
                            ms_InputDir + "Scripts/GameState", ms_OutputDir + "Runtime/Plugins/GameStateEditor.dll", "UNITY_EDITOR",
                            ms_OutputDir + "Runtime/Plugins/ExternEngineEditor.dll", 
                            ms_OutputDir + "Runtime/Plugins/GameFrameworkEditor.dll");
                        }
                        else
                        {
                            onCallback(false);
                        }
                    },
                    ms_InputDir + "Scripts/GuideSystem", ms_OutputDir + "Runtime/Plugins/GuideSystemEditor.dll", "UNITY_EDITOR",
                    ms_OutputDir + "Runtime/Plugins/ExternEngineEditor.dll",
                    ms_OutputDir + "Runtime/Plugins/GameFrameworkEditor.dll");
                }
                else
                    onCallback(false);
            }, ms_InputDir + "Scripts/GameFramework", ms_OutputDir + "Runtime/Plugins/GameFrameworkEditor.dll", "UNITY_EDITOR",
        ms_OutputDir + "Runtime/Plugins/ExternEngineEditor.dll",
        ms_OutputDir + "Runtime/Plugins/ChnCharInfo.dll");
        }, new string[] {
            ms_InputDir + "../../Plugins/ExternEngine/Properties",
            ms_InputDir + "../../Plugins/ExternEngine/Source"
        }, ms_OutputDir + "Runtime/Plugins/ExternEngineEditor.dll", "UNITY_EDITOR;USE_FIXEDMATH");
    }
    //--------------------------------------------------------
    static void BuildDll(System.Action<bool> onCallback)
    {
        ComplierExternalDll((bExternSucceed) => {
            if (bExternSucceed)
            {
                Framework.ED.EditorUtils.CopyFile(ms_OutputDir + "Runtime/Plugins/ExternEngine.dll", ms_InputDir + "Scripts/Plugins/ExternEngine.dll");
            }
            ComplierDll((bSucceed) => {
            OnEditorUpdate(ms_DllComplierWeight);
            if (bSucceed)
            {
                OnEditorUpdate(ms_DllComplierWeight);
                ComplierDll((succeed4) =>
                {
                    if(succeed4)
                    {
                        OnEditorUpdate(ms_DllComplierWeight);
                        ComplierDll((succeed4) =>
                        {
                            onCallback(succeed4);
                        },
                        ms_InputDir + "Scripts/GameState", ms_OutputDir + "Runtime/Plugins/GameState.dll", "",
                        ms_OutputDir + "Runtime/Plugins/ExternEngine.dll", 
                        ms_OutputDir + "Runtime/Plugins/GameFramework.dll");
                    }
                    else
                        onCallback(false);
                },
                ms_InputDir + "Scripts/GuideSystem", ms_OutputDir + "Runtime/Plugins/GuideSystem.dll", "", 
                ms_OutputDir + "Runtime/Plugins/ExternEngine.dll",
                ms_OutputDir + "Runtime/Plugins/GameFramework.dll");
            }
            else
                onCallback(false);
        }, ms_InputDir + "Scripts/GameFramework", ms_OutputDir + "Runtime/Plugins/GameFramework.dll","",
        ms_OutputDir + "Runtime/Plugins/ExternEngine.dll");
        }, new string[] {
            ms_InputDir + "../../Plugins/ExternEngine/Properties",
            ms_InputDir + "../../Plugins/ExternEngine/Source"
        }, ms_OutputDir + "Runtime/Plugins/ExternEngine.dll", "USE_FIXEDMATH");
    }
    //--------------------------------------------------------
    public static void ComplierDll(System.Action<bool> onCallback, string sourceDir, string outputDll, string defineSymbols = "", params string[] referenceDlls)
    {
        ms_strProcessTile = "编译dll：" + Path.GetFileNameWithoutExtension(outputDll);
        OnEditorUpdate(0);
        var scripts = Directory.GetFiles(sourceDir, "*.cs", SearchOption.AllDirectories);
        if (scripts.Length == 0)
        {
            UnityEngine.Debug.LogError("未找到C#文件: " + sourceDir);
            return;
        }
        var references = new List<string>();
        var unityEnginePath = Path.Combine(EditorApplication.applicationContentsPath, "Managed");
        var unityEditorPath = Path.Combine(EditorApplication.applicationContentsPath, "Managed");
        if (defineSymbols.Contains("UNITY_EDITOR"))
        {
            references.Add(Path.Combine(unityEditorPath, "UnityEngine/UnityEditor.UIElementsModule.dll"));
            references.Add(Path.Combine(unityEditorPath, "UnityEngine/UnityEditor.CoreModule.dll"));
            references.Add(Path.Combine(unityEditorPath, "UnityEngine/UnityEditor.GraphViewModule.dll"));
            references.Add(Path.Combine(unityEditorPath, "UnityEngine/UnityEditor.dll"));
            references.Add(Path.Combine(unityEditorPath, "UnityEditor.dll"));
        }

        if (referenceDlls != null)
        {
            references.AddRange(referenceDlls);
        }

        var builder = new AssemblyBuilder(outputDll, scripts)
        {
            buildTarget = EditorUserBuildSettings.activeBuildTarget,
            buildTargetGroup = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget),
            additionalDefines = defineSymbols.Split(';').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray(),
            additionalReferences = references.ToArray()
        };
        builder.compilerOptions.AllowUnsafeCode = true;
        builder.referencesOptions = ReferencesOptions.UseEngineModules;
         builder.compilerOptions.ApiCompatibilityLevel = apiCompatibilityLevel;

        List<string> excludeReferences = new List<string>();
        if (referenceDlls != null && referenceDlls.Length>0)
        {
            Dictionary<string, string> vRefDlls = new Dictionary<string, string>();
            for(int i =0; i < builder.defaultReferences.Length; ++i)
            {
                string name = Path.GetFileNameWithoutExtension(builder.defaultReferences[i]).ToLower();
                vRefDlls[name] = builder.defaultReferences[i];
              //  if(builder.defaultReferences[i].StartsWith("Library/ScriptAssemblies"))
                if(ms_vPackageDlls.Contains(name))
                {
                    excludeReferences.Add(builder.defaultReferences[i]);
                }
            }
            for(int i =0; i < referenceDlls.Length; ++i)
            {
                string name = Path.GetFileNameWithoutExtension(referenceDlls[i]);
                if (!name.EndsWith("Editor"))
                    continue;
                name = name.Substring(0, name.Length - "Editor".Length).ToLower();
                if (vRefDlls.TryGetValue(name, out var dllPath))
                {
                    if(!excludeReferences.Contains(dllPath))
                        excludeReferences.Add(dllPath);
                }
            }
        }
        if(excludeReferences.Count>0)
            builder.excludeReferences = excludeReferences.ToArray();


        string dllName = Path.GetFileNameWithoutExtension(outputDll);

        builder.buildFinished += (path, result) =>
        {
            bool bSucceed = true;
            for(int i =0; i < result.Length; ++i)
            {
                if (result[i].type == CompilerMessageType.Error)
                {
                    bSucceed = false;
                    try
                    {
                        UnityEngine.Debug.LogError(dllName + "  编译失败: " + path + "\n" + string.Join("\n", result[i].message));
                    }
                    catch
                    {

                    }
                }
            }
            if(bSucceed)
            {
                UnityEngine.Debug.Log(dllName + "  DLL编译成功");
            }
            onCallback(bSucceed);
        };

        if (!builder.Build())
        {
            UnityEngine.Debug.LogError(dllName + "  编译启动失败");
            onCallback(false);
        }
    }
    //--------------------------------------------------------
    public static void ComplierExternalDll(System.Action<bool> onCallback, string[] sourceDirs, string outputDll, string defineSymbols = "", params string[] referenceDlls)
    {
        ms_strProcessTile = "编译dll：" + Path.GetFileNameWithoutExtension(outputDll);
        OnEditorUpdate(0);
        List<string> scripts = new List<string>();
        for(int i =0; i < sourceDirs.Length; ++i)
        {
            var temps = Directory.GetFiles(sourceDirs[i], "*.cs", SearchOption.AllDirectories);
            scripts.AddRange(temps);
        }
        if (scripts.Count == 0)
        {
            UnityEngine.Debug.LogError("未找到C#文件");
            return;
        }
        var references = new List<string>();
        var unityEnginePath = Path.Combine(EditorApplication.applicationContentsPath, "Managed");
        var unityEditorPath = Path.Combine(EditorApplication.applicationContentsPath, "Managed");
        if (defineSymbols.Contains("UNITY_EDITOR"))
        {
            references.Add(Path.Combine(unityEditorPath, "UnityEngine/UnityEditor.dll"));
            references.Add(Path.Combine(unityEditorPath, "UnityEditor.dll"));
        }

        if (referenceDlls != null)
        {
            references.AddRange(referenceDlls);
        }

        var builder = new AssemblyBuilder(outputDll, scripts.ToArray())
        {
            buildTarget = EditorUserBuildSettings.activeBuildTarget,
            buildTargetGroup = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget),
            additionalDefines = defineSymbols.Split(';').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray(),
            additionalReferences = references.ToArray()
        };
        builder.compilerOptions.AllowUnsafeCode = true;
        builder.referencesOptions = ReferencesOptions.UseEngineModules;
        builder.compilerOptions.ApiCompatibilityLevel = apiCompatibilityLevel;

        List<string> excludeReferences = new List<string>();
        if (referenceDlls != null && referenceDlls.Length > 0)
        {
            Dictionary<string, string> vRefDlls = new Dictionary<string, string>();
            for (int i = 0; i < builder.defaultReferences.Length; ++i)
            {
                string name = Path.GetFileNameWithoutExtension(builder.defaultReferences[i]).ToLower();
                vRefDlls[name] = builder.defaultReferences[i];
                //  if(builder.defaultReferences[i].StartsWith("Library/ScriptAssemblies"))
                if (ms_vPackageDlls.Contains(name))
                {
                    excludeReferences.Add(builder.defaultReferences[i]);
                }
            }
            for (int i = 0; i < referenceDlls.Length; ++i)
            {
                string name = Path.GetFileNameWithoutExtension(referenceDlls[i]);
                if (!name.EndsWith("Editor"))
                    continue;
                name = name.Substring(0, name.Length - "Editor".Length).ToLower();
                if (vRefDlls.TryGetValue(name, out var dllPath))
                {
                    if (!excludeReferences.Contains(dllPath))
                        excludeReferences.Add(dllPath);
                }
            }
        }
        if (excludeReferences.Count > 0)
            builder.excludeReferences = excludeReferences.ToArray();


        string dllName = Path.GetFileNameWithoutExtension(outputDll);

        builder.buildFinished += (path, result) =>
        {
            bool bSucceed = true;
            for (int i = 0; i < result.Length; ++i)
            {
                if (result[i].type == CompilerMessageType.Error)
                {
                    bSucceed = false;
                    try
                    {
                        UnityEngine.Debug.LogError(dllName + "  编译失败: " + path + "\n" + string.Join("\n", result[i].message));
                    }
                    catch
                    {

                    }
                }
            }
            if (bSucceed)
            {
                UnityEngine.Debug.Log(dllName + "  DLL编译成功");
            }
            onCallback(bSucceed);
        };

        if (!builder.Build())
        {
            UnityEngine.Debug.LogError(dllName + "  编译启动失败");
            onCallback(false);
        }
    }
}
#endif