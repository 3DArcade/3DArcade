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

using SK.Utilities.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;

namespace Arcade
{
    public static class ArtworkUtils
    {
        public static event System.Action OnVideoPlayerAdded;

        private static readonly AssetCache<Texture> _textureCache;
        private static readonly AssetCache<string> _videoCache;

        static ArtworkUtils()
        {
            _textureCache = new TextureCache();
            _videoCache   = new VideoCache();
        }

        public static void SetupImages(IEnumerable<string> directories, IEnumerable<string> namesToTry, Renderer[] renderers, bool isMarqueeNode)
        {
            Texture[] textures = _textureCache.LoadMultiple(directories, namesToTry);

            if (textures == null)
                return;

            if (textures.Length > 0)
            {
                for (int i = 0; i < renderers.Length; i++)
                {
                    renderers[i].material.SetEmissionTexture(textures[0]);

                    // Only setup magic pixels for the first Marquee Node found
                    if (i == 0 && isMarqueeNode)
                        SetupMagicPixels(renderers[i]);
                }
            }

            if (textures.Length > 1)
                SetupImageCycling(renderers, textures);
            else if (textures.Length == 1)
                SetupStaticImage(renderers, textures[0]);
        }

        public static void SetupVideos(IEnumerable<string> directories, IEnumerable<string> namesToTry, Renderer[] renderers, float audioMinDistance, float audioMaxDistance, AnimationCurve volumeCurve)
        {
            string videopath = _videoCache.Load(directories, namesToTry);
            if (string.IsNullOrEmpty(videopath))
                return;

            foreach (Renderer renderer in renderers)
            {
                AudioSource audioSource  = renderer.gameObject.AddComponentIfNotFound<AudioSource>();
                audioSource.playOnAwake  = false;
                audioSource.dopplerLevel = 0f;
                audioSource.spatialBlend = 1f;
                audioSource.minDistance  = audioMinDistance;
                audioSource.maxDistance  = audioMaxDistance;
                audioSource.volume       = 1f;
                audioSource.rolloffMode  = AudioRolloffMode.Custom;
                audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, volumeCurve);

                VideoPlayer videoPlayer               = renderer.gameObject.AddComponentIfNotFound<VideoPlayer>();
                videoPlayer.errorReceived            -= OnVideoPlayerErrorReceived;
                videoPlayer.errorReceived            += OnVideoPlayerErrorReceived;
                videoPlayer.playOnAwake               = false;
                videoPlayer.waitForFirstFrame         = true;
                videoPlayer.isLooping                 = true;
                videoPlayer.skipOnDrop                = true;
                videoPlayer.source                    = VideoSource.Url;
                videoPlayer.url                       = videopath;
                videoPlayer.renderMode                = VideoRenderMode.MaterialOverride;
                videoPlayer.targetMaterialProperty    = MaterialUtils.SHADER_EMISSIVE_TEXTURE_NAME;
                videoPlayer.audioOutputMode           = VideoAudioOutputMode.AudioSource;
                videoPlayer.controlledAudioTrackCount = 1;
                videoPlayer.Stop();

                OnVideoPlayerAdded?.Invoke();
            }
        }

        private static void SetupImageCycling(Renderer[] renderers, Texture[] textures)
        {
            foreach (Renderer renderer in renderers)
            {
                DynamicArtworkComponent dynamicArtworkComponent = renderer.gameObject.AddComponentIfNotFound<DynamicArtworkComponent>();
                dynamicArtworkComponent.Construct(textures);
            }
        }

        private static void SetupStaticImage(Renderer[] renderers, Texture texture)
        {
            foreach (Renderer renderer in renderers)
                renderer.material.SetEmissionTexture(texture);
        }

        private static void SetupMagicPixels(Renderer sourceRenderer)
        {
            Transform parentTransform = sourceRenderer.transform.parent;
            if (parentTransform == null)
                return;

            IEnumerable<Renderer> renderers = parentTransform.GetComponentsInChildren<Renderer>()
                                                             .Where(r => r.GetComponent<NodeTag>() == null
                                                                      && sourceRenderer.sharedMaterial.name.StartsWith(r.sharedMaterial.name));

            Color color;
            Texture texture;

            bool sourceIsEmissive = sourceRenderer.IsEmissive();

            if (sourceIsEmissive)
            {
                color = sourceRenderer.material.GetEmissionColor();
                texture = sourceRenderer.material.GetEmissionTexture();
            }
            else
            {
                color = sourceRenderer.material.GetBaseColor();
                texture = sourceRenderer.material.GetBaseTexture();
            }

            if (texture == null)
                return;

            MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();

            foreach (Renderer renderer in renderers)
            {
                renderer.GetPropertyBlock(materialPropertyBlock);

                if (renderer.IsEmissive())
                {
                    materialPropertyBlock.SetColor(MaterialUtils.SHADER_EMISSIVE_COLOR_ID, color);
                    materialPropertyBlock.SetTexture(MaterialUtils.SHADER_EMISSIVE_TEXTURE_ID, texture);
                }
                else
                {
                    color = sourceIsEmissive ? Color.white : color;
                    materialPropertyBlock.SetColor(MaterialUtils.SHADER_BASE_COLOR_ID, color);
                    materialPropertyBlock.SetTexture(MaterialUtils.SHADER_BASE_TEXTURE_ID, texture);
                }

                for (int i = 0; i < renderer.materials.Length; ++i)
                    renderer.SetPropertyBlock(materialPropertyBlock, i);
            }
        }

        private static void OnVideoPlayerErrorReceived(VideoPlayer _, string message)
            => Debug.LogError($"OnVideoPlayerErrorReceived: {message}");
    }
}
