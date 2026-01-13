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
    public class GameStateModeData
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
    public class GameStateData
    {
        public int stateType;
        public GameStateModeData modeData;
        public List<int> stateLogics;
#if UNITY_EDITOR
        public string name;
        public string strDesc = "";
#endif
    }
}

