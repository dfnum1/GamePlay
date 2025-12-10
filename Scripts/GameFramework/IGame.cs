/********************************************************************
生成日期:	3:10:2019  15:03
类    名: 	AFramework
作    者:	HappLI
描    述:	框架基类
*********************************************************************/
using System.Collections;
using UnityEngine;

namespace Framework.Core
{
    public interface IGame
    {
        Coroutine BeginCoroutine(IEnumerator coroutine);
        void EndAllCoroutine();
        void EndCoroutine(Coroutine cortuine);
        void EndCoroutine(IEnumerator cortuine);
        bool IsEditor();
        ScriptableObject[] GetDatas();
        Transform GetTransform();
        UnityEngine.Rendering.RenderPipelineAsset GetURPAsset();
    }
}