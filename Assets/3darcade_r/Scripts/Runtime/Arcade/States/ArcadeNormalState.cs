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
    public sealed class ArcadeNormalState : ArcadeState
    {
        private const float INTERACT_MAX_DISTANCE = 2.5f;

        public ArcadeNormalState(ArcadeContext context)
        : base(context)
        {
        }

        public override void OnEnter()
        {
            Debug.Log("> <color=green>Entered</color> ArcadeNormalState");

            _context.App.PlayerControls.FirstPersonActions.Enable();
            if (Cursor.visible)
            {
                _context.App.PlayerControls.FirstPersonActions.Look.Disable();
            }

            _context.CurrentModelConfiguration = null;

            _context.App.UIController.EnableNormalUI();
        }

        public override void OnExit()
        {
            Debug.Log("> <color=orange>Exited</color> ArcadeNormalState");

            _context.App.PlayerControls.FirstPersonActions.Disable();

            _context.App.UIController.DisableNormalUI();
        }

        public override void Update(float dt)
        {
            if (_context.App.PlayerControls.GlobalActions.Quit.triggered)
            {
                SystemUtils.ExitApp();
            }

            if (_context.App.PlayerControls.GlobalActions.ToggleCursor.triggered)
            {
                SystemUtils.ToggleMouseCursor();
                if (!Cursor.visible)
                {
                    _context.App.PlayerControls.FirstPersonActions.Look.Enable();
                }
                else
                {
                    _context.App.PlayerControls.FirstPersonActions.Look.Disable();
                }
            }

            if (Time.frameCount % 10 == 0)
            {
                InteractionController.FindInteractable(ref _context.CurrentModelConfiguration,
                                                       _context.App.Camera,
                                                       INTERACT_MAX_DISTANCE,
                                                       _context.RaycastLayers);
            }

            if (!Cursor.visible && _context.App.PlayerControls.FirstPersonActions.Interact.triggered)
            {
                HandleInteraction();
            }

            if (_context.App.PlayerControls.FirstPersonActions.ToggleMoveCab.triggered)
            {
                _context.TransitionTo<ArcadeMoveCabState>();
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
                    case InteractionType.Internal:
                    {
                        _context.TransitionTo<ArcadeInternalGameState>();
                    }
                    break;
                    case InteractionType.External:
                    {
                        _context.TransitionTo<ArcadeExternalGameState>();
                    }
                    break;
                    case InteractionType.MenuConfiguration:
                    {
                        _ = _context.SetAndStartCurrentArcadeConfiguration(_context.CurrentModelConfiguration.Id);
                    }
                    break;
                    case InteractionType.URL:
                    case InteractionType.None:
                    default:
                        break;
                }
            }
        }
    }
}
