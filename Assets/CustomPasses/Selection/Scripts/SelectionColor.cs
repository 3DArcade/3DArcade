using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class SelectionColor : MonoBehaviour
{
    public Color Color = new Color(1f, 0.5f, 0f, 1f);

    private Renderer _renderer;

    private void Awake() => _renderer = GetComponent<Renderer>();

    private void Start() => SetColor();

    private void OnValidate() => SetColor();

    void SetColor()
    {
        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
        _renderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor("_SelectionColor", Color);
        _renderer.SetPropertyBlock(propertyBlock);
    }
}
