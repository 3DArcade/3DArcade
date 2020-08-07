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
        protected override string[] DefaultImageDirectories { get; } = new string[] { $"{_defaultMediaDirectory}/Marquees" };
        protected override string[] DefaultVideoDirectories { get; } = new string[] { $"{_defaultMediaDirectory}/MarqueesVideo" };

        public MarqueeNodeController(AssetCache<string> videoCache, AssetCache<Texture> textureCache)
        : base(videoCache, textureCache)
        {
        }

        protected override void PostSetup(Renderer renderer, Texture texture, float emissionIntensity)
        {
            SetupStaticImage(renderer.material, texture, true, true, emissionIntensity);
            SetupMagicPixels(renderer);
        }

        protected override Renderer GetNodeRenderer(GameObject model) => GetNodeRenderer<MarqueeNodeTag>(model);

        protected override string[] GetModelImageDirectories(ModelConfiguration modelConfiguration) => modelConfiguration?.MarqueeImageDirectories;

        protected override string[] GetModelVideoDirectories(ModelConfiguration modelConfiguration) => modelConfiguration?.MarqueeVideoDirectories;

        protected override string[] GetEmulatorImageDirectories(EmulatorConfiguration emulator) => emulator?.MarqueeImagesDirectories;

        protected override string[] GetEmulatorVideoDirectories(EmulatorConfiguration emulator) => emulator?.MarqueeVideosDirectories;

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
                        material.ClearBaseColorAndTexture();
                        material.SetEmissiveColorAndTexture(color, texture);
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
