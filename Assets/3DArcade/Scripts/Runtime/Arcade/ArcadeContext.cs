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
using UnityEngine;

namespace Arcade
{
    public sealed class ArcadeContext : FSM.Context<ArcadeState>
    {
        public VideoPlayerController VideoPlayerController { get; private set; }
        public ArcadeController ArcadeController { get; private set; }
        public LayerMask RaycastLayers => LayerMask.GetMask("Arcade/ArcadeModels", "Arcade/GameModels", "Arcade/PropModels", "Selection");
        public ArcadeConfiguration CurrentArcadeConfiguration { get; private set; }
        public ArcadeType CurrentArcadeType { get; private set; }

        public PlayerControls CurrentPlayerControls;
        public ModelConfigurationComponent CurrentModelConfiguration;

        public readonly PlayerFpsControls PlayerFpsControls;
        public readonly PlayerCylControls PlayerCylControls;

        public readonly UIController UIController;

        private readonly OS _currentOS;
        private readonly IVirtualFileSystem _virtualFileSystem;
        private readonly ArcadeHierarchy _arcadeHierarchy;

        private readonly GeneralConfiguration _generalConfiguration;

        private readonly Database<ArcadeConfiguration> _arcadeDatabase;
        private readonly Database<EmulatorConfiguration> _emulatorDatabase;

        private readonly AssetCache<GameObject> _gameObjectCache;

        private readonly NodeController<MarqueeNodeTag> _marqueeNodeController;
        private readonly NodeController<ScreenNodeTag> _screenNodeController;
        private readonly NodeController<GenericNodeTag> _genericNodeController;

        public ArcadeContext(PlayerFpsControls playerFpsControls, PlayerCylControls playerCylControls, Transform uiRoot)
        {
            PlayerFpsControls = playerFpsControls;
            PlayerCylControls = playerCylControls;

            try
            {
                _currentOS = SystemUtils.GetCurrentOS();
                Debug.Log($"Current OS: {_currentOS}");
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
                SystemUtils.ExitApp();
                return;
            }

            string vfsRootDirectory = SystemUtils.GetDataPath();
            Debug.Log($"Data path: {vfsRootDirectory}");
            _virtualFileSystem = InitVFS(vfsRootDirectory);

            _arcadeHierarchy = new ArcadeHierarchy();

            UIController = new UIController(uiRoot);

            _generalConfiguration = new GeneralConfiguration(_virtualFileSystem);
            if (!_generalConfiguration.Load())
            {
                SystemUtils.ExitApp();
                return;
            }

            _arcadeDatabase   = new ArcadeDatabase(_virtualFileSystem);
            _emulatorDatabase = new EmulatorDatabase(_virtualFileSystem);

            _gameObjectCache = new GameObjectCache();

            _marqueeNodeController = new MarqueeNodeController();
            _screenNodeController  = new ScreenNodeController();
            _genericNodeController = new GenericNodeController();

            _ = SetCurrentArcadeConfiguration(_generalConfiguration.StartingArcade, _generalConfiguration.StartingArcadeType);
        }

        public bool SetCurrentArcadeConfiguration(string id, ArcadeType type)
        {
            ArcadeConfiguration arcadeConfiguration = _arcadeDatabase.Get(id);
            if (arcadeConfiguration != null)
            {
                CurrentArcadeConfiguration = arcadeConfiguration;
                CurrentArcadeType          = type;
            }
            return arcadeConfiguration != null;
        }

        public void SetAndStartCurrentArcadeConfiguration(string id, ArcadeType type)
        {
            if (SetCurrentArcadeConfiguration(id, type))
            {
                TransitionTo<ArcadeLoadState>();
            }
        }

        public bool StartCurrentArcade()
        {
            if (CurrentArcadeConfiguration == null)
            {
                return false;
            }


            _arcadeHierarchy.RootNode.gameObject.AddComponentIfNotFound<ArcadeConfigurationComponent>()
                                               .Restore(CurrentArcadeConfiguration);

            _arcadeHierarchy.Reset();

            switch (CurrentArcadeType)
            {
                case ArcadeType.Fps:
                {
                    VideoPlayerController = new VideoPlayerControllerFps(LayerMask.GetMask("Arcade/GameModels", "Arcade/PropModels"));
                    ArcadeController      = new FpsArcadeController(_arcadeHierarchy, PlayerFpsControls, PlayerCylControls, _emulatorDatabase, _gameObjectCache, _marqueeNodeController, _screenNodeController, _genericNodeController);
                }
                break;
                case ArcadeType.Cyl:
                {
                    VideoPlayerController = null;

                    switch (CurrentArcadeConfiguration.CylArcadeProperties.WheelVariant)
                    {
                        case WheelVariant.CameraInsideHorizontal:
                            ArcadeController = new CylArcadeControllerWheel3DCameraInsideHorizontal(_arcadeHierarchy, PlayerFpsControls, PlayerCylControls, _emulatorDatabase, _gameObjectCache, _marqueeNodeController, _screenNodeController, _genericNodeController);
                            break;
                        case WheelVariant.CameraOutsideHorizontal:
                            ArcadeController = new CylArcadeControllerWheel3DCameraOutsideHorizontal(_arcadeHierarchy, PlayerFpsControls, PlayerCylControls, _emulatorDatabase, _gameObjectCache, _marqueeNodeController, _screenNodeController, _genericNodeController);
                            break;
                        case WheelVariant.LineHorizontal:
                            ArcadeController = new CylArcadeControllerLineHorizontal(_arcadeHierarchy, PlayerFpsControls, PlayerCylControls, _emulatorDatabase, _gameObjectCache, _marqueeNodeController, _screenNodeController, _genericNodeController);
                            break;
                        case WheelVariant.CameraInsideVertical:
                            ArcadeController = new CylArcadeControllerWheel3DCameraInsideVertical(_arcadeHierarchy, PlayerFpsControls, PlayerCylControls, _emulatorDatabase, _gameObjectCache, _marqueeNodeController, _screenNodeController, _genericNodeController);
                            break;
                        case WheelVariant.CameraOutsideVertical:
                            ArcadeController = new CylArcadeControllerWheel3DCameraOutsideVertical(_arcadeHierarchy, PlayerFpsControls, PlayerCylControls, _emulatorDatabase, _gameObjectCache, _marqueeNodeController, _screenNodeController, _genericNodeController);
                            break;
                        case WheelVariant.LineVertical:
                            ArcadeController = new CylArcadeControllerLineVertical(_arcadeHierarchy, PlayerFpsControls, PlayerCylControls, _emulatorDatabase, _gameObjectCache, _marqueeNodeController, _screenNodeController, _genericNodeController);
                            break;
                        case WheelVariant.LineCustom:
                            ArcadeController = new CylArcadeControllerLine(_arcadeHierarchy, PlayerFpsControls, PlayerCylControls, _emulatorDatabase, _gameObjectCache, _marqueeNodeController, _screenNodeController, _genericNodeController);
                            break;
                    }
                }
                break;
            }

            ArcadeController.StartArcade(CurrentArcadeConfiguration);
            return true;
        }

