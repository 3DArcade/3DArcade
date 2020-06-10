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

using System.Linq;
using UnityEngine;

namespace Arcade_r
{
    public sealed class ArcadeContext : FSM.Context<ArcadeState>
    {
        public readonly App App;
        public readonly ArcadeController ArcadeController;
        public readonly LayerMask RaycastLayers;

        public ArcadeConfiguration CurrentArcadeConfiguration { get; private set; }
        public ModelConfigurationComponent CurrentModelConfiguration;

        public ArcadeContext(App app, string startingArcade)
        {
            App              = app;
            ArcadeController = new ArcadeController(app.ArcadeHierarchy, App.GameObjectCache, App.PlayerControls.transform, App.LauncherDatabase, App.ContentListDatabase, App.TextureCache);
            RaycastLayers    = LayerMask.GetMask("Arcade/ArcadeModels", "Arcade/GameModels", "Arcade/PropModels", "UI");

            SetCurrentArcadeConfiguration(startingArcade);
        }

        public bool SetAndStartCurrentArcadeConfiguration(string id)
        {
            SetCurrentArcadeConfiguration(id);
            return StartCurrentArcade();
        }

        public void SetCurrentArcadeConfiguration(string id)
        {
            CurrentArcadeConfiguration = App.ArcadeDatabase.Get(id);
        }

        public bool StartCurrentArcade()
        {
            if (CurrentArcadeConfiguration == null)
            {
                return false;
            }

            ArcadeConfigurationComponent arcadeCfg = App.ArcadeHierarchy.RootNode.gameObject.AddComponentIfNotFound<ArcadeConfigurationComponent>();
            arcadeCfg.FromArcadeConfiguration(CurrentArcadeConfiguration);

            switch (CurrentArcadeConfiguration.ArcadeType)
            {
                case ArcadeType.Fps:
                case ArcadeType.Cyl:
                {
                    App.ArcadeHierarchy.Reset();
                    return ArcadeController.StartArcade(CurrentArcadeConfiguration);
                }
                case ArcadeType.FpsMenu:
                case ArcadeType.CylMenu:
                {
                    return false;
                }
            }

            return false;
        }

        public void ReloadCurrentArcadeConfigurationModels()
        {
            ArcadeConfigurationComponent arcadeCfg = App.ArcadeHierarchy.RootNode.GetComponent<ArcadeConfigurationComponent>();
            arcadeCfg.SetGamesAndPropsTransforms(CurrentArcadeConfiguration.GameModelList, CurrentArcadeConfiguration.PropModelList);
        }

        public void SaveCurrentArcadeConfigurationModels()
        {
            ArcadeConfigurationComponent arcadeCfg = App.ArcadeHierarchy.RootNode.GetComponent<ArcadeConfigurationComponent>();
            arcadeCfg.GetGamesAndProps(out CurrentArcadeConfiguration.GameModelList, out CurrentArcadeConfiguration.PropModelList);
            _ = App.ArcadeDatabase.Save(CurrentArcadeConfiguration);
        }

        public void SaveCurrentArcadeConfiguration()
        {
            ArcadeConfigurationComponent arcadeCfg = App.ArcadeHierarchy.RootNode.GetComponent<ArcadeConfigurationComponent>();
            _ = App.ArcadeDatabase.Save(arcadeCfg.ToArcadeConfiguration(App.PlayerControls.transform, App.Camera));
        }

        public bool GetLauncherAndContentForCurrentModelConfiguration(out LauncherConfiguration launcher, out ContentConfiguration content)
        {
            launcher = null;
            content  = null;

            ContentListConfiguration contentList = App.ContentListDatabase.Get(CurrentModelConfiguration.ContentList);
            if (contentList != null)
            {
                content = contentList.Games.FirstOrDefault(x => x.Id.Equals(CurrentModelConfiguration.Id, System.StringComparison.OrdinalIgnoreCase));
                if (content != null)
                {
                    // Get launcher from content list
                    launcher = App.LauncherDatabase.Get(contentList.Launcher);
                    if (launcher == null)
                    {
                        // Get launcher from content
                        launcher = App.LauncherDatabase.Get(content.Launcher);
                    }
                    return launcher != null;
                }
            }

            return false;
        }
    }
}
