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

namespace Arcade_r
{
    public sealed class LibretroLauncher : MonoBehaviour
    {
        [SerializeField, Range(0.5f, 2f)] private float _timeScale = 1.0f;
        [SerializeField] private bool _useAudioRateForSync = false;
        [SerializeField] private float _audioMaxVolume = 1f;
        [SerializeField] private float _audioMinDistance = 2f;
        [SerializeField] private float _audioMaxDistance = 10f;
        [SerializeField] private FilterMode _videoFilterMode = FilterMode.Point;

        public Wrapper Wrapper { get; private set; }

        public bool VideoEnabled
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

        public float AudioMaxVolume
        {
            get => _audioMaxVolume;
            set
            {
                _audioMaxVolume = value;
                AudioSetVolume(value);
            }
        }

        private Transform _player;

        private Transform _screenTransform = null;
        private Renderer _rendererComponent = null;
        private Material _originalMaterial = null;

        private float _gameFps = 0f;
        private float _gameSampleRate = 0f;

        private float _frameTimer = 0f;

        private bool _graphicsEnabled = false;
        private bool _audioEnabled = false;
        private bool _inputEnabled = false;

        private void Awake()
        {
            _player = FindObjectOfType<PlayerControls>().transform;
        }

        private void Update()
        {
            if (Wrapper != null && _gameFps > 0f && _gameSampleRate > 0f)
            {
                _frameTimer += Time.deltaTime;
                float targetFrameTime = 1f / (_useAudioRateForSync ? _gameSampleRate / 1000f : _gameFps) / _timeScale;
                while (_frameTimer >= targetFrameTime)
                {
                    Wrapper.Update();
                    _frameTimer -= targetFrameTime;
                }

                GraphicsSetFilterMode(_videoFilterMode);

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
                if (AudioVolumeControlledByDistance && Wrapper.AudioProcessor != null && Wrapper.AudioProcessor is SK.Libretro.NAudio.AudioProcessor NAudioAudio)
                {
                    float distance = Vector3.Distance(transform.position, _player.transform.position);
                    if (distance > 0f)
                    {
                        float volume = Mathf.Clamp(Mathf.Pow((distance - _audioMaxDistance) / (_audioMinDistance - _audioMaxDistance), 2f), 0f, _audioMaxVolume);
                        NAudioAudio.SetVolume(volume);
                    }
                }
#endif
            }
        }

        public void StartGame(string core, string gameDirectory, string gameName)
        {
            if (!string.IsNullOrEmpty(core) && !string.IsNullOrEmpty(gameDirectory) && !string.IsNullOrEmpty(gameName))
            {
                if (transform.childCount > 1)
                {
                    _screenTransform = transform.GetChild(1);
                    if (_screenTransform.TryGetComponent(out _rendererComponent))
                    {
                        Wrapper = new Wrapper((TargetPlatform)Application.platform, $"{Application.streamingAssetsPath}/3darcade~/Libretro");

                        if (Wrapper.StartGame(core, gameDirectory, gameName))
                        {
                            _gameFps = (float)Wrapper.Game.SystemAVInfo.timing.fps;
                            _gameSampleRate = (float)Wrapper.Game.SystemAVInfo.timing.sample_rate;

                            VideoEnabled = true;
                            AudioEnabled = true;
                            InputEnabled = true;

                            _originalMaterial = _rendererComponent.sharedMaterial;
                            _rendererComponent.material.color = Color.black;
                            _rendererComponent.material.EnableKeyword("_EMISSION");
                            _rendererComponent.material.SetColor("_EmissionColor", Color.white * 1.2f);
                        }
                        else
                        {
                            StopGame();
                        }
                    }
                }
            }
        }

