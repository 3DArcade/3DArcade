using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System;
using System.IO;

namespace Arcade
{
    public class ArcadesConfigurationArcadeProperties : MonoBehaviour
    {
        public static List<ModelProperties> selectedModelList = new List<ModelProperties>();
        public static List<ModelProperties> filteredSelectedModelList = new List<ModelProperties>();

        public Dropdown arcades;
        private int arcadesIndex = 0;
        public Dropdown modelLists;

        public InputField descriptiveName;
        public InputField id;
        public Dropdown gameLauncherMethod;
        public Dropdown videoOnModelEnabled;
        public Dropdown externalModels;
        public Dropdown showFPS;
        public InputField previewImage;

        private ArcadeConfiguration arcadeConfiguration;
        private int listIndex = 0;
        private ModelType selectedModelListType = ModelType.Game;
        // TODO: Camera properties

        public ArcadesConfigurationModelProperties modelProperties;

        public void Start()
        {
            Set(false);  // False because arcadesConfigurationList is already set in ArcadeManager Awake()
        }

        public void Set(bool updateArcadesConfigurationList = true)
        {
            if (updateArcadesConfigurationList)
            {
                ArcadeManager.arcadesConfigurationList.Clear();
                if (ArcadeManager.arcadesConfigurationList.Count < 1)
                {
                    if (!(ArcadeManager.loadSaveArcadeConfiguration.LoadArcadesConfigurationList()))
                    {
                        print("Warning no arcade configuration list found when setting up ArcadesConfigurationModelProperties");
                        return;
                    }
                }
            }
           
            // Arcade configurations
            if (ArcadeManager.arcadesConfigurationList.Count > 0)
            {
                arcades.options.Clear();
                foreach (ArcadeConfiguration item in ArcadeManager.arcadesConfigurationList)
                {
                    arcades.options.Add(new Dropdown.OptionData(item.descriptiveName));
                }

                arcades.value = arcades.options.FindIndex(option => option.text == ArcadeManager.arcadeConfiguration.descriptiveName);
                arcadesIndex = arcades.value;
                //print("arcades" + arcades.value);
                if (arcades.value < ArcadeManager.arcadesConfigurationList.Count)
                {
                    arcadeConfiguration = ArcadeManager.arcadesConfigurationList[arcades.value];
                    selectedModelList = ArcadeManager.arcadesConfigurationList[arcades.value].gameModelList;
                    selectedModelListType = ModelType.Game;
                }
                else
                {
                    arcades.value = 0;
                    arcadesIndex = 0;
                    arcadeConfiguration = ArcadeManager.arcadesConfigurationList[arcades.value];
                    selectedModelList = ArcadeManager.arcadesConfigurationList[arcades.value].gameModelList;
                    selectedModelListType = ModelType.Game;
                }
                arcades.onValueChanged.AddListener(delegate { DropdownValueChangedHandler(arcades); });
                if (arcadeConfiguration != null)
                {
                    //print("cfg is " + arcadeConfiguration.id);
                    descriptiveName.text = arcadeConfiguration.descriptiveName;
                    id.text = arcadeConfiguration.id;
                    //print("arcade1 " + arcadeConfiguration.gameModelList);
                    SetupDropDownList(gameLauncherMethod, Enum.GetNames(typeof(GameLauncherMethod)).ToList());
                    gameLauncherMethod.value = gameLauncherMethod.options.FindIndex(option => option.text == arcadeConfiguration.gameLauncherMethod);
                    gameLauncherMethod.RefreshShownValue();
                    SetupDropDownList(videoOnModelEnabled, Enum.GetNames(typeof(ModelVideoEnabled)).ToList());
                    videoOnModelEnabled.value = videoOnModelEnabled.options.FindIndex(option => option.text == arcadeConfiguration.modelSharedProperties.videoOnModelEnabled);
                    videoOnModelEnabled.RefreshShownValue();
                    SetupDropDownList(externalModels, new List<string> { "False", "True" });
                    externalModels.value = arcadeConfiguration.externalModels ? 1 : 0;
                    externalModels.RefreshShownValue();
                    SetupDropDownList(showFPS, new List<string> { "False", "True" });
                    showFPS.value = arcadeConfiguration.showFPS ? 1 : 0;
                    showFPS.RefreshShownValue();
                    SetupDropDownList(modelLists, new List<string>(new string[] { "Game Models", "Arcade Models", "Prop Models" }));
                    modelLists.onValueChanged.AddListener(delegate { DropdownValueChangedHandler(modelLists); });
                    arcades.RefreshShownValue();
                    modelLists.RefreshShownValue();
                    var filePath = FileManager.FileExists(ArcadeManager.applicationPath + ArcadeManager.arcadesConfigurationPath, arcadeConfiguration.id + ".jpg");
                    if (filePath != null)
                    {
                        var fileName = FileManager.getFilePart(FileManager.FilePart.Name_Extension, null, null, filePath);
                        if (fileName != null)
                        {
                            previewImage.text = fileName;
                        }
                    }
                    filePath = FileManager.FileExists(ArcadeManager.applicationPath + ArcadeManager.arcadesConfigurationPath, arcadeConfiguration.id + ".png");
                    if (filePath != null)
                    {
                        var fileName = FileManager.getFilePart(FileManager.FilePart.Name_Extension, null, null, filePath);
                        if (fileName != null)
                        {
                            previewImage.text = fileName;
                        }
                    }
                    UpdateModelList();
                    //print("arcade2 " + arcadeConfiguration.gameModelList);
                    ArcadeManager.arcadeConfiguration = arcadeConfiguration;
                }
            }
        }

