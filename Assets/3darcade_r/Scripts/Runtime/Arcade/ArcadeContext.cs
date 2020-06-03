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

using Cinemachine;
using UnityEngine;

namespace Arcade_r
{
    public sealed class ArcadeContext : FSM.Context<ArcadeState>
    {
        public readonly App App;
        public readonly LayerMask RaycastLayers;

        public string CurrentArcadeId;
        public IInteractable CurrentInteractable;
        public IGrabbable CurrentGrabbable;

        private readonly ArcadeController _arcadeController;

        public ArcadeContext(App app, string startingArcade)
        {
            App             = app;
            CurrentArcadeId = startingArcade;
            RaycastLayers   = LayerMask.GetMask("Arcade/ArcadeModels", "Arcade/GameModels", "Arcade/PropModels", "UI");

            _arcadeController = new ArcadeController(App.ArcadeHierarchy, App.GameObjectCache, App.EmulatorManager, App.DiskTextureCache, App.PlayerControls.transform);
        }

        public void SaveCurrentArcadeConfiguration()
        {
            ArcadeConfigurationComponent cfgComponent = App.ArcadeHierarchy.RootNode.GetComponent<ArcadeConfigurationComponent>();
            if (cfgComponent == null)
            {
                return;
            }

            Transform player = App.PlayerControls.transform;
            Camera mainCamera = App.Camera;
            CinemachineVirtualCamera vCamera = player.GetComponentInChildren<CinemachineVirtualCamera>();

            _ = App.ArcadeManager.Save(cfgComponent.ToArcadeConfiguration(player, mainCamera, vCamera));
        }

        public void StartCurrentArcade()
        {
            ArcadeConfiguration arcadeConfiguration = App.ArcadeManager.Get(CurrentArcadeId);
            if (arcadeConfiguration != null)
            {
                App.ArcadeHierarchy.Reset();
                _arcadeController.StartArcade(arcadeConfiguration);
            }
        }

        public void ReloadCurrentArcadeConfigurationModels()
        {
            ArcadeConfigurationComponent cfgComponent = App.ArcadeHierarchy.RootNode.GetComponent<ArcadeConfigurationComponent>();
            if (cfgComponent == null)
            {
                return;
            }

            ArcadeConfiguration cfg = App.ArcadeManager.Get(cfgComponent.Id);
            cfgComponent.SetGamesAndPropsTransforms(cfg.GameModelList, cfg.PropModelList);
        }

        public void SaveCurrentArcadeConfigurationModels()
        {
            ArcadeConfigurationComponent cfgComponent = App.ArcadeHierarchy.RootNode.GetComponent<ArcadeConfigurationComponent>();
            if (cfgComponent == null)
            {
                return;
            }

            (ModelConfiguration[] games, ModelConfiguration[] props) = cfgComponent.GetGamesAndProps();

            ArcadeConfiguration cfg = App.ArcadeManager.Get(cfgComponent.Id);
            cfg.GameModelList = games;
            cfg.PropModelList = props;
            _ = App.ArcadeManager.Save(cfg);
        }
    }
}
