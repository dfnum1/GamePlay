/********************************************************************
生成日期:	07:03:2025
类    名: 	ATRtti
作    者:	HappLI
描    述:	RTTI 类
*********************************************************************/
using Framework.Core;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
namespace Framework.AT.Runtime
{
	public class ATRtti
	{
		static Dictionary<int, System.Type> ms_vIdTypes = null;
		static Dictionary<System.Type, int> ms_vTypeIds = null;
        static Dictionary<int, int> ms_vParentTypeIds = null;
#if UNITY_EDITOR
        static Dictionary<string, int> ms_TypeFullNameIds = new Dictionary<string, int>(128);
#endif
        //-----------------------------------------------------
        internal static void ClearAll()
        {
            ms_vIdTypes?.Clear();
            ms_vTypeIds?.Clear();
            ms_vParentTypeIds?.Clear();
#if UNITY_EDITOR
            ms_TypeFullNameIds?.Clear();
#endif
        }
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
#if UNITY_EDITOR
            string fullName = type.FullName.Replace("+", "/").Replace(".", "/");
            string[] parts = fullName.Split('/');
            string regName = parts[parts.Length - 1];
            bool found = false;
            for (int i = parts.Length - 1; i >= 0; i--)
            {
                string tryName = string.Join("/", parts, i, parts.Length - i);
                if (!ms_TypeFullNameIds.ContainsKey(tryName))
                {
                    regName = tryName;
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                regName = fullName;
            }
            ms_TypeFullNameIds[regName] = typeId;
#endif
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
        //-----------------------------------------------------
        public static int GetClassTypeId(IUserData pPointer)
        {
            if (pPointer == null) return 0;
            if (ms_vIdTypes == null)
            {
                return BuildHashCode(pPointer.GetType());
            }
            if (ms_vTypeIds.TryGetValue(pPointer.GetType(), out var typeId)) return typeId;
            return BuildHashCode(pPointer.GetType());
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

            return BuildHashCode(type.FullName);
#else
            return 0;
#endif
        }
        //------------------------------------------------------
        internal static int BuildHashCode(string typeName)
        {
            typeName = typeName.ToLower();
            int hash = Animator.StringToHash(typeName);
            if (hash >= 0 && hash <= 50000)
            {
                string baseName = typeName;
                string append = "";
                int tryCount = 0;
                while (hash >= 0 && hash <= 50000 && tryCount < 1000)
                {
                    char ch = (char)('a' + (tryCount % 26));
                    append += ch;
                    hash = Animator.StringToHash(baseName + append);
                    ++tryCount;
                }
            }
            return hash;
        }
#if UNITY_EDITOR
        //------------------------------------------------------
        internal static Dictionary<string,int> GetTypeNameIds()
        {
            return ms_TypeFullNameIds;
        }
        //------------------------------------------------------
        internal static bool IsSubOfTypeId(int typeId, int parentId)
        {
            if (typeId == 0 || parentId == 0) return false;
            if (typeId == parentId) return true;
            var tempId = GetClassParentTypeId(typeId);
            while(tempId!=0)
            {
                if (tempId == parentId)
                    return true;
                tempId = GetClassParentTypeId(tempId);
            }
            return false;
        }
        //------------------------------------------------------
        internal static bool HasCommonConcreteBaseType(System.Type typeA, System.Type typeB)
        {
            if (typeA == typeB) return true;
            var baseTypesA = GetConcreteBaseTypes(typeA);
            var baseTypesB = GetConcreteBaseTypes(typeB);
            return baseTypesA.Intersect(baseTypesB).Any();
        }
        //------------------------------------------------------
        private static IEnumerable<System.Type> GetConcreteBaseTypes(System.Type type)
        {
            var current = type.BaseType;
            while (current != null && current != typeof(object))
            {
                if (!current.IsAbstract && !current.IsInterface)
                    yield return current;
                current = current.BaseType;
            }
        }
#endif
    }
}
