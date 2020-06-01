using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcade
{
    public class ModelVideoSetup : MonoBehaviour
    {
        public UnityEngine.Video.VideoPlayer videoPlayer = null;
        private ModelVideoEnabled videoEnabled;
        private Renderer _renderer;
        private bool setupVideoAfterGameDisable = false;
        private List<Texture2D> imageList = null;
        private bool visible = false;
        private float waitTime = 2.0f;
        private float timer = 0.0f;
        private int index = 0;
        //private int frames = 0;
        private bool selectedDone = false;
        private bool? videoIsActive;

        private bool prepareDone;

        public ModelProperties modelProperties;
        private ModelSharedProperties modelSharedProperties;
        private ModelComponentType modelComponentType = ModelComponentType.Screen;
        private Camera thisCamera;
        private GameObject dummyNode;
        //private bool isCylArcade = false;

        //private LayerMask layerMask;

        // Public so that Arcade Audio Manager can access these.
        public bool arcadeLayer = true;
        public float distance = 100000000000f;

        private float maxDistance = 3f;

        // Using Global Variables
        // ArcadeManager.arcadeCameras, get the correct active camera
        // ArcadeManager.arcadeState, the current state.
        // ArcadeStateManager.selectedModel

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        public void Setup(List<Texture2D> textureList, string videoURL, float? animatedTextureSpeed, ModelComponentType modelComponentType, ModelProperties modelProperties, ModelSharedProperties modelSharedProperties)
        {
            //Debug.Log("videosetup " + gameObject.transform.parent.name + " imagecount " + textureList.Count + " " + videoURL + " " + (videoPlayer == null));

            this.modelProperties = modelProperties;
            this.modelComponentType = modelComponentType;
            imageList = null;
            this.modelSharedProperties = modelSharedProperties;
            dummyNode = gameObject.transform.parent.transform.parent.gameObject;

            string layer = LayerMask.LayerToName(gameObject.layer);
            if (layer.StartsWith("Arcade"))
            {
                //layerMask = LayerMask.GetMask("Arcade/ArcadeModels", "Arcade/GameModels", "Arcade/PropModels");
                thisCamera = ArcadeManager.arcadeCameras[ArcadeType.FpsArcade];
                arcadeLayer = true;
            }
            else
            {
                //layerMask = LayerMask.GetMask("Menu/ArcadeModels", "Menu/GameModels", "Menu/PropModels");
                thisCamera = ArcadeManager.arcadeCameras[ArcadeType.CylMenu];
                arcadeLayer = false;
            }

            if (ArcadeManager.arcadeConfiguration.arcadeType == ArcadeType.FpsArcade.ToString() || ArcadeManager.arcadeConfiguration.arcadeType == ArcadeType.FpsMenu.ToString())
            {
                //isCylArcade = false;
                maxDistance = 5f;
            }
            else
            {
                //isCylArcade = true;
                maxDistance = 10000f;
            }

            // Setup image, nescessary for magic pixels and to get an unique material instance.
            Texture2D tex = null;
            if (textureList.Count > 0)
            { tex = textureList[0]; }
            ModelImageSetup modelImageSetup = GetComponent<ModelImageSetup>();
            if (modelImageSetup == null)
            { modelImageSetup = gameObject.AddComponent<ModelImageSetup>(); }
            modelImageSetup.Setup(tex, modelSharedProperties.renderSettings, modelProperties, modelComponentType);

            if (videoURL != null && videoPlayer == null)
            {
                videoPlayer = gameObject.GetComponent<UnityEngine.Video.VideoPlayer>();
            }
            if (animatedTextureSpeed != null)
            {
                waitTime = animatedTextureSpeed.Value;
            }
            videoEnabled = (ModelVideoEnabled)System.Enum.Parse(typeof(ModelVideoEnabled), modelSharedProperties.videoOnModelEnabled);
            if (videoURL == null)
            {
                videoEnabled = ModelVideoEnabled.Never;
            }
            else
            {
                if (videoPlayer != null && videoEnabled != ModelVideoEnabled.Never)
                {
                    videoPlayer.SetDirectAudioMute(0, true);
                    videoPlayer.enabled = true;
                    videoPlayer.url = videoURL;
                }
            }
            if (textureList.Count > 0)
            {
                imageList = textureList;
                if (_renderer != null)
                {
                    _renderer.material.mainTexture = textureList[0];
                }
            }
            else
            {
                if (_renderer != null && transform.parent.CompareTag("gamemodel") && modelComponentType == ModelComponentType.Screen)
                {
                    _renderer.material.mainTexture = Texture2D.blackTexture;
                }
            }
            if (_renderer != null && (textureList.Count > 0 || videoEnabled != ModelVideoEnabled.Never))
            {
                SetupVideoPlayer();
            }
        }

        public void SetupVideoPlayer()
        {
            if (videoEnabled == ModelVideoEnabled.Never || videoPlayer == null)
            {
                if (videoPlayer != null)
                {
                    videoPlayer.enabled = false;
                }
                SetupImageMaterial();
                if (imageList == null)
                {
                    enabled = false;
                }
                return;
            }

            ArcadeAudioManager.ActiveVideos.Add(gameObject.GetComponent<ModelVideoSetup>());

            // Setup videoplayer properties
            ulong frameCount = videoPlayer.frameCount;
            int frameCountInt = (int)frameCount;
            long randomFrame = Random.Range(0, frameCountInt);
            videoPlayer.frame = randomFrame;
            videoPlayer.isLooping = true;
            videoPlayer.waitForFirstFrame = true;
            videoPlayer.targetMaterialProperty = "_EmissionMap";
            videoPlayer.SetDirectAudioMute(0, true);

            if (_renderer != null)
            {
                // TODO: This fixes black textures when some prefabs are loaded from an assetbundle
                string shaderName = _renderer.material.shader.name;
                Shader newShader = Shader.Find(shaderName);
                if (newShader != null)
                {
                    _renderer.material.shader = newShader;
                }
                else
                {
                    Debug.LogWarning("unable to refresh shader: " + shaderName + " in material " + _renderer.material.name);
                }

                // TODO: Setup material, resize to better fit the screen. Should we still do this? I think we did this because the mame32 screenshots had a white border.
                if (modelComponentType == ModelComponentType.Screen)
                {
                    //renderer.material.SetTextureOffset("_MainTex", new Vector2(-0.075f, -0.075f));
                    //renderer.material.SetTextureScale("_MainTex", new Vector2(1.15f, 1.15f));
                }
                if (modelComponentType != ModelComponentType.Generic)
                {
                    //renderer.material.SetTexture("_MainTex", null);
                    _renderer.material.color = Color.black;
                    _renderer.material.SetColor("_EmissionColor", Color.white);
                    _renderer.material.EnableKeyword("_EMISSION");
                    _renderer.material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
                }
                if (modelComponentType == ModelComponentType.Screen)
                {
                    if (modelProperties.genre.ToLower().Contains("vector"))
                    {
                        _renderer.material.SetVector("_EmissionColor", Color.white * modelSharedProperties.renderSettings.screenVectorIntenstity);
                    }
                    else if (modelProperties.screen.ToLower().Contains("pinball"))
                    {
                        _renderer.material.SetVector("_EmissionColor", Color.white * modelSharedProperties.renderSettings.screenPinballIntensity);
                    }
                    else
                    {
                        _renderer.material.SetVector("_EmissionColor", Color.white * modelSharedProperties.renderSettings.screenRasterIntensity);
                    }
                }
                if (modelComponentType == ModelComponentType.Marquee)
                {
                    _renderer.material.SetVector("_EmissionColor", Color.white * modelSharedProperties.renderSettings.marqueeIntensity);
                }
            }

            if (videoEnabled == ModelVideoEnabled.Always)
            {
                videoPlayer.enabled = true;
                videoPlayer.Prepare();
                videoPlayer.Play();
            }
            else if (videoEnabled == ModelVideoEnabled.VisiblePlay || videoEnabled == ModelVideoEnabled.VisibleUnobstructed)
            {
                prepareDone = false;
                videoPlayer.enabled = true;
                videoPlayer.Prepare();
                videoPlayer.frame = 10;
                videoPlayer.Play();
                _ = StartCoroutine("PrepareVideo");
            }
            else if (videoEnabled == ModelVideoEnabled.VisibleEnable || videoEnabled == ModelVideoEnabled.Selected)
            {
                videoPlayer.enabled = false;
                SetupImageMaterial();
            }
        }

        IEnumerator PrepareVideo()
        {
            bool ok = true; // Protection for when a videoplayer is already destroyed. Happens sometimes if cylarcade cant keep up on a slow machine.
            while (ok)
            {
                if (videoPlayer == null || !videoPlayer.isActiveAndEnabled)
                {
                    yield return null;
                    ok = false;
                    continue;
                }
                videoPlayer.Play();
                if (videoPlayer.frameCount > 0)
                {
                    ulong frameCount = videoPlayer.frameCount;
                    int frameCountInt = (int)frameCount;
                    long randomFrame = Random.Range(0, frameCountInt);
                    videoPlayer.frame = randomFrame;
                }
                yield return null;
                ok = !videoPlayer.isPlaying || !videoPlayer.isPrepared || !(videoPlayer.frameCount > 0);
            }
            if (videoPlayer != null)
            {
                videoPlayer.Pause();
            }
            prepareDone = true;
        }

        private void SetupImageMaterial()
        {
            if (modelComponentType == ModelComponentType.Screen || modelComponentType == ModelComponentType.Marquee)
            {
                Texture myTexture = _renderer.material.GetTexture("_MainTex");
                _renderer.material.SetTexture("_EmissionMap", myTexture);
                _renderer.material.color = Color.black;
                _renderer.material.SetColor("_EmissionColor", Color.white);
                _renderer.material.EnableKeyword("_EMISSION");
                _renderer.material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
            }
            if (modelComponentType == ModelComponentType.Screen)
            {
                //renderer.material.SetTextureOffset("_MainTex", new Vector2(-0.075f, -0.075f));
                //renderer.material.SetTextureScale("_MainTex", new Vector2(1.15f, 1.15f));
                if (modelProperties.genre.ToLower().Contains("vector"))
                {
                    _renderer.material.SetVector("_EmissionColor", Color.white * modelSharedProperties.renderSettings.screenVectorIntenstity);
                }
                else if (modelProperties.screen.ToLower().Contains("pinball"))
                {
                    _renderer.material.SetVector("_EmissionColor", Color.white * modelSharedProperties.renderSettings.screenPinballIntensity);
                }
                else
                {
                    _renderer.material.SetVector("_EmissionColor", Color.white * modelSharedProperties.renderSettings.screenRasterIntensity);
                }
            }
            if (modelComponentType == ModelComponentType.Marquee)
            {
                _renderer.material.SetVector("_EmissionColor", Color.white * modelSharedProperties.renderSettings.marqueeIntensity);
            }
        }

        public void ReleasePlayer(bool setupImageMaterial = true)
        {
            if (videoPlayer != null)
            {
                videoPlayer.Pause();
                videoPlayer.enabled = false;
            }
            if (setupImageMaterial)
            {
                SetupImageMaterial();
            }
        }

        private void OnBecameVisible()
        {
            visible = true;
            if (videoPlayer == null)
            {
                return;
            }

            if (videoEnabled == ModelVideoEnabled.VisiblePlay)
            {
                videoPlayer.Play();
            }
            if (videoEnabled == ModelVideoEnabled.VisibleUnobstructed)
            {
                //   if (prepareDone) { videoPlayer.Pause(); }
            }
            if (videoEnabled == ModelVideoEnabled.VisibleEnable)
            {
                SetupVideoPlayer();
                videoPlayer.enabled = true;
                videoPlayer.Play();
            }
        }

        private void OnBecameInvisible()
        {
            visible = false;
            if (videoPlayer == null)
            {
                return;
            }

            if (videoEnabled == ModelVideoEnabled.VisiblePlay)
            {
                videoPlayer.Pause();
            }
            if (videoEnabled == ModelVideoEnabled.VisibleUnobstructed)
            {
                //    if (prepareDone) { videoPlayer.Pause(); }
            }
            if (videoEnabled == ModelVideoEnabled.VisibleEnable)
            {
                ReleasePlayer();
            }
        }

        void Update()
        {
            if (ArcadeManager.arcadeState == ArcadeStates.Game)
            {
                setupVideoAfterGameDisable = true;
                if (videoPlayer == null)
                {
                    return;
                }
                if (videoPlayer.enabled == true)
                {
                    videoPlayer.Pause();
                }
                return;
            }
            else if (ArcadeManager.arcadeState == ArcadeStates.Running)
            {
                if (setupVideoAfterGameDisable && dummyNode.GetComponent<ModelLibretroGameSetup>() == null)
                {
                    setupVideoAfterGameDisable = false;
                    if (videoPlayer == null)
                    {
                        return;
                    }
                    // videoPlayer.enabled = false; // TODO: Why did i have that here???
                    if (videoEnabled == ModelVideoEnabled.Never)
                    {
                        return;
                    }

                    videoPlayer.enabled = true;
                    videoPlayer.Play();
                    return;
                }
            }

            //
            if (videoPlayer != null && videoPlayer.isPlaying != videoIsActive)
            {
                videoIsActive = videoPlayer.isPlaying;
                if (videoIsActive ?? true)
                {
                    videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.MaterialOverride;
                }
                else
                {
                    videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.APIOnly;
                }
            }

            // distance = Vector3.Distance(gameObject.transform.position, thisCamera.transform.position);
            distance = (gameObject.transform.position - thisCamera.transform.position).sqrMagnitude;
            //Debug.Log(modelProperties.descriptiveName + " distance " + distance);

            // if I am the savedArcadeModel when cylmenu or fpsmenu are active then dont put the video on me
            if (arcadeLayer && ArcadeManager.activeMenuType != ArcadeType.None && ArcadeStateManager.savedArcadeModel == gameObject.transform.parent.gameObject)
            {
                return;
            }

            if (visible == true && !setupVideoAfterGameDisable && (videoEnabled == ModelVideoEnabled.Always || (videoEnabled == ModelVideoEnabled.VisibleUnobstructed && prepareDone)))
            {
                if (IsInView(/*gameObject*/))
                {
                    if (!videoPlayer.isPlaying)
                    {
                        videoPlayer.enabled = true;
                        videoPlayer.Play();
                    }
                }
                else
                {
                    if (videoPlayer.isPlaying)
                    {
                        videoPlayer.Pause();
                    }
                }
                //frames = 0;
            }

            if (!setupVideoAfterGameDisable && videoEnabled == ModelVideoEnabled.Selected)
            {
                if (ArcadeStateManager.selectedModel != null)
                {
                    if (ArcadeStateManager.selectedModel.transform == gameObject.transform.parent.transform)
                    {
                        if (!selectedDone)
                        {
                            selectedDone = true;
                            SetupVideoPlayer();
                            videoPlayer.enabled = true;
                            videoPlayer.Play();
                        }
                    }
                    else
                    {
                        selectedDone = false;
                        if (videoPlayer.isPlaying)
                        {
                            ReleasePlayer();
                        }
                    }
                }
                else
                {
                    if (videoPlayer.isPlaying)
                    {
                        ReleasePlayer();
                    }
                }
            }

            if (visible == true && !setupVideoAfterGameDisable && (videoEnabled == ModelVideoEnabled.Never || (videoEnabled == ModelVideoEnabled.Selected && selectedDone == false) || videoPlayer.isPaused) && imageList != null)
            {
                timer += Time.deltaTime;
                if (timer > waitTime)
                {
                    index += 1;
                    if (index > (imageList.Count - 1))
                    { index = 0; }
                    _renderer.material.mainTexture = imageList[index];
                    if (_renderer.material.globalIlluminationFlags == MaterialGlobalIlluminationFlags.RealtimeEmissive)
                    {
                        _renderer.material.SetTexture("_EmissionMap", imageList[index]);
                    }
                    timer = 0f;
                }
            }

            if (videoPlayer == null)
            {
                return;
            }

            if (!videoPlayer.isPlaying)
            {
                return;
            }

            if (modelSharedProperties.spatialSound)
            {
                return;
            }

            // Fake sound by distance.
            float max = 10f;
            float min = 2f;
            float volumeByDistance = Mathf.Clamp(Mathf.Pow((distance - max) / (min - max), 2f), 0f, 1f);
            videoPlayer.SetDirectAudioVolume(0, volumeByDistance);
        }

        private bool IsInView(/*GameObject toCheck*/)
        {
            return distance <= maxDistance;

            /*
             * Vector3 pointOnScreen = thisCamera.WorldToScreenPoint(toCheck.GetComponentInChildren<Renderer>().bounds.center);
            //if (arcadeLayer) { Debug.Log("check: " + modelProperties.id); }

            //Is in front
            if (pointOnScreen.z < 0)
            {
                //if (arcadeLayer) { Debug.Log("checknotinfront: " + modelProperties.id); }
                //return false;
            }

            //Is in FOV
            if (arcadeLayer)
            {
                if ((pointOnScreen.x < 0) || (pointOnScreen.x > Screen.width) ||
                    (pointOnScreen.y < 0) || (pointOnScreen.y > Screen.height))
                {
                    if (arcadeLayer)
                    { Debug.Log("checkOutOfBounds: " + modelProperties.id); }
                    //return false;
                }
            }

            // Vector3 heading = toCheck.transform.position - thisCamera.transform.position;
            // Vector3 direction = heading.normalized;// / heading.magnitude;
            if (Physics.Linecast(thisCamera.transform.position, toCheck.GetComponentInChildren<Renderer>().bounds.center, out RaycastHit hit, layerMask))
            {
                ModelSetup hitModelSetup = hit.transform.parent.gameObject.GetComponent<ModelSetup>();
                if (hitModelSetup == null)
                { return false; }
                // Debug.LogError(modelProperties.id + " occluded by " + hitModelSetup.id);
                if (modelProperties.id == hitModelSetup.id)
                { return true; }
                //  Debug.DrawLine(Camera.main.transform.position, toCheck.GetComponentInChildren<Renderer>().bounds.center, Color.red);
                //  Debug.LogError(modelSetup.id + " occluded by " + hitModelSetup.id);
                return false;
            }
            return true;
            */
        }
    }
}
