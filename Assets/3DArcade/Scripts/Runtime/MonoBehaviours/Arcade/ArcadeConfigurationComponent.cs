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
using UnityEngine.Assertions;

namespace Arcade
{
    public static class Defaults
    {
        public static CameraSettings CameraSettings => new CameraSettings();
        public static FpsArcadeProperties FpsArcadeProperties => new FpsArcadeProperties { CameraSettings = CameraSettings };
        public static CylArcadeProperties CylArcadeProperties => new CylArcadeProperties { CameraSettings = CameraSettings };
    }

    [DisallowMultipleComponent]
    public sealed class ArcadeConfigurationComponent : MonoBehaviour
    {
        public string DescriptiveName                  = default;
        public string Id                               = default;
        public RenderSettings RenderSettings           = default;
        public AudioSettings AudioSettings             = default;
        public FpsArcadeProperties FpsArcadeProperties = default;
        public CylArcadeProperties CylArcadeProperties = default;

        public bool Save(Database<ArcadeConfiguration> arcadeDatabase, CameraSettings fpsCameraSettings, CameraSettings cylCameraSettings, bool saveGameTransforms)
        {
            GetChildNodes(out Transform tGames, out Transform tProps);

            ArcadeConfiguration cfg = new ArcadeConfiguration
            {
                DescriptiveName     = DescriptiveName,
                Id                  = Id,
                RenderSettings      = RenderSettings,
                FpsArcadeProperties = FpsArcadeProperties ?? Defaults.FpsArcadeProperties,
                CylArcadeProperties = CylArcadeProperties ?? Defaults.CylArcadeProperties,
                GameModelList       = saveGameTransforms ? GetModelConfigurations(tGames) : arcadeDatabase.Get(Id).GameModelList,
                PropModelList       = GetModelConfigurations(tProps)
            };

            if (fpsCameraSettings != null)
            {
                cfg.FpsArcadeProperties.CameraSettings = fpsCameraSettings;
            }

            if (cylCameraSettings != null)
            {
                cfg.CylArcadeProperties.CameraSettings = cylCameraSettings;
            }

            return arcadeDatabase.Save(cfg);
        }

        public bool SaveModelsOnly(Database<ArcadeConfiguration> arcadeDatabase, ArcadeConfiguration cfg)
        {
            GetGamesAndProps(out cfg.GameModelList, out cfg.PropModelList);
            return arcadeDatabase.Save(cfg);
        }

        public void Restore(ArcadeConfiguration cfg)
        {
            DescriptiveName     = cfg.DescriptiveName;
            Id                  = cfg.Id;
            RenderSettings      = cfg.RenderSettings;
            FpsArcadeProperties = cfg.FpsArcadeProperties ?? Defaults.FpsArcadeProperties;
            CylArcadeProperties = cfg.CylArcadeProperties ?? Defaults.CylArcadeProperties;
        }

        public void SetGamesAndPropsTransforms(ArcadeConfiguration cfg) => SetGamesAndPropsTransforms(cfg.GameModelList, cfg.PropModelList);

        public void SetGamesAndPropsTransforms(ModelConfiguration[] games, ModelConfiguration[] props)
        {
            GetChildNodes(out Transform tGames, out Transform tProps);
            SetModelTransforms(tGames, games);
            SetModelTransforms(tProps, props);
        }

        private void GetGamesAndProps(out ModelConfiguration[] games, out ModelConfiguration[] props)
        {
            GetChildNodes(out Transform tGames, out Transform tProps);
            games = GetModelConfigurations(tGames);
            props = GetModelConfigurations(tProps);
        }

        private void GetChildNodes(out Transform tGames, out Transform tProps)
        {
            Assert.IsTrue(transform.childCount >= 2);

            tGames   = transform.GetChild(0);
            tProps   = transform.GetChild(1);

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
