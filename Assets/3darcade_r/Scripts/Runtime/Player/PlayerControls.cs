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
    public abstract class PlayerControls : MonoBehaviour
    {
        [SerializeField] protected Camera _camera = default;
        [SerializeField] protected CinemachineVirtualCamera _virtualCamera = default;

        [SerializeField] protected float _walkSpeed = 3f;

        [SerializeField] protected float _minVerticalLookAngle = -89f;
        [SerializeField] protected float _maxVerticalLookAngle = 89f;

        public Camera Camera => _camera;
        public CinemachineVirtualCamera VirtualCamera => _virtualCamera;

        public InputSettingsActions.GlobalActions GlobalActions { get; private set; }

        protected CharacterController _characterController;
        protected InputSettingsActions _inputActions;

        protected Vector2 _lookInputValue;

        protected Vector3 _moveVelocity;
        protected float _lookHorizontal;
        protected float _lookVertical;

        protected void Construct()
        {
            _characterController = GetComponent<CharacterController>();
            Assert.IsNotNull(_characterController);
            Assert.IsNotNull(_camera);
            Assert.IsNotNull(_virtualCamera);

            _inputActions = new InputSettingsActions();
            GlobalActions = _inputActions.Global;
        }

        private void OnEnable() => GlobalActions.Enable();

        private void OnDisable() => GlobalActions.Disable();

        protected abstract void GatherMovementInputValues();

        protected abstract void GatherLookInputValues();

        protected abstract void HandleMovement();

        protected abstract void HandleLook();
    }
}
