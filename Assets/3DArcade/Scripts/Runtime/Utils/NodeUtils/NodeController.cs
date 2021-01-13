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
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Arcade
{
    public abstract class NodeController<T> where T : NodeTag
    {
        protected static readonly string _defaultMediaDirectory = $"{SystemUtils.GetDataPath()}/3darcade~/Configuration/Media";

        protected abstract string[] DefaultImageDirectories { get; }
        protected abstract string[] DefaultVideoDirectories { get; }

        public void Setup(ArcadeController arcadeController, GameObject model, ModelConfiguration modelConfiguration, EmulatorConfiguration emulator, float emissionIntensity)
        {
            Renderer[] renderers = GetNodeRenderers(model);
            if (renderers == null)
                return;

            foreach (Renderer renderer in renderers)
            {
                renderer.material.EnableEmission(true);
                renderer.material.SetBaseColor(Color.black);
                renderer.material.SetBaseTexture(null);
                renderer.material.SetEmissionColor(Color.white * emissionIntensity);
            }

            List<string> namesToTry = ArtworkMatcher.GetNamesToTry(modelConfiguration, emulator);

            List<string> directories = ArtworkMatcher.GetDirectoriesToTry(GetModelImageDirectories(modelConfiguration), GetEmulatorImageDirectories(emulator), DefaultImageDirectories);
            ArtworkUtils.SetupImages(directories, namesToTry, renderers, this as MarqueeNodeController != null);

            directories = ArtworkMatcher.GetDirectoriesToTry(GetModelVideoDirectories(modelConfiguration), GetEmulatorVideoDirectories(emulator), DefaultVideoDirectories);
            ArtworkUtils.SetupVideos(directories, namesToTry, renderers, arcadeController.AudioMinDistance, arcadeController.AudioMaxDistance, arcadeController.VolumeCurve);
        }

        protected abstract string[] GetModelImageDirectories(ModelConfiguration modelConfiguration);

        protected abstract string[] GetModelVideoDirectories(ModelConfiguration modelConfiguration);

        protected abstract string[] GetEmulatorImageDirectories(EmulatorConfiguration emulator);

        protected abstract string[] GetEmulatorVideoDirectories(EmulatorConfiguration emulator);

        // [SuppressMessage("Type Safety", "UNT0014:Invalid type for call to GetComponent", Justification = "Analyzer is dumb")]
        private static Renderer[] GetNodeRenderers(GameObject model)
        {
            if (model == null)
                return null;

            List<Renderer> renderers = new List<Renderer>();

            T[] nodeTags = model.GetComponentsInChildren<T>();
            if (nodeTags != null)
            {
                foreach (T nodeTag in nodeTags)
                {
                    Renderer renderer = nodeTag.GetComponent<Renderer>();
                    if (renderer != null)
                        renderers.Add(renderer);
                }
            }

            return renderers.Count > 0 ? renderers.ToArray() : null;
        }
    }
}
