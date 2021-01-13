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

namespace Arcade
{
    [DisallowMultipleComponent]
    public sealed class UIController
    {
        private readonly GameObject _loadingUI;
        private readonly GameObject _normalUI;
        private readonly GameObject _moveCabUI;

        public UIController(Transform uiRoot)
        {
            UILoadingCanvasTag loadingCanvasTag = uiRoot.GetComponentInChildren<UILoadingCanvasTag>(true);
            if (loadingCanvasTag != null)
                _loadingUI = loadingCanvasTag.gameObject;

            UINormalCanvasTag normalCanvasTag = uiRoot.GetComponentInChildren<UINormalCanvasTag>(true);
            if (normalCanvasTag != null)
                _normalUI = normalCanvasTag.gameObject;

            UIMoveCabCanvasTag moveCabCanvasTag = uiRoot.GetComponentInChildren<UIMoveCabCanvasTag>(true);
            if (moveCabCanvasTag != null)
                _moveCabUI = moveCabCanvasTag.gameObject;

            DisableLoadingUI();
            DisableNormalUI();
            DisableMoveCabUI();
        }

        public void EnableLoadingUI()
        {
            if (_loadingUI != null)
                _loadingUI.SetActive(true);
        }

        public void DisableLoadingUI()
        {
            if (_loadingUI != null)
                _loadingUI.SetActive(false);
        }

        public void EnableNormalUI()
        {
            if (_normalUI != null)
                _normalUI.SetActive(true);
        }

        public void DisableNormalUI()
        {
            if (_normalUI != null)
                _normalUI.SetActive(false);
        }

        public void EnableMoveCabUI()
        {
            if (_moveCabUI != null)
                _moveCabUI.SetActive(true);
        }

        public void DisableMoveCabUI()
        {
            if (_moveCabUI != null)
                _moveCabUI.SetActive(false);
        }
    }
}
