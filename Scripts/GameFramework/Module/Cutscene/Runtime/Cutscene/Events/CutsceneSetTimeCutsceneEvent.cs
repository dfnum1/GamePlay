/********************************************************************
生成日期:	06:30:2025
类    名: 	CutsceneSetTimeCutsceneEvent
作    者:	HappLI
描    述:	设置播放位置
*********************************************************************/
using Framework.AT.Runtime;
using Framework.DrawProps;

namespace Framework.Cutscene.Runtime
{
    [System.Serializable, CutsceneEvent("Cutscene/设置播放位置")]
    public class CutsceneSetTimeCutsceneEvent : ACutsceneStatusCutsceneEvent
    {
        [Display("跳到的时间")] public float setTime;
        //-----------------------------------------------------
        public override ushort GetIdType()
        {
            return (ushort)EEventType.eSetTimeCutscene;
        }
    }
}