        public bool SaveCurrentArcadeConfiguration()
        {
            ArcadeConfigurationComponent cfgComponent = _arcadeHierarchy.RootNode.GetComponent<ArcadeConfigurationComponent>();
            if (cfgComponent == null)
            {
                return false;
            }

            Camera fpsCamera                          = PlayerFpsControls.Camera;
            CinemachineVirtualCamera fpsVirtualCamera = PlayerFpsControls.VirtualCamera;
            CameraSettings fpsCameraSettings          = new CameraSettings
            {
                Position      = PlayerFpsControls.transform.position,
                Rotation      = MathUtils.CorrectEulerAngles(fpsCamera.transform.eulerAngles),
                Height        = fpsVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y,
                Orthographic  = fpsCamera.orthographic,
                FieldOfView   = fpsVirtualCamera.m_Lens.FieldOfView,
                AspectRatio   = fpsVirtualCamera.m_Lens.OrthographicSize,
                NearClipPlane = fpsVirtualCamera.m_Lens.NearClipPlane,
                FarClipPlane  = fpsVirtualCamera.m_Lens.FarClipPlane,
                ViewportRect  = fpsCamera.rect
            };

            Camera cylCamera                          = PlayerCylControls.Camera;
            CinemachineVirtualCamera cylVirtualCamera = PlayerCylControls.VirtualCamera;
            CameraSettings cylCameraSettings          = new CameraSettings
            {
                Position      = PlayerCylControls.transform.position,
                Rotation      = MathUtils.CorrectEulerAngles(cylCamera.transform.eulerAngles),
                Height        = cylVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y,
                Orthographic  = cylCamera.orthographic,
                FieldOfView   = cylVirtualCamera.m_Lens.FieldOfView,
                AspectRatio   = cylVirtualCamera.m_Lens.OrthographicSize,
                NearClipPlane = cylVirtualCamera.m_Lens.NearClipPlane,
                FarClipPlane  = cylVirtualCamera.m_Lens.FarClipPlane,
                ViewportRect  = cylCamera.rect
            };

            return cfgComponent.Save(_arcadeDatabase, fpsCameraSettings, cylCameraSettings, !cylCamera.gameObject.activeInHierarchy);
        }

        public bool SaveCurrentArcadeConfigurationModels()
        {
            ArcadeConfigurationComponent cfgComponent = _arcadeHierarchy.RootNode.GetComponent<ArcadeConfigurationComponent>();
            return cfgComponent != null && cfgComponent.SaveModelsOnly(_arcadeDatabase, CurrentArcadeConfiguration);
        }

        public void ReloadCurrentArcadeConfigurationModels()
        {
            if (_arcadeHierarchy.RootNode.TryGetComponent(out ArcadeConfigurationComponent cfgComponent))
            {
                cfgComponent.SetGamesAndPropsTransforms(CurrentArcadeConfiguration);
            }
        }

        public EmulatorConfiguration GetEmulatorForCurrentModelConfiguration() => _emulatorDatabase.Get(CurrentModelConfiguration.Emulator);

        private static VirtualFileSystem InitVFS(string rootDirectory)
        {
            VirtualFileSystem result = new VirtualFileSystem();

            result.MountDirectory("arcade_cfgs", $"{rootDirectory}/3darcade~/Configuration/Arcades");
            result.MountDirectory("emulator_cfgs", $"{rootDirectory}/3darcade~/Configuration/Emulators");
            result.MountDirectory("gamelist_cfgs", $"{rootDirectory}/3darcade~/Configuration/Gamelists");
            result.MountDirectory("medias", $"{rootDirectory}/3darcade~/Media");

            result.MountFile("general_cfg", $"{rootDirectory}/3darcade~/Configuration/GeneralConfiguration.json");

            return result;
        }
    }
}
