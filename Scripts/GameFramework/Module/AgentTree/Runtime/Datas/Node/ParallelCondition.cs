/********************************************************************
生成日期:	06:30:2025
类    名: 	ParallelCondition
作    者:	HappLI
描    述:	并行条件节点
*********************************************************************/
using UnityEngine;

namespace Framework.AT.Runtime
{
    //-----------------------------------------------------
    [System.Serializable, ATNode("条件并行器", "AT/at_parallel_condition")]
    public class ParallelCondition : BaseNode
    {
        public ECompareOpType[] opTypes;
        [SerializeField] internal NodePort[] argvs;
        public override bool IsTask() { return true; }
#if UNITY_EDITOR
        internal override NodePort[] GetInports(bool bAutoNew = false, int cnt = 0)
#else
        internal override NodePort[] GetInports()
#endif
        {
#if UNITY_EDITOR
            if (bAutoNew)
            {
                argvs = CreatePorts(argvs, cnt);
            }
#endif
            return argvs;
        }

#if UNITY_EDITOR
        internal override NodePort[] GetOutports(bool bAutoNew = false, int cnt = 0)
#else
        internal override NodePort[] GetOutports()
#endif
        {
            return null;
        }
        internal override void Init(AgentTreeData pTree)
        {
            base.Init(pTree);
            if (argvs != null)
            {
                for (int i = 0; i < argvs.Length; ++i)
                {
                    argvs[i].pVariable = pTree.GetVariable(argvs[i].varGuid);
                    pTree.SetVarOwner(argvs[i].varGuid, this);
                }
            }
        }
    }
}