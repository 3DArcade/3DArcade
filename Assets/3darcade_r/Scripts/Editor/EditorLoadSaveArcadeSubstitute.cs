﻿/* MIT License

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
    public class EditorLoadSaveArcadeSubstitute
    {
        public readonly ArcadeHierarchy ArcadeHierarchy;
        public readonly Database<ArcadeConfiguration> ArcadeDatabase;

        private static GameObject _hierarchyPrefab;

        private readonly IVirtualFileSystem _virtualFileSystem;
        private readonly AssetCache<GameObject> _gameObjectCache;
        private readonly Database<LauncherConfiguration> _launcherDatabase;
        private readonly Database<ContentListConfiguration> _contentListDatabase;
        private readonly PlayerControls _player;
        private readonly ArcadeController _arcadeController;

        public EditorLoadSaveArcadeSubstitute()
        {
            if (_hierarchyPrefab == null)
            {
                _hierarchyPrefab = Resources.Load<GameObject>("Misc/pfArcadeHierarchy");
            }

            ArcadeHierarchy = new ArcadeHierarchy();

            _virtualFileSystem = new VirtualFileSystem();
            _virtualFileSystem.MountDirectory("arcade_cfgs", $"{SystemUtils.GetDataPath()}/3darcade_r~/Configuration/Arcades");
            _virtualFileSystem.MountDirectory("launcher_cfgs", $"{SystemUtils.GetDataPath()}/3darcade_r~/Configuration/Launchers");
            _virtualFileSystem.MountDirectory("contentlist_cfgs", $"{SystemUtils.GetDataPath()}/3darcade_r~/Configuration/ContentLists");

            _gameObjectCache  = new GameObjectCache();

            ArcadeDatabase       = new ArcadeDatabase(_virtualFileSystem);
            _launcherDatabase    = new LauncherDatabase(_virtualFileSystem);
            _contentListDatabase = new ContentListDatabase(_virtualFileSystem);

            _player = Object.FindObjectOfType<PlayerControls>();
            Assert.IsNotNull(_player);

            _arcadeController = new ArcadeController(ArcadeHierarchy, _gameObjectCache, _player.transform, _launcherDatabase, _contentListDatabase, null);
        }

        public void LoadAndStartArcade(string name)
        {
            ArcadeConfiguration arcadeConfiguration = ArcadeDatabase.Get(name);
            if (arcadeConfiguration == null)
            {
                return;
            }

            if (!ArcadeHierarchy.RootNode.TryGetComponent(out ArcadeConfigurationComponent arcadeConfigurationComponent))
            {
                arcadeConfigurationComponent = ArcadeHierarchy.RootNode.gameObject.AddComponent<ArcadeConfigurationComponent>();
            }
            arcadeConfigurationComponent.FromArcadeConfiguration(arcadeConfiguration);

            if (arcadeConfiguration.ArcadeType == ArcadeType.Fps || arcadeConfiguration.ArcadeType == ArcadeType.Cyl)
            {
                ArcadeHierarchy.Reset();
            }

            _ = _arcadeController.StartArcade(arcadeConfiguration);
        }

        public void SaveArcade(ArcadeConfigurationComponent arcadeConfiguration)
        {
            _ = ArcadeDatabase.Save(arcadeConfiguration.ToArcadeConfiguration(_player.transform, Camera.main));
        }
    }
}