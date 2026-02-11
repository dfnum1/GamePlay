/********************************************************************
生成日期:	06:30:2025
类    名: 	KDTree
作    者:	HappLI
描    述:	KD树(KDTree)
            - 适用于高维空间查询
            - 交替沿不同坐标轴划分空间
            - 支持快速的最近邻查询
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
    public class KDTree : ISpatialWorld
    {
        private struct KDTreeNode
        {
            public FVector3 Position;
            public Actor Actor;
            public int Axis;
            public int LeftChild;
            public int RightChild;
        }
        //--------------------------------------------------------
        private const int MAX_DIMENSIONS = 3;
        private const int INITIAL_CAPACITY = 1024;

        private KDTreeNode[]    m_arrNodes;
        private int             m_nNodeCount;
        private Actor[]         m_arrActors;
        private int             m_nActorCount;
        private bool            m_isBuilt;
        public KDTree()
        {
            m_arrNodes = new KDTreeNode[INITIAL_CAPACITY];
            m_arrActors = new Actor[INITIAL_CAPACITY];
            m_nNodeCount = 0;
            m_nActorCount = 0;
            m_isBuilt = false;
        }
        //--------------------------------------------------------
        public void AddActor(Actor actor)
        {
            if (m_nActorCount >= m_arrActors.Length)
            {
                ResizeArrays(m_arrActors.Length * 2);
            }

            m_arrActors[m_nActorCount++] = actor;
            m_isBuilt = false;
        }
        //--------------------------------------------------------
        public void RemoveActor(Actor actor)
        {
            for (int i = 0; i < m_nActorCount; i++)
            {
                if (m_arrActors[i] == actor)
                {
                    m_arrActors[i] = m_arrActors[--m_nActorCount];
                    m_arrActors[m_nActorCount] = null;
                    m_isBuilt = false;
                    break;
                }
            }
        }
        //--------------------------------------------------------
        public void UpdateActor(Actor actor)
        {
            // For KD-Tree, we need to rebuild the tree on updates
            m_isBuilt = false;
        }
        //--------------------------------------------------------
        public void QueryActorsAtPosition(FVector3 position, FFloat radius, List<Actor> result, Actor pIngore = null)
        {
            result.Clear();
            BuildTreeIfNeeded();

            if (m_nNodeCount == 0)
            {
                return;
            }

            Vector3 queryPos = new Vector3(position.x, position.y, position.z);
            float radiusSquared = radius * radius;
            QueryNode(0, queryPos, radiusSquared, result,pIngore);
        }
        //--------------------------------------------------------
        public void QueryActorsInBounds(WorldBoundBox boundBox, List<Actor> result, Actor pIngore = null)
        {
            result.Clear();
            BuildTreeIfNeeded();

            if (m_nNodeCount == 0)
            {
                return;
            }

            Bounds queryBounds = new Bounds(boundBox.GetCenter(true), boundBox.GetSize());

            QueryNodeBounds(0, queryBounds, result);
        }
        //--------------------------------------------------------
        public void QueryActorsByRay(FRay ray, FFloat maxDistance, List<Actor> result, Actor pIngore = null)
        {
            result.Clear();
            BuildTreeIfNeeded();

            if (m_nNodeCount == 0)
            {
                return;
            }

            QueryNodeRay(0, ray, maxDistance, result);
        }
        //--------------------------------------------------------
        public void Clear()
        {
            for (int i = 0; i < m_nNodeCount; i++)
            {
                m_arrNodes[i] = default(KDTreeNode);
            }

            for (int i = 0; i < m_nActorCount; i++)
            {
                m_arrActors[i] = null;
            }

            m_nNodeCount = 0;
            m_nActorCount = 0;
            m_isBuilt = false;
        }
        //--------------------------------------------------------
        public int Count => m_nActorCount;
        //--------------------------------------------------------
        public void Dispose()
        {
            Clear();
            m_arrNodes = null;
            m_arrActors = null;
        }
        //--------------------------------------------------------
        private void BuildTreeIfNeeded()
        {
            if (!m_isBuilt && m_nActorCount > 0)
            {
                BuildTree();
            }
        }
        //--------------------------------------------------------
        private void BuildTree()
        {
            if (m_nActorCount == 0)
            {
                return;
            }

            m_nNodeCount = 0;
            BuildNode(0, m_nActorCount, 0);
            m_isBuilt = true;
        }
        //--------------------------------------------------------
        private int BuildNode(int start, int end, int depth)
        {
            if (start >= end)
            {
                return -1;
            }

            int axis = depth % MAX_DIMENSIONS;
            int median = Partition(start, end, axis);

            int nodeIndex = m_nNodeCount++;
            if (nodeIndex >= m_arrNodes.Length)
            {
                ResizeArrays(m_arrNodes.Length * 2);
            }

            Actor medianActor = m_arrActors[median];
            Vector3 position = medianActor.GetPosition();

            m_arrNodes[nodeIndex] = new KDTreeNode
            {
                Position = position,
                Actor = medianActor,
                Axis = axis,
                LeftChild = BuildNode(start, median, depth + 1),
                RightChild = BuildNode(median + 1, end, depth + 1)
            };

            return nodeIndex;
        }
        //--------------------------------------------------------
        private int Partition(int start, int end, int axis)
        {
            int mid = (start + end) / 2;
            SelectKthSmallest(start, end - 1, mid, axis);
            return mid;
        }
        //--------------------------------------------------------
        private void SelectKthSmallest(int left, int right, int k, int axis)
        {
            while (left < right)
            {
                int pivotIndex = PartitionAroundPivot(left, right, axis);

                if (k <= pivotIndex)
                {
                    right = pivotIndex;
                }
                else
                {
                    left = pivotIndex + 1;
                }
            }
        }
        //--------------------------------------------------------
        private int PartitionAroundPivot(int left, int right, int axis)
        {
            Vector3 pivotValue = m_arrActors[right].GetPosition();
            int pivotIndex = left;

            for (int i = left; i < right; i++)
            {
                Vector3 currentValue = m_arrActors[i].GetPosition();
                if (GetValueByAxis(currentValue, axis) < GetValueByAxis(pivotValue, axis))
                {
                    Swap(i, pivotIndex);
                    pivotIndex++;
                }
            }

            Swap(pivotIndex, right);
            return pivotIndex;
        }
        //--------------------------------------------------------
        private float GetValueByAxis(FVector3 position, int axis)
        {
            switch (axis)
            {
                case 0: return position.x;
                case 1: return position.y;
                case 2: return position.z;
                default: return 0;
            }
        }
        //--------------------------------------------------------
        private void Swap(int i, int j)
        {
            Actor temp = m_arrActors[i];
            m_arrActors[i] = m_arrActors[j];
            m_arrActors[j] = temp;
        }
        //--------------------------------------------------------
        private void QueryNode(int nodeIndex, FVector3 queryPos, float radiusSquared, List<Actor> result, Actor pIngore = null)
        {
            if (nodeIndex < 0 || nodeIndex >= m_nNodeCount)
            {
                return;
            }

            KDTreeNode node = m_arrNodes[nodeIndex];
            float distanceSquared = (node.Position - queryPos).sqrMagnitude;

            if (distanceSquared <= radiusSquared)
            {
                if(node.Actor != pIngore) result.Add(node.Actor);
            }

            float axisValue = GetValueByAxis(queryPos, node.Axis);
            float nodeAxisValue = GetValueByAxis(node.Position, node.Axis);

            if (axisValue < nodeAxisValue)
            {
                QueryNode(node.LeftChild, queryPos, radiusSquared, result, pIngore);
                if ((nodeAxisValue - axisValue) * (nodeAxisValue - axisValue) <= radiusSquared)
                {
                    QueryNode(node.RightChild, queryPos, radiusSquared, result, pIngore);
                }
            }
            else
            {
                QueryNode(node.RightChild, queryPos, radiusSquared, result, pIngore);
                if ((axisValue - nodeAxisValue) * (axisValue - nodeAxisValue) <= radiusSquared)
                {
                    QueryNode(node.LeftChild, queryPos, radiusSquared, result, pIngore);
                }
            }
        }
        //--------------------------------------------------------
        private void QueryNodeBounds(int nodeIndex, Bounds queryBounds, List<Actor> result, Actor pIngore = null)
        {
            if (nodeIndex < 0 || nodeIndex >= m_nNodeCount)
            {
                return;
            }

            KDTreeNode node = m_arrNodes[nodeIndex];

            if (queryBounds.Contains(node.Position))
            {
                if( node.Actor != pIngore)
                    result.Add(node.Actor);
            }

            float axisValue = GetValueByAxis(node.Position, node.Axis);
            float minValue = GetValueByAxis(queryBounds.min, node.Axis);
            float maxValue = GetValueByAxis(queryBounds.max, node.Axis);

            if (minValue < axisValue)
            {
                QueryNodeBounds(node.LeftChild, queryBounds, result, pIngore);
            }

            if (maxValue > axisValue)
            {
                QueryNodeBounds(node.RightChild, queryBounds, result, pIngore);
            }
        }
        //--------------------------------------------------------
        private void QueryNodeRay(int nodeIndex, FRay ray, float maxDistance, List<Actor> result, Actor pIngore = null)
        {
            if (nodeIndex < 0 || nodeIndex >= m_nNodeCount)
            {
                return;
            }

            KDTreeNode node = m_arrNodes[nodeIndex];

            if (node.Actor.GetBounds().RayHit(ray))
            {
                if(node.Actor != pIngore)
                    result.Add(node.Actor);
            }

            float axisValue = GetValueByAxis(node.Position, node.Axis);
            float rayOriginValue = GetValueByAxis(ray.origin, node.Axis);
            float rayDirectionValue = GetValueByAxis(ray.direction, node.Axis);

            if (rayDirectionValue != 0)
            {
                float t = (axisValue - rayOriginValue) / rayDirectionValue;

                if (t >= 0 && t <= maxDistance)
                {
                    QueryNodeRay(node.LeftChild, ray, maxDistance, result, pIngore);
                    QueryNodeRay(node.RightChild, ray, maxDistance, result, pIngore);
                    return;
                }
            }

            if ((rayOriginValue < axisValue) ^ (rayDirectionValue > 0))
            {
                QueryNodeRay(node.LeftChild, ray, maxDistance, result, pIngore);
            }
            else
            {
                QueryNodeRay(node.RightChild, ray, maxDistance, result, pIngore);
            }
        }
        //--------------------------------------------------------
        private void ResizeArrays(int newCapacity)
        {
            System.Array.Resize(ref m_arrNodes, newCapacity);
            System.Array.Resize(ref m_arrActors, newCapacity);
        }
        //--------------------------------------------------------
        public void DebugDraw(bool bGizmos)
        {
#if UNITY_EDITOR
            BuildTreeIfNeeded();
            if (m_nNodeCount > 0)
            {
                DebugDrawNode(0, Color.white, bGizmos);
            }
#endif
        }
#if UNITY_EDITOR
        //--------------------------------------------------------
        private void DebugDrawNode(int nodeIndex, Color color, bool bGizmos)
        {
            if (nodeIndex < 0 || nodeIndex >= m_nNodeCount)
            {
                return;
            }

            KDTreeNode node = m_arrNodes[nodeIndex];
            
            // 绘制节点位置
            Debug.DrawRay(node.Position, Vector3.up * 0.5f, color);
            Debug.DrawRay(node.Position, Vector3.down * 0.5f, color);
            Debug.DrawRay(node.Position, Vector3.left * 0.5f, color);
            Debug.DrawRay(node.Position, Vector3.right * 0.5f, color);
            Debug.DrawRay(node.Position, Vector3.forward * 0.5f, color);
            Debug.DrawRay(node.Position, Vector3.back * 0.5f, color);
            
            // 绘制分割平面（仅可视化表示）
            float planeSize = 2.0f;
            Vector3 planeNormal = Vector3.zero;
            planeNormal[node.Axis] = 1.0f;
            
            // 绘制分割平面的示意线
            Vector3 lineStart = node.Position;
            Vector3 lineEnd = node.Position;
            lineStart[(node.Axis + 1) % 3] -= planeSize;
            lineStart[(node.Axis + 2) % 3] -= planeSize;
            lineEnd[(node.Axis + 1) % 3] += planeSize;
            lineEnd[(node.Axis + 2) % 3] += planeSize;
            Debug.DrawLine(lineStart, lineEnd, Color.green);
            
            lineStart = node.Position;
            lineEnd = node.Position;
            lineStart[(node.Axis + 1) % 3] -= planeSize;
            lineStart[(node.Axis + 2) % 3] += planeSize;
            lineEnd[(node.Axis + 1) % 3] += planeSize;
            lineEnd[(node.Axis + 2) % 3] -= planeSize;
            Debug.DrawLine(lineStart, lineEnd, Color.green);
            
            // 递归绘制子节点
            if (node.LeftChild >= 0)
            {
                DebugDrawNode(node.LeftChild, new Color(color.r * 0.8f, color.g * 0.8f, color.b * 0.8f), bGizmos);
            }
            if (node.RightChild >= 0)
            {
                DebugDrawNode(node.RightChild, new Color(color.r * 0.8f, color.g * 0.8f, color.b * 0.8f), bGizmos);
            }
        }
#endif
    }
}
