/********************************************************************
生成日期:	06:30:2025
类    名: 	GroupNode
作    者:	HappLI
描    述:	组节点
*********************************************************************/

using UnityEngine;

namespace Framework.AT.Runtime
{
    //-----------------------------------------------------
    [System.Serializable]
    public class GroupNode : BaseNode
    {
#if UNITY_EDITOR
        [SerializeField] internal short[] childGuids;
        [SerializeField] internal int width;
        [SerializeField] internal int height;
#endif
        public override bool IsTask() { return false; }
#if UNITY_EDITOR
        internal override NodePort[] GetInports(bool bAutoNew = false, int cnt = 0)
#else
        internal override NodePort[] GetInports()
#endif
        {
            return null;
        }

#if UNITY_EDITOR
        internal override NodePort[] GetOutports(bool bAutoNew = false, int cnt = 0)
#else
        internal override NodePort[] GetOutports()
#endif
        {
            return null;
        }
    }
}