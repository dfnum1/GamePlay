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
#if UNITY_EDITOR
using Framework.State.Editor;
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
        public virtual bool OnDeserialize(string dataContent)
        {
            if (string.IsNullOrEmpty(dataContent))
                return false;

            JsonUtility.FromJsonOverwrite(dataContent, this);
            return true;
        }
        //------------------------------------------------
        public virtual string OnSerialize()
        {
            return JsonUtility.ToJson(this);
        }
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
        public int dataType;
        [SerializeField]
        private string dataContent;

        AGameCfgData m_pGameData = null;
        //------------------------------------------------
        public T GetGameData<T>() where T : AGameCfgData
        {
            if(m_pGameData == null)
            {
                m_pGameData = GameWorldHandler.Malloc<AGameCfgData>(dataType);
                if (m_pGameData != null)
                {
                    m_pGameData.OnDeserialize(dataContent);
                }
            }
            return m_pGameData as T;
        }
        //------------------------------------------------
        public void Deserialize()
        {
            if(m_pGameData!=null) m_pGameData.Free();
            m_pGameData = null;
            if (dataType == 0)
                return;
            m_pGameData = GameWorldHandler.Malloc<AGameCfgData>(dataType);
            if (m_pGameData != null)
            {
                m_pGameData.OnDeserialize(dataContent);
            }
        }
        //------------------------------------------------
#if UNITY_EDITOR
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
                dataContent = m_pGameData.OnSerialize();
            }
        }
#endif
    }
}

