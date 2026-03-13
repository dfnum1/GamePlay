/********************************************************************
生成日期:	5:11:2020  20:36
类    名: 	ActorPartAgent
作    者:	HappLI
描    述:	单位部件逻辑
*********************************************************************/
using ExternEngine;
using Framework.AT.Runtime;
using Framework.Base;
using Framework.Core;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Playables;
namespace Framework.ActorSystem.Runtime
{
    public class ActorPartAgent : AActorAgent, IInstanceAbleCallback
    {
        class PartData : TypeObject
        {
            public InstanceAble pAble;
            public int          partId;

            public string strPartFile;
            public Vector3 vOffset;
            public Vector3 vRotation;
            public Vector3 vScale;
            public string strBindSlot;
            public int nBindFlags;

            public bool bKeepDead = false;
            public bool bDestroyed = false;
        }
        private int m_nGUID = 0;
        private Dictionary<int, PartData> m_vParts = null;
        private Dictionary<InstanceAble, int> m_vInstnaceParts = null;
        private HashSet<int> m_vDeleteQueue = null;
        //--------------------------------------------------------
        public int AddPart(string partFile, Vector3 offset, Vector3 offsetEulerAngle, Vector3 scale, string bindSlot, byte bindFlags = (byte)ESlotBindBit.All, bool bKeepDead = false)
        {
            if (m_vParts == null) m_vParts = new Dictionary<int, PartData>(8);

            if (m_nGUID >= int.MaxValue - 1) m_nGUID = 0;
            int partId = m_nGUID++;
            var partData = TypeInstancePool.Malloc<PartData>(GetFramework());
            partData.partId = partId;
            partData.bDestroyed = false;
            partData.strBindSlot = bindSlot;
            partData.nBindFlags = bindFlags;
            partData.vOffset = offset;
            partData.vRotation = offsetEulerAngle;
            partData.vScale = scale;
            partData.strPartFile = partFile;
            partData.bKeepDead = bKeepDead;
            m_vParts[partId] = partData;
            var op = GetFileSystem().SpawnInstance(partFile, OnSpawnInstance);
            if (op == null)
                return 0;
            op.SetUserData(0,partData);
            return partId;
        }
        //--------------------------------------------------------
        public void DeletePart(int partId)
        {
            AddDeleteQueue(partId);
        }
        //--------------------------------------------------------
        protected override void OnLoadActorGraphData(ActorGraphData component)
        {
        }
        //--------------------------------------------------------
        protected override void OnFlagDirty(EActorFlag flag, bool IsUsed)
        {
            if(flag == EActorFlag.Killed)
            {
                if(IsUsed)
                {
                    foreach (var db in m_vParts)
                    {
                        if (db.Value.bKeepDead) continue;
                        if (db.Value.bDestroyed) continue;
                        DeletePart(db.Key);
                    }
                }
            }
        }
        //--------------------------------------------------------
        protected override void OnUpdate(FFloat fDelta)
        {
            if(m_vParts!=null)
            {
                foreach (var db in m_vParts)
                {
                    if (db.Value.bDestroyed) continue;
                    if (db.Value.pAble == null) continue;

                    var worldMatrix = m_pActor.GetEventBindSlot(db.Value.strBindSlot);
                    if (ActorSystemUtil.HasBindSlot(db.Value.nBindFlags, ESlotBindBit.All))
                    {
                        db.Value.pAble.SetTransform(worldMatrix * Matrix4x4.TRS(db.Value.vOffset, Quaternion.Euler(db.Value.vRotation), db.Value.vScale));
                    }
                    else
                    {
                        if (ActorSystemUtil.HasBindSlot(db.Value.nBindFlags, ESlotBindBit.Position))
                        {
                            db.Value.pAble.SetPosition(worldMatrix.MultiplyPoint3x4(db.Value.vOffset));
                        }
                        if (ActorSystemUtil.HasBindSlot(db.Value.nBindFlags, ESlotBindBit.Rotation))
                        {
                            db.Value.pAble.SetRotation(Quaternion.Euler(db.Value.vRotation) * worldMatrix.rotation);
                        }
                        if (ActorSystemUtil.HasBindSlot(db.Value.nBindFlags, ESlotBindBit.Scale))
                        {
                            db.Value.pAble.SetScale(Vector3.Scale(db.Value.vScale, worldMatrix.lossyScale));
                        }
                    }
                }
            }
            if(m_vDeleteQueue!=null && m_vDeleteQueue.Count>0)
            {
                foreach (var db in m_vDeleteQueue)
                {
                    RealDeletePart(db);
                }
                m_vDeleteQueue.Clear();
            }

            base.OnUpdate(fDelta);
        }
        //--------------------------------------------------------
        void OnSpawnInstance(InstanceOperator op, bool check)
        {
            var partData = op.GetUserData<PartData>(0);
            if (check)
            {
                op.SetUsed(!partData.bDestroyed && m_pActor != null && !m_pActor.IsDestroy());
                return;
            }
            partData.pAble = op.GetInstanceAble();
            if (m_vInstnaceParts == null) m_vInstnaceParts = new Dictionary<InstanceAble, int>(8);
            if(partData.pAble!=null)
                m_vInstnaceParts[partData.pAble] = partData.partId;
        }
        //--------------------------------------------------------
        public void OnInstanceCallback(InstanceAble pAble, EInstanceCallbackType eType)
        {
            if(eType == EInstanceCallbackType.eDestroy)
            {
                if(m_vInstnaceParts!=null && m_vInstnaceParts.TryGetValue(pAble, out var partId))
                {
                    AddDeleteQueue(partId);
                }
            }
        }
        //--------------------------------------------------------
        void AddDeleteQueue(int partId)
        {
            if (m_vDeleteQueue == null) m_vDeleteQueue = new HashSet<int>(8);
            m_vDeleteQueue.Add(partId);
        }
        //--------------------------------------------------------
        void RealDeletePart(int partId)
        {
            if (m_vParts == null) return;
            if (!m_vParts.TryGetValue(partId, out var partData)) return;
            partData.bDestroyed = true;
            if (partData.pAble != null)
            {
                GetFileSystem().DeSpawnInstance(partData.pAble);
                partData.pAble = null;
            }
        }
        //--------------------------------------------------------
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}