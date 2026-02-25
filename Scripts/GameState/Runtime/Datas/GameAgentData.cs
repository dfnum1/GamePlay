/********************************************************************
生成日期:	11:07:2025
类    名: 	GameAgentData
作    者:	HappLI
描    述:	世界逻辑蓝图数据体
*********************************************************************/
using Framework.AT.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.State.Runtime
{
    public class GameAgentData :IGameWorldItem
    {
        public int agentId =0;
        public AgentTreeData atData;
#if UNITY_EDITOR
        public string strDesc = "";
#endif
    }
}

