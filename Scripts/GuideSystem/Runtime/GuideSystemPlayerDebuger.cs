/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	GuideSystemPlayerDebuger
作    者:	HappLI
描    述:	引导系统连接调试
*********************************************************************/
#if USE_DEBUG || UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking.PlayerConnection;
namespace Framework.Guide
{
    public enum EGuideDebugType : byte
    {
        GetDatas =0,
        ExecuteNode,
        TestNode,
        StopGuide,
        OpenCurGuide,
        SyncGuideGroupData,
        SyncGuideFlags,
    }
    //-----------------------------------------------------
    [System.Serializable]
    public struct GuideDebugInfo
    {
        public byte msgType;
        public int msgData;
        public string msgContent;
    }
    //-----------------------------------------------------
    [System.Serializable]
    internal struct GuideDebugTestNode
    {
        public int guideGuid;
        public int startNodeGuid;
        public bool bForce;
    }
    //-----------------------------------------------------
    [System.Serializable]
    internal struct GuideDebugFlags
    {
        public List<int> keys;
        public List<UInt64> values;

    }
    //-----------------------------------------------------
    [System.Serializable]
    internal struct GuideDebugNodeInfo
    {
        public string nodeContent;
        public List<int> portKeys;
        public List<int> portValues;
        public List<string> portStrValues;
    }
    //-----------------------------------------------------
    [System.Serializable]
    internal struct GuideDebugNodeDatas
    {
        public List<string> datas;
    }
    //-----------------------------------------------------
    internal class GuideSystemPlayerDebuger
    {
        internal static readonly Guid kSendPlayerToEditor = new Guid("e225f75e9217344ae83c81f2a84fc759");
        internal static readonly Guid kSendEditorToPlayer = new Guid("6de2c3825f6bae7cd57be882949511c0");

