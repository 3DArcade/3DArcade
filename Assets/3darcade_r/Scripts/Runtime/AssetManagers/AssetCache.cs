﻿/* MIT License

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
using System.IO;
using UnityEngine;

namespace Arcade_r
{
    public abstract class AssetCache<T>
        where T : Object
    {
        protected readonly Dictionary<string, T> _loadedAssets;

        public AssetCache()
        {
            _loadedAssets = new Dictionary<string, T>();
        }

        protected abstract T LoadAsset(string filePathNoExt);

        protected abstract void UnloadAsset(T asset);

        public bool Get(string filePathNoExt, out T asset) => _loadedAssets.TryGetValue(filePathNoExt, out asset);

        public T Load(string[] directories, params string[] namesToTry)
        {
            foreach (string directory in directories)
            {
                T result = Load(directory, namesToTry);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        public T Load(string directory, params string[] namesToTry)
        {
            foreach (string name in namesToTry)
            {
                string filePathNoExt = Path.Combine(directory, name);
                if (_loadedAssets.TryGetValue(filePathNoExt, out T foundAsset))
                {
                    return foundAsset;
                }
                else
                {
                    T newAsset = LoadAsset(filePathNoExt);
                    if (newAsset != null)
                    {
                        _loadedAssets[filePathNoExt] = newAsset;
                        return newAsset;
                    }
                }
            }

            return null;
        }

        public void Unload(string filePathNoExt)
        {
            if (Get(filePathNoExt, out T asset))
            {
                UnloadInternal(filePathNoExt, asset);
            }
        }

        public void Reset()
        {
            foreach (KeyValuePair<string, T> loadedAsset in _loadedAssets)
            {
                UnloadInternal(loadedAsset.Key, loadedAsset.Value);
            }
            _loadedAssets.Clear();
        }

        private void UnloadInternal(string filePathNoExt, T asset)
        {
            if (asset != null)
            {
                UnloadAsset(asset);
            }
            _ = _loadedAssets.Remove(filePathNoExt);
        }
    }
}