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
    [DisallowMultipleComponent]
    public sealed class Main : MonoBehaviour
    {
        [SerializeField] private PlayerFpsControls _playerFpsControls = default;
        [SerializeField] private PlayerCylControls _playerCylControls = default;
        [SerializeField] private Transform _uiRoot                    = default;
        [SerializeField] private GameObject _theAbyss                 = default;

        private ArcadeContext _arcadeContext;
        private bool _badLuck = false;

#if !UNITY_EDITOR
        private bool _focused;
        private void OnApplicationFocus(bool focus) => _focused = focus;
#endif
        private void Awake()
        {
            Assert.IsNotNull(_playerFpsControls);
            Assert.IsNotNull(_playerCylControls);
            Assert.IsNotNull(_uiRoot);
            Assert.IsNotNull(_theAbyss);

            Application.targetFrameRate = -1;
            Time.timeScale              = 1f;

            _arcadeContext = new ArcadeContext(_playerFpsControls, _playerCylControls, _uiRoot);
        }

        private void Start()
        {
            SystemUtils.HideMouseCursor();

            _arcadeContext.TransitionTo<ArcadeLoadState>();
        }

        private void Update()
        {
 #if !UNITY_EDITOR
            if (!_focused)
            {
                System.Threading.Thread.Sleep(200);
                return;
            }
#endif
            _arcadeContext.Update(Time.deltaTime);

            YouAreNotSupposedToBeHere();
        }

        private void FixedUpdate()
        {
            _arcadeContext.FixedUpdate(Time.fixedDeltaTime);
        }

        private void YouAreNotSupposedToBeHere()
        {
            if (!_badLuck && _playerFpsControls.transform.position.y < -340f)
            {
                _playerFpsControls.transform.position = new Vector3(0f, _playerFpsControls.transform.position.y, 0f);
                _ = Instantiate(_theAbyss);
                _badLuck = true;
            }
        }
    }
}
