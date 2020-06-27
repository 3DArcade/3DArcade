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
using System.Linq;
using UnityEngine;

namespace Arcade_r
{
    public sealed class ArcadeCylCameraInsideController : ArcadeCylController
    {
        public ArcadeCylCameraInsideController(ArcadeHierarchy arcadeHierarchy,
                                               PlayerFpsControls playerFpsControls,
                                               PlayerCylControls playerCylControls,
                                               Database<EmulatorConfiguration> emulatorDatabase,
                                               AssetCache<GameObject> gameObjectCache,
                                               AssetCache<Texture> textureCache,
                                               AssetCache<string> videoCache)
        : base(arcadeHierarchy, playerFpsControls, playerCylControls, emulatorDatabase, gameObjectCache, textureCache, videoCache)
        {
        }

        public override void Forward(int count, float dt)
        {
            _playerCylControls.StopAllCoroutines();
            _ = _playerCylControls.StartCoroutine(CoRotateLeft(count, dt));
        }

        public override void Backward(int count, float dt)
        {
            _playerCylControls.StopAllCoroutines();
            _ = _playerCylControls.StartCoroutine(CoRotateRight(count, dt));
        }

        protected override void SetupWheel()
        {
            if (_allGames.Count < 1)
            {
                return;
            }

            Transform previousModel;
            float angle = 0f;
            for (int i = _selectionIndex; i < _sprockets; ++i)
            {
                Transform currentModel = _allGames[i];
                currentModel.gameObject.SetActive(true);
                currentModel.SetPositionAndRotation(_centerTargetPosition, Quaternion.Euler(_cylArcadeProperties.SprocketRotation));

                if (i > _selectionIndex)
                {
                    previousModel  = _allGames[i - 1];
                    float spacing  = previousModel.gameObject.GetHalfWidth() + currentModel.gameObject.GetHalfWidth() + _cylArcadeProperties.ModelSpacing;
                    angle         += (spacing / _cylArcadeProperties.WheelRadius) * Mathf.Rad2Deg;
                    currentModel.RotateAround(Vector3.zero, Vector3.up, angle);
                }
            }

            previousModel = _allGames[_selectionIndex];
            angle         = 0f;
            for (int i = _selectionIndex - 1; i >= 0; --i)
            {
                Transform currentModel = _allGames[i];
                currentModel.gameObject.SetActive(true);
                currentModel.SetPositionAndRotation(_centerTargetPosition, Quaternion.Euler(_cylArcadeProperties.SprocketRotation));

                if (i < _selectionIndex - 1)
                {
                    previousModel = _allGames[i + 1];
                }

                float spacing  = previousModel.gameObject.GetHalfWidth() + currentModel.gameObject.GetHalfWidth() + _cylArcadeProperties.ModelSpacing;
                angle         -= (spacing / _cylArcadeProperties.WheelRadius) * Mathf.Rad2Deg;
                currentModel.RotateAround(Vector3.zero, Vector3.up, angle);
            }

            foreach (Transform model in _allGames.Skip(_sprockets))
            {
                model.gameObject.SetActive(false);
                model.localPosition = Vector3.zero;
            }
        }

        private IEnumerator CoRotateLeft(int count, float dt)
        {
            Transform targetSelection = _allGames[_selectionIndex + count];

            while (targetSelection.localPosition.x > 0f && targetSelection.localPosition.z < _centerTargetPosition.z)
            {
                for (int j = 0; j < _sprockets; ++j)
                {
                    _allGames[j].RotateAround(Vector3.zero, Vector3.up, -20f * dt);
                }

                yield return null;
            }

            targetSelection.localPosition = _centerTargetPosition;

            for (int i = 0; i < count; ++i)
            {
                _allGames.RotateLeft();
                SetupWheel();
            }
        }

        private IEnumerator CoRotateRight(int count, float dt)
        {
            Transform targetSelection = _allGames[_selectionIndex - count];

            while (targetSelection.localPosition.x < 0f && targetSelection.localPosition.z < _centerTargetPosition.z)
            {
                for (int j = 0; j < _sprockets; ++j)
                {
                    _allGames[j].RotateAround(Vector3.zero, Vector3.up, 20f * dt);
                }

                yield return null;
            }

            targetSelection.localPosition = _centerTargetPosition;

            for (int i = 0; i < count; ++i)
            {
                _allGames.RotateRight();
                SetupWheel();
            }
        }
    }
}
