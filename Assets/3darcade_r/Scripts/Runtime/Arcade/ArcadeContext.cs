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
using UnityEngine.Assertions;

namespace Arcade_r
{
    public sealed class ArcadeContext : FSM.Context<ArcadeState>
    {
        public bool ArcadeLoaded;
        public PlayerControls CurrentPlayerControls;
        public ModelConfigurationComponent CurrentModelConfiguration;

        public readonly PlayerFpsControls PlayerFpsControls;
        public readonly PlayerCylControls PlayerCylControls;

        public readonly OS CurrentOS;
        public readonly IVirtualFileSystem VirtualFileSystem;
        public readonly ArcadeHierarchy ArcadeHierarchy;
        public readonly UIController UIController;

        public readonly GeneralConfiguration GeneralConfiguration;
        public readonly Database<ArcadeConfiguration> ArcadeDatabase;
        public readonly Database<EmulatorConfiguration> EmulatorDatabase;

        public readonly AssetCache<GameObject> GameObjectCache;
        public readonly AssetCache<Texture> TextureCache;
        public readonly AssetCache<string> VideoCache;

        public readonly VideoPlayerController VideoPlayerController;
        public readonly CoroutineHelper CoroutineHelper;

        public ArcadeController ArcadeController { get; private set; }
        public LayerMask RaycastLayers => LayerMask.GetMask("Arcade/ArcadeModels", "Arcade/GameModels", "Arcade/PropModels");
        public ArcadeConfiguration CurrentArcadeConfiguration { get; private set; }
        public ArcadeType CurrentArcadeType { get; private set; }

        public ArcadeContext(PlayerFpsControls playerFpsControls, PlayerCylControls playerCylControls, Transform uiRoot)
        {
            PlayerFpsControls = playerFpsControls;
            PlayerCylControls = playerCylControls;

            try
            {
                CurrentOS = SystemUtils.GetCurrentOS();
                Debug.Log($"Current OS: {CurrentOS}");
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
                SystemUtils.ExitApp();
                return;
            }

            string vfsRootDirectory = SystemUtils.GetDataPath();
            Debug.Log($"Data path: {vfsRootDirectory}");
            VirtualFileSystem = InitVFS(vfsRootDirectory);

            ArcadeHierarchy = new ArcadeHierarchy();

            UIController = new UIController(uiRoot);

            GeneralConfiguration = new GeneralConfiguration(VirtualFileSystem);
            if (!GeneralConfiguration.Load())
            {
                SystemUtils.ExitApp();
                return;
            }

            ArcadeDatabase   = new ArcadeDatabase(VirtualFileSystem);
            EmulatorDatabase = new EmulatorDatabase(VirtualFileSystem);

            GameObjectCache = new GameObjectCache();
            TextureCache    = new TextureCache();
            VideoCache      = new VideoCache();

            VideoPlayerController = new VideoPlayerController(LayerMask.GetMask("Arcade/GameModels", "Arcade/PropModels"));

            CoroutineHelper = Object.FindObjectOfType<CoroutineHelper>();
            Assert.IsNotNull(CoroutineHelper);

            _ = SetCurrentArcadeConfiguration(GeneralConfiguration.StartingArcade, GeneralConfiguration.StartingArcadeType);
        }

        public bool SetCurrentArcadeConfiguration(string id, ArcadeType type)
        {
            CurrentArcadeConfiguration = ArcadeDatabase.Get(id);
            CurrentArcadeType          = type;
            return CurrentArcadeConfiguration != null;
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
            ArcadeLoaded = false;

            if (CurrentArcadeConfiguration == null)
            {
                return false;
            }

            VideoPlayerController.StopAllVideos();

            ArcadeHierarchy.RootNode.gameObject.AddComponentIfNotFound<ArcadeConfigurationComponent>()
                                               .Restore(CurrentArcadeConfiguration);

            ArcadeHierarchy.Reset();

            switch (CurrentArcadeType)
            {
                case ArcadeType.Fps:
                {
                    ArcadeController = new ArcadeFpsController(ArcadeHierarchy, PlayerFpsControls, PlayerCylControls, EmulatorDatabase, GameObjectCache, TextureCache, VideoCache);
                }
                break;
                case ArcadeType.Cyl:
                {
                    switch (CurrentArcadeConfiguration.CylArcadeProperties.WheelVariant)
                    {
                        case WheelVariant.CameraInsideWheel:
                        {
                            ArcadeController = new ArcadeCylCameraInsideController(ArcadeHierarchy, PlayerFpsControls, PlayerCylControls, EmulatorDatabase, GameObjectCache, TextureCache, VideoCache);
                        }
                        break;
                        case WheelVariant.CameraOutsideWheel:
                            break;
                        case WheelVariant.FlatHorizontal:
                        {
                            ArcadeController = new ArcadeCylFlatHorizontalController(ArcadeHierarchy, PlayerFpsControls, PlayerCylControls, EmulatorDatabase, GameObjectCache, TextureCache, VideoCache);
                        }
                        break;
                        case WheelVariant.FlatVertical:
                            break;
                        case WheelVariant.Custom:
                            break;
                    }
                }
                break;
            }

            ArcadeLoaded = ArcadeController.StartArcade(CurrentArcadeConfiguration);
            return ArcadeLoaded;
        }

        public bool SaveCurrentArcadeConfiguration()
        {
            ArcadeConfigurationComponent cfgComponent = ArcadeHierarchy.RootNode.GetComponent<ArcadeConfigurationComponent>();
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

            return cfgComponent.Save(ArcadeDatabase, fpsCameraSettings, cylCameraSettings, !cylCamera.gameObject.activeInHierarchy);
        }

        public bool SaveCurrentArcadeConfigurationModels()
        {
            ArcadeConfigurationComponent cfgComponent = ArcadeHierarchy.RootNode.GetComponent<ArcadeConfigurationComponent>();
            return cfgComponent != null && cfgComponent.SaveModelsOnly(ArcadeDatabase, CurrentArcadeConfiguration);
        }

        public void ReloadCurrentArcadeConfigurationModels()
        {
            if (ArcadeHierarchy.RootNode.TryGetComponent(out ArcadeConfigurationComponent cfgComponent))
            {
                cfgComponent.SetGamesAndPropsTransforms(CurrentArcadeConfiguration);
            }
        }

        public EmulatorConfiguration GetEmulatorForCurrentModelConfiguration() => EmulatorDatabase.Get(CurrentModelConfiguration.Emulator);

        private static VirtualFileSystem InitVFS(string rootDirectory)
        {
            VirtualFileSystem result = new VirtualFileSystem();

            result.MountDirectory("arcade_cfgs", $"{rootDirectory}/3darcade_r~/Configuration/Arcades");
            result.MountDirectory("emulator_cfgs", $"{rootDirectory}/3darcade_r~/Configuration/Emulators");
            result.MountDirectory("gamelist_cfgs", $"{rootDirectory}/3darcade_r~/Configuration/Gamelists");
            result.MountDirectory("medias", $"{rootDirectory}/3darcade_r~/Media");

            result.MountFile("general_cfg", $"{rootDirectory}/3darcade_r~/Configuration/GeneralConfiguration.json");

            return result;
        }
    }
}
