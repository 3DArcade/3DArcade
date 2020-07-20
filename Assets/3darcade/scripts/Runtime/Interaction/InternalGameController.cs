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
using SK.Libretro;
using UnityEngine.InputSystem;
using Unity.Mathematics;
using UnityEngine.Assertions;
using UnityEngine.Video;

namespace Arcade
{
    public sealed class InternalGameController
    {
        public bool GraphicsEnabled
        {
            get => _graphicsEnabled;
            set
            {
                if (value)
                {
                    ActivateGraphics();
                }
                else
                {
                    DeactivateGraphics();
                }
            }
        }

        public bool AudioEnabled
        {
            get => _audioEnabled;
            set
            {
                if (value)
                {
                    ActivateAudio();
                }
                else
                {
                    DeactivateAudio();
                }
            }
        }

        public bool InputEnabled
        {
            get => _inputEnabled;
            set
            {
                if (value)
                {
                    ActivateInput();
                }
                else
                {
                    DeactivateInput();
                }
            }
        }

        public bool AudioVolumeControlledByDistance { get; set; } = true;

        // TODO(Tom): Make these customizable
        private readonly float _timeScale            = 1.0f;
        private readonly bool _useAudioRateForSync   = false;
        private readonly float _audioMaxVolume       = 1f;
        private readonly float _audioMinDistance     = 2f;
        private readonly float _audioMaxDistance     = 10f;
        private readonly FilterMode _videoFilterMode = FilterMode.Point;
        //

        private readonly Transform _player;

        private LibretroWrapper _libretroWrapper         = null;
        private MonoBehaviour _screenNode                = null;
        private Renderer _rendererComponent              = null;
        private Material _originalMaterial               = null;
        private VideoRenderMode _originalVideoRenderMode = VideoRenderMode.MaterialOverride;

        private float _gameFps        = 0f;
        private float _gameSampleRate = 0f;

        private float _frameTimer = 0f;

        private bool _graphicsEnabled = false;
        private bool _audioEnabled    = false;
        private bool _inputEnabled    = false;

        public InternalGameController(Transform player)
        {
            Assert.IsNotNull(player);
            _player = player;
        }

        public bool StartGame(MonoBehaviour screenNode, string core, string gameDirectory, string gameName)
        {
            ResetFields();

            Assert.IsNotNull(screenNode);
            _screenNode = screenNode;

            if (!_screenNode.TryGetComponent(out _rendererComponent))
            {
                return false;
            }

            if (string.IsNullOrEmpty(core) || string.IsNullOrEmpty(gameDirectory) || string.IsNullOrEmpty(gameName))
            {
                return false;
            }

            _libretroWrapper = new LibretroWrapper((TargetPlatform)Application.platform, $"{SystemUtils.GetDataPath()}/3darcade~/Libretro");
            if (_libretroWrapper.StartGame(core, gameDirectory, gameName))
            {
                _gameFps        = (float)_libretroWrapper.Game.SystemAVInfo.timing.fps;
                _gameSampleRate = (float)_libretroWrapper.Game.SystemAVInfo.timing.sample_rate;

                if (_gameFps > 0f && _gameSampleRate > 0f)
                {
                    ActivateGraphics();
                    ActivateAudio();
                    ActivateInput();

                    SaveVideoPlayerRenderMode();
                    SetVideoPlayerRenderState(false);

                    SaveMaterial();
                    SetMaterialProperties();

                    return true;
                }
            }

            StopGame();
            return false;
        }

        public void UpdateGame(float dt)
        {
            if (_libretroWrapper == null)
            {
                return;
            }

            _frameTimer += dt;
            float targetFrameTime = 1f / (_useAudioRateForSync ? _gameSampleRate / 1000f : _gameFps) / _timeScale;
            while (_frameTimer >= targetFrameTime)
            {
                _libretroWrapper.Update();
                _frameTimer -= targetFrameTime;
            }

            GraphicsSetFilterMode(_videoFilterMode);

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            if (AudioVolumeControlledByDistance && _libretroWrapper.AudioProcessor != null && _libretroWrapper.AudioProcessor is SK.Libretro.NAudio.AudioProcessor NAudioAudio)
            {
                float distance = MathUtils.DistanceFast(_screenNode.transform.position, _player.transform.position);
                if (distance > 0f)
                {
                    float volume = math.clamp(math.pow((distance - _audioMaxDistance) / (_audioMinDistance - _audioMaxDistance), 2f), 0f, _audioMaxVolume);
                    NAudioAudio.SetVolume(volume);
                }
            }
#endif
        }

        public void StopGame()
        {
            RestoreMaterial();
            RestoreVideoPlayerRenderMode();

            _libretroWrapper?.StopGame();
            _libretroWrapper = null;

#if !UNITY_EDITOR_WIN || !UNITY_STANDALONE_WIN
            if (_screenNode != null)
            {
                SK.Libretro.Unity.AudioProcessor unityAudio = _screenNode.GetComponentInChildren<SK.Libretro.Unity.AudioProcessor>();
                if (unityAudio != null)
                {
                    Object.Destroy(unityAudio.gameObject, 1f);
                }
            }
#endif
            _screenNode = null;
        }

        public void PauseGame(bool pauseGraphics, bool pauseAudio, bool pauseInput)
        {
            if (_libretroWrapper == null)
            {
                return;
            }

            if (pauseGraphics)
            {
                DeactivateGraphics();
            }

            if (pauseAudio)
            {
                DeactivateAudio();
            }

            if (pauseInput)
            {
                DeactivateInput();
            }
        }

