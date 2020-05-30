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

namespace Arcade_r
{
    public sealed class ArcadeHierarchy
    {
        public readonly GameObject RootNode;
        public readonly GameObject ArcadesNode;
        public readonly GameObject GamesNode;
        public readonly GameObject PropsNode;

        public ArcadeHierarchy()
        {
            GameObject rootNode = UnityUtils.GameObjectCreateIfNotFound("Arcade", LayerMask.NameToLayer("Arcade"));
            Assert.IsNotNull(rootNode);

            string[] childNames     = new string[] { "ArcadeModels", "GameModels", "PropModels" };
            GameObject[] childNodes = new GameObject[childNames.Length];
            for (int i = 0; i < childNames.Length; ++i)
            {
                string childName       = childNames[i];
                GameObject childObject = UnityUtils.GameObjectCreateIfNotFound(childName, LayerMask.NameToLayer($"Arcade/{childName}"));
                Assert.IsNotNull(childObject);
                childObject.transform.SetParent(rootNode.transform);
                childNodes[i] = childObject;
            }

            RootNode    = rootNode;
            ArcadesNode = childNodes[0];
            GamesNode   = childNodes[1];
            PropsNode   = childNodes[2];
        }

        public void Reset()
        {
            ResetNode(ArcadesNode.transform);
            ResetNode(GamesNode.transform);
            ResetNode(PropsNode.transform);
        }

        private static void ResetNode(Transform transform)
        {
            while (transform.childCount > 0)
            {
                Object.DestroyImmediate(transform.GetChild(0).gameObject);
            }
        }
    }
}
