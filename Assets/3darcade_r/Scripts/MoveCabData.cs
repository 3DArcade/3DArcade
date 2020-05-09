using UnityEngine;

namespace Arcade_r
{
    [CreateAssetMenu(fileName = "New MoveCab Data", menuName = "3DArcade/MoveCab Data")]
    public class MoveCabData : ScriptableObject
    {
        public Transform Transform { get; private set; }
        public Collider Collider { get; private set; }
        public Rigidbody Rigidbody { get; private set; }

        public float DistanceFromPlayer { get; private set; }

        public void Set(RaycastHit hitInfo)
        {
            Transform          = hitInfo.transform;
            Collider           = hitInfo.collider;
            Rigidbody          = hitInfo.rigidbody;
            DistanceFromPlayer = hitInfo.distance + (hitInfo.point - Transform.position).sqrMagnitude;
        }

        public void Reset()
        {
            Transform          = null;
            Collider           = null;
            Rigidbody          = null;
            DistanceFromPlayer = 0f;
        }

        private void OnEnable() => Reset();

        private void OnDisable() => Reset();

        private void OnDestroy() => Reset();
    }
}
