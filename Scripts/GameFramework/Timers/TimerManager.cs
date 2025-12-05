/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	TimerManager
作    者:	HappLI
描    述:	
*********************************************************************/

using System.Collections.Generic;

namespace Framework.Core
{
    public interface ITimerTicker
    {
        bool OnTimerTick(IBaseTimerEvent hHandle, IUserData param);
        bool IsTimerValid();
    }
    //------------------------------------------------------
    public class TimerManager /*: AModule*/
    {
        public delegate bool EventFunction(int nEventHash, IUserData param);
        static Stack<TimerEvent>            ms_vPools = new Stack<TimerEvent>(32);
        static TimerManager                 ms_Instance = null;
        public static TimerManager getInstance()
        {
            if (ms_Instance == null)
            {
                ms_Instance = new TimerManager();
                ms_Instance.Awake();
            }
            return ms_Instance;
        }

        protected long                      m_lRuntime = 0;
        protected long                      m_lRuntimeUnScale = 0;
        long                                m_dwStartGameTime;
        long                                m_dwCurrentSeverTime;
        uint                                m_nDiffSeconds;

        long                                m_dwLastCountTime;
        long                                m_dwServerTimeDelta;

        int                                 m_nAutoHash = 0;
        
        Dictionary<string, IBaseTimerEvent> m_vTimerEvents;
        HashSet<IBaseTimerEvent>            m_vDestroyed = null;

