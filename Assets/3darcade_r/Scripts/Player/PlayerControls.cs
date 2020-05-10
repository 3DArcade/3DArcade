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

        public bool EnableMovement
        {
            get => InputActions.FPSControls.Movement.enabled;
            set
            {
                if (value)
                {
                    InputActions.FPSControls.Movement.Enable();
                    InputActions.FPSControls.Sprint.Enable();
                    InputActions.FPSControls.Jump.Enable();
                }
                else
                {
                    InputActions.FPSControls.Movement.Disable();
                    InputActions.FPSControls.Sprint.Disable();
                    InputActions.FPSControls.Jump.Disable();
                }
            }
        }

        public bool EnableLook
        {
            get => InputActions.FPSControls.Look.enabled;
            set
            {
                if (value)
                {
                    InputActions.FPSControls.Look.Enable();
                }
                else
                {
                    InputActions.FPSControls.Look.Disable();
                }
            }
        }

        public bool EnableInteract
        {
            get => InputActions.FPSControls.Interact.enabled;
            set
            {
                if (value)
                {
                    InputActions.FPSControls.Interact.Enable();
                }
                else
                {
                    InputActions.FPSControls.Interact.Disable();
                }
            }
        }

        public bool EnableToggleMoveCab
        {
            get => InputActions.FPSControls.ToggleMoveCab.enabled;
            set
            {
                if (value)
                {
                    InputActions.FPSControls.ToggleMoveCab.Enable();
                }
                else
                {
                    InputActions.FPSControls.ToggleMoveCab.Disable();
                }
            }
        }

        public InputSettingsActions InputActions { get; private set; }

        private CharacterController _characterController = null;
        private CinemachineVirtualCamera _camera = null;

        private Vector2 _movementInputValue;
        private Vector2 _lookInputValue;
        private bool _sprinting;
        private bool _performJump;

        private Vector3 _moveVelocity = Vector3.zero;
        private float _lookHorizontal;
        private float _lookVertical;

        private GameObject _theAbyss;
        private bool _badLuck;

        private bool _modifierIsDown_TEMP;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _camera              = GetComponentInChildren<CinemachineVirtualCamera>();
            InputActions         = new InputSettingsActions();
            _theAbyss            = Resources.Load<GameObject>("Misc/TheAbyss");

            Assert.IsNotNull(_characterController);
            Assert.IsNotNull(_camera);
            Assert.IsNotNull(InputActions);
            Assert.IsNotNull(_theAbyss);
        }

        private void OnEnable()
        {
            InputActions.GlobalControls.Enable();
        }

        private void OnDisable()
        {
            InputActions.GlobalControls.Disable();
        }

        private void Update()
        {
            _modifierIsDown_TEMP = InputActions.GlobalControls.TempModifierWorkaround.ReadValue<float>() > 0.5f;

            if (EnableMovement)
            {
                GatheMovementInputValues();
            }
            HandleMovement();

            if (EnableLook)
            {
                GatherLookInputValues();
                HandleLook();
            }

            YouAreNotSupposedToBeHere();
        }

        private void GatheMovementInputValues()
        {
            _movementInputValue = InputActions.FPSControls.Movement.ReadValue<Vector2>();
            _sprinting          = !_modifierIsDown_TEMP && InputActions.FPSControls.Sprint.ReadValue<float>() > 0f;
            _performJump        = !_modifierIsDown_TEMP && InputActions.FPSControls.Jump.triggered;
        }

        private void GatherLookInputValues()
        {
            _lookInputValue = !_modifierIsDown_TEMP ? InputActions.FPSControls.Look.ReadValue<Vector2>() : Vector2.zero;
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

        private void YouAreNotSupposedToBeHere()
        {
            if (!_badLuck && transform.position.y < -340f)
            {
                transform.position = new Vector3(0f, transform.position.y, 0f);
                _ = Instantiate(_theAbyss);
                _badLuck = true;
            }
        }
    }
}
