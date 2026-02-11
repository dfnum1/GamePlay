/********************************************************************
生成日期:	06:30:2025
类    名: 	Octree
作    者:	HappLI
描    述:	八叉树(Octree)
            - 适用于3D游戏
            - 将空间递归划分为八个八分之一区域
            - 每个节点最多包含指定数量的Actor
*********************************************************************/
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if USE_FIXEDMATH
using ExternEngine;
#else
using FFloat = System.Single;
using FMatrix4x4 = UnityEngine.Matrix4x4;
using FQuaternion = UnityEngine.Quaternion;
using FVector2 = UnityEngine.Vector2;
using FVector3 = UnityEngine.Vector3;
using FBounds = UnityEngine.Bounds;
using FRay = UnityEngine.Ray;
#endif
namespace Framework.ActorSystem.Runtime
{
    public class Octree : ISpatialWorld
    {
        private readonly OctreeNode m_pRoot;
        private readonly int        m_nMaxDepth;
        private readonly int        m_nMaxActorsPerNode;
        private int                 m_nCount;
        //-----------------------------------------------------
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="worldBounds">世界边界</param>
        /// <param name="maxDepth">最大深度</param>
        /// <param name="maxActorsPerNode">每个节点最大Actor数量</param>
        public Octree(FBounds worldBounds, int maxDepth = 8, int maxActorsPerNode = 10)
        {
            m_pRoot = new OctreeNode(worldBounds, 0);
            m_nMaxDepth = maxDepth;
            m_nMaxActorsPerNode = maxActorsPerNode;
            m_nCount = 0;
        }
        //-----------------------------------------------------
        public void AddActor(Actor actor)
        {
            if (m_pRoot.Insert(actor, m_nMaxDepth, m_nMaxActorsPerNode))
            {
                m_nCount++;
            }
        }
        //-----------------------------------------------------
        public void RemoveActor(Actor actor)
        {
            if (m_pRoot.Remove(actor))
            {
                m_nCount--;
            }
        }
        //-----------------------------------------------------
        public void UpdateActor(Actor actor)
        {
            RemoveActor(actor);
            AddActor(actor);
        }
        //-----------------------------------------------------
        public void QueryActorsAtPosition(FVector3 position, FFloat radius, List<Actor> result, Actor pIngore = null)
        {
            result.Clear();
            FBounds queryBounds = new FBounds(new Vector3(position.x, position.y, position.z), new Vector3(radius * 2, radius * 2, radius * 2));
            m_pRoot.Query(queryBounds, result,pIngore);
        }
        //-----------------------------------------------------
        public void QueryActorsInBounds(WorldBoundBox boundBox, List<Actor> result, Actor pIngore = null)
        {
            result.Clear();
            FBounds queryBounds = new FBounds( boundBox.GetCenter(true),boundBox.GetSize());
            m_pRoot.Query(queryBounds, result, pIngore);
        }
        //-----------------------------------------------------
        public void QueryActorsByRay(FRay ray, FFloat maxDistance, List<Actor> result, Actor pIngore = null)
        {
            result.Clear();
            m_pRoot.Query(ray, maxDistance, result, pIngore);
        }
        //-----------------------------------------------------
        public void Clear()
        {
            m_pRoot.Clear();
            m_nCount = 0;
        }
        //-----------------------------------------------------
        public int Count => m_nCount;
        //-----------------------------------------------------
        public void Dispose()
        {
            Clear();
        }
        //-----------------------------------------------------
        public void DebugDraw(bool bGizmos)
        {
            m_pRoot.DebugDraw(bGizmos);
        }
        //-----------------------------------------------------
        //! OctreeNode
        //-----------------------------------------------------
        private class OctreeNode
        {
            private readonly FBounds m_Bounds;
            private readonly List<Actor>    m_vActors;
            private OctreeNode[]            m_arrChildren;
            private readonly int            m_nDepth;

