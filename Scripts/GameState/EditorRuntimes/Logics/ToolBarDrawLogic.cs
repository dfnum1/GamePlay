/********************************************************************
生成日期:	11:07:2025
类    名: 	ToolBarDrawLogic
作    者:	HappLI
描    述:	工具栏绘制逻辑
*********************************************************************/
#if UNITY_EDITOR
using Framework.ED;
using Framework.State.Runtime;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

namespace Framework.State.Editor
{
    [EditorBinder(typeof(GameWorldEditor), "ToolBarRect")]
    public class ToolBarDrawLogic : AEditorLogic
    {
        struct ToolBar
        {
            public string name;
            public AGameEditor.OnToolBarMenu menu;
            public System.Object callParam;
        }
        List<ToolBar> m_vToolBar = new List<ToolBar>();
        //--------------------------------------------------------
        public void AddToolBar(string name, AGameEditor.OnToolBarMenu menu, System.Object callParam = null)
        {
            if (string.IsNullOrEmpty(name) || menu == null)
                return;
            for(int i =0; i < m_vToolBar.Count; ++i)
            {
                var menuTmp = m_vToolBar[i];
                if (m_vToolBar[i].name.CompareTo(name) ==0)
                {
                    menuTmp.menu = menu;
                    menuTmp.callParam = callParam;
                    m_vToolBar[i] = menuTmp;
                    return;
                }
            }
            m_vToolBar.Add(new ToolBar() { name = name, menu = menu, callParam = callParam });
        }
        //--------------------------------------------------------
        public void RemoveToolBar(string name)
        {
            if (string.IsNullOrEmpty(name))
                return;
            for (int i = 0; i < m_vToolBar.Count; ++i)
            {
                var menuTmp = m_vToolBar[i];
                if (m_vToolBar[i].name.CompareTo(name) == 0)
                {
                    m_vToolBar.RemoveAt(i);
                    return;
                }
            }
        }
        //--------------------------------------------------------
        protected override void OnGUI()
        {
            GUILayout.BeginHorizontal();
            if(GUILayout.Button("创建", new GUILayoutOption[] { GUILayout.Width(80) }))
            {
                string savePath = EditorUtility.SaveFilePanelInProject("创建游戏世界对象", "GameWorld", "asset", "", Application.dataPath);
                if (string.IsNullOrEmpty(savePath))
                {
                    return;
                }
                AGameWorldObject projData = Framework.ED.EditorUtils.CreateUnityScriptObject<AGameWorldObject>();
                projData.name = "GameWorld";
                AssetDatabase.CreateAsset(projData, savePath);
                EditorUtility.SetDirty(projData);
                AssetDatabase.SaveAssetIfDirty(projData);
                AGameWorldObject battleObj = AssetDatabase.LoadAssetAtPath<AGameWorldObject>(savePath);
                GetOwner().OnChangeSelect(battleObj);
            }
            if(GUILayout.Button("保存", new GUILayoutOption[] { GUILayout.Width(80) }))
            {
                GetOwner().SaveChanges();
            }
            foreach(var db in m_vToolBar)
            {
                if (GUILayout.Button(db.name, new GUILayoutOption[] { GUILayout.Width(GUI.skin.label.CalcSize(new GUIContent(db.name)).x + 10) }))
                {
                    db.menu.Invoke(db.callParam);
                }
            }
            if (GUILayout.Button("文档说明", new GUILayoutOption[] { GUILayout.Width(80) }))
            {
                Application.OpenURL("https://docs.qq.com/doc/DTHpDaENHVUJGeVB6");
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginArea(new Rect(GetRect().width - 120,0, 120, GetRect().height));
            if (GUILayout.Button("编辑器偏好设置", new GUILayoutOption[] { GUILayout.Width(120) }))
            {
                EditorPreferences.OpenUserPreferences();
            }
            GUILayout.EndArea();
        }
    }
}

#endif