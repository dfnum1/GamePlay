/********************************************************************
生成日期:	5:11:2020  20:36
类    名: 	ActorAttrDatas
作    者:	HappLI
描    述:	Actor 属性配置器
*********************************************************************/
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections.Generic;
using System;

namespace Framework.ActorSystem.Runtime
{
    //-----------------------------------------------------
    //! Formula Type
    //-----------------------------------------------------
    [DrawProps.UnFilter]
    public enum EAttrFormulaType : byte
    {
        [DrawProps.Disable] eNone,
        [DrawProps.Display("值")]eVal,
        [DrawProps.Display("加")] eAdd,   //两个数相加
        [DrawProps.Display("减")] eSub,   //两个数相减
        [DrawProps.Display("乘")] eMul,   //两个数相乘
        [DrawProps.Display("除")] eDiv,   //两个数相除
        [DrawProps.Display("幂")]ePower, //幂运算   参数1-底数，参数2-指数
        [DrawProps.Display("最小")]eMin,   //取最小值  参数1-最小值，参数2-最大值
        [DrawProps.Display("最大")]eMax,   //取最大值  参数1-最小值，参数2-最大值
        [DrawProps.Display("下取整")]eFloor, //向下取整
        [DrawProps.Display("上取整")]eCeil,  //向上取整
        [DrawProps.Display("取绝对")]eAbs,   //取绝对值
        [DrawProps.Display("随机")]eRandom,    //随机数   参数1-最小值，参数2-最大值
        [DrawProps.Display("括号运算")]eBracket,   //括号运算
        [DrawProps.Display("属性")] eActorAttr, //取属性值,参数1-敌/我，参数2-属性类型
    }
    //-----------------------------------------------------
    //! ActorAttrDatas
    //-----------------------------------------------------
    [System.Serializable]
    public abstract class AActorAttrDatas : ScriptableObject
    {
        [System.Serializable]
        public struct AttrInfo
        {
            public byte attr;
            public string name;
            public string desc;
        }
        [System.Serializable]
        public class AttrFormula
        {
            [System.Serializable]
            public class LambdaParam
            {
                public EAttrFormulaType type;
                public float paramValue0;
                public float paramValue1;
                public bool isUnary; // 新增：是否一元操作
#if UNITY_EDITOR
                internal bool bExpand;
#endif
                public List<LambdaParam> subLambda;
            }
            public byte applayAttr;
            public string name;
            public List<LambdaParam> vLambda;
#if UNITY_EDITOR
            public string inputLambda;
#endif
        }
        public AttrInfo[] vAttributes;
        public AttrFormula[] vFormulas;
    }
#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(AActorAttrDatas))]
    public class AActorAttrDatasEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if(GUILayout.Button("编辑"))
            {
                Editor.AttrFormulaEditorWindow.Open(target as AActorAttrDatas);
            }
        }
        //-----------------------------------------------------
        [UnityEditor.Callbacks.OnOpenAsset(0)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            var obj = EditorUtility.InstanceIDToObject(instanceID);
            if (obj != null && obj is AActorAttrDatas)
            {
                Editor.AttrFormulaEditorWindow.Open(obj as AActorAttrDatas);
                return true;
            }
            return false;
        }
    }
#endif
}
