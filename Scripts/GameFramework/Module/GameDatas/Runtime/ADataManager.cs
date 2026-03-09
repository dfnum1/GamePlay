/********************************************************************
生成日期:	1:11:2020 10:05
类    名: 	ADataManager
作    者:	HappLI
描    述:	
*********************************************************************/
using Framework.AT.Runtime;
using Framework.Base;
using Framework.Core;
using System;
using System.Collections.Generic;
#if USE_SERVER
using ExternEngine;
#else
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Windows;
#endif

namespace Framework.Data
{
    [ATInteralExport("配置数据系统/管理器", -1, icon: "GameData/tabledata")]
    public abstract class ADataManager : Core.AModule
    {
        private System.IO.MemoryStream m_pMemoryStream = null;
        private System.IO.BinaryReader m_pBinaryReader = null;

        protected int m_nLoadCnt = 0;
        protected int m_nTotalCnt = 0;
        Dictionary<int, Data_Base> m_vDatas = new Dictionary<int, Data_Base>(64);
        Dictionary<int, Data_Base> m_vHashDatas = new Dictionary<int, Data_Base>(64);
        Dictionary<string, IUserData> m_vCustomDatas = null;
        public static Action OnLoaded = null;
        public bool bInited { get; set; }
        //-------------------------------------------
        public ADataManager()
        {
            bInited = false;
        }
        //-------------------------------------------
        protected override void OnInit()
        {
            bInited = false;
            m_nLoadCnt = 0;
            m_nTotalCnt = 0;
            foreach (var db in m_vDatas)
            {
                db.Value.Free();
            }
            m_vDatas.Clear();
            m_vHashDatas.Clear();
        }
        //-------------------------------------------
        protected override void OnAwake()
        {
            if (GetFramework() == null)
                return;
            if (bInited)
            {
                CheckLoaded();
                return;
            }
            bInited = false;
            var game = GetFramework().gameStartup;
            if (game == null)
            {
                return;
            }
            ACsvConfig csv = GetFramework().GetLaunchData<ACsvConfig>();
            InitCsv(csv);
            CheckLoaded();
        }
        //-------------------------------------------
        void InitCsv(ACsvConfig pConfig)
        {
            if (pConfig == null) return;
            m_nLoadCnt = 0;
            m_nTotalCnt = 0;
            foreach (var db in m_vDatas)
            {
                db.Value.Free();
            }
            m_vDatas.Clear();
            m_vHashDatas.Clear();
            m_nTotalCnt += pConfig.Assets.Length;
            CsvParser csvParser = new CsvParser();
            for (int i = 0; i < pConfig.Assets.Length; ++i)
            {
                csvParser.Clear();
#if UNITY_EDITOR
                CsvAsset curParseAsset = new CsvAsset() { Asset = null };
                try
                {
#endif
                    if (pConfig.Assets[i].nHash == 0)
                    {
#if UNITY_EDITOR
                        string strPath = pConfig.Assets[i].Asset != null ? UnityEditor.AssetDatabase.GetAssetPath(pConfig.Assets[i].Asset.GetInstanceID()) : "";
                        if (UnityEditor.EditorUtility.DisplayDialog("错误", "配置表[" + strPath + "]数据读取失败", "请确认"))
                        {
                            UnityEditor.Selection.activeObject = pConfig;
                            UnityEngine.Debug.Break();
                        }
#endif
                        Debug.LogError("load csv[" + (pConfig.Assets[i].Asset ? pConfig.Assets[i].Asset.name : i.ToString()) + "]");
                        continue;
                    }
                    Data_Base csvData = Parser(csvParser,pConfig.Assets[i].nHash, pConfig.Assets[i].Asset, pConfig.Assets[i].type);
                    if (csvData == null)
                    {
#if UNITY_EDITOR
                        string strPath = pConfig.Assets[i].Asset != null ? UnityEditor.AssetDatabase.GetAssetPath(pConfig.Assets[i].Asset.GetInstanceID()) : "";
                        if (UnityEditor.EditorUtility.DisplayDialog("错误", "配置表[" + strPath + "]数据读取失败", "请确认"))
                        {
                            UnityEditor.Selection.activeObject = pConfig;
                            UnityEngine.Debug.Break();
                        }
#endif
                    }
                    else
                    {
                        csvData.SetSystem(this);
                        csvData.SetHashID(pConfig.Assets[i].nHash);
                        m_vHashDatas[pConfig.Assets[i].nHash] = csvData;
                        m_vDatas[csvData.GetType().GetHashCode()] = csvData;
                    }
#if UNITY_EDITOR
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex.ToString());
                    if (curParseAsset.Asset != null)
                    {
                        string strPath = curParseAsset.Asset != null ? UnityEditor.AssetDatabase.GetAssetPath(curParseAsset.Asset.GetInstanceID()) : "";
                        if (UnityEditor.EditorUtility.DisplayDialog("错误", "配置表[" + strPath + "]数据读取失败\r\n" + ex.ToString(), "请确认"))
                        {
                            UnityEditor.Selection.activeObject = pConfig;
                            UnityEngine.Debug.Break();
                        }
                    }
                }
#endif
            }
            csvParser.Clear();
        }
        //-------------------------------------------
        [ATField]
        public float Progress
        {
            get
            {
                if (m_nTotalCnt <= 0) return 1;
                return (float)m_nLoadCnt / (float)m_nTotalCnt;
            }
        }
        //------------------------------------------------------
        private void CheckLoaded()
        {
            if (Progress >= 1)
            {
                Mapping();
                OnParserOver();
                if (OnLoaded != null) OnLoaded();
                OnLoaded = null;

                Debug.Log("config data loaded!");
            }
        }
        //-------------------------------------------
        [ATMethod("获取配置数据"),ATArgvDrawer("table", "DrawCsvTablePop")]
        public Data_Base GetCsvData(int table)
        {
            if (m_vHashDatas.TryGetValue(table, out var data))
                return data;
            return null;
        }
        //-------------------------------------------
        public T GetCsvData<T>() where T : Data_Base
        {
            int hash = (typeof(T)).GetHashCode();
            if (m_vDatas.TryGetValue(hash, out var data))
                return data as T;
            return null;
        }
        //-------------------------------------------
        public T GetCustomData<T>(string strFile) where T : IUserData
        {
            if (string.IsNullOrEmpty(strFile)) return default;
            if (m_vCustomDatas != null)
            {
                IUserData getData = null;
                if (m_vCustomDatas.TryGetValue(strFile, out getData))
                    return (T)getData;
            }
            return default;
        }
        //-------------------------------------------
        public void AddCustomData(string strFile, IUserData userData)
        {
            if (string.IsNullOrEmpty(strFile) || userData == null) return;
            if (m_vCustomDatas == null) m_vCustomDatas = new Dictionary<string, IUserData>(64);
            m_vCustomDatas[strFile] = userData;
        }
        //-------------------------------------------
        public void UnloadCustom(string strFile)
        {
            if (string.IsNullOrEmpty(strFile)) return;
            if (m_vCustomDatas != null) m_vCustomDatas.Remove(strFile);
        }
        //-------------------------------------------
        public void LoadBinary<T>(string strFile, System.Action<IUserData, bool> onCallback, bool bCache = false, bool bAbsFile = false) where T : IUserData
        {
            FileSystem fileSystem = GetFileSystem();
            if (fileSystem == null)
            {
                if (onCallback != null) onCallback(null, false);
                return;
            }
            if (string.IsNullOrEmpty(strFile))
            {
                if (onCallback != null) onCallback(null, false);
                return;
            }
            IUserData getData = null;
            if (m_vCustomDatas != null && m_vCustomDatas.TryGetValue(strFile, out getData))
            {
                if (onCallback != null) onCallback((T)getData, true);
                return;
            }
            var assetOp = fileSystem.LoadAsset(strFile, OnReadBinaryFile);
            assetOp.SetUserData(0, new TypeVar() { type = typeof(T) }) ;
            assetOp.SetUserData(1, new Callback1Var() { callback = onCallback });
            assetOp.SetUserData(2, new ByteVar() { boolVal = bCache });
            assetOp.SetUserData(3, new StringVar() { strValue = strFile });
        }
        //-------------------------------------------
        void OnReadBinaryFile(AssetOperator assetOp, bool check)
        {
            if(check) return;

            IUserData newData = null;
            Callback1Var callback = assetOp.GetUserData<Callback1Var>(1);
            var pTextAsset = assetOp.GetObject<TextAsset>();
            if(pTextAsset == null)
            {
                callback.Invoke(newData);
                return;
            }

            TypeVar userType = assetOp.GetUserData<TypeVar>(0);
            ByteVar cacheFlag = assetOp.GetUserData<ByteVar>(2);
            StringVar strFile = assetOp.GetUserData<StringVar>(3);

            var bytes = pTextAsset.bytes;
            newData = OnReadBinary(userType.type, bytes, bytes!=null? bytes.Length:0);
            callback.Invoke(newData);
            if (cacheFlag.boolVal)
            {
                m_vCustomDatas[strFile.strValue] = newData;
            }
        }
        //-------------------------------------------
        public System.IO.BinaryReader BeginBinary(byte[] bytes)
        {
            if (bytes == null || bytes.Length <= 0) return null;
            if (m_pBinaryReader == null)
            {
                m_pMemoryStream = new System.IO.MemoryStream();
                m_pBinaryReader = new System.IO.BinaryReader(m_pMemoryStream);
            }
            m_pMemoryStream.Seek(0, System.IO.SeekOrigin.Begin);
            m_pMemoryStream.SetLength(bytes.Length);
            m_pMemoryStream.Write(bytes, 0, bytes.Length);
            m_pMemoryStream.Seek(0, System.IO.SeekOrigin.Begin);
            return m_pBinaryReader;
        }
        //-------------------------------------------
        public void EndBinary()
        {
            if (m_pMemoryStream != null)
                m_pMemoryStream.SetLength(0);
        }
        //------------------------------------------------------
        protected override void OnDestroy()
        {
            //if (m_pAssetRef != null) m_pAssetRef.Release(0);
            //m_pAssetRef = null;
            OnLoaded = null;
            m_nLoadCnt = 0;
            m_nTotalCnt = 0;
            bInited = false;
            if (m_vCustomDatas != null)
            {
                m_vCustomDatas.Clear();
            }
            foreach(var db in m_vDatas)
            {
                db.Value.Free();
            }
            m_vDatas.Clear();
            m_vHashDatas.Clear();
        }
        //-------------------------------------------
        protected abstract Data_Base Parser(CsvParser csvParser, int index, TextAsset pAsset, EDataType eType = EDataType.Binary);
        protected abstract void Mapping();
        //-------------------------------------------
        protected virtual IUserData OnReadBinary(System.Type classType, byte[] buffers, int dataSize)
        {
#if UNITY_EDITOR
            Debug.LogError("binary data read not implement! type:" + classType.ToString());
#endif
            return null;
        }
        //-------------------------------------------
        protected virtual void OnParserOver() { }
    }
}
