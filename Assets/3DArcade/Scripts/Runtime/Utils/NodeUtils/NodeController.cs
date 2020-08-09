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

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.Video;

namespace Arcade
{
    public abstract class NodeController
    {
        public static event System.Action OnVideoPlayerAdded;

        protected static readonly string _defaultMediaDirectory = $"{SystemUtils.GetDataPath()}/3darcade~/Configuration/Media";

        protected abstract string[] DefaultImageDirectories { get; }
        protected abstract string[] DefaultVideoDirectories { get; }

        private readonly AssetCache<string> _videoCache;
        private readonly AssetCache<Texture> _textureCache;

        protected NodeController(AssetCache<string> videoCache, AssetCache<Texture> textureCache)
        {
            _videoCache   = videoCache;
            _textureCache = textureCache;
        }

        public void Setup(ArcadeController arcadeController, GameObject model, ModelConfiguration modelConfiguration, EmulatorConfiguration emulator, float emissionIntensity)
        {
            Renderer renderer = GetNodeRenderer(model);
            if (renderer == null)
            {
                return;
            }

            renderer.material.EnableEmission(true);
            renderer.material.SetBaseColor(Color.black);
            renderer.material.SetBaseTexture(null);
            renderer.material.SetEmissionColor(Color.white * emissionIntensity);

            List<string> namesToTry  = ArtworkMatcher.GetNamesToTry(modelConfiguration, emulator);
            List<string> directories = ArtworkMatcher.GetDirectoriesToTry(GetModelImageDirectories(modelConfiguration), GetEmulatorImageDirectories(emulator), DefaultImageDirectories);

            Texture[] textures = _textureCache.LoadMultiple(directories, namesToTry);
            if (textures != null)
            {
                if (textures.Length > 0)
                {
                    renderer.material.SetEmissionTexture(textures[0]);
                    if (this is MarqueeNodeController marqueeNodeController)
                    {
                        marqueeNodeController.SetupMagicPixels(renderer);
                    }
                }

                if (textures.Length > 1)
                {
                    DynamicArtworkComponent dynamicArtworkComponent = renderer.gameObject.AddComponentIfNotFound<DynamicArtworkComponent>();
                    dynamicArtworkComponent.Construct(textures);
                }
                else if (textures.Length > 0)
                {
                    renderer.material.SetEmissionTexture(textures[0]);
                }
            }

            directories = ArtworkMatcher.GetDirectoriesToTry(GetModelVideoDirectories(modelConfiguration), GetEmulatorVideoDirectories(emulator), DefaultVideoDirectories);
            SetupVideo(renderer, directories, namesToTry, arcadeController.VideoPlayOnAwake, arcadeController.AudioMinDistance, arcadeController.AudioMaxDistance, arcadeController.VolumeCurve);
        }

        protected abstract Renderer GetNodeRenderer(GameObject model);

        protected abstract string[] GetModelImageDirectories(ModelConfiguration modelConfiguration);

        protected abstract string[] GetModelVideoDirectories(ModelConfiguration modelConfiguration);

        protected abstract string[] GetEmulatorImageDirectories(EmulatorConfiguration emulator);

        protected abstract string[] GetEmulatorVideoDirectories(EmulatorConfiguration emulator);

        [SuppressMessage("Type Safety", "UNT0014:Invalid type for call to GetComponent", Justification = "Analyzer is dumb")]
        protected static Renderer GetNodeRenderer<T>(GameObject model) where T : NodeTag
        {
            if (model == null)
            {
                return null;
            }

            T nodeTag = model.GetComponentInChildren<T>(true);
            if (nodeTag != null)
            {
                Renderer renderer = nodeTag.GetComponent<Renderer>();
                if (renderer != null)
                {
                    return renderer;
                }
            }
            return null;
        }

        private void SetupVideo(Renderer renderer, List<string> directories, List<string> namesToTry, bool playOnAwake, float audioMinDistance, float audioMaxDistance, AnimationCurve volumeCurve)
        {
            string videopath = _videoCache.Load(directories, namesToTry);
            if (string.IsNullOrEmpty(videopath))
            {
                return;
            }

            AudioSource audioSource  = renderer.gameObject.AddComponentIfNotFound<AudioSource>();
            audioSource.playOnAwake  = false;
            audioSource.dopplerLevel = 0f;
            audioSource.spatialBlend = 1f;
            audioSource.minDistance  = audioMinDistance;
            audioSource.maxDistance  = audioMaxDistance;
            audioSource.volume       = 1f;
            audioSource.rolloffMode  = AudioRolloffMode.Custom;
            audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, volumeCurve);

            VideoPlayer videoPlayer    = renderer.gameObject.AddComponentIfNotFound<VideoPlayer>();
            videoPlayer.errorReceived -= OnVideoPlayerErrorReceived;
            videoPlayer.errorReceived += OnVideoPlayerErrorReceived;
            if (playOnAwake)
            {
                videoPlayer.prepareCompleted -= OnVideoPlayerPrepareCompleted;
                videoPlayer.prepareCompleted += OnVideoPlayerPrepareCompleted;
            }
            videoPlayer.playOnAwake               = playOnAwake;
            videoPlayer.waitForFirstFrame         = true;
            videoPlayer.isLooping                 = true;
            videoPlayer.skipOnDrop                = true;
            videoPlayer.source                    = VideoSource.Url;
            videoPlayer.url                       = videopath;
            videoPlayer.renderMode                = VideoRenderMode.MaterialOverride;
            videoPlayer.audioOutputMode           = VideoAudioOutputMode.AudioSource;
            videoPlayer.controlledAudioTrackCount = 1;
            videoPlayer.targetMaterialProperty    = MaterialUtils.SHADER_EMISSIVE_TEXTURE_NAME;
            if (playOnAwake)
            {
                videoPlayer.Prepare();
            }
            else
            {
                videoPlayer.Stop();
            }

            OnVideoPlayerAdded?.Invoke();
        }

        private static void OnVideoPlayerErrorReceived(VideoPlayer _, string message) => Debug.Log($"Error: {message}");

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
