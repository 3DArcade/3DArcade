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

namespace Arcade_r
{
    public sealed class ArcadeCylNormalState : ArcadeState
    {
        private InputAction _navigationInput;

        private float _timer        = 0f;
        private float _acceleration = 1f;

        public ArcadeCylNormalState(ArcadeContext context)
        : base(context)
        {
        }

        public override void OnEnter()
        {
            Debug.Log($"> <color=green>Entered</color> {GetType().Name}");

            _context.PlayerCylControls.CylArcadeActions.Enable();
            if (!_context.PlayerCylControls.MouseLookEnabled)
            {
                _context.PlayerCylControls.CylArcadeActions.Look.Disable();
            }

            _context.UIController.EnableNormalUI();

            _context.CurrentPlayerControls = _context.PlayerCylControls;

            _context.VideoPlayerController.SetPlayer(_context.PlayerCylControls.transform);

            switch (_context.CurrentArcadeConfiguration.CylArcadeProperties.WheelVariant)
            {
                case WheelVariant.CameraInsideHorizontal:
                case WheelVariant.CameraOutsideHorizontal:
                case WheelVariant.LineHorizontal:
                {
                    _navigationInput = _context.PlayerCylControls.CylArcadeActions.NavigationLeftRight;
                    _context.PlayerCylControls.SetupForHorizontalWheel();
                }
                break;
                case WheelVariant.CameraInsideVertical:
                case WheelVariant.CameraOutsideVertical:
                case WheelVariant.LineVertical:
                {
                    _navigationInput = _context.PlayerCylControls.CylArcadeActions.NavigationUpDown;
                    _context.PlayerCylControls.SetupForVerticalWheel();
                }
                break;
                case WheelVariant.LineCustom:
                {
                    if (_context.CurrentArcadeConfiguration.CylArcadeProperties.HorizontalNavigation)
                    {
                        _navigationInput = _context.PlayerCylControls.CylArcadeActions.NavigationLeftRight;
                        _context.PlayerCylControls.SetupForHorizontalWheel();
                    }
                    else
                    {
                        _navigationInput = _context.PlayerCylControls.CylArcadeActions.NavigationUpDown;
                        _context.PlayerCylControls.SetupForVerticalWheel();
                    }
                }
                break;
            }

            UpdateCurrentInteractable();
        }

        public override void OnExit()
        {
            Debug.Log($"> <color=orange>Exited</color> {GetType().Name}");

            _context.PlayerCylControls.CylArcadeActions.Disable();

            _context.UIController.DisableNormalUI();
        }

        bool _videoWorkaroundApplied = false;
        int _frames = 0;

        public override void Update(float dt)
        {
            if (!_videoWorkaroundApplied)
            {
                if (++_frames >= 60)
                {
                    UpdateCurrentInteractable();
                    _videoWorkaroundApplied = true;
                }
            }

            if (_context.PlayerCylControls.GlobalActions.Quit.triggered)
            {
                SystemUtils.ExitApp();
            }

            if (_context.PlayerCylControls.GlobalActions.ToggleCursor.triggered)
            {
                SystemUtils.ToggleMouseCursor();
                if (_context.PlayerCylControls.MouseLookEnabled)
                {
                    if (!Cursor.visible)
                    {
                        _context.PlayerCylControls.CylArcadeActions.Look.Enable();
                    }
                    else
                    {
                        _context.PlayerCylControls.CylArcadeActions.Look.Disable();
                    }
                }
            }

            HandleNavigation(dt);

            if (_context.CurrentModelConfiguration != _context.ArcadeController.CurrentGame)
            {
                UpdateCurrentInteractable();
            }

            if (!Cursor.visible && _context.PlayerCylControls.CylArcadeActions.Interact.triggered)
            {
                HandleInteraction();
            }
        }

        private void UpdateCurrentInteractable()
        {
            InteractionController.FindInteractable(ref _context.CurrentModelConfiguration, _context.ArcadeController);
            _context.VideoPlayerController.UpdateVideosStateCyl(_context.CurrentModelConfiguration.transform.position);
        }

        private void HandleNavigation(float dt)
        {
            if (_navigationInput.phase != InputActionPhase.Started)
            {
                return;
            }

            float direction = _navigationInput.ReadValue<float>();

            if (_navigationInput.triggered)
            {
                _timer = 0f;
                _acceleration = 1f;
                if (direction > 0f)
                {
                    if (_context.CurrentArcadeConfiguration.CylArcadeProperties.InverseNavigation)
                    {
                        _context.ArcadeController.NavigateBackward(dt);
                    }
                    else
                    {
                        _context.ArcadeController.NavigateForward(dt);
                    }
                }
                else if (direction < 0f)
                {
                    if (_context.CurrentArcadeConfiguration.CylArcadeProperties.InverseNavigation)
                    {
                        _context.ArcadeController.NavigateForward(dt);
                    }
                    else
                    {
                        _context.ArcadeController.NavigateBackward(dt);
                    }
                }
            }
            else if ((_timer += _acceleration * dt) > 1.0f)
            {
                _acceleration += 0.5f;
                _acceleration  = Mathf.Clamp(_acceleration, 1f, 20f);
                if (direction > 0f)
                {
                    if (_context.CurrentArcadeConfiguration.CylArcadeProperties.InverseNavigation)
                    {
                        _context.ArcadeController.NavigateBackward(_acceleration * dt);
                    }
                    else
                    {
                        _context.ArcadeController.NavigateForward(_acceleration * dt);
                    }
                }
                else if (direction < 0f)
                {
                    if (_context.CurrentArcadeConfiguration.CylArcadeProperties.InverseNavigation)
                    {
                        _context.ArcadeController.NavigateForward(_acceleration * dt);
                    }
                    else
                    {
                        _context.ArcadeController.NavigateBackward(_acceleration * dt);
                    }
                }
                _timer = 0f;
            }
        }

        private void HandleInteraction()
        {
            if (_context.CurrentModelConfiguration == null)
            {
                return;
            }

            //if (_context.CurrentModelConfiguration.Grabbable)
            //{
            //    _context.TransitionTo<ArcadeGrabState>();
            //}
            //else
            {
                switch (_context.CurrentModelConfiguration.InteractionType)
                {
                    case InteractionType.GameInternal:
                    {
                        _context.TransitionTo<ArcadeInternalGameState>();
                    }
                    break;
                    case InteractionType.GameExternal:
                    {
                        _context.TransitionTo<ArcadeExternalGameState>();
                    }
                    break;
                    case InteractionType.FpsArcadeConfiguration:
                    {
                        _context.SetAndStartCurrentArcadeConfiguration(_context.CurrentModelConfiguration.Id, ArcadeType.Fps);
                    }
                    break;
                    case InteractionType.CylArcadeConfiguration:
                    {
                        _context.SetAndStartCurrentArcadeConfiguration(_context.CurrentModelConfiguration.Id, ArcadeType.Cyl);
                    }
                    break;
                    case InteractionType.FpsMenuConfiguration:
                    case InteractionType.CylMenuConfiguration:
                    case InteractionType.URL:
                    case InteractionType.None:
                    default:
                        break;
                }
            }
        }
    }
}
