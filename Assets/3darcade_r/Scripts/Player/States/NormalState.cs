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
            _raycastLayers                      = LayerMask.GetMask("Arcade/ArcadeModels", "Arcade/GameModels", "Arcade/PropModels");
            _playerControls.EnableMovement      = true;
            _playerControls.EnableLook          = true;
            _playerControls.EnableInteract      = true;
            _playerControls.EnableToggleMoveCab = true;
        }

        public override void OnUpdate(float dt)
        {
            if (Time.frameCount % 10 == 0)
            {
                _interactable = FindInteractable();
            }

            if (_playerControls.InputActions.GlobalControls.ToggleMouseCursor.triggered)
            {
                Utils.ToggleMouseCursor();
                _playerControls.EnableLook = !_playerControls.EnableLook;
            }

            if (_playerControls.InputActions.GlobalControls.Quit.triggered)
            {
                Utils.ExitApp();
            }

            if (!Cursor.visible)
            {
                if (_playerControls.InputActions.FPSControls.ToggleMoveCab.triggered)
                {
                    _stateController.TransitionTo<MoveCabNormalState>();
                }

                if (_interactable != null && _playerControls.InputActions.FPSControls.Interact.triggered)
                {
                    Debug.Log("<color=red>It's not implemented :D</color>");
                }
            }
        }

        public override void OnExit()
        {
            _playerControls.EnableMovement      = false;
            _playerControls.EnableLook          = false;
            _playerControls.EnableInteract      = false;
            _playerControls.EnableToggleMoveCab = false;
        }

        public override void OnDrawDebugUI()
        {
            GUILayout.Label("Current state: Normal State");

            GUILayout.Label("\n## Player controls:");
            GUILayout.Label("  Move: WASD, Arrows, LeftStick(GamePad)");
            GUILayout.Label("  Look: Mouse, RFQE, RightStick(GamePad)");
            GUILayout.Label("  Sprint: Shift, ButtonWest(GamePad)");
            GUILayout.Label("  Jump: Space, ButtonNorth(GamePad)");

            GUILayout.Label("\n## State controls:");
            GUILayout.Label("  Enter MoveCab mode: M, Select+ButtonNorth(GamePad)");
            GUILayout.Label("  Quit/Exit PlayMode: Esc, Select+Start(GamePad)");

            GUILayout.Label($"\n## Selected Interactable: {(_interactable != null ? _interactable.name : "none")}");
            if (_interactable != null)
            {
                GUILayout.Label("  <color=red>NOT IMPLEMENTED!</color> Press Ctrl, LeftMouse Button or ButtonSouth(GamePad) to invoke the interactable behavior (ie. start game, launch menu, etc...)");
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
