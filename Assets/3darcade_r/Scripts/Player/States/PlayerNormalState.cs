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

namespace Arcade_r
{
    public class PlayerNormalState : PlayerState
    {
        private readonly LayerMask _raycastLayers;
        private readonly float _raycastMaxDistance = 2.5f;

        private ModelSetup _currentModelSetup;

        public PlayerNormalState(PlayerStateContext stateContext)
        : base(stateContext)
        {
            _raycastLayers = LayerMask.GetMask("Arcade/ArcadeModels", "Arcade/GameModels", "Arcade/PropModels");
        }

        public override void OnEnter()
        {
            Debug.Log("<color=green>Entered</color> PlayerNormalState");

            _stateContext.PlayerControls.InputActions.FPSControls.Enable();
            if (Cursor.visible)
            {
                _stateContext.PlayerControls.InputActions.FPSControls.Look.Disable();
            }
        }

        public override void OnExit()
        {
            Debug.Log("<color=orange>Exited</color> PlayerNormalState");

            _stateContext.PlayerControls.InputActions.FPSControls.Disable();
        }

        public override void OnUpdate(float dt)
        {
            if (Time.frameCount % 10 == 0)
            {
                GetCurrentModelSetup();
            }

            if (_stateContext.PlayerControls.InputActions.GlobalControls.ToggleMouseCursor.triggered)
            {
                Utils.ToggleMouseCursor();
                if (!Cursor.visible)
                {
                    _stateContext.PlayerControls.InputActions.FPSControls.Look.Enable();
                }
                else
                {
                    _stateContext.PlayerControls.InputActions.FPSControls.Look.Disable();
                }
            }

            if (_stateContext.PlayerControls.InputActions.GlobalControls.Quit.triggered)
            {
                Utils.ExitApp();
            }

            if (!Cursor.visible)
            {
                if (_stateContext.PlayerControls.InputActions.FPSControls.Interact.triggered)
                {
                    if (_currentModelSetup != null && _currentModelSetup is Interaction.IInteractable interactable)
                    {
                        interactable.OnInteract();
                    }
                }
            }

            if (_stateContext.PlayerControls.InputActions.FPSControls.ToggleMoveCab.triggered)
            {
                _stateContext.TransitionTo<PlayerMoveCabState>();
            }
        }

        private void GetCurrentModelSetup()
        {
            Ray ray = _stateContext.Camera.ScreenPointToRay(new Vector2(Screen.width * 0.5f, Screen.height * 0.5f));
            if (Physics.Raycast(ray, out RaycastHit hitInfo, _raycastMaxDistance, _raycastLayers))
            {
                ModelSetup targetModel = hitInfo.transform.GetComponent<ModelSetup>();
                if (targetModel != _currentModelSetup)
                {
                    _currentModelSetup = targetModel;
                }
            }
            else
            {
                _currentModelSetup = null;
            }
        }
    }
}
