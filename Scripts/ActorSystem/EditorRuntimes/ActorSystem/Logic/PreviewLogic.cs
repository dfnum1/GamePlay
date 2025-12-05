#if UNITY_EDITOR
/********************************************************************
生成日期:	11:06:2023
类    名: 	SkillEditorPreviewLogic
作    者:	HappLI
描    述:	
*********************************************************************/
using Framework.ActorSystem.Runtime;
using Framework.Cutscene.Editor;
using Framework.ED;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Framework.ActorSystem.Editor
{
    [EditorBinder(typeof(ActionEditorWindow), "PreviewRect")]
    public class PreviewLogic : ActionEditorLogic
    {
        TargetPreview m_Preview;
        GUIStyle m_PreviewStyle;

        List<Actor> m_vActors = new List<Actor>();
        //--------------------------------------------------------
        protected override void OnEnable()
        {
            if (m_Preview == null) m_Preview = new TargetPreview(GetOwner());
            GameObject[] roots = new GameObject[1];
            roots[0] = new GameObject("EditorRoot");
            m_Preview.AddPreview(roots[0]);

            m_Preview.SetCamera(0.01f, 10000f, 60f);
            m_Preview.Initialize(roots);
            m_Preview.SetPreviewInstance(roots[0] as GameObject);
            m_Preview.SetFloorTexture(Framework.ED.EditorUtils.GetFloorTexture());
            m_Preview.bLeftMouseForbidMove = true;
            m_Preview.OnDrawAfterCB += OnDraw;
        }
        //--------------------------------------------------------
        public override void OnSelectActor(Actor pActor)
        {
            if (pActor == null)
                return;

            if (m_vActors.Contains(pActor))
                return;
            m_vActors.Add(pActor);
            if(pActor != null && pActor.GetUniyTransform()!=null)
                m_Preview.AddPreview(pActor.GetUniyTransform().gameObject, HideFlags.HideInInspector | HideFlags.DontSaveInEditor | HideFlags.DontSaveInBuild);
        }
        //--------------------------------------------------------
        protected override void OnDisable()
        {
            if (m_Preview != null) m_Preview.Destroy();
            m_Preview = null;
        }
        //--------------------------------------------------------
        public void AddInstance(GameObject pAble)
        {
            if (m_Preview != null && pAble)
                m_Preview.AddPreview(pAble.gameObject, HideFlags.HideInInspector | HideFlags.DontSaveInEditor | HideFlags.DontSaveInBuild);
        }
        //--------------------------------------------------------
       public override  void OnSpwanGameObejct(GameObject pGo)
        {
            AddInstance(pGo);
        }
        //--------------------------------------------------------
        void OnDraw(int controllerId, Camera camera, Event evt)
        {
            GetOwner<ActionEditorWindow>().OnPreviewDraw(controllerId, camera, evt);
            for(int i =0 ; i < m_vActors.Count;)
            {
                var actor = m_vActors[i];
                if (actor == null || actor.IsDestroy())
                {
                    m_vActors.RemoveAt(i);
                    continue;
                }
                ActorSystemUtil.DrawBoundingBox(actor.GetBounds().GetCenter(), actor.GetBounds().GetHalf(), actor.GetMatrix(), Color.green, false);

                Handles.Label(actor.GetPosition() + Vector3.up * 0.5f, "攻击组[" + actor.GetAttackGroup() + "]");
                //        Handles.ArrowHandleCap(0, actor.GetPosition(), Quaternion.Euler(actor.GetEulerAngle()), 0.5f, EventType.Repaint);
                if(!evt.shift)
                {
                    if (Tools.current == Tool.Rotate)
                    {
                        EditorGUI.BeginChangeCheck();
                        var rotation = Handles.DoRotationHandle(Quaternion.Euler(actor.GetEulerAngle()), actor.GetPosition());
                        if (EditorGUI.EndChangeCheck())
                            actor.SetEulerAngle(rotation.eulerAngles);
                    }
                    else
                    {
                        EditorGUI.BeginChangeCheck();
                        var pos = Handles.DoPositionHandle(actor.GetPosition(), Quaternion.identity);
                        if (EditorGUI.EndChangeCheck())
                            actor.SetPosition(pos);
                    }
                }


                var objectAble = actor.GetObjectAble();
                if(objectAble.pContextData!=null)
                {
                    AActorComponent actorComp = objectAble.CastContextData<AActorComponent>();
                    if(actorComp!=null && actorComp.slots!=null)
                    {
                        for(int j =0; j < actorComp.slots.Length; ++j)
                        {
                            var slot = actorComp.slots[j];
                            if (slot.slot == null) continue;
                            Vector3 pos = slot.slot.position + slot.offset;
                            Handles.color = Color.cyan;
                            Handles.SphereHandleCap(0, pos, Quaternion.identity, 0.05f, EventType.Repaint);
                            Handles.Label(pos + Vector3.up * 0.1f, slot.name, EditorStyles.boldLabel);
                            if(evt.control)
                            {
                                Handles.DoPositionHandle(pos, slot.slot.rotation);
                            }
                        }
                    }
                }
                ++i;
            }
        }
        //--------------------------------------------------------
        protected override void OnUpdate(float delta)
        {
            base.OnUpdate(delta);
            m_Preview.Update(delta);
        }
        //--------------------------------------------------------
        protected override void OnGUI()
        {
            var window = GetOwner<ActionEditorWindow>();
            if (m_Preview != null && window.PreviewRect.width > 0 && window.PreviewRect.height > 0)
            {
                if(m_PreviewStyle == null)
                    m_PreviewStyle = new GUIStyle(EditorStyles.textField);
                m_Preview.OnPreviewGUI(window.PreviewRect, m_PreviewStyle);
            }
        }
    }
}

#endif