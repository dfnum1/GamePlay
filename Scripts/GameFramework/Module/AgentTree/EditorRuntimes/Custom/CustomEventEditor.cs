/********************************************************************
生成日期:	06:30:2025
类    名: 	CustomEventEditor
作    者:	HappLI
描    述:	自定义事件编辑器
*********************************************************************/
#if UNITY_EDITOR
using Framework.AT.Runtime;
using Framework.DrawProps;
using Framework.ED;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Framework.AT.Editor
{
    public class CustomEventEditor : EditorWindowBase
    {
        static ACustomEventObject ms_EventObject;
        private CustomEventData.EventData m_CurEdit = null;
        private Vector2 m_ListScroll = Vector2.zero;
        private Vector2 m_DataScroll = Vector2.zero;
        static float ms_LeftListWidth = 300;
        static CustomEventEditor ms_Window = null;
        //-----------------------------------------------------
        public static void Open()
        {
            CheckCustomEventObject();
            if(ms_Window == null)
                ms_Window = GetWindow<CustomEventEditor>("自定义蓝图事件");
            ms_Window.Focus();
            ms_Window.Show();
        }
        //-----------------------------------------------------
        public static void Open(ACustomEventObject pObj)
        {
            ms_EventObject = pObj;
            if(ms_Window == null)
                ms_Window = GetWindow<CustomEventEditor>("自定义蓝图事件");
            ms_Window.Focus();
            ms_Window.Show();
        }
        //-----------------------------------------------------
        public static CustomEventData GetCustomEventData()
        {
            if(ms_EventObject != null)
            {
                return ms_EventObject.eventData;
            }
            var eventObj = AssetDatabase.FindAssets("t:ACustomEventObject");
            if (eventObj == null || eventObj.Length <= 0)
                return null;
            ms_EventObject = AssetDatabase.LoadAssetAtPath<ACustomEventObject>(AssetDatabase.GUIDToAssetPath(eventObj[0]));
            if (ms_EventObject == null) return null;
            return ms_EventObject.eventData;
        }
        //-----------------------------------------------------
        public static ACustomEventObject CheckCustomEventObject()
        {
            if(ms_EventObject == null)
            {
                var eventObj = AssetDatabase.FindAssets("t:ACustomEventObject");
                if(eventObj == null || eventObj.Length<=0)
                {
                    var obj = Framework.ED.EditorUtils.CreateUnityScriptObject<ACustomEventObject>();
                    obj.name = "AgentTreeCustomEventData";
                    string file = EditorUtility.SaveFilePanelInProject("创建自定义蓝图事件", "AgentTreeCustomEventData", "asset","还没创建自定义蓝图事件数据体，先创建数据体");
                    if (file == null)
                        return null;
                    AssetDatabase.CreateAsset(obj, file);
                    EditorUtility.SetDirty(obj);
                    AssetDatabase.SaveAssetIfDirty(obj);
                    ms_EventObject = AssetDatabase.LoadAssetAtPath<ACustomEventObject>(file);
                }
            }
            return ms_EventObject;
        }
        //-----------------------------------------------------
        protected override void OnInnerEnable()
        {
            base.OnInnerEnable();
            this.minSize = new Vector2(1024, 600);
            CheckCustomEventObject();
        }
        //-----------------------------------------------------
        protected override void OnInnerDestroy()
        {
            base.OnInnerDestroy();
            ms_Window = null;
        }
        //-----------------------------------------------------
        protected override void OnInnerGUI()
        {
            base.OnInnerGUI();
            GUILayout.BeginArea(new Rect(0, 0, ms_LeftListWidth, position.height));
            m_ListScroll = EditorGUILayout.BeginScrollView(m_ListScroll, GUILayout.Height(this.position.height - 40));
            DrawEventList();
            EditorGUILayout.EndScrollView();
            if(ms_EventObject == null)
            {
                if (GUILayout.Button("创建数据体", GUILayout.Width(ms_LeftListWidth), GUILayout.Height(30)))
                {
                    CheckCustomEventObject();
                }
            }
            else
            {
                if (GUILayout.Button("添加事件", GUILayout.Width(ms_LeftListWidth), GUILayout.Height(30)))
                {
                    if (ms_EventObject.eventData == null)
                        ms_EventObject.eventData = new CustomEventData();

                    if (ms_EventObject.eventData.vEvents == null)
                        ms_EventObject.eventData.vEvents = new List<CustomEventData.EventData>();
                    CustomEventData.EventData evtData = new CustomEventData.EventData();
                    evtData.eventType = NewEventTypeId();
                    evtData.name = "NEW";
                    evtData.inputs = new List<CustomEventData.EventParam>();
                    evtData.outputs = new List<CustomEventData.EventParam>();
                    ms_EventObject.eventData.vEvents.Add(evtData);
                    m_CurEdit = evtData;
                }
            }

            GUILayout.EndArea();

            Framework.ED.UIDrawUtils.DrawColorLine(new Vector3(ms_LeftListWidth, 0), new Vector3(ms_LeftListWidth, position.height), Color.green);

            GUILayout.BeginArea(new Rect(ms_LeftListWidth, 0, position.width- ms_LeftListWidth, position.height));
            m_DataScroll = EditorGUILayout.BeginScrollView(m_DataScroll, GUILayout.Height(this.position.height - 40));
            DrawEventData();
            EditorGUILayout.EndScrollView();
            GUILayout.EndArea();

            Framework.ED.UIDrawUtils.DrawColorLine(new Vector3(ms_LeftListWidth, position.height - 30), new Vector3(position.width, position.height - 30), Color.green);
            GUILayout.BeginArea(new Rect(ms_LeftListWidth, position.height-30, position.width - ms_LeftListWidth, 30));
            GUILayout.BeginHorizontal();
            if(m_CurEdit !=null && GUILayout.Button("删除该事件",GUILayout.Height(30)))
            {
                if(EditorUtility.DisplayDialog("提示","确认删除该事件?", "删除", "取消"))
                {
                    ms_EventObject.eventData.vEvents.Remove(m_CurEdit);
                    m_CurEdit = null;
                }
            }
            if(ms_EventObject && GUILayout.Button("保存", GUILayout.Height(30)))
            {
                EditorUtility.SetDirty(ms_EventObject);
                AssetDatabase.SaveAssetIfDirty(ms_EventObject);
                AgentTreeUtil.Init(true);
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }
        //-----------------------------------------------------
        void DrawEventList()
        {
            if (ms_EventObject == null) return;
            if (ms_EventObject.eventData == null)
                ms_EventObject.eventData = new CustomEventData();

            if (ms_EventObject.eventData.vEvents == null)
                ms_EventObject.eventData.vEvents = new List<CustomEventData.EventData>();

            for (int i =0; i < ms_EventObject.eventData.vEvents.Count; ++i)
            {
                var evt = ms_EventObject.eventData.vEvents[i];
                using (new GUIColorScope(m_CurEdit == evt ? Color.green : Color.white))
                {
                    if (GUILayout.Button(evt.name + "[" + evt.eventType + "]"))
                    {
                        m_CurEdit = evt;
                    }
                }
            }
        }
        //-----------------------------------------------------
        void DrawEventData()
        {
            if (m_CurEdit == null)
                return;
            GUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(true);
            m_CurEdit.eventType = EditorGUILayout.IntField(new GUIContent("事件Id","该事件Id用户配置层做配置"), m_CurEdit.eventType);
            EditorGUI.EndDisabledGroup();
            if(GUILayout.Button("复制Id", GUILayout.Width(60)))
            {
                EditorGUIUtility.systemCopyBuffer = m_CurEdit.eventType.ToString();
                ShowNotification("事件Id已复制", 1f);
            }
            GUILayout.EndHorizontal();
            m_CurEdit.name = EditorGUILayout.TextField("事件名", m_CurEdit.name);
            EditorGUILayout.LabelField("描述说明:");
            m_CurEdit.desc = EditorGUILayout.TextArea(m_CurEdit.desc, GUILayout.Height(60));

            m_CurEdit.inputs = DrawEventPort("输入",m_CurEdit.inputs);
            m_CurEdit.outputs = DrawEventPort("输出",m_CurEdit.outputs);
        }
        //-----------------------------------------------------
        List<CustomEventData.EventParam> DrawEventPort(string label, List<CustomEventData.EventParam> vParams)
        {
            if (vParams == null) vParams = new List<CustomEventData.EventParam>();

            float width = this.position.width - ms_LeftListWidth-30;

            float varWidth = 300;
            float argvWidth = (width- varWidth-30-30) / 3;
            Framework.ED.UIDrawUtils.DrawHeader(label);
            GUILayout.BeginHorizontal();
            GUILayout.Label("变量名", GUILayout.Width(argvWidth));
            GUILayout.Label("变量类型", GUILayout.Width(varWidth));
            GUILayout.Label("默认值", GUILayout.Width(argvWidth));
            GUILayout.Label("是否可编辑", GUILayout.Width(30));
            if (GUILayout.Button("+", GUILayout.Width(30)))
            {
                vParams.Add(new CustomEventData.EventParam());
            }
            GUILayout.EndHorizontal();

            for(int i =0; i < vParams.Count; ++i)
            {
                var paramData = vParams[i];
                GUILayout.BeginHorizontal();
                paramData.name = EditorGUILayout.TextField(paramData.name, GUILayout.Width(argvWidth));

                if (paramData.type == EVariableType.eUserData)
                {
                    paramData.type = (EVariableType)Framework.ED.EditorEnumPop.PopEnum(string.Empty, (int)paramData.type, typeof(EVariableType), new GUILayoutOption[] { GUILayout.Width(varWidth / 2) });
                    if (GUILayout.Button(AgentTreeUtil.GetATClassTypeDisplayName(paramData.clsId), GUILayout.Width(varWidth / 2)))
                    {
                        ATExportTypeProvider.PopSearch((clasType, index) =>
                        {
                            var paramData = vParams[index];
                            paramData.clsId = AgentTreeUtil.GetATClassID(clasType);
                            vParams[index] = paramData;
                        }, i);
                    }
                    EditorGUILayout.LabelField("", GUILayout.Width(argvWidth));
                }
                else
                {
                    paramData.type = (EVariableType)Framework.ED.EditorEnumPop.PopEnum(string.Empty, (int)paramData.type, typeof(EVariableType), new GUILayoutOption[] { GUILayout.Width(argvWidth) });
                    paramData.defaultValue = EditorGUILayout.TextField(paramData.defaultValue, GUILayout.Width(argvWidth));
                }
                paramData.canEdit = EditorGUILayout.Toggle(paramData.canEdit, GUILayout.Width(30));

                if(GUILayout.Button("-", GUILayout.Width(30)))
                {
                    if(EditorUtility.DisplayDialog("提示", "是否删除该参数","删除", "取消"))
                    {
                        vParams.RemoveAt(i);
                        GUILayout.EndHorizontal();
                        break;
                    }
                }

                GUILayout.EndHorizontal();

                vParams[i] = paramData;
            }
            return vParams;
        }
        //-----------------------------------------------------
        int NewEventTypeId()
        {
            if (ms_EventObject == null) return 1;
            if (ms_EventObject.eventData == null || ms_EventObject.eventData.vEvents == null)
                return 1;

            HashSet<int> types = new HashSet<int>();
            foreach (var db in ms_EventObject.eventData.vEvents)
                types.Add(db.eventType);

            int strackCnt = 0;
            int type = 1;
            while(types.Contains(type))
            {
                type++;
                if (strackCnt >= int.MaxValue - 1) break;
            }
            return type;
        }
    }
}
#endif