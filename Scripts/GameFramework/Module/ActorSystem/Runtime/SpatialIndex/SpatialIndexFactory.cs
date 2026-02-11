/********************************************************************
生成日期:	06:30:2025
类    名: 	SpatialIndexFactory
作    者:	HappLI
描    述:	创建空间
*********************************************************************/
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
#endif
namespace Framework.ActorSystem.Runtime
{
    internal static class SpatialIndexFactory
    {
        /// <summary>
        /// 默认世界边界
        /// </summary>
        public static readonly FBounds DefaultWorldBounds = new FBounds(Vector3.zero, new Vector3(1000, 1000, 1000));
        //-----------------------------------------------------
        /// <summary>
        /// 创建空间索引
        /// </summary>
        /// <param name="indexType">索引类型</param>
        /// <param name="worldBounds">世界边界（可选）</param>
        /// <returns>创建的空间索引实例</returns>
        public static ISpatialWorld CreateIndex(ESpatialIndexType indexType, FBounds? worldBounds = null)
        {
            FBounds bounds = worldBounds ?? DefaultWorldBounds;

            switch (indexType)
            {
                case ESpatialIndexType.Quadtree:
                    return new Quadtree(bounds);
                case ESpatialIndexType.Octree:
                    return new Octree(bounds);
                case ESpatialIndexType.KDTree:
                    return new KDTree();
                default:
                    throw new System.ArgumentException($"Unknown spatial index type: {indexType}");
            }
        }
        //-----------------------------------------------------
        /// <summary>
        /// 创建空间索引
        /// </summary>
        /// <param name="indexType">索引类型</param>
        /// <param name="worldMin">世界边界最小值</param>
        /// <param name="worldMax">世界边界最大值</param>
        /// <returns>创建的空间索引实例</returns>
        public static ISpatialWorld CreateIndex(ESpatialIndexType indexType, FVector3 worldMin, FVector3 worldMax)
        {
            FVector3 size = worldMax - worldMin;
            FVector3 center = worldMin + size * 0.5f;
            FBounds bounds = new FBounds(center, size);
            return CreateIndex(indexType, bounds);
        }
        //-----------------------------------------------------
        /// <summary>
        /// 创建空间索引
        /// </summary>
        /// <param name="indexType">索引类型</param>
        /// <param name="worldMinX">世界边界最小值X</param>
        /// <param name="worldMinY">世界边界最小值Y</param>
        /// <param name="worldMinZ">世界边界最小值Z</param>
        /// <param name="worldMaxX">世界边界最大值X</param>
        /// <param name="worldMaxY">世界边界最大值Y</param>
        /// <param name="worldMaxZ">世界边界最大值Z</param>
        /// <returns>创建的空间索引实例</returns>
        public static ISpatialWorld CreateIndex(ESpatialIndexType indexType, 
            float worldMinX, float worldMinY, float worldMinZ, 
            float worldMaxX, float worldMaxY, float worldMaxZ)
        {
            Vector3 min = new Vector3(worldMinX, worldMinY, worldMinZ);
            Vector3 max = new Vector3(worldMaxX, worldMaxY, worldMaxZ);
            return CreateIndex(indexType, min, max);
        }
    }
}
