//auto generated
namespace Framework.AT.Runtime
{
	[ATEditorInitialize]
	internal class ATRegisterInternalHandler
	{
		//-----------------------------------------------------
		public static void Init()
		{
			Register(-6, typeof(Framework.State.Runtime.GameWorld),Framework.State.Runtime.Framework_State_Runtime_GameWorld.DoAction,-1834191620/*Framework.Core.AModule*/);
			Register(-2, typeof(Framework.ActorSystem.Runtime.Actor),Framework.ActorSystem.Runtime.Framework_ActorSystem_Runtime_Actor.DoAction,-179068835/*Framework.ActorSystem.Runtime.TypeActor*/);
			Register(-1, typeof(Framework.ActorSystem.Runtime.ActorManager),Framework.ActorSystem.Runtime.Framework_ActorSystem_Runtime_ActorManager.DoAction,-1834191620/*Framework.Core.AModule*/);
			Register(-5, typeof(Framework.ActorSystem.Runtime.HitFrameActor),Framework.ActorSystem.Runtime.Framework_ActorSystem_Runtime_HitFrameActor.DoAction,0);
			Register(-4, typeof(Framework.ActorSystem.Runtime.Skill),Framework.ActorSystem.Runtime.Framework_ActorSystem_Runtime_Skill.DoAction,84673252/*Framework.ActorSystem.Runtime.AActorStateInfo*/);
			Register(-3, typeof(Framework.ActorSystem.Runtime.SkillSystem),Framework.ActorSystem.Runtime.Framework_ActorSystem_Runtime_SkillSystem.DoAction,-179068835/*Framework.ActorSystem.Runtime.TypeActor*/);
		}
		//-----------------------------------------------------
		public static void Register(int typeId, System.Type type, ATCallHandler.OnActionDelegate onFunction, int parentTypeId =0)
		{
			ATRtti.Register(typeId,type,parentTypeId);
			ATCallHandler.RegisterHandler(typeId,onFunction);
		}
	}
}
