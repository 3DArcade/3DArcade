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
    public sealed class ApplicationMoveCabState : ApplicationState
    {
        private readonly FSM.Context<MoveCabState> _moveCabSubContext;

        public ApplicationMoveCabState(ApplicationStateContext context)
        : base(context)
        {
            _moveCabSubContext = new MoveCabStateContext(_data.PlayerControls, _data.Camera);
        }

        public override void OnEnter()
        {
            Debug.Log("> <color=green>Entered</color> ApplicationMoveCabState");

            _data.PlayerControls.FirstPersonActions.Enable();
            if (Cursor.visible)
            {
                _data.PlayerControls.FirstPersonActions.Look.Disable();
            }
            _data.PlayerControls.FirstPersonActions.Interact.Disable();

            _data.PlayerControls.FirstPersonMoveCabActions.Enable();

            _moveCabSubContext.TransitionTo<MoveCabAimState>();
        }

        public override void OnExit()
        {
            Debug.Log("> <color=orange>Exited</color> ApplicationMoveCabState");

            _data.PlayerControls.FirstPersonActions.Disable();
            _data.PlayerControls.FirstPersonMoveCabActions.Disable();
        }

        public override void Update(float dt)
        {
            if (_data.PlayerControls.FirstPersonActions.ToggleMoveCab.triggered || _data.PlayerControls.GlobalActions.Quit.triggered)
            {
                _context.TransitionTo<ApplicationRunningState>();
            }

            if (_data.PlayerControls.GlobalActions.ToggleCursor.triggered)
            {
                SystemUtils.ToggleMouseCursor();
                if (!Cursor.visible)
                {
                    _data.PlayerControls.FirstPersonActions.Look.Enable();
                }
                else
                {
                    _data.PlayerControls.FirstPersonActions.Look.Disable();
                }
            }

            _moveCabSubContext.Update(dt);
        }

        public override void FixedUpdate(float dt)
        {
            _moveCabSubContext.FixedUpdate(dt);
        }
    }
}
