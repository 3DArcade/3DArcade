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
    public static class ArtworkMatcher
    {
        public static bool GetDirectoriesToTry(out List<string> directories, string gameArtworkDirectory, string emulatorArtworkDirectory, string defaultArtworkDirectory)
        {
            directories = new List<string>();

            directories.AddStringIfNotNullOrEmpty(FileSystem.CorrectPath(gameArtworkDirectory));
            directories.AddStringIfNotNullOrEmpty(FileSystem.CorrectPath(emulatorArtworkDirectory));
            directories.AddStringIfNotNullOrEmpty(FileSystem.CorrectPath(defaultArtworkDirectory));

            return directories.Count > 0;
        }

        public static bool GetNamesToTry(out List<string> namesToTry, ModelConfiguration game, EmulatorConfiguration emulator)
        {
            namesToTry  = new List<string>();

            if (game != null)
            {
                namesToTry.AddStringIfNotNullOrEmpty(game.Id);
                namesToTry.AddStringIfNotNullOrEmpty(game.CloneOf);
                namesToTry.AddStringIfNotNullOrEmpty(game.RomOf);
            }

            if (emulator != null)
            {
                namesToTry.AddStringIfNotNullOrEmpty(emulator.Id);
            }

            return namesToTry.Count > 0;
        }
    }
}
