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
        public readonly Transform RootNode;
        public readonly Transform ArcadesNode;
        public readonly Transform GamesNode;
        public readonly Transform PropsNode;

        private static readonly string[] _childNames = new string[] { "ArcadeModels", "GameModels", "PropModels" };

        public ArcadeHierarchy()
        {
            GameObject rootNode = GameObjectUtils.GameObjectCreateIfNotFound("Arcade", LayerMask.NameToLayer("Arcade"));
            Assert.IsNotNull(rootNode);

            GameObject[] childNodes = new GameObject[_childNames.Length];
            for (int i = 0; i < _childNames.Length; ++i)
            {
                string childName       = _childNames[i];
                GameObject childObject = GameObjectUtils.GameObjectCreateIfNotFound(childName, LayerMask.NameToLayer($"Arcade/{childName}"));
                Assert.IsNotNull(childObject);
                childObject.transform.SetParent(rootNode.transform);
                childNodes[i] = childObject;
            }

            RootNode    = rootNode.transform;
            ArcadesNode = childNodes[0].transform;
            GamesNode   = childNodes[1].transform;
            PropsNode   = childNodes[2].transform;
        }

        public void Reset()
        {
            ResetNode(ArcadesNode);
            ResetNode(GamesNode);
            ResetNode(PropsNode);
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
