/********************************************************************
生成日期:	11:07:2025
类    名: 	BattleAgentAttribute
作    者:	HappLI
描    述:	战争世界中涉及到的属性数据定义
*********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.State.Runtime
{
    public enum EBattleAgentValueType : byte
    {
        eInt,
        eFloat,
    }
    //--------------------------------------------------------
    //! 属性
    //--------------------------------------------------------
    [System.Serializable]
    public struct BattleAgentAttribute
    {
        public int attriType;
        public EBattleAgentValueType attriValueType; // 0:int 1:float
        public int attriValue;
#if UNITY_EDITOR
        public string strName;
#endif
    }
    //--------------------------------------------------------
    //! 属性列表
    //--------------------------------------------------------
    [System.Serializable]
    public class BattleAgentAttributes
    {
        public BattleAgentAttribute[] attributes;
    }
}

