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
    public class MoveCabGrabState : MoveCabState
    {
        private const float _raycastMaxDistance = 22.0f;

        private MoveCabGrabSavedData _savedValues;

        public MoveCabGrabState(MoveCabContext context)
        : base(context)
        {
        }

        public override void OnEnter()
        {
            Debug.Log(">>> <color=green>Entered</color> MoveCabGrabState");

            _savedValues = MoveCabController.InitGrabMode(_data, _context.PlayerFpsControls.Camera);
        }

        public override void OnExit()
        {
            Debug.Log(">>> <color=orange>Exited</color> MoveCabGrabState");

            MoveCabController.RestoreSavedValues(_data, _savedValues);
            _savedValues = null;
        }

        public override void Update(float dt)
        {
            bool useMousePosition = Mouse.current != null && Cursor.visible;
            Vector2 rayPosition   = useMousePosition ? Mouse.current.position.ReadValue() : _data.ScreenPoint;
            Ray ray               = _context.PlayerFpsControls.Camera.ScreenPointToRay(rayPosition);
            MoveCabController.AutoMoveAndRotate(_data, ray, _context.PlayerFpsControls.transform.forward, _raycastMaxDistance, _context.RaycastLayers);

            if (_context.PlayerFpsControls.FirstPersonMoveCabActions.GrabReleaseModel.triggered)
            {
                _context.TransitionTo<MoveCabAimState>();
            }
        }
    }
}
