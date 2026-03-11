//auto generated
namespace Framework.AT.Runtime
{
	[Framework.Base.EditorSetupInit]
	internal class ATRegisterInternalHandler
	{
		//-----------------------------------------------------
		public static void Init()
		{
			Register(-8, typeof(Framework.Db.User),Framework.Db.Framework_Db_User.DoAction,-14/*Framework.Base.TypeObject*/);
			Register(-7, typeof(Framework.Db.UserManager),Framework.Db.Framework_Db_UserManager.DoAction,-19/*Framework.Core.AModule*/);
			Register(-6, typeof(Framework.State.Runtime.GameWorld),Framework.State.Runtime.Framework_State_Runtime_GameWorld.DoAction,-19/*Framework.Core.AModule*/);
			Register(-18, typeof(Framework.Cutscene.Runtime.ICutsceneData),null,-1/*Framework.Base.IUserData*/);
			Register(-2, typeof(Framework.ActorSystem.Runtime.Actor),Framework.ActorSystem.Runtime.Framework_ActorSystem_Runtime_Actor.DoAction,-179068835/*Framework.ActorSystem.Runtime.TypeActor*/);
			Register(-20, typeof(Framework.ActorSystem.Runtime.ActorManager),Framework.ActorSystem.Runtime.Framework_ActorSystem_Runtime_ActorManager.DoAction,-19/*Framework.Core.AModule*/);
			Register(-10, typeof(Framework.ActorSystem.Runtime.Buff),Framework.ActorSystem.Runtime.Framework_ActorSystem_Runtime_Buff.DoAction,84673252/*Framework.ActorSystem.Runtime.AActorStateInfo*/);
			Register(-9, typeof(Framework.ActorSystem.Runtime.BuffSystem),Framework.ActorSystem.Runtime.Framework_ActorSystem_Runtime_BuffSystem.DoAction,-179068835/*Framework.ActorSystem.Runtime.TypeActor*/);
			Register(-5, typeof(Framework.ActorSystem.Runtime.HitFrameActor),Framework.ActorSystem.Runtime.Framework_ActorSystem_Runtime_HitFrameActor.DoAction,-2011946460/*System.ValueType*/);
			Register(-17, typeof(Framework.ActorSystem.Runtime.IContextData),null,-1/*Framework.Base.IUserData*/);
			Register(-4, typeof(Framework.ActorSystem.Runtime.Skill),Framework.ActorSystem.Runtime.Framework_ActorSystem_Runtime_Skill.DoAction,84673252/*Framework.ActorSystem.Runtime.AActorStateInfo*/);
			Register(-3, typeof(Framework.ActorSystem.Runtime.SkillSystem),Framework.ActorSystem.Runtime.Framework_ActorSystem_Runtime_SkillSystem.DoAction,-179068835/*Framework.ActorSystem.Runtime.TypeActor*/);
			Register(-11, typeof(Framework.ActorSystem.Runtime.ActorSystemUtil),Framework.ActorSystem.Runtime.Framework_ActorSystem_Runtime_ActorSystemUtil.DoAction,0);
			Register(-12, typeof(Framework.Data.ADataManager),null,-19/*Framework.Core.AModule*/);
			Register(-16, typeof(Framework.Data.ABaseData),null,-17/*Framework.ActorSystem.Runtime.IContextData*/);
			Register(-15, typeof(Framework.Data.Data_Base),null,-14/*Framework.Base.TypeObject*/);
			Register(-1, typeof(Framework.Base.IUserData),null,0);
			Register(-14, typeof(Framework.Base.TypeObject),null,-1/*Framework.Base.IUserData*/);
			Register(-19, typeof(Framework.Core.AModule),null,-1/*Framework.Base.IUserData*/);
		}
		//-----------------------------------------------------
		public static void Register(int typeId, System.Type type, ATCallHandler.OnActionDelegate onFunction, int parentTypeId =0)
		{
			ATRtti.Register(typeId,type,parentTypeId);
			ATCallHandler.RegisterHandler(typeId,onFunction);
		}
	}
}
