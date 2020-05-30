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
using System.Diagnostics.CodeAnalysis;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace Arcade_r
{
    internal sealed class LoadArcadeWindow : EditorWindow
    {
        private static ArcadeHierarchy _arcadeHierarchy;
        private static IVirtualFileSystem _virtualFileSystem;
        private static ArcadeConfigurationManager _arcadeManager;
        private static PlayerControls _player;
        private static Camera _mainCamera;
        private static CinemachineVirtualCamera _vCamera;

        private static string[] _configurationNames;

        private Vector2 _scrollPos = Vector2.zero;

        [MenuItem("3DArcade_r/Load Arcade", false, 0), SuppressMessage("CodeQuality", "IDE0051:Remove unused private members")]
        private static void ShowWindow()
        {
            InitFields();
            LoadArcadeWindow window = GetWindow<LoadArcadeWindow>("Load Arcade");
            window.minSize = new Vector2(120f, 120f);
        }

        private void OnGUI()
        {
            GUILayout.Space(8f);
            DrawConfigurationsList();
        }

        private void DrawConfigurationsList()
        {
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, false, false);
            foreach (string name in _configurationNames)
            {
                if (GUILayout.Button(name))
                {
                    ArcadeConfiguration arcadeConfiguration = _arcadeManager.Get(name);
                    if (arcadeConfiguration == null)
                    {
                        return;
                    }

                    _arcadeHierarchy.Reset();
                    ArcadeController.StartArcade(arcadeConfiguration, _arcadeHierarchy.RootNode.transform, _player.transform);
                    GetWindow<LoadArcadeWindow>().Close();
                }
            }
            GUILayout.EndScrollView();
        }

        private static void InitFields()
        {
            _arcadeHierarchy = new ArcadeHierarchy();

            if (_virtualFileSystem == null)
            {
                _virtualFileSystem = new VirtualFileSystem();
                _virtualFileSystem.MountDirectory("arcade_cfgs", $"{SystemUtils.GetDataPath()}/3darcade_r~/Configuration/Arcades");
            }

            if (_arcadeManager == null)
            {
                _arcadeManager = new ArcadeConfigurationManager(_virtualFileSystem);
            }
            _arcadeManager.Refresh();

            if (_player == null)
            {
                _player = FindObjectOfType<PlayerControls>();
            }
            Assert.IsNotNull(_player);

            if (_mainCamera == null)
            {
                _mainCamera = Camera.main;
            }
            Assert.IsNotNull(_mainCamera);

            if (_vCamera == null)
            {
                _vCamera = _player.GetComponentInChildren<CinemachineVirtualCamera>();
            }
            Assert.IsNotNull(_vCamera);

            _configurationNames = _arcadeManager.GetNames();
        }

        // Validation
        [MenuItem("3DArcade_r/Load Arcade", true), SuppressMessage("CodeQuality", "IDE0051:Remove unused private members")]
        private static bool LoadArcadeWindowValidation()
        {
            return !Application.isPlaying;
        }
    }

    public static class EditorMenus
    {
        private static ArcadeHierarchy _arcadeHierarchy;
        private static IVirtualFileSystem _virtualFileSystem;
        private static ArcadeConfigurationManager _arcadeManager;
        private static PlayerControls _player;
        private static Camera _mainCamera;
        private static CinemachineVirtualCamera _vCamera;

        [MenuItem("3DArcade_r/Save Arcade", false, 1), SuppressMessage("CodeQuality", "IDE0051:Remove unused private members")]
        private static void MenuSaveArcade()
        {
            InitFields();

            if (_arcadeHierarchy.RootNode.TryGetComponent(out ArcadeConfigurationComponent arcadeConfigurationComponent))
            {
                if (string.IsNullOrEmpty(arcadeConfigurationComponent.Id))
                {
                    return;
                }
            }

            ArcadeConfiguration arcadeConfiguration = _arcadeHierarchy.RootNode.GetComponent<ArcadeConfigurationComponent>()
                                                                               .ToArcadeConfiguration(_player.transform, _mainCamera, _vCamera);
            _ = _arcadeManager.Save(arcadeConfiguration);
        }

        private static void InitFields()
        {
            _arcadeHierarchy = new ArcadeHierarchy();

            if (_virtualFileSystem == null)
            {
                _virtualFileSystem = new VirtualFileSystem();
                _virtualFileSystem.MountDirectory("arcade_cfgs", $"{SystemUtils.GetDataPath()}/3darcade_r~/Configuration/Arcades");
            }

            if (_arcadeManager == null)
            {
                _arcadeManager = new ArcadeConfigurationManager(_virtualFileSystem);
            }
            _arcadeManager.Refresh();

            if (_player == null)
            {
                _player = Object.FindObjectOfType<PlayerControls>();
            }
            Assert.IsNotNull(_player);

            if (_mainCamera == null)
            {
                _mainCamera = Camera.main;
            }
            Assert.IsNotNull(_mainCamera);

            if (_vCamera == null)
            {
                _vCamera = _player.GetComponentInChildren<CinemachineVirtualCamera>();
            }
            Assert.IsNotNull(_vCamera);
        }

        // Validation
        [MenuItem("3DArcade_r/Save Arcade", true), SuppressMessage("CodeQuality", "IDE0051:Remove unused private members")]
        private static bool MenuSaveArcadeValidation()
        {
            return !Application.isPlaying;
        }
    }
}
