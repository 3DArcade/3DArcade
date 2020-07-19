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
    public class CylArcadeControllerLine : CylArcadeController
    {
        protected override Vector3 TransformVector => Quaternion.Euler(0f, 0f, -Mathf.Clamp(_cylArcadeProperties.LineAngle, -90f, 90f)) * Vector3.right;

        public CylArcadeControllerLine(ArcadeHierarchy arcadeHierarchy,
                                       PlayerFpsControls playerFpsControls,
                                       PlayerCylControls playerCylControls,
                                       Database<EmulatorConfiguration> emulatorDatabase,
                                       AssetCache<GameObject> gameObjectCache,
                                       AssetCache<Texture> textureCache,
                                       AssetCache<string> videoCache)
        : base(arcadeHierarchy, playerFpsControls, playerCylControls, emulatorDatabase, gameObjectCache, textureCache, videoCache)
        {
        }

        protected sealed override IEnumerator CoNavigateForward(float dt)
        {
            _animating = true;

            Transform targetSelection = _allGames[_selectionIndex + 1];

            ParentGamesToSelection(targetSelection);

            while (targetSelection.localPosition != _centerTargetPosition)
            {
                targetSelection.localPosition = Vector3.MoveTowards(targetSelection.localPosition, _centerTargetPosition, 10f * dt);
                yield return null;
            }

            ResetGamesParent(targetSelection);

            _allGames.RotateLeft();

            UpdateWheel();

            _animating = false;
        }

        protected sealed override IEnumerator CoNavigateBackward(float dt)
        {
            _animating = true;

            Transform targetSelection = _allGames[_selectionIndex - 1];

            ParentGamesToSelection(targetSelection);

            while (targetSelection.localPosition != _centerTargetPosition)
            {
                targetSelection.localPosition = Vector3.MoveTowards(targetSelection.localPosition, _centerTargetPosition, 10f * dt);
                yield return null;
            }

            ResetGamesParent(targetSelection);

            _allGames.RotateRight();

            UpdateWheel();

            _animating = false;
        }

        protected sealed override void AdjustModelPosition(Transform model, bool forward, float spacing) => model.Translate(TransformVector * (forward ? spacing : -spacing), Space.World);

        protected override float GetSpacing(Transform previousModel, Transform currentModel)
        {
            if (_cylArcadeProperties.LineAngle >= 45f)
            {
                return GetVerticalSpacing(previousModel, currentModel);
            }
            else if (_cylArcadeProperties.LineAngle <= -45f)
            {
                return GetVerticalSpacing(previousModel, currentModel);
            }
            else
            {
                return GetHorizontalSpacing(previousModel, currentModel);
            }
        }

        protected void ParentGamesToSelection(Transform targetSelection)
        {
            foreach (Transform game in _allGames.Take(_sprockets)
                                                .Where(t => t != targetSelection))
            {
                game.SetParent(targetSelection);
            }
        }

        protected void ResetGamesParent(Transform targetSelection)
        {
            foreach (Transform game in _allGames.Take(_sprockets)
                                                .Where(t => t != targetSelection))
            {
                game.SetParent(_arcadeHierarchy.GamesNode);
            }
        }
    }
}
