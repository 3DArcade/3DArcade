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
using System.Linq;
using UnityEngine;

namespace Arcade
{
    public sealed class MarqueeNodeController : NodeController
    {
        protected override string DefaultImageDirectory => $"{_defaultMediaDirectory}/Marquees";
        protected override string DefaultVideoDirectory => $"{_defaultMediaDirectory}/MarqueesVideo";

        public MarqueeNodeController(ArcadeController arcadeController, AssetCache<Texture> textureCache)
        : base(arcadeController, textureCache)
        {
        }

        protected override void PostSetup(Renderer renderer, Texture texture, float emissionIntensity)
        {
            ArtworkController.SetupStaticImage(renderer.material, texture, true, true, emissionIntensity);
            SetupMagicPixels(renderer);
        }

        protected override Renderer GetNodeRenderer(GameObject model) => GetNodeRenderer<MarqueeNodeTag>(model);

        protected override string GetModelImageDirectory(ModelConfiguration modelConfiguration) => modelConfiguration?.MarqueeDirectory;

        protected override string GetModelVideoDirectory(ModelConfiguration modelConfiguration) => modelConfiguration?.MarqueeVideoDirectory;

        protected override string GetEmulatorImageDirectory(EmulatorConfiguration emulator) => emulator?.MarqueesDirectory;

        protected override string GetEmulatorVideoDirectory(EmulatorConfiguration emulator) => emulator?.MarqueesVideoDirectory;

        private static void SetupMagicPixels(Renderer sourceRenderer)
        {
            Transform parentTransform = sourceRenderer.transform.parent;
            if (parentTransform == null)
            {
                return;
            }

            IEnumerable<Renderer> renderers = parentTransform.GetComponentsInChildren<Renderer>()
                                                             .Where(r => r.GetComponent<NodeTag>() == null
                                                                      && sourceRenderer.sharedMaterial.name.StartsWith(r.sharedMaterial.name));

            Color color;
            Texture texture;

            bool sourceMaterialIsEmissive = sourceRenderer.material.IsEmissiveEnabled();
            if (sourceMaterialIsEmissive)
            {
                color   = sourceRenderer.material.GetEmissiveColor();
                texture = sourceRenderer.material.GetEmissiveTexture();
            }
            else
            {
                color   = sourceRenderer.material.GetBaseColor();
                texture = sourceRenderer.material.GetBaseTexture();
            }

            foreach (Renderer renderer in renderers)
            {
                foreach (Material material in renderer.materials)
                {
                    if (material.IsEmissiveEnabled())
                    {
                        material.SetEmissiveColorAndTexture(color, texture, true);
                    }
                    else
                    {
                        material.SetBaseColorAndTexture(Color.white, texture);
                    }
                }
            }
        }
    }
}
