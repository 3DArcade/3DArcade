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

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Video;

namespace Arcade
{
    public abstract class ArcadeController
    {
        public bool ArcadeLoaded { get; protected set; }
        public ModelConfigurationComponent CurrentGame { get; protected set; }

        protected const string ARCADE_RESOURCES_DIRECTORY            = "Arcades";
        protected const string GAME_RESOURCES_DIRECTORY              = "Games";
        protected const string PROP_RESOURCES_DIRECTORY              = "Props";
        protected const string CYLARCADE_PIVOT_POINT_GAMEOBJECT_NAME = "InternalCylArcadeWheelPivotPoint";

        protected readonly ArcadeHierarchy _arcadeHierarchy;
        protected readonly PlayerFpsControls _playerFpsControls;
        protected readonly PlayerCylControls _playerCylControls;

        protected readonly AssetCache<GameObject> _gameObjectCache;
        protected readonly AssetCache<string> _videoCache;

        protected readonly ContentMatcher _contentMatcher;

        protected readonly List<Transform> _allGames;

        protected readonly CoroutineHelper _coroutineHelper;

        protected float _audioMinDistance;
        protected float _audioMaxDistance;
        protected AnimationCurve _volumeCurve;

        protected bool _animating;
        protected bool _gameModelsLoaded;

        protected readonly NodeController _marqueeNodeController;
        protected readonly NodeController _screenNodeController;
        protected readonly NodeController _genericNodeController;

        public ArcadeController(ArcadeHierarchy arcadeHierarchy,
                                PlayerFpsControls playerFpsControls,
                                PlayerCylControls playerCylControls,
                                Database<EmulatorConfiguration> emulatorDatabase,
                                AssetCache<GameObject> gameObjectCache,
                                AssetCache<Texture> textureCache,
                                AssetCache<string> videoCache)
        {
            Assert.IsNotNull(arcadeHierarchy);
            Assert.IsNotNull(playerFpsControls);
            Assert.IsNotNull(playerCylControls);
            Assert.IsNotNull(emulatorDatabase);
            Assert.IsNotNull(gameObjectCache);

            _arcadeHierarchy   = arcadeHierarchy;
            _playerFpsControls = playerFpsControls;
            _playerCylControls = playerCylControls;
            _gameObjectCache   = gameObjectCache;
            _videoCache        = videoCache;
            _contentMatcher    = new ContentMatcher(emulatorDatabase);
            _allGames          = new List<Transform>();
            _coroutineHelper   = Object.FindObjectOfType<CoroutineHelper>();
            Assert.IsNotNull(_coroutineHelper);

            _marqueeNodeController = new MarqueeNodeController(this, textureCache);
            _screenNodeController  = new ScreenNodeController(this, textureCache);
            _genericNodeController = new GenericNodeController(this, textureCache);
        }

        public abstract void StartArcade(ArcadeConfiguration arcadeConfiguration);

        public abstract bool SetupVideo(Renderer screen, List<string> directories, List<string> namesToTry);

        protected abstract IEnumerator SetupWorld(ArcadeConfiguration arcadeConfiguration);

        protected abstract IEnumerator AddGameModelsToWorld(ModelConfiguration[] modelConfigurations, RenderSettings renderSettings, string resourceDirectory, ContentMatcher.GetNamesToTryDelegate getNamesToTry);

        protected virtual IEnumerator CoNavigateForward(float dt)
        {
            yield break;
        }

        protected virtual IEnumerator CoNavigateBackward(float dt)
        {
            yield break;
        }

        protected virtual void LateSetupWorld()
        {
            GameObject foundPivotPoint = GameObject.Find(CYLARCADE_PIVOT_POINT_GAMEOBJECT_NAME);
            if (foundPivotPoint != null)
            {
#if UNITY_EDITOR
                Object.DestroyImmediate(foundPivotPoint);
#else
                Object.Destroy(foundPivotPoint);
#endif
            }
        }

        public void NavigateForward(float dt)
        {
            if (!_animating)
            {
                _ = _playerCylControls.StartCoroutine(CoNavigateForward(dt));
            }
        }

