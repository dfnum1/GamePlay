/********************************************************************
生成日期:	01:20:2026
类    名: 	SpriteSequence
作    者:	HappLI
描    述:	常规的基于Sprite的序列帧动画播放组件
*********************************************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.U2D;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.U2D;
#endif

namespace Framework.SpriteSeq
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteSequence : ASpriteSequence
    {
    }
    //----------------------------------------------
    //! Editor
    //----------------------------------------------
#if UNITY_EDITOR
    [CustomEditor(typeof(SpriteSequence))]
    public class SpriteSequenceInspectorEditor : ASpriteSequenceInspectorEditor
    {
    }
#endif
}
