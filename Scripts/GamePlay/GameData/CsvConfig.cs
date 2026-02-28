/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	ACsvConfig
作    者:	HappLI
描    述:	
*********************************************************************/
using Framework.ED;
using UnityEditor;
using UnityEngine;

namespace Framework.Data
{
    [CreateAssetMenu(menuName = "GamePlay/配置表数据")]
    public class CsvConfig : ACsvConfig
    {
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(CsvConfig))]
    public class CsvConfigEditor : ACsvConfigEditor
    {

    }
#endif
}
