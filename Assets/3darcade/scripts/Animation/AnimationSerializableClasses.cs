using UnityEngine;

namespace Arcade
{
    [System.Serializable]
    public class AnimationProperties
    {
        public string name = "";
        public bool loop;
        public float speed;
        [ArcadePopUp(typeof(PlayMode))]
        public string playMode = PlayMode.StopSameLayer.ToString();
        public int layer = 0;
    }
}
