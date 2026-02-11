/********************************************************************
生成日期:	11:07:2025
类    名: 	GameStateData
作    者:	HappLI
描    述:	游戏状态数据体
*********************************************************************/
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
        public string name;
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
        public string name;
        public string strDesc = "";
#endif
    }
    //------------------------------------------------------
    [System.Serializable]
    public class GameWorldData
    {
        public GameStateData gameStateData = new GameStateData();
        public List<GameStateModeData> modeDatas = new List<GameStateModeData>(2);
        public GameLevelData gameLevel;
        public GameVariables warVariables = new GameVariables();
        public List<GameAgentData> warAgents = new List<GameAgentData>();
    }
}

