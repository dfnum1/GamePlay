/********************************************************************
生成日期:	11:03:2023
类    名: 	AttriCustomDrawer
作    者:	HappLI
描    述:	自定义属性绘制
*********************************************************************/
#if UNITY_EDITOR
using Framework.ActorSystem.Runtime;
using Framework.AT.Editor;
using Framework.AT.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Framework.ActorSystem.Editor
{
    [ATDrawer("DrawAttributePop", "OnDrawAttributePop")]
    public class AttriCustomDrawer
    {
        static List<byte> ms_vAttris =  null;
        static List<string> ms_vAttriPops = null;
        static void Init()
        {
            if (ms_vAttris != null && ms_vAttriPops != null)
                return;

            AActorAttrDatas pData = null;
            string[] guidDatas = AssetDatabase.FindAssets("t:AActorAttrDatas");
            if (guidDatas != null && guidDatas.Length > 0)
            {
                pData = AssetDatabase.LoadAssetAtPath<AActorAttrDatas>(AssetDatabase.GUIDToAssetPath(guidDatas[0]));
                if(pData.vAttributes!=null)
                {
                    ms_vAttris = new List<byte>();
                    ms_vAttriPops = new List<string>();
                    for (int i = 0; i < pData.vAttributes.Length; ++i)
                    {
                        var pAttri = pData.vAttributes[i];
                        ms_vAttris.Add(pAttri.attr);
                        ms_vAttriPops.Add(pAttri.name);
                    }
                }
            }
        }
        //----------------------------------------
        internal static VisualElement OnDrawAttributePop(ArvgPort port, IVariable portValue, Action<IVariable> onValueChanged, int width)
        {
            if (portValue == null || portValue.GetVariableType() != EVariableType.eInt)
                return null;
            Init();
            bool canEdit = port.attri.canEdit;

            var intVar = (AT.Runtime.VariableInt)portValue;
            int curIndex = ms_vAttris.IndexOf((byte)intVar.value);
            if (curIndex < 0) curIndex = 0;

            var popup = new PopupField<string>(ms_vAttriPops, curIndex)
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
                int idx = ms_vAttriPops.IndexOf(evt.newValue);
                if (idx >= 0)
                {
                    intVar.value = ms_vAttris[idx];
                    onValueChanged?.Invoke(intVar);
                }
            });

            return popup;
        }
    }
}
#endif