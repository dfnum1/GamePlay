//auto generated
namespace Framework.AT.Runtime
{
	[ATEditorInitialize]
	internal class ATRegisterInternalHandler
	{
		//-----------------------------------------------------
		public static void Init()
		{
			Register(-1, typeof(Framework.ActorSystem.Runtime.Actor),Framework.ActorSystem.Runtime.Framework_ActorSystem_Runtime_Actor.DoAction,-179068835/*Framework.ActorSystem.Runtime.TypeActor*/);
			Register(-3, typeof(Framework.ActorSystem.Runtime.HitFrameActor),Framework.ActorSystem.Runtime.Framework_ActorSystem_Runtime_HitFrameActor.DoAction,0);
			Register(-2, typeof(Framework.ActorSystem.Runtime.Skill),Framework.ActorSystem.Runtime.Framework_ActorSystem_Runtime_Skill.DoAction,84673252/*Framework.ActorSystem.Runtime.AActorStateInfo*/);
		}
		//-----------------------------------------------------
		public static void Register(int typeId, System.Type type, ATCallHandler.OnActionDelegate onFunction, int parentTypeId =0)
		{
			
			ATRtti.Register(typeId,type,parentTypeId);
			ATCallHandler.RegisterHandler(typeId,onFunction);
		}
	}
}
