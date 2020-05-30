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
    [DisallowMultipleComponent]
    public sealed class ArcadeConfigurationComponent : MonoBehaviour
    {
        [SerializeField] private string _descriptiveName                    = default;
        [SerializeField] private string _id                                 = default;
        [SerializeField] private ArcadeType _arcadeType                     = default;
        [SerializeField] private RenderSettings _renderSettings             = default;
        [SerializeField] private AudioSettings _audioSettings               = default;
        [SerializeField] private VideoSettings _videoSettings               = default;
        [SerializeField] private FpsArcadeProperties[] _fpsArcadeProperties = default;
        [SerializeField] private CylArcadeProperties[] _cylArcadeProperties = default;
        [SerializeField] private Zone[] _zones                              = default;

        public string Id => _id;

        public ArcadeConfiguration ToArcadeConfiguration(Transform player, Camera mainCamera, CinemachineVirtualCamera vCamera)
        {
            return new ArcadeConfiguration
            {
                DescriptiveName = _descriptiveName,
                Id              = _id,
                ArcadeType      = _arcadeType.ToString(),
                RenderSettings  = _renderSettings,
                AudioSettings   = _audioSettings,
                VideoSettings   = _videoSettings,
                CameraSettings  = new CameraSettings
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
                },
                FpsArcadeProperties   = _fpsArcadeProperties,
                CylArcadeProperties   = _cylArcadeProperties,
                Zones                 = _zones,
                ArcadeModelList       = GetModelConfigurations(transform.GetChild(0)),
                GameModelList         = GetModelConfigurations(transform.GetChild(1)),
                PropModelList         = GetModelConfigurations(transform.GetChild(2)),
            };
        }

        public void FromArcadeConfiguration(ArcadeConfiguration arcadeConfiguration)
        {
            _descriptiveName       = arcadeConfiguration.DescriptiveName;
            _id                    = arcadeConfiguration.Id;
            if (!System.Enum.TryParse(arcadeConfiguration.ArcadeType, true, out _arcadeType))
            {
                _arcadeType = ArcadeType.None;
            }
            _renderSettings        = arcadeConfiguration.RenderSettings;
            _audioSettings         = arcadeConfiguration.AudioSettings;
            _videoSettings         = arcadeConfiguration.VideoSettings;
            _fpsArcadeProperties   = arcadeConfiguration.FpsArcadeProperties;
            _cylArcadeProperties   = arcadeConfiguration.CylArcadeProperties;
            _zones                 = arcadeConfiguration.Zones;
        }

        private static ModelConfiguration[] GetModelConfigurations(Transform node)
        {
            ModelConfiguration[] result = new ModelConfiguration[node.childCount];

            for (int i = 0; i < result.Length; ++i)
            {
                Transform child = node.GetChild(i);
                ModelSetup modelSetup = child.GetComponent<ModelSetup>();
                result[i] = modelSetup.ToModelConfiguration();
            }

            return result;
        }
    }
}
