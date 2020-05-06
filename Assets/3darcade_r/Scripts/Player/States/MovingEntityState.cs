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

using UnityEngine.InputSystem;

namespace Arcade_r.Player
{
    public class MovingEntityState : State
    {
        public MovingEntityState(StateContext stateContext, PlayerControls playerControls, PlayerInteractions playerInteractions)
        : base(stateContext, playerControls, playerInteractions)
        {
        }

        public override void OnEnter()
        {
            if (_playerInteractions.globalData.CurrentEntityTransform != null)
            {
                if (!_playerInteractions.globalData.CurrentEntityRigidBody.isKinematic)
                {
                    _playerInteractions.globalData.CurrentEntityRigidBody.isKinematic = true;
                }
            }

            _playerControls.EnableMovement = true;
            _playerControls.EnableLook     = true;
        }

        public override void OnUpdate(float dt)
        {
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                Utils.ToggleMouseCursor();
                _playerControls.EnableLook = !_playerControls.EnableLook;
            }

            if (Keyboard.current.gKey.wasPressedThisFrame)
            {
                _stateContext.TransitionTo<NormalState>();
            }

            _playerInteractions.TryMoveEntityToWorldPoint();
        }

        public override void OnExit()
        {
            if (_playerInteractions.globalData.CurrentEntityTransform != null)
            {
                if (_playerInteractions.globalData.CurrentEntityRigidBody.isKinematic)
                {
                    _playerInteractions.globalData.CurrentEntityRigidBody.isKinematic = false;
                }
            }

            _playerControls.EnableMovement = false;
            _playerControls.EnableLook     = false;
        }
    }
}
