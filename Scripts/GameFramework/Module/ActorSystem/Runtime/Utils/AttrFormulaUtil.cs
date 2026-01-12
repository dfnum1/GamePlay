/********************************************************************
生成日期:	5:11:2020  20:36
类    名: 	AttrFormulaUtil
作    者:	HappLI
描    述:	属性表达式计算工具
*********************************************************************/
using System.Collections.Generic;
using static Framework.ActorSystem.Runtime.AActorAttrDatas;
using System;

namespace Framework.ActorSystem.Runtime
{
    //-----------------------------------------------------
    //! AttrFormulaUtil
    //-----------------------------------------------------
    public class AttrFormulaUtil
    {
        static Stack<float> ms_FormulaStack = null;
        public static float CalcAttrFormula(AttrFormula formula, Actor pAttacker, Actor pTarget)
        {
            if (ms_FormulaStack == null) ms_FormulaStack = new Stack<float>(2);
            ms_FormulaStack.Clear();
            float value = CalcLambdaList(formula.vLambda, pAttacker, pTarget);
            ms_FormulaStack.Clear();
            return value;
        }
        //-----------------------------------------------------
        private static float CalcLambdaList(List<AttrFormula.LambdaParam> vLambda, Actor pAttacker, Actor pTarget)
        {
            if (vLambda == null || vLambda.Count == 0)
                return 0f;

            foreach (var lambda in vLambda)
            {
                if (lambda == null)
                {
                    ms_FormulaStack.Push(0f);
                    continue;
                }

                try
                {
                    switch (lambda.type)
                    {
                        case EAttrFormulaType.eAdd:
                        case EAttrFormulaType.eSub:
                        case EAttrFormulaType.eMul:
                        case EAttrFormulaType.eDiv:
                            {
                                float result = 0f;
                                if (lambda.isUnary)
                                {
                                    float leftA = ms_FormulaStack.Count > 0 ? ms_FormulaStack.Pop() : 0;
                                    result = lambda.type switch
                                    {
                                        EAttrFormulaType.eAdd => leftA + lambda.paramValue0,
                                        EAttrFormulaType.eSub => leftA - lambda.paramValue0,
                                        EAttrFormulaType.eMul => leftA * lambda.paramValue0,
                                        EAttrFormulaType.eDiv => (lambda.paramValue0 == 0) ? 0f : leftA / lambda.paramValue0,
                                        _ => 0f
                                    };
                                }
                                else if (lambda.subLambda != null && lambda.subLambda.Count == 2)
                                {
                                    float a = CalcLambdaList(new List<AttrFormula.LambdaParam> { lambda.subLambda[0] }, pAttacker, pTarget);
                                    float b = CalcLambdaList(new List<AttrFormula.LambdaParam> { lambda.subLambda[1] }, pAttacker, pTarget);
                                    result = lambda.type switch
                                    {
                                        EAttrFormulaType.eAdd => a + b,
                                        EAttrFormulaType.eSub => a - b,
                                        EAttrFormulaType.eMul => a * b,
                                        EAttrFormulaType.eDiv => (b == 0) ? 0f : a / b,
                                        _ => 0f
                                    };
                                }
                                else
                                {
                                    float b = ms_FormulaStack.Count > 0 ? ms_FormulaStack.Pop() : lambda.paramValue0;
                                    float a = ms_FormulaStack.Count > 0 ? ms_FormulaStack.Pop() : lambda.paramValue1;
                                    result = lambda.type switch
                                    {
                                        EAttrFormulaType.eAdd => a + b,
                                        EAttrFormulaType.eSub => a - b,
                                        EAttrFormulaType.eMul => a * b,
                                        EAttrFormulaType.eDiv => (b == 0) ? 0f : a / b,
                                        _ => 0f
                                    };
                                }
                                ms_FormulaStack.Push(result);
                                break;
                            }
                        case EAttrFormulaType.ePower:
                        case EAttrFormulaType.eMin:
                        case EAttrFormulaType.eMax:
                            {
                                float result = 0f;
                                if (lambda.subLambda != null && lambda.subLambda.Count >= 2)
                                {
                                    float a = CalcLambdaList(new List<AttrFormula.LambdaParam> { lambda.subLambda[0] }, pAttacker, pTarget);
                                    float b = CalcLambdaList(new List<AttrFormula.LambdaParam> { lambda.subLambda[1] }, pAttacker, pTarget);
                                    switch (lambda.type)
                                    {
                                        case EAttrFormulaType.ePower: result = (float)Math.Pow(a, b); break;
                                        case EAttrFormulaType.eMin: result = Math.Min(a, b); break;
                                        case EAttrFormulaType.eMax: result = Math.Max(a, b); break;
                                    }
                                }
                                else
                                {
                                    float b = ms_FormulaStack.Count > 0 ? ms_FormulaStack.Pop() : lambda.paramValue0;
                                    float a = ms_FormulaStack.Count > 0 ? ms_FormulaStack.Pop() : lambda.paramValue1;
                                    switch (lambda.type)
                                    {
                                        case EAttrFormulaType.ePower: result = (float)Math.Pow(a, b); break;
                                        case EAttrFormulaType.eMin: result = Math.Min(a, b); break;
                                        case EAttrFormulaType.eMax: result = Math.Max(a, b); break;
                                    }
                                }
                                ms_FormulaStack.Push(result);
                                break;
                            }
                        case EAttrFormulaType.eFloor:
                        case EAttrFormulaType.eCeil:
                        case EAttrFormulaType.eAbs:
                            {
                                float a;
                                if (lambda.subLambda != null && lambda.subLambda.Count > 0)
                                    a = CalcLambdaList(lambda.subLambda, pAttacker, pTarget);
                                else
                                    a = ms_FormulaStack.Count > 0 ? ms_FormulaStack.Pop() : lambda.paramValue0;
                                float result = 0f;
                                switch (lambda.type)
                                {
                                    case EAttrFormulaType.eFloor: result = (float)Math.Floor(a); break;
                                    case EAttrFormulaType.eCeil: result = (float)Math.Ceiling(a); break;
                                    case EAttrFormulaType.eAbs: result = Math.Abs(a); break;
                                }
                                ms_FormulaStack.Push(result);
                                break;
                            }
                        case EAttrFormulaType.eRandom:
                            {
                                if (lambda.subLambda != null && lambda.subLambda.Count >= 2)
                                {
                                    float min = CalcLambdaList(new List<AttrFormula.LambdaParam> { lambda.subLambda[0] }, pAttacker, pTarget);
                                    float max = CalcLambdaList(new List<AttrFormula.LambdaParam> { lambda.subLambda[1] }, pAttacker, pTarget);
                                    ms_FormulaStack.Push(UnityEngine.Random.Range(min, max));
                                }
                                else
                                {
                                    ms_FormulaStack.Push(UnityEngine.Random.Range(lambda.paramValue0, lambda.paramValue1));
                                }
                                break;
                            }
                        case EAttrFormulaType.eBracket:
                            {
                                if (lambda.subLambda != null && lambda.subLambda.Count > 0)
                                    ms_FormulaStack.Push(CalcLambdaList(lambda.subLambda, pAttacker, pTarget));
                                else
                                    ms_FormulaStack.Push(lambda.paramValue0);
                                break;
                            }
                        case EAttrFormulaType.eActorAttr:
                            {
                                byte attrType = (byte)lambda.paramValue1;
                                int camp = (int)lambda.paramValue0;
                                float value = 0f;
                                if (camp == 0 && pAttacker != null)
                                    value = pAttacker.GetAttr(attrType);
                                else if (camp == 1 && pTarget != null)
                                    value = pTarget.GetAttr(attrType);
                                ms_FormulaStack.Push(value);
                                break;
                            }
                        case EAttrFormulaType.eVal:
                        default:
                            ms_FormulaStack.Push(lambda.paramValue0);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogError($"CalcLambdaList error: {ex.Message}\n{ex.StackTrace}");
                    ms_FormulaStack.Push(0f);
                }
            }
            return ms_FormulaStack.Count > 0 ? ms_FormulaStack.Pop() : 0f;
        }
    }
}
