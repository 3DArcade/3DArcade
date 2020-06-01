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

using System;
using System.IO;

namespace Arcade_r
{
    public static class FileSystem
    {
        public static bool FileExists(string filePath)
        {
            return File.Exists(Path.GetFullPath(filePath));
        }

        public static string ReadAllText(string filePath)
        {
            return File.ReadAllText(Path.GetFullPath(filePath));
        }

        public static byte[] ReadAllBytes(string filePath)
        {
            return File.ReadAllBytes(Path.GetFullPath(filePath));
        }

        public static void WriteAllText(string filePath, string content)
        {
            File.WriteAllText(Path.GetFullPath(filePath), content);
        }

        public static bool DirectoryExists(string dirPath)
        {
            return Directory.Exists(Path.GetFullPath(dirPath));
        }

        public static string[] GetFiles(string dirPath, string searchPattern, bool searchAllDirectories)
        {
            if (!Directory.Exists(dirPath))
            {
                return null;
            }

            SearchOption searchOption;
            if (string.IsNullOrEmpty(searchPattern))
            {
                searchOption = SearchOption.TopDirectoryOnly;
            }
            else
            {
                searchOption = searchAllDirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            }

            string[] files = Directory.GetFiles(dirPath, searchPattern, searchOption);
            for (int i = 0; i < files.Length; i++)
            {
                files[i] = Path.GetFullPath(files[i]);
            }

            return files.Length > 0 ? files : null;
        }
    }
}
