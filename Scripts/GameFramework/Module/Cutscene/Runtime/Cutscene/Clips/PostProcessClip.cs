/********************************************************************
生成日期:	06:30:2025
类    名: 	PostProcessClip
作    者:	HappLI
描    述:	后处理剪辑
*********************************************************************/
#if UNITY_EDITOR
using Framework.Cutscene.Editor;
using UnityEditor;
#endif
using Framework.DrawProps;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Framework.Cutscene.Runtime
{
    [System.Serializable, CutsceneClip("后处理Clip")]
    public class PostProcessClip : IBaseClip
    {
        public enum EPostProcessType : byte
        {
            Vignette = 0,
            ChromaticAberration = 1,
            ColorAdjustments = 2,
            Bloom = 6,
        }
        [Display("基本属性")] public BaseClipProp baseProp;
        [Display("后处理类型")] public EPostProcessType postProcessType = EPostProcessType.Vignette;
        [Disable]public List<AnimationCurve> values;
        [Disable]public List<Color> colors;
        [Disable]public List<Vector4> vectors;
        [Disable]public List<bool> toggles;
        //-----------------------------------------------------
        public ACutsceneDriver CreateDriver()
        {
            return new PostProcessDriver();
        }
        //-----------------------------------------------------
        public ushort GetIdType()
        {
            return (ushort)EClipType.ePostProcess;
        }
        //-----------------------------------------------------
        public float GetDuration()
        {
            return baseProp.duration;
        }
        //-----------------------------------------------------
        public EClipEdgeType GetEndEdgeType()
        {
            return baseProp.endEdgeType;
        }
        //-----------------------------------------------------
        public string GetName()
        {
            return baseProp.name;
        }
        //-----------------------------------------------------
        public ushort GetRepeatCount()
        {
            return baseProp.repeatCnt;
        }
        //-----------------------------------------------------
        public float GetTime()
        {
            return baseProp.time;
        }
        //-----------------------------------------------------
        public float GetBlend(bool bIn)
        {
            return baseProp.GetBlend(bIn);
        }
#if UNITY_EDITOR
        [AddInspector]
        public void OnInspector()
        {
            float normalizedTime = 0;
            VolumeProfile volumeProfiler = null;
            if (baseProp.ownerTrackObject != null && baseProp.ownerTrackObject.GetPlayable()!=null)
            {

                float time = baseProp.ownerTrackObject.GetPlayable().GetTime();
                if (GetDuration()>0)
                {
                    normalizedTime = Mathf.Clamp01((time - GetTime()) / GetDuration());

                    var bindObj = baseProp.ownerTrackObject.GetBindLastCutsceneObject();
                    if (bindObj != null && bindObj.GetUniyTransform() != null)
                    {
                        var volume = bindObj.GetUniyTransform().GetComponent<Volume>();
                        if (volume != null && volume.profile)
                        {
                            volumeProfiler = volume.profile;
                        }
                    }
                }
            }
            switch(postProcessType)
            {
                case EPostProcessType.Vignette:
                    {
                        if (colors == null || colors.Count != 1)
                        {
                            colors = new List<Color>(1);
                            colors.Add(Color.white);
                        }
                        colors[0] = EditorGUILayout.ColorField("Color", colors[0]);

                        if (vectors == null || vectors.Count != 1)
                        {
                            vectors = new List<Vector4>(1);
                            vectors.Add(Vector4.zero);
                        }
                        vectors[0] = EditorGUILayout.Vector2Field("Center", new Vector2(vectors[0].x, vectors[0].y));

                        if (values == null || values.Count != 2)
                        {
                            values = new List<AnimationCurve>(2);
                            values.Add(new AnimationCurve());
                            values.Add(new AnimationCurve());
                        }
                        GUILayout.BeginHorizontal();
                        values[0] = EditorGUILayout.CurveField("Intensity", values[0]);
                        if(GUILayout.Button("Clear", GUILayout.Width(30)))
                        {
                            values[0] = new AnimationCurve();
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        values[1] = EditorGUILayout.CurveField("Smoothness", values[1]);
                        if (GUILayout.Button("Clear", GUILayout.Width(30)))
                        {
                            values[1] = new AnimationCurve();
                        }
                        GUILayout.EndHorizontal();

                        if (toggles == null || toggles.Count != 1)
                        {
                            toggles = new List<bool>(1);
                            toggles.Add(false);
                        }
                        toggles[0] = EditorGUILayout.Toggle("Rounded", toggles[0]);

                        if(volumeProfiler!=null && normalizedTime>=0 && normalizedTime<=1)
                        {
                            volumeProfiler.TryGet<Vignette>(out var comp);
                            if (comp!=null && GUILayout.Button("设置参数"))
                            {
                                toggles[0] = comp.rounded.value;
                                vectors[0] = new Vector4(comp.center.value.x, comp.center.value.y, 0, 0);
                                colors[0] = comp.color.value;
                                AddOrReplaceKey(values[0],normalizedTime, comp.intensity.value);
                                AddOrReplaceKey(values[1],normalizedTime, comp.smoothness.value);
                            }
                        }
                    }
                    break;
                case EPostProcessType.ChromaticAberration:
                    {
                        if (values == null || values.Count != 1)
                        {
                            values = new List<AnimationCurve>(1);
                            values.Add(new AnimationCurve());
                        }
                        GUILayout.BeginHorizontal();
                        values[0] = EditorGUILayout.CurveField("Intensity", values[0]);
                        if (GUILayout.Button("Clear", GUILayout.Width(30)))
                        {
                            values[0] = new AnimationCurve();
                        }
                        GUILayout.EndHorizontal();
                        if (volumeProfiler != null && normalizedTime >= 0 && normalizedTime <= 1)
                        {
                            volumeProfiler.TryGet<ChromaticAberration>(out var comp);
                            if (comp != null && GUILayout.Button("设置参数"))
                            {
                                AddOrReplaceKey(values[0],normalizedTime, comp.intensity.value);
                            }
                        }
                    }
                    break;
                case EPostProcessType.ColorAdjustments:
                    {
                        if (values == null || values.Count != 4)
                        {
                            values = new List<AnimationCurve>(4);
                            values.Add(new AnimationCurve());
                            values.Add(new AnimationCurve());
                            values.Add(new AnimationCurve());
                            values.Add(new AnimationCurve());
                        }
                        GUILayout.BeginHorizontal();
                        values[0] = EditorGUILayout.CurveField("Post Exposure", values[0]);
                        if (GUILayout.Button("Clear", GUILayout.Width(30)))
                        {
                            values[0] = new AnimationCurve();
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        values[1] = EditorGUILayout.CurveField("Contrast", values[1]);
                        if (GUILayout.Button("Clear", GUILayout.Width(30)))
                        {
                            values[1] = new AnimationCurve();
                        }
                        GUILayout.EndHorizontal();


                        if (colors == null || colors.Count != 1)
                        {
                            colors = new List<Color>(1);
                            colors.Add(Color.white);
                        }
                        colors[0] = EditorGUILayout.ColorField("Color Filters", colors[0]);

                        GUILayout.BeginHorizontal();
                        values[2] = EditorGUILayout.CurveField("Hue Shift", values[1]);
                        if (GUILayout.Button("Clear", GUILayout.Width(30)))
                        {
                            values[2] = new AnimationCurve();
                        }
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        values[3] = EditorGUILayout.CurveField("Saturation", values[1]);
                        if (GUILayout.Button("Clear", GUILayout.Width(30)))
                        {
                            values[3] = new AnimationCurve();
                        }
                        GUILayout.EndHorizontal();

                        if (volumeProfiler != null && normalizedTime >= 0 && normalizedTime <= 1)
                        {
                            volumeProfiler.TryGet<ColorAdjustments>(out var comp);
                            if (comp != null && GUILayout.Button("设置参数"))
                            {
                                colors[0] = comp.colorFilter.value;
                                AddOrReplaceKey(values[0], normalizedTime, comp.postExposure.value);
                                AddOrReplaceKey(values[1], normalizedTime, comp.contrast.value);
                                AddOrReplaceKey(values[2], normalizedTime, comp.hueShift.value);
                                AddOrReplaceKey(values[3], normalizedTime, comp.saturation.value);
                            }
                        }
                    }
                    break;
                case EPostProcessType.Bloom:
                    {
                        if (values == null || values.Count != 5)
                        {
                            values = new List<AnimationCurve>(5);
                            values.Add(new AnimationCurve());
                            values.Add(new AnimationCurve());
                            values.Add(new AnimationCurve());
                            values.Add(new AnimationCurve());
                            values.Add(new AnimationCurve());
                        }
                        GUILayout.BeginHorizontal();
                        values[0] = EditorGUILayout.CurveField("Threshold", values[0]);
                        if (GUILayout.Button("Clear", GUILayout.Width(30)))
                        {
                            values[0] = new AnimationCurve();
                        }
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        values[1] = EditorGUILayout.CurveField("Intensity", values[1]);
                        if (GUILayout.Button("Clear", GUILayout.Width(30)))
                        {
                            values[1] = new AnimationCurve();
                        }
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        values[1] = EditorGUILayout.CurveField("Scatter", values[1]);
                        if (GUILayout.Button("Clear", GUILayout.Width(30)))
                        {
                            values[1] = new AnimationCurve();
                        }
                        GUILayout.EndHorizontal();

                        if (colors == null || colors.Count != 1)
                        {
                            colors = new List<Color>(1);
                            colors.Add(Color.white);
                        }
                        colors[0] = EditorGUILayout.ColorField("Tint", colors[0]);

                        GUILayout.BeginHorizontal();
                        values[2] = EditorGUILayout.CurveField("Clamp", values[1]);
                        if (GUILayout.Button("Clear", GUILayout.Width(30)))
                        {
                            values[2] = new AnimationCurve();
                        }
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        values[3] = EditorGUILayout.CurveField("Dirt Intensity", values[1]);
                        if (GUILayout.Button("Clear", GUILayout.Width(30)))
                        {
                            values[3] = new AnimationCurve();
                        }
                        GUILayout.EndHorizontal();
                        if (volumeProfiler != null && normalizedTime >= 0 && normalizedTime <= 1)
                        {
                            volumeProfiler.TryGet<Bloom>(out var comp);
                            if (comp != null && GUILayout.Button("设置参数"))
                            {
                                colors[0] = comp.tint.value;
                                AddOrReplaceKey(values[0],normalizedTime, comp.threshold.value);
                                AddOrReplaceKey(values[1],normalizedTime, comp.intensity.value);
                                AddOrReplaceKey(values[2],normalizedTime, comp.scatter.value);
                                AddOrReplaceKey(values[3],normalizedTime, comp.clamp.value);
                                AddOrReplaceKey(values[4],normalizedTime, comp.dirtIntensity.value);
                            }
                        }
                    }
                    break;
            }
        }
        //-----------------------------------------------------
        private static void AddOrReplaceKey(AnimationCurve curve, float time, float value)
        {
            if (curve == null) return;
            // 查找是否已存在该时间点的key
            int idx = -1;
            for (int i = 0; i < curve.length; ++i)
            {
                if (Mathf.Approximately(curve.keys[i].time, time))
                {
                    idx = i;
                    break;
                }
            }
            if (idx >= 0)
            {
                // 替换
                var key = curve.keys[idx];
                key.value = value;
                curve.MoveKey(idx, key);
            }
            else
            {
                // 新增
                curve.AddKey(new Keyframe(time, value));
            }
        }
#endif
    }
    //-----------------------------------------------------
    //缩放剪辑逻辑
    //-----------------------------------------------------
    public class PostProcessDriver : ACutsceneDriver
    {
        private VolumeProfile m_profile;
        private VolumeComponent m_volumeComponent;
        List<float> m_OriValues;
        List<Color> m_OriColors;
        List<Vector4> m_OriVectors;
        List<bool> m_OriToggles;
        //-----------------------------------------------------
        public override void OnDestroy()
        {
        }
        //-----------------------------------------------------
        public override bool OnClipEnter(CutsceneTrack pTrack, FrameData frameData)
        {
            ICutsceneObject pObj = pTrack.GetBindLastCutsceneObject();
            if (pObj != null && pObj.GetUniyTransform() != null)
            {
                var volume = pObj.GetUniyTransform().GetComponent<Volume>();
                if (volume != null)
                {
                    m_profile = volume.profile;
                    //! 记录原有参数状态
                    switch (frameData.clip.Cast<PostProcessClip>().postProcessType)
                    {
                        case PostProcessClip.EPostProcessType.Vignette:
                            m_profile.TryGet<Vignette>(out var vignette);
                            if (vignette != null)
                            {
                                m_volumeComponent = vignette;
                                m_OriColors = new List<Color>() { vignette.color.value };
                                m_OriVectors = new List<Vector4>() { new Vector4(vignette.center.value.x, vignette.center.value.y, 0, 0) };
                                m_OriValues = new List<float>() { vignette.intensity.value, vignette.smoothness.value };
                                m_OriToggles = new List<bool>() { vignette.rounded.value };
                            }
                            break;
                        case PostProcessClip.EPostProcessType.ChromaticAberration:
                            m_profile.TryGet<ChromaticAberration>(out var chroma);
                            if (chroma != null)
                            {
                                m_volumeComponent = chroma;
                                m_OriValues = new List<float>() { chroma.intensity.value };
                            }
                            break;
                        case PostProcessClip.EPostProcessType.ColorAdjustments:
                            m_profile.TryGet<ColorAdjustments>(out var colorAdj);
                            if (colorAdj != null)
                            {
                                m_volumeComponent = colorAdj;
                                m_OriColors = new List<Color>() { colorAdj.colorFilter.value };
                                m_OriValues = new List<float>() { colorAdj.postExposure.value, colorAdj.contrast.value, colorAdj.hueShift.value, colorAdj.saturation.value };
                            }
                            break;
                        case PostProcessClip.EPostProcessType.Bloom:
                            m_profile.TryGet<Bloom>(out var bloom);
                            if (bloom != null)
                            {
                                m_volumeComponent = bloom;
                                m_OriColors = new List<Color>() { bloom.tint.value };
                                m_OriValues = new List<float>() { bloom.threshold.value, bloom.intensity.value, bloom.scatter.value, bloom.clamp.value, bloom.dirtIntensity.value };
                            }
                            break;
                    }
                }
            }

            return true;
        }
        //-----------------------------------------------------
        public override bool OnClipLeave(CutsceneTrack pTrack, FrameData clip)
        {
            if (clip.CanRestore() || clip.IsLeaveIn())
            {
                //! 恢复原有参数状态
                if (m_volumeComponent != null)
                {
                    switch (clip.clip.Cast<PostProcessClip>().postProcessType)
                    {
                        case PostProcessClip.EPostProcessType.Vignette:
                            Vignette vignette = m_volumeComponent as Vignette;
                            if (vignette != null && m_OriColors != null && m_OriColors.Count > 0 && m_OriVectors != null && m_OriVectors.Count > 0 && m_OriValues != null && m_OriValues.Count > 1 && m_OriToggles != null && m_OriToggles.Count > 0)
                            {
                                vignette.color.value = m_OriColors[0];
                                vignette.center.value = new Vector2(m_OriVectors[0].x, m_OriVectors[0].y);
                                vignette.intensity.value = m_OriValues[0];
                                vignette.smoothness.value = m_OriValues[1];
                                vignette.rounded.value = m_OriToggles[0];
                            }
                            break;
                        case PostProcessClip.EPostProcessType.ChromaticAberration:
                            ChromaticAberration chroma = m_volumeComponent as ChromaticAberration;
                            if (chroma != null && m_OriValues != null && m_OriValues.Count > 0)
                            {
                                chroma.intensity.value = m_OriValues[0];
                            }
                            break;
                        case PostProcessClip.EPostProcessType.ColorAdjustments:
                            ColorAdjustments colorAdj = m_volumeComponent as ColorAdjustments;
                            if (colorAdj != null && m_OriColors != null && m_OriColors.Count > 0 && m_OriValues != null && m_OriValues.Count > 3)
                            {
                                colorAdj.colorFilter.value = m_OriColors[0];
                                colorAdj.postExposure.value = m_OriValues[0];
                                colorAdj.contrast.value = m_OriValues[1];
                                colorAdj.hueShift.value = m_OriValues[2];
                                colorAdj.saturation.value = m_OriValues[3];
                            }
                            break;
                        case PostProcessClip.EPostProcessType.Bloom:
                            Bloom bloom = m_volumeComponent as Bloom;
                            if (bloom != null && m_OriColors != null && m_OriColors.Count > 0 && m_OriValues != null && m_OriValues.Count > 4)
                            {
                                bloom.tint.value = m_OriColors[0];
                                bloom.threshold.value = m_OriValues[0];
                                bloom.intensity.value = m_OriValues[1];
                                bloom.scatter.value = m_OriValues[2];
                                bloom.clamp.value = m_OriValues[3];
                                bloom.dirtIntensity.value = m_OriValues[4];
                            }
                            break;
                    }
                }
            }
            return true;
        }
        //-----------------------------------------------------
        public override bool OnFrameClip(CutsceneTrack pTrack, FrameData frameData)
        {
            var clipData = frameData.clip.Cast<PostProcessClip>();
            if (m_profile != null)
            {
                switch(clipData.postProcessType)
                {
                    case PostProcessClip.EPostProcessType.Vignette:
                        {
                            if(m_volumeComponent == null)
                            {
                                m_profile.TryGet<Vignette>(out var comp);
                                m_volumeComponent = comp;
                            }
                            else
                            {
                                Vignette vignette = m_volumeComponent as Vignette;
                                if(vignette)
                                {
                                    if (clipData.colors != null && clipData.colors.Count > 0)
                                        vignette.color.value = clipData.colors[0];
                                    if (clipData.vectors != null && clipData.vectors.Count > 0)
                                        vignette.center.value = clipData.vectors[0];

                                    if (clipData.values != null && clipData.values.Count > 0 && clipData.values[0] != null && clipData.values[0].length > 0)
                                        vignette.intensity.value = clipData.values[0].Evaluate(frameData.subTime);

                                    if (clipData.values != null && clipData.values.Count > 1 && clipData.values[1] != null && clipData.values[1].length > 0)
                                        vignette.smoothness.value = clipData.values[1].Evaluate(frameData.subTime);

                                    if (clipData.toggles!=null && clipData.toggles.Count>0)
                                        vignette.rounded.value =clipData.toggles[0];
                                }
                            }
                        }
                        break;
                    case PostProcessClip.EPostProcessType.ColorAdjustments:
                        {
                            if (m_volumeComponent == null)
                            {
                                m_profile.TryGet<ColorAdjustments>(out var comp);
                                m_volumeComponent = comp;
                            }
                            else
                            {
                                ColorAdjustments colorAdj = m_volumeComponent as ColorAdjustments;
                                if (colorAdj)
                                {
                                    if (clipData.colors != null && clipData.colors.Count > 0)
                                        colorAdj.colorFilter.value = clipData.colors[0];
                                    if (clipData.values != null && clipData.values.Count > 0 && clipData.values[0] != null && clipData.values[0].length > 0)
                                        colorAdj.postExposure.value = clipData.values[0].Evaluate(frameData.subTime);
                                    if (clipData.values != null && clipData.values.Count > 1 && clipData.values[1] != null && clipData.values[1].length > 0)
                                        colorAdj.contrast.value = clipData.values[1].Evaluate(frameData.subTime);
                                    if (clipData.values != null && clipData.values.Count > 2 && clipData.values[2] != null && clipData.values[2].length > 0)
                                        colorAdj.hueShift.value = clipData.values[2].Evaluate(frameData.subTime);
                                    if (clipData.values != null && clipData.values.Count > 3 && clipData.values[3] != null && clipData.values[3].length > 0)
                                        colorAdj.saturation.value = clipData.values[3].Evaluate(frameData.subTime);
                                }
                            }
                        }
                        break;
                    case PostProcessClip.EPostProcessType.ChromaticAberration:
                        {
                            if (m_volumeComponent == null)
                            {
                                m_profile.TryGet<ChromaticAberration>(out var comp);
                                m_volumeComponent = comp;
                            }
                            else
                            {
                                ChromaticAberration chroma = m_volumeComponent as ChromaticAberration;
                                if (chroma)
                                {
                                    if (clipData.values != null && clipData.values.Count > 0 && clipData.values[0] != null && clipData.values[0].length > 0)
                                        chroma.intensity.value = clipData.values[0].Evaluate(frameData.subTime);
                                }
                            }
                        }
                        break;
                    case PostProcessClip.EPostProcessType.Bloom:
                        {
                            if (m_volumeComponent == null)
                            {
                                m_profile.TryGet<Bloom>(out var comp);
                                m_volumeComponent = comp;
                            }
                            else
                            {
                                Bloom bloom = m_volumeComponent as Bloom;
                                if (bloom)
                                {
                                    if (clipData.colors != null && clipData.colors.Count > 0)
                                        bloom.tint.value = clipData.colors[0];
                                    if (clipData.values != null && clipData.values.Count > 0 && clipData.values[0] != null && clipData.values[0].length > 0)
                                        bloom.threshold.value = clipData.values[0].Evaluate(frameData.subTime);
                                    if (clipData.values != null && clipData.values.Count > 1 && clipData.values[1] != null && clipData.values[1].length > 0)
                                        bloom.intensity.value = clipData.values[1].Evaluate(frameData.subTime);
                                    if (clipData.values != null && clipData.values.Count > 2 && clipData.values[2] != null && clipData.values[2].length > 0)
                                        bloom.scatter.value = clipData.values[2].Evaluate(frameData.subTime);
                                    if (clipData.values != null && clipData.values.Count > 3 && clipData.values[3] != null && clipData.values[3].length > 0)
                                        bloom.clamp.value = clipData.values[3].Evaluate(frameData.subTime);
                                    if (clipData.values != null && clipData.values.Count > 4 && clipData.values[4]!=null && clipData.values[4].length>0)
                                        bloom.dirtIntensity.value = clipData.values[4].Evaluate(frameData.subTime);
                                }
                            }
                        }
                        break;
                }
            }
            return true;
        }
    }
}