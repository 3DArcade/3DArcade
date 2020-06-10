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
        private const string ARCADE_RESOURCES_DIRECTORY = "Arcades";
        private const string GAME_RESOURCES_DIRECTORY   = "Games";
        private const string PROP_RESOURCES_DIRECTORY   = "Props";

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "const")]
        private static readonly string DEFAULT_MARQUEES_DIRECTORY = $"{SystemUtils.GetDataPath()}/3darcade_r~/Configuration/Media/Default/Marquees";
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "const")]
        private static readonly string DEFAULT_SCREENS_DIRECTORY  = $"{SystemUtils.GetDataPath()}/3darcade_r~/Configuration/Media/Default/Screens";

        private readonly ArcadeHierarchy _arcadeHierarchy;
        private readonly AssetCache<GameObject> _gameObjectCache;
        private readonly Transform _player;
        private readonly AssetCache<Texture> _textureCache;

        private readonly ContentMatcher _contentMatcher;

        private ArcadeConfiguration _currentConfiguration;

        public ArcadeController(ArcadeHierarchy arcadeHierarchy,
                                AssetCache<GameObject> gameObjectCache,
                                Transform player,
                                Database<LauncherConfiguration> launcherDatabase,
                                Database<ContentListConfiguration> contentListDatabase,
                                AssetCache<Texture> textureCache)
        {
            Assert.IsNotNull(arcadeHierarchy);
            Assert.IsNotNull(gameObjectCache);
            Assert.IsNotNull(player);
            Assert.IsNotNull(launcherDatabase);
            Assert.IsNotNull(contentListDatabase);

            _arcadeHierarchy     = arcadeHierarchy;
            _gameObjectCache     = gameObjectCache;
            _player              = player;

            _textureCache = textureCache;

            _contentMatcher = new ContentMatcher(launcherDatabase, contentListDatabase);
        }

        public bool StartArcade(ArcadeConfiguration arcadeConfiguration)
        {
            Assert.IsNotNull(arcadeConfiguration);

            _currentConfiguration = arcadeConfiguration;

            if (_currentConfiguration.ArcadeType == ArcadeType.Fps)
            {
                SetupFpsPlayer();
            }

            AddModelsToWorld(_currentConfiguration.ArcadeModelList, _arcadeHierarchy.ArcadesNode, ARCADE_RESOURCES_DIRECTORY, ContentMatcher.GetNamesToTryForArcade);
            AddModelsToWorld(_currentConfiguration.GameModelList,   _arcadeHierarchy.GamesNode,   GAME_RESOURCES_DIRECTORY,   ContentMatcher.GetNamesToTryForGame);
            AddModelsToWorld(_currentConfiguration.PropModelList,   _arcadeHierarchy.PropsNode,   PROP_RESOURCES_DIRECTORY,   ContentMatcher.GetNamesToTryForProp);

            return true;
        }

        private void SetupFpsPlayer()
        {
            CameraSettings cameraSettings = _currentConfiguration.FpsProperties.CameraSettings;
            _player.SetPositionAndRotation(cameraSettings.Position, Quaternion.Euler(0f, cameraSettings.Rotation.y, 0f));
            Camera.main.rect                 = cameraSettings.ViewportRect;
            CinemachineVirtualCamera vCam    = _player.GetComponentInChildren<CinemachineVirtualCamera>();
            CinemachineTransposer transposer = vCam.GetCinemachineComponent<CinemachineTransposer>();
            transposer.m_FollowOffset.y      = cameraSettings.Height;
        }

        private void AddModelsToWorld(ModelConfiguration[] modelConfigurations, Transform parent, string resourceDirectory, ContentMatcher.GetNamesToTryDelegate getNamesToTry)
        {
            foreach (ModelConfiguration modelConfiguration in modelConfigurations)
            {
                _contentMatcher.GetLauncherAndContentForConfiguration(modelConfiguration, out LauncherConfiguration launcher, out ContentConfiguration content);

                List<string> namesToTry = getNamesToTry(modelConfiguration, launcher, content);

                GameObject prefab = _gameObjectCache.Load(resourceDirectory, namesToTry);
                Assert.IsNotNull(prefab, "prefab is null!");

                GameObject instantiatedModel = InstantiatePrefab(prefab, parent, modelConfiguration);

                // Only look for artworks in play mode / at runtime
                if (Application.isPlaying && _textureCache != null)
                {
                    SetupMarqueeNode(instantiatedModel, content, launcher);
                    SetupScreenNode(instantiatedModel , content, launcher);
                    SetupGenericNode(instantiatedModel, content, launcher);
                }
            }
        }

        private static GameObject InstantiatePrefab(GameObject prefab, Transform parent, ModelConfiguration modelConfiguration)
        {
            GameObject model = Object.Instantiate(prefab, modelConfiguration.Position, Quaternion.Euler(modelConfiguration.Rotation), parent);
            model.StripCloneFromName();
            model.transform.localScale = modelConfiguration.Scale;
            model.transform.SetLayersRecursively(parent.gameObject.layer);
            model.AddComponent<ModelConfigurationComponent>()
                 .FromModelConfiguration(modelConfiguration);
            return model;
        }

        private void SetupMarqueeNode(GameObject model, ContentConfiguration content, LauncherConfiguration launcher)
        {
            Renderer nodeRenderer = GetNodeRenderer<MarqueeNodeTag>(model);
            if (nodeRenderer == null)
            {
                return;
            }

            if (!ArtworkMatcher.GetDirectoriesToTry(out List<string> directories, content?.MarqueeDirectory, launcher?.MarqueesDirectory, DEFAULT_MARQUEES_DIRECTORY))
            {
                return;
            }

            if (!ArtworkMatcher.GetNamesToTry(out List<string> namesToTry, content, launcher))
            {
                return;
            }

            Texture texture         = _textureCache.Load(directories, namesToTry);
            float emissionIntensity = _currentConfiguration.RenderSettings.MarqueeIntensity;
            SetupNode(nodeRenderer, texture, true, true, emissionIntensity);
            SetupMagicPixels(nodeRenderer);
        }

        private void SetupScreenNode(GameObject model, ContentConfiguration content, LauncherConfiguration launcher)
        {
            Renderer nodeRenderer = GetNodeRenderer<ScreenNodeTag>(model);
            if (nodeRenderer == null)
            {
                return;
            }

            if (!ArtworkMatcher.GetDirectoriesToTry(out List<string> directories, content?.ScreenDirectory, launcher?.ScreensDirectory, DEFAULT_SCREENS_DIRECTORY))
            {
                return;
            }

            if (!ArtworkMatcher.GetNamesToTry(out List<string> namesToTry, content, launcher))
            {
                return;
            }

            Texture texture         = _textureCache.Load(directories, namesToTry);
            float emissionIntensity = GetScreenIntensity();
            SetupNode(nodeRenderer, texture, true, true, emissionIntensity);

            float GetScreenIntensity()
            {
                switch (content.Screen)
                {
                    case ContentScreenType.Raster:
                        return _currentConfiguration.RenderSettings.ScreenRasterIntensity;
                    case ContentScreenType.Vector:
                        return _currentConfiguration.RenderSettings.ScreenVectorIntenstity;
                    case ContentScreenType.Pinball:
                        return _currentConfiguration.RenderSettings.ScreenPinballIntensity;
                    case ContentScreenType.Unspecified:
                    default:
                        return 1f;
                }
            }
        }

        private void SetupGenericNode(GameObject model, ContentConfiguration content, LauncherConfiguration launcher)
        {
            Renderer nodeRenderer = GetNodeRenderer<GenericNodeTag>(model);
            if (nodeRenderer == null)
            {
                return;
            }

            if (!ArtworkMatcher.GetDirectoriesToTry(out List<string> directories, content?.GenericDirectory, launcher?.GenericsDirectory, null))
            {
                return;
            }

            if (!ArtworkMatcher.GetNamesToTry(out List<string> namesToTry, content, launcher))
            {
                return;
            }

            Texture texture = _textureCache.Load(directories, namesToTry);
            SetupNode(nodeRenderer, texture);
        }

        private static void SetupNode(Renderer renderer, Texture texture, bool overwriteColor = false, bool forceEmissive = false, float emissionFactor = 1f)
        {
            if (renderer == null || texture == null)
            {
                return;
            }

            // Video
            // ...

            // Image

            if (renderer.material.IsKeywordEnabled(MaterialUtils.SHADER_EMISSIVE_KEYWORD))
            {
                Color color = (overwriteColor ? Color.white : renderer.material.GetColor(MaterialUtils.SHADER_EMISSIVE_COLOR_NAME)) * emissionFactor;
                renderer.material.SetEmissiveColorAndTexture(color, texture, true);
            }
            else if (forceEmissive)
            {
                Color color = Color.white * emissionFactor;
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
    }
}
