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
    public static class MaterialExtensionMethods
    {
        public static void SetAlbedoColor(this Material material, Color color)
        {
            if (material == null)
            {
                return;
            }

            material.SetColor(MaterialUtils.SHADER_ALBEDO_COLOR_NAME, color);
        }

        public static void ClearAlbedoColor(this Material material)
        {
            if (material == null)
            {
                return;
            }

            material.SetColor(MaterialUtils.SHADER_ALBEDO_COLOR_NAME, Color.black);
        }

        public static void SetAlbedoTexture(this Material material, Texture texture)
        {
            if (material == null)
            {
                return;
            }

            material.SetTexture(MaterialUtils.SHADER_ALBEDO_TEXTURE_NAME, texture);
        }

        public static void ClearAlbedoTexture(this Material material)
        {
            if (material == null)
            {
                return;
            }

            material.SetTexture(MaterialUtils.SHADER_ALBEDO_TEXTURE_NAME, null);
        }

        public static void SetAlbedoColorAndTexture(this Material material, Color color, Texture texture)
        {
            if (material == null)
            {
                return;
            }

            material.SetAlbedoTexture(texture);
            material.SetAlbedoColor(color);
        }

        public static void ClearAlbedoColorAndTexture(this Material material)
        {
            if (material == null)
            {
                return;
            }

            material.ClearAlbedoTexture();
            material.ClearAlbedoColor();
        }

        public static void SetEmissiveColorAndTexture(this Material material, Color color, Texture texture, bool clearMainColorAndTexture)
        {
            if (material == null)
            {
                return;
            }

            if (clearMainColorAndTexture)
            {
                material.ClearAlbedoColorAndTexture();
            }

            material.EnableKeyword(MaterialUtils.SHADER_EMISSIVE_KEYWORD);
            material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.BakedEmissive;
            material.SetTexture(MaterialUtils.SHADER_EMISSIVE_TEXTURE_NAME, texture);

            material.SetColor(MaterialUtils.SHADER_EMISSIVE_COLOR_NAME, color);
        }
    }
}
