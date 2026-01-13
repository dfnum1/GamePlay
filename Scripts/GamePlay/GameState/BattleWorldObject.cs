/********************************************************************
生成日期:	11:03:2023
类    名: 	CutsceneObject
作    者:	HappLI
描    述:	过场unity 存储对象
*********************************************************************/
using Framework.State.Runtime;
using UnityEngine;

namespace Framework.Cutscene.Runtime
{
    [CreateAssetMenu(menuName = "GamePlay/游戏世界")]
    public class BattleWorldObject : ABattleWorldObject
    {
    }
#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(BattleWorldObject))]
    public class BattleWorldObjectEditor : ABattleWorldObjectEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
#endif
}