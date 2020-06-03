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
using UnityEngine.InputSystem;

namespace Arcade_r
{
    public sealed class ArcadeInternalGameState : ArcadeState
    {
        private const string _coreName = "mame2003_plus";
        private const string _romsDirectory = "D:/mame2003-plus/roms";

        private LibretroLauncher _libretroLauncher;

        public ArcadeInternalGameState(ArcadeContext context)
        : base(context)
        {
        }

        public override void OnEnter()
        {
            Debug.Log(">> <color=green>Entered</color> ArcadeInternalGameState");

            ModelSetup modelSetup = _context.CurrentInteractable as ModelSetup;
            _libretroLauncher = modelSetup.gameObject.AddComponent<LibretroLauncher>();
            _libretroLauncher.StartGame(_coreName, _romsDirectory, modelSetup.Id);
        }

        public override void OnExit()
        {
            Debug.Log(">> <color=orange>Exited</color> ArcadeInternalGameState");
            _libretroLauncher.StopGame();
        }

        public override void Update(float dt)
        {
            if (_context.App.PlayerControls.GlobalActions.Quit.triggered)
            {
                _context.TransitionTo<ArcadeNormalState>();
            }

            if (Keyboard.current.f1Key.wasPressedThisFrame)
            {
                if (_context.App.PlayerControls.FirstPersonActions.Look.enabled)
                {
                    _context.App.PlayerControls.FirstPersonActions.Look.Disable();
                }
                else
                {
                    _context.App.PlayerControls.FirstPersonActions.Look.Enable();
                }
            }
        }
    }
}
