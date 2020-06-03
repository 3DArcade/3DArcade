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
        public readonly ArcadeConfigurationManager ArcadeManager;
        public readonly GameObjectCache GameObjectCache;
        public readonly PlayerControls Player;
        public readonly Camera MainCamera;
        public readonly CinemachineVirtualCamera VirtualCamera;
        public readonly string[] ConfigurationNames;

        private readonly IVirtualFileSystem _virtualFileSystem;
        private readonly ArcadeController _arcadeController;

        public EditorLoadSaveArcadeSubstitute()
        {
            _virtualFileSystem = new VirtualFileSystem();
            _virtualFileSystem.MountDirectory("arcade_cfgs", $"{SystemUtils.GetDataPath()}/3darcade_r~/Configuration/Arcades");

            ArcadeHierarchy = new ArcadeHierarchy();

            ArcadeManager   = new ArcadeConfigurationManager(_virtualFileSystem);
            GameObjectCache = new GameObjectCache();

            Player = Object.FindObjectOfType<PlayerControls>();
            Assert.IsNotNull(Player);

            MainCamera = Camera.main;
            Assert.IsNotNull(MainCamera);

            VirtualCamera = Player.GetComponentInChildren<CinemachineVirtualCamera>();
            Assert.IsNotNull(VirtualCamera);

            ConfigurationNames = ArcadeManager.GetNames();

            _arcadeController = new ArcadeController(ArcadeHierarchy, GameObjectCache, null, Player.transform);
        }

        public void LoadAndStartArcade(string name)
        {
            ArcadeConfiguration arcadeConfiguration = ArcadeManager.Get(name);
            if (arcadeConfiguration == null)
            {
                return;
            }

            ArcadeHierarchy.Reset();
            _arcadeController.StartArcade(arcadeConfiguration);
        }

        public void SaveArcade(ArcadeConfigurationComponent arcadeConfigurationComponent)
        {
            _ = ArcadeManager.Save(arcadeConfigurationComponent.ToArcadeConfiguration(Player.transform, MainCamera, VirtualCamera));
        }
    }
}
