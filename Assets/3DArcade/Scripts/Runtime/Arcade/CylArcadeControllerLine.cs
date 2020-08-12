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
    public class CylArcadeControllerLine : CylArcadeController
    {
        protected sealed override Transform TransformAnchor => _targetSelection;

        protected override Vector3 TransformVector => Quaternion.Euler(0f, 0f, -Mathf.Clamp(_cylArcadeProperties.LineAngle, -90f, 90f)) * Vector3.right;

        public CylArcadeControllerLine(ArcadeHierarchy arcadeHierarchy,
                                       PlayerFpsControls playerFpsControls,
                                       PlayerCylControls playerCylControls,
                                       Database<EmulatorConfiguration> emulatorDatabase,
                                       AssetCache<GameObject> gameObjectCache,
                                       NodeController<MarqueeNodeTag> marqueeNodeController,
                                       NodeController<ScreenNodeTag> screenNodeController,
                                       NodeController<GenericNodeTag> genericNodeController)
        : base(arcadeHierarchy, playerFpsControls, playerCylControls, emulatorDatabase, gameObjectCache, marqueeNodeController, screenNodeController, genericNodeController)
        {
        }

        protected override float GetSpacing(Transform previousModel, Transform currentModel)
        {
            if (_cylArcadeProperties.LineAngle >= 45f || _cylArcadeProperties.LineAngle <= -45f)
            {
                return GetVerticalSpacing(previousModel, currentModel);
            }
            else
            {
                return GetHorizontalSpacing(previousModel, currentModel);
            }
        }

        protected sealed override bool MoveForwardCondition() => _targetSelection.localPosition != _centerTargetPosition;

        protected sealed override bool MoveBackwardCondition() => MoveForwardCondition();

        protected override void AdjustWheelForward(float dt) => _targetSelection.localPosition = Vector3.MoveTowards(_targetSelection.localPosition, _centerTargetPosition, 10f * dt);

        protected override void AdjustWheelBackward(float dt) => AdjustWheelForward(dt);

        protected sealed override void AdjustModelPosition(Transform model, bool forward, float spacing) => model.Translate(TransformVector * (forward ? spacing : -spacing), Space.World);
    }
}
