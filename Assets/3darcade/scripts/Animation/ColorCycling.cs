using UnityEngine;

[RequireComponent(typeof(Light))]
public class ColorCycling : MonoBehaviour
{
    private const float PINGPONG_LENGTH = 4f;

    [SerializeField] private Color _colorA = Color.red;
    [SerializeField] private Color _colorB = Color.blue;

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
