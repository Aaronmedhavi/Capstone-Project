using UnityEngine;

public class ColorDrop : MonoBehaviour, IPickable
{
    [SerializeField] ColorData data;

    [Header("Settings")]
    [SerializeField] ParticleSystem particle;
    [SerializeField] Recipe.ColorItems Color;
    [SerializeField] bool retainElement;
    public Recipe.ColorItems color => Color;
    public IPickable.type Type => IPickable.type.Color;

    private SpriteRenderer _sr;
    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        SetColor(Color);
    }
    public void SetColor(Recipe.ColorItems color)
    {
        Color = color;
        var color_data = data.GetColor(color);
        _sr.color = color_data;
        var main = particle.main;
        main.startColor = color_data;
    }

    public void OnAdd()
    {
        if(!retainElement) ObjectPoolManager.ReleaseObject(gameObject);
    }
}
