/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	Projectile
作    者:	HappLI
描    述:	飞行道具
*********************************************************************/

#if USE_FIXEDMATH
using ExternEngine;
#else
using FFloat = System.Single;
using FVector3 = UnityEngine.Vector3;
#endif
using System.Collections.Generic;
using UnityEngine;
using Framework.DrawProps;
using Framework.ActorSystem.Runtime;


#if USE_SERVER
using AudioClip = ExternEngine.AudioClip;
using Transform = ExternEngine.Transform;
#endif

#if UNITY_EDITOR
using System.Linq;
#endif

namespace Framework.ActorSystem.Runtime
{
    //------------------------------------------------------
    [CreateAssetMenu(menuName = "GamePlay/ProjectileDatas")]
    public class ProjectileDatas : ScriptableObject
    {
        public ProjectileData[] projectiles;
        //-----------------------------------------------------
        public Dictionary<uint, ProjectileData> GetDatas(Dictionary<uint, ProjectileData> projectileDatas = null)
        {
            if(projectileDatas == null) projectileDatas = new Dictionary<uint, ProjectileData>();
            if (projectiles == null)
                return projectileDatas;

            for(int i =0; i < projectiles.Length; ++i)
            {
                projectileDatas[projectiles[i].id] = projectiles[i];
            }
            return projectileDatas;
        }
        //-----------------------------------------------------
#if UNITY_EDITOR
        void Refresh()
        {
            string projectileFileRoot = UnityEditor.AssetDatabase.GetAssetPath(this);
            if (string.IsNullOrEmpty(projectileFileRoot))
                return;
            projectileFileRoot = projectileFileRoot.Replace("\\", "/");
            var files = System.IO.Directory.GetFiles(projectileFileRoot, "*.json", System.IO.SearchOption.AllDirectories);
            List<ProjectileData> vDatas = new List<ProjectileData>();
            for (int i = 0; i < files.Length; ++i)
            {
                try
                {
                    ProjectileData projeileData = JsonUtility.FromJson<ProjectileData>(System.IO.File.ReadAllText(files[i]));
                    if (projeileData != null)
                        vDatas.Add(projeileData);
                }
                catch (System.Exception expec)
                {
                    string file = files[i].Replace("\\", "/").Replace(projectileFileRoot, "");
                    Debug.LogError($"file {file} data convert ProjectileData error: {expec.Message}");
                }
            }
            projectiles = vDatas.ToArray();
            UnityEditor.EditorUtility.SetDirty(this);
        }
        //-----------------------------------------------------
        public static void RefreshDatas(ProjectileDatas projectileData = null)
        {
            if(projectileData!=null)
            {
                projectileData.Refresh();
                UnityEditor.EditorUtility.SetDirty(projectileData);
                UnityEditor.AssetDatabase.SaveAssetIfDirty(projectileData);
                return;
            }
            string[] projectileDataGuids = UnityEditor.AssetDatabase.FindAssets("t:ProjectileDatas");
            if (projectileDataGuids == null || projectileDataGuids.Length<=0)
                return;
            var projectiles = UnityEditor.AssetDatabase.LoadAssetAtPath<ProjectileDatas>(UnityEditor.AssetDatabase.GUIDToAssetPath(projectileDataGuids[0]));
            if (projectiles == null)
                return;

            projectiles.Refresh();

            UnityEditor.AssetDatabase.SaveAssetIfDirty(projectileData);
        }
#endif
    }
}

