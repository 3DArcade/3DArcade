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
using System.IO;
using System.Linq;
using UnityEngine;

namespace Arcade_r
{
    public abstract class Database<T>
        where T : Configuration
    {
        protected readonly IVirtualFileSystem _virtualFileSystem;
        protected readonly string _directoryAlias;

        protected readonly Dictionary<string, T> _configurations;

        public Database(IVirtualFileSystem virtualFileSystem, string directoryAlias)
        {
            _virtualFileSystem = virtualFileSystem;
            _directoryAlias    = directoryAlias;
            _configurations    = new Dictionary<string, T>();

            Refresh();
        }

        public string[] GetNames() => _configurations.Keys.ToArray();

        public void Refresh()
        {
            _configurations.Clear();
            string[] filePaths = _virtualFileSystem.GetFiles(_directoryAlias, "*.json", false);
            foreach (string filePath in filePaths)
            {
                _configurations.Add(Path.GetFileNameWithoutExtension(filePath), null);
            }
        }

        public T Get(string id, bool reload = false)
        {
            if (string.IsNullOrEmpty(id))
            {
                Debug.LogWarning($"[{GetType().Name}] Passed null for configuration ID");
                return null;
            }

            if (!_configurations.ContainsKey(id))
            {
                Debug.LogWarning($"[{GetType().Name}] Configuration not found: {id}");
                return null;
            }

            if (_configurations[id] == null || reload)
            {
                _configurations[id] = Load(id);
            }

            return _configurations[id];
        }

        public bool Save(in T configuration)
        {
            try
            {
                FileSystem.JsonSerialize($"{_virtualFileSystem.GetDirectory(_directoryAlias)}/{configuration.Id}.json", configuration);
                Debug.Log($"[{GetType().Name}] Saved configuration: {configuration.DescriptiveName} ({configuration.Id})");
                _configurations[configuration.Id] = configuration;
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
            }

            return false;
        }

        private T Load(string id)
        {
            try
            {
                T cfg = FileSystem.JsonDeserialize<T>($"{_virtualFileSystem.GetDirectory(_directoryAlias)}/{id}.json");
                Debug.Log($"[{GetType().Name}] Loaded configuration: {cfg.DescriptiveName} ({cfg.Id})");
                return cfg;
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
            }

            return null;
        }
    }
}
