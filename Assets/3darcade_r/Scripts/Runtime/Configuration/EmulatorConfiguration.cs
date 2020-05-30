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

namespace Arcade_r
{
    [System.Serializable]
    public sealed class EmulatorConfiguration : IConfiguration
    {
        string IConfiguration.DescriptiveName => DescriptiveName;
        string IConfiguration.Id => Id;

        public string DescriptiveName;
        public string Id;
        public string About;
        public string Executable;
        public string Extension;
        public string LibretroCore;
        public string Arguments;
        public string Options;
        public string EmulatorPath;
        public string GamePath;
        public string WorkingDir;
        public string MarqueePath;
        public string ScreenPath;
        public string ScreenVideoPath;
        public string GenericPath;
        public string TitlePath;
        public string InfoPath;
        public string GameLauncherMethod;
        public bool OutputCommandLine;
    }
}
