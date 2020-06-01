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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;

namespace Arcade_r
{
    public static class ArcadeController
    {
        private static readonly string[] RESOURCES_SUB_DIRECTORIES = new [] { "Arcades", "Games", "Props" };
        private static readonly string[] GAME_RESOURCES_DIRECTORY  = new [] { "Games" };

        public static void StartArcade(ArcadeConfiguration configuration, ArcadeHierarchy arcadeHierarchy, Transform player)
        {
            if (arcadeHierarchy.RootNode.TryGetComponent(out ArcadeConfigurationComponent arcadeConfigurationComponent))
            {
                arcadeConfigurationComponent.FromArcadeConfiguration(configuration);
            }
            else
            {
                arcadeHierarchy.RootNode.AddComponent<ArcadeConfigurationComponent>()
                                        .FromArcadeConfiguration(configuration);
            }

            SetupPlayer(player, configuration.CameraSettings.Position, configuration.CameraSettings.Rotation, configuration.CameraSettings.Height);

            AddModelsToWorld<ArcadeModelSetup>(configuration.ArcadeModelList, configuration.RenderSettings, arcadeHierarchy.ArcadesNode);
            AddModelsToWorld<GameModelSetup>(configuration.GameModelList, configuration.RenderSettings, arcadeHierarchy.GamesNode);
            AddModelsToWorld<PropModelSetup>(configuration.PropModelList, configuration.RenderSettings, arcadeHierarchy.PropsNode);
        }

        private static void SetupPlayer(Transform player, Vector3 position, Vector3 rotation, float height)
        {
            player.SetPositionAndRotation(position, Quaternion.Euler(0f, rotation.y, 0f));
            CinemachineVirtualCamera vCam    = player.GetComponentInChildren<CinemachineVirtualCamera>();
            CinemachineTransposer transposer = vCam.GetCinemachineComponent<CinemachineTransposer>();
            transposer.m_FollowOffset.y      = height;
        }

        private static void AddModelsToWorld<T>(ModelConfiguration[] models, RenderSettings renderSettings, GameObject parent)
            where T : ModelSetup
        {
            foreach (ModelConfiguration modelConfiguration in models)
            {
                GameObject prefab = AssetManager.LoadPrefab(RESOURCES_SUB_DIRECTORIES, modelConfiguration.Model, modelConfiguration.Id, modelConfiguration.IdParent);
                if (prefab == null)
                {
                    // Generic model
                    if (int.TryParse(modelConfiguration.Year, out int year))
                    {
                        bool isVertical = modelConfiguration.Screen.ToLowerInvariant().Contains("vertical");

                        string prefabName;
                        if (year >= 1970 && year < 1980)
                        {
                            prefabName = isVertical ? "default70vert" : "default70hor";
                        }
                        else if (year < 1990)
                        {
                            prefabName = isVertical ? "default80vert" : "default80hor";
                        }
                        else if (year < 2000)
                        {
                            prefabName = isVertical ? "default90vert" : "default90hor";
                        }
                        else
                        {
                            prefabName = "default80hor";
                        }

                        prefab = AssetManager.LoadPrefab(GAME_RESOURCES_DIRECTORY, prefabName);
                    }
                }

                if (prefab == null)
                {
                    continue;
                }

                GameObject instantiatedModel = InstantiatePrefab<T>(prefab, parent, modelConfiguration);

                // Only look for artworks in play mode / at runtime
                if (Application.isPlaying)
                {
                    SetupMarqueeNode(instantiatedModel, modelConfiguration, renderSettings);
                    SetupScreenNode(instantiatedModel, modelConfiguration, renderSettings);
                    SetupGenericNode(instantiatedModel, modelConfiguration);
                }
            }
        }

        private static GameObject InstantiatePrefab<T>(GameObject prefab, GameObject parent, ModelConfiguration modelConfiguration)
            where T : ModelSetup
        {
            GameObject model = Object.Instantiate(prefab, modelConfiguration.Position, Quaternion.Euler(modelConfiguration.Rotation), parent.transform);
            model.StripCloneFromName();
            model.transform.localScale = modelConfiguration.Scale;
            model.transform.SetLayersRecursively(parent.layer);
            model.AddComponent<T>()
                 .FromModelConfiguration(modelConfiguration);
            return model;
        }

        private static void SetupMarqueeNode(GameObject model, ModelConfiguration modelConfiguration, RenderSettings renderSettings)
        {
            Renderer nodeRenderer = GetDynamicNodeRenderer<MarqueeNodeTag>(model);
            if (nodeRenderer == null)
            {
                return;
            }

            string imagePath  = $"{Application.streamingAssetsPath}/3darcade_r~/Emulators/mame/marquees/{modelConfiguration.Id}.png";
            Texture2D texture = TextureUtils.LoadTextureFromFile(imagePath, true);
            if (texture == null)
            {
                return;
            }

            SetupDynamicNode<MarqueeNodeTag>(nodeRenderer, texture, true, true, renderSettings.MarqueeIntensity);
            SetupMagicPixels(nodeRenderer);
        }