        //------------------------------------------------------
        public /*override*/ void Awake()
        {
            m_vTimerEvents = new Dictionary<string, IBaseTimerEvent>(16);
            m_vDestroyed = new HashSet<IBaseTimerEvent>(8);
        }
        //------------------------------------------------------
        public void Clear()
        {
            m_lRuntime = 0;
            m_lRuntimeUnScale = 0;
            m_dwStartGameTime = 0;
            m_dwCurrentSeverTime = 0;
            m_nDiffSeconds = 0;
            m_dwLastCountTime = 0;
            m_dwServerTimeDelta = 0;

            ClearTimerEvent();
            if (m_vDestroyed != null)
                m_vDestroyed.Clear();
            m_nAutoHash = 0;
        }
        //------------------------------------------------------
        public /*override*/ void Destroy()
        {
            Clear();
        }
        //------------------------------------------------------
        public void GameStart(long StartTime)
        {
            m_dwStartGameTime = StartTime;
        }
        //------------------------------------------------------
        public long GetGameStart()
        {
            return m_dwStartGameTime;
        }
        //------------------------------------------------------
        public long GetGameTime()
        {
            return m_dwCurrentSeverTime +m_nDiffSeconds;
        }
        //------------------------------------------------------
        public void SynchronousTime(long SynTime, long localTime)
        {
            if(SynTime ==0)
            {
                //! req net
            }
            else
            {       
                m_dwCurrentSeverTime = SynTime;
                m_dwServerTimeDelta = 0;
                m_dwLastCountTime = localTime;
            }
        }
        //------------------------------------------------------
        public uint GetDiffSeconds()
        {
            return m_nDiffSeconds;
        }
        //------------------------------------------------------
        internal void Update(long lRuntime, long lUnScaleRunTime, bool bProcess = true)
        {
            if(m_dwStartGameTime > 0 && m_dwCurrentSeverTime > 0)
            {
                long dwDelta = lRuntime;
                long dwFrameTime = (dwDelta - m_dwLastCountTime);
                m_dwLastCountTime = dwDelta;

                m_dwServerTimeDelta += dwFrameTime;
                m_dwCurrentSeverTime += m_dwServerTimeDelta / 1000;
                m_dwServerTimeDelta %= 1000;
            }

            if (bProcess)
            {
                long runTime = lRuntime;
                long runUnScaleTime = lUnScaleRunTime;
                IBaseTimerEvent timer;
                foreach (var db in m_vTimerEvents)
                {
                    timer = db.Value;
                    if (!timer.Update(timer.IsTimeScale()? runTime: runUnScaleTime))
                        m_vDestroyed.Add(db.Value);
                }
            }
            IBaseTimerEvent tiemr;
            foreach (var db in m_vDestroyed)
            {
                tiemr = db;
                m_vTimerEvents.Remove(tiemr.GetEventName());
                RecylePool(tiemr);
            }
            m_vDestroyed.Clear();
        }
        //------------------------------------------------------
        public bool FindTimer(string strEvent)
        {
            return IsTimerEvent(strEvent);
        }
        //------------------------------------------------------
        public void RemoveTimer(string strEvent)
        {
            if (string.IsNullOrEmpty(strEvent)) return;
            IBaseTimerEvent pEvent;
            if (m_vTimerEvents.TryGetValue(strEvent, out pEvent))
            {
                RecylePool(pEvent);
                m_vTimerEvents.Remove(strEvent);
            }
        }
        //------------------------------------------------------
        public void ClearTimer()
        {
            ClearTimerEvent();
        }
        //------------------------------------------------------
        private void RecylePool(IBaseTimerEvent timer)
        {
            timer.Clear();
            if (timer is TimerEvent)
            {
                if (ms_vPools.Count < 32) ms_vPools.Push(timer as TimerEvent);
            }
        }
        //------------------------------------------------------
        private TimerEvent NewPool(ITimerTicker pTicker, TimerManager.EventFunction Function, IUserData param)
        {
            TimerEvent timer = null;
            if (ms_vPools.Count > 0) timer = ms_vPools.Pop();
            else timer = new TimerEvent();

            timer.Clear();
            timer.Set(pTicker, Function, param);
            return timer;
        }
        //------------------------------------------------------
        internal IBaseTimerEvent AddTimer(string strEventName, ITimerTicker pTicker, TimerManager.EventFunction Function, long Interval, long ExeTimes =-1, bool bDelta = false, bool bTimerScale = true, IUserData param = null, bool bReplace = false)
        {
            if (string.IsNullOrEmpty(strEventName)) return null;
            IBaseTimerEvent pEvent = RegisterTimerEvent(strEventName, pTicker, Function, param);
            if (pEvent!=null)
            {
                TimerEvent timer = (TimerEvent)pEvent;
                timer.bDelta = bDelta;
                timer.deltaTime = 0;
                timer.interval = Interval;
                timer.eventName = strEventName;
                timer.exeTimes = ExeTimes;
                timer.bTimerScale = bTimerScale;
                if(bTimerScale)  timer.callTime = m_lRuntime + Interval;
                else timer.callTime = m_lRuntimeUnScale + Interval;
                timer.eventHash = ++m_nAutoHash;
            }
            else if(bReplace)
            {
                pEvent = GetTimerEvent(strEventName);
                TimerEvent timer = (TimerEvent)pEvent;
                timer.bDelta = bDelta;
                timer.deltaTime = 0;
                timer.interval = Interval;
                timer.exeTimes = ExeTimes;
                timer.bTimerScale = bTimerScale;
                if (bTimerScale) timer.callTime = m_lRuntime + Interval;
                else timer.callTime = m_lRuntimeUnScale + Interval;
            }
            return pEvent;
        }
        //------------------------------------------------------
        IBaseTimerEvent RegisterTimerEvent(string strEventName, ITimerTicker pTicker, TimerManager.EventFunction Function, IUserData param)
        {
            if (string.IsNullOrEmpty(strEventName)) return null;
            if (!IsTimerEvent(strEventName))
            {
                IBaseTimerEvent timer_event = NewPool(pTicker, Function, param);
                m_vTimerEvents.Add(strEventName, timer_event);
                return timer_event;
            }
            return null;
        }
        //------------------------------------------------------
        bool IsTimerEvent(string strEventName)
        {
            if (string.IsNullOrEmpty(strEventName)) return false;
            return m_vTimerEvents.ContainsKey(strEventName);
        }
        //------------------------------------------------------
        IBaseTimerEvent GetTimerEvent(string strEventName)
        {
            if (string.IsNullOrEmpty(strEventName)) return null;
            IBaseTimerEvent pEvent;
            if (m_vTimerEvents.TryGetValue(strEventName, out pEvent))
                return pEvent;
            return null;
        }
        //------------------------------------------------------
        public void ClearTimerEvent()
        {
            if (m_vTimerEvents == null)
                return;
            foreach (var db in m_vTimerEvents)
            {
                RecylePool(db.Value);
            }
            m_vTimerEvents.Clear();
        }
        //------------------------------------------------------
        public static void Process(long lRuntime, long lUnScaleRunTime, bool bProcess = true)
        {
            getInstance().Update(lRuntime, lUnScaleRunTime, bProcess);
        }
        //------------------------------------------------------
        /**********************************************************************************************************
        函数名:AddTimer
        参数说明:TimerEventName=事件名,Object=对像,Function=方法,Interval=间隔,ExeTimes=次数,bDelta=是否补回时间差
        输出说明:无
        功能描述:增加定时器
        注意:bDelta为true时,补回时间差是将延时或慢了的多用的时间,补回到下次计时中去.减少整个时间段的误差.
             如一秒定时,这次耗时1.5时,下次会在0.5秒时回调.若这次卡了用时3.5秒.将连续回调3次,之后0.5秒回调第四次.
             但,如果处理耗时间太长,或暂时时间过长,会出现连续回调多次或卡机状态. 慎用!!!
        **********************************************************************************************************/
        public static IBaseTimerEvent RegisterTimer(string strEventName, ITimerTicker pTicker, long Interval, long ExeTimes = -1, bool bDelta = false, bool bTimeScale = true, IUserData param = null, bool bReplace = false)
        {
            return getInstance().AddTimer(strEventName, pTicker, null, Interval, ExeTimes, bDelta, bTimeScale, param, bReplace);
        }
        //------------------------------------------------------
        public static IBaseTimerEvent RegisterTimer(string strEventName,TimerManager.EventFunction Function, long Interval, long ExeTimes = -1, bool bDelta = false, bool bTimeScale = true, IUserData param = null, bool bReplace = false)
        {
            return getInstance().AddTimer(strEventName, null, Function, Interval, ExeTimes, bDelta, bTimeScale, param, bReplace);
        }
        //------------------------------------------------------
        public static void UnRegisterTimer( string strEventName)
        {
            if (ms_Instance == null)
            {
                return;
            }
            ms_Instance.RemoveTimer(strEventName);
        }
        //------------------------------------------------------
        public static bool HasTimer(string strEventName)
        {
            if (ms_Instance == null)
            {
                return false;
            }
            return ms_Instance.FindTimer(strEventName);
        }
    }
}