        //-----------------------------------------------------
        public static void Enable(bool bEnable)
        {
            if(bEnable)
            {
                if(PlayerConnection.instance) PlayerConnection.instance.Register(kSendEditorToPlayer, OnPlayerDebugResponse);
            }
            else
            {
                if (PlayerConnection.instance) PlayerConnection.instance.Unregister(kSendEditorToPlayer, OnPlayerDebugResponse);
            }
        }
        //------------------------------------------------------
        static void OnPlayerDebugResponse(MessageEventArgs data)
        {
            try
            {
                var nodeJson = Encoding.UTF8.GetString(data.data);
                var debugInfo = JsonUtility.FromJson<GuideDebugInfo>(nodeJson);
                if (debugInfo.msgType == (byte)EGuideDebugType.GetDatas)
                {
                    if (PlayerConnection.instance && PlayerConnection.instance.isConnected)
                    {
                        var infoMsg = new GuideDebugInfo();
                        infoMsg.msgType = debugInfo.msgType;
                        infoMsg.msgData = -1;

                        var nodes = GuideSystem.getInstance().datas;
                        GuideDebugNodeDatas debugDatas = new GuideDebugNodeDatas();
                        debugDatas.datas = new List<string>();
                        foreach (var db in nodes)
                        {
                            debugDatas.datas.Add(JsonUtility.ToJson(db.Value));
                        }

                        infoMsg.msgContent = JsonUtility.ToJson(debugDatas);
                        PlayerConnection.instance.Send(kSendPlayerToEditor, Encoding.UTF8.GetBytes(JsonUtility.ToJson(infoMsg)));
                    }
                }
                else if (debugInfo.msgType == (byte)EGuideDebugType.TestNode)
                {
                    GuideDebugTestNode node = JsonUtility.FromJson<GuideDebugTestNode>(debugInfo.msgContent);

                    if(GuideSystem.getInstance().datas.TryGetValue(node.guideGuid, out var guideGroup))
                    {
                        GuideSystem.getInstance().Enable(true);
                        GuideSystem.getInstance().OverGuide(false);
                        GuideSystem.getInstance().AddGuide(guideGroup, true);
                        GuideSystem.getInstance().DoGuide(node.guideGuid, node.startNodeGuid, node.bForce);
                    }
                }
                if (debugInfo.msgType == (byte)EGuideDebugType.OpenCurGuide)
                {
                    if(PlayerConnection.instance && PlayerConnection.instance.isConnected)
                    {
                        var infoMsg = new GuideDebugInfo();
                        infoMsg.msgType = debugInfo.msgType;
                        infoMsg.msgData = -1;
                        var node = GuideSystem.getInstance().DoingTriggerNode;
                        if(node!=null)
                        {
                            infoMsg.msgData = node.guideGroupGUID;
                        }
                        GuideDebugTestNode testNode = new GuideDebugTestNode();
                        testNode.guideGuid = GuideSystem.getInstance().DoingTriggerNode != null ? GuideSystem.getInstance().DoingTriggerNode.Guid : 0;
                        testNode.startNodeGuid = GuideSystem.getInstance().DoingSeqNode != null ? GuideSystem.getInstance().DoingSeqNode.Guid : 0;
                        infoMsg.msgContent = JsonUtility.ToJson(testNode);
                        PlayerConnection.instance.Send(kSendPlayerToEditor, Encoding.UTF8.GetBytes(JsonUtility.ToJson(infoMsg)));
                    }
                }
                else if (debugInfo.msgType == (byte)EGuideDebugType.StopGuide)
                {
                    GuideSystem.getInstance().OverGuide(false);
                }
                else if (debugInfo.msgType == (byte)EGuideDebugType.SyncGuideGroupData)
                {
                    GuideGroup groupData = JsonUtility.FromJson<GuideGroup>(debugInfo.msgContent);
                    if (groupData != null)
                    {
                        GuideSystem.getInstance().AddGuide(groupData);
                    }
                }
                else if (debugInfo.msgType == (byte)EGuideDebugType.SyncGuideFlags)
                {
                    if (PlayerConnection.instance && PlayerConnection.instance.isConnected)
                    {
                        var flags = GuideSystem.getInstance().GetGuideFlags();
                        GuideDebugFlags msgFlags = new GuideDebugFlags();
                        msgFlags.keys = flags.Keys.ToList();
                        msgFlags.values = flags.Values.ToList();

                        var infoMsg = new GuideDebugInfo();
                        infoMsg.msgType = debugInfo.msgType;
                        infoMsg.msgData = 0;
                        infoMsg.msgContent = JsonUtility.ToJson(msgFlags);

                        PlayerConnection.instance.Send(kSendPlayerToEditor, Encoding.UTF8.GetBytes(JsonUtility.ToJson(infoMsg)));
                    }
                }
            }
            catch
            {

            }
        }
        //-----------------------------------------------------
        public static void OnGuideExecuteNode(BaseNode pNode)
        {
            if (PlayerConnection.instance == null || !PlayerConnection.instance.isConnected)
                return;
            GuideDebugInfo debugInfo = new GuideDebugInfo();
            debugInfo.msgType = (byte)EGuideDebugType.ExecuteNode;
            if (pNode is StepNode) debugInfo.msgData = 0;
            else if (pNode is TriggerNode) debugInfo.msgData = 1;
            else if (pNode is ExcudeNode) debugInfo.msgData = 2;
            else if (pNode is GuideOperate) debugInfo.msgData = 3;
            else debugInfo.msgData = 4;

            GuideDebugNodeInfo nodeInfo = new GuideDebugNodeInfo();
            nodeInfo.nodeContent = JsonUtility.ToJson(pNode);
            var ports = pNode.GetArgvPorts();
            if(ports!=null)
            {
                nodeInfo.portKeys = new List<int>(ports.Count);
                nodeInfo.portValues = new List<int>(ports.Count);
                nodeInfo.portStrValues = new List<string>(ports.Count);
                foreach (var db in ports)
                {
                    nodeInfo.portKeys.Add(db.guid);
                    nodeInfo.portValues.Add(db.fillValue);
                    nodeInfo.portStrValues.Add(db.fillStrValue);
                }
            }

            debugInfo.msgContent = JsonUtility.ToJson(nodeInfo);


            PlayerConnection.instance.Send(kSendPlayerToEditor, Encoding.UTF8.GetBytes(JsonUtility.ToJson(debugInfo)));
        }
        //-----------------------------------------------------
        public static void StopGuide(int guideGroup)
        {
            if (PlayerConnection.instance == null || !PlayerConnection.instance.isConnected)
                return;
            GuideDebugInfo debugInfo = new GuideDebugInfo();
            debugInfo.msgType = (byte)EGuideDebugType.StopGuide;
            debugInfo.msgData = guideGroup;
            PlayerConnection.instance.Send(kSendPlayerToEditor, Encoding.UTF8.GetBytes(JsonUtility.ToJson(debugInfo)));
        }
    }
}
#endif
