using System.Collections.Generic;

namespace Arcade
{
    [System.Serializable]
    public class EmulatorConfiguration
    {
        public EmulatorProperties emulator;
        public string lastMasterGamelistUpdate = "";
        public string md5MasterGamelist = "";
        public List<ModelProperties> masterGamelist = new List<ModelProperties>();
    }

    [System.Serializable]
    public class EmulatorProperties
    {
        public string descriptiveName = "";
        public string id = "";
        public string about = "";
        public string executable = "";
        public string extension = "";
        public string libretroCore = "";
        public string arguments = "";
        public string options = "";
        public string emulatorPath = "";
        public string gamePath = "";
        public string workingDir = "";
        public string marqueePath = "";
        public string screenPath = "";
        public string screenVideoPath = "";
        public string genericPath = "";
        public string titlePath = "";
        public string infoPath = "";
        public string gameLauncherMethod = GameLauncherMethod.External.ToString();
        public List<DefaultModelFilter> defaultModelFilters;
        public bool outputCommandLine;
    }
}
