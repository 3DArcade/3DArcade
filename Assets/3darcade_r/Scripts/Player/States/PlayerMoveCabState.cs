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
        private readonly MoveCabStateContext<MoveCabState> _moveCabStateContext;

        public PlayerMoveCabState(PlayerStateContext<PlayerState> stateContext)
        : base(stateContext)
        {
            _moveCabStateContext = new MoveCabStateContext<MoveCabState>(_stateContext.PlayerControls);
        }

        public override void OnEnter()
        {
            Debug.Log("<color=green>Entered</color> PlayerMoveCabState");

            _stateContext.PlayerControls.FirstPersonActions.Enable();
            if (Cursor.visible)
            {
                _stateContext.PlayerControls.FirstPersonActions.Look.Disable();
            }
            _stateContext.PlayerControls.FirstPersonActions.Interact.Disable();

            _stateContext.PlayerControls.FirstPersonMoveCabActions.Enable();

            if (Cursor.visible)
            {
                _moveCabStateContext.TransitionTo<MoveCabFromCursorState>();
            }
            else
            {
                _moveCabStateContext.TransitionTo<MoveCabFromViewState>();
            }
        }

        public override void OnExit()
        {
            Debug.Log("<color=orange>Exited</color> PlayerMoveCabState");

            _stateContext.PlayerControls.FirstPersonActions.Disable();
            _stateContext.PlayerControls.FirstPersonMoveCabActions.Disable();
        }

        public override void OnUpdate(float dt)
        {
            if (_stateContext.PlayerControls.GlobalActions.Quit.triggered || _stateContext.PlayerControls.FirstPersonActions.ToggleMoveCab.triggered)
            {
                _stateContext.TransitionTo<PlayerNormalState>();
            }

            if (_stateContext.PlayerControls.GlobalActions.ToggleCursor.triggered)
            {
                Utils.ToggleMouseCursor();
                if (!Cursor.visible)
                {
                    _stateContext.PlayerControls.FirstPersonActions.Look.Enable();
                }
                else
                {
                    _stateContext.PlayerControls.FirstPersonActions.Look.Disable();
                }
            }

            _moveCabStateContext.UpdateState(dt);
        }

        public override void OnFixedUpdate(float dt)
        {
            _moveCabStateContext.FixedUpdateState(dt);
        }
    }
}
