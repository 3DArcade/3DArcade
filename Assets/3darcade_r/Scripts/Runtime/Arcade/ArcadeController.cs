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
using UnityEngine.Assertions;

namespace Arcade_r
{
    public class ArcadeController
    {
        private static readonly string[] RESOURCES_SUB_DIRECTORIES = new [] { "Arcades", "Games", "Props" };
        private const string GAME_RESOURCES_DIRECTORY              = "Games";

        private readonly ArcadeHierarchy _arcadeHierarchy;
        private readonly AssetCache<GameObject> _gameObjectCache;
        private readonly AssetCache<Texture> _textureCache;
        private readonly Transform _player;

        private ArcadeConfiguration _currentConfiguration;

        public ArcadeController(ArcadeHierarchy arcadeHierarchy, AssetCache<GameObject> gameObjectCache, AssetCache<Texture> textureCache, Transform player)
        {
            Assert.IsNotNull(arcadeHierarchy);
            Assert.IsNotNull(gameObjectCache);
            Assert.IsNotNull(player);

            _arcadeHierarchy = arcadeHierarchy;
            _gameObjectCache = gameObjectCache;
            _textureCache    = textureCache;
            _player          = player;
        }

        public void StartArcade(ArcadeConfiguration arcadeConfiguration)
        {
            Assert.IsNotNull(arcadeConfiguration);

            _currentConfiguration = arcadeConfiguration;

            if (_arcadeHierarchy.RootNode.TryGetComponent(out ArcadeConfigurationComponent arcadeConfigurationComponent))
            {
                arcadeConfigurationComponent.FromArcadeConfiguration(arcadeConfiguration);
            }
            else
            {
                _arcadeHierarchy.RootNode.AddComponent<ArcadeConfigurationComponent>()
                                         .FromArcadeConfiguration(arcadeConfiguration);
            }

            SetupPlayer();

            AddModelsToWorld<ArcadeModelSetup>(arcadeConfiguration.ArcadeModelList, _arcadeHierarchy.ArcadesNode, arcadeConfiguration.RenderSettings);
            AddModelsToWorld<GameModelSetup>(arcadeConfiguration.GameModelList, _arcadeHierarchy.GamesNode, arcadeConfiguration.RenderSettings);
            AddModelsToWorld<PropModelSetup>(arcadeConfiguration.PropModelList, _arcadeHierarchy.PropsNode, arcadeConfiguration.RenderSettings);
        }

        private void SetupPlayer()
        {
            Assert.IsNotNull(_player);
            Assert.IsNotNull(_currentConfiguration);

            _player.SetPositionAndRotation(_currentConfiguration.CameraSettings.Position, Quaternion.Euler(0f, _currentConfiguration.CameraSettings.Rotation.y, 0f));
            CinemachineVirtualCamera vCam    = _player.GetComponentInChildren<CinemachineVirtualCamera>();
            CinemachineTransposer transposer = vCam.GetCinemachineComponent<CinemachineTransposer>();
            transposer.m_FollowOffset.y      = _currentConfiguration.CameraSettings.Height;
        }

        private void AddModelsToWorld<T>(ModelConfiguration[] models, GameObject parent, RenderSettings renderSettings)
            where T : ModelSetup
        {
            foreach (ModelConfiguration modelConfiguration in models)
            {
                AddModelToWorld<T>(modelConfiguration, parent, renderSettings);
            }
        }

        private void AddModelToWorld<T>(ModelConfiguration modelConfiguration, GameObject parent, RenderSettings renderSettings)
            where T : ModelSetup
        {
            GameObject prefab = _gameObjectCache.Load(RESOURCES_SUB_DIRECTORIES, modelConfiguration.Model, modelConfiguration.Id, modelConfiguration.IdParent);
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

                    prefab = _gameObjectCache.Load(GAME_RESOURCES_DIRECTORY, prefabName);
                }
            }

