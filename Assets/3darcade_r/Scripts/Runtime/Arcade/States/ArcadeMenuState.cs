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

using System.Collections.Generic;
using UnityEngine;

namespace Arcade_r
{
    public sealed class ArcadeMenuState : ArcadeState
    {
        private const float RAYCAST_MAX_DISTANCE = 12f;
        private readonly LayerMask _layerMask;

        private Stack<ArcadeConfiguration> _menuStack;
        private ArcadeConfiguration _currentArcadeConfiguration;
        private ModelConfigurationComponent _currentModelConfiguration;

        public ArcadeMenuState(ArcadeContext context)
        : base(context)
        {
            _layerMask = LayerMask.GetMask("Menu/GameModels");
        }

        public override void OnEnter()
        {
            Debug.Log("> <color=green>Entered</color> ArcadeMenuState");

            _menuStack = new Stack<ArcadeConfiguration>();

            _currentArcadeConfiguration = _context.App.ArcadeDatabase.Get(_context.CurrentModelConfiguration.Id);
            _menuStack.Push(_currentArcadeConfiguration);
        }

        public override void OnExit()
        {
            Debug.Log("> <color=orange>Exited</color> ArcadeMenuState");
        }

        public override void Update(float dt)
        {
            if (Time.frameCount % 10 == 0)
            {
                InteractionController.FindInteractable(ref _currentModelConfiguration, _context.App.Camera, RAYCAST_MAX_DISTANCE, _layerMask);
            }
        }
    }
}
