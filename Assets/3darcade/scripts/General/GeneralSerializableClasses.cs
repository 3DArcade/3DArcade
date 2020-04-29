using System.Collections.Generic;

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
