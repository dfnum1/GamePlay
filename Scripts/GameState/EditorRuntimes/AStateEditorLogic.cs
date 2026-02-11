/********************************************************************
生成日期:	11:07:2025
类    名: 	AStateEditorLogic
作    者:	HappLI
描    述:	世界逻辑基础类
*********************************************************************/
#if UNITY_EDITOR
using Framework.ED;
using Framework.State.Runtime;
using UnityEditor;

namespace Framework.State.Editor
{
    public class AStateEditorLogic : AEditorLogic
    {
        public T GetObject<T>() where T : State.Runtime.AGameWorldObject
        {
            var obj = GetOwner().GetCurrentObj();
            if (obj == null) return null;
            return obj as T;
        }
        //-----------------------------------------------------
        public virtual void OnGameItemSelected(IGameWorldItem pGameItem)
        {
        }
        //-----------------------------------------------------
        public GameWorldData GetWorldData()
        {
            var worldObj = GetObject<AGameWorldObject>();
            if (worldObj == null) return null;
            return worldObj.gameWorldData;
        }
        //-----------------------------------------------------
        public void UndoRegister(bool bDityData = false)
        {
            UndoLogic logic = GetLogic<UndoLogic>();
            if (logic == null)
                return;
            logic.LockUndoData(GetWorldData(), bDityData);
        }
    }
}

#endif