/********************************************************************
生成日期:	06:30:2025
类    名: 	AIDrawLogic
作    者:	HappLI
描    述:	AI控制面板
*********************************************************************/
#if UNITY_EDITOR
using Framework.AT.Runtime;
using Framework.Cutscene.Runtime;
using Framework.ED;
using Framework.State.Runtime;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Framework.State.Editor
{
    [EditorBinder(typeof(GameWorldEditor))]
    public class UndoLogic : AStateEditorLogic
    {
        struct StackData
        {
            public System.Object data;
            public string json;
            public bool isDirtyData;
            public StackData(System.Object data, bool isDirtyData)
            {
                this.isDirtyData = isDirtyData;
                this.data = data;
                this.json = JsonUtility.ToJson(data);
            }
        }
        Stack<StackData> m_vUndos = new Stack<StackData>();
        Stack<StackData> m_vRedos = new Stack<StackData>();
        //--------------------------------------------------------
        public void LockUndoData(System.Object pData, bool bDirtyData= false)
        {
            if (pData == null)
                return;
            try
            {
                m_vUndos.Push(new StackData(pData, bDirtyData));
            }
            catch
            {

            }
        }
        //--------------------------------------------------------
        protected override void OnEvent(Event evt)
        {
            base.OnEvent(evt);
            if (evt.type == EventType.KeyDown)
            {
                if (evt.control && evt.keyCode == KeyCode.Z)
                {
                    UndoData();
                    evt.Use();
                }
            }
        }
        //--------------------------------------------------------
        public void UndoData()
        {
            if (m_vUndos.Count <= 0)
                return;

            var obj = m_vUndos.Pop();
            bool bStack = false;
            if (!string.IsNullOrEmpty(obj.json))
            {
                if (obj.data is GameWorldData)
                {
                    GameWorldData graphData = (GameWorldData)obj.data;
                    JsonUtility.FromJson<GameWorldData>(obj.json);
                    bStack = true;
                }
            }
        }
    }
}
#endif