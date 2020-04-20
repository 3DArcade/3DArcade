using System.Collections.Generic;

namespace Arcade
{
    [System.Serializable]
    public class MasterGamelistConfiguration
    {
        public string descriptiveName = "";
        public string id = "";
        public string info = "";
        public string lastUpdate = "";
        public List<ModelProperties> masterGamelist;
    }
}

