/********************************************************************
生成日期:	1:11:2020 10:05
类    名: 	FileSystem
作    者:	HappLI
描    述:	文件操作系统
*********************************************************************/
using ExternEngine;
using System.Collections.Generic;
namespace Framework.Core
{
    public class FileSystem : AModule
    {
        protected LinkedList<AssetOperator> m_vReqAssetOps = null;
        protected LinkedList<AssetOperator> m_vAsyncReqAssetOps = null;
        protected override void OnInit()
        {
            m_vReqAssetOps = new LinkedList<AssetOperator>();
            m_vAsyncReqAssetOps = new LinkedList<AssetOperator>();
        }
        //--------------------------------------------------------
        public AssetOperator LoadAsset(string strFile, IOperatorCallback callback = null)
        {
            AssetOperator handler = OperatorHandlerUtil.Malloc<AssetOperator>(GetFramework());
#if UNITY_EDITOR
            handler.SetRawAssetPath(BuildAssetFilePath(strFile));
#endif
            handler.SetAssetPath(strFile);
            handler.AddCallback(callback);
            return handler;
        }
        //--------------------------------------------------------
        public AssetOperator LoadAssetAsync(string strFile, IOperatorCallback callback = null)
        {
            AssetOperator handler = OperatorHandlerUtil.Malloc<AssetOperator>(GetFramework());
#if UNITY_EDITOR
            handler.SetRawAssetPath(BuildAssetFilePath(strFile));
#endif
            handler.SetAssetPath(strFile);
            handler.SetAsync(true);
            handler.AddCallback(callback);
            return handler;
        }
        //------------------------------------------------------
        public SceneOperator LoadScene(string strFile, ELoadSceneMode mode = ELoadSceneMode.eSingle, bool bAysnc = false)
        {
            if (string.IsNullOrEmpty(strFile)) return null;
            SceneOperator op = OperatorHandlerUtil.Malloc<SceneOperator>(GetFramework());
            op.SetAssetPath(strFile);
#if UNITY_EDITOR
            op.SetRawAssetPath(BuildAssetFilePath(strFile));
#endif
            return op;
        }
        //--------------------------------------------------------
        public AOperatorHandle SpawnInstance(string strFile, IOperatorCallback callback = null)
        {
            InstanceOperator handler = OperatorHandlerUtil.Malloc<InstanceOperator>(GetFramework());
#if UNITY_EDITOR
            handler.SetRawAssetPath(BuildAssetFilePath(strFile));
#endif
            handler.SetAssetPath(strFile);
            handler.AddCallback(callback);
            return handler;
        }
        //--------------------------------------------------------
        public void DeSpawnInstance(InstanceAble pAble, int recyleMax =2)
        {

        }
        //------------------------------------------------------
        public bool IsInLoadQueue(string strFile)
        {
            if (string.IsNullOrEmpty(strFile)) return false;
            for (var node = m_vAsyncReqAssetOps.First; node != null; node = node.Next)
            {
                if (strFile.CompareTo(node.Value.GetAssetPath()) == 0)
                    return true;
            }
            for (var node = m_vReqAssetOps.First; node != null; node = node.Next)
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
            if (m_vAsyncReqAssetOps != null) loadingCount += m_vAsyncReqAssetOps.Count;
            return loadingCount;
        }
        //--------------------------------------------------------
        protected override void OnUpdate(FFloat fFrame)
        {
            base.OnUpdate(fFrame);
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
            if (m_vAsyncReqAssetOps != null)
            {
                var node = m_vAsyncReqAssetOps.First;
                while (node != null)
                {
                    var next = node.Next;
                    node.Value.Free();
                    node = next;
                }
                m_vAsyncReqAssetOps.Clear();
            }
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
            base.OnDestroy();
        }
    }
}