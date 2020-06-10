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
    public sealed class VirtualFileSystem : IVirtualFileSystem
    {
        private readonly Dictionary<string, string> _mountedDirectories;
        private readonly Dictionary<string, string> _mountedFiles;

        public VirtualFileSystem()
        {
            _mountedDirectories = new Dictionary<string, string>();
            _mountedFiles       = new Dictionary<string, string>();
        }

        public void MountDirectory(string alias, string path)
        {
            if (!_mountedDirectories.ContainsKey(alias))
            {
                if (FileSystem.DirectoryExists(path))
                {
                    _mountedDirectories.Add(alias, path);
                }
            }
        }

        public void MountFile(string alias, string path)
        {
            if (!_mountedFiles.ContainsKey(alias))
            {
                if (FileSystem.FileExists(path))
                {
                    _mountedFiles.Add(alias, path);
                }
            }
        }

        public string GetDirectory(string alias)
        {
            if (_mountedDirectories.TryGetValue(alias, out string result))
            {
                return result;
            }
            return null;
        }

        public string GetFile(string alias)
        {
            if (_mountedFiles.TryGetValue(alias, out string result))
            {
                return result;
            }
            return null;
        }

        public string[] GetFiles(string alias, string searchPattern, bool searchAllDirectories)
        {
            string directory = GetDirectory(alias);
            if (directory != null)
            {
                return FileSystem.GetFiles(directory, searchPattern, searchAllDirectories);
            }
            return new string[0];
        }
    }
}
