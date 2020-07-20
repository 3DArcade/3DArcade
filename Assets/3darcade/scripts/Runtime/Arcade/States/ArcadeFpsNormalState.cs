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

namespace Arcade
{
    public sealed class ArcadeFpsNormalState : ArcadeState
    {
        private const float INTERACT_MAX_DISTANCE = 2.5f;

        public ArcadeFpsNormalState(ArcadeContext context)
        : base(context)
        {
        }

        public override void OnEnter()
        {
            Debug.Log($"> <color=green>Entered</color> {GetType().Name}");

            _context.PlayerFpsControls.FpsArcadeActions.Enable();
            if (Cursor.visible)
            {
                _context.PlayerFpsControls.FpsArcadeActions.Look.Disable();
            }

            _context.CurrentModelConfiguration = null;

            _context.UIController.EnableNormalUI();

            _context.CurrentPlayerControls = _context.PlayerFpsControls;

            _context.VideoPlayerController.SetPlayer(_context.PlayerFpsControls.transform);
        }

        public override void OnExit()
        {
            Debug.Log($"> <color=orange>Exited</color> {GetType().Name}");

            _context.PlayerFpsControls.FpsArcadeActions.Disable();

            _context.UIController.DisableNormalUI();
        }

        public override void Update(float dt)
        {
            if (_context.PlayerFpsControls.GlobalActions.Quit.triggered)
            {
                SystemUtils.ExitApp();
            }

            if (_context.PlayerFpsControls.GlobalActions.ToggleCursor.triggered)
            {
                SystemUtils.ToggleMouseCursor();
                if (!Cursor.visible)
                {
                    _context.PlayerFpsControls.FpsArcadeActions.Look.Enable();
                }
                else
                {
                    _context.PlayerFpsControls.FpsArcadeActions.Look.Disable();
                }
            }

            InteractionController.FindInteractable(ref _context.CurrentModelConfiguration,
                                                   _context.PlayerFpsControls.Camera,
                                                   INTERACT_MAX_DISTANCE,
                                                   _context.RaycastLayers);

            _context.VideoPlayerController.UpdateVideosStateFps();

            if (!Cursor.visible && _context.PlayerFpsControls.FpsArcadeActions.Interact.triggered)
            {
                HandleInteraction();
            }

            if (_context.PlayerFpsControls.FpsArcadeActions.ToggleMoveCab.triggered)
            {
                _context.TransitionTo<ArcadeFpsMoveCabState>();
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
