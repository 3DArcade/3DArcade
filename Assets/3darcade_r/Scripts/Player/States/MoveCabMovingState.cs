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
    public class MoveCabMovingState : State
    {
        [SerializeField] private Vector3 _raycastOffset    = new Vector3(0f, -200f, 0f);
        [SerializeField] private float _raycastMaxDistance = 12f;
        [SerializeField] private LayerMask _raycastLayers  = 0;

        [SerializeField] private bool _useMouseWheelForRotation = true;
        [SerializeField] private float _mouseWheelSensitivity   = 2.0f;

        private MoveCabData _moveCabData    = null;
        private bool _colliderWasTrigger    = false;
        private bool _rigidBodyWasKinematic = false;

        private float _mouseWheelDelta = 0f;

        public override void OnEnter()
        {
            _playerControls.EnableMovement = true;
            _playerControls.EnableLook     = true;

            _raycastLayers = LayerMask.GetMask("Arcade/ArcadeModels");

            if (_moveCabData == null)
            {
                _moveCabData = Resources.Load<MoveCabData>("ScriptableObjects/MoveCabData");
            }

            _colliderWasTrigger    = _moveCabData.Collider.isTrigger;
            _rigidBodyWasKinematic = _moveCabData.Rigidbody.isKinematic;

            _moveCabData.Collider.isTrigger    = true;
            _moveCabData.Rigidbody.isKinematic = true;
        }

        public override void OnUpdate(float dt)
        {
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                Utils.ToggleMouseCursor();
                _playerControls.EnableLook = !_playerControls.EnableLook;
            }

            if (Mouse.current.middleButton.wasPressedThisFrame)
            {
                _useMouseWheelForRotation = !_useMouseWheelForRotation;
                _mouseWheelDelta          = _moveCabData.Transform.forward.z;
            }

            if (_useMouseWheelForRotation)
            {
                _mouseWheelDelta += Mouse.current.scroll.ReadValue().y * _mouseWheelSensitivity * Time.deltaTime;
            }

            if (Keyboard.current.escapeKey.wasPressedThisFrame || Keyboard.current.mKey.wasPressedThisFrame)
            {
                _stateController.TransitionTo<NormalState>();
            }

            if ((Keyboard.current.eKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame))
            {
                _stateController.TransitionTo<MoveCabNormalState>();
            }

            TrySetModelPosition();
        }

        public override void OnExit()
        {
            _playerControls.EnableMovement = false;
            _playerControls.EnableLook = false;

            _moveCabData.Collider.isTrigger    = _colliderWasTrigger;
            _moveCabData.Rigidbody.isKinematic = _rigidBodyWasKinematic;
        }

        public override void OnDrawDebugUI()
        {
            GUILayout.Label("Current state: MoveCabMoving State");

            GUILayout.Label("\nPlayer controls: WASD to move, <Mouse> to look around, Shift to sprint, Space to jump, right click to toggle cursor");

            GUILayout.Label($"\nUsing mouse wheel for rotation: {_useMouseWheelForRotation}");
            GUILayout.Label("Press MiddleMouse Button to toggle");
            GUILayout.Label("-If false, always face the player");
            GUILayout.Label("-Ignored when targeting a wall");

            GUILayout.Label($"\nSelected Movable: {_moveCabData.Transform.name}");
            GUILayout.Label("Press E or LeftMouse Button to release the model and bo back to 'MoveCabNormal' state");
            GUILayout.Label("Press M or ESC to go back to 'Normal' state (will also release the model)");
        }

        public void TrySetModelPosition()
        {
            if (PhysicsUtils.RaycastFromScreen(out RaycastHit hitInfo,
                                               _camera,
                                               _raycastOffset,
                                               _raycastMaxDistance,
                                               _raycastLayers))
            {
                Vector3 hitPosition       = hitInfo.point;
                float distanceFromPlayer  = (hitPosition - transform.position).sqrMagnitude - 1f;
                float entityColliderSizeZ = _moveCabData.Collider.bounds.size.z;
                if (distanceFromPlayer > entityColliderSizeZ)
                {
                    Vector3 hitNormal = hitInfo.normal;
                    float dot         = Vector3.Dot(Vector3.up, hitNormal);
                    if (dot > 0f)
                    {
                        _moveCabData.Transform.position      = hitPosition;
                        _moveCabData.Transform.localRotation = Quaternion.FromToRotation(Vector3.up, hitNormal);

                        if (_useMouseWheelForRotation)
                        {
                            _moveCabData.Transform.RotateAround(hitPosition, hitNormal, _mouseWheelDelta);
                        }
                        else
                        {
                            _moveCabData.Transform.localRotation *= Quaternion.LookRotation(-transform.forward);
                        }
                    }
                    else
                    {
                        Vector3 positionOffset = new Vector3(hitNormal.x, 0f, hitNormal.z) * (entityColliderSizeZ * 0.7f);
                        _moveCabData.Transform.position = new Vector3(hitPosition.x,
                                                                      _moveCabData.Transform.position.y,
                                                                      hitPosition.z)
                                                        + positionOffset;
                        _moveCabData.Transform.localRotation = Quaternion.LookRotation(hitNormal);
                    }
                }
            }
        }
    }
}
