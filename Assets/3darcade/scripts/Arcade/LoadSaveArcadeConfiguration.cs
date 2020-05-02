using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace Arcade
{
    public class LoadSaveArcadeConfiguration
    {
        private readonly ArcadeManager arcadeManager;

        public LoadSaveArcadeConfiguration(ArcadeManager arcadeManager)
        {
            this.arcadeManager = arcadeManager; // We need a reference for updating the public properties
        }

        public void StartArcadeWithIndex(int arcadeIndex)
        {
            if (ArcadeManager.arcadesConfigurationList.Count >= (arcadeIndex + 1))
            {
                StartArcade(ArcadeManager.arcadesConfigurationList[arcadeIndex]);
            }
        }

        public void StartArcade(ArcadeConfiguration arcadeConfiguration)
        {
            Debug.Log("Loading Arcade Configuration " + arcadeConfiguration.id + " in ArcadeType " + arcadeConfiguration.arcadeType);
            if (arcadeConfiguration.arcadeType == ArcadeType.FpsArcade.ToString() || arcadeConfiguration.arcadeType == ArcadeType.CylArcade.ToString())
            {
                ArcadeManager.arcadeState = ArcadeStates.LoadingArcade;
            }

            // We are loading stuff...dont do these
            RigidbodyFirstPersonController arcadeRigidbodyFirstPersonController = ArcadeManager.arcadeControls[ArcadeType.FpsArcade].GetComponent<RigidbodyFirstPersonController>();
            if (arcadeRigidbodyFirstPersonController != null)
            {
                arcadeRigidbodyFirstPersonController.pause = true;
            }
            RigidbodyFirstPersonController menuRigidbodyFirstPersonController = ArcadeManager.arcadeControls[ArcadeType.FpsMenu].GetComponent<RigidbodyFirstPersonController>();
            if (menuRigidbodyFirstPersonController != null)
            {
                menuRigidbodyFirstPersonController.pause = true;
            }
            Rigidbody arcadeCameraRigidBody = ArcadeManager.arcadeControls[ArcadeType.FpsArcade].GetComponent<Rigidbody>();
            if (arcadeCameraRigidBody != null)
            {
                arcadeCameraRigidBody.isKinematic = true;
            }
            Rigidbody menuCameraRigidBody = ArcadeManager.arcadeControls[ArcadeType.FpsMenu].GetComponent<Rigidbody>();
            if (menuCameraRigidBody != null)
            {
                menuCameraRigidBody.isKinematic = true;
            }
            CapsuleCollider arcadeCapsuleCollider = ArcadeManager.arcadeControls[ArcadeType.FpsArcade].GetComponent<CapsuleCollider>();
            if (arcadeCapsuleCollider != null)
            {
                arcadeCapsuleCollider.enabled = false;
            }
            CapsuleCollider menuCapsuleCollider = ArcadeManager.arcadeControls[ArcadeType.FpsMenu].GetComponent<CapsuleCollider>();
            if (menuCapsuleCollider != null)
            {
                menuCapsuleCollider.enabled = false;
            }

            // Arcade
            if (arcadeConfiguration.arcadeType == ArcadeType.FpsArcade.ToString() || arcadeConfiguration.arcadeType == ArcadeType.CylArcade.ToString())
            {
                ArcadeManager.activeArcadeType = ArcadeType.None;
                ResetArcade(); // Reset current state to zero

                if (LoadArcade(arcadeConfiguration))
                {
                    UpdateController(ArcadeManager.activeArcadeType);
                    TriggerManager.SendEvent(Event.ArcadeStarted);
                    if (ArcadeManager.arcadeHistory.Count == 1)
                    {
                        TriggerManager.SendEvent(Event.MainMenuStarted);
                    }
                    if (ArcadeManager.activeArcadeType == ArcadeType.FpsArcade)
                    {
                        arcadeCameraRigidBody.isKinematic = false;
                        arcadeCapsuleCollider.enabled = true;
                        arcadeRigidbodyFirstPersonController.pause = false;
                    }
                    ArcadeManager.arcadeState = ArcadeStates.Running;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
                else
                {
                    Debug.Log("Loading the Arcade Configuration of type " + arcadeConfiguration.arcadeType + " Failed!");
                    // TODO: Show an error dialog!
                }
            }
            // Menu
            if (arcadeConfiguration.arcadeType == ArcadeType.CylMenu.ToString() || arcadeConfiguration.arcadeType == ArcadeType.FpsMenu.ToString())
            {
                ArcadeManager.activeMenuType = ArcadeType.None;
                ResetMenu();

                if (LoadArcade(arcadeConfiguration))
                {
                    UpdateController(Application.isPlaying ? ArcadeManager.activeMenuType : ArcadeManager.activeArcadeType);
                    GameObject obj = ArcadeStateManager.selectedModel;
                    if (obj != null && obj.transform.childCount > 1)
                    {
                        ModelVideoSetup modelVideoSetup = obj.transform.GetChild(1).GetComponent<ModelVideoSetup>();
                        if (modelVideoSetup != null)
                        {
                            modelVideoSetup.ReleasePlayer();
                        }
                        ModelImageSetup modelImageSetup = obj.transform.GetChild(1).GetComponent<ModelImageSetup>();
                        if (modelImageSetup != null)
                        {
                            if (arcadeConfiguration.cylArcadeProperties.Count > 0)
                            {
                                if (arcadeConfiguration.cylArcadeProperties[0].cylArcadeOnScreenSelectedModel)
                                {
                                    modelImageSetup.SetMenuTexture();
                                }
                            }

                        }
                        ArcadeStateManager.savedArcadeModel = ArcadeStateManager.selectedModel;
                        ArcadeStateManager.savedArcadeModelSetup = ArcadeStateManager.selectedModelSetup;
                    }
                    if (ArcadeManager.activeArcadeType == ArcadeType.FpsMenu)
                    {
                        arcadeCameraRigidBody.isKinematic = false;
                        arcadeCapsuleCollider.enabled = true;
                        arcadeRigidbodyFirstPersonController.pause = false;
                    }
                    ArcadeManager.activeMenuType = ArcadeType.CylMenu;
                    ArcadeManager.arcadeState = ArcadeStates.ArcadeMenu;
                    TriggerManager.SendEvent(Event.MenuStarted);
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    return;
                }
                else
                {
                    Debug.Log("Loading the Arcade Configuration of type " + arcadeConfiguration.arcadeType + " Failed!");
                    // TODO: Show an error dialog!
                    ArcadeManager.activeMenuType = ArcadeType.None;
                }
                // If the menu setup fails go back to the regular arcade.
                ArcadeManager.arcadeState = ArcadeStates.Running;
                return;
            }

            void UpdateController(ArcadeType arcadeType)
            {
                ArcadeManager.arcadeControls[arcadeType].transform.position = arcadeConfiguration.camera.position;
                ArcadeManager.arcadeControls[arcadeType].transform.rotation = Quaternion.identity;
                ArcadeManager.arcadeControls[arcadeType].transform.localRotation = Quaternion.identity;
                ArcadeManager.arcadeControls[arcadeType].transform.GetChild(0).transform.rotation = Quaternion.identity;
                ArcadeManager.arcadeControls[arcadeType].transform.GetChild(0).transform.localRotation = arcadeConfiguration.camera.rotation;
                ArcadeManager.arcadeControls[arcadeType].transform.GetChild(0).transform.position = Vector3.zero;
                ArcadeManager.arcadeControls[arcadeType].transform.GetChild(0).transform.localPosition = new Vector3(0, arcadeConfiguration.camera.height, 0);
                RigidbodyFirstPersonController rigidbodyFirstPersonController = ArcadeManager.arcadeControls[arcadeType].GetComponent<RigidbodyFirstPersonController>();
                if (rigidbodyFirstPersonController != null)
                {
                    rigidbodyFirstPersonController.Setup();
                }
                ArcadeManager.arcadeCameras[ArcadeType.FpsArcade].orthographic = arcadeConfiguration.camera.orthographic;
                ArcadeManager.arcadeCameras[ArcadeType.FpsArcade].fieldOfView = arcadeConfiguration.camera.fieldOfView;
                ArcadeManager.arcadeCameras[ArcadeType.FpsArcade].nearClipPlane = arcadeConfiguration.camera.nearClipPlane;
                ArcadeManager.arcadeCameras[ArcadeType.FpsArcade].farClipPlane = arcadeConfiguration.camera.farClipPlane;
                ArcadeManager.arcadeCameras[ArcadeType.FpsArcade].rect = arcadeConfiguration.camera.viewportRect;
                ArcadeManager.arcadeCameras[ArcadeType.FpsArcade].allowDynamicResolution = arcadeConfiguration.camera.allowDynamicResolution;
                if (arcadeConfiguration.camera.aspectRatio != 0)
                {
                    ArcadeManager.arcadeCameras[ArcadeType.FpsArcade].aspect = arcadeConfiguration.camera.aspectRatio;
                }
            }
        }

        // This saves an ArcadeConfiguration from the loaded gameobjects, not from ArcadeManager's static arcadeConfiguration. Only useful if we support making changes in the editor!
        public void SaveArcade()
        {
            if (arcadeManager == null)
            {
                Debug.Log("No Arcade node found");
                return;
            }
            ArcadeConfiguration arcadeConfiguration = arcadeManager.GetArcadeConfiguration();
            SaveArcadeConfiguration(arcadeConfiguration);
        }

        public void SaveArcadeConfiguration(ArcadeConfiguration arcadeConfiguration)
        {
            string filePath = ArcadeManager.applicationPath + ArcadeManager.arcadesConfigurationPath;
            string fileName = arcadeConfiguration.id + ".json";
            FileManager.SaveJSONData(arcadeConfiguration, filePath, fileName);
        }

        public void SaveArcadesConfigurationList()
        {
            string[] files = FileManager.FilesFromDirectory(ArcadeManager.applicationPath + ArcadeManager.arcadesConfigurationPath, "*.json");
            if (files != null)
            {
                foreach (string file in files)
                {
                    File.Delete(file);
                }
            }
            foreach (ArcadeConfiguration arcadeConfiguration in ArcadeManager.arcadesConfigurationList)
            {
                SaveArcadeConfiguration(arcadeConfiguration);
            }
        }

        public void DeleteArcadeConfiguration(ArcadeConfiguration arcadeConfiguration)
        {
            string filePath = ArcadeManager.applicationPath + ArcadeManager.arcadesConfigurationPath;
            string fileName = arcadeConfiguration.id + ".json";
            _ = FileManager.DeleteFile(filePath, fileName);
            fileName = arcadeConfiguration.id + ".json.meta";
            _ = FileManager.DeleteFile(filePath, fileName);
            fileName = arcadeConfiguration.id + ".jpg";
            _ = FileManager.DeleteFile(filePath, fileName);
            fileName = arcadeConfiguration.id + ".jpg.meta";
            _ = FileManager.DeleteFile(filePath, fileName);
        }

        public bool LoadArcade(ArcadeConfiguration arcadeConfiguration)
        {
            if (arcadeManager == null)
            {
                Debug.Log("No ArcadeManager reference found, create one...");
                return false;
            }
            if (arcadeConfiguration == null)
            {
                return false;
            }
            if (arcadeConfiguration.arcadeType == ArcadeType.FpsArcade.ToString() || arcadeConfiguration.arcadeType == ArcadeType.CylArcade.ToString() || !Application.isPlaying)
            {
                arcadeManager.SetArcadeConfiguration(arcadeConfiguration);
            }
            else
            {
                ArcadeManager.menuConfiguration = arcadeConfiguration;
            }
            _ = System.Enum.TryParse(arcadeConfiguration.arcadeType, true, out ArcadeType arcadeType);
            if (arcadeType == ArcadeType.FpsArcade || arcadeType == ArcadeType.FpsMenu)
            {
                ArcadeManager.allZones[arcadeType] = new Dictionary<int, List<int>>();
                ArcadeManager.visibleZones[arcadeType] = new Dictionary<int, List<GameObject>>();
                foreach (Zone zone in arcadeConfiguration.zones)
                {
                    ArcadeManager.allZones[arcadeType][zone.zone] = zone.visibleZones;
                }
            }

            SetListOfModelProperties(ModelType.Arcade, arcadeConfiguration.arcadeModelList);
            if (arcadeConfiguration.arcadeType == ArcadeType.FpsArcade.ToString() || !Application.isPlaying)
            {
                SetListOfModelProperties(ModelType.Game, arcadeConfiguration.gameModelList);
            }
            SetListOfModelProperties(ModelType.Prop, arcadeConfiguration.propModelList);

            if (!Application.isPlaying)
            {
                return true;
            } // TODO: Why true?

            if (arcadeConfiguration.arcadeType == ArcadeType.CylArcade.ToString())
            {
                ArcadeManager.activeArcadeType = ArcadeType.CylArcade;
                ArcadeManager.arcadeHistory.Add(arcadeConfiguration.id);
                CylController cylController = ArcadeManager.arcadeControls[ArcadeType.CylArcade].GetComponentInChildren<CylController>();
                _ = cylController.SetupCylArcade(arcadeConfiguration);
            }
            if (arcadeConfiguration.arcadeType == ArcadeType.FpsArcade.ToString())
            {
                ArcadeManager.activeArcadeType = ArcadeType.FpsArcade;
                ArcadeManager.arcadeHistory.Add(arcadeConfiguration.id);
            }
            if (arcadeConfiguration.arcadeType == ArcadeType.CylMenu.ToString())
            {
                ArcadeManager.activeMenuType = ArcadeType.CylMenu;
                CylController cylController = ArcadeManager.arcadeControls[ArcadeType.CylMenu].GetComponentInChildren<CylController>();
                _ = cylController.SetupCylArcade(arcadeConfiguration);
            }
            if (arcadeConfiguration.arcadeType == ArcadeType.FpsMenu.ToString())
            {
                ArcadeManager.activeMenuType = ArcadeType.FpsMenu;
            }
            TriggerManager.Setup(arcadeType);

            return true;

            void SetListOfModelProperties(ModelType modelType, List<ModelProperties> list)
            {
                ArcadeType tempArcadeType = !Application.isPlaying ? ArcadeType.None : arcadeType;
                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    ModelProperties modelProperties = list[i];
                    _ = AddModelToArcade(modelType, modelProperties, tempArcadeType, false);
                }
            }
        }

        public bool FilterModel(ModelFilter modelFilter, ModelProperties modelProperties)
        {
            //Debug.Log(modelProperties.genre + " - " + modelFilter.modelProperty + " - " + modelFilter.modelPropertyValue);
            _ = System.Enum.TryParse(modelFilter.modelFilterOperator, true, out ModelFilterOperator modelFilterOperator);
            string value = modelProperties.GetType().GetField(modelFilter.modelProperty).GetValue(modelProperties).ToString();
            float first;
            float second;
            switch (modelFilterOperator)
            {
                case ModelFilterOperator.Contains:
                    if (value != "" && value.ToLower().Contains(modelFilter.modelPropertyValue.ToLower()))
                    {
                        return true;
                    }
                    return false;
                case ModelFilterOperator.NotContains:
                    if (value != "" && !value.ToLower().Contains(modelFilter.modelPropertyValue.ToLower()))
                    {
                        return true;
                    }
                    return false;
                case ModelFilterOperator.Equals:
                    if (value != "" && value.ToLower() == modelFilter.modelPropertyValue.ToLower())
                    {
                        return true;
                    }
                    return false;
                case ModelFilterOperator.NotEquals:
                    if (value != "" && !(value.ToLower() == modelFilter.modelPropertyValue.ToLower()))
                    {
                        return true;
                    }
                    return false;
                case ModelFilterOperator.Starts:
                    if (value != "" && value.ToLower().StartsWith(modelFilter.modelPropertyValue.ToLower(), System.StringComparison.Ordinal))
                    {
                        return true;
                    }
                    return false;
                case ModelFilterOperator.NotStarts:
                    if (value != "" && !value.ToLower().StartsWith(modelFilter.modelPropertyValue.ToLower(), System.StringComparison.Ordinal))
                    {
                        return true;
                    }
                    return false;
                case ModelFilterOperator.Smaller:
                    if (float.TryParse(value, out first) && float.TryParse(modelFilter.modelPropertyValue, out second))
                    {
                        if (first < second)
                        {
                            return true;
                        }
                    }
                    return false;
                case ModelFilterOperator.Larger:
                    if (float.TryParse(value, out first) && float.TryParse(modelFilter.modelPropertyValue, out second))
                    {
                        if (first > second)
                        {
                            return true;
                        }
                    }
                    return false;
                default:
                    return false;
            }
        }

        public GameObject AddModelToArcade(ModelType modelType, ModelProperties modelProperties, ArcadeType arcadeType, bool addTrigger)
        {
            AssetBundle tAsset;
            GameObject tObj;
            tAsset = GetExternalModel(modelProperties.model);
            if (tAsset != null)
            {
                return addExternalModel(tAsset);
            }
            tObj = GetInternalModel(modelProperties.model);
            if (tObj != null)
            {
                return AddInternalModel(tObj);
            }
            tAsset = GetExternalModel(modelProperties.id);
            if (tAsset != null)
            {
                return addExternalModel(tAsset);
            }
            tObj = GetInternalModel(modelProperties.id);
            if (tObj != null)
            {
                return AddInternalModel(tObj);
            }
            tAsset = GetExternalModel(modelProperties.idParent);
            if (tAsset != null)
            {
                return addExternalModel(tAsset);
            }
            tObj = GetInternalModel(modelProperties.idParent);
            if (tObj != null)
            {
                return AddInternalModel(tObj);
            }

            // Now check for defaultmodels
            // First defaultmodel filters in the emulator
            List<EmulatorConfiguration> emulatorConfiguration = ArcadeManager.emulatorsConfigurationList.Where(x => x.emulator.id == modelProperties.emulator).ToList();
            if (emulatorConfiguration.Count > 1)
            {
                List<DefaultModelFilter> defaultModelFilters = emulatorConfiguration[0].emulator.defaultModelFilters;
                foreach (DefaultModelFilter defaultModel in defaultModelFilters)
                {
                    bool success = true;
                    foreach (ModelFilter filter in defaultModel.modelFilters)
                    {
                        success &= FilterModel(filter, modelProperties);
                    }
                    if (success)
                    {
                        tAsset = GetExternalModel(defaultModel.model);
                        if (tAsset != null)
                        {
                            return addExternalModel(tAsset);
                        }
                        tObj = GetInternalModel(defaultModel.model);
                        if (tObj != null)
                        {
                            return AddInternalModel(tObj);
                        }
                    }
                }
            }

            // Generic defaultmodel filters
            if (arcadeManager.generalConfiguration != null)
            {
                List<DefaultModelFilter> defaultModelFilters = arcadeManager.generalConfiguration.defaultModelFilters;
                foreach (DefaultModelFilter defaultModel in defaultModelFilters)
                {
                    bool success = true;
                    foreach (ModelFilter filter in defaultModel.modelFilters)
                    {
                        success &= FilterModel(filter, modelProperties);
                    }
                    if (success)
                    {
                        tAsset = GetExternalModel(defaultModel.model);
                        if (tAsset != null)
                        {
                            return addExternalModel(tAsset);
                        }
                        tObj = GetInternalModel(defaultModel.model);
                        if (tObj != null)
                        {
                            return AddInternalModel(tObj);
                        }
                    }
                }
            }

            // defaultmodel
            string[] defaultModels = { "default70hor", "default70vert", "default80hor", "default80vert", "default90hor", "default90vert" };
            System.Random rnd = new System.Random();
            tObj = GetInternalModel(defaultModels[rnd.Next(defaultModels.Length)]);
            if (tObj != null)
            {
                return AddInternalModel(tObj);
            }

            return null;

            AssetBundle GetExternalModel(string modelName)
            {
                List<AssetBundle> prefab = ArcadeManager.modelAssets.Where(x => x.name == modelName).ToList();
                if (prefab.Count < 1)
                {
                    string file = FileManager.FileExists(ArcadeManager.applicationPath + "/3darcade~/Configuration/Assets/" + ArcadeManager.currentOS.ToString() + "/" + modelType.ToString() + "s/", modelName + ".unity3d");
                    if (file != null)
                    {
                        AssetBundle asset = AssetBundle.LoadFromFile(file);
                        if (asset != null && asset.name != null && asset.name != "")
                        {
                            ArcadeManager.modelAssets.Add(asset);
                            prefab.Add(asset);
                            return asset;
                        }
                    }
                }
                return prefab.Count == 1 ? prefab[0] : null;
            }

            GameObject addExternalModel(AssetBundle asset)
            {
                GameObject me = asset.LoadAsset(asset.name) as GameObject;
                GameObject child = UnityEngine.Object.Instantiate(me);
                return AddModel(child, modelProperties);
            }

            GameObject GetInternalModel(string modelName)
            {
                GameObject obj = (GameObject)Resources.Load(modelType.ToString() + "s/" + modelName, typeof(GameObject));

                // TODO: NBNB remove this hack to be able to use gamelist models as prop models
                if (obj == null)
                {
                    obj = (GameObject)Resources.Load(ModelType.Game.ToString() + "s/" + modelName, typeof(GameObject));
                }
                return obj == null ? null : obj;
            }

            GameObject AddInternalModel(GameObject obj)
            {
                GameObject child = UnityEngine.Object.Instantiate(obj);
                return AddModel(child, modelProperties);
            }

            GameObject AddModel(GameObject obj, ModelProperties model)
            {
                GameObject dummyNode;
                if (Application.isPlaying)
                {
                    dummyNode = new GameObject("dummy");
                    obj.transform.SetParent(dummyNode.transform);
                }
                else
                {
                    dummyNode = obj;
                }

                ModelSetup modelSetup = dummyNode.GetComponent<ModelSetup>();
                if (modelSetup == null)
                {
                    _ = dummyNode.AddComponent<ModelSetup>();
                    modelSetup = dummyNode.GetComponent<ModelSetup>();
                }
                //Rigidbody rigidbody = obj.GetComponent<Rigidbody>();
                //if (rigidbody != null)
                //{
                //    rigidbody.isKinematic = true;
                //}
                dummyNode.transform.position = model.position; // model is redundant you aleady have access to modelProperties
                dummyNode.transform.rotation = model.rotation;
                dummyNode.transform.localScale = model.scale;
                if (modelType == ModelType.Arcade)
                { dummyNode.tag = "arcademodel"; }
                if (modelType == ModelType.Game)
                { dummyNode.tag = "gamemodel"; }
                if (modelType == ModelType.Prop)
                { dummyNode.tag = "propmodel"; }
                bool isArcadeLayer = arcadeType == ArcadeType.FpsArcade || arcadeType == ArcadeType.CylArcade || arcadeType == ArcadeType.None ? true : false;
                string layer = (isArcadeLayer ? "Arcade/" : "Menu/") + modelType.ToString() + "Models";
                dummyNode.layer = LayerMask.NameToLayer(layer);
                dummyNode.RunOnChildrenRecursive(tChild => tChild.layer = LayerMask.NameToLayer(layer));

                GameObject node = null;
                if (isArcadeLayer)
                {
                    node = GameObject.Find("Arcade/" + modelType.ToString() + "Models");
                }
                else
                {
                    node = GameObject.Find("Menu/" + modelType.ToString() + "Models");
                }

                if (node != null)
                {
                    dummyNode.transform.SetParent(node.transform);
                }
                else
                {
                    Debug.Log("Error: Could not find the models parent node...");
                }

                // Zoning
                if (arcadeType == ArcadeType.FpsArcade || arcadeType == ArcadeType.FpsMenu)
                {
                    if (!ArcadeManager.visibleZones[arcadeType].ContainsKey(modelProperties.zone))
                    { ArcadeManager.visibleZones[arcadeType][modelProperties.zone] = new List<GameObject>(); }

                    if (modelProperties.zone != 0)
                    {
                        ArcadeManager.visibleZones[arcadeType][modelProperties.zone].Add(dummyNode);
                    }
                    else
                    {
                        if (Physics.Raycast(dummyNode.transform.position, -dummyNode.transform.up, out RaycastHit vision, 100.0f))
                        {
                            GameObject objectHit = vision.transform.gameObject;
                            if (objectHit != null)
                            {
                                ModelSetup hitModelSetup = objectHit.transform.parent.gameObject.GetComponent<ModelSetup>();
                                if (hitModelSetup != null)
                                {
                                    //Debug.Log("zonemodel " + modelSetup.descriptiveName);
                                    ArcadeManager.visibleZones[arcadeType][hitModelSetup.zone].Add(dummyNode);
                                }
                            }
                        }
                    }
                }

                ArcadeConfiguration arcadeConfiguration = isArcadeLayer ? ArcadeManager.arcadeConfiguration : ArcadeManager.menuConfiguration;
                modelSetup.Setup(model, arcadeConfiguration.modelSharedProperties);
                if (addTrigger && modelSetup.triggers.Count > 0 && Application.isPlaying)
                {
                    _ = TriggerManager.Add(modelSetup, arcadeType);
                }
                return dummyNode;
            }
        }

        public bool LoadArcadesConfigurationList()
        {
            string[] files = FileManager.FilesFromDirectory(ArcadeManager.applicationPath + ArcadeManager.arcadesConfigurationPath, "*.json", SearchOption.AllDirectories);
            ArcadeManager.arcadesConfigurationList.Clear();
            foreach (string file in files)
            {
                ArcadeConfiguration cfg = FileManager.LoadJSONData<ArcadeConfiguration>(file);
                ArcadeManager.arcadesConfigurationList.Add(cfg);
            }

            return files.Length > 0;
        }

        public ArcadeConfiguration GetArcadeConfigurationByID(string arcadeConfigurationID)
        {

            if (ArcadeManager.arcadesConfigurationList.Count < 1)
            {
                _ = LoadArcadesConfigurationList();
            }

            if (ArcadeManager.arcadesConfigurationList.Count > 0)
            {
                List<ArcadeConfiguration> tempArcadesConfigurationList = ArcadeManager.arcadesConfigurationList.Where(x => x.id == arcadeConfigurationID).ToList();
                if (tempArcadesConfigurationList.Count > 0)
                {
                    return tempArcadesConfigurationList[0];
                }
            }
            return null;
        }


        public void ResetArcade()
        {
            TriggerManager.SendEvent(Event.ArcadeEnded);
            TriggerManager.triggersActive = new Dictionary<Event, List<ModelTriggerSetup>>();

            ResetMenu();

            // Destroy the models attached to the camera
            GameObject tobj = ArcadeManager.arcadeCameras[ArcadeType.FpsArcade].transform.gameObject;
            List<GameObject> gameObjects = tobj.GetChildren();
            foreach (GameObject child in gameObjects)
            {
                ModelSetup modelSetup = child.GetComponent<ModelSetup>();
                if (modelSetup != null)
                {
                    DestroyModel(child);
                }
            }

            // Reset external assets.
            AssetBundle.UnloadAllAssetBundles(true);
            ArcadeManager.modelAssets.Clear();
            GameObject obj = GameObject.Find("EditorModelCache");
            if (obj != null)
            {
                foreach (Transform child in obj.transform)
                {
                    DestroyModel(child.gameObject);
                }
                DestroyModel(obj);
            }

            obj = GameObject.Find("Arcade/GameModels");
            if (obj == null)
            {
                Debug.Log("no game models found");
                return;
            }
            Transform transform = obj.transform;
            for (int i = transform.childCount - 1; i >= 0; --i)
            {
                DestroyModel(transform.GetChild(i).gameObject);
            }
            transform.DetachChildren();

            obj = GameObject.Find("Arcade/ArcadeModels");
            if (obj == null)
            {
                Debug.Log("no arcade models found");
                return;
            }
            transform = obj.transform;
            for (int i = transform.childCount - 1; i >= 0; --i)
            {
                DestroyModel(transform.GetChild(i).gameObject);
            }
            transform.DetachChildren();

            obj = GameObject.Find("Arcade/PropModels");
            if (obj == null)
            {
                Debug.Log("no prop models found");
                return;
            }
            transform = obj.transform;
            for (int i = transform.childCount - 1; i >= 0; --i)
            {
                DestroyModel(transform.GetChild(i).gameObject);
            }
            transform.DetachChildren();
            void DestroyModel(GameObject child)
            {
                if (Application.isPlaying)
                {
                    Object.Destroy(child);
                }
                else
                {
                    Object.DestroyImmediate(child);
                }
            }
            _ = Resources.UnloadUnusedAssets();
            TriggerManager.UnLoadTriggers();
        }

        public void ResetMenu()
        {
            // Destroy the models attached to the camera
            GameObject tobj = ArcadeManager.arcadeCameras[ArcadeType.CylMenu].transform.gameObject;
            List<GameObject> gameObjects = tobj.GetChildren();
            foreach (GameObject child in gameObjects)
            {
                ModelSetup modelSetup = child.GetComponent<ModelSetup>();
                if (modelSetup != null)
                {
                    DestroyModel(child);
                }
            }

            GameObject obj = GameObject.Find("Menu/GameModels");
            if (obj == null)
            {
                Debug.Log("no game models found");
                return;
            }
            Transform transform = obj.transform;
            for (int i = transform.childCount - 1; i >= 0; --i)
            {
                DestroyModel(transform.GetChild(i).gameObject);
            }
            transform.DetachChildren();

            obj = GameObject.Find("Menu/ArcadeModels");
            if (obj == null)
            {
                Debug.Log("no arcade models found");
                return;
            }
            transform = obj.transform;
            for (int i = transform.childCount - 1; i >= 0; --i)
            {
                DestroyModel(transform.GetChild(i).gameObject);
            }
            transform.DetachChildren();

            obj = GameObject.Find("Menu/PropModels");
            if (obj == null)
            {
                Debug.Log("no prop models found");
                return;
            }
            transform = obj.transform;
            for (int i = transform.childCount - 1; i >= 0; --i)
            {
                DestroyModel(transform.GetChild(i).gameObject);
            }
            transform.DetachChildren();
            void DestroyModel(GameObject child)
            {
                if (Application.isPlaying)
                {
                    Object.Destroy(child);
                }
                else
                {
                    Object.DestroyImmediate(child);
                }
            }
        }
    }
}