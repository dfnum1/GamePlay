/********************************************************************
生成日期:		11:03:2020
类    名: 	EditorUtils
作    者:	HappLI
描    述:	编辑器工具类
*********************************************************************/

#if UNITY_EDITOR
using Framework.DrawProps;
using Microsoft.International.Converters.PinYinConverter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
namespace Framework.ED
{
    public class EditorUtils
    {
        const string PACKAGE_NAME = "com.iggrd1.gameplay";
        private static bool ms_LoadUnityPluginCheck = false;
        private static System.Reflection.MethodInfo ms_pLoadUnityPlugin = null;
        //-----------------------------------------------------
        static void EditorPluginInit()
        {
            if (ms_LoadUnityPluginCheck)
                return;
            ms_LoadUnityPluginCheck =true;
            if (ms_pLoadUnityPlugin == null)
            {
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
                        if (tp.IsDefined(typeof(EditorLoaderAttribute), false))
                        {
                            var clipAttri = tp.GetCustomAttribute<EditorLoaderAttribute>();
                            ms_pLoadUnityPlugin = tp.GetMethod(clipAttri.method, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                            break;
                        }
                    }
                }
            }
        }
        //-----------------------------------------------------
        public static UnityEngine.Object EditLoadUnityObject(string file)
        {
            return EditLoadUnityObject<UnityEngine.Object>(file);
        }
        //-----------------------------------------------------
        public static T EditLoadUnityObject<T>(string file) where T : UnityEngine.Object
        {
            EditorPluginInit();
            if (ms_pLoadUnityPlugin != null)
            {
                var returnObj = ms_pLoadUnityPlugin.Invoke(null, new object[] { file });
                if (returnObj != null && returnObj is T)
                    return returnObj as T;
            }
            return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(file);
        }
        //-----------------------------------------------------
        public static UnityEngine.Object EditLoadUnityObject(string file, System.Type type) 
        {
            EditorPluginInit();
            if (ms_pLoadUnityPlugin != null)
            {
                var returnObj = ms_pLoadUnityPlugin.Invoke(null, new object[] { file });
                if (returnObj != null && returnObj.GetType().IsSubclassOf(type))
                    return returnObj as UnityEngine.Object;
            }
            return UnityEditor.AssetDatabase.LoadAssetAtPath(file,type);
        }
        //-------------------------------------------------
        static Dictionary<string, System.Type> ms_vBindTypes = null;
        public static System.Type GetTypeByName(string typeName)
        {
            if (string.IsNullOrEmpty(typeName)) return null;
            if (ms_vBindTypes == null)
            {
                ms_vBindTypes = new Dictionary<string, System.Type>();
                foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
                {
                    Type[] types = null;
                    try
                    {
                        types = assembly.GetTypes();
                    }
                    catch (ReflectionTypeLoadException ex)
                    {
                        types = ex.Types; // 部分可用类型
                                          // 可选：输出警告
                        UnityEngine.Debug.LogWarning($"加载程序集 {assembly.FullName} 时部分类型无法加载: {ex}");
                    }
                    catch (Exception ex)
                    {
                        UnityEngine.Debug.LogWarning($"加载程序集 {assembly.FullName} 时发生异常: {ex}");
                        continue;
                    }
                    for (int i = 0; i < types.Length; ++i)
                    {
                        System.Type tp = types[i];
                        if (tp == null)
                            continue;
                        if (tp.IsDefined(typeof(DrawProps.BinderTypeAttribute), false))
                        {
                            DrawProps.BinderTypeAttribute attr = (DrawProps.BinderTypeAttribute)tp.GetCustomAttribute(typeof(DrawProps.BinderTypeAttribute));
                            ms_vBindTypes[attr.bindName] = tp;
                        }
                    }
                }
            }
            System.Type returnType;
            if (ms_vBindTypes.TryGetValue(typeName, out returnType))
                return returnType;
            returnType = Type.GetType(typeName);
            if (returnType != null) return returnType;
            try
            {
                foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
                {
                    returnType = assembly.GetType(typeName, false, true);
                    if (returnType != null) return returnType;
                }
                int index = typeName.LastIndexOf(".");
                if (index > 0 && index + 1 < typeName.Length)
                {
                    string strTypeName = typeName.Substring(0, index) + "+" + typeName.Substring(index + 1, typeName.Length - index - 1);
                    foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
                    {
                        returnType = assembly.GetType(strTypeName, false, true);
                        if (returnType != null) return returnType;
                    }
                }
            }
            catch
            {

            }

            return null;
        }
        //-----------------------------------------------------
        public static string PinYin(string chinese)
        {
            string retValue = string.Empty;

            foreach (char chr in chinese)
            {
                try
                {
                    if (ChineseChar.IsValidChar(chr))
                    {
                        ChineseChar chineseChar = new ChineseChar(chr);
                        string t = chineseChar.Pinyins[0].ToString().ToLower();
                        retValue += t.Substring(0, t.Length - 1);
                    }
                    else
                    {
                        retValue += chr.ToString();
                    }
                }
                catch (Exception e)
                {
                    return chinese;
                }
            }

            return retValue;
        }
        //-----------------------------------------------------
        public static string GetInstallPath()
        {
            return GetPackagePath(PACKAGE_NAME);
        }
        //-----------------------------------------------------
        public static string GetInstallEditorResourcePath()
        {
            string path = GetPackageAssetPath(PACKAGE_NAME);
            if(string.IsNullOrEmpty(path))
            {
                return Path.Combine(Application.dataPath, "Editor Resources");
            }
            return Path.Combine(path, "Editor Resources");
        }
        //-----------------------------------------------------
        public static string GetPackagePath(string packageName)
        {
            var packages = UnityEditor.PackageManager.PackageInfo.GetAllRegisteredPackages();
            foreach (var pkg in packages)
            {
                if (pkg.name == packageName)
                {
                    return pkg.resolvedPath; // 绝对路径
                }
            }
            return null;
        }
        //-----------------------------------------------------
        public static string GetPackageAssetPath(string packageName)
        {
            var packages = UnityEditor.PackageManager.PackageInfo.GetAllRegisteredPackages();
            foreach (var pkg in packages)
            {
                if (pkg.name == packageName)
                {
                    return pkg.assetPath; // 绝对路径
                }
            }
            return null;
        }
        //-----------------------------------------------------
        public static void CollectAllChildPaths(Transform parent, string parentPath, List<string> pathList)
        {
            foreach (Transform child in parent)
            {
                string path = string.IsNullOrEmpty(parentPath) ? child.name : parentPath + "/" + child.name;
                pathList.Add(path);
                CollectAllChildPaths(child, path, pathList);
            }
        }
        //------------------------------------------------------
        public static string GetEnumDisplayName(System.Enum curVar)
        {
            FieldInfo fi = curVar.GetType().GetField(curVar.ToString());
            string strTemName = curVar.ToString();
            if (fi != null && fi.IsDefined(typeof(Framework.DrawProps.DisplayAttribute)))
            {
                strTemName = fi.GetCustomAttribute<Framework.DrawProps.DisplayAttribute>().displayName;
            }
            if (fi != null && fi.IsDefined(typeof(InspectorNameAttribute)))
            {
                strTemName = fi.GetCustomAttribute<InspectorNameAttribute>().displayName;
            }
            return strTemName;
        }
        //------------------------------------------------------
        public static string GetEnumDisplayName(Type enumType, int value)
        {
            string name = Enum.GetName(enumType, value);
            if (name == null) return value.ToString();
            FieldInfo fi = enumType.GetField(name);
            if (fi != null && fi.IsDefined(typeof(DisplayAttribute), false))
            {
                var attr = fi.GetCustomAttribute<DisplayAttribute>();
                if (!string.IsNullOrEmpty(attr.displayName))
                    return attr.displayName;
            }
            return name;
        }
        //------------------------------------------------------
        public static void Destroy(UnityEngine.Object go)
        {
            if (Application.isPlaying) UnityEngine.Object.Destroy(go);
            else UnityEngine.Object.DestroyImmediate(go);
        }
        //-----------------------------------------------------
        public static Texture2D GetFloorTexture()
        {
            return LoadEditorResource<Texture2D>("Editor/ground.png");
        }
        //-----------------------------------------------------
        public static T LoadEditorResource<T>(string path) where T : UnityEngine.Object
        {
            string install = GetInstallEditorResourcePath();
            if (string.IsNullOrEmpty(install)) return null;
            string fullPath = install + "/" + path;
            if (!File.Exists(fullPath))
                return null;
            return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(fullPath);
        }
        //------------------------------------------------------
        public static Texture2D GenerateGridTexture(Color line, Color bg)
        {
            Texture2D tex = new Texture2D(64, 64);
            Color[] cols = new Color[64 * 64];
            for (int y = 0; y < 64; y++)
            {
                for (int x = 0; x < 64; x++)
                {
                    Color col = bg;
                    if (y % 16 == 0 || x % 16 == 0) col = Color.Lerp(line, bg, 0.65f);
                    if (y == 63 || x == 63) col = Color.Lerp(line, bg, 0.35f);
                    cols[(y * 64) + x] = col;
                }
            }
            tex.SetPixels(cols);
            tex.wrapMode = TextureWrapMode.Repeat;
            tex.filterMode = FilterMode.Bilinear;
            tex.name = "Grid";
            tex.Apply();
            return tex;
        }
        //------------------------------------------------------
        public static Texture2D GenerateCrossTexture(Color line)
        {
            Texture2D tex = new Texture2D(64, 64);
            Color[] cols = new Color[64 * 64];
            for (int y = 0; y < 64; y++)
            {
                for (int x = 0; x < 64; x++)
                {
                    Color col = line;
                    if (y != 31 && x != 31) col.a = 0;
                    cols[(y * 64) + x] = col;
                }
            }
            tex.SetPixels(cols);
            tex.wrapMode = TextureWrapMode.Clamp;
            tex.filterMode = FilterMode.Bilinear;
            tex.name = "Grid";
            tex.Apply();
            return tex;
        }
        //------------------------------------------------------
        public static bool IsMp4ByHeader(string filePath)
        {
            if (!File.Exists(filePath)) return false;
            byte[] buffer = new byte[32];
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                fs.Read(buffer, 0, buffer.Length);
            }
            string header = System.Text.Encoding.ASCII.GetString(buffer);
            return header.Contains("ftyp");
        }
        //------------------------------------------------------
        public static float GetVideoDuration(string filePath)
        {
            if (!File.Exists(filePath)) return 0;
            Assembly TagLibSharp = null;
            foreach (var ass in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                if(ass.GetName().Name == "TagLibSharp")
                {
                    TagLibSharp = ass;
                    break;
                }
            }
            if(TagLibSharp == null)
            {
                string dllFile = "Assets/Plugins/TagLibSharp.dll";
                if (!File.Exists(dllFile))
                {
                    string[] guids = UnityEditor.AssetDatabase.FindAssets("TagLibSharp t:Dll");
                    foreach (string guid in guids)
                    {
                        string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                        if (path.EndsWith("TagLibSharp.dll", StringComparison.OrdinalIgnoreCase))
                        {
                            dllFile = path;
                            break;
                        }
                    }
                }

                if (!File.Exists(dllFile))
                    return 0;

                TagLibSharp = Assembly.LoadFrom(dllFile);
            }
            
            if (TagLibSharp == null) return 0;

            // 获取 TagLib.File 类型
            var fileType = TagLibSharp.GetType("TagLib.File");
            if (fileType == null) return 0;

            // 调用静态方法 File.Create
            var createMethod = fileType.GetMethod("Create", new[] { typeof(string) });
            if (createMethod == null) return 0;

            var tfile = createMethod.Invoke(null, new object[] { filePath });
            if (tfile == null) return 0;

            // 获取 Properties 属性
            var propertiesProp = fileType.GetProperty("Properties");
            var properties = propertiesProp.GetValue(tfile);

            // 获取 Duration 属性
            var durationProp = properties.GetType().GetProperty("Duration");
            var duration = (TimeSpan)durationProp.GetValue(properties);

            return (float)duration.TotalSeconds;
        }
        //------------------------------------------------------
        public static string GetEditorResourcePath(string scriptLabel, string resSubPath = "EditorResources")
        {
            string[] scripts = AssetDatabase.FindAssets($"t:Script {scriptLabel}");
            if (scripts.Length > 0)
            {
                string installPath = System.IO.Path.GetDirectoryName(UnityEditor.AssetDatabase.GUIDToAssetPath(scripts[0])).Replace("\\", "/");

                installPath = Path.Combine(installPath, resSubPath).Replace("\\", "/");
                if (System.IO.Directory.Exists(installPath))
                {
                    return installPath;
                }
            }
            return null;
        }
        //------------------------------------------------------
        public static string DrawUIObjectByPath<T>(string label, string strFile, bool bClear = true, Action onDel = null) where T : UnityEngine.Object
        {
            return DrawUIObjectByPath<T>(label,strFile,bClear, onDel);
        }
        //------------------------------------------------------
        public static string DrawUIObjectByPath<T>(GUIContent label, string strFile, bool bClear = true, Action onDel = null) where T : UnityEngine.Object
        {
            Color color = GUI.color;
            T asset = AssetDatabase.LoadAssetAtPath<T>(strFile);
            if (asset == null)
            {
                GUI.color = Color.red;
            }
            EditorGUILayout.BeginHorizontal();
            asset = EditorGUILayout.ObjectField(label, asset, typeof(T), false) as T;
            if (asset != null)
                strFile = AssetDatabase.GetAssetPath(asset);
            if (bClear && GUILayout.Button("清除", new GUILayoutOption[] { GUILayout.Width(50) }))
            {
                strFile = "";
            }
            if (onDel != null)
            {
                if (GUILayout.Button("清除", new GUILayoutOption[] { GUILayout.Width(50) }))
                {
                    onDel();
                }
            }
            EditorGUILayout.EndHorizontal();
            GUI.color = color;
            return strFile;
        }
        //------------------------------------------------------
        public static string DrawUIObjectByPath(string label, string strFile, System.Type type, bool bClear = true, Action onDel = null)
        {
            return DrawUIObjectByPath(new GUIContent(label), strFile, type, bClear, onDel);
        }
        //------------------------------------------------------
        public static string DrawUIObjectByPath(GUIContent label, string strFile, System.Type type, bool bClear = true, Action onDel = null)
        {
            Color color = GUI.color;
            var asset = EditLoadUnityObject(strFile, type);
            if (asset == null)
            {
                GUI.color = Color.red;
            }
            EditorGUILayout.BeginHorizontal();
            asset = EditorGUILayout.ObjectField(label, asset, type, false);
            if (asset != null)
                strFile = AssetDatabase.GetAssetPath(asset);
            if (bClear && GUILayout.Button("清除", new GUILayoutOption[] { GUILayout.Width(50) }))
            {
                strFile = "";
            }
            if (onDel != null)
            {
                if (GUILayout.Button("清除", new GUILayoutOption[] { GUILayout.Width(50) }))
                {
                    onDel();
                }
            }
            EditorGUILayout.EndHorizontal();
            GUI.color = color;
            return strFile;
        }
        //------------------------------------------------------
        public static T CreateUnityScriptObject<T>() where T : UnityEngine.Object
        {
            var uniType = GetUnityComponentType<T>();
            if (uniType == null)
            {
                UnityEngine.Debug.LogError($"无法找到类型 {typeof(T).FullName} 的子类。");
                return null;
            }
            return ScriptableObject.CreateInstance(uniType) as T;
        }
        //------------------------------------------------------
        public static T AddUnityScriptComponent<T>(GameObject pIns) where T : UnityEngine.Component
        {
            if (pIns == null) return null;
            var uniType = GetUnityComponentType<T>();
            if (uniType == null)
            {
                UnityEngine.Debug.LogError($"无法找到类型 {typeof(T).FullName} 的子类。");
                return null;
            }
            return pIns.AddComponent(uniType) as T;
        }
        //------------------------------------------------------
        public static System.Type GetUnityComponentType<T>() where T : UnityEngine.Object
        {
            foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] types = null;
                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    types = ex.Types; // 部分可用类型
                                      // 可选：输出警告
                    UnityEngine.Debug.LogWarning($"加载程序集 {assembly.FullName} 时部分类型无法加载: {ex}");
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogWarning($"加载程序集 {assembly.FullName} 时发生异常: {ex}");
                    continue;
                }
                for (int i = 0; i < types.Length; ++i)
                {
                    System.Type tp = types[i];
                    if (tp == null)
                        continue;
                    if(tp.IsSubclassOf(typeof(T)) && tp.IsClass && !tp.IsAbstract && !tp.IsInterface)
                    {
                        return tp;
                    }
                }
            }
            return null;
        }
        //------------------------------------------------------
        public static void CommitGit(string commitFile, bool bCommitListFile = false, bool bWait = true)
        {
            if(bCommitListFile)
            {
                if(!File.Exists(commitFile))
                {
                    EditorUtility.DisplayDialog("提交引导修改", "提交列表文件不存在，请检查路径是否正确。", "确定");
                    return;
                }
            }
            else
            {
                if (!File.Exists(commitFile) && !Directory.Exists(commitFile))
                {
                    return;
                }
            }

            string tortoiseGitExe = "TortoiseGitProc.exe";
            string[] possiblePaths = {
    @"C:\Program Files\TortoiseGit\bin\TortoiseGitProc.exe",
    @"C:\Program Files (x86)\TortoiseGit\bin\TortoiseGitProc.exe"
};
            foreach (var path in possiblePaths)
            {
                if (System.IO.File.Exists(path))
                {
                    tortoiseGitExe = path;
                    break;
                }
            }

            //   if(!System.IO.File.Exists(tortoiseGitExe))
            //   {
            //       UnityEngine.Debug.LogError("TortoiseGitProc.exe 未找到，请确保已安装 TortoiseGit 并正确配置路径。");
            //       return;
            //   }
            if (bCommitListFile)
            {
                string commandStr = $"/command:commit /pathfile:\"{commitFile}\"";
                try
                {
                    System.Diagnostics.Process p = new System.Diagnostics.Process();
                    p.StartInfo.FileName = tortoiseGitExe;
                    p.StartInfo.Arguments = commandStr;
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardOutput = bWait;
                    p.StartInfo.WorkingDirectory = Application.dataPath;
                    p.Start();
                    if (bWait)
                    {
                        p.BeginOutputReadLine();
                        p.WaitForExit();
                        int exitCode = p.ExitCode;
                        p.Close();
                        p.Dispose();
                        if (exitCode == 0)
                        {
                            //    if(File.Exists(cacheFile))
                            //       File.Delete(cacheFile);
                            //    EditorUtility.DisplayDialog("提交引导修改", "提交成功。", "确定");
                        }
                        else if (exitCode != 0)
                        {
                            EditorUtility.DisplayDialog("提交引导修改", "提交失败。", "确定");
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    UnityEngine.Debug.LogError($"无法启动 TortoiseGit 提交界面: {ex.Message}");
                }
                return;
            }
            try
            {
                commitFile = commitFile.Replace("\\", "/");
                if(commitFile.StartsWith("Assets/"))
                {
                    commitFile = Application.dataPath.Replace("\\", "/") + "/" + commitFile.Substring(7);
                }
                int commitCode = 0;
                string commandStr = $"/command:commit /path:\"{commitFile}\"";
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = tortoiseGitExe;
                p.StartInfo.Arguments = commandStr;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = bWait;
                p.StartInfo.RedirectStandardError = bWait;
                p.StartInfo.WorkingDirectory = Application.dataPath;
                p.Start();
                if(bWait)
                {
                    string output = p.StandardOutput.ReadToEnd();
                    string error = p.StandardError.ReadToEnd();
                    p.WaitForExit();
                    int exitCode = p.ExitCode;
                    p.Close();
                    p.Dispose();
                    commitCode += exitCode;
                    if (commitCode == 0)
                    {
                        if (!string.IsNullOrEmpty(output))
                            EditorUtility.DisplayDialog("提交引导修改", output, "确定");
                    }
                    else if (commitCode != 0)
                    {
                        if (error == null) error = "";
                        EditorUtility.DisplayDialog("提交引导修改", "提交失败。" + "\r\n" + error, "确定");
                    }
                }
                
            }
            catch
            {
                EditorUtility.DisplayDialog("提交引导修改", "提交失败。", "确定");
            }
        }
        //------------------------------------------------------
        private static List<FileInfo> FindDirFiles(string strDir)
        {
            List<FileInfo> vRets = new List<FileInfo>();
            if (!Directory.Exists(strDir))
                return vRets;

            FindDirFiles(strDir, vRets);

            return vRets;

        }
        //------------------------------------------------------
        public static void FindDirFiles(string strDir, List<FileInfo> vRes)
        {
            if (!Directory.Exists(strDir)) return;

            string[] dir = Directory.GetDirectories(strDir);
            DirectoryInfo fdir = new DirectoryInfo(strDir);
            FileInfo[] file = fdir.GetFiles();
            if (file.Length != 0 || dir.Length != 0)
            {
                foreach (FileInfo f in file)
                {
                    vRes.Add(f);
                }
                foreach (string d in dir)
                {
                    FindDirFiles(d, vRes);
                }
            }
        }
        //------------------------------------------------------
        public static void CopyDir(string srcDir, string destDir, HashSet<string> vFilerExtension = null, HashSet<string> vIgoreExtension = null)
        {
            if (srcDir.Length <= 0 || destDir.Length < 0) return;

            srcDir = srcDir.Replace("\\", "/");
            if (srcDir[srcDir.Length - 1] == '/') srcDir = srcDir.Substring(0, srcDir.Length - 1);
            string[] split = srcDir.Split('/');
            List<string> vPop = new List<string>();
            int preLen = 0;
            for (int i = 0; i < split.Length; ++i)
            {
                if (split[i].CompareTo("..") == 0)
                {
                    vPop.RemoveAt(vPop.Count - 1);
                    continue;
                }
                preLen = srcDir.Length;
                vPop.Add(split[i]);
            }
            srcDir = "";
            foreach (var db in vPop)
            {
                srcDir += db + "/";
            }

            if (!Directory.Exists(srcDir)) return;
            if (!Directory.Exists(destDir))
                Directory.CreateDirectory(destDir);

            destDir = destDir.Replace("\\", "/");
            if (destDir[destDir.Length - 1] != '/') destDir += "/";

            List<FileInfo> vFiles = FindDirFiles(srcDir);

            string tile = "Copy:" + srcDir + "->" + destDir;

            int total = vFiles.Count;
            int cur = 0;
            EditorUtility.DisplayProgressBar(tile, "...", 0);
            foreach (FileInfo db in vFiles)
            {
                string file = db.FullName.Replace("\\", "/").Replace(srcDir, "");
                cur++;
                string extension = Path.GetExtension(file);
                EditorUtility.DisplayProgressBar(tile, file, (float)((float)cur / (float)total));

                if (vIgoreExtension != null && vIgoreExtension.Contains(extension)) continue;
                if (vFilerExtension != null && !vFilerExtension.Contains(extension)) continue;
                string destFile = destDir + file;

                string tmpFolder = Path.GetDirectoryName(destFile);
                if (!Directory.Exists(tmpFolder))
                    Directory.CreateDirectory(tmpFolder);
                File.Copy(db.FullName, destFile,true);
            }
            EditorUtility.ClearProgressBar();
        }
        //------------------------------------------------------
        public static void CopyFile(string srcFile, string destFile)
        {
            if (srcFile.Length <= 0 || destFile.Length < 0) return;

            srcFile = srcFile.Replace("\\", "/");
            if (!File.Exists(srcFile)) return;

            destFile = destFile.Replace("\\", "/");
            if (!Directory.Exists(Path.GetDirectoryName(destFile)))
                Directory.CreateDirectory(Path.GetDirectoryName(destFile));
            File.Copy(srcFile, destFile, true);
        }
        //------------------------------------------------------
        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.SetAttributes(path, FileAttributes.Normal);
                File.Delete(path);
            }
        }
        //------------------------------------------------------
        public static void DeleteDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                return;
            }
            string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            string[] dirs = Directory.GetDirectories(path);
            foreach (string file in files)
            {
                DeleteFile(file);
            }
            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }
            Directory.Delete(path);
        }
        //------------------------------------------------------
        public static void ClearDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                return;
            }
            string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            string[] dirs = Directory.GetDirectories(path);
            foreach (string file in files)
            {
                DeleteFile(file);
            }
        }
        //------------------------------------------------------
        public static void OpenPathInExplorer(string path)
        {
            if (path.Length <= 0f) return;
            System.Diagnostics.Process[] prpgress = System.Diagnostics.Process.GetProcesses();

            string args = "";
            if (!path.Contains(":/") && !path.Contains(":\\"))
            {
                if ((path[0] == '/') || (path[0] == '\\'))
                    path = Application.dataPath.Substring(0, Application.dataPath.Length - "/Assets".Length) + path;
                else
                    path = Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length) + path;
            }

            args = path.Replace(":/", ":\\");
            args = args.Replace("/", "\\");
            if (path.Contains("."))
            {
                args = string.Format("/Select, \"{0}\"", args);
            }
            else
            {
                if (args[args.Length - 1] != '\\')
                {
                    args += "\\";
                }
            }
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            System.Diagnostics.Process.Start("Explorer.exe", args);
#elif UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
            Debug.Log("IOS 打包路径: " + path);
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("open", path));
#endif
        }
        //------------------------------------------------------
        public static void RepaintPlayModeView()
        {
            var unityEditorAssembly = typeof(AudioImporter).Assembly;
            var audioUtilClass = unityEditorAssembly.GetType("UnityEditor.PlayModeView");
            var method = audioUtilClass.GetMethod("RepaintAll", BindingFlags.Static | BindingFlags.Public);
            if (method == null) method = audioUtilClass.GetMethod("RepaintAll", BindingFlags.Static | BindingFlags.NonPublic);
            if (method == null) return;
            method.Invoke(null, null);
        }
        //------------------------------------------------------
        public static void SetGameViewTargetSize(int width, int height)
        {
            try
            {
                var unityEditorAssembly = typeof(UnityEditor.GameViewSizeGroupType).Assembly;
                var gameView = unityEditorAssembly.GetType("UnityEditor.GameView");

                var gameViewSizesType = unityEditorAssembly.GetType("UnityEditor.GameViewSizes");
                var selectedSizeIndex = gameView.GetProperty("selectedSizeIndex", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var GameViewSizeGroupType = gameView.GetProperty("currentSizeGroupType", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var SizeSelectionCallback = gameView.GetMethod("SizeSelectionCallback", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

                var gameViewIns = UnityEditor.EditorWindow.GetWindow(gameView);
                if (gameViewIns == null) return;
                UnityEditor.GameViewSizeGroupType groupType = (UnityEditor.GameViewSizeGroupType)GameViewSizeGroupType.GetValue(null);
                int selectIndex = (int)selectedSizeIndex.GetValue(gameViewIns);

                string file = UnityEditorInternal.InternalEditorUtility.unityPreferencesFolder + "/GameViewSizes.asset";
                UnityEngine.Object[] objects = UnityEditorInternal.InternalEditorUtility.LoadSerializedFileAndForget(file);
                for (int i = 0; i < objects.Length; ++i)
                {
                    if (objects[i].GetType() == gameViewSizesType)
                    {
                        var method = gameViewSizesType.GetMethod("GetGroup", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                        if (method != null)
                        {
                            var gameViewSizeGroupInst = method.Invoke(objects[i], new System.Object[] { groupType });
                            if (gameViewSizeGroupInst != null)
                            {
                                var GetTotalCountMethod = gameViewSizeGroupInst.GetType().GetMethod("GetTotalCount", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                                if (GetTotalCountMethod != null)
                                {
                                    var GetGameViewSizeMethod = gameViewSizeGroupInst.GetType().GetMethod("GetGameViewSize", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                                    int TotalCount = (int)GetTotalCountMethod.Invoke(gameViewSizeGroupInst, null);
                                    for (int j = 0; j < TotalCount; ++j)
                                    {
                                        var gameviewSize = GetGameViewSizeMethod.Invoke(gameViewSizeGroupInst, new System.Object[] { j });
                                        if (gameviewSize != null)
                                        {
                                            var widthProp = gameviewSize.GetType().GetProperty("width", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                                            var heightProp = gameviewSize.GetType().GetProperty("height", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                                            int widthGV = (int)widthProp.GetValue(gameviewSize);
                                            int heightGV = (int)heightProp.GetValue(gameviewSize);

                                            if (widthGV == width && heightGV == height)
                                            {
                                                SizeSelectionCallback.Invoke(gameViewIns, new System.Object[] { j, gameviewSize });

                                                UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    }
                }
            }
            catch (System.Exception expection)
            {
                UnityEngine.Debug.Log("更改分辨率失败");
                Debug.LogWarning(expection.ToString());
            }
        }
        //------------------------------------------------------
        public static void PlayClip(AudioClip clip, int startSample = 0, bool loop = false)
        {
            var unityEditorAssembly = typeof(AudioImporter).Assembly;
            var audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
            var method = audioUtilClass.GetMethod("PlayClip", BindingFlags.Static | BindingFlags.Public);
            if (method == null)
            {
                method = audioUtilClass.GetMethod("PlayPreviewClip", BindingFlags.Static | BindingFlags.Public);
            }
            if (method == null)
                return;

            method.Invoke(null, new object[] { clip, startSample, loop });
        }
        static MethodInfo ms_pExternAudioPlayer = null;
        //------------------------------------------------------
        public static void PlayClip(string audioFile, int startSample = 0, bool loop = false)
        {
            AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(audioFile);
            if (clip == null)
            {
                if (ms_pExternAudioPlayer == null)
                {
                    foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
                    {
                        Type[] types = null;
                        try
                        {
                            types = assembly.GetTypes();
                        }
                        catch (ReflectionTypeLoadException ex)
                        {
                            types = ex.Types; // 部分可用类型
                                              // 可选：输出警告
                            UnityEngine.Debug.LogWarning($"加载程序集 {assembly.FullName} 时部分类型无法加载: {ex}");
                        }
                        catch (Exception ex)
                        {
                            UnityEngine.Debug.LogWarning($"加载程序集 {assembly.FullName} 时发生异常: {ex}");
                            continue;
                        }
                        for (int i = 0; i < types.Length; ++i)
                        {
                            System.Type tp = types[i];
                            if (tp == null) continue;
                            if (tp.IsDefined(typeof(ExternPlayAudioAttribute), false))
                            {
                                ExternPlayAudioAttribute attr = (ExternPlayAudioAttribute)tp.GetCustomAttribute(typeof(ExternPlayAudioAttribute));
                                ms_pExternAudioPlayer = tp.GetMethod(attr.strMethod, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
                                break;
                            }
                        }
                    }
                }
                if (ms_pExternAudioPlayer != null)
                {
                    ms_pExternAudioPlayer.Invoke(null, new object[] { audioFile });
                }
                return;
            }

            PlayClip(clip, startSample, loop);
        }
        //------------------------------------------------------
        public static void StopAllAudioClips()
        {
            var unityEditorAssembly = typeof(AudioImporter).Assembly;
            var audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
            var method = audioUtilClass.GetMethod("StopAllClips", BindingFlags.Static | BindingFlags.Public);
            if (method == null) return;
            method.Invoke(null, null);
        }
    }
}
#endif
