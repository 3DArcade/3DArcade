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
using SK.Utilities.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace Arcade
{
    public abstract class ArcadeController
    {
        public bool ArcadeLoaded { get; protected set; }
        public ModelConfigurationComponent CurrentGame { get; protected set; }

        public abstract float AudioMinDistance { get; protected set; }
        public abstract float AudioMaxDistance { get; protected set; }
        public abstract AnimationCurve VolumeCurve { get; protected set; }

        protected abstract bool UseModelTransforms { get; }
        protected abstract PlayerControls PlayerControls { get; }
        protected abstract CameraSettings CameraSettings { get; }

        protected const string GAME_RESOURCES_DIRECTORY              = "Games";
        protected const string PROP_RESOURCES_DIRECTORY              = "Props";
        protected const string CYLARCADE_PIVOT_POINT_GAMEOBJECT_NAME = "InternalCylArcadeWheelPivotPoint";

        protected readonly ArcadeHierarchy _arcadeHierarchy;
        protected readonly PlayerFpsControls _playerFpsControls;
        protected readonly PlayerCylControls _playerCylControls;

        protected readonly List<Transform> _allGames;

        protected ArcadeConfiguration _arcadeConfiguration;

        protected bool _animating;

        private static Scene _loadedScene;
        private static bool _sceneLoaded;

        private readonly Database<EmulatorConfiguration> _emulatorDatabase;
        private readonly AssetCache<GameObject> _gameObjectCache;

        private readonly NodeController<MarqueeNodeTag> _marqueeNodeController;
        private readonly NodeController<ScreenNodeTag> _screenNodeController;
        private readonly NodeController<GenericNodeTag> _genericNodeController;

        private readonly CoroutineHelper _coroutineHelper;

        private bool _gameModelsLoaded;

        public ArcadeController(ArcadeHierarchy arcadeHierarchy,
                                PlayerFpsControls playerFpsControls,
                                PlayerCylControls playerCylControls,
                                Database<EmulatorConfiguration> emulatorDatabase,
                                AssetCache<GameObject> gameObjectCache,
                                NodeController<MarqueeNodeTag> marqueeNodeController,
                                NodeController<ScreenNodeTag> screenNodeController,
                                NodeController<GenericNodeTag> genericNodeController)
        {
            _arcadeHierarchy   = arcadeHierarchy ?? throw new System.ArgumentNullException(nameof(arcadeHierarchy));
            _playerFpsControls = playerFpsControls != null ? playerFpsControls : throw new System.ArgumentNullException(nameof(playerFpsControls));
            _playerCylControls = playerCylControls != null ? playerCylControls : throw new System.ArgumentNullException(nameof(playerCylControls));
            _allGames          = new List<Transform>();
            _emulatorDatabase  = emulatorDatabase ?? throw new System.ArgumentNullException(nameof(emulatorDatabase));
            _gameObjectCache   = gameObjectCache ?? throw new System.ArgumentNullException(nameof(gameObjectCache));

            _marqueeNodeController = marqueeNodeController;
            _screenNodeController  = screenNodeController;
            _genericNodeController = genericNodeController;

            _coroutineHelper = Object.FindObjectOfType<CoroutineHelper>();
            Assert.IsNotNull(_coroutineHelper);

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
                _ = _coroutineHelper.StartCoroutine(CoUnloadArcadeScene());
            else
                _ = _coroutineHelper.StartCoroutine(CoSetupWorld());
        }

        public void NavigateForward(float dt)
        {
            if (!_animating)
                _ = _playerCylControls.StartCoroutine(CoNavigateForward(dt));
        }

        public void NavigateBackward(float dt)
        {
            if (!_animating)
                _ = _playerCylControls.StartCoroutine(CoNavigateBackward(dt));
        }

        protected abstract void PreSetupPlayer();

        protected virtual void PostLoadScene()
        {
        }

        protected virtual void AddModelsToWorldAdditionalLoopStepsForGames(GameObject instantiatedModel)
        {
        }

        protected virtual void AddModelsToWorldAdditionalLoopStepsForProps(GameObject instantiatedModel)
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

        protected IEnumerator AddModelsToWorld(bool gameModels, ModelConfiguration[] modelConfigurations, Transform parent, RenderSettings renderSettings, string resourceDirectory, ContentMatcher.GetNamesToTryDelegate getNamesToTry)
        {
            if (modelConfigurations == null)
                yield break;

            if (gameModels)
            {
                _gameModelsLoaded = false;
                _allGames.Clear();
            }

            foreach (ModelConfiguration modelConfiguration in modelConfigurations)
            {
                EmulatorConfiguration emulator = ContentMatcher.GetEmulatorForConfiguration(_emulatorDatabase, modelConfiguration);
                List<string> namesToTry        = getNamesToTry(modelConfiguration, emulator);

                GameObject prefab = _gameObjectCache.Load(resourceDirectory, namesToTry);
                if (prefab == null)
                    continue;

                GameObject instantiatedModel = InstantiatePrefab(prefab, parent, modelConfiguration, !gameModels || UseModelTransforms);

                // Look for artworks only in play mode / runtime
                if (Application.isPlaying)
                {
                    _marqueeNodeController?.Setup(this, instantiatedModel, modelConfiguration, emulator, renderSettings.MarqueeIntensity);
                    _screenNodeController?.Setup(this, instantiatedModel, modelConfiguration, emulator, GetScreenIntensity(modelConfiguration, renderSettings));
                    _genericNodeController?.Setup(this, instantiatedModel, modelConfiguration, emulator, 1f);
                }

                if (gameModels)
                {
                    _allGames.Add(instantiatedModel.transform);
                    AddModelsToWorldAdditionalLoopStepsForGames(instantiatedModel);
                }
                else
                    AddModelsToWorldAdditionalLoopStepsForProps(instantiatedModel);

                // Instantiate asynchronously only when loaded from the editor menu / auto reload
                if (Application.isPlaying)
                    yield return null;
            }

            if (gameModels)
                _gameModelsLoaded = true;
        }

        protected static GameObject InstantiatePrefab(GameObject prefab, Transform parent, ModelConfiguration modelConfiguration, bool useModelTransforms)
        {
            Vector3 position    = useModelTransforms ? modelConfiguration.Position : Vector3.zero;
            Quaternion rotation = useModelTransforms ? Quaternion.Euler(modelConfiguration.Rotation) : Quaternion.identity;

            GameObject model           = Object.Instantiate(prefab, position, rotation, parent);
            model.name                 = modelConfiguration.Id;
            model.transform.localScale = modelConfiguration.Scale;
            model.transform.SetLayersRecursively(parent.gameObject.layer);
            model.AddComponent<ModelConfigurationComponent>()
                 .FromModelConfiguration(modelConfiguration);
            return model;
        }

        protected static float GetScreenIntensity(ModelConfiguration modelConfiguration, RenderSettings renderSettings) => modelConfiguration.ScreenType switch
        {
            GameScreenType.Raster => renderSettings.ScreenRasterIntensity,
            GameScreenType.Vector => renderSettings.ScreenVectorIntenstity,
            GameScreenType.Pinball => renderSettings.ScreenPinballIntensity,
            _ => 1.4f,
        };

        private IEnumerator CoUnloadArcadeScene()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                UnityEditor.SceneManagement.EditorSceneManager.CloseScene(_loadedScene, true);
            else
            {
                AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(_loadedScene);
                while (!asyncOperation.isDone)
                    yield return null;
            }
#else
            AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(_loadedScene);
            while (!asyncOperation.isDone)
                yield return null;
#endif
           _ = _coroutineHelper.StartCoroutine(CoSetupWorld());
        }

        private IEnumerator CoSetupWorld()
        {
            _sceneLoaded = false;

            string sceneName;
            if (!string.IsNullOrEmpty(_arcadeConfiguration.ArcadeScene))
                sceneName = _arcadeConfiguration.ArcadeScene;
            else
                sceneName = _arcadeConfiguration.Id;

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                try
                {
                    UnityEditor.SceneManagement.EditorSceneManager.OpenScene($"Assets/3DArcade/Scenes/{sceneName}/{sceneName}.unity", UnityEditor.SceneManagement.OpenSceneMode.Additive);
                }
                catch (System.Exception)
                {
                    UnityEditor.SceneManagement.EditorSceneManager.OpenScene($"Assets/3DArcade/Scenes/empty/empty.unity", UnityEditor.SceneManagement.OpenSceneMode.Additive);
                }
            }
            else
            {
                AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                while (!asyncOperation.isDone)
                    yield return null;
            }
#else
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            while (!asyncOperation.isDone)
                yield return null;
#endif
            _loadedScene = SceneManager.GetSceneByName(_arcadeConfiguration.ArcadeScene);

            PostLoadScene();

            RenderSettings renderSettings = _arcadeConfiguration.RenderSettings;

            _ = _coroutineHelper.StartCoroutine(AddModelsToWorld(false, _arcadeConfiguration.PropModelList, _arcadeHierarchy.PropsNode, renderSettings, PROP_RESOURCES_DIRECTORY, ContentMatcher.GetNamesToTryForProp));
            _ = _coroutineHelper.StartCoroutine(AddModelsToWorld(true, _arcadeConfiguration.GameModelList, _arcadeHierarchy.GamesNode, renderSettings, GAME_RESOURCES_DIRECTORY, ContentMatcher.GetNamesToTryForGame));
            while (!_gameModelsLoaded)
                yield return null;

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
