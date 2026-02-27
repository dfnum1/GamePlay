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
    public class UndoLogic : AStateEditorLogic, UndoHandler
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
                var serializeMethod = data.GetType().GetMethod("Serialize", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                if(serializeMethod == null || serializeMethod.ReflectedType !=typeof(string))
                    this.json = JsonUtility.ToJson(data);
                else
                {
                    this.json = (string)serializeMethod.Invoke(data, null);
                }
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
                GetOwner<GameWorldEditor>().OnUndoAction(JsonUtility.FromJson(obj.json, obj.data.GetType()), obj.isDirtyData);
                bStack = true;
            }
        }
        //--------------------------------------------------------
        public void RegisterUndoData(object data)
        {
            LockUndoData(data, false);
        }
    }
}
#endif