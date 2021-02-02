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
    public static class MaterialUtils
    {
        public const string SHADER_EMISSIVE_KEYWORD                   = "_EMISSIVE_COLOR_MAP";
        public const string SHADER_EMISSIVE_TEXTURE_NAME              = "_EmissiveColorMap";
        public static readonly int SHADER_BASE_COLOR_ID               = Shader.PropertyToID("_BaseColor");
        public static readonly int SHADER_BASE_TEXTURE_ID             = Shader.PropertyToID("_BaseColorMap");
        public static readonly int SHADER_EMISSIVE_COLOR_ID           = Shader.PropertyToID("_EmissiveColor");
        public static readonly int SHADER_EMISSIVE_TEXTURE_ID         = Shader.PropertyToID(SHADER_EMISSIVE_TEXTURE_NAME);
        public static readonly int SHADER_EMISSIVE_EXPOSURE_WEIGTH_ID = Shader.PropertyToID("_EmissiveExposureWeight");

        public static void SetGPUInstancing(bool active, params GameObject[] models)
        {
            for (int modelInxed = 0; modelInxed < models.Length; ++modelInxed)
                SetGPUInstancing(active, models[modelInxed].GetComponentsInChildren<Renderer>());
        }

        public static void SetGPUInstancing(bool active, params Renderer[] renderers)
        {
            for (int rendererIndex = 0; rendererIndex < renderers.Length; ++rendererIndex)
                SetGPUInstancing(active, renderers[rendererIndex].sharedMaterials);
        }

        public static void SetGPUInstancing(bool active, params Material[] materials)
        {
            for (int materialIndex = 0; materialIndex < materials.Length; ++materialIndex)
                if (materials[materialIndex] != null)
                    materials[materialIndex].enableInstancing = active;
        }

        public static void SetGPUInstancing(bool active, IEnumerable<GameObject> models)
        {
            foreach (GameObject model in models)
                SetGPUInstancing(active, model.GetComponentsInChildren<Renderer>());
        }

        public static void SetGPUInstancing(bool active, IEnumerable<Renderer> renderers)
        {
            foreach (Renderer renderer in renderers)
                SetGPUInstancing(active, renderer.sharedMaterials);
        }

        public static void SetGPUInstancing(bool active, IEnumerable<Material> materials)
        {
            foreach (Material material in materials)
                if (material != null)
                    material.enableInstancing = active;
        }
    }
}
