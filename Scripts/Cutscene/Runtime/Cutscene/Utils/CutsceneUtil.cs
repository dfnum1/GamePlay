/********************************************************************
生成日期:	07:23:2025
类    名: 	CutsceneUtil
作    者:	HappLI
描    述:	工具类
*********************************************************************/
using Framework.AT.Runtime;
using UnityEngine;
namespace Framework.Cutscene.Runtime
{
    public class CutsceneUtil
    {
        public static bool RayInsectionFloor(out Vector3 retPos, Vector3 pos, Vector3 dir, float floorY = 0)
        {
            retPos = Vector3.zero;
            Vector3 vPlanePos = Vector3.zero;
            vPlanePos.y = floorY;

            Vector3 vPlaneNor = Vector3.up;

            float fdot = Vector3.Dot(dir, vPlaneNor);
            if (fdot == 0.0f)
                return false;

            float fRage = ((vPlanePos.x - pos.x) * vPlaneNor.x + (vPlanePos.y - pos.y) * vPlaneNor.y + (vPlanePos.z - pos.z) * vPlaneNor.z) / fdot;

            retPos = pos + dir * fRage;
            return true;
        }
        //-----------------------------------------------------
        public static Vector3 GetPosition(Matrix4x4 matrix)
        {
            return matrix.GetColumn(3);
        }
        //-----------------------------------------------------
        public static Transform Find(Transform pRoot, string path)
        {
            if (pRoot == null || string.IsNullOrEmpty(path)) return null;
            var result = pRoot.Find(path);
            if (result != null) return result;

            //! path 的第一个路径/ 将忽略查找
            var paths = path.Split('/');
            if (paths.Length > 1)
            {
                var newPath = string.Join("/", paths, 1, paths.Length - 1);
                for(int i =0; i < pRoot.childCount; ++i)
                {
                    result = pRoot.GetChild(i).Find(newPath);
                    if (result != null) return result;
                }
            }
            return null;
        }
        //-----------------------------------------------------
        internal static void FillCustomAgentParam(ref Variables variable, CutsceneCustomAgent.AgentUnit.ParamData[] paramValues)
        {
            if (paramValues.Length > 0)
            {
                variable.variables = VariableList.Malloc(paramValues.Length);
                for (int j = 0; j < paramValues.Length; ++j)
                {
                    var param = paramValues[j];
                    switch (param.type)
                    {
                        case EVariableType.eInt:
                            {
                                int v = 0;
                                int.TryParse(param.defaultValue, out v);
                                variable.variables.AddInt(v);
                            }
                            break;
                        case EVariableType.eFloat:
                            {
                                float v = 0;
                                float.TryParse(param.defaultValue, out v);
                                variable.variables.AddFloat(v);
                            }
                            break;
                        case EVariableType.eBool:
                            {
                                bool v = false;
                                bool.TryParse(param.defaultValue, out v);
                                variable.variables.AddBool(v);
                            }
                            break;
                        case EVariableType.eString:
                            {
                                variable.variables.AddString(param.defaultValue ?? string.Empty);
                            }
                            break;
                        case EVariableType.eVec2:
                            {
                                Vector2 v = Vector2.zero;
                                var split = (param.defaultValue ?? "").Split('|');
                                if (split.Length >= 2)
                                {
                                    float.TryParse(split[0], out v.x);
                                    float.TryParse(split[1], out v.y);
                                }
                                variable.variables.AddVec2(v);
                            }
                            break;
                        case EVariableType.eVec3:
                            {
                                Vector3 v = Vector3.zero;
                                var split = (param.defaultValue ?? "").Split('|');
                                if (split.Length >= 3)
                                {
                                    float.TryParse(split[0], out v.x);
                                    float.TryParse(split[1], out v.y);
                                    float.TryParse(split[2], out v.z);
                                }
                                variable.variables.AddVec3(v);
                            }
                            break;
                        case EVariableType.eVec4:
                            {
                                Vector4 v = Vector4.zero;
                                var split = (param.defaultValue ?? "").Split('|');
                                if (split.Length >= 4)
                                {
                                    float.TryParse(split[0], out v.x);
                                    float.TryParse(split[1], out v.y);
                                    float.TryParse(split[2], out v.z);
                                    float.TryParse(split[3], out v.w);
                                }
                                variable.variables.AddVec4(v);
                            }
                            break;
                        case EVariableType.eObjId:
                            {
                                ObjId obj = new ObjId();
                                int.TryParse(param.defaultValue, out obj.id);
                                variable.variables.AddObjId(obj);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}