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
                if (bindData is GameStateLogicData)
                {
                    GameStateLogicData item = bindData as GameStateLogicData;
                    if (item.GetType().IsDefined(typeof(StateIconAttribute)))
                    {
                        var iconAttr = item.GetType().GetCustomAttribute<StateIconAttribute>();
                        if (iconAttr != null && !string.IsNullOrEmpty(iconAttr.name))
                        {
                            m_pIcon = Framework.ED.EditorUtils.LoadEditorResource<Texture2D>(iconAttr.name);
                        }
                    }
                    if (m_pIcon == null)
                    {
                        if(StateEditorUtil.IsStateWorldType<AStateLogic>(item.logicType))
                            m_pIcon = Framework.ED.EditorUtils.LoadEditorResource<Texture2D>("GameWorld/statelogic.png");
                        else
                            m_pIcon = Framework.ED.EditorUtils.LoadEditorResource<Texture2D>("GameWorld/modelogic.png");
                    }
                }
                else
                {
                    if (bindData != null && bindData.GetType().IsDefined(typeof(StateIconAttribute)))
                    {
                        var iconAttr = bindData.GetType().GetCustomAttribute<StateIconAttribute>();
                        if (iconAttr != null && !string.IsNullOrEmpty(iconAttr.name))
                        {
                            m_pIcon = Framework.ED.EditorUtils.LoadEditorResource<Texture2D>(iconAttr.name);
                        }
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

        bool m_bItemRightPop = false;
        Framework.ED.TreeAssetView m_pGameEleTree = null;
        Framework.ED.TreeAssetView.ItemData m_pSelectGameElement = null;
        //--------------------------------------------------------
        protected override void OnEnable()
        {
            m_pGameEleTree = new TreeAssetView(new string[] { "" });
            m_pGameEleTree.SetRowHeight(25);
            m_pGameEleTree.buildMutiColumnDepth = true;
            m_pGameEleTree.DepthIndentWidth = 10;
            m_pGameEleTree.ShowAlternatingRowBackgrounds(false);
            m_pGameEleTree.Reload();
            m_pGameEleTree.OnCellDraw += OnGameElementDraw;
            m_pGameEleTree.OnSelectChange += OnGameElementSelect;
            m_pGameEleTree.OnItemDoubleClick += OnGameElementSelect;
            m_pGameEleTree.OnItemRightClick += OnGameElementRightClick;
            m_pGameEleTree.OnViewRightClick += OnGameViewRightClick;
            RefreshGameElements();
        }

        //--------------------------------------------------------
        protected override void OnGUI()
        {
            Rect rect = GetRect();
            if (rect.width <= 20)
                return;

            GUILayout.BeginArea(new Rect(rect.x, rect.y, rect.width, 20));

            GUILayout.BeginHorizontal(GUILayout.Width(rect.width-20));
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
        public override void OnSaveChanges()
        {
            base.OnSaveChanges();
            if (GetWorldData() == null)
                return;
            GetWorldData().Serialize();
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
        public override void OnRefreshData(object pData)
        {
            base.OnRefreshData(pData);
            RefreshGameElements();

            var worldObj = GetObject<AGameWorldObject>();
            if (worldObj == null || worldObj.gameWorldData == null || worldObj.gameWorldData.gameLevel== null)
                return;

            var cfgData = worldObj.gameWorldData.gameLevel.GetGameData<AGameCfgData>(GetFramework());
            if (cfgData != null)
            {
                if (cfgData.GetEditor() != null)
                {
                    cfgData.GetEditor(GetOwner()).OnRefreshData(pData);
                }
            }
        }
        //--------------------------------------------------------
        void RefreshGameElements()
        {
            if (m_pGameEleTree == null)
                return;

            var worldObj = GetWorldData();
            m_pGameEleTree.BeginTreeData();
            if (worldObj != null)
            {
                int id = 0;
                if(worldObj.atData !=null)
                {
                    m_pGameEleTree.AddData(new GameElementItem() { id = ++id, bindData = worldObj.atData, name = "逻辑蓝图", depth = 0 });
                }
                m_pGameEleTree.AddData(new GameElementItem() { id = ++id, bindData = worldObj.gameStateData, name = worldObj.gameStateData.name, depth =0 });
                if(worldObj.modeDatas!=null)
                {
                    foreach(var db in worldObj.modeDatas)
                    {
                        m_pGameEleTree.AddData(new GameElementItem() { id = ++id, bindData = db, name = db.name, depth = 1 });
                        if(db.modeLogics!=null)
                        {
                            foreach(var logic in db.modeLogics)
                            {
                                m_pGameEleTree.AddData(new GameElementItem() { id = ++id, bindData = logic, name = StateEditorUtil.GetStateWorldTypeName(logic.logicType), depth = 2 });
                            }
                        }
                    }
                }
                if (worldObj.gameStateData.stateLogics != null)
                {
                    foreach (var logic in worldObj.gameStateData.stateLogics)
                    {
                        m_pGameEleTree.AddData(new GameElementItem() { id = ++id, bindData = logic, name = StateEditorUtil.GetStateWorldTypeName(logic.logicType), depth = 1 });
                    }
                }
                if (worldObj.gameLevel == null)
                {
                    worldObj.gameLevel = new GameLevelData();
                    worldObj.gameLevel.name = "游戏玩法配置数据";
                }
                m_pGameEleTree.AddData(new GameElementItem() { id = ++id, bindData = worldObj.gameLevel, name = worldObj.gameLevel.name, depth = 0 });
            }
            else
                m_pGameEleTree.AddData(new GameElementItem() { id = 0, bindData = new GameStateData() { name = "游戏状态" }, name = "游戏状态" });
            m_pGameEleTree.EndTreeData();
        }
        //--------------------------------------------------------
        bool OnGameElementDraw(Framework.ED.TreeAssetView.RowArgvData rowData)
        {
            if(rowData.column ==0)
            {
                GameElementItem item = rowData.itemData.data as GameElementItem;
                float iconSize = rowData.rowRect.height;
                
                if(item.bindData!=null && item.bindData is GameStateLogicData)
                {
                    EditorGUI.BeginDisabledGroup(!((GameStateLogicData)item.bindData).enabled);
                }

                if (rowData.itemData.icon == null)
                    rowData.itemData.icon = item.itemIcon();
                if (rowData.itemData.icon != null)
                {
                    GUI.DrawTexture(new Rect(rowData.rowRect.xMin + 5, rowData.rowRect.y + (rowData.rowRect.height - iconSize) / 2, iconSize, iconSize), rowData.itemData.icon);
                }
                EditorGUI.LabelField(new Rect(rowData.rowRect.xMin + iconSize+10, rowData.rowRect.y, rowData.rowRect.width- iconSize-10, rowData.rowRect.height), item.displayName);
                if (item.bindData != null && item.bindData is GameStateLogicData)
                {
                    EditorGUI.EndDisabledGroup();
                }
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
        void OnGameViewRightClick()
        {
            if (m_bItemRightPop)
                return;
            PopMenu(null);
            m_bItemRightPop = false;
        }
        //--------------------------------------------------------
        void OnGameElementRightClick(Framework.ED.TreeAssetView.ItemData itemData)
        {
            m_bItemRightPop = true;
            var gameItem = itemData as GameElementItem;
            PopMenu(gameItem.bindData);
        }
        //--------------------------------------------------------
        void DrawGameAgnets(Rect rect)
        {

        }
        //--------------------------------------------------------
        void PopMenu(System.Object bindData)
        {
            GenericMenu menu = new GenericMenu();
            if(bindData!=null)
            {
                if (bindData is GameStateData)
                {
                    //! 弹出右键菜单创建游戏模式
                    menu.AddItem(new GUIContent("添加玩法模式"), false, (menuData) => {
                        MenuContextData data = (MenuContextData)menuData;
                        GameModeTypeProvider.PopSearch(data.mousePosition, (cls, index) =>
                        {
                            var worldData = GetWorldData();
                            if (worldData.modeDatas == null)
                            {
                                worldData.modeDatas = new System.Collections.Generic.List<GameStateModeData>();
                            }
                            bool bHasMode = false;
                            for (int i = 0; i < worldData.modeDatas.Count; ++i)
                            {
                                if (worldData.modeDatas[i].modeType == cls)
                                {
                                    bHasMode = true;
                                    break;
                                }
                            }
                            if (!bHasMode)
                            {
                                UndoRegister(true);
                                GameStateModeData modeData = new GameStateModeData();
                                modeData.modeType = cls;
                                modeData.name = StateEditorUtil.GetStateWorldTypeName(cls);
                                worldData.modeDatas.Add(modeData);
                                RefreshGameElements();
                            }
                        }, -1);
                    }, new MenuContextData(bindData, Event.current.mousePosition));

                }
                if (bindData is GameStateModeData)
                {
                    //! 弹出右键菜单创建游戏模式
                    menu.AddItem(new GUIContent("删除模式"), false, (menuData) => {
                        MenuContextData data = (MenuContextData)menuData;
                        if (data.bindData != null)
                        {
                            var worldData = GetWorldData();
                            if (worldData.modeDatas != null)
                            {
                                if (EditorUtility.DisplayDialog("提示", "确定是否删除该玩法模式", "删除", "再想想"))
                                {
                                    UndoRegister(true);
                                    //! 添加undo
                                    GameStateModeData gameData = data.bindData as GameStateModeData;
                                    worldData.modeDatas.Remove(gameData);
                                    RefreshGameElements();
                                }
                            }
                        }
                    }, new MenuContextData(bindData, Event.current.mousePosition));
                }
                if (bindData is GameStateLogicData)
                {
                    //! 弹出右键菜单创建游戏模式
                    menu.AddItem(new GUIContent("删除逻辑"), false, (menuData) => {
                        MenuContextData data = (MenuContextData)menuData;
                        if (data.bindData != null)
                        {
                            var worldData = GetWorldData();
                            if (worldData.modeDatas != null)
                            {
                                if (EditorUtility.DisplayDialog("提示", "确定是否删除该玩法模式", "删除", "再想想"))
                                {
                                    GameStateLogicData logic = data.bindData as GameStateLogicData;
                                    if(worldData.gameStateData.stateLogics!=null && worldData.gameStateData.stateLogics.Contains(logic))
                                    {
                                        UndoRegister(true);
                                        worldData.gameStateData.stateLogics.Remove(logic);
                                        RefreshGameElements();
                                    }
                                    if (worldData.modeDatas != null && worldData.gameStateData.stateLogics.Contains(logic))
                                    {
                                        foreach(var mode in worldData.modeDatas)
                                        {
                                            if(mode.modeLogics!=null && mode.modeLogics.Contains(logic))
                                            {
                                                UndoRegister(true);
                                                mode.modeLogics.Remove(logic);
                                                RefreshGameElements();
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }, new MenuContextData(bindData, Event.current.mousePosition));
                }
            }
            menu.AddItem(new GUIContent("刷新"), false, () => {
                RefreshGameElements();
            });
            menu.ShowAsContext();
        }
    }
}

#endif