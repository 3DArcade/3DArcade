using System.Collections.Generic;

namespace Arcade
{
    [System.Serializable]
    public class AudioProperties
    {
        public string name = "";
        public int priority = 128;
        public float volume = 1;
        public bool loop;
        public bool playOnAwake;
        public bool Randomize;
        public float spatialBlend = 1;
        public bool spatialize = true;
        public float minDistance = 4;
        public float maxDistance = 200;
        public List<AudioFile> audioFiles;
        public override bool Equals(object obj)
        {
            AudioProperties other = obj as AudioProperties;
            if (other == null)
            {
                return false;
            }
            if (name != other.name)
            {
                return false;
            }
            return true;
        }
        //public override int GetHashCode()
        //{
        //    var hashCode = 1342805303;
        //    hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(name);
        //    hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(file);
        //    hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(path);
        //    return hashCode;
        //}
    }

    [System.Serializable]
    public class AudioFile
    {
        public string file = "";
        public string path = "";
    }
}
