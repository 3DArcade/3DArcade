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

namespace Arcade
{
    public sealed class EmulatorDatabase : Database<EmulatorConfiguration>
    {
        public readonly EmulatorConfiguration FpsArcadeLauncher;
        public readonly EmulatorConfiguration CylArcadeLauncher;
        public readonly EmulatorConfiguration FpsMenuLauncher;
        public readonly EmulatorConfiguration CylMenuLauncher;

        public EmulatorDatabase(IVirtualFileSystem virtualFileSystem)
        : base(virtualFileSystem, "emulator_cfgs")
        {
            FpsArcadeLauncher = MakeInternalLauncher("FpsArcadeLauncher", "internal_fps_arcade_launcher");
            CylArcadeLauncher = MakeInternalLauncher("CylArcadeLauncher", "internal_cyl_arcade_launcher");
            FpsMenuLauncher   = MakeInternalLauncher("FpsMenuLauncher", "internal_fps_menu_launcher");
            CylMenuLauncher   = MakeInternalLauncher("CylMenuLauncher", "internal_cyl_menu_launcher");
        }

        private EmulatorConfiguration MakeInternalLauncher(string descriptiveName, string id)
        {
            string mediaFolder = _virtualFileSystem.GetDirectory("media");
            return new EmulatorConfiguration
            {
                DescriptiveName        = descriptiveName,
                Id                     = id,
                Directory              = null,
                WorkingDirectory       = null,
                Executable             = null,
                Arguments              = null,
                GamesDirectory         = null,
                SupportedExtensions    = null,
                Model                  = null,
                MarqueesDirectory      = $"{mediaFolder}/Marquees",
                MarqueesVideoDirectory = $"{mediaFolder}/MarqueesVideo",
                ScreensDirectory       = $"{mediaFolder}/Screens",
                ScreensVideoDirectory  = $"{mediaFolder}/ScreensVideo",
                TitlesDirectory        = $"{mediaFolder}/Titles",
                GenericsDirectory      = $"{mediaFolder}/Generics",
                GenericsVideoDirectory = $"{mediaFolder}/GenericsVideo",
                InfoDirectory          = $"{mediaFolder}/Info",
                About                  = $"{mediaFolder}/About",
                OutputCommandLine      = false
            };
        }
    }
}
