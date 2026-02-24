using System;

namespace Framework.Data
{
    public class BinaryDiscardAttribute : System.Attribute
    {
#if UNITY_EDITOR
        public int version;
#endif
        public BinaryDiscardAttribute(int version)
        {
#if UNITY_EDITOR
            this.version = version;
#endif
        }
    }

    public class BinaryCodeMarcosAttribute : System.Attribute
    {
#if UNITY_EDITOR
        public string marcos;
#endif
        public BinaryCodeMarcosAttribute(string marcos)
        {
#if UNITY_EDITOR
            this.marcos = marcos;
#endif
        }
    }
    public class BinaryCodeAttribute : System.Attribute
    {
#if UNITY_EDITOR
        public int version;
        public string savePath = "";
#endif
        public BinaryCodeAttribute(int version, string savePath = "")
        {
#if UNITY_EDITOR
            this.version = version;
            this.savePath = savePath;
#endif
        }
    }

    public class BinaryFieldVersionAttribute : System.Attribute
    {
#if UNITY_EDITOR
        public int version;
#endif
        public BinaryFieldVersionAttribute(int version)
        {
#if UNITY_EDITOR
            this.version = version;
#endif
        }
    }

    public class BinaryUnServerAttribute : System.Attribute
    {
        public BinaryUnServerAttribute()
        {

        }
    }

    public class BinaryServerCodeAttribute : System.Attribute
    {
#if UNITY_EDITOR
        public string savePath = "";
#endif
        public BinaryServerCodeAttribute(string savePath = "")
        {
#if UNITY_EDITOR
            this.savePath = savePath;
#endif
        }
    }
}