/********************************************************************
生成日期:	06:30:2025
类    名: 	ABaseEvent
作    者:	HappLI
描    述:	事件接口
*********************************************************************/

using Framework.DrawProps;
namespace Framework.Cutscene.Runtime
{
    public enum EEventType
    {
        eCustom = 1,
        eSetATPortData = 2,
        eExecuteATNode = 3,
        eSetGameCamera = 4,
        ePlayCutscene = 5,
        eStopCutscene = 6,
        ePauseCutscene = 7,
        eResumeCutscene = 8,
        eSetTimeCutscene = 9,
        eCutsomBegin = 1000,
    }
    //-----------------------------------------------------
    public enum EEventFlag
    {
        [Display("关闭时还未触发过的时将强制触发")]eStopFireIfNoTrigger = 1<<0,
        [Display("设置时间小于触发时间将重置触发状态")] eLessTimeReTrigger = 1<<1,
    }
    //-----------------------------------------------------
    [System.Serializable]
    public struct BaseEventProp
    {
        [DefaultValue("")] public string name; //剪辑名称
        [DefaultValue(0),UnEdit] public float time; //开始时间
        [DefaultValue(0),Display("标志"), DisplayEnumBit(typeof(EEventFlag)),DisplayLabelWidth(250)]public ushort flags;
#if UNITY_EDITOR
        [System.NonSerialized] public System.Object ownerObject;
        [System.NonSerialized] public CutsceneTrack ownerTrackObject;
#endif
    }
    //-----------------------------------------------------
    public interface IBaseEvent : IDataer
    {
        ushort GetEventFlags();
    }
  //  public abstract class ABaseEvent
  //  {
  //      public float time;
  //      public string name;
		//public abstract int GetEventType();
  //      public T Cast<T>() where T : ABaseEvent
  //      {
  //          return this as T;
  //      }
  //      public bool IsCast<T>() where T : ABaseEvent
  //      {
  //          return this is T;
  //      }
  //  }
}