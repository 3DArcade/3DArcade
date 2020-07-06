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
    public sealed class CylArcadeControllerCameraOutsideHorizontal : CylArcadeControllerCameraOutside
    {
        public CylArcadeControllerCameraOutsideHorizontal(ArcadeHierarchy arcadeHierarchy,
                                                          PlayerFpsControls playerFpsControls,
                                                          PlayerCylControls playerCylControls,
                                                          Database<EmulatorConfiguration> emulatorDatabase,
                                                          AssetCache<GameObject> gameObjectCache,
                                                          AssetCache<Texture> textureCache,
                                                          AssetCache<string> videoCache)
        : base(arcadeHierarchy, playerFpsControls, playerCylControls, emulatorDatabase, gameObjectCache, textureCache, videoCache)
        {
        }

        protected override void SetupWheel()
        {
            if (_allGames.Count < 1)
            {
                return;
            }

            Transform firstModel = _allGames[_selectionIndex];
            firstModel.gameObject.SetActive(true);
            firstModel.SetPositionAndRotation(_centerTargetPosition, Quaternion.Euler(_cylArcadeProperties.SprocketRotation));

            for (int i = _selectionIndex + 1; i < _sprockets; ++i)
            {
                Transform previousModel = _allGames[i - 1];

                Transform currentModel = _allGames[i];
                currentModel.gameObject.SetActive(true);
                currentModel.SetPositionAndRotation(previousModel.localPosition, previousModel.localRotation);

                float spacing = previousModel.GetHalfWidth() + currentModel.GetHalfWidth() + _cylArcadeProperties.ModelSpacing;
                float angle   = spacing / _cylArcadeProperties.WheelRadius;
                currentModel.RotateAround(_pivotPoint.transform.localPosition, Vector3.up, -angle * Mathf.Rad2Deg);
            }

            for (int i = _selectionIndex - 1; i >= 0; --i)
            {
                Transform previousModel = _allGames[i + 1];

                Transform currentModel = _allGames[i];
                currentModel.gameObject.SetActive(true);
                currentModel.SetPositionAndRotation(previousModel.localPosition, previousModel.localRotation);

                float spacing = previousModel.GetHalfWidth() + currentModel.GetHalfWidth() + _cylArcadeProperties.ModelSpacing;
                float angle   = spacing / _cylArcadeProperties.WheelRadius;
                currentModel.RotateAround(_pivotPoint.transform.localPosition, Vector3.up, angle * Mathf.Rad2Deg);
            }

            foreach (Transform model in _allGames.Skip(_sprockets))
            {
                model.gameObject.SetActive(false);
                model.localPosition = Vector3.zero;
            }
        }

        protected override IEnumerator CoNavigateForward(float dt)
        {
            _animating = true;

            Transform targetSelection = _allGames[_selectionIndex + 1];

            ParentGamesToWheel();

            while (targetSelection.position.x > 0f || targetSelection.position.z > _pivotPoint.transform.position.z)
            {
                _pivotPoint.Rotate(Vector3.up * 20f * dt, Space.Self);
                yield return null;
            }

            ResetGamesParent();

            _allGames.RotateLeft();

            UpdateWheel();

            _animating = false;
        }

        protected override IEnumerator CoNavigateBackward(float dt)
        {
            _animating = true;

            Transform targetSelection = _allGames[_selectionIndex - 1];

            ParentGamesToWheel();

            while (targetSelection.position.x < 0f || targetSelection.position.z > _pivotPoint.transform.position.z)
            {
                _pivotPoint.Rotate(-Vector3.up * 20f * dt, Space.Self);
                yield return null;
            }

            ResetGamesParent();

            _allGames.RotateRight();

            UpdateWheel();

            _animating = false;
        }

        protected override void UpdateWheel()
        {
            if (_allGames.Count < 1)
            {
                return;
            }

            Transform previousModel = _allGames[_sprockets - 2];
            Transform newModel      = _allGames[_sprockets - 1];
            newModel.gameObject.SetActive(true);
            newModel.SetPositionAndRotation(previousModel.localPosition, previousModel.localRotation);
            float spacing = previousModel.GetHalfWidth() + newModel.GetHalfWidth() + _cylArcadeProperties.ModelSpacing;
            float angle   = (spacing / _cylArcadeProperties.WheelRadius) * Mathf.Rad2Deg;
            newModel.RotateAround(_pivotPoint.transform.localPosition, -Vector3.up, angle);

            previousModel = _allGames[1];
            newModel      = _allGames[0];
            newModel.gameObject.SetActive(true);
            newModel.SetPositionAndRotation(previousModel.localPosition, previousModel.localRotation);
            spacing = previousModel.GetHalfWidth() + newModel.GetHalfWidth() + _cylArcadeProperties.ModelSpacing;
            angle   = (spacing / _cylArcadeProperties.WheelRadius) * Mathf.Rad2Deg;
            newModel.RotateAround(_pivotPoint.transform.localPosition, Vector3.up, angle);

            foreach (Transform model in _allGames.Skip(_sprockets))
            {
                model.gameObject.SetActive(false);
                model.localPosition = Vector3.zero;
            }

            CurrentGame = _allGames[_selectionIndex].GetComponent<ModelConfigurationComponent>();
        }
    }
}
