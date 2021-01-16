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

using System.Diagnostics.CodeAnalysis;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace Arcade.UnityEditor
{
    public static class EditorMenus
    {
        [MenuItem("3DArcade/Switch Arcade Type", false, 0), SuppressMessage("CodeQuality", "IDE0051:Remove unused private members")]
        private static void MenuSwitchArcadeType()
        {
            GameObject playerControls = GameObject.Find("PlayerControls");
            Assert.IsNotNull(playerControls);

            PlayerFpsControls fpsControls = playerControls.GetComponentInChildren<PlayerFpsControls>(true);
            Assert.IsNotNull(fpsControls);
            PlayerCylControls cylControls = playerControls.GetComponentInChildren<PlayerCylControls>(true);
            Assert.IsNotNull(cylControls);

            if (fpsControls.gameObject.activeInHierarchy)
            {
                fpsControls.gameObject.SetActive(false);
                cylControls.gameObject.SetActive(true);
            }
            else
            {
                fpsControls.gameObject.SetActive(true);
                cylControls.gameObject.SetActive(false);
            }

            ArcadeConfigurationComponent arcadeConfigurationComponent = Object.FindObjectOfType<ArcadeConfigurationComponent>();
            if (arcadeConfigurationComponent != null)
                new EditorLoadSaveArcadeSubstitute().LoadAndStartArcade(arcadeConfigurationComponent.Id);
        }

        [MenuItem("3DArcade/Reload Current Arcade", false, 100), SuppressMessage("CodeQuality", "IDE0051:Remove unused private members")]
        private static void MenuReloadCurrentArcade()
        {
            ArcadeConfigurationComponent arcadeConfigurationComponent = Object.FindObjectOfType<ArcadeConfigurationComponent>();
            if (arcadeConfigurationComponent != null)
                new EditorLoadSaveArcadeSubstitute().LoadAndStartArcade(arcadeConfigurationComponent.Id);
        }

        [MenuItem("3DArcade/Save Arcade", false, 102), SuppressMessage("CodeQuality", "IDE0051:Remove unused private members")]
        private static void MenuSaveArcade()
        {
            EditorLoadSaveArcadeSubstitute loadSaveSubstitute = new EditorLoadSaveArcadeSubstitute();
            if (loadSaveSubstitute.ArcadeHierarchy.RootNode.TryGetComponent(out ArcadeConfigurationComponent arcadeCfgComponent))
                loadSaveSubstitute.SaveArcade(arcadeCfgComponent);
        }

        // Validation
        [MenuItem("3DArcade/Switch Arcade Type", true), SuppressMessage("CodeQuality", "IDE0051:Remove unused private members")]
        private static bool MenuSwitchArcadeTypeValidation() => !Application.isPlaying;

        [MenuItem("3DArcade/Reload Current Arcade", true), SuppressMessage("CodeQuality", "IDE0051:Remove unused private members")]
        private static bool MenuReloadCurrentArcadeValidation() => !Application.isPlaying;

        [MenuItem("3DArcade/Save Arcade", true), SuppressMessage("CodeQuality", "IDE0051:Remove unused private members")]
        private static bool MenuSaveArcadeValidation() => !Application.isPlaying;
    }
}
