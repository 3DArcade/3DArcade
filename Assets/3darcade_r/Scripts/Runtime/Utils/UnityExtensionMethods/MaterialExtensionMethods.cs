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

namespace Arcade_r
{
    public static class MaterialExtensionMethods
    {
        public static void SetAlbedoColor(this Material material, Color color)
        {
            if (material == null)
            {
                return;
            }

            material.SetColor(MaterialUtils.SHADER_MAINCOLOR_NAME, color);
        }

        public static void SetAlbedoTexture(this Material material, Texture texture)
        {
            if (material == null)
            {
                return;
            }

            material.SetTexture(MaterialUtils.SHADER_MAINTEXTURE_NAME, texture);
        }

        public static void SetAlbedoColorAndTexture(this Material material, Color color, Texture texture)
        {
            if (material == null)
            {
                return;
            }

            material.SetTexture(MaterialUtils.SHADER_MAINTEXTURE_NAME, texture);
            material.SetColor(MaterialUtils.SHADER_MAINCOLOR_NAME, color);
        }

        public static void ClearAlbedoColorAndTexture(this Material material)
        {
            if (material == null)
            {
                return;
            }

            material.SetTexture(MaterialUtils.SHADER_MAINTEXTURE_NAME, null);
            material.SetColor(MaterialUtils.SHADER_MAINCOLOR_NAME, Color.black);
        }

        public static void SetEmissionColorAndTexture(this Material material, Color color, Texture texture, bool clearMainColorAndTexture)
        {
            if (material == null)
            {
                return;
            }

            if (clearMainColorAndTexture)
            {
                material.ClearAlbedoColorAndTexture();
            }

            material.EnableKeyword(MaterialUtils.SHADER_EMISSIVEKEYWORD);
            material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.BakedEmissive;
            material.SetTexture(MaterialUtils.SHADER_EMISSIVETEXTURE_NAME, texture);

            material.SetColor(MaterialUtils.SHADER_EMISSIVECOLOR_NAME, color);
        }
    }
}
