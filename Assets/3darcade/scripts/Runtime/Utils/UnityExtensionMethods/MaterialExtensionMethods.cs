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
        public static Color GetBaseColor(this Material material) => material != null ? material.GetColor(MaterialUtils.SHADER_BASE_COLOR_NAME) : Color.black;

        public static void SetBaseColor(this Material material, Color color)
        {
            if (material == null)
            {
                return;
            }

            material.SetColor(MaterialUtils.SHADER_BASE_COLOR_NAME, color);
        }

        public static void ClearBaseColor(this Material material)
        {
            if (material == null)
            {
                return;
            }

            material.SetColor(MaterialUtils.SHADER_BASE_COLOR_NAME, Color.black);
        }

        public static Texture GetBaseTexture(this Material material) => material != null ? material.GetTexture(MaterialUtils.SHADER_BASE_TEXTURE_NAME) : null;

        public static void SetBaseTexture(this Material material, Texture texture)
        {
            if (material == null)
            {
                return;
            }

            material.SetTexture(MaterialUtils.SHADER_BASE_TEXTURE_NAME, texture);
        }

        public static void ClearBaseTexture(this Material material)
        {
            if (material == null)
            {
                return;
            }

            material.SetTexture(MaterialUtils.SHADER_BASE_TEXTURE_NAME, null);
        }

        public static void SetBaseColorAndTexture(this Material material, Color color, Texture texture)
        {
            if (material == null)
            {
                return;
            }

            material.SetBaseColor(color);
            material.SetBaseTexture(texture);
        }

        public static void ClearBaseColorAndTexture(this Material material)
        {
            if (material == null)
            {
                return;
            }

            material.ClearBaseColor();
            material.ClearBaseTexture();
        }

        public static Color GetEmissiveColor(this Material material) => material != null ? material.GetColor(MaterialUtils.SHADER_EMISSIVE_COLOR_NAME) : Color.black;

        public static void SetEmissiveColor(this Material material, Color color)
        {
            if (material == null)
            {
                return;
            }

            material.SetColor(MaterialUtils.SHADER_EMISSIVE_COLOR_NAME, color);
        }

        public static void ClearEmissiveColor(this Material material)
        {
            if (material == null)
            {
                return;
            }

            material.SetColor(MaterialUtils.SHADER_EMISSIVE_COLOR_NAME, Color.black);
        }

        public static Texture GetEmissiveTexture(this Material material) => material != null ? material.GetTexture(MaterialUtils.SHADER_EMISSIVE_TEXTURE_NAME) : null;

        public static void SetEmissiveTexture(this Material material, Texture texture)
        {
            if (material == null)
            {
                return;
            }

            material.EnableEmissive();
            material.SetTexture(MaterialUtils.SHADER_EMISSIVE_TEXTURE_NAME, texture);
        }

        public static void ClearEmissiveTexture(this Material material)
        {
            if (material == null)
            {
                return;
            }

            material.SetTexture(MaterialUtils.SHADER_EMISSIVE_TEXTURE_NAME, null);
        }

        public static void SetEmissiveColorAndTexture(this Material material, Color color, Texture texture, bool clearMainColorAndTexture)
        {
            if (material == null)
            {
                return;
            }

            if (clearMainColorAndTexture)
            {
                material.ClearBaseColorAndTexture();
            }

            material.SetEmissiveColor(color);
            material.SetEmissiveTexture(texture);
        }

        public static void ClearEmissiveColorAndTexture(this Material material)
        {
            if (material == null)
            {
                return;
            }

            material.ClearEmissiveColor();
            material.ClearEmissiveTexture();
        }

        public static void EnableEmissive(this Material material)
        {
            if (material == null)
            {
                return;
            }

            material.SetFloat(MaterialUtils.SHADER_EMISSIVE_EXPOSURE_WEIGTH_KEYWORD, 0f);
            material.EnableKeyword(MaterialUtils.SHADER_EMISSIVE_KEYWORD);
        }

        public static bool IsEmissiveEnabled(this Material material) => material != null && material.IsKeywordEnabled(MaterialUtils.SHADER_EMISSIVE_KEYWORD);
    }
}
