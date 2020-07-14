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

namespace Arcade_r
{
    public abstract class CylArcadeControllerWheel3DCameraOutside : CylArcadeControllerWheel3D
    {
        public CylArcadeControllerWheel3DCameraOutside(ArcadeHierarchy arcadeHierarchy,
                                                       PlayerFpsControls playerFpsControls,
                                                       PlayerCylControls playerCylControls,
                                                       Database<EmulatorConfiguration> emulatorDatabase,
                                                       AssetCache<GameObject> gameObjectCache,
                                                       AssetCache<Texture> textureCache,
                                                       AssetCache<string> videoCache)
        : base(arcadeHierarchy, playerFpsControls, playerCylControls, emulatorDatabase, gameObjectCache, textureCache, videoCache)
        {
        }

        protected sealed override void LateSetupWorld()
        {
            base.LateSetupWorld();

            _pivotPoint.localPosition = _centerTargetPosition + new Vector3(0f, 0f, _cylArcadeProperties.WheelRadius);
        }

        protected sealed override void RotateWheelForward(float dt) => RotateWheel(false, dt);

        protected sealed override void RotateWheelBackward(float dt) => RotateWheel(true, dt);

        protected sealed override void AdjustModelPosition(Transform model, bool forward, float spacing)
        {
            float angle = GetAngle(spacing);
            model.RotateAround(_pivotPoint.transform.localPosition, TransformVector, (forward ? -angle : angle) * Mathf.Rad2Deg);
        }
    }
}
