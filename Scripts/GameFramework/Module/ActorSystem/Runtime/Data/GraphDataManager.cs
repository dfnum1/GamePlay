/********************************************************************
生成日期:	5:11:2020  20:36
类    名: 	GraphDataManager
作    者:	HappLI
描    述:   配置数据管理器
*********************************************************************/
using Framework.DrawProps;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Framework.ActorSystem.Runtime
{
    internal class GraphDataManager
    {
        static Dictionary<string, IContextData> ms_vCustomDatas = null;
        //-------------------------------------------
        public static T GetCustomData<T>(string strFile) where T : IContextData
        {
            if (string.IsNullOrEmpty(strFile)) return default;
            if (ms_vCustomDatas != null)
            {
                IContextData getData = null;
                if (ms_vCustomDatas.TryGetValue(strFile, out getData))
                    return (T)getData;
            }
            return default;
        }
        //-------------------------------------------
        public static void AddCustomData(string strFile, IContextData userData)
        {
            if (string.IsNullOrEmpty(strFile) || userData == null) return;
            if (ms_vCustomDatas == null) ms_vCustomDatas = new Dictionary<string, IContextData>(64);
            ms_vCustomDatas[strFile] = userData;
        }
        //-------------------------------------------
        public static void UnloadCustom(string strFile)
        {
            if (string.IsNullOrEmpty(strFile)) return;
            if (ms_vCustomDatas != null) ms_vCustomDatas.Remove(strFile);
        }
    }
}
