using UnityEngine;

namespace Arcade
{
    public static class MathUtils
    {
        public static float DistanceFast(Vector3 v1, Vector3 v2) => (v1 - v2).sqrMagnitude;
    }
}
