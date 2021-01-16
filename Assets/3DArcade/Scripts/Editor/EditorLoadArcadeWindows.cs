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

namespace Arcade.UnityEditor
{
    public sealed class EditorLoadArcadeWindow : EditorWindow
    {
        private static EditorLoadSaveArcadeSubstitute _loadSaveSubstitute;
        private static string[] _configurationNames;

        private Vector2 _scrollPos = Vector2.zero;

        [MenuItem("3DArcade/Load Arcade", false, 101)]
        private static void ShowWindow()
        {
            _loadSaveSubstitute = new EditorLoadSaveArcadeSubstitute();
            _configurationNames = _loadSaveSubstitute.ArcadeDatabase.GetNames();

            EditorLoadArcadeWindow window = GetWindow<EditorLoadArcadeWindow>("Load Arcade");
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
                    _loadSaveSubstitute.LoadAndStartArcade(name);
                    GetWindow<EditorLoadArcadeWindow>().Close();
                }
            }
            GUILayout.EndScrollView();
        }

        // Validation
        [MenuItem("3DArcade/Load Arcade", true)]
        private static bool LoadArcadeWindowValidation() => !Application.isPlaying;
    }
}
