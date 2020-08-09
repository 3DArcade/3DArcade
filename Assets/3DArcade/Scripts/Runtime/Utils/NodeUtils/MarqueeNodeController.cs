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

        private readonly MaterialPropertyBlock _materialPropertyBlock;

        public MarqueeNodeController(AssetCache<string> videoCache, AssetCache<Texture> textureCache)
        : base(videoCache, textureCache)
        {
            _materialPropertyBlock = new MaterialPropertyBlock();
        }

        public void SetupMagicPixels(Renderer sourceRenderer)
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

            bool sourceIsEmissive = sourceRenderer.material.IsEmissive();

            if (sourceIsEmissive)
            {
                color   = sourceRenderer.material.GetEmissionColor();
                texture = sourceRenderer.material.GetEmissionTexture();
            }
            else
            {
                color   = sourceRenderer.material.GetBaseColor();
                texture = sourceRenderer.material.GetBaseTexture();
            }

            if (texture == null)
            {
                return;
            }

            foreach (Renderer renderer in renderers)
            {
                renderer.GetPropertyBlock(_materialPropertyBlock);

                if (renderer.material.IsEmissive())
                {
                    _materialPropertyBlock.SetColor(MaterialUtils.SHADER_EMISSIVE_COLOR_NAME, color);
                    _materialPropertyBlock.SetTexture(MaterialUtils.SHADER_EMISSIVE_TEXTURE_NAME, texture);
                }
                else
                {
                    color = sourceIsEmissive ? Color.white : color;
                    _materialPropertyBlock.SetColor(MaterialUtils.SHADER_BASE_COLOR_NAME, color);
                    _materialPropertyBlock.SetTexture(MaterialUtils.SHADER_BASE_TEXTURE_NAME, texture);
                }

                for (int i = 0; i < renderer.materials.Length; ++i)
                {
                    renderer.SetPropertyBlock(_materialPropertyBlock, i);
                }
            }
        }

        protected override Renderer GetNodeRenderer(GameObject model) => GetNodeRenderer<MarqueeNodeTag>(model);

        protected override string[] GetModelImageDirectories(ModelConfiguration modelConfiguration) => modelConfiguration?.MarqueeImageDirectories;

        protected override string[] GetModelVideoDirectories(ModelConfiguration modelConfiguration) => modelConfiguration?.MarqueeVideoDirectories;

        protected override string[] GetEmulatorImageDirectories(EmulatorConfiguration emulator) => emulator?.MarqueeImagesDirectories;

        protected override string[] GetEmulatorVideoDirectories(EmulatorConfiguration emulator) => emulator?.MarqueeVideosDirectories;
    }
}
