/********************************************************************
生成日期:	06:30:2025
类    名: 	AgentTreeData
作    者:	HappLI
描    述:	过场动画行为树
*********************************************************************/
#if UNITY_EDITOR
using Framework.AT.Editor;
#endif
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Framework.AT.Runtime
{
    //-----------------------------------------------------
    [System.Serializable]
    struct VaribaleSerizlizeGuidData
    {
        public VariableBool[]           boolVariables;
        public VariableInt[]            intVariables;
        public VariableLong[]           longVariables;
        public VariableFloat[]          floatVariables;
        public VariableDouble[]         doubleVariables;
        public VariableVec2[]           vec2Variables;
        public VariableVec3[]           vec3Variables;
        public VariableVec4[]           vec4Variables;
        public VariableRay[]            rayVariables;
        public VariableColor[]          colorVariables;
        public VariableQuaternion[]     quaternionVariables;
        public VariableBounds[]         boundsVariables;
        public VariableRect[]           rectVariables;
        public VariableMatrix[]         matrixVariables;
        public VariableString[]         stringVariables;

        public UnityEngine.Object[]     objectVariables;
        public int GetVariableCnt()
        {
            int cnt = 0;
            if(boolVariables!=null) cnt += boolVariables.Length;
            if (intVariables != null) cnt += intVariables.Length;
            if (longVariables != null) cnt += longVariables.Length;
            if (floatVariables != null) cnt += floatVariables.Length;
            if (doubleVariables != null) cnt += doubleVariables.Length;
            if (vec2Variables != null) cnt += vec2Variables.Length;
            if (vec3Variables != null) cnt += vec3Variables.Length;
            if (vec4Variables != null) cnt += vec4Variables.Length;
            if (rayVariables != null) cnt += rayVariables.Length;
            if (colorVariables != null) cnt += colorVariables.Length;
            if (quaternionVariables != null) cnt += quaternionVariables.Length;
            if (boundsVariables != null) cnt += boundsVariables.Length;
            if (rectVariables != null) cnt += rectVariables.Length;
            if (matrixVariables != null) cnt += matrixVariables.Length;
            if (stringVariables != null) cnt += stringVariables.Length;
            return cnt;
        }
        //-----------------------------------------------------
        internal void Fill(Dictionary<short, IVariable> vVariables)
        {
            if (boolVariables != null)
            {
                for (int i = 0; i < boolVariables.Length; ++i)
                {
                    vVariables[boolVariables[i].GetGuid()] = boolVariables[i];
                }
            }
            if (intVariables != null)
            {
                for (int i = 0; i < intVariables.Length; ++i)
                {
                    vVariables[intVariables[i].GetGuid()] = intVariables[i];
                }
            }
            if (longVariables != null)
            {
                for (int i = 0; i < longVariables.Length; ++i)
                {
                    vVariables[longVariables[i].GetGuid()] = longVariables[i];
                }
            }
            if (floatVariables != null)
            {
                for (int i = 0; i < floatVariables.Length; ++i)
                {
                    vVariables[floatVariables[i].GetGuid()] = floatVariables[i];
                }
            }
            if (doubleVariables != null)
            {
                for (int i = 0; i < doubleVariables.Length; ++i)
                {
                    vVariables[doubleVariables[i].GetGuid()] = doubleVariables[i];
                }
            }
            if (vec2Variables != null)
            {
                for (int i = 0; i < vec2Variables.Length; ++i)
                {
                    vVariables[vec2Variables[i].GetGuid()] = vec2Variables[i];
                }
            }
            if (vec3Variables != null)
            {
                for (int i = 0; i < vec3Variables.Length; ++i)
                {
                    vVariables[vec3Variables[i].GetGuid()] = vec3Variables[i];
                }
            }
            if (vec4Variables != null)
            {
                for (int i = 0; i < vec4Variables.Length; ++i)
                {
                    vVariables[vec4Variables[i].GetGuid()] = vec4Variables[i];
                }
            }
            if (rayVariables != null)
            {
                for (int i = 0; i < rayVariables.Length; ++i)
                {
                    vVariables[rayVariables[i].GetGuid()] = rayVariables[i];
                }
            }
            if (colorVariables != null)
            {
                for (int i = 0; i < colorVariables.Length; ++i)
                {
                    vVariables[colorVariables[i].GetGuid()] = colorVariables[i];
                }
            }
            if (this.quaternionVariables != null)
            {
                for (int i = 0; i < quaternionVariables.Length; ++i)
                {
                    vVariables[quaternionVariables[i].GetGuid()] = quaternionVariables[i];
                }
            }
            if (this.boundsVariables != null)
            {
                for (int i = 0; i < this.boundsVariables.Length; ++i)
                {
                    vVariables[this.boundsVariables[i].GetGuid()] = this.boundsVariables[i];
                }
            }
            if (this.rectVariables != null)
            {
                for (int i = 0; i < this.rectVariables.Length; ++i)
                {
                    vVariables[this.rectVariables[i].GetGuid()] = this.rectVariables[i];
                }
            }
            if (this.matrixVariables != null)
            {
                for (int i = 0; i < this.matrixVariables.Length; ++i)
                {
                    vVariables[this.matrixVariables[i].GetGuid()] = this.matrixVariables[i];
                }
            }
            if (this.stringVariables != null)
            {
                for (int i = 0; i < this.stringVariables.Length; ++i)
                {
                    vVariables[this.stringVariables[i].GetGuid()] = this.stringVariables[i];
                }
            }
        }
    }
}