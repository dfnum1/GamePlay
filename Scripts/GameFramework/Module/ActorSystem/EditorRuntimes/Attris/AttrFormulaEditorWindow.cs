/********************************************************************
生成日期:	11:03:2023
类    名: 	AttrFormulaEditorWindow
作    者:	HappLI
描    述:	属性表达式
*********************************************************************/
#if UNITY_EDITOR
using Framework.ActorSystem.Runtime;
using Framework.ED;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using static Framework.ActorSystem.Runtime.AActorAttrDatas;
using static Framework.ActorSystem.Runtime.AActorAttrDatas.AttrFormula;
namespace Framework.ActorSystem.Editor
{
    public class AttrFormulaEditorWindow : EditorWindowBase
    {
        private AActorAttrDatas m_Data;
        private int m_TabIndex = 0; // 0:属性 1:表达式
        private int m_SelectedAttrIndex = -1;
        private int m_SelectedFormulaIndex = -1;

        // 滚动位置
        private List<LambdaParam> m_vInputLambdas = new List<LambdaParam>();
        private string m_strFormulaInput = "";
        private Vector2 m_LeftScroll;
        private Vector2 m_RightScroll;
        [MenuItem("Tools/GamePlay/属性编辑器")]
        public static void Open()
        {
            if (EditorApplication.isCompiling)
            {
                EditorUtility.DisplayDialog("警告", "请等待编辑器完成编译再执行此功能", "确定");
                return;
            }
            AActorAttrDatas pData = null;
            string[] guidDatas = AssetDatabase.FindAssets("t:AActorAttrDatas");
            if (guidDatas != null && guidDatas.Length > 0)
            {
                pData = AssetDatabase.LoadAssetAtPath<AActorAttrDatas>(AssetDatabase.GUIDToAssetPath(guidDatas[0]));
            }
            if(pData == null)
            {
                if(EditorUtility.DisplayDialog("错误", "未找到任何AActorAttrDatas资源，请先创建", "创建", "取消"))
                {
                    string savePath = EditorUtility.SaveFilePanelInProject("创建属性数据", "ActorAttrDatas", "asset", "请选择保存路径", Application.dataPath);
                    if (string.IsNullOrEmpty(savePath))
                    {
                        return;
                    }
                    AActorAttrDatas projData = Framework.ED.EditorUtils.CreateUnityScriptObject<AActorAttrDatas>();
                    projData.name = "ActorAttrDatas";
                    AssetDatabase.CreateAsset(projData, savePath);
                    EditorUtility.SetDirty(projData);
                    AssetDatabase.SaveAssetIfDirty(projData);
                    pData = AssetDatabase.LoadAssetAtPath<AActorAttrDatas>(savePath);
                }
                else
                    return;
            }

            Open(pData);
        }
        //-----------------------------------------------------
        public static void Open(AActorAttrDatas attriData)
        {
            if (EditorApplication.isCompiling)
            {
                EditorUtility.DisplayDialog("警告", "请等待编辑器完成编译再执行此功能", "确定");
                return;
            }
            var window = GetWindow<AttrFormulaEditorWindow>("属性表达式编辑器");
            window.m_Data = attriData;
            window.titleContent = new GUIContent("属性表达式编辑器", AssetUtil.LoadTexture("ActorSystem/actor_attris.png"));
            window.minSize = new Vector2(800, 500);
        }
        //-----------------------------------------------------
        protected override void OnInnerGUI()
        {
            if (m_Data == null)
            {
                EditorGUILayout.HelpBox("未加载数据", MessageType.Info);
                return;
            }

            EditorGUILayout.BeginHorizontal();

            // 左侧：标签页+列表（带滚动）
            EditorGUILayout.BeginVertical(GUILayout.Width(position.width * 0.4f));
            m_TabIndex = GUILayout.Toolbar(m_TabIndex, new[] { "属性", "表达式" });
            EditorGUILayout.Space();

            m_LeftScroll = EditorGUILayout.BeginScrollView(m_LeftScroll);
            if (m_TabIndex == 0)
            {
                DrawAttrList();
            }
            else
            {
                DrawFormulaList();
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            // 右侧：编辑区（带滚动）
            EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
            EditorGUILayout.Space();
            m_RightScroll = EditorGUILayout.BeginScrollView(m_RightScroll);
            if (m_TabIndex == 0)
            {
                DrawAttrEdit();
            }
            else
            {
                DrawFormulaEdit();
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();

            // 底部操作栏
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal("box");
            GUILayout.FlexibleSpace();

            // 删除按钮
            if (m_TabIndex == 0 && m_SelectedAttrIndex >= 0 && m_SelectedAttrIndex < (m_Data.vAttributes?.Length ?? 0))
            {
                if (GUILayout.Button("删除属性", GUILayout.Width(100)))
                {
                    if (EditorUtility.DisplayDialog("确认删除", "确定要删除该属性吗？", "删除", "取消"))
                    {
                        var list = new List<AttrInfo>(m_Data.vAttributes);
                        list.RemoveAt(m_SelectedAttrIndex);
                        m_Data.vAttributes = list.ToArray();
                        m_SelectedAttrIndex = -1;
                    }
                }
            }
            if (m_TabIndex == 1 && m_SelectedFormulaIndex >= 0 && m_SelectedFormulaIndex < (m_Data.vFormulas?.Length ?? 0))
            {
                if (GUILayout.Button("删除表达式", GUILayout.Width(100)))
                {
                    if (EditorUtility.DisplayDialog("确认删除", "确定要删除该表达式吗？", "删除", "取消"))
                    {
                        var list = new List<AttrFormula>(m_Data.vFormulas);
                        list.RemoveAt(m_SelectedFormulaIndex);
                        m_Data.vFormulas = list.ToArray();
                        m_SelectedFormulaIndex = -1;
                    }
                }
            }

            // 保存按钮
            if (GUILayout.Button("保存", GUILayout.Width(100)))
            {
                EditorUtility.SetDirty(m_Data);
                AssetDatabase.SaveAssets();
                ShowNotification(new GUIContent("保存成功"));
            }
            EditorGUILayout.EndHorizontal();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(m_Data);
            }
        }
        //-----------------------------------------------------
        private void DrawAttrList()
        {
            if (m_Data.vAttributes == null)
                m_Data.vAttributes = new AttrInfo[0];

            for (int i = 0; i < m_Data.vAttributes.Length; ++i)
            {
                var attr = m_Data.vAttributes[i];
                using (new GUIColorScope(m_SelectedAttrIndex==i?Color.green:Color.white))
                {
                    if (GUILayout.Button($"{attr.name}[{attr.attr}]", EditorStyles.toolbarButton))
                    {
                        m_SelectedAttrIndex = i;
                    }
                }
    
            }
            if (GUILayout.Button("添加属性"))
            {
                var list = new List<AttrInfo>(m_Data.vAttributes);
                var attr = new AttrInfo() { name = "新属性", desc = "" };
                byte newAttr = 0;
                // 自动分配一个未使用的属性类型
                bool bExist = true;
                while (bExist)
                {
                    bExist = false;
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].attr == newAttr)
                        {
                            bExist = true;
                            newAttr++;
                            break;
                        }
                    }
                }
                attr.attr = newAttr;
                list.Add(attr);
                m_Data.vAttributes = list.ToArray();
                m_SelectedAttrIndex = m_Data.vAttributes.Length - 1;
            }
        }
        //-----------------------------------------------------
        private void DrawFormulaList()
        {
            if (m_Data.vFormulas == null)
                m_Data.vFormulas = new AttrFormula[0];

            for (int i = 0; i < m_Data.vFormulas.Length; ++i)
            {
                var formula = m_Data.vFormulas[i];
                using (new GUIColorScope(m_SelectedFormulaIndex == i ? Color.green : Color.white))
                {
                    if (GUILayout.Button($"{formula.name}", EditorStyles.toolbarButton))
                    {
                        m_SelectedFormulaIndex = i;
                    }
                }
  
            }
            if (GUILayout.Button("添加表达式"))
            {
                var list = new List<AttrFormula>(m_Data.vFormulas);
                list.Add(new AttrFormula() { name = "新表达式", vLambda = new List<LambdaParam>() });
                m_Data.vFormulas = list.ToArray();
                m_SelectedFormulaIndex = m_Data.vFormulas.Length - 1;
            }
        }
        //-----------------------------------------------------
        private void DrawAttrEdit()
        {
            if (m_SelectedAttrIndex < 0 || m_SelectedAttrIndex >= m_Data.vAttributes.Length)
            {
                EditorGUILayout.HelpBox("请选择属性", MessageType.Info);
                return;
            }
            var attr = m_Data.vAttributes[m_SelectedAttrIndex];
            byte newAttr = (byte)EditorGUILayout.IntField("属性类型", attr.attr);
            if (newAttr != attr.attr)
            {
                bool bExist = false;
                for (int i = 0; i < attr.attr; i++)
                {
                    if (m_Data.vAttributes[i].attr == newAttr)
                    {
                        this.ShowNotificationWarning("属性类型已存在，请选择其他类型");
                        bExist = true;
                        break;
                    }
                }
                if (!bExist)
                {
                    attr.attr = newAttr;
                }
            }
            attr.name = EditorGUILayout.DelayedTextField("属性名", attr.name);
            attr.desc = EditorGUILayout.TextField("描述", attr.desc);
            m_Data.vAttributes[m_SelectedAttrIndex] = attr;
        }
        //-----------------------------------------------------
        private void DrawFormulaEdit()
        {
            if (m_SelectedFormulaIndex < 0 || m_SelectedFormulaIndex >= m_Data.vFormulas.Length)
            {
                EditorGUILayout.HelpBox("请选择表达式", MessageType.Info);
                return;
            }
            var formula = m_Data.vFormulas[m_SelectedFormulaIndex];
            formula.name = EditorGUILayout.TextField("表达式名", formula.name);

            int curIndex = 0;
            string[] attrNames = GetAttrNames(out curIndex, formula.applayAttr);
            curIndex = EditorGUILayout.Popup("应用属性", curIndex, attrNames);
            if (curIndex >= 0 && curIndex < m_Data.vAttributes.Length)
                formula.applayAttr = m_Data.vAttributes[curIndex].attr;

            if (formula.vLambda == null)
                formula.vLambda = new List<LambdaParam>();
            m_strFormulaInput = EditorGUILayout.TextArea(m_strFormulaInput, GUILayout.Height(120));
            if (!string.IsNullOrEmpty(m_strFormulaInput))
            {
                if (GUILayout.Button("应用输入表达式"))
                {
                    formula.vLambda = FormulaTextToLambdaList(m_strFormulaInput);
                }
            }

            EditorGUILayout.LabelField("表达式内容：");
            DrawLambdaList(formula.vLambda, 0);

            // 公式文本
            string formulaText = LambdaListToFormulaText(formula.vLambda);
            EditorGUILayout.HelpBox("公式预览: " + formulaText, MessageType.None);

            m_Data.vFormulas[m_SelectedFormulaIndex] = formula;
        }
        //-----------------------------------------------------
        private void DrawLambdaList(List<LambdaParam> list, int indent)
        {
            if (list == null) return;

            int removeIndex = -1;
            for (int i = 0; i < list.Count; ++i)
            {
                var lambda = list[i];
                EditorGUILayout.BeginVertical("box", GUILayout.ExpandWidth(true));
                EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));

                GUILayout.Space(indent * 16);

                lambda.type = (EAttrFormulaType)EditorGUILayout.EnumPopup(lambda.type, GUILayout.Width(80));

                if (lambda.type == EAttrFormulaType.eBracket)
                {
                    if (lambda.subLambda == null)
                        lambda.subLambda = new List<LambdaParam>();
                    EditorGUILayout.LabelField("括号", GUILayout.Width(40));
                }
                else if (lambda.type == EAttrFormulaType.eActorAttr)
                {
                    // 属性下拉
                    int attrIndex = 0;
                    string[] attrNames = GetAttrNames(out attrIndex, (int)lambda.paramValue1);
                    using (new GUILabelWidthScope(40))
                    {
                        lambda.paramValue0 = EditorGUILayout.Popup("阵营", (int)lambda.paramValue0, new string[] { "攻击方", "受击方" }, GUILayout.Width(200));
                        attrIndex = EditorGUILayout.Popup("属性", attrIndex, attrNames, GUILayout.Width(200));
                    }
                    if (attrIndex >= 0 && attrIndex < m_Data.vAttributes.Length)
                        lambda.paramValue1 = m_Data.vAttributes[attrIndex].attr;
                }
                else if (lambda.type == EAttrFormulaType.eRandom)
                {
                    using (new GUILabelWidthScope(40))
                    {
                        lambda.paramValue0 = EditorGUILayout.FloatField("最小", lambda.paramValue0, GUILayout.Width(200));
                        lambda.paramValue1 = EditorGUILayout.FloatField("最大", lambda.paramValue1, GUILayout.Width(200));
                    }
                }
                else if (lambda.type == EAttrFormulaType.eAdd ||
                        lambda.type == EAttrFormulaType.eSub ||
                        lambda.type == EAttrFormulaType.eMul ||
                        lambda.type == EAttrFormulaType.eDiv)
                {
                    lambda.isUnary = EditorGUILayout.ToggleLeft("一元操作", lambda.isUnary, GUILayout.Width(67));
                    using (new GUILabelWidthScope(40))
                    {
                        if (lambda.isUnary)
                        {
                            lambda.paramValue0 = EditorGUILayout.FloatField("参数", lambda.paramValue0, GUILayout.Width(330));
                        }
                        else
                        {
                            lambda.paramValue0 = EditorGUILayout.FloatField("参数0", lambda.paramValue0, GUILayout.Width(165));
                            lambda.paramValue1 = EditorGUILayout.FloatField("参数1", lambda.paramValue1, GUILayout.Width(165));
                        }
                    }

                }
                else
                {
                    using (new GUILabelWidthScope(40))
                    {
                        lambda.paramValue0 = EditorGUILayout.FloatField("参数0", lambda.paramValue0, GUILayout.Width(200));
                        lambda.paramValue1 = EditorGUILayout.FloatField("参数1", lambda.paramValue1, GUILayout.Width(200));
                    }
                }

                if (GUILayout.Button("删除", GUILayout.Width(40)))
                {
                    if (EditorUtility.DisplayDialog("确认删除", "确定要删除该表达式项吗？", "删除", "取消"))
                    {
                        removeIndex = i;
                    }
                }

                EditorGUILayout.EndHorizontal();

                // 递归括号
                if (lambda.type == EAttrFormulaType.eBracket)
                {
                    EditorGUILayout.LabelField("括号内容：", GUILayout.Width(80));
                    DrawLambdaList(lambda.subLambda, indent + 1);
                    // 括号内添加表达式入口已存在
                }

                EditorGUILayout.EndVertical();
                list[i] = lambda;
            }

            if (removeIndex >= 0)
            {
                list.RemoveAt(removeIndex);
            }

            // ★★★ 这里加“添加表达式”按钮（每一层都能加） ★★★
            if (GUILayout.Button("添加表达式", GUILayout.Width(120)))
            {
                list.Add(new LambdaParam() { type = EAttrFormulaType.eAdd });
            }
        }
        //-----------------------------------------------------
        private string[] GetAttrNames(out int curIndex, int curAttrId)
        {
            curIndex = 0;
            if (m_Data.vAttributes == null || m_Data.vAttributes.Length == 0)
                return new string[] { "无属性" };
            string[] names = new string[m_Data.vAttributes.Length];
            for (int i = 0; i < m_Data.vAttributes.Length; ++i)
            {
                names[i] = $"{m_Data.vAttributes[i].attr}:{m_Data.vAttributes[i].name}";
                if (m_Data.vAttributes[i].attr == curAttrId)
                    curIndex = i;
            }
            return names;
        }
        //-----------------------------------------------------
        private List<LambdaParam> FormulaTextToLambdaList(string formula)
        {
            var tokens = TokenizeFormula(formula);
            int pos = 0;
            var lambdaList = ParseExpression(tokens, ref pos);
            return lambdaList;
        }
        //-----------------------------------------------------
        private string LambdaListToFormulaText(List<LambdaParam> vLambda)
        {
            if (vLambda == null || vLambda.Count == 0)
                return "";

            Stack<string> stack = new Stack<string>();
            foreach (var lambda in vLambda)
            {
                switch (lambda.type)
                {
                    case EAttrFormulaType.eAdd:
                    case EAttrFormulaType.eSub:
                    case EAttrFormulaType.eMul:
                    case EAttrFormulaType.eDiv:
                        {
                            string op = "";
                            switch (lambda.type)
                            {
                                case EAttrFormulaType.eAdd: op = "+"; break;
                                case EAttrFormulaType.eSub: op = "-"; break;
                                case EAttrFormulaType.eMul: op = "*"; break;
                                case EAttrFormulaType.eDiv: op = "/"; break;
                            }
                            if (lambda.isUnary)
                            {
                                string leftA = stack.Count > 0 ? stack.Pop() : "0";
                                string rightB = lambda.paramValue0.ToString();
                                stack.Push($"({leftA} {op} {rightB})");
                            }
                            else
                            {
                                string b = stack.Count > 0 ? stack.Pop() : lambda.paramValue0.ToString();
                                string a = stack.Count > 0 ? stack.Pop() : lambda.paramValue1.ToString();
                                stack.Push($"({a} {op} {b})");
                            }
                            break;
                        }
                    case EAttrFormulaType.ePower:
                    case EAttrFormulaType.eMin:
                    case EAttrFormulaType.eMax:
                        {
                            string op = "";
                            switch (lambda.type)
                            {
                                case EAttrFormulaType.ePower: op = "^"; break;
                                case EAttrFormulaType.eMin: op = "min"; break;
                                case EAttrFormulaType.eMax: op = "max"; break;
                            }
                            string b = stack.Count > 0 ? stack.Pop() : lambda.paramValue0.ToString();
                            string a = stack.Count > 0 ? stack.Pop() : lambda.paramValue1.ToString();
                            if (op == "min" || op == "max")
                                stack.Push($"{op}({a},{b})");
                            else
                                stack.Push($"({a} {op} {b})");
                            break;
                        }
                    case EAttrFormulaType.eFloor:
                        {
                            string a = stack.Count > 0 ? stack.Pop() : lambda.paramValue0.ToString();
                            stack.Push($"floor({a})");
                            break;
                        }
                    case EAttrFormulaType.eCeil:
                        {
                            string a = stack.Count > 0 ? stack.Pop() : lambda.paramValue0.ToString();
                            stack.Push($"ceil({a})");
                            break;
                        }
                    case EAttrFormulaType.eAbs:
                        {
                            string a = stack.Count > 0 ? stack.Pop() : lambda.paramValue0.ToString();
                            stack.Push($"abs({a})");
                            break;
                        }
                    case EAttrFormulaType.eRandom:
                        {
                            stack.Push($"rand({lambda.paramValue0},{lambda.paramValue1})");
                            break;
                        }
                    case EAttrFormulaType.eBracket:
                        {
                            string inner = LambdaListToFormulaText(lambda.subLambda);
                            stack.Push($"({inner})");
                            break;
                        }
                    case EAttrFormulaType.eActorAttr:
                        {
                            string camp = ((int)lambda.paramValue0 == 0) ? "攻击方" : "受击方";
                            string attrName = GetAttrNameById((int)lambda.paramValue1);
                            stack.Push($"{camp}.{attrName}");
                            break;
                        }
                    case EAttrFormulaType.eNone:
                    default:
                        stack.Push(lambda.paramValue0.ToString());
                        break;
                }
            }
            return stack.Count > 0 ? stack.Pop() : "";
        }
        //-----------------------------------------------------
        private List<LambdaParam> ParseExpression(List<string> tokens, ref int pos)
        {
            var output = new List<LambdaParam>();
            Stack<string> opStack = new Stack<string>();
            Stack<List<LambdaParam>> lambdaStack = new Stack<List<LambdaParam>>();
            lambdaStack.Push(new List<LambdaParam>());

            while (pos < tokens.Count)
            {
                string token = tokens[pos];

                // 数字
                if (float.TryParse(token, out float num))
                {
                    lambdaStack.Peek().Add(new LambdaParam { type = EAttrFormulaType.eNone, paramValue0 = num });
                    pos++;
                }
                // 属性（攻击方.攻击/受击方.防御等）
                else if (token == "攻击方" || token == "受击方" || token == "攻方" || token == "受方")
                {
                    string camp = token;
                    pos++;
                    if (pos < tokens.Count && tokens[pos] == ".")
                    {
                        pos++;
                        if (pos < tokens.Count)
                        {
                            string attrName = tokens[pos];
                            pos++;
                            int attrId = GetAttrIdByName(attrName);
                            lambdaStack.Peek().Add(new LambdaParam
                            {
                                type = EAttrFormulaType.eActorAttr,
                                paramValue0 = (camp == "攻击方" || camp == "攻方") ? 0 : 1,
                                paramValue1 = attrId
                            });
                        }
                    }
                }
                // 函数
                else if (token.Equals("min", System.StringComparison.OrdinalIgnoreCase) || 
                    token.Equals("max", System.StringComparison.OrdinalIgnoreCase) || 
                    token.Equals("rand", System.StringComparison.OrdinalIgnoreCase) || 
                    token.Equals("floor", System.StringComparison.OrdinalIgnoreCase) || 
                    token.Equals("ceil", System.StringComparison.OrdinalIgnoreCase) || 
                    token.Equals("abs", System.StringComparison.OrdinalIgnoreCase))
                {
                    EAttrFormulaType funcType = EAttrFormulaType.eNone;
                    string tempToken = token.ToLower();
                    switch (tempToken)
                    {
                        case "min": funcType = EAttrFormulaType.eMin; break;
                        case "max": funcType = EAttrFormulaType.eMax; break;
                        case "rand": funcType = EAttrFormulaType.eRandom; break;
                        case "floor": funcType = EAttrFormulaType.eFloor; break;
                        case "ceil": funcType = EAttrFormulaType.eCeil; break;
                        case "abs": funcType = EAttrFormulaType.eAbs; break;
                    }
                    pos++;
                    if (pos < tokens.Count && tokens[pos] == "(")
                    {
                        pos++;
                        var args = new List<float>();
                        while (pos < tokens.Count && tokens[pos] != ")")
                        {
                            if (float.TryParse(tokens[pos], out float arg))
                            {
                                args.Add(arg);
                                pos++;
                            }
                            else if (tokens[pos] == ",")
                            {
                                pos++;
                            }
                            else
                            {
                                // 支持嵌套表达式
                                var subLambdas = ParseExpression(tokens, ref pos);
                                if (subLambdas.Count > 0)
                                {
                                    lambdaStack.Peek().AddRange(subLambdas);
                                }
                            }
                        }
                        pos++; // skip ')'
                               // 只支持2参数
                        lambdaStack.Peek().Add(new LambdaParam
                        {
                            type = funcType,
                            paramValue0 = args.Count > 0 ? args[0] : 0,
                            paramValue1 = args.Count > 1 ? args[1] : 0
                        });
                    }
                }
                // 括号
                else if (token == "(")
                {
                    pos++;
                    var subLambdas = ParseExpression(tokens, ref pos);
                    lambdaStack.Peek().Add(new LambdaParam
                    {
                        type = EAttrFormulaType.eBracket,
                        subLambda = subLambdas
                    });
                }
                else if (token == ")")
                {
                    pos++;
                    break;
                }
                // 运算符
                else if (IsOperator(token))
                {
                    // 处理一元/二元
                    EAttrFormulaType opType = GetOpType(token);
                    bool isUnary = false;
                    // 判断一元：前面是(或开头或运算符
                    if (lambdaStack.Peek().Count == 0 ||
                        (pos > 0 && (tokens[pos - 1] == "(" || IsOperator(tokens[pos - 1]))))
                    {
                        isUnary = true;
                    }
                    pos++;
                    // 读取右操作数
                    if (isUnary)
                    {
                        // 右操作数
                        if (pos < tokens.Count && float.TryParse(tokens[pos], out float right))
                        {
                            lambdaStack.Peek().Add(new LambdaParam
                            {
                                type = opType,
                                isUnary = true,
                                paramValue0 = right
                            });
                            pos++;
                        }
                        else if (pos < tokens.Count && tokens[pos] == "(")
                        {
                            var subLambdas = ParseExpression(tokens, ref pos);
                            lambdaStack.Peek().Add(new LambdaParam
                            {
                                type = opType,
                                isUnary = true,
                                subLambda = subLambdas
                            });
                        }
                    }
                    else
                    {
                        // 二元，先处理左侧（已在栈），再处理右侧
                        if (pos < tokens.Count && float.TryParse(tokens[pos], out float right))
                        {
                            var last = lambdaStack.Peek()[lambdaStack.Peek().Count - 1];
                            lambdaStack.Peek().RemoveAt(lambdaStack.Peek().Count - 1);
                            lambdaStack.Peek().Add(new LambdaParam
                            {
                                type = opType,
                                isUnary = false,
                                paramValue0 = right,
                                paramValue1 = last.paramValue0
                            });
                            pos++;
                        }
                        else if (pos < tokens.Count && tokens[pos] == "(")
                        {
                            var subLambdas = ParseExpression(tokens, ref pos);
                            var last = lambdaStack.Peek()[lambdaStack.Peek().Count - 1];
                            lambdaStack.Peek().RemoveAt(lambdaStack.Peek().Count - 1);
                            lambdaStack.Peek().Add(new LambdaParam
                            {
                                type = opType,
                                isUnary = false,
                                subLambda = subLambdas,
                                paramValue1 = last.paramValue0
                            });
                        }
                    }
                }
                else
                {
                    pos++;
                }
            }
            return lambdaStack.Pop();
        }
        //-----------------------------------------------------
        private bool IsOperator(string token)
        {
            return token == "+" || token == "-" || token == "*" || token == "/";
        }
        //-----------------------------------------------------
        private EAttrFormulaType GetOpType(string token)
        {
            switch (token)
            {
                case "+": return EAttrFormulaType.eAdd;
                case "-": return EAttrFormulaType.eSub;
                case "*": return EAttrFormulaType.eMul;
                case "/": return EAttrFormulaType.eDiv;
                default: return EAttrFormulaType.eNone;
            }
        }
        //-----------------------------------------------------
        private int GetAttrIdByName(string name)
        {
            if (m_Data?.vAttributes == null) return 0;
            foreach (var attr in m_Data.vAttributes)
            {
                if (attr.name == name)
                    return attr.attr;
            }
            return 0;
        }
        //-----------------------------------------------------
        // 分词
        private List<string> TokenizeFormula(string formula)
        {
            var tokens = new List<string>();
            var pattern = @"(\d+(\.\d+)?|[\+\-\*/\^\(\),]|min|max|rand|floor|ceil|abs|攻击方|受击方|[A-Za-z_][A-Za-z0-9_]*|\.)";
            foreach (Match m in Regex.Matches(formula, pattern))
            {
                if (!string.IsNullOrWhiteSpace(m.Value))
                    tokens.Add(m.Value);
            }
            return tokens;
        }
        //-----------------------------------------------------
        private string GetAttrNameById(int attrId)
        {
            if (m_Data?.vAttributes == null) return attrId.ToString();
            foreach (var attr in m_Data.vAttributes)
            {
                if (attr.attr == attrId)
                    return attr.name;
            }
            return attrId.ToString();
        }
    }
}
#endif