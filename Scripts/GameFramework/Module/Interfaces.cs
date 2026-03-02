using Framework.ActorSystem.Runtime;
using Framework.Base;
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
namespace Framework.Core
{
    public interface IUpdate
    {
        void Update(float fFrame);
    }
    public interface IFixedUpdate
    {
        void FixedUpdate(float fFrame);
    }
    public interface ILateUpdate
    {
        void LateUpdate(float fFrame);
    }
    public interface ITouchInput
    {
        void OnTouchBegin(TouchInput.TouchData touch);
        void OnTouchMove(TouchInput.TouchData touch);
        void OnTouchWheel(float wheel, Vector2 mouse);
        void OnTouchEnd(TouchInput.TouchData touch);
    }
    public interface IKeyInput
    {
        void OnKeyDown(KeyCode button);
        void OnKeyUp(KeyCode button);
    }
    public interface IDrawGizmos
    {
        void DrawGizmos();
    }
    public interface IPause
    {
        void OnPause(bool bPause);
    }
    public interface IJobUpdate
    {
        bool OnJobUpdate(float fFrame, IUserData userData = null);
        int GetJob();
        void OnJobComplete(IUserData userData = null);
    }
    public interface IThreadJob
    {
        bool OnThreadUpdate(float fFrame, IUserData userData = null);
        int GetThread();
        void OnThreadComplete(IUserData userData = null);
    }

    public interface INavSimplePath
    {
        bool OnSimpleFindPath(Actor pActor, FVector3 toPosition, Action<List<FVector3>> vPaths);
    }
}
