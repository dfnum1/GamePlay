/********************************************************************
生成日期:	5:11:2020  20:36
类    名: 	ActorComponent
作    者:	HappLI
描    述:	
*********************************************************************/
using System.Collections.Generic;
using UnityEngine;

namespace Framework.ActorSystem.Runtime
{
    public class ActorComponent : MonoBehaviour, IContextData
    {
        public struct Slot
        {
            public string name;
            public Transform slot;
            public Vector3 offset;
        }
        public Slot[] slots;
        [HideInInspector] public List<ActorCommonAction> commonActions = new List<ActorCommonAction>(2);
        [DrawProps.Disable] public List<ActorAvatarMask> avatarMasks = new List<ActorAvatarMask>(2);
        public TextAsset ActionGraphData = null;
        public AT.Runtime.AgentTreeData ATData;

        private Transform m_pTransform;
#if UNITY_EDITOR
        private GameObject m_pBindPrefab = null;
        internal void SetBindPrefab(GameObject prefab)
        {
            m_pBindPrefab = prefab;
        }
        //-----------------------------------------------------
        internal GameObject GetBindPrefab()
        {
            return m_pBindPrefab;
        }
#endif
        //-----------------------------------------------------
        public Transform GetTransform()
        {
            if (m_pTransform == null) m_pTransform = transform;
            return m_pTransform;
        }
        //-----------------------------------------------------
        public Transform GetSlot(string name, out Vector3 offset)
        {
            offset = Vector3.zero;
            if (slots == null) return null;
            for(int i =0; i < slots.Length; ++i)
            {
                if (slots[i].name.CompareTo(name) == 0)
                {
                    offset = slots[i].offset;
                    return slots[i].slot;
                }
            }
            return null;
        }
    }
}