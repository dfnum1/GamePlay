/********************************************************************
生成日期:	5:11:2020  20:36
类    名: 	BaseATDrawer
作    者:	HappLI
描    述:	基础自定义渲染规则
*********************************************************************/
using System.Collections.Generic;
using System;
#if USE_FIXEDMATH
using ExternEngine;
#else
using FFloat = System.Single;
#endif
namespace Framework
{
    public class BaseATDrawerKey
    {
        public const string Key_ActorTypeDraw = "ActorTypeDraw";
        public const string Key_ActorSubTypeDraw = "ActorSubTypeDraw";
        public const string Key_AttackGroupDraw = "AttackGroupDraw";
        public const string Key_DrawAttributePop = "DrawAttributePop";
        public const string Key_BuffStateDraw = "BuffStateDraw";
        public const string Key_DrawFormulaTypePop = "DrawFormulaTypePop";
        public const string Key_DrawCsvTablePop = "DrawCsvTablePop";
        public const string Key_DrawProxyDbTypePop = "DrawProxyDbTypePop";
    }
}