        public void NavigateBackward(float dt)
        {
            if (!_animating)
            {
                _ = _playerCylControls.StartCoroutine(CoNavigateBackward(dt));
            }
        }

        protected static void SetupPlayer(PlayerControls playerControls, CameraSettings cameraSettings)
        {
            playerControls.Camera.orthographic = cameraSettings.Orthographic;
            playerControls.Camera.rect         = cameraSettings.ViewportRect;

            playerControls.Camera.transform.position    = cameraSettings.Position;
            playerControls.Camera.transform.eulerAngles = new Vector3(0f, cameraSettings.Rotation.y, 0f);

            playerControls.transform.SetPositionAndRotation(cameraSettings.Position, Quaternion.Euler(0f, cameraSettings.Rotation.y, 0f));

            CinemachineVirtualCamera vCam = playerControls.VirtualCamera;
            vCam.transform.eulerAngles    = playerControls.Camera.transform.eulerAngles;
            vCam.m_Lens.FieldOfView       = cameraSettings.FieldOfView;
            vCam.m_Lens.OrthographicSize  = cameraSettings.AspectRatio;
            vCam.m_Lens.NearClipPlane     = cameraSettings.NearClipPlane;
            vCam.m_Lens.FarClipPlane      = cameraSettings.FarClipPlane;

            CinemachineTransposer transposer = vCam.GetCinemachineComponent<CinemachineTransposer>();
            transposer.m_FollowOffset.y      = cameraSettings.Height;
        }

        protected IEnumerator AddModelsToWorld(ModelConfiguration[] modelConfigurations, Transform parent, RenderSettings renderSettings, string resourceDirectory, ContentMatcher.GetNamesToTryDelegate getNamesToTry)
        {
            if (modelConfigurations == null)
            {
                yield break;
            }

            foreach (ModelConfiguration modelConfiguration in modelConfigurations)
            {
                EmulatorConfiguration emulator = _contentMatcher.GetEmulatorForConfiguration(modelConfiguration);
                List<string> namesToTry        = getNamesToTry(modelConfiguration, emulator);

                GameObject prefab = _gameObjectCache.Load(resourceDirectory, namesToTry);
                if (prefab == null)
                {
                    continue;
                }

                GameObject instantiatedModel = InstantiatePrefab(prefab, parent, modelConfiguration);

                // Look for artworks only in play mode / runtime
                if (Application.isPlaying)
                {
                    _marqueeNodeController.Setup(instantiatedModel, modelConfiguration, emulator, renderSettings.MarqueeIntensity);
                    _screenNodeController.Setup(instantiatedModel, modelConfiguration, emulator, GetScreenIntensity(modelConfiguration, renderSettings));
                    _genericNodeController.Setup(instantiatedModel, modelConfiguration, emulator, 1f);
                }

                // Instantiate asynchronously only when loaded from the editor menu / auto reload
                if (Application.isPlaying)
                {
                    yield return null;
                }
            }
        }

        protected GameObject InstantiatePrefab(GameObject prefab, Transform parent, ModelConfiguration modelConfiguration)
        {
            GameObject model           = Object.Instantiate(prefab, modelConfiguration.Position, Quaternion.Euler(modelConfiguration.Rotation), parent);
            model.name                 = modelConfiguration.Id;
            model.transform.localScale = modelConfiguration.Scale;
            model.transform.SetLayersRecursively(parent.gameObject.layer);
            model.AddComponent<ModelConfigurationComponent>()
                 .FromModelConfiguration(modelConfiguration);
            return model;
        }

        protected static float GetScreenIntensity(ModelConfiguration modelConfiguration, RenderSettings renderSettings)
        {
            switch (modelConfiguration.ScreenType)
            {
                case GameScreenType.Raster:
                    return renderSettings.ScreenRasterIntensity;
                case GameScreenType.Vector:
                    return renderSettings.ScreenVectorIntenstity;
                case GameScreenType.Pinball:
                    return renderSettings.ScreenPinballIntensity;
                case GameScreenType.Unspecified:
                default:
                    return 1f;
            }
        }

        protected static void OnVideoPlayerErrorReceived(VideoPlayer _, string message) => Debug.Log($"Error: {message}");
    }
}
