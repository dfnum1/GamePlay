/********************************************************************
生成日期:	06:30:2025
类    名: 	ISpatialWorld
作    者:	HappLI
描    述:	Actor管理器
*********************************************************************/
using System;
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
    public enum ESpatialIndexType
    {
        Quadtree,// 四叉树(2D)
        Octree, // 八叉树(3D)
        KDTree // KD树(3D)
    }
    public interface ISpatialWorld : IDisposable
    {
        void AddActor(Actor actor);
        void RemoveActor(Actor actor);
        void UpdateActor(Actor actor);
        void QueryActorsAtPosition(FVector3 position, FFloat radius, List<Actor> result, Actor pIngore = null);
        void QueryActorsInBounds(WorldBoundBox boundBox, List<Actor> result, Actor pIngore = null);
        void QueryActorsByRay(FRay ray, FFloat maxDistance, List<Actor> result, Actor pIngore = null);
        void Clear();
        int Count { get; }
        void DebugDraw(bool bGizmos);
    }
}
