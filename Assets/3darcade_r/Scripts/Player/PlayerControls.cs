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

using Cinemachine;
using UnityEngine;
using UnityEngine.Assertions;

namespace Arcade_r
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerControls : MonoBehaviour
    {
        [SerializeField] private float _walkSpeed = 3f;
        [SerializeField] private float _runSpeed  = 6f;
        [SerializeField] private float _jumpForce = 10f;

        [SerializeField] private float _minVerticalLookAngle = -89f;
        [SerializeField] private float _maxVerticalLookAngle = 89f;
        [SerializeField] private float _extraGravity         = 40f;

        public InputSettingsActions.GlobalActions GlobalActions { get; private set; }
        public InputSettingsActions.FirstPersonActions FirstPersonActions { get; private set; }
        public InputSettingsActions.FirstPersonMoveCabActions FirstPersonMoveCabActions { get; private set; }
        public bool InputModifierIsDown_TEMP { get; private set; }

        private CharacterController _characterController;
        private CinemachineVirtualCamera _camera;

        private InputSettingsActions _inputActions;

        private Vector2 _movementInputValue;
        private Vector2 _lookInputValue;
        private bool _sprinting;
        private bool _performJump;

        private Vector3 _moveVelocity;
        private float _lookHorizontal;
        private float _lookVertical;

        private void Awake()
        {
            _characterController= GetComponent<CharacterController>();
            _camera             = GetComponentInChildren<CinemachineVirtualCamera>();

            Assert.IsNotNull(_characterController);
            Assert.IsNotNull(_camera);

            _inputActions             = new InputSettingsActions();
            GlobalActions             = _inputActions.Global;
            FirstPersonActions        = _inputActions.FirstPerson;
            FirstPersonMoveCabActions = _inputActions.FirstPersonMoveCab;
        }

        private void OnEnable()
        {
            GlobalActions.Enable();
        }

        private void OnDisable()
        {
            GlobalActions.Disable();
        }

        private void Update()
        {
            InputModifierIsDown_TEMP = GlobalActions.TempModifierWorkaround.ReadValue<float>() > 0.5f;

            if (FirstPersonActions.Movement.enabled)
            {
                GatherMovementInputValues();
            }
            HandleMovement();

            if (FirstPersonActions.Look.enabled)
            {
                GatherLookInputValues();
                HandleLook();
            }
        }

        private void GatherMovementInputValues()
        {
            _movementInputValue = FirstPersonActions.Movement.ReadValue<Vector2>();
            if (!InputModifierIsDown_TEMP)
            {
                _sprinting   = FirstPersonActions.Sprint.ReadValue<float>() > 0f;
                _performJump = FirstPersonActions.Jump.triggered;
            }
        }

        private void GatherLookInputValues()
        {
            _lookInputValue = !InputModifierIsDown_TEMP ? FirstPersonActions.Look.ReadValue<Vector2>() : Vector2.zero;
        }

        private void HandleMovement()
        {
            if (_characterController.isGrounded)
            {
                _moveVelocity = new Vector3(_movementInputValue.x, -0.1f, _movementInputValue.y);
                _moveVelocity.Normalize();

                float speed = _sprinting ? _runSpeed : _walkSpeed;
                _moveVelocity = transform.TransformDirection(_moveVelocity) * speed;

                if (_performJump)
                {
                    _moveVelocity.y = _jumpForce;
                }
            }

            if ((_characterController.collisionFlags & CollisionFlags.Above) != 0 && _moveVelocity.y > 0f)
            {
                _moveVelocity.y -= _moveVelocity.y;
            }

            _moveVelocity.y -= _extraGravity * Time.deltaTime;
            _ = _characterController.Move(_moveVelocity * Time.deltaTime);
        }

        private void HandleLook()
        {
            _lookHorizontal = _lookInputValue.x;
            _lookVertical  += _lookInputValue.y;
            _lookVertical   = Mathf.Clamp(_lookVertical, _minVerticalLookAngle, _maxVerticalLookAngle);
            if (_camera != null)
            {
                _camera.transform.localEulerAngles = new Vector3(-_lookVertical, 0f, 0f);
            }
            transform.Rotate(new Vector3(0f, _lookHorizontal, 0f));
        }
    }
}
