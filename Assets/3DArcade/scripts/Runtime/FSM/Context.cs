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

namespace Arcade.FSM
{
    public abstract class Context<T> where T : State<T>
    {
        private readonly List<T> _states;
        private T _currentState;

        public Context()
        {
            _states       = new List<T>();
            _currentState = null;
        }

        public void Update(float dt) => _currentState?.Update(dt);

        public void FixedUpdate(float dt) => _currentState?.FixedUpdate(dt);

        public void TransitionTo<U>() where U : T
        {
            T foundState = _states.Find(x => x.GetType() == typeof(U));
            if (foundState != null)
            {
                if (_currentState != foundState)
                {
                    _currentState?.OnExit();
                    _currentState = foundState;
                    _currentState.OnEnter();
                }
            }
            else
            {
                U newState = System.Activator.CreateInstance(typeof(U), new object[] { this }) as U;
                _states.Add(newState);
                TransitionTo<U>();
            }
        }
    }
}
