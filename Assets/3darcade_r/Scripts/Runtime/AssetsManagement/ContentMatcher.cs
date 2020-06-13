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
using System.Collections.Generic;

namespace Arcade_r
{
    public sealed class ContentMatcher
    {
        public delegate List<string> GetNamesToTryDelegate(ModelConfiguration modelConfiguration, EmulatorConfiguration emulator);

        private const string DEFAULT_ARCADE_MODEL    = "defaultCylinder";
        private const string DEFAULT_GAME_HOR_MODEL  = "default80hor";
        private const string DEFAULT_GAME_VERT_MODEL = "default80vert";
        private const string DEFAULT_PROP_MODEL      = "penguin";

        private readonly EmulatorDatabase _emulatorDatabase;

        public ContentMatcher(Database<EmulatorConfiguration> emulatorDatabase)
            => _emulatorDatabase = emulatorDatabase as EmulatorDatabase ?? throw new ArgumentNullException(nameof(emulatorDatabase));

        public EmulatorConfiguration GetEmulatorForConfiguration(ModelConfiguration modelConfiguration)
        {
            switch (modelConfiguration.InteractionType)
            {
                case InteractionType.GameInternal:
                case InteractionType.GameExternal:
                case InteractionType.URL:
                {
                    return _emulatorDatabase.Get(modelConfiguration.Emulator);
                }
                case InteractionType.FpsArcadeConfiguration:
                {
                    return _emulatorDatabase.FpsArcadeLauncher;
                }
                case InteractionType.CylArcadeConfiguration:
                {
                    return _emulatorDatabase.CylArcadeLauncher;
                }
                case InteractionType.FpsMenuConfiguration:
                {
                    return _emulatorDatabase.FpsMenuLauncher;
                }
                case InteractionType.CylMenuConfiguration:
                {
                    return _emulatorDatabase.CylMenuLauncher;
                }
                case InteractionType.None:
                default:
                    break;
            }

            return null;
        }

        public static List<string> GetNamesToTryForArcade(ModelConfiguration modelConfiguration, EmulatorConfiguration _)
            => new List<string> { modelConfiguration.Id, DEFAULT_ARCADE_MODEL };

        public static List<string> GetNamesToTryForGame(ModelConfiguration modelConfiguration, EmulatorConfiguration emulator)
        {
            List<string> result = new List<string>();

            // Model from game
            if (modelConfiguration != null)
            {
                result.AddStringIfNotNullOrEmpty(modelConfiguration.Model);
                result.AddStringIfNotNullOrEmpty(modelConfiguration.Id);
                result.AddStringIfNotNullOrEmpty(modelConfiguration.CloneOf);
                result.AddStringIfNotNullOrEmpty(modelConfiguration.RomOf);
            }

            // Model from emulator
            if (emulator != null)
            {
                result.AddStringIfNotNullOrEmpty(emulator.Model);
                result.AddStringIfNotNullOrEmpty(emulator.Id);
            }

            // Generic model from orientation/year
            if (modelConfiguration != null)
            {
                bool isVertical = modelConfiguration.ScreenOrientation == GameScreenOrientation.Vertical;
                string prefabName = isVertical ? DEFAULT_GAME_VERT_MODEL : DEFAULT_GAME_HOR_MODEL;
                if (int.TryParse(modelConfiguration.Year, out int year))
                {
                    if (year >= 1970 && year < 1980)
                    {
                        prefabName = isVertical ? "default70vert" : "default70hor";
                    }
                    else if (year < 1990)
                    {
                        prefabName = isVertical ? "default80vert" : "default80hor";
                    }
                    else if (year < 2000)
                    {
                        prefabName = isVertical ? "default90vert" : "default90hor";
                    }
                }
                result.Add(prefabName);
            }

            // Default model
            result.Add(DEFAULT_GAME_HOR_MODEL);

            return result;
        }

        public static List<string> GetNamesToTryForProp(ModelConfiguration modelConfiguration, EmulatorConfiguration _)
            => new List<string> { modelConfiguration.Id, DEFAULT_PROP_MODEL };
    }
}
