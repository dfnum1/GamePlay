/********************************************************************
生成日期:	07:03:2025
类    名: 	ATRtti
作    者:	HappLI
描    述:	RTTI 类
*********************************************************************/
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
namespace Framework.AT.Runtime
{
	public class ATRtti
	{
		static Dictionary<int, System.Type> ms_vIdTypes = null;
		static Dictionary<System.Type, int> ms_vTypeIds = null;
        static Dictionary<int, int> ms_vParentTypeIds = null;
        //-----------------------------------------------------
        public static void Register(int typeId, System.Type type, int parentTypeId = 0)
		{
			if(ms_vIdTypes == null)
			{
				ms_vIdTypes = new Dictionary<int, System.Type>(128);
				ms_vTypeIds = new Dictionary<System.Type, int>(128);
                ms_vParentTypeIds = new Dictionary<int, int>(128);
            }
			ms_vIdTypes[typeId] = type;
            ms_vTypeIds[type] = typeId;
            if(parentTypeId != 0)
            {
                ms_vParentTypeIds[typeId] = parentTypeId;
            }
        }
        //-----------------------------------------------------
        public static System.Type GetClassType(int typeId)
		{
            if (ms_vIdTypes == null) return null;
            if (ms_vIdTypes.TryGetValue(typeId, out var classType)) return classType;
			return null;
		}
        //-----------------------------------------------------
        public static int GetClassParentTypeId(int typeId)
        {
            if (ms_vParentTypeIds == null) return 0;
            if (ms_vParentTypeIds.TryGetValue(typeId, out var classType)) return classType;
            return 0;
        }
        //-----------------------------------------------------
        public static int GetClassTypeId(System.Type type)
		{
            if (ms_vIdTypes == null)
            {
                return BuildHashCode(type);
            }
			if(ms_vTypeIds.TryGetValue(type, out var typeId)) return typeId;
            return BuildHashCode(type);
        }
        //------------------------------------------------------
        internal static int BuildHashCode(System.Type type)
        {
#if UNITY_EDITOR
            if (type == null) return 0;
            if (type.IsDefined(typeof(ATExportAttribute), false))
            {
                int hashGuid = type.GetCustomAttribute<ATExportAttribute>().guid;
                if (hashGuid != 0) return hashGuid;
            }
            return Animator.StringToHash(type.FullName);
#else
            return 0;
#endif

        }
    }
}
