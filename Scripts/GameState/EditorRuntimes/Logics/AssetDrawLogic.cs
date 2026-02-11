/********************************************************************
生成日期:	11:07:2025
类    名: 	AssetDrawLogic
作    者:	HappLI
描    述:	资源面板逻辑
*********************************************************************/
#if UNITY_EDITOR
using Framework.ED;
using Framework.State.Runtime;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Framework.State.Editor
{
    [EditorBinder(typeof(GameWorldEditor), "AssetRect", -1)]
    public class AssetDrawLogic : AStateEditorLogic
    {
        public class GameElementItem : Framework.ED.TreeAssetView.ItemData
        {
            public IGameWorldItem bindData;
            private System.Reflection.FieldInfo m_NameField;
            Texture2D m_pIcon = null;
            public override Color itemColor()
            { 
                return Color.white;
            }
            public override Texture2D itemIcon()
            {
                if(bindData!=null && bindData.GetType().IsDefined(typeof(StateIconAttribute)))
                {
                    var iconAttr = bindData.GetType().GetCustomAttribute<StateIconAttribute>();
                    if(iconAttr!=null &&!string.IsNullOrEmpty(iconAttr.name))
                    {
                        m_pIcon=Framework.ED.EditorUtils.LoadEditorResource<Texture2D>(iconAttr.name);
                    }
                }
                return m_pIcon;
            }
            public string displayName
            {
                get
                {
                    if(m_NameField == null && bindData!=null)
                    {
                        m_NameField = bindData.GetType().GetField("name", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.IgnoreCase);
                    }
                    if (m_NameField == null)
                        return this.name;
                    var nameVal = m_NameField.GetValue(bindData);
                    if (nameVal != null) this.name = nameVal.ToString();
                    return this.name;
                }
            }
        }
        public enum ETab
        {
            GameElement,
            AgentLibrary,
        }
        static string[] TABS = new string[] { "游戏元素", "Agent图书馆" };
        ETab m_eTab = ETab.GameElement;

        Framework.ED.TreeAssetView m_pGameEleTree = null;
        Framework.ED.TreeAssetView.ItemData m_pSelectGameElement = null;
        //--------------------------------------------------------
        protected override void OnEnable()
        {
            m_pGameEleTree = new TreeAssetView(new string[] { "" });
            m_pGameEleTree.SetRowHeight(30);
            m_pGameEleTree.buildMutiColumnDepth = true;
            m_pGameEleTree.ShowAlternatingRowBackgrounds(false);
            m_pGameEleTree.Reload();
            m_pGameEleTree.OnCellDraw += OnGameElementDraw;
            m_pGameEleTree.OnSelectChange += OnGameElementSelect;
            m_pGameEleTree.OnItemDoubleClick += OnGameElementSelect;
            RefreshGameElements();
        }
        //--------------------------------------------------------
        protected override void OnGUI()
        {
            Rect rect = GetRect();

            GUILayout.BeginArea(new Rect(rect.x, rect.y, rect.width, 20));

            GUILayout.BeginHorizontal();
            Color color = GUI.color;
            for (int i = 0; i < TABS.Length; ++i)
            {
                GUI.color = (m_eTab == (ETab)i) ? Color.yellow : color;
                if (GUILayout.Button(TABS[i]))
                {
                    if (m_eTab != (ETab)i)
                    {
                        m_eTab = (ETab)i;
                        OnTabChange();
                    }
                }
                GUI.color = color;
            }
            GUILayout.EndHorizontal();

            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(rect.x, rect.y+20, rect.width, rect.height));
            switch(m_eTab)
            {
                case ETab.GameElement:
                    DrawGameElement(rect);
                    break;
                case ETab.AgentLibrary:
                    DrawGameAgnets(rect);
                    break;
            }
            GUILayout.EndArea();
            UIDrawUtils.DrawColorLine(new Vector2(rect.xMin, rect.y+20 ), new Vector2(rect.xMax, rect.y + 20), new Color(1, 1, 1, 0.5f));
        }
        //--------------------------------------------------------
        void OnTabChange()
        {
            switch (m_eTab)
            {
                case ETab.GameElement:
                    RefreshGameElements();
                    break;
                case ETab.AgentLibrary:
                    break;
            }
        }
        //--------------------------------------------------------
        public override void OnChangeSelect(object pData)
        {
            base.OnChangeSelect(pData);
            RefreshGameElements();
        }
        //--------------------------------------------------------
        void DrawGameElement(Rect rect)
        {
            if (m_pGameEleTree != null)
            {
                m_pGameEleTree.GetColumn(0).width = rect.width-5;
                m_pGameEleTree.OnGUI(new Rect(0, 0, rect.width, rect.height - 30));
            }
            var worldObj = GetObject<AGameWorldObject>();
            if (worldObj == null)
                return;
        }
        //--------------------------------------------------------
        void RefreshGameElements()
        {
            var worldObj = GetWorldData();
            m_pGameEleTree.BeginTreeData();
            if(worldObj!=null)
                m_pGameEleTree.AddData(new GameElementItem() { id = 0, bindData = worldObj.gameStateData, name = worldObj.gameStateData.name });
            else
                m_pGameEleTree.AddData(new GameElementItem() { id = 0, bindData = new GameStateData() { name="游戏状态" }, name = "游戏状态" });
            m_pGameEleTree.EndTreeData();
        }
        //--------------------------------------------------------
        bool OnGameElementDraw(Framework.ED.TreeAssetView.RowArgvData rowData)
        {
            if(rowData.column ==0)
            {
                GameElementItem item = rowData.itemData.data as GameElementItem;
                float iconSize = rowData.rowRect.height;
                if (rowData.itemData.icon == null)
                    rowData.itemData.icon = item.itemIcon();
                if (rowData.itemData.icon != null)
                {
                    GUI.DrawTexture(new Rect(rowData.rowRect.xMin + 5, rowData.rowRect.y + (rowData.rowRect.height - iconSize) / 2, iconSize, iconSize), rowData.itemData.icon);
                }
                EditorGUI.LabelField(new Rect(rowData.rowRect.xMin + iconSize+10, rowData.rowRect.y, rowData.rowRect.width- iconSize-10, rowData.rowRect.height), item.displayName);
                return true;
            }
            return false;
        }
        //--------------------------------------------------------
        void OnGameElementSelect(Framework.ED.TreeAssetView.ItemData itemData)
        {
            var gameItem = itemData as GameElementItem;
            m_pSelectGameElement = gameItem;
            GetOwner<GameWorldEditor>()?.OnGameItemSelected(gameItem.bindData);
        }
        //--------------------------------------------------------
        void DrawGameAgnets(Rect rect)
        {

        }
    }
}

#endif