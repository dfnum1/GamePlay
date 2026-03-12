/********************************************************************
生成日期:	11:07:2025
类    名: 	GameLevelData
作    者:	HappLI
描    述:	游戏关卡数据
*********************************************************************/
#if USE_SERVER
using JsonUtility = ExternEngine.JsonUtility;
#endif
using Framework.Core;
using Framework.Base;
using UnityEditor.VersionControl;


#if UNITY_EDITOR
using Framework.State.Editor;
using UnityEditor;
#endif
using UnityEngine;

namespace Framework.State.Runtime
{
    //------------------------------------------------
    //! 游戏数据体
    //------------------------------------------------
    public abstract class AGameCfgData : TypeObject
    {
        //------------------------------------------------
        public virtual bool OnDeserialize(TextAsset dataAsset)
        {
            if (dataAsset == null)
                return false;

            JsonUtility.FromJsonOverwrite(dataAsset.text, this);
            return true;
        }
        //------------------------------------------------
        public virtual bool OnDeserialize(byte[] byteData)
        {
            return false;
        }
        //------------------------------------------------
        public virtual string OnSerialize()
        {
            return "";
        }
#if UNITY_EDITOR
        private AGameEditor m_pEditor = null;
        internal AGameEditor GetEditor(EditorWindow pEditorWindow = null)
        {
            if (m_pEditor == null)
            {
                var editorType = StateEditorUtil.GetTypeEditorType(this.GetType());
                if (editorType != null)
                {
                    m_pEditor = (AGameEditor)System.Activator.CreateInstance(editorType);
                }
            }
            if (m_pEditor != null)
            {
                m_pEditor.SetData(this);
                m_pEditor.SetEditor(pEditorWindow);
            }
            return m_pEditor;
        }
#endif
    }
    //------------------------------------------------
    //! 游戏数据体
    //------------------------------------------------
    [StateIcon("GameWorld/gamelevel"),System.Serializable]
    public class GameLevelData : IGameWorldItem
    {
#if UNITY_EDITOR
        public string name;
        public string strDesc = "";
#endif
        internal int    dataType;
        public string   linkFile;

        public System.Collections.Generic.List<int> useATs;

        [System.NonSerialized]AGameCfgData     m_pGameData = null;
        [System.NonSerialized] string          m_strLoadFile = null;
        //------------------------------------------------
        public T GetGameData<T>(AFramework pFramework, System.Action<T> onCallback = null, bool bImmediately = false, bool bAsync = false) where T : AGameCfgData
        {
            if(m_pGameData == null)
                m_pGameData = GameWorldHandler.Malloc<AGameCfgData>(pFramework, dataType);
            if (string.IsNullOrEmpty(linkFile))
            {
                Framework.Base.Logger.Warning("当前状态数据中，没有关联关卡文件数据，不能用此接口创建!!!");
                return m_pGameData as T;
            }
            if (m_pGameData!=null && linkFile.CompareTo(m_strLoadFile) == 0)
                return m_pGameData as T;

            m_strLoadFile = linkFile;
            if (m_pGameData == null)
            {
                m_pGameData = GameWorldHandler.Malloc<AGameCfgData>(pFramework,dataType);
                if (m_pGameData != null && !string.IsNullOrEmpty(linkFile))
                {
                    if(bImmediately)
                    {
                        var pObj = pFramework.GetFileSystem().ImmediatelyLoadAsset(linkFile);
                        if (pObj != null)
                        {
                            TextAsset pAsset = pObj as TextAsset;
                            if (pAsset)
                                m_pGameData.OnDeserialize(pAsset);
                            if (onCallback != null) onCallback(m_pGameData as T);
                        }
                    }
                    else
                    {
                        var pOp = pFramework.GetFileSystem().LoadAsset(linkFile, OnLoadLevelData);
                        if (onCallback != null)
                            pOp.SetUserData(0, new Callback1Var() { callback = onCallback });
                    }
                }
            }
            return m_pGameData as T;
        }
        //------------------------------------------------
        public T GetGameData<T>(AFramework pFramework, string dataFile, System.Action<T> onCallback = null, bool bImmediately = false, bool bAsync = false) where T : AGameCfgData
        {
            if (m_pGameData == null)
            {
                m_pGameData = GameWorldHandler.Malloc<AGameCfgData>(pFramework, dataType);
            }
            if (m_pGameData != null && !string.IsNullOrEmpty(dataFile))
            {
                if(dataFile.CompareTo(m_strLoadFile) !=0)
                {
                    m_strLoadFile = dataFile;
                    if(bImmediately)
                    {
                        var pObj = pFramework.GetFileSystem().ImmediatelyLoadAsset(dataFile);
                        if(pObj!=null)
                        {
                            TextAsset pAsset = pObj as TextAsset;
                            if (pAsset)
                                m_pGameData.OnDeserialize(pAsset);
                            if (onCallback != null) onCallback(m_pGameData as T);
                        }
                    }
                    else
                    {
                        var pOp = pFramework.GetFileSystem().LoadAsset(dataFile, OnLoadLevelData, bAsync);
                        if (onCallback != null)
                            pOp.SetUserData(0, new Callback1Var() { callback = onCallback });
                    }
                }
            }
            return m_pGameData as T;
        }
        //------------------------------------------------
        void OnLoadLevelData(AssetOperator assetOp, bool check)
        {
            if (check)
            {
                assetOp.SetUsed(m_pGameData != null);
                return;
            }
            TextAsset pAsset = assetOp.GetObject<TextAsset>();
            if (pAsset == null) return;
            m_pGameData.OnDeserialize(pAsset);

            var callbackVar = assetOp.GetUserData<Callback1Var>(0);
            callbackVar.Invoke<AGameCfgData>(m_pGameData);
        }
#if UNITY_EDITOR
        //------------------------------------------------
        internal void Deserialize(AFramework pFramework, string file)
        {
            if(m_pGameData!=null) m_pGameData.Free();
            m_pGameData = null;
            if (dataType == 0)
                return;
            m_pGameData = GameWorldHandler.Malloc<AGameCfgData>(pFramework,dataType);
            if (m_pGameData != null && !string.IsNullOrEmpty(file))
            {
                pFramework.GetFileSystem().LoadAsset(file, OnLoadLevelData);
            }
        }
        //------------------------------------------------
        internal void SetUserData(AGameCfgData pData)
        {
            m_pGameData = pData;
        }
        //------------------------------------------------
        internal void Serialize()
        {
            if (m_pGameData != null)
            {
                dataType = StateEditorUtil.GetTypeClassId(m_pGameData.GetType());
                linkFile = m_pGameData.OnSerialize();
            }
        }
#endif
    }
}

