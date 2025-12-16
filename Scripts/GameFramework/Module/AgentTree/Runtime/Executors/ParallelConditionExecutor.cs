/********************************************************************
生成日期:	07:03:2025
类    名: 	ParallelConditionExecutor
作    者:	HappLI
描    述:	并行条件节点执行
*********************************************************************/
namespace Framework.AT.Runtime
{
    internal class ParallelConditionExecutor
    {
        public static bool OnExecutor(AgentTree pAgent, BaseNode pNode)
        {
            ParallelCondition pCondition = pNode as ParallelCondition;

            int portCnt = pNode.GetInportCount();
            if (portCnt <= 0) return false;
            if (portCnt % 2 != 0 || pCondition.opTypes == null)
            {
                UnityEngine.Debug.LogError("ParallelCondition The number of ports is not a multiple of 2.");
                return false;
            }
            if (pCondition.opTypes.Length*2 == portCnt)
                return false;


            int index = 0;
            var ports = pNode.GetInports();
            for (int i =0; i < portCnt; i+=2)
            {
                var portType0 = pAgent.GetInportVarType(pNode, i);
                var portType1 = pAgent.GetInportVarType(pNode, i+1);
                if(portType0 != portType1)
                {
                    UnityEngine.Debug.LogError("ParallelCondition condition[" + i+1 + "] var type is not equal");
                    return false;
                }
                var opType = pCondition.opTypes[index];

                if(!ConditionExecutor.OnExecute(pAgent, pNode, i, i+1, opType))
                {
                    return false;
                }

                index++;
            }

            return true;
        }
    }
}