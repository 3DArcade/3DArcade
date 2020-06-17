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

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Arcade_r
{
    public sealed class ArcadeCylController : ArcadeController
    {
        private readonly List<ModelConfigurationComponent> _allGames;

        private CylArcadeProperties _cylArcadeProperties;
        private int _currentIndex;

        public ArcadeCylController(ArcadeHierarchy arcadeHierarchy,
                                   PlayerFpsControls playerFpsControls,
                                   PlayerCylControls playerCylControls,
                                   Database<EmulatorConfiguration> emulatorDatabase,
                                   AssetCache<GameObject> gameObjectCache,
                                   AssetCache<Texture> textureCache,
                                   AssetCache<string> videoCache)
        : base(arcadeHierarchy, playerFpsControls, playerCylControls, emulatorDatabase, gameObjectCache, textureCache, videoCache)
        {
            _allGames = new List<ModelConfigurationComponent>();
        }

        public override bool StartArcade(ArcadeConfiguration arcadeConfiguration)
        {
            Assert.IsNotNull(arcadeConfiguration);
            Assert.IsNotNull(arcadeConfiguration.CylArcadeProperties);

            _playerFpsControls.gameObject.SetActive(false);
            _playerCylControls.gameObject.SetActive(true);

            SetupPlayer(_playerCylControls, arcadeConfiguration.CylArcadeProperties.CameraSettings);

            SetupWorld(arcadeConfiguration);

            return true;
        }

        public static void RotateLeft<T>(IList<T> elements)
        {
            T first = elements[0];
            for (int i = 1; i < elements.Count; ++i)
            {
                elements[i - 1] = elements[i];
            }
            elements[elements.Count - 1] = first;
        }

        public static void RotateRight<T>(IList<T> elements)
        {
            T last = elements[elements.Count - 1];
            for (int i = elements.Count - 2; i >= 0; --i)
            {
                elements[i + 1] = elements[i];
            }
            elements[0] = last;
        }

        protected override void SetupWorld(ArcadeConfiguration arcadeConfiguration)
        {
            RenderSettings renderSettings = arcadeConfiguration.RenderSettings;
            AddModelsToWorld(arcadeConfiguration.ArcadeModelList, _arcadeHierarchy.ArcadesNode, renderSettings, ARCADE_RESOURCES_DIRECTORY, ContentMatcher.GetNamesToTryForArcade);
            AddModelsToWorld(arcadeConfiguration.PropModelList, _arcadeHierarchy.PropsNode, renderSettings, PROP_RESOURCES_DIRECTORY, ContentMatcher.GetNamesToTryForProp);

            AddGameModelsToWorld(arcadeConfiguration.GameModelList, renderSettings, GAME_RESOURCES_DIRECTORY, ContentMatcher.GetNamesToTryForGame);

            _cylArcadeProperties = arcadeConfiguration.CylArcadeProperties;
            _currentIndex = 0;

            _cylArcadeProperties.Sprockets = Mathf.Max(1, _cylArcadeProperties.Sprockets);

            int initialSelectionCount;
            if (_cylArcadeProperties.SelectedSprocket < 0)
            {
                initialSelectionCount = (_cylArcadeProperties.Sprockets / 2) + 1;
            }
            else
            {
                initialSelectionCount = _cylArcadeProperties.SelectedSprocket;
            }

            for (int i = 0; i < initialSelectionCount - 1; ++i)
            {
                RotateRight(_allGames);
            }

            UpdateWheel();
        }

        private void UpdateWheel()
        {
            _arcadeHierarchy.GamesNode.position = Vector3.zero;

            if (_currentIndex >= _allGames.Count)
            {
                _currentIndex = 0;
            }
            else if (_currentIndex < 0)
            {
                _currentIndex = _allGames.Count - 1;
            }

            IEnumerable<ModelConfigurationComponent> toShow = _allGames.Take(_cylArcadeProperties.Sprockets);
            IEnumerable<ModelConfigurationComponent> toHide = _allGames.Except(toShow);

            foreach (ModelConfigurationComponent item in toHide)
            {
                item.gameObject.SetActive(false);
            }

            foreach (ModelConfigurationComponent item in toShow)
            {
                item.gameObject.SetActive(true);
            }

            ModelConfigurationComponent[] rightPart = toShow.Skip(_cylArcadeProperties.Sprockets / 2).ToArray();
            ModelConfigurationComponent[] leftPart = toShow.Except(rightPart).ToArray();

            Vector3 position = new Vector3(0f, 0f, _cylArcadeProperties.WheelRadius);
            float angle = 0f;
            for (int i = 0; i < rightPart.Length; ++i)
            {
                ModelConfigurationComponent currentModel = rightPart[i];
                currentModel.gameObject.SetActive(true);
                currentModel.transform.SetPositionAndRotation(position, Quaternion.Euler(_cylArcadeProperties.SprocketRotation));

                if (i > 0)
                {
                    ModelConfigurationComponent previousModel = rightPart[i - 1];

                    Collider previousCollider = previousModel.GetComponent<Collider>();
                    float previousHalfWidth   = previousCollider.bounds.extents.x;

                    Collider currentCollider = currentModel.GetComponent<Collider>();
                    float currentHalfWidth   = currentCollider.bounds.extents.x;

                    float width = previousHalfWidth + currentHalfWidth + _cylArcadeProperties.ModelSpacing;

                    angle += (width / _cylArcadeProperties.WheelRadius) * Mathf.Rad2Deg;

                    currentModel.transform.RotateAround(Vector3.zero, Vector3.up, angle);
                }
            }

            ModelConfigurationComponent centerModel = rightPart[0];

            angle = 0f;
            for (int i = leftPart.Length - 1; i >= 0; --i)
            {
                ModelConfigurationComponent currentModel = leftPart[i];
                currentModel.gameObject.SetActive(true);
                currentModel.transform.SetPositionAndRotation(position, Quaternion.Euler(_cylArcadeProperties.SprocketRotation));

                if (i < leftPart.Length - 2)
                {
                    centerModel = leftPart[i + 1];
                }

                Collider previousCollider = centerModel.GetComponent<Collider>();
                float previousHalfWidth   = previousCollider.bounds.extents.x;

                Collider currentCollider = currentModel.GetComponent<Collider>();
                float currentHalfWidth   = currentCollider.bounds.extents.x;

                float width = previousHalfWidth + currentHalfWidth + _cylArcadeProperties.ModelSpacing;

                angle -= (width / _cylArcadeProperties.WheelRadius) * Mathf.Rad2Deg;

                currentModel.transform.RotateAround(Vector3.zero, Vector3.up, angle);
            }

            _arcadeHierarchy.GamesNode.position = new Vector3(0f, 0f, -_cylArcadeProperties.WheelOffsetZ);
        }

        public void Forward(int count)
        {
            _currentIndex += count;
            for (int i = 0; i < count; ++i)
            {
                RotateLeft(_allGames);
            }
            UpdateWheel();
        }

        public void Backward(int count)
        {
            _currentIndex -= count;
            for (int i = 0; i < count; ++i)
            {
                RotateRight(_allGames);
            }
            UpdateWheel();
        }

        private void AddGameModelsToWorld(IEnumerable<ModelConfiguration> modelConfigurations, RenderSettings renderSettings, string resourceDirectory, ContentMatcher.GetNamesToTryDelegate getNamesToTry)
        {
            _allGames.Clear();

            Vector3 position = new Vector3(0f, 0f, -1000f);

            foreach (ModelConfiguration modelConfiguration in modelConfigurations)
            {
                EmulatorConfiguration emulator = _contentMatcher.GetEmulatorForConfiguration(modelConfiguration);
                List<string> namesToTry        = getNamesToTry(modelConfiguration, emulator);

                GameObject prefab = _gameObjectCache.Load(resourceDirectory, namesToTry);
                Assert.IsNotNull(prefab, "prefab is null!");

                GameObject instantiatedModel = Object.Instantiate(prefab, position, Quaternion.Euler(0f, 180f, 0f), _arcadeHierarchy.GamesNode);
                instantiatedModel.StripCloneFromName();
                instantiatedModel.transform.localScale = modelConfiguration.Scale;
                instantiatedModel.transform.SetLayersRecursively(_arcadeHierarchy.GamesNode.gameObject.layer);
                ModelConfigurationComponent modelConfigurationComponent = instantiatedModel.AddComponent<ModelConfigurationComponent>();
                modelConfigurationComponent.FromModelConfiguration(modelConfiguration);

                _allGames.Add(modelConfigurationComponent);

                if (instantiatedModel.TryGetComponent(out Rigidbody rigidbody))
                {
                    rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
                    rigidbody.interpolation          = RigidbodyInterpolation.None;
                    rigidbody.useGravity             = false;
                    rigidbody.isKinematic            = true;
                }

                // Only look for artworks in play mode / at runtime
                if (Application.isPlaying && _textureCache != null)
                {
                    SetupMarqueeNode(instantiatedModel, modelConfiguration, emulator, renderSettings.MarqueeIntensity);
                    SetupScreenNode(instantiatedModel, modelConfiguration, emulator, GetScreenIntensity(modelConfiguration, renderSettings));
                    SetupGenericNode(instantiatedModel, modelConfiguration, emulator);
                }

                instantiatedModel.SetActive(false);
            }
        }
    }
}
