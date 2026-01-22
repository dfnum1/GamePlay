/********************************************************************
生成日期:		1:11:2020 10:09
类    名: 	ParticleSortLayerSetterEditorLogic
作    者:	HappLI
描    述:	特效排序编辑
*********************************************************************/
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Text;
using System.Linq;

namespace Framework.ED
{
	public class FxInfo
	{
		public string PrefabPath = "";
		public string FxTransformPath = null;
		public bool IsMeshRenderer = false;
	}

	public class ParticleSortLayerSetterEditorLogic
    {
        float m_fPopTipsDelta = 0;
        string m_PopTipMessage = "";

		const string strBasePathDefault = "Assets/Res/Effect/scene";
		string m_strBasePath = strBasePathDefault;

		int m_nBeginIndex = 1;
        Dictionary<Material, List<FxInfo>> m_MaterialToFxList = new Dictionary<Material, List<FxInfo>>();
        //-----------------------------------------------------
        public void OnEnable()
        {
            
        }
        //-----------------------------------------------------
        public void OnDisable()
        {
            GUI.FocusControl("");
            EditorUtility.ClearProgressBar();
        }
        //-----------------------------------------------------
        public void OnDrawLayerPanel()
        {
			GUILayout.BeginHorizontal();
			GUILayout.Label("粒子预制体根目录:");
			m_strBasePath = GUILayout.TextField(m_strBasePath);
			if (GUILayout.Button("..."))
			{
				string path = EditorUtility.OpenFolderPanel("简化后数据存储路径", "", "");
				if (path != "" && path.StartsWith(Application.dataPath))
				{
					m_strBasePath = string.Format("Assets{0}", path.Substring(Application.dataPath.Length, path.Length - Application.dataPath.Length));

				}
			}
			GUILayout.EndHorizontal();

			m_nBeginIndex = EditorGUILayout.IntField("启始层级节点", m_nBeginIndex);
			if (m_nBeginIndex == 0)
			{
				Debug.LogWarning("Unity自身存在BUG SortingLayer设置为0不会合批，强制调节为1");
				m_nBeginIndex = 1;
			}

			if (GUILayout.Button("排序"))
			{
				CollectFileData();
			}
		}
        //-----------------------------------------------------

		string BuildTransformPath(Transform cur, Transform root = null)
		{
			if (cur == root)
				return "";

			string rt = cur.name;

			cur = cur.parent;

			while (cur != null && cur != root)
			{
				rt = string.Format("{0}/{1}", cur.name, rt);
				cur = cur.parent;
			}

			if (rt.EndsWith(" (UnityEngine.Transform)"))
			{
				rt = rt.Substring(0, rt.Length - " (UnityEngine.Transform)".Length);
			}

			return rt;
		}

        //-----------------------------------------------------
        void CollectFileData()
		{
			m_MaterialToFxList.Clear();

			List<string> withoutExtensions = new List<string>() { ".prefab" };
			string[] files = Directory.GetFiles(Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length) + m_strBasePath, "*.*", SearchOption.AllDirectories)
				.Where(s => withoutExtensions.Contains(Path.GetExtension(s).ToLower())).ToArray();

			int startIndex = 0;

