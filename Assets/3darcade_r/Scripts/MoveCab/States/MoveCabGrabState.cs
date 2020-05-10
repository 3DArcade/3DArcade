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
    public class MoveCabGrabState : MoveCabState
    {
        private const float _raycastMaxDistance = 22.0f;

        private MoveCab.SavedValues _savedValues;

        public MoveCabGrabState(MoveCabStateContext stateContext)
        : base(stateContext)
        {
        }

        public override void OnEnter()
        {
            Debug.Log("<color=green>Entered</color> MoveCabGrabState");
            _savedValues = MoveCab.InitGrabMode(_stateContext.Data, _stateContext.Camera);
        }

        public override void OnExit()
        {
            Debug.Log("<color=orange>Exited</color> MoveCabGrabState");
            MoveCab.RestoreSavedValues(_stateContext.Data, _savedValues);
            _savedValues = null;
        }

        public override void OnUpdate(float dt)
        {
            MoveCab.AutoMoveAndRotate(_stateContext.Data, _stateContext.Camera, _stateContext.PlayerControls.transform.forward, _raycastMaxDistance, _stateContext.RaycastLayers);

            if (_stateContext.PlayerControls.InputActions.FPSControls.Interact.triggered)
            {
                _stateContext.TransitionTo<MoveCabAimState>();
            }
        }
    }
}
