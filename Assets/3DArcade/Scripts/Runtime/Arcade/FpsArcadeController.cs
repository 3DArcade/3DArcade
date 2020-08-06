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
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace Arcade
{
    public sealed class FpsArcadeController : ArcadeController
    {
        protected override bool UseModelTransfoms => true;
        protected override PlayerControls PlayerControls => _playerFpsControls;
        protected override CameraSettings CameraSettings => _arcadeConfiguration.FpsArcadeProperties.CameraSettings;

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

        public override bool SetupVideo(Renderer screen, List<string> directories, List<string> namesToTry, float emissionIntensity)
        {
            string videopath = _videoCache.Load(directories, namesToTry);
            if (string.IsNullOrEmpty(videopath))
            {
                return false;
            }

            screen.material.EnableEmissive();
            screen.material.SetEmissiveColor(Color.white * emissionIntensity);

            AudioSource audioSource  = screen.gameObject.AddComponentIfNotFound<AudioSource>();
            audioSource.playOnAwake  = false;
            audioSource.dopplerLevel = 0f;
            audioSource.spatialBlend = 1f;
            audioSource.minDistance  = _audioMinDistance;
            audioSource.maxDistance  = _audioMaxDistance;
            audioSource.volume       = 1f;
            audioSource.rolloffMode  = AudioRolloffMode.Custom;
            audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, _volumeCurve);

            VideoPlayer videoPlayer               = screen.gameObject.AddComponentIfNotFound<VideoPlayer>();
            videoPlayer.errorReceived            -= OnVideoPlayerErrorReceived;
            videoPlayer.errorReceived            += OnVideoPlayerErrorReceived;
            videoPlayer.prepareCompleted         -= OnVideoPlayerPrepareCompleted;
            videoPlayer.prepareCompleted         += OnVideoPlayerPrepareCompleted;
            videoPlayer.playOnAwake               = true;
            videoPlayer.waitForFirstFrame         = true;
            videoPlayer.isLooping                 = true;
            videoPlayer.skipOnDrop                = true;
            videoPlayer.source                    = VideoSource.Url;
            videoPlayer.url                       = videopath;
            videoPlayer.renderMode                = VideoRenderMode.MaterialOverride;
            videoPlayer.audioOutputMode           = VideoAudioOutputMode.AudioSource;
            videoPlayer.controlledAudioTrackCount = 1;
            videoPlayer.targetMaterialProperty    = MaterialUtils.SHADER_EMISSIVE_TEXTURE_NAME;
            videoPlayer.Prepare();

            return true;
        }
        protected override void PreSetupPlayer()
        {
            _playerFpsControls.gameObject.SetActive(true);
            _playerCylControls.gameObject.SetActive(false);
        }

        private static void OnVideoPlayerPrepareCompleted(VideoPlayer videoPlayer)
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
