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

namespace Arcade_r
{
    public sealed class LibretroController
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
        private const float _timeScale            = 1.0f;
        private const bool _useAudioRateForSync   = false;
        private const float _audioMaxVolume       = 1f;
        private const float _audioMinDistance     = 2f;
        private const float _audioMaxDistance     = 10f;
        private const FilterMode _videoFilterMode = FilterMode.Point;
        //

        private readonly Transform _player;

        private LibretroWrapper _libretroWrapper;
        private ScreenNodeTag _screenNode;
        private Renderer _rendererComponent = null;
        private Material _originalMaterial  = null;

        private float _gameFps        = 0f;
        private float _gameSampleRate = 0f;

        private float _frameTimer = 0f;

        private bool _graphicsEnabled = false;
        private bool _audioEnabled    = false;
        private bool _inputEnabled    = false;

        public LibretroController(Transform player)
        {
            Assert.IsNotNull(player);
            _player = player;
        }

        public bool StartContent(ScreenNodeTag screenNode, string core, string contentDirectory, string contentName)
        {
            Assert.IsNotNull(screenNode);
            _screenNode = screenNode;

            if (string.IsNullOrEmpty(core) || string.IsNullOrEmpty(contentDirectory) || string.IsNullOrEmpty(contentName))
            {
                return false;
            }

            if (!_screenNode.TryGetComponent(out _rendererComponent))
            {
                return false;
            }

            _libretroWrapper = new LibretroWrapper((TargetPlatform)Application.platform, $"{SystemUtils.GetDataPath()}/3darcade_r~/Libretro");

            if (_libretroWrapper.StartGame(core, contentDirectory, contentName))
            {
                _gameFps        = (float)_libretroWrapper.Game.SystemAVInfo.timing.fps;
                _gameSampleRate = (float)_libretroWrapper.Game.SystemAVInfo.timing.sample_rate;

                if (_gameFps > 0f && _gameSampleRate > 0f)
                {
                    ActivateGraphics();
                    ActivateAudio();
                    ActivateInput();

                    _originalMaterial = _rendererComponent.sharedMaterial;

                    _rendererComponent.material.color = Color.black;
                    _rendererComponent.material.EnableKeyword("_EMISSION");
                    _rendererComponent.material.SetColor("_EmissionColor", Color.white * 1.08f);

                    return true;
                }
                else
                {
                    StopContent();
                }
            }
            else
            {
                StopContent();
            }

            return false;
        }

        public void UpdateContent(float dt)
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

        public void StopContent()
        {
            if (_rendererComponent != null && _rendererComponent.material != null && _originalMaterial != null)
            {
                _rendererComponent.material = _originalMaterial;
                _originalMaterial           = null;
            }

            _libretroWrapper?.StopGame();
            _libretroWrapper = null;

            if (_screenNode != null && _screenNode.TryGetComponent(out SK.Libretro.Unity.AudioProcessor unityAudio))
            {
                Object.Destroy(unityAudio);
            }

            _screenNode = null;
        }

        public void PauseContent(bool pauseGraphics, bool pauseAudio, bool pauseInput)
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

        public void ResumeContent()
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
            SK.Libretro.Unity.AudioProcessor unityAudio = _screenNode.GetComponentInChildren< SK.Libretro.Unity.AudioProcessor>(true);
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
    }
}
