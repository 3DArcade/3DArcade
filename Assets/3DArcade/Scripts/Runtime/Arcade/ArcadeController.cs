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
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace Arcade
{
    public abstract class ArcadeController
    {
        public bool ArcadeLoaded { get; protected set; }
        public ModelConfigurationComponent CurrentGame { get; protected set; }

        protected abstract bool UseModelTransfoms { get; }
        protected abstract PlayerControls PlayerControls { get; }
        protected abstract CameraSettings CameraSettings { get; }

        protected const string GAME_RESOURCES_DIRECTORY              = "Games";
        protected const string PROP_RESOURCES_DIRECTORY              = "Props";
        protected const string CYLARCADE_PIVOT_POINT_GAMEOBJECT_NAME = "InternalCylArcadeWheelPivotPoint";

        protected readonly ArcadeHierarchy _arcadeHierarchy;
        protected readonly PlayerFpsControls _playerFpsControls;
        protected readonly PlayerCylControls _playerCylControls;

        protected readonly AssetCache<string> _videoCache;

        protected readonly List<Transform> _allGames;

        protected ArcadeConfiguration _arcadeConfiguration;

        protected float _audioMinDistance;
        protected float _audioMaxDistance;
        protected AnimationCurve _volumeCurve;

        protected bool _animating;

        private static Scene _loadedScene;
        private static bool _sceneLoaded;

        private readonly AssetCache<GameObject> _gameObjectCache;
        private readonly ContentMatcher _contentMatcher;

        private readonly NodeController _marqueeNodeController;
        private readonly NodeController _screenNodeController;
        private readonly NodeController _genericNodeController;

        private readonly CoroutineHelper _coroutineHelper;

        private bool _gameModelsLoaded;

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

        public void StartArcade(ArcadeConfiguration arcadeConfiguration)
        {
            Assert.IsNotNull(arcadeConfiguration);

            _arcadeConfiguration = arcadeConfiguration;
            ArcadeLoaded         = false;

            if (_sceneLoaded)
            {
                _ = _coroutineHelper.StartCoroutine(CoUnloadArcadeScene());
            }
            else
            {
                _ = _coroutineHelper.StartCoroutine(CoSetupWorld());
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

        public abstract bool SetupVideo(Renderer screen, List<string> directories, List<string> namesToTry, float emissionIntensity);
        protected abstract void PreSetupPlayer();

        protected virtual void GameModelAdditionalSteps(GameObject instantiatedModel)
        {
        }

        protected virtual void LateSetupWorld()
        {
        }

        protected virtual IEnumerator CoNavigateForward(float dt)
        {
            yield break;
        }

        protected virtual IEnumerator CoNavigateBackward(float dt)
        {
            yield break;
        }

        protected IEnumerator AddPropModelsToWorld(ModelConfiguration[] modelConfigurations, Transform parent, RenderSettings renderSettings, string resourceDirectory, ContentMatcher.GetNamesToTryDelegate getNamesToTry)
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

                GameObject instantiatedModel = InstantiatePrefab(prefab, parent, modelConfiguration, true);

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

        protected IEnumerator AddGameModelsToWorld(ModelConfiguration[] modelConfigurations, RenderSettings renderSettings, string resourceDirectory, ContentMatcher.GetNamesToTryDelegate getNamesToTry)
        {
            _gameModelsLoaded = false;


            foreach (ModelConfiguration modelConfiguration in modelConfigurations)
            {
                EmulatorConfiguration emulator = _contentMatcher.GetEmulatorForConfiguration(modelConfiguration);
                List<string> namesToTry        = getNamesToTry(modelConfiguration, emulator);

                GameObject prefab = _gameObjectCache.Load(resourceDirectory, namesToTry);
                if (prefab == null)
                {
                    continue;
                }

                GameObject instantiatedModel = InstantiatePrefab(prefab, _arcadeHierarchy.GamesNode, modelConfiguration, UseModelTransfoms);

                _allGames.Add(instantiatedModel.transform);

                GameModelAdditionalSteps(instantiatedModel);

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

            _gameModelsLoaded = true;
        }

        protected GameObject InstantiatePrefab(GameObject prefab, Transform parent, ModelConfiguration modelConfiguration, bool useModelTransform)
        {
            Vector3 position    = useModelTransform ? modelConfiguration.Position : Vector3.zero;
            Quaternion rotation = useModelTransform ? Quaternion.Euler(modelConfiguration.Rotation) : Quaternion.identity;

            GameObject model           = Object.Instantiate(prefab, position, rotation, parent);
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

        private IEnumerator CoUnloadArcadeScene()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                UnityEditor.SceneManagement.EditorSceneManager.CloseScene(_loadedScene, true);
            }
            else
            {
                AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(_loadedScene);
                while (!asyncOperation.isDone)
                {
                    yield return null;
                }
            }
#else
            AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(_loadedScene);
            while (!asyncOperation.isDone)
            {
                yield return null;
            }
#endif
           _ = _coroutineHelper.StartCoroutine(CoSetupWorld());
        }

        private IEnumerator CoSetupWorld()
        {
            _loadedScene = default;
            _sceneLoaded = false;

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                UnityEditor.SceneManagement.EditorSceneManager.OpenScene($"Assets/3DArcade/Scenes/{_arcadeConfiguration.ArcadeScene}/{_arcadeConfiguration.ArcadeScene}.unity", UnityEditor.SceneManagement.OpenSceneMode.Additive);
            }
            else
            {
                AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(_arcadeConfiguration.ArcadeScene, LoadSceneMode.Additive);
                while (!asyncOperation.isDone)
                {
                    yield return null;
                }
            }
#else
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(_arcadeConfiguration.ArcadeScene, LoadSceneMode.Additive);
            while (!asyncOperation.isDone)
            {
                yield return null;
            }
#endif
            _loadedScene = SceneManager.GetSceneByName(_arcadeConfiguration.ArcadeScene);
            _ = SceneManager.SetActiveScene(_loadedScene);

            RenderSettings renderSettings = _arcadeConfiguration.RenderSettings;

            _ = _coroutineHelper.StartCoroutine(AddPropModelsToWorld(_arcadeConfiguration.PropModelList, _arcadeHierarchy.PropsNode, renderSettings, PROP_RESOURCES_DIRECTORY, ContentMatcher.GetNamesToTryForProp));

            _allGames.Clear();
            _ = _coroutineHelper.StartCoroutine(AddGameModelsToWorld(_arcadeConfiguration.GameModelList, renderSettings, GAME_RESOURCES_DIRECTORY, ContentMatcher.GetNamesToTryForGame));
            while (!_gameModelsLoaded)
            {
                yield return null;
            }

            LateSetupWorld();

            SetupPlayer();

            _sceneLoaded = true;
            ArcadeLoaded = true;
        }

        private void SetupPlayer()
        {
            PreSetupPlayer();

            PlayerControls.Camera.orthographic = CameraSettings.Orthographic;
            PlayerControls.Camera.rect         = CameraSettings.ViewportRect;

            PlayerControls.Camera.transform.position    = CameraSettings.Position;
            PlayerControls.Camera.transform.eulerAngles = new Vector3(0f, CameraSettings.Rotation.y, 0f);

            PlayerControls.transform.SetPositionAndRotation(CameraSettings.Position, Quaternion.Euler(0f, CameraSettings.Rotation.y, 0f));

            CinemachineVirtualCamera vCam = PlayerControls.VirtualCamera;
            vCam.transform.eulerAngles    = PlayerControls.Camera.transform.eulerAngles;
            vCam.m_Lens.FieldOfView       = CameraSettings.FieldOfView;
            vCam.m_Lens.OrthographicSize  = CameraSettings.AspectRatio;
            vCam.m_Lens.NearClipPlane     = CameraSettings.NearClipPlane;
            vCam.m_Lens.FarClipPlane      = CameraSettings.FarClipPlane;

            CinemachineTransposer transposer = vCam.GetCinemachineComponent<CinemachineTransposer>();
            transposer.m_FollowOffset.y      = CameraSettings.Height;
        }
    }
}
