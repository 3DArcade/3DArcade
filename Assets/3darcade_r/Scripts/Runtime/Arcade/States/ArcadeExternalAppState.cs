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

namespace Arcade_r
{
    public sealed class ArcadeExternalAppState : ArcadeState
    {
        private readonly ExternalAppController _externalAppController;
        private bool _isContentRunning;

        public ArcadeExternalAppState(ArcadeContext context)
        : base(context)
        {
            _externalAppController = new ExternalAppController();

            _externalAppController.OnAppStarted += OnAppStarted;
            _externalAppController.OnAppExited  += OnAppExited;
        }

        public override void OnEnter()
        {
            Debug.Log(">> <color=green>Entered</color> ArcadeExternalAppState");

            if (_context.GetLauncherAndContentForCurrentModelConfiguration(out LauncherConfiguration launcher, out ContentConfiguration content))
            {
                SaveUnityWindow();

                _isContentRunning = _externalAppController.StartContent(launcher, content);
                if (_isContentRunning)
                {
                    return;
                }
            }

            _context.TransitionTo<ArcadeNormalState>();
        }

        public override void OnExit()
        {
            Debug.Log(">> <color=orange>Exited</color> ArcadeExternalAppState");
        }

        public override void Update(float dt)
        {
            if (_isContentRunning)
            {
                return;
            }

            RestoreUnityWindow();

            _externalAppController.StopCurrent();
            _context.TransitionTo<ArcadeNormalState>();
        }

        private void OnAppStarted(OSUtils.ProcessStartedData data, LauncherConfiguration launcher, ContentConfiguration content)
        {
        }

        private void OnAppExited(OSUtils.ProcessExitedData data, LauncherConfiguration launcher, ContentConfiguration content)
        {
            _isContentRunning = false;
        }

#if UNITY_EDITOR_WIN
        [System.Runtime.InteropServices.DllImport("user32")] static extern uint GetActiveWindow();
        [System.Runtime.InteropServices.DllImport("user32")] static extern bool SetForegroundWindow(System.IntPtr hWnd);
        private System.IntPtr _editorHwnd;
        private void SaveUnityWindow() => _editorHwnd = (System.IntPtr)GetActiveWindow();
        private void RestoreUnityWindow()
        {
            _ = SetForegroundWindow(_editorHwnd);
            SystemUtils.ToggleMouseCursor();
            SystemUtils.ToggleMouseCursor();
        }
#else
        private void SaveUnityWindow() {}
        private void RestoreUnityWindow() {}
#endif
    }
}
