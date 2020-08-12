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
    public sealed class CylArcadeControllerLineVertical : CylArcadeControllerLine
    {
        protected override Vector3 TransformVector => Vector3.down;

        public CylArcadeControllerLineVertical(ArcadeHierarchy arcadeHierarchy,
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

        protected override float GetSpacing(Transform previousModel, Transform currentModel) => GetVerticalSpacing(previousModel, currentModel);
    }
}
