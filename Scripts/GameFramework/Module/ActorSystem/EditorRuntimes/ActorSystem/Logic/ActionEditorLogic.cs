#if UNITY_EDITOR
/********************************************************************
生成日期:	11:06:2023
类    名: 	ActionEditorLogic
作    者:	HappLI
描    述:	
*********************************************************************/
using UnityEngine;
using UnityEditor;
using Framework.ED;
using Framework.ActorSystem.Runtime;

namespace Framework.ActorSystem.Editor
{
    public abstract class ActionEditorLogic : AEditorLogic
    {
        AssetDrawLogic m_pAssetDraw = null;
        //--------------------------------------------------------
        public Actor GetActor()
        {
            if (m_pAssetDraw == null) m_pAssetDraw = GetLogic<AssetDrawLogic>();
            if (m_pAssetDraw == null)
                return null;
            return m_pAssetDraw.GetActor();
        }
        //--------------------------------------------------------
        public virtual void OnSelectActor(Actor pActor) { }
        //--------------------------------------------------------
        public virtual void OnSpwanGameObejct(GameObject pObject) { }
    }
}
#endif
