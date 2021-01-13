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

namespace Arcade
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class PlayerCylControls : PlayerControls
    {
        [SerializeField] private float _minHorizontalLookAngle = -40f;
        [SerializeField] private float _maxHorizontalLookAngle = 40f;

        public bool MouseLookEnabled;

        public InputSettingsActions.CylArcadeActions CylArcadeActions { get; private set; }

        private InputAction _movementInputAction;
        private float _movementInputValue;

        private void Awake()
        {
            Construct();
            CylArcadeActions     = _inputActions.CylArcade;
            _movementInputAction = _inputActions.CylArcade.NavigationLeftRight;
        }

        private void Update()
        {
            if (_movementInputAction.enabled)
                GatherMovementInputValues();
            HandleMovement(Time.deltaTime);

            if (MouseLookEnabled)
                GatherLookInputValues();
                HandleLook();
        }

        public void SetupForHorizontalWheel() => _movementInputAction = _inputActions.CylArcade.NavigationUpDown;

        public void SetupForVerticalWheel() => _movementInputAction = _inputActions.CylArcade.NavigationLeftRight;

        public void SetHorizontalLookLimits(float min, float max)
        {
            _minHorizontalLookAngle = Mathf.Clamp(min, -90f, 0f);
            _maxHorizontalLookAngle = Mathf.Clamp(max, 0f, 90f);
        }

        protected override void GatherMovementInputValues() => _movementInputValue = _movementInputAction.ReadValue<float>();

        protected override void GatherLookInputValues() => _lookInputValue = _inputActions.CylArcade.Look.ReadValue<Vector2>();

        protected override void HandleMovement(float dt)
        {
            _ = _characterController.Move(new Vector3(0f, 0f, _movementInputValue) * _walkSpeed * dt);

            if (transform.localPosition.z < 0f)
                transform.localPosition = new Vector3(0f, transform.localPosition.y, 0f);
        }

        protected override void HandleLook()
        {
            _lookHorizontal += _lookInputValue.x;
            _lookVertical   += _lookInputValue.y;
            _lookHorizontal = Mathf.Clamp(_lookHorizontal, _minHorizontalLookAngle, _maxHorizontalLookAngle);
            _lookVertical   = Mathf.Clamp(_lookVertical, _minVerticalLookAngle, _maxVerticalLookAngle);
            if (_virtualCamera != null)
                _virtualCamera.transform.localEulerAngles = new Vector3(-_lookVertical, 0f, 0f);
            transform.localEulerAngles = new Vector3(0f, _lookHorizontal, 0f);
        }
    }
}
