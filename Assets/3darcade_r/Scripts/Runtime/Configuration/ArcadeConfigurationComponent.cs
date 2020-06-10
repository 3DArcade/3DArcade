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
using UnityEngine.Assertions;

namespace Arcade_r
{
    [DisallowMultipleComponent]
    public sealed class ArcadeConfigurationComponent : MonoBehaviour
    {
        public string DescriptiveName            = default;
        public string Id                         = default;
        public ArcadeType ArcadeType             = default;
        public RenderSettings RenderSettings     = default;
        public AudioSettings AudioSettings       = default;
        public VideoSettings VideoSettings       = default;
        public FpsProperties FpsArcadeProperties = default;
        public CylProperties CylArcadeProperties = default;
        public Zone[] Zones                      = default;

        public void FromArcadeConfiguration(ArcadeConfiguration cfg)
        {
            DescriptiveName     = cfg.DescriptiveName;
            Id                  = cfg.Id;
            ArcadeType          = cfg.ArcadeType;
            RenderSettings      = cfg.RenderSettings;
            AudioSettings       = cfg.AudioSettings;
            VideoSettings       = cfg.VideoSettings;
            FpsArcadeProperties = cfg.FpsProperties;
            CylArcadeProperties = cfg.CylProperties;
            Zones               = cfg.Zones;
        }

        public ArcadeConfiguration ToArcadeConfiguration(Transform player, Camera mainCamera)
        {
            GetChildNodes(out Transform tArcades, out Transform tGames, out Transform tProps);

            ArcadeConfiguration result = new ArcadeConfiguration
            {
                DescriptiveName = DescriptiveName,
                Id              = Id,
                ArcadeType      = ArcadeType,
                RenderSettings  = RenderSettings,
                AudioSettings   = AudioSettings,
                VideoSettings   = VideoSettings,
                FpsProperties   = FpsArcadeProperties,
                CylProperties   = CylArcadeProperties,
                Zones           = Zones,
                ArcadeModelList = GetModelConfigurations(tArcades),
                GameModelList   = GetModelConfigurations(tGames),
                PropModelList   = GetModelConfigurations(tProps)
            };

            CinemachineVirtualCamera vCamera = player.GetComponentInChildren<CinemachineVirtualCamera>();

            result.FpsProperties.CameraSettings = new CameraSettings
            {
                Position      = player.position,
                Rotation      = MathUtils.CorrectEulerAngles(mainCamera.transform.eulerAngles),
                Height        = vCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y,
                Orthographic  = mainCamera.orthographic,
                FieldOfView   = vCamera.m_Lens.FieldOfView,
                AspectRatio   = vCamera.m_Lens.OrthographicSize,
                NearClipPlane = vCamera.m_Lens.NearClipPlane,
                FarClipPlane  = vCamera.m_Lens.FarClipPlane,
                ViewportRect  = mainCamera.rect
            };

            return result;
        }

        public void GetGamesAndProps(out ModelConfiguration[] games, out ModelConfiguration[] props)
        {
            GetChildNodes(out Transform _, out Transform tGames, out Transform tProps);
            games = GetModelConfigurations(tGames);
            props = GetModelConfigurations(tProps);
        }

        public void SetGamesAndPropsTransforms(ModelConfiguration[] games, ModelConfiguration[] props)
        {
            GetChildNodes(out Transform _, out Transform tGames, out Transform tProps);
            SetModelTransforms(tGames, games);
            SetModelTransforms(tProps, props);
        }

        private void GetChildNodes(out Transform tArcades, out Transform tGames, out Transform tProps)
        {
            Assert.IsTrue(transform.childCount >= 3);

            tArcades = transform.GetChild(0);
            tGames   = transform.GetChild(1);
            tProps   = transform.GetChild(2);

            Assert.IsTrue(tArcades.name == "ArcadeModels");
            Assert.IsTrue(tGames.name == "GameModels");
            Assert.IsTrue(tProps.name == "PropModels");
        }

        private static ModelConfiguration[] GetModelConfigurations(Transform node)
        {
            ModelConfiguration[] result = new ModelConfiguration[node.childCount];

            for (int i = 0; i < result.Length; ++i)
            {
                Transform child = node.GetChild(i);
                ModelConfigurationComponent modelSetup = child.GetComponent<ModelConfigurationComponent>();
                result[i] = modelSetup.ToModelConfiguration();
            }

            return result;
        }

        private static void SetModelTransforms(Transform node, ModelConfiguration[] modelConfigurations)
        {
            for (int i = 0; i < node.childCount; ++i)
            {
                Transform child = node.GetChild(i);
                ModelConfiguration modelConfiguration = modelConfigurations[i];
                child.SetPositionAndRotation(modelConfiguration.Position, Quaternion.Euler(modelConfiguration.Rotation));
                child.localScale = modelConfiguration.Scale;
            }
        }
    }
}
