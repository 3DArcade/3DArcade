using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Arcade
{
    public class EmulatorsConfigurationEmulatorProperties : MonoBehaviour
    {
        public static List<ModelProperties> selectedModelList = new List<ModelProperties>();
        public static List<ModelProperties> filteredSelectedModelList = new List<ModelProperties>();

        public Dropdown emulators;
        private int emulatorsIndex = 0;
        public InputField descriptiveName;
        public InputField id;
        public Dropdown gameLauncherMethod;
        public InputField executable;
        public InputField libretroCore;
        public InputField extension;
        public InputField arguments;
        public InputField options;
        public InputField emulatorPath;
        public InputField gamePath;
        public InputField workingDir;
        public InputField marqueePath;
        public InputField screenPath;
        public InputField screenVideoPath;
        public InputField genericPath;
        public InputField titlePath;
        public InputField infoPath;

        public EmulatorsConfigurationModelProperties modelProperties;
        public EmulatorConfiguration emulatorConfiguration;

        private int listIndex = 0;
        private string selectedName = "";

        private void Start()
        {
            Set("", false); // False because emulatorsConfigurationList is already set in ArcadeManager Awake()
        }

        public void Set(string descriptiveName, bool updateEmulatorsConfigurationList = true)
        {

            if (updateEmulatorsConfigurationList)
            {
                // Emulators
                ArcadeManager.emulatorsConfigurationList.Clear();
                if (ArcadeManager.emulatorsConfigurationList.Count < 1)
                {
                    if (!(ArcadeManager.loadSaveEmulatorConfiguration.LoadEmulatorsConfigurationList()))
                    {
                        print("Warning no emulator configuration list found when setting up EmulatorsConfigurationModelProperties");
                        return;
                    }
                }
            }

            selectedName = descriptiveName;
            if (ArcadeManager.emulatorsConfigurationList.Count > 0)
            {
                emulators.options.Clear();
                foreach (EmulatorConfiguration item in ArcadeManager.emulatorsConfigurationList)
                {
                    emulators.options.Add(new Dropdown.OptionData(item.emulator.descriptiveName));
                    if (item.emulator.id == "mame" && selectedName == "")
                    {
                        selectedName = item.emulator.descriptiveName;
                    }
                }
                emulators.value = emulators.options.FindIndex(option => option.text == selectedName);
                emulators.RefreshShownValue();
                emulatorsIndex = emulators.value;
                emulatorConfiguration = ArcadeManager.emulatorsConfigurationList[emulators.value];
                emulators.onValueChanged.AddListener(delegate
                { DropdownValueChangedHandler(emulators); });

                if (emulatorConfiguration != null)
                {
                    SetupList();
                }
            }
        }

        public void SetupList()
        {
            descriptiveName.text = emulatorConfiguration.emulator.descriptiveName;
            id.text = emulatorConfiguration.emulator.id;
            SetupDropDownList(gameLauncherMethod, Enum.GetNames(typeof(GameLauncherMethod)).ToList());
            gameLauncherMethod.value = gameLauncherMethod.options.FindIndex(option => option.text == emulatorConfiguration.emulator.gameLauncherMethod);
            selectedModelList = emulatorConfiguration.masterGamelist ?? new List<ModelProperties>();
            selectedModelList = selectedModelList.OrderBy(x => x.descriptiveName).ToList();
            executable.text = emulatorConfiguration.emulator.executable;
            libretroCore.text = emulatorConfiguration.emulator.libretroCore;
            extension.text = emulatorConfiguration.emulator.extension;
            arguments.text = emulatorConfiguration.emulator.arguments;
            options.text = emulatorConfiguration.emulator.options;
            emulatorPath.text = FileManager.CorrectFilePath(emulatorConfiguration.emulator.emulatorPath);
            gamePath.text = FileManager.CorrectFilePath(emulatorConfiguration.emulator.gamePath);
            workingDir.text = FileManager.CorrectFilePath(emulatorConfiguration.emulator.workingDir);
            marqueePath.text = FileManager.CorrectFilePath(emulatorConfiguration.emulator.marqueePath);
            screenPath.text = FileManager.CorrectFilePath(emulatorConfiguration.emulator.screenPath);
            screenVideoPath.text = FileManager.CorrectFilePath(emulatorConfiguration.emulator.screenVideoPath);
            genericPath.text = FileManager.CorrectFilePath(emulatorConfiguration.emulator.genericPath);
            titlePath.text = FileManager.CorrectFilePath(emulatorConfiguration.emulator.titlePath);
            infoPath.text = FileManager.CorrectFilePath(emulatorConfiguration.emulator.infoPath);

            UpdateModelList();
        }

        private void SetupDropDownList(Dropdown dropdown, List<string> list)
        {
            dropdown.options.Clear();
            //dropdown.options.Add(new Dropdown.OptionData("All"));
            foreach (string item in list)
            {
                dropdown.options.Add(new Dropdown.OptionData(item));
            }
            // dropdown.value = 0;
        }

        private void UpdateModelList()
        {
            filteredSelectedModelList = selectedModelList;
            listIndex = 0;
            print("works? " + filteredSelectedModelList.Count);
            modelProperties.SetModelProperties(filteredSelectedModelList.Count > 0 ? filteredSelectedModelList[listIndex] : new ModelProperties());

            LoopScrollRect scrollRects = gameObject.GetComponentInChildren<LoopScrollRect>();
            LoopScrollRect ls = scrollRects;
            ls.totalCount = filteredSelectedModelList.Count;
            ls.RefillCells();
            //print("listcount " + filteredSelectedGamelist.Count);
        }

        private void InputFieldValueChangedHandler(InputField target)
        {
            // Debug.Log("selectedsearch: " + target.text);
            //if (target == search)
            //{
            //    currentSearchSelection = target.text;
            //    FilterGamelist();
            //}
        }

        private void DropdownValueChangedHandler(Dropdown target)
        {
            Debug.Log("selected: " + target.name + " " + target.value);

            if (target == emulators)
            {
                UpdateEmulatorConfiguration();
                List<EmulatorConfiguration> list = ArcadeManager.emulatorsConfigurationList.Where(x => x.emulator.descriptiveName.ToLower() == target.options[target.value].text.ToLower()).ToList<EmulatorConfiguration>();
                if (list.Count > 0)
                {
                    emulatorConfiguration = ArcadeManager.emulatorsConfigurationList[target.value];
                    emulatorsIndex = target.value;
                    SetupList();
                }
            }
        }

        public void UpdateEmulatorConfiguration()
        {
            emulatorConfiguration.emulator.descriptiveName = descriptiveName.text;
            emulatorConfiguration.emulator.id = id.text;
            emulatorConfiguration.emulator.gameLauncherMethod = gameLauncherMethod.options[gameLauncherMethod.value].text;
            if (filteredSelectedModelList.Count > 0)
            {
                filteredSelectedModelList[listIndex] = modelProperties.GetModelProperties(); // Get last changes made to the currently selected model
                emulatorConfiguration.masterGamelist = filteredSelectedModelList;
            }
            emulatorConfiguration.emulator.executable = executable.text;
            emulatorConfiguration.emulator.libretroCore = libretroCore.text;
            emulatorConfiguration.emulator.extension = extension.text;
            emulatorConfiguration.emulator.arguments = arguments.text;
            emulatorConfiguration.emulator.options = options.text;
            emulatorConfiguration.emulator.emulatorPath = FileManager.CorrectFilePath(emulatorPath.text);
            emulatorConfiguration.emulator.gamePath = FileManager.CorrectFilePath(gamePath.text);
            emulatorConfiguration.emulator.workingDir = FileManager.CorrectFilePath(workingDir.text);
            emulatorConfiguration.emulator.marqueePath = FileManager.CorrectFilePath(marqueePath.text);
            emulatorConfiguration.emulator.screenPath = FileManager.CorrectFilePath(screenPath.text);
            emulatorConfiguration.emulator.screenVideoPath = FileManager.CorrectFilePath(screenVideoPath.text);
            emulatorConfiguration.emulator.genericPath = FileManager.CorrectFilePath(genericPath.text);
            emulatorConfiguration.emulator.titlePath = FileManager.CorrectFilePath(titlePath.text);
            emulatorConfiguration.emulator.infoPath = FileManager.CorrectFilePath(infoPath.text);
            ArcadeManager.emulatorsConfigurationList[emulatorsIndex] = emulatorConfiguration;
        }

        public void Save()
        {
            ArcadeManager.loadSaveEmulatorConfiguration.SaveEmulatorsConfigurationList();
        }

        public void Delete()
        {
            ArcadeManager.loadSaveEmulatorConfiguration.DeleteEmulatorConfiguration(emulatorConfiguration);
        }

        public ModelProperties GetSelectedGame()
        {
            ModelProperties modelProperties = new ModelProperties();
            return null;
        }

        public void SetSelectedModel(int index)
        {
            //print("old game selected " + filteredSelectedModelList[listIndex].descriptiveName);
            filteredSelectedModelList[listIndex] = modelProperties.GetModelProperties();
            //print("old game updated " + filteredSelectedModelList[listIndex].descriptiveName);
            //print("new game selected " + filteredSelectedModelList[index].descriptiveName);
            listIndex = index;
            modelProperties.SetModelProperties(filteredSelectedModelList[index]);
        }
    }
}
