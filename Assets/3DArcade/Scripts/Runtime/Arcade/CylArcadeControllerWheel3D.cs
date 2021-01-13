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
    public abstract class CylArcadeControllerWheel3D : CylArcadeController
    {
        protected sealed override Transform TransformAnchor => _pivotPoint;

        protected Transform _pivotPoint;

        public CylArcadeControllerWheel3D(ArcadeHierarchy arcadeHierarchy,
                                          PlayerFpsControls playerFpsControls,
                                          PlayerCylControls playerCylControls,
                                          Database<EmulatorConfiguration> emulatorDatabase,
                                          AssetCache<GameObject> gameObjectCache,
                                          NodeController<MarqueeNodeTag> marqueeNodeController,
                                          NodeController<ScreenNodeTag> screenNodeController,
                                          NodeController<GenericNodeTag> genericNodeController)
        : base(arcadeHierarchy, playerFpsControls, playerCylControls, emulatorDatabase, gameObjectCache, marqueeNodeController, screenNodeController, genericNodeController)
            => _pivotPoint = new GameObject(CYLARCADE_PIVOT_POINT_GAMEOBJECT_NAME).transform;

        protected sealed override void AdjustWheelForward(float dt) => _pivotPoint.Rotate(-TransformVector * 20f * dt, Space.Self);

        protected sealed override void AdjustWheelBackward(float dt) => _pivotPoint.Rotate(TransformVector * 20f * dt, Space.Self);

        protected sealed override void AdjustModelPosition(Transform model, bool forward, float spacing)
        {
            float angle = spacing / _cylArcadeProperties.WheelRadius;
            model.RotateAround(_pivotPoint.transform.position, TransformVector, (forward ? angle : -angle) * Mathf.Rad2Deg);
        }
    }
}
