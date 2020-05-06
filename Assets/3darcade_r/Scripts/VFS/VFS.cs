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

namespace Arcade_r
{
    public sealed class VFS
    {
        public static VFS Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new VFS();
                        }
                    }
                }
                return _instance;
            }
        }

        private readonly Dictionary<string, string> _mountedDirectories;
        private readonly Dictionary<string, string> _mountedFiles;

        private static volatile VFS _instance;
        private static readonly object _syncRoot = new object();

        private VFS()
        {
            _mountedDirectories = new Dictionary<string, string>();
            _mountedFiles       = new Dictionary<string, string>();
            MountDirectory(string.Empty, string.Empty);
        }

        public string GetDirectory(string alias)
        {
            _ = _mountedDirectories.TryGetValue(alias, out string result);
            return !string.IsNullOrEmpty(result) ? result : null;
        }
        public string GetFile(string alias)
        {
            _ = _mountedFiles.TryGetValue(alias, out string result);
            return !string.IsNullOrEmpty(result) ? result : null;
        }

        public void MountDirectory(string alias, string path)
        {
            if (!_mountedDirectories.ContainsKey(alias))
            {
                _mountedDirectories.Add(alias, path);
            }
        }

        public void MountFile(string alias, string path)
        {
            if (!_mountedFiles.ContainsKey(alias))
            {
                _mountedFiles.Add(alias, path);
            }
        }
    }
}
