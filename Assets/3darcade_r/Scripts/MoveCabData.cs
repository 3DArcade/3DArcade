using UnityEngine;

namespace Arcade_r
{
    [CreateAssetMenu(fileName = "New MoveCab Data", menuName = "3DArcade/MoveCab Data")]
    public class MoveCabData : ScriptableObject
    {
        public Transform Transform { get; private set; }
        public Collider Collider { get; private set; }
        public Rigidbody Rigidbody { get; private set; }

        public void SetData(Transform transform, Collider collider, Rigidbody rigidbody)
        {
            Transform = transform;
            Collider  = collider;
            Rigidbody = rigidbody;
        }

        public void ResetData()
        {
            Transform = null;
            Collider  = null;
            Rigidbody = null;
        }

        private void OnEnable() => ResetData();

        private void OnDisable() => ResetData();
    }
}
