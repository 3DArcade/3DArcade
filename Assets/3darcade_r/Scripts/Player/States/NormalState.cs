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
    public class NormalState : State
    {
        [SerializeField] private Vector3 _raycastOffset    = Vector3.zero;
        [SerializeField] private float _raycastMaxDistance = 2.5f;
        [SerializeField] private LayerMask _raycastLayers  = 0;

        private Transform _interactable = null;

        public override void OnEnter()
        {
            _raycastLayers                 = LayerMask.GetMask("Arcade/ArcadeModels", "Arcade/GameModels", "Arcade/PropModels");
            _playerControls.EnableMovement = true;
            _playerControls.EnableLook     = true;
        }

        public override void OnUpdate(float dt)
        {
            if (Time.frameCount % 10 == 0)
            {
                _interactable = FindInteractable();
            }

            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                Utils.ToggleMouseCursor();
                _playerControls.EnableLook = !_playerControls.EnableLook;
            }

            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                Utils.ExitApp();
            }

            if (Keyboard.current.mKey.wasPressedThisFrame)
            {
                _stateController.TransitionTo<MoveCabNormalState>();
            }

            if ((Keyboard.current.eKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame) && _interactable != null)
            {
                Debug.Log("<color=red>Like mentioned, it's not implemented :D</color>");
            }
        }

        public override void OnExit()
        {
            _playerControls.EnableMovement = false;
            _playerControls.EnableLook     = false;
        }

        public override void OnDrawDebugUI()
        {
            GUILayout.Label("Current state: Normal State");
            GUILayout.Label("\nPlayer controls: WASD to move, <Mouse> to look around, Shift to sprint, Space to jump, right click to toggle cursor");
            GUILayout.Label("\nPress M to enter 'MoveCab Normal' state");
            GUILayout.Label("Press ESC to quit/exit PlayMode");
            GUILayout.Label($"\nSelected Interactable: {(_interactable != null ? _interactable.name : "none")}");
            if (_interactable != null)
            {
                GUILayout.Label("<color=red>NOT IMPLEMENTED!</color> Press E or LeftMouse Button to invoke the interactable behavior (ie. start game, launch menu, etc...)");
            }
        }

        private Transform FindInteractable()
        {
            if (PhysicsUtils.RaycastFromScreen(out RaycastHit hitInfo, _camera, _raycastOffset, _raycastMaxDistance, _raycastLayers))
            {
                return hitInfo.transform.GetComponent<IInteractable>() != null ? hitInfo.transform : null;
            }

            return null;
        }
    }
}
