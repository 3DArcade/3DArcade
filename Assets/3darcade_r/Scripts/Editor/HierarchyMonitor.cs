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
    // [InitializeOnLoad]
    public static class HierarchyMonitor
    {
        static HierarchyMonitor()
        {
            if (SceneManager.GetActiveScene().name != "3darcade_r")
            {
                return;
            }

            EditorApplication.hierarchyChanged += OnHierarchyChanged;
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
    }
}
