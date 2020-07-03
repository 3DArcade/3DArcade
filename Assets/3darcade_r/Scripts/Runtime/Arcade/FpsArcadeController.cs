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
using UnityEngine.Assertions;

namespace Arcade_r
{
    public sealed class FpsArcadeController : ArcadeController
    {
        public FpsArcadeController(ArcadeHierarchy arcadeHierarchy,
                                   PlayerFpsControls playerFpsControls,
                                   PlayerCylControls playerCylControls,
                                   Database<EmulatorConfiguration> emulatorDatabase,
                                   AssetCache<GameObject> gameObjectCache,
                                   AssetCache<Texture> textureCache,
                                   AssetCache<string> videoCache)
        : base(arcadeHierarchy, playerFpsControls, playerCylControls, emulatorDatabase, gameObjectCache, textureCache, videoCache)
        {
        }

        public override bool StartArcade(ArcadeConfiguration arcadeConfiguration)
        {
            Assert.IsNotNull(arcadeConfiguration);

            _playerFpsControls.gameObject.SetActive(true);
            _playerCylControls.gameObject.SetActive(false);

            SetupPlayer(_playerFpsControls, arcadeConfiguration.FpsArcadeProperties.CameraSettings);

            SetupWorld(arcadeConfiguration);

            return true;
        }

        protected override void SetupWorld(ArcadeConfiguration arcadeConfiguration)
        {
            RenderSettings renderSettings = arcadeConfiguration.RenderSettings;
            AddModelsToWorld(arcadeConfiguration.ArcadeModelList, _arcadeHierarchy.ArcadesNode, renderSettings, ARCADE_RESOURCES_DIRECTORY, ContentMatcher.GetNamesToTryForArcade);
            AddModelsToWorld(arcadeConfiguration.GameModelList, _arcadeHierarchy.GamesNode, renderSettings, GAME_RESOURCES_DIRECTORY, ContentMatcher.GetNamesToTryForGame);
            AddModelsToWorld(arcadeConfiguration.PropModelList, _arcadeHierarchy.PropsNode, renderSettings, PROP_RESOURCES_DIRECTORY, ContentMatcher.GetNamesToTryForProp);
        }
    }
}
