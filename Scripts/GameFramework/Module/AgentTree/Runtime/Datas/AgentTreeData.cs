/********************************************************************
生成日期:	06:30:2025
类    名: 	AgentTreeData
作    者:	HappLI
描    述:	过场动画行为树
*********************************************************************/
#if UNITY_EDITOR
using Framework.AT.Editor;
#endif
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Framework.AT.Runtime
{
    //-----------------------------------------------------
    [System.Serializable]
    public class AgentTreeData
    {
        public EnterTask[] tasks;
        public ActionNode[] actions;
        public CustomEvent[] events;
        public ParallelCondition[] parallelConditions;
        [UnityEngine.SerializeField] VaribaleSerizlizeGuidData varGuids;
        [System.NonSerialized]private Dictionary<short, IVariable> m_vVariables = null;
        [System.NonSerialized] private Dictionary<short, BaseNode> m_vNodes = null;
        [System.NonSerialized] private Dictionary<short, BaseNode> m_vVarOwnerNodes = null;
        [System.NonSerialized] private bool m_bInited = false;
        //-----------------------------------------------------
        public IVariable GetVariable(short guid)
        {
            if (m_vVariables == null) return null;
            IVariable variable = null;
            m_vVariables.TryGetValue(guid, out variable);
            return variable;
        }
        //-----------------------------------------------------
        internal void SetVarOwner(short varGuid, BaseNode pNode)
        {
            if (pNode.type == (short)EActionType.eGetVariable)
                return;
            if (m_vVarOwnerNodes == null)
                m_vVarOwnerNodes = new Dictionary<short, BaseNode>(32);
            m_vVarOwnerNodes[varGuid] = pNode;
        }
        //-----------------------------------------------------
        public BaseNode GetVarOwnerNode(short varGuid)
        {
            if (m_vVarOwnerNodes == null)
                return null;
            if (m_vVarOwnerNodes.TryGetValue(varGuid, out var pNode))
                return pNode;
            return null;
        }
        //-----------------------------------------------------
        internal BaseNode GetNode(short guid)
        {
            if (m_vNodes == null)
            {
                Init(true);
                return null;
            }
            if (m_vNodes.TryGetValue(guid, out var pNode))
                return pNode;
            return null;
        }
        //-----------------------------------------------------
        public int GetNodeCnt()
        {
            int nodeCnt = 0;
            if (tasks != null) nodeCnt += tasks.Length;
            if (actions != null) nodeCnt += actions.Length;
            if (events != null) nodeCnt += events.Length;
            if (parallelConditions != null) nodeCnt += parallelConditions.Length;
            return nodeCnt;
        }
        //-----------------------------------------------------
        public bool IsValid()
        {
            return GetNodeCnt() > 0;
        }
        //-----------------------------------------------------
        internal void Init(bool bForce = false)
        {
            if (!bForce && m_bInited)
                return;
            m_bInited = true;
            int cnt = varGuids.GetVariableCnt();
            if (cnt > 0)
            {
                if (m_vVariables == null) m_vVariables = new Dictionary<short, IVariable>(cnt);
                else m_vVariables.Clear();
                varGuids.Fill(m_vVariables);
            }
            int nodeCnt = GetNodeCnt();
            if (nodeCnt > 0)
            {
                if (m_vVarOwnerNodes == null) m_vVarOwnerNodes = new Dictionary<short, BaseNode>(nodeCnt);
                if (m_vNodes == null)  m_vNodes = new Dictionary<short, BaseNode>(nodeCnt);
                m_vNodes.Clear();
                m_vVarOwnerNodes.Clear();
                if (tasks != null)
                {
                    for (int i = 0; i < tasks.Length; ++i)
                        m_vNodes[tasks[i].guid] = tasks[i];
                }
                if (actions != null)
                {
                    for (int i = 0; i < actions.Length; ++i)
                        m_vNodes[actions[i].guid] = actions[i];
                }
                if (events != null)
                {
                    for (int i = 0; i < events.Length; ++i)
                        m_vNodes[events[i].guid] = events[i];
                }
                if (parallelConditions != null)
                {
                    for (int i = 0; i < parallelConditions.Length; ++i)
                        m_vNodes[parallelConditions[i].guid] = parallelConditions[i];
                }
            }
            else
            {
                if (m_vNodes != null) m_vNodes.Clear();
                if (m_vVarOwnerNodes != null) m_vVarOwnerNodes.Clear();
            }
            if (m_vNodes != null)
            {
                foreach (var db in m_vNodes)
                {
                    db.Value.Init(this);
                }
            }
        }
        //-----------------------------------------------------
        public bool Deserialize(string content = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(content))
                    JsonUtility.FromJsonOverwrite(content, this);
                Init();
                return true;
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
                return false;
            }
        }
#if UNITY_EDITOR
        //-----------------------------------------------------
        internal Dictionary<short, BaseNode> GetNodes()
        {
            return m_vNodes;
        }
        //-----------------------------------------------------
        internal Dictionary<short, IVariable> GetVariableGUIDs()
        {
            if (m_vVariables == null) m_vVariables = new Dictionary<short, IVariable>();
            return m_vVariables;
        }
        //-----------------------------------------------------
        internal void SetVariables(Dictionary<short,IVariable> guidVars)
        {
            if (m_vVariables == null) m_vVariables = new Dictionary<short, IVariable>();
            m_vVariables.Clear();
            foreach (var db in guidVars)
            {
                m_vVariables[db.Key] = db.Value;
            }
        }
        //-----------------------------------------------------
        internal string Serialize(bool toJson)
        {
            varGuids = new VaribaleSerizlizeGuidData();
            if (m_vVariables != null)
                varGuids.Save(m_vVariables);
            Init(true);
            if (toJson) return JsonUtility.ToJson(this, true);
            return null;
        }
#endif
    }
    //-----------------------------------------------------
    //! AAgentTreeObject 
    //-----------------------------------------------------
    public abstract class AAgentTreeObject :ScriptableObject
    {
        public AgentTreeData atData;
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(AAgentTreeObject))]
    public class AAgentTreeObjectEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            AAgentTreeObject cutsceneObject = (AAgentTreeObject)target;
            if (GUILayout.Button("编辑"))
            {
                Editor.AgentTreeWindow.Open(cutsceneObject.atData, cutsceneObject);
            }
        }
        [UnityEditor.Callbacks.OnOpenAsset(0)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            var obj = EditorUtility.InstanceIDToObject(instanceID);
            if (obj != null && obj is AAgentTreeObject)
            {
                AAgentTreeObject atObj = obj as AAgentTreeObject;
                Editor.AgentTreeWindow.Open(atObj.atData, atObj);
                return true;
            }
            return false;
        }
    }
#endif
}