/********************************************************************
生成日期:	5:11:2020  20:36
类    名: 	ActorAttrDatas
作    者:	HappLI
描    述:	
*********************************************************************/
using System.Collections.Generic;
using UnityEngine;
namespace Framework.ActorSystem.Runtime
{
    [CreateAssetMenu(menuName = "GamePlay/AttrDatas")]
    public class ActorAttrDatas : AActorAttrDatas
    {
    }
#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(ActorAttrDatas))]
    class ActorAttrDatasEditor : AActorAttrDatasEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
#endif
}