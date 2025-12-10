//auto generated
namespace Framework.AT.Runtime
{
	[ATEditorInitialize]
	internal class ATRegisterInternalHandler
	{
		//-----------------------------------------------------
		public static void Init()
		{
			Register(-1, typeof(Framework.ActorSystem.Runtime.Actor),Framework.ActorSystem.Runtime.Framework_ActorSystem_Runtime_Actor.DoAction,-677674211/*Framework.ActorSystem.Runtime.TypeActor*/);
		}
		//-----------------------------------------------------
		public static void Register(int typeId, System.Type type, ATCallHandler.OnActionDelegate onFunction, int parentTypeId =0)
		{
			
			ATRtti.Register(typeId,type,parentTypeId);
			ATCallHandler.RegisterHandler(typeId,onFunction);
		}
	}
}
