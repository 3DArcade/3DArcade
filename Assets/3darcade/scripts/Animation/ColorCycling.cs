using UnityEngine;

[RequireComponent(typeof(Light))]
public class ColorCycling : MonoBehaviour
{
    [SerializeField] private Color _colorA = Color.red;
    [SerializeField] private Color _colorB = Color.blue;

    private const float PINGPONG_LENGTH = 4f;

    private Light _light;

    private void Awake()
    {
        _light = GetComponent<Light>();
    }

    private void Update()
    {
        _light.color = Color.Lerp(_colorA, _colorB, Mathf.PingPong(Time.time, PINGPONG_LENGTH));
    }
}
