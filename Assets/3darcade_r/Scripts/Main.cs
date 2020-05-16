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
    public class Main : MonoBehaviour
    {
        [SerializeField] private PlayerControls _player;
        [SerializeField] private Camera _camera;
        [SerializeField] private GameObject _theAbyss;

        private ApplicationStateContext _applicationContext;

        private void Awake()
        {
            Assert.IsNotNull(_player);
            Assert.IsNotNull(_camera);
            Assert.IsNotNull(_theAbyss);

            _player.gameObject.SetActive(true);
            _camera.gameObject.SetActive(true);

            _applicationContext = new ApplicationStateContext(_player, _camera, _theAbyss);
        }

        private void Start()
        {
            SystemUtils.HideMouseCursor();

            _applicationContext.TransitionTo<ApplicationInitState>();
        }

        private void Update()
        {
            _applicationContext.Update(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            _applicationContext.FixedUpdate(Time.fixedDeltaTime);
        }
    }
}
