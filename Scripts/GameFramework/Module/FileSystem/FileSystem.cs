/********************************************************************
生成日期:	1:11:2020 10:05
类    名: 	FileSystem
作    者:	HappLI
描    述:	文件操作系统
*********************************************************************/
using ExternEngine;
using Framework.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Build.Content;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Networking;
namespace Framework.Core
{
    public class FileSystem : AModule
    {
        protected string                                m_strAssetPath = "";
        protected string                                m_strStreamPath = "";
        protected string                                m_strStreamPackagesPath = "";
        protected string                                m_strStreamRawPath = "";
        protected string                                m_strStreamBinaryPath = "";
        protected string                                m_strPersistentDataPath = "";
        protected string                                m_strUpdateDataPath = "";

        protected LinkedList<AssetOperator>             m_vReqAssetOps = null;
        protected LinkedList<SceneOperator>             m_vReqSceneOps = null;
        protected LinkedList<InstanceOperator>          m_vReqInstanceOps = null;
        private Dictionary<int, IInstanceAbleCallback>  m_vInstanceCallback = null;

        protected bool                                  m_bEnableCoroutines = true;
        private bool                                    m_bCoroutining = false;
         WaitForEndOfFrame                              m_waitForEndOfFrame = new WaitForEndOfFrame();

#if UNITY_EDITOR
        private IEnumerator                             m_pEditorCoroutine;
        private Stack<IEnumerator>                      m_EditorCoroutineStack = new Stack<IEnumerator>();
#endif
        public string AssetPath { get { return m_strAssetPath; } }
        public string StreamPath { get { return m_strStreamPath; } }
        public string StreamRawPath { get { return m_strStreamRawPath; } }
        public string StreamBinaryPath { get { return m_strStreamBinaryPath; } }
        public string StreamPackagesPath { get { return m_strStreamPackagesPath; } }
        public string PersistenDataPath { get { return m_strPersistentDataPath; } }
        //--------------------------------------------------------
        protected override void OnInit()
        {
#if UNITY_EDITOR
            m_strStreamPath = Application.streamingAssetsPath;
            m_strStreamPackagesPath = Application.streamingAssetsPath;
            m_strStreamRawPath = Application.streamingAssetsPath + "/raws/";
            m_strStreamBinaryPath = Application.dataPath + "/../Binarys/";
#elif UNITY_STANDALONE
            m_strStreamPath = Application.streamingAssetsPath;
            m_strStreamPackagesPath = Application.streamingAssetsPath + "/packages/";
            m_strStreamRawPath = Application.streamingAssetsPath + "/packages/raws/";
            m_strStreamBinaryPath = m_strStreamRawPath + "Binarys/";
#elif UNITY_ANDROID
            m_strStreamPath = Application.streamingAssetsPath;
            m_strStreamPackagesPath = Application.streamingAssetsPath + "/packages/";
            m_strStreamRawPath = "packages/raws/";
            m_strStreamBinaryPath = m_strStreamRawPath + "Binarys/";
#elif UNITY_IOS
            m_strStreamPath = Application.streamingAssetsPath;
            m_strStreamPackagesPath =  Application.streamingAssetsPath + "/packages/";
            m_strStreamRawPath = Application.streamingAssetsPath + "/packages/raws/";
            m_strStreamBinaryPath = m_strStreamRawPath + "Binarys/";
#elif UNITY_WEBGL
            m_strStreamPath = Application.streamingAssetsPath;
            m_strStreamPackagesPath =  Application.streamingAssetsPath + "/packages/";
            m_strStreamRawPath = Application.streamingAssetsPath + "/packages/raws/";
            m_strStreamBinaryPath = m_strStreamRawPath + "Binarys/";
#else
            m_strStreamPath = Application.streamingAssetsPath;
            m_strStreamPackagesPath = Application.streamingAssetsPath + "/packages/";
            m_strStreamRawPath = Application.streamingAssetsPath + "/raws/";
            m_strStreamBinaryPath = Application.dataPath + "/../Binarys/";
#endif
            m_strAssetPath = Application.dataPath;
            m_strPersistentDataPath = Application.persistentDataPath + "/";
#if UNITY_EDITOR
            m_strPersistentDataPath = Application.dataPath + "/../Local/";
#endif

            m_vReqAssetOps = new LinkedList<AssetOperator>();
            m_vReqSceneOps = new LinkedList<SceneOperator>();
            m_vReqInstanceOps = new LinkedList<InstanceOperator>();
        }
        //--------------------------------------------------------
        public AssetOperator LoadAsset(string strFile, System.Action<AssetOperator, bool> callback = null, bool bAsync = false)
        {
            if (string.IsNullOrEmpty(strFile)) return null;
            AssetOperator handler = OperatorHandlerUtil.Malloc<AssetOperator>(GetFramework());
#if UNITY_EDITOR
            handler.SetRawAssetPath(BuildAssetFilePath(strFile));
#endif
            handler.SetAssetPath(strFile);
            handler.AddCallback(callback);
            handler.SetAsync(bAsync);
            m_vReqAssetOps.AddLast(handler);
            CheckStartCoroutinesLoad();
            return handler;
        }
        //--------------------------------------------------------
        public UnityEngine.Object ImmediatelyLoadAsset(string strFile)
        {
            if (string.IsNullOrEmpty(strFile)) return null;
            AssetOperator handler = OperatorHandlerUtil.Malloc<AssetOperator>(GetFramework());
#if UNITY_EDITOR
            handler.SetRawAssetPath(BuildAssetFilePath(strFile));
#endif
            handler.SetAssetPath(strFile);
            handler.SetUsed(true);
            handler.SetAsync(false);
            handler.DoCallback(true);
            if (!handler.IsUsed())
            {
                handler.Free();
                return null;
            }
            var enumerator = m_pFramework.OnLoadAsset(handler);
            while (enumerator.MoveNext()) { }
            var pObj = handler.GetObject();
            handler.DoCallback(false);
            handler.Free();
            return pObj;
        }
        //--------------------------------------------------------
        public AssetOperator LoadAssetAsync(string strFile, System.Action<AssetOperator, bool> callback = null)
        {
            if (string.IsNullOrEmpty(strFile)) return null;
            AssetOperator handler = OperatorHandlerUtil.Malloc<AssetOperator>(GetFramework());
#if UNITY_EDITOR
            handler.SetRawAssetPath(BuildAssetFilePath(strFile));
#endif
            handler.SetAssetPath(strFile);
            handler.SetAsync(true);
            handler.AddCallback(callback);
            m_vReqAssetOps.AddLast(handler);
            CheckStartCoroutinesLoad();
            return handler;
        }
        //------------------------------------------------------
        public SceneOperator LoadScene(string strFile, System.Action<SceneOperator, bool> callback = null, ELoadSceneMode mode = ELoadSceneMode.eSingle, bool bAysnc = false)
        {
            if (string.IsNullOrEmpty(strFile)) return null;
            SceneOperator handler = OperatorHandlerUtil.Malloc<SceneOperator>(GetFramework());
            handler.SetAssetPath(strFile);
#if UNITY_EDITOR
            handler.SetRawAssetPath(BuildAssetFilePath(strFile));
#endif
            handler.AddCallback(callback);
            m_vReqSceneOps.AddLast(handler);
            CheckStartCoroutinesLoad();
            return handler;
        }
        //--------------------------------------------------------
        public InstanceOperator SpawnInstance(string strFile, System.Action<InstanceOperator, bool> callback = null, bool bAsync = false)
        {
            if (string.IsNullOrEmpty(strFile)) return null;
            InstanceOperator handler = OperatorHandlerUtil.Malloc<InstanceOperator>(GetFramework());
#if UNITY_EDITOR
            handler.SetRawAssetPath(BuildAssetFilePath(strFile));
#endif
            handler.SetAsync(bAsync);
            handler.SetAssetPath(strFile);
            handler.AddCallback(callback);
            m_vReqInstanceOps.AddLast(handler);
            CheckStartCoroutinesLoad();
            return handler;
        }
        //--------------------------------------------------------
        public void DeSpawnInstance(InstanceAble pAble, int recyleMax = 2)
        {
            if (pAble == null) return;
#if UNITY_EDITOR
            if (Application.isPlaying) GameObject.Destroy(pAble.GetGameObject());
            else GameObject.DestroyImmediate(pAble.GetGameObject());
#endif
        }
        //------------------------------------------------------
        public bool IsInLoadQueue(string strFile)
        {
            if (string.IsNullOrEmpty(strFile)) return false;
            for (var node = m_vReqAssetOps.First; node != null; node = node.Next)
            {
                if (strFile.CompareTo(node.Value.GetAssetPath()) == 0)
                    return true;
            }
            for(var node = m_vReqSceneOps.First; node!=null; node = node.Next)
            {
                if (strFile.CompareTo(node.Value.GetAssetPath()) == 0)
                    return true;
            }
            for (var node = m_vReqInstanceOps.First; node != null; node = node.Next)
            {
                if (strFile.CompareTo(node.Value.GetAssetPath()) == 0)
                    return true;
            }
            return false;
        }
        //------------------------------------------------------
        public int GetCurLoadingCount()
        {
            int loadingCount = 0;
            if (m_vReqAssetOps != null) loadingCount += m_vReqAssetOps.Count;
            if (m_vReqSceneOps != null) loadingCount += m_vReqSceneOps.Count;
            if (m_vReqInstanceOps != null) loadingCount += m_vReqInstanceOps.Count;
            return loadingCount;
        }
        //--------------------------------------------------------
        protected override void OnUpdate(FFloat fFrame)
        {
            base.OnUpdate(fFrame);
        }
        //------------------------------------------------------
        public T GetLaunchData<T>(string name = null) where T : UnityEngine.Object
        {
            var gameLanuch = GetFramework().gameStartup;
            if (gameLanuch == null || gameLanuch.GetDatas() == null) return null;
            var gameDatas = gameLanuch.GetDatas();
            for (int i = 0; i < gameDatas.Length; ++i)
            {
                if (gameDatas[i] == null) continue;
                if (!string.IsNullOrEmpty(name) && gameDatas[i].name.CompareTo(name) != 0)
                    continue;

                if (gameDatas[i] is T)
                    return gameDatas[i] as T;
            }
            return null;
        }
        //--------------------------------------------------------
        public void Clear()
        {
            if (m_vReqAssetOps != null)
            {
                var node = m_vReqAssetOps.First;
                while (node != null)
                {
                    var next = node.Next;
                    node.Value.Free();
                    node = next;
                }
                m_vReqAssetOps.Clear();
            }
            if (m_vReqSceneOps != null)
            {
                var node = m_vReqSceneOps.First;
                while (node != null)
                {
                    var next = node.Next;
                    node.Value.Free();
                    node = next;
                }
                m_vReqSceneOps.Clear();
            }
            if (m_vReqInstanceOps != null)
            {
                var node = m_vReqInstanceOps.First;
                while (node != null)
                {
                    var next = node.Next;
                    node.Value.Free();
                    node = next;
                }
                m_vReqInstanceOps.Clear();
            }
        }
        //------------------------------------------------------
        internal void OnInstanceCallback(InstanceAble pAble, EInstanceCallbackType eType)
        {
            if (m_vInstanceCallback == null) return;
            foreach(var db in m_vInstanceCallback)
            {
                db.Value.OnInstanceCallback(pAble, eType);
            }
        }
        //------------------------------------------------------
        internal void RegisterFunction(IUserData pointer, int hashCode)
        {
            IInstanceAbleCallback instanceCb = pointer as IInstanceAbleCallback;
            if (instanceCb != null)
            {
                if (m_vInstanceCallback == null) m_vInstanceCallback = new Dictionary<int, IInstanceAbleCallback>(32);
                m_vInstanceCallback[hashCode] = instanceCb;
            }
        }
        //------------------------------------------------------
        internal void UnRegisterFunction(IUserData pointer, int hashCode)
        {
            if (m_vInstanceCallback == null) return;
            IInstanceAbleCallback instanceCb = pointer as IInstanceAbleCallback;
            if (instanceCb != null)
            {
                m_vInstanceCallback.Remove(hashCode);
            }
        }
        //------------------------------------------------------
        private void CheckStartCoroutinesLoad()
        {
            if (!m_bEnableCoroutines) return;
            if (m_bCoroutining) return;
            if (m_vReqAssetOps.Count > 0 || m_vReqSceneOps.Count > 0 || m_vReqInstanceOps.Count>0)
            {
                m_bCoroutining = true;
#if UNITY_EDITOR
                if (!Application.isPlaying || !AFramework.isStartup)
                {
                    m_pEditorCoroutine = CoroutinesUpdateLoad();
                    EditorApplication.update += EditorCoroutineUpdate;
                    return;
                }
#endif
                StartCoroutine(CoroutinesUpdateLoad());
            }
        }
        //------------------------------------------------------
#if UNITY_EDITOR
        private void EditorCoroutineUpdate()
        {
            if (m_pEditorCoroutine == null)
            {
                EditorApplication.update -= EditorCoroutineUpdate;
                m_bCoroutining = false;
                return;
            }

            if (m_EditorCoroutineStack.Count == 0)
                m_EditorCoroutineStack.Push(m_pEditorCoroutine);

            while (m_EditorCoroutineStack.Count > 0)
            {
                var top = m_EditorCoroutineStack.Peek();
                if (top.MoveNext())
                {
                    var current = top.Current;
                    if (current is IEnumerator nested)
                    {
                        m_EditorCoroutineStack.Push(nested);
                        continue;
                    }
                    // 其它 yield return 类型（如 WaitForEndOfFrame），可直接跳过

                    break;
                }
                else
                {
                    m_EditorCoroutineStack.Pop();
                }
            }

            if (m_EditorCoroutineStack.Count == 0)
            {
                EditorApplication.update -= EditorCoroutineUpdate;
                m_bCoroutining = false;
                m_pEditorCoroutine = null;
            }
        }
#endif
        //------------------------------------------------------
        private IEnumerator CoroutinesUpdateLoad()
        {
            m_bCoroutining = true;
            yield return m_waitForEndOfFrame;
            while (m_vReqAssetOps.Count > 0)
            {
                var reqInfo = m_vReqAssetOps.First.Value;
                m_vReqAssetOps.RemoveFirst();
                reqInfo.DoCallback(true);
                if (!reqInfo.IsUsed())
                {
                    reqInfo.Free();
                    continue;
                }
                yield return m_pFramework.OnLoadAsset(reqInfo);
                reqInfo.DoCallback(false);
                reqInfo.Free();
            }

            while (m_vReqInstanceOps.Count > 0)
            {
                var reqInfo = m_vReqInstanceOps.First.Value;
                m_vReqInstanceOps.RemoveFirst();
                reqInfo.DoCallback(true);
                if (!reqInfo.IsUsed())
                {
                    reqInfo.Free();
                    continue;
                }
                yield return m_pFramework.OnSpawnInstance(reqInfo);
                reqInfo.DoCallback(false);
                reqInfo.Free();
            }
            if (m_vReqAssetOps.Count > 0 || m_vReqSceneOps.Count > 0 || m_vReqInstanceOps.Count > 0)
            {
                yield return CoroutinesUpdateLoad();
            }
            m_bCoroutining = false;
        }
        //------------------------------------------------------
        Coroutine StartCoroutine(IEnumerator coroutine)
        {
            return GetFramework().BeginCoroutine(coroutine);
        }
        //------------------------------------------------------
        public string BuildAssetFilePath(string strFile)
        {
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(strFile)) return strFile;
            return Framework.ED.EditorUtils.GetAssetFullPath(strFile);
#else
            return strFile; 
#endif
        }
        //--------------------------------------------------------
        protected override void OnDestroy()
        {
            if (m_vInstanceCallback != null) m_vInstanceCallback.Clear();
            base.OnDestroy();
        }
    }
}