using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Arcade
{
    public class MoveCabsEditGameProperties : MonoBehaviour
    {
        public ModelProperties currentModel = new ModelProperties();
        public InputField descriptiveName;
        public InputField id;
        public InputField idParent;
        public Dropdown gameLauncherMethod;
        public Dropdown model;

        // Global Variables:
        // ArcadeManager.loadSaveArcade.AddModelToArcade
        // ArcadeManager.activeMenuType
        // ArcadeManager.activeArcadeType
        // ArcadeManager.availableModels

        private void SetupDropDownList(Dropdown dropdown, List<string> list)
        {
            dropdown.options.Clear();
            foreach (string item in list)
            {
                dropdown.options.Add(new Dropdown.OptionData(item));
            }
        }

        public void SetGameProperties(ModelProperties gameModelProperties)
        {
            descriptiveName.text = gameModelProperties.descriptiveName;
            id.text = gameModelProperties.id;
            idParent.text = gameModelProperties.idParent;
            print("id " + id.text + " parent " + idParent.text);
            SetupDropDownList(gameLauncherMethod, System.Enum.GetNames(typeof(GameLauncherMethod)).ToList());
            gameLauncherMethod.value = gameLauncherMethod.options.FindIndex(option => option.text == gameModelProperties.gameLauncherMethod);
            gameLauncherMethod.RefreshShownValue();
            List<string> availableModels = ArcadeManager.availableModels.game.ToList();
            if (availableModels.Count > 0 && availableModels[0] != "none")
            {
                availableModels.Insert(0, "none");
            }
            SetupDropDownList(model, availableModels);
            int index = 0;
            if (gameModelProperties.model != "" && gameModelProperties.model != "none")
            {
                index = availableModels.FindIndex(x => x == gameModelProperties.model);
                if (index == -1)
                {
                    index = 0;
                }
                index += 1;
            }
            model.value = index;
            model.RefreshShownValue();
            currentModel = gameModelProperties;
            currentModel.model = model.options[model.value].text;
            //print("currentset " + currentModel.emulator);
        }

        public ModelProperties GetGameProperties()
        {

            currentModel.descriptiveName = descriptiveName.text; // TODO: NB: got error after replacing with snes model invaders
            currentModel.id = id.text;
            currentModel.idParent = idParent.text;
            currentModel.gameLauncherMethod = gameLauncherMethod.options[gameLauncherMethod.value].text;
            currentModel.model = model.options[model.value].text;
            if (currentModel.model == "none")
            { currentModel.model = ""; }
            //currentModel.modelType = ModelType.Game.ToString();
            //print("current id " + currentModel.id);
            //print("current emu " + currentModel.emulator);
            return currentModel;
        }

        public void AddModel()
        {
            ModelProperties modelProperties = GetGameProperties();
            ArcadeType arcadeType = ArcadeManager.activeMenuType == ArcadeType.None ? ArcadeManager.activeArcadeType : ArcadeManager.activeMenuType;
            GameObject myObj = ArcadeManager.loadSaveArcadeConfiguration.AddModelToArcade(ModelType.Game, modelProperties, arcadeType, true);
            myObj.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2f;
            myObj.transform.LookAt(Camera.main.transform.parent.transform);
            myObj.transform.rotation = Quaternion.Euler(0, myObj.transform.eulerAngles.y, myObj.transform.eulerAngles.z);
            myObj.transform.localScale = new Vector3(1, 1, 1);
        }

        public void ReplaceModel(GameObject obj)
        {
            Vector3 tranformPosition = obj.transform.parent.transform.position;
            Quaternion tranformRotation = obj.transform.parent.transform.rotation;
            Vector3 tranformScale = obj.transform.parent.transform.localScale;
            Destroy(obj.transform.parent.gameObject);
            ModelProperties modelProperties = GetGameProperties();
            ArcadeType arcadeType = ArcadeManager.activeMenuType == ArcadeType.None ? ArcadeManager.activeArcadeType : ArcadeManager.activeMenuType;
            GameObject myObj = ArcadeManager.loadSaveArcadeConfiguration.AddModelToArcade(ModelType.Game, modelProperties, arcadeType, true);
            myObj.transform.position = tranformPosition;
            myObj.transform.position = myObj.transform.position + new Vector3(0, 0.5f, 0);
            myObj.transform.rotation = tranformRotation;
            myObj.transform.localScale = tranformScale;
        }
    }
}
