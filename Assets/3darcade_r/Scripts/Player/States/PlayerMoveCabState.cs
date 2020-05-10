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
    public class PlayerMoveCabState : PlayerState
    {
        private readonly MoveCabStateContext _moveCabStateContext;

        public PlayerMoveCabState(PlayerStateContext stateController, PlayerControls playerControls, Camera camera)
        : base(stateController, playerControls, camera)
        {
            _moveCabStateContext = new MoveCabStateContext(playerControls, camera);
        }

        public override void OnEnter()
        {
            Debug.Log("<color=green>Entered</color> PlayerMoveCabState");

            _playerControls.EnableMovement      = true;
            _playerControls.EnableLook          = !Cursor.visible;
            _playerControls.EnableInteract      = true;
            _playerControls.EnableToggleMoveCab = true;
            _playerControls.InputActions.FPSMoveCab.Enable();

            _moveCabStateContext.TransitionTo<MoveCabAimState>();
        }

        public override void OnExit()
        {
            Debug.Log("<color=orange>Exited</color> PlayerMoveCabState");

            _playerControls.EnableMovement      = false;
            _playerControls.EnableLook          = !Cursor.visible;
            _playerControls.EnableInteract      = false;
            _playerControls.EnableToggleMoveCab = false;
            _playerControls.InputActions.FPSMoveCab.Disable();
        }

        public override void OnUpdate(float dt)
        {
            if (_playerControls.InputActions.GlobalControls.ToggleMouseCursor.triggered)
            {
                Utils.ToggleMouseCursor();
                _playerControls.EnableLook = !_playerControls.EnableLook;
            }

            if (_playerControls.InputActions.GlobalControls.Quit.triggered || _playerControls.InputActions.FPSControls.ToggleMoveCab.triggered)
            {
                _stateContext.TransitionTo<PlayerNormalState>();
            }

            _moveCabStateContext.Update(dt);
        }

        public override void OnFixedUpdate(float dt)
        {
            _moveCabStateContext.FixedUpdate(dt);
        }
    }
}
