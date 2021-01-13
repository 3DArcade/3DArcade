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

namespace Arcade
{
    [RequireComponent(typeof(Light))]
    public class LightColorCycling : MonoBehaviour
    {
        [SerializeField] private Color[] _colors    = new [] { Color.red, Color.green, Color.blue };
        [SerializeField] private float _cycleLength = 4.0f;

        private Light _light;
        private int _currentIndex;
        private int _nextIndex;
        private float _timer;

        private void Awake() => _light = GetComponent<Light>();

        private void Start()
        {
            Assert.IsTrue(_colors.Length >= 2);
            _nextIndex = (_currentIndex + 1) % _colors.Length;
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer > _cycleLength)
            {
                _currentIndex = (_currentIndex + 1) % _colors.Length;
                _nextIndex   = (_currentIndex + 1) % _colors.Length;
                _timer = 0.0f;
            }
            _light.color = Color.Lerp(_colors[_currentIndex], _colors[_nextIndex], _timer / _cycleLength);
        }
    }
}
