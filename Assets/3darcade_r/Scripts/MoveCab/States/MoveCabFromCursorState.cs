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
    public class MoveCabFromCursorState : MoveCabState
    {
        private const float _raycastMaxDistance      = 22.0f;
        private const float _movementSpeedMultiplier = 0.8f;
        private const float _rotationSpeedMultiplier = 0.8f;

        public MoveCabFromCursorState(MoveCabStateContext stateContext)
        : base(stateContext)
        {
        }

        public override void OnEnter()
        {
            Debug.Log("  <color=green>Entered</color> MoveCabFromCursorState");
        }

        public override void OnExit()
        {
            Debug.Log("  <color=orange>Exited</color> MoveCabFromCursorState");
        }

        public override void OnUpdate(float dt)
        {
            if (Time.frameCount % 10 == 0)
            {
                Ray ray = _stateContext.Camera.ScreenPointToRay(Mouse.current.position.ReadValue());
                MoveCab.FindModelSetup(_stateContext.Data, ray, _raycastMaxDistance, _stateContext.RaycastLayers);
            }

            Vector2 positionInput           = _stateContext.PlayerControls.InputActions.FPSMoveCab.MoveModel.ReadValue<Vector2>();
            float rotationInput             = _stateContext.PlayerControls.InputActions.FPSMoveCab.RotateModel.ReadValue<float>();
            _stateContext.Input.AimPosition = positionInput * _movementSpeedMultiplier;
            _stateContext.Input.AimRotation = rotationInput * _rotationSpeedMultiplier;

            if (_stateContext.PlayerControls.InputActions.FPSMoveCab.GrabRelease.triggered)
            {
                if (_stateContext.Data.ModelSetup != null && _stateContext.Data.ModelSetup is MoveCab.IGrabbable)
                {
                    _stateContext.TransitionTo<MoveCabGrabState>();
                }
            }

            if (_stateContext.PlayerControls.InputActions.FPSMoveCab.AddModel.triggered)
            {
                _stateContext.TransitionTo<MoveCabAddState>();
            }

            if (_stateContext.PlayerControls.InputActions.GlobalControls.ToggleMouseCursor.triggered)
            {
                if (!Cursor.visible)
                {
                    _stateContext.TransitionTo<MoveCabFromViewState>();
                }
            }
        }

        public override void OnFixedUpdate(float dt)
        {
            if (_stateContext.Data.ModelSetup != null && _stateContext.Data.ModelSetup is MoveCab.IMovable)
            {
                MoveCab.ManualMoveAndRotate(_stateContext.Data, _stateContext.Input);
            }
        }
    }
}
