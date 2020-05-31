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
    public class LoadSaveArcadeFramework
    {
        public readonly ArcadeHierarchy ArcadeHierarchy;
        public readonly ArcadeConfigurationManager ArcadeManager;
        public readonly PlayerControls Player;
        public readonly Camera MainCamera;
        public readonly CinemachineVirtualCamera VirtualCamera;
        public readonly string[] ConfigurationNames;

        private readonly IVirtualFileSystem _virtualFileSystem;

        public LoadSaveArcadeFramework()
        {
            _virtualFileSystem = new VirtualFileSystem();
            _virtualFileSystem.MountDirectory("arcade_cfgs", $"{SystemUtils.GetDataPath()}/3darcade_r~/Configuration/Arcades");

            ArcadeHierarchy = new ArcadeHierarchy();

            ArcadeManager = new ArcadeConfigurationManager(_virtualFileSystem);

            Player = Object.FindObjectOfType<PlayerControls>();
            Assert.IsNotNull(Player);

            MainCamera = Camera.main;
            Assert.IsNotNull(MainCamera);

            VirtualCamera = Player.GetComponentInChildren<CinemachineVirtualCamera>();
            Assert.IsNotNull(VirtualCamera);

            ConfigurationNames = ArcadeManager.GetNames();
        }
    }

    public sealed class LoadArcadeWindow : EditorWindow
    {
        private static LoadSaveArcadeFramework _framework;
        private Vector2 _scrollPos = Vector2.zero;

        [MenuItem("3DArcade_r/Load Arcade", false, 0), SuppressMessage("CodeQuality", "IDE0051:Remove unused private members")]
        private static void ShowWindow()
        {
            _framework = new LoadSaveArcadeFramework();
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
            foreach (string name in _framework.ConfigurationNames)
            {
                if (GUILayout.Button(name))
                {
                    ArcadeConfiguration arcadeConfiguration = _framework.ArcadeManager.Get(name);
                    if (arcadeConfiguration == null)
                    {
                        return;
                    }

                    _framework.ArcadeHierarchy.Reset();
                    ArcadeController.StartArcade(arcadeConfiguration, _framework.ArcadeHierarchy.RootNode.transform, _framework.Player.transform);
                    GetWindow<LoadArcadeWindow>().Close();
                }
            }
            GUILayout.EndScrollView();
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
        private static LoadSaveArcadeFramework _framework;

        [MenuItem("3DArcade_r/Save Arcade", false, 1), SuppressMessage("CodeQuality", "IDE0051:Remove unused private members")]
        private static void MenuSaveArcade()
        {
            _framework = new LoadSaveArcadeFramework();

            if (!_framework.ArcadeHierarchy.RootNode.TryGetComponent(out ArcadeConfigurationComponent arcadeConfigurationComponent))
            {
                return;
            }
            if (string.IsNullOrEmpty(arcadeConfigurationComponent.Id))
            {
                return;
            }

            _ = _framework.ArcadeManager.Save(arcadeConfigurationComponent.ToArcadeConfiguration(_framework.Player.transform,
                                                                                                 _framework.MainCamera,
                                                                                                 _framework.VirtualCamera));
        }

        // Validation
        [MenuItem("3DArcade_r/Save Arcade", true), SuppressMessage("CodeQuality", "IDE0051:Remove unused private members")]
        private static bool MenuSaveArcadeValidation()
        {
            return !Application.isPlaying;
        }
    }
}
