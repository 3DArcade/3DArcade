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

using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace Arcade_r.Player
{
    public class MoveCabNormalState : State
    {
        [SerializeField] private Vector3 _raycastOffset    = Vector3.zero;
        [SerializeField] private float _raycastMaxDistance = 6.0f;
        [SerializeField] private LayerMask _raycastLayers  = 1 << 0;

        [SerializeField] private float _movementSpeedMultiplier = 0.8f;
        [SerializeField] private float _rotationSpeedMultiplier = 2.2f;

        private MoveCabData _data = null;
        private Vector2 _modelPositionOffset;
        private float _modelRotationOffset;

        public override void OnEnter()
        {
            _playerControls.EnableMovement      = true;
            _playerControls.EnableLook          = true;
            _playerControls.EnableInteract      = true;
            _playerControls.EnableToggleMoveCab = true;

            _playerControls.InputActions.FPSMoveCab.Enable();

            _raycastLayers = LayerMask.GetMask("Arcade/GameModels", "Arcade/PropModels");

            if (_data == null)
            {
                _data = Resources.Load<MoveCabData>("ScriptableObjects/MoveCabData");
            }

            Assert.IsNotNull(_data);
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
                _stateController.TransitionTo<NormalState>();
            }

            if (Time.frameCount % 10 == 0)
            {
                FindMovable();
            }

            _modelPositionOffset = _playerControls.InputActions.FPSMoveCab.MoveModel.ReadValue<Vector2>() * _movementSpeedMultiplier;
            _modelRotationOffset = _playerControls.InputActions.FPSMoveCab.RotateModel.ReadValue<float>() * _rotationSpeedMultiplier;

            if (_data.Transform != null)
            {
                if (_playerControls.InputActions.FPSControls.Interact.triggered)
                {
                    _stateController.TransitionTo<MoveCabMovingState>();
                }
            }
        }

        public override void OnFixedUpdate(float dt)
        {
            if (_data.Transform == null || _data.Rigidbody == null)
            {
                return;
            }

            Transform tr = _data.Transform;
            Rigidbody rb = _data.Rigidbody;

            bool performingMove   = _modelPositionOffset.sqrMagnitude > 0.0001f;
            bool performingRotate = _modelRotationOffset > 0.5f || _modelRotationOffset < -0.5f;

            _data.Rigidbody.freezeRotation = performingMove || performingRotate;

            // Position
            if (performingMove)
            {
                rb.AddForce(-tr.forward * _modelPositionOffset.y, ForceMode.VelocityChange);
                rb.AddForce(-tr.right * _modelPositionOffset.x, ForceMode.VelocityChange);
                rb.AddForce(Vector3.right * -rb.velocity.x, ForceMode.VelocityChange);
                rb.AddForce(Vector3.forward * -rb.velocity.z, ForceMode.VelocityChange);
            }

            // Rotation
            if (performingRotate)
            {
                float angle = Mathf.Atan2(tr.forward.x, tr.forward.z) * Mathf.Rad2Deg;
                float targetAngle = angle + _modelRotationOffset;
                float AngleDifference = (targetAngle - angle);

                if (Mathf.Abs(AngleDifference) > 180f)
                {
                    if (AngleDifference < 0f)
                    {
                        AngleDifference = (360f + AngleDifference);
                    }
                    else if (AngleDifference > 0f)
                    {
                        AngleDifference = (360f - AngleDifference) * -1f;
                    }
                }

                rb.AddTorque(Vector3.up * AngleDifference, ForceMode.VelocityChange);
                rb.AddTorque(Vector3.up * -rb.angularVelocity.y, ForceMode.VelocityChange);
            }
        }

        public override void OnExit()
        {
            _playerControls.EnableMovement      = false;
            _playerControls.EnableLook          = false;
            _playerControls.EnableInteract      = false;
            _playerControls.EnableToggleMoveCab = false;

            _playerControls.InputActions.FPSMoveCab.Disable();
        }

        public override void OnDrawDebugUI()
        {
            GUILayout.Label("Current state: MoveCabNormal State");

            GUILayout.Label("\n## Player controls:");
            GUILayout.Label("  Move: WASD, Arrows, LeftStick(GamePad)");
            GUILayout.Label("  Look: Mouse, RFQE, RightStick(GamePad)");
            GUILayout.Label("  Sprint: Shift, ButtonWest(GamePad)");
            GUILayout.Label("  Jump: Space, ButtonNorth(GamePad)");

            GUILayout.Label("\n## State controls:");
            GUILayout.Label("  Move model: IJKL, D-Pad(GamePad)");
            GUILayout.Label("  Rotate model: UO, Left/Right Shoulders(GamePad)");
            GUILayout.Label("  Exit MoveCab mode: M, Select+ButtonNorth(GamePad)");
            GUILayout.Label("  Quit/Exit PlayMode: Esc, Select+Start(GamePad)");

            GUILayout.Label($"\n## Selected Movable: {(_data.Transform != null ? _data.Transform.name : "none")}");
            if (_data.Transform != null)
            {
                GUILayout.Label("  Press Ctrl, LeftMouse Button or ButtonSouth(GamePad) to grab this model and enter 'MoveCabMoving' state");
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
                    _data.Set(hitInfo);
                }
                else
                {
                    _data.Reset();
                }
                StopAllCoroutines();
            }
            else
            {
                _ = StartCoroutine(ResetDataTimeOut());
            }
        }

        private IEnumerator ResetDataTimeOut()
        {
            yield return new WaitForSecondsRealtime(2f);
            _data.Reset();
        }
    }
}
