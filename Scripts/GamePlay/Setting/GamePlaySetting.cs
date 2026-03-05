/********************************************************************
生成日期:	3:10:2019  15:03
类    名: 	AFrameworkSetting
作    者:	HappLI
描    述:	框架设置
*********************************************************************/
#if UNITY_EDITOR
using UnityEditor;
using Framework.DrawProps;

#endif

#if USE_FIXEDMATH
using ExternEngine;
#else
using FFloat = System.Single;
using FMatrix4x4 = UnityEngine.Matrix4x4;
using FQuaternion = UnityEngine.Quaternion;
using FVector2 = UnityEngine.Vector2;
using FVector3 = UnityEngine.Vector3;
using FBounds = UnityEngine.Bounds;
using FRay = UnityEngine.Ray;
#endif

namespace Framework.Core
{
    [System.Serializable]
    public class GamePlaySettingData : AFrameworkSettingData
    {
    }
    //-----------------------------------------------------
    //! GamePlaySetting 
    //-----------------------------------------------------
    public class GamePlaySetting : AFrameworkSetting
    {
        [UnHeader]
        public GamePlaySettingData settingData = new GamePlaySettingData();

        public override AFrameworkSettingData GetSetting()
        {
            return settingData;
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(GamePlaySetting))]
    public class GamePlaySettingEditor : AFrameworkSettingEditor
    {
        public override void OnInspectorGUI()
        {
            GamePlaySetting setting = target as GamePlaySetting;
            GamePlaySettingData settingData = setting.settingData;
            base.OnInspectorGUI();
        }
    }
#endif
}