/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	AUITouchIngore
作    者:	
描    述:	UI点击拦截
*********************************************************************/
using UnityEngine;
namespace Framework.Guide
{
    public abstract class AUITouchIngore : MonoBehaviour, UnityEngine.ICanvasRaycastFilter
    {
		bool ICanvasRaycastFilter.IsRaycastLocationValid(Vector2 screenPos, Camera eventCamera)
		{
			return false;
		}
    }
}
