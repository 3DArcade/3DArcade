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
    public static class RendererExtensions
    {
        public static Color GetBaseColor(this Renderer renderer, int materialReadIndex = 0)
        {
            if (renderer == null || renderer.material == null)
                return Color.magenta;

            MaterialPropertyBlock block = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(block, materialReadIndex);
            return block.GetColor(MaterialUtils.SHADER_BASE_COLOR_ID);
        }

        public static Texture GetBaseTexture(this Renderer renderer, int materialReadIndex = 0)
        {
            if (renderer == null || renderer.material == null)
                return null;

            MaterialPropertyBlock block = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(block, materialReadIndex);
            return block.GetTexture(MaterialUtils.SHADER_BASE_TEXTURE_ID);
        }

        public static Color GetEmissionColor(this Renderer renderer, int materialReadIndex = 0)
        {
            if (renderer == null || renderer.material == null)
                return Color.black;

            MaterialPropertyBlock block = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(block, materialReadIndex);
            return block.GetColor(MaterialUtils.SHADER_EMISSIVE_COLOR_ID);
        }

        public static Texture GetEmissionTexture(this Renderer renderer, int materialReadIndex = 0)
        {
            if (renderer == null || renderer.material == null)
                return null;

            MaterialPropertyBlock block = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(block, materialReadIndex);
            return block.GetTexture(MaterialUtils.SHADER_EMISSIVE_TEXTURE_ID);
        }

        public static void SetBaseColor(this Renderer renderer, Color color, int materialReadIndex = 0, int? materialWriteIndex = null)
        {
            if (renderer == null || renderer.material == null)
                return;

            MaterialPropertyBlock block = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(block, materialReadIndex);
            block.SetColor(MaterialUtils.SHADER_BASE_COLOR_ID, color);
            renderer.SetPropertyBlock(block, materialWriteIndex ?? materialReadIndex);
        }

        public static void SetBaseTexture(this Renderer renderer, Texture texture, int materialReadIndex = 0, int? materialWriteIndex = null)
        {
            if (renderer == null || renderer.material == null || texture == null)
                return;

            MaterialPropertyBlock block = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(block, materialReadIndex);
            block.SetTexture(MaterialUtils.SHADER_BASE_TEXTURE_ID, texture);
            renderer.SetPropertyBlock(block, materialWriteIndex ?? materialReadIndex);
        }

        public static void SetEmissionColor(this Renderer renderer, Color color, int materialReadIndex = 0, int? materialWriteIndex = null)
        {
            if (renderer == null || renderer.material == null)
                return;

            MaterialPropertyBlock block = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(block, materialReadIndex);
            block.SetColor(MaterialUtils.SHADER_EMISSIVE_COLOR_ID, color);
            renderer.SetPropertyBlock(block, materialWriteIndex ?? materialReadIndex);
        }

        public static void SetEmissionTexture(this Renderer renderer, Texture texture, int materialReadIndex = 0, int? materialWriteIndex = null)
        {
            if (renderer == null || renderer.material == null || texture == null)
                return;

            MaterialPropertyBlock block = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(block, materialReadIndex);
            block.SetTexture(MaterialUtils.SHADER_EMISSIVE_TEXTURE_ID, texture);
            renderer.SetPropertyBlock(block, materialWriteIndex ?? materialReadIndex);
        }

        public static void EnableEmission(this Renderer renderer, bool enabled, int materialReadIndex = 0, int? materialWriteIndex = null)
        {
            if (renderer == null || renderer.material == null)
                return;

            MaterialPropertyBlock block = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(block, materialReadIndex);
            if (enabled)
            {
                renderer.material.EnableKeyword(MaterialUtils.SHADER_EMISSIVE_KEYWORD);
                block.SetFloat(MaterialUtils.SHADER_EMISSIVE_EXPOSURE_WEIGTH_ID, 0f);
            }
            else
            {
                block.SetFloat(MaterialUtils.SHADER_EMISSIVE_EXPOSURE_WEIGTH_ID, 1f);
                renderer.material.DisableKeyword(MaterialUtils.SHADER_EMISSIVE_KEYWORD);
            }
            renderer.SetPropertyBlock(block, materialWriteIndex ?? materialReadIndex);
        }

        public static bool IsEmissive(this Renderer renderer)
            => renderer != null && renderer.material.IsEmissive();
    }
}
