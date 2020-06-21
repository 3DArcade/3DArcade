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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Arcade_r
{
    public sealed class ArcadeCylController : ArcadeController
    {
        private readonly List<Transform> _allGames;

        private CylArcadeProperties _cylArcadeProperties;

        private Transform[] _rightPart;
        private Transform[] _leftPart;
        private int _sprockets;
        private int _selectionIndex;

        private Vector3 _targetPosition;
        private bool _animating;

        private Vector3 _cameraOutsidePivotPoint;
        private float _cameraInsideAngle;
        private float _flatHorizontalSpacing;

        public ArcadeCylController(ArcadeHierarchy arcadeHierarchy,
                                   PlayerFpsControls playerFpsControls,
                                   PlayerCylControls playerCylControls,
                                   Database<EmulatorConfiguration> emulatorDatabase,
                                   AssetCache<GameObject> gameObjectCache,
                                   AssetCache<Texture> textureCache,
                                   AssetCache<string> videoCache)
        : base(arcadeHierarchy, playerFpsControls, playerCylControls, emulatorDatabase, gameObjectCache, textureCache, videoCache)
        {
            _allGames = new List<Transform>();
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

        private IEnumerator CoTranslateWheel(bool horizontal, bool forward, int count, float speed, float dt)
        {
            _animating = true;

            speed = 10f;

            for (int i = 0; i < count; ++i)
            {
                if (forward)
                {
                    _allGames.RotateRight();
                }
                else
                {
                    _allGames.RotateLeft();
                    dt = -dt;
                }

                IEnumerable<Transform> visibleModels = _leftPart.Concat(_rightPart);
                Transform currentModel = _allGames[_selectionIndex];
                Vector3 translation = horizontal ? new Vector3(speed * dt, 0f, 0f) : new Vector3(0f, 10f * speed, 0f);

                while (MathUtils.DistanceFast(currentModel.localPosition, _targetPosition) > 0.05f)
                {
                    foreach (Transform visibleModel in visibleModels)
                    {
                        visibleModel.Translate(translation);
                    }
                    yield return null;
                }

                currentModel.localPosition = _targetPosition;

                SetupWheel();

                visibleModels = _leftPart.Concat(_rightPart);
                foreach (Transform model in _allGames.Except(visibleModels))
                {
                    model.gameObject.SetActive(false);
                    model.localPosition = Vector3.zero;
                }
            }

            _animating = false;
        }

        private IEnumerator CoRotateWheel(bool forward, int count, float speed, float dt)
        {
            _animating = true;

            for (int i = 0; i < count; ++i)
            {
                if (forward)
                {
                    _allGames.RotateLeft();
                    dt = -dt;
                }
                else
                {
                    _allGames.RotateRight();
                }

                Transform currentModel               = _allGames[_selectionIndex];
                IEnumerable<Transform> visibleModels = _leftPart.Concat(_rightPart);

                while (MathUtils.DistanceFast(currentModel.localPosition, _targetPosition) > 0.05f)
                {
                    foreach (Transform visibleModel in visibleModels)
                    {
                        visibleModel.RotateAround(Vector3.zero, Vector3.up, speed * dt);
                    }
                    yield return null;
                }

                currentModel.localPosition = _targetPosition;

                SetupWheel();

                visibleModels = _leftPart.Concat(_rightPart);
                foreach (Transform model in _allGames.Except(visibleModels))
                {
                    model.gameObject.SetActive(false);
                    model.localPosition = Vector3.zero;
                }
            }

            _animating = false;
        }

        public void Forward(float dt, int count, float speed)
        {
            if (!_animating)
            {
                switch (_cylArcadeProperties.WheelVariant)
                {
                    case WheelVariant.CameraInsideWheel:
                        _ = _playerCylControls.StartCoroutine(CoRotateWheel(true, count, speed, dt));
                        break;
                    case WheelVariant.CameraOutsideWheel:
                        break;
                    case WheelVariant.FlatHorizontalSlide:
                        _ = _playerCylControls.StartCoroutine(CoTranslateWheel(true, true, count, speed, dt));
                        break;
                    case WheelVariant.FlatVerticalSlide:
                        break;
                    case WheelVariant.Custom:
                        break;
                    default:
                        break;
                }
            }
        }

        public void Backward(float dt, int count, float speed)
        {
            if (!_animating)
            {
                switch (_cylArcadeProperties.WheelVariant)
                {
                    case WheelVariant.CameraInsideWheel:
                        _ = _playerCylControls.StartCoroutine(CoRotateWheel(false, count, speed, dt));
                        break;
                    case WheelVariant.CameraOutsideWheel:
                        break;
                    case WheelVariant.FlatHorizontalSlide:
                        _ = _playerCylControls.StartCoroutine(CoTranslateWheel(true, false, count, speed, dt));
                        break;
                    case WheelVariant.FlatVerticalSlide:
                        break;
                    case WheelVariant.Custom:
                        break;
                    default:
                        break;
                }
            }
        }

        protected override void SetupWorld(ArcadeConfiguration arcadeConfiguration)
        {
            _cylArcadeProperties = arcadeConfiguration.CylArcadeProperties;

            _cameraOutsidePivotPoint = new Vector3(0f, 0f, -(_cylArcadeProperties.WheelRadius + _cylArcadeProperties.WheelOffsetZ));

            _playerCylControls.SetVerticalLookLimits(-40f, 40f);
            _playerCylControls.SetHorizontalLookLimits(0f, 0f);

            RenderSettings renderSettings = arcadeConfiguration.RenderSettings;
            AddModelsToWorld(arcadeConfiguration.ArcadeModelList, _arcadeHierarchy.ArcadesNode, renderSettings, ARCADE_RESOURCES_DIRECTORY, ContentMatcher.GetNamesToTryForArcade);
            AddModelsToWorld(arcadeConfiguration.PropModelList, _arcadeHierarchy.PropsNode, renderSettings, PROP_RESOURCES_DIRECTORY, ContentMatcher.GetNamesToTryForProp);

            AddGameModelsToWorld(arcadeConfiguration.GameModelList, renderSettings, GAME_RESOURCES_DIRECTORY, ContentMatcher.GetNamesToTryForGame);
        }

        private void AddGameModelsToWorld(ModelConfiguration[] modelConfigurations, RenderSettings renderSettings, string resourceDirectory, ContentMatcher.GetNamesToTryDelegate getNamesToTry)
        {
            if (modelConfigurations == null)
            {
                return;
            }

            _allGames.Clear();

            foreach (ModelConfiguration modelConfiguration in modelConfigurations)
            {
                EmulatorConfiguration emulator = _contentMatcher.GetEmulatorForConfiguration(modelConfiguration);
                List<string> namesToTry        = getNamesToTry(modelConfiguration, emulator);

                GameObject prefab = _gameObjectCache.Load(resourceDirectory, namesToTry);
                Assert.IsNotNull(prefab, "prefab is null!");

                GameObject instantiatedModel = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity, _arcadeHierarchy.GamesNode);
                instantiatedModel.name = modelConfiguration.Id;
                instantiatedModel.transform.localScale = modelConfiguration.Scale;
                instantiatedModel.transform.SetLayersRecursively(_arcadeHierarchy.GamesNode.gameObject.layer);

                instantiatedModel.AddComponent<ModelConfigurationComponent>()
                                 .FromModelConfiguration(modelConfiguration);

                _allGames.Add(instantiatedModel.transform);

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

            if (_allGames.Count < 1)
            {
                return;
            }

            _sprockets           = Mathf.Clamp(_cylArcadeProperties.Sprockets, 1, _allGames.Count);
            int selectedSprocket = Mathf.Clamp(_cylArcadeProperties.SelectedSprocket - 1, 0, _sprockets);
            _selectionIndex      = (_sprockets % 2 != 0 ? _sprockets / 2 : _sprockets / 2 - 1) - selectedSprocket;
            _allGames.RotateRight(_selectionIndex);

            SetupWheel();
        }

        private void SetupWheel()
        {
            if (_allGames.Count < 1)
            {
                return;
            }

            IEnumerable<Transform> visibleGames = _allGames.Take(_sprockets);
            int skipCount = _sprockets % 2 != 0 ? _sprockets / 2 : _sprockets / 2 - 1;
            _rightPart = visibleGames.Skip(skipCount).ToArray();
            _leftPart = visibleGames.Except(_rightPart).Reverse().ToArray();

            _targetPosition = new Vector3(0f, 0f, _cylArcadeProperties.WheelRadius);

            Transform previousModel;
            _cameraInsideAngle     = 0f;
            _flatHorizontalSpacing = 0f;
            for (int i = 0; i < _rightPart.Length; ++i)
            {
                Transform currentModel = _rightPart[i];
                currentModel.gameObject.SetActive(true);
                currentModel.SetPositionAndRotation(_targetPosition, Quaternion.Euler(_cylArcadeProperties.SprocketRotation));

                if (i > 0)
                {
                    previousModel = _rightPart[i - 1];

                    float spacing = previousModel.gameObject.GetHalfWidth() + currentModel.gameObject.GetHalfWidth() + _cylArcadeProperties.ModelSpacing;

                    switch (_cylArcadeProperties.WheelVariant)
                    {
                        case WheelVariant.CameraInsideWheel:
                            _cameraInsideAngle += (spacing / _cylArcadeProperties.WheelRadius) * Mathf.Rad2Deg;
                            currentModel.RotateAround(Vector3.zero, Vector3.up, _cameraInsideAngle);
                            break;
                        case WheelVariant.CameraOutsideWheel:
                            break;
                        case WheelVariant.FlatHorizontalSlide:
                            _flatHorizontalSpacing += spacing;
                            currentModel.Translate(_flatHorizontalSpacing, 0f, 0f);
                            break;
                        case WheelVariant.FlatVerticalSlide:
                            break;
                        case WheelVariant.Custom:
                            break;
                        default:
                            break;
                    }
                }
            }

            previousModel          = _rightPart[0];
            _cameraInsideAngle     = 0f;
            _flatHorizontalSpacing = 0f;
            for (int i = 0; i < _leftPart.Length; ++i)
            {
                Transform currentModel = _leftPart[i];
                currentModel.gameObject.SetActive(true);
                currentModel.SetPositionAndRotation(_targetPosition, Quaternion.Euler(_cylArcadeProperties.SprocketRotation));

                if (i > 0)
                {
                    previousModel = _leftPart[i - 1];
                }

                float spacing = previousModel.gameObject.GetHalfWidth() + currentModel.gameObject.GetHalfWidth() + _cylArcadeProperties.ModelSpacing;

                switch (_cylArcadeProperties.WheelVariant)
                {
                    case WheelVariant.CameraInsideWheel:
                        _cameraInsideAngle -= (spacing / _cylArcadeProperties.WheelRadius) * Mathf.Rad2Deg;
                        currentModel.RotateAround(Vector3.zero, Vector3.up, _cameraInsideAngle);
                        break;
                    case WheelVariant.CameraOutsideWheel:
                        break;
                    case WheelVariant.FlatHorizontalSlide:
                        _flatHorizontalSpacing -= spacing;
                        currentModel.Translate(_flatHorizontalSpacing, 0f, 0f);
                        break;
                    case WheelVariant.FlatVerticalSlide:
                        break;
                    case WheelVariant.Custom:
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
