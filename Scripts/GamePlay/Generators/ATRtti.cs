//auto generated
using System.Collections.Generic;
namespace Framework.AT.Runtime
{
	public class ATRtti
	{
		static Dictionary<int, System.Type> ms_vIdTypes = null;
		static Dictionary<System.Type, int> ms_vTypeIds = null;
		//-----------------------------------------------------
		static void Init()
		{
			if(ms_vIdTypes != null) return;
			if(ms_vIdTypes == null) ms_vIdTypes = new Dictionary<int, System.Type>(2);
			if(ms_vTypeIds == null) ms_vTypeIds = new Dictionary<System.Type,int>(2);
			ms_vIdTypes.Clear();
			ms_vTypeIds.Clear();
			ms_vIdTypes[-1] = typeof(Framework.ActorSystem.Runtime.Actor);
			ms_vTypeIds[typeof(Framework.ActorSystem.Runtime.Actor)] = -1;
			ms_vIdTypes[-2] = typeof(Framework.ActorSystem.Runtime.ActorManager);
			ms_vTypeIds[typeof(Framework.ActorSystem.Runtime.ActorManager)] = -2;
		}
		//-----------------------------------------------------
		public static System.Type GetClassType(int typeId)
		{
			Init();
			if(ms_vIdTypes.TryGetValue(typeId, out var classType)) return classType;
			return null;
		}
		//-----------------------------------------------------
		public static int GetClassTypeId(System.Type type)
		{
			Init();
			if(ms_vTypeIds.TryGetValue(type, out var typeId)) return typeId;
			return 0;
		}
	}
}
