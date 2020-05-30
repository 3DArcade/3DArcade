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

namespace Arcade_r
{
    public sealed class ArcadeMoveCabState : ArcadeBaseState
    {
        private readonly MoveCabContext _moveCabContext;

        public ArcadeMoveCabState(ArcadeContext context)
        : base(context)
        {
            _moveCabContext = new MoveCabContext(_context.App.PlayerControls, _context.App.Camera);
        }

        public override void OnEnter()
        {
            Debug.Log(">> <color=green>Entered</color> ArcadeMoveCabState");

            _context.App.PlayerControls.FirstPersonActions.Enable();
            if (Cursor.visible)
            {
                _context.App.PlayerControls.FirstPersonActions.Look.Disable();
            }
            _context.App.PlayerControls.FirstPersonActions.Interact.Disable();

            _context.App.PlayerControls.FirstPersonMoveCabActions.Enable();

            _moveCabContext.TransitionTo<MoveCabAimState>();

            _context.App.UIController.EnableMoveCabUI();
        }

        public override void OnExit()
        {
            Debug.Log(">> <color=orange>Exited</color> ArcadeMoveCabState");

            _context.App.PlayerControls.FirstPersonActions.Disable();
            _context.App.PlayerControls.FirstPersonMoveCabActions.Disable();

            _context.App.UIController.DisableMoveCabUI();
        }

        public override void Update(float dt)
        {
            if (_context.App.PlayerControls.GlobalActions.Quit.triggered)
            {
                _context.TransitionTo<ArcadeLoadState>();
            }

            if (_context.App.PlayerControls.FirstPersonActions.ToggleMoveCab.triggered)
            {
                SaveCurrentArcadeConfiguration(_context);
                _context.TransitionTo<ArcadeNormalState>();
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

            _moveCabContext.Update(dt);
        }

        public override void FixedUpdate(float dt)
        {
            _moveCabContext.FixedUpdate(dt);
        }

        private static void SaveCurrentArcadeConfiguration(ArcadeContext context)
        {
            ArcadeConfigurationComponent cfgComponent = context.App.ArcadeHierarchy.RootNode.GetComponent<ArcadeConfigurationComponent>();
            if (cfgComponent == null)
            {
                return;
            }

            Transform player = context.App.PlayerControls.transform;
            Camera mainCamera = context.App.Camera;
            CinemachineVirtualCamera vCamera = player.GetComponentInChildren<CinemachineVirtualCamera>();

            context.App.ArcadeManager.Save(cfgComponent.ToArcadeConfiguration(player, mainCamera, vCamera));
        }
    }
}
