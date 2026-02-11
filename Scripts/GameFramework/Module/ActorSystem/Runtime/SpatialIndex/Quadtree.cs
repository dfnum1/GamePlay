/********************************************************************
生成日期:	06:30:2025
类    名: 	Quadtree
作    者:	HappLI
描    述:	四叉树(Quadtree)
            - 适用于2D游戏或3D游戏中的平面查询
            - 将空间递归划分为四个象限
            - 每个节点最多包含指定数量的Actor
*********************************************************************/
using System.Collections.Generic;
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
    /// <summary>
    /// 四叉树实现
    /// </summary>
    public class Quadtree : ISpatialWorld
    {
        private readonly QuadtreeNode   m_pRoot;
        private readonly int            m_nMaxDepth;
        private readonly int            m_nMaxActorsPerNode;
        private int                     m_nCount;

        public Quadtree(FBounds worldBounds, int maxDepth = 8, int maxActorsPerNode = 10)
        {
            m_pRoot = new QuadtreeNode(worldBounds, 0);
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
            FBounds queryBounds = new FBounds(new FVector3(position.x, 0, position.z), new FVector3(radius * 2, 0, radius * 2));
            m_pRoot.Query(queryBounds, result, pIngore);
        }
        //-----------------------------------------------------
        public void QueryActorsInBounds(WorldBoundBox boundBox, List<Actor> result, Actor pIngore = null)
        {
            result.Clear();
            FVector3 center = boundBox.GetCenter(true);
            FVector3 size = boundBox.GetSize();
            FBounds queryBounds = new FBounds(
                new Vector3(center.x, 0, center.z),
                new Vector3(size.x, 1.0f, size.z)
            );
            m_pRoot.Query(queryBounds, result, pIngore);
        }
        //-----------------------------------------------------
        public void QueryActorsByRay(FRay ray, FFloat maxDistance, List<Actor> result, Actor pIngore = null)
        {
            result.Clear();
            // 四叉树只处理2D，忽略Z轴
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
        // 四叉树节点类
        //-----------------------------------------------------
        private class QuadtreeNode
        {
            private readonly FBounds         m_Bounds;
            private readonly List<Actor>    m_vActors;
            private QuadtreeNode[]          m_arrChildren;
            private readonly int            m_nDepth;

            public QuadtreeNode(FBounds bounds, int depth)
            {
                m_Bounds.SetMinMax(bounds.min, bounds.max+Vector3.up*2);
                m_nDepth = depth;
                m_vActors = new List<Actor>(8);
                m_arrChildren = null;
            }
            //-----------------------------------------------------
            public bool Insert(Actor actor, int maxDepth, int maxActorsPerNode)
            {
                FVector3 actorPos = actor.GetPosition();
                if (!m_Bounds.Contains(new FVector3(actorPos.x, 0.1f, actorPos.z)))
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

                    // 分裂节点
                    Split();
                }

                // 尝试插入到子节点
                for (int i = 0; i < 4; i++)
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
                    for (int i = 0; i < 4; i++)
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
                    FVector3 actorPos = actor.GetPosition();
                    if (queryBounds.Contains(new Vector3(actorPos.x, 0.1f, actorPos.z)))
                    {
                        if(actor!= pIngore)
                            result.Add(actor);
                    }
                }

                // 检查子节点
                if (m_arrChildren != null)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        m_arrChildren[i].Query(queryBounds, result, pIngore);
                    }
                }
            }
            //-----------------------------------------------------
            public void Query(FRay ray, FFloat maxDistance, List<Actor> result, Actor pIngore = null)
            {
                if (!m_Bounds.IntersectRay(ray, out FFloat distance))
                {
                    return;
                }

                if (distance > maxDistance)
                {
                    return;
                }

                // 检查当前节点的Actor
                for (int i = 0; i < m_vActors.Count; i++)
                {
                    Actor actor = m_vActors[i];
                    FBounds bound = new FBounds(new Vector3(actor.GetPosition().x, -0.1f, actor.GetPosition().z),
                        new Vector3(1, 2, 1));
                    if (bound.IntersectRay(ray, out FFloat actorDistance) && actorDistance <= maxDistance)
                    {
                        if(actor!= pIngore)
                            result.Add(actor);
                    }
                }

                // 检查子节点
                if (m_arrChildren != null)
                {
                    for (int i = 0; i < 4; i++)
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
                    for (int i = 0; i < 4; i++)
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
                m_arrChildren = new QuadtreeNode[4];
                float halfWidth = m_Bounds.size.x * 0.5f;
                float halfHeight = m_Bounds.size.z * 0.5f;
                FVector3 center = m_Bounds.center;

                // 四个子节点：西北、东北、西南、东南
                m_arrChildren[0] = new QuadtreeNode(
                    new FBounds(center + new FVector3(-halfWidth * 0.5f, 0, halfHeight * 0.5f), new FVector3(halfWidth, 0, halfHeight)),
                    m_nDepth + 1
                );

                m_arrChildren[1] = new QuadtreeNode(
                    new FBounds(center + new FVector3(halfWidth * 0.5f, 0, halfHeight * 0.5f), new FVector3(halfWidth, 0, halfHeight)),
                    m_nDepth + 1
                );

                m_arrChildren[2] = new QuadtreeNode(
                    new FBounds(center + new FVector3(-halfWidth * 0.5f, 0, -halfHeight * 0.5f), new FVector3(halfWidth, 0, halfHeight)),
                    m_nDepth + 1
                );

                m_arrChildren[3] = new QuadtreeNode(
                    new FBounds(center + new FVector3(halfWidth * 0.5f, 0, -halfHeight * 0.5f), new FVector3(halfWidth, 0, halfHeight)),
                    m_nDepth + 1
                );

                // 将当前节点的Actor重新分配到子节点
                for (int i = m_vActors.Count - 1; i >= 0; i--)
                {
                    Actor actor = m_vActors[i];
                    bool inserted = false;

                    for (int j = 0; j < 4; j++)
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
                Color color = new Color(1, 0, 0, 0.5f);
              //  Gizmos.color = color;
              //  Gizmos.DrawWireCube(m_Bounds.center, m_Bounds.size);
                ActorSystemUtil.DrawBoundingBox(m_Bounds.center, m_Bounds.size, Matrix4x4.identity, color, bGizmos);

                // 绘制节点中的Actor
                Gizmos.color = Color.green;
                UnityEditor.Handles.Label(m_Bounds.center, m_vActors.Count.ToString());
                // for (int i = 0; i < m_vActors.Count; i++)
                // {
                //  Vector3 actorPos = m_vActors[i].GetPosition();
                //Gizmos.DrawSphere(new Vector3(actorPos.x, 0, actorPos.z), 0.2f);
                // }

                // 递归绘制子节点
                if (m_arrChildren != null)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        m_arrChildren[i].DebugDraw(bGizmos);
                    }
                }
#endif
            }
        }
    }
}
