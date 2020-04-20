using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

namespace Arcade
{
    public class ArcadesConfigurationModelProperties : MonoBehaviour
    {
        public ModelProperties currentModel = new ModelProperties();
        public InputField descriptiveName;
        public InputField id;
        public InputField idParent;
        public Dropdown gameLauncherMethod;
        public Dropdown model;

        private void SetupDropDownList(Dropdown dropdown, List<string> list)
        {
            dropdown.options.Clear();
            foreach (string item in list)
            {
                dropdown.options.Add(new Dropdown.OptionData(item));
            }
        }

        public void SetModelProperties(ModelProperties modelProperties)
        {
            descriptiveName.text = modelProperties.descriptiveName;
            id.text = modelProperties.id;
            idParent.text = modelProperties.idParent;
            SetupDropDownList(gameLauncherMethod, Enum.GetNames(typeof(GameLauncherMethod)).ToList());
            gameLauncherMethod.value = gameLauncherMethod.options.FindIndex(option => option.text == modelProperties.gameLauncherMethod);
            gameLauncherMethod.RefreshShownValue();
            var availableModels = ArcadeManager.availableModels.game;
            if (availableModels.Count > 0 && availableModels[0] != "none")
            {
                availableModels.Insert(0, "none");
            }
            SetupDropDownList(model, availableModels);
            //print("id " + id.text + " parent " + idParent.text);
            var index = 0;
            if (modelProperties.model != "" && modelProperties.model != "none")
            {
                index = availableModels.FindIndex(x => x == modelProperties.model);
                if (index == -1)
                {
                    index = 0;
                }
            }
            model.value = index;
            model.RefreshShownValue();
            currentModel = modelProperties;
            currentModel.model = model.options[model.value].text;
            //print("currentset " + currentModel.emulator);
        }

        public ModelProperties GetModelProperties()
        {
            currentModel.descriptiveName = descriptiveName.text;
            currentModel.idParent = idParent.text;
            currentModel.gameLauncherMethod = gameLauncherMethod.options[gameLauncherMethod.value].text;
            currentModel.model = model.options[model.value].text;
            if (currentModel.model == "none") { currentModel.model = ""; }
            //print("current emu " + currentModel.emulator);
            return currentModel;
        }
    }
}

