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
    public abstract class CylArcadeControllerFlat : CylArcadeController
    {
        public CylArcadeControllerFlat(ArcadeHierarchy arcadeHierarchy,
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
            if (!_animating)
            {
                _ = _playerCylControls.StartCoroutine(CoNavigate(true, dt));
            }
        }

        public override void Backward(int count, float dt)
        {
            if (!_animating)
            {
                _ = _playerCylControls.StartCoroutine(CoNavigate(false, dt));
            }
        }

        protected abstract void UpdateWheel();

        private IEnumerator CoNavigate(bool forward, float dt)
        {
            _animating = true;

            int targetIndex           = forward ? _selectionIndex + 1 : _selectionIndex - 1;
            Transform targetSelection = _allGames[targetIndex];

            foreach (Transform game in _allGames.Take(_sprockets)
                                                .Where(t => t != targetSelection))
            {
                game.SetParent(targetSelection);
            }

            while (targetSelection.localPosition != _centerTargetPosition)
            {
                targetSelection.localPosition = Vector3.MoveTowards(targetSelection.localPosition, _centerTargetPosition, 10f * dt);
                yield return null;
            }

            foreach (Transform game in _allGames.Take(_sprockets).Where(t => t != targetSelection))
            {
                game.SetParent(_arcadeHierarchy.GamesNode);
            }

            if (forward)
            {
                _allGames.RotateLeft();
            }
            else
            {
                _allGames.RotateRight();
            }

            UpdateWheel();

            _animating = false;
        }
    }
}
