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
    public class MoveCabAimState : MoveCabState
    {
        private const int NUM_FRAMES_TO_SKIP = 10;

        private static readonly float _raycastMaxDistance      = 22.0f;
        private static readonly float _movementSpeedMultiplier = 0.8f;
        private static readonly float _rotationSpeedMultiplier = 0.8f;

        public MoveCabAimState(MoveCabContext context)
        : base(context)
        {
        }

        public override void OnEnter()
        {
            Debug.Log(">>> <color=green>Entered</color> MoveCabAimState");
        }

        public override void OnExit()
        {
            Debug.Log(">>> <color=orange>Exited</color> MoveCabAimState");
        }

        public override void Update(float dt)
        {
            if (Time.frameCount % NUM_FRAMES_TO_SKIP == 0)
            {
                Vector2 rayPosition;
                if (Cursor.visible && Mouse.current != null)
                {
                    rayPosition = Mouse.current.position.ReadValue();
                }
                else
                {
                    rayPosition = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
                }
                Ray ray = _context.PlayerFpsControls.Camera.ScreenPointToRay(rayPosition);
                MoveCabController.FindModelSetup(_data, ray, _raycastMaxDistance, _context.RaycastLayers);
            }

            if (_data.ModelSetup != null && _data.ModelSetup.MoveCabMovable)
            {
                Vector2 positionInput = _context.PlayerFpsControls.FirstPersonMoveCabActions.MoveModel.ReadValue<Vector2>();
                float rotationInput   = _context.PlayerFpsControls.FirstPersonMoveCabActions.RotateModel.ReadValue<float>();
                _data.AimPosition     = positionInput * _movementSpeedMultiplier;
                _data.AimRotation     = rotationInput * _rotationSpeedMultiplier;

                if (_data.ModelSetup.MoveCabGrabbable)
                {
                    if (_context.PlayerFpsControls.FirstPersonMoveCabActions.GrabReleaseModel.triggered)
                    {
                        _context.TransitionTo<MoveCabGrabState>();
                    }
                }
            }
        }

        public override void FixedUpdate(float dt)
        {
            if (_data.ModelSetup != null && _data.ModelSetup.MoveCabMovable)
            {
                MoveCabController.ManualMoveAndRotate(_data.ModelSetup.transform, _data.Rigidbody, _data.AimPosition, _data.AimRotation);
            }
        }
    }
}
