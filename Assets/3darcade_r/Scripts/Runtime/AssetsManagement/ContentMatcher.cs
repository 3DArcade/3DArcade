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
using System.Linq;

namespace Arcade_r
{

    public class ContentMatcher
    {
        public delegate List<string> GetNamesToTryDelegate(ModelConfiguration modelConfiguration, LauncherConfiguration launcher, ContentConfiguration content);

        private const string DEFAULT_ARCADE_MODEL    = "defaultCylinder";
        private const string DEFAULT_GAME_HOR_MODEL  = "default80hor";
        private const string DEFAULT_GAME_VERT_MODEL = "default80vert";
        private const string DEFAULT_PROP_MODEL      = "penguin";

        private readonly Database<LauncherConfiguration> _launcherDatabase;
        private readonly Database<ContentListConfiguration> _contentListDatabase;

        public ContentMatcher(Database<LauncherConfiguration> launcherDatabase, Database<ContentListConfiguration> contentListDatabase)
        {
            _launcherDatabase    = launcherDatabase ?? throw new ArgumentNullException(nameof(launcherDatabase));
            _contentListDatabase = contentListDatabase ?? throw new ArgumentNullException(nameof(contentListDatabase));
        }

        public void GetLauncherAndContentForConfiguration(ModelConfiguration modelConfiguration, out LauncherConfiguration launcher, out ContentConfiguration content)
        {
            launcher = null;
            content  = null;

            if (modelConfiguration.ContentInteraction == ContentInteraction.None
             || modelConfiguration.ContentInteraction == ContentInteraction.MenuConfiguration)
            {
                return;
            }

            ContentListConfiguration contentList = _contentListDatabase.Get(modelConfiguration.ContentList);
            if (contentList != null)
            {
                content = contentList.Games.FirstOrDefault(x => x.Id.Equals(modelConfiguration.Id, StringComparison.OrdinalIgnoreCase));
                if (content != null)
                {
                    // Get launcher from content list
                    launcher = _launcherDatabase.Get(contentList.Launcher);
                    if (launcher == null)
                    {
                        // Get launcher from content
                        launcher = _launcherDatabase.Get(content.Launcher);
                    }
                }
            }
        }

        public static List<string> GetNamesToTryForArcade(ModelConfiguration modelConfiguration, LauncherConfiguration launcher, ContentConfiguration content)
        {
            return new List<string> { modelConfiguration.Id, DEFAULT_ARCADE_MODEL };
        }

        public static List<string> GetNamesToTryForGame(ModelConfiguration modelConfiguration, LauncherConfiguration launcher, ContentConfiguration content)
        {
            List<string> result = new List<string>();

            // Model from content
            if (content != null)
            {
                result.AddStringIfNotNullOrEmpty(modelConfiguration.Model);
                result.AddStringIfNotNullOrEmpty(content.Id);
                result.AddStringIfNotNullOrEmpty(content.CloneOf);
                result.AddStringIfNotNullOrEmpty(content.RomOf);
            }

            // Model from launcher
            if (launcher != null)
            {
                result.AddStringIfNotNullOrEmpty(launcher.Model);
                result.AddStringIfNotNullOrEmpty(launcher.Id);
            }

            // Generic model from orientation/year
            if (content != null)
            {
                bool isVertical = content.Orientation == ContentOrientation.Vertical;
                string prefabName = isVertical ? DEFAULT_GAME_VERT_MODEL : DEFAULT_GAME_HOR_MODEL;
                if (int.TryParse(content.Year, out int year))
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

        public static List<string> GetNamesToTryForProp(ModelConfiguration modelConfiguration, LauncherConfiguration launcher, ContentConfiguration content)
        {
            return new List<string> { modelConfiguration.Id, DEFAULT_PROP_MODEL };
        }
    }
}