            if (prefab != null)
            {
                GameObject instantiatedModel = InstantiatePrefab<T>(prefab, parent, modelConfiguration);

                // Only look for artworks in play mode / at runtime
                if (Application.isPlaying && _textureCache != null)
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

        private void SetupMarqueeNode(GameObject model, ModelConfiguration modelConfiguration, RenderSettings renderSettings)
        {
            Renderer nodeRenderer = GetNodeRenderer<MarqueeNodeTag>(model);
            if (nodeRenderer == null)
            {
                return;
            }

            string directory = $"{Application.streamingAssetsPath}/3darcade_r~/Emulators/mame/marquees";
            Texture texture  = _textureCache.Load(directory, modelConfiguration.Id, modelConfiguration.IdParent);
            if (texture == null)
            {
                return;
            }

            SetupNode(nodeRenderer, texture, true, true, renderSettings.MarqueeIntensity);
            SetupMagicPixels(nodeRenderer);
        }

        private void SetupScreenNode(GameObject model, ModelConfiguration modelConfiguration, RenderSettings renderSettings)
        {
            Renderer nodeRenderer = GetNodeRenderer<ScreenNodeTag>(model);
            if (nodeRenderer == null)
            {
                return;
            }

            string directory = $"{Application.streamingAssetsPath}/3darcade_r~/Emulators/mame/snap";
            Texture texture  = _textureCache.Load(directory, modelConfiguration.Id, modelConfiguration.IdParent);
            if (texture == null)
            {
                return;
            }

            float screenIntensity;
            if (modelConfiguration.Screen.ToLowerInvariant().Contains("vector"))
            {
                screenIntensity = renderSettings.ScreenVectorIntenstity;
            }
            else if (modelConfiguration.Screen.ToLowerInvariant().Contains("pinball"))
            {
                screenIntensity = renderSettings.ScreenPinballIntensity;
            }
            else
            {
                screenIntensity = renderSettings.ScreenRasterIntensity;
            }

            SetupNode(nodeRenderer, texture, true, true, screenIntensity);
        }

        private void SetupGenericNode(GameObject model, ModelConfiguration modelConfiguration)
        {
            Renderer nodeRenderer = GetNodeRenderer<GenericNodeTag>(model);
            if (nodeRenderer == null)
            {
                return;
            }

            string directory = $"{Application.streamingAssetsPath}/3darcade_r~/Emulators/mame/cabinets";
            Texture texture  = _textureCache.Load(directory, modelConfiguration.Id, modelConfiguration.IdParent);
            if (texture == null)
            {
                return;
            }

            SetupNode(nodeRenderer, texture);
        }

        [SuppressMessage("Type Safety", "UNT0014:Invalid type for call to GetComponent", Justification = "Analyzer is dumb")]
        private static Renderer GetNodeRenderer<T>(GameObject model)
            where T : NodeTag
        {
            T nodeTag = model.GetComponentInChildren<T>();
            if (nodeTag != null)
            {
                return nodeTag.GetComponent<Renderer>();
            }
            return null;
        }

        private static void SetupNode(Renderer renderer, Texture texture, bool overwriteColor = false, bool forceEmissive = false, float emissionFactor = 1f)
        {
            // Video
            // ...

            // Image
            if (forceEmissive || renderer.material.IsKeywordEnabled(MaterialUtils.SHADER_EMISSIVE_KEYWORD))
            {
                Color color = (overwriteColor ? Color.white : renderer.material.GetColor(MaterialUtils.SHADER_EMISSIVE_COLOR_NAME)) * emissionFactor;
                renderer.material.SetEmissiveColorAndTexture(color, texture, true);
            }
            else
            {
                Color color = overwriteColor ? Color.white : renderer.material.GetColor(MaterialUtils.SHADER_ALBEDO_COLOR_NAME);
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

            bool baseRendererIsEmissive = baseRenderer.material.IsKeywordEnabled(MaterialUtils.SHADER_EMISSIVE_KEYWORD);

            foreach (Renderer renderer in renderers)
            {
                if (baseRendererIsEmissive)
                {
                    Color color     = baseRenderer.material.GetColor(MaterialUtils.SHADER_EMISSIVE_COLOR_NAME);
                    Texture texture = baseRenderer.material.GetTexture(MaterialUtils.SHADER_EMISSIVE_TEXTURE_NAME);
                    if (renderer.material.IsKeywordEnabled(MaterialUtils.SHADER_EMISSIVE_KEYWORD))
                    {
                        renderer.material.SetEmissiveColorAndTexture(color, texture, true);
                    }
                    else
                    {
                        renderer.material.SetAlbedoColorAndTexture(color, texture);
                    }
                }
                else
                {
                    Color color     = baseRenderer.material.GetColor(MaterialUtils.SHADER_ALBEDO_COLOR_NAME);
                    Texture texture = baseRenderer.material.GetTexture(MaterialUtils.SHADER_ALBEDO_TEXTURE_NAME);
                    renderer.material.SetAlbedoColorAndTexture(color, texture);
                }
            }
        }
    }
}
