/********************************************************************
生成日期:	5:11:2020  20:36
类    名: 	LockTargetUtil
作    者:	HappLI
描    述:	索敌工具
*********************************************************************/
using Framework.Base;
using Framework.AT.Runtime;
using UnityEngine;

namespace Framework.ActorSystem.Runtime
{
    //------------------------------------------------------
    [ATInteralExport("Actor系统/索敌工具", -21)]
    public class LockTargetUtil
    {
        //------------------------------------------------------
        [ATMethod("根据Actor类型索敌"), ATArgvDrawer("actorType", BaseATDrawerKey.Key_ActorTypeDraw)]
        public static void LockAttackTarget(AActorStateInfo pActorState, byte actorType, bool bClear = true, bool bFriend = false)
        {
            if (pActorState == null || pActorState.GetOwner()==null)
                return;
            var vLocks = pActorState.GetLockTargets(true);
            if (bClear) pActorState.ClearLockTargets();
            var actors = pActorState.GetOwner().GetActorManager().GetActors();
            foreach(var db in actors)
            {
                if (db.Value.GetActorType() != actorType)
                    continue;

                if(bFriend)
                {
                    if (!(pActorState.GetOwner().CanAttackGroup(db.Value.GetAttackGroup())))
                    {
                        vLocks.Add(db.Value);
                    }
                }
                else
                {
                    if ((pActorState.GetOwner().CanAttackGroup(db.Value.GetAttackGroup())))
                    {
                        vLocks.Add(db.Value);
                    }
                }

            }
        }
        //------------------------------------------------------
        [ATMethod("根据Actor类型-子类型索敌"), ATArgvDrawer("actorType", BaseATDrawerKey.Key_ActorTypeDraw), ATArgvDrawer("subType", BaseATDrawerKey.Key_ActorSubTypeDraw)]
        public static void LockAttackTarget(AActorStateInfo pActorState, byte actorType, byte subType, bool bClear = true, bool bFriend = false)
        {
            if (pActorState == null || pActorState.GetOwner() == null)
                return;
            var vLocks = pActorState.GetLockTargets(true);
            if (bClear) pActorState.ClearLockTargets();
            var actors = pActorState.GetOwner().GetActorManager().GetActors();
            foreach (var db in actors)
            {
                if (db.Value.GetActorType() != actorType || db.Value.GetActorSubType() != subType)
                    continue;
                if (bFriend)
                {
                    if (!(pActorState.GetOwner().CanAttackGroup(db.Value.GetAttackGroup())))
                    {
                        vLocks.Add(db.Value);
                    }
                }
                else
                {
                    if ((pActorState.GetOwner().CanAttackGroup(db.Value.GetAttackGroup())))
                    {
                        vLocks.Add(db.Value);
                    }
                }
            }
        }
        //------------------------------------------------------
        [ATMethod("根据属性排序"), ATArgvDrawer("attrType", BaseATDrawerKey.Key_DrawAttributePop)]
        public static void SortByAttr(AActorStateInfo pActorState, byte attrType, bool bUpper)
        {
            if (pActorState == null)
                return;
            var vLocks = pActorState.GetLockTargets();
            if (vLocks == null) return;
            if (bUpper)  SortUtility.QuickSort<Actor>(ref vLocks, SortAttrUpper, attrType);
            else SortUtility.QuickSort<Actor>(ref vLocks, SortAttrDown, attrType);
        }
        //------------------------------------------------------
        [ATMethod("根据属性比排序"), ATArgvDrawer("baseType", BaseATDrawerKey.Key_DrawAttributePop), ATArgvDrawer("totalType", BaseATDrawerKey.Key_DrawAttributePop)]
        public static void SortByDistance(AActorStateInfo pActorState, byte baseType, byte totalType, bool bUpper)
        {
            if (pActorState == null)
                return;
            var vLocks = pActorState.GetLockTargets();
            if (vLocks == null) return;
            Value2Var attrRate = new Value2Var();
            attrRate.intVal0 = baseType;
            attrRate.intVal1 = totalType;
            if (bUpper) SortUtility.QuickSort<Actor>(ref vLocks, SortAttrRateUpper, attrRate);
            else SortUtility.QuickSort<Actor>(ref vLocks, SortAttrRateDown, attrRate);
        }
        //------------------------------------------------------
        [ATMethod("根据距离排序")]
        public static void SortByDistance(AActorStateInfo pActorState, Actor pActor, bool bUpper)
        {
            if (pActorState == null)
                return;
            var vLocks = pActorState.GetLockTargets();
            if (vLocks == null) return;
            pActor.GetPosition();
            if (bUpper) SortUtility.QuickSort<Actor>(ref vLocks, SortDistanceUpper, pActor);
            else SortUtility.QuickSort<Actor>(ref vLocks, SortDistanceDown, pActor);
        }
        //------------------------------------------------------
        [ATMethod("保留锁定目标个数")]
        public static void RetainLockCount(AActorStateInfo pActorState, int nRetainCnt)
        {
            if (pActorState == null)
                return;
            var vLocks = pActorState.GetLockTargets();
            if (vLocks == null) return;
            while (nRetainCnt < vLocks.Count)
            {
                vLocks.RemoveAt(vLocks.Count - 1);
            }
        }
        //------------------------------------------------------
        static int SortAttrUpper(int attrType, Actor left, Actor right)
        {
            var leftVal = left.GetAttr((byte)attrType);
            var rightVal = right.GetAttr((byte)attrType);
            if (leftVal < rightVal) return -1;
            else if (leftVal > rightVal) return 1;
            return 0;
        }
        //------------------------------------------------------
        static int SortAttrRateDown(IUserData pData, Actor left, Actor right)
        {
            if (pData == null) return 0;
            Value2Var value2 = (Value2Var)pData;
            var leftVal = left.GetAttr((byte)value2.intVal0)/ Mathf.Max(0.01f, left.GetAttr((byte)value2.intVal1,1));
            var rightVal = right.GetAttr((byte)value2.intVal0) / Mathf.Max(0.01f, right.GetAttr((byte)value2.intVal1, 1));
            if (leftVal < rightVal) return 1;
            else if (leftVal > rightVal) return -1;
            return 0;
        }
        //------------------------------------------------------
        static int SortAttrRateUpper(IUserData pData, Actor left, Actor right)
        {
            if (pData == null) return 0;
            Value2Var value2 = (Value2Var)pData;
            var leftVal = left.GetAttr((byte)value2.intVal0) / Mathf.Max(0.01f, left.GetAttr((byte)value2.intVal1, 1));
            var rightVal = right.GetAttr((byte)value2.intVal0) / Mathf.Max(0.01f, right.GetAttr((byte)value2.intVal1, 1));
            if (leftVal < rightVal) return -1;
            else if (leftVal > rightVal) return 1;
            return 0;
        }
        //------------------------------------------------------
        static int SortAttrDown(int attrType, Actor left, Actor right)
        {
            var leftVal = left.GetAttr((byte)attrType);
            var rightVal = right.GetAttr((byte)attrType);
            if (leftVal < rightVal) return 1;
            else if (leftVal > rightVal) return -1;
            return 0;
        }
        //------------------------------------------------------
        static int SortDistanceUpper(IUserData pUser, Actor left, Actor right)
        {
            if (pUser == null) return 0;
            Actor pCur = (Actor)pUser;
            var leftVal = (left.GetPosition() - pCur.GetPosition()).sqrMagnitude;
            var rightVal = (right.GetPosition() - pCur.GetPosition()).sqrMagnitude;
            if (leftVal < rightVal) return -1;
            else if (leftVal > rightVal) return 1;
            return 0;
        }
        //------------------------------------------------------
        static int SortDistanceDown(IUserData pUser, Actor left, Actor right)
        {
            if (pUser == null) return 0;
            Actor pCur = (Actor)pUser;
            var leftVal = (left.GetPosition() - pCur.GetPosition()).sqrMagnitude;
            var rightVal = (right.GetPosition() - pCur.GetPosition()).sqrMagnitude;
            if (leftVal < rightVal) return 1;
            else if (leftVal > rightVal) return -1;
            return 0;
        }
    }
}