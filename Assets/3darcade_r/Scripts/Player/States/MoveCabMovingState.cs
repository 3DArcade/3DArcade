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
using UnityEngine.Assertions;

namespace Arcade_r.Player
{
    public class MoveCabMovingState : State
    {
        [SerializeField] private float _raycastMaxDistance = 12f;
        [SerializeField] private LayerMask _raycastLayers  = 1 << 0;

        private MoveCabData _data = null;

        private bool _savedColliderTrigger = false;
        private RigidbodyInterpolation _savedRigidbodyInterpolation;
        private CollisionDetectionMode _savedRigidbodyMode;
        private bool _savedRigidbodyKinematic = false;

        private Vector3 _screenPoint;

        public override void OnEnter()
        {
            _playerControls.EnableMovement      = true;
            _playerControls.EnableLook          = true;
            _playerControls.EnableInteract      = true;
            _playerControls.EnableToggleMoveCab = true;

            _raycastLayers = LayerMask.GetMask("Arcade/ArcadeModels");

            if (_data == null)
            {
                _data = Resources.Load<MoveCabData>("ScriptableObjects/MoveCabData");
            }

            Assert.IsNotNull(_data);
            Assert.IsNotNull(_data.Transform);
            Assert.IsNotNull(_data.Collider);
            Assert.IsNotNull(_data.Rigidbody);

            _savedColliderTrigger        = _data.Collider.isTrigger;
            _savedRigidbodyInterpolation = _data.Rigidbody.interpolation;
            _savedRigidbodyMode          = _data.Rigidbody.collisionDetectionMode;
            _savedRigidbodyKinematic     = _data.Rigidbody.isKinematic;

            _data.Collider.isTrigger               = true;
            _data.Rigidbody.interpolation          = RigidbodyInterpolation.Interpolate;
            _data.Rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            _data.Rigidbody.isKinematic            = true;

            _screenPoint = _camera.WorldToScreenPoint(_data.Transform.position);
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

            if (_playerControls.InputActions.FPSControls.Interact.triggered)
            {
                _stateController.TransitionTo<MoveCabNormalState>();
            }

            TrySetModelPosition();
        }

        public override void OnExit()
        {
            _playerControls.EnableMovement      = false;
            _playerControls.EnableLook          = false;
            _playerControls.EnableInteract      = false;
            _playerControls.EnableToggleMoveCab = false;

            _data.Collider.isTrigger               = _savedColliderTrigger;
            _data.Rigidbody.isKinematic            = _savedRigidbodyKinematic;
            _data.Rigidbody.interpolation          = _savedRigidbodyInterpolation;
            _data.Rigidbody.collisionDetectionMode = _savedRigidbodyMode;
        }

        public override void OnDrawDebugUI()
        {
            GUILayout.Label("Current state: MoveCabMoving State");

            GUILayout.Label("\n## Player controls:");
            GUILayout.Label("  Move: WASD, Arrows, LeftStick(GamePad)");
            GUILayout.Label("  Look: Mouse, RFQE, RightStick(GamePad)");
            GUILayout.Label("  Sprint: Shift, ButtonWest(GamePad)");
            GUILayout.Label("  Jump: Space, ButtonNorth(GamePad)");

            GUILayout.Label("\n## State controls:");
            GUILayout.Label("  Leave: M, Select+ButtonNorth(GamePad)");

            GUILayout.Label($"\n## Selected Movable: {_data.Transform.name}");
            GUILayout.Label("  Release Model: Ctrl, LeftButton(Mouse), ButtonSouth(GamePad)");
        }

        public void TrySetModelPosition()
        {
            Ray ray = _camera.ScreenPointToRay(_screenPoint);
            if (!Physics.Raycast(ray, out RaycastHit hitInfo, _raycastMaxDistance, _raycastLayers))
            {
                return;
            }

            Vector3 hitPosition = hitInfo.point;
            Vector3 hitNormal   = hitInfo.normal;
            float dot           = Vector3.Dot(Vector3.up, hitNormal);
            if (dot > 0.05f)
            {
                _data.Transform.position      = Vector3.Lerp(_data.Transform.position, hitPosition, Time.deltaTime * 12f);
                _data.Transform.localRotation = Quaternion.FromToRotation(Vector3.up, hitNormal)
                                              * Quaternion.LookRotation(-transform.forward);
            }
            else
            {
                Vector3 positionOffset        = new Vector3(hitNormal.x, 0f, hitNormal.z) * (_data.Collider.bounds.size.z * 0.7f);
                Vector3 newPosition           = new Vector3(hitPosition.x, _data.Transform.position.y, hitPosition.z) + positionOffset;
                _data.Transform.position      = Vector3.Lerp(_data.Transform.position, newPosition, Time.deltaTime * 12f);
                _data.Transform.localRotation = Quaternion.LookRotation(hitNormal);
            }
        }
    }
}
