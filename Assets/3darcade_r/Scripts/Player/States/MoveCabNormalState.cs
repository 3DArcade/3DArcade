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

namespace Arcade_r.Player
{
    public class MoveCabNormalState : State
    {
        [SerializeField] private Vector3 _raycastOffset    = Vector3.zero;
        [SerializeField] private float _raycastMaxDistance = 2.5f;
        [SerializeField] private LayerMask _raycastLayers  = 0;

        private MoveCabData _data = null;

        public override void OnEnter()
        {
            _playerControls.EnableMovement = true;
            _playerControls.EnableLook     = true;

            _raycastLayers = LayerMask.GetMask("Arcade/GameModels", "Arcade/PropModels");

            if (_data == null)
            {
                _data = Resources.Load<MoveCabData>("ScriptableObjects/MoveCabData");
            }
        }

        public override void OnUpdate(float dt)
        {
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                Utils.ToggleMouseCursor();
                _playerControls.EnableLook = !_playerControls.EnableLook;
            }

            if (Keyboard.current.escapeKey.wasPressedThisFrame || Keyboard.current.mKey.wasPressedThisFrame)
            {
                _stateController.TransitionTo<NormalState>();
            }

            if (Time.frameCount % 10 == 0)
            {
                FindMovable();
            }

            if ((Keyboard.current.eKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame) && _data.Transform != null)
            {
                _stateController.TransitionTo<MoveCabMovingState>();
            }
        }

        public override void OnExit()
        {
            _playerControls.EnableMovement = false;
            _playerControls.EnableLook     = false;
        }

        public override void OnDrawDebugUI()
        {
            GUILayout.Label("Current state: MoveCabNormal State");
            GUILayout.Label("\nPlayer controls: WASD to move, <Mouse> to look around, Shift to sprint, Space to jump, right click to toggle cursor");
            GUILayout.Label("\nPress M or ESC to go back to 'Normal' state");
            GUILayout.Label($"\nSelected Movable: {(_data.Transform != null ? _data.Transform.name : "none")}");
            if (_data.Transform != null)
            {
                GUILayout.Label("Press E or LeftMouse Button to grab this model and enter 'MoveCabMoving' state");
            }
        }

        private void FindMovable()
        {
            if (PhysicsUtils.RaycastFromScreen(out RaycastHit hitInfo,
                                               _camera,
                                               _raycastOffset,
                                               _raycastMaxDistance,
                                               _raycastLayers))
            {
                if (hitInfo.transform.GetComponent<IMoveCabMovable>() != null)
                {
                    _data.SetData(hitInfo.transform, hitInfo.collider, hitInfo.rigidbody);
                }
                else
                {
                    _data.ResetData();
                }
            }
            else
            {
                _data.ResetData();
            }
        }
    }
}
