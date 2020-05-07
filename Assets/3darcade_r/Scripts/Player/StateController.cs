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

namespace Arcade_r.Player
{
    [RequireComponent(typeof(PlayerControls))]
    public class StateController : MonoBehaviour
    {
        private readonly List<State> _allStates = new List<State>();
        private State _currentState;

        private void Start()
        {
            TransitionTo<NormalState>();
        }

        private void Update()
        {
            if (_currentState != null)
            {
                _currentState.OnUpdate(Time.deltaTime);
            }
        }
        private void OnGUI()
        {
            DrawCurrentStateDebugUI();
        }

        public void TransitionTo<T>() where T : State
        {
            State newState = _allStates.Find(x => x.GetType() == typeof(T));
            if (newState != null)
            {
                if (_currentState != newState)
                {
                    if (_currentState != null)
                    {
                        _currentState.OnExit();
                    }
                    _currentState = newState;
                    _currentState.OnEnter();
                }
            }
            else
            {
                InitState<T>();
            }
        }

        public void DrawCurrentStateDebugUI()
        {
            if (_currentState != null)
            {
                _currentState.OnDrawDebugUI();
            }
        }

        private void InitState<T>() where T : State
        {
            GameObject obj              = new GameObject(typeof(T).Name);
            obj.transform.parent        = transform;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localScale    = Vector3.one;

            T newState = obj.AddComponent<T>();
            if (!_allStates.Contains(newState))
            {
                _allStates.Add(newState);
            }

            TransitionTo<T>();
        }
    }
}
