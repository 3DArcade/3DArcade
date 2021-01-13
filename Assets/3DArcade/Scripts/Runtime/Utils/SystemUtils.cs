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

namespace Arcade
{
    public enum OS
    {
        MacOS,
        iOS,
        tvOS,
        Windows,
        Linux,
        Android
    }

    public static class SystemUtils
    {
        public static void ToggleMouseCursor()
        {
            if (Cursor.lockState == CursorLockMode.Locked)
                ShowMouseCursor();
            else
                HideMouseCursor();
        }

        public static void ShowMouseCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible   = true;
        }

        public static void HideMouseCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible   = false;
        }

        public static void ExitApp() =>
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit(0);
#endif

        public static OS GetCurrentOS()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:
                    return OS.MacOS;
                case RuntimePlatform.IPhonePlayer:
                    return OS.iOS;
                case RuntimePlatform.tvOS:
                    return OS.tvOS;
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    return OS.Windows;
                case RuntimePlatform.LinuxEditor:
                case RuntimePlatform.LinuxPlayer:
                    return OS.Linux;
                case RuntimePlatform.Android:
                    return OS.Android;
                case RuntimePlatform.WebGLPlayer:
                case RuntimePlatform.WSAPlayerX86:
                case RuntimePlatform.WSAPlayerX64:
                case RuntimePlatform.WSAPlayerARM:
                case RuntimePlatform.PS4:
                case RuntimePlatform.XboxOne:
                case RuntimePlatform.Switch:
                case RuntimePlatform.Lumin:
                case RuntimePlatform.Stadia:
                default:
                {
                    throw new System.NotSupportedException($"Platform not supported '{Application.platform}'");
                }
            }
        }

        public static string GetDataPath()
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
                return $"{Application.dataPath}/Raw";

            return Application.streamingAssetsPath;
        }
    }
}