        private static void SetupScreenNode(GameObject model, ModelConfiguration modelConfiguration, RenderSettings renderSettings)
        {
            Renderer nodeRenderer = GetDynamicNodeRenderer<ScreenNodeTag>(model);
            if (nodeRenderer == null)
            {
                return;
            }

            string imagePath  = $"{Application.streamingAssetsPath}/3darcade_r~/Emulators/mame/snap/{modelConfiguration.Id}.png";
            Texture2D texture = TextureUtils.LoadTextureFromFile(imagePath, false);
            if (texture == null)
            {
                return;
            }

            float intensity;
            if (modelConfiguration.Screen.ToLowerInvariant().Contains("vector"))
            {
                intensity = renderSettings.ScreenVectorIntenstity;
            }
            else if (modelConfiguration.Screen.ToLowerInvariant().Contains("pinball"))
            {
                intensity = renderSettings.ScreenPinballIntensity;
            }
            else
            {
                intensity = renderSettings.ScreenRasterIntensity;
            }

            SetupDynamicNode<ScreenNodeTag>(nodeRenderer, texture, true, true, intensity);
        }

        private static void SetupGenericNode(GameObject model, ModelConfiguration modelConfiguration)
        {
            Renderer nodeRenderer = GetDynamicNodeRenderer<GenericNodeTag>(model);
            if (nodeRenderer == null)
            {
                return;
            }

            string imagePath  = $"{Application.streamingAssetsPath}/3darcade_r~/Emulators/mame/cabinets/{modelConfiguration.Id}.png";
            Texture2D texture = TextureUtils.LoadTextureFromFile(imagePath, true);
            if (texture == null)
            {
                return;
            }

            SetupDynamicNode<GenericNodeTag>(nodeRenderer, texture);
        }

        [SuppressMessage("Type Safety", "UNT0014:Invalid type for call to GetComponent", Justification = "Analyzer is dumb")]
        private static Renderer GetDynamicNodeRenderer<T>(GameObject model)
            where T : NodeTag
        {
            T nodeTag = model.GetComponentInChildren<T>();
            if (nodeTag != null)
            {
                return nodeTag.GetComponent<Renderer>();
            }
            return null;
        }

        [SuppressMessage("Type Safety", "UNT0014:Invalid type for call to GetComponent", Justification = "Analyzer is dumb")]
        private static void SetupDynamicNode<T>(Renderer renderer, Texture2D texture, bool overwriteColor = false, bool forceEmissive = false, float emissionFactor = 1f)
            where T : NodeTag
        {
            // Video
            // ...

            // Image
            if (forceEmissive || renderer.material.IsKeywordEnabled(MaterialUtils.SHADER_EMISSIVEKEYWORD))
            {
                Color color = (overwriteColor ? Color.white : renderer.material.GetColor(MaterialUtils.SHADER_EMISSIVECOLOR_NAME)) * emissionFactor;
                renderer.material.SetEmissionColorAndTexture(color, texture, true);
            }
            else
            {
                Color color = overwriteColor ? Color.white : renderer.material.GetColor(MaterialUtils.SHADER_MAINCOLOR_NAME);
                renderer.material.SetAlbedoColorAndTexture(color, texture);
            }
        }

        private static void SetupMagicPixels(Renderer baseRenderer)
        {
            Transform parentTransform = baseRenderer.transform.parent;
            if (parentTransform == null)
            {
                return;
            }

            IEnumerable<Renderer> renderers = parentTransform.GetComponentsInChildren<Renderer>()
                                                             .Where(r => r.GetComponent<NodeTag>() == null
                                                                      && baseRenderer.sharedMaterial.name.StartsWith(r.sharedMaterial.name));

            bool baseRendererIsEmissive = baseRenderer.material.IsKeywordEnabled(MaterialUtils.SHADER_EMISSIVEKEYWORD);

            foreach (Renderer renderer in renderers)
            {
                if (baseRendererIsEmissive)
                {
                    Color color     = baseRenderer.material.GetColor(MaterialUtils.SHADER_EMISSIVECOLOR_NAME);
                    Texture texture = baseRenderer.material.GetTexture(MaterialUtils.SHADER_EMISSIVETEXTURE_NAME);
                    if (renderer.material.IsKeywordEnabled(MaterialUtils.SHADER_EMISSIVEKEYWORD))
                    {
                        renderer.material.SetEmissionColorAndTexture(color, texture, true);
                    }
                    else
                    {
                        renderer.material.SetAlbedoColorAndTexture(color, texture);
                    }
                }
                else
                {
                    Color color     = baseRenderer.material.GetColor(MaterialUtils.SHADER_MAINCOLOR_NAME);
                    Texture texture = baseRenderer.material.GetTexture(MaterialUtils.SHADER_MAINTEXTURE_NAME);
                    renderer.material.SetAlbedoColorAndTexture(color, texture);
                }
            }
        }
    }
}
