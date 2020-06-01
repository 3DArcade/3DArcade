/* MIT License

 * Copyright (c) 2020 Skurdt
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:

 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.

 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE. */

using Cinemachine;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Arcade_r
{
    public static class ArcadeController
    {
        private static readonly string[] RESOURCES_SUB_DIRECTORIES = new [] { "Arcades", "Games", "Props" };
        private static readonly string[] GAME_RESOURCES_DIRECTORY  = new [] { "Games" };

        public static void StartArcade(ArcadeConfiguration configuration, Transform root, Transform player)
        {
            if (root.TryGetComponent(out ArcadeConfigurationComponent arcadeConfigurationComponent))
            {
                arcadeConfigurationComponent.FromArcadeConfiguration(configuration);
            }
            else
            {
                root.gameObject.AddComponent<ArcadeConfigurationComponent>()
                               .FromArcadeConfiguration(configuration);
            }

            SetupPlayer(player, configuration.CameraSettings.Position, configuration.CameraSettings.Rotation, configuration.CameraSettings.Height);

            AddModelsToWorld<ArcadeModelSetup>(configuration.ArcadeModelList, configuration.RenderSettings, root.GetChild(0));
            AddModelsToWorld<GameModelSetup>(configuration.GameModelList, configuration.RenderSettings, root.GetChild(1));
            AddModelsToWorld<PropModelSetup>(configuration.PropModelList, configuration.RenderSettings, root.GetChild(2));
        }

        private static void SetupPlayer(Transform player, Vector3 position, Vector3 rotation, float height)
        {
            player.SetPositionAndRotation(position, Quaternion.Euler(0f, rotation.y, 0f));
            CinemachineVirtualCamera vCam    = player.GetComponentInChildren<CinemachineVirtualCamera>();
            CinemachineTransposer transposer = vCam.GetCinemachineComponent<CinemachineTransposer>();
            transposer.m_FollowOffset.y      = height;
        }

        private static void AddModelsToWorld<T>(ModelConfiguration[] models, RenderSettings renderSettings, Transform parent)
            where T : ModelSetup
        {
            foreach (ModelConfiguration modelConfiguration in models)
            {
                GameObject prefab = AssetManager.LoadPrefab(RESOURCES_SUB_DIRECTORIES, modelConfiguration.Model, modelConfiguration.Id, modelConfiguration.IdParent);
                if (prefab == null)
                {
                    // Generic model
                    bool isVertical = modelConfiguration.Screen.ToLowerInvariant().Contains("vertical");
                    _ = int.TryParse(modelConfiguration.Year, out int year);
                    if (year >= 1970 && year < 1980)
                    {
                        prefab = AssetManager.LoadPrefab(GAME_RESOURCES_DIRECTORY, isVertical ? "default70vert" : "default70hor");
                    }
                    else if (year < 1990)
                    {
                        prefab = AssetManager.LoadPrefab(GAME_RESOURCES_DIRECTORY, isVertical ? "default80vert" : "default80hor");
                    }
                    else if (year < 2000)
                    {
                        prefab = AssetManager.LoadPrefab(GAME_RESOURCES_DIRECTORY, isVertical ? "default90vert" : "default90hor");
                    }
                    else
                    {
                        prefab = AssetManager.LoadPrefab(GAME_RESOURCES_DIRECTORY, "default80hor");
                    }
                }

                MaterialUtils.SetGPUInstancing(true, prefab);
                InstantiatePrefab<T>(prefab, parent, modelConfiguration, renderSettings);
            }
        }

        private static void InstantiatePrefab<T>(GameObject prefab, Transform parent, ModelConfiguration modelConfiguration, RenderSettings renderSettings)
            where T : ModelSetup
        {
            GameObject model = Object.Instantiate(prefab, modelConfiguration.Position, Quaternion.Euler(modelConfiguration.Rotation), parent);
            model.StripCloneFromName();
            model.transform.localScale = modelConfiguration.Scale;
            model.transform.SetLayersRecursively(parent.gameObject.layer);

            model.AddComponent<T>()
                 .FromModelConfiguration(modelConfiguration);

            // Only look for artworks at runtime
            if (!Application.isPlaying)
            {
                return;
            }

            string marqueeImagePath = $"{Application.streamingAssetsPath}/3darcade_r~/Emulators/mame/marquees/{modelConfiguration.Id}.png";
            Texture2D marqueeTexture = TextureUtils.LoadTextureFromFile(marqueeImagePath, true);

            string screenImagePath = $"{Application.streamingAssetsPath}/3darcade_r~/Emulators/mame/snap/{modelConfiguration.Id}.png";
            Texture2D screenTexture = TextureUtils.LoadTextureFromFile(screenImagePath, false);
            float screenIntensity;
            if (modelConfiguration.Genre.ToLower().Contains("vector"))
            {
                screenIntensity = renderSettings.ScreenVectorIntenstity;
            }
            else if (modelConfiguration.Screen.ToLower().Contains("pinball"))
            {
                screenIntensity = renderSettings.ScreenPinballIntensity;
            }
            else
            {
                screenIntensity = renderSettings.ScreenRasterIntensity;
            }

            string genericImagePath = $"{Application.streamingAssetsPath}/3darcade_r~/Emulators/mame/cabinets/{modelConfiguration.Id}.png";
            Texture2D genericTexture = TextureUtils.LoadTextureFromFile(genericImagePath, true);

            SetupDynamicNode<MarqueeNodeTag>(model, marqueeTexture, true, renderSettings.MarqueeIntensity);
            SetupDynamicNode<ScreenNodeTag>(model, screenTexture, true, screenIntensity);
            SetupDynamicNode<GenericNodeTag>(model, genericTexture);
        }

        private static void SetupDynamicNode<T>(GameObject model, Texture2D texture, bool overwriteColor = false, float emissionFactor = 1f)
            where T : NodeTag
        {
            if (model == null || texture == null)
            {
                return;
            }

            if (!TryGetMaterialForNode<T>(model, out Material material))
            {
                return;
            }

            // Video
            // ...

            // Image
            if (material.IsKeywordEnabled(MaterialUtils.SHADER_EMISSIVEKEYWORD))
            {
                Color color = overwriteColor ? Color.white : material.GetColor(MaterialUtils.SHADER_EMISSIVECOLOR_NAME) * emissionFactor;
                material.SetEmissionColorAndTexture(color, texture, true);
            }
            else
            {
                Color color = overwriteColor ? Color.white : material.GetColor(MaterialUtils.SHADER_MAINCOLOR_NAME);
                material.SetAlbedoColorAndTexture(color, texture);
            }

            // Magic cabs
            Renderer[] modelRenderers = model.GetComponentsInChildren<Renderer>();
        }

        [SuppressMessage("Type Safety", "UNT0014:Invalid type for call to GetComponent", Justification = "Analyzer is dumb")]
        private static bool TryGetMaterialForNode<T>(GameObject model, out Material material)
            where T : NodeTag
        {
            T nodeTag = model.GetComponentInChildren<T>();
            if (nodeTag != null)
            {
                Renderer renderer = nodeTag.GetComponent<Renderer>();
                if (renderer != null)
                {
                    material = renderer.material;
                    return true;
                }
            }

            material = null;
            return false;
        }
    }
}
