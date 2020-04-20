using System.Collections.Generic;
using UnityEngine;
using Arcade;
using System.Linq;
using UnityEngine.UI;
using System.IO;

public class GeneralConfigurationGeneralProperties : MonoBehaviour
{
    public Dropdown mainMenuArcadeConfiguration;
  
    private GeneralConfiguration generalConfiguration;

    public void Start()
    {
        SetupList();
    }
    public void SetupList()
    {
    
        SetupDropDownList(mainMenuArcadeConfiguration, ArcadeManager.arcadesConfigurationList.Select(x => x.id).ToList());
        mainMenuArcadeConfiguration.RefreshShownValue();
        generalConfiguration = FileManager.LoadJSONData<GeneralConfiguration>(Path.Combine(ArcadeManager.applicationPath + "/3darcade~/Configuration/GeneralConfiguration.json"));
        if (generalConfiguration == null)
        {
            generalConfiguration = FileManager.LoadJSONData<GeneralConfiguration>(Path.Combine(Application.dataPath + "/Resources/cfg/GeneralConfiguration.json"));
        }
        if (generalConfiguration != null)
        {
            mainMenuArcadeConfiguration.value = mainMenuArcadeConfiguration.options.FindIndex(option => option.text == generalConfiguration.mainMenuArcadeConfiguration);
        }
    }

    private void SetupDropDownList(Dropdown dropdown, List<string> list)
    {
        dropdown.options.Clear();
        foreach (string item in list)
        {
            dropdown.options.Add(new Dropdown.OptionData(item));
        }
    }

    public void SaveGeneralConfiguration()
    {
     if (generalConfiguration.mainMenuArcadeConfiguration != mainMenuArcadeConfiguration.options[mainMenuArcadeConfiguration.value].text)
        {
            generalConfiguration.mainMenuArcadeConfiguration = mainMenuArcadeConfiguration.options[mainMenuArcadeConfiguration.value].text;
            FileManager.SaveJSONData<GeneralConfiguration>(generalConfiguration, Path.Combine(ArcadeManager.applicationPath + "/3darcade~/Configuration/"), "GeneralConfiguration.json");
            // Now reset arcade and load this new one!
            GameObject arcadeObject = GameObject.Find("Arcade");
            if (arcadeObject != null)
            {
                ArcadeManager arcadeManager = arcadeObject.GetComponent<ArcadeManager>();
                if (arcadeManager != null)
                {
                    arcadeManager.generalConfiguration = generalConfiguration;
                    ArcadeManager.arcadeHistory.Clear();
                    ArcadeManager.loadSaveArcadeConfiguration.StartArcade(ArcadeManager.loadSaveArcadeConfiguration.GetArcadeConfigurationByID(generalConfiguration.mainMenuArcadeConfiguration), null);
                }

            }
        }
    }
}
