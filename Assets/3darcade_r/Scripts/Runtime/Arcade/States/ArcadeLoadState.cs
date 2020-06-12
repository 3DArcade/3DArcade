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

using System.Collections;
using UnityEngine;

namespace Arcade_r
{
    public sealed class ArcadeLoadState : ArcadeState
    {
        private bool _loaded;

        public ArcadeLoadState(ArcadeContext context)
        : base(context)
        {
        }

        public override void OnEnter()
        {
            Debug.Log($"> <color=green>Entered</color> {GetType().Name}");

            _loaded = false;

            SystemUtils.HideMouseCursor();

            _context.App.UIController.EnableLoadingUI();

            _context.App.StopCoroutine(CoStartCurrentArcade());
            _ = _context.App.StartCoroutine(CoStartCurrentArcade());
        }

        public override void OnExit()
        {
            Debug.Log($"> <color=orange>Exited</color> {GetType().Name}");

            _context.App.UIController.DisableLoadingUI();
        }

        public override void Update(float dt)
        {
            if (_loaded)
            {
                _context.TransitionTo<ArcadeNormalState>();
            }
        }

        private IEnumerator CoStartCurrentArcade()
        {
            yield return new WaitUntil(() => _context.StartCurrentArcade());
            _loaded = true;
        }
    }
}
