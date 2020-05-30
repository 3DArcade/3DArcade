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

namespace Arcade_r
{
    public static class AssetManager
    {
        private static Dictionary<string, GameObject> _loadedPrefabs;

        static AssetManager()
        {
            _loadedPrefabs = new Dictionary<string, GameObject>();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration), SuppressMessage("CodeQuality", "IDE0051:Remove unused private members")]
        private static void ResetForDomainReloadDisabled()
        {
            _loadedPrefabs = new Dictionary<string, GameObject>();
        }

        public static GameObject LoadPrefab(string[] subdirectories, params string[] namesToTry)
        {
            foreach (string subDirectory in subdirectories)
            {
                foreach (string name in namesToTry)
                {
                    if (_loadedPrefabs.TryGetValue(name, out GameObject foundPrefab))
                    {
                        return foundPrefab;
                    }
                    else
                    {
                        GameObject newPrefab = Resources.Load<GameObject>($"{subDirectory}/{name}");
                        if (newPrefab != null)
                        {
                            _loadedPrefabs[name] = newPrefab;
                            return newPrefab;
                        }
                    }
                }
            }

            return null;
        }

        public static void Reset()
        {
            foreach (KeyValuePair<string, GameObject> loadedPrefab in _loadedPrefabs)
            {
                UnloadPrefab(loadedPrefab.Key);
            }
            _loadedPrefabs.Clear();
        }

        public static void UnloadPrefab(string name)
        {
            if (_loadedPrefabs.TryGetValue(name, out GameObject foundPrefab))
            {
                Resources.UnloadAsset(foundPrefab);
                _ = _loadedPrefabs.Remove(name);
            }
        }
    }
}
