/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	IProjectileParticle
作    者:	HappLI
描    述:	飞行道具特效接口
*********************************************************************/
using Framework.ActorSystem.Runtime;
namespace Framework.ActorSystem.Runtime
{
    public interface IProjectileParticle: IContextData
    {
        void SetOwner(Actor pActor);
        void EnableLaunch(bool bEnable);
        void PauseLaunch();
        void ResumeLaunch();
    }
}

