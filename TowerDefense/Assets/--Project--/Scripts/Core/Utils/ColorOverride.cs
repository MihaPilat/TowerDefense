using UnityEngine;

public class ColorOverride : MonoBehaviour
{
    [SerializeField] private Color _color = Color.gray;

    private void OnValidate()
    {
        var renderer = GetComponent<Renderer>();
        if (renderer == null) return;

        var propBlock = new MaterialPropertyBlock();
        renderer.GetPropertyBlock(propBlock);

        propBlock.SetColor("_BaseColor", _color);
        renderer.SetPropertyBlock(propBlock);
    }
}