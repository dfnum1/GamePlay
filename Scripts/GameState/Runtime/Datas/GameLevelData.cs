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
#if UNITY_EDITOR
        private AGameEditor m_pEditor = null;
        internal AGameEditor GetEditor()
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
                m_pEditor.SetData(this);
            return m_pEditor;
        }
#endif
    }
    //------------------------------------------------
    //! 游戏数据体编辑器
    //------------------------------------------------
#if UNITY_EDITOR
    public class AGameEditor
    {
        protected AGameCfgData m_pData;
        //------------------------------------------------
        internal void SetData(AGameCfgData pData)
        {
            m_pData = pData;
        }
        //------------------------------------------------
        public T GetData<T>() where T : AGameCfgData
        {
            return m_pData as T;
        }
        //------------------------------------------------
        public virtual void OnInspectorGUI()
        {
            Framework.ED.InspectorDrawUtil.DrawProperty(m_pData, null);
        }
        //------------------------------------------------
        public virtual void OnSceneView(SceneView view)
        {
        }
        //------------------------------------------------
        public virtual void OnPreviewEnable(Framework.ED.TargetPreview preview)
        {

        }
        //------------------------------------------------
        public virtual void OnPreviewDisable(Framework.ED.TargetPreview preview)
        {

        }
        //------------------------------------------------
        public virtual void OnPreviewView(Framework.ED.TargetPreview preview)
        {
        }
    }
#endif
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

