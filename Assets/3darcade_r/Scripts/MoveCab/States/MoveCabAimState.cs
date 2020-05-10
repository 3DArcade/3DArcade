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
    public class MoveCabAimState : MoveCabState
    {
        private const float _raycastMaxDistance      = 22.0f;
        private const float _movementSpeedMultiplier = 0.8f;
        private const float _rotationSpeedMultiplier = 0.8f;

        public MoveCabAimState(MoveCabStateContext stateContext)
        : base(stateContext)
        {
        }

        public override void OnEnter()
        {
            Debug.Log("<color=green>Entered</color> MoveCabAimState");
        }

        public override void OnExit()
        {
            Debug.Log("<color=orange>Exited</color> MoveCabAimState");
        }

        public override void OnUpdate(float dt)
        {
            if (Time.frameCount % 10 == 0)
            {
                MoveCab.FindModelSetup(_stateContext.Data, _stateContext.Camera, _raycastMaxDistance, _stateContext.RaycastLayers);
            }

            _stateContext.Input.AimPosition = _stateContext.PlayerControls.InputActions.FPSMoveCab.MoveModel.ReadValue<Vector2>() * _movementSpeedMultiplier;
            _stateContext.Input.AimRotation = _stateContext.PlayerControls.InputActions.FPSMoveCab.RotateModel.ReadValue<float>() * _rotationSpeedMultiplier;

            if (_stateContext.PlayerControls.InputActions.FPSControls.Interact.triggered)
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