        public void StopGame()
        {
            if (_rendererComponent != null && _rendererComponent.material != null && _originalMaterial != null)
            {
                _rendererComponent.material = _originalMaterial;
                _originalMaterial = null;
            }

            if (Wrapper == null)
            {
                return;
            }

            Wrapper?.StopGame();
            Wrapper = null;

            if (TryGetComponent(out SK.Libretro.Unity.AudioProcessor unityAudio))
            {
                Destroy(unityAudio);
            }

            Destroy(this);
        }

        public void PauseGame(bool pauseGraphics, bool pauseInput, bool pauseAudio)
        {
            if (Wrapper == null)
            {
                return;
            }

            if (pauseInput)
            {
                DeactivateInput();
            }

            if (pauseGraphics)
            {
                DeactivateGraphics();
            }

            if (pauseAudio)
            {
                DeactivateAudio();
            }
        }

        public void ResumeGame()
        {
            if (Wrapper == null)
            {
                return;
            }

            ActivateGraphics();
            ActivateAudio();
            ActivateInput();
        }

        private void GraphicsSetFilterMode(FilterMode filterMode)
        {
            if (Wrapper != null && Wrapper.GraphicsProcessor != null && Wrapper.GraphicsProcessor is SK.Libretro.Unity.GraphicsProcessor unityGraphics)
            {
                unityGraphics.VideoFilterMode = filterMode;
            }
        }

        private void ActivateGraphics()
        {
            SK.Libretro.Unity.GraphicsProcessor unityGraphics = new SK.Libretro.Unity.GraphicsProcessor
            {
                OnTextureRecreated = GraphicsSetTextureCallback,
                VideoFilterMode = _videoFilterMode
            };
            Wrapper?.ActivateGraphics(unityGraphics);
            _graphicsEnabled = true;
        }

        private void DeactivateGraphics()
        {
            Wrapper?.DeactivateGraphics();
            _graphicsEnabled = false;
        }

        private void ActivateAudio()
        {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            SK.Libretro.Unity.AudioProcessor unityAudio = GetComponentInChildren<SK.Libretro.Unity.AudioProcessor>();
            if (unityAudio != null)
            {
                Wrapper?.ActivateAudio(unityAudio);
            }
            else
            {
                Wrapper?.ActivateAudio(new SK.Libretro.NAudio.AudioProcessor());
            }
#else
            Libretro.UnityAudioProcessorComponent unityAudio = GetComponentInChildren<Libretro.UnityAudioProcessorComponent>(true);
            if (unityAudio != null)
            {
                unityAudio.gameObject.SetActive(true);
                Wrapper?.ActivateAudio(unityAudio);
            }
            else
            {
                GameObject audioProcessorGameObject = new GameObject("AudioProcessor");
                audioProcessorGameObject.transform.SetParent(_screenTransform);
                Libretro.UnityAudioProcessorComponent audioProcessorComponent = audioProcessorGameObject.AddComponent<Libretro.UnityAudioProcessorComponent>();
                Wrapper?.ActivateAudio(audioProcessorComponent);
            }
#endif
            _audioEnabled = true;
        }

        private void DeactivateAudio()
        {
            Wrapper?.DeactivateAudio();
            _audioEnabled = false;
        }

        private void ActivateInput()
        {
            Wrapper?.ActivateInput(FindObjectOfType<PlayerInputManager>().GetComponent<IInputProcessor>());
            _inputEnabled = true;
        }

        private void DeactivateInput()
        {
            Wrapper?.DeactivateInput();
            _inputEnabled = false;
        }

        private void GraphicsSetTextureCallback(Texture2D texture)
        {
            if (_rendererComponent != null)
            {
                _rendererComponent.material.SetTexture("_EmissionMap", texture);
            }
        }

        private void AudioSetVolume(float volume)
        {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            if (Wrapper.AudioProcessor != null && Wrapper.AudioProcessor is SK.Libretro.NAudio.AudioProcessor NAudioAudio)
            {
                NAudioAudio.SetVolume(Mathf.Clamp(volume, 0f, _audioMaxVolume));
            }
#endif
        }
    }
}
