using System.Collections.Generic;
namespace Framework.AT.Runtime
{
	public class ATRtti
	{
		static Dictionary<int, System.Type> ms_vIdTypes = null;
		static Dictionary<System.Type, int> ms_vTypeIds = null;
        //-----------------------------------------------------
		public static void Register(int typeId, System.Type type)
		{
			if(ms_vIdTypes == null)
			{
				ms_vIdTypes = new Dictionary<int, System.Type>(128);
				ms_vTypeIds = new Dictionary<System.Type, int>(128);
            }
			ms_vIdTypes[typeId] = type;
            ms_vTypeIds[type] = typeId;
        }
        //-----------------------------------------------------
        public static System.Type GetClassType(int typeId)
		{
            if (ms_vIdTypes == null) return null;
            if (ms_vIdTypes.TryGetValue(typeId, out var classType)) return classType;
			return null;
		}
		//-----------------------------------------------------
		public static int GetClassTypeId(System.Type type)
		{
			if (ms_vIdTypes == null) return 0;
			if(ms_vTypeIds.TryGetValue(type, out var typeId)) return typeId;
			return 0;
		}
	}
}
