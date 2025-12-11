/********************************************************************
生成日期:	06:30:2025
类    名: 	EActionType
作    者:	HappLI
描    述:	内置的行为类型
*********************************************************************/
using Framework.AT.Runtime;

namespace Framework.Cutscne.Runtime
{
    //-----------------------------------------------------
    [ATType("Cutscene系统")]
    public enum ECutsceneATTaskType
    {
        [ATAction("回调/开始播放回调", true, false, true)]
        [Return("实例id", typeof(int))]
        [Return("配置id", typeof(int))]
        eCutscenePlayablePlayedCallback = 50,

        [ATAction("回调/停止播放回调", true, false, true)]
        [Return("实例id", typeof(int))]
        [Return("配置id", typeof(int))]
        eCutscenePlayableStopedCallback,

        [ATAction("回调/暂停播放回调", true, false, true)]
        [Return("实例id", typeof(int))]
        [Return("配置id", typeof(int))]
        eCutscenePlayablePauseCallback,

        [ATAction("回调/继续播放回调", true, false, true)]
        [Return("实例id", typeof(int))]
        [Return("配置id", typeof(int))]
        eCutscenePlayableResumeCallback,
    }
    //-----------------------------------------------------
    [ATType("Cutscene系统")]
    public enum ECutsceneATActionType
    {
        [DrawProps.Disable]eActionBegin = 101,

        [ATAction("播放")]
        [Argv("过场id", typeof(ushort), true)]
        [Return("实例Id", typeof(int))]
        ePlaySubCutscene = eActionBegin + 100, //播放动画

        [ATAction("暂停")]
        [Argv("实例Id","当为0时，表示暂停所有当前cutscene正在播放的过场", typeof(int), true)]
        ePauseSubCutscene,

        [ATAction("继续播放")]
        [Argv("实例Id", "当为0时，表示继续所有当前cutscene正在播放的过场", typeof(int), true)]
        eResumeSubCutscene,

        [ATAction("停止")]
        [Argv("实例Id", "当为0时，表示停止所有当前cutscene正在播放的过场", typeof(int), true)]
        eStopSubCutscene,

        [ATAction("跳到指定位置开始播")]
        [Argv("实例Id", "当为0时，表示操作所有当前cutscene正在播放的过场", typeof(int), true)]
        [Argv("播放位置", "", typeof(float), true)]
        eSeekSubCutscene,

        [ATAction("轨道数据绑定")]
        [Argv("实例Id", "", typeof(int), true)]
        [Argv("轨道", "", typeof(int), true)]
        [Argv("数据", "", typeof(IVariable), true)]
        eBindCutsceneTrackData,
    }
}