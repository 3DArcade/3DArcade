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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Video;

namespace Arcade
{
    public sealed class FpsArcadeController : ArcadeController
    {
        public FpsArcadeController(ArcadeHierarchy arcadeHierarchy,
                                   PlayerFpsControls playerFpsControls,
                                   PlayerCylControls playerCylControls,
                                   Database<EmulatorConfiguration> emulatorDatabase,
                                   AssetCache<GameObject> gameObjectCache,
                                   AssetCache<Texture> textureCache,
                                   AssetCache<string> videoCache)
        : base(arcadeHierarchy, playerFpsControls, playerCylControls, emulatorDatabase, gameObjectCache, textureCache, videoCache)
        {
            _audioMinDistance = 1f;
            _audioMaxDistance = 3f;

            _volumeCurve = new AnimationCurve(new Keyframe[]
            {
                 new Keyframe(0.8f,                     1.0f, -2.6966875f,  -2.6966875f,  0.2f,        0.10490462f),
                 new Keyframe(_audioMaxDistance * 0.5f, 0.3f, -0.49866775f, -0.49866775f, 0.28727788f, 0.2f),
                 new Keyframe(_audioMaxDistance,        0.0f, -0.08717632f, -0.08717632f, 0.5031141f,  0.2f)
            });
        }

        public override void StartArcade(ArcadeConfiguration arcadeConfiguration)
        {
            Assert.IsNotNull(arcadeConfiguration);

            _playerFpsControls.gameObject.SetActive(true);
            _playerCylControls.gameObject.SetActive(false);

            SetupPlayer(_playerFpsControls, arcadeConfiguration.FpsArcadeProperties.CameraSettings);

            _ = _coroutineHelper.StartCoroutine(SetupWorld(arcadeConfiguration));
        }

        public override bool SetupVideo(Renderer screen, List<string> directories, List<string> namesToTry)
        {
            string videopath = _videoCache.Load(directories, namesToTry);
            if (string.IsNullOrEmpty(videopath))
            {
                return false;
            }

            screen.material.EnableEmissive();

            AudioSource audioSource = screen.gameObject.AddComponentIfNotFound<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.dopplerLevel = 0f;
            audioSource.spatialBlend = 1f;
            audioSource.minDistance = _audioMinDistance;
            audioSource.maxDistance = _audioMaxDistance;
            audioSource.volume = 1f;
            audioSource.rolloffMode = AudioRolloffMode.Custom;
            audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, _volumeCurve);

            VideoPlayer videoPlayer = screen.gameObject.AddComponentIfNotFound<VideoPlayer>();
            videoPlayer.errorReceived -= OnVideoPlayerErrorReceived;
            videoPlayer.errorReceived += OnVideoPlayerErrorReceived;
            videoPlayer.prepareCompleted -= OnVideoPlayerPrepareCompleted;
            videoPlayer.prepareCompleted += OnVideoPlayerPrepareCompleted;
            videoPlayer.playOnAwake = true;
            videoPlayer.waitForFirstFrame = true;
            videoPlayer.isLooping = true;
            videoPlayer.skipOnDrop = true;
            videoPlayer.source = VideoSource.Url;
            videoPlayer.url = videopath;
            videoPlayer.renderMode = VideoRenderMode.MaterialOverride;
            videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
            videoPlayer.controlledAudioTrackCount = 1;
            videoPlayer.targetMaterialProperty = MaterialUtils.SHADER_EMISSIVE_TEXTURE_NAME;
            videoPlayer.Prepare();

            return true;
        }

        protected override IEnumerator SetupWorld(ArcadeConfiguration arcadeConfiguration)
        {
            ArcadeLoaded = false;

            RenderSettings renderSettings = arcadeConfiguration.RenderSettings;
            _ = _coroutineHelper.StartCoroutine(AddModelsToWorld(arcadeConfiguration.ArcadeModelList, _arcadeHierarchy.ArcadesNode, renderSettings, ARCADE_RESOURCES_DIRECTORY, ContentMatcher.GetNamesToTryForArcade));
            _ = _coroutineHelper.StartCoroutine(AddModelsToWorld(arcadeConfiguration.PropModelList, _arcadeHierarchy.PropsNode, renderSettings, PROP_RESOURCES_DIRECTORY, ContentMatcher.GetNamesToTryForProp));
            _ = _coroutineHelper.StartCoroutine(AddGameModelsToWorld(arcadeConfiguration.GameModelList, renderSettings, GAME_RESOURCES_DIRECTORY, ContentMatcher.GetNamesToTryForGame));

            while (!_gameModelsLoaded)
            {
                yield return null;
            }

            _allGames.Clear();
            for (int i = 0; i < _arcadeHierarchy.GamesNode.childCount; ++i)
            {
                _allGames.Add(_arcadeHierarchy.GamesNode.GetChild(i));
            }

            LateSetupWorld();

            ArcadeLoaded = true;
        }

        protected override IEnumerator AddGameModelsToWorld(ModelConfiguration[] modelConfigurations, RenderSettings renderSettings, string resourceDirectory, ContentMatcher.GetNamesToTryDelegate getNamesToTry)
        {
            _gameModelsLoaded = false;

            foreach (ModelConfiguration modelConfiguration in modelConfigurations)
            {
                EmulatorConfiguration emulator = _contentMatcher.GetEmulatorForConfiguration(modelConfiguration);
                List<string> namesToTry = getNamesToTry(modelConfiguration, emulator);

                GameObject prefab = _gameObjectCache.Load(resourceDirectory, namesToTry);
                if (prefab == null)
                {
                    continue;
                }

                GameObject instantiatedModel = InstantiatePrefab(prefab, _arcadeHierarchy.GamesNode, modelConfiguration);

                // Look for artworks only in play mode / runtime
                if (Application.isPlaying)
                {
                    _marqueeNodeController.Setup(instantiatedModel, modelConfiguration, emulator, renderSettings.MarqueeIntensity);
                    _screenNodeController.Setup(instantiatedModel, modelConfiguration, emulator, GetScreenIntensity(modelConfiguration, renderSettings));
                    _genericNodeController.Setup(instantiatedModel, modelConfiguration, emulator, 1f);
                }

                // Instantiate asynchronously only when loaded from the editor menu / auto reload
                if (Application.isPlaying)
                {
                    yield return null;
                }
            }

            _gameModelsLoaded = true;
        }

        private void OnVideoPlayerPrepareCompleted(VideoPlayer videoPlayer)
        {
            float frameCount = videoPlayer.frameCount;
            float frameRate  = videoPlayer.frameRate;
            double duration  = frameCount / frameRate;
            videoPlayer.time = Random.Range(0.02f, 0.98f) * duration;

            videoPlayer.EnableAudioTrack(0, false);
            videoPlayer.Pause();
        }
    }
}
