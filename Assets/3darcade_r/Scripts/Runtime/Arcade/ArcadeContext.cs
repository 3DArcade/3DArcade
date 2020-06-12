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
using System;
using UnityEngine;

namespace Arcade_r
{
    public sealed class ArcadeContext : FSM.Context<ArcadeState>
    {
        public readonly App App;
        public readonly ArcadeController ArcadeController;
        public readonly VideoPlayerController VideoPlayerController;
        public readonly LayerMask RaycastLayers;

        public ModelConfigurationComponent CurrentModelConfiguration;

        public ArcadeConfiguration CurrentArcadeConfiguration { get; private set; }
        public ArcadeType CurrentArcadeType { get; private set; }

        private static readonly LayerMask _interactionLayers  = LayerMask.GetMask("Arcade/ArcadeModels", "Arcade/GameModels", "Arcade/PropModels");
        private static readonly LayerMask _videoControlLayers = LayerMask.GetMask("Arcade/GameModels", "Arcade/PropModels");

        public ArcadeContext(App app, GeneralConfiguration generalConfiguration)
        {
            App                   = app;
            ArcadeController      = new ArcadeController(app.ArcadeHierarchy, App.GameObjectCache, App.PlayerControls.transform, App.EmulatorDatabase, App.TextureCache, App.VideoCache);
            VideoPlayerController = new VideoPlayerController(app.PlayerControls.transform, _videoControlLayers);
            RaycastLayers         = _interactionLayers;

            SetCurrentArcadeConfiguration(generalConfiguration.StartingArcade, generalConfiguration.StartingArcadeType);
        }

        public bool SetAndStartCurrentArcadeConfiguration(string id, ArcadeType type)
        {
            SetCurrentArcadeConfiguration(id, type);
            return StartCurrentArcade();
        }

        public void SetCurrentArcadeConfiguration(string id, ArcadeType type)
        {
            CurrentArcadeConfiguration = App.ArcadeDatabase.Get(id);
            CurrentArcadeType          = type;
        }

        public bool StartCurrentArcade()
        {
            if (CurrentArcadeConfiguration == null)
            {
                return false;
            }

            VideoPlayerController.StopAllVideos();

            ArcadeConfigurationComponent cfgComponent = App.ArcadeHierarchy.RootNode.gameObject.AddComponentIfNotFound<ArcadeConfigurationComponent>();
            cfgComponent.Restore(CurrentArcadeConfiguration);

            switch (CurrentArcadeType)
            {
                case ArcadeType.Fps:
                case ArcadeType.Cyl:
                {
                    App.ArcadeHierarchy.Reset();
                    return ArcadeController.StartArcade(CurrentArcadeConfiguration, CurrentArcadeType);
                }
            }

            return false;
        }

        public bool SaveCurrentArcadeConfiguration()
        {
            ArcadeConfigurationComponent cfgComponent = App.ArcadeHierarchy.RootNode.GetComponent<ArcadeConfigurationComponent>();

            CinemachineVirtualCamera vCamera = App.PlayerControls.GetComponentInChildren<CinemachineVirtualCamera>();

            CameraSettings cameraSettings = new CameraSettings
            {
                Position      = App.PlayerControls.transform.position,
                Rotation      = MathUtils.CorrectEulerAngles(App.Camera.transform.eulerAngles),
                Height        = vCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y,
                Orthographic  = App.Camera.orthographic,
                FieldOfView   = vCamera.m_Lens.FieldOfView,
                AspectRatio   = vCamera.m_Lens.OrthographicSize,
                NearClipPlane = vCamera.m_Lens.NearClipPlane,
                FarClipPlane  = vCamera.m_Lens.FarClipPlane,
                ViewportRect  = App.Camera.rect
            };

            if (CurrentArcadeType == ArcadeType.Fps)
            {
                return cfgComponent != null && cfgComponent.Save(App.ArcadeDatabase, cameraSettings, null);
            }
            else if (CurrentArcadeType == ArcadeType.Cyl)
            {
                return cfgComponent != null && cfgComponent.Save(App.ArcadeDatabase, null, cameraSettings);
            }

            return false;
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
