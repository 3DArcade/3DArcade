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

namespace Arcade
{
    public static class MaterialExtensions
    {
        public static Color GetBaseColor(this Material material)
        {
            Assert.IsNotNull(material);
            return material.GetColor(MaterialUtils.SHADER_BASE_COLOR_ID);
        }

        public static Texture GetBaseTexture(this Material material)
        {
            Assert.IsNotNull(material);
            return material.GetTexture(MaterialUtils.SHADER_BASE_TEXTURE_ID);
        }

        public static Color GetEmissionColor(this Material material)
        {
            Assert.IsNotNull(material);
            return material.GetColor(MaterialUtils.SHADER_EMISSIVE_COLOR_ID);
        }

        public static Texture GetEmissionTexture(this Material material)
        {
            Assert.IsNotNull(material);
            return material.GetTexture(MaterialUtils.SHADER_EMISSIVE_TEXTURE_ID);
        }

        public static void SetBaseColor(this Material material, Color color)
        {
            Assert.IsNotNull(material);
            material.SetColor(MaterialUtils.SHADER_BASE_COLOR_ID, color);
        }

        public static void SetBaseTexture(this Material material, Texture texture)
        {
            Assert.IsNotNull(material);
            material.SetTexture(MaterialUtils.SHADER_BASE_TEXTURE_ID, texture);
        }

        public static void SetEmissionColor(this Material material, Color color)
        {
            Assert.IsNotNull(material);
            material.SetColor(MaterialUtils.SHADER_EMISSIVE_COLOR_ID, color);
        }

        public static void SetEmissionTexture(this Material material, Texture texture)
        {
            Assert.IsNotNull(material);
            material.SetTexture(MaterialUtils.SHADER_EMISSIVE_TEXTURE_ID, texture);
        }

        public static void EnableEmission(this Material material, bool enabled)
        {
            Assert.IsNotNull(material);
            if (enabled)
            {
                material.EnableKeyword(MaterialUtils.SHADER_EMISSIVE_KEYWORD);
                material.SetFloat(MaterialUtils.SHADER_EMISSIVE_EXPOSURE_WEIGTH_ID, 0f);
            }
            else
            {
                material.SetFloat(MaterialUtils.SHADER_EMISSIVE_EXPOSURE_WEIGTH_ID, 1f);
                material.DisableKeyword(MaterialUtils.SHADER_EMISSIVE_KEYWORD);
            }
        }

        public static bool IsEmissive(this Material material)
            => material != null && material.IsKeywordEnabled(MaterialUtils.SHADER_EMISSIVE_KEYWORD);
    }
}
