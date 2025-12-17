/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	AUIPenetrate
作    者:	HappLI
描    述:	UI穿透
*********************************************************************/
using UnityEngine;
using UnityEngine.EventSystems;
namespace Framework.Guide
{
    [AddComponentMenu("")]
    public abstract class AUIPenetrate : AEventTriggerListener
    {
        GameObject m_TriggerGo;
        public GameObject TriggerGo
        {
            get { return m_TriggerGo; }
            set
            {
                if (m_TriggerGo != value)
                {
                    m_Listener = null;
                    m_TriggerGo = value;
                    CheckTargetListen();
                }
            }
        }
        AEventTriggerListener m_Listener;

        private bool m_bPenetrate = false;
        private int m_PenetrateGUID = 0;
        private int m_nListIndex = -1;
        private string m_PenetrateTag = null;
        public string SearchListenName;
        //------------------------------------------------------
        public void EnablePenetrate(bool bPenetrate, int target = 0, int listIndex = -1, string targetTag = null)
        {
            m_bPenetrate = bPenetrate;
            m_PenetrateGUID = target;
            m_PenetrateTag = targetTag;
            m_nListIndex = listIndex;
        }
        //------------------------------------------------------
        bool OnUIWidgetTrigger(BaseEventData eventData, EUIWidgetTriggerType eventType)
        {
            if (m_bPenetrate && m_PenetrateGUID != 0 && eventType != EUIWidgetTriggerType.None)
                Guide.GuideSystem.getInstance().OnUIWidgetTrigger(m_PenetrateGUID, m_nListIndex, m_PenetrateTag, eventType);
            return false;
        }
        //------------------------------------------------------
        void CheckTargetListen()
        {
            return;
        }
        //------------------------------------------------------
        private bool bCommonListener()
        {
            return m_Listener != null && m_Listener.gameObject == m_TriggerGo;
        }
        //------------------------------------------------------
        public override void OnBeginDrag(PointerEventData eventData)
        {
            if (m_Listener != null)
            {
                bool bCommon = bCommonListener();
                if (ExecuteEvents.Execute<IBeginDragHandler>(m_Listener.gameObject, eventData, ExecuteEvents.beginDragHandler))
                    return;
                if (bCommon) return;
            }
            if (TriggerGo)
            {
                ExecuteEvents.Execute<IBeginDragHandler>(TriggerGo, eventData, ExecuteEvents.beginDragHandler);
            }
            OnUIWidgetTrigger(eventData, EUIWidgetTriggerType.BeginDrag);
        }
        //------------------------------------------------------
        public override void OnDrag(PointerEventData eventData)
        {
            if (m_Listener != null)
            {
                bool bCommon = bCommonListener();
                if (ExecuteEvents.Execute<IDragHandler>(m_Listener.gameObject, eventData, ExecuteEvents.dragHandler))
                    return;
                if (bCommon) return;
            }
            if (TriggerGo)
                ExecuteEvents.Execute<IDragHandler>(TriggerGo, eventData, ExecuteEvents.dragHandler);
            OnUIWidgetTrigger(eventData, EUIWidgetTriggerType.Drag);
        }
        //------------------------------------------------------
        public override void OnEndDrag(PointerEventData eventData)
        {
            if (m_Listener != null)
            {
                bool bCommon = bCommonListener();
                if (ExecuteEvents.Execute<IEndDragHandler>(m_Listener.gameObject, eventData, ExecuteEvents.endDragHandler))
                    return;
                if (bCommon) return;
            }
            if (TriggerGo)
                ExecuteEvents.Execute<IEndDragHandler>(TriggerGo, eventData, ExecuteEvents.endDragHandler);
            OnUIWidgetTrigger(eventData, EUIWidgetTriggerType.EndDrag);
        }
        //------------------------------------------------------
        public override void OnPointerClick(PointerEventData eventData)
        {
            bool bActive = TriggerGo ? TriggerGo.activeSelf : false;
            if (!bActive && TriggerGo) TriggerGo.SetActive(true);
            if (m_Listener != null)
            {
                bool bCommon = bCommonListener();

                if (TriggerGo) TriggerGo.SetActive(bActive);
                if (ExecuteEvents.Execute<IPointerClickHandler>(m_Listener.gameObject, eventData, ExecuteEvents.pointerClickHandler))
                {
                    return;
                }
                if (bCommon)
                {
                    if (TriggerGo) TriggerGo.SetActive(bActive);
                    return;
                }
            }
            if (TriggerGo)
            {
                TriggerGo.SetActive(bActive);
                ExecuteEvents.Execute<IPointerClickHandler>(TriggerGo, eventData, ExecuteEvents.pointerClickHandler);
            }
            OnUIWidgetTrigger(eventData, EUIWidgetTriggerType.Click);
        }
        //------------------------------------------------------
        public override void OnPointerDown(PointerEventData eventData)
        {
            DoPressAction();
            CheckTargetListen();
            if (m_Listener != null)
            {
                bool bCommon = bCommonListener();
                if (ExecuteEvents.Execute<IPointerDownHandler>(m_Listener.gameObject, eventData, ExecuteEvents.pointerDownHandler))
                    return;
                if (bCommon) return;
            }
            if (TriggerGo)
                ExecuteEvents.Execute<IPointerDownHandler>(TriggerGo, eventData, ExecuteEvents.pointerDownHandler);
            OnUIWidgetTrigger(eventData, EUIWidgetTriggerType.Down);
        }
        //------------------------------------------------------
        public override void OnPointerUp(PointerEventData eventData)
        {
            DoUpAction();
            if (m_Listener != null)
            {
                bool bCommon = bCommonListener();
                if (ExecuteEvents.Execute<IPointerUpHandler>(m_Listener.gameObject, eventData, ExecuteEvents.pointerUpHandler))
                    return;
                if (bCommon) return;
            }
            if (TriggerGo)
                ExecuteEvents.Execute<IPointerUpHandler>(TriggerGo, eventData, ExecuteEvents.pointerUpHandler);
            OnUIWidgetTrigger(eventData, EUIWidgetTriggerType.Up);
        }
        //------------------------------------------------------
        public override void OnPointerExit(PointerEventData eventData)
        {
            if (m_Listener != null)
            {
                bool bCommon = bCommonListener();
                if (ExecuteEvents.Execute<IPointerExitHandler>(m_Listener.gameObject, eventData, ExecuteEvents.pointerExitHandler))
                    return;
                if (bCommon) return;
            }
            if (TriggerGo)
                ExecuteEvents.Execute<IPointerExitHandler>(TriggerGo, eventData, ExecuteEvents.pointerExitHandler);
            OnUIWidgetTrigger(eventData, EUIWidgetTriggerType.Exit);
        }
        //------------------------------------------------------
        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (m_Listener != null)
            {
                bool bCommon = bCommonListener();
                if (ExecuteEvents.Execute<IPointerEnterHandler>(m_Listener.gameObject, eventData, ExecuteEvents.pointerEnterHandler))
                    return;
                if (bCommon) return;
            }
            if (TriggerGo)
                ExecuteEvents.Execute<IPointerEnterHandler>(TriggerGo, eventData, ExecuteEvents.pointerEnterHandler);
            OnUIWidgetTrigger(eventData, EUIWidgetTriggerType.Enter);
        }
        //------------------------------------------------------
        public override void OnDrop(PointerEventData eventData)
        {
            if (m_Listener != null)
            {
                bool bCommon = bCommonListener();
                if (ExecuteEvents.Execute<IDropHandler>(m_Listener.gameObject, eventData, ExecuteEvents.dropHandler))
                    return;
                if (bCommon) return;
            }
            if (TriggerGo)
                ExecuteEvents.Execute<IDropHandler>(TriggerGo, eventData, ExecuteEvents.dropHandler);
            OnUIWidgetTrigger(eventData, EUIWidgetTriggerType.Drop);
        }
        //------------------------------------------------------
        public override void OnScroll(PointerEventData eventData)
        {
            if (m_Listener != null)
            {
                bool bCommon = bCommonListener();
                if (ExecuteEvents.Execute<IScrollHandler>(m_Listener.gameObject, eventData, ExecuteEvents.scrollHandler))
                    return;
                if (bCommon) return;
            }
            if (TriggerGo)
                ExecuteEvents.Execute<IScrollHandler>(TriggerGo, eventData, ExecuteEvents.scrollHandler);
            OnUIWidgetTrigger(eventData, EUIWidgetTriggerType.Scroll);
        }
        //------------------------------------------------------
        public override void OnMove(AxisEventData eventData)
        {
            if (m_Listener != null)
            {
                bool bCommon = bCommonListener();
                if (ExecuteEvents.Execute<IMoveHandler>(m_Listener.gameObject, eventData, ExecuteEvents.moveHandler))
                    return;
                if (bCommon) return;
            }
            if (TriggerGo)
                ExecuteEvents.Execute<IMoveHandler>(TriggerGo, eventData, ExecuteEvents.moveHandler);
            OnUIWidgetTrigger(eventData, EUIWidgetTriggerType.Move);
        }
        //------------------------------------------------------
        public override void OnSubmit(BaseEventData eventData)
        {
            if (m_Listener != null)
            {
                bool bCommon = bCommonListener();
                if (ExecuteEvents.Execute<ISubmitHandler>(m_Listener.gameObject, eventData, ExecuteEvents.submitHandler))
                    return;
                if (bCommon) return;
            }
            if (TriggerGo)
                ExecuteEvents.Execute<ISubmitHandler>(TriggerGo, eventData, ExecuteEvents.submitHandler);
            OnUIWidgetTrigger(eventData, EUIWidgetTriggerType.Submit);
        }
        //------------------------------------------------------
        public override void OnCancel(BaseEventData eventData)
        {
            if (m_Listener != null)
            {
                bool bCommon = bCommonListener();
                if (ExecuteEvents.Execute<ICancelHandler>(m_Listener.gameObject, eventData, ExecuteEvents.cancelHandler))
                    return;
                if (bCommon) return;
            }
            if (TriggerGo)
                ExecuteEvents.Execute<ICancelHandler>(TriggerGo, eventData, ExecuteEvents.cancelHandler);
            OnUIWidgetTrigger(eventData, EUIWidgetTriggerType.Cancel);
        }
        //------------------------------------------------------
        public override void OnSelect(BaseEventData eventData)
        {
            if (m_Listener != null)
            {
                bool bCommon = bCommonListener();
                if (ExecuteEvents.Execute<ISelectHandler>(m_Listener.gameObject, eventData, ExecuteEvents.selectHandler))
                    return;
                if (bCommon) return;
            }
            if (TriggerGo)
                ExecuteEvents.Execute<ISelectHandler>(TriggerGo, eventData, ExecuteEvents.selectHandler);
            OnUIWidgetTrigger(eventData, EUIWidgetTriggerType.Select);
        }
        //------------------------------------------------------
        public override void OnUpdateSelected(BaseEventData eventData)
        {
            if (m_Listener != null)
            {
                bool bCommon = bCommonListener();
                if (ExecuteEvents.Execute<IUpdateSelectedHandler>(m_Listener.gameObject, eventData, ExecuteEvents.updateSelectedHandler))
                    return;
                if (bCommon) return;
            }
            if (TriggerGo)
                ExecuteEvents.Execute<IUpdateSelectedHandler>(TriggerGo, eventData, ExecuteEvents.updateSelectedHandler);
            OnUIWidgetTrigger(eventData, EUIWidgetTriggerType.UpdateSelect);
        }
        //------------------------------------------------------
        public override void OnDeselect(BaseEventData eventData)
        {
            if (m_Listener != null)
            {
                bool bCommon = bCommonListener();
                if (ExecuteEvents.Execute<IDeselectHandler>(m_Listener.gameObject, eventData, ExecuteEvents.deselectHandler))
                    return;
                if (bCommon) return;
            }
            if (TriggerGo)
                ExecuteEvents.Execute<IDeselectHandler>(TriggerGo, eventData, ExecuteEvents.deselectHandler);
            OnUIWidgetTrigger(eventData, EUIWidgetTriggerType.Deselect);
        }
    }
}