        public void ResumeGame()
        {
            if (_libretroWrapper == null)
            {
                return;
            }

            ActivateGraphics();
            ActivateAudio();
            ActivateInput();
        }

        public void GraphicsSetFilterMode(FilterMode filterMode)
        {
            if (_libretroWrapper != null && _libretroWrapper.GraphicsProcessor is SK.Libretro.Unity.GraphicsProcessor unityGraphics)
            {
                unityGraphics.VideoFilterMode = filterMode;
            }
        }

        public void AudioSetVolume(float volume)
        {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            if (_libretroWrapper.AudioProcessor != null && _libretroWrapper.AudioProcessor is SK.Libretro.NAudio.AudioProcessor NAudioAudio)
            {
                NAudioAudio.SetVolume(Mathf.Clamp(volume, 0f, _audioMaxVolume));
            }
#endif
        }

        private void ActivateGraphics()
        {
            SK.Libretro.Unity.GraphicsProcessor unityGraphics = new SK.Libretro.Unity.GraphicsProcessor
            {
                OnTextureRecreated = OnGraphicsTextureChanged,
                VideoFilterMode    = _videoFilterMode
            };
            _libretroWrapper?.ActivateGraphics(unityGraphics);
            _graphicsEnabled = true;
        }

        private void DeactivateGraphics()
        {
            _libretroWrapper?.DeactivateGraphics();
            _graphicsEnabled = false;
        }

        private void ActivateAudio()
        {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            SK.Libretro.Unity.AudioProcessor unityAudio = _screenNode.GetComponentInChildren<SK.Libretro.Unity.AudioProcessor>();
            if (unityAudio != null)
            {
                _libretroWrapper?.ActivateAudio(unityAudio);
            }
            else
            {
                _libretroWrapper?.ActivateAudio(new SK.Libretro.NAudio.AudioProcessor());
            }
#else
            SK.Libretro.Unity.AudioProcessor unityAudio = _screenNode.GetComponentInChildren<SK.Libretro.Unity.AudioProcessor>(true);
            if (unityAudio != null)
            {
                unityAudio.gameObject.SetActive(true);
                _libretroWrapper?.ActivateAudio(unityAudio);
            }
            else
            {
                GameObject audioProcessorGameObject = new GameObject("AudioProcessor");
                audioProcessorGameObject.transform.SetParent(_screenNode.transform);
                SK.Libretro.Unity.AudioProcessor audioProcessorComponent = audioProcessorGameObject.AddComponent<SK.Libretro.Unity.AudioProcessor>();
                _libretroWrapper?.ActivateAudio(audioProcessorComponent);
            }
#endif
            _audioEnabled = true;
        }

        private void DeactivateAudio()
        {
            _libretroWrapper?.DeactivateAudio();
            _audioEnabled = false;
        }

        private void ActivateInput()
        {
            _libretroWrapper?.ActivateInput(Object.FindObjectOfType<PlayerInputManager>().GetComponent<IInputProcessor>());
            _inputEnabled = true;
        }

        private void DeactivateInput()
        {
            _libretroWrapper?.DeactivateInput();
            _inputEnabled = false;
        }

        private void OnGraphicsTextureChanged(Texture2D texture)
        {
            if (_rendererComponent != null)
            {
                _rendererComponent.material.SetTexture("_EmissionMap", texture);
            }
        }

        private void SaveVideoPlayerRenderMode()
        {
            if (_screenNode != null && _screenNode.TryGetComponent(out VideoPlayer videoPlayer))
            {
                _originalVideoRenderMode = videoPlayer.renderMode;
            }
            else
            {
                _originalVideoRenderMode = VideoRenderMode.MaterialOverride;
            }
        }

        private void SetVideoPlayerRenderState(bool enabled)
        {
            if (_screenNode != null && _screenNode.TryGetComponent(out VideoPlayer videoPlayer))
            {
                videoPlayer.renderMode = enabled ? VideoRenderMode.MaterialOverride : VideoRenderMode.APIOnly;
            }
        }

        private void RestoreVideoPlayerRenderMode()
        {
            if (_screenNode != null && _screenNode.TryGetComponent(out VideoPlayer videoPlayer))
            {
                videoPlayer.renderMode = _originalVideoRenderMode;
            }
        }

        private void SaveMaterial()
        {
            if (_rendererComponent != null)
            {
                _originalMaterial = _rendererComponent.sharedMaterial;
            }
        }

        private void SetMaterialProperties()
        {
            if (_rendererComponent != null)
            {
                _rendererComponent.material.color = Color.black;
                _rendererComponent.material.EnableKeyword("_EMISSION");
                _rendererComponent.material.SetColor("_EmissionColor", Color.white * 1.08f);
            }
        }

        private void RestoreMaterial()
        {
            if (_rendererComponent != null && _rendererComponent.material != null && _originalMaterial != null)
            {
                _rendererComponent.material = _originalMaterial;
            }
        }

        private void ResetFields()
        {
            _libretroWrapper         = null;
            _screenNode              = null;
            _rendererComponent       = null;
            _originalMaterial        = null;
            _originalVideoRenderMode = VideoRenderMode.MaterialOverride;
            _gameFps                 = 0f;
            _gameSampleRate          = 0f;
            _frameTimer              = 0f;
            _graphicsEnabled         = false;
            _audioEnabled            = false;
            _inputEnabled            = false;
        }
    }
}
