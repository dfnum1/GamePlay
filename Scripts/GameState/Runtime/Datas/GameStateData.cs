/********************************************************************
生成日期:	11:07:2025
类    名: 	GameStateData
作    者:	HappLI
描    述:	游戏状态数据体
*********************************************************************/
using Framework.AT.Runtime;
using System.Collections.Generic;
namespace Framework.State.Runtime
{
    //------------------------------------------------
    //! 游戏状态下模式数据体
    //------------------------------------------------
    [StateIcon("GameWorld/gamemode"),System.Serializable]
    public class GameStateModeData : IGameWorldItem
    {
        public int modeType;
        public List<int> modeLogics;
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
        public List<int> stateLogics;
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
    }
}

