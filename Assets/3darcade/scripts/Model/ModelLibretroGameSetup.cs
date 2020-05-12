using SK.Libretro;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Arcade
{
    public class ModelLibretroGameSetup : MonoBehaviour
    {
        [Range(0.5f, 2f)]
        [SerializeField] private float _timeScale = 1.0f;

        ModelSetup modelSetup;

        public Wrapper Wrapper { get; private set; }

        private Renderer _rendererComponent = null;
        private Material _originalMaterial = null;

        private float _frameTimer;

#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        private readonly bool useUnityAudio = true;
#else
        private readonly bool useUnityAudio = false;
#endif

        private bool useRunLoop = true;
        private bool activateGraphics = true; // When Update() is used we need to activate graphics there so unityGraphics.TextureUpdated is at least once true

        void Start()
        {
            modelSetup = gameObject.GetComponent<ModelSetup>();
            if (modelSetup == null)
            {
                print("Error - no modelsetup found for " + gameObject.name);
            }
        }

        private void Update()
        {
            //if (savedSelected != modelSetup.isSelected)
            //{
            //    if (modelSetup.isSelected == true)
            //    {
            //        savedSelected = true;
            //      //  ResumeGame();
            //       // print("Resume " + modelSetup.descriptiveName);
            //    }
            //    else
            //    {
            //        savedSelected = false;
            //        PauseGame(false, true, true);
            //      //  print("Pause " + modelSetup.descriptiveName);
            //    }
            //}

            if (useRunLoop)
            {
                return;
            }

            if (activateGraphics)
            {
                ActivateGraphics();
                activateGraphics = false;
            }

            if (Wrapper != null && Wrapper.Game.SystemAVInfo.timing.fps > 0.0)
            {
                _frameTimer += Time.deltaTime;
                float timePerFrame = 1f / (float)Wrapper.Game.SystemAVInfo.timing.fps / _timeScale;

                while (_frameTimer >= timePerFrame)
                {
                    Wrapper.Update();
                    _frameTimer -= timePerFrame;
                }
                if (Wrapper.GraphicsProcessor != null && Wrapper.GraphicsProcessor is UnityGraphicsProcessor unityGraphics)
                {
                    if (unityGraphics.TextureUpdated)
                    {
                        _rendererComponent.material.SetTexture("_EmissionMap", unityGraphics.Texture);
                    }
                }
            }
        }

        public void StartGame(string core, string gameDirectory, string gameName, bool runLoop = true)
        {
            useRunLoop = runLoop;
            if (!string.IsNullOrEmpty(core) && !string.IsNullOrEmpty(gameDirectory) && !string.IsNullOrEmpty(gameName))
            {
                if (transform.childCount > 0)
                {
                    Transform modelTransform = transform.GetChild(0);
                    if (modelTransform != null && modelTransform.childCount > 1)
                    {
                        Transform screenTransform = modelTransform.GetChild(1);
                        if (screenTransform.TryGetComponent(out _rendererComponent))
                        {
                            Wrapper = new Wrapper();
                            if (Wrapper.StartGame(core, gameDirectory, gameName))
                            {
                                ActivateGraphics();
                                ActivateAudio();
                                ActivateInput();
                                _originalMaterial = _rendererComponent.material;
                                Material _newMaterial = new Material(_rendererComponent.material);
                                _rendererComponent.material = _newMaterial;
                                _rendererComponent.material.mainTextureScale = new Vector2(1f, -1f);
                                _rendererComponent.material.color = Color.black;
                                _rendererComponent.material.EnableKeyword("_EMISSION");
                                _rendererComponent.material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
                                _rendererComponent.material.SetColor("_EmissionColor", Color.white);
                                if (useRunLoop) { InvokeRepeating("LibretroRunLoop", 0f, 1f / (float)Wrapper.Game.SystemAVInfo.timing.fps); }
                            }
                            else
                            {
                                Wrapper.StopGame();
                                Wrapper = null;
                            }
                        }
                    }
                }
            }
        }

        [SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Called by InvokeRepeating")]
        private void LibretroRunLoop()
        {
            if (Wrapper != null)
            {
                Wrapper.Update();
                if (Wrapper.GraphicsProcessor != null && Wrapper.GraphicsProcessor is UnityGraphicsProcessor unityGraphics)
                {
                    if (unityGraphics.TextureUpdated)
                    {
                        _rendererComponent.material.SetTexture("_EmissionMap", unityGraphics.Texture);
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
            CancelInvoke();
            Wrapper?.StopGame();
            Wrapper = null;
            DeactivateInput();
            DeactivateAudio();
            UnityAudioProcessorComponent unityAudio = GetComponent<UnityAudioProcessorComponent>();
            if (unityAudio != null) { Destroy(unityAudio); }
            Destroy(this);
        }

        public void PauseGame(bool pauseGraphics, bool pauseInput, bool pauseAudio)
        {
            if (Wrapper == null)
            {
                return;
            }
            if (pauseInput) { DeactivateInput(); }
            if (pauseGraphics) { DeactivateGraphics(); }
            if (pauseAudio) { DeactivateAudio(); }
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

        public void ActivateGraphics()
        {
            Wrapper?.ActivateGraphics(new UnityGraphicsProcessor());
        }

        public void DeactivateGraphics()
        {
            Wrapper?.DeactivateGraphics();
        }

        public void ActivateAudio()
        {
            //  var unityAudioProcessorComponent = gameObject.AddComponent<UnityAudioProcessorComponent>();
            UnityAudioProcessorComponent unityAudio = GetComponent<UnityAudioProcessorComponent>();
            if (unityAudio == null && useUnityAudio) { unityAudio = gameObject.AddComponent<UnityAudioProcessorComponent>(); }
            if (unityAudio != null)
            {
                Wrapper?.ActivateAudio(unityAudio);
            }
            else
            {
                Wrapper?.ActivateAudio(new NAudioAudioProcessor());
            }
        }

        public void DeactivateAudio()
        {
            Wrapper?.DeactivateAudio();
        }

        public void ActivateInput()
        {
            Wrapper?.ActivateInput(FindObjectOfType<PlayerInputManager>().GetComponent<IInputProcessor>());
        }

        public void DeactivateInput()
        {
            Wrapper?.DeactivateInput();
        }
    }
}
