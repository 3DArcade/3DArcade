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

using SK.Libretro.Unity;
using SK.Utilities.Unity;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Video;

namespace Arcade
{
    public sealed class InternalGameController
    {
        private static Material _screenMaterial;

        private readonly Transform _player;

        private LibretroBridge _libretroBridge                   = null;
        private ScreenNodeTag _screenNode                        = null;
        private DynamicArtworkComponent _dynamicArtworkComponent = null;
        private VideoRenderMode _originalVideoRenderMode         = VideoRenderMode.MaterialOverride;

        public InternalGameController(Transform player)
        {
            Assert.IsNotNull(player);
            if (_player == null)
                _player = player;

            if (_screenMaterial == null)
                _screenMaterial = Resources.Load<Material>("Materials/ScreenMaterial");
        }

        public bool StartGame(ScreenNodeTag screenNodeTag, string core, string[] gameDirectories, string gameName)
        {
            ResetFields();

            Assert.IsNotNull(screenNodeTag);
            _screenNode = screenNodeTag;

            if (!_screenNode.TryGetComponent(out Renderer _))
                return false;

            if (string.IsNullOrEmpty(core) || gameDirectories == null || gameDirectories.Length < 1 || string.IsNullOrEmpty(gameName))
                return false;

            if (_screenNode.TryGetComponent(out _dynamicArtworkComponent))
                _dynamicArtworkComponent.EnableCycling = false;

            ScreenNode screenNode = screenNodeTag.gameObject.AddComponentIfNotFound<ScreenNode>();
            _libretroBridge = new LibretroBridge(screenNode, _player);

            foreach (string gameDirectory in gameDirectories)
            {
                if (_libretroBridge.Start(core, gameDirectory, gameName))
                {
                    _screenNode.GetComponent<Renderer>().material = _screenMaterial;
                    _libretroBridge.InputEnabled = true;

                    SaveVideoPlayerRenderMode();
                    SetVideoPlayerRenderState(false);

                    return true;
                }
                else
                    StopGame();
            }

            StopGame();

            return false;
        }

        public void UpdateGame() => _libretroBridge?.Update();

        public void StopGame()
        {
            RestoreVideoPlayerRenderMode();

            _libretroBridge?.Stop();
            _libretroBridge = null;

            if (_screenNode != null && _screenNode.gameObject.TryGetComponent(out ScreenNode screenNode))
                Object.Destroy(screenNode);

            if (_dynamicArtworkComponent != null)
                _dynamicArtworkComponent.EnableCycling = true;

            ResetFields();
        }

        private void SaveVideoPlayerRenderMode()
        {
            if (_screenNode != null && _screenNode.TryGetComponent(out VideoPlayer videoPlayer))
                _originalVideoRenderMode = videoPlayer.renderMode;
            else
                _originalVideoRenderMode = VideoRenderMode.MaterialOverride;
        }

        private void SetVideoPlayerRenderState(bool enabled)
        {
            if (_screenNode != null && _screenNode.TryGetComponent(out VideoPlayer videoPlayer))
                videoPlayer.renderMode = enabled ? VideoRenderMode.MaterialOverride : VideoRenderMode.APIOnly;
        }

        private void RestoreVideoPlayerRenderMode()
        {
            if (_screenNode != null && _screenNode.TryGetComponent(out VideoPlayer videoPlayer))
                videoPlayer.renderMode = _originalVideoRenderMode;
        }

        private void ResetFields()
        {
            _libretroBridge          = null;
            _screenNode              = null;
            _dynamicArtworkComponent = null;
            _originalVideoRenderMode = VideoRenderMode.MaterialOverride;
        }
    }
}
