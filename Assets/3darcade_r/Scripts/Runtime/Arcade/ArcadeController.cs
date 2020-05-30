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
    public static class ArcadeController
    {
        private static readonly string[] RESOURCES_SUB_DIRECTORIES = new string[] { "Arcades", "Games", "Props" };
        private static readonly string[] GAME_RESOURCES_DIRECTORY  = new string[] { "Games" };

        public static void StartArcade(ArcadeConfiguration configuration, Transform root, Transform player)
        {
            Assert.IsNotNull(configuration);
            Assert.IsNotNull(root);
            Assert.IsNotNull(player);

            if (root.TryGetComponent(out ArcadeConfigurationComponent arcadeConfigurationComponent))
            {
                arcadeConfigurationComponent.FromArcadeConfiguration(configuration);
            }
            else
            {
                root.gameObject.AddComponent<ArcadeConfigurationComponent>()
                               .FromArcadeConfiguration(configuration);
            }

            SetupPlayer(player, configuration.CameraSettings.Position, configuration.CameraSettings.Rotation, configuration.CameraSettings.Height);

            AddModelsToWorld<ArcadeModelSetup>(configuration.ArcadeModelList, root.GetChild(0));
            AddModelsToWorld<GameModelSetup>(configuration.GameModelList, root.GetChild(1));
            AddModelsToWorld<PropModelSetup>(configuration.PropModelList, root.GetChild(2));
        }

        private static void SetupPlayer(Transform player, Vector3 position, Vector3 rotation, float height)
        {
            player.SetPositionAndRotation(position, Quaternion.Euler(0f, rotation.y, 0f));
            CinemachineVirtualCamera vCam    = player.GetComponentInChildren<CinemachineVirtualCamera>();
            CinemachineTransposer transposer = vCam.GetCinemachineComponent<CinemachineTransposer>();
            transposer.m_FollowOffset.y      = height;
        }

        private static void AddModelsToWorld<T>(ModelConfiguration[] models, Transform parent)
            where T : ModelSetup
        {
            foreach (ModelConfiguration modelConfiguration in models)
            {
                GameObject prefab = AssetManager.LoadPrefab(RESOURCES_SUB_DIRECTORIES, modelConfiguration.Model, modelConfiguration.Id, modelConfiguration.IdParent);
                if (prefab == null)
                {
                    // Generic model
                    bool isVertical = modelConfiguration.Screen.ToLowerInvariant().Contains("vertical");
                    _ = int.TryParse(modelConfiguration.Year, out int year);
                    if (year >= 1970 && year < 1980)
                    {
                        prefab = AssetManager.LoadPrefab(GAME_RESOURCES_DIRECTORY, isVertical ? "default70vert" : "default70hor");
                    }
                    else if (year < 1990)
                    {
                        prefab = AssetManager.LoadPrefab(GAME_RESOURCES_DIRECTORY, isVertical ? "default80vert" : "default80hor");
                    }
                    else if (year < 2000)
                    {
                        prefab = AssetManager.LoadPrefab(GAME_RESOURCES_DIRECTORY, isVertical ? "default90vert" : "default90hor");
                    }
                    else
                    {
                        prefab = AssetManager.LoadPrefab(GAME_RESOURCES_DIRECTORY, "default80hor");
                    }
                }

                Assert.IsNotNull(prefab);

                MaterialUtils.SetGPUInstancing(true, prefab);
                InstantiatePrefab<T>(prefab, modelConfiguration, parent);
            }
        }

        private static void InstantiatePrefab<T>(GameObject prefab, ModelConfiguration modelConfiguration, Transform parent)
            where T : ModelSetup
        {
            GameObject model = Object.Instantiate(prefab, modelConfiguration.Position, Quaternion.Euler(modelConfiguration.Rotation), parent);
            model.StripCloneFromName();
            model.transform.localScale = modelConfiguration.Scale;
            model.transform.SetLayersRecursively(parent.gameObject.layer);

            model.AddComponent<T>()
                 .FromModelConfiguration(modelConfiguration);
        }
    }
}
