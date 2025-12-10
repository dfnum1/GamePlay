#if UNITY_EDITOR
/********************************************************************
生成日期:		11:06:2020
类    名: 	EditorFramework
作    者:	HappLI
描    述:	编辑器框架类
*********************************************************************/
using Framework.ActorSystem.Runtime;
using Framework.AT.Runtime;
using Framework.Core;
using Framework.Cutscene.Runtime;
using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;

namespace Framework.ED
{
    //-----------------------------------------------------
    [AttributeUsage(AttributeTargets.Class)]
    public class EditorGameModuleAttribute : Attribute
    {
    }
    //-----------------------------------------------------
    class EditorGame : IGame
    {
        ScriptableObject[] m_Datas = null;
        public EditorGame()
        {

        }
        public Coroutine BeginCoroutine(IEnumerator coroutine)
        {
            return null;
        }
        public void EndAllCoroutine()
        {
        }
        public void EndCoroutine(Coroutine cortuine)
        {
        }
        public void EndCoroutine(IEnumerator cortuine)
        {
        }


        public ScriptableObject[] GetDatas()
        {
            return m_Datas;
        }
        public Transform GetTransform()
        {
            return null;
        }

        public RenderPipelineAsset GetURPAsset()
        {
            return null;
        }

        public bool IsEditor()
        {
            return true;
        }
    }
    //-----------------------------------------------------
    public class EditorFramework : AFramework
    {
        //--------------------------------------------------------
        public EditorFramework()
        {
            Init(CreateEditGame());
            Awake();
            Start();
        }
        //--------------------------------------------------------
        protected virtual IGame CreateEditGame()
        {
            return new EditorGame();
        }
        //--------------------------------------------------------
        protected override void OnAwake()
        {
            GetModule<CutsceneManager>().SetEditorMode(true);
        }
        //--------------------------------------------------------
        protected override void OnDestroy()
        {
        }
        //--------------------------------------------------------
        protected override void OnInit()
        {

        }
        //--------------------------------------------------------
        protected override void OnStart()
        {
        }
        //-----------------------------------------------------
        static System.Type ms_EditorGameModule = null;
        internal static Core.AFramework BuildEditorInstnace()
        {
            if (ms_EditorGameModule == null)
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
                        if (tp.IsDefined(typeof(EditorGameModuleAttribute), false) && tp.IsSubclassOf(typeof(EditorFramework)))
                        {
                            ms_EditorGameModule = tp;
                            break;
                        }
                    }
                    if (ms_EditorGameModule != null) break;
                }
            }
            if (ms_EditorGameModule == null) return new EditorFramework();
            var instance = Activator.CreateInstance(ms_EditorGameModule, true);
            Core.AFramework aFramework = (Core.AFramework)instance;
            return aFramework;
        }
    }
}
#endif