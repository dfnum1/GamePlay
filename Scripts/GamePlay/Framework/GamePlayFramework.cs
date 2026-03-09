/********************************************************************
生成日期:	11:06:2020
类    名: 	GamePlayFramework
作    者:	HappLI
描    述:	GamePlay框架类
*********************************************************************/
using Framework.AT.Runtime;
using Framework.State.Runtime;

namespace Framework.Core
{
    //-----------------------------------------------------
    public class GamePlayFramework : AFramework
    {
        //--------------------------------------------------------
        protected override void OnInit()
        {
            AddModule<Framework.State.Runtime.GameWorld>();
            AddModule<Framework.Db.UserManager>();
 //           ATRegisterInternalHandler.Init();
            GameStateInnerTypeRegistry.Init();
        }
        //--------------------------------------------------------
        protected override void OnAwake()
        {
        }
        //--------------------------------------------------------
        protected override void OnDestroy()
        {
        }
        //--------------------------------------------------------
        protected override void OnStart()
        {
        }
    }
}
