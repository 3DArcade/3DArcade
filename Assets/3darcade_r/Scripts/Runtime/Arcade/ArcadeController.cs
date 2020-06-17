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
    public abstract class ArcadeController
    {
        protected const string ARCADE_RESOURCES_DIRECTORY = "Arcades";
        protected const string GAME_RESOURCES_DIRECTORY   = "Games";
        protected const string PROP_RESOURCES_DIRECTORY   = "Props";

        private static readonly string _defaultMediaDirectory         = $"{SystemUtils.GetDataPath()}/3darcade_r~/Configuration/Media";
        private static readonly string _defaultMarqueesDirectory      = $"{_defaultMediaDirectory}/Marquees";
        private static readonly string _defaultMarqueesVideoDirectory = $"{_defaultMediaDirectory}/MarqueesVideo";
        private static readonly string _defaultScreensDirectory       = $"{_defaultMediaDirectory}/Screens";
        private static readonly string _defaultScreensVideoDirectory  = $"{_defaultMediaDirectory}/ScreensVideo";
        private static readonly string _defaultGenericsDirectory      = $"{_defaultMediaDirectory}/Generics";
        private static readonly string _defaultGenericsVideoDirectory = $"{_defaultMediaDirectory}/GenericsVideo";

        protected readonly ArcadeHierarchy _arcadeHierarchy;
        protected readonly PlayerFpsControls _playerFpsControls;
        protected readonly PlayerCylControls _playerCylControls;

        protected readonly AssetCache<GameObject> _gameObjectCache;
        protected readonly AssetCache<Texture> _textureCache;
        private readonly AssetCache<string> _videoCache;

        protected readonly ContentMatcher _contentMatcher;
        private readonly AnimationCurve _volumeCurve;

        public ArcadeController(ArcadeHierarchy arcadeHierarchy,
                                PlayerFpsControls playerFpsControls,
                                PlayerCylControls playerCylControls,
                                Database<EmulatorConfiguration> emulatorDatabase,
                                AssetCache<GameObject> gameObjectCache,
                                AssetCache<Texture> textureCache,
                                AssetCache<string> videoCache)
        {
            Assert.IsNotNull(arcadeHierarchy);
            Assert.IsNotNull(playerFpsControls);
            Assert.IsNotNull(playerCylControls);
            Assert.IsNotNull(emulatorDatabase);
            Assert.IsNotNull(gameObjectCache);

            _arcadeHierarchy   = arcadeHierarchy;
            _playerFpsControls = playerFpsControls;
            _playerCylControls = playerCylControls;
            _gameObjectCache   = gameObjectCache;

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

        public abstract bool StartArcade(ArcadeConfiguration arcadeConfiguration);

        protected abstract void SetupWorld(ArcadeConfiguration arcadeConfiguration);


        protected static void SetupPlayer(PlayerControls playerControls, CameraSettings cameraSettings)
        {
            playerControls.Camera.orthographic = cameraSettings.Orthographic;
            playerControls.Camera.rect         = cameraSettings.ViewportRect;

            playerControls.transform.SetPositionAndRotation(cameraSettings.Position, Quaternion.Euler(0f, cameraSettings.Rotation.y, 0f));

            CinemachineVirtualCamera vCam = playerControls.VirtualCamera;
            vCam.m_Lens.FieldOfView       = cameraSettings.FieldOfView;
            vCam.m_Lens.OrthographicSize  = cameraSettings.AspectRatio;
            vCam.m_Lens.NearClipPlane     = cameraSettings.NearClipPlane;
            vCam.m_Lens.FarClipPlane      = cameraSettings.FarClipPlane;

            CinemachineTransposer transposer = vCam.GetCinemachineComponent<CinemachineTransposer>();
            transposer.m_FollowOffset.y      = cameraSettings.Height;
        }

        protected void AddModelsToWorld(ModelConfiguration[] modelConfigurations, Transform parent, RenderSettings renderSettings, string resourceDirectory, ContentMatcher.GetNamesToTryDelegate getNamesToTry)
        {
            foreach (ModelConfiguration modelConfiguration in modelConfigurations)
            {
                EmulatorConfiguration emulator = _contentMatcher.GetEmulatorForConfiguration(modelConfiguration);
                List<string> namesToTry        = getNamesToTry(modelConfiguration, emulator);

                GameObject prefab = _gameObjectCache.Load(resourceDirectory, namesToTry);
                Assert.IsNotNull(prefab, "prefab is null!");

                GameObject instantiatedModel = InstantiatePrefab(prefab, parent, modelConfiguration);

                // Only look for artworks in play mode / at runtime
                if (Application.isPlaying && _textureCache != null)
                {
                    SetupMarqueeNode(instantiatedModel, modelConfiguration, emulator, renderSettings.MarqueeIntensity);
                    SetupScreenNode(instantiatedModel, modelConfiguration, emulator, GetScreenIntensity(modelConfiguration, renderSettings));
                    SetupGenericNode(instantiatedModel, modelConfiguration, emulator);
                }
            }
        }

        protected GameObject InstantiatePrefab(GameObject prefab, Transform parent, ModelConfiguration modelConfiguration)
        {
            GameObject model = Object.Instantiate(prefab, modelConfiguration.Position, Quaternion.Euler(modelConfiguration.Rotation), parent);
            model.StripCloneFromName();
            model.transform.localScale = modelConfiguration.Scale;
            model.transform.SetLayersRecursively(parent.gameObject.layer);
            model.AddComponent<ModelConfigurationComponent>()
                 .FromModelConfiguration(modelConfiguration);
            return model;
        }

        protected void SetupMarqueeNode(GameObject model, ModelConfiguration modelConfiguration, EmulatorConfiguration emulator, float emissionIntensity)
        {
            Renderer renderer = GetNodeRenderer<MarqueeNodeTag>(model);
            if (renderer == null)
            {
                return;
            }

            List<string> namesToTry = ArtworkMatcher.GetNamesToTry(modelConfiguration, emulator);

            List<string> directories = ArtworkMatcher.GetDirectoriesToTry(modelConfiguration?.MarqueeVideoDirectory, emulator?.MarqueesVideoDirectory, _defaultMarqueesVideoDirectory);
            if (SetupVideo(renderer.gameObject, directories, namesToTry))
            {
                renderer.material.ClearAlbedoColorAndTexture();
                return;
            }

            directories = ArtworkMatcher.GetDirectoriesToTry(modelConfiguration?.MarqueeDirectory, emulator?.MarqueesDirectory, _defaultMarqueesDirectory);
            Texture texture = _textureCache.Load(directories, namesToTry);
            if (texture != null)
            {
                SetupStaticImage(renderer.material, texture, true, true, emissionIntensity);
            }

            SetupMagicPixels(renderer);
        }

        protected void SetupScreenNode(GameObject model, ModelConfiguration modelConfiguration, EmulatorConfiguration emulator, float emissionIntensity)
        {
            Renderer renderer = GetNodeRenderer<ScreenNodeTag>(model);
            if (renderer == null)
            {
                return;
            }

            List<string> namesToTry = ArtworkMatcher.GetNamesToTry(modelConfiguration, emulator);

            List<string> directories = ArtworkMatcher.GetDirectoriesToTry(modelConfiguration?.ScreenVideoDirectory, emulator?.ScreensVideoDirectory, _defaultScreensVideoDirectory);
            if (SetupVideo(renderer.gameObject, directories, namesToTry))
            {
                renderer.material.ClearAlbedoColorAndTexture();
                return;
            }

            directories = ArtworkMatcher.GetDirectoriesToTry(modelConfiguration?.ScreenDirectory, emulator?.ScreensDirectory, _defaultScreensDirectory);
            Texture texture = _textureCache.Load(directories, namesToTry);
            if (texture != null)
            {
                SetupStaticImage(renderer.material, texture, true, true, emissionIntensity);
            }
        }

        protected void SetupGenericNode(GameObject model, ModelConfiguration modelConfiguration, EmulatorConfiguration emulator)
        {
            Renderer renderer = GetNodeRenderer<GenericNodeTag>(model);
            if (renderer == null)
            {
                return;
            }

            List<string> namesToTry = ArtworkMatcher.GetNamesToTry(modelConfiguration, emulator);

            List<string> directories = ArtworkMatcher.GetDirectoriesToTry(modelConfiguration?.GenericVideoDirectory, emulator?.GenericsVideoDirectory, _defaultGenericsVideoDirectory);
            if (SetupVideo(renderer.gameObject, directories, namesToTry))
            {
                renderer.material.ClearAlbedoColorAndTexture();
                return;
            }

            directories = ArtworkMatcher.GetDirectoriesToTry(modelConfiguration?.GenericDirectory, emulator?.GenericsDirectory, _defaultGenericsDirectory);
            Texture texture = _textureCache.Load(directories, namesToTry);
            SetupStaticImage(renderer.material, texture);
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
            videoPlayer.playOnAwake            = true;
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
                    Color color = baseRenderer.material.GetColor(MaterialUtils.SHADER_EMISSIVE_COLOR_NAME);
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
                    Color color = baseRenderer.material.GetColor(MaterialUtils.SHADER_ALBEDO_COLOR_NAME);
                    Texture texture = baseRenderer.material.GetTexture(MaterialUtils.SHADER_ALBEDO_TEXTURE_NAME);
                    renderer.material.SetAlbedoColorAndTexture(color, texture);
                }
            }
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

        protected static float GetScreenIntensity(ModelConfiguration modelConfiguration, RenderSettings renderSettings)
        {
            switch (modelConfiguration.ScreenType)
            {
                case GameScreenType.Raster:
                    return renderSettings.ScreenRasterIntensity;
                case GameScreenType.Vector:
                    return renderSettings.ScreenVectorIntenstity;
                case GameScreenType.Pinball:
                    return renderSettings.ScreenPinballIntensity;
                case GameScreenType.Unspecified:
                default:
                    return 1f;
            }
        }
    }
}
