using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcade
{
    [System.Serializable]
    public class GeneralConfiguration
    {
        [ArcadePopUp("Arcade")] // arcadeConfigurationList
        public string mainMenuArcadeConfiguration = "mainmenu";
        public List<DefaultModelFilter> defaultModelFilters;
    }
}