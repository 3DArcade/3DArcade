using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Arcade
{
    public class DialogAddEmulatorConfiguration : MonoBehaviour
    {
        public InputField descriptiveName;
        public InputField id;
        public InputField masterGamelist;
        public InputField catVer;

        public Dictionary<string, string> GetEmulatorProperties()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict["descriptiveName"] = descriptiveName.text;
            dict["id"] = id.text;
            dict["masterGamelist"] = masterGamelist.text;
            dict["catVer"] = catVer.text;
            return dict;
        }
    }
}