            public OctreeNode(FBounds bounds, int depth)
            {
                m_Bounds = bounds;
                m_nDepth = depth;
                m_vActors = new List<Actor>(8);
                m_arrChildren = null;
            }
            //-----------------------------------------------------
            public bool Insert(Actor actor, int maxDepth, int maxActorsPerNode)
            {
                Vector3 actorPos = actor.GetPosition();
                if (!m_Bounds.Contains(new Vector3(actorPos.x, actorPos.y, actorPos.z)))
                {
                    return false;
                }

                if (m_arrChildren == null)
                {
                    if (m_vActors.Count < maxActorsPerNode || m_nDepth >= maxDepth)
                    {
                        m_vActors.Add(actor);
                        return true;
                    }

                    Split();
                }

                // 尝试插入到子节点
                for (int i = 0; i < 8; i++)
                {
                    if (m_arrChildren[i].Insert(actor, maxDepth, maxActorsPerNode))
                    {
                        return true;
                    }
                }

                // 如果无法插入到任何子节点，留在当前节点
                m_vActors.Add(actor);
                return true;
            }
            //-----------------------------------------------------
            public bool Remove(Actor actor)
            {
                // 先在当前节点查找
                int index = m_vActors.IndexOf(actor);
                if (index != -1)
                {
                    m_vActors.RemoveAt(index);
                    return true;
                }

                // 再在子节点查找
                if (m_arrChildren != null)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        if (m_arrChildren[i].Remove(actor))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
            //-----------------------------------------------------
            public void Query(FBounds queryBounds, List<Actor> result, Actor pIngore = null)
            {
                if (!m_Bounds.Intersects(queryBounds))
                {
                    return;
                }

                // 检查当前节点的Actor
                for (int i = 0; i < m_vActors.Count; i++)
                {
                    Actor actor = m_vActors[i];
                    Vector3 actorPos = actor.GetPosition();
                    if (queryBounds.Contains(new Vector3(actorPos.x, actorPos.y, actorPos.z)))
                    {
                        if(actor != pIngore)
                            result.Add(actor);
                    }
                }

                // 检查子节点
                if (m_arrChildren != null)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        m_arrChildren[i].Query(queryBounds, result, pIngore);
                    }
                }
            }
            //-----------------------------------------------------
            public void Query(FRay ray, float maxDistance, List<Actor> result, Actor pIngore = null)
            {
                FFloat distance;
                if (!m_Bounds.IntersectRay(ray, out distance) || distance > maxDistance)
                {
                    return;
                }

                // 检查当前节点的Actor
                for (int i = 0; i < m_vActors.Count; i++)
                {
                    Actor actor = m_vActors[i];
                    if (actor.GetBounds().RayHit(ray))
                    {
                        if(actor != pIngore)
                            result.Add(actor);
                    }
                }

                // 检查子节点
                if (m_arrChildren != null)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        m_arrChildren[i].Query(ray, maxDistance, result, pIngore);
                    }
                }
            }
            //-----------------------------------------------------
            public void Clear()
            {
                m_vActors.Clear();

                if (m_arrChildren != null)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        m_arrChildren[i].Clear();
                        m_arrChildren[i] = null;
                    }
                    m_arrChildren = null;
                }
            }
            //-----------------------------------------------------
            private void Split()
            {
                m_arrChildren = new OctreeNode[8];
                float halfWidth = m_Bounds.size.x * 0.5f;
                float halfHeight = m_Bounds.size.y * 0.5f;
                float halfDepth = m_Bounds.size.z * 0.5f;
                Vector3 center = m_Bounds.center;

                // 八个子节点
                m_arrChildren[0] = new OctreeNode(
                    new FBounds(center + new FVector3(-halfWidth * 0.5f, halfHeight * 0.5f, -halfDepth * 0.5f), new FVector3(halfWidth, halfHeight, halfDepth)),
                    m_nDepth + 1
                );

                m_arrChildren[1] = new OctreeNode(
                    new FBounds(center + new FVector3(halfWidth * 0.5f, halfHeight * 0.5f, -halfDepth * 0.5f), new FVector3(halfWidth, halfHeight, halfDepth)),
                    m_nDepth + 1
                );

                m_arrChildren[2] = new OctreeNode(
                    new FBounds(center + new FVector3(-halfWidth * 0.5f, -halfHeight * 0.5f, -halfDepth * 0.5f), new FVector3(halfWidth, halfHeight, halfDepth)),
                    m_nDepth + 1
                );

                m_arrChildren[3] = new OctreeNode(
                    new FBounds(center + new FVector3(halfWidth * 0.5f, -halfHeight * 0.5f, -halfDepth * 0.5f), new FVector3(halfWidth, halfHeight, halfDepth)),
                    m_nDepth + 1
                );

                m_arrChildren[4] = new OctreeNode(
                    new FBounds(center + new FVector3(-halfWidth * 0.5f, halfHeight * 0.5f, halfDepth * 0.5f), new FVector3(halfWidth, halfHeight, halfDepth)),
                    m_nDepth + 1
                );

                m_arrChildren[5] = new OctreeNode(
                    new FBounds(center + new FVector3(halfWidth * 0.5f, halfHeight * 0.5f, halfDepth * 0.5f), new FVector3(halfWidth, halfHeight, halfDepth)),
                    m_nDepth + 1
                );

                m_arrChildren[6] = new OctreeNode(
                    new FBounds(center + new FVector3(-halfWidth * 0.5f, -halfHeight * 0.5f, halfDepth * 0.5f), new FVector3(halfWidth, halfHeight, halfDepth)),
                    m_nDepth + 1
                );

                m_arrChildren[7] = new OctreeNode(
                    new FBounds(center + new FVector3(halfWidth * 0.5f, -halfHeight * 0.5f, halfDepth * 0.5f), new FVector3(halfWidth, halfHeight, halfDepth)),
                    m_nDepth + 1
                );

                // 将当前节点的Actor重新分配到子节点
                for (int i = m_vActors.Count - 1; i >= 0; i--)
                {
                    Actor actor = m_vActors[i];
                    bool inserted = false;

                    for (int j = 0; j < 8; j++)
                    {
                        if (m_arrChildren[j].Insert(actor, int.MaxValue, int.MaxValue))
                        {
                            inserted = true;
                            break;
                        }
                    }

                    if (inserted)
                    {
                        m_vActors.RemoveAt(i);
                    }
                }
            }
            //-----------------------------------------------------
            public void DebugDraw(bool bGizmos)
            {
#if UNITY_EDITOR
                // 绘制节点边界
                Color color = new Color(0, 1, 0, 0.5f);
                // Gizmos.color = color;
                //Gizmos.DrawWireCube(m_Bounds.center, m_Bounds.size);
                ActorSystemUtil.DrawBoundingBox(m_Bounds.center, m_Bounds.size, FMatrix4x4.identity, color, bGizmos);

                // 绘制节点中的Actor
                UnityEditor.Handles.Label(m_Bounds.center, m_vActors.Count.ToString ());
                //Gizmos.color = Color.blue;
                //for (int i = 0; i < m_vActors.Count; i++)
                //{
                //    Vector3 actorPos = m_vActors[i].GetPosition();
                //    Gizmos.DrawSphere(new Vector3(actorPos.x, actorPos.y, actorPos.z), 0.2f);
                //}

                // 递归绘制子节点
                if (m_arrChildren != null)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        m_arrChildren[i].DebugDraw(bGizmos);
                    }
                }
#endif
            }
        }
    }
}
