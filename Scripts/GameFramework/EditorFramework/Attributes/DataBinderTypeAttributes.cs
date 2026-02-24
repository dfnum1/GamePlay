/********************************************************************
生成日期:		1:11:2020 10:06
类    名: 	BinderTypeAttribute
作    者:	HappLI
描    述:	数据类型绑定
*********************************************************************/
using System;

namespace Framework.Data
{
    public class DataBinderTypeAttribute : System.Attribute
    {
        public string strConfigName = "";
        public string strMainKeyField = "nID";
        public string strMainKeyType="";
        public string DataField = "datas";
        public DataBinderTypeAttribute(string strConfigName, string strMainKeyType="ushort", string strMainKeyField = "nID", string DataField="datas")
        {
            this.strConfigName = strConfigName;
            this.strMainKeyField = strMainKeyField;
            this.strMainKeyType = strMainKeyType;
            this.DataField = DataField;
        }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class DataBindSetAttribute : System.Attribute
    {
#if UNITY_EDITOR
        public string field = "";
#endif
        public DataBindSetAttribute(string field)
        {
#if UNITY_EDITOR
            this.field = field;
#endif
        }
    }
}