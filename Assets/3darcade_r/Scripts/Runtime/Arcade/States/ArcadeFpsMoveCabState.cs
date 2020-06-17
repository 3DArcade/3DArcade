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
    public sealed class ArcadeFpsMoveCabState : ArcadeState
    {
        private readonly MoveCabContext _moveCabContext;

        public ArcadeFpsMoveCabState(ArcadeContext context)
        : base(context)
        {
            _moveCabContext = new MoveCabContext(_context.App.PlayerFpsControls);
        }

        public override void OnEnter()
        {
            Debug.Log($">> <color=green>Entered</color> {GetType().Name}");

            _context.App.PlayerFpsControls.FpsArcadeActions.Enable();
            if (Cursor.visible)
            {
                _context.App.PlayerFpsControls.FpsArcadeActions.Look.Disable();
            }
            _context.App.PlayerFpsControls.FpsArcadeActions.Interact.Disable();

            _context.App.PlayerFpsControls.FpsMoveCabActions.Enable();

            _moveCabContext.TransitionTo<MoveCabAimState>();

            _context.App.UIController.EnableMoveCabUI();
        }

        public override void OnExit()
        {
            Debug.Log($">> <color=orange>Exited</color> {GetType().Name}");

            _context.App.PlayerFpsControls.FpsArcadeActions.Disable();
            _context.App.PlayerFpsControls.FpsMoveCabActions.Disable();

            _context.App.UIController.DisableMoveCabUI();
        }

        public override void Update(float dt)
        {
            if (_context.App.PlayerFpsControls.GlobalActions.Quit.triggered)
            {
                _context.ReloadCurrentArcadeConfigurationModels();
                _context.TransitionTo<ArcadeFpsNormalState>();
            }

            if (_context.App.PlayerFpsControls.FpsArcadeActions.ToggleMoveCab.triggered)
            {
                _ = _context.SaveCurrentArcadeConfigurationModels();
                _context.TransitionTo<ArcadeFpsNormalState>();
            }

            if (_context.App.PlayerFpsControls.GlobalActions.ToggleCursor.triggered)
            {
                SystemUtils.ToggleMouseCursor();
                if (!Cursor.visible)
                {
                    _context.App.PlayerFpsControls.FpsArcadeActions.Look.Enable();
                }
                else
                {
                    _context.App.PlayerFpsControls.FpsArcadeActions.Look.Disable();
                }
            }

            _moveCabContext.Update(dt);
        }

        public override void FixedUpdate(float dt)
        {
            _moveCabContext.FixedUpdate(dt);
        }
    }
}
