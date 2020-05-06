using UnityEngine;

[CreateAssetMenu(fileName = "New Global Data", menuName = "3DArcade/Global Data")]
public class GlobalData : ScriptableObject
{
    public Transform CurrentEntityTransform { get; private set; }
    public Collider CurrentEntityCollider { get; private set; }
    public Rigidbody CurrentEntityRigidBody { get; private set; }

    public void UpdateCurrentGame(RaycastHit? hitInfo)
    {
        if (hitInfo.HasValue && CurrentEntityTransform != hitInfo.Value.transform)
        {
            CurrentEntityTransform = hitInfo.Value.transform;
            CurrentEntityCollider  = hitInfo.Value.collider;
            CurrentEntityRigidBody = CurrentEntityCollider.attachedRigidbody;
        }
    }

    private void OnEnable() => Reset();

    private void OnDisable() => Reset();

    private void Reset()
    {
        CurrentEntityTransform = null;
        CurrentEntityCollider  = null;
        CurrentEntityRigidBody = null;
    }
}
