using System.Collections.Generic;
using UnityEngine;

namespace Arcade
{
    [System.Serializable]
    public class Trigger
    {
        [ArcadePopUp(typeof(Event))]
        public string triggerEvent = "";
        [Tooltip("Use self for this object and camera for the active camera.")]
        public List<string> triggerSource; // special values: self, camera
        [Tooltip("Use self for this object and camera for the active camera.")]
        public List<string> triggerTarget; // special values: self, camera
        [ArcadePopUp(typeof(Action))]
        public string triggerAction = "";
        public List<AnimationProperties> animationProperties;
        public List<AudioProperties> audioProperties;
        public List<TriggerTransformProperties> triggerTansformProperties;
    }

    public class TriggerWrapper
    {
        public List<GameObject> triggerSourceGameObjects;
        public List<GameObject> triggerTargetGameObjects;
        public Trigger trigger;
    }

    [System.Serializable]
    public class TriggerTransformProperties
    {
        public bool setParent = false;
        public bool setIsKinematic = false;
        public bool setPosition = false;
        public bool setRotation = false;
        public bool setScale = false;
        public bool setIsActive = false;

        public string parent = "";
        public bool isKinematic = false;
        public Vector3 position = new Vector3(0f, 0f, 0f);
        public Quaternion rotation;
        public Vector3 scale = new Vector3(1f, 1f, 1f);
        public bool isActive = true;
    }
}
