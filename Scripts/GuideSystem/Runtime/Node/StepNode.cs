/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	StepNode
作    者:	HappLI
描    述:	引导步骤节点
*********************************************************************/
using System.Collections.Generic;
namespace Framework.Guide
{
    [System.Serializable]
    public class StepNode : SeqNode
    {
        public int type = -1;
        /// <summary>
        /// 延时
        /// </summary>
        public float fDeltaTime = 0;
        /// <summary>
        /// 交互延迟
        /// </summary>
        public float fDeltaSignTime = 0;
        /// <summary>
        /// 非强制
        /// </summary>
        public bool bOption = false;
        /// <summary>
        /// 关键步骤
        /// </summary>
    //    public bool bMaster = false;
        /// <summary>
        /// 自动跳转
        /// </summary>
        public bool bAutoNext = false;
        /// <summary>
        /// 信号检测
        /// </summary>
        public bool bAutoSignCheck = false;

        public bool bSignFailedListenerBreak = false;
        public int nSignFailedBreakSkipTo = -1;
        public float fSignFailedBreakDelayTime = 0;
        /// <summary>
        /// 自动跳转倒计时
        /// </summary>
        public float fAutoTime = 0;
        public int autoExcudeNodeGuid = 0;
        public int[] argvGuids = null;

        public string[] beginEvents = null;
        [System.NonSerialized]
        public List<IUserData> vBeginEvents = null;

        public string[] endEvents = null;
        [System.NonSerialized]
        public List<IUserData> vEndEvents = null;


        [System.NonSerialized]
        public List<ArgvPort> _Ports = new List<ArgvPort>();

        [System.NonSerialized]
        public ExcudeNode pAutoExcudeNode = null;

        [System.NonSerialized]
        public BaseNode pSignFailedListenerBreakNode = null;

