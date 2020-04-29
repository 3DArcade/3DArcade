using System.Collections.Generic;
using UnityEngine;

namespace Arcade
{
    [System.Serializable]
    public class ModelProperties
    {
        public string descriptiveName;
        public string id;
        public string idParent;
        public string emulator;
        public string model;
        public bool grabbable; // not implemented
        //public bool lightmap; // not implemented, probably unnescessary
        //public bool animatedTextureSequence; // not used anymore
        public float animatedTextureSpeed = 2.0f;
        public Vector3 position = new Vector3(0f, 0f, 0f);
        public Quaternion rotation;
        public Vector3 scale = new Vector3(1f, 1f, 1f);
        public string screen;
        public string manufacturer;
        public string year;
        public string genre;
        public bool mature;
        public bool runnable; // TODO: implement
        public bool available; // partially implemented
        public string gameLauncherMethod;
        public int playCount;
        public int zone;
        public List<Trigger> triggers;
        public List<string> triggerIDs;
    }

    [System.Serializable]
    public class ModelSharedProperties
    {
        public bool spatialSound = false;
        [ArcadePopUp(typeof(ModelVideoEnabled))]
        public string videoOnModelEnabled = ModelVideoEnabled.VisibleUnobstructed.ToString();
        public RenderSettings renderSettings = new RenderSettings();
    }

    [System.Serializable]
    public class ModelFilter
    {
        // add all props to choose from
        // [ArcadePopUp(typeof(ModelProperties))]
        [ArcadePopUp(typeof(ModelProperties))]
        public string modelProperty = "";
        public string modelPropertyValue = "";
        [ArcadePopUp(typeof(ModelFilterOperator))]
        public string modelFilterOperator = "";
    }

    [System.Serializable]
    public class DefaultModelFilter
    {
        public List<ModelFilter> modelFilters;
        public string model = "";
    }
}
