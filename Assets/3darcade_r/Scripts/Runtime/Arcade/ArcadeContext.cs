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

using UnityEngine;

namespace Arcade_r
{
    public sealed class ArcadeContext : FSM.Context<ArcadeState>
    {
        public readonly App App;
        public readonly ArcadeController ArcadeController;
        public readonly VideoPlayerController VideoPlayerController;
        public readonly LayerMask RaycastLayers;

        public ArcadeConfiguration CurrentArcadeConfiguration { get; private set; }
        public ModelConfigurationComponent CurrentModelConfiguration;

        private static readonly LayerMask _interactionLayers  = LayerMask.GetMask("Arcade/ArcadeModels", "Arcade/GameModels", "Arcade/PropModels");
        private static readonly LayerMask _videoControlLayers = LayerMask.GetMask("Arcade/GameModels", "Arcade/PropModels");

        public ArcadeContext(App app, string startingArcade)
        {
            App                   = app;
            ArcadeController      = new ArcadeController(app.ArcadeHierarchy, App.GameObjectCache, App.PlayerControls.transform, App.EmulatorDatabase, App.TextureCache, App.VideoCache);
            VideoPlayerController = new VideoPlayerController(app.PlayerControls.transform, _videoControlLayers);
            RaycastLayers         = _interactionLayers;

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

            if (!App.ArcadeHierarchy.RootNode.TryGetComponent(out ArcadeConfigurationComponent cfgComponent))
            {
                return false;
            }

            cfgComponent.Restore(CurrentArcadeConfiguration);

            switch (cfgComponent.ArcadeType)
            {
                case ArcadeType.FpsArcade:
                case ArcadeType.CylArcade:
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

        public bool SaveCurrentArcadeConfiguration()
        {
            ArcadeConfigurationComponent cfgComponent = App.ArcadeHierarchy.RootNode.GetComponent<ArcadeConfigurationComponent>();
            return cfgComponent != null && cfgComponent.Save(App.ArcadeDatabase, App.PlayerControls.transform, App.Camera);
        }

        public bool SaveCurrentArcadeConfigurationModels()
        {
            ArcadeConfigurationComponent cfgComponent = App.ArcadeHierarchy.RootNode.GetComponent<ArcadeConfigurationComponent>();
            return cfgComponent != null && cfgComponent.SaveModelsOnly(App.ArcadeDatabase, CurrentArcadeConfiguration);
        }

        public void ReloadCurrentArcadeConfigurationModels()
        {
            if (App.ArcadeHierarchy.RootNode.TryGetComponent(out ArcadeConfigurationComponent cfgComponent))
            {
                cfgComponent.SetGamesAndPropsTransforms(CurrentArcadeConfiguration);
            }
        }

        public EmulatorConfiguration GetEmulatorForCurrentModelConfiguration() => App.EmulatorDatabase.Get(CurrentModelConfiguration.Emulator);
    }
}
