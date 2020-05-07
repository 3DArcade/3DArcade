using System.Collections.Generic;
using UnityEngine;

namespace Arcade_r
{
    [CreateAssetMenu(fileName = "New Model List", menuName = "3DArcade/Model List")]
    public class ModelList : ScriptableObject
    {
        public List<GameObject> AvailableModels;
    }
}
