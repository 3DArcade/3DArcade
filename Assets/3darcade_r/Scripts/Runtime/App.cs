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
using UnityEngine.Assertions;

namespace Arcade_r
{
    [DisallowMultipleComponent]
    public sealed class App : MonoBehaviour
    {
        [SerializeField] private PlayerControls _playerControls   = default;
        [SerializeField] private Camera _camera                   = default;
        [SerializeField] private Transform _uiRoot                = default;
        [SerializeField] private GameObject _theAbyss             = default;

        public PlayerControls PlayerControls => _playerControls;
        public Camera Camera => _camera;
        public UIController UIController { get; private set; }

        public OS CurrentOS { get; private set; }
        public IVirtualFileSystem VirtualFileSystem { get; private set; }
        public ArcadeHierarchy ArcadeHierarchy { get; private set; }
        public GeneralConfiguration GeneralConfiguration { get; private set; }
        public ArcadeConfigurationManager ArcadeManager { get; private set; }
        public EmulatorConfigurationManager EmulatorManager { get; private set; }
        public GameObjectCache GameObjectCache { get; private set; }
        public DiskTextureCache DiskTextureCache { get; private set; }

        private ArcadeContext _arcadeContext;
        private bool _badLuck = false;

        private void Awake()
        {
            Assert.IsNotNull(_playerControls);
            Assert.IsNotNull(_camera);
            Assert.IsNotNull(_uiRoot);
            Assert.IsNotNull(_theAbyss);

            UIController = new UIController(_uiRoot);

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

            ArcadeHierarchy      = new ArcadeHierarchy();
            GeneralConfiguration = new GeneralConfiguration(VirtualFileSystem);
            ArcadeManager        = new ArcadeConfigurationManager(VirtualFileSystem);
            EmulatorManager      = new EmulatorConfigurationManager(VirtualFileSystem);

            if (!GeneralConfiguration.Load())
            {
                SystemUtils.ExitApp();
                return;
            }

            GameObjectCache  = new GameObjectCache();
            DiskTextureCache = new DiskTextureCache();

            _arcadeContext = new ArcadeContext(this, GeneralConfiguration.StartingArcade);
        }

        private void Start()
        {
            SystemUtils.HideMouseCursor();

            _arcadeContext.TransitionTo<ArcadeLoadState>();
        }

        private void Update()
        {
            _arcadeContext.Update(Time.deltaTime);

            YouAreNotSupposedToBeHere();
        }

        private void FixedUpdate()
        {
            _arcadeContext.FixedUpdate(Time.fixedDeltaTime);
        }

        private static VirtualFileSystem InitVFS(string rootDirectory)
        {
            VirtualFileSystem result = new VirtualFileSystem();
#if UNITY_EDITOR
            result.MountDirectory("models", "3darcade/models");
            result.MountDirectory("arcade_models", "3darcade/models/arcades");
            result.MountDirectory("game_models", "3darcade/models/games");
            result.MountDirectory("prop_models", "3darcade/models/props");
#endif
            result.MountDirectory("cfgs", $"{rootDirectory}/3darcade_r~/Configuration");
            result.MountDirectory("arcade_cfgs", $"{rootDirectory}/3darcade_r~/Configuration/Arcades");
            result.MountDirectory("emulator_cfgs", $"{rootDirectory}/3darcade_r~/Configuration/Emulators");
            result.MountDirectory("emulators", $"{rootDirectory}/3darcade_r~/Emulators");
            result.MountDirectory("media", $"{rootDirectory}/3darcade_r~/Media");

            result.MountFile("general_cfg", $"{rootDirectory}/3darcade_r~/Configuration/GeneralConfiguration.json");

            return result;
        }

        private void YouAreNotSupposedToBeHere()
        {
            if (!_badLuck && PlayerControls.transform.position.y < -340f)
            {
                PlayerControls.transform.position = new Vector3(0f, PlayerControls.transform.position.y, 0f);
                _ = Instantiate(_theAbyss);
                _badLuck = true;
            }
        }
    }
}
