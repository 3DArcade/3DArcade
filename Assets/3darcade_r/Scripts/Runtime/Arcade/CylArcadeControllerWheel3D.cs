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
    public abstract class CylArcadeControllerWheel3D : CylArcadeController
    {
        protected Transform _pivotPoint;
        protected Vector3 _rotationVector;

        public CylArcadeControllerWheel3D(ArcadeHierarchy arcadeHierarchy,
                                          PlayerFpsControls playerFpsControls,
                                          PlayerCylControls playerCylControls,
                                          Database<EmulatorConfiguration> emulatorDatabase,
                                          AssetCache<GameObject> gameObjectCache,
                                          AssetCache<Texture> textureCache,
                                          AssetCache<string> videoCache)
        : base(arcadeHierarchy, playerFpsControls, playerCylControls, emulatorDatabase, gameObjectCache, textureCache, videoCache)
        {
        }

        protected abstract bool MoveForwardCondition(Transform targetSelection);

        protected abstract bool MoveBackwardCondition(Transform targetSelection);

        protected abstract void RotateWheelForward(float dt);

        protected abstract void RotateWheelBackward(float dt);

        protected override void LateSetupWorld()
        {
            base.LateSetupWorld();

            _pivotPoint = new GameObject(CYLARCADE_PIVOT_POINT_GAMEOBJECT_NAME).transform;
        }

        protected sealed override IEnumerator CoNavigateForward(float dt)
        {
            _animating = true;

            Transform targetSelection = _allGames[_selectionIndex + 1];

            ParentGamesToWheel();

            while (MoveForwardCondition(targetSelection))
            {
                RotateWheelForward(dt);
                yield return null;
            }

            ResetGamesParent();

            _allGames.RotateLeft();

            UpdateWheel();

            _animating = false;
        }

        protected sealed override IEnumerator CoNavigateBackward(float dt)
        {
            _animating = true;

            Transform targetSelection = _allGames[_selectionIndex - 1];

            ParentGamesToWheel();

            while (MoveBackwardCondition(targetSelection))
            {
                RotateWheelBackward(dt);
                yield return null;
            }

            ResetGamesParent();

            _allGames.RotateRight();

            UpdateWheel();

            _animating = false;
        }

        protected void ParentGamesToWheel()
        {
            foreach (Transform game in _allGames.Take(_sprockets))
            {
                game.SetParent(_pivotPoint);
            }
        }

        protected void RotateWheel(bool forward, float dt)
        {
            _pivotPoint.Rotate((forward ? -_rotationVector : _rotationVector) * 20f * dt, Space.Self);
        }

        protected void ResetGamesParent()
        {
            foreach (Transform game in _allGames.Take(_sprockets))
            {
                game.SetParent(_arcadeHierarchy.GamesNode);
            }
        }
    }
}
