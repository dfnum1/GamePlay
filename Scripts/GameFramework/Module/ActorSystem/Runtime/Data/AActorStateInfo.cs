/********************************************************************
生成日期:	5:11:2020  20:36
类    名: 	AActorStateInfo
作    者:	HappLI
描    述:	
*********************************************************************/
using Framework.AT.Runtime;
using System.Collections.Generic;

namespace Framework.ActorSystem.Runtime
{
    public enum EActorStateStatus
    {
        eBegin,
        eTicking,
        eEnd,
    }
    [ATInteralExport("Actor系统/Actor状态对象", -22, "ActorSystem/actor_state")]
    public abstract class AActorStateInfo : TypeActor
    {
        public abstract Actor GetOwner();
        public virtual uint GetDamageID() { return 0; }
        [ATMethod("获取属性计算公式")]
        public virtual int GetAttrFormulaType() { return 0; }
        [ATMethod("添加索敌目标")]
        public abstract void AddLockTarget(Actor pNode, bool bClear = false);
        [ATMethod("清空索敌目标")]
        public abstract void ClearLockTargets();
        public abstract List<Actor> GetLockTargets(bool isNullCreate = true);
    }
}
