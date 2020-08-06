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

namespace Arcade
{
    public static class ArtworkMatcher
    {
        public static List<string> GetDirectoriesToTry(string[] gameArtworkDirectories, string[] emulatorArtworkDirectories, string[] defaultArtworkDirectories)
        {
            List<string> result = new List<string>();

            if (gameArtworkDirectories != null)
            {
                foreach (string directory in gameArtworkDirectories)
                {
                    result.AddStringIfNotNullOrEmpty(FileSystem.CorrectPath(directory));
                }
            }

            if (emulatorArtworkDirectories != null)
            {
                foreach (string directory in emulatorArtworkDirectories)
                {
                    result.AddStringIfNotNullOrEmpty(FileSystem.CorrectPath(directory));
                }
            }

            if (defaultArtworkDirectories != null)
            {
                foreach (string directory in defaultArtworkDirectories)
                {
                    result.AddStringIfNotNullOrEmpty(FileSystem.CorrectPath(directory));
                }
            }

            return result;
        }

        public static List<string> GetNamesToTry(ModelConfiguration game, EmulatorConfiguration emulator)
        {
            List<string> result = new List<string>();

            if (game != null)
            {
                result.AddStringIfNotNullOrEmpty(game.Id);
                result.AddStringIfNotNullOrEmpty(game.CloneOf);
                result.AddStringIfNotNullOrEmpty(game.RomOf);
            }

            if (emulator != null)
            {
                result.AddStringIfNotNullOrEmpty(emulator.Id);
            }

            return result;
        }
    }
}
