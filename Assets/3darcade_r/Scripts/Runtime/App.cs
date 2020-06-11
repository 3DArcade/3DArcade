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
        [SerializeField] private PlayerControls _playerControls = default;
        [SerializeField] private Camera _camera                 = default;
        [SerializeField] private Transform _uiRoot              = default;
        [SerializeField] private GameObject _theAbyss           = default;

        public PlayerControls PlayerControls => _playerControls;
        public Camera Camera => _camera;

        public OS CurrentOS { get; private set; }
        public IVirtualFileSystem VirtualFileSystem { get; private set; }
        public ArcadeHierarchy ArcadeHierarchy { get; private set; }
        public UIController UIController { get; private set; }
        public GeneralConfiguration GeneralConfiguration { get; private set; }
        public Database<ArcadeConfiguration> ArcadeDatabase { get; private set; }
        public Database<EmulatorConfiguration> EmulatorDatabase { get; private set; }
        public AssetCache<GameObject> GameObjectCache { get; private set; }
        public AssetCache<Texture> TextureCache { get; private set; }

        private ArcadeContext _arcadeContext;
        private bool _badLuck = false;

#if !UNITY_EDITOR
        private bool _focused;
        private void OnApplicationFocus(bool focus) => _focused = focus;
#endif
        private void Awake()
        {
            Assert.IsNotNull(_playerControls);
            Assert.IsNotNull(_camera);
            Assert.IsNotNull(_uiRoot);
            Assert.IsNotNull(_theAbyss);

            Application.targetFrameRate = -1;
            Time.timeScale              = 1f;

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

            UIController = new UIController(_uiRoot);

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

            _arcadeContext = new ArcadeContext(this, GeneralConfiguration.StartingMenu);
        }

        private void Start()
        {
            SystemUtils.HideMouseCursor();

            _arcadeContext.TransitionTo<ArcadeLoadState>();
        }

        private void Update()
        {
 #if !UNITY_EDITOR
            if (!_focused)
            {
                System.Threading.Thread.Sleep(200);
                return;
            }
#endif
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

            result.MountDirectory("arcade_cfgs", $"{rootDirectory}/3darcade_r~/Configuration/Arcades");
            result.MountDirectory("emulator_cfgs", $"{rootDirectory}/3darcade_r~/Configuration/Emulators");
            result.MountDirectory("gamelist_cfgs", $"{rootDirectory}/3darcade_r~/Configuration/Gamelists");

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