			EditorApplication.update = delegate ()
			{
				string file = files[startIndex];

				file = file.Substring(Application.dataPath.Length - "Assets".Length, file.Length - Application.dataPath.Length + "Assets".Length);

				bool isCancel = EditorUtility.DisplayCancelableProgressBar("搜寻特效文件", file, (float)startIndex / (float)files.Length);

				//var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(file);

				var obj = AssetDatabase.LoadAssetAtPath(file.Replace('\\','/'), typeof(UnityEngine.Object));

				GameObject prefab = obj as GameObject;

				if (prefab != null)
				{
					var particleRendereres = prefab.GetComponentsInChildren<ParticleSystemRenderer>();

					for (int i = 0; i < particleRendereres.Length; ++i)
					{
						var particleRenderer = particleRendereres[i];

						if (particleRenderer.sharedMaterial == null)
							continue;

						List<FxInfo> fxInfos = null;
						if (!m_MaterialToFxList.TryGetValue(particleRenderer.sharedMaterial, out fxInfos))
						{
							fxInfos = new List<FxInfo>();
							m_MaterialToFxList.Add(particleRenderer.sharedMaterial, fxInfos);
						}

						FxInfo info = new FxInfo();
						info.PrefabPath = file;
						info.FxTransformPath = BuildTransformPath(particleRenderer.transform, prefab.transform);

						if (fxInfos.Find(infoInList =>
						{
							return infoInList.PrefabPath.CompareTo(info.PrefabPath) == 0
								&& infoInList.FxTransformPath.CompareTo(info.FxTransformPath) == 0;
						}) == null)
						{
							fxInfos.Add(info);
						}
					}

					var meshRendereres = prefab.GetComponentsInChildren<MeshRenderer>();

					for (int i = 0; i < meshRendereres.Length; ++i)
					{
						var meshRenderer = meshRendereres[i];

						if (meshRenderer.sharedMaterial == null)
							continue;

						List<FxInfo> fxInfos = null;
						if (!m_MaterialToFxList.TryGetValue(meshRenderer.sharedMaterial, out fxInfos))
						{
							fxInfos = new List<FxInfo>();
							m_MaterialToFxList.Add(meshRenderer.sharedMaterial, fxInfos);
						}

						FxInfo info = new FxInfo();
						info.PrefabPath = file;
						info.IsMeshRenderer = true;
						info.FxTransformPath = BuildTransformPath(meshRenderer.transform, prefab.transform);

						if (fxInfos.Find(infoInList =>
						{
							return infoInList.PrefabPath.CompareTo(info.PrefabPath) == 0
								&& infoInList.FxTransformPath.CompareTo(info.FxTransformPath) == 0;
						}) == null)
						{
							fxInfos.Add(info);
						}
					}

				}

				startIndex++;
				if (isCancel || startIndex >= files.Length)
				{
					EditorUtility.ClearProgressBar();
					EditorApplication.update = null;
					startIndex = 0;
					Debug.Log("搜寻结束");

					SettingSortingLayer();
				}

			};
		}
        //-----------------------------------------------------
        void SettingSortingLayer()
		{
			if (m_MaterialToFxList.Count == 0)
				return;

			List<FxInfo>[] datas = new List<FxInfo>[m_MaterialToFxList.Values.Count];
			m_MaterialToFxList.Values.CopyTo(datas, 0);

			if (m_nBeginIndex == 0)
				m_nBeginIndex = 1;

			int curIndex = m_nBeginIndex;
			int dataIndex = 0;

			EditorApplication.update = delegate ()
			{
				var value = datas[dataIndex];

				bool isCancel = false;

				for (int i = 0; i < value.Count; ++i)
				{
					isCancel |= EditorUtility.DisplayCancelableProgressBar("替换特效文件渲染层级", value[i].PrefabPath, (float)dataIndex / (float)datas.Length);

					var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(value[i].PrefabPath);
					Transform transform = prefab.transform;
					if (!string.IsNullOrEmpty(value[i].FxTransformPath))
					{
						transform = prefab.transform.Find(value[i].FxTransformPath);
					}

					if (transform == null)
					{
						Debug.LogErrorFormat(string.Format("transform {0} is not exsit in prefab {1}!", value[i].FxTransformPath, value[i].PrefabPath));
						break;
					}

					//if (value[i].IsMeshRenderer)
					//{
					//	var renderer = transform.GetComponent<MeshRenderer>();
					//	var sorter = transform.GetComponent<MeshRenderSorter>();
					//	if (renderer == null)
					//	{
					//		continue;
					//	}

					//	if (sorter == null)
					//		sorter = transform.gameObject.AddComponent<MeshRenderSorter>();

					//	sorter.sortingOrder = curIndex;

					//	renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
					//	renderer.receiveShadows = false;
					//	renderer.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
					//}
					//else
					{
						var renderer = transform.GetComponent<ParticleSystemRenderer>();
						if (renderer == null)
							continue;
						bool bDirty = false;
						if(renderer.sortingOrder != curIndex)
						{
                            renderer.sortingOrder = curIndex;
							bDirty = true;
                        }

						if (renderer.shadowCastingMode != UnityEngine.Rendering.ShadowCastingMode.Off)
						{
							renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                            bDirty = true;
                        }
						if(renderer.receiveShadows)
						{
                            renderer.receiveShadows = false;
                            bDirty = true;
                        }
						if(renderer.motionVectorGenerationMode != MotionVectorGenerationMode.ForceNoMotion)
						{
                            renderer.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
                            bDirty = true;
                        }
						if(bDirty)
						{
                            EditorUtility.SetDirty(prefab);
                        }
                    }

					if (isCancel)
					{
						break;
					}
				}

				++dataIndex;
				++curIndex;

				if (isCancel || dataIndex >= datas.Length)
				{
					EditorUtility.ClearProgressBar();
					EditorApplication.update = null;
					Debug.Log(string.Format("排序结束，最后一个索引值为{0}", dataIndex));
					m_nBeginIndex = dataIndex + 1;
					dataIndex = 0;

                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
                }
			};
		}
	}
}
