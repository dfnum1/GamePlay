#if UNITY_EDITOR
/********************************************************************
生成日期:	25:7:2019   14:35
类    名: 	AProjectileEditorLogic
作    者:	HappLI
描    述:	飞行道具编辑器逻辑基础类
*********************************************************************/
using Framework.ActorSystem.Runtime;
using Framework.ED;
using System;
using System.Collections.Generic;

namespace Framework.ProjectileSystem.Editor
{
    public abstract class AProjectileEditorLogic : AEditorLogic
    {
        public virtual void New() { }
        public virtual void Play(bool bPlay) { }
        public virtual void Stop() { }
        public virtual void Reload() { }
    }
}
#endif