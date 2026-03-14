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
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Framework.ActorSystem.Editor
{
    [ATDrawer(BaseATDrawerKey.Key_DrawAttributePop, "OnDrawAttributePop")]
    [ATDrawer(BaseATDrawerKey.Key_DrawFormulaTypePop, "OnDrawFormulaTypePop")]
    [ATDrawer(BaseATDrawerKey.Key_BuffStateDraw, "OnBuffStateDraw")]
    [ATDrawer(BaseATDrawerKey.Key_AttackGroupDraw, "OnAttackGroupDraw")]
    [ATDrawer(BaseATDrawerKey.Key_ActorTypeDraw, "OnActorTypeDraw")]
    [ATDrawer(BaseATDrawerKey.Key_ActorSubTypeDraw, "OnActorSubTypeDraw")]
    public class AttriCustomDrawer
    {
        static List<byte> ms_vAttris = null;
        static List<string> ms_vAttriPops = null;

        static List<int> ms_vFormulaTypes = null;
        static List<string> ms_vFormulaTypePops = null;

        static List<byte> ms_vBuffStateTypes = null;
        static List<string> ms_vBuffStatePops = null;

        static List<byte> ms_vAttackGroupTypes = null;
        static List<string> ms_vAttackGroupPops = null;

        static List<byte> ms_vActorTypeTypes = null;
        static List<string> ms_vActorTypePops = null;
        static Dictionary<byte, List<byte>> ms_vActorSubTypeTypes = null;
        static Dictionary<byte, List<string>> ms_vActorSubTypePops = null;

        internal static void Init(bool bForce = false)
        {
            if (!bForce)
            {
                if (ms_vAttris != null && ms_vAttriPops != null)
                    return;
            }

            ms_vBuffStatePops = new List<string>();
            ms_vBuffStateTypes = new List<byte>();
            ms_vFormulaTypes = new List<int>();
            ms_vFormulaTypePops = new List<string>();

            ms_vAttackGroupPops = new List<string>();
            ms_vAttackGroupTypes = new List<byte>();

            ms_vActorTypePops = new List<string>();
            ms_vActorTypeTypes = new List<byte>();
            ms_vActorSubTypeTypes = new Dictionary<byte, List<byte>>();
            ms_vActorSubTypePops = new Dictionary<byte, List<string>>();

            AActorAttrDatas pData = null;
            string[] guidDatas = AssetDatabase.FindAssets("t:AActorAttrDatas");
            if (guidDatas != null && guidDatas.Length > 0)
            {
                pData = AssetDatabase.LoadAssetAtPath<AActorAttrDatas>(AssetDatabase.GUIDToAssetPath(guidDatas[0]));
                if (pData.vAttributes != null)
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
                if (pData.vBuffStates != null)
                {
                    for (int i = 0; i < pData.vBuffStates.Length; ++i)
                    {
                        var pAttri = pData.vBuffStates[i];
                        ms_vBuffStateTypes.Add(pAttri.state);
                        ms_vBuffStatePops.Add(pAttri.name);
                    }
                }
                if (pData.vFormulas != null)
                {
                    for (int i = 0; i < pData.vFormulas.Length; ++i)
                    {
                        var pAttri = pData.vFormulas[i];
                        ms_vFormulaTypes.Add(pAttri.labelId);
                        ms_vFormulaTypePops.Add(pAttri.name);
                    }
                }
                if (pData.vAttackGroups != null)
                {
                    for (int i = 0; i < pData.vAttackGroups.Length; ++i)
                    {
                        var pAttri = pData.vAttackGroups[i];
                        ms_vAttackGroupTypes.Add(pAttri.group);
                        ms_vAttackGroupPops.Add(pAttri.name);
                    }
                }
                if (pData.vActorTypes != null)
                {
                    for (int i = 0; i < pData.vActorTypes.Length; ++i)
                    {
                        var pAttri = pData.vActorTypes[i];
                        ms_vActorTypeTypes.Add(pAttri.type);
                        ms_vActorTypePops.Add(pAttri.name);

                        if(pAttri.subTypes!=null)
                        {
                            List<string> names = new List<string>();
                            List<byte> types = new List<byte>();
                            foreach (var db in pAttri.subTypes)
                            {
                                names.Add(db.name);
                                types.Add(db.type);
                            }
                            ms_vActorSubTypePops[pAttri.type] = names;
                            ms_vActorSubTypeTypes[pAttri.type] = types;
                        }
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

            var popup = new UnityEditor.UIElements.PopupField<string>(ms_vAttriPops, curIndex)
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
        //----------------------------------------
        internal static VisualElement OnDrawFormulaTypePop(ArvgPort port, IVariable portValue, Action<IVariable> onValueChanged, int width)
        {
            if (portValue == null || portValue.GetVariableType() != EVariableType.eInt)
                return null;
            Init();
            bool canEdit = port.attri.canEdit;

            var intVar = (AT.Runtime.VariableInt)portValue;
            int curIndex = ms_vAttris.IndexOf((byte)intVar.value);
            if (curIndex < 0) curIndex = 0;

            var popup = new UnityEditor.UIElements.PopupField<string>(ms_vFormulaTypePops, curIndex)
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
                int idx = ms_vFormulaTypePops.IndexOf(evt.newValue);
                if (idx >= 0)
                {
                    intVar.value = ms_vFormulaTypes[idx];
                    onValueChanged?.Invoke(intVar);
                }
            });

            return popup;
        }
        //----------------------------------------
        internal static VisualElement OnAttackGroupDraw(ArvgPort port, IVariable portValue, Action<IVariable> onValueChanged, int width)
        {
            if (portValue == null || portValue.GetVariableType() != EVariableType.eInt)
                return null;
            Init();
            bool canEdit = port.attri.canEdit;

            var intVar = (AT.Runtime.VariableInt)portValue;
            int curIndex = ms_vAttackGroupTypes.IndexOf((byte)intVar.value);
            if (curIndex < 0) curIndex = 0;

            var popup = new UnityEditor.UIElements.PopupField<string>(ms_vAttackGroupPops, curIndex)
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
                int idx = ms_vAttackGroupPops.IndexOf(evt.newValue);
                if (idx >= 0)
                {
                    intVar.value = ms_vAttackGroupTypes[idx];
                    onValueChanged?.Invoke(intVar);
                }
            });

            return popup;
        }
        //----------------------------------------
        internal static VisualElement OnActorTypeDraw(ArvgPort port, IVariable portValue, Action<IVariable> onValueChanged, int width)
        {
            if (portValue == null || portValue.GetVariableType() != EVariableType.eInt)
                return null;
            Init();
            bool canEdit = port.attri.canEdit;

            var intVar = (AT.Runtime.VariableInt)portValue;
            int curIndex = ms_vActorTypeTypes.IndexOf((byte)intVar.value);
            if (curIndex < 0) curIndex = 0;

            var popup = new UnityEditor.UIElements.PopupField<string>(ms_vActorTypePops, curIndex)
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
                int idx = ms_vActorTypePops.IndexOf(evt.newValue);
                if (idx >= 0)
                {
                    intVar.value = ms_vActorTypeTypes[idx];
                    onValueChanged?.Invoke(intVar);
                }
            });

            return popup;
        }
        //----------------------------------------
        internal static VisualElement OnActorSubTypeDraw(ArvgPort port, IVariable portValue, Action<IVariable> onValueChanged, int width)
        {
            if (portValue == null || portValue.GetVariableType() != EVariableType.eInt)
                return null;
            Init();
            bool canEdit = port.attri.canEdit;

            List<string> vSubTypeNames = null;
            List<byte> vSubTypes = null;
            var byPort = port.GetByArgvPort();
            if(byPort!=null)
            {
                var portVar = byPort.GetVariable();
                if(portVar!=null && portVar.GetVariableType() == EVariableType.eInt)
                {
                    var portVal = (AT.Runtime.VariableInt)portVar;
                    ms_vActorSubTypePops.TryGetValue((byte)portVal.value, out vSubTypeNames);
                    ms_vActorSubTypeTypes.TryGetValue((byte)portVal.value, out vSubTypes);
                }
                byPort.onValueChange += (newVal) =>
                {
                    if (newVal != null && newVal.GetVariableType() == EVariableType.eInt)
                    {
                        var portVal = (AT.Runtime.VariableInt)newVal;
                        ms_vActorSubTypePops.TryGetValue((byte)portVal.value, out vSubTypeNames);
                        ms_vActorSubTypeTypes.TryGetValue((byte)portVal.value, out vSubTypes);

                        if (port.fieldElement != null)
                        {
                            port.fieldRoot.Remove(port.fieldElement);
                        }
                        var portValue = port.GetVariable();
                        var intVar = (AT.Runtime.VariableInt)portValue;
                        int curIndex = vSubTypes.IndexOf((byte)intVar.value);
                        if (curIndex < 0) curIndex = 0;

                        var popup = new UnityEditor.UIElements.PopupField<string>(vSubTypeNames, curIndex)
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
                            int idx = vSubTypeNames.IndexOf(evt.newValue);
                            if (idx >= 0)
                            {
                                intVar.value = vSubTypes[idx];
                                onValueChanged?.Invoke(intVar);
                            }
                        });
                        port.fieldRoot.Add(popup);
                        port.fieldElement = popup;
                    }
                };
            }
            if (vSubTypeNames == null || vSubTypes == null)
                return null;

            var intVar = (AT.Runtime.VariableInt)portValue;
            int curIndex = vSubTypes.IndexOf((byte)intVar.value);
            if (curIndex < 0) curIndex = 0;

            var popup = new UnityEditor.UIElements.PopupField<string>(vSubTypeNames, curIndex)
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
                int idx = vSubTypeNames.IndexOf(evt.newValue);
                if (idx >= 0)
                {
                    intVar.value = vSubTypes[idx];
                    onValueChanged?.Invoke(intVar);
                }
            });

            return popup;
        }
        //----------------------------------------
        internal static VisualElement OnBuffStateDraw(ArvgPort port, IVariable portValue, Action<IVariable> onValueChanged, int width)
        {
            if (portValue == null || portValue.GetVariableType() != EVariableType.eInt)
                return null;
            Init();
            bool canEdit = port.attri.canEdit;

            var intVar = (AT.Runtime.VariableInt)portValue;
            int curValue = intVar.value;

            // 控制展开/收起
            bool isExpanded = false;

            var container = new VisualElement();
            container.style.flexDirection = FlexDirection.Column;
            container.style.width = width;

            // 占位和折叠按钮
            var placeholder = new VisualElement();
            placeholder.style.height = 25;
            placeholder.style.flexGrow = 0;
            placeholder.style.flexDirection = FlexDirection.Row;
            placeholder.style.alignItems = Align.Center;

            // 图标
            var img = new Image
            {
                image = AgentTreeUtil.LoadIcon("Node/foldout"), // 默认收起
                style =
                {
                    width = 16,
                    height = 16,
                    marginRight = 4,
                    marginTop = 2,
                    marginLeft = 2,
                }
            };
            placeholder.Add(img);

            // 可点击区域
            var label = new Label("Buff状态");
            label.style.unityTextAlign = TextAnchor.MiddleLeft;
            label.style.flexGrow = 1;
            placeholder.Add(label);

            container.Add(placeholder);

            // 展开/收起事件
            placeholder.RegisterCallback<MouseDownEvent>(evt =>
            {
                isExpanded = !isExpanded;
                if(isExpanded)
                {
                    // 多选内容
                    var multiSelectContainer = new VisualElement();
                    multiSelectContainer.style.flexDirection = FlexDirection.Column;
                    multiSelectContainer.style.display = DisplayStyle.None; // 默认隐藏

                    void RefreshMultiSelect()
                    {
                        multiSelectContainer.Clear();
                        for (int i = 0; i < ms_vBuffStatePops.Count; ++i)
                        {
                            bool isChecked = (intVar.value & (1 << i)) != 0;

                            var row = new VisualElement();
                            row.style.flexDirection = FlexDirection.Row;
                            row.style.alignItems = Align.Center;
                            row.style.justifyContent = Justify.SpaceBetween;
                            row.style.marginTop = 1;
                            row.style.marginBottom = 1;

                            var stateLabel = new Label(ms_vBuffStatePops[i]);
                            stateLabel.style.flexGrow = 1;
                            stateLabel.style.unityTextAlign = TextAnchor.MiddleLeft;
                            stateLabel.style.marginRight = 4;

                            var toggle = new Toggle()
                            {
                                value = isChecked
                            };
                            toggle.style.flexGrow = 0;
                            toggle.style.width = 18;
                            toggle.SetEnabled(canEdit);

                            int bitIndex = i;
                            toggle.RegisterValueChangedCallback(evt =>
                            {
                                if (evt.newValue)
                                    intVar.value |= (1 << bitIndex);
                                else
                                    intVar.value &= ~(1 << bitIndex);

                                onValueChanged?.Invoke(intVar);
                            });

                            row.Add(stateLabel);
                            row.Add(toggle);
                            multiSelectContainer.Add(row);
                        }
                    }

                    RefreshMultiSelect();
                    container.Add(multiSelectContainer);
                    multiSelectContainer.style.display = isExpanded ? DisplayStyle.Flex : DisplayStyle.None;

                    float height = (ms_vBuffStatePops.Count) * 35;
                    port.bindPort.style.marginTop = height / 2;
                    port.bindPort.style.marginBottom = 5;
                    multiSelectContainer.style.minHeight = height;
                    img.transform.rotation = Quaternion.Euler(0, 0, 90);
                }
                else
                {
                    img.transform.rotation = Quaternion.identity;
                    //删除multiSelectContainer
                    if (container.childCount > 1)
                        container.RemoveAt(1);
                    port.bindPort.style.marginTop = 0;
                    port.bindPort.style.marginBottom = 0;

                }
                evt.StopPropagation();
            });

            return container;
        }
    }
}
#endif