        //-----------------------------------------------------
        public override List<ArgvPort> GetArgvPorts()
        {
            return _Ports;
        }
        //-----------------------------------------------------
        public override int GetEnumType()
        {
            return type;
        }
        //-----------------------------------------------------
        public override ExcudeNode GetAutoExcudeNode()
        {
            return pAutoExcudeNode;
        }
        //-----------------------------------------------------
        public override List<IUserData> GetBeginEvents()
        {
#if UNITY_EDITOR
            if (vBeginEvents == null) vBeginEvents = new List<IUserData>();
#endif
            return vBeginEvents;
        }
        //-----------------------------------------------------
        public override List<IUserData> GetEndEvents()
        {
#if UNITY_EDITOR
            if (vEndEvents == null) vEndEvents = new List<IUserData>();
#endif
            return vEndEvents;
        }
        //-----------------------------------------------------
        public override float GetDeltaTime()
        {
            return fDeltaTime;
        }
        //-----------------------------------------------------
        public override float GetAutoNextTime()
        {
            return bAutoNext?fAutoTime:0;
        }
        //-----------------------------------------------------
        public override float GetFailSignCheckTime()
        {
            return bSignFailedListenerBreak?fSignFailedBreakDelayTime:0;
        }
        //-----------------------------------------------------
        public override bool IsAutoNext()
        {
            return bAutoNext;
        }
        //-----------------------------------------------------
        public override bool IsAutoSignCheck()
        {
            return bAutoSignCheck;
        }
        //-----------------------------------------------------
        public override bool IsSuccessedListenerBreak()
        {
            return bSignFailedListenerBreak;
        }
        //-----------------------------------------------------
        public override bool IsOption()
        {
            return bOption;
        }
        //-----------------------------------------------------
        public override float GetDeltaSignTime()
        {
            return fDeltaSignTime;
        }
        //-----------------------------------------------------
        public override bool IsCustom()
        {
            return this.type >= (int)GuideStepType.CustomBegin && this.type <= (int)GuideStepType.CustomEnd;
        }
        //-----------------------------------------------------
        public override void Init(GuideGroup pGroup)
        {
            base.Init(pGroup);
            _Ports.Clear();
            if (argvGuids != null)
            {
                for (int i = 0; i < argvGuids.Length; ++i)
                {
                    ArgvPort port = pGroup.GetPort(argvGuids[i]);
                    if(port == null)  continue;
                    _Ports.Add(port);
                }
            }
            vEndEvents = null;
            vBeginEvents = null;
            if (beginEvents != null && beginEvents.Length > 0)
            {
                vBeginEvents = new List<IUserData>(beginEvents.Length);
                for (int i = 0; i < beginEvents.Length; ++i)
                {
                    IUserData pEvt = GuideSystem.getInstance().BuildEvent(beginEvents[i]);
                    if (pEvt == null) continue;
                    vBeginEvents.Add(pEvt);
                }
            }
            if (endEvents != null && endEvents.Length > 0)
            {
                vEndEvents = new List<IUserData>(endEvents.Length);
                for (int i = 0; i < endEvents.Length; ++i)
                {
                    IUserData pEvt = GuideSystem.getInstance().BuildEvent(endEvents[i]);
                    if (pEvt == null) continue;
                    vEndEvents.Add(pEvt);
                }
            }

            if (autoExcudeNodeGuid != 0)
                pAutoExcudeNode = pGroup.GetNode<ExcudeNode>(autoExcudeNodeGuid);
            else
                pAutoExcudeNode = null;

            if (nSignFailedBreakSkipTo != 0)
                pSignFailedListenerBreakNode = pGroup.GetNode<BaseNode>(nSignFailedBreakSkipTo);
            else pSignFailedListenerBreakNode = null;
        }
#if UNITY_EDITOR
        //-----------------------------------------------------
        public override void SetArgvPorts(List<ArgvPort> vPorts)
        {
            _Ports = vPorts;
            if (_Ports != null && _Ports.Count > 0)
            {
                argvGuids = new int[_Ports.Count];
                for (int i = 0; i < _Ports.Count; ++i)
                {
                    argvGuids[i] = _Ports[i].guid;
                }
            }
            else
                argvGuids = null;
        }
        //------------------------------------------------------
        internal override void CheckPorts()
        {
            base.CheckPorts();
            Framework.Guide.Editor.GuideSystemEditor.NodeAttr nodeAttr;
            if (!Framework.Guide.Editor.GuideSystemEditor.StepTypes.TryGetValue(type, out nodeAttr))
                return;

            if (_Ports == null) _Ports = new List<ArgvPort>();
            if (nodeAttr.argvs != null && nodeAttr.argvs.Count != _Ports.Count)
            {
                if (nodeAttr.argvs.Count < _Ports.Count)
                    _Ports.RemoveRange(nodeAttr.argvs.Count, _Ports.Count - nodeAttr.argvs.Count);
                else
                {
                    for (int i = _Ports.Count; i < nodeAttr.argvs.Count; ++i)
                    {
                        _Ports.Add(new ArgvPort() { guid = Framework.Guide.Editor.GuideEditorLogic.BuildPortGUID() });
                    }
                }
            }
        }
        //------------------------------------------------------
        public override void Save()
        {
            base.Save();
            if (_Ports != null && _Ports.Count>0)
            {
                argvGuids = new int[_Ports.Count];
                for (int i = 0; i < _Ports.Count; ++i)
                {
                    argvGuids[i] = _Ports[i].guid;
                }
            }
            else
                argvGuids = null;

            if (vBeginEvents !=null && vBeginEvents.Count > 0)
            {
                List<string> vCmd = new List<string>();
                for (int i = 0; i < vBeginEvents.Count; ++i)
                {
                    if (vBeginEvents[i] == null) continue;
                    vCmd.Add(vBeginEvents[i].ToString());
                }
                beginEvents = vCmd.ToArray();
            }
            else
                beginEvents = null;

            if (vEndEvents!=null && vEndEvents.Count > 0)
            {
                List<string> vCmd = new List<string>();
                for (int i = 0; i < vEndEvents.Count; ++i)
                {
                    if (vEndEvents[i] == null) continue;
                    vCmd.Add(vEndEvents[i].ToString());
                }
                endEvents = vCmd.ToArray();
            }
            else
                endEvents = null;

            if (pAutoExcudeNode != null)
                autoExcudeNodeGuid = pAutoExcudeNode.Guid;
            else
                autoExcudeNodeGuid = 0;
            if (pSignFailedListenerBreakNode != null)
                nSignFailedBreakSkipTo = pSignFailedListenerBreakNode.Guid;
            else nSignFailedBreakSkipTo = 0;
        }
#endif
    }
}
