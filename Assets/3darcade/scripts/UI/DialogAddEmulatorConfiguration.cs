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
            Dictionary<string, string> dict = new Dictionary<string, string>
            {
                ["descriptiveName"] = descriptiveName.text,
                ["id"] = id.text,
                ["masterGamelist"] = masterGamelist.text,
                ["catVer"] = catVer.text
            };
            return dict;
        }
    }
}
