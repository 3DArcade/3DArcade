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

using System.Linq;
using UnityEngine;

namespace Arcade_r
{
    public class Main : MonoBehaviour
    {
        private VFS _vfs;
        public GameObject[] LoadedModels;

        private void Awake()
        {
            InitVFS();
        }

        private void Start()
        {
            Utils.HideMouseCursor();

            LoadModels();
            MaterialUtils.SetGPUInstancing(true, LoadedModels.Where(x => x != null));

            Vector3 position    = new Vector3(49.4f, 0f, 20f);
            Quaternion rotation = Quaternion.LookRotation(Vector3.left, Vector3.up);

            foreach (GameObject loadedModel in LoadedModels)
            {
                GameObject model = Instantiate(loadedModel, position, rotation);
                _ = model.AddComponent<GameModelSetup>();
                model.layer = LayerMask.NameToLayer("Arcade/GameModels");
                position.z--;
            }
        }

        private void InitVFS()
        {
            _vfs = VFS.Instance;

            if (Application.isEditor)
            {
                _vfs.MountDirectory("models", "3darcade/models");
                _vfs.MountDirectory("arcade_models", "3darcade/models/arcades");
                _vfs.MountDirectory("game_models", "3darcade/models/games");
                _vfs.MountDirectory("prop_models", "3darcade/models/props");
            }

            string streamingAssetsPath = Application.streamingAssetsPath;
            _vfs.MountDirectory("cfgs", $"{streamingAssetsPath}/Configuration");
            _vfs.MountDirectory("arcade_cfgs", $"{streamingAssetsPath}/Configuration/Arcades");
            _vfs.MountDirectory("emulator_cfgs", $"{streamingAssetsPath}/Configuration/Emulators");
            _vfs.MountDirectory("emulators", $"{streamingAssetsPath}/Emulators");
            _vfs.MountDirectory("media", $"{streamingAssetsPath}/Media");

            _vfs.MountFile("general_cfg", "cfg/GeneralConfiguration.json");
        }

        private void LoadModels()
        {
            LoadedModels = new GameObject[]
            {
                Resources.Load<GameObject>("Games/1942"),
                Resources.Load<GameObject>("Games/1943"),
                Resources.Load<GameObject>("Games/alpine"),
                Resources.Load<GameObject>("Games/amidar"),
                Resources.Load<GameObject>("Games/arkanoid"),
                Resources.Load<GameObject>("Games/asteroid")
            };
        }
    }
}