        private MasterGamelistConfiguration getMasterGameList(string fileName)
        {
            return FileManager.LoadJSONData<MasterGamelistConfiguration>(Path.Combine(ArcadeManager.applicationPath + "/3darcade~/Configuration/MasterGamelists/" + fileName + ".json"));


        }
        private void SetupDropDownList(Dropdown dropdown, List<string> list)
        {
            dropdown.options.Clear();
            foreach (string item in list)
            {
                dropdown.options.Add(new Dropdown.OptionData(item));
            }
        }

        private void UpdateModelList()
        {
            filteredSelectedModelList = selectedModelList;
            listIndex = 0;
            modelProperties.SetModelProperties(filteredSelectedModelList.Count > 0 ? filteredSelectedModelList[listIndex] : new ModelProperties());
            var scrollRects = gameObject.GetComponentInChildren<LoopScrollRect>();
            var ls = scrollRects;
            ls.totalCount = filteredSelectedModelList.Count;
            ls.RefillCells();
        }

        private void InputFieldValueChangedHandler(InputField target)
        {
            Debug.Log("selectedsearch: " + target.text);
            //if (target == search)
            //{
            //    currentSearchSelection = target.text;
            //    FilterGamelist();
            //}
        }

        private void DropdownValueChangedHandler(Dropdown target)
        {
            Debug.Log("selected: " + target.name + " " + target.value);
            print("cfg " + arcadeConfiguration == null);
            UpdateArcadeConfiguration();
            if (target == modelLists)
            {
                string list = target.options[target.value].text;
                if (target.value == 0)
                {
                    selectedModelList = arcadeConfiguration.gameModelList;
                    selectedModelListType = ModelType.Game;
                    UpdateModelList();
                }
                if (target.value == 1)
                {
                    selectedModelList = arcadeConfiguration.arcadeModelList;
                    selectedModelListType = ModelType.Arcade;
                    UpdateModelList();
                }
                if (target.value == 2)
                {
                    selectedModelList = arcadeConfiguration.propModelList;
                    selectedModelListType = ModelType.Prop;
                    UpdateModelList();
                }
            }
            if (target == arcades)
            {
                arcadesIndex = arcades.value;
                arcadeConfiguration = ArcadeManager.arcadesConfigurationList[arcades.value];
                ArcadeManager.arcadeConfiguration = arcadeConfiguration;
                selectedModelList = ArcadeManager.arcadesConfigurationList[arcades.value].gameModelList;
                selectedModelListType = ModelType.Game;
                descriptiveName.text = arcadeConfiguration.descriptiveName;
                id.text = arcadeConfiguration.id;
                SetupDropDownList(gameLauncherMethod, Enum.GetNames(typeof(GameLauncherMethod)).ToList());
                gameLauncherMethod.value = gameLauncherMethod.options.FindIndex(option => option.text == arcadeConfiguration.gameLauncherMethod);
                SetupDropDownList(videoOnModelEnabled, Enum.GetNames(typeof(ModelVideoEnabled)).ToList());
                videoOnModelEnabled.value = videoOnModelEnabled.options.FindIndex(option => option.text == arcadeConfiguration.modelSharedProperties.videoOnModelEnabled);
                SetupDropDownList(externalModels, new List<string> { "False", "True" });
                externalModels.value = arcadeConfiguration.externalModels ? 1 : 0;
                SetupDropDownList(showFPS, new List<string> { "False", "True" });
                showFPS.value = arcadeConfiguration.showFPS ? 1 : 0;
                SetupDropDownList(modelLists, new List<string>(new string[] { "Game Models", "Arcade Models", "Prop Models" }));
                modelLists.value = 0;
                // TODO: get file preview image
                var filePath = FileManager.FileExists(ArcadeManager.applicationPath + ArcadeManager.arcadesConfigurationPath, arcadeConfiguration.id + ".jpg");
                if (filePath != null)
                {
                    var fileName = FileManager.getFilePart(FileManager.FilePart.Name_Extension, null, null, filePath);
                    if (fileName != null)
                    {
                        previewImage.text = fileName;
                    }
                }
                filePath = FileManager.FileExists(ArcadeManager.applicationPath + ArcadeManager.arcadesConfigurationPath, arcadeConfiguration.id + ".png");
                if (filePath != null)
                {
                    var fileName = FileManager.getFilePart(FileManager.FilePart.Name_Extension, null, null, filePath);
                    if (fileName != null)
                    {
                        previewImage.text = fileName;
                    }
                }

                // modelLists.onValueChanged.AddListener(delegate { DropdownValueChangedHandler(modelLists); });
                UpdateModelList();
            }
        }

