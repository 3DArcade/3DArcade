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

using System.Collections.Generic;
using UnityEngine;

namespace Arcade
{
    public sealed class ScreenNodeController : NodeController
    {
        protected override string[] DefaultImageDirectories => _defaultImageDirectories;
        protected override string[] DefaultVideoDirectories => _defaultVideoDirectories;

        private readonly string[] _defaultImageDirectories = new string[] { $"{_defaultMediaDirectory}/Screens", $"{_defaultMediaDirectory}/Titles" };
        private readonly string[] _defaultVideoDirectories = new string[] { $"{_defaultMediaDirectory}/ScreensVideo" };

        public ScreenNodeController(ArcadeController arcadeController, AssetCache<Texture> textureCache)
        : base(arcadeController, textureCache)
        {
        }

        protected override void PostSetup(Renderer renderer, Texture texture, float emissionIntensity)
        {
            ArtworkController.SetupStaticImage(renderer.material, texture, true, true, emissionIntensity);
        }

        protected override Renderer GetNodeRenderer(GameObject model) => GetNodeRenderer<ScreenNodeTag>(model);

        protected override string[] GetModelImageDirectories(ModelConfiguration modelConfiguration)
        {
            List<string> result = new List<string>();

            if (modelConfiguration.ScreenImageDirectories != null)
            {
                result.AddRange(modelConfiguration.ScreenImageDirectories);
            }

            if (modelConfiguration.TitleImageDirectories != null)
            {
                result.AddRange(modelConfiguration.TitleImageDirectories);
            }

            return result.Count > 0 ? result.ToArray() : null;
        }

        protected override string[] GetModelVideoDirectories(ModelConfiguration modelConfiguration) => modelConfiguration?.ScreenVideoDirectories;

        protected override string[] GetEmulatorImageDirectories(EmulatorConfiguration emulator)
        {
            List<string> result = new List<string>();

            if (emulator.ScreenImagesDirectories != null)
            {
                result.AddRange(emulator.ScreenImagesDirectories);
            }

            if (emulator.TitleImagesDirectories != null)
            {
                result.AddRange(emulator.TitleImagesDirectories);
            }

            return result.Count > 0 ? result.ToArray() : null;
        }

        protected override string[] GetEmulatorVideoDirectories(EmulatorConfiguration emulator) => emulator?.ScreenVideosDirectories;
    }
}
