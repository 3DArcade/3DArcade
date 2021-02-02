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

using UnityEngine;
using UnityEngine.Video;

namespace Arcade
{
    [DisallowMultipleComponent, RequireComponent(typeof(Renderer))]
    public class DynamicArtworkComponent : MonoBehaviour
    {
        public bool EnableCycling { get; set; } = true;

        private float _imageCyclingDelay = 0.6f;
        private readonly bool _useRandomDelay = false;
        private readonly bool _changeRandomDelayAfterTimerEnds = false;

        private bool VideoIsPlaying => _videoPlayer != null && _videoPlayer.enabled && _videoPlayer.isPlaying;

        private Renderer _renderer;
        private VideoPlayer _videoPlayer;

        private Texture[] _imageCyclingTextures;

        private MaterialPropertyBlock _materialPropertyBlock;
        private int _imageCyclingIndex;
        private float _imageCyclingTimer;

        private void OnEnable()
        {
            ArtworkUtils.OnVideoPlayerAdded += OnVideoPlayerAdded;

            Construct(null);
            if (_useRandomDelay)
                _imageCyclingDelay = Random.Range(0.4f, 1f);
        }

        private void OnVideoPlayerAdded() => _videoPlayer = GetComponent<VideoPlayer>();

        private void OnDisable() => ArtworkUtils.OnVideoPlayerAdded -= OnVideoPlayerAdded;

        private void Update()
        {
            if (VideoIsPlaying || !EnableCycling)
                return;

            if (_imageCyclingTextures.Length < 2)
                return;

            if ((_imageCyclingTimer += Time.deltaTime) >= _imageCyclingDelay)
            {
                CycleTexture();
                if (_changeRandomDelayAfterTimerEnds)
                    _imageCyclingDelay = Random.Range(0.6f, 1.2f);
                _imageCyclingTimer = 0f;
            }
        }

        public void Construct(Texture[] textures)
        {
            _imageCyclingIndex     = 0;
            _renderer              = GetComponent<Renderer>();
            _materialPropertyBlock = new MaterialPropertyBlock();
            _videoPlayer           = GetComponent<VideoPlayer>();

            if (textures != null)
                _imageCyclingTextures = textures;

            if (_renderer.enabled && _imageCyclingTextures != null && _imageCyclingTextures.Length > 0)
                SwapTexture();
        }

        public void SetImageCyclingTextures(Texture[] textures) => _imageCyclingTextures = textures;

        public void SetImageCyclingDelay(float delay) => _imageCyclingDelay = Mathf.Max(0.01f, delay);

        private void CycleTexture()
        {
            if (_imageCyclingIndex++ >= _imageCyclingTextures.Length - 1)
                _imageCyclingIndex = 0;

            SwapTexture();
        }

        private void SwapTexture()
        {
            if (_imageCyclingTextures.Length < 1)
                return;

            _renderer.GetPropertyBlock(_materialPropertyBlock);

            if (_renderer.material.IsKeywordEnabled(MaterialUtils.SHADER_EMISSIVE_KEYWORD))
                _materialPropertyBlock.SetTexture(MaterialUtils.SHADER_EMISSIVE_TEXTURE_ID, _imageCyclingTextures[_imageCyclingIndex]);
            else
                _materialPropertyBlock.SetTexture(MaterialUtils.SHADER_BASE_TEXTURE_ID, _imageCyclingTextures[_imageCyclingIndex]);

            _renderer.SetPropertyBlock(_materialPropertyBlock);
        }
    }
}
