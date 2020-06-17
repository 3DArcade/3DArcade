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
    public class EditorLoadSaveArcadeSubstitute
    {
        public readonly ArcadeHierarchy ArcadeHierarchy;
        public readonly Database<ArcadeConfiguration> ArcadeDatabase;

        private static GameObject _hierarchyPrefab;

        private readonly IVirtualFileSystem _virtualFileSystem;
        private readonly AssetCache<GameObject> _gameObjectCache;
        private readonly Database<EmulatorConfiguration> _emulatorDatabase;
        private readonly PlayerFpsControls _playerFpsControls;
        private readonly PlayerCylControls _playerCylControls;
        private readonly ArcadeController _fpsArcadeController;
        private readonly ArcadeController _cylArcadeController;

        public EditorLoadSaveArcadeSubstitute()
        {
            if (_hierarchyPrefab == null)
            {
                _hierarchyPrefab = Resources.Load<GameObject>("Misc/pfArcadeHierarchy");
            }

            ArcadeHierarchy = new ArcadeHierarchy();

            string dataPath    = SystemUtils.GetDataPath();
            _virtualFileSystem = new VirtualFileSystem();
            _virtualFileSystem.MountDirectory("arcade_cfgs", $"{dataPath}/3darcade_r~/Configuration/Arcades");
            _virtualFileSystem.MountDirectory("emulator_cfgs", $"{dataPath}/3darcade_r~/Configuration/Emulators");
            _virtualFileSystem.MountDirectory("gamelist_cfgs", $"{dataPath}/3darcade_r~/Configuration/Gamelists");
            _virtualFileSystem.MountDirectory("medias", $"{dataPath}/3darcade_r~/Media");

            _gameObjectCache = new GameObjectCache();

            ArcadeDatabase    = new ArcadeDatabase(_virtualFileSystem);
            _emulatorDatabase = new EmulatorDatabase(_virtualFileSystem);

            GameObject playerControls = GameObject.Find("PlayerControls");
            Assert.IsNotNull(playerControls);
            _playerFpsControls = playerControls.GetComponentInChildren<PlayerFpsControls>(true);
            Assert.IsNotNull(_playerFpsControls);
            _playerCylControls = playerControls.GetComponentInChildren<PlayerCylControls>(true);
            Assert.IsNotNull(_playerCylControls);

            if (_playerFpsControls.gameObject.activeInHierarchy)
            {
                _playerFpsControls.gameObject.SetActive(true);
                _playerCylControls.gameObject.SetActive(false);
            }
            else
            {
                _playerFpsControls.gameObject.SetActive(false);
                _playerCylControls.gameObject.SetActive(true);
            }

            _fpsArcadeController = new ArcadeFpsController(ArcadeHierarchy, _playerFpsControls, _playerCylControls, _emulatorDatabase, _gameObjectCache, null, null);
            _cylArcadeController = new ArcadeCylController(ArcadeHierarchy, _playerFpsControls, _playerCylControls, _emulatorDatabase, _gameObjectCache, null, null);
        }

        public void LoadAndStartArcade(string name)
        {
            ArcadeConfiguration arcadeConfiguration = ArcadeDatabase.Get(name);
            if (arcadeConfiguration == null)
            {
                return;
            }

            ArcadeHierarchy.RootNode.gameObject.AddComponentIfNotFound<ArcadeConfigurationComponent>()
                                               .Restore(arcadeConfiguration);

            ArcadeHierarchy.Reset();
            if (_playerFpsControls.gameObject.activeInHierarchy)
            {
                _ = _fpsArcadeController.StartArcade(arcadeConfiguration);
            }
            else
            {
                _ = _cylArcadeController.StartArcade(arcadeConfiguration);
            }
        }

        public void SaveArcade(ArcadeConfigurationComponent arcadeConfiguration)
        {
            Camera fpsCamera                          = _playerFpsControls.Camera;
            CinemachineVirtualCamera fpsVirtualCamera = _playerFpsControls.VirtualCamera;
            CameraSettings fpsCameraSettings          = new CameraSettings
            {
                Position      = _playerFpsControls.transform.position,
                Rotation      = MathUtils.CorrectEulerAngles(fpsCamera.transform.eulerAngles),
                Height        = fpsVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y,
                Orthographic  = fpsCamera.orthographic,
                FieldOfView   = fpsVirtualCamera.m_Lens.FieldOfView,
                AspectRatio   = fpsVirtualCamera.m_Lens.OrthographicSize,
                NearClipPlane = fpsVirtualCamera.m_Lens.NearClipPlane,
                FarClipPlane  = fpsVirtualCamera.m_Lens.FarClipPlane,
                ViewportRect  = fpsCamera.rect
            };

            Camera cylCamera                          = _playerCylControls.Camera;
            CinemachineVirtualCamera cylVirtualCamera = _playerCylControls.VirtualCamera;
            CameraSettings cylCameraSettings          = new CameraSettings
            {
                Position      = _playerFpsControls.transform.position,
                Rotation      = MathUtils.CorrectEulerAngles(cylCamera.transform.eulerAngles),
                Height        = cylVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y,
                Orthographic  = cylCamera.orthographic,
                FieldOfView   = cylVirtualCamera.m_Lens.FieldOfView,
                AspectRatio   = cylVirtualCamera.m_Lens.OrthographicSize,
                NearClipPlane = cylVirtualCamera.m_Lens.NearClipPlane,
                FarClipPlane  = cylVirtualCamera.m_Lens.FarClipPlane,
                ViewportRect  = cylCamera.rect
            };

            _ = arcadeConfiguration.Save(ArcadeDatabase, fpsCameraSettings, cylCameraSettings);
        }
    }
}
