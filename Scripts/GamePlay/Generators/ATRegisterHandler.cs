//auto generated
namespace Framework.AT.Runtime
{
	internal class ATRegisterInternalHandler
    {
		//-----------------------------------------------------
		public static void Init()
		{
			Register(-1, typeof(Framework.ActorSystem.Runtime.Actor),Framework.ActorSystem.Runtime.Framework_ActorSystem_Runtime_Actor.DoAction);
		}
		//-----------------------------------------------------
		public static void Register(int typeId, System.Type type, ATCallHandler.OnActionDelegate onFunction)
		{
			ATRtti.Register(typeId, type);
			ATCallHandler.RegisterHandler(typeId,onFunction);
		}
	}
}
