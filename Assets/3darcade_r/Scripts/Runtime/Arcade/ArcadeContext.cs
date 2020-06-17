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

        public ModelConfigurationComponent CurrentModelConfiguration;

        public ArcadeConfiguration CurrentArcadeConfiguration { get; private set; }
        public ArcadeType CurrentArcadeType { get; private set; }

        private static readonly LayerMask _interactionLayers  = LayerMask.GetMask("Arcade/ArcadeModels", "Arcade/GameModels", "Arcade/PropModels");

        public ArcadeContext(App app, GeneralConfiguration generalConfiguration)
        {
            App           = app;
            RaycastLayers = _interactionLayers;

            _ = SetCurrentArcadeConfiguration(generalConfiguration.StartingArcade, generalConfiguration.StartingArcadeType);
        }

        public bool SetCurrentArcadeConfiguration(string id, ArcadeType type)
        {
            CurrentArcadeConfiguration = App.ArcadeDatabase.Get(id);
            CurrentArcadeType          = type;
            return CurrentArcadeConfiguration != null;
        }

        public void SetAndStartCurrentArcadeConfiguration(string id, ArcadeType type)
        {
            if (SetCurrentArcadeConfiguration(id, type))
            {
                TransitionTo<ArcadeLoadState>();
            }
        }

        public bool StartCurrentArcade()
        {
            if (CurrentArcadeConfiguration == null)
            {
                return false;
            }

            App.VideoPlayerController.StopAllVideos();

            ArcadeConfigurationComponent cfgComponent = App.ArcadeHierarchy.RootNode.gameObject.AddComponentIfNotFound<ArcadeConfigurationComponent>();
            cfgComponent.Restore(CurrentArcadeConfiguration);

            App.ArcadeHierarchy.Reset();
            if (CurrentArcadeType == ArcadeType.Fps)
            {
                return App.ArcadeFpsController.StartArcade(CurrentArcadeConfiguration);
            }
            else if (CurrentArcadeType == ArcadeType.Cyl)
            {
                return App.ArcadeCylController.StartArcade(CurrentArcadeConfiguration);
            }

            return false;
        }

        public bool SaveCurrentArcadeConfiguration()
        {
            ArcadeConfigurationComponent cfgComponent = App.ArcadeHierarchy.RootNode.GetComponent<ArcadeConfigurationComponent>();
            if (cfgComponent == null)
            {
                return false;
            }

            Camera fpsCamera                          = App.PlayerFpsControls.Camera;
            CinemachineVirtualCamera fpsVirtualCamera = App.PlayerFpsControls.VirtualCamera;
            CameraSettings fpsCameraSettings          = new CameraSettings
            {
                Position      = App.PlayerFpsControls.transform.position,
                Rotation      = MathUtils.CorrectEulerAngles(fpsCamera.transform.eulerAngles),
                Height        = fpsVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y,
                Orthographic  = fpsCamera.orthographic,
                FieldOfView   = fpsVirtualCamera.m_Lens.FieldOfView,
                AspectRatio   = fpsVirtualCamera.m_Lens.OrthographicSize,
                NearClipPlane = fpsVirtualCamera.m_Lens.NearClipPlane,
                FarClipPlane  = fpsVirtualCamera.m_Lens.FarClipPlane,
                ViewportRect  = fpsCamera.rect
            };

            Camera cylCamera                          = App.PlayerCylControls.Camera;
            CinemachineVirtualCamera cylVirtualCamera = App.PlayerCylControls.VirtualCamera;
            CameraSettings cylCameraSettings          = new CameraSettings
            {
                Position      = App.PlayerCylControls.transform.position,
                Rotation      = MathUtils.CorrectEulerAngles(cylCamera.transform.eulerAngles),
                Height        = cylVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y,
                Orthographic  = cylCamera.orthographic,
                FieldOfView   = cylVirtualCamera.m_Lens.FieldOfView,
                AspectRatio   = cylVirtualCamera.m_Lens.OrthographicSize,
                NearClipPlane = cylVirtualCamera.m_Lens.NearClipPlane,
                FarClipPlane  = cylVirtualCamera.m_Lens.FarClipPlane,
                ViewportRect  = cylCamera.rect
            };

            return cfgComponent.Save(App.ArcadeDatabase, fpsCameraSettings, cylCameraSettings);
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