        public void UpdateArcadeConfiguration()
        {
            print("arcade " + arcadeConfiguration.gameModelList);
            print("selected " + selectedModelList);
            if (selectedModelListType == ModelType.Game)
            {
                arcadeConfiguration.gameModelList = selectedModelList;
            }
            if (selectedModelListType == ModelType.Arcade)
            {
                arcadeConfiguration.arcadeModelList = selectedModelList;
            }
            if (selectedModelListType == ModelType.Prop)
            {
                arcadeConfiguration.propModelList = selectedModelList;
            }
            arcadeConfiguration.descriptiveName = descriptiveName.text;
            arcadeConfiguration.id = id.text;
            arcadeConfiguration.gameLauncherMethod = gameLauncherMethod.options[gameLauncherMethod.value].text;
            arcadeConfiguration.modelSharedProperties.videoOnModelEnabled = videoOnModelEnabled.options[videoOnModelEnabled.value].text;
            arcadeConfiguration.externalModels = externalModels.value == 0 ? false : true;
            arcadeConfiguration.showFPS = showFPS.value == 0 ? false : true;
            ArcadeManager.arcadeConfiguration = arcadeConfiguration;
            int index = ArcadeManager.arcadesConfigurationList.FindIndex(x => x.id == arcadeConfiguration.id);
            if (index != -1)
            {
                ArcadeManager.arcadesConfigurationList[arcadesIndex] = arcadeConfiguration;
            }
        }

        //public ModelProperties getSelectedGame()
        //{
        //    ModelProperties modelProperties = new ModelProperties();
        //    return null;
        //}

        public void SetSelectedModel(int index)
        {
            //print("old game selected " + filteredSelectedModelList[listIndex].descriptiveName);
            filteredSelectedModelList[listIndex] = modelProperties.GetModelProperties();
            SetModelList();
            //print("old game updated " + filteredSelectedModelList[listIndex].descriptiveName);
            //print("new game selected " + filteredSelectedModelList[index].descriptiveName);
            listIndex = index;
            modelProperties.SetModelProperties(filteredSelectedModelList[index]);
        }

        private void SetModelList()
        {
            filteredSelectedModelList[listIndex] = modelProperties.GetModelProperties();
            if (arcades.value == 0)
            {
                arcadeConfiguration.gameModelList = filteredSelectedModelList;
            }
            if (arcades.value == 1)
            {
                arcadeConfiguration.arcadeModelList = filteredSelectedModelList;
            }
            if (arcades.value == 2)
            {
                arcadeConfiguration.propModelList = filteredSelectedModelList;
            }
        }
    }
}


