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
    public sealed class FpsArcadeController : ArcadeController
    {
        public override bool VideoPlayOnAwake => true;
        public override float AudioMinDistance { get; protected set; }
        public override float AudioMaxDistance { get; protected set; }
        public override AnimationCurve VolumeCurve { get; protected set; }

        protected override bool UseModelTransfoms => true;
        protected override PlayerControls PlayerControls => _playerFpsControls;
        protected override CameraSettings CameraSettings => _arcadeConfiguration.FpsArcadeProperties.CameraSettings;

        public FpsArcadeController(ArcadeHierarchy arcadeHierarchy,
                                   PlayerFpsControls playerFpsControls,
                                   PlayerCylControls playerCylControls,
                                   Database<EmulatorConfiguration> emulatorDatabase,
                                   AssetCache<GameObject> gameObjectCache,
                                   AssetCache<Texture> textureCache,
                                   AssetCache<string> videoCache)
        : base(arcadeHierarchy, playerFpsControls, playerCylControls, emulatorDatabase, gameObjectCache, textureCache, videoCache)
        {
            AudioMinDistance = 1f;
            AudioMaxDistance = 3f;

            VolumeCurve = new AnimationCurve(new Keyframe[]
            {
                 new Keyframe(0.8f,                     1.0f, -2.6966875f,  -2.6966875f,  0.2f,        0.10490462f),
                 new Keyframe(AudioMaxDistance * 0.5f, 0.3f, -0.49866775f, -0.49866775f, 0.28727788f, 0.2f),
                 new Keyframe(AudioMaxDistance,        0.0f, -0.08717632f, -0.08717632f, 0.5031141f,  0.2f)
            });
        }

        protected override void PreSetupPlayer()
        {
            _playerFpsControls.gameObject.SetActive(true);
            _playerCylControls.gameObject.SetActive(false);
        }
    }
}
