/********************************************************************
生成日期:	5:11:2020  20:36
类    名: 	AActorStateInfo
作    者:	HappLI
描    述:	
*********************************************************************/
using System.Collections.Generic;

namespace Framework.ActorSystem.Runtime
{
    public enum EActorStateStatus
    {
        eBegin,
        eTicking,
        eEnd,
    }
    public abstract class AActorStateInfo : TypeActor
    {
        public virtual uint GetDamageID() { return 0; }
        public abstract void AddLockTarget(Actor pNode, bool bClear = false);
        public abstract void ClearLockTargets();
        public abstract List<Actor> GetLockTargets(bool isEmptyReLock = true);
    }
}
