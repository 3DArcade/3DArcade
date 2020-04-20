using UnityEngine;

public class ColorCycling : MonoBehaviour
{
    Color lerpedColor = Color.white;

    void Update()
    {
        lerpedColor = Color.Lerp(Color.red, Color.blue, Mathf.PingPong(Time.time, 4));
        Light myLight = (UnityEngine.Light)gameObject.GetComponent("Light");
        myLight.color = lerpedColor;
    }
}
