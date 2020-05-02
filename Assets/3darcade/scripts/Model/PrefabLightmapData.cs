using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Arcade
{
    public class PrefabLightmapData : MonoBehaviour
    {
        [System.Serializable]
        struct RendererInfo
        {
            public Renderer renderer;
            public int lightmapIndex;
            public Vector4 lightmapOffsetScale;
        }

        [SerializeField]
        public bool updateLightmaps = false;

        [SerializeField]
        RendererInfo[] m_RendererInfo;
        [SerializeField]
        Texture2D[] m_Lightmaps;

        private void Awake()
        {
            if (m_RendererInfo == null || m_RendererInfo.Length == 0)
            {
                return;
            }

            LightmapData[] lightmaps = LightmapSettings.lightmaps;
            int[] offsetsindexes = new int[m_Lightmaps.Length];
            int counttotal = lightmaps.Length;
            List<LightmapData> combinedLightmaps = new List<LightmapData>();

            for (int i = 0; i < m_Lightmaps.Length; i++)
            {
                bool exists = false;
                for (int j = 0; j < lightmaps.Length; j++)
                {
                    if (m_Lightmaps[i] == lightmaps[j].lightmapColor)
                    {
                        exists = true;
                        offsetsindexes[i] = j;

                    }
                }
                if (!exists)
                {
                    offsetsindexes[i] = counttotal;
                    LightmapData newlightmapdata = new LightmapData
                    {
                        lightmapColor = m_Lightmaps[i]
                    };
                    combinedLightmaps.Add(newlightmapdata);
                    counttotal += 1;
                }
            }

            LightmapData[] combinedLightmaps2 = new LightmapData[counttotal];
            lightmaps.CopyTo(combinedLightmaps2, 0);
            combinedLightmaps.ToArray().CopyTo(combinedLightmaps2, lightmaps.Length);
            LightmapSettings.lightmapsMode = LightmapsMode.NonDirectional;
            ApplyRendererInfo(m_RendererInfo, offsetsindexes);
            LightmapSettings.lightmaps = combinedLightmaps2;
        }

        static void ApplyRendererInfo(RendererInfo[] infos, int[] lightmapOffsetIndex)
        {
            for (int i = 0; i < infos.Length; i++)
            {
                RendererInfo info = infos[i];
                info.renderer.lightmapIndex = lightmapOffsetIndex[info.lightmapIndex];
                info.renderer.lightmapScaleOffset = info.lightmapOffsetScale;
            }
        }

#if UNITY_EDITOR
        [MenuItem("3DArcade/Save Baked Lightmaps to Prefabs", false, 10000)]
        static void SaveLightmapInfo()
        {
            if (Lightmapping.giWorkflowMode != Lightmapping.GIWorkflowMode.OnDemand)
            {
                Debug.LogError("Extracting Lightmap data requires that you have baked your lightmaps and Auto baking mode is disabled.");
                return;
            }
            //UnityEditor.Lightmapping.Bake();

            PrefabLightmapData[] prefabsLightmapData = FindObjectsOfType<PrefabLightmapData>();

            foreach (PrefabLightmapData prefabLightmapData in prefabsLightmapData)
            {
                GameObject gameObject = prefabLightmapData.gameObject;
                if (prefabLightmapData != null)
                {
                    if (!prefabLightmapData.updateLightmaps)
                    {
                        continue;
                    }
                }
                print("Saving baked Lightmaps? " + gameObject.name);
                GenerateLightmapInfo(gameObject);

            }
        }

        [MenuItem("CONTEXT/PrefabLightmapData/Save Baked Lightmaps to Prefab")]
        static void MenuOptionBakeLightmaps(MenuCommand menuCommand)
        {

            if (Lightmapping.giWorkflowMode != Lightmapping.GIWorkflowMode.OnDemand)
            {
                Debug.LogError("Extracting Lightmap data requires that you have baked your lightmaps and Auto baking mode is disabled.");
                return;
            }

            PrefabLightmapData lightmapData = menuCommand.context as PrefabLightmapData;
            GameObject gameObject = lightmapData.gameObject;
            print("Saving baked Lightmaps? " + gameObject.name);
            GenerateLightmapInfo(gameObject);

        }

        //[MenuItem("Assets/Bake Lightmapsx")]
        //private static void MenuOptionAssetsBakeLightmaps(MenuCommand menuCommand)
        //{
        //    var gameObject = Selection.activeObject as GameObject;
        //    print("Saving baked Lightmaps? " + gameObject.name);
        //    GenerateLightmapInfo(gameObject);

        //}
        //[MenuItem("Assets/Bake Lightmapsx", true)]
        //private static bool MenuOptionAssetsBakeLightmapsValidation(MenuCommand menuCommand)
        //{
        //    return AssetDatabase.GetAssetPath(Selection.activeObject).Contains("Assets/Resources") ? true : false;
        //}

        static void GenerateLightmapInfo(GameObject root)
        {
            ModelSetup modelSetup = root.GetComponent<ModelSetup>();
            if (modelSetup == null)
            { return; }
            PrefabLightmapData prefabLightmapData = root.GetComponent<PrefabLightmapData>();
            if (prefabLightmapData == null)
            { return; }
            List<RendererInfo> rendererInfos = new List<RendererInfo>();
            List<Texture2D> lightmaps = new List<Texture2D>();
            MeshRenderer[] renderers = root.GetComponentsInChildren<MeshRenderer>();
            print("Renderers count " + renderers.Length);
            if (renderers.Length < 1)
            { return; }
            foreach (MeshRenderer renderer in renderers)
            {
                if (renderer.lightmapIndex != -1)
                {
                    RendererInfo info = new RendererInfo
                    {
                        renderer = renderer,
                        lightmapOffsetScale = renderer.lightmapScaleOffset
                    };

                    Texture2D lightmap = LightmapSettings.lightmaps[renderer.lightmapIndex].lightmapColor;

                    info.lightmapIndex = lightmaps.IndexOf(lightmap);
                    if (info.lightmapIndex == -1)
                    {
                        info.lightmapIndex = lightmaps.Count;
                        lightmaps.Add(lightmap);
                    }

                    rendererInfos.Add(info);
                }
            }
            print("lightmaps count " + lightmaps.Count);
            for (int i = 0; i < lightmaps.Count; i++)
            {
                string[] asset = AssetDatabase.FindAssets(lightmaps[i].name);
                if (asset.Length > 0)
                {
                    if (modelSetup != null)
                    {
                        string name = modelSetup.id + "_" + lightmaps[i].name;
                        _ = AssetDatabase.RenameAsset(AssetDatabase.GUIDToAssetPath(asset[0]), name);
                        lightmaps[i].name = name;
                    }
                }
            }
            prefabLightmapData.m_RendererInfo = rendererInfos.ToArray();
            prefabLightmapData.m_Lightmaps = lightmaps.ToArray();

            GameObject targetPrefab = PrefabUtility.GetCorrespondingObjectFromSource(root) as GameObject;
            if (targetPrefab != null)
            {
                print("target prefab " + targetPrefab.name + " for gameobject " + root.name);
                string path = AssetDatabase.GetAssetPath(targetPrefab);
                print(path);
                if (PrefabUtility.SaveAsPrefabAsset(root, path))
                {
                    print("Saved lightmap changes for" + root.name + "!");
                }
                else
                {
                    print("Failed to save Lightmapchanges for" + root.name + "!");
                }
            }


        }
#endif
    }
}
