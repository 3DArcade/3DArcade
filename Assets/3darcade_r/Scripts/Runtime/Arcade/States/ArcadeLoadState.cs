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
    public sealed class ArcadeLoadState : ArcadeBaseState
    {
        public ArcadeLoadState(ArcadeContext context)
        : base(context)
        {
        }

        public override void OnEnter()
        {
            Debug.Log("> <color=green>Entered</color> ArcadeLoadState");

            SystemUtils.HideMouseCursor();

            ArcadeConfiguration arcadeConfiguration = _context.App.ArcadeManager.Get(_context.CurrentArcadeId);
            if (arcadeConfiguration == null)
            {
                return;
            }

            _context.App.ArcadeHierarchy.Reset();
            ArcadeController.StartArcade(arcadeConfiguration, _context.App.ArcadeHierarchy.RootNode.transform, _context.App.PlayerControls.transform);

            _context.TransitionTo<ArcadeNormalState>();
        }

        public override void OnExit()
        {
            Debug.Log("> <color=orange>Exited</color> ArcadeLoadState");
        }
    }
}
