using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Arcade
{
    // Integrate this with the arcadesconfiguration version, they are the same! Maybe also the movecabs version
    public class EmulatorsConfigurationModelProperties : MonoBehaviour
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
            // TODO: Add all properties
            descriptiveName.text = modelProperties.descriptiveName;
            id.text = modelProperties.id;
            idParent.text = modelProperties.idParent;
            //print("id " + id.text + " parent " + idParent.text);
            SetupDropDownList(gameLauncherMethod, System.Enum.GetNames(typeof(GameLauncherMethod)).ToList());
            gameLauncherMethod.value = gameLauncherMethod.options.FindIndex(option => option.text == modelProperties.gameLauncherMethod);
            gameLauncherMethod.RefreshShownValue();
            List<string> availableModels = ArcadeManager.availableModels.game;
            if (availableModels.Count > 0 && availableModels[0] != "none")
            {
                availableModels.Insert(0, "none");
            }
            SetupDropDownList(model, availableModels);
            int index = 0;
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
            // TODO: Add all properties
            currentModel.descriptiveName = descriptiveName.text;
            currentModel.idParent = idParent.text;
            currentModel.gameLauncherMethod = gameLauncherMethod.options[gameLauncherMethod.value].text;
            currentModel.model = model.options[model.value].text;
            if (currentModel.model == "none")
            { currentModel.model = ""; }
            //print("current emu " + currentModel.emulator);
            return currentModel;
        }
    }
}
