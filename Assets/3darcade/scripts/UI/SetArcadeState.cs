using UnityEngine;

namespace Arcade
{
    public class SetArcadeState : MonoBehaviour
    {
        public ArcadeStates arcadeState;

        public void Set()
        {
            ArcadeManager.arcadeState = arcadeState;
            Debug.Log("set arcade state to " + arcadeState.ToString());
        }
    }
}
