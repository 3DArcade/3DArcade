using UnityEngine;

[RequireComponent(typeof(Animation))]
public class LocalAnimation : MonoBehaviour
{
    Animation animation;
    Vector3 localPos;
    bool wasPlaying;

    void Awake()
    {
    //    localPos = transform.position;
   //     wasPlaying = false;
    }

    void Start()
    {
        animation = GetComponent<Animation>();
        localPos = transform.position;
        wasPlaying = false;
    }

    public void Set(Vector3 position)
    {
        localPos = transform.position;
        wasPlaying = false;
    }

    void LateUpdate()
    {
        if (!animation.isPlaying && !wasPlaying)
            return;

        transform.localPosition += localPos;

        wasPlaying = animation.isPlaying;
    }
}