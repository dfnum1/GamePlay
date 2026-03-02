/********************************************************************
生成日期:	11:07:2025
类    名: 	GameStateData
作    者:	HappLI
描    述:	游戏状态数据体
*********************************************************************/
using Framework.AT.Runtime;
using System.Collections.Generic;
using UnityEngine;
namespace Framework.State.Runtime
{
    //------------------------------------------------
    //! 游戏状态下模式数据体
    //------------------------------------------------
    [System.Serializable]
    public class GameStateLogicData : IGameWorldItem
    {
        public int logicType;
        public bool enabled;
    }
    //------------------------------------------------
    //! 游戏状态下模式数据体
    //------------------------------------------------
    [StateIcon("GameWorld/gamemode"),System.Serializable]
    public class GameStateModeData : IGameWorldItem
    {
        public int modeType;
        public List<GameStateLogicData> modeLogics;
#if UNITY_EDITOR
        public string name="玩法模式";
        public string strDesc = "";
#endif
    }
    //------------------------------------------------
    //! 游戏状态数据体
    //------------------------------------------------
    [StateIcon("GameWorld/gamestate"), System.Serializable]
    public class GameStateData : IGameWorldItem
    {
        public int stateType;
        public int activeMode;
        public List<GameStateLogicData> stateLogics;
#if UNITY_EDITOR
        public string name="游戏状态";
        public string strDesc = "";
#endif
    }
    //------------------------------------------------------
    //! 游戏蓝图数据
    //------------------------------------------------------
    [StateIcon("AT/AgentTree"), System.Serializable]
    public class GameWorldATData : IGameWorldItem
    {
        public AgentTreeData worldAgentTree = new AgentTreeData();
    }
    //------------------------------------------------------
    //! 游戏世界数据体
    //------------------------------------------------------
    [System.Serializable]
    public class GameWorldData
    {
        public GameWorldATData atData;
        public GameStateData gameStateData = new GameStateData();
        public List<GameStateModeData> modeDatas = new List<GameStateModeData>(2);
        public GameLevelData gameLevel;
        public GameVariables warVariables = new GameVariables();
        public List<GameAgentData> warAgents = new List<GameAgentData>();
#if UNITY_EDITOR
        internal void Deserialize()
        {
            if (gameLevel != null)
                gameLevel.Deserialize(null);
        }
        internal string Serialize()
        {
            if(gameLevel!=null) gameLevel.Serialize();
            return JsonUtility.ToJson(this, true);
        }
        internal ushort NewWarAgentID()
        {
            ushort id = 1;
            HashSet<ushort> vGp = new HashSet<ushort>();
            if (warAgents != null)
            {
                foreach (var db in warAgents)
                {
                    vGp.Add(db.agentId);
                }
            }
            int stackCnt = 65535;
            while (vGp.Contains(id))
            {
                id++;
                stackCnt--;
                if (stackCnt <= 0) break;
            }
            return id;

        }
#endif
    }
}

