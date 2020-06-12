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
using UnityEngine.Video;

namespace Arcade_r
{
    public class ArcadeController
    {
        private const string ARCADE_RESOURCES_DIRECTORY = "Arcades";
        private const string GAME_RESOURCES_DIRECTORY   = "Games";
        private const string PROP_RESOURCES_DIRECTORY   = "Props";

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "const")]
        private static readonly string DEFAULT_MARQUEES_DIRECTORY = $"{SystemUtils.GetDataPath()}/3darcade_r~/Configuration/Media/Marquees";
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "const")]
        private static readonly string DEFAULT_MARQUEES_VIDEO_DIRECTORY = $"{SystemUtils.GetDataPath()}/3darcade_r~/Configuration/Media/MarqueesVideo";
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "const")]
        private static readonly string DEFAULT_SCREENS_DIRECTORY = $"{SystemUtils.GetDataPath()}/3darcade_r~/Configuration/Media/Screens";
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "const")]
        private static readonly string DEFAULT_SCREENS_VIDEO_DIRECTORY  = $"{SystemUtils.GetDataPath()}/3darcade_r~/Configuration/Media/ScreensVideo";
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "const")]
        private static readonly string DEFAULT_GENERICS_DIRECTORY = $"{SystemUtils.GetDataPath()}/3darcade_r~/Configuration/Media/Generics";
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "const")]
        private static readonly string DEFAULT_GENERICS_VIDEO_DIRECTORY = $"{SystemUtils.GetDataPath()}/3darcade_r~/Configuration/Media/GenericsVideo";

        private readonly ArcadeHierarchy _arcadeHierarchy;
        private readonly AssetCache<GameObject> _gameObjectCache;
        private readonly Transform _player;
        private readonly AssetCache<Texture> _textureCache;
        private readonly AssetCache<string> _videoCache;
        private readonly ContentMatcher _contentMatcher;
        private readonly AnimationCurve _volumeCurve;

        private ArcadeConfiguration _currentConfiguration;

        public ArcadeController(ArcadeHierarchy arcadeHierarchy,
                                AssetCache<GameObject> gameObjectCache,
                                Transform player,
                                Database<EmulatorConfiguration> emulatorDatabase,
                                AssetCache<Texture> textureCache,
                                AssetCache<string> videoCache)
        {
            Assert.IsNotNull(arcadeHierarchy);
            Assert.IsNotNull(gameObjectCache);
            Assert.IsNotNull(player);
            Assert.IsNotNull(emulatorDatabase);

            _arcadeHierarchy = arcadeHierarchy;
            _gameObjectCache = gameObjectCache;
            _player          = player;

            _textureCache = textureCache;
            _videoCache   = videoCache;

            _contentMatcher = new ContentMatcher(emulatorDatabase);

            _volumeCurve = new AnimationCurve(new Keyframe[]
            {
                new Keyframe(0.8f, 1.0f, -2.6966875f,  -2.6966875f,  0.2f,        0.10490462f),
                new Keyframe(1.5f, 0.3f, -0.49866775f, -0.49866775f, 0.28727788f, 0.2f),
                new Keyframe(3.0f, 0.0f, -0.08717632f, -0.08717632f, 0.5031141f,  0.2f)
            });
        }

        public bool StartArcade(ArcadeConfiguration arcadeConfiguration)
        {
            Assert.IsNotNull(arcadeConfiguration);

            _currentConfiguration = arcadeConfiguration;

            if (_currentConfiguration.ArcadeType == ArcadeType.FpsArcade)
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
            CameraSettings cameraSettings = _currentConfiguration.FpsArcadeProperties.CameraSettings;
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
                _contentMatcher.GetEmulatorForConfiguration(modelConfiguration, out EmulatorConfiguration emulator);

                List<string> namesToTry = getNamesToTry(modelConfiguration, emulator);

                GameObject prefab = _gameObjectCache.Load(resourceDirectory, namesToTry);
                Assert.IsNotNull(prefab, "prefab is null!");

                GameObject instantiatedModel = InstantiatePrefab(prefab, parent, modelConfiguration);

                // Only look for artworks in play mode / at runtime
                if (Application.isPlaying && _textureCache != null)
                {
                    SetupMarqueeNode(instantiatedModel, modelConfiguration, emulator);
                    SetupScreenNode(instantiatedModel , modelConfiguration, emulator);
                    SetupGenericNode(instantiatedModel, modelConfiguration, emulator);
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

        private void SetupMarqueeNode(GameObject model, ModelConfiguration modelConfiguration, EmulatorConfiguration emulator)
        {
            Renderer renderer = GetNodeRenderer<MarqueeNodeTag>(model);
            if (renderer == null)
            {
                return;
            }

            List<string> namesToTry = ArtworkMatcher.GetNamesToTry(modelConfiguration, emulator);

            List<string> directories = ArtworkMatcher.GetDirectoriesToTry(modelConfiguration?.MarqueeVideoDirectory, emulator?.MarqueesVideoDirectory, DEFAULT_MARQUEES_VIDEO_DIRECTORY);
            if (SetupVideo(renderer.gameObject, directories, namesToTry))
            {
                renderer.material.ClearAlbedoColorAndTexture();
                return;
            }

            directories = ArtworkMatcher.GetDirectoriesToTry(modelConfiguration?.MarqueeDirectory, emulator?.MarqueesDirectory, DEFAULT_MARQUEES_DIRECTORY);
            Texture texture = _textureCache.Load(directories, namesToTry);
            if (texture != null)
            {
                SetupStaticImage(renderer.material, texture, true, true, _currentConfiguration.RenderSettings.MarqueeIntensity);
            }

            SetupMagicPixels(renderer);
        }

        private void SetupScreenNode(GameObject model, ModelConfiguration modelConfiguration, EmulatorConfiguration emulator)
        {
            Renderer renderer = GetNodeRenderer<ScreenNodeTag>(model);
            if (renderer == null)
            {
                return;
            }

            List<string> namesToTry = ArtworkMatcher.GetNamesToTry(modelConfiguration, emulator);

            List<string> directories = ArtworkMatcher.GetDirectoriesToTry(modelConfiguration?.ScreenVideoDirectory, emulator?.ScreensVideoDirectory, DEFAULT_SCREENS_VIDEO_DIRECTORY);
            if (SetupVideo(renderer.gameObject, directories, namesToTry))
            {
                renderer.material.ClearAlbedoColorAndTexture();
                return;
            }

            directories = ArtworkMatcher.GetDirectoriesToTry(modelConfiguration?.ScreenDirectory, emulator?.ScreensDirectory, DEFAULT_SCREENS_DIRECTORY);
            Texture texture = _textureCache.Load(directories, namesToTry);
            if (texture != null)
            {
                SetupStaticImage(renderer.material, texture, true, true, GetScreenIntensity());
            }

            float GetScreenIntensity()
            {
                switch (modelConfiguration.ScreenType)
                {
                    case GameScreenType.Raster:
                        return _currentConfiguration.RenderSettings.ScreenRasterIntensity;
                    case GameScreenType.Vector:
                        return _currentConfiguration.RenderSettings.ScreenVectorIntenstity;
                    case GameScreenType.Pinball:
                        return _currentConfiguration.RenderSettings.ScreenPinballIntensity;
                    case GameScreenType.Unspecified:
                    default:
                        return 1f;
                }
            }
        }

        private void SetupGenericNode(GameObject model, ModelConfiguration modelConfiguration, EmulatorConfiguration emulator)
        {
            Renderer renderer = GetNodeRenderer<GenericNodeTag>(model);
            if (renderer == null)
            {
                return;
            }

            List<string> namesToTry = ArtworkMatcher.GetNamesToTry(modelConfiguration, emulator);

            List<string> directories = ArtworkMatcher.GetDirectoriesToTry(modelConfiguration?.GenericVideoDirectory, emulator?.GenericsVideoDirectory, DEFAULT_GENERICS_VIDEO_DIRECTORY);
            if (SetupVideo(renderer.gameObject, directories, namesToTry))
            {
                renderer.material.ClearAlbedoColorAndTexture();
                return;
            }

            directories = ArtworkMatcher.GetDirectoriesToTry(modelConfiguration?.GenericDirectory, emulator?.GenericsDirectory, DEFAULT_GENERICS_DIRECTORY);
            Texture texture = _textureCache.Load(directories, namesToTry);
            SetupStaticImage(renderer.material, texture);
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

        private bool SetupVideo(GameObject screen, List<string> directories, List<string> namesToTry)
        {
            string videopath = _videoCache.Load(directories, namesToTry);
            if (string.IsNullOrEmpty(videopath))
            {
                return false;
            }

           _ = screen.AddComponentIfNotFound<AudioSource>();

            VideoPlayer videoPlayer            = screen.AddComponentIfNotFound<VideoPlayer>();
            videoPlayer.errorReceived          -= OnVideoPlayerErrorReceived;
            videoPlayer.errorReceived          += OnVideoPlayerErrorReceived;
            videoPlayer.prepareCompleted       -= OnVideoPlayerPrepareCompleted;
            videoPlayer.prepareCompleted       += OnVideoPlayerPrepareCompleted;
            videoPlayer.playOnAwake            = false;
            videoPlayer.waitForFirstFrame      = true;
            videoPlayer.isLooping              = true;
            videoPlayer.source                 = VideoSource.Url;
            videoPlayer.url                    = videopath;
            videoPlayer.renderMode             = VideoRenderMode.MaterialOverride;
            videoPlayer.audioOutputMode        = VideoAudioOutputMode.AudioSource;
            videoPlayer.targetMaterialProperty = MaterialUtils.SHADER_EMISSIVE_TEXTURE_NAME;
            videoPlayer.Prepare();

            return true;
        }

        private void OnVideoPlayerErrorReceived(VideoPlayer videoPlayer, string message)
        {
            Debug.Log($"Error: {message}");
        }

        private void OnVideoPlayerPrepareCompleted(VideoPlayer videoPlayer)
        {
            float frameCount = videoPlayer.frameCount;
            float frameRate  = videoPlayer.frameRate;
            double duration  = frameCount / frameRate;
            videoPlayer.time = Random.Range(0.1f, 0.9f) * duration;

            AudioSource audioSource  = videoPlayer.GetTargetAudioSource(0);
            audioSource.playOnAwake  = false;
            audioSource.spatialBlend = 1f;
            audioSource.minDistance  = 1f;
            audioSource.maxDistance  = 3f;
            audioSource.volume       = 1f;
            audioSource.rolloffMode  = AudioRolloffMode.Custom;
            audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, _volumeCurve);

            videoPlayer.EnableAudioTrack(0, false);
            videoPlayer.Pause();
        }

        private static void SetupStaticImage(Material material, Texture texture, bool overwriteColor = false, bool forceEmissive = false, float emissionFactor = 1f)
        {
            if (material.IsKeywordEnabled(MaterialUtils.SHADER_EMISSIVE_KEYWORD))
            {
                Color color = (overwriteColor ? Color.white : material.GetColor(MaterialUtils.SHADER_EMISSIVE_COLOR_NAME)) * emissionFactor;
                material.SetEmissiveColorAndTexture(color, texture, true);
            }
            else if (forceEmissive)
            {
                Color color = Color.white * emissionFactor;
                material.SetEmissiveColorAndTexture(color, texture, true);
            }
            else
            {
                Color color = overwriteColor ? Color.white : material.GetColor(MaterialUtils.SHADER_ALBEDO_COLOR_NAME);
                material.SetAlbedoColorAndTexture(color, texture);
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
