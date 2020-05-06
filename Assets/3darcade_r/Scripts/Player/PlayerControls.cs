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
using UnityEngine.InputSystem;

namespace Arcade_r.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerControls : MonoBehaviour
    {
        public bool EnableMovement{ get; set; }
        public bool EnableLook { get; set; }

        [SerializeField] private float _walkSpeed = 2f;
        [SerializeField] private float _runSpeed  = 4f;
        [SerializeField] private float _jumpForce = 10f;

        [SerializeField] private float _lookSensitivityHorizontal = 3f;
        [SerializeField] private float _lookSensitivityVertical   = 2f;

        [SerializeField] private float _minVerticalLookAngle = -89f;
        [SerializeField] private float _maxVerticalLookAngle = 89f;
        [SerializeField] private float _extraGravity         = 40f;

        private CharacterController _characterController = null;
        private CinemachineVirtualCamera _camera = null;

        private Vector3 _moveVelocity = Vector3.zero;
        private float _lookHorizontal = 0f;
        private float _lookVertical = 0f;

        private float _movementInputValueX;
        private float _movementInputValueY;
        private float _lookInputValueH;
        private float _lookInputValueV;
        private bool _sprinting;
        private bool _performJump;

        private GameObject _theAbyss;
        private bool _badLuck;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _camera              = GetComponentInChildren<CinemachineVirtualCamera>();
            _theAbyss            = Resources.Load<GameObject>("Misc/TheAbyss");
        }

        private void Update()
        {
            if (EnableMovement)
            {
                GatherMoveInputValues();
            }
            HandleMovement();

            if (EnableLook)
            {
                GatherLookInputValues();
                HandleLook();
            }

            YouAreNotSupposedToBeHere();
        }

        private void HandleLook()
        {
            _lookHorizontal = _lookSensitivityHorizontal * _lookInputValueH;
            _lookVertical  += _lookSensitivityVertical * _lookInputValueV;
            _lookVertical   = Mathf.Clamp(_lookVertical, _minVerticalLookAngle, _maxVerticalLookAngle);
            if (_camera != null)
            {
                _camera.transform.localEulerAngles = new Vector3(-_lookVertical, 0f, 0f);
            }
            transform.Rotate(new Vector3(0f, _lookHorizontal, 0f));
        }

        private void HandleMovement()
        {
            if (_characterController.isGrounded)
            {
                _moveVelocity = new Vector3(_movementInputValueX, -0.1f, _movementInputValueY);
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

        private void GatherMoveInputValues()
        {
            _movementInputValueX = Input.GetAxisRaw("Horizontal");
            _movementInputValueY = Input.GetAxisRaw("Vertical");
            _sprinting           = Keyboard.current.leftShiftKey.isPressed;
            _performJump         = Keyboard.current.spaceKey.wasPressedThisFrame;
        }

        private void GatherLookInputValues()
        {
            _lookInputValueH = Input.GetAxis("Mouse X");
            _lookInputValueV = Input.GetAxis("Mouse Y");
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
