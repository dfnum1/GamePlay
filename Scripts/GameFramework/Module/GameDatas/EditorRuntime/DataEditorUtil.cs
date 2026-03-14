#if UNITY_EDITOR
/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	DataEditorUtil
作    者:	HappLI
描    述:	
*********************************************************************/
using Framework.AT.Editor;
using Framework.AT.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
namespace Framework.Data.ED
{
    [ATDrawer(BaseATDrawerKey.Key_DrawCsvTablePop, "OnDrawCsvTablePop")]
    public class DataEditorUtil
    {
        private static System.IO.MemoryStream m_pMemoryStream = null;
        private static System.IO.BinaryReader m_pBinaryReader = null;
        //-------------------------------------------
        static System.IO.BinaryReader BeginBinary(byte[] bytes)
        {
            if (bytes == null || bytes.Length <= 0) return null;
            if (m_pBinaryReader == null)
            {
                m_pMemoryStream = new System.IO.MemoryStream();
                m_pBinaryReader = new System.IO.BinaryReader(m_pMemoryStream);
            }
            m_pMemoryStream.Seek(0, System.IO.SeekOrigin.Begin);
            m_pMemoryStream.SetLength(bytes.Length);
            m_pMemoryStream.Write(bytes, 0, bytes.Length);
            m_pMemoryStream.Seek(0, System.IO.SeekOrigin.Begin);
            return m_pBinaryReader;
        }
        //-------------------------------------------
        static void EndBinary()
        {
            if (m_pMemoryStream != null)
                m_pMemoryStream.SetLength(0);
        }
        //-------------------------------------------
        static Dictionary<string, System.Type> ms_TableNameTypes= null;
        static List<string> ms_TableNamePops = null;
        static List<int> ms_TableHashs = null;
        private static void CheckTableTypeMethod()
        {
            if(ms_TableNameTypes != null) return;
            ms_TableNameTypes = new Dictionary<string, System.Type>();
            foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                System.Type[] types = assembly.GetTypes();
                for (int i = 0; i < types.Length; ++i)
                {
                     System.Type tp = types[i];
                    if (!tp.IsSubclassOf(typeof(Data.Data_Base)))
                        continue;

                    ms_TableNameTypes[tp.Name.ToLower()] = tp;
                    if(tp.Name.StartsWith("CsvData_", System.StringComparison.OrdinalIgnoreCase))
                    {
                        ms_TableNameTypes[tp.Name.Substring("CsvData_".Length).ToLower()] = tp;
                    }
                }
            }
        }
        //-------------------------------------------
        private static void InitTableLists()
        {
            string[] assets = AssetDatabase.FindAssets("t:ACsvConfig");
            if (assets == null || assets.Length <= 0) return;
            ACsvConfig csvCfg = AssetDatabase.LoadAssetAtPath<ACsvConfig>(AssetDatabase.GUIDToAssetPath(assets[0]));
            if(csvCfg.Assets!=null && ms_TableNamePops == null)
            {
                ms_TableNamePops = new List<string>(csvCfg.Assets.Length);
                ms_TableHashs = new List<int>(csvCfg.Assets.Length);
                for (int i = 0; i < csvCfg.Assets.Length; ++i)
                {
                    if (csvCfg.Assets[i].Asset == null  || !csvCfg.Assets[i].exportAT) continue;
                    ms_TableNamePops.Add(csvCfg.Assets[i].Asset.name);
                    ms_TableHashs.Add(csvCfg.Assets[i].nHash);
                }
            }
        }
        //-------------------------------------------
        private static Dictionary<int, Data_Base> ms_TempDatas = new Dictionary<int, Data_Base>();
        //-------------------------------------------
        public static void ClearTables()
        {
            ms_TempDatas.Clear();
        }
        //-------------------------------------------
        public static System.Type GetTableType(string tableName)
        {
            CheckTableTypeMethod();
            if (ms_TableNameTypes.TryGetValue(tableName.ToLower(), out var tableType))
                return tableType;
            return null;
        }
        //-------------------------------------------
        public static T GetTable<T>(string tableName, bool bReload = false) where T : Data_Base
        {
            System.Type tableType = GetTableType(tableName);
            if (tableType == null) return null;
            return GetTable<T>(tableType, bReload);
        }
        //-------------------------------------------
        public static Dictionary<KT, VT> GetTableDatas<KT,VT>(string tableName, string datasName ="datas", bool bReload = false)
        {
            Data_Base csvTable = GetTable<Data_Base>(tableName, bReload);
            PropertyInfo prop = csvTable.GetType().GetProperty(datasName, BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);
            if (prop == null) return null;
            try
            {
                return (Dictionary<KT, VT>)prop.GetValue(csvTable);
            }
            catch (System.Exception ex)
            {
            	
            }
            return null;
        }
        //-------------------------------------------
        public static Dictionary<KT, VT> GetTableDatas<KT, VT>(Data_Base pPointer, string datasName = "datas", bool bReload = false)
        {
            PropertyInfo prop = pPointer.GetType().GetProperty(datasName, BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);
            if (prop == null) return null;
            try
            {
                return (Dictionary<KT, VT>)prop.GetValue(pPointer);
            }
            catch (System.Exception ex)
            {

            }
            return null;
        }
        //-------------------------------------------
        public static T GetTable<T>(System.Type type, bool bReload=false) where T : Data_Base
        {
            int hash = UnityEngine.Animator.StringToHash(type.Name.ToLower());
            Data_Base table;
           if (!bReload && ms_TempDatas.TryGetValue(hash, out table)) return table as T;

            string[] assets = AssetDatabase.FindAssets("t:ACsvConfig");
            if (assets == null || assets.Length <= 0) return null;
            ACsvConfig csvCfg = AssetDatabase.LoadAssetAtPath<ACsvConfig>(AssetDatabase.GUIDToAssetPath(assets[0]));
			if (hash == 0) return null;
			bool bHash = false;
			CsvAsset csvData = new CsvAsset();
			for (int i = 0; i < csvCfg.Assets.Length;++i)
            {
				if(csvCfg.Assets[i].nHash == hash)
                {
					bHash = true;
					csvData = csvCfg.Assets[i];
					break;
				}
            }
			if(!bHash)
            {
				return null;
            }

			T newTable = (T)System.Activator.CreateInstance(type);
            if (csvData.type == EDataType.Binary)
            {
                if (!newTable.LoadBinary(BeginBinary(csvData.Asset.bytes)))
                    Framework.Base.Logger.Warning(csvData.Asset.name + ".bytes: load failed ... ");
                    EndBinary();
            }
            else if (csvData.type == EDataType.Json)
            {
                if (!newTable.LoadJson(csvData.Asset.text))
                    Framework.Base.Logger.Warning(csvData.Asset.name + ".json: load failed ... ");
            }
            else
            {
				if (!newTable.LoadData(csvData.Asset.text))
				{
					Framework.Base.Logger.Warning(csvData.Asset.name + ".csv: load failed ... ");
				}
			}
			newTable.strFilePath = AssetDatabase.GetAssetPath(csvData.Asset);
			ms_TempDatas[hash] = newTable;
			return newTable;
        }
        //-------------------------------------------
        public static System.Type GetTableDataType(System.Type tableType)
        {
            System.Type dataType = null;
            var nestedTypes = tableType.GetNestedTypes(BindingFlags.Public | BindingFlags.Instance);
            for (int i = 0; i < nestedTypes.Length; ++i)
            {
                if (nestedTypes[i].IsSubclassOf(typeof(ABaseData)))
                {
                    dataType = nestedTypes[i];
                    break;
                }
            }
            return dataType;
        }
        //-------------------------------------------
        static Dictionary<string, string> ms_vCacheAttrFiledDisplayName = new Dictionary<string, string>();
        public static string GetTableDataAttrFieldDisplayName(Data_Base dataBase, string attrFieldName, string attrValue, string displayFieldName)
        {
            if (dataBase == null) return null;
            string key = dataBase.GetType() + attrFieldName + attrValue;
            if (ms_vCacheAttrFiledDisplayName.TryGetValue(key, out var displayName))
                return displayName;

            var datasProp = dataBase.GetType().GetProperty("datas", BindingFlags.Instance | BindingFlags.Public);
            if (datasProp == null)
            {
                ms_vCacheAttrFiledDisplayName[key] = null;
                return null;
            }

            var dataMap = datasProp.GetValue(dataBase);
            if (dataMap == null)
            {
                ms_vCacheAttrFiledDisplayName[key] = null;
                return null;
            }

            if (!typeof(System.Collections.IDictionary).IsAssignableFrom(dataMap.GetType()))
            {
                ms_vCacheAttrFiledDisplayName[key] = null;
                return null;
            }

            PropertyInfo[] properties = dataMap.GetType().GetProperties();
            PropertyInfo keysProp = System.Array.Find(properties, prop => prop.Name == "Keys");
            PropertyInfo valuesProp = System.Array.Find(properties, prop => prop.Name == "Values");

            if (keysProp == null || valuesProp == null)
            {
                ms_vCacheAttrFiledDisplayName[key] = null;
                return null;
            }

            MethodInfo getValueMethod = valuesProp.PropertyType.GetMethod("GetEnumerator");
            if (getValueMethod == null)
            {
                ms_vCacheAttrFiledDisplayName[key] = null;
                return null;
            }

            var genicParams = dataMap.GetType().GenericTypeArguments;
            if (genicParams.Length != 2)
            {
                ms_vCacheAttrFiledDisplayName[key] = null;
                return null;
            }
            List<object> vDatas = new List<object>();
            IEnumerator valueEnumerator = (IEnumerator)getValueMethod.Invoke(valuesProp.GetValue(dataMap), null);
            while (valueEnumerator.MoveNext())
            {
                object value = valueEnumerator.Current;

                if (typeof(IList).IsAssignableFrom(value.GetType()))
                    continue;
                else
                {
                    var filedAttr = value.GetType().GetField(attrFieldName, BindingFlags.Public| BindingFlags.Instance);
                    if(filedAttr!=null)
                    {
                        if(attrValue ==  filedAttr.GetValue(value).ToString())
                        {
                            var displayField = value.GetType().GetField(displayFieldName, BindingFlags.Public | BindingFlags.Instance);
                            if(displayField!=null)
                            {
                                string tempName = displayField.GetValue(value)?.ToString();
                                ms_vCacheAttrFiledDisplayName[key] = tempName;
                                return tempName;
                            }
                        }
                    }
                    vDatas.Add(value);
                }
            }
            ms_vCacheAttrFiledDisplayName[key] = null;
            return null;
        }
        //-------------------------------------------
        public static T GetTable<T>(bool bReload = false) where T : Data_Base, new()
        {
            return GetTable<T>(typeof(T), bReload);
		}
        //----------------------------------------
        internal static VisualElement OnDrawCsvTablePop(ArvgPort port, IVariable portValue, Action<IVariable> onValueChanged, int width)
        {
            if (portValue == null || portValue.GetVariableType() != EVariableType.eInt)
                return null;
            InitTableLists();
            bool canEdit = port.attri.canEdit;

            var intVar = (AT.Runtime.VariableInt)portValue;
            int curIndex = ms_TableHashs.IndexOf((int)intVar.value);
            if (curIndex < 0) curIndex = 0;

            var popup = new UnityEditor.UIElements.PopupField<string>(ms_TableNamePops, curIndex)
            {
                style =
                {
                    width = width,
                    marginLeft = 4,
                    unityTextAlign = UnityEngine.TextAnchor.MiddleRight
                }
            };
            popup.SetEnabled(canEdit);

            popup.RegisterValueChangedCallback(evt =>
            {
                int idx = ms_TableNamePops.IndexOf(evt.newValue);
                if (idx >= 0)
                {
                    intVar.value = ms_TableHashs[idx];
                    port.onValueChange?.Invoke(intVar);
                    onValueChanged?.Invoke(intVar);
                }
            });

            return popup;
        }
    }
}
#endif