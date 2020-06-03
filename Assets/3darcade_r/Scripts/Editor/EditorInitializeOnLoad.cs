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

using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Arcade_r
{
    [InitializeOnLoad]
    public static class EditorInitializeOnLoad
    {
        private const string EXPECTED_SCENE_NAME = "3darcade_r";

        static EditorInitializeOnLoad()
        {
            // Using 'update' because InitializeOnLoad occurs before the scene is loaded and active...
            EditorApplication.update += OnEditorUpdate;
        }

        private static void OnEditorUpdate()
        {
            if (SceneManager.GetActiveScene().name != EXPECTED_SCENE_NAME)
            {
                return;
            }

            EditorApplication.update -= OnEditorUpdate;

            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

            EditorApplication.hierarchyChanged -= OnHierarchyChanged;
            EditorApplication.hierarchyChanged += OnHierarchyChanged;

            if (!EditorApplication.isPlayingOrWillChangePlaymode)
            {
                ReloadCurrentArcade();
            }
        }

        private static void OnHierarchyChanged()
        {
            if (Application.isPlaying)
            {
                return;
            }

            GameObject activeObj = Selection.activeGameObject;
            if (activeObj == null)
            {
                return;
            }

            Transform parentTransform = activeObj.transform.parent;
            if (parentTransform == null)
            {
                return;
            }

            switch (parentTransform.name)
            {
                case "ArcadeModels":
                    activeObj.layer = LayerMask.NameToLayer("Arcade/ArcadeModels");
                    activeObj.AddModelSetupIfNotFound<ArcadeModelSetup>();
                    break;
                case "GameModels":
                    activeObj.layer = LayerMask.NameToLayer("Arcade/GameModels");
                    activeObj.AddModelSetupIfNotFound<GameModelSetup>();
                    break;
                case "PropModels":
                    activeObj.layer = LayerMask.NameToLayer("Arcade/PropModels");
                    activeObj.AddModelSetupIfNotFound<PropModelSetup>();
                    break;
                default:
                    break;
            }
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredEditMode)
            {
                ReloadCurrentArcade();
            }
        }

        private static void ReloadCurrentArcade()
        {
            EditorLoadSaveArcadeSubstitute loadSaveSubstitute = new EditorLoadSaveArcadeSubstitute();

            if (!loadSaveSubstitute.ArcadeHierarchy.RootNode.TryGetComponent(out ArcadeConfigurationComponent arcadeConfigurationComponent))
            {
                return;
            }

            if (string.IsNullOrEmpty(arcadeConfigurationComponent.Id))
            {
                return;
            }

            loadSaveSubstitute.LoadAndStartArcade(arcadeConfigurationComponent.Id);
        }
    }
}
