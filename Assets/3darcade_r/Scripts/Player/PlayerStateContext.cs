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
using UnityEngine.Assertions;

namespace Arcade_r
{
    public class PlayerStateContext : StateContext
    {
        public PlayerControls PlayerControls { get; private set; }
        public Camera Camera { get; private set; }

        public PlayerStateContext(PlayerControls playerControls, Camera camera)
        {
            PlayerControls = playerControls;
            Camera         = camera;
        }

        public override void TransitionTo<T>()
        {
            State foundState = _allStates.Find(x => x.GetType() == typeof(T));
            if (foundState is T foundPlayerState)
            {
                if (_currentState != foundPlayerState)
                {
                    _currentState?.OnExit();
                    _currentState = foundPlayerState;
                    _currentState.OnEnter();
                }
            }
            else
            {
                T newPlayerState = System.Activator.CreateInstance(typeof(T), new object[] { this }) as T;
                Assert.IsTrue(newPlayerState is PlayerState);
                _allStates.Add(newPlayerState);
                TransitionTo<T>();
            }
        }
    }
}
